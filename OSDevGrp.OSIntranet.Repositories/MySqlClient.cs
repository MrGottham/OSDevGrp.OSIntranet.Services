using MySql.Data.MySqlClient;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories
{
    /// <summary>
    /// Implementering af en MySql klient.
    /// </summary>
    public class MySqlClient : IMySqlClient
    {
        #region Private variables

        private readonly MySqlConnection _mySqlConnection = new MySqlConnection();

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
    }
}
