using System;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.QueryHandlers;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Resources;
using OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste;
using AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers
{
    /// <summary>
    /// Test the query handler which handles the query for getting household data for one of the current user households.
    /// </summary>
    [TestFixture]
    public class HouseholdDataGetQueryHandlerTests
    {
        /// <summary>
        /// Tests that the constructor initialize the query handler which handles the query for getting household data for one of the current user households.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeHouseholdDataGetQueryHandler()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var householdDataGetQueryHandler = new HouseholdDataGetQueryHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock);
            Assert.That(householdDataGetQueryHandler, Is.Not.Null);
            Assert.That(householdDataGetQueryHandler.ShouldBeActivated, Is.True);
            Assert.That(householdDataGetQueryHandler.ShouldHaveAcceptedPrivacyPolicy, Is.True);
            Assert.That(householdDataGetQueryHandler.RequiredMembership, Is.EqualTo(Membership.Basic));
        }

        /// <summary>
        /// Tests that GetData throws ArgumentNullException when the household member is null.
        /// </summary>
        [Test]
        public void TestThatGetDataThrowsArgumentNullExceptionWhenHouseholdMemberIsNull()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var householdDataGetQueryHandler = new HouseholdDataGetQueryHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock);
            Assert.That(householdDataGetQueryHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdDataGetQueryHandler.GetData(null, fixture.Create<HouseholdDataGetQuery>(), DomainObjectMockBuilder.BuildTranslationInfoMock()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("householdMember"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetData throws ArgumentNullException when the query for for getting household data for one of the current user households is null.
        /// </summary>
        [Test]
        public void TestThatGetDataThrowsArgumentNullExceptionWhenQueryIsNull()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var householdDataGetQueryHandler = new HouseholdDataGetQueryHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock);
            Assert.That(householdDataGetQueryHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdDataGetQueryHandler.GetData(DomainObjectMockBuilder.BuildHouseholdMemberMock(), null, DomainObjectMockBuilder.BuildTranslationInfoMock()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("query"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetData throws ArgumentNullException when the translation informations which can be used for translation is null.
        /// </summary>
        [Test]
        public void TestThatGetDataThrowsArgumentNullExceptionWhenTranslationInfoIsNull()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var householdDataGetQueryHandler = new HouseholdDataGetQueryHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock);
            Assert.That(householdDataGetQueryHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdDataGetQueryHandler.GetData(DomainObjectMockBuilder.BuildHouseholdMemberMock(), fixture.Create<HouseholdDataGetQuery>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("translationInfo"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetData calls Households on the household member on which data for a household should be returned.
        /// </summary>
        [Test]
        public void TestThatGetDataCallsHouseholdsOnHouseholdMember()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var householdDataGetQueryHandler = new HouseholdDataGetQueryHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock);
            Assert.That(householdDataGetQueryHandler, Is.Not.Null);

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            Assert.That(householdMemberMock, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Empty);

            var householdDataGetQuery = fixture.Build<HouseholdDataGetQuery>()
                // ReSharper disable PossibleInvalidOperationException
                .With(m => m.HouseholdIdentifier, householdMemberMock.Households.First().Identifier.Value)
                // ReSharper restore PossibleInvalidOperationException
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            householdDataGetQueryHandler.GetData(householdMemberMock, householdDataGetQuery, DomainObjectMockBuilder.BuildTranslationInfoMock());

            householdMemberMock.AssertWasCalled(m => m.Households, opt => opt.Repeat.Times(3 + 1)); // Tree times in the unit test and one time in the query handler.
        }

        /// <summary>
        /// Tests that GetData throws an IntranetBusinessException when a household with the household identifier does not exists on the household member.
        /// </summary>
        [Test]
        public void TestThatGetDataThrowsIntranetBusinessExceptionWhenHouseholdIdentifierDoesNotExistOnHouseholdMember()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var householdDataGetQueryHandler = new HouseholdDataGetQueryHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock);
            Assert.That(householdDataGetQueryHandler, Is.Not.Null);

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            Assert.That(householdMemberMock, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Empty);

            var householdIdentifier = Guid.NewGuid();
            Assert.That(householdMemberMock.Households.Any(household => household.Identifier.HasValue && household.Identifier.Value == householdIdentifier), Is.False);

            var householdDataGetQuery = fixture.Build<HouseholdDataGetQuery>()
                .With(m => m.HouseholdIdentifier, householdIdentifier)
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            var exception = Assert.Throws<IntranetBusinessException>(() => householdDataGetQueryHandler.GetData(householdMemberMock, householdDataGetQuery, DomainObjectMockBuilder.BuildTranslationInfoMock()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IdentifierUnknownToSystem, householdIdentifier)));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetData calls Translate on the household with the household identifier.
        /// </summary>
        [Test]
        public void TestThatGetDataCallsTranslateOnHouseholdForHouseholdIdentifier()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var householdDataGetQueryHandler = new HouseholdDataGetQueryHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock);
            Assert.That(householdDataGetQueryHandler, Is.Not.Null);

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            Assert.That(householdMemberMock, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Empty);

            var householdMock = householdMemberMock.Households.FirstOrDefault();
            Assert.That(householdMock, Is.Not.Null);

            var householdDataGetQuery = fixture.Build<HouseholdDataGetQuery>()
                // ReSharper disable PossibleInvalidOperationException
                .With(m => m.HouseholdIdentifier, householdMock.Identifier.Value)
                // ReSharper restore PossibleInvalidOperationException
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            var translationInfoMock = DomainObjectMockBuilder.BuildTranslationInfoMock();

            householdDataGetQueryHandler.GetData(householdMemberMock, householdDataGetQuery, translationInfoMock);

            householdMock.AssertWasCalled(m => m.Translate(Arg<CultureInfo>.Is.Equal(translationInfoMock.CultureInfo), Arg<bool>.Is.Equal(false)));
        }

        /// <summary>
        /// Tests that GetData returns the household with the household identifier.
        /// </summary>
        [Test]
        public void TestThatGetDataReturnsHouseholdForHouseholdIdentifier()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var householdDataGetQueryHandler = new HouseholdDataGetQueryHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock);
            Assert.That(householdDataGetQueryHandler, Is.Not.Null);

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            Assert.That(householdMemberMock, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Empty);

            var householdMock = householdMemberMock.Households.FirstOrDefault();
            Assert.That(householdMock, Is.Not.Null);

            var householdDataGetQuery = fixture.Build<HouseholdDataGetQuery>()
                // ReSharper disable PossibleInvalidOperationException
                .With(m => m.HouseholdIdentifier, householdMock.Identifier.Value)
                // ReSharper restore PossibleInvalidOperationException
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            var result = householdDataGetQueryHandler.GetData(householdMemberMock, householdDataGetQuery, DomainObjectMockBuilder.BuildTranslationInfoMock());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(householdMock));
        }
    }
}
