using NUnit.Framework;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.QueryHandlers;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers
{
    /// <summary>
    /// Tests the query handler which handles a query for getting the collection of food items for system usage.
    /// </summary>
    [TestFixture]
    public class FoodItemCollectionGetQueryHandlerForSystemViewTests
    {
        /// <summary>
        /// Private class for testing the query handler which handles a query for getting the collection of food items for system usage.
        /// </summary>
        private class MyFoodItemCollectionGetQueryHandlerForSystemView : FoodItemCollectionGetQueryHandlerForSystemView
        {
            #region Constructor

            /// <summary>
            /// Creates a private class for testing the query handler which handles a query for getting the collection of food items for system usage.
            /// </summary>
            /// <param name="systemDataRepository">Implementation for a repository which can access system data in the food waste domain.</param>
            /// <param name="foodWasteObjectMapper">Implementation for a object mapper which can map domain object in the food waste domain.</param>
            public MyFoodItemCollectionGetQueryHandlerForSystemView(ISystemDataRepository systemDataRepository, IFoodWasteObjectMapper foodWasteObjectMapper) 
                : base(systemDataRepository, foodWasteObjectMapper)
            {
            }

            #endregion

            #region Properties

            /// <summary>
            /// Gets whether only active food items should be included.
            /// </summary>
            public new bool OnlyActive
            {
                get { return base.OnlyActive; }
            }

            #endregion
        }

        /// <summary>
        /// Tests that the constructor initialize a query handler which handles a query for getting the collection of food items for system usage.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeFoodItemCollectionGetQueryHandlerForSystemView()
        {
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var foodItemCollectionGetQueryHandlerForSystemView = new MyFoodItemCollectionGetQueryHandlerForSystemView(systemDataRepositoryMock, foodWasteObjectMapperMock);
            Assert.That(foodItemCollectionGetQueryHandlerForSystemView, Is.Not.Null);
            Assert.That(foodItemCollectionGetQueryHandlerForSystemView.OnlyActive, Is.EqualTo(false));
        }
    }
}
