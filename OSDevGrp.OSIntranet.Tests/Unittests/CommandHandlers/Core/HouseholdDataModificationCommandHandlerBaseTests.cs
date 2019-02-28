using System;
using System.Linq;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommandHandlers.Core;
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

namespace OSDevGrp.OSIntranet.Tests.Unittests.CommandHandlers.Core
{
    /// <summary>
    /// Test the basic functionality which can handle a command for modifying some data on a given household on the current household member.
    /// </summary>
    [TestFixture]
    public class HouseholdDataModificationCommandHandlerBaseTests
    {
        /// <summary>
        /// Private class for a command for modifying some data on a given household on the current household member.
        /// </summary>
        private class MyHouseholdDataModificationCommand : HouseholdDataModificationCommandBase
        {
        }

        /// <summary>
        /// Private class for testing basic functionality which can handle a command for modifying some data on a given household on the current household member.
        /// </summary>
        /// <typeparam name="TCommand">Type of the command for modifying some data on a given household on the current household member.</typeparam>
        private class MyHouseholdDataModificationCommandHandler<TCommand> : HouseholdDataModificationCommandHandlerBase<TCommand> where TCommand : HouseholdDataModificationCommandBase
        {
            #region Private variables

            private IIdentifiable _modifyDataResult;

            #endregion

            #region Constructor

            /// <summary>
            /// Create an instance of the private class for testing basic functionality which can handle a command for modifying some data on a given household on the current household member.
            /// </summary>
            /// <param name="householdDataRepository">Implementation of a repository which can access household data for the food waste domain.</param>
            /// <param name="claimValueProvider">Implementation of a provider which can resolve values from the current users claims.</param>
            /// <param name="foodWasteObjectMapper">Implementation of an object mapper which can map objects in the food waste domain.</param>
            /// <param name="specification">Implementation of a specification which encapsulates validation rules.</param>
            /// <param name="commonValidations">Implementation of a common validations.</param>
            /// <param name="exceptionBuilder">Implementation of a builder which can build exceptions.</param>
            public MyHouseholdDataModificationCommandHandler(IHouseholdDataRepository householdDataRepository, IClaimValueProvider claimValueProvider, IFoodWasteObjectMapper foodWasteObjectMapper, ISpecification specification, ICommonValidations commonValidations, IExceptionBuilder exceptionBuilder)
                : base(householdDataRepository, claimValueProvider, foodWasteObjectMapper, specification, commonValidations, exceptionBuilder)
            {
            }

            #endregion

            #region Properties

            /// <summary>
            /// Indicates whether AddValidationRules is called.
            /// </summary>
            public bool AddValidationRulesIsCalled { get; private set; }

            /// <summary>
            /// Indicates whether ModifyData is called.
            /// </summary>
            public bool ModifyDataIsCalled { get; private set; }

            /// <summary>
            /// Gets the household which has been handled.
            /// </summary>
            public IHousehold HandledHousehold { get; private set; }

            /// <summary>
            /// Gets the command which has been handled.
            /// </summary>
            public TCommand HandledCommand { get; private set; }

            /// <summary>
            /// Gets the result which ModifyData should return.
            /// </summary>
            public IIdentifiable ModifyDataResult => _modifyDataResult ?? (_modifyDataResult = MockRepository.GenerateMock<IIdentifiable>());

            #endregion

            #region Methods

            /// <summary>
            /// Gets the repository which can access household data for the food waste domain.
            /// </summary>
            public IHouseholdDataRepository GetHouseholdDataRepository()
            {
                return base.HouseholdDataRepository;
            }

            /// <summary>
            /// Gets the provider which can resolve values from the current users claims.
            /// </summary>
            public IClaimValueProvider GetClaimValueProvider()
            {
                return base.ClaimValueProvider;
            }

            /// <summary>
            /// Gets the object mapper which can map objects in the food waste domain.
            /// </summary>
            public IFoodWasteObjectMapper GetObjectMapper()
            {
                return base.ObjectMapper;
            }

