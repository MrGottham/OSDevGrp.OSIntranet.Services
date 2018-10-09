using System.Configuration;
using MySql.Data.MySqlClient;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;

namespace OSDevGrp.OSIntranet.Repositories.DataProviders
{
    /// <summary>
    /// Data provider which can access data in the food waste repository.
    /// </summary>
    public class FoodWasteDataProvider : MySqlDataProvider, IFoodWasteDataProvider
    {
        #region Constructor

        /// <summary>
        /// Creates a data provider which can access data in the food waste repository.
        /// </summary>
        /// <param name="connectionStringSettings">Settings for the connection string which can open the connection to the food waste repository.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="connectionStringSettings"/> is null.</exception>
        public FoodWasteDataProvider(ConnectionStringSettings connectionStringSettings) 
            : base(connectionStringSettings)
        {
        }

        /// <summary>
        /// Creates a data provider which can access data in the food waste repository.
        /// </summary>
        /// <param name="mySqlConnection">The MySQL connection to use within the data provider.</param>
        /// <param name="clonedWithReusableConnection">True when the MySQL connection has been cloned with a reusable connection; otherwise false.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="mySqlConnection"/> is null.</exception>
        private FoodWasteDataProvider(MySqlConnection mySqlConnection, bool clonedWithReusableConnection)
            : base(mySqlConnection, clonedWithReusableConnection)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Clone the data provider which can access data in the food waste repository.
        /// </summary>
        /// <param name="mySqlConnection">The connection which should be used in the clone.</param>
        /// <param name="clonedWithReusableConnection">True when the MySQL connection has been cloned with a reusable connection; otherwise false.</param>
        /// <returns>Cloned data provider which can access data in the food waste repository.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="mySqlConnection"/> is null.</exception>
        protected override object Clone(MySqlConnection mySqlConnection, bool clonedWithReusableConnection)
        {
            return new FoodWasteDataProvider(mySqlConnection, clonedWithReusableConnection);
        }

        #endregion
    }
}
