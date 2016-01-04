using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.QueryHandlers.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers.Core
{
    /// <summary>
    /// Tests the basic functionality which can handle a query for getting some data for a household member.
    /// </summary>
    [TestFixture]
    public class HouseholdMemberDataGetQueryHandlerBaseTests
    {
        /// <summary>
        /// Private class for a query which can get some data for a household member.
        /// </summary>
        private class MyHouseholdMemberDataGetQuery : HouseholdMemberDataGetQueryBase
        {
        }

        /// <summary>
        /// Private class for a query which can get some translatable data for a household member.
        /// </summary>
        private class MyHouseholdMemberTranslatableDataGetQuery : HouseholdMemberTranslatableDataGetQueryBase
        {
        }

        /// <summary>
        /// Private class for testing the basic functionality which can handle a query for getting some data for a household member.
        /// </summary>
        /// <typeparam name="TQuery">Type of the query for getting some data for a household member.</typeparam>
        private class MyHouseholdMemberDataGetQueryHandler<TQuery> : HouseholdMemberDataGetQueryHandlerBase<TQuery, object, IView> where TQuery : HouseholdMemberDataGetQueryBase
        {
            #region Constructor

            /// <summary>
            /// Creates an instance of the private class for testing the basic functionality which can handle a query for getting some data for a household member.
            /// </summary>
            /// <param name="householdDataRepository">Implementation of a repository which can access household data for the food waste domain.</param>
            /// <param name="claimValueProvider">Implementation of a provider which can resolve values from the current users claims.</param>
            /// <param name="foodWasteObjectMapper">Implementation of an object mapper which can map objects in the food waste domain.</param>
            public MyHouseholdMemberDataGetQueryHandler(IHouseholdDataRepository householdDataRepository, IClaimValueProvider claimValueProvider, IFoodWasteObjectMapper foodWasteObjectMapper)
                : base(householdDataRepository, claimValueProvider, foodWasteObjectMapper)
            {
            }

            #endregion

            #region Methods

            /// <summary>
            /// Gets the repository which can access household data for the food waste domain.
            /// </summary>
            /// <returns>Repository which can access household data for the food waste domain</returns>
            public IHouseholdDataRepository GetHouseholdDataRepository()
            {
                return base.HouseholdDataRepository;
            }

            /// <summary>
            /// Gets the provider which can resolve values from the current users claims.
            /// </summary>
            /// <returns>Provider which can resolve values from the current users claims.</returns>
            public IClaimValueProvider GetClaimValueProvider()
            {
                return base.ClaimValueProvider;
            }

            /// <summary>
            /// Gets the object mapper which can map objects in the food waste domain.
            /// </summary>
            /// <returns>Object mapper which can map objects in the food waste domain.</returns>
            public IFoodWasteObjectMapper GetObjectMapper()
            {
                return base.ObjectMapper;
            }

            #endregion
        }

        /// <summary>
        /// Tests that the constructor initialize the basic functionality which can handle a query for getting some data for a household member.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeHouseholdMemberDataGetQueryHandlerBase()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var householdMemberDataGetQueryHandlerBase = new MyHouseholdMemberDataGetQueryHandler<MyHouseholdMemberDataGetQuery>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock);
            Assert.That(householdMemberDataGetQueryHandlerBase, Is.Not.Null);
            Assert.That(householdMemberDataGetQueryHandlerBase.GetHouseholdDataRepository(), Is.Not.Null);
            Assert.That(householdMemberDataGetQueryHandlerBase.GetHouseholdDataRepository(), Is.EqualTo(householdDataRepositoryMock));
            Assert.That(householdMemberDataGetQueryHandlerBase.GetClaimValueProvider(), Is.Not.Null);
            Assert.That(householdMemberDataGetQueryHandlerBase.GetClaimValueProvider(), Is.EqualTo(claimValueProviderMock));
            Assert.That(householdMemberDataGetQueryHandlerBase.GetObjectMapper(), Is.Not.Null);
            Assert.That(householdMemberDataGetQueryHandlerBase.GetObjectMapper(), Is.EqualTo(objectMapperMock));
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the repository which can access household data for the food waste domain is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenHouseholdDataRepositoryIsNull()
        {
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyHouseholdMemberDataGetQueryHandler<MyHouseholdMemberDataGetQuery>(null, claimValueProviderMock, objectMapperMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("householdDataRepository"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the provider which can resolve values from the current users claims is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenClaimValueProviderIsNull()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyHouseholdMemberDataGetQueryHandler<MyHouseholdMemberDataGetQuery>(householdDataRepositoryMock, null, objectMapperMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("claimValueProvider"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the object mapper which can map objects in the food waste domain is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenHouseholdFoodWasteObjectMapperIsNull()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyHouseholdMemberDataGetQueryHandler<MyHouseholdMemberDataGetQuery>(householdDataRepositoryMock, claimValueProviderMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("foodWasteObjectMapper"));
            Assert.That(exception.InnerException, Is.Null);
        }
    }
}
