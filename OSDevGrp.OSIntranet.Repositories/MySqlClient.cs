using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Resources;
using MySql.Data.MySqlClient;

namespace OSDevGrp.OSIntranet.Repositories
{
    /// <summary>
    /// Implementering af en MySql klient.
    /// </summary>
    public class MySqlClient : IMySqlClient
    {
        #region Private variables

        private readonly MySqlConnection _mySqlConnection = new MySqlConnection(ConfigurationManager.ConnectionStrings["OSDevGrp.OSIntranet.Repositories.MySqlClient"].ConnectionString);

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Disposing the MySql client.
        /// </summary>
        public void Dispose()
        {
            _mySqlConnection.Dispose();
        }

        #endregion

        #region IMySqlClient Members

        /// <summary>
        /// Henter fra MySql en collection af en given type.
        /// </summary>
        /// <typeparam name="T">Typen, som skal hentes fra MySql.</typeparam>
        /// <param name="query">Sql-query til forespørgelse efter objekter af den givne type.</param>
        /// <param name="builder">Callbackmetode til bygning af objekt.</param>
        /// <returns>Collection af en given type.</returns>
        public IEnumerable<T> GetCollection<T>(string query, Func<IMySqlDataRecord, T> builder)
        {
            if (string.IsNullOrEmpty(query))
            {
                throw new ArgumentNullException("query");
            }
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }
            try
            {
                _mySqlConnection.Open();
                try
                {
                    var collection = new List<T>();
                    using (var command = _mySqlConnection.CreateCommand())
                    {
                        command.CommandText = query;
                        using (var reader = command.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                collection.Add(builder(new MySqlDataRecord(reader)));
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
            catch (MySqlException ex)
            {
                throw new IntranetRepositoryException(
                    Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, MethodBase.GetCurrentMethod().Name,
                                                 ex.Message), ex);
            }
        }

        #endregion
    }
}
