using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.DataProviders;
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

        /// <summary>
        /// Gets the SQL statement for selecting a given storage type.
        /// </summary>
        /// <param name="storageType">Storage type for which to get the SQL statement for selecting.</param>
        /// <returns>SQL statement for selecting a given storage type.</returns>
        public virtual string GetSqlQueryForId(IStorageType storageType)
        {
            if (storageType == null)
            {
                throw new ArgumentNullException(nameof(storageType));
            }
            if (storageType.Identifier.HasValue)
            {
                return $"SELECT StorageTypeIdentifier,SortOrder,Temperature,TemperatureRangeStartValue,TemperatureRangeEndValue,Creatable,Editable,Deletable FROM StorageTypes WHERE StorageTypeIdentifier='{storageType.Identifier.Value.ToString("D").ToUpper()}'";
            }
            throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, storageType.Identifier, "Identifier"));
        }

        /// <summary>
        /// Gets the SQL statement to insert this storage type.
        /// </summary>
        /// <returns>SQL statement to insert this storage type.</returns>
        public virtual string GetSqlCommandForInsert()
        {
            return $"INSERT INTO StorageTypes (StorageTypeIdentifier,SortOrder,Temperature,TemperatureRangeStartValue,TemperatureRangeEndValue,Creatable,Editable,Deletable) VALUES('{UniqueId}',{SortOrder},{Temperature},{TemperatureRange.StartValue},{TemperatureRange.EndValue},{Convert.ToInt32(Creatable)},{Convert.ToInt32(Editable)},{Convert.ToInt32(Deletable)})";
        }

        /// <summary>
        /// Gets the SQL statement to update this storage type.
        /// </summary>
        /// <returns>SQL statement to update this storage type.</returns>
        public virtual string GetSqlCommandForUpdate()
        {
            return $"UPDATE StorageTypes SET SortOrder={SortOrder},Temperature={Temperature},TemperatureRangeStartValue={TemperatureRange.StartValue},TemperatureRangeEndValue={TemperatureRange.EndValue},Creatable={Convert.ToInt32(Creatable)},Editable={Convert.ToInt32(Editable)},Deletable={Convert.ToInt32(Deletable)} WHERE StorageTypeIdentifier='{UniqueId}'";
        }

        /// <summary>
        /// Gets the SQL statement to delete this storage type.
        /// </summary>
        /// <returns>SQL statement to delete this storage type.</returns>
        public virtual string GetSqlCommandForDelete()
        {
            return $"DELETE FROM StorageTypes WHERE StorageTypeIdentifier='{UniqueId}'";
        }

        #endregion

        #region IDataProxyBase Members

        /// <summary>
        /// Maps data from the data reader.
        /// </summary>
        /// <param name="dataReader">Data reader.</param>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void MapData(object dataReader, IDataProviderBase<MySqlCommand> dataProvider)
        {
            if (dataReader == null)
            {
                throw new ArgumentNullException(nameof(dataReader));
            }
            if (dataProvider == null)
            {
                throw new ArgumentNullException(nameof(dataProvider));
            }

            MySqlDataReader mySqlDataReader = dataReader as MySqlDataReader;
            if (mySqlDataReader == null)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, "dataReader", dataReader.GetType().Name));
            }

            Identifier = Guid.Parse(mySqlDataReader.GetString("StorageTypeIdentifier"));
            SortOrder = mySqlDataReader.GetInt16("SortOrder");
            Temperature = mySqlDataReader.GetInt16("Temperature");
            TemperatureRange = new Range<int>(
                mySqlDataReader.GetInt16("TemperatureRangeStartValue"),
                mySqlDataReader.GetInt16("TemperatureRangeEndValue"));
            Creatable = Convert.ToBoolean(mySqlDataReader.GetInt32("Creatable"));
            Editable = Convert.ToBoolean(mySqlDataReader.GetInt32("Editable"));
            Deletable = Convert.ToBoolean(mySqlDataReader.GetInt32("Deletable"));
        }

        /// <summary>
        /// Maps relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void MapRelations(IDataProviderBase<MySqlCommand> dataProvider)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException(nameof(dataProvider));
            }

            Translations = new List<ITranslation>(TranslationProxy.GetDomainObjectTranslations(dataProvider, Guid.Parse(UniqueId)));
        }

        /// <summary>
        /// Save relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        /// <param name="isInserting">Indication of whether we are inserting or updating</param>
        public virtual void SaveRelations(IDataProviderBase<MySqlCommand> dataProvider, bool isInserting)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Delete relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void DeleteRelations(IDataProviderBase<MySqlCommand> dataProvider)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Creates the SQL statement for getting this storage type.
        /// </summary>
        /// <returns>SQL statement for getting this storage type.</returns>
        public virtual MySqlCommand CreateGetCommand()
        {
            return new FoodWasteCommandBuilder(GetSqlQueryForId(this)).Build();
        }

        /// <summary>
        /// Creates the SQL statement for inserting this storage type.
        /// </summary>
        /// <returns>SQL statement for inserting this storage type.</returns>
        public virtual MySqlCommand CreateInsertCommand()
        {
            return new FoodWasteCommandBuilder(GetSqlCommandForInsert()).Build();
        }

        /// <summary>
        /// Creates the SQL statement for updating this storage type.
        /// </summary>
        /// <returns>SQL statement for updating this storage type.</returns>
        public virtual MySqlCommand CreateUpdateCommand()
        {
            return new FoodWasteCommandBuilder(GetSqlCommandForUpdate()).Build();
        }

        /// <summary>
        /// Creates the SQL statement for deleting this storage type.
        /// </summary>
        /// <returns>SQL statement for deleting this storage type.</returns>
        public virtual MySqlCommand CreateDeleteCommand()
        {
            return new FoodWasteCommandBuilder(GetSqlCommandForDelete()).Build();
        }

        #endregion
    }
}
