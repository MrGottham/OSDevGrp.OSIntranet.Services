using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommandHandlers;
using OSDevGrp.OSIntranet.CommandHandlers.Core;
using OSDevGrp.OSIntranet.CommandHandlers.Validation;
using OSDevGrp.OSIntranet.Contracts.Commands;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;
using Ploeh.AutoFixture;
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
        /// Tests that Execute calls HasValue with the name for the household on the common validations.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsHasValueWithNameOnCommonValidations()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
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
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
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
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
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
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
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
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
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
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
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
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
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
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
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
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
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
        /// Tests that Execute calls IsSatisfiedBy on the specification which encapsulates validation rules 6 times.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsIsSatisfiedByOnSpecification6Times()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

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

            specificationMock.AssertWasCalled(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<IntranetBusinessException>.Is.TypeOf), opt => opt.Repeat.Times(6));
        }

        /// <summary>
        /// Tests that Execute calls Evaluate on the specification which encapsulates validation rules.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsEvaluateOnSpecification()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

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
