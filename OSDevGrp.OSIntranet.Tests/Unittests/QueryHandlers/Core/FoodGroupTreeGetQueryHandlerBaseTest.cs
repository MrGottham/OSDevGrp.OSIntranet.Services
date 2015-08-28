using NUnit.Framework;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.QueryHandlers.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers.Core
{
    /// <summary>
    /// Tests the functionality which handles a query for getting the tree of food groups.
    /// </summary>
    [TestFixture]
    public class FoodGroupTreeGetQueryHandlerBaseTest
    {
        /// <summary>
        /// Private class for testing the functionality which handles a query for getting the tree of food groups.
        /// </summary>
        private class MyFoodGroupTreeGetQueryHandler : FoodGroupTreeGetQueryHandlerBase<IView>
        {
            #region Constructor

            /// <summary>
            /// Creates a private class for testing the functionality which handles a query for getting the tree of food groups.
            /// </summary>
            /// <param name="systemDataRepository">Implementation for a repository which can access system data in the food waste domain.</param>
            /// <param name="foodWasteObjectMapper">Implementation for a object mapper which can map domain object in the food waste domain.</param>
            public MyFoodGroupTreeGetQueryHandler(ISystemDataRepository systemDataRepository, IFoodWasteObjectMapper foodWasteObjectMapper) 
                : base(systemDataRepository, foodWasteObjectMapper)
            {
            }

            #endregion

            #region Properties

            /// <summary>
            /// Gets whether only active food groups should be included.
            /// </summary>
            protected override bool OnlyActive
            {
                get { return false; }
            }

            #endregion
        }


    }
}
