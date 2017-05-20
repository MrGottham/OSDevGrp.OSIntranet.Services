using System;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
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
        /// <param name="sortOrder">Order for sortering storage types.</param>
        /// <param name="temperature">Default temperature for the storage type.</param>
        /// <param name="temperatureRange">Temperature range for the storage type.</param>
        /// <param name="creatable">Indicates whether household members can create storages of this type.</param>
        /// <param name="editable">Indicates whether household members can edit storages of this type.</param>
        /// <param name="deletable">Indicates whether household members can delete storages of this type.</param>
        public StorageTypeProxy(int sortOrder, int temperature, IRange<int> temperatureRange, bool creatable, bool editable, bool deletable) 
            : base(sortOrder, temperature, temperatureRange, creatable, editable, deletable)
        {
        }

        #endregion

        #region IMySqlDataProxy<IStorageType> Members

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

        /// <summary>
        /// Gets the SQL statement for selecting a given storage type.
        /// </summary>
        /// <param name="storageType">Storage type for which to get the SQL statement for selecting.</param>
        /// <returns>SQL statement for selecting a given storage type.</returns>
        public virtual string GetSqlQueryForId(IStorageType storageType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the SQL statement to insert this storage type.
        /// </summary>
        /// <returns>SQL statement to insert this storage type.</returns>
        public virtual string GetSqlCommandForInsert()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the SQL statement to update this storage type.
        /// </summary>
        /// <returns>SQL statement to update this storage type.</returns>
        public virtual string GetSqlCommandForUpdate()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the SQL statement to delete this storage type.
        /// </summary>
        /// <returns>SQL statement to delete this storage type.</returns>
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
        /// <param name="isInserting">Indication of whether we are inserting or updating</param>
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
