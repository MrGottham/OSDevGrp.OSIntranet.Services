using System;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommandHandlers;
using OSDevGrp.OSIntranet.CommandHandlers.Validation;
using OSDevGrp.OSIntranet.Contracts.Commands;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
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
    /// Test the command handler which handles a command for updatering a household to the current users household account.
    /// </summary>
    [TestFixture]
    public class HouseholdUpdateCommandHandlerTests
    {
        /// <summary>
        /// Tests that the constructor initialize a command handler which handles a command for updatering a household to the current users household account.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeHouseholdUpdateCommandHandler()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdUpdateCommandHandler = new HouseholdUpdateCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdUpdateCommandHandler, Is.Not.Null);
            Assert.That(householdUpdateCommandHandler.ShouldBeActivated, Is.True);
            Assert.That(householdUpdateCommandHandler.ShouldHaveAcceptedPrivacyPolicy, Is.True);
            Assert.That(householdUpdateCommandHandler.RequiredMembership, Is.EqualTo(Membership.Basic));

        }

        /// <summary>
        /// Tests that AddValidationRules throws an ArgumentNullException when the household on which to modify data is null.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesThrowsArgumentNullExceptionWhenHouseholdIsNull()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdUpdateCommandHandler = new HouseholdUpdateCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdUpdateCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdUpdateCommandHandler.AddValidationRules(null, fixture.Create<HouseholdUpdateCommand>(), specificationMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("household"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that AddValidationRules throws an ArgumentNullException when the command for updatering a household to the current users household account is null.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesThrowsArgumentNullExceptionWhenHouseholdUpdateCommandIsNull()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdUpdateCommandHandler = new HouseholdUpdateCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdUpdateCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdUpdateCommandHandler.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), null, specificationMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("command"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that AddValidationRules throws an ArgumentNullException when the specification which encapsulates validation rules is null.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesThrowsArgumentNullExceptionWhenSpecificationIsNull()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdUpdateCommandHandler = new HouseholdUpdateCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdUpdateCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdUpdateCommandHandler.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), fixture.Create<HouseholdUpdateCommand>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("specification"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that AddValidationRules calls HasValue with Name on the common validations.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsHasValueWithNameOnCommonValidations()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<Exception>.Is.Anything))
                .WhenCalled(e =>
                {
                    var func = (Func<bool>) e.Arguments.ElementAt(0);
                    func.Invoke();
                })
                .Return(specificationMock)
                .Repeat.Any();

            var householdUpdateCommandHandler = new HouseholdUpdateCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdUpdateCommandHandler, Is.Not.Null);

            var name = fixture.Create<string>();
            var householdUpdateCommand = fixture.Build<HouseholdUpdateCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.Name, name)
                .With(m => m.Description, fixture.Create<string>())
                .Create();

            householdUpdateCommandHandler.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), householdUpdateCommand, specificationMock);

            commonValidationsMock.AssertWasCalled(m => m.HasValue(Arg<string>.Is.Equal(name)));
        }

        /// <summary>
        /// Tests that AddValidationRules calls IsLengthValid with Name on the common validations.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsIsLengthValidWithNameOnCommonValidations()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<Exception>.Is.Anything))
                .WhenCalled(e =>
                {
                    var func = (Func<bool>) e.Arguments.ElementAt(0);
                    func.Invoke();
                })
                .Return(specificationMock)
                .Repeat.Any();

            var householdUpdateCommandHandler = new HouseholdUpdateCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdUpdateCommandHandler, Is.Not.Null);

            var name = fixture.Create<string>();
            var householdUpdateCommand = fixture.Build<HouseholdUpdateCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.Name, name)
                .With(m => m.Description, fixture.Create<string>())
                .Create();

            householdUpdateCommandHandler.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), householdUpdateCommand, specificationMock);

            commonValidationsMock.AssertWasCalled(m => m.IsLengthValid(Arg<string>.Is.Equal(name), Arg<int>.Is.Equal(1), Arg<int>.Is.Equal(64)));
        }

        /// <summary>
        /// Tests that AddValidationRules calls ContainsIllegalChar with Name on the common validations.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsContainsIllegalCharWithNameOnCommonValidations()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<Exception>.Is.Anything))
                .WhenCalled(e =>
                {
                    var func = (Func<bool>) e.Arguments.ElementAt(0);
                    func.Invoke();
                })
                .Return(specificationMock)
                .Repeat.Any();

            var householdUpdateCommandHandler = new HouseholdUpdateCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdUpdateCommandHandler, Is.Not.Null);

            var name = fixture.Create<string>();
            var householdUpdateCommand = fixture.Build<HouseholdUpdateCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.Name, name)
                .With(m => m.Description, fixture.Create<string>())
                .Create();

            householdUpdateCommandHandler.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), householdUpdateCommand, specificationMock);

            commonValidationsMock.AssertWasCalled(m => m.ContainsIllegalChar(Arg<string>.Is.Equal(name)));
        }

        /// <summary>
        /// Tests that AddValidationRules calls HasValue with Description on the common validations when the description is not null.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsHasValueWithDescriptionOnCommonValidationsWhenDescriptionIsNotNull()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<Exception>.Is.Anything))
                .WhenCalled(e =>
                {
                    var func = (Func<bool>) e.Arguments.ElementAt(0);
                    func.Invoke();
                })
                .Return(specificationMock)
                .Repeat.Any();

            var householdUpdateCommandHandler = new HouseholdUpdateCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdUpdateCommandHandler, Is.Not.Null);

            var description = fixture.Create<string>();
            var householdUpdateCommand = fixture.Build<HouseholdUpdateCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.Name, fixture.Create<string>())
                .With(m => m.Description, description)
                .Create();

            householdUpdateCommandHandler.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), householdUpdateCommand, specificationMock);

            commonValidationsMock.AssertWasCalled(m => m.HasValue(Arg<string>.Is.Equal(description)));
        }

        /// <summary>
        /// Tests that AddValidationRules calls IsLengthValid with Description on the common validations when the description is not null.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsIsLengthValidWithDescriptionOnCommonValidationsWhenDescriptionIsNotNull()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<Exception>.Is.Anything))
                .WhenCalled(e =>
                {
                    var func = (Func<bool>) e.Arguments.ElementAt(0);
                    func.Invoke();
                })
                .Return(specificationMock)
                .Repeat.Any();

            var householdUpdateCommandHandler = new HouseholdUpdateCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdUpdateCommandHandler, Is.Not.Null);

            var description = fixture.Create<string>();
            var householdUpdateCommand = fixture.Build<HouseholdUpdateCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.Name, fixture.Create<string>())
                .With(m => m.Description, description)
                .Create();

            householdUpdateCommandHandler.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), householdUpdateCommand, specificationMock);

            commonValidationsMock.AssertWasCalled(m => m.IsLengthValid(Arg<string>.Is.Equal(description), Arg<int>.Is.Equal(1), Arg<int>.Is.Equal(2048)));
        }

        /// <summary>
        /// Tests that AddValidationRules calls ContainsIllegalChar with Description on the common validations when the description is not null.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsContainsIllegalCharWithDescriptionOnCommonValidationsWhenDescriptionIsNotNull()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<Exception>.Is.Anything))
                .WhenCalled(e =>
                {
                    var func = (Func<bool>) e.Arguments.ElementAt(0);
                    func.Invoke();
                })
                .Return(specificationMock)
                .Repeat.Any();

            var householdUpdateCommandHandler = new HouseholdUpdateCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdUpdateCommandHandler, Is.Not.Null);

            var description = fixture.Create<string>();
            var householdUpdateCommand = fixture.Build<HouseholdUpdateCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.Name, fixture.Create<string>())
                .With(m => m.Description, description)
                .Create();

            householdUpdateCommandHandler.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), householdUpdateCommand, specificationMock);

            commonValidationsMock.AssertWasCalled(m => m.ContainsIllegalChar(Arg<string>.Is.Equal(description)));
        }

        /// <summary>
        /// Tests that AddValidationRules does not call HasValue with Description on the common validations when the description is null.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesDoesNotCallHasValueWithDescriptionOnCommonValidationsWhenDescriptionIsNull()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<Exception>.Is.Anything))
                .WhenCalled(e =>
                {
                    var func = (Func<bool>) e.Arguments.ElementAt(0);
                    func.Invoke();
                })
                .Return(specificationMock)
                .Repeat.Any();

            var householdUpdateCommandHandler = new HouseholdUpdateCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdUpdateCommandHandler, Is.Not.Null);

            const string description = null;
            var householdUpdateCommand = fixture.Build<HouseholdUpdateCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.Name, fixture.Create<string>())
                .With(m => m.Description, description)
                .Create();

            householdUpdateCommandHandler.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), householdUpdateCommand, specificationMock);

            commonValidationsMock.AssertWasNotCalled(m => m.HasValue(Arg<string>.Is.Equal(description)));
        }

        /// <summary>
        /// Tests that AddValidationRules does not call IsLengthValid with Description on the common validations when the description is null.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesDoesNotCallIsLengthValidWithDescriptionOnCommonValidationsWhenDescriptionIsNull()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<Exception>.Is.Anything))
                .WhenCalled(e =>
                {
                    var func = (Func<bool>) e.Arguments.ElementAt(0);
                    func.Invoke();
                })
                .Return(specificationMock)
                .Repeat.Any();

            var householdUpdateCommandHandler = new HouseholdUpdateCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdUpdateCommandHandler, Is.Not.Null);

            const string description = null;
            var householdUpdateCommand = fixture.Build<HouseholdUpdateCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.Name, fixture.Create<string>())
                .With(m => m.Description, description)
                .Create();

            householdUpdateCommandHandler.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), householdUpdateCommand, specificationMock);

            commonValidationsMock.AssertWasNotCalled(m => m.IsLengthValid(Arg<string>.Is.Equal(description), Arg<int>.Is.Anything, Arg<int>.Is.Anything));
        }

        /// <summary>
        /// Tests that AddValidationRules does not call ContainsIllegalChar with Description on the common validations when the description is null.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesDoesNotCallContainsIllegalCharWithDescriptionOnCommonValidationsWhenDescriptionIsNull()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<Exception>.Is.Anything))
                .WhenCalled(e =>
                {
                    var func = (Func<bool>) e.Arguments.ElementAt(0);
                    func.Invoke();
                })
                .Return(specificationMock)
                .Repeat.Any();

            var householdUpdateCommandHandler = new HouseholdUpdateCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdUpdateCommandHandler, Is.Not.Null);

            const string description = null;
            var householdUpdateCommand = fixture.Build<HouseholdUpdateCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.Name, fixture.Create<string>())
                .With(m => m.Description, description)
                .Create();

            householdUpdateCommandHandler.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), householdUpdateCommand, specificationMock);

            commonValidationsMock.AssertWasNotCalled(m => m.ContainsIllegalChar(Arg<string>.Is.Equal(description)));
        }

        /// <summary>
        /// Tests that AddValidationRules calls IsSatisfiedBy on the specification which encapsulates validation rules 6 times.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsIsSatisfiedByOnSpecification6Times()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdUpdateCommandHandler = new HouseholdUpdateCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdUpdateCommandHandler, Is.Not.Null);

            var householdUpdateCommand = fixture.Build<HouseholdUpdateCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.Name, fixture.Create<string>())
                .With(m => m.Description, fixture.Create<string>())
                .Create();

            householdUpdateCommandHandler.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), householdUpdateCommand, specificationMock);

            specificationMock.AssertWasCalled(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<IntranetBusinessException>.Is.TypeOf), opt => opt.Repeat.Times(6));
        }

        /// <summary>
        /// Tests that ModifyData throws an ArgumentNullException when the household on which to modify data is null.
        /// </summary>
        [Test]
        public void TestThatModifyDataThrowsArgumentNullExceptionWhenHouseholdIsNull()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdUpdateCommandHandler = new HouseholdUpdateCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdUpdateCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdUpdateCommandHandler.ModifyData(null, fixture.Create<HouseholdUpdateCommand>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("household"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that ModifyData throws an ArgumentNullException when the command for updatering a household to the current users household account is null.
        /// </summary>
        [Test]
        public void TestThatModifyDataThrowsArgumentNullExceptionWhenHouseholdUpdateCommandIsNull()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdUpdateCommandHandler = new HouseholdUpdateCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdUpdateCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdUpdateCommandHandler.ModifyData(DomainObjectMockBuilder.BuildHouseholdMock(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("command"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that ModifyData calls the setter of Name on the household with the name from the command.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsNameSetterOnHouseholdWithNameFromCommand()
        {
            var fixture = new Fixture();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();

            var householdUpdateCommandHandler = new HouseholdUpdateCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdUpdateCommandHandler, Is.Not.Null);

            var householdMock = DomainObjectMockBuilder.BuildHouseholdMock();

            var name = fixture.Create<string>();
            var householdUpdateCommand = fixture.Build<HouseholdUpdateCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.Name, name)
                .With(m => m.Description, fixture.Create<string>())
                .Create();

            householdUpdateCommandHandler.ModifyData(householdMock, householdUpdateCommand);

            householdMock.AssertWasCalled(m => m.Name = Arg<string>.Is.Equal(name));
        }

        /// <summary>
        /// Tests that ModifyData calls the setter of Description on the household with the name from the command.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsDescriptionSetterOnHouseholdWithDescriptionFromCommand()
        {
            var fixture = new Fixture();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();

            var householdUpdateCommandHandler = new HouseholdUpdateCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdUpdateCommandHandler, Is.Not.Null);

            var householdMock = DomainObjectMockBuilder.BuildHouseholdMock();

            var description = fixture.Create<string>();
            var householdUpdateCommand = fixture.Build<HouseholdUpdateCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.Name, fixture.Create<string>())
                .With(m => m.Description, description)
                .Create();

            householdUpdateCommandHandler.ModifyData(householdMock, householdUpdateCommand);

            householdMock.AssertWasCalled(m => m.Description = Arg<string>.Is.Equal(description));
        }

        /// <summary>
        /// Tests that ModifyData calls Update with the updated household on the repository which can access household data for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsUpdateWithUpdatedHouseholdOnHouseholdDataRepository()
        {
            var fixture = new Fixture();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();

            var householdUpdateCommandHandler = new HouseholdUpdateCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdUpdateCommandHandler, Is.Not.Null);

            var householdMock = DomainObjectMockBuilder.BuildHouseholdMock();

            var householdUpdateCommand = fixture.Build<HouseholdUpdateCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.Name, fixture.Create<string>())
                .With(m => m.Description, fixture.Create<string>())
                .Create();

            householdUpdateCommandHandler.ModifyData(householdMock, householdUpdateCommand);

            householdDataRepositoryMock.AssertWasCalled(m => m.Update(Arg<IHousehold>.Is.Equal(householdMock)));
        }

        /// <summary>
        /// Tests that ModifyData return the result from Update on the repository which can access household data for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatModifyDataReturnsResultFromUpdateWithOnHouseholdDataRepository()
        {
            var fixture = new Fixture();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var updatedHouseholdMock = DomainObjectMockBuilder.BuildHouseholdMock();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(updatedHouseholdMock)
                .Repeat.Any();

            var householdUpdateCommandHandler = new HouseholdUpdateCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdUpdateCommandHandler, Is.Not.Null);

            var householdUpdateCommand = fixture.Build<HouseholdUpdateCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.Name, fixture.Create<string>())
                .With(m => m.Description, fixture.Create<string>())
                .Create();

            var result = householdUpdateCommandHandler.ModifyData(DomainObjectMockBuilder.BuildHouseholdMock(), householdUpdateCommand);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(updatedHouseholdMock));
        }
    }
}
