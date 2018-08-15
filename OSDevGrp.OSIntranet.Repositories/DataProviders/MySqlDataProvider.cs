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
    public class MySqlDataProvider : DataProviderBase, IMySqlDataProvider
    {
        #region Private variables

        private readonly MySqlConnection _mySqlConnection;
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

            _mySqlConnection = new MySqlConnection(connectionStringSettings.ConnectionString);
            _clonedWithinTransaction = false;
        }

        /// <summary>
        /// Danner en data provider, som benytter MySql.
        /// </summary>
        /// <param name="mySqlConnection">Eksisterende MySql connection.</param>
        /// <param name="clonedWithinTransaction">True, ved en igangværende transaktion ellers false.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="mySqlConnection"/> is null.</exception>
        private MySqlDataProvider(MySqlConnection mySqlConnection, bool clonedWithinTransaction)
        {
            ArgumentNullGuard.NotNull(mySqlConnection, nameof(mySqlConnection));

            _mySqlConnection = mySqlConnection;
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
            _mySqlConnection.Dispose();
        }

        /// <summary>
        /// Danner ny instans af data provideren til MySql.
        /// </summary>
        /// <returns>Ny instans af data provideren til MSql.</returns>
        public override object Clone()
        {
            return Transaction.Current == null ? new MySqlDataProvider((MySqlConnection) _mySqlConnection.Clone(), false) : new MySqlDataProvider(_mySqlConnection, true);
        }

        /// <summary>
        /// Henter og returnerer data fra MySql.
        /// </summary>
        /// <typeparam name="TDataProxy">Typen på data proxy med data fra MySql.</typeparam>
        /// <param name="query">SQL foresprøgelse efter data.</param>
        /// <returns>Collection indeholdende data proxies.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="query"/> is null, empty or white space.</exception>
        public override IEnumerable<TDataProxy> GetCollection<TDataProxy>(string query)
        {
            ArgumentNullGuard.NotNullOrWhiteSpace(query, nameof(query));

            Open();
            try
            {
                List<TDataProxy> collection = new List<TDataProxy>();
                using (MySqlCommand command = _mySqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = query;
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
        /// <param name="queryForDataProxy">Data proxy, som indeholder nødvendige værdier til fremsøgning i MySql.</param>
        /// <returns>Data proxy med data fra MySql.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="queryForDataProxy"/> is null.</exception>
        public override TDataProxy Get<TDataProxy>(TDataProxy queryForDataProxy)
        {
            ArgumentNullGuard.NotNull(queryForDataProxy, nameof(queryForDataProxy));

            Open();
            try
            {
                string sqlQuery = ((IMySqlDataProxy<TDataProxy>) queryForDataProxy).GetSqlQueryForId(queryForDataProxy);
                using (MySqlCommand command = _mySqlConnection.CreateCommand())
                {
                    bool dataHasBeenReaded = false;
                    TDataProxy dataProxy = new TDataProxy();
                    // Execute the command and read the data.
                    command.CommandType = CommandType.Text;
                    command.CommandText = sqlQuery;
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows == false)
                        {
                            reader.Close();
                            throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, queryForDataProxy.GetType().Name, ((IMySqlDataProxy<TDataProxy>) queryForDataProxy).UniqueId));
                        }
                        if (reader.Read())
                        {
                            dataProxy.MapData(reader, this);
                            dataHasBeenReaded = true;
                        }
                        reader.Close();
                    }
                    // When data has been readed then map the relations.
                    if (dataHasBeenReaded)
                    {
                        dataProxy.MapRelations(this);
                    }
                    return dataProxy;
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
                string sqlCommand = ((IMySqlDataProxy<TDataProxy>) dataProxy).GetSqlCommandForInsert();
                using (MySqlCommand command = _mySqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = sqlCommand;
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
                string sqlCommand = ((IMySqlDataProxy<TDataProxy>) dataProxy).GetSqlCommandForUpdate();
                using (MySqlCommand command = _mySqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = sqlCommand;
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
                string sqlCommand = ((IMySqlDataProxy<TDataProxy>)dataProxy).GetSqlCommandForDelete();
                using (MySqlCommand command = _mySqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = sqlCommand;
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
            if (_clonedWithinTransaction && _mySqlConnection.State == ConnectionState.Open)
            {
                return;
            }
            if (_mySqlConnection.State == ConnectionState.Open)
            {
                return;
            }
            _mySqlConnection.Open();
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
            _mySqlConnection.Close();
        }

        #endregion
    }
}
