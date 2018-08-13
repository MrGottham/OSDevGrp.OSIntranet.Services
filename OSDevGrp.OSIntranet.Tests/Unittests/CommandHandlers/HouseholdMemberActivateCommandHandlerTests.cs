using System;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommandHandlers;
using OSDevGrp.OSIntranet.CommandHandlers.Validation;
using OSDevGrp.OSIntranet.Contracts.Commands;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste;
using AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.CommandHandlers
{
    /// <summary>
    /// Test the command handler which can handles the command for activating the current users household member account.
    /// </summary>
    [TestFixture]
    public class HouseholdMemberActivateCommandHandlerTests
    {
        /// <summary>
        /// Tests that the constructor initialize the command handler which can handles the command for activating the current users household member account.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeHouseholdMemberActivateCommandHandler()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdMemberActivateCommandHandler = new HouseholdMemberActivateCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdMemberActivateCommandHandler, Is.Not.Null);
            Assert.That(householdMemberActivateCommandHandler.ShouldBeActivated, Is.False);
            Assert.That(householdMemberActivateCommandHandler.ShouldHaveAcceptedPrivacyPolicy, Is.False);
            Assert.That(householdMemberActivateCommandHandler.RequiredMembership, Is.EqualTo(Membership.Basic));
        }

        /// <summary>
        /// Tests that AddValidationRules throws an ArgumentNullException when the household member which should be activated is null.
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

            var householdMemberActivateCommandHandler = new HouseholdMemberActivateCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdMemberActivateCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdMemberActivateCommandHandler.AddValidationRules(null, fixture.Create<HouseholdMemberActivateCommand>(), specificationMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("householdMember"));
        }

        /// <summary>
        /// Tests that AddValidationRules throws an ArgumentNullException when the command for activating the current users household member account is null.
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

            var householdMemberActivateCommandHandler = new HouseholdMemberActivateCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdMemberActivateCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdMemberActivateCommandHandler.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMemberMock(), null, specificationMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("command"));
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

            var householdMemberActivateCommandHandler = new HouseholdMemberActivateCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdMemberActivateCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdMemberActivateCommandHandler.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMemberMock(), fixture.Create<HouseholdMemberActivateCommand>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("specification"));
        }

        /// <summary>
        /// Tests that AddValidationRules calls HasValue with the activation code from the command on the common validations.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsHasValueWithActivationCodeOnCommonValidations()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<Exception>.Is.Anything))
                .WhenCalled(e =>
                {
                    var func = (Func<bool>) e.Arguments.ElementAt(0);
                    func.Invoke();
                })
                .Return(specificationMock)
                .Repeat.Any();

            var householdMemberActivateCommandHandler = new HouseholdMemberActivateCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdMemberActivateCommandHandler, Is.Not.Null);

            var activationCode = fixture.Create<string>();
            var householdMemberActivateCommand = fixture.Build<HouseholdMemberActivateCommand>()
                .With(m => m.ActivationCode, activationCode)
                .Create();

            householdMemberActivateCommandHandler.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMemberMock(), householdMemberActivateCommand, specificationMock);

            commonValidationsMock.AssertWasCalled(m => m.HasValue(Arg<string>.Is.Equal(activationCode)));
        }

        /// <summary>
        /// Tests that AddValidationRules calls IsLengthValid with the activation code from the command on the common validations.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsIsLengthValidWithActivationCodeOnCommonValidations()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<Exception>.Is.Anything))
                .WhenCalled(e =>
                {
                    var func = (Func<bool>) e.Arguments.ElementAt(0);
                    func.Invoke();
                })
                .Return(specificationMock)
                .Repeat.Any();

            var householdMemberActivateCommandHandler = new HouseholdMemberActivateCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdMemberActivateCommandHandler, Is.Not.Null);

            var activationCode = fixture.Create<string>();
            var householdMemberActivateCommand = fixture.Build<HouseholdMemberActivateCommand>()
                .With(m => m.ActivationCode, activationCode)
                .Create();

            householdMemberActivateCommandHandler.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMemberMock(), householdMemberActivateCommand, specificationMock);

            commonValidationsMock.AssertWasCalled(m => m.IsLengthValid(Arg<string>.Is.Equal(activationCode), Arg<int>.Is.Equal(1), Arg<int>.Is.Equal(64)));
        }

        /// <summary>
        /// Tests that AddValidationRules calls ContainsIllegalChar with the activation code from the command on the common validations.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsContainsIllegalCharWithActivationCodeOnCommonValidations()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<Exception>.Is.Anything))
                .WhenCalled(e =>
                {
                    var func = (Func<bool>) e.Arguments.ElementAt(0);
                    func.Invoke();
                })
                .Return(specificationMock)
                .Repeat.Any();

            var householdMemberActivateCommandHandler = new HouseholdMemberActivateCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdMemberActivateCommandHandler, Is.Not.Null);

            var activationCode = fixture.Create<string>();
            var householdMemberActivateCommand = fixture.Build<HouseholdMemberActivateCommand>()
                .With(m => m.ActivationCode, activationCode)
                .Create();

            householdMemberActivateCommandHandler.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMemberMock(), householdMemberActivateCommand, specificationMock);

            commonValidationsMock.AssertWasCalled(m => m.ContainsIllegalChar(Arg<string>.Is.Equal(activationCode)));
        }

        /// <summary>
        /// Tests that AddValidationRules calls ActivationCode on the household member which should be activated.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsActivationCodeHouseholdMember()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<Exception>.Is.Anything))
                .WhenCalled(e =>
                {
                    var func = (Func<bool>) e.Arguments.ElementAt(0);
                    func.Invoke();
                })
                .Return(specificationMock)
                .Repeat.Any();

            var householdMemberActivateCommandHandler = new HouseholdMemberActivateCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdMemberActivateCommandHandler, Is.Not.Null);

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();

            householdMemberActivateCommandHandler.AddValidationRules(householdMemberMock, fixture.Create<HouseholdMemberActivateCommand>(), specificationMock);

            householdMemberMock.AssertWasCalled(m => m.ActivationCode);
        }

        /// <summary>
        /// Tests that AddValidationRules calls Equals with activation code from the command and activation code from the household member which should be activated on the common validations.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsEqualsWithActivationCodeFromCommandAndActivationCodeFromHouseholdMemberOnCommonValidations()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<Exception>.Is.Anything))
                .WhenCalled(e =>
                {
                    var func = (Func<bool>) e.Arguments.ElementAt(0);
                    func.Invoke();
                })
                .Return(specificationMock)
                .Repeat.Any();

            var householdMemberActivateCommandHandler = new HouseholdMemberActivateCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdMemberActivateCommandHandler, Is.Not.Null);

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();

            var activationCode = fixture.Create<string>();
            var householdMemberActivateCommand = fixture.Build<HouseholdMemberActivateCommand>()
                .With(m => m.ActivationCode, activationCode)
                .Create();

            householdMemberActivateCommandHandler.AddValidationRules(householdMemberMock, householdMemberActivateCommand, specificationMock);

            commonValidationsMock.AssertWasCalled(m => m.Equals(Arg<string>.Is.Equal(activationCode), Arg<string>.Is.Equal(householdMemberMock.ActivationCode), Arg<StringComparison>.Is.Equal(StringComparison.Ordinal)));
        }

        /// <summary>
        /// Tests that AddValidationRules calls IsSatisfiedBy on the specification which encapsulates validation rules 4 times.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsIsSatisfiedByOnSpecification4Times()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdMemberActivateCommandHandler = new HouseholdMemberActivateCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdMemberActivateCommandHandler, Is.Not.Null);

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();

            var activationCode = fixture.Create<string>();
            var householdMemberActivateCommand = fixture.Build<HouseholdMemberActivateCommand>()
                .With(m => m.ActivationCode, activationCode)
                .Create();

            householdMemberActivateCommandHandler.AddValidationRules(householdMemberMock, householdMemberActivateCommand, specificationMock);

            specificationMock.AssertWasCalled(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<IntranetBusinessException>.Is.TypeOf), opt => opt.Repeat.Times(4));
        }

        /// <summary>
        /// Tests that ModifyData throws an ArgumentNullException when the household member which should be activated is null.
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

            var householdMemberActivateCommandHandler = new HouseholdMemberActivateCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdMemberActivateCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdMemberActivateCommandHandler.ModifyData(null, fixture.Create<HouseholdMemberActivateCommand>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("householdMember"));
        }

        /// <summary>
        /// Tests that ModifyData sets ActivationTime to the current date and time.
        /// </summary>
        [Test]
        public void TestThatModifyDataSetsActivationTimeToDateTimeNow()
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

            var householdMemberActivateCommandHandler = new HouseholdMemberActivateCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdMemberActivateCommandHandler, Is.Not.Null);

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();

            var testTime = DateTime.Now;

            householdMemberActivateCommandHandler.ModifyData(householdMemberMock, fixture.Create<HouseholdMemberActivateCommand>());

            householdMemberMock.AssertWasCalled(m => m.ActivationTime = Arg<DateTime>.Is.GreaterThanOrEqual(testTime));
            householdMemberMock.AssertWasCalled(m => m.ActivationTime = Arg<DateTime>.Is.LessThanOrEqual(testTime.AddSeconds(1)));
        }

        /// <summary>
        /// Tests that ModifyData calls Update with the household member which should be activated on the repository which can access household data for the food waste domain.
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

            var householdMemberActivateCommandHandler = new HouseholdMemberActivateCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdMemberActivateCommandHandler, Is.Not.Null);

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();

            householdMemberActivateCommandHandler.ModifyData(householdMemberMock, fixture.Create<HouseholdMemberActivateCommand>());

            householdDataRepositoryMock.AssertWasCalled(m => m.Update(Arg<IHouseholdMember>.Is.Equal(householdMemberMock)));
        }

        /// <summary>
        /// Tests that ModifyData returns the activated household member from the repository which can access household data for the food waste domain.
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

            var householdMemberActivateCommandHandler = new HouseholdMemberActivateCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdMemberActivateCommandHandler, Is.Not.Null);

            var result = householdMemberActivateCommandHandler.ModifyData(DomainObjectMockBuilder.BuildHouseholdMemberMock(), fixture.Create<HouseholdMemberActivateCommand>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(activatedHouseholdMemberMock));
        }
    }
}
