using System;
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
            var logicExecutor = MockRepository.GenerateMock<ILogicExecutor>();

            var foodItemImportFromDataProviderCommandHandler = new FoodItemImportFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
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

            var exception = Assert.Throws<ArgumentNullException>(() => new FoodItemImportFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, null));
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
            var logicExecutor = MockRepository.GenerateMock<ILogicExecutor>();

            var foodItemImportFromDataProviderCommandHandler = new FoodItemImportFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
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
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutor = MockRepository.GenerateMock<ILogicExecutor>();

            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();

            var foodItemImportFromDataProviderCommandHandler = new FoodItemImportFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
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
        /// Tests that HandleException throws an ArgumentNullException if the command for importing a food item from a given data provider is null.
        /// </summary>
        [Test]
        public void TestThatHandleExceptionThrowsArgumentNullExceptionIfFoodItemImportFromDataProviderCommandIsNull()
        {
            var fixture = new Fixture();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var logicExecutor = MockRepository.GenerateMock<ILogicExecutor>();

            var foodItemImportFromDataProviderCommandHandler = new FoodItemImportFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(foodItemImportFromDataProviderCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodItemImportFromDataProviderCommandHandler.HandleException(null, fixture.Create<Exception>()));
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

            var foodItemImportFromDataProviderCommandHandler = new FoodItemImportFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(foodItemImportFromDataProviderCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodItemImportFromDataProviderCommandHandler.HandleException(fixture.Create<FoodItemImportFromDataProviderCommand>(), null));
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

            var foodItemImportFromDataProviderCommandHandler = new FoodItemImportFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(foodItemImportFromDataProviderCommandHandler, Is.Not.Null);

            var incomingException = fixture.Create<IntranetRepositoryException>();

            var exception = Assert.Throws<IntranetRepositoryException>(() => foodItemImportFromDataProviderCommandHandler.HandleException(fixture.Create<FoodItemImportFromDataProviderCommand>(), incomingException));
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

            var foodItemImportFromDataProviderCommandHandler = new FoodItemImportFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(foodItemImportFromDataProviderCommandHandler, Is.Not.Null);

            var incomingException = fixture.Create<IntranetBusinessException>();

            var exception = Assert.Throws<IntranetBusinessException>(() => foodItemImportFromDataProviderCommandHandler.HandleException(fixture.Create<FoodItemImportFromDataProviderCommand>(), incomingException));
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

            var foodItemImportFromDataProviderCommandHandler = new FoodItemImportFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(foodItemImportFromDataProviderCommandHandler, Is.Not.Null);

            var incomingException = fixture.Create<IntranetSystemException>();

            var exception = Assert.Throws<IntranetSystemException>(() => foodItemImportFromDataProviderCommandHandler.HandleException(fixture.Create<FoodItemImportFromDataProviderCommand>(), incomingException));
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

            var foodItemImportFromDataProviderCommandHandler = new FoodItemImportFromDataProviderCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, logicExecutor);
            Assert.That(foodItemImportFromDataProviderCommandHandler, Is.Not.Null);

            var incomingException = fixture.Create<Exception>();

            var exception = Assert.Throws<IntranetSystemException>(() => foodItemImportFromDataProviderCommandHandler.HandleException(fixture.Create<FoodItemImportFromDataProviderCommand>(), incomingException));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.ErrorInCommandHandlerWithReturnValue, typeof (FoodItemImportFromDataProviderCommand).Name, typeof (ServiceReceiptResponse).Name, incomingException.Message)));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.EqualTo(incomingException));
        }
    }
}
