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
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Resources;
using OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.CommandHandlers
{
    /// <summary>
    /// Tests the command handler which handles a command for modifying a foreign key.
    /// </summary>
    [TestFixture]
    public class ForeignKeyModifyCommandHandlerTests
    {
        /// <summary>
        /// Tests that the constructor initialize a command handler which handles a command for modifying a foreign key.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeForeignKeyModifyCommandHandler()
        {
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var foreignKeyModifyCommandHandler = new ForeignKeyModifyCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock);
            Assert.That(foreignKeyModifyCommandHandler, Is.Not.Null);
        }

        /// <summary>
        /// Tests that Execute throws an ArgumentNullException if the command for modifying a foreign key is null.
        /// </summary>
        [Test]
        public void TestThatExecuteThrowsArgumentNullExceptionIfForeignKeyModifyCommandIsNull()
        {
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var foreignKeyModifyCommandHandler = new ForeignKeyModifyCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock);
            Assert.That(foreignKeyModifyCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foreignKeyModifyCommandHandler.Execute(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("command"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Execute calls Get to get the foreign key on the repository which can access system data for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsGetForForeignKeyIdentifierOnSystemDataRepository()
        {
            var fixture = new Fixture();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var foodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IForeignKey>(Arg<Guid>.Is.Anything))
                // ReSharper disable PossibleInvalidOperationException
                .Return(DomainObjectMockBuilder.BuildForeignKeyMock(foodGroupMock.Identifier.Value, foodGroupMock.GetType()))
                // ReSharper restore PossibleInvalidOperationException
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var foreignKeyModifyCommandHandler = new ForeignKeyModifyCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock);
            Assert.That(foreignKeyModifyCommandHandler, Is.Not.Null);

            var foreignKeyModifyCommand = fixture.Build<ForeignKeyModifyCommand>()
                .With(m => m.ForeignKeyIdentifier, Guid.NewGuid())
                .With(m => m.ForeignKeyValue, fixture.Create<string>())
                .Create();

            foreignKeyModifyCommandHandler.Execute(foreignKeyModifyCommand);

            systemDataRepositoryMock.AssertWasCalled(m => m.Get<IForeignKey>(Arg<Guid>.Is.Equal(foreignKeyModifyCommand.ForeignKeyIdentifier)));
        }

        /// <summary>
        /// Test that Execute calls IsNotNull for the foreign key on the common validations.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsIsNotNullForForeignKeyOnCommonValidations()
        {
            var fixture = new Fixture();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var foodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock();
            // ReSharper disable PossibleInvalidOperationException
            var foreignKeyMock = DomainObjectMockBuilder.BuildForeignKeyMock(foodGroupMock.Identifier.Value, foodGroupMock.GetType());
            // ReSharper restore PossibleInvalidOperationException
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IForeignKey>(Arg<Guid>.Is.Anything))
                .Return(foreignKeyMock)
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

            var foreignKeyModifyCommandHandler = new ForeignKeyModifyCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock);
            Assert.That(foreignKeyModifyCommandHandler, Is.Not.Null);

            var foreignKeyModifyCommand = fixture.Build<ForeignKeyModifyCommand>()
                .With(m => m.ForeignKeyIdentifier, Guid.NewGuid())
                .With(m => m.ForeignKeyValue, fixture.Create<string>())
                .Create();

            foreignKeyModifyCommandHandler.Execute(foreignKeyModifyCommand);

            commonValidationsMock.AssertWasCalled(m => m.IsNotNull(Arg<IForeignKey>.Is.Equal(foreignKeyMock)));
        }

        /// <summary>
        /// Test that Execute calls HasValue for the foreign key value on the common validations.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsHasValueForForeignKeyValueOnCommonValidations()
        {
            var fixture = new Fixture();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var foodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IForeignKey>(Arg<Guid>.Is.Anything))
                // ReSharper disable PossibleInvalidOperationException
                .Return(DomainObjectMockBuilder.BuildForeignKeyMock(foodGroupMock.Identifier.Value, foodGroupMock.GetType()))
                // ReSharper restore PossibleInvalidOperationException
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

            var foreignKeyModifyCommandHandler = new ForeignKeyModifyCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock);
            Assert.That(foreignKeyModifyCommandHandler, Is.Not.Null);

            var foreignKeyModifyCommand = fixture.Build<ForeignKeyModifyCommand>()
                .With(m => m.ForeignKeyIdentifier, Guid.NewGuid())
                .With(m => m.ForeignKeyValue, fixture.Create<string>())
                .Create();

            foreignKeyModifyCommandHandler.Execute(foreignKeyModifyCommand);

            commonValidationsMock.AssertWasCalled(m => m.HasValue(Arg<string>.Is.Equal(foreignKeyModifyCommand.ForeignKeyValue)));
        }

        /// <summary>
        /// Test that Execute calls ContainsIllegalChar for the foreign key value on the common validations.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsContainsIllegalCharForForeignKeyValueOnCommonValidations()
        {
            var fixture = new Fixture();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var foodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IForeignKey>(Arg<Guid>.Is.Anything))
                // ReSharper disable PossibleInvalidOperationException
                .Return(DomainObjectMockBuilder.BuildForeignKeyMock(foodGroupMock.Identifier.Value, foodGroupMock.GetType()))
                // ReSharper restore PossibleInvalidOperationException
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

            var foreignKeyModifyCommandHandler = new ForeignKeyModifyCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock);
            Assert.That(foreignKeyModifyCommandHandler, Is.Not.Null);

            var foreignKeyModifyCommand = fixture.Build<ForeignKeyModifyCommand>()
                .With(m => m.ForeignKeyIdentifier, Guid.NewGuid())
                .With(m => m.ForeignKeyValue, fixture.Create<string>())
                .Create();

            foreignKeyModifyCommandHandler.Execute(foreignKeyModifyCommand);

            commonValidationsMock.AssertWasCalled(m => m.ContainsIllegalChar(Arg<string>.Is.Equal(foreignKeyModifyCommand.ForeignKeyValue)));
        }

        /// <summary>
        /// Test that Execute calls IsSatisfiedBy on the specification which encapsulates validation rules 3 times.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsIsSatisfiedByOnSpecification3Times()
        {
            var fixture = new Fixture();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var foodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IForeignKey>(Arg<Guid>.Is.Anything))
                // ReSharper disable PossibleInvalidOperationException
                .Return(DomainObjectMockBuilder.BuildForeignKeyMock(foodGroupMock.Identifier.Value, foodGroupMock.GetType()))
                // ReSharper restore PossibleInvalidOperationException
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var foreignKeyModifyCommandHandler = new ForeignKeyModifyCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock);
            Assert.That(foreignKeyModifyCommandHandler, Is.Not.Null);

            var foreignKeyModifyCommand = fixture.Build<ForeignKeyModifyCommand>()
                .With(m => m.ForeignKeyIdentifier, Guid.NewGuid())
                .With(m => m.ForeignKeyValue, fixture.Create<string>())
                .Create();

            foreignKeyModifyCommandHandler.Execute(foreignKeyModifyCommand);

            specificationMock.AssertWasCalled(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<IntranetBusinessException>.Is.TypeOf), opt => opt.Repeat.Times(3));
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

            var foodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IForeignKey>(Arg<Guid>.Is.Anything))
                // ReSharper disable PossibleInvalidOperationException
                .Return(DomainObjectMockBuilder.BuildForeignKeyMock(foodGroupMock.Identifier.Value, foodGroupMock.GetType()))
                // ReSharper restore PossibleInvalidOperationException
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var foreignKeyModifyCommandHandler = new ForeignKeyModifyCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock);
            Assert.That(foreignKeyModifyCommandHandler, Is.Not.Null);

            var foreignKeyModifyCommand = fixture.Build<ForeignKeyModifyCommand>()
                .With(m => m.ForeignKeyIdentifier, Guid.NewGuid())
                .With(m => m.ForeignKeyValue, fixture.Create<string>())
                .Create();

            foreignKeyModifyCommandHandler.Execute(foreignKeyModifyCommand);

            specificationMock.AssertWasCalled(m => m.Evaluate());
        }

        /// <summary>
        /// Test that Execute sets the value for ForeignKeyValue with the foreign key value from the command.
        /// </summary>
        [Test]
        public void TestThatExecuteSetsForeignKeyValueWithForeignKeyValueFromCommand()
        {
            var fixture = new Fixture();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var foodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock();
            // ReSharper disable PossibleInvalidOperationException
            var foreginKeyMock = DomainObjectMockBuilder.BuildForeignKeyMock(foodGroupMock.Identifier.Value, foodGroupMock.GetType());
            // ReSharper restore PossibleInvalidOperationException
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IForeignKey>(Arg<Guid>.Is.Anything))
                .Return(foreginKeyMock)
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var foreignKeyModifyCommandHandler = new ForeignKeyModifyCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock);
            Assert.That(foreignKeyModifyCommandHandler, Is.Not.Null);

            var foreignKeyModifyCommand = fixture.Build<ForeignKeyModifyCommand>()
                .With(m => m.ForeignKeyIdentifier, Guid.NewGuid())
                .With(m => m.ForeignKeyValue, fixture.Create<string>())
                .Create();

            foreignKeyModifyCommandHandler.Execute(foreignKeyModifyCommand);

            foreginKeyMock.AssertWasCalled(m => m.ForeignKeyValue = Arg<string>.Is.Equal(foreignKeyModifyCommand.ForeignKeyValue));
        }

        /// <summary>
        /// Tests that HandleException throws an ArgumentNullException if the command for modifying a foreign key is null.
        /// </summary>
        [Test]
        public void TestThatHandleExceptionThrowsArgumentNullExceptionIfForeignKeyModifyCommandIsNull()
        {
            var fixture = new Fixture();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var foreignKeyModifyCommandHandler = new ForeignKeyModifyCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock);
            Assert.That(foreignKeyModifyCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foreignKeyModifyCommandHandler
                .HandleException(null, fixture.Create<Exception>()));
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

            var foreignKeyModifyCommandHandler = new ForeignKeyModifyCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock);
            Assert.That(foreignKeyModifyCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foreignKeyModifyCommandHandler.HandleException(fixture.Create<ForeignKeyModifyCommand>(), null));
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

            var foreignKeyModifyCommandHandler = new ForeignKeyModifyCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock);
            Assert.That(foreignKeyModifyCommandHandler, Is.Not.Null);

            var incomingException = fixture.Create<IntranetRepositoryException>();

            var exception = Assert.Throws<IntranetRepositoryException>(() => foreignKeyModifyCommandHandler.HandleException(fixture.Create<ForeignKeyModifyCommand>(), incomingException));
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

            var foreignKeyModifyCommandHandler = new ForeignKeyModifyCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock);
            Assert.That(foreignKeyModifyCommandHandler, Is.Not.Null);

            var incomingException = fixture.Create<IntranetBusinessException>();

            var exception = Assert.Throws<IntranetBusinessException>(() => foreignKeyModifyCommandHandler.HandleException(fixture.Create<ForeignKeyModifyCommand>(), incomingException));
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

            var foreignKeyModifyCommandHandler = new ForeignKeyModifyCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock);
            Assert.That(foreignKeyModifyCommandHandler, Is.Not.Null);

            var incomingException = fixture.Create<IntranetSystemException>();

            var exception = Assert.Throws<IntranetSystemException>(() => foreignKeyModifyCommandHandler.HandleException(fixture.Create<ForeignKeyModifyCommand>(), incomingException));
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

            var foreignKeyModifyCommandHandler = new ForeignKeyModifyCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock);
            Assert.That(foreignKeyModifyCommandHandler, Is.Not.Null);

            var incomingException = fixture.Create<Exception>();

            var exception = Assert.Throws<IntranetSystemException>(() => foreignKeyModifyCommandHandler.HandleException(fixture.Create<ForeignKeyModifyCommand>(), incomingException));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.ErrorInCommandHandlerWithReturnValue, typeof (ForeignKeyModifyCommand).Name, typeof (ServiceReceiptResponse).Name, incomingException.Message)));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.EqualTo(incomingException));
        }
    }
}
