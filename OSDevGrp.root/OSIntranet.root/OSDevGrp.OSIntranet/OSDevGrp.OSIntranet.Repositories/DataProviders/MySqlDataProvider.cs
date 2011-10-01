using System;
using System.Collections.Generic;
using System.Configuration;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies;
using OSDevGrp.OSIntranet.Resources;
using MySql.Data.MySqlClient;

namespace OSDevGrp.OSIntranet.Repositories.DataProviders
{
    /// <summary>
    /// Data provider, som benytter MySql.
    /// </summary>
    public class MySqlDataProvider : DataProviderBase, IMySqlDataProvider
    {
        #region Private variables

        private readonly MySqlConnection _mySqlConnection = new MySqlConnection(ConfigurationManager.ConnectionStrings["OSDevGrp.OSIntranet.Repositories.DataProviders.MySqlDataProvider"].ConnectionString);

        #endregion

        #region Methods

        /// <summary>
        /// Henter og returnerer data fra MySql.
        /// </summary>
        /// <typeparam name="TDataProxy">Typen på data proxy med data fra MySql.</typeparam>
        /// <param name="query">SQL foresprøgelse efter data.</param>
        /// <returns>Collection indeholdende data proxies.</returns>
        public override IEnumerable<TDataProxy> GetCollection<TDataProxy>(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                throw new ArgumentNullException("query");
            }
            _mySqlConnection.Open();
            try
            {
                var collection = new List<TDataProxy>();
                using (var command = _mySqlConnection.CreateCommand())
                {
                    command.CommandText = query;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var dataProxy = new TDataProxy();
                            dataProxy.MapData(reader, this);
                            collection.Add(dataProxy);
                        }
                        reader.Dispose();
                    }
                    command.Dispose();
                }
                return collection;
            }
            finally
            {
                _mySqlConnection.Close();
            }
        }

        /// <summary>
        /// Henter og returnerer en given data proxy fra MySql.
        /// </summary>
        /// <typeparam name="TDataProxy">Typen på data proxy med data fra MySql.</typeparam>
        /// <typeparam name="TId">Typen på den unikke identifikation for data proxy på MySql.</typeparam>
        /// <param name="id">Unik identifikation af data proxy, som skal fremsøges fra MySql.</param>
        /// <returns>Data proxy med data fra MySql.</returns>
        public override TDataProxy Get<TDataProxy, TId>(TId id)
        {
            if (Equals(id, null))
            {
                throw new ArgumentNullException("id");
            }
            _mySqlConnection.Open();
            try
            {
                var queryDataProxy = (IMySqlDataProxy<TId>) new TDataProxy();
                var sqlQuery = queryDataProxy.GetSqlQueryForId(id);
                
                using (var command = _mySqlConnection.CreateCommand())
                {
                    command.CommandText = sqlQuery;
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            reader.Dispose();
                            throw new IntranetRepositoryException(
                                Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById,
                                                             queryDataProxy.GetType().Name, id));
                        }
                        var dataProxy = new TDataProxy();
                        if (reader.Read())
                        {
                            dataProxy.MapData(reader, this);
                        }
                        reader.Dispose();
                        return dataProxy;
                    }
                }
            }
            finally
            {
                _mySqlConnection.Close();
            }
        }

        /// <summary>
        /// Tilføjer data i en data proxy til MySql.
        /// </summary>
        /// <typeparam name="TDataProxy">Typen på data proxy med data til MySql.</typeparam>
        /// <param name="dataProxy">Data proxy med data, som skal tilføjes i MySql.</param>
        /// <returns>Data proxy med tilføjede data.</returns>
        public override TDataProxy Add<TDataProxy>(TDataProxy dataProxy)
        {
            if (dataProxy == null)
            {
                throw new ArgumentNullException("dataProxy");
            }
            _mySqlConnection.Open();
            try
            {
                throw new NotImplementedException();
            }
            finally
            {
                _mySqlConnection.Close();
            }
        }

        /// <summary>
        /// Gemmer data fra en data proxy i MySql.
        /// </summary>
        /// <typeparam name="TDataProxy">Typen på data proxy med data til MySql.</typeparam>
        /// <param name="dataProxy">Data proxy med data, som skal gemmes i MySql.</param>
        /// <returns>Data proxy med gemte data.</returns>
        public override TDataProxy Save<TDataProxy>(TDataProxy dataProxy)
        {
            if (dataProxy == null)
            {
                throw new ArgumentNullException("dataProxy");
            }
            _mySqlConnection.Open();
            try
            {
                throw new NotImplementedException();
            }
            finally
            {
                _mySqlConnection.Close();
            }
        }

        /// <summary>
        /// Sletter data i en data proxy fra MySql.
        /// </summary>
        /// <typeparam name="TDataProxy">Typen på data proxy med data til MySql.</typeparam>
        /// <param name="dataProxy">Data proxy med data, som skal slette fra MySql.</param>
        public override void Delete<TDataProxy>(TDataProxy dataProxy)
        {
            if (dataProxy == null)
            {
                throw new ArgumentNullException("dataProxy");
            }
            _mySqlConnection.Open();
            try
            {
                throw new NotImplementedException();
            }
            finally
            {
                _mySqlConnection.Close();
            }
        }

        #endregion
    }
}
