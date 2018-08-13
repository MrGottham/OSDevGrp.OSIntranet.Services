using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommandHandlers;
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
    /// Tests the command handler which handles a command for modifying a translation.
    /// </summary>
    [TestFixture]
    public class TranslationModifyCommandHandlerTests
    {
        /// <summary>
        /// Tests that the constructor initialize a command handler which handles a command for modifying a translation.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeTranslationModifyCommandHandler()
        {
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var translationModifyCommandHandler = new TranslationModifyCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(translationModifyCommandHandler, Is.Not.Null);
        }

        /// <summary>
        /// Tests that Execute throws an ArgumentNullException if the command for modifying a translation is null.
        /// </summary>
        [Test]
        public void TestThatExecuteThrowsArgumentNullExceptionIfTranslationModifyCommandIsNull()
        {
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var translationModifyCommandHandler = new TranslationModifyCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(translationModifyCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => translationModifyCommandHandler.Execute(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("command"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Execute calls Get to get the translation on the repository which can access system data for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsGetForTranslationOnSystemDataRepository()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslation>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationMock(Guid.NewGuid()))
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Update(Arg<ITranslation>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationMock(Guid.NewGuid()))
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var translationModifyCommand = fixture.Build<TranslationModifyCommand>()
                .With(m => m.TranslationIdentifier, Guid.NewGuid())
                .With(m => m.TranslationValue, fixture.Create<string>())
                .Create();

            var translationModifyCommandHandler = new TranslationModifyCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(translationModifyCommandHandler, Is.Not.Null);

            translationModifyCommandHandler.Execute(translationModifyCommand);

            systemDataRepositoryMock.AssertWasCalled(m => m.Get<ITranslation>(Arg<Guid>.Is.Equal(translationModifyCommand.TranslationIdentifier)));
        }

        /// <summary>
        /// Test that Execute calls IsNotNull for the translation on the common validations.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsIsNotNullForTranslationOnCommonValidations()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var translationMock = DomainObjectMockBuilder.BuildTranslationMock(Guid.NewGuid());
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslation>(Arg<Guid>.Is.Anything))
                .Return(translationMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Update(Arg<ITranslation>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationMock(Guid.NewGuid()))
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

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var translationModifyCommand = fixture.Build<TranslationModifyCommand>()
                .With(m => m.TranslationIdentifier, Guid.NewGuid())
                .With(m => m.TranslationValue, fixture.Create<string>())
                .Create();

            var translationModifyCommandHandler = new TranslationModifyCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(translationModifyCommandHandler, Is.Not.Null);

            translationModifyCommandHandler.Execute(translationModifyCommand);

            commonValidationsMock.AssertWasCalled(m => m.IsNotNull(Arg<ITranslation>.Is.Equal(translationMock)));
        }

        /// <summary>
        /// Test that Execute calls HasValue for the translation value on the common validations.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsHasValueForTranslationValueOnCommonValidations()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslation>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationMock(Guid.NewGuid()))
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Update(Arg<ITranslation>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationMock(Guid.NewGuid()))
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

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var translationModifyCommand = fixture.Build<TranslationModifyCommand>()
                .With(m => m.TranslationIdentifier, Guid.NewGuid())
                .With(m => m.TranslationValue, fixture.Create<string>())
                .Create();

            var translationModifyCommandHandler = new TranslationModifyCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(translationModifyCommandHandler, Is.Not.Null);

            translationModifyCommandHandler.Execute(translationModifyCommand);

            commonValidationsMock.AssertWasCalled(m => m.HasValue(Arg<string>.Is.Equal(translationModifyCommand.TranslationValue)));
        }

        /// <summary>
        /// Test that Execute calls ContainsIllegalChar for the translation value on the common validations.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsContainsIllegalCharForTranslationValueOnCommonValidations()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslation>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationMock(Guid.NewGuid()))
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Update(Arg<ITranslation>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationMock(Guid.NewGuid()))
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

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var translationModifyCommand = fixture.Build<TranslationModifyCommand>()
                .With(m => m.TranslationIdentifier, Guid.NewGuid())
                .With(m => m.TranslationValue, fixture.Create<string>())
                .Create();

            var translationModifyCommandHandler = new TranslationModifyCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(translationModifyCommandHandler, Is.Not.Null);

            translationModifyCommandHandler.Execute(translationModifyCommand);

            commonValidationsMock.AssertWasCalled(m => m.ContainsIllegalChar(Arg<string>.Is.Equal(translationModifyCommand.TranslationValue)));
        }

        /// <summary>
        /// Test that Execute calls IsSatisfiedBy on the specification which encapsulates validation rules 3 times.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsIsSatisfiedByOnSpecification3Times()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslation>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationMock(Guid.NewGuid()))
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Update(Arg<ITranslation>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationMock(Guid.NewGuid()))
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var translationModifyCommand = fixture.Build<TranslationModifyCommand>()
                .With(m => m.TranslationIdentifier, Guid.NewGuid())
                .With(m => m.TranslationValue, fixture.Create<string>())
                .Create();

            var translationModifyCommandHandler = new TranslationModifyCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(translationModifyCommandHandler, Is.Not.Null);

            translationModifyCommandHandler.Execute(translationModifyCommand);

            specificationMock.AssertWasCalled(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<IntranetBusinessException>.Is.TypeOf), opt => opt.Repeat.Times(3));
        }

        /// <summary>
        /// Test that Execute calls Evaluate on the specification which encapsulates validation rules.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsEvaluateOnSpecification()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslation>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationMock(Guid.NewGuid()))
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Update(Arg<ITranslation>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationMock(Guid.NewGuid()))
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var translationModifyCommand = fixture.Build<TranslationModifyCommand>()
                .With(m => m.TranslationIdentifier, Guid.NewGuid())
                .With(m => m.TranslationValue, fixture.Create<string>())
                .Create();

            var translationModifyCommandHandler = new TranslationModifyCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(translationModifyCommandHandler, Is.Not.Null);

            translationModifyCommandHandler.Execute(translationModifyCommand);

            specificationMock.AssertWasCalled(m => m.Evaluate());
        }

        /// <summary>
        /// Test that Execute sets the value for Value with the translation value from the command.
        /// </summary>
        [Test]
        public void TestThatExecuteSetsValueWithTranslationValueFromCommand()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var translationMock = DomainObjectMockBuilder.BuildTranslationMock(Guid.NewGuid());
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslation>(Arg<Guid>.Is.Anything))
                .Return(translationMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Update(Arg<ITranslation>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationMock(Guid.NewGuid()))
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var translationModifyCommand = fixture.Build<TranslationModifyCommand>()
                .With(m => m.TranslationIdentifier, Guid.NewGuid())
                .With(m => m.TranslationValue, fixture.Create<string>())
                .Create();

            var translationModifyCommandHandler = new TranslationModifyCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(translationModifyCommandHandler, Is.Not.Null);

            translationModifyCommandHandler.Execute(translationModifyCommand);

            translationMock.AssertWasCalled(m => m.Value = Arg<string>.Is.Equal(translationModifyCommand.TranslationValue));
        }

        /// <summary>
        /// Test that Execute calls Update on the repository which can access system data for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsUpdateOnSystemDataRepository()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var translationMock = DomainObjectMockBuilder.BuildTranslationMock(Guid.NewGuid());
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslation>(Arg<Guid>.Is.Anything))
                .Return(translationMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Update(Arg<ITranslation>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationMock(Guid.NewGuid()))
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var translationModifyCommand = fixture.Build<TranslationModifyCommand>()
                .With(m => m.TranslationIdentifier, Guid.NewGuid())
                .With(m => m.TranslationValue, fixture.Create<string>())
                .Create();

            var translationModifyCommandHandler = new TranslationModifyCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(translationModifyCommandHandler, Is.Not.Null);

            translationModifyCommandHandler.Execute(translationModifyCommand);

            systemDataRepositoryMock.AssertWasCalled(m => m.Update(Arg<ITranslation>.Is.Equal(translationMock)));
        }

        /// <summary>
        /// Test that Execute calls Map for the updated translation on the object mapper for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsMapForUpdatedTranslationOnFoodWasteObjectMapper()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var updatedTranslationMock = DomainObjectMockBuilder.BuildTranslationMock(Guid.NewGuid());
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslation>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationMock(Guid.NewGuid()))
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Update(Arg<ITranslation>.Is.Anything))
                .Return(updatedTranslationMock)
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var translationModifyCommand = fixture.Build<TranslationModifyCommand>()
                .With(m => m.TranslationIdentifier, Guid.NewGuid())
                .With(m => m.TranslationValue, fixture.Create<string>())
                .Create();

            var translationModifyCommandHandler = new TranslationModifyCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(translationModifyCommandHandler, Is.Not.Null);

            translationModifyCommandHandler.Execute(translationModifyCommand);

            foodWasteObjectMapperMock.AssertWasCalled(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Equal(updatedTranslationMock), Arg<CultureInfo>.Is.Null));
        }

        /// <summary>
        /// Test that Execute returns the service receipt created Map for the inserted translation on the object mapper for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatExecuteReturnsServiceReceiptFromMapOnFoodWasteObjectMapper()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslation>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationMock(Guid.NewGuid()))
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Update(Arg<ITranslation>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationMock(Guid.NewGuid()))
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var serviceReceipt = fixture.Build<ServiceReceiptResponse>()
                .With(m => m.Identifier, Guid.NewGuid())
                .With(m => m.EventDate, DateTime.Now)
                .Create();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(serviceReceipt)
                .Repeat.Any();

            var translationModifyCommand = fixture.Build<TranslationModifyCommand>()
                .With(m => m.TranslationIdentifier, Guid.NewGuid())
                .With(m => m.TranslationValue, fixture.Create<string>())
                .Create();

            var translationModifyCommandHandler = new TranslationModifyCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(translationModifyCommandHandler, Is.Not.Null);

            var result = translationModifyCommandHandler.Execute(translationModifyCommand);
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
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();
            exceptionBuilderMock.Stub(m => m.Build(Arg<Exception>.Is.Anything, Arg<MethodBase>.Is.NotNull))
                .WhenCalled(e =>
                {
                    var methodBase = (MethodBase) e.Arguments.ElementAt(1);
                    Assert.That(methodBase, Is.Not.Null);
                    Assert.That(methodBase.ReflectedType.Name, Is.EqualTo(typeof (TranslationModifyCommandHandler).Name));
                })
                .Return(fixture.Create<Exception>())
                .Repeat.Any();

            var translationModifyCommandHandler = new TranslationModifyCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(translationModifyCommandHandler, Is.Not.Null);

            var exception = fixture.Create<Exception>();
            Assert.Throws<Exception>(() => translationModifyCommandHandler.HandleException(fixture.Create<TranslationModifyCommand>(), exception));

            exceptionBuilderMock.AssertWasCalled(m => m.Build(Arg<Exception>.Is.Equal(exception), Arg<MethodBase>.Is.NotNull));
        }

        /// <summary>
        /// Tests that HandleException throws the created exception from the builder which can build exceptions.
        /// </summary>
        [Test]
        public void TestThatHandleExceptionThrowsCreatedExceptionFromExceptionBuilder()
        {
            var fixture = new Fixture();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var exceptionToThrow = fixture.Create<Exception>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();
            exceptionBuilderMock.Stub(m => m.Build(Arg<Exception>.Is.Anything, Arg<MethodBase>.Is.Anything))
                .Return(exceptionToThrow)
                .Repeat.Any();

            var translationModifyCommandHandler = new TranslationModifyCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(translationModifyCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<Exception>(() => translationModifyCommandHandler.HandleException(fixture.Create<TranslationModifyCommand>(), fixture.Create<Exception>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception, Is.EqualTo(exceptionToThrow));
        }
    }
}
