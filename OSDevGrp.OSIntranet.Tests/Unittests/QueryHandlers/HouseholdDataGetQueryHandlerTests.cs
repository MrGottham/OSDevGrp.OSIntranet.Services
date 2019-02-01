using System;
using System.Globalization;
using System.Linq;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.QueryHandlers;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Resources;
using OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers
{
    /// <summary>
    /// Test the query handler which handles the query for getting household data for one of the current user households.
    /// </summary>
    [TestFixture]
    public class HouseholdDataGetQueryHandlerTests
    {
        #region Private variables

        private Fixture _fixture;
        private IHouseholdDataRepository _householdDataRepositoryMock;
        private IClaimValueProvider _claimValueProviderMock;
        private IFoodWasteObjectMapper _objectMapperMock;

        #endregion

        /// <summary>
        /// Setup each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            _claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            _objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
        }

        /// <summary>
        /// Tests that the constructor initialize the query handler which handles the query for getting household data for one of the current user households.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeHouseholdDataGetQueryHandler()
        {
            HouseholdDataGetQueryHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.ShouldBeActivated, Is.True);
            Assert.That(sut.ShouldHaveAcceptedPrivacyPolicy, Is.True);
            Assert.That(sut.RequiredMembership, Is.EqualTo(Membership.Basic));
        }

        /// <summary>
        /// Tests that GetData throws ArgumentNullException when the household member is null.
        /// </summary>
        [Test]
        public void TestThatGetDataThrowsArgumentNullExceptionWhenHouseholdMemberIsNull()
        {
            HouseholdDataGetQueryHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.GetData(null, BuildHouseholdDataGetQuery(), DomainObjectMockBuilder.BuildTranslationInfoMock()));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "householdMember");
        }

        /// <summary>
        /// Tests that GetData throws ArgumentNullException when the query for for getting household data for one of the current user households is null.
        /// </summary>
        [Test]
        public void TestThatGetDataThrowsArgumentNullExceptionWhenQueryIsNull()
        {
            HouseholdDataGetQueryHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.GetData(DomainObjectMockBuilder.BuildHouseholdMemberMock(), null, DomainObjectMockBuilder.BuildTranslationInfoMock()));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "query");
        }

        /// <summary>
        /// Tests that GetData throws ArgumentNullException when the translation information which can be used for translation is null.
        /// </summary>
        [Test]
        public void TestThatGetDataThrowsArgumentNullExceptionWhenTranslationInfoIsNull()
        {
            HouseholdDataGetQueryHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.GetData(DomainObjectMockBuilder.BuildHouseholdMemberMock(), BuildHouseholdDataGetQuery(), null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "translationInfo");
        }

        /// <summary>
        /// Tests that GetData calls Households on the household member on which data for a household should be returned.
        /// </summary>
        [Test]
        public void TestThatGetDataCallsHouseholdsOnHouseholdMember()
        {
            HouseholdDataGetQueryHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            IHouseholdMember householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            Assert.That(householdMemberMock, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Empty);

            // ReSharper disable PossibleInvalidOperationException
            Guid householdIdentifier = householdMemberMock.Households.First().Identifier.Value;
            // ReSharper restore PossibleInvalidOperationException
            HouseholdDataGetQuery householdDataGetQuery = BuildHouseholdDataGetQuery(householdIdentifier);

            sut.GetData(householdMemberMock, householdDataGetQuery, DomainObjectMockBuilder.BuildTranslationInfoMock());

            householdMemberMock.AssertWasCalled(m => m.Households, opt => opt.Repeat.Times(3 + 1)); // Tree times in the unit test and one time in the query handler.
        }

        /// <summary>
        /// Tests that GetData throws an IntranetBusinessException when a household with the household identifier does not exists on the household member.
        /// </summary>
        [Test]
        public void TestThatGetDataThrowsIntranetBusinessExceptionWhenHouseholdIdentifierDoesNotExistOnHouseholdMember()
        {
            HouseholdDataGetQueryHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            IHouseholdMember householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            Assert.That(householdMemberMock, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Empty);

            Guid householdIdentifier = Guid.NewGuid();
            Assert.That(householdMemberMock.Households.Any(household => household.Identifier.HasValue && household.Identifier.Value == householdIdentifier), Is.False);

            HouseholdDataGetQuery householdDataGetQuery = BuildHouseholdDataGetQuery(householdIdentifier);

            IntranetBusinessException result = Assert.Throws<IntranetBusinessException>(() => sut.GetData(householdMemberMock, householdDataGetQuery, DomainObjectMockBuilder.BuildTranslationInfoMock()));

            TestHelper.AssertIntranetBusinessExceptionIsValid(result, ExceptionMessage.IdentifierUnknownToSystem, householdIdentifier);
        }

        /// <summary>
        /// Tests that GetData calls Translate on the household with the household identifier.
        /// </summary>
        [Test]
        public void TestThatGetDataCallsTranslateOnHouseholdForHouseholdIdentifier()
        {
            HouseholdDataGetQueryHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            IHouseholdMember householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            Assert.That(householdMemberMock, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Empty);

            IHousehold householdMock = householdMemberMock.Households.FirstOrDefault();
            Assert.That(householdMock, Is.Not.Null);

            // ReSharper disable PossibleInvalidOperationException
            Guid householdIdentifier = householdMock.Identifier.Value;
            // ReSharper restore PossibleInvalidOperationException
            HouseholdDataGetQuery householdDataGetQuery = BuildHouseholdDataGetQuery(householdIdentifier);

            ITranslationInfo translationInfoMock = DomainObjectMockBuilder.BuildTranslationInfoMock();

            sut.GetData(householdMemberMock, householdDataGetQuery, translationInfoMock);

            householdMock.AssertWasCalled(m => m.Translate(Arg<CultureInfo>.Is.Equal(translationInfoMock.CultureInfo), Arg<bool>.Is.Equal(false), Arg<bool>.Is.Equal(true)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that GetData returns the household with the household identifier.
        /// </summary>
        [Test]
        public void TestThatGetDataReturnsHouseholdForHouseholdIdentifier()
        {
            HouseholdDataGetQueryHandler sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            IHouseholdMember householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            Assert.That(householdMemberMock, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Empty);

            IHousehold householdMock = householdMemberMock.Households.FirstOrDefault();
            Assert.That(householdMock, Is.Not.Null);

            // ReSharper disable PossibleInvalidOperationException
            Guid householdIdentifier = householdMock.Identifier.Value;
            // ReSharper restore PossibleInvalidOperationException
            HouseholdDataGetQuery householdDataGetQuery = BuildHouseholdDataGetQuery(householdIdentifier);

            IHousehold result = sut.GetData(householdMemberMock, householdDataGetQuery, DomainObjectMockBuilder.BuildTranslationInfoMock());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(householdMock));
        }

        /// <summary>
        /// Creates an instance of the <see cref="HouseholdDataGetQueryHandler"/> which can be used for unit testing.
        /// </summary>
        /// <returns>Instance of the <see cref="HouseholdDataGetQueryHandler"/> which can be used for unit testing.</returns>
        private HouseholdDataGetQueryHandler CreateSut()
        {
            return new HouseholdDataGetQueryHandler(_householdDataRepositoryMock, _claimValueProviderMock, _objectMapperMock);
        }

        /// <summary>
        /// Builds an instance of the <see cref="HouseholdDataGetQuery"/> which can be used for unit testing.
        /// </summary>
        /// <returns>Instance of the <see cref="HouseholdDataGetQuery"/> which can be used for unit testing.</returns>
        private HouseholdDataGetQuery BuildHouseholdDataGetQuery(Guid? householdIdentifier = null, Guid? translationInfoIdentifier = null)
        {
            return _fixture.Build<HouseholdDataGetQuery>()
                .With(m => m.HouseholdIdentifier, householdIdentifier ?? Guid.NewGuid())
                .With(m => m.TranslationInfoIdentifier, translationInfoIdentifier ?? Guid.NewGuid())
                .Create();
        }
    }
}
