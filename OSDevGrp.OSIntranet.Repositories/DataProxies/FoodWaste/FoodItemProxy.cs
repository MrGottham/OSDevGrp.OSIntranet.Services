using System;
using System.Collections.Generic;
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

        private bool _foodGroupsIsLoaded = false;
        private bool _translationsIsLoaded = false;
        private bool _foreignKeysIsLoaded = false;
        private IDataProviderBase _dataProvider = null;

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
                if (base.PrimaryFoodGroup != null || _dataProvider == null)
                {
                    return base.PrimaryFoodGroup;
                }
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the food groups which this food item belong to.
        /// </summary>
        public override IEnumerable<IFoodGroup> FoodGroups
        {
            get
            {
                if (_foodGroupsIsLoaded || _dataProvider == null)
                {
                    return base.FoodGroups;
                }
                throw new NotImplementedException();
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
                throw new NotImplementedException();
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
                throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the SQL statement to update this food item.
        /// </summary>
        /// <returns>SQL statement to update this food item.</returns>
        public virtual string GetSqlCommandForUpdate()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the SQL statement to delete this food item.
        /// </summary>
        /// <returns>SQL statement to delete this food item.</returns>
        public virtual string GetSqlCommandForDelete()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Maps relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void MapRelations(IDataProviderBase dataProvider)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Save relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        /// <param name="isInserting">Indication of whether we are inserting or updating.</param>
        public virtual void SaveRelations(IDataProviderBase dataProvider, bool isInserting)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Delete relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void DeleteRelations(IDataProviderBase dataProvider)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
