using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommandHandlers;
using OSDevGrp.OSIntranet.CommandHandlers.Validation;
using OSDevGrp.OSIntranet.Contracts.Commands;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste;
using AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.CommandHandlers
{
    /// <summary>
    /// Test the command handler which can handles the command for accepting privacy policy on the current users household member account.
    /// </summary>
    [TestFixture]
    public class HouseholdMemberAcceptPrivacyPolicyCommandHandlerTests
    {
        /// <summary>
        /// Tests that the constructor initialize a command handler which can handles the command for accepting privacy policy on the current users household member account.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeHouseholdMemberAcceptPrivacyPolicyCommandHandler()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdMemberAcceptPrivacyPolicyCommandHandler = new HouseholdMemberAcceptPrivacyPolicyCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdMemberAcceptPrivacyPolicyCommandHandler, Is.Not.Null);
            Assert.That(householdMemberAcceptPrivacyPolicyCommandHandler.ShouldBeActivated, Is.False);
            Assert.That(householdMemberAcceptPrivacyPolicyCommandHandler.ShouldHaveAcceptedPrivacyPolicy, Is.False);
            Assert.That(householdMemberAcceptPrivacyPolicyCommandHandler.RequiredMembership, Is.EqualTo(Membership.Basic));

        }

        /// <summary>
        /// Tests that AddValidationRules throws an ArgumentNullException when the household member which should accecpt the privacy policy is null.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesThrowsArgumentNullExceptionWhenHouseholdMemberIsNull()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdMemberAcceptPrivacyPolicyCommandHandler = new HouseholdMemberAcceptPrivacyPolicyCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdMemberAcceptPrivacyPolicyCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdMemberAcceptPrivacyPolicyCommandHandler.AddValidationRules(null, fixture.Create<HouseholdMemberAcceptPrivacyPolicyCommand>(), specificationMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("householdMember"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that AddValidationRules throws an ArgumentNullException when the command for accepting privacy policy on the current users household member account is null.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesThrowsArgumentNullExceptionWhenCommandIsNull()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdMemberAcceptPrivacyPolicyCommandHandler = new HouseholdMemberAcceptPrivacyPolicyCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdMemberAcceptPrivacyPolicyCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdMemberAcceptPrivacyPolicyCommandHandler.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMemberMock(), null, specificationMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("command"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that AddValidationRules throws an ArgumentNullException when the specification which encapsulates validation rules is null.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesThrowsArgumentNullExceptionWhenSpecificationIsNull()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdMemberAcceptPrivacyPolicyCommandHandler = new HouseholdMemberAcceptPrivacyPolicyCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdMemberAcceptPrivacyPolicyCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdMemberAcceptPrivacyPolicyCommandHandler.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMemberMock(), fixture.Create<HouseholdMemberAcceptPrivacyPolicyCommand>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("specification"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that ModifyData throws an ArgumentNullException when the household member which should accecpt the privacy policy is null.
        /// </summary>
        [Test]
        public void TestThatModifyDataThrowsArgumentNullExceptionWhenHouseholdMemberIsNull()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdMemberAcceptPrivacyPolicyCommandHandler = new HouseholdMemberAcceptPrivacyPolicyCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdMemberAcceptPrivacyPolicyCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdMemberAcceptPrivacyPolicyCommandHandler.ModifyData(null, fixture.Create<HouseholdMemberAcceptPrivacyPolicyCommand>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("householdMember"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that ModifyData sets PrivacyPolicyAcceptedTime to the current date and time.
        /// </summary>
        [Test]
        public void TestThatModifyDataSetsPrivacyPolicyAcceptedTimeToDateTimeNow()
        {
            var fixture = new Fixture();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHouseholdMember>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();

            var householdMemberAcceptPrivacyPolicyCommandHandler = new HouseholdMemberAcceptPrivacyPolicyCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdMemberAcceptPrivacyPolicyCommandHandler, Is.Not.Null);

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();

            var testTime = DateTime.Now;

            householdMemberAcceptPrivacyPolicyCommandHandler.ModifyData(householdMemberMock, fixture.Create<HouseholdMemberAcceptPrivacyPolicyCommand>());

            householdMemberMock.AssertWasCalled(m => m.PrivacyPolicyAcceptedTime = Arg<DateTime>.Is.GreaterThanOrEqual(testTime));
            householdMemberMock.AssertWasCalled(m => m.PrivacyPolicyAcceptedTime = Arg<DateTime>.Is.LessThanOrEqual(testTime.AddSeconds(1)));
        }

        /// <summary>
        /// Tests that ModifyData calls Update with the household member who accepted privacy policy on the repository which can access household data for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsUpdateWithHouseholdMemberOnHouseholdDataRepository()
        {
            var fixture = new Fixture();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHouseholdMember>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();

            var householdMemberAcceptPrivacyPolicyCommandHandler = new HouseholdMemberAcceptPrivacyPolicyCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdMemberAcceptPrivacyPolicyCommandHandler, Is.Not.Null);

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();

            householdMemberAcceptPrivacyPolicyCommandHandler.ModifyData(householdMemberMock, fixture.Create<HouseholdMemberAcceptPrivacyPolicyCommand>());

            householdDataRepositoryMock.AssertWasCalled(m => m.Update(Arg<IHouseholdMember>.Is.Equal(householdMemberMock)));
        }

        /// <summary>
        /// Tests that ModifyData returns the household member who accepted privacy policy from the repository which can access household data for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatModifyDataReturnsActivatedHouseholdMemberFromHouseholdDataRepository()
        {
            var fixture = new Fixture();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var activatedHouseholdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHouseholdMember>.Is.Anything))
                .Return(activatedHouseholdMemberMock)
                .Repeat.Any();

            var householdMemberAcceptPrivacyPolicyCommandHandler = new HouseholdMemberAcceptPrivacyPolicyCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdMemberAcceptPrivacyPolicyCommandHandler, Is.Not.Null);

            var result = householdMemberAcceptPrivacyPolicyCommandHandler.ModifyData(DomainObjectMockBuilder.BuildHouseholdMemberMock(), fixture.Create<HouseholdMemberAcceptPrivacyPolicyCommand>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(activatedHouseholdMemberMock));
        }
    }
}
