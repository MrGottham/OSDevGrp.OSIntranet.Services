using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.QueryHandlers.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers.Core
{
    /// <summary>
    /// Tests the functionality which handles a query for getting a collection of storage types.
    /// </summary>
    [TestFixture]
    public class StorageTypeCollectionGetQueryHandlerBaseTests
    {
        /// <summary>
        /// Private class for testing the functionality which handles a query for getting a collection of storage types.
        /// </summary>
        private class MyStorageTypeCollectionGetQueryHandler : StorageTypeCollectionGetQueryHandlerBase<StorageTypeCollectionGetQuery, StorageTypeIdentificationView>
        {
            #region Constructor

            /// <summary>
            /// Creates an instance of the private class for testing the functionality which handles a query for getting a collection of storage types.
            /// </summary>
            /// <param name="systemDataRepository">Implementation of the repository which can access system data for the food waste domain.</param>
            /// <param name="objectMapper">Implementation of the object mapper which can map objects in the food waste domain.</param>
            public MyStorageTypeCollectionGetQueryHandler(ISystemDataRepository systemDataRepository, IFoodWasteObjectMapper objectMapper)
                : base(systemDataRepository, objectMapper)
            {
            }
            
            #endregion
        }

        private MyStorageTypeCollectionGetQueryHandler CreateSut()
        {
            return null;
        }
    }
}