            /// <summary>
            /// Gets the specification which encapsulates validation rules.
            /// </summary>
            public ISpecification GetSpecification()
            {
                return base.Specification;
            }

            /// <summary>
            /// Gets the common validations.
            /// </summary>
            public ICommonValidations GetCommonValidations()
            {
                return base.CommonValidations;
            }

            /// <summary>
            /// Gets the builder which can build exceptions.
            /// </summary>
            public IExceptionBuilder GetExceptionBuilder()
            {
                return base.ExceptionBuilder;
            }

            /// <summary>
            /// Adds validation rules to the specification which encapsulates validation rules.
            /// </summary>
            /// <param name="household">Household on which to modify data.</param>
            /// <param name="command">Command for modifying some data on a given household on the current household member.</param>
            /// <param name="specification">Specification which encapsulates validation rules.</param>
            public override void AddValidationRules(IHousehold household, TCommand command, ISpecification specification)
            {
                Assert.That(household, Is.Not.Null);
                Assert.That(command, Is.Not.Null);
                Assert.That(specification, Is.Not.Null);
                Assert.That(specification, Is.EqualTo(base.Specification));

                AddValidationRulesIsCalled = true;
                HandledHousehold = household;
                HandledCommand = command;
            }

            /// <summary>
            /// Modifies the data.
            /// </summary>
            /// <param name="household">Household on which to modify data.</param>
            /// <param name="command">Command for modifying some data on a given household on the current household member.</param>
            /// <returns>An identifiable domain object in the food waste domain.</returns>
            public override IIdentifiable ModifyData(IHousehold household, TCommand command)
            {
                Assert.That(household, Is.Not.Null);
                Assert.That(command, Is.Not.Null);

                ModifyDataIsCalled = true;
                HandledHousehold = household;
                HandledCommand = command;

                return ModifyDataResult;
            }

            #endregion
        }

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
        /// Tests that the constructor initialize the basic functionality which can handle a command for modifying some data on a given household on the current household member.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeHouseholdDataModificationCommandHandlerBase()
        {
            MyHouseholdDataModificationCommandHandler<MyHouseholdDataModificationCommand> sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.ShouldBeActivated, Is.True);
            Assert.That(sut.ShouldHaveAcceptedPrivacyPolicy, Is.True);
            Assert.That(sut.RequiredMembership, Is.EqualTo(Membership.Basic));
            Assert.That(sut.GetHouseholdDataRepository(), Is.Not.Null);
            Assert.That(sut.GetHouseholdDataRepository(), Is.EqualTo(_householdDataRepositoryMock));
            Assert.That(sut.GetClaimValueProvider(), Is.Not.Null);
            Assert.That(sut.GetClaimValueProvider(), Is.EqualTo(_claimValueProviderMock));
            Assert.That(sut.GetObjectMapper(), Is.Not.Null);
            Assert.That(sut.GetObjectMapper(), Is.EqualTo(_objectMapperMock));
            Assert.That(sut.GetSpecification(), Is.Not.Null);
            Assert.That(sut.GetSpecification(), Is.EqualTo(_specificationMock));
            Assert.That(sut.GetCommonValidations(), Is.Not.Null);
            Assert.That(sut.GetCommonValidations(), Is.EqualTo(_commonValidationsMock));
            Assert.That(sut.GetExceptionBuilder(), Is.Not.Null);
            Assert.That(sut.GetExceptionBuilder(), Is.EqualTo(_exceptionBuilderMock));
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the repository which can access household data for the food waste domain is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenHouseholdDataRepositoryIsNull()
        {
            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => new MyHouseholdDataModificationCommandHandler<MyHouseholdDataModificationCommand>(null, _claimValueProviderMock, _objectMapperMock, _specificationMock, _commonValidationsMock, _exceptionBuilderMock));
            // ReSharper restore ObjectCreationAsStatement

