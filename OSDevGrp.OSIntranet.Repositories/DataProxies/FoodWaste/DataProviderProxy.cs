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
        /// <param name="handlesPayments">Indication of whether the data provider handles payments.</param>
        /// <param name="dataSourceStatementIdentifier">Identifier for the data source statement.</param>
        public DataProviderProxy(string name, bool handlesPayments, Guid dataSourceStatementIdentifier)
            : base(name, handlesPayments, dataSourceStatementIdentifier)
        {
        }

        #endregion

        #region IMySqlDataProxy

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
                return string.Format("SELECT DataProviderIdentifier,Name,HandlesPayments,DataSourceStatementIdentifier FROM DataProviders WHERE DataProviderIdentifier='{0}'", dataProvider.Identifier.Value.ToString("D").ToUpper());
            }
            throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, dataProvider.Identifier, "Identifier"));
        }

        /// <summary>
        /// Gets the SQL statement to insert this data provider.
        /// </summary>
        /// <returns>SQL statement to insert this data provider.</returns>
        public virtual string GetSqlCommandForInsert()
        {
            return string.Format("INSERT INTO DataProviders (DataProviderIdentifier,Name,HandlesPayments,DataSourceStatementIdentifier) VALUES('{0}','{1}',{2},'{3}')", UniqueId, Name, Convert.ToInt32(HandlesPayments), DataSourceStatementIdentifier.ToString("D").ToUpper());
        }

        /// <summary>
        /// Gets the SQL statement to update this data provider.
        /// </summary>
        /// <returns>SQL statement to update this data provider,</returns>
        public virtual string GetSqlCommandForUpdate()
        {
            return string.Format("UPDATE DataProviders SET Name='{1}',HandlesPayments={2},DataSourceStatementIdentifier='{3}' WHERE DataProviderIdentifier='{0}'", UniqueId, Name, Convert.ToInt32(HandlesPayments), DataSourceStatementIdentifier.ToString("D").ToUpper());
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
        public virtual void MapData(MySqlDataReader dataReader, IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider)
        {
            if (dataReader == null)
            {
                throw new ArgumentNullException("dataReader");
            }
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }

            Identifier = new Guid(dataReader.GetString("DataProviderIdentifier"));
            Name = dataReader.GetString("Name");
            HandlesPayments = Convert.ToBoolean(dataReader.GetInt32("HandlesPayments"));
            DataSourceStatementIdentifier = new Guid(dataReader.GetString("DataSourceStatementIdentifier"));
        }

        /// <summary>
        /// Maps relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void MapRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }

            Translations = new List<ITranslation>(TranslationProxy.GetDomainObjectTranslations(dataProvider, DataSourceStatementIdentifier));
        }

        /// <summary>
        /// Save relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        /// <param name="isInserting">Indication of whether we are inserting or updating.</param>
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
        /// Creates the SQL statement for getting this data provider.
        /// </summary>
        /// <returns>SQL statement for getting this data provider.</returns>
        public virtual MySqlCommand CreateGetCommand()
        {
            return new FoodWasteCommandBuilder(GetSqlQueryForId(this)).Build();
        }

        /// <summary>
        /// Creates the SQL statement for inserting this data provider.
        /// </summary>
        /// <returns>SQL statement for inserting this data provider.</returns>
        public virtual MySqlCommand CreateInsertCommand()
        {
            return new FoodWasteCommandBuilder(GetSqlCommandForInsert()).Build();
        }

        /// <summary>
        /// Creates the SQL statement for updating this data provider.
        /// </summary>
        /// <returns>SQL statement for updating this data provider.</returns>
        public virtual MySqlCommand CreateUpdateCommand()
        {
            return new FoodWasteCommandBuilder(GetSqlCommandForUpdate()).Build();
        }

        /// <summary>
        /// Creates the SQL statement for deleting this data provider.
        /// </summary>
        /// <returns>SQL statement for deleting this data provider.</returns>
        public virtual MySqlCommand CreateDeleteCommand()
        {
            return new FoodWasteCommandBuilder(GetSqlCommandForDelete()).Build();
        }

        #endregion
    }
}
