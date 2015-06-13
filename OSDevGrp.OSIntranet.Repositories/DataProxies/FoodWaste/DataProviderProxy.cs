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
                    return Identifier.Value.ToString().ToUpper();
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the SQL statement to insert this data provider.
        /// </summary>
        /// <returns>SQL statement to insert this data provider.</returns>
        public virtual string GetSqlCommandForInsert()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the SQL statement to update this data provider.
        /// </summary>
        /// <returns>SQL statement to update this data provider,</returns>
        public virtual string GetSqlCommandForUpdate()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the SQL statement to delete this data provider.
        /// </summary>
        /// <returns>SQL statement to delete this data provider.</returns>
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

        #endregion
    }
}
