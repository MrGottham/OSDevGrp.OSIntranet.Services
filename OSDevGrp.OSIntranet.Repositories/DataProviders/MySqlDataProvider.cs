using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Transactions;
using MySql.Data.MySqlClient;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Guards;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.Repositories.DataProviders
{
    /// <summary>
    /// Data provider, som benytter MySql.
    /// </summary>
    public class MySqlDataProvider : DataProviderBase<MySqlDataReader, MySqlCommand>, IMySqlDataProvider
    {
        #region Private variables

        private readonly MySqlConnection _mySqlConnection;
        private readonly bool _clonedWithReusableConnection;

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
            _clonedWithReusableConnection = false;
        }

        /// <summary>
        /// Danner en data provider, som benytter MySql.
        /// </summary>
        /// <param name="mySqlConnection">Eksisterende MySql connection.</param>
        /// <param name="clonedWithReusableConnection">True, ved en genbrugelig conntion ellers false.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="mySqlConnection"/> is null.</exception>
        protected MySqlDataProvider(MySqlConnection mySqlConnection, bool clonedWithReusableConnection)
        {
            ArgumentNullGuard.NotNull(mySqlConnection, nameof(mySqlConnection));

            _mySqlConnection = mySqlConnection;
            _clonedWithReusableConnection = clonedWithReusableConnection;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Frigørelse af allokerede ressourcer i data provideren til MySql.
        /// </summary>
        public override void Dispose()
        {
            if (_clonedWithReusableConnection)
            {
                return;
            }
            _mySqlConnection.Dispose();
        }

        /// <summary>
        /// Danner ny instans af data provideren til MySql.
        /// </summary>
        /// <returns>Ny instans af data provideren til MySql.</returns>
        public sealed override object Clone()
        {
            if (Transaction.Current != null)
            {
                return Clone(_mySqlConnection, true);
            }

            if (_mySqlConnection.State == ConnectionState.Open)
            {
                return Clone(_mySqlConnection, true);
            }

            return Clone((MySqlConnection) _mySqlConnection.Clone(), false);
        }

        /// <summary>
        /// Danner ny instans af data provideren til MySql.
        /// </summary>
        /// <param name="mySqlConnection">Connection, som skal bruges i den nye data provider.</param>
        /// <param name="clonedWithReusableConnection">Angiver, om data provideren skal klones med samme connection.</param>
        /// <returns>Ny instans af data provideren til MySql.</returns>
        protected virtual object Clone(MySqlConnection mySqlConnection, bool clonedWithReusableConnection)
        {
            ArgumentNullGuard.NotNull(mySqlConnection, nameof(mySqlConnection));

            return new MySqlDataProvider(mySqlConnection, clonedWithReusableConnection);
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
                    command.Connection = _mySqlConnection;
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
                    command.Connection = _mySqlConnection;

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
                if (Transaction.Current != null)
                {
                    _mySqlConnection.EnlistTransaction(Transaction.Current);
                }
                using (MySqlCommand command = dataProxy.CreateInsertCommand())
                {
                    command.Connection = _mySqlConnection;
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
                if (Transaction.Current != null)
                {
                    _mySqlConnection.EnlistTransaction(Transaction.Current);
                }
                using (MySqlCommand command = dataProxy.CreateUpdateCommand())
                {
                    command.Connection = _mySqlConnection;
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
                if (Transaction.Current != null)
                {
                    _mySqlConnection.EnlistTransaction(Transaction.Current);
                }
                dataProxy.DeleteRelations(this);
                using (MySqlCommand command = dataProxy.CreateDeleteCommand())
                {
                    command.Connection = _mySqlConnection;
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
            if (_clonedWithReusableConnection)
            {
                if (_mySqlConnection.State == ConnectionState.Open)
                {
                    return;
                }

                _mySqlConnection.Open();
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
            if (_clonedWithReusableConnection)
            {
                return;
            }
            _mySqlConnection.Close();
        }

        #endregion
    }
}
