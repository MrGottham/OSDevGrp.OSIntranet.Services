using System;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommandHandlers;
using OSDevGrp.OSIntranet.CommandHandlers.Core;
using OSDevGrp.OSIntranet.CommandHandlers.Validation;
using OSDevGrp.OSIntranet.Contracts.Commands;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.CommandHandlers
{
    /// <summary>
    /// Test the command handler which handles a command for adding a household member to a given household on the current users household account.
    /// </summary>
    [TestFixture]
    public class HouseholdAddHouseholdMemberCommandHandlerTests
    {
        /// <summary>
        /// Tests that the constructor initialize a command handler which handles a command for adding a household member to a given household on the current users household account.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeHouseholdAddHouseholdMemberCommandHandler()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdAddHouseholdMemberCommandHandler = new HouseholdAddHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddHouseholdMemberCommandHandler, Is.Not.Null);
            Assert.That(householdAddHouseholdMemberCommandHandler.ShouldBeActivated, Is.True);
            Assert.That(householdAddHouseholdMemberCommandHandler.ShouldHaveAcceptedPrivacyPolicy, Is.True);
            Assert.That(householdAddHouseholdMemberCommandHandler.RequiredMembership, Is.EqualTo(Membership.Basic));
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the common validations used by domain objects in the food waste domain is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenDomainObjectValidationsIsNull()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var exception = Assert.Throws<ArgumentNullException>(() => new HouseholdAddHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, null, logicExecutorMock, exceptionBuilderMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("domainObjectValidations"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the logic executor which can execute basic logic is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenLogicExecutorIsNull()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var exception = Assert.Throws<ArgumentNullException>(() => new HouseholdAddHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, null, exceptionBuilderMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("logicExecutor"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that AddValidationRules throws an ArgumentNullException when the household on which to modify data is null.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesThrowsArgumentNullExceptionWhenHouseholdIsNull()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdAddHouseholdMemberCommandHandler = new HouseholdAddHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddHouseholdMemberCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdAddHouseholdMemberCommandHandler.AddValidationRules(null, fixture.Create<HouseholdAddHouseholdMemberCommand>(), specificationMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("household"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that AddValidationRules throws an ArgumentNullException when the command for adding a household member to a given household on the current users household account is null.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesThrowsArgumentNullExceptionWhenHouseholdAddHouseholdMemberCommandIsNull()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdAddHouseholdMemberCommandHandler = new HouseholdAddHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddHouseholdMemberCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdAddHouseholdMemberCommandHandler.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), null, specificationMock));
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
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdAddHouseholdMemberCommandHandler = new HouseholdAddHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddHouseholdMemberCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdAddHouseholdMemberCommandHandler.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), fixture.Create<HouseholdAddHouseholdMemberCommand>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("specification"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that AddValidationRules calls MailAddress on the provider which can resolve values from the current users claims.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsMailAddressOnClaimValueProvider()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdAddHouseholdMemberCommandHandler = new HouseholdAddHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddHouseholdMemberCommandHandler, Is.Not.Null);

            var householdAddHouseholdMemberCommand = fixture.Build<HouseholdAddHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdAddHouseholdMemberCommandHandler.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), householdAddHouseholdMemberCommand, specificationMock);

            claimValueProviderMock.AssertWasCalled(m => m.MailAddress);
        }

        /// <summary>
        /// Tests that AddValidationRules calls HasValue with the mail address for the household member to add on the common validations.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsHasValueWithMailAddressOnCommonValidations()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<Exception>.Is.Anything))
                .WhenCalled(e =>
                {
                    var func = (Func<bool>) e.Arguments.ElementAt(0);
                    func.Invoke();
                })
                .Return(specificationMock)
                .Repeat.Any();

            var householdAddHouseholdMemberCommandHandler = new HouseholdAddHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddHouseholdMemberCommandHandler, Is.Not.Null);

            var mailAddress = fixture.Create<string>();
            var householdAddHouseholdMemberCommand = fixture.Build<HouseholdAddHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, mailAddress)
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdAddHouseholdMemberCommandHandler.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), householdAddHouseholdMemberCommand, specificationMock);

            commonValidationsMock.AssertWasCalled(m => m.HasValue(Arg<string>.Is.Equal(mailAddress)));
        }

        /// <summary>
        /// Tests that AddValidationRules calls IsLengthValid with the mail address for the household member to add on the common validations.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsIsLengthValidWithMailAddressOnCommonValidations()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<Exception>.Is.Anything))
                .WhenCalled(e =>
                {
                    var func = (Func<bool>) e.Arguments.ElementAt(0);
                    func.Invoke();
                })
                .Return(specificationMock)
                .Repeat.Any();

            var householdAddHouseholdMemberCommandHandler = new HouseholdAddHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddHouseholdMemberCommandHandler, Is.Not.Null);

            var mailAddress = fixture.Create<string>();
            var householdAddHouseholdMemberCommand = fixture.Build<HouseholdAddHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, mailAddress)
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdAddHouseholdMemberCommandHandler.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), householdAddHouseholdMemberCommand, specificationMock);

            commonValidationsMock.AssertWasCalled(m => m.IsLengthValid(Arg<string>.Is.Equal(mailAddress), Arg<int>.Is.Equal(1), Arg<int>.Is.Equal(128)));
        }

        /// <summary>
        /// Tests that AddValidationRules calls ContainsIllegalChar with the mail address for the household member to add on the common validations.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsContainsIllegalCharWithMailAddressOnCommonValidations()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<Exception>.Is.Anything))
                .WhenCalled(e =>
                {
                    var func = (Func<bool>) e.Arguments.ElementAt(0);
                    func.Invoke();
                })
                .Return(specificationMock)
                .Repeat.Any();

            var householdAddHouseholdMemberCommandHandler = new HouseholdAddHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddHouseholdMemberCommandHandler, Is.Not.Null);

            var mailAddress = fixture.Create<string>();
            var householdAddHouseholdMemberCommand = fixture.Build<HouseholdAddHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, mailAddress)
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdAddHouseholdMemberCommandHandler.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), householdAddHouseholdMemberCommand, specificationMock);

            commonValidationsMock.AssertWasCalled(m => m.ContainsIllegalChar(Arg<string>.Is.Equal(mailAddress)));
        }

        /// <summary>
        /// Tests that AddValidationRules calls IsMailAddress with the mail address for the household member to add on the common validations used by domain objects in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsIsMailAddressWithMailAddressOnDomainObjectValidations()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<Exception>.Is.Anything))
                .WhenCalled(e =>
                {
                    var func = (Func<bool>) e.Arguments.ElementAt(0);
                    func.Invoke();
                })
                .Return(specificationMock)
                .Repeat.Any();

            var householdAddHouseholdMemberCommandHandler = new HouseholdAddHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddHouseholdMemberCommandHandler, Is.Not.Null);

            var mailAddress = fixture.Create<string>();
            var householdAddHouseholdMemberCommand = fixture.Build<HouseholdAddHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, mailAddress)
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdAddHouseholdMemberCommandHandler.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), householdAddHouseholdMemberCommand, specificationMock);

            domainObjectValidationsMock.AssertWasCalled(m => m.IsMailAddress(Arg<string>.Is.Equal(mailAddress)));
        }

        /// <summary>
        /// Tests that AddValidationRules calls Equals with the mail address for the household member to add and the current users mail address on the common validations.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsEqualsWithMailAddressAndCurrentUserMailAddressOnCommonValidations()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var currentUserMailAddress = fixture.Create<string>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(currentUserMailAddress)
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<Exception>.Is.Anything))
                .WhenCalled(e =>
                {
                    var func = (Func<bool>) e.Arguments.ElementAt(0);
                    func.Invoke();
                })
                .Return(specificationMock)
                .Repeat.Any();

            var householdAddHouseholdMemberCommandHandler = new HouseholdAddHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddHouseholdMemberCommandHandler, Is.Not.Null);

            var mailAddress = fixture.Create<string>();
            var householdAddHouseholdMemberCommand = fixture.Build<HouseholdAddHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, mailAddress)
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdAddHouseholdMemberCommandHandler.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), householdAddHouseholdMemberCommand, specificationMock);

            commonValidationsMock.AssertWasCalled(m => m.Equals(Arg<string>.Is.Equal(mailAddress), Arg<string>.Is.Equal(currentUserMailAddress), Arg<StringComparison>.Is.Equal(StringComparison.OrdinalIgnoreCase)));
        }

        /// <summary>
        /// Tests that AddValidationRules calls IsSatisfiedBy on the specification which encapsulates validation rules 5 times.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsIsSatisfiedByOnSpecification5Times()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdAddHouseholdMemberCommandHandler = new HouseholdAddHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddHouseholdMemberCommandHandler, Is.Not.Null);

            var householdAddHouseholdMemberCommand = fixture.Build<HouseholdAddHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdAddHouseholdMemberCommandHandler.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), householdAddHouseholdMemberCommand, specificationMock);

            specificationMock.AssertWasCalled(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<IntranetBusinessException>.Is.TypeOf), opt => opt.Repeat.Times(5));
        }

        /// <summary>
        /// Tests that AddValidationRules does not call Evaluate on the specification which encapsulates validation rules.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesDoesNotCallsEvaluateOnSpecification()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdAddHouseholdMemberCommandHandler = new HouseholdAddHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddHouseholdMemberCommandHandler, Is.Not.Null);

            var householdAddHouseholdMemberCommand = fixture.Build<HouseholdAddHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdAddHouseholdMemberCommandHandler.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), householdAddHouseholdMemberCommand, specificationMock);

            specificationMock.AssertWasNotCalled(m => m.Evaluate());
        }

        /// <summary>
        /// Tests that ModifyData throws an ArgumentNullException when the household on which to modify data is null.
        /// </summary>
        [Test]
        public void TestThatModifyDataThrowsArgumentNullExceptionWhenHouseholdIsNull()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdAddHouseholdMemberCommandHandler = new HouseholdAddHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddHouseholdMemberCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdAddHouseholdMemberCommandHandler.ModifyData(null, fixture.Create<HouseholdAddHouseholdMemberCommand>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("household"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that ModifyData throws an ArgumentNullException when the command for adding a household member to a given household on the current users household account is null.
        /// </summary>
        [Test]
        public void TestThatModifyDataThrowsArgumentNullExceptionWhenHouseholdAddHouseholdMemberCommandIsNull()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdAddHouseholdMemberCommandHandler = new HouseholdAddHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddHouseholdMemberCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdAddHouseholdMemberCommandHandler.ModifyData(DomainObjectMockBuilder.BuildHouseholdMock(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("command"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that ModifyData calls Get with the identifier of the translation informations which should be used in the translation on the repository which can access household data for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsGetWithTranslationInfoIdentifierOnHouseholdDataRepository()
        {
            var fixture = new Fixture();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdAddHouseholdMemberCommandHandler = new HouseholdAddHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddHouseholdMemberCommandHandler, Is.Not.Null);

            var translationInfoIdentifier = Guid.NewGuid();
            var householdAddHouseholdMemberCommand = fixture.Build<HouseholdAddHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, translationInfoIdentifier)
                .Create();

            householdAddHouseholdMemberCommandHandler.ModifyData(DomainObjectMockBuilder.BuildHouseholdMock(), householdAddHouseholdMemberCommand);

            householdDataRepositoryMock.AssertWasCalled(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Equal(translationInfoIdentifier)));
        }

        /// <summary>
        /// Tests that ModifyData calls HouseholdMembers on the household on which to modify data.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsHouseholdMembersOnHousehold()
        {
            var fixture = new Fixture();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdAddHouseholdMemberCommandHandler = new HouseholdAddHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddHouseholdMemberCommandHandler, Is.Not.Null);

            var householdMock = DomainObjectMockBuilder.BuildHouseholdMock();

            var householdAddHouseholdMemberCommand = fixture.Build<HouseholdAddHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdAddHouseholdMemberCommandHandler.ModifyData(householdMock, householdAddHouseholdMemberCommand);

            householdMock.AssertWasCalled(m => m.HouseholdMembers);
        }

        /// <summary>
        /// Tests that ModifyData calls IsNotNull with the the translation informations which should be used in the translation on the common validations.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsIsNotNullWithTranslationInfoOnCommonValidations()
        {
            var fixture = new Fixture();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var translationInfoMock = DomainObjectMockBuilder.BuildTranslationInfoMock();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(translationInfoMock)
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<Exception>.Is.Anything))
                .WhenCalled(e =>
                {
                    var func = (Func<bool>) e.Arguments.ElementAt(0);
                    func.Invoke();
                })
                .Return(specificationMock)
                .Repeat.Any();

            var householdAddHouseholdMemberCommandHandler = new HouseholdAddHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddHouseholdMemberCommandHandler, Is.Not.Null);

            var householdAddHouseholdMemberCommand = fixture.Build<HouseholdAddHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdAddHouseholdMemberCommandHandler.ModifyData(DomainObjectMockBuilder.BuildHouseholdMock(), householdAddHouseholdMemberCommand);

            commonValidationsMock.AssertWasCalled(m => m.IsNotNull(Arg<ITranslationInfo>.Is.Equal(translationInfoMock)));
        }

        /// <summary>
        /// Tests that ModifyData calls IsNull with null on the common validations when the household to modify does not have a household member with the mail address.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsIsNullWithNullOnCommonValidationsWhenHouseholdDoesNotHaveHouseholdMemberWithMailAddress()
        {
            var fixture = new Fixture();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var translationInfoMock = DomainObjectMockBuilder.BuildTranslationInfoMock();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(translationInfoMock)
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<Exception>.Is.Anything))
                .WhenCalled(e =>
                {
                    var func = (Func<bool>) e.Arguments.ElementAt(0);
                    func.Invoke();
                })
                .Return(specificationMock)
                .Repeat.Any();

            var householdAddHouseholdMemberCommandHandler = new HouseholdAddHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddHouseholdMemberCommandHandler, Is.Not.Null);

            var householdMock = DomainObjectMockBuilder.BuildHouseholdMock();
            Assert.That(householdMock, Is.Not.Null);
            Assert.That(householdMock.HouseholdMembers, Is.Not.Null);
            Assert.That(householdMock.HouseholdMembers, Is.Not.Empty);

            var mailAddress = fixture.Create<string>();
            Assert.That(householdMock.HouseholdMembers.Any(householdMember => string.Compare(householdMember.MailAddress, mailAddress, StringComparison.OrdinalIgnoreCase) == 0), Is.False);

            var householdAddHouseholdMemberCommand = fixture.Build<HouseholdAddHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, mailAddress)
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdAddHouseholdMemberCommandHandler.ModifyData(householdMock, householdAddHouseholdMemberCommand);

            commonValidationsMock.AssertWasCalled(m => m.IsNull(Arg<IHouseholdMember>.Is.Null));
        }

        /// <summary>
        /// Tests that ModifyData calls IsNull with the existing household member on the common validations when the household to modify does have a household member with the mail address.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsIsNullWithHouseholdMemberOnCommonValidationsWhenHouseholdDoesHaveHouseholdMemberWithMailAddress()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var translationInfoMock = DomainObjectMockBuilder.BuildTranslationInfoMock();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(translationInfoMock)
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<Exception>.Is.Anything))
                .WhenCalled(e =>
                {
                    var func = (Func<bool>) e.Arguments.ElementAt(0);
                    func.Invoke();
                })
                .Return(specificationMock)
                .Repeat.Any();

            var householdAddHouseholdMemberCommandHandler = new HouseholdAddHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddHouseholdMemberCommandHandler, Is.Not.Null);

            var householdMock = DomainObjectMockBuilder.BuildHouseholdMock();
            Assert.That(householdMock, Is.Not.Null);
            Assert.That(householdMock.HouseholdMembers, Is.Not.Null);
            Assert.That(householdMock.HouseholdMembers, Is.Not.Empty);

            var householdMemberMock = householdMock.HouseholdMembers.ElementAt(random.Next(0, householdMock.HouseholdMembers.Count() - 1));
            Assert.That(householdMemberMock, Is.Not.Null);
            Assert.That(householdMemberMock.MailAddress, Is.Not.Null);
            Assert.That(householdMemberMock.MailAddress, Is.Not.Empty);

            var mailAddress = householdMemberMock.MailAddress;
            Assert.That(householdMock.HouseholdMembers.Any(householdMember => string.Compare(householdMember.MailAddress, mailAddress, StringComparison.OrdinalIgnoreCase) == 0), Is.True);

            var householdAddHouseholdMemberCommand = fixture.Build<HouseholdAddHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, mailAddress)
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdAddHouseholdMemberCommandHandler.ModifyData(householdMock, householdAddHouseholdMemberCommand);

            commonValidationsMock.AssertWasCalled(m => m.IsNull(Arg<IHouseholdMember>.Is.Equal(householdMemberMock)));
        }

        /// <summary>
        /// Tests that ModifyData calls IsSatisfiedBy on the specification which encapsulates validation rules 2 times.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsIsSatisfiedByOnSpecification2Times()
        {
            var fixture = new Fixture();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdAddHouseholdMemberCommandHandler = new HouseholdAddHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddHouseholdMemberCommandHandler, Is.Not.Null);

            var householdAddHouseholdMemberCommand = fixture.Build<HouseholdAddHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdAddHouseholdMemberCommandHandler.ModifyData(DomainObjectMockBuilder.BuildHouseholdMock(), householdAddHouseholdMemberCommand);

            specificationMock.AssertWasCalled(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<IntranetBusinessException>.Is.TypeOf), opt => opt.Repeat.Times(2));
        }

        /// <summary>
        /// Tests that ModifyData calls Evaluate on the specification which encapsulates validation rules.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsEvaluateOnSpecification()
        {
            var fixture = new Fixture();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdAddHouseholdMemberCommandHandler = new HouseholdAddHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddHouseholdMemberCommandHandler, Is.Not.Null);

            var householdAddHouseholdMemberCommand = fixture.Build<HouseholdAddHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdAddHouseholdMemberCommandHandler.ModifyData(DomainObjectMockBuilder.BuildHouseholdMock(), householdAddHouseholdMemberCommand);

            specificationMock.AssertWasCalled(m => m.Evaluate());
        }

        /// <summary>
        /// Tests that ModifyData calls HouseholdMemberGetByMailAddress on the repository which can access household data for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsHouseholdMemberGetByMailAddressOnHouseholdDataRepository()
        {
            var fixture = new Fixture();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdAddHouseholdMemberCommandHandler = new HouseholdAddHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddHouseholdMemberCommandHandler, Is.Not.Null);

            var mailAddress = fixture.Create<string>();
            var householdAddHouseholdMemberCommand = fixture.Build<HouseholdAddHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, mailAddress)
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdAddHouseholdMemberCommandHandler.ModifyData(DomainObjectMockBuilder.BuildHouseholdMock(), householdAddHouseholdMemberCommand);

            householdDataRepositoryMock.AssertWasCalled(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Equal(mailAddress)));
        }

        /// <summary>
        /// Tests that ModifyData calls HouseholdMemberAdd on the logic executor which can execute basic logic when a household member with the mail address does not exists.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsHouseholdMemberAddOnLogicExecutorWhenHouseholdMemberForMailAddressDoesNotExist()
        {
            var fixture = new Fixture();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var translationInfoMock = DomainObjectMockBuilder.BuildTranslationInfoMock();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(translationInfoMock)
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(null)
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutorMock.Stub(m => m.HouseholdMemberAdd(Arg<string>.Is.Anything, Arg<Guid>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var householdAddHouseholdMemberCommandHandler = new HouseholdAddHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddHouseholdMemberCommandHandler, Is.Not.Null);

            var mailAddress = fixture.Create<string>();
            var householdAddHouseholdMemberCommand = fixture.Build<HouseholdAddHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, mailAddress)
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdAddHouseholdMemberCommandHandler.ModifyData(DomainObjectMockBuilder.BuildHouseholdMock(), householdAddHouseholdMemberCommand);

            // ReSharper disable PossibleInvalidOperationException
            logicExecutorMock.AssertWasCalled(m => m.HouseholdMemberAdd(Arg<string>.Is.Equal(mailAddress), Arg<Guid>.Is.Equal(translationInfoMock.Identifier.Value)));
            // ReSharper restore PossibleInvalidOperationException
        }

        /// <summary>
        /// Tests that ModifyData calls Get with the identifier for the created household member on the repository which can access household data for the food waste domain when a household member with the mail address does not exists.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsGetWithIdentifierForCreatedHouseholdMemberOnHouseholdDataRepositoryWhenHouseholdMemberForMailAddressDoesNotExist()
        {
            var fixture = new Fixture();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(null)
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Get<IHouseholdMember>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var createdHouseholdMemberIdentifier = Guid.NewGuid();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutorMock.Stub(m => m.HouseholdMemberAdd(Arg<string>.Is.Anything, Arg<Guid>.Is.Anything))
                .Return(createdHouseholdMemberIdentifier)
                .Repeat.Any();

            var householdAddHouseholdMemberCommandHandler = new HouseholdAddHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddHouseholdMemberCommandHandler, Is.Not.Null);

            var householdAddHouseholdMemberCommand = fixture.Build<HouseholdAddHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdAddHouseholdMemberCommandHandler.ModifyData(DomainObjectMockBuilder.BuildHouseholdMock(), householdAddHouseholdMemberCommand);

            householdDataRepositoryMock.AssertWasCalled(m => m.Get<IHouseholdMember>(Arg<Guid>.Is.Equal(createdHouseholdMemberIdentifier)));
        }

        /// <summary>
        /// Tests that ModifyData calls HouseholdMemberAdd with the created household member on the household when a household member with the mail address does not exists.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsHouseholdMemberAddWithCreatedHouseholdMemberOnHouseholdWhenHouseholdMemberForMailAddressDoesNotExist()
        {
            var fixture = new Fixture();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var createdHouseholdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(null)
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Get<IHouseholdMember>(Arg<Guid>.Is.Anything))
                .Return(createdHouseholdMemberMock)
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutorMock.Stub(m => m.HouseholdMemberAdd(Arg<string>.Is.Anything, Arg<Guid>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var householdAddHouseholdMemberCommandHandler = new HouseholdAddHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddHouseholdMemberCommandHandler, Is.Not.Null);

            var householdMock = DomainObjectMockBuilder.BuildHouseholdMock();

            var householdAddHouseholdMemberCommand = fixture.Build<HouseholdAddHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdAddHouseholdMemberCommandHandler.ModifyData(householdMock, householdAddHouseholdMemberCommand);

            householdMock.AssertWasCalled(m => m.HouseholdMemberAdd(Arg<IHouseholdMember>.Is.Equal(createdHouseholdMemberMock)));
        }

        /// <summary>
        /// Tests that ModifyData does not call HouseholdMemberAdd on the logic executor which can execute basic logic when a household member with the mail address exists.
        /// </summary>
        [Test]
        public void TestThatModifyDataDoesNotCallHouseholdMemberAddOnLogicExecutorWhenHouseholdMemberForMailAddressExists()
        {
            var fixture = new Fixture();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdAddHouseholdMemberCommandHandler = new HouseholdAddHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddHouseholdMemberCommandHandler, Is.Not.Null);

            var householdAddHouseholdMemberCommand = fixture.Build<HouseholdAddHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdAddHouseholdMemberCommandHandler.ModifyData(DomainObjectMockBuilder.BuildHouseholdMock(), householdAddHouseholdMemberCommand);

            logicExecutorMock.AssertWasNotCalled(m => m.HouseholdMemberAdd(Arg<string>.Is.Anything, Arg<Guid>.Is.Anything));
        }

        /// <summary>
        /// Tests that ModifyData does not call Get for any household members on the repository which can access household data for the food waste domain when a household member with the mail address exists.
        /// </summary>
        [Test]
        public void TestThatModifyDataDoesNotCallGetForAnyHouseholdMemberOnHouseholdDataRepositoryWhenHouseholdMemberForMailAddressExists()
        {
            var fixture = new Fixture();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdAddHouseholdMemberCommandHandler = new HouseholdAddHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddHouseholdMemberCommandHandler, Is.Not.Null);

            var householdAddHouseholdMemberCommand = fixture.Build<HouseholdAddHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdAddHouseholdMemberCommandHandler.ModifyData(DomainObjectMockBuilder.BuildHouseholdMock(), householdAddHouseholdMemberCommand);

            householdDataRepositoryMock.AssertWasNotCalled(m => m.Get<IHouseholdMember>(Arg<Guid>.Is.Anything));
        }

        /// <summary>
        /// Tests that ModifyData calls HouseholdMemberAdd with the household member for the mail address on the household when a household member with the mail address exists.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsHouseholdMemberAddWithHouseholdMemberForMailAddressOnHouseholdWhenHouseholdMemberForMailAddressExists()
        {
            var fixture = new Fixture();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdMemberForMailAddressMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(householdMemberForMailAddressMock)
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdAddHouseholdMemberCommandHandler = new HouseholdAddHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddHouseholdMemberCommandHandler, Is.Not.Null);

            var householdMock = DomainObjectMockBuilder.BuildHouseholdMock();

            var householdAddHouseholdMemberCommand = fixture.Build<HouseholdAddHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdAddHouseholdMemberCommandHandler.ModifyData(householdMock, householdAddHouseholdMemberCommand);

            householdMock.AssertWasCalled(m => m.HouseholdMemberAdd(Arg<IHouseholdMember>.Is.Equal(householdMemberForMailAddressMock)));
        }

        /// <summary>
        /// Tests that ModifyData calls Update with household on the repository which can access household data for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsUpdateWithHouseholdOnHouseholdDataRepository()
        {
            var fixture = new Fixture();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdAddHouseholdMemberCommandHandler = new HouseholdAddHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddHouseholdMemberCommandHandler, Is.Not.Null);

            var householdMock = DomainObjectMockBuilder.BuildHouseholdMock();

            var householdAddHouseholdMemberCommand = fixture.Build<HouseholdAddHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdAddHouseholdMemberCommandHandler.ModifyData(householdMock, householdAddHouseholdMemberCommand);

            householdDataRepositoryMock.AssertWasCalled(m => m.Update(Arg<IHousehold>.Is.Equal(householdMock)));
        }

        /// <summary>
        /// Tests that ModifyData returns the result from Update called with household on the repository which can access household data for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatModifyDataReturnsResultFromUpdateCalledWithHouseholdOnHouseholdDataRepository()
        {
            var fixture = new Fixture();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var updatedHouseholdMock = DomainObjectMockBuilder.BuildHouseholdMock();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(updatedHouseholdMock)
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdAddHouseholdMemberCommandHandler = new HouseholdAddHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddHouseholdMemberCommandHandler, Is.Not.Null);

            var householdAddHouseholdMemberCommand = fixture.Build<HouseholdAddHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            var result = householdAddHouseholdMemberCommandHandler.ModifyData(DomainObjectMockBuilder.BuildHouseholdMock(), householdAddHouseholdMemberCommand);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(updatedHouseholdMock));
        }
    }
}
