using System;
using System.Linq;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommandHandlers;
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

namespace OSDevGrp.OSIntranet.Tests.Unittests.CommandHandlers
{
    /// <summary>
    /// Test the command handler which handles a command for adding a household member to a given household on the current users household account.
    /// </summary>
    [TestFixture]
    public class HouseholdAddHouseholdMemberCommandHandlerTests
    {
        #region Private variables

        private IHouseholdDataRepository _householdDataRepositoryMock;
        private IClaimValueProvider _claimValueProviderMock;
        private IFoodWasteObjectMapper _objectMapperMock;
        private ISpecification _specificationMock;
        private ICommonValidations _commonValidationsMock;
        private IDomainObjectValidations _domainObjectValidationsMock;
        private ILogicExecutor _logicExecutorMock;
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
            _logicExecutorMock = MockRepository.GenerateMock<ILogicExecutor>();
            _exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        /// <summary>
        /// Tests that the constructor initialize a command handler which handles a command for adding a household member to a given household on the current users household account.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeHouseholdAddHouseholdMemberCommandHandler()
        {
            HouseholdAddHouseholdMemberCommandHandler sut = CreateSut();
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
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => new HouseholdAddHouseholdMemberCommandHandler(_householdDataRepositoryMock, _claimValueProviderMock, _objectMapperMock, _specificationMock, _commonValidationsMock, null, _logicExecutorMock, _exceptionBuilderMock));
            // ReSharper restore ObjectCreationAsStatement

            TestHelper.AssertArgumentNullExceptionIsValid(result, "domainObjectValidations");
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the logic executor which can execute basic logic is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenLogicExecutorIsNull()
        {
            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => new HouseholdAddHouseholdMemberCommandHandler(_householdDataRepositoryMock, _claimValueProviderMock, _objectMapperMock, _specificationMock, _commonValidationsMock, _domainObjectValidationsMock, null, _exceptionBuilderMock));
            // ReSharper restore ObjectCreationAsStatement

            TestHelper.AssertArgumentNullExceptionIsValid(result, "logicExecutor");
        }