            TestHelper.AssertArgumentNullExceptionIsValid(result, "householdDataRepository");
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the provider which can resolve values from the current users claims is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenClaimValueProviderIsNull()
        {
            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => new MyHouseholdDataModificationCommandHandler<MyHouseholdDataModificationCommand>(_householdDataRepositoryMock, null, _objectMapperMock, _specificationMock, _commonValidationsMock, _exceptionBuilderMock));
            // ReSharper restore ObjectCreationAsStatement

            TestHelper.AssertArgumentNullExceptionIsValid(result, "claimValueProvider");
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the object mapper which can map objects in the food waste domain is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenFoodWasteObjectMapperIsNull()
        {
            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => new MyHouseholdDataModificationCommandHandler<MyHouseholdDataModificationCommand>(_householdDataRepositoryMock, _claimValueProviderMock, null, _specificationMock, _commonValidationsMock, _exceptionBuilderMock));
            // ReSharper restore ObjectCreationAsStatement

            TestHelper.AssertArgumentNullExceptionIsValid(result, "foodWasteObjectMapper");
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the specification which encapsulates validation rules is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenSpecificationIsNull()
        {
            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => new MyHouseholdDataModificationCommandHandler<MyHouseholdDataModificationCommand>(_householdDataRepositoryMock, _claimValueProviderMock, _objectMapperMock, null, _commonValidationsMock, _exceptionBuilderMock));
            // ReSharper restore ObjectCreationAsStatement

            TestHelper.AssertArgumentNullExceptionIsValid(result, "specification");
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the common validations is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenCommonValidationsIsNull()
        {
            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => new MyHouseholdDataModificationCommandHandler<MyHouseholdDataModificationCommand>(_householdDataRepositoryMock, _claimValueProviderMock, _objectMapperMock, _specificationMock, null, _exceptionBuilderMock));
            // ReSharper restore ObjectCreationAsStatement

            TestHelper.AssertArgumentNullExceptionIsValid(result, "commonValidations");
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the builder which can build exceptions is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenExceptionBuilderIsNull()
        {
            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => new MyHouseholdDataModificationCommandHandler<MyHouseholdDataModificationCommand>(_householdDataRepositoryMock, _claimValueProviderMock, _objectMapperMock, _specificationMock, _commonValidationsMock, null));
            // ReSharper restore ObjectCreationAsStatement

            TestHelper.AssertArgumentNullExceptionIsValid(result, "exceptionBuilder");
        }

