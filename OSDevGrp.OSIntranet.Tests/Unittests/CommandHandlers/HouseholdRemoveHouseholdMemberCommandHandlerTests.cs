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
    /// Test the command handler which handles a command for removing a household member from a given household on the current users household account.
    /// </summary>
    [TestFixture]
    public class HouseholdRemoveHouseholdMemberCommandHandlerTests
    {
        /// <summary>
        /// Tests that the constructor initialize a command handler which handles a command for removing a household member from a given household on the current users household account.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeHouseholdRemoveHouseholdMemberCommandHandler()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdRemoveHouseholdMemberCommandHandler = new HouseholdRemoveHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, exceptionBuilderMock);
            Assert.That(householdRemoveHouseholdMemberCommandHandler, Is.Not.Null);
            Assert.That(householdRemoveHouseholdMemberCommandHandler.ShouldBeActivated, Is.True);
            Assert.That(householdRemoveHouseholdMemberCommandHandler.ShouldHaveAcceptedPrivacyPolicy, Is.True);
            Assert.That(householdRemoveHouseholdMemberCommandHandler.RequiredMembership, Is.EqualTo(Membership.Basic));
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
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var exception = Assert.Throws<ArgumentNullException>(() => new HouseholdRemoveHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, null, exceptionBuilderMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("domainObjectValidations"));
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
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdRemoveHouseholdMemberCommandHandler = new HouseholdRemoveHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, exceptionBuilderMock);
            Assert.That(householdRemoveHouseholdMemberCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdRemoveHouseholdMemberCommandHandler.AddValidationRules(null, fixture.Create<HouseholdRemoveHouseholdMemberCommand>(), specificationMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("household"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that AddValidationRules throws an ArgumentNullException when the command for removing a household member from a given household on the current users household account is null.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesThrowsArgumentNullExceptionWhenHouseholdRemoveHouseholdMemberCommandIsNull()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdRemoveHouseholdMemberCommandHandler = new HouseholdRemoveHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, exceptionBuilderMock);
            Assert.That(householdRemoveHouseholdMemberCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdRemoveHouseholdMemberCommandHandler.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), null, specificationMock));
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
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdRemoveHouseholdMemberCommandHandler = new HouseholdRemoveHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, exceptionBuilderMock);
            Assert.That(householdRemoveHouseholdMemberCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdRemoveHouseholdMemberCommandHandler.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), fixture.Create<HouseholdRemoveHouseholdMemberCommand>(), null));
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
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdRemoveHouseholdMemberCommandHandler = new HouseholdRemoveHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, exceptionBuilderMock);
            Assert.That(householdRemoveHouseholdMemberCommandHandler, Is.Not.Null);

            var householdRemoveHouseholdMemberCommand = fixture.Build<HouseholdRemoveHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, fixture.Create<string>())
                .Create();

            householdRemoveHouseholdMemberCommandHandler.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), householdRemoveHouseholdMemberCommand, specificationMock);

            claimValueProviderMock.AssertWasCalled(m => m.MailAddress);
        }

        /// <summary>
        /// Tests that AddValidationRules calls HasValue with the mail address for the household member to remove on the common validations.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsHasValueWithMailAddressOnCommonValidations()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
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

            var householdRemoveHouseholdMemberCommandHandler = new HouseholdRemoveHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, exceptionBuilderMock);
            Assert.That(householdRemoveHouseholdMemberCommandHandler, Is.Not.Null);

            var mailAddress = fixture.Create<string>();
            var householdRemoveHouseholdMemberCommand = fixture.Build<HouseholdRemoveHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, mailAddress)
                .Create();

            householdRemoveHouseholdMemberCommandHandler.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), householdRemoveHouseholdMemberCommand, specificationMock);

            commonValidationsMock.AssertWasCalled(m => m.HasValue(Arg<string>.Is.Equal(mailAddress)));
        }

        /// <summary>
        /// Tests that AddValidationRules calls IsLengthValid with the mail address for the household member to remove on the common validations.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsIsLengthValidWithMailAddressOnCommonValidations()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
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

            var householdRemoveHouseholdMemberCommandHandler = new HouseholdRemoveHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, exceptionBuilderMock);
            Assert.That(householdRemoveHouseholdMemberCommandHandler, Is.Not.Null);

            var mailAddress = fixture.Create<string>();
            var householdRemoveHouseholdMemberCommand = fixture.Build<HouseholdRemoveHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, mailAddress)
                .Create();

            householdRemoveHouseholdMemberCommandHandler.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), householdRemoveHouseholdMemberCommand, specificationMock);

            commonValidationsMock.AssertWasCalled(m => m.IsLengthValid(Arg<string>.Is.Equal(mailAddress), Arg<int>.Is.Equal(1), Arg<int>.Is.Equal(128)));
        }

        /// <summary>
        /// Tests that AddValidationRules calls ContainsIllegalChar with the mail address for the household member to remove on the common validations.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsContainsIllegalCharWithMailAddressOnCommonValidations()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
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

            var householdRemoveHouseholdMemberCommandHandler = new HouseholdRemoveHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, exceptionBuilderMock);
            Assert.That(householdRemoveHouseholdMemberCommandHandler, Is.Not.Null);

            var mailAddress = fixture.Create<string>();
            var householdRemoveHouseholdMemberCommand = fixture.Build<HouseholdRemoveHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, mailAddress)
                .Create();

            householdRemoveHouseholdMemberCommandHandler.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), householdRemoveHouseholdMemberCommand, specificationMock);

            commonValidationsMock.AssertWasCalled(m => m.ContainsIllegalChar(Arg<string>.Is.Equal(mailAddress)));
        }

        /// <summary>
        /// Tests that AddValidationRules calls IsMailAddress with the mail address for the household member to remove on the the common validations used by domain objects in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsIsMailAddressWithMailAddressOnDomainObjectValidations()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
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

            var householdRemoveHouseholdMemberCommandHandler = new HouseholdRemoveHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, exceptionBuilderMock);
            Assert.That(householdRemoveHouseholdMemberCommandHandler, Is.Not.Null);

            var mailAddress = fixture.Create<string>();
            var householdRemoveHouseholdMemberCommand = fixture.Build<HouseholdRemoveHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, mailAddress)
                .Create();

            householdRemoveHouseholdMemberCommandHandler.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), householdRemoveHouseholdMemberCommand, specificationMock);

            domainObjectValidationsMock.AssertWasCalled(m => m.IsMailAddress(Arg<string>.Is.Equal(mailAddress)));
        }

        /// <summary>
        /// Tests that AddValidationRules calls Equals with the mail address for the household member to remove and the current users mail address on the common validations.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsEqualsWithMailAddressAndCurrentUserMailAddressOnCommonValidations()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
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

            var householdRemoveHouseholdMemberCommandHandler = new HouseholdRemoveHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, exceptionBuilderMock);
            Assert.That(householdRemoveHouseholdMemberCommandHandler, Is.Not.Null);

            var mailAddress = fixture.Create<string>();
            var householdRemoveHouseholdMemberCommand = fixture.Build<HouseholdRemoveHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, mailAddress)
                .Create();

            householdRemoveHouseholdMemberCommandHandler.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), householdRemoveHouseholdMemberCommand, specificationMock);

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
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdRemoveHouseholdMemberCommandHandler = new HouseholdRemoveHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, exceptionBuilderMock);
            Assert.That(householdRemoveHouseholdMemberCommandHandler, Is.Not.Null);

            var householdRemoveHouseholdMemberCommand = fixture.Build<HouseholdRemoveHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, fixture.Create<string>())
                .Create();

            householdRemoveHouseholdMemberCommandHandler.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), householdRemoveHouseholdMemberCommand, specificationMock);

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
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdRemoveHouseholdMemberCommandHandler = new HouseholdRemoveHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, exceptionBuilderMock);
            Assert.That(householdRemoveHouseholdMemberCommandHandler, Is.Not.Null);

            var householdRemoveHouseholdMemberCommand = fixture.Build<HouseholdRemoveHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, fixture.Create<string>())
                .Create();

            householdRemoveHouseholdMemberCommandHandler.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), householdRemoveHouseholdMemberCommand, specificationMock);

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
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdRemoveHouseholdMemberCommandHandler = new HouseholdRemoveHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, exceptionBuilderMock);
            Assert.That(householdRemoveHouseholdMemberCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdRemoveHouseholdMemberCommandHandler.ModifyData(null, fixture.Create<HouseholdRemoveHouseholdMemberCommand>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("household"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that ModifyData throws an ArgumentNullException when the command for removing a household member from a given household on the current users household account is null.
        /// </summary>
        [Test]
        public void TestThatModifyDataThrowsArgumentNullExceptionWhenHouseholdRemoveHouseholdMemberCommandIsNull()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdRemoveHouseholdMemberCommandHandler = new HouseholdRemoveHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, exceptionBuilderMock);
            Assert.That(householdRemoveHouseholdMemberCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdRemoveHouseholdMemberCommandHandler.ModifyData(DomainObjectMockBuilder.BuildHouseholdMock(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("command"));
            Assert.That(exception.InnerException, Is.Null);
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
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();
                
            var householdRemoveHouseholdMemberCommandHandler = new HouseholdRemoveHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, exceptionBuilderMock);
            Assert.That(householdRemoveHouseholdMemberCommandHandler, Is.Not.Null);

            var householdMock = DomainObjectMockBuilder.BuildHouseholdMock();

            var householdRemoveHouseholdMemberCommand = fixture.Build<HouseholdRemoveHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, fixture.Create<string>())
                .Create();

            householdRemoveHouseholdMemberCommandHandler.ModifyData(householdMock, householdRemoveHouseholdMemberCommand);

            householdMock.AssertWasCalled(m => m.HouseholdMembers);
        }

        /// <summary>
        /// Tests that ModifyData calls IsNotNull with the existing household member on the common validations when the household to modify does have a household member with the mail address.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsIsNotNullWithHouseholdMemberOnCommonValidationsWhenHouseholdDoesHaveHouseholdMemberWithMailAddress()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
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

            var householdRemoveHouseholdMemberCommandHandler = new HouseholdRemoveHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, exceptionBuilderMock);
            Assert.That(householdRemoveHouseholdMemberCommandHandler, Is.Not.Null);

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

            var householdRemoveHouseholdMemberCommand = fixture.Build<HouseholdRemoveHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, mailAddress)
                .Create();

            householdRemoveHouseholdMemberCommandHandler.ModifyData(householdMock, householdRemoveHouseholdMemberCommand);

            commonValidationsMock.AssertWasCalled(m => m.IsNotNull(Arg<IHouseholdMember>.Is.Equal(householdMemberMock)));
        }

        /// <summary>
        /// Tests that ModifyData calls IsNotNull with null on the common validations when the household to modify does not have a household member with the mail address.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsIsNotNullWithNullOnCommonValidationsWhenHouseholdDoesNotHaveHouseholdMemberWithMailAddress()
        {
            var fixture = new Fixture();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
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

            var householdRemoveHouseholdMemberCommandHandler = new HouseholdRemoveHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, exceptionBuilderMock);
            Assert.That(householdRemoveHouseholdMemberCommandHandler, Is.Not.Null);

            var householdMock = DomainObjectMockBuilder.BuildHouseholdMock();
            Assert.That(householdMock, Is.Not.Null);
            Assert.That(householdMock.HouseholdMembers, Is.Not.Null);
            Assert.That(householdMock.HouseholdMembers, Is.Not.Empty);

            var mailAddress = fixture.Create<string>();
            Assert.That(householdMock.HouseholdMembers.Any(householdMember => string.Compare(householdMember.MailAddress, mailAddress, StringComparison.OrdinalIgnoreCase) == 0), Is.False);

            var householdRemoveHouseholdMemberCommand = fixture.Build<HouseholdRemoveHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, mailAddress)
                .Create();

            householdRemoveHouseholdMemberCommandHandler.ModifyData(householdMock, householdRemoveHouseholdMemberCommand);

            commonValidationsMock.AssertWasCalled(m => m.IsNotNull(Arg<IHouseholdMember>.Is.Null));
        }

        /// <summary>
        /// Tests that ModifyData calls IsSatisfiedBy on the specification which encapsulates validation rules 1 time.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsIsSatisfiedByOnSpecification1Time()
        {
            var fixture = new Fixture();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdRemoveHouseholdMemberCommandHandler = new HouseholdRemoveHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, exceptionBuilderMock);
            Assert.That(householdRemoveHouseholdMemberCommandHandler, Is.Not.Null);

            var householdRemoveHouseholdMemberCommand = fixture.Build<HouseholdRemoveHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, fixture.Create<string>())
                .Create();

            householdRemoveHouseholdMemberCommandHandler.ModifyData(DomainObjectMockBuilder.BuildHouseholdMock(), householdRemoveHouseholdMemberCommand);

            specificationMock.AssertWasCalled(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<IntranetBusinessException>.Is.TypeOf), opt => opt.Repeat.Times(1));
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
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdRemoveHouseholdMemberCommandHandler = new HouseholdRemoveHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, exceptionBuilderMock);
            Assert.That(householdRemoveHouseholdMemberCommandHandler, Is.Not.Null);

            var householdRemoveHouseholdMemberCommand = fixture.Build<HouseholdRemoveHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, fixture.Create<string>())
                .Create();

            householdRemoveHouseholdMemberCommandHandler.ModifyData(DomainObjectMockBuilder.BuildHouseholdMock(), householdRemoveHouseholdMemberCommand);

            specificationMock.AssertWasCalled(m => m.Evaluate());
        }

        /// <summary>
        /// Tests that ModifyData calls HouseholdMemberRemove with the household member for the mail address.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsHouseholdMemberRemoveWithHouseholdMemberForMailAddressOnHousehold()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdRemoveHouseholdMemberCommandHandler = new HouseholdRemoveHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, exceptionBuilderMock);
            Assert.That(householdRemoveHouseholdMemberCommandHandler, Is.Not.Null);

            var householdMock = DomainObjectMockBuilder.BuildHouseholdMock();
            Assert.That(householdMock, Is.Not.Null);
            Assert.That(householdMock.HouseholdMembers, Is.Not.Null);
            Assert.That(householdMock.HouseholdMembers, Is.Not.Empty);

            var householdMemberMock = householdMock.HouseholdMembers.ElementAt(random.Next(0, householdMock.HouseholdMembers.Count() - 1));
            Assert.That(householdMemberMock, Is.Not.Null);
            Assert.That(householdMemberMock.MailAddress, Is.Not.Null);
            Assert.That(householdMemberMock.MailAddress, Is.Not.Empty);

            var householdRemoveHouseholdMemberCommand = fixture.Build<HouseholdRemoveHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, householdMemberMock.MailAddress)
                .Create();

            householdRemoveHouseholdMemberCommandHandler.ModifyData(householdMock, householdRemoveHouseholdMemberCommand);

            householdMock.AssertWasCalled(m => m.HouseholdMemberRemove(Arg<IHouseholdMember>.Is.Equal(householdMemberMock)));
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
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdRemoveHouseholdMemberCommandHandler = new HouseholdRemoveHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, exceptionBuilderMock);
            Assert.That(householdRemoveHouseholdMemberCommandHandler, Is.Not.Null);

            var householdMock = DomainObjectMockBuilder.BuildHouseholdMock();

            var householdRemoveHouseholdMemberCommand = fixture.Build<HouseholdRemoveHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, fixture.Create<string>())
                .Create();

            householdRemoveHouseholdMemberCommandHandler.ModifyData(householdMock, householdRemoveHouseholdMemberCommand);

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
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var updatedHouseholdMock = DomainObjectMockBuilder.BuildHouseholdMock();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(updatedHouseholdMock)
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdRemoveHouseholdMemberCommandHandler = new HouseholdRemoveHouseholdMemberCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, exceptionBuilderMock);
            Assert.That(householdRemoveHouseholdMemberCommandHandler, Is.Not.Null);

            var householdRemoveHouseholdMemberCommand = fixture.Build<HouseholdRemoveHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, fixture.Create<string>())
                .Create();

            var result = householdRemoveHouseholdMemberCommandHandler.ModifyData(DomainObjectMockBuilder.BuildHouseholdMock(), householdRemoveHouseholdMemberCommand);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(updatedHouseholdMock));
        }
    }
}
