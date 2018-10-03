using System.Configuration;
using System.Transactions;
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
        /// <param name="clonedWithinTransaction">True when the MySQL connection has been cloned within a transaction; otherwise false.</param>
        private FoodWasteDataProvider(MySqlConnection mySqlConnection, bool clonedWithinTransaction)
            : base(mySqlConnection, clonedWithinTransaction)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Clone the data provider which can access data in the food waste repository.
        /// </summary>
        /// <returns>Cloned data provider which can access data in the food waste repository.</returns>
        public override object Clone()
        {
            return Transaction.Current == null ? new FoodWasteDataProvider((MySqlConnection) MySqlConnection.Clone(), false) : new FoodWasteDataProvider(MySqlConnection, true);
        }

        #endregion
    }
}
