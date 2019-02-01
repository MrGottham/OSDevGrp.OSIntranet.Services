using System;
using System.Collections.Generic;
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
    /// Data proxy to a given storage.
    /// </summary>
    public class StorageProxy : Storage, IStorageProxy
    {
        #region Private variables

        private IFoodWasteDataProvider _dataProvider;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a data proxy to a given storage.
        /// </summary>
        public StorageProxy()
        {
        }

        /// <summary>
        /// Creates a data proxy to a given storage.
        /// </summary>
        /// <param name="household">Household where the storage are placed.</param>
        /// <param name="sortOrder">Sort order for the storage.</param>
        /// <param name="storageType">Storage type for the storage.</param>
        /// <param name="temperature">Temperature for the storage.</param>
        /// <param name="creationTime">Creation date and time for when the storage was created.</param>
        /// <param name="description">Description for the storage.</param>
        /// <param name="dataProvider">The data provider which the created data proxy should use.</param>
        public StorageProxy(IHousehold household, int sortOrder, IStorageType storageType, int temperature, DateTime creationTime, string description = null, IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider = null)
            : base(household, sortOrder, storageType, temperature, creationTime, description)
        {
            if (dataProvider == null)
            {
                return;
            }

            _dataProvider = (IFoodWasteDataProvider) dataProvider;
        }

        #endregion

        #region IMySqlDataProxy Members

        /// <summary>
        /// Gets the unique identification for the storage.
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

            Identifier = GetStorageIdentifier(dataReader, "StorageIdentifier");
            SortOrder = GetSortOrder(dataReader, "SortOrder");
            // ReSharper disable StringLiteralTypo
            Description = GetDescription(dataReader, "Descr");
            // ReSharper restore StringLiteralTypo
            Temperature = GetTemperature(dataReader, "Temperature");
            CreationTime = GetCreationTime(dataReader, "CreationTime");

            // ReSharper disable StringLiteralTypo
            Household = dataProvider.Create(new HouseholdProxy(), dataReader, "HouseholdIdentifier", "HouseholdName", "HouseholdDescr", "HouseholdCreationTime");
            // ReSharper restore StringLiteralTypo
            StorageType = dataProvider.Create(new StorageTypeProxy(), dataReader, "StorageTypeIdentifier", "StorageTypeSortOrder", "StorageTypeTemperature", "StorageTypeTemperatureRangeStartValue", "StorageTypeTemperatureRangeEndValue", "StorageTypeCreatable", "StorageTypeEditable", "StorageTypeDeletable");

            _dataProvider = (IFoodWasteDataProvider) dataProvider;
        }

        /// <summary>
        /// Maps relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void MapRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider)
        {
            ArgumentNullGuard.NotNull(dataProvider, nameof(dataProvider));

            _dataProvider = (IFoodWasteDataProvider) dataProvider;
        }

        /// <summary>
        /// Save relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        /// <param name="isInserting">Indication of whether we are inserting or updating</param>
        public virtual void SaveRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, bool isInserting)
        {
            ArgumentNullGuard.NotNull(dataProvider, nameof(dataProvider));

            if (Identifier.HasValue == false)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, Identifier, "Identifier"));
            }

            _dataProvider = (IFoodWasteDataProvider)dataProvider;
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

            _dataProvider = (IFoodWasteDataProvider)dataProvider;
        }

        /// <summary>
        /// Creates the SQL statement for getting this storage.
        /// </summary>
        /// <returns>SQL statement for getting this storage.</returns>
        public virtual MySqlCommand CreateGetCommand()
        {
            return BuildHouseholdDataCommandForSelecting("WHERE s.StorageIdentifier=@storageIdentifier", householdDataCommandBuilder => householdDataCommandBuilder.AddStorageIdentifierParameter(Identifier));
        }

        /// <summary>
        /// Creates the SQL statement for inserting this storage.
        /// </summary>
        /// <returns>SQL statement for inserting this storage.</returns>
        public virtual MySqlCommand CreateInsertCommand()
        {
            // ReSharper disable StringLiteralTypo
            return new HouseholdDataCommandBuilder("INSERT INTO Storages (StorageIdentifier,HouseholdIdentifier,SortOrder,StorageTypeIdentifier,Descr,Temperature,CreationTime) VALUES(@storageIdentifier,@householdIdentifier,@sortOrder,@storageTypeIdentifier,@descr,@temperature,@creationTime)")
            // ReSharper restore StringLiteralTypo
                .AddStorageIdentifierParameter(Identifier)
                .AddHouseholdIdentifierParameter(Household.Identifier)
                .AddStorageSortOrderParameter(SortOrder)
                .AddStorageTypeIdentifierParameter(StorageType.Identifier)
                .AddStorageDescriptionParameter(Description)
                .AddStorageTemperatureParameter(Temperature)
                .AddCreationTimeParameter(CreationTime)
                .Build();
        }

        /// <summary>
        /// Creates the SQL statement for updating this storage.
        /// </summary>
        /// <returns>SQL statement for updating this storage.</returns>
        public virtual MySqlCommand CreateUpdateCommand()
        {
            // ReSharper disable StringLiteralTypo
            return new HouseholdDataCommandBuilder("UPDATE Storages SET HouseholdIdentifier=@householdIdentifier,SortOrder=@sortOrder,StorageTypeIdentifier=@storageTypeIdentifier,Descr=@descr,Temperature=@temperature,CreationTime=@creationTime WHERE StorageIdentifier=@storageIdentifier")
            // ReSharper restore StringLiteralTypo
                .AddStorageIdentifierParameter(Identifier)
                .AddHouseholdIdentifierParameter(Household.Identifier)
                .AddStorageSortOrderParameter(SortOrder)
                .AddStorageTypeIdentifierParameter(StorageType.Identifier)
                .AddStorageDescriptionParameter(Description)
                .AddStorageTemperatureParameter(Temperature)
                .AddCreationTimeParameter(CreationTime)
                .Build();
        }

        /// <summary>
        /// Creates the SQL statement for deleting this storage.
        /// </summary>
        /// <returns>SQL statement for deleting this storage.</returns>
        public virtual MySqlCommand CreateDeleteCommand()
        {
            // ReSharper disable StringLiteralTypo
            return new HouseholdDataCommandBuilder("DELETE FROM Storages WHERE StorageIdentifier=@storageIdentifier")
            // ReSharper restore StringLiteralTypo
                .AddStorageIdentifierParameter(Identifier)
                .Build();
        }

        #endregion

        #region IMySqlDataProxyCreator<IHouseholdProxy> Members

        /// <summary>
        /// Creates an instance of the data proxy to a given storage with values from the data reader.
        /// </summary>
        /// <param name="dataReader">Data reader from which column values should be read.</param>
        /// <param name="dataProvider">Data provider which supports the data reader.</param>
        /// <param name="columnNameCollection">Collection of column names which should be read from the data reader.</param>
        /// <returns>Instance of the data proxy to a given storage with values from the data reader.</returns>
        public virtual IStorageProxy Create(MySqlDataReader dataReader, IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, params string[] columnNameCollection)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNull(dataProvider, nameof(dataProvider))
                .NotNull(columnNameCollection, nameof(columnNameCollection));

            IHouseholdProxy householdProxy = dataProvider.Create(new HouseholdProxy(), dataReader, columnNameCollection[1], columnNameCollection[7], columnNameCollection[8], columnNameCollection[9]);
            IStorageTypeProxy storageTypeProxy = dataProvider.Create(new StorageTypeProxy(), dataReader, columnNameCollection[3], columnNameCollection[10], columnNameCollection[11], columnNameCollection[12], columnNameCollection[13], columnNameCollection[14], columnNameCollection[15], columnNameCollection[16]);
            return new StorageProxy(householdProxy, GetSortOrder(dataReader, columnNameCollection[2]), storageTypeProxy, GetTemperature(dataReader, columnNameCollection[5]), GetCreationTime(dataReader, columnNameCollection[6]), GetDescription(dataReader, columnNameCollection[4]), dataProvider)
            {
                Identifier = GetStorageIdentifier(dataReader, columnNameCollection[0])
            };
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets all the storages for a given household.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        /// <param name="householdIdentifier">The identifier for the household on which to get the storages.</param>
        /// <returns>All the storages for the given household.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataProvider"/> is null.</exception>
        internal static IEnumerable<StorageProxy> GetStorages(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, Guid householdIdentifier)
        {
            ArgumentNullGuard.NotNull(dataProvider, nameof(dataProvider));

            using (IFoodWasteDataProvider subDataProvider = (IFoodWasteDataProvider) dataProvider.Clone())
            {
                MySqlCommand command = BuildHouseholdDataCommandForSelecting("WHERE s.HouseholdIdentifier=@householdIdentifier", householdDataCommandBuilder => householdDataCommandBuilder.AddHouseholdIdentifierParameter(householdIdentifier));
                return subDataProvider.GetCollection<StorageProxy>(command);
            }
        }

        /// <summary>
        /// Deletes all the storages for a given household.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        /// <param name="householdIdentifier">The identifier for the household on which to delete the storages.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataProvider"/> is null.</exception>
        internal static void DeleteStorages(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, Guid householdIdentifier)
        {
            ArgumentNullGuard.NotNull(dataProvider, nameof(dataProvider));

            foreach (StorageProxy storageProxy in GetStorages(dataProvider, householdIdentifier))
            {
                using (IFoodWasteDataProvider subDataProvider = (IFoodWasteDataProvider) dataProvider.Clone())
                {
                    subDataProvider.Delete(storageProxy);
                }
            }
        }

        /// <summary>
        /// Gets the storage identifier from the data reader.
        /// </summary>
        /// <param name="dataReader">The data reader from which to read.</param>
        /// <param name="columnName">The column name for value to read.</param>
        /// <returns>The storage identifier.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataReader"/> is null or <paramref name="columnName"/> is null, empty or white space.</exception>
        private static Guid GetStorageIdentifier(MySqlDataReader dataReader, string columnName)
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
        private static int GetSortOrder(MySqlDataReader dataReader, string columnName)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNullOrWhiteSpace(columnName, nameof(columnName));

            return dataReader.GetInt16(columnName);
        }

        /// <summary>
        /// Gets the description from the data reader.
        /// </summary>
        /// <param name="dataReader">The data reader from which to read.</param>
        /// <param name="columnName">The column name for value to read.</param>
        /// <returns>The description.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataReader"/> is null or <paramref name="columnName"/> is null, empty or white space.</exception>
        private static string GetDescription(MySqlDataReader dataReader, string columnName)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNullOrWhiteSpace(columnName, nameof(columnName));

            int columnNo = dataReader.GetOrdinal(columnName);
            return dataReader.IsDBNull(columnNo) == false ? dataReader.GetString(columnNo) : null;
        }

        /// <summary>
        /// Gets the temperature from the data reader.
        /// </summary>
        /// <param name="dataReader">The data reader from which to read.</param>
        /// <param name="columnName">The column name for value to read.</param>
        /// <returns>The temperature.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataReader"/> is null or <paramref name="columnName"/> is null, empty or white space.</exception>
        private static int GetTemperature(MySqlDataReader dataReader, string columnName)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNullOrWhiteSpace(columnName, nameof(columnName));

            return dataReader.GetInt16(columnName);
        }

        /// <summary>
        /// Gets the time for when the household was created from the data reader.
        /// </summary>
        /// <param name="dataReader">The data reader from which to read.</param>
        /// <param name="columnName">The column name for value to read.</param>
        /// <returns>The time for when the household was created.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataReader"/> is null or <paramref name="columnName"/> is null, empty or white space.</exception>
        private static DateTime GetCreationTime(MySqlDataReader dataReader, string columnName)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNullOrWhiteSpace(columnName, nameof(columnName));

            return dataReader.GetMySqlDateTime(columnName).Value.ToLocalTime();
        }

        /// <summary>
        /// Creates a MySQL command selecting a collection of <see cref="StorageProxy"/>.
        /// </summary>
        /// <param name="whereClause">The WHERE clause which the MySQL command should use.</param>
        /// <param name="parameterAdder">The callback to add MySQL parameters to the MySQL command.</param>
        /// <returns>MySQL command selecting a collection of <see cref="StorageProxy"/>.</returns>
        private static MySqlCommand BuildHouseholdDataCommandForSelecting(string whereClause = null, Action<HouseholdDataCommandBuilder> parameterAdder = null)
        {
            // ReSharper disable StringLiteralTypo
            StringBuilder selectStatementBuilder = new StringBuilder("SELECT s.StorageIdentifier,s.HouseholdIdentifier,s.SortOrder,s.StorageTypeIdentifier,s.Descr,s.Temperature,s.CreationTime,h.Name AS HouseholdName,h.Descr AS HouseholdDescr,h.CreationTime AS HouseholdCreationTime,st.SortOrder AS StorageTypeSortOrder,st.Temperature AS StorageTypeTemperature,st.TemperatureRangeStartValue AS StorageTypeTemperatureRangeStartValue,st.TemperatureRangeEndValue AS StorageTypeTemperatureRangeEndValue,st.Creatable AS StorageTypeCreatable,st.Editable AS StorageTypeEditable,st.Deletable AS StorageTypeDeletable FROM Storages AS s INNER JOIN Households AS h ON h.HouseholdIdentifier=s.HouseholdIdentifier INNER JOIN StorageTypes AS st ON st.StorageTypeIdentifier=s.StorageTypeIdentifier");
            // ReSharper restore StringLiteralTypo
            if (string.IsNullOrWhiteSpace(whereClause) == false)
            {
                selectStatementBuilder.Append($" {whereClause}");
            }

            HouseholdDataCommandBuilder householdDataCommandBuilder = new HouseholdDataCommandBuilder(selectStatementBuilder.ToString());
            if (parameterAdder == null)
            {
                return householdDataCommandBuilder.Build();
            }

            parameterAdder(householdDataCommandBuilder);
            return householdDataCommandBuilder.Build();
        }

        #endregion
    }
}
