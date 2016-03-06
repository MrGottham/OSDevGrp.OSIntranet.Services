using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommandHandlers;
using OSDevGrp.OSIntranet.CommandHandlers.Dispatchers;
using OSDevGrp.OSIntranet.CommandHandlers.Validation;
using OSDevGrp.OSIntranet.Contracts.Commands;
using OSDevGrp.OSIntranet.Contracts.Responses;
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
    /// Tests the command handler which handles a command for adding a household member.
    /// </summary>
    [TestFixture]
    public class HouseholdMemberAddCommandHandlerTests
    {
        /// <summary>
        /// Tests that the constructor initialize the basic functionality for command handlers which handles commands for household data in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeFoodWasteHouseholdDataCommandHandlerBase()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var welcomeLetterDispatcherMock = MockRepository.GenerateMock<IWelcomeLetterDispatcher>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdMemberAddCommandHandler = new HouseholdMemberAddCommandHandler(householdDataRepositoryMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, welcomeLetterDispatcherMock, exceptionBuilderMock);
            Assert.That(householdMemberAddCommandHandler, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the common validations used by domain objects in the food waste domain is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenDomainObjectValidationsIsNull()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var welcomeLetterDispatcherMock = MockRepository.GenerateMock<IWelcomeLetterDispatcher>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var exception = Assert.Throws<ArgumentNullException>(() => new HouseholdMemberAddCommandHandler(householdDataRepositoryMock, objectMapperMock, specificationMock, commonValidationsMock, null, welcomeLetterDispatcherMock, exceptionBuilderMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("domainObjectValidations"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the dispatcher which can dispatch the welcome letter to a household member is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenWelcomeLetterDispatcherIsNull()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var exception = Assert.Throws<ArgumentNullException>(() => new HouseholdMemberAddCommandHandler(householdDataRepositoryMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, null, exceptionBuilderMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("welcomeLetterDispatcher"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Execute throws an ArgumentNullException when command which adds a household member is null.
        /// </summary>
        [Test]
        public void TestThatExecuteThrowsArgumentNullExceptionWhenHouseholdMemberAddCommandIsNull()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var welcomeLetterDispatcherMock = MockRepository.GenerateMock<IWelcomeLetterDispatcher>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdMemberAddCommandHandler = new HouseholdMemberAddCommandHandler(householdDataRepositoryMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, welcomeLetterDispatcherMock, exceptionBuilderMock);
            Assert.That(householdMemberAddCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdMemberAddCommandHandler.Execute(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("command"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Execute calls Get with the identifier of translation informations on the repository which can access household data for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsGetForTranslationInfoIdentifierOnHouseholdDataRepository()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var welcomeLetterDispatcherMock = MockRepository.GenerateMock<IWelcomeLetterDispatcher>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IHouseholdMember>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var householdMemberAddCommandHandler = new HouseholdMemberAddCommandHandler(householdDataRepositoryMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, welcomeLetterDispatcherMock, exceptionBuilderMock);
            Assert.That(householdMemberAddCommandHandler, Is.Not.Null);

            var householdMemberAddCommand = fixture.Build<HouseholdMemberAddCommand>()
                .With(m => m.MailAddress, string.Format("test.{0}@osdevgrp.dk", Guid.NewGuid().ToString("D")).ToLower())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdMemberAddCommandHandler.Execute(householdMemberAddCommand);

            householdDataRepositoryMock.AssertWasCalled(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Equal(householdMemberAddCommand.TranslationInfoIdentifier)));
        }

        /// <summary>
        /// Tests that Execute calls IsNotNull with the translation informations on the common validations.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsIsNotNullWithTranslationInfoOnCommonValidations()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var welcomeLetterDispatcherMock = MockRepository.GenerateMock<IWelcomeLetterDispatcher>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var translationInfoMock = DomainObjectMockBuilder.BuildTranslationInfoMock();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(translationInfoMock)
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IHouseholdMember>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
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

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var householdMemberAddCommandHandler = new HouseholdMemberAddCommandHandler(householdDataRepositoryMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, welcomeLetterDispatcherMock, exceptionBuilderMock);
            Assert.That(householdMemberAddCommandHandler, Is.Not.Null);

            var householdMemberAddCommand = fixture.Build<HouseholdMemberAddCommand>()
                .With(m => m.MailAddress, string.Format("test.{0}@osdevgrp.dk", Guid.NewGuid().ToString("D")).ToLower())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdMemberAddCommandHandler.Execute(householdMemberAddCommand);

            commonValidationsMock.AssertWasCalled(m => m.IsNotNull(Arg<ITranslationInfo>.Is.Equal(translationInfoMock)));
        }

        /// <summary>
        /// Tests that Execute calls HasValue with the mail address on the common validations.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsHasValueWithMailAddressOnCommonValidations()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var welcomeLetterDispatcherMock = MockRepository.GenerateMock<IWelcomeLetterDispatcher>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IHouseholdMember>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
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

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var householdMemberAddCommandHandler = new HouseholdMemberAddCommandHandler(householdDataRepositoryMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, welcomeLetterDispatcherMock, exceptionBuilderMock);
            Assert.That(householdMemberAddCommandHandler, Is.Not.Null);

            var householdMemberAddCommand = fixture.Build<HouseholdMemberAddCommand>()
                .With(m => m.MailAddress, string.Format("test.{0}@osdevgrp.dk", Guid.NewGuid().ToString("D")).ToLower())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdMemberAddCommandHandler.Execute(householdMemberAddCommand);

            commonValidationsMock.AssertWasCalled(m => m.HasValue(Arg<string>.Is.Equal(householdMemberAddCommand.MailAddress)));
        }

        /// <summary>
        /// Tests that Execute calls IsLengthValid with the mail address on the common validations.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsIsLengthValidWithMailAddressOnCommonValidations()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var welcomeLetterDispatcherMock = MockRepository.GenerateMock<IWelcomeLetterDispatcher>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IHouseholdMember>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
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

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var householdMemberAddCommandHandler = new HouseholdMemberAddCommandHandler(householdDataRepositoryMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, welcomeLetterDispatcherMock, exceptionBuilderMock);
            Assert.That(householdMemberAddCommandHandler, Is.Not.Null);

            var householdMemberAddCommand = fixture.Build<HouseholdMemberAddCommand>()
                .With(m => m.MailAddress, string.Format("test.{0}@osdevgrp.dk", Guid.NewGuid().ToString("D")).ToLower())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdMemberAddCommandHandler.Execute(householdMemberAddCommand);

            commonValidationsMock.AssertWasCalled(m => m.IsLengthValid(Arg<string>.Is.Equal(householdMemberAddCommand.MailAddress), Arg<int>.Is.Equal(1), Arg<int>.Is.Equal(128)));
        }

        /// <summary>
        /// Tests that Execute calls ContainsIllegalChar with the mail address on the common validations.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsContainsIllegalCharWithMailAddressOnCommonValidations()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var welcomeLetterDispatcherMock = MockRepository.GenerateMock<IWelcomeLetterDispatcher>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IHouseholdMember>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
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

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var householdMemberAddCommandHandler = new HouseholdMemberAddCommandHandler(householdDataRepositoryMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, welcomeLetterDispatcherMock, exceptionBuilderMock);
            Assert.That(householdMemberAddCommandHandler, Is.Not.Null);

            var householdMemberAddCommand = fixture.Build<HouseholdMemberAddCommand>()
                .With(m => m.MailAddress, string.Format("test.{0}@osdevgrp.dk", Guid.NewGuid().ToString("D")).ToLower())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdMemberAddCommandHandler.Execute(householdMemberAddCommand);

            commonValidationsMock.AssertWasCalled(m => m.ContainsIllegalChar(Arg<string>.Is.Equal(householdMemberAddCommand.MailAddress)));
        }

        /// <summary>
        /// Tests that Execute calls IsMailAddress with the mail address on the common validations used by domain objects in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsIsMailAddressWithMailAddressOnDomainObjectValidations()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var welcomeLetterDispatcherMock = MockRepository.GenerateMock<IWelcomeLetterDispatcher>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IHouseholdMember>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
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

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var householdMemberAddCommandHandler = new HouseholdMemberAddCommandHandler(householdDataRepositoryMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, welcomeLetterDispatcherMock, exceptionBuilderMock);
            Assert.That(householdMemberAddCommandHandler, Is.Not.Null);

            var householdMemberAddCommand = fixture.Build<HouseholdMemberAddCommand>()
                .With(m => m.MailAddress, string.Format("test.{0}@osdevgrp.dk", Guid.NewGuid().ToString("D")).ToLower())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdMemberAddCommandHandler.Execute(householdMemberAddCommand);

            domainObjectValidationsMock.AssertWasCalled(m => m.IsMailAddress(Arg<string>.Is.Equal(householdMemberAddCommand.MailAddress)));
        }

        /// <summary>
        /// Tests that Execute calls IsSatisfiedBy on the specification which encapsulates validation rules 5 times.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsIsSatisfiedByOnSpecification5Times()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var welcomeLetterDispatcherMock = MockRepository.GenerateMock<IWelcomeLetterDispatcher>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IHouseholdMember>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var householdMemberAddCommandHandler = new HouseholdMemberAddCommandHandler(householdDataRepositoryMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, welcomeLetterDispatcherMock, exceptionBuilderMock);
            Assert.That(householdMemberAddCommandHandler, Is.Not.Null);

            var householdMemberAddCommand = fixture.Build<HouseholdMemberAddCommand>()
                .With(m => m.MailAddress, string.Format("test.{0}@osdevgrp.dk", Guid.NewGuid().ToString("D")).ToLower())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdMemberAddCommandHandler.Execute(householdMemberAddCommand);

            specificationMock.AssertWasCalled(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<IntranetBusinessException>.Is.TypeOf), opt => opt.Repeat.Times(5));
        }

        /// <summary>
        /// Tests that Execute calls Evaluate on the specification which encapsulates validation.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsEvaluateOnSpecification()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var welcomeLetterDispatcherMock = MockRepository.GenerateMock<IWelcomeLetterDispatcher>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IHouseholdMember>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var householdMemberAddCommandHandler = new HouseholdMemberAddCommandHandler(householdDataRepositoryMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, welcomeLetterDispatcherMock, exceptionBuilderMock);
            Assert.That(householdMemberAddCommandHandler, Is.Not.Null);

            var householdMemberAddCommand = fixture.Build<HouseholdMemberAddCommand>()
                .With(m => m.MailAddress, string.Format("test.{0}@osdevgrp.dk", Guid.NewGuid().ToString("D")).ToLower())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdMemberAddCommandHandler.Execute(householdMemberAddCommand);

            specificationMock.AssertWasCalled(m => m.Evaluate());
        }

        /// <summary>
        /// Tests that Execute calls Insert with a newly build household member on the repository which can access household data for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsInsertWithNewHouseholdMemberOnHouseholdDataRepository()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var welcomeLetterDispatcherMock = MockRepository.GenerateMock<IWelcomeLetterDispatcher>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var mailAddress = string.Format("test.{0}@osdevgrp.dk", Guid.NewGuid().ToString("D")).ToLower();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IHouseholdMember>.Is.NotNull))
                .WhenCalled(e =>
                {
                    var householdMember = (IHouseholdMember) e.Arguments.ElementAt(0);
                    Assert.That(householdMember, Is.Not.Null);
                    Assert.That(householdMember.Identifier, Is.Null);
                    Assert.That(householdMember.Identifier.HasValue, Is.False);
                    Assert.That(householdMember.MailAddress, Is.Not.Null);
                    Assert.That(householdMember.MailAddress, Is.Not.Empty);
                    Assert.That(householdMember.MailAddress, Is.EqualTo(mailAddress));
                    Assert.That(householdMember.Membership, Is.EqualTo(Membership.Basic));
                    Assert.That(householdMember.MembershipExpireTime, Is.Null);
                    Assert.That(householdMember.MembershipExpireTime.HasValue, Is.False);
                    Assert.That(householdMember.ActivationCode, Is.Not.Null);
                    Assert.That(householdMember.ActivationCode, Is.Not.Empty);
                    Assert.That(householdMember.ActivationTime, Is.Null);
                    Assert.That(householdMember.ActivationTime.HasValue, Is.False);
                    Assert.That(householdMember.IsActivated, Is.False);
                    Assert.That(householdMember.PrivacyPolicyAcceptedTime, Is.Null);
                    Assert.That(householdMember.PrivacyPolicyAcceptedTime.HasValue, Is.False);
                    Assert.That(householdMember.IsPrivacyPolictyAccepted, Is.False);
                    Assert.That(householdMember.CreationTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);
                    Assert.That(householdMember.Households, Is.Not.Null);
                    Assert.That(householdMember.Households, Is.Empty);
                    Assert.That(householdMember.Payments, Is.Not.Null);
                    Assert.That(householdMember.Payments, Is.Empty);
                })
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var householdMemberAddCommandHandler = new HouseholdMemberAddCommandHandler(householdDataRepositoryMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, welcomeLetterDispatcherMock, exceptionBuilderMock);
            Assert.That(householdMemberAddCommandHandler, Is.Not.Null);

            var householdMemberAddCommand = fixture.Build<HouseholdMemberAddCommand>()
                .With(m => m.MailAddress, mailAddress)
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdMemberAddCommandHandler.Execute(householdMemberAddCommand);

            householdDataRepositoryMock.AssertWasCalled(m => m.Insert(Arg<IHouseholdMember>.Is.NotNull));
        }

        /// <summary>
        /// Tests that Execute calls Dispatch on the dispatcher which can dispatch the welcome letter to a household member.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsDispatchOnWelcomeLetterDispatcher()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var welcomeLetterDispatcherMock = MockRepository.GenerateMock<IWelcomeLetterDispatcher>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var translationInfoMock = DomainObjectMockBuilder.BuildTranslationInfoMock();
            var insertedHouseholdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(translationInfoMock)
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IHouseholdMember>.Is.NotNull))
                .Return(insertedHouseholdMemberMock)
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var householdMemberAddCommandHandler = new HouseholdMemberAddCommandHandler(householdDataRepositoryMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, welcomeLetterDispatcherMock, exceptionBuilderMock);
            Assert.That(householdMemberAddCommandHandler, Is.Not.Null);

            var householdMemberAddCommand = fixture.Build<HouseholdMemberAddCommand>()
                .With(m => m.MailAddress, string.Format("test.{0}@osdevgrp.dk", Guid.NewGuid().ToString("D")).ToLower())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdMemberAddCommandHandler.Execute(householdMemberAddCommand);

            welcomeLetterDispatcherMock.AssertWasCalled(m => m.Dispatch(Arg<IStakeholder>.Is.Equal(insertedHouseholdMemberMock), Arg<IHouseholdMember>.Is.Equal(insertedHouseholdMemberMock), Arg<ITranslationInfo>.Is.Equal(translationInfoMock)));
        }

        /// <summary>
        /// Tests that Execute calls Map with the inserted household member on the object mapper which can map objects in the food waste domain..
        /// </summary>
        [Test]
        public void TestThatExecuteCallsMapWithInsertedHouseholdMemberOnFoodWasteObjectMapper()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var welcomeLetterDispatcherMock = MockRepository.GenerateMock<IWelcomeLetterDispatcher>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var translationInfoMock = DomainObjectMockBuilder.BuildTranslationInfoMock();
            var insertedHouseholdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(translationInfoMock)
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IHouseholdMember>.Is.NotNull))
                .Return(insertedHouseholdMemberMock)
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var householdMemberAddCommandHandler = new HouseholdMemberAddCommandHandler(householdDataRepositoryMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, welcomeLetterDispatcherMock, exceptionBuilderMock);
            Assert.That(householdMemberAddCommandHandler, Is.Not.Null);

            var householdMemberAddCommand = fixture.Build<HouseholdMemberAddCommand>()
                .With(m => m.MailAddress, string.Format("test.{0}@osdevgrp.dk", Guid.NewGuid().ToString("D")).ToLower())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdMemberAddCommandHandler.Execute(householdMemberAddCommand);

            objectMapperMock.AssertWasCalled(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Equal(insertedHouseholdMemberMock), Arg<CultureInfo>.Is.Equal(translationInfoMock.CultureInfo)));
        }

        /// <summary>
        /// Tests that Execute returns the results from Map on the object mapper which can map objects in the food waste domain..
        /// </summary>
        [Test]
        public void TestThatExecuteReturnsResultFromMapOnFoodWasteObjectMapper()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var welcomeLetterDispatcherMock = MockRepository.GenerateMock<IWelcomeLetterDispatcher>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IHouseholdMember>.Is.NotNull))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var serviceReceipt = fixture.Create<ServiceReceiptResponse>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(serviceReceipt)
                .Repeat.Any();

            var householdMemberAddCommandHandler = new HouseholdMemberAddCommandHandler(householdDataRepositoryMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, welcomeLetterDispatcherMock, exceptionBuilderMock);
            Assert.That(householdMemberAddCommandHandler, Is.Not.Null);

            var householdMemberAddCommand = fixture.Build<HouseholdMemberAddCommand>()
                .With(m => m.MailAddress, string.Format("test.{0}@osdevgrp.dk", Guid.NewGuid().ToString("D")).ToLower())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            var result = householdMemberAddCommandHandler.Execute(householdMemberAddCommand);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(serviceReceipt));
        }

        /// <summary>
        /// Tests that HandleException calls Build on the builder which can build exceptions.
        /// </summary>
        [Test]
        public void TestThatHandleExceptionCallsBuildOnExceptionBuilder()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var welcomeLetterDispatcherMock = MockRepository.GenerateMock<IWelcomeLetterDispatcher>();

            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();
            exceptionBuilderMock.Stub(m => m.Build(Arg<Exception>.Is.Anything, Arg<MethodBase>.Is.NotNull))
                .WhenCalled(e =>
                {
                    var methodBase = (MethodBase) e.Arguments.ElementAt(1);
                    Assert.That(methodBase, Is.Not.Null);
                    Assert.That(methodBase.ReflectedType.Name, Is.EqualTo(typeof (HouseholdMemberAddCommandHandler).Name));
                })
                .Return(fixture.Create<Exception>())
                .Repeat.Any();

            var householdMemberAddCommandHandler = new HouseholdMemberAddCommandHandler(householdDataRepositoryMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, welcomeLetterDispatcherMock, exceptionBuilderMock);
            Assert.That(householdMemberAddCommandHandler, Is.Not.Null);

            var exception = fixture.Create<Exception>();
            Assert.Throws<Exception>(() => householdMemberAddCommandHandler.HandleException(fixture.Create<HouseholdMemberAddCommand>(), exception));

            exceptionBuilderMock.AssertWasCalled(m => m.Build(Arg<Exception>.Is.Equal(exception), Arg<MethodBase>.Is.NotNull));
        }

        /// <summary>
        /// Tests that HandleException throws the created exception from the builder which can build exceptions.
        /// </summary>
        [Test]
        public void TestThatHandleExceptionThrowsCreatedExceptionFromExceptionBuilder()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var welcomeLetterDispatcherMock = MockRepository.GenerateMock<IWelcomeLetterDispatcher>();

            var exceptionToThrow = fixture.Create<Exception>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();
            exceptionBuilderMock.Stub(m => m.Build(Arg<Exception>.Is.Anything, Arg<MethodBase>.Is.Anything))
                .Return(exceptionToThrow)
                .Repeat.Any();

            var householdMemberAddCommandHandler = new HouseholdMemberAddCommandHandler(householdDataRepositoryMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, welcomeLetterDispatcherMock, exceptionBuilderMock);
            Assert.That(householdMemberAddCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<Exception>(() => householdMemberAddCommandHandler.HandleException(fixture.Create<HouseholdMemberAddCommand>(), fixture.Create<Exception>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception, Is.EqualTo(exceptionToThrow));
        }
    }
}
