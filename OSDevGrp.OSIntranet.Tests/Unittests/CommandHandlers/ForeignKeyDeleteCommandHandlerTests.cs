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
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.CommandHandlers
{
    /// <summary>
    /// Tests the command handler which handles a command for deleting a foreign key.
    /// </summary>
    [TestFixture]
    public class ForeignKeyDeleteCommandHandlerTests
    {
        /// <summary>
        /// Tests that the constructor initialize a command handler which handles a command for deleting a foreign key.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeForeignKeyDeleteCommandHandler()
        {
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var foreignKeyDeleteCommandHandler = new ForeignKeyDeleteCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(foreignKeyDeleteCommandHandler, Is.Not.Null);
        }

        /// <summary>
        /// Tests that Execute throws an ArgumentNullException if the command for deleting a foreign key is null.
        /// </summary>
        [Test]
        public void TestThatExecuteThrowsArgumentNullExceptionIfForeignKeyModifyCommandIsNull()
        {
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var foreignKeyDeleteCommandHandler = new ForeignKeyDeleteCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(foreignKeyDeleteCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foreignKeyDeleteCommandHandler.Execute(null));
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
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

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

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var foreignKeyDeleteCommandHandler = new ForeignKeyDeleteCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(foreignKeyDeleteCommandHandler, Is.Not.Null);

            var foreignKeyDeleteCommand = new ForeignKeyDeleteCommand
            {
                ForeignKeyIdentifier = Guid.NewGuid()
            };

            foreignKeyDeleteCommandHandler.Execute(foreignKeyDeleteCommand);

            systemDataRepositoryMock.AssertWasCalled(m => m.Get<IForeignKey>(Arg<Guid>.Is.Equal(foreignKeyDeleteCommand.ForeignKeyIdentifier)));
        }

        /// <summary>
        /// Test that Execute calls IsNotNull for the foreign key on the common validations.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsIsNotNullForForeignKeyOnCommonValidations()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

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

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var foreignKeyDeleteCommandHandler = new ForeignKeyDeleteCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(foreignKeyDeleteCommandHandler, Is.Not.Null);

            var foreignKeyDeleteCommand = new ForeignKeyDeleteCommand
            {
                ForeignKeyIdentifier = Guid.NewGuid()
            };

            foreignKeyDeleteCommandHandler.Execute(foreignKeyDeleteCommand);

            commonValidationsMock.AssertWasCalled(m => m.IsNotNull(Arg<IForeignKey>.Is.Equal(foreignKeyMock)));
        }

        /// <summary>
        /// Test that Execute calls IsSatisfiedBy on the specification which encapsulates validation rules 1 time.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsIsSatisfiedByOnSpecification1Time()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

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

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var foreignKeyDeleteCommandHandler = new ForeignKeyDeleteCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(foreignKeyDeleteCommandHandler, Is.Not.Null);

            var foreignKeyDeleteCommand = new ForeignKeyDeleteCommand
            {
                ForeignKeyIdentifier = Guid.NewGuid()
            };

            foreignKeyDeleteCommandHandler.Execute(foreignKeyDeleteCommand);

            specificationMock.AssertWasCalled(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<IntranetBusinessException>.Is.TypeOf), opt => opt.Repeat.Times(1));
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

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var foreignKeyDeleteCommandHandler = new ForeignKeyDeleteCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(foreignKeyDeleteCommandHandler, Is.Not.Null);

            var foreignKeyDeleteCommand = new ForeignKeyDeleteCommand
            {
                ForeignKeyIdentifier = Guid.NewGuid()
            };

            foreignKeyDeleteCommandHandler.Execute(foreignKeyDeleteCommand);

            specificationMock.AssertWasCalled(m => m.Evaluate());
        }

        /// <summary>
        /// Test that Execute calls Delete on the repository which can access system data for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsDeleteOnSystemDataRepository()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var foodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock();
            // ReSharper disable PossibleInvalidOperationException
            var foreignKeyMock = DomainObjectMockBuilder.BuildForeignKeyMock(foodGroupMock.Identifier.Value, foodGroupMock.GetType());
            // ReSharper restore PossibleInvalidOperationException
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IForeignKey>(Arg<Guid>.Is.Anything))
                .Return(foreignKeyMock)
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var foreignKeyDeleteCommandHandler = new ForeignKeyDeleteCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(foreignKeyDeleteCommandHandler, Is.Not.Null);

            var foreignKeyDeleteCommand = new ForeignKeyDeleteCommand
            {
                ForeignKeyIdentifier = Guid.NewGuid()
            };

            foreignKeyDeleteCommandHandler.Execute(foreignKeyDeleteCommand);

            systemDataRepositoryMock.AssertWasCalled(m => m.Delete(Arg<IForeignKey>.Is.Equal(foreignKeyMock)));
        }

        /// <summary>
        /// Test that Execute calls Map for the foreign key which has been deleted on the object mapper for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsMapForDeletedForeignKeyOnFoodWasteObjectMapper()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var foodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock();
            // ReSharper disable PossibleInvalidOperationException
            var foreignKeyMock = DomainObjectMockBuilder.BuildForeignKeyMock(foodGroupMock.Identifier.Value, foodGroupMock.GetType());
            // ReSharper restore PossibleInvalidOperationException
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IForeignKey>(Arg<Guid>.Is.Anything))
                .Return(foreignKeyMock)
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var foreignKeyDeleteCommandHandler = new ForeignKeyDeleteCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(foreignKeyDeleteCommandHandler, Is.Not.Null);

            var foreignKeyDeleteCommand = new ForeignKeyDeleteCommand
            {
                ForeignKeyIdentifier = Guid.NewGuid()
            };

            foreignKeyDeleteCommandHandler.Execute(foreignKeyDeleteCommand);

            foodWasteObjectMapperMock.AssertWasCalled(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Equal(foreignKeyMock), Arg<CultureInfo>.Is.Null));
        }

        /// <summary>
        /// Test that Execute returns the service receipt created by Map for the updated foreign key on the object mapper for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatExecuteReturnsServiceReceiptFromMapOnFoodWasteObjectMapper()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

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

            var serviceReceipt = fixture.Create<ServiceReceiptResponse>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(serviceReceipt)
                .Repeat.Any();

            var foreignKeyDeleteCommandHandler = new ForeignKeyDeleteCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(foreignKeyDeleteCommandHandler, Is.Not.Null);

            var foreignKeyDeleteCommand = new ForeignKeyDeleteCommand
            {
                ForeignKeyIdentifier = Guid.NewGuid()
            };

            var result = foreignKeyDeleteCommandHandler.Execute(foreignKeyDeleteCommand);
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
                    Assert.That(methodBase.ReflectedType.Name, Is.EqualTo(typeof (ForeignKeyDeleteCommandHandler).Name));
                })
                .Return(fixture.Create<Exception>())
                .Repeat.Any();

            var foreignKeyDeleteCommandHandler = new ForeignKeyDeleteCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(foreignKeyDeleteCommandHandler, Is.Not.Null);

            var exception = fixture.Create<Exception>();
            Assert.Throws<Exception>(() => foreignKeyDeleteCommandHandler.HandleException(fixture.Create<ForeignKeyDeleteCommand>(), exception));

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

            var foreignKeyDeleteCommandHandler = new ForeignKeyDeleteCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(foreignKeyDeleteCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<Exception>(() => foreignKeyDeleteCommandHandler.HandleException(fixture.Create<ForeignKeyDeleteCommand>(), fixture.Create<Exception>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception, Is.EqualTo(exceptionToThrow));
        }
    }
}
