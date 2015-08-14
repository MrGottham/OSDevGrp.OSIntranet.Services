using System;
using System.Linq;
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
        public void TestThatExecuteThrowsArgumentNullExceptionIfImportFoodGroupFromDataProviderCommandIsNull()
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
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutor = MockRepository.GenerateMock<ILogicExecutor>();

            var key = fixture.Create<string>();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var importFoodGroupFromDataProviderCommandHandler = new ImportFoodGroupFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(importFoodGroupFromDataProviderCommandHandler, Is.Not.Null);

            var importFoodGroupFromDataProviderCommand = new ImportFoodGroupFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = key,
                Name = fixture.Create<string>(),
                IsActive = fixture.Create<bool>(),
                ParentKey = null
            };

            importFoodGroupFromDataProviderCommandHandler.Execute(importFoodGroupFromDataProviderCommand);

            systemDataRepositoryMock.AssertWasCalled(m => m.Get<IDataProvider>(Arg<Guid>.Is.Equal(importFoodGroupFromDataProviderCommand.DataProviderIdentifier)));
        }

        /// <summary>
        /// Tests that Execute calls Get to get the translation informations on the repository which can access system data for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsGetForTranslationInfoIdentifierOnSystemDataRepository()
        {
            var fixture = new Fixture();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutor = MockRepository.GenerateMock<ILogicExecutor>();

            var key = fixture.Create<string>();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var importFoodGroupFromDataProviderCommandHandler = new ImportFoodGroupFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(importFoodGroupFromDataProviderCommandHandler, Is.Not.Null);

            var importFoodGroupFromDataProviderCommand = new ImportFoodGroupFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = key,
                Name = fixture.Create<string>(),
                IsActive = fixture.Create<bool>(),
                ParentKey = null
            };

            importFoodGroupFromDataProviderCommandHandler.Execute(importFoodGroupFromDataProviderCommand);

            systemDataRepositoryMock.AssertWasCalled(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Equal(importFoodGroupFromDataProviderCommand.TranslationInfoIdentifier)));
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
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutor = MockRepository.GenerateMock<ILogicExecutor>();

            var key = fixture.Create<string>();
            var dataProviderMock = DomainObjectMockBuilder.BuildDataProviderMock();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(dataProviderMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var importFoodGroupFromDataProviderCommandHandler = new ImportFoodGroupFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(importFoodGroupFromDataProviderCommandHandler, Is.Not.Null);

            var importFoodGroupFromDataProviderCommand = new ImportFoodGroupFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = key,
                Name = fixture.Create<string>(),
                IsActive = fixture.Create<bool>(),
                ParentKey = noValue
            };

            importFoodGroupFromDataProviderCommandHandler.Execute(importFoodGroupFromDataProviderCommand);

            systemDataRepositoryMock.AssertWasNotCalled(m => m.FoodGroupGetByForeignKey(Arg<IDataProvider>.Is.Equal(dataProviderMock), Arg<string>.Is.Equal(importFoodGroupFromDataProviderCommand.ParentKey)));
        }

        /// <summary>
        /// Tests that Execute calls FoodGroupGetByForeignKey to get the parent food group on the repository which can access system data for the food waste domain when parent key has a value.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsFoodGroupGetByForeignKeyForParentKeyOnSystemDataRepositoryWhenParentKeyHasValue()
        {
            var fixture = new Fixture();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutor = MockRepository.GenerateMock<ILogicExecutor>();

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
            systemDataRepositoryMock.Stub(m => m.FoodGroupGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Equal(parentKey)))
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var importFoodGroupFromDataProviderCommandHandler = new ImportFoodGroupFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(importFoodGroupFromDataProviderCommandHandler, Is.Not.Null);

            var importFoodGroupFromDataProviderCommand = new ImportFoodGroupFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = key,
                Name = fixture.Create<string>(),
                IsActive = fixture.Create<bool>(),
                ParentKey = parentKey
            };

            importFoodGroupFromDataProviderCommandHandler.Execute(importFoodGroupFromDataProviderCommand);

            systemDataRepositoryMock.AssertWasCalled(m => m.FoodGroupGetByForeignKey(Arg<IDataProvider>.Is.Equal(dataProviderMock), Arg<string>.Is.Equal(importFoodGroupFromDataProviderCommand.ParentKey)));
        }

        /// <summary>
        /// Test that Execute calls IsNotNull for the data provider on the common validations.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsIsNotNullForDataProviderOnCommonValidations()
        {
            var fixture = new Fixture();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutor = MockRepository.GenerateMock<ILogicExecutor>();

            var key = fixture.Create<string>();
            var dataProviderMock = DomainObjectMockBuilder.BuildDataProviderMock();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(dataProviderMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
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

            var importFoodGroupFromDataProviderCommandHandler = new ImportFoodGroupFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(importFoodGroupFromDataProviderCommandHandler, Is.Not.Null);

            var importFoodGroupFromDataProviderCommand = new ImportFoodGroupFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = key,
                Name = fixture.Create<string>(),
                IsActive = fixture.Create<bool>(),
                ParentKey = null
            };

            importFoodGroupFromDataProviderCommandHandler.Execute(importFoodGroupFromDataProviderCommand);

            commonValidationsMock.AssertWasCalled(m => m.IsNotNull(Arg<IDataProvider>.Is.Equal(dataProviderMock)));
        }

        /// <summary>
        /// Test that Execute calls IsNotNull for the translation informations on the common validations.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsIsNotNullForTranslationInfoOnCommonValidations()
        {
            var fixture = new Fixture();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutor = MockRepository.GenerateMock<ILogicExecutor>();

            var key = fixture.Create<string>();
            var translationInfoMock = DomainObjectMockBuilder.BuildTranslationInfoMock();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(translationInfoMock)
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

            var importFoodGroupFromDataProviderCommandHandler = new ImportFoodGroupFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(importFoodGroupFromDataProviderCommandHandler, Is.Not.Null);

            var importFoodGroupFromDataProviderCommand = new ImportFoodGroupFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = key,
                Name = fixture.Create<string>(),
                IsActive = fixture.Create<bool>(),
                ParentKey = null
            };

            importFoodGroupFromDataProviderCommandHandler.Execute(importFoodGroupFromDataProviderCommand);

            commonValidationsMock.AssertWasCalled(m => m.IsNotNull(Arg<ITranslation>.Is.Equal(translationInfoMock)));
        }

        /// <summary>
        /// Test that Execute calls HasValue for the food group key on the common validations.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsHasValueForKeyOnCommonValidations()
        {
            var fixture = new Fixture();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutor = MockRepository.GenerateMock<ILogicExecutor>();

            var key = fixture.Create<string>();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
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

            var importFoodGroupFromDataProviderCommandHandler = new ImportFoodGroupFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(importFoodGroupFromDataProviderCommandHandler, Is.Not.Null);

            var importFoodGroupFromDataProviderCommand = new ImportFoodGroupFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = key,
                Name = fixture.Create<string>(),
                IsActive = fixture.Create<bool>(),
                ParentKey = null
            };

            importFoodGroupFromDataProviderCommandHandler.Execute(importFoodGroupFromDataProviderCommand);

            commonValidationsMock.AssertWasCalled(m => m.HasValue(Arg<string>.Is.Equal(importFoodGroupFromDataProviderCommand.Key)));
        }

        /// <summary>
        /// Test that Execute calls ContainsIllegalChar for the food group key on the common validations.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsContainsIllegalCharForKeyOnCommonValidations()
        {
            var fixture = new Fixture();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutor = MockRepository.GenerateMock<ILogicExecutor>();

            var key = fixture.Create<string>();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
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

            var importFoodGroupFromDataProviderCommandHandler = new ImportFoodGroupFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(importFoodGroupFromDataProviderCommandHandler, Is.Not.Null);

            var importFoodGroupFromDataProviderCommand = new ImportFoodGroupFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = key,
                Name = fixture.Create<string>(),
                IsActive = fixture.Create<bool>(),
                ParentKey = null
            };

            importFoodGroupFromDataProviderCommandHandler.Execute(importFoodGroupFromDataProviderCommand);

            commonValidationsMock.AssertWasCalled(m => m.ContainsIllegalChar(Arg<string>.Is.Equal(importFoodGroupFromDataProviderCommand.Key)));
        }

        /// <summary>
        /// Test that Execute calls HasValue for the food group name on the common validations.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsHasValueForNameOnCommonValidations()
        {
            var fixture = new Fixture();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutor = MockRepository.GenerateMock<ILogicExecutor>();

            var key = fixture.Create<string>();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
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

            var importFoodGroupFromDataProviderCommandHandler = new ImportFoodGroupFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(importFoodGroupFromDataProviderCommandHandler, Is.Not.Null);

            var importFoodGroupFromDataProviderCommand = new ImportFoodGroupFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = key,
                Name = fixture.Create<string>(),
                IsActive = fixture.Create<bool>(),
                ParentKey = null
            };

            importFoodGroupFromDataProviderCommandHandler.Execute(importFoodGroupFromDataProviderCommand);

            commonValidationsMock.AssertWasCalled(m => m.HasValue(Arg<string>.Is.Equal(importFoodGroupFromDataProviderCommand.Name)));
        }

        /// <summary>
        /// Test that Execute calls ContainsIllegalChar for the food group name on the common validations.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsContainsIllegalCharForNameOnCommonValidations()
        {
            var fixture = new Fixture();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutor = MockRepository.GenerateMock<ILogicExecutor>();

            var key = fixture.Create<string>();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
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

            var importFoodGroupFromDataProviderCommandHandler = new ImportFoodGroupFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(importFoodGroupFromDataProviderCommandHandler, Is.Not.Null);

            var importFoodGroupFromDataProviderCommand = new ImportFoodGroupFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = key,
                Name = fixture.Create<string>(),
                IsActive = fixture.Create<bool>(),
                ParentKey = null
            };

            importFoodGroupFromDataProviderCommandHandler.Execute(importFoodGroupFromDataProviderCommand);

            commonValidationsMock.AssertWasCalled(m => m.ContainsIllegalChar(Arg<string>.Is.Equal(importFoodGroupFromDataProviderCommand.Name)));
        }

        /// <summary>
        /// Test that Execute calls IsNotNull for the parent food group on the common validations.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsIsNotNullForParentFoodGroupOnCommonValidations()
        {
            var fixture = new Fixture();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutor = MockRepository.GenerateMock<ILogicExecutor>();

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
            systemDataRepositoryMock.Stub(m => m.FoodGroupGetByForeignKey(Arg<IDataProvider>.Is.Anything, Arg<string>.Is.Equal(parentKey)))
                .Return(parentFoodGroup)
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

            var importFoodGroupFromDataProviderCommandHandler = new ImportFoodGroupFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(importFoodGroupFromDataProviderCommandHandler, Is.Not.Null);

            var importFoodGroupFromDataProviderCommand = new ImportFoodGroupFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = key,
                Name = fixture.Create<string>(),
                IsActive = fixture.Create<bool>(),
                ParentKey = parentKey
            };

            importFoodGroupFromDataProviderCommandHandler.Execute(importFoodGroupFromDataProviderCommand);

            commonValidationsMock.AssertWasCalled(m => m.IsNotNull(Arg<IFoodGroup>.Is.Equal(parentFoodGroup)));
        }

        /// <summary>
        /// Test that Execute calls IsSatisfiedBy on the specification which encapsulates validation rules 7 times.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsIsSatisfiedByOnSpecification7Times()
        {
            var fixture = new Fixture();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutor = MockRepository.GenerateMock<ILogicExecutor>();

            var key = fixture.Create<string>();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var importFoodGroupFromDataProviderCommandHandler = new ImportFoodGroupFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(importFoodGroupFromDataProviderCommandHandler, Is.Not.Null);

            var importFoodGroupFromDataProviderCommand = new ImportFoodGroupFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = key,
                Name = fixture.Create<string>(),
                IsActive = fixture.Create<bool>(),
                ParentKey = null
            };

            importFoodGroupFromDataProviderCommandHandler.Execute(importFoodGroupFromDataProviderCommand);

            specificationMock.AssertWasCalled(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<IntranetBusinessException>.Is.TypeOf), opt => opt.Repeat.Times(7));
        }

        /// <summary>
        /// Test that Execute calls Evaluate on the specification which encapsulates validation rules.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsEvaluateOnSpecification()
        {
            var fixture = new Fixture();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutor = MockRepository.GenerateMock<ILogicExecutor>();

            var key = fixture.Create<string>();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var importFoodGroupFromDataProviderCommandHandler = new ImportFoodGroupFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(importFoodGroupFromDataProviderCommandHandler, Is.Not.Null);

            var importFoodGroupFromDataProviderCommand = new ImportFoodGroupFromDataProviderCommand
            {
                DataProviderIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = Guid.NewGuid(),
                Key = key,
                Name = fixture.Create<string>(),
                IsActive = fixture.Create<bool>(),
                ParentKey = null
            };

            importFoodGroupFromDataProviderCommandHandler.Execute(importFoodGroupFromDataProviderCommand);

            specificationMock.AssertWasCalled(m => m.Evaluate());
        }

        /// <summary>
        /// Tests that HandleException throws an ArgumentNullException if the command for importing a food group from a given data provider is null.
        /// </summary>
        [Test]
        public void TestThatHandleExceptionThrowsArgumentNullExceptionIfImportFoodGroupFromDataProviderCommandIsNull()
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

            var exception = Assert.Throws<ArgumentNullException>(() => importFoodGroupFromDataProviderCommandHandler.HandleException(fixture.Create<ImportFoodGroupFromDataProviderCommand>(), null));
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

            var exception = Assert.Throws<IntranetRepositoryException>(() => importFoodGroupFromDataProviderCommandHandler.HandleException(fixture.Create<ImportFoodGroupFromDataProviderCommand>(), incomingException));
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

            var exception = Assert.Throws<IntranetBusinessException>(() => importFoodGroupFromDataProviderCommandHandler.HandleException(fixture.Create<ImportFoodGroupFromDataProviderCommand>(), incomingException));
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

            var exception = Assert.Throws<IntranetSystemException>(() => importFoodGroupFromDataProviderCommandHandler.HandleException(fixture.Create<ImportFoodGroupFromDataProviderCommand>(), incomingException));
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

            var exception = Assert.Throws<IntranetSystemException>(() => importFoodGroupFromDataProviderCommandHandler.HandleException(fixture.Create<ImportFoodGroupFromDataProviderCommand>(), incomingException));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.ErrorInCommandHandlerWithReturnValue, typeof (ImportFoodGroupFromDataProviderCommand).Name, typeof (ServiceReceiptResponse).Name, incomingException.Message)));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.EqualTo(incomingException));
        }
    }
}
