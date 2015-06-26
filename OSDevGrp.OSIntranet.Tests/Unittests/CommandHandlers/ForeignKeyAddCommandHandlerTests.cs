using System;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommandHandlers;
using OSDevGrp.OSIntranet.CommandHandlers.Validation;
using OSDevGrp.OSIntranet.Contracts.Commands;
using OSDevGrp.OSIntranet.Contracts.Responses;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Validation;
using OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Resources;
using OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.CommandHandlers
{
    /// <summary>
    /// Tests the command handler which handles a command for adding a foreign key.
    /// </summary>
    [TestFixture]
    public class ForeignKeyAddCommandHandlerTests
    {
        /// <summary>
        /// Tests that the constructor initialize a command handler which handles a command for adding a foreign key.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeTranslationAddCommandHandler()
        {
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var foreignKeyAddCommandHandler = new ForeignKeyAddCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock);
            Assert.That(foreignKeyAddCommandHandler, Is.Not.Null);
        }

        /// <summary>
        /// Tests that Execute throws an ArgumentNullException if the command for adding a foreign key is null.
        /// </summary>
        [Test]
        public void TestThatExecuteThrowsArgumentNullExceptionIfForeignKeyAddCommandIsNull()
        {
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var foreignKeyAddCommandHandler = new ForeignKeyAddCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock);
            Assert.That(foreignKeyAddCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foreignKeyAddCommandHandler.Execute(null));
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

            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<IFoodGroup>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock())
                .Repeat.Any();

            var foreignKeyAddCommand = fixture.Build<ForeignKeyAddCommand>()
                .With(m => m.DataProviderIdentifier, Guid.NewGuid())
                .With(m => m.ForeignKeyForIdentifier, Guid.NewGuid())
                .With(m => m.ForeignKeyValue, fixture.Create<string>())
                .Create();

            var foreignKeyAddCommandHandler = new ForeignKeyAddCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock);
            Assert.That(foreignKeyAddCommandHandler, Is.Not.Null);

            foreignKeyAddCommandHandler.Execute(foreignKeyAddCommand);

            systemDataRepositoryMock.AssertWasCalled(m => m.Get<IDataProvider>(Arg<Guid>.Is.Equal(foreignKeyAddCommand.DataProviderIdentifier)));
        }

        /// <summary>
        /// Tests that Execute calls Get to get the foreign keyable domain object on the repository which can access system data for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsGetForForeignKeyForIdentifierOnSystemDataRepository()
        {
            var fixture = new Fixture();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<IFoodGroup>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock())
                .Repeat.Any();

            var foreignKeyAddCommand = fixture.Build<ForeignKeyAddCommand>()
                .With(m => m.DataProviderIdentifier, Guid.NewGuid())
                .With(m => m.ForeignKeyForIdentifier, Guid.NewGuid())
                .With(m => m.ForeignKeyValue, fixture.Create<string>())
                .Create();

            var foreignKeyAddCommandHandler = new ForeignKeyAddCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock);
            Assert.That(foreignKeyAddCommandHandler, Is.Not.Null);

            foreignKeyAddCommandHandler.Execute(foreignKeyAddCommand);

            systemDataRepositoryMock.AssertWasCalled(m => m.Get<IFoodGroup>(Arg<Guid>.Is.Equal(foreignKeyAddCommand.ForeignKeyForIdentifier)));
        }

        /// <summary>
        /// Test that Execute calls IsNotNull for the data provider on the common validations.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsIsNotNullForDataProviderOnCommonValidations()
        {
            
        }

        /// <summary>
        /// Tests that HandleException throws an ArgumentNullException if the command for adding a foreign key is null.
        /// </summary>
        [Test]
        public void TestThatHandleExceptionThrowsArgumentNullExceptionIfForeignKeyCommandIsNull()
        {
            var fixture = new Fixture();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var foreignKeyAddCommandHandler = new ForeignKeyAddCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock);
            Assert.That(foreignKeyAddCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foreignKeyAddCommandHandler.HandleException(null, fixture.Create<Exception>()));
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

            var foreignKeyAddCommandHandler = new ForeignKeyAddCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock);
            Assert.That(foreignKeyAddCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foreignKeyAddCommandHandler.HandleException(fixture.Create<ForeignKeyAddCommand>(), null));
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

            var foreignKeyAddCommandHandler = new ForeignKeyAddCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock);
            Assert.That(foreignKeyAddCommandHandler, Is.Not.Null);

            var incomingException = fixture.Create<IntranetRepositoryException>();

            var exception = Assert.Throws<IntranetRepositoryException>(() => foreignKeyAddCommandHandler.HandleException(fixture.Create<ForeignKeyAddCommand>(), incomingException));
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

            var foreignKeyAddCommandHandler = new ForeignKeyAddCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock);
            Assert.That(foreignKeyAddCommandHandler, Is.Not.Null);

            var incomingException = fixture.Create<IntranetBusinessException>();

            var exception = Assert.Throws<IntranetBusinessException>(() => foreignKeyAddCommandHandler.HandleException(fixture.Create<ForeignKeyAddCommand>(), incomingException));
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

            var foreignKeyAddCommandHandler = new ForeignKeyAddCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock);
            Assert.That(foreignKeyAddCommandHandler, Is.Not.Null);

            var incomingException = fixture.Create<IntranetSystemException>();

            var exception = Assert.Throws<IntranetSystemException>(() => foreignKeyAddCommandHandler.HandleException(fixture.Create<ForeignKeyAddCommand>(), incomingException));
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

            var foreignKeyAddCommandHandler = new ForeignKeyAddCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock);
            Assert.That(foreignKeyAddCommandHandler, Is.Not.Null);

            var incomingException = fixture.Create<Exception>();

            var exception = Assert.Throws<IntranetSystemException>(() => foreignKeyAddCommandHandler.HandleException(fixture.Create<ForeignKeyAddCommand>(), incomingException));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.ErrorInCommandHandlerWithReturnValue, typeof (ForeignKeyAddCommand).Name, typeof (ServiceReceiptResponse).Name, incomingException.Message)));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.EqualTo(incomingException));
        }
    }
}
