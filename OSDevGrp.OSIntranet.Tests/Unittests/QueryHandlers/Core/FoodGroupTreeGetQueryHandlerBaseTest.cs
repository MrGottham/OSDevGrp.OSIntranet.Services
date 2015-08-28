using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.QueryHandlers.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;
using Ploeh.AutoFixture;
using Rhino.Mocks;

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
            #region Private variables

            private readonly bool _onlyActive;
            
            #endregion

            #region Constructor

            /// <summary>
            /// Creates a private class for testing the functionality which handles a query for getting the tree of food groups.
            /// </summary>
            /// <param name="systemDataRepository">Implementation for a repository which can access system data in the food waste domain.</param>
            /// <param name="foodWasteObjectMapper">Implementation for a object mapper which can map domain object in the food waste domain.</param>
            /// <param name="onlyActive">Indication of whether only active food groups should be includes.</param>
            public MyFoodGroupTreeGetQueryHandler(ISystemDataRepository systemDataRepository, IFoodWasteObjectMapper foodWasteObjectMapper, bool onlyActive) 
                : base(systemDataRepository, foodWasteObjectMapper)
            {
                _onlyActive = onlyActive;
            }

            #endregion

            #region Properties

            /// <summary>
            /// Gets whether only active food groups should be included.
            /// </summary>
            protected override bool OnlyActive
            {
                get { return _onlyActive; }
            }

            #endregion

            #region Methods

            /// <summary>
            /// Returns whether only active food groups should be included.
            /// </summary>
            /// <returns>Indication of whether only active food groups should be included</returns>
            public bool GetOnlyActive()
            {
                return OnlyActive;
            }

            #endregion
        }

        /// <summary>
        /// Tests that the constructor initialize functionality which handles a query for getting the tree of food groups.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatConstructorInitializeFoodGroupTreeGetQueryHandlerBase(bool activeOnly)
        {
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var foodGroupTreeGetQueryHandlerBase = new MyFoodGroupTreeGetQueryHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, activeOnly);
            Assert.That(foodGroupTreeGetQueryHandlerBase, Is.Not.Null);
            Assert.That(foodGroupTreeGetQueryHandlerBase.GetOnlyActive(), Is.EqualTo(activeOnly));
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the repository which can access system data in the food waste domain is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenSystemDataRepositoryIsNull()
        {
            var fixture = new Fixture();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyFoodGroupTreeGetQueryHandler(null, foodWasteObjectMapperMock, fixture.Create<bool>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("systemDataRepository"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the object mapper which can map domain object in the food waste domain is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenFoodWasteObjectMapperIsNull()
        {
            var fixture = new Fixture();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyFoodGroupTreeGetQueryHandler(systemDataRepositoryMock, null, fixture.Create<bool>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("foodWasteObjectMapper"));
            Assert.That(exception.InnerException, Is.Null);
        }
    }
}
