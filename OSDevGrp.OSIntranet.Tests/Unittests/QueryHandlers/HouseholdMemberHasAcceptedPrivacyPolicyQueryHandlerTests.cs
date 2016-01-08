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
    /// Test the query handler which handles the query for checking whether the current user has accepted the privacy policy.
    /// </summary>
    [TestFixture]
    public class HouseholdMemberHasAcceptedPrivacyPolicyQueryHandlerTests
    {
        /// <summary>
        /// Tests that the constructor initialize a query handler which handles the query for checking whether the current user has accepted the privacy policy.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeHouseholdMemberHasAcceptedPrivacyPolicyQueryHandler()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var householdMemberHasAcceptedPrivacyPolicyQueryHandler = new HouseholdMemberHasAcceptedPrivacyPolicyQueryHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock);
            Assert.That(householdMemberHasAcceptedPrivacyPolicyQueryHandler, Is.Not.Null);
            Assert.That(householdMemberHasAcceptedPrivacyPolicyQueryHandler.ShouldBeActivated, Is.False);
            Assert.That(householdMemberHasAcceptedPrivacyPolicyQueryHandler.ShouldHaveAcceptedPrivacyPolicy, Is.False);
            Assert.That(householdMemberHasAcceptedPrivacyPolicyQueryHandler.RequiredMembership, Is.EqualTo(Membership.Basic));
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

            var householdMemberHasAcceptedPrivacyPolicyQueryHandler = new HouseholdMemberHasAcceptedPrivacyPolicyQueryHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock);
            Assert.That(householdMemberHasAcceptedPrivacyPolicyQueryHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdMemberHasAcceptedPrivacyPolicyQueryHandler.GetData(null, fixture.Create<HouseholdMemberHasAcceptedPrivacyPolicyQuery>(), DomainObjectMockBuilder.BuildTranslationInfoMock()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("householdMember"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetData calls IsPrivacyPolictyAccepted on the household member.
        /// </summary>
        [Test]
        public void TestThatGetDataCallsIsPrivacyPolictyAcceptedOnHouseholdMember()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();

            var householdMemberHasAcceptedPrivacyPolicyQueryHandler = new HouseholdMemberHasAcceptedPrivacyPolicyQueryHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock);
            Assert.That(householdMemberHasAcceptedPrivacyPolicyQueryHandler, Is.Not.Null);

            householdMemberHasAcceptedPrivacyPolicyQueryHandler.GetData(householdMemberMock, fixture.Create<HouseholdMemberHasAcceptedPrivacyPolicyQuery>(), DomainObjectMockBuilder.BuildTranslationInfoMock());

            householdMemberMock.AssertWasCalled(m => m.IsPrivacyPolictyAccepted);
        }

        /// <summary>
        /// Tests that GetData returns the value from IsPrivacyPolictyAccepted on the household member.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatGetDataReturnsValueFromIsPrivacyPolictyAcceptedOnHouseholdMember(bool isPrivacyPolictyAccepted)
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock(isPrivacyPolictyAccepted: isPrivacyPolictyAccepted);

            var householdMemberHasAcceptedPrivacyPolicyQueryHandler = new HouseholdMemberHasAcceptedPrivacyPolicyQueryHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock);
            Assert.That(householdMemberHasAcceptedPrivacyPolicyQueryHandler, Is.Not.Null);

            var result = householdMemberHasAcceptedPrivacyPolicyQueryHandler.GetData(householdMemberMock, fixture.Create<HouseholdMemberHasAcceptedPrivacyPolicyQuery>(), DomainObjectMockBuilder.BuildTranslationInfoMock());

            Assert.That(result, Is.EqualTo(isPrivacyPolictyAccepted));
        }
    }
}
