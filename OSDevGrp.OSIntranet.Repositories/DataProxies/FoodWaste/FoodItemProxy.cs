using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Guards;
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

        private bool _foodGroupCollectionHasBeenLoaded;
        private bool _foodGroupCollectionIsLoading;
        private bool _translationCollectionHasBeenLoaded;
        private bool _foreignKeyCollectionHasBeenLoaded;
        private IFoodWasteDataProvider _dataProvider;

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
            _foodGroupCollectionHasBeenLoaded = true;
        }

        /// <summary>
        /// Creates a data proxy for a food item.
        /// </summary>
        /// <param name="dataProvider">The data provider which the created data proxy should use.</param>
        private FoodItemProxy(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider)
        {
            if (dataProvider == null)
            {
                return;
            }

            _dataProvider = (IFoodWasteDataProvider) dataProvider;
        }

        #endregion

        #region Properties

        public override IFoodGroup PrimaryFoodGroup
        {
            get
            {
                if (_foodGroupCollectionHasBeenLoaded || _foodGroupCollectionIsLoading || _dataProvider == null || Identifier.HasValue == false)
                {
                    return base.PrimaryFoodGroup;
                }

                // Force the lazy load of food groups which should initialize the primary food group.
                // ReSharper disable ReturnValueOfPureMethodIsNotUsed
                FoodGroups.ToArray();
                // ReSharper restore ReturnValueOfPureMethodIsNotUsed

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
                if (_foodGroupCollectionHasBeenLoaded || _foodGroupCollectionIsLoading || _dataProvider == null || Identifier.HasValue == false)
                {
                    return base.FoodGroups;
                }

                _foodGroupCollectionIsLoading = true;
                try
                {
                    IList<FoodItemGroupProxy> foodItemGroupProxyCollection = new List<FoodItemGroupProxy>(FoodItemGroupProxy.GetFoodItemGroups(_dataProvider, Identifier.Value));

                    PrimaryFoodGroup = foodItemGroupProxyCollection.Single(m => m.IsPrimary).FoodGroup;
                    FoodGroups = new List<IFoodGroup>(foodItemGroupProxyCollection.Select(m => m.FoodGroup));
                    return base.FoodGroups;
                }
                finally
                {
                    _foodGroupCollectionIsLoading = false;
                }
            }
            protected set
            {
                base.FoodGroups = value;
                _foodGroupCollectionHasBeenLoaded = true;
            }
        }

        /// <summary>
        /// Gets the translations for the food item.
        /// </summary>
        public override IEnumerable<ITranslation> Translations
        {
            get
            {
                if (_translationCollectionHasBeenLoaded || _dataProvider == null || Identifier.HasValue == false)
                {
                    return base.Translations;
                }

                Translations = new List<ITranslation>(TranslationProxy.GetDomainObjectTranslations(_dataProvider, Identifier.Value));
                return base.Translations;
            }
            protected set
            {
                base.Translations = value;
                _translationCollectionHasBeenLoaded = true;
            }
        }

        /// <summary>
        /// Gets the foreign keys for the food item.
        /// </summary>
        public override IEnumerable<IForeignKey> ForeignKeys
        {
            get
            {
                if (_foreignKeyCollectionHasBeenLoaded || _dataProvider == null || Identifier.HasValue == false)
                {
                    return base.ForeignKeys;
                }

                ForeignKeys = new List<IForeignKey>(ForeignKeyProxy.GetDomainObjectForeignKeys(_dataProvider, Identifier.Value));
                return base.ForeignKeys;
            }
            protected set
            {
                base.ForeignKeys = value;
                _foreignKeyCollectionHasBeenLoaded = true;
            }
        }

        #endregion

        #region IMySqlDataProxy Members

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

        #endregion

        #region IDataProxyBase<MySqlDataReader, MySqlCommand> Members

        /// <summary>
        /// Maps data from the data reader.
        /// </summary>
        /// <param name="dataReader">Data reader.</param>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void MapData(MySqlDataReader dataReader, IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNull(dataProvider, nameof(dataProvider));

            Identifier = GetFoodItemIdentifier(dataReader, "FoodItemIdentifier");
            IsActive = GetIsActive(dataReader, "IsActive");

            _foodGroupCollectionHasBeenLoaded = false;
            _translationCollectionHasBeenLoaded = false;
            _foreignKeyCollectionHasBeenLoaded = false;
            _dataProvider = (IFoodWasteDataProvider) dataProvider;
        }

        /// <summary>
        /// Maps relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void MapRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider)
        {
            ArgumentNullGuard.NotNull(dataProvider, nameof(dataProvider));

            if (Identifier.HasValue == false)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, Identifier, "Identifier"));
            }

            IList<FoodItemGroupProxy> foodItemGroupProxyCollection = new List<FoodItemGroupProxy>(FoodItemGroupProxy.GetFoodItemGroups(dataProvider, Identifier.Value));

            PrimaryFoodGroup = foodItemGroupProxyCollection.Single(m => m.IsPrimary).FoodGroup;
            FoodGroups = new List<IFoodGroup>(foodItemGroupProxyCollection.Select(m => m.FoodGroup));
            Translations = new List<ITranslation>(TranslationProxy.GetDomainObjectTranslations(dataProvider, Identifier.Value));
            ForeignKeys = new List<IForeignKey>(ForeignKeyProxy.GetDomainObjectForeignKeys(dataProvider, Identifier.Value));

            _dataProvider = (IFoodWasteDataProvider) dataProvider;
        }

        /// <summary>
        /// Save relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        /// <param name="isInserting">Indication of whether we are inserting or updating.</param>
        public virtual void SaveRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, bool isInserting)
        {
            ArgumentNullGuard.NotNull(dataProvider, nameof(dataProvider));

            if (Identifier.HasValue == false)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, Identifier, "Identifier"));
            }

            if (PrimaryFoodGroup != null && PrimaryFoodGroup.Identifier.HasValue == false)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, PrimaryFoodGroup.Identifier, "PrimaryFoodGroup.Identifier"));
            }

            IFoodGroup foodGroupWithoutIdentifier = FoodGroups.FirstOrDefault(foodGroup => foodGroup.Identifier.HasValue == false);
            if (foodGroupWithoutIdentifier != null)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, foodGroupWithoutIdentifier.Identifier, "FoodGroups[].Identifier"));
            }

            IList<FoodItemGroupProxy> foodItemGroups = FoodItemGroupProxy.GetFoodItemGroups(dataProvider, Identifier.Value).ToList();
            if (PrimaryFoodGroup?.Identifier != null && foodItemGroups.SingleOrDefault(foodItemGroup => foodItemGroup.FoodGroupIdentifier.HasValue && foodItemGroup.FoodGroupIdentifier.Value == PrimaryFoodGroup.Identifier.Value) == null)
            {
                using (IFoodWasteDataProvider subDataProvider = (IFoodWasteDataProvider) dataProvider.Clone())
                {
                    foodItemGroups.Add(subDataProvider.Add(FoodItemGroupProxy.Build(this, PrimaryFoodGroup, true)));
                }
            }

            IFoodGroup missingFoodGroup = FoodGroups.FirstOrDefault(foodGroup => foodGroup.Identifier.HasValue && foodItemGroups.Any(foodItemGroup => foodItemGroup.FoodGroupIdentifier.HasValue && foodItemGroup.FoodGroupIdentifier.Value == foodGroup.Identifier.Value) == false);
            while (missingFoodGroup != null)
            {
                using (IFoodWasteDataProvider subDataProvider = (IFoodWasteDataProvider) dataProvider.Clone())
                {
                    foodItemGroups.Add(subDataProvider.Add(FoodItemGroupProxy.Build(this, missingFoodGroup, false)));
                }
                missingFoodGroup = FoodGroups.FirstOrDefault(foodGroup => foodGroup.Identifier.HasValue && foodItemGroups.Any(foodItemGroup => foodItemGroup.FoodGroupIdentifier.HasValue && foodItemGroup.FoodGroupIdentifier.Value == foodGroup.Identifier.Value) == false);
            }

            FoodItemGroupProxy noLongerExistingFoodItemGroup = foodItemGroups.FirstOrDefault(foodItemGroup => foodItemGroup.FoodGroupIdentifier.HasValue && FoodGroups.Any(foodGroup => foodGroup.Identifier.HasValue && foodGroup.Identifier.Value == foodItemGroup.FoodGroupIdentifier.Value) == false);
            while (noLongerExistingFoodItemGroup != null)
            {
                using (IFoodWasteDataProvider subDataProvider = (IFoodWasteDataProvider) dataProvider.Clone())
                {
                    subDataProvider.Delete(noLongerExistingFoodItemGroup);
                    foodItemGroups.Remove(noLongerExistingFoodItemGroup);
                }
                noLongerExistingFoodItemGroup = foodItemGroups.FirstOrDefault(foodItemGroup => foodItemGroup.FoodGroupIdentifier.HasValue && FoodGroups.Any(foodGroup => foodGroup.Identifier.HasValue && foodGroup.Identifier.Value == foodItemGroup.FoodGroupIdentifier.Value) == false);
            }

            if (PrimaryFoodGroup == null || PrimaryFoodGroup.Identifier.HasValue == false)
            {
                _dataProvider = (IFoodWasteDataProvider) dataProvider;
                return;
            }

            FoodItemGroupProxy primaryFoodItemGroup = foodItemGroups.SingleOrDefault(foodItemGroup => foodItemGroup.FoodGroupIdentifier.HasValue && foodItemGroup.FoodGroupIdentifier.Value == PrimaryFoodGroup.Identifier.Value);
            if (primaryFoodItemGroup != null && primaryFoodItemGroup.IsPrimary == false)
            {
                primaryFoodItemGroup.IsPrimary = true;
                using (IFoodWasteDataProvider subDataProvider = (IFoodWasteDataProvider) dataProvider.Clone())
                {
                    foodItemGroups.Remove(primaryFoodItemGroup);
                    foodItemGroups.Add(subDataProvider.Save(primaryFoodItemGroup));
                }
            }

            FoodItemGroupProxy nonPrimaryFoodItemGroup = foodItemGroups.Where(foodItemGroup => foodItemGroup.FoodGroupIdentifier.HasValue && foodItemGroup.FoodGroupIdentifier.Value != PrimaryFoodGroup.Identifier.Value).SingleOrDefault(foodItemGroup => foodItemGroup.IsPrimary);
            while (nonPrimaryFoodItemGroup != null)
            {
                nonPrimaryFoodItemGroup.IsPrimary = false;
                using (IFoodWasteDataProvider subDataProvider = (IFoodWasteDataProvider) dataProvider.Clone())
                {
                    foodItemGroups.Remove(nonPrimaryFoodItemGroup);
                    foodItemGroups.Add(subDataProvider.Save(nonPrimaryFoodItemGroup));
                }
                nonPrimaryFoodItemGroup = foodItemGroups.Where(foodItemGroup => foodItemGroup.FoodGroupIdentifier.HasValue && foodItemGroup.FoodGroupIdentifier.Value != PrimaryFoodGroup.Identifier.Value).SingleOrDefault(foodItemGroup => foodItemGroup.IsPrimary);
            }

            _dataProvider = (IFoodWasteDataProvider) dataProvider;
        }

        /// <summary>
        /// Delete relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void DeleteRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider)
        {
            ArgumentNullGuard.NotNull(dataProvider, nameof(dataProvider));

            if (Identifier.HasValue == false)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, Identifier, "Identifier"));
            }

            FoodItemGroupProxy.DeleteFoodItemGroups(dataProvider, this);
            TranslationProxy.DeleteDomainObjectTranslations(dataProvider, Identifier.Value);
            ForeignKeyProxy.DeleteDomainObjectForeignKeys(dataProvider, Identifier.Value);

            _dataProvider = (IFoodWasteDataProvider) dataProvider;
        }

        /// <summary>
        /// Creates the SQL statement for getting this food item.
        /// </summary>
        /// <returns>SQL statement for getting this food item.</returns>
        public virtual MySqlCommand CreateGetCommand()
        {
            return BuildSystemDataCommandForSelecting("WHERE fi.FoodItemIdentifier=@foodItemIdentifier", systemDataCommandBuilder => systemDataCommandBuilder.AddFoodItemIdentifierParameter(Identifier));
        }

        /// <summary>
        /// Creates the SQL statement for inserting this food item.
        /// </summary>
        /// <returns>SQL statement for inserting this food item.</returns>
        public virtual MySqlCommand CreateInsertCommand()
        {
            return new SystemDataCommandBuilder("INSERT INTO FoodItems (FoodItemIdentifier,IsActive) VALUES(@foodItemIdentifier,@isActive)")
                .AddFoodItemIdentifierParameter(Identifier)
                .AddFoodItemIsActiveParameter(IsActive)
                .Build();
        }

        /// <summary>
        /// Creates the SQL statement for updating this food item.
        /// </summary>
        /// <returns>SQL statement for updating this food item.</returns>
        public virtual MySqlCommand CreateUpdateCommand()
        {
            return new SystemDataCommandBuilder("UPDATE FoodItems SET IsActive=@isActive WHERE FoodItemIdentifier=@foodItemIdentifier")
                .AddFoodItemIdentifierParameter(Identifier)
                .AddFoodItemIsActiveParameter(IsActive)
                .Build();
        }

        /// <summary>
        /// Creates the SQL statement for deleting this food item.
        /// </summary>
        /// <returns>SQL statement for deleting this food item.</returns>
        public virtual MySqlCommand CreateDeleteCommand()
        {
            return new SystemDataCommandBuilder("DELETE FROM FoodItems WHERE FoodItemIdentifier=@foodItemIdentifier")
                .AddFoodItemIdentifierParameter(Identifier)
                .Build();
        }

        #endregion

        #region IMySqlDataProxyCreator<IFoodItemProxy> Members

        /// <summary>
        /// Creates an instance of the data proxy for a food item with values from the data reader.
        /// </summary>
        /// <param name="dataReader">Data reader from which column values should be read.</param>
        /// <param name="dataProvider">Data provider which supports the data reader.</param>
        /// <param name="columnNameCollection">Collection of column names which should be read from the data reader.</param>
        /// <returns>Instance of the data proxy for a food item with values from the data reader.</returns>
        public virtual IFoodItemProxy Create(MySqlDataReader dataReader, IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, params string[] columnNameCollection)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNull(dataProvider, nameof(dataProvider))
                .NotNull(columnNameCollection, nameof(columnNameCollection));

            return new FoodItemProxy(dataProvider)
            {
                Identifier = GetFoodItemIdentifier(dataReader, columnNameCollection[0]),
                IsActive = GetIsActive(dataReader, columnNameCollection[1])
            };
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a MySQL command selecting a collection of <see cref="FoodItemProxy"/>.
        /// </summary>
        /// <param name="whereClause">The WHERE clause which the MySQL command should use.</param>
        /// <param name="parameterAdder">The callback to add MySQL parameters to the MySQL command.</param>
        /// <returns>MySQL command selecting a collection of <see cref="FoodItemProxy"/>.</returns>
        internal static MySqlCommand BuildSystemDataCommandForSelecting(string whereClause = null, Action<SystemDataCommandBuilder> parameterAdder = null)
        {
            StringBuilder selectStatementBuilder = new StringBuilder("SELECT fi.FoodItemIdentifier,fi.IsActive FROM FoodItems AS fi");
            if (string.IsNullOrWhiteSpace(whereClause) == false)
            {
                selectStatementBuilder.Append($" {whereClause}");
            }

            SystemDataCommandBuilder systemDataCommandBuilder = new SystemDataCommandBuilder(selectStatementBuilder.ToString());
            if (parameterAdder == null)
            {
                return systemDataCommandBuilder.Build();
            }

            parameterAdder(systemDataCommandBuilder);
            return systemDataCommandBuilder.Build();
        }

        /// <summary>
        /// Gets the food item identifier from the data reader.
        /// </summary>
        /// <param name="dataReader">The data reader from which to read.</param>
        /// <param name="columnName">The column name for value to read.</param>
        /// <returns>The food item identifier.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataReader"/> is null or <paramref name="columnName"/> is null, empty or white space.</exception>
        private static Guid GetFoodItemIdentifier(MySqlDataReader dataReader, string columnName)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNullOrWhiteSpace(columnName, nameof(columnName));

            return new Guid(dataReader.GetString(columnName));
        }

        /// <summary>
        /// Gets the indication for whether the food item is active from the data reader.
        /// </summary>
        /// <param name="dataReader">The data reader from which to read.</param>
        /// <param name="columnName">The column name for value to read.</param>
        /// <returns>The indication for whether the food item is active.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataReader"/> is null or <paramref name="columnName"/> is null, empty or white space.</exception>
        private static bool GetIsActive(MySqlDataReader dataReader, string columnName)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNullOrWhiteSpace(columnName, nameof(columnName));

            return Convert.ToBoolean(dataReader.GetInt32(columnName));
        }

        #endregion
    }
}
