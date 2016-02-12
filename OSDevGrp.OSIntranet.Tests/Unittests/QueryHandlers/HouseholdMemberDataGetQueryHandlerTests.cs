using System;
using System.Globalization;
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
    /// Test the query handler which handles the query for getting household member data for the current user.
    /// </summary>
    [TestFixture]
    public class HouseholdMemberDataGetQueryHandlerTests
    {
        /// <summary>
        /// Tests that the constructor initialize the query handler which handles the query for getting household member data for the current user.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeHouseholdMemberDataGetQueryHandler()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var householdMemberDataGetQueryHandler = new HouseholdMemberDataGetQueryHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock);
            Assert.That(householdMemberDataGetQueryHandler, Is.Not.Null);
            Assert.That(householdMemberDataGetQueryHandler.ShouldBeActivated, Is.True);
            Assert.That(householdMemberDataGetQueryHandler.ShouldHaveAcceptedPrivacyPolicy, Is.True);
            Assert.That(householdMemberDataGetQueryHandler.RequiredMembership, Is.EqualTo(Membership.Basic));
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

            var householdMemberDataGetQueryHandler = new HouseholdMemberDataGetQueryHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock);
            Assert.That(householdMemberDataGetQueryHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdMemberDataGetQueryHandler.GetData(null, fixture.Create<HouseholdMemberDataGetQuery>(), DomainObjectMockBuilder.BuildTranslationInfoMock()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("householdMember"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetData throws ArgumentNullException when the query for getting household member data for the current user is null.
        /// </summary>
        [Test]
        public void TestThatGetDataThrowsArgumentNullExceptionWhenQueryIsNull()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var householdMemberDataGetQueryHandler = new HouseholdMemberDataGetQueryHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock);
            Assert.That(householdMemberDataGetQueryHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdMemberDataGetQueryHandler.GetData(DomainObjectMockBuilder.BuildHouseholdMemberMock(), null, DomainObjectMockBuilder.BuildTranslationInfoMock()));
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

            var householdMemberDataGetQueryHandler = new HouseholdMemberDataGetQueryHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock);
            Assert.That(householdMemberDataGetQueryHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdMemberDataGetQueryHandler.GetData(DomainObjectMockBuilder.BuildHouseholdMemberMock(), fixture.Create<HouseholdMemberDataGetQuery>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("translationInfo"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetData Calls X on the household member.
        /// </summary>
        [Test]
        public void TestThatGetDataCallsTranslateOnHouseholdMember()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            var translationInfoMock = DomainObjectMockBuilder.BuildTranslationInfoMock();

            var householdMemberDataGetQueryHandler = new HouseholdMemberDataGetQueryHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock);
            Assert.That(householdMemberDataGetQueryHandler, Is.Not.Null);

            householdMemberDataGetQueryHandler.GetData(householdMemberMock, fixture.Create<HouseholdMemberDataGetQuery>(), translationInfoMock);

            householdMemberMock.AssertWasCalled(m => m.Translate(Arg<CultureInfo>.Is.Equal(translationInfoMock.CultureInfo), Arg<bool>.Is.Equal(false), Arg<bool>.Is.Equal(true)));
        }

        /// <summary>
        /// Tests that GetData returns the household member.
        /// </summary>
        [Test]
        public void TestThatGetDataReturnsHouseholdMember()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();

            var householdMemberDataGetQueryHandler = new HouseholdMemberDataGetQueryHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock);
            Assert.That(householdMemberDataGetQueryHandler, Is.Not.Null);

            var result = householdMemberDataGetQueryHandler.GetData(householdMemberMock, fixture.Create<HouseholdMemberDataGetQuery>(), DomainObjectMockBuilder.BuildTranslationInfoMock());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(householdMemberMock));
        }
    }
}
