using System.Configuration;
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
        public FoodWasteDataProvider(ConnectionStringSettings connectionStringSettings) 
            : base(connectionStringSettings)
        {
        }

        #endregion
    }
}
