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
    /// Data proxy to a given foreign key to a domain object in the food waste domain.
    /// </summary>
    public class ForeignKeyProxy : ForeignKey, IForeignKeyProxy
    {
        #region Constructors

        /// <summary>
        /// Creates a data proxy to a given foreign key to a domain object in the food waste domain.
        /// </summary>
        public ForeignKeyProxy()
        {
        }

        /// <summary>
        /// Creates a data proxy to a given foreign key to a domain object in the food waste domain.
        /// </summary>
        /// <param name="dataProvider">Data provider who own the foreign key.</param>
        /// <param name="foreignKeyForIdentifier">Identifier for the domain object which has this foreign key.</param>
        /// <param name="foreignKeyForType">Type which has this foreign key.</param>
        /// <param name="foreignKeyValue">Value of the foreign key.</param>
        public ForeignKeyProxy(IDataProvider dataProvider, Guid foreignKeyForIdentifier, Type foreignKeyForType, string foreignKeyValue)
            : base(dataProvider, foreignKeyForIdentifier, foreignKeyForType, foreignKeyValue)
        {
        }

        #endregion

        #region IMySqlDataProxy<IForeignKey>

        /// <summary>
        ///  Gets the unique identification for the foreign key.
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
        /// Gets the SQL statement for selecting a given foreign key.
        /// </summary>
        /// <param name="foreignKey">Foreign key for which to get the SQL statement for selecting.</param>
        /// <returns>SQL statement for selection af given foreign key.</returns>
        public virtual string GetSqlQueryForId(IForeignKey foreignKey)
        {
            if (foreignKey == null)
            {
                throw new ArgumentNullException("foreignKey");
            }
            if (foreignKey.Identifier.HasValue)
            {
                return string.Format("SELECT ForeignKeyIdentifier,DataProviderIdentifier,ForeignKeyForIdentifier,ForeignKeyForTypes,ForeignKeyValue FROM ForeignKeys WHERE ForeignKeyIdentifier='{0}'", foreignKey.Identifier.Value.ToString("D").ToUpper());
            }
            throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, foreignKey.Identifier, "Identifier"));
        }

        /// <summary>
        /// Gets the SQL statement to insert this foreign key.
        /// </summary>
        /// <returns>SQL statement to insert this foreign key.</returns>
        public virtual string GetSqlCommandForInsert()
        {
            return string.Format("INSERT INTO ForeignKeys (ForeignKeyIdentifier,DataProviderIdentifier,ForeignKeyForIdentifier,ForeignKeyForTypes,ForeignKeyValue) VALUES('{0}','{1}','{2}','{3}','{4}')", UniqueId, DataProvider.Identifier.HasValue ? DataProvider.Identifier.Value.ToString("D").ToUpper() : Guid.Empty.ToString("D").ToUpper(), ForeignKeyForIdentifier, string.Join(";", ForeignKeyForTypes.Select(m => m.Name)), ForeignKeyValue);
        }

        /// <summary>
        /// Gets the SQL statement to update this foreign key.
        /// </summary>
        /// <returns>SQL statement to update this foreign key.</returns>
        public virtual string GetSqlCommandForUpdate()
        {
            return string.Format("UPDATE ForeignKeys SET DataProviderIdentifier='{1}',ForeignKeyForIdentifier='{2}',ForeignKeyForTypes='{3}',ForeignKeyValue='{4}' WHERE ForeignKeyIdentifier='{0}'", UniqueId, DataProvider.Identifier.HasValue ? DataProvider.Identifier.Value.ToString("D").ToUpper() : Guid.Empty.ToString("D").ToUpper(), ForeignKeyForIdentifier, string.Join(";", ForeignKeyForTypes.Select(m => m.Name)), ForeignKeyValue);
        }

        /// <summary>
        /// Gets the SQL statement to delete this foreign key.
        /// </summary>
        /// <returns>SQL statement to delete this foreign key.</returns>
        public virtual string GetSqlCommandForDelete()
        {
            return string.Format("DELETE FROM ForeignKeys WHERE ForeignKeyIdentifier='{0}'", UniqueId);
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

            Identifier = new Guid(mySqlDataReader.GetString("ForeignKeyIdentifier"));
            ForeignKeyForIdentifier = new Guid(mySqlDataReader.GetString("ForeignKeyForIdentifier"));
            ForeignKeyValue = mySqlDataReader.GetString("ForeignKeyValue");

            var foreignKeyForTypes = new List<Type>();
            foreach (var foreignKeyForTypeName in mySqlDataReader.GetString("ForeignKeyForTypes").Split(';').Where(m => string.IsNullOrWhiteSpace(m) == false))
            {
                // ReSharper disable EmptyGeneralCatchClause
                try
                {
                    var assembly = typeof(IDomainObject).Assembly;
                    var type = assembly.GetType(foreignKeyForTypeName);
                    if (type != null)
                    {
                        foreignKeyForTypes.Add(type);
                    }
                }
                catch
                {
                    // Don't throw the exception. Just ignore the type.
                }
                // ReSharper restore EmptyGeneralCatchClause
            }

            var dataProviderIdentifier = new Guid(mySqlDataReader.GetString("DataProviderIdentifier"));
            using (var subDataProvider = (IDataProviderBase) dataProvider.Clone())
            {
                var dataProviderProxy = new DataProviderProxy
                {
                    Identifier = dataProviderIdentifier
                };
                DataProvider = subDataProvider.Get(dataProviderProxy);
            }
        }

        #endregion
    }
}
