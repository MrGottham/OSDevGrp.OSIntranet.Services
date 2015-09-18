﻿using NUnit.Framework;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.QueryHandlers;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers
{
    /// <summary>
    /// Tests the query handler which handles a query for getting the tree of food groups for household usage.
    /// </summary>
    [TestFixture]
    public class FoodGroupTreeGetQueryHandlerTests
    {
        /// <summary>
        /// Private class for testing the query handler which handles a query for getting the tree of food groups for household usage.
        /// </summary>
        private class MyFoodGroupTreeGetQueryHandler : FoodGroupTreeGetQueryHandler
        {
            #region Constructor

            /// <summary>
            /// Creates an instance of the private class for testing the query handler which handles a query for getting the tree of food groups for household usage.
            /// </summary>
            /// <param name="systemDataRepository">Implementation for a repository which can access system data in the food waste domain.</param>
            /// <param name="foodWasteObjectMapper">Implementation for a object mapper which can map domain object in the food waste domain.</param>
            public MyFoodGroupTreeGetQueryHandler(ISystemDataRepository systemDataRepository, IFoodWasteObjectMapper foodWasteObjectMapper) : base(systemDataRepository, foodWasteObjectMapper)
            {
            }

            #endregion

            #region Properties

            /// <summary>
            /// Gets whether only active food groups should be included.
            /// </summary>
            public new bool OnlyActive
            {
                get { return base.OnlyActive; }
            }

            #endregion
        }

        /// <summary>
        /// Tests that the constructor initialize a query handler which handles a query for getting the tree of food groups for household usage.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeFoodGroupTreeGetQueryHandler()
        {
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var foodGroupTreeGetQueryHandlerForSystemView = new MyFoodGroupTreeGetQueryHandler(systemDataRepositoryMock, foodWasteObjectMapperMock);
            Assert.That(foodGroupTreeGetQueryHandlerForSystemView, Is.Not.Null);
            Assert.That(foodGroupTreeGetQueryHandlerForSystemView.OnlyActive, Is.EqualTo(true));
        }
    }
}
