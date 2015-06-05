﻿using System;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommandHandlers;
using OSDevGrp.OSIntranet.CommandHandlers.Validation;
using OSDevGrp.OSIntranet.Contracts.Commands;
using OSDevGrp.OSIntranet.Contracts.Responses;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Resources;
using OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste;
using Ploeh.AutoFixture;
using Rhino.Mocks;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.Tests.Unittests.CommandHandlers
{
    /// <summary>
    /// Tests the command handler which handles a command for adding a translation.
    /// </summary>
    [TestFixture]
    public class TranslationAddCommandHandlerTests
    {
        /// <summary>
        /// Tests that the constructor initialize a command handler which handles a command for adding a translation.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeTranslationAddCommandHandler()
        {
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var translationAddCommandHandler = new TranslationAddCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock);
            Assert.That(translationAddCommandHandler, Is.Not.Null);
        }

        /// <summary>
        /// Tests that Execute throws an ArgumentNullException if the command for adding a translation is null.
        /// </summary>
        [Test]
        public void TestThatExecuteThrowsArgumentNullExceptionIfTranslationAddCommandIsNull()
        {
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var translationAddCommandHandler = new TranslationAddCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock);
            Assert.That(translationAddCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => translationAddCommandHandler.Execute(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("command"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Execute calls Get to get the translation information on the repository which can access system data for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsGetForTranslationInfoOnSystemDataRepository()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<ITranslation>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationMock())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var translationAddCommand = fixture.Build<TranslationAddCommand>()
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .With(m => m.TranslationOfIdentifier, Guid.NewGuid())
                .With(m => m.TranslationValue, fixture.Create<string>())
                .Create();

            var translationAddCommandHandler = new TranslationAddCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock);
            Assert.That(translationAddCommandHandler, Is.Not.Null);

            translationAddCommandHandler.Execute(translationAddCommand);

            systemDataRepositoryMock.AssertWasCalled(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Equal(translationAddCommand.TranslationInfoIdentifier)));
        }

        /// <summary>
        /// Test that Execute calls IsNotNull for the translation information on the common validations.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsIsNotNullForTranslationInfoOnCommonValidations()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var translationInfoMock = DomainObjectMockBuilder.BuildTranslationInfoMock();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(translationInfoMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<ITranslation>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationMock())
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

            var translationAddCommand = fixture.Build<TranslationAddCommand>()
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .With(m => m.TranslationOfIdentifier, Guid.NewGuid())
                .With(m => m.TranslationValue, fixture.Create<string>())
                .Create();

            var translationAddCommandHandler = new TranslationAddCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock);
            Assert.That(translationAddCommandHandler, Is.Not.Null);

            translationAddCommandHandler.Execute(translationAddCommand);

            commonValidationsMock.AssertWasCalled(m => m.IsNotNull(Arg<ITranslationInfo>.Is.Equal(translationInfoMock)));
        }

        /// <summary>
        /// Test that Execute calls HasValue for the translation value on the common validations.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsHasValueForTranslationValueOnCommonValidations()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<ITranslation>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationMock())
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

            var translationAddCommand = fixture.Build<TranslationAddCommand>()
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .With(m => m.TranslationOfIdentifier, Guid.NewGuid())
                .With(m => m.TranslationValue, fixture.Create<string>())
                .Create();

            var translationAddCommandHandler = new TranslationAddCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock);
            Assert.That(translationAddCommandHandler, Is.Not.Null);

            translationAddCommandHandler.Execute(translationAddCommand);

            commonValidationsMock.AssertWasCalled(m => m.HasValue(Arg<string>.Is.Equal(translationAddCommand.TranslationValue)));
        }

        /// <summary>
        /// Test that Execute calls ContainsIllegalChar for the translation value on the common validations.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsContainsIllegalCharForTranslationValueOnCommonValidations()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<ITranslation>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationMock())
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

            var translationAddCommand = fixture.Build<TranslationAddCommand>()
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .With(m => m.TranslationOfIdentifier, Guid.NewGuid())
                .With(m => m.TranslationValue, fixture.Create<string>())
                .Create();

            var translationAddCommandHandler = new TranslationAddCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock);
            Assert.That(translationAddCommandHandler, Is.Not.Null);

            translationAddCommandHandler.Execute(translationAddCommand);

            commonValidationsMock.AssertWasCalled(m => m.ContainsIllegalChar(Arg<string>.Is.Equal(translationAddCommand.TranslationValue)));
        }

        /// <summary>
        /// Test that Execute calls IsSatisfiedBy on the specification which encapsulates validation rules 3 times.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsIsSatisfiedByOnSpecification3Times()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<ITranslation>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationMock())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var translationAddCommand = fixture.Build<TranslationAddCommand>()
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .With(m => m.TranslationOfIdentifier, Guid.NewGuid())
                .With(m => m.TranslationValue, fixture.Create<string>())
                .Create();

            var translationAddCommandHandler = new TranslationAddCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock);
            Assert.That(translationAddCommandHandler, Is.Not.Null);

            translationAddCommandHandler.Execute(translationAddCommand);

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

            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<ITranslation>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationMock())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var translationAddCommand = fixture.Build<TranslationAddCommand>()
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .With(m => m.TranslationOfIdentifier, Guid.NewGuid())
                .With(m => m.TranslationValue, fixture.Create<string>())
                .Create();

            var translationAddCommandHandler = new TranslationAddCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock);
            Assert.That(translationAddCommandHandler, Is.Not.Null);

            translationAddCommandHandler.Execute(translationAddCommand);

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

            var translationInfoMock = DomainObjectMockBuilder.BuildTranslationInfoMock();
            var translationOfIdentifier = Guid.NewGuid();
            var translationValue = fixture.Create<string>();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(translationInfoMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<ITranslation>.Is.NotNull))
                .WhenCalled(e =>
                {
                    var translation = (ITranslation) e.Arguments.ElementAt(0);
                    Assert.That(translation, Is.Not.Null);
                    Assert.That(translation.Identifier.HasValue, Is.False);
                    Assert.That(translation.TranslationOfIdentifier, Is.EqualTo(translationOfIdentifier));
                    Assert.That(translation.TranslationInfo, Is.Not.Null);
                    Assert.That(translation.TranslationInfo, Is.EqualTo(translationInfoMock));
                    Assert.That(translation.Value, Is.Not.Null);
                    Assert.That(translation.Value, Is.Not.Empty);
                    Assert.That(translation.Value, Is.EqualTo(translationValue));
                })
                .Return(DomainObjectMockBuilder.BuildTranslationMock())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var translationAddCommand = fixture.Build<TranslationAddCommand>()
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .With(m => m.TranslationOfIdentifier, translationOfIdentifier)
                .With(m => m.TranslationValue, translationValue)
                .Create();

            var translationAddCommandHandler = new TranslationAddCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock);
            Assert.That(translationAddCommandHandler, Is.Not.Null);

            translationAddCommandHandler.Execute(translationAddCommand);

            systemDataRepositoryMock.AssertWasCalled(m => m.Insert(Arg<ITranslation>.Is.NotNull));
        }

        /// <summary>
        /// Test that Execute calls Map for the inserted translation on the object mapper for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsMapForInsertedTranslationOnFoodWasteObjectMapper()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var insertedTranslationMock = DomainObjectMockBuilder.BuildTranslationMock();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<ITranslation>.Is.NotNull))
                .Return(insertedTranslationMock)
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var translationAddCommand = fixture.Build<TranslationAddCommand>()
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .With(m => m.TranslationOfIdentifier, Guid.NewGuid())
                .With(m => m.TranslationValue, fixture.Create<string>())
                .Create();

            var translationAddCommandHandler = new TranslationAddCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock);
            Assert.That(translationAddCommandHandler, Is.Not.Null);

            translationAddCommandHandler.Execute(translationAddCommand);

            foodWasteObjectMapperMock.AssertWasCalled(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Equal(insertedTranslationMock), Arg<CultureInfo>.Is.Null));
        }

        /// <summary>
        /// Test that Execute returns the service receipt created Map for the inserted translation on the object mapper for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatExecuteReturnsServiceReceiptFromMapOnFoodWasteObjectMapper()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Insert(Arg<ITranslation>.Is.NotNull))
                .Return(DomainObjectMockBuilder.BuildTranslationMock())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var serviceReceipt = fixture.Build<ServiceReceiptResponse>()
                .With(m => m.Identifier, Guid.NewGuid())
                .With(m => m.EventDate, DateTime.Now)
                .Create();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(serviceReceipt)
                .Repeat.Any();

            var translationAddCommand = fixture.Build<TranslationAddCommand>()
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .With(m => m.TranslationOfIdentifier, Guid.NewGuid())
                .With(m => m.TranslationValue, fixture.Create<string>())
                .Create();

            var translationAddCommandHandler = new TranslationAddCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock);
            Assert.That(translationAddCommandHandler, Is.Not.Null);

            var result = translationAddCommandHandler.Execute(translationAddCommand);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(serviceReceipt));
        }

        /// <summary>
        /// Tests that HandleException throws an ArgumentNullException if the command for adding a translation is null.
        /// </summary>
        [Test]
        public void TestThatHandleExceptionThrowsArgumentNullExceptionIfTranslationAddCommandIsNull()
        {
            var fixture = new Fixture();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var translationAddCommandHandler = new TranslationAddCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock);
            Assert.That(translationAddCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => translationAddCommandHandler.HandleException(null, fixture.Create<Exception>()));
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

            var translationAddCommandHandler = new TranslationAddCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock);
            Assert.That(translationAddCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => translationAddCommandHandler.HandleException(fixture.Create<TranslationAddCommand>(), null));
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

            var translationAddCommandHandler = new TranslationAddCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock);
            Assert.That(translationAddCommandHandler, Is.Not.Null);

            var incomingException = fixture.Create<IntranetRepositoryException>();

            var exception = Assert.Throws<IntranetRepositoryException>(() => translationAddCommandHandler.HandleException(fixture.Create<TranslationAddCommand>(), incomingException));
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

            var translationAddCommandHandler = new TranslationAddCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock);
            Assert.That(translationAddCommandHandler, Is.Not.Null);

            var incomingException = fixture.Create<IntranetBusinessException>();

            var exception = Assert.Throws<IntranetBusinessException>(() => translationAddCommandHandler.HandleException(fixture.Create<TranslationAddCommand>(), incomingException));
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

            var translationAddCommandHandler = new TranslationAddCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock);
            Assert.That(translationAddCommandHandler, Is.Not.Null);

            var incomingException = fixture.Create<IntranetSystemException>();

            var exception = Assert.Throws<IntranetSystemException>(() => translationAddCommandHandler.HandleException(fixture.Create<TranslationAddCommand>(), incomingException));
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

            var translationAddCommandHandler = new TranslationAddCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock);
            Assert.That(translationAddCommandHandler, Is.Not.Null);

            var incomingException = fixture.Create<Exception>();

            var exception = Assert.Throws<IntranetSystemException>(() => translationAddCommandHandler.HandleException(fixture.Create<TranslationAddCommand>(), incomingException));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.ErrorInCommandHandlerWithReturnValue, typeof (TranslationAddCommand).Name, typeof (ServiceReceiptResponse).Name, incomingException.Message)));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.EqualTo(incomingException));
        }
    }
}
