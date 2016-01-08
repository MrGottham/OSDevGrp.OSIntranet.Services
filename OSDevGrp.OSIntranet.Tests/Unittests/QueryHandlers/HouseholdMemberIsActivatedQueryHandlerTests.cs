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
    /// Tests the query handler which handles the query for checking whether the current user has been activated.
    /// </summary>
    [TestFixture]
    public class HouseholdMemberIsActivatedQueryHandlerTests
    {
        /// <summary>
        /// Tests that the constructor initialize a query handler which handles the query for checking whether the current user has been activated.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeHouseholdMemberIsActivatedQueryHandler()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var householdMemberIsActivatedQueryHandler = new HouseholdMemberIsActivatedQueryHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock);
            Assert.That(householdMemberIsActivatedQueryHandler, Is.Not.Null);
            Assert.That(householdMemberIsActivatedQueryHandler.ShouldBeActivated, Is.False);
            Assert.That(householdMemberIsActivatedQueryHandler.ShouldHaveAcceptedPrivacyPolicy, Is.False);
            Assert.That(householdMemberIsActivatedQueryHandler.RequiredMembership, Is.EqualTo(Membership.Basic));
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

            var householdMemberIsActivatedQueryHandler = new HouseholdMemberIsActivatedQueryHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock);
            Assert.That(householdMemberIsActivatedQueryHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdMemberIsActivatedQueryHandler.GetData(null, fixture.Create<HouseholdMemberIsActivatedQuery>(), DomainObjectMockBuilder.BuildTranslationInfoMock()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("householdMember"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetData calls IsActivated on the household member.
        /// </summary>
        [Test]
        public void TestThatGetDataCallsIsActivatedOnHouseholdMember()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();

            var householdMemberIsActivatedQueryHandler = new HouseholdMemberIsActivatedQueryHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock);
            Assert.That(householdMemberIsActivatedQueryHandler, Is.Not.Null);

            householdMemberIsActivatedQueryHandler.GetData(householdMemberMock, fixture.Create<HouseholdMemberIsActivatedQuery>(), DomainObjectMockBuilder.BuildTranslationInfoMock());

            householdMemberMock.AssertWasCalled(m => m.IsActivated);
        }

        /// <summary>
        /// Tests that GetData returns the value from IsActivated on the household member.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatGetDataReturnsValueFromIsActivatedOnHouseholdMember(bool isActivated)
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock(isActivated: isActivated);

            var householdMemberIsActivatedQueryHandler = new HouseholdMemberIsActivatedQueryHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock);
            Assert.That(householdMemberIsActivatedQueryHandler, Is.Not.Null);

            var result = householdMemberIsActivatedQueryHandler.GetData(householdMemberMock, fixture.Create<HouseholdMemberIsActivatedQuery>(), DomainObjectMockBuilder.BuildTranslationInfoMock());

            Assert.That(result, Is.EqualTo(isActivated));
        }
    }
}
