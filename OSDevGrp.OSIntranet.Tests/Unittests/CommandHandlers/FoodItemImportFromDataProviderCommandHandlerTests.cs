using System;
using System.Collections.Generic;
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
    /// Tests the command handler which handles a command for importing a food item from a given data provider.
    /// </summary>
    [TestFixture]
    public class FoodItemImportFromDataProviderCommandHandlerTests
    {
        /// <summary>
        /// Tests that the constructor initialize a command handler which handles a command for importing a food item from a given data provider.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeFoodItemImportFromDataProviderCommandHandler()
        {
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();

            var foodItemImportFromDataProviderCommandHandler = new FoodItemImportFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, logicExecutorMock);
            Assert.That(foodItemImportFromDataProviderCommandHandler, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the logic executor is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenLogicExecutorIsNull()
        {
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var exception = Assert.Throws<ArgumentNullException>(() => new FoodItemImportFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("logicExecutor"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Execute throws an ArgumentNullException if the command for importing a food item from a given data provider is null.
        /// </summary>
        [Test]
        public void TestThatExecuteThrowsArgumentNullExceptionIfFoodItemImportFromDataProviderCommandIsNull()
        {
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();

            var foodItemImportFromDataProviderCommandHandler = new FoodItemImportFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, logicExecutorMock);
            Assert.That(foodItemImportFromDataProviderCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodItemImportFromDataProviderCommandHandler.Execute(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("command"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Execute calls Get to get the data provider on the repository which can access system data for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsGetForDataProviderIdentifierOnSystemDataRepository()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<IFoodGroup>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodItemGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Anything))
                .Return(null)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<IFoodItem>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodItemMock(translations: new List<ITranslation>(0)))
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutorMock.Stub(m => m.ForeignKeyAdd(Arg<IForeignKey>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();
            logicExecutorMock.Stub(m => m.TranslationAdd(Arg<ITranslation>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var foodItemImportFromDataProviderCommandHandler = new FoodItemImportFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, logicExecutorMock);
            Assert.That(foodItemImportFromDataProviderCommandHandler, Is.Not.Null);

            var command = new FoodItemImportFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = fixture.Create<string>(),
                Name = fixture.Create<string>(),
                PrimaryFoodGroupIdentifier = Guid.NewGuid(),
                IsActive = fixture.Create<bool>()
            };

            foodItemImportFromDataProviderCommandHandler.Execute(command);

            systemDataRepositoryMock.AssertWasCalled(m => m.Get<IDataProvider>(Arg<Guid>.Is.Equal(command.DataProviderIdentifier)));
        }

        /// <summary>
        /// Tests that Execute calls Get to get the transation informations on the repository which can access system data for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsGetForTranslationInfoIdentifierOnSystemDataRepository()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<IFoodGroup>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodItemGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Anything))
                .Return(null)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<IFoodItem>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodItemMock(translations: new List<ITranslation>(0)))
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutorMock.Stub(m => m.ForeignKeyAdd(Arg<IForeignKey>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();
            logicExecutorMock.Stub(m => m.TranslationAdd(Arg<ITranslation>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var foodItemImportFromDataProviderCommandHandler = new FoodItemImportFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, logicExecutorMock);
            Assert.That(foodItemImportFromDataProviderCommandHandler, Is.Not.Null);

            var command = new FoodItemImportFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = fixture.Create<string>(),
                Name = fixture.Create<string>(),
                PrimaryFoodGroupIdentifier = Guid.NewGuid(),
                IsActive = fixture.Create<bool>()
            };

            foodItemImportFromDataProviderCommandHandler.Execute(command);

            systemDataRepositoryMock.AssertWasCalled(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Equal(command.TranslationInfoIdentifier)));
        }

        /// <summary>
        /// Tests that Execute calls Get to get the primary food group on the repository which can access system data for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsGetForPrimaryFoodGroupIdentifierOnSystemDataRepository()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<IFoodGroup>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodItemGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Anything))
                .Return(null)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<IFoodItem>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodItemMock(translations: new List<ITranslation>(0)))
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutorMock.Stub(m => m.ForeignKeyAdd(Arg<IForeignKey>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();
            logicExecutorMock.Stub(m => m.TranslationAdd(Arg<ITranslation>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var foodItemImportFromDataProviderCommandHandler = new FoodItemImportFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, logicExecutorMock);
            Assert.That(foodItemImportFromDataProviderCommandHandler, Is.Not.Null);

            var command = new FoodItemImportFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = fixture.Create<string>(),
                Name = fixture.Create<string>(),
                PrimaryFoodGroupIdentifier = Guid.NewGuid(),
                IsActive = fixture.Create<bool>()
            };

            foodItemImportFromDataProviderCommandHandler.Execute(command);

            systemDataRepositoryMock.AssertWasCalled(m => m.Get<IFoodGroup>(Arg<Guid>.Is.Equal(command.PrimaryFoodGroupIdentifier)));
        }

        /// <summary>
        /// Tests that Execute calls IsNotNull for the data provider on the common validations.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsIsNotNullForDataProviderOnCommonValidations()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var dataProviderMock = DomainObjectMockBuilder.BuildDataProviderMock();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(dataProviderMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<IFoodGroup>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodItemGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Anything))
                .Return(null)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<IFoodItem>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodItemMock(translations: new List<ITranslation>(0)))
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

            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutorMock.Stub(m => m.ForeignKeyAdd(Arg<IForeignKey>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();
            logicExecutorMock.Stub(m => m.TranslationAdd(Arg<ITranslation>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var foodItemImportFromDataProviderCommandHandler = new FoodItemImportFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, logicExecutorMock);
            Assert.That(foodItemImportFromDataProviderCommandHandler, Is.Not.Null);

            var command = new FoodItemImportFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = fixture.Create<string>(),
                Name = fixture.Create<string>(),
                PrimaryFoodGroupIdentifier = Guid.NewGuid(),
                IsActive = fixture.Create<bool>()
            };

            foodItemImportFromDataProviderCommandHandler.Execute(command);

            commonValidationsMock.AssertWasCalled(m => m.IsNotNull(Arg<IDataProvider>.Is.Equal(dataProviderMock)));
        }

        /// <summary>
        /// Tests that Execute calls IsNotNull for the translation informations on the common validations.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsIsNotNullForTranslationInfoOnCommonValidations()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var translationInfoMock = DomainObjectMockBuilder.BuildTranslationInfoMock();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(translationInfoMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<IFoodGroup>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodItemGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Anything))
                .Return(null)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<IFoodItem>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodItemMock(translations: new List<ITranslation>(0)))
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

            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutorMock.Stub(m => m.ForeignKeyAdd(Arg<IForeignKey>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();
            logicExecutorMock.Stub(m => m.TranslationAdd(Arg<ITranslation>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var foodItemImportFromDataProviderCommandHandler = new FoodItemImportFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, logicExecutorMock);
            Assert.That(foodItemImportFromDataProviderCommandHandler, Is.Not.Null);

            var command = new FoodItemImportFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = fixture.Create<string>(),
                Name = fixture.Create<string>(),
                PrimaryFoodGroupIdentifier = Guid.NewGuid(),
                IsActive = fixture.Create<bool>()
            };

            foodItemImportFromDataProviderCommandHandler.Execute(command);

            commonValidationsMock.AssertWasCalled(m => m.IsNotNull(Arg<ITranslationInfo>.Is.Equal(translationInfoMock)));
        }

        /// <summary>
        /// Tests that Execute calls IsNotNull for the primary food group on the common validations.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsIsNotNullForPrimaryFoodGroupOnCommonValidations()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var primaryFoodGroup = DomainObjectMockBuilder.BuildFoodGroupMock();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<IFoodGroup>(Arg<Guid>.Is.Anything))
                .Return(primaryFoodGroup)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodItemGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Anything))
                .Return(null)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<IFoodItem>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodItemMock(translations: new List<ITranslation>(0)))
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

            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutorMock.Stub(m => m.ForeignKeyAdd(Arg<IForeignKey>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();
            logicExecutorMock.Stub(m => m.TranslationAdd(Arg<ITranslation>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var foodItemImportFromDataProviderCommandHandler = new FoodItemImportFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, logicExecutorMock);
            Assert.That(foodItemImportFromDataProviderCommandHandler, Is.Not.Null);

            var command = new FoodItemImportFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = fixture.Create<string>(),
                Name = fixture.Create<string>(),
                PrimaryFoodGroupIdentifier = Guid.NewGuid(),
                IsActive = fixture.Create<bool>()
            };

            foodItemImportFromDataProviderCommandHandler.Execute(command);

            commonValidationsMock.AssertWasCalled(m => m.IsNotNull(Arg<IFoodGroup>.Is.Equal(primaryFoodGroup)));
        }

        /// <summary>
        /// Tests that Execute calls HasValue for the food item key on the common validations.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsHasValueForKeyOnCommonValidations()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<IFoodGroup>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodItemGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Anything))
                .Return(null)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<IFoodItem>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodItemMock(translations: new List<ITranslation>(0)))
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

            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutorMock.Stub(m => m.ForeignKeyAdd(Arg<IForeignKey>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();
            logicExecutorMock.Stub(m => m.TranslationAdd(Arg<ITranslation>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var foodItemImportFromDataProviderCommandHandler = new FoodItemImportFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, logicExecutorMock);
            Assert.That(foodItemImportFromDataProviderCommandHandler, Is.Not.Null);

            var command = new FoodItemImportFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = fixture.Create<string>(),
                Name = fixture.Create<string>(),
                PrimaryFoodGroupIdentifier = Guid.NewGuid(),
                IsActive = fixture.Create<bool>()
            };

            foodItemImportFromDataProviderCommandHandler.Execute(command);

            commonValidationsMock.AssertWasCalled(m => m.HasValue(Arg<string>.Is.Equal(command.Key)));
        }

        /// <summary>
        /// Tests that Execute calls ContainsIllegalChar for the food item key on the common validations.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsContainsIllegalCharForKeyOnCommonValidations()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<IFoodGroup>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodItemGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Anything))
                .Return(null)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<IFoodItem>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodItemMock(translations: new List<ITranslation>(0)))
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

            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutorMock.Stub(m => m.ForeignKeyAdd(Arg<IForeignKey>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();
            logicExecutorMock.Stub(m => m.TranslationAdd(Arg<ITranslation>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var foodItemImportFromDataProviderCommandHandler = new FoodItemImportFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, logicExecutorMock);
            Assert.That(foodItemImportFromDataProviderCommandHandler, Is.Not.Null);

            var command = new FoodItemImportFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = fixture.Create<string>(),
                Name = fixture.Create<string>(),
                PrimaryFoodGroupIdentifier = Guid.NewGuid(),
                IsActive = fixture.Create<bool>()
            };

            foodItemImportFromDataProviderCommandHandler.Execute(command);

            commonValidationsMock.AssertWasCalled(m => m.ContainsIllegalChar(Arg<string>.Is.Equal(command.Key)));
        }

        /// <summary>
        /// Tests that Execute calls HasValue for the food item name on the common validations.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsHasValueForNameOnCommonValidations()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<IFoodGroup>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodItemGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Anything))
                .Return(null)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<IFoodItem>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodItemMock(translations: new List<ITranslation>(0)))
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

            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutorMock.Stub(m => m.ForeignKeyAdd(Arg<IForeignKey>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();
            logicExecutorMock.Stub(m => m.TranslationAdd(Arg<ITranslation>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var foodItemImportFromDataProviderCommandHandler = new FoodItemImportFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, logicExecutorMock);
            Assert.That(foodItemImportFromDataProviderCommandHandler, Is.Not.Null);

            var command = new FoodItemImportFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = fixture.Create<string>(),
                Name = fixture.Create<string>(),
                PrimaryFoodGroupIdentifier = Guid.NewGuid(),
                IsActive = fixture.Create<bool>()
            };

            foodItemImportFromDataProviderCommandHandler.Execute(command);

            commonValidationsMock.AssertWasCalled(m => m.HasValue(Arg<string>.Is.Equal(command.Name)));
        }

        /// <summary>
        /// Tests that Execute calls ContainsIllegalChar for the food item name on the common validations.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsContainsIllegalCharForNameOnCommonValidations()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<IFoodGroup>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodItemGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Anything))
                .Return(null)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<IFoodItem>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodItemMock(translations: new List<ITranslation>(0)))
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

            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutorMock.Stub(m => m.ForeignKeyAdd(Arg<IForeignKey>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();
            logicExecutorMock.Stub(m => m.TranslationAdd(Arg<ITranslation>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var foodItemImportFromDataProviderCommandHandler = new FoodItemImportFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, logicExecutorMock);
            Assert.That(foodItemImportFromDataProviderCommandHandler, Is.Not.Null);

            var command = new FoodItemImportFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = fixture.Create<string>(),
                Name = fixture.Create<string>(),
                PrimaryFoodGroupIdentifier = Guid.NewGuid(),
                IsActive = fixture.Create<bool>()
            };

            foodItemImportFromDataProviderCommandHandler.Execute(command);

            commonValidationsMock.AssertWasCalled(m => m.ContainsIllegalChar(Arg<string>.Is.Equal(command.Name)));
        }

        /// <summary>
        /// Tests that Execute calls IsSatisfiedBy on the specification which encapsulates validation rules 7 times.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsIsSatisfiedByOnSpecification5Times()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<IFoodGroup>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodItemGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Anything))
                .Return(null)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<IFoodItem>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodItemMock(translations: new List<ITranslation>(0)))
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutorMock.Stub(m => m.ForeignKeyAdd(Arg<IForeignKey>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();
            logicExecutorMock.Stub(m => m.TranslationAdd(Arg<ITranslation>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var foodItemImportFromDataProviderCommandHandler = new FoodItemImportFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, logicExecutorMock);
            Assert.That(foodItemImportFromDataProviderCommandHandler, Is.Not.Null);

            var command = new FoodItemImportFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = fixture.Create<string>(),
                Name = fixture.Create<string>(),
                PrimaryFoodGroupIdentifier = Guid.NewGuid(),
                IsActive = fixture.Create<bool>()
            };

            foodItemImportFromDataProviderCommandHandler.Execute(command);

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
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<IFoodGroup>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodItemGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Anything))
                .Return(null)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<IFoodItem>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodItemMock(translations: new List<ITranslation>(0)))
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutorMock.Stub(m => m.ForeignKeyAdd(Arg<IForeignKey>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();
            logicExecutorMock.Stub(m => m.TranslationAdd(Arg<ITranslation>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var foodItemImportFromDataProviderCommandHandler = new FoodItemImportFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, logicExecutorMock);
            Assert.That(foodItemImportFromDataProviderCommandHandler, Is.Not.Null);

            var command = new FoodItemImportFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = fixture.Create<string>(),
                Name = fixture.Create<string>(),
                PrimaryFoodGroupIdentifier = Guid.NewGuid(),
                IsActive = fixture.Create<bool>()
            };

            foodItemImportFromDataProviderCommandHandler.Execute(command);

            specificationMock.AssertWasCalled(m => m.Evaluate());
        }

        /// <summary>
        /// Tests that Execute calls FoodItemGetByForeignKey on the repository which can access system data for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsFoodItemGetByForeignKeyOnSystemDataRepository()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var dataProviderMock = DomainObjectMockBuilder.BuildDataProviderMock();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(dataProviderMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<IFoodGroup>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodItemGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Anything))
                .Return(null)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<IFoodItem>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodItemMock(translations: new List<ITranslation>(0)))
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutorMock.Stub(m => m.ForeignKeyAdd(Arg<IForeignKey>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();
            logicExecutorMock.Stub(m => m.TranslationAdd(Arg<ITranslation>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var foodItemImportFromDataProviderCommandHandler = new FoodItemImportFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, logicExecutorMock);
            Assert.That(foodItemImportFromDataProviderCommandHandler, Is.Not.Null);

            var command = new FoodItemImportFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = fixture.Create<string>(),
                Name = fixture.Create<string>(),
                PrimaryFoodGroupIdentifier = Guid.NewGuid(),
                IsActive = fixture.Create<bool>()
            };

            foodItemImportFromDataProviderCommandHandler.Execute(command);

            systemDataRepositoryMock.AssertWasCalled(m => m.FoodItemGetByForeignKey(Arg<IDataProvider>.Is.Equal(dataProviderMock), Arg<string>.Is.Equal(command.Key)));
        }

        /// <summary>
        /// Tests that Execute calls Insert on the repository which can access system data for the food waste domain when a food item for the key does not exist.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsInsertOnSystemDataRepositoryWhenFoodItemForKeyDoesNotExist()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var isActive = fixture.Create<bool>();
            var primaryFoodGroup = DomainObjectMockBuilder.BuildFoodGroupMock();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<IFoodGroup>(Arg<Guid>.Is.Anything))
                .Return(primaryFoodGroup)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodItemGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Anything))
                .Return(null)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<IFoodItem>.Is.NotNull))
                .WhenCalled(e =>
                {
                    var foodItem = (IFoodItem) e.Arguments.ElementAt(0);
                    Assert.That(foodItem, Is.Not.Null);
                    Assert.That(foodItem.Identifier, Is.Null);
                    Assert.That(foodItem.Identifier.HasValue, Is.False);
                    Assert.That(foodItem.PrimaryFoodGroup, Is.Not.Null);
                    Assert.That(foodItem.PrimaryFoodGroup, Is.EqualTo(primaryFoodGroup));
                    Assert.That(foodItem.IsActive, Is.EqualTo(isActive));
                    Assert.That(foodItem.FoodGroups, Is.Not.Null);
                    Assert.That(foodItem.FoodGroups, Is.Not.Empty);
                    Assert.That(foodItem.FoodGroups.Count(), Is.EqualTo(1));
                    Assert.That(foodItem.FoodGroups.Contains(primaryFoodGroup), Is.True);
                    Assert.That(foodItem.Translation, Is.Null);
                    Assert.That(foodItem.Translations, Is.Not.Null);
                    Assert.That(foodItem.Translations, Is.Empty);
                    Assert.That(foodItem.ForeignKeys, Is.Not.Null);
                    Assert.That(foodItem.ForeignKeys, Is.Empty);
                })
                .Return(DomainObjectMockBuilder.BuildFoodItemMock(translations: new List<ITranslation>(0)))
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutorMock.Stub(m => m.ForeignKeyAdd(Arg<IForeignKey>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();
            logicExecutorMock.Stub(m => m.TranslationAdd(Arg<ITranslation>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var foodItemImportFromDataProviderCommandHandler = new FoodItemImportFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, logicExecutorMock);
            Assert.That(foodItemImportFromDataProviderCommandHandler, Is.Not.Null);

            var command = new FoodItemImportFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = fixture.Create<string>(),
                Name = fixture.Create<string>(),
                PrimaryFoodGroupIdentifier = Guid.NewGuid(),
                IsActive = isActive
            };

            foodItemImportFromDataProviderCommandHandler.Execute(command);

            systemDataRepositoryMock.AssertWasCalled(m => m.Insert(Arg<IFoodItem>.Is.NotNull));
        }

        /// <summary>
        /// Tests that Execute calls ForeignKeyAdd on the logic executor when a food item for the key does not exist.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsForeignKeyAddOnLogicExecutorWhenFoodItemForKeyDoesNotExist()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var dataProviderMock = DomainObjectMockBuilder.BuildDataProviderMock();
            var insertedFoodItemMock = DomainObjectMockBuilder.BuildFoodItemMock(translations: new List<ITranslation>(0));
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(dataProviderMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<IFoodGroup>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodItemGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Anything))
                .Return(null)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<IFoodItem>.Is.Anything))
                .Return(insertedFoodItemMock)
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var key = fixture.Create<string>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutorMock.Stub(m => m.ForeignKeyAdd(Arg<IForeignKey>.Is.NotNull))
                .WhenCalled(e =>
                {
                    var foreignKey = (IForeignKey) e.Arguments.ElementAt(0);
                    Assert.That(foreignKey, Is.Not.Null);
                    Assert.That(foreignKey.Identifier, Is.Null);
                    Assert.That(foreignKey.Identifier.HasValue, Is.False);
                    Assert.That(foreignKey.DataProvider, Is.Not.Null);
                    Assert.That(foreignKey.DataProvider, Is.EqualTo(dataProviderMock));
                    // ReSharper disable PossibleInvalidOperationException
                    Assert.That(foreignKey.ForeignKeyForIdentifier, Is.EqualTo(insertedFoodItemMock.Identifier.Value));
                    // ReSharper restore PossibleInvalidOperationException
                    Assert.That(foreignKey.ForeignKeyForTypes, Is.Not.Null);
                    Assert.That(foreignKey.ForeignKeyForTypes, Is.Not.Empty);
                    Assert.That(foreignKey.ForeignKeyForTypes.Contains(typeof (IFoodItem)), Is.True);
                    Assert.That(foreignKey.ForeignKeyValue, Is.Not.Null);
                    Assert.That(foreignKey.ForeignKeyValue, Is.Not.Empty);
                    Assert.That(foreignKey.ForeignKeyValue, Is.EqualTo(key));
                })
                .Return(Guid.NewGuid())
                .Repeat.Any();
            logicExecutorMock.Stub(m => m.TranslationAdd(Arg<ITranslation>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var foodItemImportFromDataProviderCommandHandler = new FoodItemImportFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, logicExecutorMock);
            Assert.That(foodItemImportFromDataProviderCommandHandler, Is.Not.Null);

            var command = new FoodItemImportFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = key,
                Name = fixture.Create<string>(),
                PrimaryFoodGroupIdentifier = Guid.NewGuid(),
                IsActive = fixture.Create<bool>()
            };

            foodItemImportFromDataProviderCommandHandler.Execute(command);

            logicExecutorMock.AssertWasCalled(m => m.ForeignKeyAdd(Arg<IForeignKey>.Is.NotNull));
        }

        /// <summary>
        /// Tests that Execute calls TranslationAdd on the logic executor when a food item for the key does not exist.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsTranslationAddOnLogicExecutorWhenFoodItemForKeyDoesNotExist()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var translationInfoMock = DomainObjectMockBuilder.BuildTranslationInfoMock();
            var insertedFoodItemMock = DomainObjectMockBuilder.BuildFoodItemMock(translations: new List<ITranslation>(0));
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(translationInfoMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<IFoodGroup>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodItemGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Anything))
                .Return(null)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<IFoodItem>.Is.Anything))
                .Return(insertedFoodItemMock)
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var name = fixture.Create<string>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutorMock.Stub(m => m.ForeignKeyAdd(Arg<IForeignKey>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();
            logicExecutorMock.Stub(m => m.TranslationAdd(Arg<ITranslation>.Is.NotNull))
                .WhenCalled(e =>
                {
                    var translation = (ITranslation) e.Arguments.ElementAt(0);
                    Assert.That(translation, Is.Not.Null);
                    Assert.That(translation.Identifier, Is.Null);
                    Assert.That(translation.Identifier.HasValue, Is.False);
                    Assert.That(translation.TranslationInfo, Is.Not.Null);
                    Assert.That(translation.TranslationInfo, Is.EqualTo(translationInfoMock));
                    // ReSharper disable PossibleInvalidOperationException
                    Assert.That(translation.TranslationOfIdentifier, Is.EqualTo(insertedFoodItemMock.Identifier.Value));
                    // ReSharper restore PossibleInvalidOperationException
                    Assert.That(translation.Value, Is.Not.Null);
                    Assert.That(translation.Value, Is.Not.Empty);
                    Assert.That(translation.Value, Is.EqualTo(name));
                })
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var foodItemImportFromDataProviderCommandHandler = new FoodItemImportFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, logicExecutorMock);
            Assert.That(foodItemImportFromDataProviderCommandHandler, Is.Not.Null);

            var command = new FoodItemImportFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = fixture.Create<string>(),
                Name = name,
                PrimaryFoodGroupIdentifier = Guid.NewGuid(),
                IsActive = fixture.Create<bool>()
            };

            foodItemImportFromDataProviderCommandHandler.Execute(command);

            logicExecutorMock.AssertWasCalled(m => m.TranslationAdd(Arg<ITranslation>.Is.NotNull));
        }

        /// <summary>
        /// Tests that Execute calls Map for the inserted food item on the object mapper for the food waste domain when a food item for the key does not exist.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsMapForInsertedFoodItemOnFoodWasteObjectMapperWhenFoodItemForKeyDoesNotExist()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var insertedFoodItemMock = DomainObjectMockBuilder.BuildFoodItemMock(translations: new List<ITranslation>(0));
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<IFoodGroup>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodItemGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Anything))
                .Return(null)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<IFoodItem>.Is.Anything))
                .Return(insertedFoodItemMock)
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutorMock.Stub(m => m.ForeignKeyAdd(Arg<IForeignKey>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();
            logicExecutorMock.Stub(m => m.TranslationAdd(Arg<ITranslation>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var foodItemImportFromDataProviderCommandHandler = new FoodItemImportFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, logicExecutorMock);
            Assert.That(foodItemImportFromDataProviderCommandHandler, Is.Not.Null);

            var command = new FoodItemImportFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = fixture.Create<string>(),
                Name = fixture.Create<string>(),
                PrimaryFoodGroupIdentifier = Guid.NewGuid(),
                IsActive = fixture.Create<bool>()
            };

            foodItemImportFromDataProviderCommandHandler.Execute(command);

            foodWasteObjectMapperMock.AssertWasCalled(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Equal(insertedFoodItemMock), Arg<CultureInfo>.Is.Null));
        }

        /// <summary>
        /// Tests that Execute returns the service receipt created by Map for the inserted food item on the object mapper for the food waste domain when a food item for the key does not exist.
        /// </summary>
        [Test]
        public void TestThatExecuteReturnsServiceReceiptFromMapOnFoodWasteObjectMapperWhenFoodItemForKeyDoesNotExist()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<IFoodGroup>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodItemGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Anything))
                .Return(null)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<IFoodItem>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodItemMock(translations: new List<ITranslation>(0)))
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutorMock.Stub(m => m.ForeignKeyAdd(Arg<IForeignKey>.Is.NotNull))
                .Return(Guid.NewGuid())
                .Repeat.Any();
            logicExecutorMock.Stub(m => m.TranslationAdd(Arg<ITranslation>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var serviceReceipt = fixture.Create<ServiceReceiptResponse>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(serviceReceipt)
                .Repeat.Any();

            var foodItemImportFromDataProviderCommandHandler = new FoodItemImportFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, logicExecutorMock);
            Assert.That(foodItemImportFromDataProviderCommandHandler, Is.Not.Null);

            var command = new FoodItemImportFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = fixture.Create<string>(),
                Name = fixture.Create<string>(),
                PrimaryFoodGroupIdentifier = Guid.NewGuid(),
                IsActive = fixture.Create<bool>()
            };

            var result = foodItemImportFromDataProviderCommandHandler.Execute(command);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(serviceReceipt));
        }

        /// <summary>
        /// Tests that Execute sets the value for IsActive with the value from the command when a food item for the key does exist.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatExecuteSetsIsActiveWithIsActiveFromCommandWhenFoodItemForKeyDoesExist(bool testValue)
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var foodItemMock = DomainObjectMockBuilder.BuildFoodItemMock(translations: new List<ITranslation>(0));
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<IFoodGroup>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodItemGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Anything))
                .Return(foodItemMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Update(Arg<IFoodItem>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodItemMock(translations: new List<ITranslation>(0)))
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutorMock.Stub(m => m.TranslationAdd(Arg<ITranslation>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var foodItemImportFromDataProviderCommandHandler = new FoodItemImportFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, logicExecutorMock);
            Assert.That(foodItemImportFromDataProviderCommandHandler, Is.Not.Null);

            var command = new FoodItemImportFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = fixture.Create<string>(),
                Name = fixture.Create<string>(),
                PrimaryFoodGroupIdentifier = Guid.NewGuid(),
                IsActive = testValue
            };

            foodItemImportFromDataProviderCommandHandler.Execute(command);

            foodItemMock.AssertWasCalled(m => m.IsActive = Arg<bool>.Is.Equal(testValue));
        }

        /// <summary>
        /// Tests that Execute calls Update on the repository which can access system data for the food waste domain when a food item for the key does exist.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsUpdateOnSystemDataRepositoryWhenFoodItemForKeyDoesExist()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var foodItemMock = DomainObjectMockBuilder.BuildFoodItemMock(translations: new List<ITranslation>(0));
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<IFoodGroup>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodItemGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Anything))
                .Return(foodItemMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Update(Arg<IFoodItem>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodItemMock(translations: new List<ITranslation>(0)))
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutorMock.Stub(m => m.TranslationAdd(Arg<ITranslation>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var foodItemImportFromDataProviderCommandHandler = new FoodItemImportFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, logicExecutorMock);
            Assert.That(foodItemImportFromDataProviderCommandHandler, Is.Not.Null);

            var command = new FoodItemImportFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = fixture.Create<string>(),
                Name = fixture.Create<string>(),
                PrimaryFoodGroupIdentifier = Guid.NewGuid(),
                IsActive = fixture.Create<bool>()
            };

            foodItemImportFromDataProviderCommandHandler.Execute(command);

            systemDataRepositoryMock.AssertWasCalled(m => m.Update(Arg<IFoodItem>.Is.Equal(foodItemMock)));
        }

        /// <summary>
        /// Tests that Execute calls TranslationAdd on the logic exeuctor when a food item for the key does exist and it does not have the translation in the command.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsTranslationAddOnLogicExecutorWhenFoodItemForKeyDoesExistAndFoodItemForKeyDoesNotHaveTranslationInCommand()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var translationInfoMock = DomainObjectMockBuilder.BuildTranslationInfoMock();
            var updatedFoodItemMock = DomainObjectMockBuilder.BuildFoodItemMock(translations: new List<ITranslation>(0));
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(translationInfoMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<IFoodGroup>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodItemGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodItemMock(translations: new List<ITranslation>(0)))
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Update(Arg<IFoodItem>.Is.Anything))
                .Return(updatedFoodItemMock)
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var name = fixture.Create<string>();
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutorMock.Stub(m => m.TranslationAdd(Arg<ITranslation>.Is.NotNull))
                .WhenCalled(e =>
                {
                    var translation = (ITranslation) e.Arguments.ElementAt(0);
                    Assert.That(translation, Is.Not.Null);
                    Assert.That(translation.Identifier, Is.Null);
                    Assert.That(translation.Identifier.HasValue, Is.False);
                    Assert.That(translation.TranslationInfo, Is.Not.Null);
                    Assert.That(translation.TranslationInfo, Is.EqualTo(translationInfoMock));
                    // ReSharper disable PossibleInvalidOperationException
                    Assert.That(translation.TranslationOfIdentifier, Is.EqualTo(updatedFoodItemMock.Identifier.Value));
                    // ReSharper restore PossibleInvalidOperationException
                    Assert.That(translation.Value, Is.Not.Null);
                    Assert.That(translation.Value, Is.Not.Empty);
                    Assert.That(translation.Value, Is.EqualTo(name));
                })
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var foodItemImportFromDataProviderCommandHandler = new FoodItemImportFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, logicExecutorMock);
            Assert.That(foodItemImportFromDataProviderCommandHandler, Is.Not.Null);

            var command = new FoodItemImportFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = fixture.Create<string>(),
                Name = name,
                PrimaryFoodGroupIdentifier = Guid.NewGuid(),
                IsActive = fixture.Create<bool>()
            };

            foodItemImportFromDataProviderCommandHandler.Execute(command);

            logicExecutorMock.AssertWasCalled(m => m.TranslationAdd(Arg<ITranslation>.Is.NotNull));
        }

        /// <summary>
        /// Tests that Execute set the value for Value on the translation when a food item for the key does exist and it have the translation in the command.
        /// </summary>
        [Test]
        public void TestThatExecuteSetsValueOnTranslationWhenFoodItemForKeyDoesExistAndFoodItemForKeyDoesHaveTranslationInCommand()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var updatedFoodItemMock = DomainObjectMockBuilder.BuildFoodItemMock();
            var translationMock = updatedFoodItemMock.Translations.First();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(translationMock.TranslationInfo)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<IFoodGroup>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodItemGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodItemMock(translations: new List<ITranslation>(0)))
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Update(Arg<IFoodItem>.Is.Anything))
                .Return(updatedFoodItemMock)
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutorMock.Stub(m => m.TranslationModify(Arg<ITranslation>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var foodItemImportFromDataProviderCommandHandler = new FoodItemImportFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, logicExecutorMock);
            Assert.That(foodItemImportFromDataProviderCommandHandler, Is.Not.Null);

            var command = new FoodItemImportFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = fixture.Create<string>(),
                Name = fixture.Create<string>(),
                PrimaryFoodGroupIdentifier = Guid.NewGuid(),
                IsActive = fixture.Create<bool>()
            };

            foodItemImportFromDataProviderCommandHandler.Execute(command);

            translationMock.AssertWasCalled(m => m.Value = Arg<string>.Is.Equal(command.Name));
        }

        /// <summary>
        /// Tests that Execute calls TranslationModify on the logic exeuctor when a food item for the key does exist and it have the translation in the command.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsTranslationModifyOnLogicExecutorWhenFoodItemForKeyDoesExistAndFoodItemForKeyDoesHaveTranslationInCommand()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var updatedFoodItemMock = DomainObjectMockBuilder.BuildFoodItemMock();
            var translationMock = updatedFoodItemMock.Translations.First();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(translationMock.TranslationInfo)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<IFoodGroup>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodItemGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodItemMock(translations: new List<ITranslation>(0)))
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Update(Arg<IFoodItem>.Is.Anything))
                .Return(updatedFoodItemMock)
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutorMock.Stub(m => m.TranslationModify(Arg<ITranslation>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var foodItemImportFromDataProviderCommandHandler = new FoodItemImportFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, logicExecutorMock);
            Assert.That(foodItemImportFromDataProviderCommandHandler, Is.Not.Null);

            var command = new FoodItemImportFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = fixture.Create<string>(),
                Name = fixture.Create<string>(),
                PrimaryFoodGroupIdentifier = Guid.NewGuid(),
                IsActive = fixture.Create<bool>()
            };

            foodItemImportFromDataProviderCommandHandler.Execute(command);

            logicExecutorMock.AssertWasCalled(m => m.TranslationModify(Arg<ITranslation>.Is.Equal(translationMock)));
        }

        /// <summary>
        /// Tests that Execute calls Map for the updated food item on the object mapper for the food waste domain when a food item for the key does exist.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsMapForUpdatedFoodItemOnFoodWasteObjectMapperWhenFoodItemForKeyDoesExist()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var updatedFoodItemMock = DomainObjectMockBuilder.BuildFoodItemMock(translations: new List<ITranslation>(0));
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<IFoodGroup>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodItemGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodItemMock(translations: new List<ITranslation>(0)))
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Update(Arg<IFoodItem>.Is.Anything))
                .Return(updatedFoodItemMock)
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutorMock.Stub(m => m.TranslationAdd(Arg<ITranslation>.Is.NotNull))
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var foodItemImportFromDataProviderCommandHandler = new FoodItemImportFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, logicExecutorMock);
            Assert.That(foodItemImportFromDataProviderCommandHandler, Is.Not.Null);

            var command = new FoodItemImportFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = fixture.Create<string>(),
                Name = fixture.Create<string>(),
                PrimaryFoodGroupIdentifier = Guid.NewGuid(),
                IsActive = fixture.Create<bool>()
            };

            foodItemImportFromDataProviderCommandHandler.Execute(command);

            foodWasteObjectMapperMock.AssertWasCalled(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Equal(updatedFoodItemMock), Arg<CultureInfo>.Is.Null));
        }

        /// <summary>
        /// Tests that Execute returns the service receipt created by Map for the updated food item on the object mapper for the food waste domain when a food item for the key does exist.
        /// </summary>
        [Test]
        public void TestThatExecuteReturnsServiceReceiptFromMapOnFoodWasteObjectMapperWhenFoodItemForKeyDoesExist()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<IFoodGroup>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodItemGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodItemMock(translations: new List<ITranslation>(0)))
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Update(Arg<IFoodItem>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodItemMock(translations: new List<ITranslation>(0)))
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutorMock.Stub(m => m.TranslationAdd(Arg<ITranslation>.Is.NotNull))
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var serviceReceipt = fixture.Create<ServiceReceiptResponse>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(serviceReceipt)
                .Repeat.Any();

            var foodItemImportFromDataProviderCommandHandler = new FoodItemImportFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, logicExecutorMock);
            Assert.That(foodItemImportFromDataProviderCommandHandler, Is.Not.Null);

            var command = new FoodItemImportFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = fixture.Create<string>(),
                Name = fixture.Create<string>(),
                PrimaryFoodGroupIdentifier = Guid.NewGuid(),
                IsActive = fixture.Create<bool>()
            };

            var result = foodItemImportFromDataProviderCommandHandler.Execute(command);
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
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();

            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();
            exceptionBuilderMock.Stub(m => m.Build(Arg<Exception>.Is.Anything, Arg<MethodBase>.Is.NotNull))
                .WhenCalled(e =>
                {
                    var methodBase = (MethodBase) e.Arguments.ElementAt(1);
                    Assert.That(methodBase, Is.Not.Null);
                    Assert.That(methodBase.ReflectedType.Name, Is.EqualTo(typeof (FoodItemImportFromDataProviderCommandHandler).Name));
                })
                .Return(fixture.Create<Exception>())
                .Repeat.Any();

            var foodItemImportFromDataProviderCommandHandler = new FoodItemImportFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, logicExecutorMock);
            Assert.That(foodItemImportFromDataProviderCommandHandler, Is.Not.Null);

            var exception = fixture.Create<Exception>();
            Assert.Throws<Exception>(() => foodItemImportFromDataProviderCommandHandler.HandleException(fixture.Create<FoodItemImportFromDataProviderCommand>(), exception));

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
            var logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();

            var exceptionToThrow = fixture.Create<Exception>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();
            exceptionBuilderMock.Stub(m => m.Build(Arg<Exception>.Is.Anything, Arg<MethodBase>.Is.Anything))
                .Return(exceptionToThrow)
                .Repeat.Any();

            var foodItemImportFromDataProviderCommandHandler = new FoodItemImportFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, logicExecutorMock);
            Assert.That(foodItemImportFromDataProviderCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<Exception>(() => foodItemImportFromDataProviderCommandHandler.HandleException(fixture.Create<FoodItemImportFromDataProviderCommand>(), fixture.Create<Exception>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception, Is.EqualTo(exceptionToThrow));
        }
    }
}
