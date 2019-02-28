using System;
using System.Linq;
using AutoFixture;
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
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.CommandHandlers
{
    /// <summary>
    /// Test the command handler which handles a command for updating a household to the current users household account.
    /// </summary>
    [TestFixture]
    public class HouseholdUpdateCommandHandlerTests
    {
        #region Private variables

        private IHouseholdDataRepository _householdDataRepositoryMock;
        private IClaimValueProvider _claimValueProviderMock;
        private IFoodWasteObjectMapper _objectMapperMock;
        private ISpecification _specificationMock;
        private ICommonValidations _commonValidationsMock;
        private IExceptionBuilder _exceptionBuilderMock;
        private Fixture _fixture;

        #endregion

        /// <summary>
        /// Setup each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            _claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            _objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            _specificationMock = MockRepository.GenerateMock<ISpecification>();
            _commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            _exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();
            _fixture = new Fixture();
        }

        /// <summary>
        /// Tests that the constructor initialize a command handler which handles a command for updating a household to the current users household account.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeHouseholdUpdateCommandHandler()
        {
            HouseholdUpdateCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.ShouldBeActivated, Is.True);
            Assert.That(sut.ShouldHaveAcceptedPrivacyPolicy, Is.True);
            Assert.That(sut.RequiredMembership, Is.EqualTo(Membership.Basic));
        }

        /// <summary>
        /// Tests that AddValidationRules throws an ArgumentNullException when the household on which to modify data is null.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesThrowsArgumentNullExceptionWhenHouseholdIsNull()
        {
            HouseholdUpdateCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AddValidationRules(null, _fixture.Create<HouseholdUpdateCommand>(), _specificationMock));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "household");
        }

        /// <summary>
        /// Tests that AddValidationRules throws an ArgumentNullException when the command for updating a household to the current users household account is null.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesThrowsArgumentNullExceptionWhenHouseholdUpdateCommandIsNull()
        {
            HouseholdUpdateCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), null, _specificationMock));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "command");
        }

        /// <summary>
        /// Tests that AddValidationRules throws an ArgumentNullException when the specification which encapsulates validation rules is null.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesThrowsArgumentNullExceptionWhenSpecificationIsNull()
        {
            HouseholdUpdateCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), _fixture.Create<HouseholdUpdateCommand>(), null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "specification");
        }

        /// <summary>
        /// Tests that AddValidationRules calls HasValue with Name on the common validations.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsHasValueWithNameOnCommonValidations()
        {
            HouseholdUpdateCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            string name = _fixture.Create<string>();
            HouseholdUpdateCommand householdUpdateCommand = _fixture.Build<HouseholdUpdateCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.Name, name)
                .With(m => m.Description, _fixture.Create<string>())
                .Create();

            sut.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), householdUpdateCommand, _specificationMock);

            _commonValidationsMock.AssertWasCalled(m => m.HasValue(Arg<string>.Is.Equal(name)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that AddValidationRules calls IsLengthValid with Name on the common validations.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsIsLengthValidWithNameOnCommonValidations()
        {
            HouseholdUpdateCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            string name = _fixture.Create<string>();
            HouseholdUpdateCommand householdUpdateCommand = _fixture.Build<HouseholdUpdateCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.Name, name)
                .With(m => m.Description, _fixture.Create<string>())
                .Create();

            sut.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), householdUpdateCommand, _specificationMock);

            _commonValidationsMock.AssertWasCalled(m => m.IsLengthValid(Arg<string>.Is.Equal(name), Arg<int>.Is.Equal(1), Arg<int>.Is.Equal(64)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that AddValidationRules calls ContainsIllegalChar with Name on the common validations.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsContainsIllegalCharWithNameOnCommonValidations()
        {
            HouseholdUpdateCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            string name = _fixture.Create<string>();
            HouseholdUpdateCommand householdUpdateCommand = _fixture.Build<HouseholdUpdateCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.Name, name)
                .With(m => m.Description, _fixture.Create<string>())
                .Create();

            sut.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), householdUpdateCommand, _specificationMock);

            _commonValidationsMock.AssertWasCalled(m => m.ContainsIllegalChar(Arg<string>.Is.Equal(name)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that AddValidationRules calls HasValue with Description on the common validations when the description is not null.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsHasValueWithDescriptionOnCommonValidationsWhenDescriptionIsNotNull()
        {
            HouseholdUpdateCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            string description = _fixture.Create<string>();
            HouseholdUpdateCommand householdUpdateCommand = _fixture.Build<HouseholdUpdateCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.Name, _fixture.Create<string>())
                .With(m => m.Description, description)
                .Create();

            sut.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), householdUpdateCommand, _specificationMock);

            _commonValidationsMock.AssertWasCalled(m => m.HasValue(Arg<string>.Is.Equal(description)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that AddValidationRules calls IsLengthValid with Description on the common validations when the description is not null.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsIsLengthValidWithDescriptionOnCommonValidationsWhenDescriptionIsNotNull()
        {
            HouseholdUpdateCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            string description = _fixture.Create<string>();
            HouseholdUpdateCommand householdUpdateCommand = _fixture.Build<HouseholdUpdateCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.Name, _fixture.Create<string>())
                .With(m => m.Description, description)
                .Create();

            sut.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), householdUpdateCommand, _specificationMock);

            _commonValidationsMock.AssertWasCalled(m => m.IsLengthValid(Arg<string>.Is.Equal(description), Arg<int>.Is.Equal(1), Arg<int>.Is.Equal(2048)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that AddValidationRules calls ContainsIllegalChar with Description on the common validations when the description is not null.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsContainsIllegalCharWithDescriptionOnCommonValidationsWhenDescriptionIsNotNull()
        {
            HouseholdUpdateCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            string description = _fixture.Create<string>();
            HouseholdUpdateCommand householdUpdateCommand = _fixture.Build<HouseholdUpdateCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.Name, _fixture.Create<string>())
                .With(m => m.Description, description)
                .Create();

            sut.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), householdUpdateCommand, _specificationMock);

            _commonValidationsMock.AssertWasCalled(m => m.ContainsIllegalChar(Arg<string>.Is.Equal(description)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that AddValidationRules does not call HasValue with Description on the common validations when the description is null.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesDoesNotCallHasValueWithDescriptionOnCommonValidationsWhenDescriptionIsNull()
        {
            HouseholdUpdateCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            const string description = null;
            HouseholdUpdateCommand householdUpdateCommand = _fixture.Build<HouseholdUpdateCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.Name, _fixture.Create<string>())
                .With(m => m.Description, description)
                .Create();

            sut.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), householdUpdateCommand, _specificationMock);

            _commonValidationsMock.AssertWasNotCalled(m => m.HasValue(Arg<string>.Is.Equal(description)));
        }

        /// <summary>
        /// Tests that AddValidationRules does not call IsLengthValid with Description on the common validations when the description is null.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesDoesNotCallIsLengthValidWithDescriptionOnCommonValidationsWhenDescriptionIsNull()
        {
            HouseholdUpdateCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            const string description = null;
            HouseholdUpdateCommand householdUpdateCommand = _fixture.Build<HouseholdUpdateCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.Name, _fixture.Create<string>())
                .With(m => m.Description, description)
                .Create();

            sut.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), householdUpdateCommand, _specificationMock);

            _commonValidationsMock.AssertWasNotCalled(m => m.IsLengthValid(Arg<string>.Is.Equal(description), Arg<int>.Is.Anything, Arg<int>.Is.Anything));
        }

        /// <summary>
        /// Tests that AddValidationRules does not call ContainsIllegalChar with Description on the common validations when the description is null.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesDoesNotCallContainsIllegalCharWithDescriptionOnCommonValidationsWhenDescriptionIsNull()
        {
            HouseholdUpdateCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            const string description = null;
            HouseholdUpdateCommand householdUpdateCommand = _fixture.Build<HouseholdUpdateCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.Name, _fixture.Create<string>())
                .With(m => m.Description, description)
                .Create();

            sut.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), householdUpdateCommand, _specificationMock);

            _commonValidationsMock.AssertWasNotCalled(m => m.ContainsIllegalChar(Arg<string>.Is.Equal(description)));
        }

        /// <summary>
        /// Tests that AddValidationRules calls IsSatisfiedBy on the specification which encapsulates validation rules 6 times.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsIsSatisfiedByOnSpecification6Times()
        {
            HouseholdUpdateCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            HouseholdUpdateCommand householdUpdateCommand = _fixture.Build<HouseholdUpdateCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.Name, _fixture.Create<string>())
                .With(m => m.Description, _fixture.Create<string>())
                .Create();

            sut.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), householdUpdateCommand, _specificationMock);

            _specificationMock.AssertWasCalled(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<IntranetBusinessException>.Is.TypeOf), opt => opt.Repeat.Times(6));
        }

        /// <summary>
        /// Tests that ModifyData throws an ArgumentNullException when the household on which to modify data is null.
        /// </summary>
        [Test]
        public void TestThatModifyDataThrowsArgumentNullExceptionWhenHouseholdIsNull()
        {
            HouseholdUpdateCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ModifyData(null, _fixture.Create<HouseholdUpdateCommand>()));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "household");
        }

        /// <summary>
        /// Tests that ModifyData throws an ArgumentNullException when the command for updating a household to the current users household account is null.
        /// </summary>
        [Test]
        public void TestThatModifyDataThrowsArgumentNullExceptionWhenHouseholdUpdateCommandIsNull()
        {
            HouseholdUpdateCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ModifyData(DomainObjectMockBuilder.BuildHouseholdMock(), null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "command");
        }

        /// <summary>
        /// Tests that ModifyData calls the setter of Name on the household with the name from the command.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsNameSetterOnHouseholdWithNameFromCommand()
        {
            HouseholdUpdateCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            IHousehold householdMock = DomainObjectMockBuilder.BuildHouseholdMock();

            string name = _fixture.Create<string>();
            HouseholdUpdateCommand householdUpdateCommand = _fixture.Build<HouseholdUpdateCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.Name, name)
                .With(m => m.Description, _fixture.Create<string>())
                .Create();

            sut.ModifyData(householdMock, householdUpdateCommand);

            householdMock.AssertWasCalled(m => m.Name = Arg<string>.Is.Equal(name), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that ModifyData calls the setter of Description on the household with the name from the command.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsDescriptionSetterOnHouseholdWithDescriptionFromCommand()
        {
            HouseholdUpdateCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            IHousehold householdMock = DomainObjectMockBuilder.BuildHouseholdMock();

            string description = _fixture.Create<string>();
            HouseholdUpdateCommand householdUpdateCommand = _fixture.Build<HouseholdUpdateCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.Name, _fixture.Create<string>())
                .With(m => m.Description, description)
                .Create();

            sut.ModifyData(householdMock, householdUpdateCommand);

            householdMock.AssertWasCalled(m => m.Description = Arg<string>.Is.Equal(description), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that ModifyData calls Update with the updated household on the repository which can access household data for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsUpdateWithUpdatedHouseholdOnHouseholdDataRepository()
        {
            HouseholdUpdateCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            IHousehold householdMock = DomainObjectMockBuilder.BuildHouseholdMock();

            HouseholdUpdateCommand householdUpdateCommand = _fixture.Build<HouseholdUpdateCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.Name, _fixture.Create<string>())
                .With(m => m.Description, _fixture.Create<string>())
                .Create();

            sut.ModifyData(householdMock, householdUpdateCommand);

            _householdDataRepositoryMock.AssertWasCalled(m => m.Update(Arg<IHousehold>.Is.Equal(householdMock)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that ModifyData return the result from Update on the repository which can access household data for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatModifyDataReturnsResultFromUpdateWithOnHouseholdDataRepository()
        {
            IHousehold updatedHouseholdMock = DomainObjectMockBuilder.BuildHouseholdMock();

            HouseholdUpdateCommandHandler sut = CreateSut(updatedHouseholdMock);
            Assert.That(sut, Is.Not.Null);

            HouseholdUpdateCommand householdUpdateCommand = _fixture.Build<HouseholdUpdateCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.Name, _fixture.Create<string>())
                .With(m => m.Description, _fixture.Create<string>())
                .Create();

            IIdentifiable result = sut.ModifyData(DomainObjectMockBuilder.BuildHouseholdMock(), householdUpdateCommand);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(updatedHouseholdMock));
        }

        /// <summary>
        /// Creates an instance of the <see cref="HouseholdUpdateCommandHandler"/> which can be used for unit testing.
        /// </summary>
        /// <returns>Instance of the <see cref="HouseholdUpdateCommandHandler"/> which can be used for unit testing.</returns>
        private HouseholdUpdateCommandHandler CreateSut(IHousehold updatedHousehold = null)
        {
            _specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .WhenCalled(e =>
                {
                    Func<bool> func = (Func<bool>) e.Arguments.ElementAt(0);
                    func();
                })
                .Return(_specificationMock)
                .Repeat.Any();

            _householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(updatedHousehold ?? DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();

            return new HouseholdUpdateCommandHandler(_householdDataRepositoryMock, _claimValueProviderMock, _objectMapperMock, _specificationMock, _commonValidationsMock, _exceptionBuilderMock);
        }
    }
}
