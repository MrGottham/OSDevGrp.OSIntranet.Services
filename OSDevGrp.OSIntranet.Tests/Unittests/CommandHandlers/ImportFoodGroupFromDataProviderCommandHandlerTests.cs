using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommandHandlers;
using OSDevGrp.OSIntranet.CommandHandlers.Core;
using OSDevGrp.OSIntranet.CommandHandlers.Validation;
using OSDevGrp.OSIntranet.Contracts.Commands;
using OSDevGrp.OSIntranet.Contracts.Responses;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Resources;
using OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.CommandHandlers
{
    /// <summary>
    /// Tests the command handler which handles a command for importing a food group from a given data provider.
    /// </summary>
    [TestFixture]
    public class ImportFoodGroupFromDataProviderCommandHandlerTests
    {
        /// <summary>
        /// Tests that the constructor initialize a command handler which handles a command for importing a food group from a given data provider.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeImportFoodGroupFromDataProviderCommandHandler()
        {
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutor = MockRepository.GenerateMock<ILogicExecutor>();

            var importFoodGroupFromDataProviderCommandHandler = new ImportFoodGroupFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(importFoodGroupFromDataProviderCommandHandler, Is.Not.Null);
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

            var exception = Assert.Throws<ArgumentNullException>(() =>  new ImportFoodGroupFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("logicExecutor"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Execute throws an ArgumentNullException if the command for importing a food group from a given data provider is null.
        /// </summary>
        [Test]
        public void TestThatExecuteThrowsArgumentNullExceptionIfFoodGroupImportFromDataProviderCommandIsNull()
        {
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutor = MockRepository.GenerateMock<ILogicExecutor>();

            var importFoodGroupFromDataProviderCommandHandler = new ImportFoodGroupFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(importFoodGroupFromDataProviderCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => importFoodGroupFromDataProviderCommandHandler.Execute(null));
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

            var key = fixture.Create<string>();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodGroupGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Equal(key)))
                .Return(null)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<IFoodGroup>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock(translations: new List<Translation>(0)))
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var logicExecutor = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutor.Stub(m => m.ForeignKeyAdd(Arg<IForeignKey>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();
            logicExecutor.Stub(m => m.TranslationAdd(Arg<ITranslation>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var importFoodGroupFromDataProviderCommandHandler = new ImportFoodGroupFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(importFoodGroupFromDataProviderCommandHandler, Is.Not.Null);

            var foodGroupImportFromDataProviderCommand = new FoodGroupImportFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = key,
                Name = fixture.Create<string>(),
                IsActive = fixture.Create<bool>(),
                ParentKey = null
            };

            importFoodGroupFromDataProviderCommandHandler.Execute(foodGroupImportFromDataProviderCommand);

            systemDataRepositoryMock.AssertWasCalled(m => m.Get<IDataProvider>(Arg<Guid>.Is.Equal(foodGroupImportFromDataProviderCommand.DataProviderIdentifier)));
        }

        /// <summary>
        /// Tests that Execute calls Get to get the translation informations on the repository which can access system data for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsGetForTranslationInfoIdentifierOnSystemDataRepository()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var key = fixture.Create<string>();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodGroupGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Equal(key)))
                .Return(null)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<IFoodGroup>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock(translations: new List<Translation>(0)))
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var logicExecutor = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutor.Stub(m => m.ForeignKeyAdd(Arg<IForeignKey>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();
            logicExecutor.Stub(m => m.TranslationAdd(Arg<ITranslation>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var importFoodGroupFromDataProviderCommandHandler = new ImportFoodGroupFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(importFoodGroupFromDataProviderCommandHandler, Is.Not.Null);

            var foodGroupImportFromDataProviderCommand = new FoodGroupImportFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = key,
                Name = fixture.Create<string>(),
                IsActive = fixture.Create<bool>(),
                ParentKey = null
            };

            importFoodGroupFromDataProviderCommandHandler.Execute(foodGroupImportFromDataProviderCommand);

            systemDataRepositoryMock.AssertWasCalled(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Equal(foodGroupImportFromDataProviderCommand.TranslationInfoIdentifier)));
        }

        /// <summary>
        /// Tests that Execute does not call FoodGroupGetByForeignKey to get the parent food group on the repository which can access system data for the food waste domain when the parent key has no value.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestThatExecuteDontCallFoodGroupGetByForeignKeyForParentKeyOnSystemDataRepositoryWhenParentKeyHasNoValue(string noValue)
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var key = fixture.Create<string>();
            var dataProviderMock = DomainObjectMockBuilder.BuildDataProviderMock();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(dataProviderMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodGroupGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Equal(key)))
                .Return(null)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<IFoodGroup>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock(translations: new List<Translation>(0)))
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var logicExecutor = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutor.Stub(m => m.ForeignKeyAdd(Arg<IForeignKey>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();
            logicExecutor.Stub(m => m.TranslationAdd(Arg<ITranslation>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var importFoodGroupFromDataProviderCommandHandler = new ImportFoodGroupFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(importFoodGroupFromDataProviderCommandHandler, Is.Not.Null);

            var foodGroupImportFromDataProviderCommand = new FoodGroupImportFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = key,
                Name = fixture.Create<string>(),
                IsActive = fixture.Create<bool>(),
                ParentKey = noValue
            };

            importFoodGroupFromDataProviderCommandHandler.Execute(foodGroupImportFromDataProviderCommand);

            systemDataRepositoryMock.AssertWasNotCalled(m => m.FoodGroupGetByForeignKey(Arg<IDataProvider>.Is.Equal(dataProviderMock), Arg<string>.Is.Equal(foodGroupImportFromDataProviderCommand.ParentKey)));
        }

        /// <summary>
        /// Tests that Execute calls FoodGroupGetByForeignKey to get the parent food group on the repository which can access system data for the food waste domain when parent key has a value.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsFoodGroupGetByForeignKeyForParentKeyOnSystemDataRepositoryWhenParentKeyHasValue()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var key = fixture.Create<string>();
            var parentKey = fixture.Create<string>();
            var dataProviderMock = DomainObjectMockBuilder.BuildDataProviderMock();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(dataProviderMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodGroupGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Equal(key)))
                .Return(null)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodGroupGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Equal(parentKey)))
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<IFoodGroup>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock(translations: new List<Translation>(0)))
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var logicExecutor = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutor.Stub(m => m.ForeignKeyAdd(Arg<IForeignKey>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();
            logicExecutor.Stub(m => m.TranslationAdd(Arg<ITranslation>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var importFoodGroupFromDataProviderCommandHandler = new ImportFoodGroupFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(importFoodGroupFromDataProviderCommandHandler, Is.Not.Null);

            var foodGroupImportFromDataProviderCommand = new FoodGroupImportFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = key,
                Name = fixture.Create<string>(),
                IsActive = fixture.Create<bool>(),
                ParentKey = parentKey
            };

            importFoodGroupFromDataProviderCommandHandler.Execute(foodGroupImportFromDataProviderCommand);

            systemDataRepositoryMock.AssertWasCalled(m => m.FoodGroupGetByForeignKey(Arg<IDataProvider>.Is.Equal(dataProviderMock), Arg<string>.Is.Equal(foodGroupImportFromDataProviderCommand.ParentKey)));
        }

        /// <summary>
        /// Test that Execute calls IsNotNull for the data provider on the common validations.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsIsNotNullForDataProviderOnCommonValidations()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var key = fixture.Create<string>();
            var dataProviderMock = DomainObjectMockBuilder.BuildDataProviderMock();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(dataProviderMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodGroupGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Equal(key)))
                .Return(null)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<IFoodGroup>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock(translations: new List<Translation>(0)))
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

            var logicExecutor = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutor.Stub(m => m.ForeignKeyAdd(Arg<IForeignKey>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();
            logicExecutor.Stub(m => m.TranslationAdd(Arg<ITranslation>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var importFoodGroupFromDataProviderCommandHandler = new ImportFoodGroupFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(importFoodGroupFromDataProviderCommandHandler, Is.Not.Null);

            var foodGroupImportFromDataProviderCommand = new FoodGroupImportFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = key,
                Name = fixture.Create<string>(),
                IsActive = fixture.Create<bool>(),
                ParentKey = null
            };

            importFoodGroupFromDataProviderCommandHandler.Execute(foodGroupImportFromDataProviderCommand);

            commonValidationsMock.AssertWasCalled(m => m.IsNotNull(Arg<IDataProvider>.Is.Equal(dataProviderMock)));
        }

        /// <summary>
        /// Test that Execute calls IsNotNull for the translation informations on the common validations.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsIsNotNullForTranslationInfoOnCommonValidations()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var key = fixture.Create<string>();
            var translationInfoMock = DomainObjectMockBuilder.BuildTranslationInfoMock();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(translationInfoMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodGroupGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Equal(key)))
                .Return(null)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<IFoodGroup>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock(translations: new List<Translation>(0)))
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

            var logicExecutor = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutor.Stub(m => m.ForeignKeyAdd(Arg<IForeignKey>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();
            logicExecutor.Stub(m => m.TranslationAdd(Arg<ITranslation>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var importFoodGroupFromDataProviderCommandHandler = new ImportFoodGroupFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(importFoodGroupFromDataProviderCommandHandler, Is.Not.Null);

            var foodGroupImportFromDataProviderCommand = new FoodGroupImportFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = key,
                Name = fixture.Create<string>(),
                IsActive = fixture.Create<bool>(),
                ParentKey = null
            };

            importFoodGroupFromDataProviderCommandHandler.Execute(foodGroupImportFromDataProviderCommand);

            commonValidationsMock.AssertWasCalled(m => m.IsNotNull(Arg<ITranslation>.Is.Equal(translationInfoMock)));
        }

        /// <summary>
        /// Test that Execute calls HasValue for the food group key on the common validations.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsHasValueForKeyOnCommonValidations()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var key = fixture.Create<string>();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodGroupGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Equal(key)))
                .Return(null)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<IFoodGroup>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock(translations: new List<Translation>(0)))
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

            var logicExecutor = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutor.Stub(m => m.ForeignKeyAdd(Arg<IForeignKey>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();
            logicExecutor.Stub(m => m.TranslationAdd(Arg<ITranslation>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var importFoodGroupFromDataProviderCommandHandler = new ImportFoodGroupFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(importFoodGroupFromDataProviderCommandHandler, Is.Not.Null);

            var foodGroupImportFromDataProviderCommand = new FoodGroupImportFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = key,
                Name = fixture.Create<string>(),
                IsActive = fixture.Create<bool>(),
                ParentKey = null
            };

            importFoodGroupFromDataProviderCommandHandler.Execute(foodGroupImportFromDataProviderCommand);

            commonValidationsMock.AssertWasCalled(m => m.HasValue(Arg<string>.Is.Equal(foodGroupImportFromDataProviderCommand.Key)));
        }

        /// <summary>
        /// Test that Execute calls ContainsIllegalChar for the food group key on the common validations.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsContainsIllegalCharForKeyOnCommonValidations()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var key = fixture.Create<string>();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodGroupGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Equal(key)))
                .Return(null)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<IFoodGroup>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock(translations: new List<Translation>(0)))
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

            var logicExecutor = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutor.Stub(m => m.ForeignKeyAdd(Arg<IForeignKey>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();
            logicExecutor.Stub(m => m.TranslationAdd(Arg<ITranslation>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var importFoodGroupFromDataProviderCommandHandler = new ImportFoodGroupFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(importFoodGroupFromDataProviderCommandHandler, Is.Not.Null);

            var foodGroupImportFromDataProviderCommand = new FoodGroupImportFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = key,
                Name = fixture.Create<string>(),
                IsActive = fixture.Create<bool>(),
                ParentKey = null
            };

            importFoodGroupFromDataProviderCommandHandler.Execute(foodGroupImportFromDataProviderCommand);

            commonValidationsMock.AssertWasCalled(m => m.ContainsIllegalChar(Arg<string>.Is.Equal(foodGroupImportFromDataProviderCommand.Key)));
        }

        /// <summary>
        /// Test that Execute calls HasValue for the food group name on the common validations.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsHasValueForNameOnCommonValidations()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var key = fixture.Create<string>();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodGroupGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Equal(key)))
                .Return(null)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<IFoodGroup>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock(translations: new List<Translation>(0)))
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

            var logicExecutor = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutor.Stub(m => m.ForeignKeyAdd(Arg<IForeignKey>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();
            logicExecutor.Stub(m => m.TranslationAdd(Arg<ITranslation>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var importFoodGroupFromDataProviderCommandHandler = new ImportFoodGroupFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(importFoodGroupFromDataProviderCommandHandler, Is.Not.Null);

            var foodGroupImportFromDataProviderCommand = new FoodGroupImportFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = key,
                Name = fixture.Create<string>(),
                IsActive = fixture.Create<bool>(),
                ParentKey = null
            };

            importFoodGroupFromDataProviderCommandHandler.Execute(foodGroupImportFromDataProviderCommand);

            commonValidationsMock.AssertWasCalled(m => m.HasValue(Arg<string>.Is.Equal(foodGroupImportFromDataProviderCommand.Name)));
        }

        /// <summary>
        /// Test that Execute calls ContainsIllegalChar for the food group name on the common validations.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsContainsIllegalCharForNameOnCommonValidations()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var key = fixture.Create<string>();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodGroupGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Equal(key)))
                .Return(null)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<IFoodGroup>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock(translations: new List<Translation>(0)))
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

            var logicExecutor = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutor.Stub(m => m.ForeignKeyAdd(Arg<IForeignKey>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();
            logicExecutor.Stub(m => m.TranslationAdd(Arg<ITranslation>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var importFoodGroupFromDataProviderCommandHandler = new ImportFoodGroupFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(importFoodGroupFromDataProviderCommandHandler, Is.Not.Null);

            var foodGroupImportFromDataProviderCommand = new FoodGroupImportFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = key,
                Name = fixture.Create<string>(),
                IsActive = fixture.Create<bool>(),
                ParentKey = null
            };

            importFoodGroupFromDataProviderCommandHandler.Execute(foodGroupImportFromDataProviderCommand);

            commonValidationsMock.AssertWasCalled(m => m.ContainsIllegalChar(Arg<string>.Is.Equal(foodGroupImportFromDataProviderCommand.Name)));
        }

        /// <summary>
        /// Test that Execute calls IsNotNull for the parent food group on the common validations.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsIsNotNullForParentFoodGroupOnCommonValidations()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var key = fixture.Create<string>();
            var parentKey = fixture.Create<string>();
            var parentFoodGroup = DomainObjectMockBuilder.BuildFoodGroupMock();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodGroupGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Equal(key)))
                .Return(null)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodGroupGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Equal(parentKey)))
                .Return(parentFoodGroup)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<IFoodGroup>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock(translations: new List<Translation>(0)))
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

            var logicExecutor = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutor.Stub(m => m.ForeignKeyAdd(Arg<IForeignKey>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();
            logicExecutor.Stub(m => m.TranslationAdd(Arg<ITranslation>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var importFoodGroupFromDataProviderCommandHandler = new ImportFoodGroupFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(importFoodGroupFromDataProviderCommandHandler, Is.Not.Null);

            var foodGroupImportFromDataProviderCommand = new FoodGroupImportFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = key,
                Name = fixture.Create<string>(),
                IsActive = fixture.Create<bool>(),
                ParentKey = parentKey
            };

            importFoodGroupFromDataProviderCommandHandler.Execute(foodGroupImportFromDataProviderCommand);

            commonValidationsMock.AssertWasCalled(m => m.IsNotNull(Arg<IFoodGroup>.Is.Equal(parentFoodGroup)));
        }

        /// <summary>
        /// Test that Execute calls IsSatisfiedBy on the specification which encapsulates validation rules 7 times.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsIsSatisfiedByOnSpecification7Times()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var key = fixture.Create<string>();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodGroupGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Equal(key)))
                .Return(null)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<IFoodGroup>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock(translations: new List<Translation>(0)))
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var logicExecutor = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutor.Stub(m => m.ForeignKeyAdd(Arg<IForeignKey>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();
            logicExecutor.Stub(m => m.TranslationAdd(Arg<ITranslation>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var importFoodGroupFromDataProviderCommandHandler = new ImportFoodGroupFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(importFoodGroupFromDataProviderCommandHandler, Is.Not.Null);

            var foodGroupImportFromDataProviderCommand = new FoodGroupImportFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = key,
                Name = fixture.Create<string>(),
                IsActive = fixture.Create<bool>(),
                ParentKey = null
            };

            importFoodGroupFromDataProviderCommandHandler.Execute(foodGroupImportFromDataProviderCommand);

            specificationMock.AssertWasCalled(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<IntranetBusinessException>.Is.TypeOf), opt => opt.Repeat.Times(7));
        }

        /// <summary>
        /// Test that Execute calls Evaluate on the specification which encapsulates validation rules.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsEvaluateOnSpecification()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var key = fixture.Create<string>();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodGroupGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Equal(key)))
                .Return(null)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<IFoodGroup>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock(translations: new List<Translation>(0)))
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var logicExecutor = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutor.Stub(m => m.ForeignKeyAdd(Arg<IForeignKey>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();
            logicExecutor.Stub(m => m.TranslationAdd(Arg<ITranslation>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var importFoodGroupFromDataProviderCommandHandler = new ImportFoodGroupFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(importFoodGroupFromDataProviderCommandHandler, Is.Not.Null);

            var foodGroupImportFromDataProviderCommand = new FoodGroupImportFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = key,
                Name = fixture.Create<string>(),
                IsActive = fixture.Create<bool>(),
                ParentKey = null
            };

            importFoodGroupFromDataProviderCommandHandler.Execute(foodGroupImportFromDataProviderCommand);

            specificationMock.AssertWasCalled(m => m.Evaluate());
        }

        /// <summary>
        /// Test that Execute calls FoodGroupGetByForeignKey to get the food group on the repository which can access system data for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsFoodGroupGetByForeignKeyForKeyOnSystemDataRepository()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var key = fixture.Create<string>();
            var dataProviderMock = DomainObjectMockBuilder.BuildDataProviderMock();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(dataProviderMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodGroupGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Equal(key)))
                .Return(null)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<IFoodGroup>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock(translations: new List<Translation>(0)))
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var logicExecutor = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutor.Stub(m => m.ForeignKeyAdd(Arg<IForeignKey>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();
            logicExecutor.Stub(m => m.TranslationAdd(Arg<ITranslation>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var importFoodGroupFromDataProviderCommandHandler = new ImportFoodGroupFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(importFoodGroupFromDataProviderCommandHandler, Is.Not.Null);

            var foodGroupImportFromDataProviderCommand = new FoodGroupImportFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = key,
                Name = fixture.Create<string>(),
                IsActive = fixture.Create<bool>(),
                ParentKey = null
            };

            importFoodGroupFromDataProviderCommandHandler.Execute(foodGroupImportFromDataProviderCommand);

            systemDataRepositoryMock.AssertWasCalled(m => m.FoodGroupGetByForeignKey(Arg<IDataProvider>.Is.Equal(dataProviderMock), Arg<string>.Is.Equal(foodGroupImportFromDataProviderCommand.Key)));
        }

        /// <summary>
        /// Test that Execute calls Insert on the repository which can access system data for the food waste domain when a food group for the key does not exist.
        /// </summary>
        [Test]
        [TestCase(false)]
        [TestCase(true)]
        public void TestThatExecuteCallsInsertOnSystemDataRepositoryWhenFoodGroupForKeyDoesNotExist(bool hasParent)
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var key = fixture.Create<string>();
            var parentKey = fixture.Create<string>();
            var parentFoodGroup = DomainObjectMockBuilder.BuildFoodGroupMock();
            var isActive = fixture.Create<bool>();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodGroupGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Equal(key)))
                .Return(null)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodGroupGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Equal(parentKey)))
                .Return(parentFoodGroup)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<IFoodGroup>.Is.NotNull))
                .WhenCalled(e =>
                {
                    var foodGroup = (IFoodGroup) e.Arguments.ElementAt(0);
                    Assert.That(foodGroup, Is.Not.Null);
                    Assert.That(foodGroup.Identifier, Is.Null);
                    Assert.That(foodGroup.Identifier.HasValue, Is.False);
                    Assert.That(foodGroup.IsActive, Is.EqualTo(isActive));
                    if (hasParent)
                    {
                        Assert.That(foodGroup.Parent, Is.Not.Null);
                        Assert.That(foodGroup.Parent, Is.EqualTo(parentFoodGroup));
                    }
                    else
                    {
                        Assert.That(foodGroup.Parent, Is.Null);
                    }
                })
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock(translations: new List<Translation>(0)))
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var logicExecutor = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutor.Stub(m => m.ForeignKeyAdd(Arg<IForeignKey>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();
            logicExecutor.Stub(m => m.TranslationAdd(Arg<ITranslation>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var importFoodGroupFromDataProviderCommandHandler = new ImportFoodGroupFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(importFoodGroupFromDataProviderCommandHandler, Is.Not.Null);

            var foodGroupImportFromDataProviderCommand = new FoodGroupImportFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = key,
                Name = fixture.Create<string>(),
                IsActive = isActive,
                ParentKey = hasParent ? parentKey : null
            };

            importFoodGroupFromDataProviderCommandHandler.Execute(foodGroupImportFromDataProviderCommand);

            systemDataRepositoryMock.AssertWasCalled(m => m.Insert(Arg<IFoodGroup>.Is.NotNull));
        }

        /// <summary>
        /// Test that Execute calls ForeignKeyAdd on the logic exeuctor when a food group for the key does not exist.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsForeignKeyAddOnLogicExecutorWhenFoodGroupForKeyDoesNotExist()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var key = fixture.Create<string>();
            var dataProviderMock = DomainObjectMockBuilder.BuildDataProviderMock();
            var insertedFoodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock(translations: new List<Translation>(0));
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(dataProviderMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodGroupGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Equal(key)))
                .Return(null)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<IFoodGroup>.Is.Anything))
                .Return(insertedFoodGroupMock)
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var logicExecutor = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutor.Stub(m => m.ForeignKeyAdd(Arg<IForeignKey>.Is.NotNull))
                .WhenCalled(e =>
                {
                    var foreignKey = (IForeignKey) e.Arguments.ElementAt(0);
                    Assert.That(foreignKey, Is.Not.Null);
                    Assert.That(foreignKey.Identifier, Is.Null);
                    Assert.That(foreignKey.Identifier.HasValue, Is.False);
                    Assert.That(foreignKey.DataProvider, Is.Not.Null);
                    Assert.That(foreignKey.DataProvider, Is.EqualTo(dataProviderMock));
                    // ReSharper disable PossibleInvalidOperationException
                    Assert.That(foreignKey.ForeignKeyForIdentifier, Is.EqualTo(insertedFoodGroupMock.Identifier.Value));
                    // ReSharper restore PossibleInvalidOperationException
                    Assert.That(foreignKey.ForeignKeyForTypes, Is.Not.Null);
                    Assert.That(foreignKey.ForeignKeyForTypes, Is.Not.Empty);
                    Assert.That(foreignKey.ForeignKeyForTypes.Contains(typeof (IFoodGroup)), Is.True);
                    Assert.That(foreignKey.ForeignKeyValue, Is.Not.Null);
                    Assert.That(foreignKey.ForeignKeyValue, Is.Not.Empty);
                    Assert.That(foreignKey.ForeignKeyValue, Is.EqualTo(key));
                })
                .Return(Guid.NewGuid())
                .Repeat.Any();
            logicExecutor.Stub(m => m.TranslationAdd(Arg<ITranslation>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var importFoodGroupFromDataProviderCommandHandler = new ImportFoodGroupFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(importFoodGroupFromDataProviderCommandHandler, Is.Not.Null);

            var foodGroupImportFromDataProviderCommand = new FoodGroupImportFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = key,
                Name = fixture.Create<string>(),
                IsActive = fixture.Create<bool>(),
                ParentKey = null
            };

            importFoodGroupFromDataProviderCommandHandler.Execute(foodGroupImportFromDataProviderCommand);

            logicExecutor.AssertWasCalled(m => m.ForeignKeyAdd(Arg<IForeignKey>.Is.NotNull));
        }

        /// <summary>
        /// Test that Execute calls TranslationAdd on the logic exeuctor when a food group for the key does not exist.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsTranslationAddOnLogicExecutorWhenFoodGroupForKeyDoesNotExist()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var key = fixture.Create<string>();
            var translationInfoMock = DomainObjectMockBuilder.BuildTranslationInfoMock();
            var insertedFoodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock(translations: new List<ITranslation>(0));
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(translationInfoMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodGroupGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Equal(key)))
                .Return(null)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<IFoodGroup>.Is.Anything))
                .Return(insertedFoodGroupMock)
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var name = fixture.Create<string>();
            var logicExecutor = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutor.Stub(m => m.ForeignKeyAdd(Arg<IForeignKey>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();
            logicExecutor.Stub(m => m.TranslationAdd(Arg<ITranslation>.Is.NotNull))
                .WhenCalled(e =>
                {
                    var translation = (ITranslation) e.Arguments.ElementAt(0);
                    Assert.That(translation, Is.Not.Null);
                    Assert.That(translation.Identifier, Is.Null);
                    Assert.That(translation.Identifier.HasValue, Is.False);
                    Assert.That(translation.TranslationInfo, Is.Not.Null);
                    Assert.That(translation.TranslationInfo, Is.EqualTo(translationInfoMock));
                    // ReSharper disable PossibleInvalidOperationException
                    Assert.That(translation.TranslationOfIdentifier, Is.EqualTo(insertedFoodGroupMock.Identifier.Value));
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

            var importFoodGroupFromDataProviderCommandHandler = new ImportFoodGroupFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(importFoodGroupFromDataProviderCommandHandler, Is.Not.Null);

            var foodGroupImportFromDataProviderCommand = new FoodGroupImportFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = key,
                Name = name,
                IsActive = fixture.Create<bool>(),
                ParentKey = null
            };

            importFoodGroupFromDataProviderCommandHandler.Execute(foodGroupImportFromDataProviderCommand);

            logicExecutor.AssertWasCalled(m => m.TranslationAdd(Arg<ITranslation>.Is.NotNull));
        }

        /// <summary>
        /// Test that Execute calls Map for the inserted food group on the object mapper for the food waste domain when a food group for the key does not exist.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsMapForInsertedFoodGroupOnFoodWasteObjectMapperWhenFoodGroupForKeyDoesNotExist()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var key = fixture.Create<string>();
            var insertedFoodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock(translations: new List<ITranslation>(0));
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodGroupGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Equal(key)))
                .Return(null)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<IFoodGroup>.Is.Anything))
                .Return(insertedFoodGroupMock)
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var logicExecutor = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutor.Stub(m => m.ForeignKeyAdd(Arg<IForeignKey>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();
            logicExecutor.Stub(m => m.TranslationAdd(Arg<ITranslation>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var importFoodGroupFromDataProviderCommandHandler = new ImportFoodGroupFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(importFoodGroupFromDataProviderCommandHandler, Is.Not.Null);

            var foodGroupImportFromDataProviderCommand = new FoodGroupImportFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = key,
                Name = fixture.Create<string>(),
                IsActive = fixture.Create<bool>(),
                ParentKey = null
            };

            importFoodGroupFromDataProviderCommandHandler.Execute(foodGroupImportFromDataProviderCommand);

            foodWasteObjectMapperMock.AssertWasCalled(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Equal(insertedFoodGroupMock), Arg<CultureInfo>.Is.Null));
        }

        /// <summary>
        /// Test that Execute returns the service receipt created by Map for the inserted food group on the object mapper for the food waste domain when a food group for the key does not exist.
        /// </summary>
        [Test]
        public void TestThatExecuteReturnsServiceReceiptFromMapOnFoodWasteObjectMapperWhenFoodGroupForKeyDoesNotExist()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var key = fixture.Create<string>();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodGroupGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Equal(key)))
                .Return(null)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<IFoodGroup>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock(translations: new List<ITranslation>(0)))
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var logicExecutor = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutor.Stub(m => m.ForeignKeyAdd(Arg<IForeignKey>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();
            logicExecutor.Stub(m => m.TranslationAdd(Arg<ITranslation>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var serviceReceipt = fixture.Create<ServiceReceiptResponse>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(serviceReceipt)
                .Repeat.Any();

            var importFoodGroupFromDataProviderCommandHandler = new ImportFoodGroupFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(importFoodGroupFromDataProviderCommandHandler, Is.Not.Null);

            var foodGroupImportFromDataProviderCommand = new FoodGroupImportFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = key,
                Name = fixture.Create<string>(),
                IsActive = fixture.Create<bool>(),
                ParentKey = null
            };

            var result = importFoodGroupFromDataProviderCommandHandler.Execute(foodGroupImportFromDataProviderCommand);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(serviceReceipt));
        }

        /// <summary>
        /// Test that Execute sets the value for IsActive with the value from the command when a food group for the key does exist.
        /// </summary>
        [Test]
        public void TestThatExecuteSetsIsActiveWithIsActiveFromCommandWhenFoodGroupForKeyDoesExist()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var key = fixture.Create<string>();
            var foodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock(translations: new List<ITranslation>(0));
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodGroupGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Equal(key)))
                .Return(foodGroupMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Update(Arg<IFoodGroup>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock(translations: new List<ITranslation>(0)))
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var logicExecutor = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutor.Stub(m => m.TranslationAdd(Arg<ITranslation>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var importFoodGroupFromDataProviderCommandHandler = new ImportFoodGroupFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(importFoodGroupFromDataProviderCommandHandler, Is.Not.Null);

            var foodGroupImportFromDataProviderCommand = new FoodGroupImportFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = key,
                Name = fixture.Create<string>(),
                IsActive = fixture.Create<bool>(),
                ParentKey = null
            };

            importFoodGroupFromDataProviderCommandHandler.Execute(foodGroupImportFromDataProviderCommand);

            foodGroupMock.AssertWasCalled(m => m.IsActive = Arg<bool>.Is.Equal(foodGroupImportFromDataProviderCommand.IsActive));
        }

        /// <summary>
        /// Test that Execute sets the value for Parent when a food group for the key does exist.
        /// </summary>
        [Test]
        [TestCase(false)]
        [TestCase(true)]
        public void TestThatExecuteSetsParentWhenFoodGroupForKeyDoesExist(bool hasParent)
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var key = fixture.Create<string>();
            var parentKey = fixture.Create<string>();
            var foodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock(translations: new List<ITranslation>(0));
            var parentFoodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodGroupGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Equal(key)))
                .Return(foodGroupMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodGroupGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Equal(parentKey)))
                .Return(parentFoodGroupMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Update(Arg<IFoodGroup>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock(translations: new List<ITranslation>(0)))
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var logicExecutor = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutor.Stub(m => m.TranslationAdd(Arg<ITranslation>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var importFoodGroupFromDataProviderCommandHandler = new ImportFoodGroupFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(importFoodGroupFromDataProviderCommandHandler, Is.Not.Null);

            var foodGroupImportFromDataProviderCommand = new FoodGroupImportFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = key,
                Name = fixture.Create<string>(),
                IsActive = fixture.Create<bool>(),
                ParentKey = hasParent ? parentKey : null
            };

            importFoodGroupFromDataProviderCommandHandler.Execute(foodGroupImportFromDataProviderCommand);

            foodGroupMock.AssertWasCalled(m => m.Parent = Arg<IFoodGroup>.Is.Equal(hasParent ? parentFoodGroupMock : null));
        }

        /// <summary>
        /// Test that Execute calls Update on the repository which can access system data for the food waste domain when a food group for the key does exist.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsUpdateOnSystemDataRepositoryWhenFoodGroupForKeyDoesExist()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var key = fixture.Create<string>();
            var foodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock(translations: new List<ITranslation>(0));
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodGroupGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Equal(key)))
                .Return(foodGroupMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Update(Arg<IFoodGroup>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock(translations: new List<ITranslation>(0)))
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var logicExecutor = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutor.Stub(m => m.TranslationAdd(Arg<ITranslation>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var importFoodGroupFromDataProviderCommandHandler = new ImportFoodGroupFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(importFoodGroupFromDataProviderCommandHandler, Is.Not.Null);

            var foodGroupImportFromDataProviderCommand = new FoodGroupImportFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = key,
                Name = fixture.Create<string>(),
                IsActive = fixture.Create<bool>(),
                ParentKey = null
            };

            importFoodGroupFromDataProviderCommandHandler.Execute(foodGroupImportFromDataProviderCommand);

            systemDataRepositoryMock.AssertWasCalled(m => m.Update(Arg<IFoodGroup>.Is.Equal(foodGroupMock)));
        }

        /// <summary>
        /// Test that Execute calls TranslationAdd on the logic exeuctor when a food group for the key does exist and it does not have the translation in the command.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsTranslationAddOnLogicExecutorWhenFoodGroupForKeyDoesExistAndFoodGroupForKeyDoesNotHaveTranslationInCommand()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var key = fixture.Create<string>();
            var translationInfoMock = DomainObjectMockBuilder.BuildTranslationInfoMock();
            var updatedFoodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock(translations: new List<ITranslation>(0));
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(translationInfoMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodGroupGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Equal(key)))
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock(translations: new List<ITranslation>(0)))
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Update(Arg<IFoodGroup>.Is.Anything))
                .Return(updatedFoodGroupMock)
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var name = fixture.Create<string>();
            var logicExecutor = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutor.Stub(m => m.TranslationAdd(Arg<ITranslation>.Is.NotNull))
                .WhenCalled(e =>
                {
                    var translation = (ITranslation) e.Arguments.ElementAt(0);
                    Assert.That(translation, Is.Not.Null);
                    Assert.That(translation.Identifier, Is.Null);
                    Assert.That(translation.Identifier.HasValue, Is.False);
                    Assert.That(translation.TranslationInfo, Is.Not.Null);
                    Assert.That(translation.TranslationInfo, Is.EqualTo(translationInfoMock));
                    // ReSharper disable PossibleInvalidOperationException
                    Assert.That(translation.TranslationOfIdentifier, Is.EqualTo(updatedFoodGroupMock.Identifier.Value));
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

            var importFoodGroupFromDataProviderCommandHandler = new ImportFoodGroupFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(importFoodGroupFromDataProviderCommandHandler, Is.Not.Null);

            var foodGroupImportFromDataProviderCommand = new FoodGroupImportFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = key,
                Name = name,
                IsActive = fixture.Create<bool>(),
                ParentKey = null
            };

            importFoodGroupFromDataProviderCommandHandler.Execute(foodGroupImportFromDataProviderCommand);

            logicExecutor.AssertWasCalled(m => m.TranslationAdd(Arg<ITranslation>.Is.NotNull));
        }

        /// <summary>
        /// Test that Execute set the value for Value on the translation when a food group for the key does exist and it have the translation in the command.
        /// </summary>
        [Test]
        public void TestThatExecuteSetsValueOnTranslationWhenFoodGroupForKeyDoesExistAndFoodGroupForKeyDoesHaveTranslationInCommand()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var key = fixture.Create<string>();
            var updatedFoodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock();
            var translationMock = updatedFoodGroupMock.Translations.First();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(translationMock.TranslationInfo)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodGroupGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Equal(key)))
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock(translations: new List<ITranslation>(0)))
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Update(Arg<IFoodGroup>.Is.Anything))
                .Return(updatedFoodGroupMock)
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var logicExecutor = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutor.Stub(m => m.TranslationModify(Arg<ITranslation>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var importFoodGroupFromDataProviderCommandHandler = new ImportFoodGroupFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(importFoodGroupFromDataProviderCommandHandler, Is.Not.Null);

            var foodGroupImportFromDataProviderCommand = new FoodGroupImportFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = key,
                Name = fixture.Create<string>(),
                IsActive = fixture.Create<bool>(),
                ParentKey = null
            };

            importFoodGroupFromDataProviderCommandHandler.Execute(foodGroupImportFromDataProviderCommand);

            translationMock.AssertWasCalled(m => m.Value = Arg<string>.Is.Equal(foodGroupImportFromDataProviderCommand.Name));
        }

        /// <summary>
        /// Test that Execute calls TranslationModify on the logic exeuctor when a food group for the key does exist and it have the translation in the command.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsTranslationModifyOnLogicExecutorWhenFoodGroupForKeyDoesExistAndFoodGroupForKeyDoesHaveTranslationInCommand()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var key = fixture.Create<string>();
            var updatedFoodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock();
            var translationMock = updatedFoodGroupMock.Translations.First();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(translationMock.TranslationInfo)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodGroupGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Equal(key)))
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock(translations: new List<ITranslation>(0)))
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Update(Arg<IFoodGroup>.Is.Anything))
                .Return(updatedFoodGroupMock)
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var logicExecutor = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutor.Stub(m => m.TranslationModify(Arg<ITranslation>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var importFoodGroupFromDataProviderCommandHandler = new ImportFoodGroupFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(importFoodGroupFromDataProviderCommandHandler, Is.Not.Null);

            var foodGroupImportFromDataProviderCommand = new FoodGroupImportFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = key,
                Name = fixture.Create<string>(),
                IsActive = fixture.Create<bool>(),
                ParentKey = null
            };

            importFoodGroupFromDataProviderCommandHandler.Execute(foodGroupImportFromDataProviderCommand);

            logicExecutor.AssertWasCalled(m => m.TranslationModify(Arg<ITranslation>.Is.Equal(translationMock)));
        }

        /// <summary>
        /// Test that Execute calls Map for the updated food group on the object mapper for the food waste domain when a food group for the key does exist.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsMapForUpdatedFoodGroupOnFoodWasteObjectMapperWhenFoodGroupForKeyDoesExist()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var key = fixture.Create<string>();
            var updatedFoodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock(translations: new List<ITranslation>(0));
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodGroupGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Equal(key)))
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock(translations: new List<ITranslation>(0)))
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Update(Arg<IFoodGroup>.Is.Anything))
                .Return(updatedFoodGroupMock)
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var logicExecutor = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutor.Stub(m => m.TranslationAdd(Arg<ITranslation>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var importFoodGroupFromDataProviderCommandHandler = new ImportFoodGroupFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(importFoodGroupFromDataProviderCommandHandler, Is.Not.Null);

            var foodGroupImportFromDataProviderCommand = new FoodGroupImportFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = key,
                Name = fixture.Create<string>(),
                IsActive = fixture.Create<bool>(),
                ParentKey = null
            };

            importFoodGroupFromDataProviderCommandHandler.Execute(foodGroupImportFromDataProviderCommand);

            foodWasteObjectMapperMock.AssertWasCalled(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Equal(updatedFoodGroupMock), Arg<CultureInfo>.Is.Null));
        }

        /// <summary>
        /// Test that Execute returns the service receipt created by Map for the updated food group on the object mapper for the food waste domain when a food group for the key does exist.
        /// </summary>
        [Test]
        public void TestThatExecuteReturnsServiceReceiptFromMapOnFoodWasteObjectMapperWhenFoodGroupForKeyDoesExist()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var key = fixture.Create<string>();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodGroupGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Equal(key)))
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock(translations: new List<ITranslation>(0)))
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Update(Arg<IFoodGroup>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock(translations: new List<ITranslation>(0)))
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var logicExecutor = MockRepository.GenerateMock<ILogicExecutor>();
            logicExecutor.Stub(m => m.TranslationAdd(Arg<ITranslation>.Is.Anything))
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var serviceReceipt = fixture.Create<ServiceReceiptResponse>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(serviceReceipt)
                .Repeat.Any();

            var importFoodGroupFromDataProviderCommandHandler = new ImportFoodGroupFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(importFoodGroupFromDataProviderCommandHandler, Is.Not.Null);

            var foodGroupImportFromDataProviderCommand = new FoodGroupImportFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = key,
                Name = fixture.Create<string>(),
                IsActive = fixture.Create<bool>(),
                ParentKey = null
            };

            var result = importFoodGroupFromDataProviderCommandHandler.Execute(foodGroupImportFromDataProviderCommand);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(serviceReceipt));
        }

        /// <summary>
        /// Tests that HandleException throws an ArgumentNullException if the command for importing a food group from a given data provider is null.
        /// </summary>
        [Test]
        public void TestThatHandleExceptionThrowsArgumentNullExceptionIfFoodGroupImportFromDataProviderCommandIsNull()
        {
            var fixture = new Fixture();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutor = MockRepository.GenerateMock<ILogicExecutor>();

            var importFoodGroupFromDataProviderCommandHandler = new ImportFoodGroupFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(importFoodGroupFromDataProviderCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => importFoodGroupFromDataProviderCommandHandler.HandleException(null, fixture.Create<Exception>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("command"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that HandleException throws an ArgumentNullException if the exception is null.
        /// </summary>
        [Test]
        public void TestThatHandleExceptionThrowsArgumentNullExceptionIfExceptionIsNull()
        {
            var fixture = new Fixture();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutor = MockRepository.GenerateMock<ILogicExecutor>();

            var importFoodGroupFromDataProviderCommandHandler = new ImportFoodGroupFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(importFoodGroupFromDataProviderCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => importFoodGroupFromDataProviderCommandHandler.HandleException(fixture.Create<FoodGroupImportFromDataProviderCommand>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("exception"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that HandleException rethrows the exception when the exception if type of IntranetRepositoryException.
        /// </summary>
        [Test]
        public void TestThatHandleExceptionRethrowsExceptionWhenExceptionIsTypeOfIntranetRepositoryException()
        {
            var fixture = new Fixture();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutor = MockRepository.GenerateMock<ILogicExecutor>();

            var importFoodGroupFromDataProviderCommandHandler = new ImportFoodGroupFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(importFoodGroupFromDataProviderCommandHandler, Is.Not.Null);

            var incomingException = fixture.Create<IntranetRepositoryException>();

            var exception = Assert.Throws<IntranetRepositoryException>(() => importFoodGroupFromDataProviderCommandHandler.HandleException(fixture.Create<FoodGroupImportFromDataProviderCommand>(), incomingException));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception, Is.EqualTo(incomingException));
        }

        /// <summary>
        /// Tests that HandleException rethrows the exception when the exception if type of IntranetBusinessException.
        /// </summary>
        [Test]
        public void TestThatHandleExceptionRethrowsExceptionWhenExceptionIsTypeOfIntranetBusinessException()
        {
            var fixture = new Fixture();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutor = MockRepository.GenerateMock<ILogicExecutor>();

            var importFoodGroupFromDataProviderCommandHandler = new ImportFoodGroupFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(importFoodGroupFromDataProviderCommandHandler, Is.Not.Null);

            var incomingException = fixture.Create<IntranetBusinessException>();

            var exception = Assert.Throws<IntranetBusinessException>(() => importFoodGroupFromDataProviderCommandHandler.HandleException(fixture.Create<FoodGroupImportFromDataProviderCommand>(), incomingException));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception, Is.EqualTo(incomingException));
        }

        /// <summary>
        /// Tests that HandleException rethrows the exception when the exception if type of IntranetSystemException.
        /// </summary>
        [Test]
        public void TestThatHandleExceptionRethrowsExceptionWhenExceptionIsTypeOfIntranetSystemException()
        {
            var fixture = new Fixture();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutor = MockRepository.GenerateMock<ILogicExecutor>();

            var importFoodGroupFromDataProviderCommandHandler = new ImportFoodGroupFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(importFoodGroupFromDataProviderCommandHandler, Is.Not.Null);

            var incomingException = fixture.Create<IntranetSystemException>();

            var exception = Assert.Throws<IntranetSystemException>(() => importFoodGroupFromDataProviderCommandHandler.HandleException(fixture.Create<FoodGroupImportFromDataProviderCommand>(), incomingException));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception, Is.EqualTo(incomingException));
        }

        /// <summary>
        /// Tests that HandleException throws an IntranetSystemException when the exception if type of Exception.
        /// </summary>
        [Test]
        public void TestThatHandleExceptionThrowsIntranetSystemExceptionWhenExceptionIsTypeOfException()
        {
            var fixture = new Fixture();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutor = MockRepository.GenerateMock<ILogicExecutor>();

            var importFoodGroupFromDataProviderCommandHandler = new ImportFoodGroupFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(importFoodGroupFromDataProviderCommandHandler, Is.Not.Null);

            var incomingException = fixture.Create<Exception>();

            var exception = Assert.Throws<IntranetSystemException>(() => importFoodGroupFromDataProviderCommandHandler.HandleException(fixture.Create<FoodGroupImportFromDataProviderCommand>(), incomingException));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.ErrorInCommandHandlerWithReturnValue, typeof (FoodGroupImportFromDataProviderCommand).Name, typeof (ServiceReceiptResponse).Name, incomingException.Message)));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.EqualTo(incomingException));
        }
    }
}
