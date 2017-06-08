using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.QueryHandlers.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.QueryHandlers
{
    /// <summary>
    /// Query handler which handle a query for getting a collection of storage types for system usage.
    /// </summary>
    public class StorageTypeCollectionGetQueryHandlerForSystemView : StorageTypeCollectionGetQueryHandlerBase<StorageTypeCollectionGetQuery, StorageTypeSystemView>
    {
        #region Constructor

        /// <summary>
        /// Creates a query handler which handle a query for getting a collection of storage types for system usage.
        /// </summary>
        /// <param name="systemDataRepository">Implementation of the repository which can access system data for the food waste domain.</param>
        /// <param name="objectMapper">Implementation of the object mapper which can map objects in the food waste domain.</param>
        public StorageTypeCollectionGetQueryHandlerForSystemView(ISystemDataRepository systemDataRepository, IFoodWasteObjectMapper objectMapper)
            : base(systemDataRepository, objectMapper)
        {
        }

        #endregion
    }
}
