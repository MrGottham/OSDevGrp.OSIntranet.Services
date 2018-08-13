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
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var foreignKeyModifyCommandHandler = new ForeignKeyModifyCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
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
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var foreignKeyModifyCommandHandler = new ForeignKeyModifyCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
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
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var foodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IForeignKey>(Arg<Guid>.Is.Anything))
                // ReSharper disable PossibleInvalidOperationException
                .Return(DomainObjectMockBuilder.BuildForeignKeyMock(foodGroupMock.Identifier.Value, foodGroupMock.GetType()))
                // ReSharper restore PossibleInvalidOperationException
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Update(Arg<IForeignKey>.Is.Anything))
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

            var foreignKeyModifyCommandHandler = new ForeignKeyModifyCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
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
            systemDataRepositoryMock.Stub(m => m.Update(Arg<IForeignKey>.Is.Anything))
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

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var foreignKeyModifyCommandHandler = new ForeignKeyModifyCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
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
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var foodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IForeignKey>(Arg<Guid>.Is.Anything))
                // ReSharper disable PossibleInvalidOperationException
                .Return(DomainObjectMockBuilder.BuildForeignKeyMock(foodGroupMock.Identifier.Value, foodGroupMock.GetType()))
                // ReSharper restore PossibleInvalidOperationException
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Update(Arg<IForeignKey>.Is.Anything))
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

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var foreignKeyModifyCommandHandler = new ForeignKeyModifyCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
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
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var foodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IForeignKey>(Arg<Guid>.Is.Anything))
                // ReSharper disable PossibleInvalidOperationException
                .Return(DomainObjectMockBuilder.BuildForeignKeyMock(foodGroupMock.Identifier.Value, foodGroupMock.GetType()))
                // ReSharper restore PossibleInvalidOperationException
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Update(Arg<IForeignKey>.Is.Anything))
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

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var foreignKeyModifyCommandHandler = new ForeignKeyModifyCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
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
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var foodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IForeignKey>(Arg<Guid>.Is.Anything))
                // ReSharper disable PossibleInvalidOperationException
                .Return(DomainObjectMockBuilder.BuildForeignKeyMock(foodGroupMock.Identifier.Value, foodGroupMock.GetType()))
                // ReSharper restore PossibleInvalidOperationException
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Update(Arg<IForeignKey>.Is.Anything))
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

            var foreignKeyModifyCommandHandler = new ForeignKeyModifyCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
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
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var foodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IForeignKey>(Arg<Guid>.Is.Anything))
                // ReSharper disable PossibleInvalidOperationException
                .Return(DomainObjectMockBuilder.BuildForeignKeyMock(foodGroupMock.Identifier.Value, foodGroupMock.GetType()))
                // ReSharper restore PossibleInvalidOperationException
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Update(Arg<IForeignKey>.Is.Anything))
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

            var foreignKeyModifyCommandHandler = new ForeignKeyModifyCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
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
            systemDataRepositoryMock.Stub(m => m.Update(Arg<IForeignKey>.Is.Anything))
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

            var foreignKeyModifyCommandHandler = new ForeignKeyModifyCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(foreignKeyModifyCommandHandler, Is.Not.Null);

            var foreignKeyModifyCommand = fixture.Build<ForeignKeyModifyCommand>()
                .With(m => m.ForeignKeyIdentifier, Guid.NewGuid())
                .With(m => m.ForeignKeyValue, fixture.Create<string>())
                .Create();

            foreignKeyModifyCommandHandler.Execute(foreignKeyModifyCommand);

            foreignKeyMock.AssertWasCalled(m => m.ForeignKeyValue = Arg<string>.Is.Equal(foreignKeyModifyCommand.ForeignKeyValue));
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

            var foodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock();
            // ReSharper disable PossibleInvalidOperationException
            var foreignKeyMock = DomainObjectMockBuilder.BuildForeignKeyMock(foodGroupMock.Identifier.Value, foodGroupMock.GetType());
            // ReSharper restore PossibleInvalidOperationException
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IForeignKey>(Arg<Guid>.Is.Anything))
                .Return(foreignKeyMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Update(Arg<IForeignKey>.Is.Anything))
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

            var foreignKeyModifyCommandHandler = new ForeignKeyModifyCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(foreignKeyModifyCommandHandler, Is.Not.Null);

            var foreignKeyModifyCommand = fixture.Build<ForeignKeyModifyCommand>()
                .With(m => m.ForeignKeyIdentifier, Guid.NewGuid())
                .With(m => m.ForeignKeyValue, fixture.Create<string>())
                .Create();

            foreignKeyModifyCommandHandler.Execute(foreignKeyModifyCommand);

            systemDataRepositoryMock.AssertWasCalled(m => m.Update(Arg<IForeignKey>.Is.Equal(foreignKeyMock)));
        }

        /// <summary>
        /// Test that Execute calls Map for the updated foreign key on the object mapper for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsMapForUpdatedForeignKeyOnFoodWasteObjectMapper()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var foodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock();
            // ReSharper disable PossibleInvalidOperationException
            var updatedForeignKeyMock = DomainObjectMockBuilder.BuildForeignKeyMock(foodGroupMock.Identifier.Value, foodGroupMock.GetType());
            // ReSharper restore PossibleInvalidOperationException
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IForeignKey>(Arg<Guid>.Is.Anything))
                // ReSharper disable PossibleInvalidOperationException
                .Return(DomainObjectMockBuilder.BuildForeignKeyMock(foodGroupMock.Identifier.Value, foodGroupMock.GetType()))
                // ReSharper restore PossibleInvalidOperationException
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Update(Arg<IForeignKey>.Is.Anything))
                .Return(updatedForeignKeyMock)
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var foreignKeyModifyCommandHandler = new ForeignKeyModifyCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(foreignKeyModifyCommandHandler, Is.Not.Null);

            var foreignKeyModifyCommand = fixture.Build<ForeignKeyModifyCommand>()
                .With(m => m.ForeignKeyIdentifier, Guid.NewGuid())
                .With(m => m.ForeignKeyValue, fixture.Create<string>())
                .Create();

            foreignKeyModifyCommandHandler.Execute(foreignKeyModifyCommand);

            foodWasteObjectMapperMock.AssertWasCalled(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Equal(updatedForeignKeyMock), Arg<CultureInfo>.Is.Null));
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
            systemDataRepositoryMock.Stub(m => m.Update(Arg<IForeignKey>.Is.Anything))
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

            var foreignKeyModifyCommandHandler = new ForeignKeyModifyCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(foreignKeyModifyCommandHandler, Is.Not.Null);

            var foreignKeyModifyCommand = fixture.Build<ForeignKeyModifyCommand>()
                .With(m => m.ForeignKeyIdentifier, Guid.NewGuid())
                .With(m => m.ForeignKeyValue, fixture.Create<string>())
                .Create();

            var result = foreignKeyModifyCommandHandler.Execute(foreignKeyModifyCommand);
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
                    Assert.That(methodBase.ReflectedType.Name, Is.EqualTo(typeof (ForeignKeyModifyCommandHandler).Name));
                })
                .Return(fixture.Create<Exception>())
                .Repeat.Any();

            var foreignKeyModifyCommandHandler = new ForeignKeyModifyCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(foreignKeyModifyCommandHandler, Is.Not.Null);

            var exception = fixture.Create<Exception>();
            Assert.Throws<Exception>(() => foreignKeyModifyCommandHandler.HandleException(fixture.Create<ForeignKeyModifyCommand>(), exception));

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

            var foreignKeyModifyCommandHandler = new ForeignKeyModifyCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(foreignKeyModifyCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<Exception>(() => foreignKeyModifyCommandHandler.HandleException(fixture.Create<ForeignKeyModifyCommand>(), fixture.Create<Exception>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception, Is.EqualTo(exceptionToThrow));
        }
    }
}
