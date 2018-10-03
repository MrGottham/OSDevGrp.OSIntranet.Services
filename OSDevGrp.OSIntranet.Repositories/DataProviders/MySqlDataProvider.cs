using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Transactions;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies;
using OSDevGrp.OSIntranet.Resources;
using MySql.Data.MySqlClient;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Guards;

namespace OSDevGrp.OSIntranet.Repositories.DataProviders
{
    /// <summary>
    /// Data provider, som benytter MySql.
    /// </summary>
    public class MySqlDataProvider : DataProviderBase<MySqlDataReader, MySqlCommand>, IMySqlDataProvider
    {
        #region Private variables

        protected readonly MySqlConnection MySqlConnection;
        private readonly bool _clonedWithinTransaction;

        #endregion

        #region Constructors

        /// <summary>
        /// Danner en data provider, som benytter MySql.
        /// </summary>
        /// <param name="connectionStringSettings">Konfiguration for en connection streng.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="connectionStringSettings"/> is null.</exception>
        public MySqlDataProvider(ConnectionStringSettings connectionStringSettings)
        {
            ArgumentNullGuard.NotNull(connectionStringSettings, nameof(connectionStringSettings));

            MySqlConnection = new MySqlConnection(connectionStringSettings.ConnectionString);
            _clonedWithinTransaction = false;
        }

        /// <summary>
        /// Danner en data provider, som benytter MySql.
        /// </summary>
        /// <param name="mySqlConnection">Eksisterende MySql connection.</param>
        /// <param name="clonedWithinTransaction">True, ved en igangværende transaktion ellers false.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="mySqlConnection"/> is null.</exception>
        protected MySqlDataProvider(MySqlConnection mySqlConnection, bool clonedWithinTransaction)
        {
            ArgumentNullGuard.NotNull(mySqlConnection, nameof(mySqlConnection));

            MySqlConnection = mySqlConnection;
            _clonedWithinTransaction = clonedWithinTransaction;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Frigørelse af allokerede ressourcer i data provideren til MySql.
        /// </summary>
        public override void Dispose()
        {
            if (_clonedWithinTransaction)
            {
                return;
            }
            MySqlConnection.Dispose();
        }

        /// <summary>
        /// Danner ny instans af data provideren til MySql.
        /// </summary>
        /// <returns>Ny instans af data provideren til MSql.</returns>
        public override object Clone()
        {
            return Transaction.Current == null ? new MySqlDataProvider((MySqlConnection) MySqlConnection.Clone(), false) : new MySqlDataProvider(MySqlConnection, true);
        }

        /// <summary>
        /// Henter og returnerer data fra MySql.
        /// </summary>
        /// <typeparam name="TDataProxy">Typen på data proxy med data fra MySql.</typeparam>
        /// <param name="queryCommand">Database command for the SQL query statement.</param>
        /// <returns>Collection indeholdende data proxies.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="queryCommand"/> is null.</exception>
        public override IEnumerable<TDataProxy> GetCollection<TDataProxy>(MySqlCommand queryCommand)
        {
            ArgumentNullGuard.NotNull(queryCommand, nameof(queryCommand));

            Open();
            try
            {
                List<TDataProxy> collection = new List<TDataProxy>();
                using (MySqlCommand command = queryCommand)
                {
                    command.Connection = MySqlConnection;
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows == false)
                        {
                            reader.Close();
                            return collection;
                        }
                        while (reader.Read())
                        {
                            TDataProxy dataProxy = new TDataProxy();
                            dataProxy.MapData(reader, this);
                            collection.Add(dataProxy);
                        }
                        reader.Close();
                    }
                }
                collection.ForEach(proxy => proxy.MapRelations(this));
                return collection;
            }
            finally
            {
                Close();
            }
        }

