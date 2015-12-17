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
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var foreignKeyAddCommandHandler = new ForeignKeyAddCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
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
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var foreignKeyAddCommandHandler = new ForeignKeyAddCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
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
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var foodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<IFoodGroup>(Arg<Guid>.Is.Anything))
                .Return(foodGroupMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<IForeignKey>.Is.Anything))
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

            var foreignKeyAddCommand = fixture.Build<ForeignKeyAddCommand>()
                .With(m => m.DataProviderIdentifier, Guid.NewGuid())
                .With(m => m.ForeignKeyForIdentifier, Guid.NewGuid())
                .With(m => m.ForeignKeyValue, fixture.Create<string>())
                .Create();

            var foreignKeyAddCommandHandler = new ForeignKeyAddCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
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
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var foodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<IFoodGroup>(Arg<Guid>.Is.Anything))
                .Return(foodGroupMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<IForeignKey>.Is.Anything))
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

            var foreignKeyAddCommand = fixture.Build<ForeignKeyAddCommand>()
                .With(m => m.DataProviderIdentifier, Guid.NewGuid())
                .With(m => m.ForeignKeyForIdentifier, Guid.NewGuid())
                .With(m => m.ForeignKeyValue, fixture.Create<string>())
                .Create();

            var foreignKeyAddCommandHandler = new ForeignKeyAddCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
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
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var dataProviderMock = DomainObjectMockBuilder.BuildDataProviderMock();
            var foodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(dataProviderMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<IFoodGroup>(Arg<Guid>.Is.Anything))
                .Return(foodGroupMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<IForeignKey>.Is.Anything))
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

            var foreignKeyAddCommand = fixture.Build<ForeignKeyAddCommand>()
                .With(m => m.DataProviderIdentifier, Guid.NewGuid())
                .With(m => m.ForeignKeyForIdentifier, Guid.NewGuid())
                .With(m => m.ForeignKeyValue, fixture.Create<string>())
                .Create();

            var foreignKeyAddCommandHandler = new ForeignKeyAddCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(foreignKeyAddCommandHandler, Is.Not.Null);

            foreignKeyAddCommandHandler.Execute(foreignKeyAddCommand);

            commonValidationsMock.AssertWasCalled(m => m.IsNotNull(Arg<IDataProvider>.Is.Equal(dataProviderMock)));
        }

        /// <summary>
        /// Test that Execute calls IsNotNull for the domain object which has the foreign key on the common validations.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsIsNotNullForForeignKeyForOnCommonValidations()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var foodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<IFoodGroup>(Arg<Guid>.Is.Anything))
                .Return(foodGroupMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<IForeignKey>.Is.Anything))
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

            var foreignKeyAddCommand = fixture.Build<ForeignKeyAddCommand>()
                .With(m => m.DataProviderIdentifier, Guid.NewGuid())
                .With(m => m.ForeignKeyForIdentifier, Guid.NewGuid())
                .With(m => m.ForeignKeyValue, fixture.Create<string>())
                .Create();

            var foreignKeyAddCommandHandler = new ForeignKeyAddCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(foreignKeyAddCommandHandler, Is.Not.Null);

            foreignKeyAddCommandHandler.Execute(foreignKeyAddCommand);

            commonValidationsMock.AssertWasCalled(m => m.IsNotNull(Arg<IFoodGroup>.Is.Equal(foodGroupMock)));
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
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<IFoodGroup>(Arg<Guid>.Is.Anything))
                .Return(foodGroupMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<IForeignKey>.Is.Anything))
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

            var foreignKeyAddCommand = fixture.Build<ForeignKeyAddCommand>()
                .With(m => m.DataProviderIdentifier, Guid.NewGuid())
                .With(m => m.ForeignKeyForIdentifier, Guid.NewGuid())
                .With(m => m.ForeignKeyValue, fixture.Create<string>())
                .Create();

            var foreignKeyAddCommandHandler = new ForeignKeyAddCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(foreignKeyAddCommandHandler, Is.Not.Null);

            foreignKeyAddCommandHandler.Execute(foreignKeyAddCommand);

            commonValidationsMock.AssertWasCalled(m => m.HasValue(Arg<string>.Is.Equal(foreignKeyAddCommand.ForeignKeyValue)));
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
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<IFoodGroup>(Arg<Guid>.Is.Anything))
                .Return(foodGroupMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<IForeignKey>.Is.Anything))
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

            var foreignKeyAddCommand = fixture.Build<ForeignKeyAddCommand>()
                .With(m => m.DataProviderIdentifier, Guid.NewGuid())
                .With(m => m.ForeignKeyForIdentifier, Guid.NewGuid())
                .With(m => m.ForeignKeyValue, fixture.Create<string>())
                .Create();

            var foreignKeyAddCommandHandler = new ForeignKeyAddCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(foreignKeyAddCommandHandler, Is.Not.Null);

            foreignKeyAddCommandHandler.Execute(foreignKeyAddCommand);

            commonValidationsMock.AssertWasCalled(m => m.ContainsIllegalChar(Arg<string>.Is.Equal(foreignKeyAddCommand.ForeignKeyValue)));
        }

        /// <summary>
        /// Test that Execute calls IsSatisfiedBy on the specification which encapsulates validation rules 4 times.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsIsSatisfiedByOnSpecification4Times()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var foodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<IFoodGroup>(Arg<Guid>.Is.Anything))
                .Return(foodGroupMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<IForeignKey>.Is.Anything))
                // ReSharper disable PossibleInvalidOperationException
                .Return(DomainObjectMockBuilder.BuildForeignKeyMock(foodGroupMock.Identifier.Value, foodGroupMock.GetType()))
                // ReSharper restore PossibleInvalidOperationException
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var foreignKeyAddCommand = fixture.Build<ForeignKeyAddCommand>()
                .With(m => m.DataProviderIdentifier, Guid.NewGuid())
                .With(m => m.ForeignKeyForIdentifier, Guid.NewGuid())
                .With(m => m.ForeignKeyValue, fixture.Create<string>())
                .Create();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var foreignKeyAddCommandHandler = new ForeignKeyAddCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(foreignKeyAddCommandHandler, Is.Not.Null);

            foreignKeyAddCommandHandler.Execute(foreignKeyAddCommand);

            specificationMock.AssertWasCalled(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<IntranetBusinessException>.Is.TypeOf), opt => opt.Repeat.Times(4));
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
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<IFoodGroup>(Arg<Guid>.Is.Anything))
                .Return(foodGroupMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<IForeignKey>.Is.Anything))
                // ReSharper disable PossibleInvalidOperationException
                .Return(DomainObjectMockBuilder.BuildForeignKeyMock(foodGroupMock.Identifier.Value, foodGroupMock.GetType()))
                // ReSharper restore PossibleInvalidOperationException
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var foreignKeyAddCommand = fixture.Build<ForeignKeyAddCommand>()
                .With(m => m.DataProviderIdentifier, Guid.NewGuid())
                .With(m => m.ForeignKeyForIdentifier, Guid.NewGuid())
                .With(m => m.ForeignKeyValue, fixture.Create<string>())
                .Create();

            var foreignKeyAddCommandHandler = new ForeignKeyAddCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(foreignKeyAddCommandHandler, Is.Not.Null);

            foreignKeyAddCommandHandler.Execute(foreignKeyAddCommand);

            specificationMock.AssertWasCalled(m => m.Evaluate());
        }

        /// <summary>
        /// Test that Execute calls Insert on the repository which can access system data for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsInsertOnSystemDataRepository()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var foreignKeyValue = fixture.Create<string>();
            var dataProviderMock = DomainObjectMockBuilder.BuildDataProviderMock();
            var foodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(dataProviderMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<IFoodGroup>(Arg<Guid>.Is.Anything))
                .Return(foodGroupMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<IForeignKey>.Is.NotNull))
                .WhenCalled(e =>
                {
                    var foreignKey = (IForeignKey) e.Arguments.ElementAt(0);
                    Assert.That(foreignKey, Is.Not.Null);
                    Assert.That(foreignKey.Identifier, Is.Null);
                    Assert.That(foreignKey.Identifier.HasValue, Is.False);
                    Assert.That(foreignKey.DataProvider, Is.Not.Null);
                    Assert.That(foreignKey.DataProvider, Is.EqualTo(dataProviderMock));
                    // ReSharper disable PossibleInvalidOperationException
                    Assert.That(foreignKey.ForeignKeyForIdentifier, Is.EqualTo(foodGroupMock.Identifier.Value));
                    // ReSharper restore PossibleInvalidOperationException
                    Assert.That(foreignKey.ForeignKeyForTypes, Is.Not.Null);
                    Assert.That(foreignKey.ForeignKeyForTypes, Is.Not.Empty);
                    Assert.That(foreignKey.ForeignKeyValue, Is.Not.Null);
                    Assert.That(foreignKey.ForeignKeyValue, Is.Not.Empty);
                    Assert.That(foreignKey.ForeignKeyValue, Is.EqualTo(foreignKeyValue));
                })
                // ReSharper disable PossibleInvalidOperationException
                .Return(DomainObjectMockBuilder.BuildForeignKeyMock(foodGroupMock.Identifier.Value, foodGroupMock.GetType()))
                // ReSharper restore PossibleInvalidOperationException
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var foreignKeyAddCommand = fixture.Build<ForeignKeyAddCommand>()
                .With(m => m.DataProviderIdentifier, Guid.NewGuid())
                .With(m => m.ForeignKeyForIdentifier, Guid.NewGuid())
                .With(m => m.ForeignKeyValue, foreignKeyValue)
                .Create();

            var foreignKeyAddCommandHandler = new ForeignKeyAddCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(foreignKeyAddCommandHandler, Is.Not.Null);

            foreignKeyAddCommandHandler.Execute(foreignKeyAddCommand);

            systemDataRepositoryMock.AssertWasCalled(m => m.Insert(Arg<IForeignKey>.Is.NotNull));
        }

        /// <summary>
        /// Test that Execute calls Map for the inserted foreign key on the object mapper for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsMapForInsertedForeignKeyOnFoodWasteObjectMapper()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var dataProviderMock = DomainObjectMockBuilder.BuildDataProviderMock();
            var foodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock();
            // ReSharper disable PossibleInvalidOperationException
            var insertedForeignKeyMock = DomainObjectMockBuilder.BuildForeignKeyMock(foodGroupMock.Identifier.Value, foodGroupMock.GetType());
            // ReSharper restore PossibleInvalidOperationException
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(dataProviderMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<IFoodGroup>(Arg<Guid>.Is.Anything))
                .Return(foodGroupMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<IForeignKey>.Is.NotNull))
                .Return(insertedForeignKeyMock)
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var foreignKeyAddCommand = fixture.Build<ForeignKeyAddCommand>()
                .With(m => m.DataProviderIdentifier, Guid.NewGuid())
                .With(m => m.ForeignKeyForIdentifier, Guid.NewGuid())
                .With(m => m.ForeignKeyValue, fixture.Create<string>())
                .Create();

            var foreignKeyAddCommandHandler = new ForeignKeyAddCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(foreignKeyAddCommandHandler, Is.Not.Null);

            foreignKeyAddCommandHandler.Execute(foreignKeyAddCommand);

            foodWasteObjectMapperMock.AssertWasCalled(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Equal(insertedForeignKeyMock), Arg<CultureInfo>.Is.Null));
        }

        /// <summary>
        /// Test that Execute returns the service receipt created by Map for the inserted foreign key on the object mapper for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatExecuteReturnsServiceReceiptFromMapOnFoodWasteObjectMapper()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var dataProviderMock = DomainObjectMockBuilder.BuildDataProviderMock();
            var foodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(dataProviderMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<IFoodGroup>(Arg<Guid>.Is.Anything))
                .Return(foodGroupMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<IForeignKey>.Is.NotNull))
                // ReSharper disable PossibleInvalidOperationException
                .Return(DomainObjectMockBuilder.BuildForeignKeyMock(foodGroupMock.Identifier.Value, foodGroupMock.GetType()))
                // ReSharper restore PossibleInvalidOperationException
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var serviceReceipt = fixture.Create<ServiceReceiptResponse>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(serviceReceipt)
                .Repeat.Any();

            var foreignKeyAddCommand = fixture.Build<ForeignKeyAddCommand>()
                .With(m => m.DataProviderIdentifier, Guid.NewGuid())
                .With(m => m.ForeignKeyForIdentifier, Guid.NewGuid())
                .With(m => m.ForeignKeyValue, fixture.Create<string>())
                .Create();

            var foreignKeyAddCommandHandler = new ForeignKeyAddCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(foreignKeyAddCommandHandler, Is.Not.Null);

            var result = foreignKeyAddCommandHandler.Execute(foreignKeyAddCommand);
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
                    Assert.That(methodBase.ReflectedType.Name, Is.EqualTo(typeof (ForeignKeyAddCommandHandler).Name));
                })
                .Return(fixture.Create<Exception>())
                .Repeat.Any();

            var foreignKeyAddCommandHandler = new ForeignKeyAddCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(foreignKeyAddCommandHandler, Is.Not.Null);

            var exception = fixture.Create<Exception>();
            Assert.Throws<Exception>(() => foreignKeyAddCommandHandler.HandleException(fixture.Create<ForeignKeyAddCommand>(), exception));

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

            var foreignKeyAddCommandHandler = new ForeignKeyAddCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(foreignKeyAddCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<Exception>(() => foreignKeyAddCommandHandler.HandleException(fixture.Create<ForeignKeyAddCommand>(), fixture.Create<Exception>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception, Is.EqualTo(exceptionToThrow));
        }
    }
}
