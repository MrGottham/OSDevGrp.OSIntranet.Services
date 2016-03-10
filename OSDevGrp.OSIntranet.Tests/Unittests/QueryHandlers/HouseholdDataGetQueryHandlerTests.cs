using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.QueryHandlers;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers
{
    /// <summary>
    /// Test the query handler which handles the query for getting household data for one of the current user households.
    /// </summary>
    [TestFixture]
    public class HouseholdDataGetQueryHandlerTests
    {
        /// <summary>
        /// Tests that the constructor initialize the query handler which handles the query for getting household data for one of the current user households.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeHouseholdDataGetQueryHandler()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var householdDataGetQueryHandler = new HouseholdDataGetQueryHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock);
            Assert.That(householdDataGetQueryHandler, Is.Not.Null);
            Assert.That(householdDataGetQueryHandler.ShouldBeActivated, Is.True);
            Assert.That(householdDataGetQueryHandler.ShouldHaveAcceptedPrivacyPolicy, Is.True);
            Assert.That(householdDataGetQueryHandler.RequiredMembership, Is.EqualTo(Membership.Basic));
        }

        /// <summary>
        /// Tests that GetData throws ArgumentNullException when the household member is null.
        /// </summary>
        [Test]
        public void TestThatGetDataThrowsArgumentNullExceptionWhenHouseholdMemberIsNull()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var householdDataGetQueryHandler = new HouseholdDataGetQueryHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock);
            Assert.That(householdDataGetQueryHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdDataGetQueryHandler.GetData(null, fixture.Create<HouseholdDataGetQuery>(), DomainObjectMockBuilder.BuildTranslationInfoMock()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("householdMember"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetData throws ArgumentNullException when the query for for getting household data for one of the current user households is null.
        /// </summary>
        [Test]
        public void TestThatGetDataThrowsArgumentNullExceptionWhenQueryIsNull()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var householdDataGetQueryHandler = new HouseholdDataGetQueryHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock);
            Assert.That(householdDataGetQueryHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdDataGetQueryHandler.GetData(DomainObjectMockBuilder.BuildHouseholdMemberMock(), null, DomainObjectMockBuilder.BuildTranslationInfoMock()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("query"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetData throws ArgumentNullException when the translation informations which can be used for translation is null.
        /// </summary>
        [Test]
        public void TestThatGetDataThrowsArgumentNullExceptionWhenTranslationInfoIsNull()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var householdDataGetQueryHandler = new HouseholdDataGetQueryHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock);
            Assert.That(householdDataGetQueryHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdDataGetQueryHandler.GetData(DomainObjectMockBuilder.BuildHouseholdMemberMock(), fixture.Create<HouseholdDataGetQuery>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("translationInfo"));
            Assert.That(exception.InnerException, Is.Null);
        }
    }
}
