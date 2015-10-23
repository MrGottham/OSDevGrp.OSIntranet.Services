using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste
{
    /// <summary>
    /// Data proxy for a food item.
    /// </summary>
    public class FoodItemProxy : FoodItem, IFoodItemProxy
    {
        #region Private variables

        private bool _foodGroupsIsLoaded;
        private bool _foodGroupsIsLoading;
        private bool _translationsIsLoaded;
        private bool _foreignKeysIsLoaded;
        private IDataProviderBase _dataProvider;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a data proxy for a food item.
        /// </summary>
        public FoodItemProxy()
        {
        }

        /// <summary>
        /// Creates a data proxy for a food item.
        /// </summary>
        /// <param name="primaryFoodGroup">Primary food group for the food item.</param>
        public FoodItemProxy(IFoodGroup primaryFoodGroup) 
            : base(primaryFoodGroup)
        {
            _foodGroupsIsLoaded = true;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the primary food group for the food item.
        /// </summary>
        public override IFoodGroup PrimaryFoodGroup
        {
            get
            {
                if (base.PrimaryFoodGroup != null)
                {
                    return base.PrimaryFoodGroup;
                }
                
                // Force the lazy load of food groups which should initialize the primary food group.
                FoodGroups.ToArray();
                
                return base.PrimaryFoodGroup;
            }
        }

        /// <summary>
        /// Gets the food groups which this food item belong to.
        /// </summary>
        public override IEnumerable<IFoodGroup> FoodGroups
        {
            get
            {
                if (_foodGroupsIsLoaded || _foodGroupsIsLoading || _dataProvider == null)
                {
                    return base.FoodGroups;
                }
                _foodGroupsIsLoading = true;
                try
                {
                    // Find all relations between this food item and it's food group.
                    var foodItemGroups = FoodItemGroupProxy.GetFoodItemGroups(_dataProvider, this).ToArray();
                    var primaryFoodItemGroup = foodItemGroups.SingleOrDefault(foodItemGroup => foodItemGroup.IsPrimary);

                    // Initialize the collection of food groups on this food item.
                    base.FoodGroups = foodItemGroups.Select(foodItemGroup => foodItemGroup.FoodGroup).ToList();
                    _foodGroupsIsLoaded = true;

                    // Initialize the primary food group for this food item.
                    if (primaryFoodItemGroup != null)
                    {
                        base.PrimaryFoodGroup = primaryFoodItemGroup.FoodGroup;
                    }

                    return base.FoodGroups;
                }
                finally
                {
                    _foodGroupsIsLoading = false;
                }
            }
        }

        /// <summary>
        /// Gets the translations for the food item.
        /// </summary>
        public override IEnumerable<ITranslation> Translations
        {
            get
            {
                if (_translationsIsLoaded || _dataProvider == null)
                {
                    return base.Translations;
                }
                base.Translations = new List<ITranslation>(TranslationProxy.GetDomainObjectTranslations(_dataProvider, Guid.Parse(UniqueId)));
                _translationsIsLoaded = true;
                return base.Translations;
            }
        }

        /// <summary>
        /// Gets the foreign keys for the food item.
        /// </summary>
        public override IEnumerable<IForeignKey> ForeignKeys
        {
            get
            {
                if (_foreignKeysIsLoaded || _dataProvider == null)
                {
                    return base.ForeignKeys;
                }
                base.ForeignKeys = new List<IForeignKey>(ForeignKeyProxy.GetDomainObjectForeignKeys(_dataProvider, Guid.Parse(UniqueId)));
                _foreignKeysIsLoaded = true;
                return base.ForeignKeys;
            }
        }

        #endregion

        #region IMySqlDataProxy<IFoodItem>

        /// <summary>
        /// Gets the unique identification for the food item.
        /// </summary>
        public virtual string UniqueId
        {
            get
            {
                if (Identifier.HasValue)
                {
                    return Identifier.Value.ToString("D").ToUpper();
                }
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, Identifier, "Identifier"));
            }
        }

        /// <summary>
        /// Gets the SQL statement for selecting a food item.
        /// </summary>
        /// <param name="foodItem">Food item for which to get the SQL statement for selecting.</param>
        /// <returns>SQL statement for selecting a food item.</returns>
        public virtual string GetSqlQueryForId(IFoodItem foodItem)
        {
            if (foodItem == null)
            {
                throw new ArgumentNullException("foodItem");
            }
            if (foodItem.Identifier.HasValue)
            {
                return string.Format("SELECT IsActive FROM FoodItems WHERE FoodItemIdentifier='{0}'", foodItem.Identifier.Value.ToString("D").ToUpper());
            }
            throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, foodItem.Identifier, "Identifier"));
        }

        /// <summary>
        /// Gets the SQL statement to insert this food item.
        /// </summary>
        /// <returns>SQL statement to insert this food item</returns>
        public virtual string GetSqlCommandForInsert()
        {
            return string.Format("INSERT INTO FoodItems (FoodItemIdentifier,IsActive) VALUES('{0}',{1})", UniqueId, Convert.ToInt32(IsActive));
        }

        /// <summary>
        /// Gets the SQL statement to update this food item.
        /// </summary>
        /// <returns>SQL statement to update this food item.</returns>
        public virtual string GetSqlCommandForUpdate()
        {
            return string.Format("UPDATE FoodItems SET IsActive={1} WHERE FoodItemIdentifier='{0}'", UniqueId, Convert.ToInt32(IsActive));
        }

        /// <summary>
        /// Gets the SQL statement to delete this food item.
        /// </summary>
        /// <returns>SQL statement to delete this food item.</returns>
        public virtual string GetSqlCommandForDelete()
        {
            return string.Format("DELETE FROM FoodItems WHERE FoodItemIdentifier='{0}'", UniqueId);
        }

        #endregion

        #region IDataProxyBase Members

        /// <summary>
        /// Maps data from the data reader.
        /// </summary>
        /// <param name="dataReader">Data reader.</param>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void MapData(object dataReader, IDataProviderBase dataProvider)
        {
            if (dataReader == null)
            {
                throw new ArgumentNullException("dataReader");
            }
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }

            var mySqlDataReader = dataReader as MySqlDataReader;
            if (mySqlDataReader == null)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, "dataReader", dataReader.GetType().Name));
            }

            Identifier = new Guid(mySqlDataReader.GetString("FoodItemIdentifier"));
            IsActive = Convert.ToBoolean(mySqlDataReader.GetInt32("IsActive"));

            if (_dataProvider == null)
            {
                _dataProvider = dataProvider;
            }
        }

        /// <summary>
        /// Maps relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void MapRelations(IDataProviderBase dataProvider)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }
        }

        /// <summary>
        /// Save relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        /// <param name="isInserting">Indication of whether we are inserting or updating.</param>
        public virtual void SaveRelations(IDataProviderBase dataProvider, bool isInserting)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }
            if (Identifier.HasValue == false)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, Identifier, "Identifier"));
            }

            if (_dataProvider == null)
            {
                _dataProvider = dataProvider;
            }

            if (PrimaryFoodGroup != null && PrimaryFoodGroup.Identifier.HasValue == false)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, PrimaryFoodGroup.Identifier, "PrimaryFoodGroup.Identifier"));
            }
            if (FoodGroups.Any(foodGroup => foodGroup.Identifier.HasValue == false))
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, FoodGroups.First(foodGroup => foodGroup.Identifier.HasValue == false).Identifier, "FoodGroups[].Identifier"));
            }

            var foodItemGroups = FoodItemGroupProxy.GetFoodItemGroups(dataProvider, this).ToList();
            if (PrimaryFoodGroup != null && PrimaryFoodGroup.Identifier.HasValue && foodItemGroups.SingleOrDefault(foodItemGroup => foodItemGroup.FoodGroupIdentifier.HasValue && foodItemGroup.FoodGroupIdentifier.Value == PrimaryFoodGroup.Identifier.Value) == null)
            {
                using (var subDataProvider = (IDataProviderBase) dataProvider.Clone())
                {
                    var foodItemGroupProxy = new FoodItemGroupProxy
                    {
                        Identifier = Guid.NewGuid(),
                        FoodItemIdentifier = Identifier.Value,
                        FoodGroupIdentifier = PrimaryFoodGroup.Identifier.Value,
                        IsPrimary = true
                    };
                    foodItemGroups.Add(subDataProvider.Add(foodItemGroupProxy));
                }
            }
            var missingfoodItemGroup = FoodGroups.FirstOrDefault(foodGroup => foodGroup.Identifier.HasValue && foodItemGroups.Any(foodItemGroup => foodItemGroup.FoodGroupIdentifier.HasValue && foodItemGroup.FoodGroupIdentifier.Value == foodGroup.Identifier.Value) == false);
            while (missingfoodItemGroup != null)
            {
                using (var subDataProvider = (IDataProviderBase) dataProvider.Clone())
                {
                    var foodItemGroupProxy = new FoodItemGroupProxy
                    {
                        Identifier = Guid.NewGuid(),
                        FoodItemIdentifier = Identifier.Value,
                        FoodGroupIdentifier = missingfoodItemGroup.Identifier,
                        IsPrimary = false
                    };
                    foodItemGroups.Add(subDataProvider.Add(foodItemGroupProxy));
                }
                missingfoodItemGroup = FoodGroups.FirstOrDefault(foodGroup => foodGroup.Identifier.HasValue && foodItemGroups.Any(foodItemGroup => foodItemGroup.FoodGroupIdentifier.HasValue && foodItemGroup.FoodGroupIdentifier.Value == foodGroup.Identifier.Value) == false);
            }
            var noLongerExistingFoodItemGroup = foodItemGroups.FirstOrDefault(foodItemGroup => foodItemGroup.FoodGroupIdentifier.HasValue && FoodGroups.Any(foodGroup => foodGroup.Identifier.HasValue && foodGroup.Identifier.Value == foodItemGroup.FoodGroupIdentifier.Value) == false);
            while (noLongerExistingFoodItemGroup != null)
            {
                using (var subDataProvider = (IDataProviderBase) dataProvider.Clone())
                {
                    subDataProvider.Delete(noLongerExistingFoodItemGroup);
                    foodItemGroups.Remove(noLongerExistingFoodItemGroup);
                }
                noLongerExistingFoodItemGroup = foodItemGroups.FirstOrDefault(foodItemGroup => foodItemGroup.FoodGroupIdentifier.HasValue && FoodGroups.Any(foodGroup => foodGroup.Identifier.HasValue && foodGroup.Identifier.Value == foodItemGroup.FoodGroupIdentifier.Value) == false);
            }

            if (PrimaryFoodGroup == null || PrimaryFoodGroup.Identifier.HasValue == false)
            {
                return;
            }
            var primaryFoodItemGroup = foodItemGroups.SingleOrDefault(foodItemGroup => foodItemGroup.FoodGroupIdentifier.HasValue && foodItemGroup.FoodGroupIdentifier.Value == PrimaryFoodGroup.Identifier.Value);
            if (primaryFoodItemGroup != null && primaryFoodItemGroup.IsPrimary == false)
            {
                using (var subDataProvider = (IDataProviderBase) dataProvider.Clone())
                {
                    primaryFoodItemGroup.IsPrimary = true;
                    foodItemGroups.Remove(primaryFoodItemGroup);
                    foodItemGroups.Add(subDataProvider.Save(primaryFoodItemGroup));
                }
            }
            var nonPrimaryFoodItemGroup = foodItemGroups.Where(foodItemGroup => foodItemGroup.FoodGroupIdentifier.HasValue && foodItemGroup.FoodGroupIdentifier.Value != PrimaryFoodGroup.Identifier.Value).SingleOrDefault(foodItemGroup => foodItemGroup.IsPrimary);
            while (nonPrimaryFoodItemGroup != null)
            {
                using (var subDataProvider = (IDataProviderBase) dataProvider.Clone())
                {
                    nonPrimaryFoodItemGroup.IsPrimary = false;
                    foodItemGroups.Remove(nonPrimaryFoodItemGroup);
                    foodItemGroups.Add(subDataProvider.Save(nonPrimaryFoodItemGroup));
                }
                nonPrimaryFoodItemGroup = foodItemGroups.Where(foodItemGroup => foodItemGroup.FoodGroupIdentifier.HasValue && foodItemGroup.FoodGroupIdentifier.Value != PrimaryFoodGroup.Identifier.Value).SingleOrDefault(foodItemGroup => foodItemGroup.IsPrimary);
            }
        }

        /// <summary>
        /// Delete relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void DeleteRelations(IDataProviderBase dataProvider)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }
            if (Identifier.HasValue == false)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, Identifier, "Identifier"));
            }

            if (_dataProvider == null)
            {
                _dataProvider = dataProvider;
            }

            FoodItemGroupProxy.DeleteFoodItemGroups(dataProvider, this);
            TranslationProxy.DeleteDomainObjectTranslations(dataProvider, Identifier.Value);
            ForeignKeyProxy.DeleteDomainObjectForeignKeys(dataProvider, Identifier.Value);
        }

        #endregion
    }
}