        /// <summary>
        /// Tests that AddValidationRules throws an ArgumentNullException when the household on which to modify data is null.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesThrowsArgumentNullExceptionWhenHouseholdIsNull()
        {
            HouseholdAddHouseholdMemberCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AddValidationRules(null, _fixture.Create<HouseholdAddHouseholdMemberCommand>(), _specificationMock));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "household");
        }

        /// <summary>
        /// Tests that AddValidationRules throws an ArgumentNullException when the command for adding a household member to a given household on the current users household account is null.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesThrowsArgumentNullExceptionWhenHouseholdAddHouseholdMemberCommandIsNull()
        {
            HouseholdAddHouseholdMemberCommandHandler sut = CreateSut();
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
            HouseholdAddHouseholdMemberCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), _fixture.Create<HouseholdAddHouseholdMemberCommand>(), null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "specification");
        }

        /// <summary>
        /// Tests that AddValidationRules calls MailAddress on the provider which can resolve values from the current users claims.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsMailAddressOnClaimValueProvider()
        {
            HouseholdAddHouseholdMemberCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            HouseholdAddHouseholdMemberCommand householdAddHouseholdMemberCommand = _fixture.Build<HouseholdAddHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, _fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            sut.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), householdAddHouseholdMemberCommand, _specificationMock);

            _claimValueProviderMock.AssertWasCalled(m => m.MailAddress, opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that AddValidationRules calls HasValue with the mail address for the household member to add on the common validations.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsHasValueWithMailAddressOnCommonValidations()
        {
            HouseholdAddHouseholdMemberCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            string mailAddress = _fixture.Create<string>();
            HouseholdAddHouseholdMemberCommand householdAddHouseholdMemberCommand = _fixture.Build<HouseholdAddHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, mailAddress)
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            sut.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), householdAddHouseholdMemberCommand, _specificationMock);

            _commonValidationsMock.AssertWasCalled(m => m.HasValue(Arg<string>.Is.Equal(mailAddress)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that AddValidationRules calls IsLengthValid with the mail address for the household member to add on the common validations.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsIsLengthValidWithMailAddressOnCommonValidations()
        {
            HouseholdAddHouseholdMemberCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            string mailAddress = _fixture.Create<string>();
            HouseholdAddHouseholdMemberCommand householdAddHouseholdMemberCommand = _fixture.Build<HouseholdAddHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, mailAddress)
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            sut.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), householdAddHouseholdMemberCommand, _specificationMock);

            _commonValidationsMock.AssertWasCalled(m => m.IsLengthValid(Arg<string>.Is.Equal(mailAddress), Arg<int>.Is.Equal(1), Arg<int>.Is.Equal(128)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that AddValidationRules calls ContainsIllegalChar with the mail address for the household member to add on the common validations.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsContainsIllegalCharWithMailAddressOnCommonValidations()
        {
            HouseholdAddHouseholdMemberCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            string mailAddress = _fixture.Create<string>();
            HouseholdAddHouseholdMemberCommand householdAddHouseholdMemberCommand = _fixture.Build<HouseholdAddHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, mailAddress)
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            sut.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), householdAddHouseholdMemberCommand, _specificationMock);

            _commonValidationsMock.AssertWasCalled(m => m.ContainsIllegalChar(Arg<string>.Is.Equal(mailAddress)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that AddValidationRules calls IsMailAddress with the mail address for the household member to add on the common validations used by domain objects in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsIsMailAddressWithMailAddressOnDomainObjectValidations()
        {
            HouseholdAddHouseholdMemberCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            string mailAddress = _fixture.Create<string>();
            HouseholdAddHouseholdMemberCommand householdAddHouseholdMemberCommand = _fixture.Build<HouseholdAddHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, mailAddress)
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            sut.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), householdAddHouseholdMemberCommand, _specificationMock);

            _domainObjectValidationsMock.AssertWasCalled(m => m.IsMailAddress(Arg<string>.Is.Equal(mailAddress)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that AddValidationRules calls Equals with the mail address for the household member to add and the current users mail address on the common validations.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsEqualsWithMailAddressAndCurrentUserMailAddressOnCommonValidations()
        {
            string currentUserMailAddress = _fixture.Create<string>();

            HouseholdAddHouseholdMemberCommandHandler sut = CreateSut(currentUserMailAddress: currentUserMailAddress);
            Assert.That(sut, Is.Not.Null);

            string mailAddress = _fixture.Create<string>();
            HouseholdAddHouseholdMemberCommand householdAddHouseholdMemberCommand = _fixture.Build<HouseholdAddHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, mailAddress)
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            sut.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), householdAddHouseholdMemberCommand, _specificationMock);

            _commonValidationsMock.AssertWasCalled(m => m.Equals(Arg<string>.Is.Equal(mailAddress), Arg<string>.Is.Equal(currentUserMailAddress), Arg<StringComparison>.Is.Equal(StringComparison.OrdinalIgnoreCase)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that AddValidationRules calls IsSatisfiedBy on the specification which encapsulates validation rules 5 times.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsIsSatisfiedByOnSpecification5Times()
        {
            HouseholdAddHouseholdMemberCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            HouseholdAddHouseholdMemberCommand householdAddHouseholdMemberCommand = _fixture.Build<HouseholdAddHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, _fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            sut.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), householdAddHouseholdMemberCommand, _specificationMock);

            _specificationMock.AssertWasCalled(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<IntranetBusinessException>.Is.TypeOf), opt => opt.Repeat.Times(5));
        }

        /// <summary>
        /// Tests that AddValidationRules does not call Evaluate on the specification which encapsulates validation rules.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesDoesNotCallsEvaluateOnSpecification()
        {
            HouseholdAddHouseholdMemberCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            HouseholdAddHouseholdMemberCommand householdAddHouseholdMemberCommand = _fixture.Build<HouseholdAddHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, _fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            sut.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMock(), householdAddHouseholdMemberCommand, _specificationMock);

            _specificationMock.AssertWasNotCalled(m => m.Evaluate());
        }

        /// <summary>
        /// Tests that ModifyData throws an ArgumentNullException when the household on which to modify data is null.
        /// </summary>
        [Test]
        public void TestThatModifyDataThrowsArgumentNullExceptionWhenHouseholdIsNull()
        {
            HouseholdAddHouseholdMemberCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ModifyData(null, _fixture.Create<HouseholdAddHouseholdMemberCommand>()));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "household");
        }

        /// <summary>
        /// Tests that ModifyData throws an ArgumentNullException when the command for adding a household member to a given household on the current users household account is null.
        /// </summary>
        [Test]
        public void TestThatModifyDataThrowsArgumentNullExceptionWhenHouseholdAddHouseholdMemberCommandIsNull()
        {
            HouseholdAddHouseholdMemberCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ModifyData(DomainObjectMockBuilder.BuildHouseholdMock(), null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "command");
        }

        /// <summary>
        /// Tests that ModifyData calls Get with the identifier of the translation information which should be used in the translation on the repository which can access household data for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsGetWithTranslationInfoIdentifierOnHouseholdDataRepository()
        {
            HouseholdAddHouseholdMemberCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            Guid translationInfoIdentifier = Guid.NewGuid();
            HouseholdAddHouseholdMemberCommand householdAddHouseholdMemberCommand = _fixture.Build<HouseholdAddHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, _fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, translationInfoIdentifier)
                .Create();

            sut.ModifyData(DomainObjectMockBuilder.BuildHouseholdMock(), householdAddHouseholdMemberCommand);

            _householdDataRepositoryMock.AssertWasCalled(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Equal(translationInfoIdentifier)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that ModifyData calls HouseholdMembers on the household on which to modify data.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsHouseholdMembersOnHousehold()
        {
            HouseholdAddHouseholdMemberCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            IHousehold householdMock = DomainObjectMockBuilder.BuildHouseholdMock();

            HouseholdAddHouseholdMemberCommand householdAddHouseholdMemberCommand = _fixture.Build<HouseholdAddHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, _fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            sut.ModifyData(householdMock, householdAddHouseholdMemberCommand);

            householdMock.AssertWasCalled(m => m.HouseholdMembers, opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that ModifyData calls IsNotNull with the the translation information which should be used in the translation on the common validations.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsIsNotNullWithTranslationInfoOnCommonValidations()
        {
            ITranslationInfo translationInfoMock = DomainObjectMockBuilder.BuildTranslationInfoMock();

            HouseholdAddHouseholdMemberCommandHandler sut = CreateSut(translationInfoMock);
            Assert.That(sut, Is.Not.Null);

            HouseholdAddHouseholdMemberCommand householdAddHouseholdMemberCommand = _fixture.Build<HouseholdAddHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, _fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            sut.ModifyData(DomainObjectMockBuilder.BuildHouseholdMock(), householdAddHouseholdMemberCommand);

            _commonValidationsMock.AssertWasCalled(m => m.IsNotNull(Arg<ITranslationInfo>.Is.Equal(translationInfoMock)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that ModifyData calls IsNull with null on the common validations when the household to modify does not have a household member with the mail address.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsIsNullWithNullOnCommonValidationsWhenHouseholdDoesNotHaveHouseholdMemberWithMailAddress()
        {
            ITranslationInfo translationInfoMock = DomainObjectMockBuilder.BuildTranslationInfoMock();

            HouseholdAddHouseholdMemberCommandHandler sut = CreateSut(translationInfoMock);
            Assert.That(sut, Is.Not.Null);

            IHousehold householdMock = DomainObjectMockBuilder.BuildHouseholdMock();
            Assert.That(householdMock, Is.Not.Null);
            Assert.That(householdMock.HouseholdMembers, Is.Not.Null);
            Assert.That(householdMock.HouseholdMembers, Is.Not.Empty);

            string mailAddress = _fixture.Create<string>();
            Assert.That(householdMock.HouseholdMembers.Any(householdMember => string.Compare(householdMember.MailAddress, mailAddress, StringComparison.OrdinalIgnoreCase) == 0), Is.False);

            HouseholdAddHouseholdMemberCommand householdAddHouseholdMemberCommand = _fixture.Build<HouseholdAddHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, mailAddress)
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            sut.ModifyData(householdMock, householdAddHouseholdMemberCommand);

            _commonValidationsMock.AssertWasCalled(m => m.IsNull(Arg<IHouseholdMember>.Is.Null), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that ModifyData calls IsNull with the existing household member on the common validations when the household to modify does have a household member with the mail address.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsIsNullWithHouseholdMemberOnCommonValidationsWhenHouseholdDoesHaveHouseholdMemberWithMailAddress()
        {
            ITranslationInfo translationInfoMock = DomainObjectMockBuilder.BuildTranslationInfoMock();

            HouseholdAddHouseholdMemberCommandHandler sut = CreateSut(translationInfoMock);
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

            HouseholdAddHouseholdMemberCommand householdAddHouseholdMemberCommand = _fixture.Build<HouseholdAddHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, mailAddress)
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            sut.ModifyData(householdMock, householdAddHouseholdMemberCommand);

            _commonValidationsMock.AssertWasCalled(m => m.IsNull(Arg<IHouseholdMember>.Is.Equal(householdMemberMock)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that ModifyData calls IsSatisfiedBy on the specification which encapsulates validation rules 2 times.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsIsSatisfiedByOnSpecification2Times()
        {
            HouseholdAddHouseholdMemberCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            HouseholdAddHouseholdMemberCommand householdAddHouseholdMemberCommand = _fixture.Build<HouseholdAddHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, _fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            sut.ModifyData(DomainObjectMockBuilder.BuildHouseholdMock(), householdAddHouseholdMemberCommand);

            _specificationMock.AssertWasCalled(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<IntranetBusinessException>.Is.TypeOf), opt => opt.Repeat.Times(2));
        }

        /// <summary>
        /// Tests that ModifyData calls Evaluate on the specification which encapsulates validation rules.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsEvaluateOnSpecification()
        {
            HouseholdAddHouseholdMemberCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            HouseholdAddHouseholdMemberCommand householdAddHouseholdMemberCommand = _fixture.Build<HouseholdAddHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, _fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            sut.ModifyData(DomainObjectMockBuilder.BuildHouseholdMock(), householdAddHouseholdMemberCommand);

            _specificationMock.AssertWasCalled(m => m.Evaluate(), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that ModifyData calls HouseholdMemberGetByMailAddress on the repository which can access household data for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsHouseholdMemberGetByMailAddressOnHouseholdDataRepository()
        {
            HouseholdAddHouseholdMemberCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            string mailAddress = _fixture.Create<string>();
            HouseholdAddHouseholdMemberCommand householdAddHouseholdMemberCommand = _fixture.Build<HouseholdAddHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, mailAddress)
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            sut.ModifyData(DomainObjectMockBuilder.BuildHouseholdMock(), householdAddHouseholdMemberCommand);

            _householdDataRepositoryMock.AssertWasCalled(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Equal(mailAddress)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that ModifyData calls HouseholdMemberAdd on the logic executor which can execute basic logic when a household member with the mail address does not exists.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsHouseholdMemberAddOnLogicExecutorWhenHouseholdMemberForMailAddressDoesNotExist()
        {
            ITranslationInfo translationInfoMock = DomainObjectMockBuilder.BuildTranslationInfoMock();

            HouseholdAddHouseholdMemberCommandHandler sut = CreateSut(translationInfoMock, false);
            Assert.That(sut, Is.Not.Null);

            string mailAddress = _fixture.Create<string>();
            HouseholdAddHouseholdMemberCommand householdAddHouseholdMemberCommand = _fixture.Build<HouseholdAddHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, mailAddress)
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            sut.ModifyData(DomainObjectMockBuilder.BuildHouseholdMock(), householdAddHouseholdMemberCommand);

            // ReSharper disable PossibleInvalidOperationException
            _logicExecutorMock.AssertWasCalled(m => m.HouseholdMemberAdd(Arg<string>.Is.Equal(mailAddress), Arg<Guid>.Is.Equal(translationInfoMock.Identifier.Value)), opt => opt.Repeat.Once());
            // ReSharper restore PossibleInvalidOperationException
        }

        /// <summary>
        /// Tests that ModifyData calls Get with the identifier for the created household member on the repository which can access household data for the food waste domain when a household member with the mail address does not exists.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsGetWithIdentifierForCreatedHouseholdMemberOnHouseholdDataRepositoryWhenHouseholdMemberForMailAddressDoesNotExist()
        {
            Guid createdHouseholdMemberIdentifier = Guid.NewGuid();

            HouseholdAddHouseholdMemberCommandHandler sut = CreateSut(knownMailAddress: false, createdHouseholdMemberIdentifier: createdHouseholdMemberIdentifier);
            Assert.That(sut, Is.Not.Null);

            HouseholdAddHouseholdMemberCommand householdAddHouseholdMemberCommand = _fixture.Build<HouseholdAddHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, _fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            sut.ModifyData(DomainObjectMockBuilder.BuildHouseholdMock(), householdAddHouseholdMemberCommand);

            _householdDataRepositoryMock.AssertWasCalled(m => m.Get<IHouseholdMember>(Arg<Guid>.Is.Equal(createdHouseholdMemberIdentifier)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that ModifyData calls HouseholdMemberAdd with the created household member on the household when a household member with the mail address does not exists.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsHouseholdMemberAddWithCreatedHouseholdMemberOnHouseholdWhenHouseholdMemberForMailAddressDoesNotExist()
        {
            IHouseholdMember createdHouseholdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();

            HouseholdAddHouseholdMemberCommandHandler sut = CreateSut(knownMailAddress: false, createdHouseholdMember: createdHouseholdMemberMock);
            Assert.That(sut, Is.Not.Null);

            IHousehold householdMock = DomainObjectMockBuilder.BuildHouseholdMock();

            HouseholdAddHouseholdMemberCommand householdAddHouseholdMemberCommand = _fixture.Build<HouseholdAddHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, _fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            sut.ModifyData(householdMock, householdAddHouseholdMemberCommand);

            householdMock.AssertWasCalled(m => m.HouseholdMemberAdd(Arg<IHouseholdMember>.Is.Equal(createdHouseholdMemberMock)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that ModifyData does not call HouseholdMemberAdd on the logic executor which can execute basic logic when a household member with the mail address exists.
        /// </summary>
        [Test]
        public void TestThatModifyDataDoesNotCallHouseholdMemberAddOnLogicExecutorWhenHouseholdMemberForMailAddressExists()
        {
            HouseholdAddHouseholdMemberCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            HouseholdAddHouseholdMemberCommand householdAddHouseholdMemberCommand = _fixture.Build<HouseholdAddHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, _fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            sut.ModifyData(DomainObjectMockBuilder.BuildHouseholdMock(), householdAddHouseholdMemberCommand);
            
            _logicExecutorMock.AssertWasNotCalled(m => m.HouseholdMemberAdd(Arg<string>.Is.Anything, Arg<Guid>.Is.Anything));
        }

        /// <summary>
        /// Tests that ModifyData does not call Get for any household members on the repository which can access household data for the food waste domain when a household member with the mail address exists.
        /// </summary>
        [Test]
        public void TestThatModifyDataDoesNotCallGetForAnyHouseholdMemberOnHouseholdDataRepositoryWhenHouseholdMemberForMailAddressExists()
        {
            HouseholdAddHouseholdMemberCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            HouseholdAddHouseholdMemberCommand householdAddHouseholdMemberCommand = _fixture.Build<HouseholdAddHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, _fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            sut.ModifyData(DomainObjectMockBuilder.BuildHouseholdMock(), householdAddHouseholdMemberCommand);

            _householdDataRepositoryMock.AssertWasNotCalled(m => m.Get<IHouseholdMember>(Arg<Guid>.Is.Anything));
        }

        /// <summary>
        /// Tests that ModifyData calls HouseholdMemberAdd with the household member for the mail address on the household when a household member with the mail address exists.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsHouseholdMemberAddWithHouseholdMemberForMailAddressOnHouseholdWhenHouseholdMemberForMailAddressExists()
        {
            IHouseholdMember householdMemberForMailAddressMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();

            HouseholdAddHouseholdMemberCommandHandler sut = CreateSut(householdMemberForMailAddress: householdMemberForMailAddressMock);
            Assert.That(sut, Is.Not.Null);

            IHousehold householdMock = DomainObjectMockBuilder.BuildHouseholdMock();

            HouseholdAddHouseholdMemberCommand householdAddHouseholdMemberCommand = _fixture.Build<HouseholdAddHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, _fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            sut.ModifyData(householdMock, householdAddHouseholdMemberCommand);

            householdMock.AssertWasCalled(m => m.HouseholdMemberAdd(Arg<IHouseholdMember>.Is.Equal(householdMemberForMailAddressMock)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that ModifyData calls Update with household on the repository which can access household data for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsUpdateWithHouseholdOnHouseholdDataRepository()
        {
            HouseholdAddHouseholdMemberCommandHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            IHousehold householdMock = DomainObjectMockBuilder.BuildHouseholdMock();

            HouseholdAddHouseholdMemberCommand householdAddHouseholdMemberCommand = _fixture.Build<HouseholdAddHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, _fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            sut.ModifyData(householdMock, householdAddHouseholdMemberCommand);

            _householdDataRepositoryMock.AssertWasCalled(m => m.Update(Arg<IHousehold>.Is.Equal(householdMock)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that ModifyData returns the result from Update called with household on the repository which can access household data for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatModifyDataReturnsResultFromUpdateCalledWithHouseholdOnHouseholdDataRepository()
        {
            IHousehold updatedHouseholdMock = DomainObjectMockBuilder.BuildHouseholdMock();

            HouseholdAddHouseholdMemberCommandHandler sut = CreateSut(updatedHousehold: updatedHouseholdMock);
            Assert.That(sut, Is.Not.Null);

            HouseholdAddHouseholdMemberCommand householdAddHouseholdMemberCommand = _fixture.Build<HouseholdAddHouseholdMemberCommand>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, _fixture.Create<string>())
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            IIdentifiable result = sut.ModifyData(DomainObjectMockBuilder.BuildHouseholdMock(), householdAddHouseholdMemberCommand);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(updatedHouseholdMock));
        }

        /// <summary>
        /// Creates an instance of the <see cref="HouseholdAddHouseholdMemberCommandHandler"/> which can be used for unit testing.
        /// </summary>
        /// <returns>Instance of the <see cref="HouseholdAddHouseholdMemberCommandHandler"/> which can be used for unit testing.</returns>
        private HouseholdAddHouseholdMemberCommandHandler CreateSut(ITranslationInfo translationInfo = null, bool knownMailAddress = true, IHouseholdMember householdMemberForMailAddress = null, IHousehold updatedHousehold = null, string currentUserMailAddress = null, Guid? createdHouseholdMemberIdentifier = null, IHouseholdMember createdHouseholdMember = null)
        {
            _householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(translationInfo ?? DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            _householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(knownMailAddress ? householdMemberForMailAddress ?? DomainObjectMockBuilder.BuildHouseholdMemberMock() : null)
                .Repeat.Any();
            _householdDataRepositoryMock.Stub(m => m.Get<IHouseholdMember>(Arg<Guid>.Is.Anything))
                .Return(createdHouseholdMember ?? DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();
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

            _logicExecutorMock.Stub(m => m.HouseholdMemberAdd(Arg<string>.Is.Anything, Arg<Guid>.Is.Anything))
                .Return(createdHouseholdMemberIdentifier ?? Guid.NewGuid())
                .Repeat.Any();

            return new HouseholdAddHouseholdMemberCommandHandler(_householdDataRepositoryMock, _claimValueProviderMock, _objectMapperMock, _specificationMock, _commonValidationsMock, _domainObjectValidationsMock, _logicExecutorMock, _exceptionBuilderMock);
        }
    }
}