        /// <summary>
        /// Tests that AddValidationRules throws an ArgumentNullException when the household member for which to modify data is null.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesThrowsArgumentNullExceptionWhenHouseholdMemberIsNull()
        {
            MyHouseholdDataModificationCommandHandler<MyHouseholdDataModificationCommand> sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AddValidationRules((IHouseholdMember) null, _fixture.Create<MyHouseholdDataModificationCommand>(), _specificationMock));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "householdMember");
        }

        /// <summary>
        /// Tests that AddValidationRules throws an ArgumentNullException when the command for modifying some data on a given household on the current household member is null.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesThrowsArgumentNullExceptionWhenCommandIsNull()
        {
            MyHouseholdDataModificationCommandHandler<MyHouseholdDataModificationCommand> sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMemberMock(), null, _specificationMock));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "command");
        }

        /// <summary>
        /// Tests that AddValidationRules throws an ArgumentNullException when the specification which encapsulates validation rules is null.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesThrowsArgumentNullExceptionWhenSpecificationIsNull()
        {
            MyHouseholdDataModificationCommandHandler<MyHouseholdDataModificationCommand> sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMemberMock(), _fixture.Create<MyHouseholdDataModificationCommand>(), null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "specification");
        }

        /// <summary>
        /// Tests that AddValidationRules calls Households on the the household member for which to modify data.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsHouseholdsOnHouseholdMember()
        {
            MyHouseholdDataModificationCommandHandler<MyHouseholdDataModificationCommand> sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            IHouseholdMember householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            Assert.That(householdMemberMock, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Empty);

            MyHouseholdDataModificationCommand command = _fixture.Build<MyHouseholdDataModificationCommand>()
                // ReSharper disable PossibleInvalidOperationException
                .With(m => m.HouseholdIdentifier, householdMemberMock.Households.First().Identifier.Value)
                // ReSharper restore PossibleInvalidOperationException
                .Create();

            sut.AddValidationRules(householdMemberMock, command, _specificationMock);

            householdMemberMock.AssertWasCalled(m => m.Households, opt => opt.Repeat.Times(4)); // Tree times in the test and one time in AddValidationRules.
        }

        /// <summary>
        /// Tests that AddValidationRules calls IsNotNull with the household on which to modify data on the common validations.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsIsNotNullWithHouseholdOnCommonValidations()
        {
            MyHouseholdDataModificationCommandHandler<MyHouseholdDataModificationCommand> sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            IHouseholdMember householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            Assert.That(householdMemberMock, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Empty);

            IHousehold householdMock = householdMemberMock.Households.FirstOrDefault();
            Assert.IsNotNull(householdMock);

            MyHouseholdDataModificationCommand command = _fixture.Build<MyHouseholdDataModificationCommand>()
                // ReSharper disable PossibleInvalidOperationException
                .With(m => m.HouseholdIdentifier, householdMock.Identifier.Value)
                // ReSharper restore PossibleInvalidOperationException
                .Create();

            sut.AddValidationRules(householdMemberMock, command, _specificationMock);

            _commonValidationsMock.AssertWasCalled(m => m.IsNotNull(Arg<IHousehold>.Is.Equal(householdMock)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that AddValidationRules calls IsSatisfiedBy on the specification which encapsulates validation rules 1 time.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsIsSatisfiedByOnSpecification1Time()
        {
            MyHouseholdDataModificationCommandHandler<MyHouseholdDataModificationCommand> sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            IHouseholdMember householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            Assert.That(householdMemberMock, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Empty);

            MyHouseholdDataModificationCommand command = _fixture.Build<MyHouseholdDataModificationCommand>()
                // ReSharper disable PossibleInvalidOperationException
                .With(m => m.HouseholdIdentifier, householdMemberMock.Households.First().Identifier.Value)
                // ReSharper restore PossibleInvalidOperationException
                .Create();

            sut.AddValidationRules(householdMemberMock, command, _specificationMock);

            _specificationMock.AssertWasCalled(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<IntranetBusinessException>.Is.NotNull), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that AddValidationRules calls Evaluate on the specification which encapsulates validation.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsEvaluateOnSpecification()
        {
            MyHouseholdDataModificationCommandHandler<MyHouseholdDataModificationCommand> sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            IHouseholdMember householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            Assert.That(householdMemberMock, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Empty);

            MyHouseholdDataModificationCommand command = _fixture.Build<MyHouseholdDataModificationCommand>()
                // ReSharper disable PossibleInvalidOperationException
                .With(m => m.HouseholdIdentifier, householdMemberMock.Households.First().Identifier.Value)
                // ReSharper restore PossibleInvalidOperationException
                .Create();

            sut.AddValidationRules(householdMemberMock, command, _specificationMock);

            _specificationMock.AssertWasCalled(m => m.Evaluate(), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that AddValidationRules calls AddValidationRules with the household on which to modify data.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsAddValidationRulesWithHousehold()
        {
            MyHouseholdDataModificationCommandHandler<MyHouseholdDataModificationCommand> sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.AddValidationRulesIsCalled, Is.False);
            Assert.That(sut.HandledHousehold, Is.Null);
            Assert.That(sut.HandledCommand, Is.Null);

            IHouseholdMember householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            Assert.That(householdMemberMock, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Empty);

            IHousehold householdMock = householdMemberMock.Households.FirstOrDefault();
            Assert.That(householdMock, Is.Not.Null);

            MyHouseholdDataModificationCommand command = _fixture.Build<MyHouseholdDataModificationCommand>()
                // ReSharper disable PossibleInvalidOperationException
                .With(m => m.HouseholdIdentifier, householdMock.Identifier.Value)
                // ReSharper restore PossibleInvalidOperationException
                .Create();

            sut.AddValidationRules(householdMemberMock, command, _specificationMock);
            Assert.That(sut.AddValidationRulesIsCalled, Is.True);
            Assert.That(sut.HandledHousehold, Is.Not.Null);
            Assert.That(sut.HandledHousehold, Is.EqualTo(householdMock));
            Assert.That(sut.HandledCommand, Is.Not.Null);
            Assert.That(sut.HandledCommand, Is.EqualTo(command));
        }

        /// <summary>
        /// Tests that ModifyData throws an ArgumentNullException when the household member for which to modify data is null.
        /// </summary>
        [Test]
        public void TestThatModifyDataThrowsArgumentNullExceptionWhenHouseholdMemberIsNull()
        {
            MyHouseholdDataModificationCommandHandler<MyHouseholdDataModificationCommand> sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ModifyData((IHouseholdMember) null, _fixture.Create<MyHouseholdDataModificationCommand>()));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "householdMember");
        }

        /// <summary>
        /// Tests that ModifyData throws an ArgumentNullException when the command for modifying some data on a given household on the current household member is null.
        /// </summary>
        [Test]
        public void TestThatModifyDataThrowsArgumentNullExceptionWhenCommandIsNull()
        {
            MyHouseholdDataModificationCommandHandler<MyHouseholdDataModificationCommand> sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ModifyData(DomainObjectMockBuilder.BuildHouseholdMemberMock(), null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "command");
        }

        /// <summary>
        /// Tests that ModifyData calls Households on the the household member for which to modify data.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsHouseholdsOnHouseholdMember()
        {
            MyHouseholdDataModificationCommandHandler<MyHouseholdDataModificationCommand> sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            IHouseholdMember householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            Assert.That(householdMemberMock, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Empty);

            MyHouseholdDataModificationCommand command = _fixture.Build<MyHouseholdDataModificationCommand>()
                // ReSharper disable PossibleInvalidOperationException
                .With(m => m.HouseholdIdentifier, householdMemberMock.Households.First().Identifier.Value)
                // ReSharper restore PossibleInvalidOperationException
                .Create();

            sut.ModifyData(householdMemberMock, command);

            householdMemberMock.AssertWasCalled(m => m.Households, opt => opt.Repeat.Times(4)); // Tree times in the test and one time in AddValidationRules.
        }

        /// <summary>
        /// Tests that ModifyData calls IsNotNull with the household on which to modify data on the common validations.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsIsNotNullWithHouseholdOnCommonValidations()
        {
            MyHouseholdDataModificationCommandHandler<MyHouseholdDataModificationCommand> sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            IHouseholdMember householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            Assert.That(householdMemberMock, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Empty);

            IHousehold householdMock = householdMemberMock.Households.FirstOrDefault();
            Assert.IsNotNull(householdMock);

            MyHouseholdDataModificationCommand command = _fixture.Build<MyHouseholdDataModificationCommand>()
                // ReSharper disable PossibleInvalidOperationException
                .With(m => m.HouseholdIdentifier, householdMock.Identifier.Value)
                // ReSharper restore PossibleInvalidOperationException
                .Create();

            sut.ModifyData(householdMemberMock, command);

            _commonValidationsMock.AssertWasCalled(m => m.IsNotNull(Arg<IHousehold>.Is.Equal(householdMock)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that ModifyData calls IsSatisfiedBy on the specification which encapsulates validation rules 1 time.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsIsSatisfiedByOnSpecification1Time()
        {
            MyHouseholdDataModificationCommandHandler<MyHouseholdDataModificationCommand> sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            IHouseholdMember householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            Assert.That(householdMemberMock, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Empty);

            MyHouseholdDataModificationCommand command = _fixture.Build<MyHouseholdDataModificationCommand>()
                // ReSharper disable PossibleInvalidOperationException
                .With(m => m.HouseholdIdentifier, householdMemberMock.Households.First().Identifier.Value)
                // ReSharper restore PossibleInvalidOperationException
                .Create();

            sut.ModifyData(householdMemberMock, command);

            _specificationMock.AssertWasCalled(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<IntranetBusinessException>.Is.NotNull), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that ModifyData calls Evaluate on the specification which encapsulates validation.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsEvaluateOnSpecification()
        {
            MyHouseholdDataModificationCommandHandler<MyHouseholdDataModificationCommand> sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            IHouseholdMember householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            Assert.That(householdMemberMock, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Empty);

            MyHouseholdDataModificationCommand command = _fixture.Build<MyHouseholdDataModificationCommand>()
                // ReSharper disable PossibleInvalidOperationException
                .With(m => m.HouseholdIdentifier, householdMemberMock.Households.First().Identifier.Value)
                // ReSharper restore PossibleInvalidOperationException
                .Create();

            sut.ModifyData(householdMemberMock, command);

            _specificationMock.AssertWasCalled(m => m.Evaluate(), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that ModifyData calls ModifyData with the household on which to modify data.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsModifyDataWithHousehold()
        {
            MyHouseholdDataModificationCommandHandler<MyHouseholdDataModificationCommand> sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.ModifyDataIsCalled, Is.False);
            Assert.That(sut.HandledHousehold, Is.Null);
            Assert.That(sut.HandledCommand, Is.Null);

            IHouseholdMember householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            Assert.That(householdMemberMock, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Empty);

            IHousehold householdMock = householdMemberMock.Households.FirstOrDefault();
            Assert.That(householdMock, Is.Not.Null);

            MyHouseholdDataModificationCommand command = _fixture.Build<MyHouseholdDataModificationCommand>()
                // ReSharper disable PossibleInvalidOperationException
                .With(m => m.HouseholdIdentifier, householdMock.Identifier.Value)
                // ReSharper restore PossibleInvalidOperationException
                .Create();

            sut.ModifyData(householdMemberMock, command);
            Assert.That(sut.ModifyDataIsCalled, Is.True);
            Assert.That(sut.HandledHousehold, Is.Not.Null);
            Assert.That(sut.HandledHousehold, Is.EqualTo(householdMock));
            Assert.That(sut.HandledCommand, Is.Not.Null);
            Assert.That(sut.HandledCommand, Is.EqualTo(command));
        }

        /// <summary>
        /// Tests that ModifyData returns the result from ModifyData called with the household on which to modify data.
        /// </summary>
        [Test]
        public void TestThatModifyDataReturnsResultFromModifyDataCalledWithHousehold()
        {
            MyHouseholdDataModificationCommandHandler<MyHouseholdDataModificationCommand> sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            IHouseholdMember householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            Assert.That(householdMemberMock, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Empty);

            MyHouseholdDataModificationCommand command = _fixture.Build<MyHouseholdDataModificationCommand>()
                // ReSharper disable PossibleInvalidOperationException
                .With(m => m.HouseholdIdentifier, householdMemberMock.Households.First().Identifier.Value)
                // ReSharper restore PossibleInvalidOperationException
                .Create();

            IIdentifiable result = sut.ModifyData(householdMemberMock, command);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(sut.ModifyDataResult));
        }

        /// <summary>
        /// Creates an instance of the <see cref="MyHouseholdDataModificationCommand"/> which can be used for unit testing.
        /// </summary>
        /// <returns>Instance of the <see cref="MyHouseholdDataModificationCommand"/> which can be used for unit testing.</returns>
        private MyHouseholdDataModificationCommandHandler<MyHouseholdDataModificationCommand> CreateSut()
        {
            _specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .WhenCalled(e =>
                {
                    Func<bool> func = (Func<bool>) e.Arguments.ElementAt(0);
                    func();
                })
                .Return(_specificationMock)
                .Repeat.Any();

            return new MyHouseholdDataModificationCommandHandler<MyHouseholdDataModificationCommand>(_householdDataRepositoryMock, _claimValueProviderMock, _objectMapperMock, _specificationMock, _commonValidationsMock, _exceptionBuilderMock);
        }
    }
}
