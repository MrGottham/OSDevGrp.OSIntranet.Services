using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste
{
    /// <summary>
    /// Data proxy to a given data provider.
    /// </summary>
    public class DataProviderProxy : DataProvider, IDataProviderProxy
    {
        #region Constructors

        /// <summary>
        /// Creates a data proxy to a given data provider.
        /// </summary>
        public DataProviderProxy()
        {
        }

        /// <summary>
        /// Creates a data proxy to a given data provider.
        /// </summary>
        /// <param name="name">Name for the data provider.</param>
        /// <param name="dataSourceStatementIdentifier">Identifier for the data source statement.</param>
        public DataProviderProxy(string name, Guid dataSourceStatementIdentifier)
            : base(name, dataSourceStatementIdentifier)
        {
        }

        #endregion

        #region IMySqlDataProxy<IDataProvider>

        /// <summary>
        /// Gets the unique identification for the data provider.
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
        /// Gets the SQL statement for selecting a given data provider.
        /// </summary>
        /// <param name="dataProvider">Data provider for which to get the SQL statement for selecting.</param>
        /// <returns>SQL statement for selection af given data provider.</returns>
        public virtual string GetSqlQueryForId(IDataProvider dataProvider)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }
            if (dataProvider.Identifier.HasValue)
            {
                return string.Format("SELECT DataProviderIdentifier,Name,DataSourceStatementIdentifier FROM DataProviders WHERE DataProviderIdentifier='{0}'", dataProvider.Identifier.Value.ToString("D").ToUpper());
            }
            throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, dataProvider.Identifier, "Identifier"));
        }

        /// <summary>
        /// Gets the SQL statement to insert this data provider.
        /// </summary>
        /// <returns>SQL statement to insert this data provider.</returns>
        public virtual string GetSqlCommandForInsert()
        {
            return string.Format("INSERT INTO DataProviders (DataProviderIdentifier,Name,DataSourceStatementIdentifier) VALUES('{0}','{1}','{2}')", UniqueId, Name, DataSourceStatementIdentifier.ToString("D").ToUpper());
        }

        /// <summary>
        /// Gets the SQL statement to update this data provider.
        /// </summary>
        /// <returns>SQL statement to update this data provider,</returns>
        public virtual string GetSqlCommandForUpdate()
        {
            return string.Format("UPDATE DataProviders SET Name='{1}',DataSourceStatementIdentifier='{2}' WHERE DataProviderIdentifier='{0}'", UniqueId, Name, DataSourceStatementIdentifier.ToString("D").ToUpper());
        }

        /// <summary>
        /// Gets the SQL statement to delete this data provider.
        /// </summary>
        /// <returns>SQL statement to delete this data provider.</returns>
        public virtual string GetSqlCommandForDelete()
        {
            return string.Format("DELETE FROM DataProviders WHERE DataProviderIdentifier='{0}'", UniqueId);
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

            Identifier = new Guid(mySqlDataReader.GetString("DataProviderIdentifier"));
            Name = mySqlDataReader.GetString("Name");
            DataSourceStatementIdentifier = new Guid(mySqlDataReader.GetString("DataSourceStatementIdentifier"));

            var translationCollection = new List<ITranslation>();
            using (var subDataProvider = (IDataProviderBase) dataProvider.Clone())
            {
                translationCollection.AddRange(subDataProvider.GetCollection<TranslationProxy>(DataRepositoryHelper.GetSqlStatementForSelectingTransactions(DataSourceStatementIdentifier)));
            }
            Translations = translationCollection;
        }

        #endregion
    }
}
