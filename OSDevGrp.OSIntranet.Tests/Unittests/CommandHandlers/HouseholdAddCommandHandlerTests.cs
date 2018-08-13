using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommandHandlers;
using OSDevGrp.OSIntranet.CommandHandlers.Core;
using OSDevGrp.OSIntranet.CommandHandlers.Validation;
using OSDevGrp.OSIntranet.Contracts.Commands;
using OSDevGrp.OSIntranet.Contracts.Responses;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
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
    /// Test the command handler which handles a command for adding a household to the current users household account.
    /// </summary>
    [TestFixture]
    public class HouseholdAddCommandHandlerTests
    {
        /// <summary>
        /// Tests that the constructor initialize a command handler which handles a command for adding a household to the current users household account.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeHouseholdAddCommandHandler()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdAddCommandHandler = new HouseholdAddCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddCommandHandler, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the constructor throw an ArgumentNullException when the provider which can resolve values from the current users claims is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenClaimValueProviderIsNull()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var exception = Assert.Throws<ArgumentNullException>(() => new HouseholdAddCommandHandler(householdDataRepositoryMock, null, objectMapperMock, specificationMock, commonValidationsMock, logicExecutorMock, exceptionBuilderMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("claimValueProvider"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor throw an ArgumentNullException when the provider which can resolve values from the current users claims is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenLogicExecutorIsNull()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var exception = Assert.Throws<ArgumentNullException>(() => new HouseholdAddCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, null, exceptionBuilderMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("logicExecutor"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Execute throws an ArgumentNullException when command which adds a household to the current users household account is null.
        /// </summary>
        [Test]
        public void TestThatExecuteThrowsArgumentNullExceptionWhenHouseholdAddCommandIsNull()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdAddCommandHandler = new HouseholdAddCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdAddCommandHandler.Execute(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("command"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Execute calls Get with the identifier of the translation informations which should be used in the translation on the repository which can access household data for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsGetWithTranslationInfoIdentifierOnHouseholdDataRepository()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IHousehold>.Is.NotNull))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdAddCommandHandler = new HouseholdAddCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddCommandHandler, Is.Not.Null);

            var translationInfoIdentifier = Guid.NewGuid();
            var householdAddCommand = fixture.Build<HouseholdAddCommand>()
                .With(m => m.Name, fixture.Create<string>())
                .With(m => m.Description, fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, translationInfoIdentifier)
                .Create();

            householdAddCommandHandler.Execute(householdAddCommand);

            householdDataRepositoryMock.AssertWasCalled(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Equal(translationInfoIdentifier)));
        }


        /// <summary>
        /// Tests that Execute calls IsNotNull with the translation informations which should be used in the translation on the common validations.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsIsNotNullWithTranslationInfoOnCommonValidations()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var translationInfoMock = DomainObjectMockBuilder.BuildTranslationInfoMock();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(translationInfoMock)
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
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

            var householdAddCommandHandler = new HouseholdAddCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddCommandHandler, Is.Not.Null);

            var name = fixture.Create<string>();
            var householdAddCommand = fixture.Build<HouseholdAddCommand>()
                .With(m => m.Name, name)
                .With(m => m.Description, fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdAddCommandHandler.Execute(householdAddCommand);

            commonValidationsMock.AssertWasCalled(m => m.IsNotNull(Arg<ITranslationInfo>.Is.Equal(translationInfoMock)));
        }

        /// <summary>
        /// Tests that Execute calls HasValue with the name for the household on the common validations.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsHasValueWithNameOnCommonValidations()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
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

            var householdAddCommandHandler = new HouseholdAddCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddCommandHandler, Is.Not.Null);

            var name = fixture.Create<string>();
            var householdAddCommand = fixture.Build<HouseholdAddCommand>()
                .With(m => m.Name, name)
                .With(m => m.Description, fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdAddCommandHandler.Execute(householdAddCommand);

            commonValidationsMock.AssertWasCalled(m => m.HasValue(Arg<string>.Is.Equal(name)));
        }

        /// <summary>
        /// Tests that Execute calls IsLengthValid with the name for the household on the common validations.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsIsLengthValidWithNameOnCommonValidations()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
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

            var householdAddCommandHandler = new HouseholdAddCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddCommandHandler, Is.Not.Null);

            var name = fixture.Create<string>();
            var householdAddCommand = fixture.Build<HouseholdAddCommand>()
                .With(m => m.Name, name)
                .With(m => m.Description, fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdAddCommandHandler.Execute(householdAddCommand);

            commonValidationsMock.AssertWasCalled(m => m.IsLengthValid(Arg<string>.Is.Equal(name), Arg<int>.Is.Equal(1), Arg<int>.Is.Equal(64)));
        }

        /// <summary>
        /// Tests that Execute calls ContainsIllegalChar with the name for the household on the common validations.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsContainsIllegalCharWithNameOnCommonValidations()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
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

            var householdAddCommandHandler = new HouseholdAddCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddCommandHandler, Is.Not.Null);

            var name = fixture.Create<string>();
            var householdAddCommand = fixture.Build<HouseholdAddCommand>()
                .With(m => m.Name, name)
                .With(m => m.Description, fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdAddCommandHandler.Execute(householdAddCommand);

            commonValidationsMock.AssertWasCalled(m => m.ContainsIllegalChar(Arg<string>.Is.Equal(name)));
        }

        /// <summary>
        /// Tests that Execute calls HasValue with the description for the household on the common validations when the description is not null.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsHasValueWithDescriptionOnCommonValidationsWhenDescriptionIsNotNull()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
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

            var householdAddCommandHandler = new HouseholdAddCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddCommandHandler, Is.Not.Null);

            var description = fixture.Create<string>();
            Assert.That(description, Is.Not.Null);

            var householdAddCommand = fixture.Build<HouseholdAddCommand>()
                .With(m => m.Name, fixture.Create<string>())
                .With(m => m.Description, description)
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdAddCommandHandler.Execute(householdAddCommand);

            commonValidationsMock.AssertWasCalled(m => m.HasValue(Arg<string>.Is.Equal(description)));
        }

        /// <summary>
        /// Tests that Execute calls IsLengthValid with the description for the household on the common validations when the description is not null.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsIsLengthValidWithDescriptionOnCommonValidationsWhenDescriptionIsNotNull()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
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

            var householdAddCommandHandler = new HouseholdAddCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddCommandHandler, Is.Not.Null);

            var description = fixture.Create<string>();
            Assert.That(description, Is.Not.Null);

            var householdAddCommand = fixture.Build<HouseholdAddCommand>()
                .With(m => m.Name, fixture.Create<string>())
                .With(m => m.Description, description)
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdAddCommandHandler.Execute(householdAddCommand);

            commonValidationsMock.AssertWasCalled(m => m.IsLengthValid(Arg<string>.Is.Equal(description), Arg<int>.Is.Equal(1), Arg<int>.Is.Equal(2048)));
        }

        /// <summary>
        /// Tests that Execute calls ContainsIllegalChar with the description for the household on the common validations when the description is not null.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsContainsIllegalCharWithDescriptionOnCommonValidationsWhenDescriptionIsNotNull()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
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

            var householdAddCommandHandler = new HouseholdAddCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddCommandHandler, Is.Not.Null);

            var description = fixture.Create<string>();
            Assert.That(description, Is.Not.Null);

            var householdAddCommand = fixture.Build<HouseholdAddCommand>()
                .With(m => m.Name, fixture.Create<string>())
                .With(m => m.Description, description)
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdAddCommandHandler.Execute(householdAddCommand);

            commonValidationsMock.AssertWasCalled(m => m.ContainsIllegalChar(Arg<string>.Is.Equal(description)));
        }

        /// <summary>
        /// Tests that Execute does not call HasValue with the description for the household on the common validations when the description is null.
        /// </summary>
        [Test]
        public void TestThatExecuteDoesNotCallHasValueWithDescriptionOnCommonValidationsWhenDescriptionIsNull()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
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

            var householdAddCommandHandler = new HouseholdAddCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddCommandHandler, Is.Not.Null);

            const string description = null;
            Assert.That(description, Is.Null);

            var householdAddCommand = fixture.Build<HouseholdAddCommand>()
                .With(m => m.Name, fixture.Create<string>())
                .With(m => m.Description, description)
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdAddCommandHandler.Execute(householdAddCommand);

            commonValidationsMock.AssertWasNotCalled(m => m.HasValue(Arg<string>.Is.Equal(description)));
        }

        /// <summary>
        /// Tests that Execute does not call IsLengthValid with the description for the household on the common validations when the description is null.
        /// </summary>
        [Test]
        public void TestThatExecuteDoesNotCallIsLengthValidWithDescriptionOnCommonValidationsWhenDescriptionIsNull()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
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

            var householdAddCommandHandler = new HouseholdAddCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddCommandHandler, Is.Not.Null);

            const string description = null;
            Assert.That(description, Is.Null);

            var householdAddCommand = fixture.Build<HouseholdAddCommand>()
                .With(m => m.Name, fixture.Create<string>())
                .With(m => m.Description, description)
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdAddCommandHandler.Execute(householdAddCommand);

            commonValidationsMock.AssertWasNotCalled(m => m.IsLengthValid(Arg<string>.Is.Equal(description), Arg<int>.Is.Anything, Arg<int>.Is.Anything));
        }

        /// <summary>
        /// Tests that Execute does not call ContainsIllegalChar with the description for the household on the common validations when the description is null.
        /// </summary>
        [Test]
        public void TestThatExecuteDoesNotCallContainsIllegalCharWithDescriptionOnCommonValidationsWhenDescriptionIsNull()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
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

            var householdAddCommandHandler = new HouseholdAddCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddCommandHandler, Is.Not.Null);

            const string description = null;
            Assert.That(description, Is.Null);

            var householdAddCommand = fixture.Build<HouseholdAddCommand>()
                .With(m => m.Name, fixture.Create<string>())
                .With(m => m.Description, description)
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdAddCommandHandler.Execute(householdAddCommand);

            commonValidationsMock.AssertWasNotCalled(m => m.ContainsIllegalChar(Arg<string>.Is.Equal(description)));
        }

        /// <summary>
        /// Tests that Execute calls IsSatisfiedBy on the specification which encapsulates validation rules 7 times.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsIsSatisfiedByOnSpecification7Times()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdAddCommandHandler = new HouseholdAddCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddCommandHandler, Is.Not.Null);

            var householdAddCommand = fixture.Build<HouseholdAddCommand>()
                .With(m => m.Name, fixture.Create<string>())
                .With(m => m.Description, fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdAddCommandHandler.Execute(householdAddCommand);

            specificationMock.AssertWasCalled(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<IntranetBusinessException>.Is.TypeOf), opt => opt.Repeat.Times(7));
        }

        /// <summary>
        /// Tests that Execute calls Evaluate on the specification which encapsulates validation rules.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsEvaluateOnSpecification()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdAddCommandHandler = new HouseholdAddCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddCommandHandler, Is.Not.Null);

            var householdAddCommand = fixture.Build<HouseholdAddCommand>()
                .With(m => m.Name, fixture.Create<string>())
                .With(m => m.Description, fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdAddCommandHandler.Execute(householdAddCommand);

            specificationMock.AssertWasCalled(m => m.Evaluate());
        }

        /// <summary>
        /// Tests that Execute calls Insert with a newly build household on the repository which can access household data for the food waste domain.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatExecuteCallsInsertWithNewHouseholdOnHouseholdDataRepository(bool hasDescription)
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var name = fixture.Create<string>();
            var description = hasDescription ? fixture.Create<string>() : null;
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IHousehold>.Is.NotNull))
                .WhenCalled(e =>
                {
                    var household = (IHousehold) e.Arguments.ElementAt(0);
                    Assert.That(household, Is.Not.Null);
                    Assert.That(household.Identifier, Is.Null);
                    Assert.That(household.Identifier.HasValue, Is.False);
                    Assert.That(household.Name, Is.Not.Null);
                    Assert.That(household.Name, Is.Not.Empty);
                    Assert.That(household.Name, Is.EqualTo(name));
                    if (hasDescription)
                    {
                        Assert.That(household.Description, Is.Not.Null);
                        Assert.That(household.Description, Is.Not.Empty);
                        Assert.That(household.Description, Is.EqualTo(description));
                    }
                    else
                    {
                        Assert.That(household.Description, Is.Null);
                    }
                    Assert.That(household.CreationTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);
                    Assert.That(household.HouseholdMembers, Is.Not.Null);
                    Assert.That(household.HouseholdMembers, Is.Empty);
                })
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdAddCommandHandler = new HouseholdAddCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddCommandHandler, Is.Not.Null);

            var householdAddCommand = fixture.Build<HouseholdAddCommand>()
                .With(m => m.Name, name)
                .With(m => m.Description, description)
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdAddCommandHandler.Execute(householdAddCommand);

            householdDataRepositoryMock.AssertWasCalled(m => m.Insert(Arg<IHousehold>.Is.NotNull));
        }

        /// <summary>
        /// Tests that Execute calls MailAddress on the provider which can resolve values from the current users claims.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsMailAddressOnClaimValueProvider()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdAddCommandHandler = new HouseholdAddCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddCommandHandler, Is.Not.Null);

            var householdAddCommand = fixture.Build<HouseholdAddCommand>()
                .With(m => m.Name, fixture.Create<string>())
                .With(m => m.Description, fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdAddCommandHandler.Execute(householdAddCommand);

            claimValueProviderMock.AssertWasCalled(m => m.MailAddress);
            householdDataRepositoryMock.AssertWasNotCalled(m => m.Delete(Arg<IHousehold>.Is.Anything));
        }

        /// <summary>
        /// Tests that Execute calls Delete with the inserted household on the repository which can access household data for the food waste domain when MailAddress on the provider which can resolve values from the current users claims throws an exception.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsDeleteWithInsertedHouseholdWhenMailAddressOnClaimValueProviderThrowsException()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var insertedHouseholdMock = DomainObjectMockBuilder.BuildHouseholdMock();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IHousehold>.Is.Anything))
                .Return(insertedHouseholdMock)
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Throw(fixture.Create<Exception>())
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdAddCommandHandler = new HouseholdAddCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddCommandHandler, Is.Not.Null);

            var householdAddCommand = fixture.Build<HouseholdAddCommand>()
                .With(m => m.Name, fixture.Create<string>())
                .With(m => m.Description, fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            Assert.Throws<Exception>(() => householdAddCommandHandler.Execute(householdAddCommand));

            claimValueProviderMock.AssertWasCalled(m => m.MailAddress);
            householdDataRepositoryMock.AssertWasCalled(m => m.Delete(Arg<IHousehold>.Is.Equal(insertedHouseholdMock)));
        }

        /// <summary>
        /// Tests that Execute calls HouseholdMemberGetByMailAddress with the current users mail address on the repository which can access household data for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsHouseholdMemberGetByMailAddressOnHouseholdDataRepository()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();

            var mailAddress = fixture.Create<string>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(mailAddress)
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdAddCommandHandler = new HouseholdAddCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddCommandHandler, Is.Not.Null);

            var householdAddCommand = fixture.Build<HouseholdAddCommand>()
                .With(m => m.Name, fixture.Create<string>())
                .With(m => m.Description, fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdAddCommandHandler.Execute(householdAddCommand);

            householdDataRepositoryMock.AssertWasCalled(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Equal(mailAddress)));
            householdDataRepositoryMock.AssertWasNotCalled(m => m.Delete(Arg<IHousehold>.Is.Anything));
        }

        /// <summary>
        /// Tests that Execute calls Delete with the inserted household on the repository which can access household data for the food waste domain when HouseholdMemberGetByMailAddress on the repository which can access household data for the food waste domain throws an exception.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsDeleteWithInsertedHouseholdWhenHouseholdMemberGetByMailAddressOnHouseholdDataRepositoryThrowsException()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var insertedHouseholdMock = DomainObjectMockBuilder.BuildHouseholdMock();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IHousehold>.Is.Anything))
                .Return(insertedHouseholdMock)
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Throw(fixture.Create<Exception>())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();

            var mailAddress = fixture.Create<string>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(mailAddress)
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdAddCommandHandler = new HouseholdAddCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddCommandHandler, Is.Not.Null);

            var householdAddCommand = fixture.Build<HouseholdAddCommand>()
                .With(m => m.Name, fixture.Create<string>())
                .With(m => m.Description, fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            Assert.Throws<Exception>(() => householdAddCommandHandler.Execute(householdAddCommand));

            householdDataRepositoryMock.AssertWasCalled(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Equal(mailAddress)));
            householdDataRepositoryMock.AssertWasCalled(m => m.Delete(Arg<IHousehold>.Is.Equal(insertedHouseholdMock)));
        }

        /// <summary>
        /// Tests that Execute calls HouseholdMemberAdd on the logic executor which can execute basic logic when HouseholdMemberGetByMailAddress on the repository which can access household data for the food waste domain does not return a household member.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsHouseholdMemberAddOnLogicExecutorWhenHouseholdMemberGetByMailAddressOnHouseholdDataRepositoryDoesNotReturnHouseholdMember()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var translationInfoMock = DomainObjectMockBuilder.BuildTranslationInfoMock();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(translationInfoMock)
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
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

            var mailAddress = fixture.Create<string>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(mailAddress)
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutorMock.Stub(m => m.HouseholdMemberAdd(Arg<string>.Is.Anything, Arg<Guid>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var householdAddCommandHandler = new HouseholdAddCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddCommandHandler, Is.Not.Null);

            var householdAddCommand = fixture.Build<HouseholdAddCommand>()
                .With(m => m.Name, fixture.Create<string>())
                .With(m => m.Description, fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdAddCommandHandler.Execute(householdAddCommand);

            logicExecutorMock.AssertWasCalled(m => m.HouseholdMemberAdd(Arg<string>.Is.Equal(mailAddress), Arg<Guid>.Is.Equal(translationInfoMock.Identifier)));
            householdDataRepositoryMock.AssertWasNotCalled(m => m.Delete(Arg<IHousehold>.Is.Anything));
        }

        /// <summary>
        /// Tests that Execute calls Delete with the inserted household on the repository which can access household data for the food waste domain when HouseholdMemberAdd on the logic executor which can execute basic logic throws an exception.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsDeleteWithInsertedHouseholdWhenHouseholdMemberAddOnLogicExecutorThrowsException()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var translationInfoMock = DomainObjectMockBuilder.BuildTranslationInfoMock();
            var insertedHouseholdMock = DomainObjectMockBuilder.BuildHouseholdMock();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(translationInfoMock)
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IHousehold>.Is.Anything))
                .Return(insertedHouseholdMock)
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

            var mailAddress = fixture.Create<string>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(mailAddress)
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutorMock.Stub(m => m.HouseholdMemberAdd(Arg<string>.Is.Anything, Arg<Guid>.Is.Anything))
                .Throw(fixture.Create<Exception>())
                .Repeat.Any();

            var householdAddCommandHandler = new HouseholdAddCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddCommandHandler, Is.Not.Null);

            var householdAddCommand = fixture.Build<HouseholdAddCommand>()
                .With(m => m.Name, fixture.Create<string>())
                .With(m => m.Description, fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            Assert.Throws<Exception>(() => householdAddCommandHandler.Execute(householdAddCommand));

            logicExecutorMock.AssertWasCalled(m => m.HouseholdMemberAdd(Arg<string>.Is.Equal(mailAddress), Arg<Guid>.Is.Equal(translationInfoMock.Identifier)));
            householdDataRepositoryMock.AssertWasCalled(m => m.Delete(Arg<IHousehold>.Is.Equal(insertedHouseholdMock)));
        }

        /// <summary>
        /// Tests that Execute does not call HouseholdMemberAdd on the logic executor which can execute basic logic when HouseholdMemberGetByMailAddress on the repository which can access household data for the food waste domain does return a household member.
        /// </summary>
        [Test]
        public void TestThatExecuteDoesNotCallHouseholdMemberAddOnLogicExecutorWhenHouseholdMemberGetByMailAddressOnHouseholdDataRepositoryDoesReturnHouseholdMember()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdAddCommandHandler = new HouseholdAddCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddCommandHandler, Is.Not.Null);

            var householdAddCommand = fixture.Build<HouseholdAddCommand>()
                .With(m => m.Name, fixture.Create<string>())
                .With(m => m.Description, fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdAddCommandHandler.Execute(householdAddCommand);

            logicExecutorMock.AssertWasNotCalled(m => m.HouseholdMemberAdd(Arg<string>.Is.Anything, Arg<Guid>.Is.Anything));
            householdDataRepositoryMock.AssertWasNotCalled(m => m.Delete(Arg<IHousehold>.Is.Anything));
        }

        /// <summary>
        /// Tests that Execute calls Get with the identifier for the newly created household member on the repository which can access household data for the food waste domain when HouseholdMemberGetByMailAddress on the repository which can access household data for the food waste domain does not return a household member.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsGetWithHouseholdMemberIdentifierForNewlyCreatedHouseholdMemberOnHouseholdDataRepositoryWhenHouseholdMemberGetByMailAddressOnHouseholdDataRepositoryDoesNotReturnHouseholdMember()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
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

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdMemberIdentifierForCreatedHouseholdMember = Guid.NewGuid();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutorMock.Stub(m => m.HouseholdMemberAdd(Arg<string>.Is.Anything, Arg<Guid>.Is.Anything))
                .Return(householdMemberIdentifierForCreatedHouseholdMember)
                .Repeat.Any();

            var householdAddCommandHandler = new HouseholdAddCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddCommandHandler, Is.Not.Null);

            var householdAddCommand = fixture.Build<HouseholdAddCommand>()
                .With(m => m.Name, fixture.Create<string>())
                .With(m => m.Description, fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdAddCommandHandler.Execute(householdAddCommand);

            householdDataRepositoryMock.AssertWasCalled(m => m.Get<IHouseholdMember>(Arg<Guid>.Is.Equal(householdMemberIdentifierForCreatedHouseholdMember)));
            householdDataRepositoryMock.AssertWasNotCalled(m => m.Delete(Arg<IHousehold>.Is.Anything));
        }

        /// <summary>
        /// Tests that Execute calls Delete with the inserted household on the repository which can access household data for the food waste domain when Get with the identifier for the newly created household member on the repository which can access household data for the food waste domain throws an exception.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsDeleteWithInsertedHouseholdWhenGetWithHouseholdMemberIdentifierForNewlyCreatedHouseholdMemberOnHouseholdDataRepositoryThrowsException()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var insertedHouseholdMock = DomainObjectMockBuilder.BuildHouseholdMock();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IHousehold>.Is.Anything))
                .Return(insertedHouseholdMock)
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(null)
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Get<IHouseholdMember>(Arg<Guid>.Is.Anything))
                .Throw(fixture.Create<Exception>())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdMemberIdentifierForCreatedHouseholdMember = Guid.NewGuid();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutorMock.Stub(m => m.HouseholdMemberAdd(Arg<string>.Is.Anything, Arg<Guid>.Is.Anything))
                .Return(householdMemberIdentifierForCreatedHouseholdMember)
                .Repeat.Any();

            var householdAddCommandHandler = new HouseholdAddCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddCommandHandler, Is.Not.Null);

            var householdAddCommand = fixture.Build<HouseholdAddCommand>()
                .With(m => m.Name, fixture.Create<string>())
                .With(m => m.Description, fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            Assert.Throws<Exception>(() => householdAddCommandHandler.Execute(householdAddCommand));

            householdDataRepositoryMock.AssertWasCalled(m => m.Get<IHouseholdMember>(Arg<Guid>.Is.Equal(householdMemberIdentifierForCreatedHouseholdMember)));
            householdDataRepositoryMock.AssertWasCalled(m => m.Delete(Arg<IHousehold>.Is.Equal(insertedHouseholdMock)));
        }

        /// <summary>
        /// Tests that Execute does not call Get for a household member on the repository which can access household data for the food waste domain when HouseholdMemberGetByMailAddress on the repository which can access household data for the food waste domain does return a household member.
        /// </summary>
        [Test]
        public void TestThatExecuteDoesNotCallGetForHouseholdMemberOnHouseholdDataRepositoryWhenHouseholdMemberGetByMailAddressOnHouseholdDataRepositoryDoesReturnHouseholdMember()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdAddCommandHandler = new HouseholdAddCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddCommandHandler, Is.Not.Null);

            var householdAddCommand = fixture.Build<HouseholdAddCommand>()
                .With(m => m.Name, fixture.Create<string>())
                .With(m => m.Description, fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdAddCommandHandler.Execute(householdAddCommand);

            householdDataRepositoryMock.AssertWasNotCalled(m => m.Get<IHouseholdMember>(Arg<Guid>.Is.Anything));
            householdDataRepositoryMock.AssertWasNotCalled(m => m.Delete(Arg<IHousehold>.Is.Anything));
        }

        /// <summary>
        /// Tests that Execute calls HouseholdMemberAdd with the existing household member on the inserted household when HouseholdMemberGetByMailAddress on the repository which can access household data for the food waste domain does return a household member.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsHouseholdMemberAddWithExistingHouseholdMemberOnInsertedHouseholdWhenHouseholdMemberGetByMailAddressOnHouseholdDataRepositoryDoesReturnHouseholdMember()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var insertedHouseholdMock = DomainObjectMockBuilder.BuildHouseholdMock();
            var existingHouseholdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IHousehold>.Is.Anything))
                .Return(insertedHouseholdMock)
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(existingHouseholdMemberMock)
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdAddCommandHandler = new HouseholdAddCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddCommandHandler, Is.Not.Null);

            var householdAddCommand = fixture.Build<HouseholdAddCommand>()
                .With(m => m.Name, fixture.Create<string>())
                .With(m => m.Description, fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdAddCommandHandler.Execute(householdAddCommand);

            insertedHouseholdMock.AssertWasCalled(m => m.HouseholdMemberAdd(Arg<IHouseholdMember>.Is.Equal(existingHouseholdMemberMock)));
            householdDataRepositoryMock.AssertWasNotCalled(m => m.Delete(Arg<IHousehold>.Is.Anything));
        }

        /// <summary>
        /// Tests that Execute calls HouseholdMemberAdd with the inserted household member on the inserted household when HouseholdMemberGetByMailAddress on the repository which can access household data for the food waste domain does not return a household member.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsHouseholdMemberAddWithInsertedHouseholdMemberOnInsertedHouseholdWhenHouseholdMemberGetByMailAddressOnHouseholdDataRepositoryDoesNotReturnHouseholdMember()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var insertedHouseholdMock = DomainObjectMockBuilder.BuildHouseholdMock();
            var insertedHouseholdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IHousehold>.Is.Anything))
                .Return(insertedHouseholdMock)
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(null)
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Get<IHouseholdMember>(Arg<Guid>.Is.Anything))
                .Return(insertedHouseholdMemberMock)
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutorMock.Stub(m => m.HouseholdMemberAdd(Arg<string>.Is.Anything, Arg<Guid>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var householdAddCommandHandler = new HouseholdAddCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddCommandHandler, Is.Not.Null);

            var householdAddCommand = fixture.Build<HouseholdAddCommand>()
                .With(m => m.Name, fixture.Create<string>())
                .With(m => m.Description, fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdAddCommandHandler.Execute(householdAddCommand);

            insertedHouseholdMock.AssertWasCalled(m => m.HouseholdMemberAdd(Arg<IHouseholdMember>.Is.Equal(insertedHouseholdMemberMock)));
            householdDataRepositoryMock.AssertWasNotCalled(m => m.Delete(Arg<IHousehold>.Is.Anything));
        }

        /// <summary>
        /// Tests that Execute calls Delete with the inserted household on the repository which can access household data for the food waste domain when HouseholdMemberAdd on the inserted household throws an exception.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsDeleteWithInsertedHouseholdWhenHouseholdMemberAddOnInsertedHouseholdThrowsException()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var insertedHouseholdMock = DomainObjectMockBuilder.BuildHouseholdMock();
            insertedHouseholdMock.Stub(m => m.HouseholdMemberAdd(Arg<IHouseholdMember>.Is.Anything))
                .Throw(fixture.Create<Exception>())
                .Repeat.Any();

            var existingHouseholdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IHousehold>.Is.Anything))
                .Return(insertedHouseholdMock)
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(existingHouseholdMemberMock)
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdAddCommandHandler = new HouseholdAddCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddCommandHandler, Is.Not.Null);

            var householdAddCommand = fixture.Build<HouseholdAddCommand>()
                .With(m => m.Name, fixture.Create<string>())
                .With(m => m.Description, fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            Assert.Throws<Exception>(() => householdAddCommandHandler.Execute(householdAddCommand));

            insertedHouseholdMock.AssertWasCalled(m => m.HouseholdMemberAdd(Arg<IHouseholdMember>.Is.Equal(existingHouseholdMemberMock)));
            householdDataRepositoryMock.AssertWasCalled(m => m.Delete(Arg<IHousehold>.Is.Equal(insertedHouseholdMock)));
        }

        /// <summary>
        /// Tests that Execute calls Update with the inserted household on the repository which can access household data for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsUpdateWithInsertedHouseholdOnHouseholdDataRepository()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var insertedHouseholdMock = DomainObjectMockBuilder.BuildHouseholdMock();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IHousehold>.Is.Anything))
                .Return(insertedHouseholdMock)
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdAddCommandHandler = new HouseholdAddCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddCommandHandler, Is.Not.Null);

            var householdAddCommand = fixture.Build<HouseholdAddCommand>()
                .With(m => m.Name, fixture.Create<string>())
                .With(m => m.Description, fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdAddCommandHandler.Execute(householdAddCommand);

            householdDataRepositoryMock.AssertWasCalled(m => m.Update(Arg<IHousehold>.Is.Equal(insertedHouseholdMock)));
            householdDataRepositoryMock.AssertWasNotCalled(m => m.Delete(Arg<IHousehold>.Is.Anything));
        }

        /// <summary>
        /// Tests that Execute calls Delete with the inserted household on the repository which can access household data for the food waste domain when Update on the repository which can access household data for the food waste domain throws an exception.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsDeleteWithInsertedHouseholdWhenUpdateForInsertedHouseholdMemberOnHouseholdDataRepositoryThrowsException()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var insertedHouseholdMock = DomainObjectMockBuilder.BuildHouseholdMock();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IHousehold>.Is.Anything))
                .Return(insertedHouseholdMock)
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Throw(fixture.Create<Exception>())
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdAddCommandHandler = new HouseholdAddCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddCommandHandler, Is.Not.Null);

            var householdAddCommand = fixture.Build<HouseholdAddCommand>()
                .With(m => m.Name, fixture.Create<string>())
                .With(m => m.Description, fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            Assert.Throws<Exception>(() => householdAddCommandHandler.Execute(householdAddCommand));

            householdDataRepositoryMock.AssertWasCalled(m => m.Update(Arg<IHousehold>.Is.Equal(insertedHouseholdMock)));
            householdDataRepositoryMock.AssertWasCalled(m => m.Delete(Arg<IHousehold>.Is.Equal(insertedHouseholdMock)));
        }

        /// <summary>
        /// Tests that Execute calls Map with the updated household on the object mapper which can map objects in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsMapWithUpdatedHouseholdOnFoodWasteObjectMapper()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var translationInfoMock = DomainObjectMockBuilder.BuildTranslationInfoMock();
            var updatedHouseholdMock = DomainObjectMockBuilder.BuildHouseholdMock();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(translationInfoMock)
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(updatedHouseholdMock)
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdAddCommandHandler = new HouseholdAddCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddCommandHandler, Is.Not.Null);

            var householdAddCommand = fixture.Build<HouseholdAddCommand>()
                .With(m => m.Name, fixture.Create<string>())
                .With(m => m.Description, fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdAddCommandHandler.Execute(householdAddCommand);

            objectMapperMock.AssertWasCalled(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Equal(updatedHouseholdMock), Arg<CultureInfo>.Is.Equal(translationInfoMock.CultureInfo)));
            householdDataRepositoryMock.AssertWasNotCalled(m => m.Delete(Arg<IHousehold>.Is.Anything));
        }

        /// <summary>
        /// Tests that Execute calls Delete with the inserted household on the repository which can access household data for the food waste domain when Map on the object mapper which can map objects in the food waste domain throws an exception.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsDeleteWithInsertedHouseholdWhenMapForUpdatedHouseholdOnFoodWasteObjectMapperThrowsException()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var translationInfoMock = DomainObjectMockBuilder.BuildTranslationInfoMock();
            var insertedHouseholdMock = DomainObjectMockBuilder.BuildHouseholdMock();
            var updatedHouseholdMock = DomainObjectMockBuilder.BuildHouseholdMock();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(translationInfoMock)
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IHousehold>.Is.Anything))
                .Return(insertedHouseholdMock)
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(updatedHouseholdMock)
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Throw(fixture.Create<Exception>())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdAddCommandHandler = new HouseholdAddCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddCommandHandler, Is.Not.Null);

            var householdAddCommand = fixture.Build<HouseholdAddCommand>()
                .With(m => m.Name, fixture.Create<string>())
                .With(m => m.Description, fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            Assert.Throws<Exception>(() => householdAddCommandHandler.Execute(householdAddCommand));

            objectMapperMock.AssertWasCalled(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Equal(updatedHouseholdMock), Arg<CultureInfo>.Is.Equal(translationInfoMock.CultureInfo)));
            householdDataRepositoryMock.AssertWasCalled(m => m.Delete(Arg<IHousehold>.Is.Equal(insertedHouseholdMock)));
        }

        /// <summary>
        /// Tests that Execute returns the results from Map on the object mapper which can map objects in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatExecuteReturnsResultFromMapOnFoodWasteObjectMapper()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var serviceReceipt = fixture.Create<ServiceReceiptResponse>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(serviceReceipt)
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdAddCommandHandler = new HouseholdAddCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddCommandHandler, Is.Not.Null);

            var householdAddCommand = fixture.Build<HouseholdAddCommand>()
                .With(m => m.Name, fixture.Create<string>())
                .With(m => m.Description, fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            var result = householdAddCommandHandler.Execute(householdAddCommand);
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
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();

            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();
            exceptionBuilderMock.Stub(m => m.Build(Arg<Exception>.Is.Anything, Arg<MethodBase>.Is.NotNull))
                .WhenCalled(e =>
                {
                    var methodBase = (MethodBase)e.Arguments.ElementAt(1);
                    Assert.That(methodBase, Is.Not.Null);
                    Assert.That(methodBase.ReflectedType.Name, Is.EqualTo(typeof(HouseholdAddCommandHandler).Name));
                })
                .Return(fixture.Create<Exception>())
                .Repeat.Any();

            var householdAddCommandHandler = new HouseholdAddCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddCommandHandler, Is.Not.Null);

            var exception = fixture.Create<Exception>();
            Assert.Throws<Exception>(() => householdAddCommandHandler.HandleException(fixture.Create<HouseholdAddCommand>(), exception));

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
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();

            var exceptionToThrow = fixture.Create<Exception>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();
            exceptionBuilderMock.Stub(m => m.Build(Arg<Exception>.Is.Anything, Arg<MethodBase>.Is.Anything))
                .Return(exceptionToThrow)
                .Repeat.Any();

            var householdAddCommandHandler = new HouseholdAddCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, logicExecutorMock, exceptionBuilderMock);
            Assert.That(householdAddCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<Exception>(() => householdAddCommandHandler.HandleException(fixture.Create<HouseholdAddCommand>(), fixture.Create<Exception>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception, Is.EqualTo(exceptionToThrow));
        }
    }
}
