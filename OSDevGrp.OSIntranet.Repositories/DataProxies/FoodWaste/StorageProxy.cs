using System;
using MySql.Data.MySqlClient;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.DataProviders;
using OSDevGrp.OSIntranet.Repositories.FoodWaste;
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

        private Guid? _householdIdentifier;
        private Guid? _storageTypeIdentifier;
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
        public StorageProxy(IHousehold household, int sortOrder, IStorageType storageType, int temperature, DateTime creationTime, string description = null)
            : base(household, sortOrder, storageType, temperature, creationTime, description)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the household where the storage are placed.
        /// </summary>
        public override IHousehold Household
        {
            get
            {
                if (base.Household != null || _householdIdentifier.HasValue == false || _dataProvider == null)
                {
                    return base.Household;
                }
                using (IFoodWasteDataProvider subDataProvider = (IFoodWasteDataProvider) _dataProvider.Clone())
                {
                    HouseholdProxy householdProxy = new HouseholdProxy
                    {
                        Identifier = _householdIdentifier.Value
                    };
                    base.Household = subDataProvider.Get(householdProxy);
                }
                return base.Household;
            }
        }

        /// <summary>
        /// Gets the storage type for the storage.
        /// </summary>
        public override IStorageType StorageType
        {
            get
            {
                if (base.StorageType != null || _storageTypeIdentifier.HasValue == false || _dataProvider == null)
                {
                    return base.StorageType;
                }
                using (IFoodWasteDataProvider subDataProvider = (IFoodWasteDataProvider) _dataProvider.Clone())
                {
                    StorageTypeProxy storageTypeProxy = new StorageTypeProxy
                    {
                        Identifier = _storageTypeIdentifier.Value
                    };
                    base.StorageType = subDataProvider.Get(storageTypeProxy);
                }
                return base.StorageType;
            }
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

        /// <summary>
        /// Gets the SQL statement for selecting a given storage.
        /// </summary>
        /// <param name="storage">Storage for which to get the SQL statement for selecting.</param>
        /// <returns>SQL statement for selecting a given storage.</returns>
        public virtual string GetSqlQueryForId(IStorage storage)
        {
            if (storage == null)
            {
                throw new ArgumentNullException(nameof(storage));
            }
            if (storage.Identifier.HasValue)
            {
                return $"SELECT StorageIdentifier,HouseholdIdentifier,SortOrder,StorageTypeIdentifier,Descr,Temperature,CreationTime FROM Storages WHERE StorageIdentifier='{storage.Identifier.Value.ToString("D").ToUpper()}'";
            }
            throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, storage.Identifier, "Identifier"));
        }

        /// <summary>
        /// Gets the SQL statement to insert this storage.
        /// </summary>
        /// <returns>SQL statement to insert this storage.</returns>
        public virtual string GetSqlCommandForInsert()
        {
            string householdIdentifierAsSql = (Household?.Identifier ?? Guid.Empty).ToString("D").ToUpper();
            string storageTypeIdentifierAsSql = (StorageType?.Identifier ?? Guid.Empty).ToString("D").ToUpper();
            string desriptionAsSql = string.IsNullOrWhiteSpace(Description) ? "NULL" : $"'{Description}'";

            return $"INSERT INTO Storages (StorageIdentifier,HouseholdIdentifier,SortOrder,StorageTypeIdentifier,Descr,Temperature,CreationTime) VALUES('{UniqueId}','{householdIdentifierAsSql}',{SortOrder},'{storageTypeIdentifierAsSql}',{desriptionAsSql},{Temperature},{DataRepositoryHelper.GetSqlValueForDateTime(CreationTime)})";
        }

        /// <summary>
        /// Gets the SQL statement to update this storage.
        /// </summary>
        /// <returns>SQL statement to update this storage.</returns>
        public virtual string GetSqlCommandForUpdate()
        {
            string householdIdentifierAsSql = (Household?.Identifier ?? Guid.Empty).ToString("D").ToUpper();
            string storageTypeIdentifierAsSql = (StorageType?.Identifier ?? Guid.Empty).ToString("D").ToUpper();
            string desriptionAsSql = string.IsNullOrWhiteSpace(Description) ? "NULL" : $"'{Description}'";

            return $"UPDATE Storages SET HouseholdIdentifier='{householdIdentifierAsSql}',SortOrder={SortOrder},StorageTypeIdentifier='{storageTypeIdentifierAsSql}',Descr={desriptionAsSql},Temperature={Temperature},CreationTime={DataRepositoryHelper.GetSqlValueForDateTime(CreationTime)} WHERE StorageIdentifier='{UniqueId}'";
        }

        /// <summary>
        /// Gets the SQL statement to delete this storage.
        /// </summary>
        /// <returns>SQL statement to delete this storage.</returns>
        public virtual string GetSqlCommandForDelete()
        {
            return $"DELETE FROM Storages WHERE StorageIdentifier='{UniqueId}'";
        }

        #endregion

        #region IDataProxyBase Members

        /// <summary>
        /// Maps data from the data reader.
        /// </summary>
        /// <param name="dataReader">Data reader.</param>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void MapData(MySqlDataReader dataReader, IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider)
        {
            if (dataReader == null)
            {
                throw new ArgumentNullException(nameof(dataReader));
            }
            if (dataProvider == null)
            {
                throw new ArgumentNullException(nameof(dataProvider));
            }

            Identifier = new Guid(dataReader.GetString("StorageIdentifier"));
            SortOrder = dataReader.GetInt16("SortOrder");
            Temperature = dataReader.GetInt16("Temperature");
            CreationTime = dataReader.GetDateTime("CreationTime").ToLocalTime();

            int descriptionColumnNo = dataReader.GetOrdinal("Descr");
            if (dataReader.IsDBNull(descriptionColumnNo) == false)
            {
                Description = dataReader.GetString(descriptionColumnNo);
            }

            _householdIdentifier = new Guid(dataReader.GetString("HouseholdIdentifier"));
            _storageTypeIdentifier = new Guid(dataReader.GetString("StorageTypeIdentifier"));

            if (_dataProvider == null)
            {
                _dataProvider = (IFoodWasteDataProvider) dataProvider;
            }
        }

        /// <summary>
        /// Maps relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void MapRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException(nameof(dataProvider));
            }

            if (_dataProvider == null)
            {
                _dataProvider = (IFoodWasteDataProvider) dataProvider;
            }
        }

        /// <summary>
        /// Save relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        /// <param name="isInserting">Indication of whether we are inserting or updating</param>
        public virtual void SaveRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, bool isInserting)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException(nameof(dataProvider));
            }

            if (_dataProvider == null)
            {
                _dataProvider = (IFoodWasteDataProvider) dataProvider;
            }
        }

        /// <summary>
        /// Delete relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void DeleteRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException(nameof(dataProvider));
            }

            if (_dataProvider == null)
            {
                _dataProvider = (IFoodWasteDataProvider) dataProvider;
            }
        }

        /// <summary>
        /// Creates the SQL statement for getting this storage.
        /// </summary>
        /// <returns>SQL statement for getting this storage.</returns>
        public virtual MySqlCommand CreateGetCommand()
        {
            return new FoodWasteCommandBuilder(GetSqlQueryForId(this)).Build();
        }

        /// <summary>
        /// Creates the SQL statement for inserting this storage.
        /// </summary>
        /// <returns>SQL statement for inserting this storage.</returns>
        public virtual MySqlCommand CreateInsertCommand()
        {
            return new FoodWasteCommandBuilder(GetSqlCommandForInsert()).Build();
        }

        /// <summary>
        /// Creates the SQL statement for updating this storage.
        /// </summary>
        /// <returns>SQL statement for updating this storage.</returns>
        public virtual MySqlCommand CreateUpdateCommand()
        {
            return new FoodWasteCommandBuilder(GetSqlCommandForUpdate()).Build();
        }

        /// <summary>
        /// Creates the SQL statement for deleting this storage.
        /// </summary>
        /// <returns>SQL statement for deleting this storage.</returns>
        public virtual MySqlCommand CreateDeleteCommand()
        {
            return new FoodWasteCommandBuilder(GetSqlCommandForDelete()).Build();
        }

        #endregion
    }
}
