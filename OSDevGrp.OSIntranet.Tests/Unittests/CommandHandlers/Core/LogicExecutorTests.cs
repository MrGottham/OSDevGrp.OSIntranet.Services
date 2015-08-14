using System;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommandHandlers.Core;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Contracts.Commands;
using OSDevGrp.OSIntranet.Contracts.Responses;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Resources;
using OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.CommandHandlers.Core
{
    /// <summary>
    /// Tests the logic executor which can execute basic logic.
    /// </summary>
    [TestFixture]
    public class LogicExecutorTests
    {
        /// <summary>
        /// Tests that the constructor initialize the logic executor which can execute basic logic.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeLogicExecutor()
        {
            var commandBusMock = MockRepository.GenerateMock<ICommandBus>();

            var logicExecutor = new LogicExecutor(commandBusMock);
            Assert.That(logicExecutor, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the command bus is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenCommandBusIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new LogicExecutor(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("commandBus"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that ForeignKeyAdd throws an ArgumentNullException when the foreign key to add is null.
        /// </summary>
        [Test]
        public void TestThatForeignKeyAddThrowsArgumentNullExceptionWhenForeignKeyIsNull()
        {
            var commandBusMock = MockRepository.GenerateMock<ICommandBus>();

            var logicExecutor = new LogicExecutor(commandBusMock);
            Assert.That(logicExecutor, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => logicExecutor.ForeignKeyAdd(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("foreignKey"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that ForeignKeyAdd calls Publish on the command bus.
        /// </summary>
        [Test]
        public void TestThatForeignKeyAddCallsPublishOnCommandBus()
        {
            var fixture = new Fixture();

            var foodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock();
            // ReSharper disable PossibleInvalidOperationException
            var foreignKeyMock = DomainObjectMockBuilder.BuildForeignKeyMock(foodGroupMock.Identifier.Value, foodGroupMock.GetType());
            // ReSharper restore PossibleInvalidOperationException

            var commandBusMock = MockRepository.GenerateMock<ICommandBus>();
            commandBusMock.Stub(m => m.Publish<ForeignKeyAddCommand, ServiceReceiptResponse>(Arg<ForeignKeyAddCommand>.Is.NotNull))
                .WhenCalled(e =>
                {
                    var command = (ForeignKeyAddCommand) e.Arguments.ElementAt(0);
                    // ReSharper disable PossibleInvalidOperationException
                    Assert.That(command.DataProviderIdentifier, Is.EqualTo(foreignKeyMock.DataProvider.Identifier.Value));
                    // ReSharper restore PossibleInvalidOperationException
                    Assert.That(command.ForeignKeyForIdentifier, Is.EqualTo(foreignKeyMock.ForeignKeyForIdentifier));
                    Assert.That(command.ForeignKeyValue, Is.Not.Null);
                    Assert.That(command.ForeignKeyValue, Is.Not.Empty);
                    Assert.That(command.ForeignKeyValue, Is.EqualTo(foreignKeyMock.ForeignKeyValue));
                })
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var logicExecutor = new LogicExecutor(commandBusMock);
            Assert.That(logicExecutor, Is.Not.Null);

            logicExecutor.ForeignKeyAdd(foreignKeyMock);

            commandBusMock.AssertWasCalled(m => m.Publish<ForeignKeyAddCommand, ServiceReceiptResponse>(Arg<ForeignKeyAddCommand>.Is.NotNull));
        }

        /// <summary>
        /// Tests that ForeignKeyAdd returns identifier from the returned service receipt.
        /// </summary>
        [Test]
        public void TestThatForeignKeyAddReturnsIdentifierFromServiceReceiptResponse()
        {
            var fixture = new Fixture();

            var foodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock();
            // ReSharper disable PossibleInvalidOperationException
            var foreignKeyMock = DomainObjectMockBuilder.BuildForeignKeyMock(foodGroupMock.Identifier.Value, foodGroupMock.GetType());
            // ReSharper restore PossibleInvalidOperationException

            var serviceReceipt = fixture.Create<ServiceReceiptResponse>();
            var commandBusMock = MockRepository.GenerateMock<ICommandBus>();
            commandBusMock.Stub(m => m.Publish<ForeignKeyAddCommand, ServiceReceiptResponse>(Arg<ForeignKeyAddCommand>.Is.Anything))
                .Return(serviceReceipt)
                .Repeat.Any();

            var logicExecutor = new LogicExecutor(commandBusMock);
            Assert.That(logicExecutor, Is.Not.Null);

            var result = logicExecutor.ForeignKeyAdd(foreignKeyMock);
            Assert.That(result, Is.EqualTo(serviceReceipt.Identifier));
        }

        /// <summary>
        /// Tests that ForeignKeyModify throws an ArgumentNullException when the foreign key to modify is null.
        /// </summary>
        [Test]
        public void TestThatForeignKeyModifyThrowsArgumentNullExceptionWhenForeignKeyIsNull()
        {
            var commandBusMock = MockRepository.GenerateMock<ICommandBus>();

            var logicExecutor = new LogicExecutor(commandBusMock);
            Assert.That(logicExecutor, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => logicExecutor.ForeignKeyModify(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("foreignKey"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that ForeignKeyModify throws an IntranetSystemException when the identifier on foreign key to modify is null.
        /// </summary>
        [Test]
        public void TestThatForeignKeyModifyThrowsIntranetSystemExceptionWhenIdentifierOnForeignKeyIsNull()
        {
            var commandBusMock = MockRepository.GenerateMock<ICommandBus>();

            var foreignKeyMock = MockRepository.GenerateMock<IForeignKey>();
            foreignKeyMock.Stub(m => m.Identifier)
                .Return(null)
                .Repeat.Any();

            var logicExecutor = new LogicExecutor(commandBusMock);
            Assert.That(logicExecutor, Is.Not.Null);

            var exception = Assert.Throws<IntranetSystemException>(() => logicExecutor.ForeignKeyModify(foreignKeyMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, foreignKeyMock.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that ForeignKeyModify calls Publish on the command bus.
        /// </summary>
        [Test]
        public void TestThatForeignKeyModifyCallsPublishOnCommandBus()
        {
            var fixture = new Fixture();

            var foodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock();
            // ReSharper disable PossibleInvalidOperationException
            var foreignKeyMock = DomainObjectMockBuilder.BuildForeignKeyMock(foodGroupMock.Identifier.Value, foodGroupMock.GetType());
            // ReSharper restore PossibleInvalidOperationException

            var commandBusMock = MockRepository.GenerateMock<ICommandBus>();
            commandBusMock.Stub(m => m.Publish<ForeignKeyModifyCommand, ServiceReceiptResponse>(Arg<ForeignKeyModifyCommand>.Is.NotNull))
                .WhenCalled(e =>
                {
                    var command = (ForeignKeyModifyCommand) e.Arguments.ElementAt(0);
                    // ReSharper disable PossibleInvalidOperationException
                    Assert.That(command.ForeignKeyIdentifier, Is.EqualTo(foreignKeyMock.Identifier.Value));
                    // ReSharper restore PossibleInvalidOperationException
                    Assert.That(command.ForeignKeyValue, Is.Not.Null);
                    Assert.That(command.ForeignKeyValue, Is.Not.Empty);
                    Assert.That(command.ForeignKeyValue, Is.EqualTo(foreignKeyMock.ForeignKeyValue));
                })
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var logicExecutor = new LogicExecutor(commandBusMock);
            Assert.That(logicExecutor, Is.Not.Null);

            logicExecutor.ForeignKeyModify(foreignKeyMock);

            commandBusMock.AssertWasCalled(m => m.Publish<ForeignKeyModifyCommand, ServiceReceiptResponse>(Arg<ForeignKeyModifyCommand>.Is.NotNull));
        }

        /// <summary>
        /// Tests that ForeignKeyModify returns identifier from the returned service receipt.
        /// </summary>
        [Test]
        public void TestThatForeignKeyModifyReturnsIdentifierFromServiceReceiptResponse()
        {
            var fixture = new Fixture();

            var foodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock();
            // ReSharper disable PossibleInvalidOperationException
            var foreignKeyMock = DomainObjectMockBuilder.BuildForeignKeyMock(foodGroupMock.Identifier.Value, foodGroupMock.GetType());
            // ReSharper restore PossibleInvalidOperationException

            var serviceReceipt = fixture.Create<ServiceReceiptResponse>();
            var commandBusMock = MockRepository.GenerateMock<ICommandBus>();
            commandBusMock.Stub(m => m.Publish<ForeignKeyModifyCommand, ServiceReceiptResponse>(Arg<ForeignKeyModifyCommand>.Is.Anything))
                .Return(serviceReceipt)
                .Repeat.Any();

            var logicExecutor = new LogicExecutor(commandBusMock);
            Assert.That(logicExecutor, Is.Not.Null);

            var result = logicExecutor.ForeignKeyModify(foreignKeyMock);
            Assert.That(result, Is.EqualTo(serviceReceipt.Identifier));
        }

        /// <summary>
        /// Tests that ForeignKeyDelete throws an ArgumentNullException when the foreign key to delete is null.
        /// </summary>
        [Test]
        public void TestThatForeignKeyDeleteThrowsArgumentNullExceptionWhenForeignKeyIsNull()
        {
            var commandBusMock = MockRepository.GenerateMock<ICommandBus>();

            var logicExecutor = new LogicExecutor(commandBusMock);
            Assert.That(logicExecutor, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => logicExecutor.ForeignKeyDelete(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("foreignKey"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that ForeignKeyDelete throws an IntranetSystemException when the identifier on foreign key to delete is null.
        /// </summary>
        [Test]
        public void TestThatForeignKeyDeleteThrowsIntranetSystemExceptionWhenIdentifierOnForeignKeyIsNull()
        {
            var commandBusMock = MockRepository.GenerateMock<ICommandBus>();

            var foreignKeyMock = MockRepository.GenerateMock<IForeignKey>();
            foreignKeyMock.Stub(m => m.Identifier)
                .Return(null)
                .Repeat.Any();

            var logicExecutor = new LogicExecutor(commandBusMock);
            Assert.That(logicExecutor, Is.Not.Null);

            var exception = Assert.Throws<IntranetSystemException>(() => logicExecutor.ForeignKeyDelete(foreignKeyMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, foreignKeyMock.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that ForeignKeyDelete calls Publish on the command bus.
        /// </summary>
        [Test]
        public void TestThatForeignKeyDeleteCallsPublishOnCommandBus()
        {
            var fixture = new Fixture();

            var foodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock();
            // ReSharper disable PossibleInvalidOperationException
            var foreignKeyMock = DomainObjectMockBuilder.BuildForeignKeyMock(foodGroupMock.Identifier.Value, foodGroupMock.GetType());
            // ReSharper restore PossibleInvalidOperationException

            var commandBusMock = MockRepository.GenerateMock<ICommandBus>();
            commandBusMock.Stub(m => m.Publish<ForeignKeyDeleteCommand, ServiceReceiptResponse>(Arg<ForeignKeyDeleteCommand>.Is.NotNull))
                .WhenCalled(e =>
                {
                    var command = (ForeignKeyDeleteCommand) e.Arguments.ElementAt(0);
                    // ReSharper disable PossibleInvalidOperationException
                    Assert.That(command.ForeignKeyIdentifier, Is.EqualTo(foreignKeyMock.Identifier.Value));
                    // ReSharper restore PossibleInvalidOperationException
                })
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var logicExecutor = new LogicExecutor(commandBusMock);
            Assert.That(logicExecutor, Is.Not.Null);

            logicExecutor.ForeignKeyDelete(foreignKeyMock);

            commandBusMock.AssertWasCalled(m => m.Publish<ForeignKeyDeleteCommand, ServiceReceiptResponse>(Arg<ForeignKeyDeleteCommand>.Is.NotNull));
        }

        /// <summary>
        /// Tests that ForeignKeyDelete returns identifier from the returned service receipt.
        /// </summary>
        [Test]
        public void TestThatForeignKeyDeleteReturnsIdentifierFromServiceReceiptResponse()
        {
            var fixture = new Fixture();

            var foodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock();
            // ReSharper disable PossibleInvalidOperationException
            var foreignKeyMock = DomainObjectMockBuilder.BuildForeignKeyMock(foodGroupMock.Identifier.Value, foodGroupMock.GetType());
            // ReSharper restore PossibleInvalidOperationException

            var serviceReceipt = fixture.Create<ServiceReceiptResponse>();
            var commandBusMock = MockRepository.GenerateMock<ICommandBus>();
            commandBusMock.Stub(m => m.Publish<ForeignKeyDeleteCommand, ServiceReceiptResponse>(Arg<ForeignKeyDeleteCommand>.Is.Anything))
                .Return(serviceReceipt)
                .Repeat.Any();

            var logicExecutor = new LogicExecutor(commandBusMock);
            Assert.That(logicExecutor, Is.Not.Null);

            var result = logicExecutor.ForeignKeyDelete(foreignKeyMock);
            Assert.That(result, Is.EqualTo(serviceReceipt.Identifier));
        }

        /// <summary>
        /// Tests that TranslationAdd throws an ArgumentNullException when the translation to add is null.
        /// </summary>
        [Test]
        public void TestThatTranslationAddThrowsArgumentNullExceptionWhenTranslationIsNull()
        {
            var commandBusMock = MockRepository.GenerateMock<ICommandBus>();

            var logicExecutor = new LogicExecutor(commandBusMock);
            Assert.That(logicExecutor, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => logicExecutor.TranslationAdd(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("translation"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that TranslationAdd calls Publish on the command bus.
        /// </summary>
        [Test]
        public void TestThatTranslationAddCallsPublishOnCommandBus()
        {
            var fixture = new Fixture();

            var translationMock = DomainObjectMockBuilder.BuildTranslationMock(Guid.NewGuid());

            var commandBusMock = MockRepository.GenerateMock<ICommandBus>();
            commandBusMock.Stub(m => m.Publish<TranslationAddCommand, ServiceReceiptResponse>(Arg<TranslationAddCommand>.Is.NotNull))
                .WhenCalled(e =>
                {
                    var command = (TranslationAddCommand) e.Arguments.ElementAt(0);
                    // ReSharper disable PossibleInvalidOperationException
                    Assert.That(command.TranslationInfoIdentifier, Is.EqualTo(translationMock.TranslationInfo.Identifier.Value));
                    // ReSharper restore PossibleInvalidOperationException
                    Assert.That(command.TranslationOfIdentifier, Is.EqualTo(translationMock.TranslationOfIdentifier));
                    Assert.That(command.TranslationValue, Is.Not.Null);
                    Assert.That(command.TranslationValue, Is.Not.Empty);
                    Assert.That(command.TranslationValue, Is.EqualTo(translationMock.Value));
                })
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var logicExecutor = new LogicExecutor(commandBusMock);
            Assert.That(logicExecutor, Is.Not.Null);

            logicExecutor.TranslationAdd(translationMock);

            commandBusMock.AssertWasCalled(m => m.Publish<TranslationAddCommand, ServiceReceiptResponse>(Arg<TranslationAddCommand>.Is.NotNull));
        }

        /// <summary>
        /// Tests that TranslationAdd returns identifier from the returned service receipt.
        /// </summary>
        [Test]
        public void TestThatTranslationAddReturnsIdentifierFromServiceReceiptResponse()
        {
            var fixture = new Fixture();

            var translationMock = DomainObjectMockBuilder.BuildTranslationMock(Guid.NewGuid());

            var serviceReceipt = fixture.Create<ServiceReceiptResponse>();
            var commandBusMock = MockRepository.GenerateMock<ICommandBus>();
            commandBusMock.Stub(m => m.Publish<TranslationAddCommand, ServiceReceiptResponse>(Arg<TranslationAddCommand>.Is.Anything))
                .Return(serviceReceipt)
                .Repeat.Any();

            var logicExecutor = new LogicExecutor(commandBusMock);
            Assert.That(logicExecutor, Is.Not.Null);

            var result = logicExecutor.TranslationAdd(translationMock);
            Assert.That(result, Is.EqualTo(serviceReceipt.Identifier));
        }

        /// <summary>
        /// Tests that TranslationModify throws an ArgumentNullException when the translation to modify is null.
        /// </summary>
        [Test]
        public void TestThatTranslationModifyThrowsArgumentNullExceptionWhenTranslationIsNull()
        {
            var commandBusMock = MockRepository.GenerateMock<ICommandBus>();

            var logicExecutor = new LogicExecutor(commandBusMock);
            Assert.That(logicExecutor, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => logicExecutor.TranslationModify(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("translation"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that TranslationModify throws an IntranetSystemException when the identifier on translation to modify is null.
        /// </summary>
        [Test]
        public void TestThatTranslationModifyThrowsIntranetSystemExceptionWhenIdentifierOnTranslationIsNull()
        {
            var commandBusMock = MockRepository.GenerateMock<ICommandBus>();

            var translationMock = MockRepository.GenerateMock<ITranslation>();
            translationMock.Stub(m => m.Identifier)
                .Return(null)
                .Repeat.Any();

            var logicExecutor = new LogicExecutor(commandBusMock);
            Assert.That(logicExecutor, Is.Not.Null);

            var exception = Assert.Throws<IntranetSystemException>(() => logicExecutor.TranslationModify(translationMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, translationMock.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that TranslationModify calls Publish on the command bus.
        /// </summary>
        [Test]
        public void TestThatTranslationModifyCallsPublishOnCommandBus()
        {
            var fixture = new Fixture();

            var translationMock = DomainObjectMockBuilder.BuildTranslationMock(Guid.NewGuid());

            var commandBusMock = MockRepository.GenerateMock<ICommandBus>();
            commandBusMock.Stub(m => m.Publish<TranslationModifyCommand, ServiceReceiptResponse>(Arg<TranslationModifyCommand>.Is.NotNull))
                .WhenCalled(e =>
                {
                    var command = (TranslationModifyCommand) e.Arguments.ElementAt(0);
                    // ReSharper disable PossibleInvalidOperationException
                    Assert.That(command.TranslationIdentifier, Is.EqualTo(translationMock.Identifier.Value));
                    // ReSharper restore PossibleInvalidOperationException
                    Assert.That(command.TranslationValue, Is.Not.Null);
                    Assert.That(command.TranslationValue, Is.Not.Empty);
                    Assert.That(command.TranslationValue, Is.EqualTo(translationMock.Value));
                })
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var logicExecutor = new LogicExecutor(commandBusMock);
            Assert.That(logicExecutor, Is.Not.Null);

            logicExecutor.TranslationModify(translationMock);

            commandBusMock.AssertWasCalled(m => m.Publish<TranslationModifyCommand, ServiceReceiptResponse>(Arg<TranslationModifyCommand>.Is.NotNull));
        }

        /// <summary>
        /// Tests that TranslationModify returns identifier from the returned service receipt.
        /// </summary>
        [Test]
        public void TestThatTranslationModifyReturnsIdentifierFromServiceReceiptResponse()
        {
            var fixture = new Fixture();

            var translationMock = DomainObjectMockBuilder.BuildTranslationMock(Guid.NewGuid());

            var serviceReceipt = fixture.Create<ServiceReceiptResponse>();
            var commandBusMock = MockRepository.GenerateMock<ICommandBus>();
            commandBusMock.Stub(m => m.Publish<TranslationModifyCommand, ServiceReceiptResponse>(Arg<TranslationModifyCommand>.Is.Anything))
                .Return(serviceReceipt)
                .Repeat.Any();

            var logicExecutor = new LogicExecutor(commandBusMock);
            Assert.That(logicExecutor, Is.Not.Null);

            var result = logicExecutor.TranslationModify(translationMock);
            Assert.That(result, Is.EqualTo(serviceReceipt.Identifier));
        }

        /// <summary>
        /// Tests that TranslationDelete throws an ArgumentNullException when the translation to delete is null.
        /// </summary>
        [Test]
        public void TestThatTranslationDeleteThrowsArgumentNullExceptionWhenTranslationIsNull()
        {
            var commandBusMock = MockRepository.GenerateMock<ICommandBus>();

            var logicExecutor = new LogicExecutor(commandBusMock);
            Assert.That(logicExecutor, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => logicExecutor.TranslationDelete(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("translation"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that TranslationDelete throws an IntranetSystemException when the identifier on translation to modify is null.
        /// </summary>
        [Test]
        public void TestThatTranslationDeleteThrowsIntranetSystemExceptionWhenIdentifierOnTranslationIsNull()
        {
            var commandBusMock = MockRepository.GenerateMock<ICommandBus>();

            var translationMock = MockRepository.GenerateMock<ITranslation>();
            translationMock.Stub(m => m.Identifier)
                .Return(null)
                .Repeat.Any();

            var logicExecutor = new LogicExecutor(commandBusMock);
            Assert.That(logicExecutor, Is.Not.Null);

            var exception = Assert.Throws<IntranetSystemException>(() => logicExecutor.TranslationDelete(translationMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, translationMock.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that TranslationDelete calls Publish on the command bus.
        /// </summary>
        [Test]
        public void TestThatTranslationDeleteCallsPublishOnCommandBus()
        {
            var fixture = new Fixture();

            var translationMock = DomainObjectMockBuilder.BuildTranslationMock(Guid.NewGuid());

            var commandBusMock = MockRepository.GenerateMock<ICommandBus>();
            commandBusMock.Stub(m => m.Publish<TranslationDeleteCommand, ServiceReceiptResponse>(Arg<TranslationDeleteCommand>.Is.NotNull))
                .WhenCalled(e =>
                {
                    var command = (TranslationDeleteCommand) e.Arguments.ElementAt(0);
                    // ReSharper disable PossibleInvalidOperationException
                    Assert.That(command.TranslationIdentifier, Is.EqualTo(translationMock.Identifier.Value));
                    // ReSharper restore PossibleInvalidOperationException
                })
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var logicExecutor = new LogicExecutor(commandBusMock);
            Assert.That(logicExecutor, Is.Not.Null);

            logicExecutor.TranslationDelete(translationMock);

            commandBusMock.AssertWasCalled(m => m.Publish<TranslationDeleteCommand, ServiceReceiptResponse>(Arg<TranslationDeleteCommand>.Is.NotNull));
        }

        /// <summary>
        /// Tests that TranslationDelete returns identifier from the returned service receipt.
        /// </summary>
        [Test]
        public void TestThatTranslationDeleteReturnsIdentifierFromServiceReceiptResponse()
        {
            var fixture = new Fixture();

            var translationMock = DomainObjectMockBuilder.BuildTranslationMock(Guid.NewGuid());

            var serviceReceipt = fixture.Create<ServiceReceiptResponse>();
            var commandBusMock = MockRepository.GenerateMock<ICommandBus>();
            commandBusMock.Stub(m => m.Publish<TranslationDeleteCommand, ServiceReceiptResponse>(Arg<TranslationDeleteCommand>.Is.Anything))
                .Return(serviceReceipt)
                .Repeat.Any();

            var logicExecutor = new LogicExecutor(commandBusMock);
            Assert.That(logicExecutor, Is.Not.Null);

            var result = logicExecutor.TranslationDelete(translationMock);
            Assert.That(result, Is.EqualTo(serviceReceipt.Identifier));
        }
    }
}
