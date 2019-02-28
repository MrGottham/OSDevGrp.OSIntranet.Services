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
    /// Test the command handler which handles a command for removing a household member from a given household on the current users household account.
    /// </summary>
    [TestFixture]
    public class HouseholdRemoveHouseholdMemberCommandHandlerTests
    {
        #region Private variables

        private IHouseholdDataRepository _householdDataRepositoryMock;
        private IClaimValueProvider _claimValueProviderMock;
        private IFoodWasteObjectMapper _objectMapperMock;
        private ISpecification _specificationMock;
        private ICommonValidations _commonValidationsMock;
        private IDomainObjectValidations _domainObjectValidationsMock;
        private IExceptionBuilder _exceptionBuilderMock;
        private Fixture _fixture;
        private Random _random;

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
            _domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            _exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        /// <summary>
        /// Tests that the constructor initialize a command handler which handles a command for removing a household member from a given household on the current users household account.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeHouseholdRemoveHouseholdMemberCommandHandler()
        {
            HouseholdRemoveHouseholdMemberCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.ShouldBeActivated, Is.True);
            Assert.That(sut.ShouldHaveAcceptedPrivacyPolicy, Is.True);
            Assert.That(sut.RequiredMembership, Is.EqualTo(Membership.Basic));
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the common validations used by domain objects in the food waste domain is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenDomainObjectValidationsIsNull()
        {
            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => new HouseholdRemoveHouseholdMemberCommandHandler(_householdDataRepositoryMock, _claimValueProviderMock, _objectMapperMock, _specificationMock, _commonValidationsMock, null, _exceptionBuilderMock));
            // ReSharper restore ObjectCreationAsStatement

            TestHelper.AssertArgumentNullExceptionIsValid(result, "domainObjectValidations");
        }

        /// <summary>
        /// Tests that AddValidationRules throws an ArgumentNullException when the household on which to modify data is null.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesThrowsArgumentNullExceptionWhenHouseholdIsNull()
        {
            HouseholdRemoveHouseholdMemberCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AddValidationRules(null, _fixture.Create<HouseholdRemoveHouseholdMemberCommand>(), _specificationMock));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "household");
        }

        /// <summary>
        /// Tests that AddValidationRules throws an ArgumentNullException when the command for removing a household member from a given household on the current users household account is null.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesThrowsArgumentNullExceptionWhenHouseholdRemoveHouseholdMemberCommandIsNull()
        {
            HouseholdRemoveHouseholdMemberCommandHandler sut = CreateSut();
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
            HouseholdRemoveHouseholdMemberCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), _fixture.Create<HouseholdRemoveHouseholdMemberCommand>(), null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "specification");
        }

        /// <summary>
        /// Tests that AddValidationRules calls MailAddress on the provider which can resolve values from the current users claims.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsMailAddressOnClaimValueProvider()
        {
            HouseholdRemoveHouseholdMemberCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            HouseholdRemoveHouseholdMemberCommand householdRemoveHouseholdMemberCommand = _fixture.Build<HouseholdRemoveHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, _fixture.Create<string>())
                .Create();

            sut.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), householdRemoveHouseholdMemberCommand, _specificationMock);

            _claimValueProviderMock.AssertWasCalled(m => m.MailAddress, opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that AddValidationRules calls HasValue with the mail address for the household member to remove on the common validations.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsHasValueWithMailAddressOnCommonValidations()
        {
            HouseholdRemoveHouseholdMemberCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            string mailAddress = _fixture.Create<string>();
            HouseholdRemoveHouseholdMemberCommand householdRemoveHouseholdMemberCommand = _fixture.Build<HouseholdRemoveHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, mailAddress)
                .Create();

            sut.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), householdRemoveHouseholdMemberCommand, _specificationMock);

            _commonValidationsMock.AssertWasCalled(m => m.HasValue(Arg<string>.Is.Equal(mailAddress)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that AddValidationRules calls IsLengthValid with the mail address for the household member to remove on the common validations.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsIsLengthValidWithMailAddressOnCommonValidations()
        {
            HouseholdRemoveHouseholdMemberCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            string mailAddress = _fixture.Create<string>();
            HouseholdRemoveHouseholdMemberCommand householdRemoveHouseholdMemberCommand = _fixture.Build<HouseholdRemoveHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, mailAddress)
                .Create();

            sut.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), householdRemoveHouseholdMemberCommand, _specificationMock);

            _commonValidationsMock.AssertWasCalled(m => m.IsLengthValid(Arg<string>.Is.Equal(mailAddress), Arg<int>.Is.Equal(1), Arg<int>.Is.Equal(128)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that AddValidationRules calls ContainsIllegalChar with the mail address for the household member to remove on the common validations.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsContainsIllegalCharWithMailAddressOnCommonValidations()
        {
            HouseholdRemoveHouseholdMemberCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            string mailAddress = _fixture.Create<string>();
            HouseholdRemoveHouseholdMemberCommand householdRemoveHouseholdMemberCommand = _fixture.Build<HouseholdRemoveHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, mailAddress)
                .Create();

            sut.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), householdRemoveHouseholdMemberCommand, _specificationMock);

            _commonValidationsMock.AssertWasCalled(m => m.ContainsIllegalChar(Arg<string>.Is.Equal(mailAddress)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that AddValidationRules calls IsMailAddress with the mail address for the household member to remove on the the common validations used by domain objects in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsIsMailAddressWithMailAddressOnDomainObjectValidations()
        {
            HouseholdRemoveHouseholdMemberCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            string mailAddress = _fixture.Create<string>();
            HouseholdRemoveHouseholdMemberCommand householdRemoveHouseholdMemberCommand = _fixture.Build<HouseholdRemoveHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, mailAddress)
                .Create();

            sut.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), householdRemoveHouseholdMemberCommand, _specificationMock);

            _domainObjectValidationsMock.AssertWasCalled(m => m.IsMailAddress(Arg<string>.Is.Equal(mailAddress)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that AddValidationRules calls Equals with the mail address for the household member to remove and the current users mail address on the common validations.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsEqualsWithMailAddressAndCurrentUserMailAddressOnCommonValidations()
        {
            string currentUserMailAddress = _fixture.Create<string>();

            HouseholdRemoveHouseholdMemberCommandHandler sut = CreateSut(currentUserMailAddress: currentUserMailAddress);
            Assert.That(sut, Is.Not.Null);

            string mailAddress = _fixture.Create<string>();
            HouseholdRemoveHouseholdMemberCommand householdRemoveHouseholdMemberCommand = _fixture.Build<HouseholdRemoveHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, mailAddress)
                .Create();

            sut.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), householdRemoveHouseholdMemberCommand, _specificationMock);

            _commonValidationsMock.AssertWasCalled(m => m.Equals(Arg<string>.Is.Equal(mailAddress), Arg<string>.Is.Equal(currentUserMailAddress), Arg<StringComparison>.Is.Equal(StringComparison.OrdinalIgnoreCase)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that AddValidationRules calls IsSatisfiedBy on the specification which encapsulates validation rules 5 times.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsIsSatisfiedByOnSpecification5Times()
        {
            HouseholdRemoveHouseholdMemberCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            HouseholdRemoveHouseholdMemberCommand householdRemoveHouseholdMemberCommand = _fixture.Build<HouseholdRemoveHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, _fixture.Create<string>())
                .Create();

            sut.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), householdRemoveHouseholdMemberCommand, _specificationMock);

            _specificationMock.AssertWasCalled(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<IntranetBusinessException>.Is.TypeOf), opt => opt.Repeat.Times(5));
        }

        /// <summary>
        /// Tests that AddValidationRules does not call Evaluate on the specification which encapsulates validation rules.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesDoesNotCallsEvaluateOnSpecification()
        {
            HouseholdRemoveHouseholdMemberCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            HouseholdRemoveHouseholdMemberCommand householdRemoveHouseholdMemberCommand = _fixture.Build<HouseholdRemoveHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, _fixture.Create<string>())
                .Create();

            sut.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), householdRemoveHouseholdMemberCommand, _specificationMock);

            _specificationMock.AssertWasNotCalled(m => m.Evaluate());
        }

        /// <summary>
        /// Tests that ModifyData throws an ArgumentNullException when the household on which to modify data is null.
        /// </summary>
        [Test]
        public void TestThatModifyDataThrowsArgumentNullExceptionWhenHouseholdIsNull()
        {
            HouseholdRemoveHouseholdMemberCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ModifyData(null, _fixture.Create<HouseholdRemoveHouseholdMemberCommand>()));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "household");
        }

        /// <summary>
        /// Tests that ModifyData throws an ArgumentNullException when the command for removing a household member from a given household on the current users household account is null.
        /// </summary>
        [Test]
        public void TestThatModifyDataThrowsArgumentNullExceptionWhenHouseholdRemoveHouseholdMemberCommandIsNull()
        {
            HouseholdRemoveHouseholdMemberCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ModifyData(DomainObjectMockBuilder.BuildHouseholdMock(), null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "command");
        }

        /// <summary>
        /// Tests that ModifyData calls HouseholdMembers on the household on which to modify data.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsHouseholdMembersOnHousehold()
        {
            HouseholdRemoveHouseholdMemberCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            IHousehold householdMock = DomainObjectMockBuilder.BuildHouseholdMock();

            HouseholdRemoveHouseholdMemberCommand householdRemoveHouseholdMemberCommand = _fixture.Build<HouseholdRemoveHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, _fixture.Create<string>())
                .Create();

            sut.ModifyData(householdMock, householdRemoveHouseholdMemberCommand);

            householdMock.AssertWasCalled(m => m.HouseholdMembers, opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that ModifyData calls IsNotNull with the existing household member on the common validations when the household to modify does have a household member with the mail address.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsIsNotNullWithHouseholdMemberOnCommonValidationsWhenHouseholdDoesHaveHouseholdMemberWithMailAddress()
        {
            HouseholdRemoveHouseholdMemberCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            IHousehold householdMock = DomainObjectMockBuilder.BuildHouseholdMock();
            Assert.That(householdMock, Is.Not.Null);
            Assert.That(householdMock.HouseholdMembers, Is.Not.Null);
            Assert.That(householdMock.HouseholdMembers, Is.Not.Empty);

            IHouseholdMember householdMemberMock = householdMock.HouseholdMembers.ElementAt(_random.Next(0, householdMock.HouseholdMembers.Count() - 1));
            Assert.That(householdMemberMock, Is.Not.Null);
            Assert.That(householdMemberMock.MailAddress, Is.Not.Null);
            Assert.That(householdMemberMock.MailAddress, Is.Not.Empty);

            string mailAddress = householdMemberMock.MailAddress;
            Assert.That(householdMock.HouseholdMembers.Any(householdMember => string.Compare(householdMember.MailAddress, mailAddress, StringComparison.OrdinalIgnoreCase) == 0), Is.True);

            HouseholdRemoveHouseholdMemberCommand householdRemoveHouseholdMemberCommand = _fixture.Build<HouseholdRemoveHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, mailAddress)
                .Create();

            sut.ModifyData(householdMock, householdRemoveHouseholdMemberCommand);

            _commonValidationsMock.AssertWasCalled(m => m.IsNotNull(Arg<IHouseholdMember>.Is.Equal(householdMemberMock)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that ModifyData calls IsNotNull with null on the common validations when the household to modify does not have a household member with the mail address.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsIsNotNullWithNullOnCommonValidationsWhenHouseholdDoesNotHaveHouseholdMemberWithMailAddress()
        {
            HouseholdRemoveHouseholdMemberCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            IHousehold householdMock = DomainObjectMockBuilder.BuildHouseholdMock();
            Assert.That(householdMock, Is.Not.Null);
            Assert.That(householdMock.HouseholdMembers, Is.Not.Null);
            Assert.That(householdMock.HouseholdMembers, Is.Not.Empty);

            string mailAddress = _fixture.Create<string>();
            Assert.That(householdMock.HouseholdMembers.Any(householdMember => string.Compare(householdMember.MailAddress, mailAddress, StringComparison.OrdinalIgnoreCase) == 0), Is.False);

            HouseholdRemoveHouseholdMemberCommand householdRemoveHouseholdMemberCommand = _fixture.Build<HouseholdRemoveHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, mailAddress)
                .Create();

            sut.ModifyData(householdMock, householdRemoveHouseholdMemberCommand);

            _commonValidationsMock.AssertWasCalled(m => m.IsNotNull(Arg<IHouseholdMember>.Is.Null), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that ModifyData calls IsSatisfiedBy on the specification which encapsulates validation rules 1 time.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsIsSatisfiedByOnSpecification1Time()
        {
            HouseholdRemoveHouseholdMemberCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            HouseholdRemoveHouseholdMemberCommand householdRemoveHouseholdMemberCommand = _fixture.Build<HouseholdRemoveHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, _fixture.Create<string>())
                .Create();

            sut.ModifyData(DomainObjectMockBuilder.BuildHouseholdMock(), householdRemoveHouseholdMemberCommand);

            _specificationMock.AssertWasCalled(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<IntranetBusinessException>.Is.TypeOf), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that ModifyData calls Evaluate on the specification which encapsulates validation rules.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsEvaluateOnSpecification()
        {
            HouseholdRemoveHouseholdMemberCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            HouseholdRemoveHouseholdMemberCommand householdRemoveHouseholdMemberCommand = _fixture.Build<HouseholdRemoveHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, _fixture.Create<string>())
                .Create();

            sut.ModifyData(DomainObjectMockBuilder.BuildHouseholdMock(), householdRemoveHouseholdMemberCommand);

            _specificationMock.AssertWasCalled(m => m.Evaluate(), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that ModifyData calls HouseholdMemberRemove with the household member for the mail address.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsHouseholdMemberRemoveWithHouseholdMemberForMailAddressOnHousehold()
        {
            HouseholdRemoveHouseholdMemberCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            IHousehold householdMock = DomainObjectMockBuilder.BuildHouseholdMock();
            Assert.That(householdMock, Is.Not.Null);
            Assert.That(householdMock.HouseholdMembers, Is.Not.Null);
            Assert.That(householdMock.HouseholdMembers, Is.Not.Empty);

            IHouseholdMember householdMemberMock = householdMock.HouseholdMembers.ElementAt(_random.Next(0, householdMock.HouseholdMembers.Count() - 1));
            Assert.That(householdMemberMock, Is.Not.Null);
            Assert.That(householdMemberMock.MailAddress, Is.Not.Null);
            Assert.That(householdMemberMock.MailAddress, Is.Not.Empty);

            HouseholdRemoveHouseholdMemberCommand householdRemoveHouseholdMemberCommand = _fixture.Build<HouseholdRemoveHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, householdMemberMock.MailAddress)
                .Create();

            sut.ModifyData(householdMock, householdRemoveHouseholdMemberCommand);

            householdMock.AssertWasCalled(m => m.HouseholdMemberRemove(Arg<IHouseholdMember>.Is.Equal(householdMemberMock)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that ModifyData calls Update with household on the repository which can access household data for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsUpdateWithHouseholdOnHouseholdDataRepository()
        {
            HouseholdRemoveHouseholdMemberCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            IHousehold householdMock = DomainObjectMockBuilder.BuildHouseholdMock();

            HouseholdRemoveHouseholdMemberCommand householdRemoveHouseholdMemberCommand = _fixture.Build<HouseholdRemoveHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, _fixture.Create<string>())
                .Create();

            sut.ModifyData(householdMock, householdRemoveHouseholdMemberCommand);

            _householdDataRepositoryMock.AssertWasCalled(m => m.Update(Arg<IHousehold>.Is.Equal(householdMock)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that ModifyData returns the result from Update called with household on the repository which can access household data for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatModifyDataReturnsResultFromUpdateCalledWithHouseholdOnHouseholdDataRepository()
        {
            IHousehold updatedHouseholdMock = DomainObjectMockBuilder.BuildHouseholdMock();

            HouseholdRemoveHouseholdMemberCommandHandler sut = CreateSut(updatedHouseholdMock);
            Assert.That(sut, Is.Not.Null);

            HouseholdRemoveHouseholdMemberCommand householdRemoveHouseholdMemberCommand = _fixture.Build<HouseholdRemoveHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, _fixture.Create<string>())
                .Create();

            IIdentifiable result = sut.ModifyData(DomainObjectMockBuilder.BuildHouseholdMock(), householdRemoveHouseholdMemberCommand);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(updatedHouseholdMock));
        }

        /// <summary>
        /// Creates an instance of the <see cref="HouseholdRemoveHouseholdMemberCommandHandler"/> which can be used for unit testing.
        /// </summary>
        /// <returns>Instance of the <see cref="HouseholdRemoveHouseholdMemberCommandHandler"/> which can be used for unit testing.</returns>
        private HouseholdRemoveHouseholdMemberCommandHandler CreateSut(IHousehold updatedHousehold = null, string currentUserMailAddress = null)
        {
            _householdDataRepositoryMock.Stub(m => m.Update(Arg<IHousehold>.Is.Anything))
                .Return(updatedHousehold ?? DomainObjectMockBuilder.BuildHouseholdMock())
                .Repeat.Any();

            _claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(currentUserMailAddress ?? _fixture.Create<string>())
                .Repeat.Any();

            _specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .WhenCalled(e =>
                {
                    Func<bool> func = (Func<bool>) e.Arguments.ElementAt(0);
                    func();
                })
                .Return(_specificationMock)
                .Repeat.Any();

            return new HouseholdRemoveHouseholdMemberCommandHandler(_householdDataRepositoryMock, _claimValueProviderMock, _objectMapperMock, _specificationMock, _commonValidationsMock, _domainObjectValidationsMock, _exceptionBuilderMock);
        }
    }
}