        /// <summary>
        /// Henter og returnerer en given data proxy fra MySql.
        /// </summary>
        /// <typeparam name="TDataProxy">Typen på data proxy med data fra MySql.</typeparam>
        /// <param name="dataProxy">Data proxy, som indeholder nødvendige værdier til fremsøgning i MySql.</param>
        /// <returns>Data proxy med data fra MySql.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="dataProxy"/> is null.</exception>
        public override TDataProxy Get<TDataProxy>(TDataProxy dataProxy)
        {
            ArgumentNullGuard.NotNull(dataProxy, nameof(dataProxy));

            Open();
            try
            {
                using (MySqlCommand command = dataProxy.CreateGetCommand())
                {
                    command.Connection = MySqlConnection;

                    bool dataHasBeenReaded = false;
                    TDataProxy result = new TDataProxy();
                    
                    // Execute the command and read the data.
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows == false)
                        {
                            reader.Close();
                            throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, dataProxy.GetType().Name, ((IMySqlDataProxy) dataProxy).UniqueId));
                        }
                        if (reader.Read())
                        {
                            result.MapData(reader, this);
                            dataHasBeenReaded = true;
                        }
                        reader.Close();
                    }
                    
                    // When data has been readed then map the relations.
                    if (dataHasBeenReaded)
                    {
                        result.MapRelations(this);
                    }

                    return result;
                }
            }
            finally
            {
                Close();
            }
        }

        /// <summary>
        /// Tilføjer data i en data proxy til MySql.
        /// </summary>
        /// <typeparam name="TDataProxy">Typen på data proxy med data til MySql.</typeparam>
        /// <param name="dataProxy">Data proxy med data, som skal tilføjes i MySql.</param>
        /// <returns>Data proxy med tilføjede data.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="dataProxy"/> is null.</exception>
        public override TDataProxy Add<TDataProxy>(TDataProxy dataProxy)
        {
            ArgumentNullGuard.NotNull(dataProxy, nameof(dataProxy));

            Open();
            try
            {
                using (MySqlCommand command = dataProxy.CreateInsertCommand())
                {
                    command.Connection = MySqlConnection;
                    command.ExecuteNonQuery();
                }
                dataProxy.SaveRelations(this, true);
                return dataProxy;
            }
            finally
            {
                Close();
            }
        }

        /// <summary>
        /// Gemmer data fra en data proxy i MySql.
        /// </summary>
        /// <typeparam name="TDataProxy">Typen på data proxy med data til MySql.</typeparam>
        /// <param name="dataProxy">Data proxy med data, som skal gemmes i MySql.</param>
        /// <returns>Data proxy med gemte data.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="dataProxy"/> is null.</exception>
        public override TDataProxy Save<TDataProxy>(TDataProxy dataProxy)
        {
            ArgumentNullGuard.NotNull(dataProxy, nameof(dataProxy));

            Open();
            try
            {
                using (MySqlCommand command = dataProxy.CreateUpdateCommand())
                {
                    command.Connection = MySqlConnection;
                    command.ExecuteNonQuery();
                }
                dataProxy.SaveRelations(this, false);
                return dataProxy;
            }
            finally
            {
                Close();
            }
        }

        /// <summary>
        /// Sletter data i en data proxy fra MySql.
        /// </summary>
        /// <typeparam name="TDataProxy">Typen på data proxy med data til MySql.</typeparam>
        /// <param name="dataProxy">Data proxy med data, som skal slette fra MySql.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="dataProxy"/> is null.</exception>
        public override void Delete<TDataProxy>(TDataProxy dataProxy)
        {
            ArgumentNullGuard.NotNull(dataProxy, nameof(dataProxy));

            Open();
            try
            {
                dataProxy.DeleteRelations(this);
                using (MySqlCommand command = dataProxy.CreateDeleteCommand())
                {
                    command.Connection = MySqlConnection;
                    command.ExecuteNonQuery();
                }
            }
            finally
            {
                Close();
            }
        }

        /// <summary>
        /// Opens the MySQL connection when it's not cloned within a transaction scope.
        /// </summary>
        private void Open()
        {
            if (_clonedWithinTransaction && MySqlConnection.State == ConnectionState.Open)
            {
                return;
            }
            if (MySqlConnection.State == ConnectionState.Open)
            {
                return;
            }
            MySqlConnection.Open();
        }

        /// <summary>
        /// Closed the MySQL connection when it's not cloned within a transaction scope.
        /// </summary>
        private void Close()
        {
            if (_clonedWithinTransaction)
            {
                return;
            }
            MySqlConnection.Close();
        }

        #endregion
    }
}
