using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommandHandlers.Core;
using OSDevGrp.OSIntranet.CommandHandlers.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;
using AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.CommandHandlers.Core
{
    /// <summary>
    /// Tests the basic functionality for command handlers which handles commands for system data in the food waste domain.
    /// </summary>
    [TestFixture]
    public class FoodWasteSystemDataCommandHandlerBaseTests
    {
        /// <summary>
        /// Private class for testing the basic functionality for command handlers which handles commands for system data in the food waste domain.
        /// </summary>
        private class MyFoodWasteSystemDataCommandHandler : FoodWasteSystemDataCommandHandlerBase
        {
            #region Constructor

            /// <summary>
            /// Creates a private class for testing the basic functionality for command handlers which handles commands for system data in the food waste domain.
            /// </summary>
            /// <param name="systemDataRepository">Implementation of the repository which can access system data for the food waste domain.</param>
            /// <param name="foodWasteObjectMapper">Implementation of an object mapper which can map objects in the food waste domain.</param>
            /// <param name="specification">Implementation of a specification which encapsulates validation rules.</param>
            /// <param name="commonValidations">Implementation of the common validations.</param>
            /// <param name="exceptionBuilder">Implementation of the builder which can build exceptions.</param>
            public MyFoodWasteSystemDataCommandHandler(ISystemDataRepository systemDataRepository, IFoodWasteObjectMapper foodWasteObjectMapper, ISpecification specification, ICommonValidations commonValidations, IExceptionBuilder exceptionBuilder)
                : base(systemDataRepository, foodWasteObjectMapper, specification, commonValidations, exceptionBuilder)
            {
            }

            #endregion

            #region Methods

            /// <summary>
            /// Gets the repository which can access system data for the food waste domain.
            /// </summary>
            public ISystemDataRepository GetSystemDataRepository()
            {
                return SystemDataRepository;
            }

            /// <summary>
            /// Gets the object mapper which can map objects in the food waste domain.
            /// </summary>
            public IFoodWasteObjectMapper GetObjectMapper()
            {
                return ObjectMapper;
            }

            /// <summary>
            /// Gets the specification which encapsulates validation rules.
            /// </summary>
            public ISpecification GetSpecification()
            {
                return Specification;
            }

            /// <summary>
            /// Gets the common validations.
            /// </summary>
            public ICommonValidations GetCommonValidations()
            {
                return CommonValidations;
            }

            /// <summary>
            /// Gets the builder which can build exceptions.
            /// </summary>
            public IExceptionBuilder GetExceptionBuilder()
            {
                return ExceptionBuilder;
            }

            /// <summary>
            /// Imports a given translation on a given translatable domain object.
            /// </summary>
            /// <param name="domainObject">Translatable domain object on which to import the translation.</param>
            /// <param name="translationInfo">Translation informations for the translation to import.</param>
            /// <param name="translationValue">The translation value for the translatable domain object.</param>
            /// <param name="logicExecutor">Implementation of the logic executor which can execute basic logic.</param>
            /// <returns>The imported translation.</returns>
            public new ITranslation ImportTranslation(ITranslatable domainObject, ITranslationInfo translationInfo, string translationValue, ILogicExecutor logicExecutor)
            {
                return base.ImportTranslation(domainObject, translationInfo, translationValue, logicExecutor);
            }

            #endregion
        }

        /// <summary>
        /// Tests that the constructor initialize the basic functionality for command handlers which handles commands for system data in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeFoodWasteSystemDataCommandHandler()
        {
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var systemDataCommandHandlerBase = new MyFoodWasteSystemDataCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(systemDataCommandHandlerBase, Is.Not.Null);
            Assert.That(systemDataCommandHandlerBase.GetSystemDataRepository(), Is.Not.Null);
            Assert.That(systemDataCommandHandlerBase.GetSystemDataRepository(), Is.EqualTo(systemDataRepositoryMock));
            Assert.That(systemDataCommandHandlerBase.GetObjectMapper(), Is.Not.Null);
            Assert.That(systemDataCommandHandlerBase.GetObjectMapper(), Is.EqualTo(foodWasteObjectMapperMock));
            Assert.That(systemDataCommandHandlerBase.GetSpecification(), Is.Not.Null);
            Assert.That(systemDataCommandHandlerBase.GetSpecification(), Is.EqualTo(specificationMock));
            Assert.That(systemDataCommandHandlerBase.GetCommonValidations(), Is.Not.Null);
            Assert.That(systemDataCommandHandlerBase.GetCommonValidations(), Is.EqualTo(commonValidationsMock));
            Assert.That(systemDataCommandHandlerBase.GetExceptionBuilder(), Is.Not.Null);
            Assert.That(systemDataCommandHandlerBase.GetExceptionBuilder(), Is.EqualTo(exceptionBuilderMock));
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the repository which can access system data for the food waste domain is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenSystemDataRepositoryIsNull()
        {
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyFoodWasteSystemDataCommandHandler(null, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("systemDataRepository"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the object mapper which can map objects in the food waste domain is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenFoodWasteObjectMapperIsNull()
        {
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyFoodWasteSystemDataCommandHandler(systemDataRepositoryMock, null, specificationMock, commonValidationsMock, exceptionBuilderMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("foodWasteObjectMapper"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the specification which encapsulates validation rules is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenSpecificationIsNull()
        {
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyFoodWasteSystemDataCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, null, commonValidationsMock, exceptionBuilderMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("specification"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the common validations is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenCommonValidationsIsNull()
        {
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyFoodWasteSystemDataCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, null, exceptionBuilderMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("commonValidations"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the builder which can build exceptions is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenExceptionBuilderIsNull()
        {
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyFoodWasteSystemDataCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("exceptionBuilder"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that ImportTranslation throws an ArgumentNullException when the translatable domain object is null.
        /// </summary>
        [Test]
        public void TestThatImportTranslationThrowsArgumentNullExceptionWhenDomainObjectIsNull()
        {
            var fixture = new Fixture();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var commandHandler = new MyFoodWasteSystemDataCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => commandHandler.ImportTranslation(null, MockRepository.GenerateMock<ITranslationInfo>(), fixture.Create<string>(), MockRepository.GenerateMock<ILogicExecutor>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("domainObject"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that ImportTranslation throws an ArgumentNullException when the translation informations for the translation to import is null.
        /// </summary>
        [Test]
        public void TestThatImportTranslationThrowsArgumentNullExceptionWhenTranslationInfoIsNull()
        {
            var fixture = new Fixture();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var commandHandler = new MyFoodWasteSystemDataCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => commandHandler.ImportTranslation(MockRepository.GenerateMock<ITranslatable>(), null, fixture.Create<string>(), MockRepository.GenerateMock<ILogicExecutor>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("translationInfo"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that ImportTranslation throws an ArgumentNullException when the translation value for the translatable domain object is invalid.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestThatImportTranslationThrowsArgumentNullExceptionWhentranslationValueIsInvalid(string invalidValue)
        {
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var commandHandler = new MyFoodWasteSystemDataCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => commandHandler.ImportTranslation(MockRepository.GenerateMock<ITranslatable>(), MockRepository.GenerateMock<ITranslationInfo>(), invalidValue, MockRepository.GenerateMock<ILogicExecutor>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("translationValue"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that ImportTranslation throws an ArgumentNullException when the translation value for the translatable domain object is null.
        /// </summary>
        [Test]
        public void TestThatImportTranslationThrowsArgumentNullExceptionWhenLogicExecutorIsNull()
        {
            var fixture = new Fixture();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var commandHandler = new MyFoodWasteSystemDataCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => commandHandler.ImportTranslation(MockRepository.GenerateMock<ITranslatable>(), MockRepository.GenerateMock<ITranslationInfo>(), fixture.Create<string>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("logicExecutor"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that ImportTranslation calls TranslationAdd on the logic executor when a translation for the translation informations does not exists.
        /// </summary>
        [Test]
        public void TestThatImportTranslationCallsTranslationAddOnLogicExecutorWhenTranslationForTranslationInfoDoesNotExists()
        {
            var fixture = new Fixture();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var translationInfoIdentifier = Guid.NewGuid();
            var translationInfoMock = MockRepository.GenerateMock<ITranslationInfo>();
            translationInfoMock.Stub(m => m.Identifier)
                .Return(translationInfoIdentifier)
                .Repeat.Any();

            var domainObjectIdentifier = Guid.NewGuid();
            var domainObjectMock = MockRepository.GenerateMock<ITranslatable>();
            domainObjectMock.Stub(m => m.Identifier)
                .Return(domainObjectIdentifier)
                .Repeat.Any();
            domainObjectMock.Stub(m => m.Translations)
                .Return(new List<ITranslation>(0))
                .Repeat.Any();

            var translationValue = fixture.Create<string>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutorMock.Stub(m => m.TranslationAdd(Arg<ITranslation>.Is.NotNull))
                .WhenCalled(e =>
                {
                    var translation = (ITranslation) e.Arguments.ElementAt(0);
                    Assert.That(translation, Is.Not.Null);
                    Assert.That(translation.Identifier, Is.Null);
                    Assert.That(translation.Identifier.HasValue, Is.False);
                    Assert.That(translation.TranslationOfIdentifier, Is.EqualTo(domainObjectIdentifier));
                    Assert.That(translation.TranslationInfo, Is.Not.Null);
                    Assert.That(translation.TranslationInfo, Is.EqualTo(translationInfoMock));
                    Assert.That(translation.Value, Is.Not.Null);
                    Assert.That(translation.Value, Is.Not.Empty);
                    Assert.That(translation.Value, Is.EqualTo(translationValue));
                })
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var commandHandler = new MyFoodWasteSystemDataCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            commandHandler.ImportTranslation(domainObjectMock, translationInfoMock, translationValue, logicExecutorMock);

            logicExecutorMock.AssertWasCalled(m => m.TranslationAdd(Arg<ITranslation>.Is.NotNull));
        }

        /// <summary>
        /// Tests that ImportTranslation calls TranslationAdd on the translatable domain object when a translation for the translation informations does not exists.
        /// </summary>
        [Test]
        public void TestThatImportTranslationCallsTranslationAddOnDomainObjectWhenTranslationForTranslationInfoDoesNotExists()
        {
            var fixture = new Fixture();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var translationInfoIdentifier = Guid.NewGuid();
            var translationInfoMock = MockRepository.GenerateMock<ITranslationInfo>();
            translationInfoMock.Stub(m => m.Identifier)
                .Return(translationInfoIdentifier)
                .Repeat.Any();

            var translationIdentifier = Guid.NewGuid();
            var translationValue = fixture.Create<string>();

            var domainObjectIdentifier = Guid.NewGuid();
            var domainObjectMock = MockRepository.GenerateMock<ITranslatable>();
            domainObjectMock.Stub(m => m.Identifier)
                .Return(domainObjectIdentifier)
                .Repeat.Any();
            domainObjectMock.Stub(m => m.Translations)
                .Return(new List<ITranslation>(0))
                .Repeat.Any();
            domainObjectMock.Stub(m => m.TranslationAdd(Arg<ITranslation>.Is.NotNull))
                .WhenCalled(e =>
                {
                    var translation = (ITranslation) e.Arguments.ElementAt(0);
                    Assert.That(translation, Is.Not.Null);
                    Assert.That(translation.Identifier, Is.Not.Null);
                    Assert.That(translation.Identifier.HasValue, Is.True);
                    // ReSharper disable PossibleInvalidOperationException
                    Assert.That(translation.Identifier.Value, Is.EqualTo(translationIdentifier));
                    // ReSharper restore PossibleInvalidOperationException
                    Assert.That(translation.TranslationOfIdentifier, Is.EqualTo(domainObjectIdentifier));
                    Assert.That(translation.TranslationInfo, Is.Not.Null);
                    Assert.That(translation.TranslationInfo, Is.EqualTo(translationInfoMock));
                    Assert.That(translation.Value, Is.Not.Null);
                    Assert.That(translation.Value, Is.Not.Empty);
                    Assert.That(translation.Value, Is.EqualTo(translationValue));
                })
                .Repeat.Any();

            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutorMock.Stub(m => m.TranslationAdd(Arg<ITranslation>.Is.NotNull))
                .Return(translationIdentifier)
                .Repeat.Any();

            var commandHandler = new MyFoodWasteSystemDataCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            commandHandler.ImportTranslation(domainObjectMock, translationInfoMock, translationValue, logicExecutorMock);

            domainObjectMock.AssertWasCalled(m => m.TranslationAdd(Arg<ITranslation>.Is.NotNull));
        }

        /// <summary>
        /// Tests that ImportTranslation returns the inserted translation when a translation for the translation informations does not exists.
        /// </summary>
        [Test]
        public void TestThatImportTranslationReturnsInsertedTranslationWhenTranslationForTranslationInfoDoesNotExists()
        {
            var fixture = new Fixture();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var translationInfoIdentifier = Guid.NewGuid();
            var translationInfoMock = MockRepository.GenerateMock<ITranslationInfo>();
            translationInfoMock.Stub(m => m.Identifier)
                .Return(translationInfoIdentifier)
                .Repeat.Any();

            var translationIdentifier = Guid.NewGuid();
            var translationValue = fixture.Create<string>();

            var domainObjectIdentifier = Guid.NewGuid();
            var domainObjectMock = MockRepository.GenerateMock<ITranslatable>();
            domainObjectMock.Stub(m => m.Identifier)
                .Return(domainObjectIdentifier)
                .Repeat.Any();
            domainObjectMock.Stub(m => m.Translations)
                .Return(new List<ITranslation>(0))
                .Repeat.Any();

            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutorMock.Stub(m => m.TranslationAdd(Arg<ITranslation>.Is.NotNull))
                .Return(translationIdentifier)
                .Repeat.Any();

            var commandHandler = new MyFoodWasteSystemDataCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            var translation = commandHandler.ImportTranslation(domainObjectMock, translationInfoMock, translationValue, logicExecutorMock);
            Assert.That(translation, Is.Not.Null);
            Assert.That(translation.Identifier, Is.Not.Null);
            Assert.That(translation.Identifier.HasValue, Is.True);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(translation.Identifier.Value, Is.EqualTo(translationIdentifier));
            // ReSharper restore PossibleInvalidOperationException
            Assert.That(translation.TranslationOfIdentifier, Is.EqualTo(domainObjectIdentifier));
            Assert.That(translation.TranslationInfo, Is.Not.Null);
            Assert.That(translation.TranslationInfo, Is.EqualTo(translationInfoMock));
            Assert.That(translation.Value, Is.Not.Null);
            Assert.That(translation.Value, Is.Not.Empty);
            Assert.That(translation.Value, Is.EqualTo(translationValue));
        }

        /// <summary>
        /// Tests that ImportTranslation updates Value on the translation when a translation for the translation informations does exists.
        /// </summary>
        [Test]
        public void TestThatImportTranslationUpdatesValueOnTranslationWhenTranslationForTranslationInfoDoesExists()
        {
            var fixture = new Fixture();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var translationInfoIdentifier = Guid.NewGuid();
            var translationInfoMock = MockRepository.GenerateMock<ITranslationInfo>();
            translationInfoMock.Stub(m => m.Identifier)
                .Return(translationInfoIdentifier)
                .Repeat.Any();

            var translationIdentifier = Guid.NewGuid();
            var domainObjectIdentifier = Guid.NewGuid();
            var translationMock = MockRepository.GenerateMock<ITranslation>();
            translationMock.Stub(m => m.Identifier)
                .Return(translationIdentifier)
                .Repeat.Any();
            translationMock.Stub(m => m.TranslationOfIdentifier)
                .Return(domainObjectIdentifier)
                .Repeat.Any();
            translationMock.Stub(m => m.TranslationInfo)
                .Return(translationInfoMock)
                .Repeat.Any();

            var domainObjectMock = MockRepository.GenerateMock<ITranslatable>();
            domainObjectMock.Stub(m => m.Identifier)
                .Return(domainObjectIdentifier)
                .Repeat.Any();
            domainObjectMock.Stub(m => m.Translations)
                .Return(new List<ITranslation> {translationMock})
                .Repeat.Any();

            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutorMock.Stub(m => m.TranslationModify(Arg<ITranslation>.Is.NotNull))
                .Return(translationIdentifier)
                .Repeat.Any();

            var translationValue = fixture.Create<string>();

            var commandHandler = new MyFoodWasteSystemDataCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            commandHandler.ImportTranslation(domainObjectMock, translationInfoMock, translationValue, logicExecutorMock);

            translationMock.AssertWasCalled(m => m.Value = Arg<string>.Is.Equal(translationValue));
        }

        /// <summary>
        /// Tests that ImportTranslation updates Identifier on the translation when a translation for the translation informations does exists.
        /// </summary>
        [Test]
        public void TestThatImportTranslationUpdatesIdentifierOnTranslationWhenTranslationForTranslationInfoDoesExists()
        {
            var fixture = new Fixture();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var translationInfoIdentifier = Guid.NewGuid();
            var translationInfoMock = MockRepository.GenerateMock<ITranslationInfo>();
            translationInfoMock.Stub(m => m.Identifier)
                .Return(translationInfoIdentifier)
                .Repeat.Any();

            var translationIdentifier = Guid.NewGuid();
            var domainObjectIdentifier = Guid.NewGuid();
            var translationMock = MockRepository.GenerateMock<ITranslation>();
            translationMock.Stub(m => m.Identifier)
                .Return(translationIdentifier)
                .Repeat.Any();
            translationMock.Stub(m => m.TranslationOfIdentifier)
                .Return(domainObjectIdentifier)
                .Repeat.Any();
            translationMock.Stub(m => m.TranslationInfo)
                .Return(translationInfoMock)
                .Repeat.Any();

            var domainObjectMock = MockRepository.GenerateMock<ITranslatable>();
            domainObjectMock.Stub(m => m.Identifier)
                .Return(domainObjectIdentifier)
                .Repeat.Any();
            domainObjectMock.Stub(m => m.Translations)
                .Return(new List<ITranslation> { translationMock })
                .Repeat.Any();

            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutorMock.Stub(m => m.TranslationModify(Arg<ITranslation>.Is.NotNull))
                .Return(translationIdentifier)
                .Repeat.Any();

            var commandHandler = new MyFoodWasteSystemDataCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            commandHandler.ImportTranslation(domainObjectMock, translationInfoMock, fixture.Create<string>(), logicExecutorMock);

            translationMock.AssertWasCalled(m => m.Identifier = Arg<Guid?>.Is.Equal(translationIdentifier));
        }

        /// <summary>
        /// Tests that ImportTranslation calls TranslationModify on the logic executor when a translation for the translation informations does exists.
        /// </summary>
        [Test]
        public void TestThatImportTranslationCallsTranslationModifyOnLogicExecutorWhenTranslationForTranslationInfoDoesExists()
        {
            var fixture = new Fixture();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var translationInfoIdentifier = Guid.NewGuid();
            var translationInfoMock = MockRepository.GenerateMock<ITranslationInfo>();
            translationInfoMock.Stub(m => m.Identifier)
                .Return(translationInfoIdentifier)
                .Repeat.Any();

            var translationIdentifier = Guid.NewGuid();
            var domainObjectIdentifier = Guid.NewGuid();
            var translationMock = MockRepository.GenerateMock<ITranslation>();
            translationMock.Stub(m => m.Identifier)
                .Return(translationIdentifier)
                .Repeat.Any();
            translationMock.Stub(m => m.TranslationOfIdentifier)
                .Return(domainObjectIdentifier)
                .Repeat.Any();
            translationMock.Stub(m => m.TranslationInfo)
                .Return(translationInfoMock)
                .Repeat.Any();

            var domainObjectMock = MockRepository.GenerateMock<ITranslatable>();
            domainObjectMock.Stub(m => m.Identifier)
                .Return(domainObjectIdentifier)
                .Repeat.Any();
            domainObjectMock.Stub(m => m.Translations)
                .Return(new List<ITranslation> { translationMock })
                .Repeat.Any();

            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutorMock.Stub(m => m.TranslationModify(Arg<ITranslation>.Is.NotNull))
                .Return(translationIdentifier)
                .Repeat.Any();

            var commandHandler = new MyFoodWasteSystemDataCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            commandHandler.ImportTranslation(domainObjectMock, translationInfoMock, fixture.Create<string>(), logicExecutorMock);

            logicExecutorMock.AssertWasCalled(m => m.TranslationModify(Arg<ITranslation>.Is.Equal(translationMock)));
        }

        /// <summary>
        /// Tests that ImportTranslation returns the updated translation when a translation for the translation informations does exists.
        /// </summary>
        [Test]
        public void TestThatImportTranslationReturnsUpdatedTranslationWhenTranslationForTranslationInfoDoesExists()
        {
            var fixture = new Fixture();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var translationInfoIdentifier = Guid.NewGuid();
            var translationInfoMock = MockRepository.GenerateMock<ITranslationInfo>();
            translationInfoMock.Stub(m => m.Identifier)
                .Return(translationInfoIdentifier)
                .Repeat.Any();

            var translationIdentifier = Guid.NewGuid();
            var domainObjectIdentifier = Guid.NewGuid();
            var translationMock = MockRepository.GenerateMock<ITranslation>();
            translationMock.Stub(m => m.Identifier)
                .Return(translationIdentifier)
                .Repeat.Any();
            translationMock.Stub(m => m.TranslationOfIdentifier)
                .Return(domainObjectIdentifier)
                .Repeat.Any();
            translationMock.Stub(m => m.TranslationInfo)
                .Return(translationInfoMock)
                .Repeat.Any();

            var domainObjectMock = MockRepository.GenerateMock<ITranslatable>();
            domainObjectMock.Stub(m => m.Identifier)
                .Return(domainObjectIdentifier)
                .Repeat.Any();
            domainObjectMock.Stub(m => m.Translations)
                .Return(new List<ITranslation> { translationMock })
                .Repeat.Any();

            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutorMock.Stub(m => m.TranslationModify(Arg<ITranslation>.Is.NotNull))
                .Return(translationIdentifier)
                .Repeat.Any();

            var commandHandler = new MyFoodWasteSystemDataCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            var translation = commandHandler.ImportTranslation(domainObjectMock, translationInfoMock, fixture.Create<string>(), logicExecutorMock);
            Assert.That(translation, Is.Not.Null);
            Assert.That(translation, Is.EqualTo(translationMock));
        }
    }
}
