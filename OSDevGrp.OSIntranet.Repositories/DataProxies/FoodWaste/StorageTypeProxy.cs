using System;
using System.Collections.Generic;
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
    /// Data proxy to a given storage type.
    /// </summary>
    public class StorageTypeProxy : StorageType, IStorageTypeProxy
    {
        #region Private variables

        private bool _translationCollectionHasBeenLoaded;
        private IFoodWasteDataProvider _dataProvider;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a data proxy to a given storage type.
        /// </summary>
        public StorageTypeProxy()
        {
        }

        /// <summary>
        /// Creates a data proxy to a given storage type.
        /// </summary>
        /// <param name="sortOrder">Order for sorting storage types.</param>
        /// <param name="temperature">Default temperature for the storage type.</param>
        /// <param name="temperatureRange">Temperature range for the storage type.</param>
        /// <param name="creatable">Indicates whether household members can create storage of this type.</param>
        /// <param name="editable">Indicates whether household members can edit storage of this type.</param>
        /// <param name="deletable">Indicates whether household members can delete storage of this type.</param>
        /// <param name="dataProvider">The data provider which the created data proxy should use.</param>
        public StorageTypeProxy(int sortOrder, int temperature, IRange<int> temperatureRange, bool creatable, bool editable, bool deletable, IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider = null) 
            : base(sortOrder, temperature, temperatureRange, creatable, editable, deletable)
        {
            if (dataProvider == null)
            {
                return;
            }

            _dataProvider = (IFoodWasteDataProvider) dataProvider;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the translations for the storage type.
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

        #endregion

        #region IMySqlDataProxy Members

        /// <summary>
        /// Gets the unique identification for the storage type.
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

            Identifier = GetStorageTypeIdentifier(dataReader, "StorageTypeIdentifier");
            SortOrder = GetSortOrder(dataReader, "SortOrder");
            Temperature = GetTemperature(dataReader, "Temperature");
            TemperatureRange = GetTemperatureRange(dataReader, "TemperatureRangeStartValue", "TemperatureRangeEndValue");
            Creatable = GetCreatable(dataReader, "Creatable");
            Editable = GetEditable(dataReader, "Editable");
            Deletable = GetDeletable(dataReader, "Deletable");

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

            Translations = new List<ITranslation>(TranslationProxy.GetDomainObjectTranslations(dataProvider, Identifier.Value));

            _dataProvider = (IFoodWasteDataProvider) dataProvider;
        }

        /// <summary>
        /// Save relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        /// <param name="isInserting">Indication of whether we are inserting or updating</param>
        public virtual void SaveRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, bool isInserting)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Delete relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void DeleteRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Creates the SQL statement for getting this storage type.
        /// </summary>
        /// <returns>SQL statement for getting this storage type.</returns>
        public virtual MySqlCommand CreateGetCommand()
        {
            return new SystemDataCommandBuilder("SELECT StorageTypeIdentifier,SortOrder,Temperature,TemperatureRangeStartValue,TemperatureRangeEndValue,Creatable,Editable,Deletable FROM StorageTypes WHERE StorageTypeIdentifier=@storageTypeIdentifier")
                .AddStorageTypeIdentifierParameter(Identifier)
                .Build();
        }

        /// <summary>
        /// Creates the SQL statement for inserting this storage type.
        /// </summary>
        /// <returns>SQL statement for inserting this storage type.</returns>
        public virtual MySqlCommand CreateInsertCommand()
        {
            return new SystemDataCommandBuilder("INSERT INTO StorageTypes (StorageTypeIdentifier,SortOrder,Temperature,TemperatureRangeStartValue,TemperatureRangeEndValue,Creatable,Editable,Deletable) VALUES(@storageTypeIdentifier,@sortOrder,@temperature,@temperatureRangeStartValue,@temperatureRangeEndValue,@creatable,@editable,@deletable)")
                .AddStorageTypeIdentifierParameter(Identifier)
                .AddStorageTypeSortOrderParameter(SortOrder)
                .AddStorageTypeTemperatureParameter(Temperature)
                .AddStorageTypeTemperatureRangeParameter(TemperatureRange)
                .AddStorageTypeCreatableParameter(Creatable)
                .AddStorageTypeEditableParameter(Editable)
                .AddStorageTypeDeletableParameter(Deletable)
                .Build();
        }

        /// <summary>
        /// Creates the SQL statement for updating this storage type.
        /// </summary>
        /// <returns>SQL statement for updating this storage type.</returns>
        public virtual MySqlCommand CreateUpdateCommand()
        {
            return new SystemDataCommandBuilder("UPDATE StorageTypes SET SortOrder=@sortOrder,Temperature=@temperature,TemperatureRangeStartValue=@temperatureRangeStartValue,TemperatureRangeEndValue=@temperatureRangeEndValue,Creatable=@creatable,Editable=@editable,Deletable=@deletable WHERE StorageTypeIdentifier=@storageTypeIdentifier")
                .AddStorageTypeIdentifierParameter(Identifier)
                .AddStorageTypeSortOrderParameter(SortOrder)
                .AddStorageTypeTemperatureParameter(Temperature)
                .AddStorageTypeTemperatureRangeParameter(TemperatureRange)
                .AddStorageTypeCreatableParameter(Creatable)
                .AddStorageTypeEditableParameter(Editable)
                .AddStorageTypeDeletableParameter(Deletable)
                .Build();
        }

        /// <summary>
        /// Creates the SQL statement for deleting this storage type.
        /// </summary>
        /// <returns>SQL statement for deleting this storage type.</returns>
        public virtual MySqlCommand CreateDeleteCommand()
        {
            return new SystemDataCommandBuilder("DELETE FROM StorageTypes WHERE StorageTypeIdentifier=@storageTypeIdentifier")
                .AddStorageTypeIdentifierParameter(Identifier)
                .Build();
        }

        #endregion

        #region IMySqlDataProxyCreator<IStorageTypeProxy> Members

        /// <summary>
        /// Creates an instance of the data proxy to a given storage type with values from the data reader.
        /// </summary>
        /// <param name="dataReader">Data reader from which column values should be read.</param>
        /// <param name="dataProvider">Data provider which supports the data reader.</param>
        /// <param name="columnNameCollection">Collection of column names which should be read from the data reader.</param>
        /// <returns>Instance of the data proxy to a given storage type with values from the data reader.</returns>
        public virtual IStorageTypeProxy Create(MySqlDataReader dataReader, IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, params string[] columnNameCollection)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNull(dataProvider, nameof(dataProvider))
                .NotNull(columnNameCollection, nameof(columnNameCollection));

            return new StorageTypeProxy(
                GetSortOrder(dataReader, columnNameCollection[1]),
                GetTemperature(dataReader, columnNameCollection[2]),
                GetTemperatureRange(dataReader, columnNameCollection[3], columnNameCollection[4]),
                GetCreatable(dataReader, columnNameCollection[5]),
                GetEditable(dataReader, columnNameCollection[6]),
                GetDeletable(dataReader, columnNameCollection[7]),
                dataProvider)
            {
                Identifier = GetStorageTypeIdentifier(dataReader, columnNameCollection[0])
            };
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the storage type identifier from the data reader.
        /// </summary>
        /// <param name="dataReader">The data reader from which to read.</param>
        /// <param name="columnName">The column name for value to read.</param>
        /// <returns>The storage type identifier.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataReader"/> is null or <paramref name="columnName"/> is null, empty or white space.</exception>
        private static Guid GetStorageTypeIdentifier(MySqlDataReader dataReader, string columnName)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNullOrWhiteSpace(columnName, nameof(columnName));

            return new Guid(dataReader.GetString(columnName));
        }

        /// <summary>
        /// Gets the sort order from the data reader.
        /// </summary>
        /// <param name="dataReader">The data reader from which to read.</param>
        /// <param name="columnName">The column name for value to read.</param>
        /// <returns>The sort order.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataReader"/> is null or <paramref name="columnName"/> is null, empty or white space.</exception>
        private static short GetSortOrder(MySqlDataReader dataReader, string columnName)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNullOrWhiteSpace(columnName, nameof(columnName));

            return dataReader.GetInt16(columnName);
        }

        /// <summary>
        /// Gets the temperature from the data reader.
        /// </summary>
        /// <param name="dataReader">The data reader from which to read.</param>
        /// <param name="columnName">The column name for value to read.</param>
        /// <returns>The temperature.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataReader"/> is null or <paramref name="columnName"/> is null, empty or white space.</exception>
        private static short GetTemperature(MySqlDataReader dataReader, string columnName)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNullOrWhiteSpace(columnName, nameof(columnName));

            return dataReader.GetInt16(columnName);
        }

        /// <summary>
        /// Gets the temperature range from the data reader.
        /// </summary>
        /// <param name="dataReader">The data reader from which to read.</param>
        /// <param name="startValueColumnName">The column name for start value to read.</param>
        /// <param name="endValueColumnName">The column name for end value to read.</param>
        /// <returns>The temperature range.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataReader"/> is null, <paramref name="startValueColumnName"/> or <paramref name="endValueColumnName"/> is null, empty or white space.</exception>
        private static IRange<int> GetTemperatureRange(MySqlDataReader dataReader, string startValueColumnName, string endValueColumnName)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNullOrWhiteSpace(startValueColumnName, nameof(startValueColumnName))
                .NotNullOrWhiteSpace(endValueColumnName, nameof(endValueColumnName));

            return new Range<int>(dataReader.GetInt16(startValueColumnName), dataReader.GetInt16(endValueColumnName));
        }

        /// <summary>
        /// Gets whether the storage type is creatable from the data reader.
        /// </summary>
        /// <param name="dataReader">The data reader from which to read.</param>
        /// <param name="columnName">The column name for value to read.</param>
        /// <returns>Whether the storage type is creatable or not.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataReader"/> is null or <paramref name="columnName"/> is null, empty or white space.</exception>
        private static bool GetCreatable(MySqlDataReader dataReader, string columnName)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNullOrWhiteSpace(columnName, nameof(columnName));

            return Convert.ToBoolean(dataReader.GetInt32(columnName));
        }

        /// <summary>
        /// Gets whether the storage type is editable from the data reader.
        /// </summary>
        /// <param name="dataReader">The data reader from which to read.</param>
        /// <param name="columnName">The column name for value to read.</param>
        /// <returns>Whether the storage type is editable or not.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataReader"/> is null or <paramref name="columnName"/> is null, empty or white space.</exception>
        private static bool GetEditable(MySqlDataReader dataReader, string columnName)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNullOrWhiteSpace(columnName, nameof(columnName));

            return Convert.ToBoolean(dataReader.GetInt32(columnName));
        }

        /// <summary>
        /// Gets whether the storage type is deletable from the data reader.
        /// </summary>
        /// <param name="dataReader">The data reader from which to read.</param>
        /// <param name="columnName">The column name for value to read.</param>
        /// <returns>Whether the storage type is deletable or not.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataReader"/> is null or <paramref name="columnName"/> is null, empty or white space.</exception>
        private static bool GetDeletable(MySqlDataReader dataReader, string columnName)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNullOrWhiteSpace(columnName, nameof(columnName));

            return Convert.ToBoolean(dataReader.GetInt32(columnName));
        }

        #endregion
    }
}
