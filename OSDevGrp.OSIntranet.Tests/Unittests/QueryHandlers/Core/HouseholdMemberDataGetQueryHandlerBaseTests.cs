using System;
using System.Globalization;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.QueryHandlers.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Resources;
using OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste;
using AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers.Core
{
    /// <summary>
    /// Tests the basic functionality which can handle a query for getting some data for a household member.
    /// </summary>
    [TestFixture]
    public class HouseholdMemberDataGetQueryHandlerBaseTests
    {
        /// <summary>
        /// Private class for a query which can get some data for a household member.
        /// </summary>
        private class MyHouseholdMemberDataGetQuery : HouseholdMemberDataGetQueryBase
        {
        }

        /// <summary>
        /// Private class for a query which can get some translatable data for a household member.
        /// </summary>
        private class MyHouseholdMemberTranslatableDataGetQuery : HouseholdMemberTranslatableDataGetQueryBase
        {
        }

        /// <summary>
        /// Private class for testing the basic functionality which can handle a query for getting some data for a household member.
        /// </summary>
        /// <typeparam name="TQuery">Type of the query for getting some data for a household member.</typeparam>
        private class MyHouseholdMemberDataGetQueryHandler<TQuery> : HouseholdMemberDataGetQueryHandlerBase<TQuery, object, IView> where TQuery : HouseholdMemberDataGetQueryBase, new()
        {
            #region Private variables

            private TQuery _query;
            private object _data;
            private readonly IHouseholdMember _householdMember;
            private readonly ITranslationInfo _translationInfo;
            private readonly bool? _shouldBeActivated;
            private readonly bool? _shouldHaveAcceptedPrivacyPolicy;
            private readonly Membership? _requiredMembership;

            #endregion

            #region Constructor

            /// <summary>
            /// Creates an instance of the private class for testing the basic functionality which can handle a query for getting some data for a household member.
            /// </summary>
            /// <param name="householdDataRepository">Implementation of a repository which can access household data for the food waste domain.</param>
            /// <param name="claimValueProvider">Implementation of a provider which can resolve values from the current users claims.</param>
            /// <param name="foodWasteObjectMapper">Implementation of an object mapper which can map objects in the food waste domain.</param>
            /// <param name="householdMember">Implementation of the household member which this query handler should get data for.</param>
            /// <param name="translationInfo">Implementation of the translation informations this query handler use for translations.</param>
            /// <param name="shouldBeActivated">Overrides whether the household member should be activated to get the data for the query handled by this query handler.</param>
            /// <param name="shouldHaveAcceptedPrivacyPolicy">Overrides whether the household member should have accepted the privacy policy to get the data for the query handled by this query handler.</param>
            /// <param name="requiredMembership">Overrides the requeired membership which the household member should have to get the data for the query handled by this query handler.</param>
            public MyHouseholdMemberDataGetQueryHandler(IHouseholdDataRepository householdDataRepository, IClaimValueProvider claimValueProvider, IFoodWasteObjectMapper foodWasteObjectMapper, IHouseholdMember householdMember, ITranslationInfo translationInfo = null, bool? shouldBeActivated = null, bool? shouldHaveAcceptedPrivacyPolicy = null, Membership? requiredMembership = null)
                : base(householdDataRepository, claimValueProvider, foodWasteObjectMapper)
            {
                if (householdMember == null)
                {
                    throw new ArgumentNullException("householdMember");
                }
                _householdMember = householdMember;
                _translationInfo = translationInfo;
                _shouldBeActivated = shouldBeActivated;
                _shouldHaveAcceptedPrivacyPolicy = shouldHaveAcceptedPrivacyPolicy;
                _requiredMembership = requiredMembership;
            }

            #endregion

            #region Properties

            /// <summary>
            /// Gets whether the household member should be activated to get the data for the query handled by this query handler.
            /// </summary>
            public override bool ShouldBeActivated
            {
                get { return _shouldBeActivated.HasValue ? _shouldBeActivated.Value : base.ShouldBeActivated; }
            }

            /// <summary>
            /// Gets whether the household member should have accepted the privacy policy to get the data for the query handled by this query handler.
            /// </summary>
            public override bool ShouldHaveAcceptedPrivacyPolicy
            {
                get { return _shouldHaveAcceptedPrivacyPolicy.HasValue ? _shouldHaveAcceptedPrivacyPolicy.Value : base.ShouldHaveAcceptedPrivacyPolicy; }
            }

            /// <summary>
            /// Gets the requeired membership which the household member should have to get the data for the query handled by this query handler.
            /// </summary>
            public override Membership RequiredMembership
            {
                get { return _requiredMembership.HasValue ? _requiredMembership.Value : base.RequiredMembership; }
            }

            /// <summary>
            /// Gets whether GetData has been called.
            /// </summary>
            public bool GetDataWasCalled { get; private set; }

            #endregion

            #region Methods

            /// <summary>
            /// Gets the repository which can access household data for the food waste domain.
            /// </summary>
            /// <returns>Repository which can access household data for the food waste domain</returns>
            public IHouseholdDataRepository GetHouseholdDataRepository()
            {
                return base.HouseholdDataRepository;
            }

            /// <summary>
            /// Gets the provider which can resolve values from the current users claims.
            /// </summary>
            /// <returns>Provider which can resolve values from the current users claims.</returns>
            public IClaimValueProvider GetClaimValueProvider()
            {
                return base.ClaimValueProvider;
            }

            /// <summary>
            /// Gets the object mapper which can map objects in the food waste domain.
            /// </summary>
            /// <returns>Object mapper which can map objects in the food waste domain.</returns>
            public IFoodWasteObjectMapper GetObjectMapper()
            {
                return base.ObjectMapper;
            }

            /// <summary>
            /// Generates and returns a query which can be used with this query handler.
            /// </summary>
            /// <returns>Query which can be used with this query handler.</returns>
            public TQuery GenerateQuery()
            {
                if (_query != null)
                {
                    return _query;
                }
                _query = new TQuery();
                if (_query is HouseholdMemberTranslatableDataGetQueryBase)
                {
                    (_query as HouseholdMemberTranslatableDataGetQueryBase).TranslationInfoIdentifier = Guid.NewGuid();
                }
                return _query;
            }

            /// <summary>
            /// Generates and returns selected data for the household member.
            /// </summary>
            /// <returns>Selected data for the household member.</returns>
            public object GenerateData()
            {
                return _data ?? (_data = new object());
            }

            /// <summary>
            /// Gets the data for the household member.
            /// </summary>
            /// <param name="householdMember">Household member for which to get the data.</param>
            /// <param name="query">Query for getting some data for a household member.</param>
            /// <param name="translationInfo">Translation informations.</param>
            /// <returns>Data for the household member.</returns>
            public override object GetData(IHouseholdMember householdMember, TQuery query, ITranslationInfo translationInfo)
            {
                Assert.That(householdMember, Is.Not.Null);
                Assert.That(householdMember, Is.EqualTo(_householdMember));
                Assert.That(query, Is.Not.Null);
                Assert.That(query, Is.EqualTo(GenerateQuery()));
                Assert.That(translationInfo, Is.EqualTo(_translationInfo));

                GetDataWasCalled = true;

                return GenerateData();
            }

            #endregion
        }

        /// <summary>
        /// Tests that the constructor initialize the basic functionality which can handle a query for getting some data for a household member.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeHouseholdMemberDataGetQueryHandlerBase()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var householdMemberDataGetQueryHandlerBase = new MyHouseholdMemberDataGetQueryHandler<MyHouseholdMemberDataGetQuery>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, DomainObjectMockBuilder.BuildHouseholdMemberMock());
            Assert.That(householdMemberDataGetQueryHandlerBase, Is.Not.Null);
            Assert.That(householdMemberDataGetQueryHandlerBase.ShouldBeActivated, Is.True);
            Assert.That(householdMemberDataGetQueryHandlerBase.ShouldHaveAcceptedPrivacyPolicy, Is.True);
            Assert.That(householdMemberDataGetQueryHandlerBase.RequiredMembership, Is.EqualTo(Membership.Basic));
            Assert.That(householdMemberDataGetQueryHandlerBase.GetHouseholdDataRepository(), Is.Not.Null);
            Assert.That(householdMemberDataGetQueryHandlerBase.GetHouseholdDataRepository(), Is.EqualTo(householdDataRepositoryMock));
            Assert.That(householdMemberDataGetQueryHandlerBase.GetClaimValueProvider(), Is.Not.Null);
            Assert.That(householdMemberDataGetQueryHandlerBase.GetClaimValueProvider(), Is.EqualTo(claimValueProviderMock));
            Assert.That(householdMemberDataGetQueryHandlerBase.GetObjectMapper(), Is.Not.Null);
            Assert.That(householdMemberDataGetQueryHandlerBase.GetObjectMapper(), Is.EqualTo(objectMapperMock));
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the repository which can access household data for the food waste domain is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenHouseholdDataRepositoryIsNull()
        {
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyHouseholdMemberDataGetQueryHandler<MyHouseholdMemberDataGetQuery>(null, claimValueProviderMock, objectMapperMock, DomainObjectMockBuilder.BuildHouseholdMemberMock()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("householdDataRepository"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the provider which can resolve values from the current users claims is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenClaimValueProviderIsNull()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyHouseholdMemberDataGetQueryHandler<MyHouseholdMemberDataGetQuery>(householdDataRepositoryMock, null, objectMapperMock, DomainObjectMockBuilder.BuildHouseholdMemberMock()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("claimValueProvider"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the object mapper which can map objects in the food waste domain is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenFoodWasteObjectMapperIsNull()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyHouseholdMemberDataGetQueryHandler<MyHouseholdMemberDataGetQuery>(householdDataRepositoryMock, claimValueProviderMock, null, DomainObjectMockBuilder.BuildHouseholdMemberMock()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("foodWasteObjectMapper"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the household member which this query handler should get data for is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenHouseholdMemberIsNull()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyHouseholdMemberDataGetQueryHandler<MyHouseholdMemberDataGetQuery>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("householdMember"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Query throws an ArgumentNullException when the query for getting some data for a household member is null.
        /// </summary>
        [Test]
        public void TestThatQueryThrowsArgumentNullExceptionWhenQueryIsNull()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var householdMemberDataGetQueryHandlerBase = new MyHouseholdMemberDataGetQueryHandler<MyHouseholdMemberDataGetQuery>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, DomainObjectMockBuilder.BuildHouseholdMemberMock());
            Assert.That(householdMemberDataGetQueryHandlerBase, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdMemberDataGetQueryHandlerBase.Query(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("query"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Query does not call Get for TranslationInfo on the repository which can access household data for the food waste domain when the query for getting some data for a household member is not a query which can get some translatable data for a household member.
        /// </summary>
        [Test]
        public void TestThatQueryDoesNotCallGetForTranslationInfoOnHouseholdDataRepositoryWhenQueryIsNotHouseholdMemberTranslatableDataGetQueryBase()
        {
            var fixture = new Fixture();

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock(fixture.Create<Membership>());
            householdMemberMock.Stub(m => m.HasRequiredMembership(Arg<Membership>.Is.Anything))
                .Return(true)
                .Repeat.Any();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(householdMemberMock)
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<object, IView>(Arg<object>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(MockRepository.GenerateMock<IView>())
                .Repeat.Any();

            var householdMemberDataGetQueryHandlerBase = new MyHouseholdMemberDataGetQueryHandler<MyHouseholdMemberDataGetQuery>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, householdMemberMock);
            Assert.That(householdMemberDataGetQueryHandlerBase, Is.Not.Null);

            householdMemberDataGetQueryHandlerBase.Query(householdMemberDataGetQueryHandlerBase.GenerateQuery());

            householdDataRepositoryMock.AssertWasNotCalled(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything));
        }

        /// <summary>
        /// Tests that Query calls Get for TranslationInfoIdentifier on the repository which can access household data for the food waste domain when the query for getting some data for a household member is a query which can get some translatable data for a household member.
        /// </summary>
        [Test]
        public void TestThatQueryCallsGetForTranslationInfoIdentifierOnHouseholdDataRepositoryWhenQueryIsHouseholdMemberTranslatableDataGetQueryBase()
        {
            var fixture = new Fixture();

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock(fixture.Create<Membership>());
            householdMemberMock.Stub(m => m.HasRequiredMembership(Arg<Membership>.Is.Anything))
                .Return(true)
                .Repeat.Any();

            var translationInfoMock = DomainObjectMockBuilder.BuildTranslationInfoMock();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(translationInfoMock)
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(householdMemberMock)
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<object, IView>(Arg<object>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(MockRepository.GenerateMock<IView>())
                .Repeat.Any();

            var householdMemberDataGetQueryHandlerBase = new MyHouseholdMemberDataGetQueryHandler<MyHouseholdMemberTranslatableDataGetQuery>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, householdMemberMock, translationInfoMock);
            Assert.That(householdMemberDataGetQueryHandlerBase, Is.Not.Null);

            var query = householdMemberDataGetQueryHandlerBase.GenerateQuery();
            householdMemberDataGetQueryHandlerBase.Query(query);

            householdDataRepositoryMock.AssertWasCalled(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Equal(query.TranslationInfoIdentifier)));
        }

        /// <summary>
        /// Tests that Query throws an IntranetBusinessException when translation informations for TranslationInfoIdentifier does not exist.
        /// </summary>
        [Test]
        public void TestThatQueryThrowsIntranetBusinessExceptionWhenTranslationInfoForTranslationInfoIdentifierDoesNotExist()
        {
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(null)
                .Repeat.Any();

            var householdMemberDataGetQueryHandlerBase = new MyHouseholdMemberDataGetQueryHandler<MyHouseholdMemberTranslatableDataGetQuery>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, DomainObjectMockBuilder.BuildHouseholdMemberMock());
            Assert.That(householdMemberDataGetQueryHandlerBase, Is.Not.Null);

            var query = householdMemberDataGetQueryHandlerBase.GenerateQuery();

            var exception = Assert.Throws<IntranetBusinessException>(() => householdMemberDataGetQueryHandlerBase.Query(query));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IdentifierUnknownToSystem, query.TranslationInfoIdentifier)));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Query calls MailAddress on the provider which can resolve values from the current users claims.
        /// </summary>
        [Test]
        public void TestThatQueryCallsMailAddressOnClaimValueProvider()
        {
            var fixture = new Fixture();

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock(fixture.Create<Membership>());
            householdMemberMock.Stub(m => m.HasRequiredMembership(Arg<Membership>.Is.Anything))
                .Return(true)
                .Repeat.Any();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(householdMemberMock)
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<object, IView>(Arg<object>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(MockRepository.GenerateMock<IView>())
                .Repeat.Any();

            var householdMemberDataGetQueryHandlerBase = new MyHouseholdMemberDataGetQueryHandler<MyHouseholdMemberDataGetQuery>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, householdMemberMock);
            Assert.That(householdMemberDataGetQueryHandlerBase, Is.Not.Null);

            householdMemberDataGetQueryHandlerBase.Query(householdMemberDataGetQueryHandlerBase.GenerateQuery());

            claimValueProviderMock.AssertWasCalled(m => m.MailAddress);
        }

        /// <summary>
        /// Tests that Query calls HouseholdMemberGetByMailAddress on the repository which can access household data for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatQueryCallsHouseholdMemberGetByMailAddressOnHouseholdDataRepository()
        {
            var fixture = new Fixture();

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock(fixture.Create<Membership>());
            householdMemberMock.Stub(m => m.HasRequiredMembership(Arg<Membership>.Is.Anything))
                .Return(true)
                .Repeat.Any();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(householdMemberMock)
                .Repeat.Any();

            var mailAddress = fixture.Create<string>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(mailAddress)
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<object, IView>(Arg<object>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(MockRepository.GenerateMock<IView>())
                .Repeat.Any();

            var householdMemberDataGetQueryHandlerBase = new MyHouseholdMemberDataGetQueryHandler<MyHouseholdMemberDataGetQuery>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, householdMemberMock);
            Assert.That(householdMemberDataGetQueryHandlerBase, Is.Not.Null);

            householdMemberDataGetQueryHandlerBase.Query(householdMemberDataGetQueryHandlerBase.GenerateQuery());

            householdDataRepositoryMock.AssertWasCalled(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Equal(mailAddress)));
        }

        /// <summary>
        /// Tests that Query throws an IntranetBusinessException when the mail address has not been created as a household member.
        /// </summary>
        [Test]
        public void TestThatQueryThrowsIntranetBusinessExceptionWhenMailAddressHasNotBeenCreatedAsHouseholdMember()
        {
            var fixture = new Fixture();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(null)
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var householdMemberDataGetQueryHandlerBase = new MyHouseholdMemberDataGetQueryHandler<MyHouseholdMemberDataGetQuery>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, DomainObjectMockBuilder.BuildHouseholdMemberMock());
            Assert.That(householdMemberDataGetQueryHandlerBase, Is.Not.Null);

            var exception = Assert.Throws<IntranetBusinessException>(() => householdMemberDataGetQueryHandlerBase.Query(householdMemberDataGetQueryHandlerBase.GenerateQuery()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.HouseholdMemberNotCreated)));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Query does not call IsActivated on the household member when the household member does not need to be activated.
        /// </summary>
        [Test]
        public void TestThatQueryDoesNotCallIsActivatedOnHouseholdMemberWhenHouseholdMemberDoesNotNeedToBeActivated()
        {
            var fixture = new Fixture();

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            householdMemberMock.Stub(m => m.HasRequiredMembership(Arg<Membership>.Is.Anything))
                .Return(true)
                .Repeat.Any();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(householdMemberMock)
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<object, IView>(Arg<object>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(MockRepository.GenerateMock<IView>())
                .Repeat.Any();

            var householdMemberDataGetQueryHandlerBase = new MyHouseholdMemberDataGetQueryHandler<MyHouseholdMemberDataGetQuery>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, householdMemberMock, shouldBeActivated: false);
            Assert.That(householdMemberDataGetQueryHandlerBase, Is.Not.Null);
            Assert.That(householdMemberDataGetQueryHandlerBase.ShouldBeActivated, Is.False);

            householdMemberDataGetQueryHandlerBase.Query(householdMemberDataGetQueryHandlerBase.GenerateQuery());

            householdMemberMock.AssertWasNotCalled(m => m.IsActivated);
        }

        /// <summary>
        /// Tests that Query calls IsActivated on the household member when the household member should be activated.
        /// </summary>
        [Test]
        public void TestThatQueryCallsIsActivatedOnHouseholdMemberWhenHouseholdMemberShouldBeActivated()
        {
            var fixture = new Fixture();

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            householdMemberMock.Stub(m => m.HasRequiredMembership(Arg<Membership>.Is.Anything))
                .Return(true)
                .Repeat.Any();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(householdMemberMock)
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<object, IView>(Arg<object>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(MockRepository.GenerateMock<IView>())
                .Repeat.Any();

            var householdMemberDataGetQueryHandlerBase = new MyHouseholdMemberDataGetQueryHandler<MyHouseholdMemberDataGetQuery>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, householdMemberMock);
            Assert.That(householdMemberDataGetQueryHandlerBase, Is.Not.Null);
            Assert.That(householdMemberDataGetQueryHandlerBase.ShouldBeActivated, Is.True);

            householdMemberDataGetQueryHandlerBase.Query(householdMemberDataGetQueryHandlerBase.GenerateQuery());

            householdMemberMock.AssertWasCalled(m => m.IsActivated);
        }

        /// <summary>
        /// Tests that Query throws an IntranetBusinessException when the household member should be activated but is not.
        /// </summary>
        [Test]
        public void TestThatQueryThrowsIntranetBusinessExceptionWhenHouseholdMemberShouldBeActivatedButIsNot()
        {
            var fixture = new Fixture();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock(isActivated: false);
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(householdMemberMock)
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var householdMemberDataGetQueryHandlerBase = new MyHouseholdMemberDataGetQueryHandler<MyHouseholdMemberDataGetQuery>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, householdMemberMock);
            Assert.That(householdMemberDataGetQueryHandlerBase, Is.Not.Null);
            Assert.That(householdMemberDataGetQueryHandlerBase.ShouldBeActivated, Is.True);

            var exception = Assert.Throws<IntranetBusinessException>(() => householdMemberDataGetQueryHandlerBase.Query(householdMemberDataGetQueryHandlerBase.GenerateQuery()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.HouseholdMemberNotActivated)));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Query does not call IsPrivacyPolictyAccepted on the household member when the household member does not need to have accepted the privacy policies.
        /// </summary>
        [Test]
        public void TestThatQueryDoesNotCallIsPrivacyPolictyAcceptedOnHouseholdMemberWhenHouseholdMemberDoesNotNeedToHaveAcceptedPrivacyPolicy()
        {
            var fixture = new Fixture();

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            householdMemberMock.Stub(m => m.HasRequiredMembership(Arg<Membership>.Is.Anything))
                .Return(true)
                .Repeat.Any();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(householdMemberMock)
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<object, IView>(Arg<object>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(MockRepository.GenerateMock<IView>())
                .Repeat.Any();

            var householdMemberDataGetQueryHandlerBase = new MyHouseholdMemberDataGetQueryHandler<MyHouseholdMemberDataGetQuery>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, householdMemberMock, shouldHaveAcceptedPrivacyPolicy: false);
            Assert.That(householdMemberDataGetQueryHandlerBase, Is.Not.Null);
            Assert.That(householdMemberDataGetQueryHandlerBase.ShouldHaveAcceptedPrivacyPolicy, Is.False);

            householdMemberDataGetQueryHandlerBase.Query(householdMemberDataGetQueryHandlerBase.GenerateQuery());

            householdMemberMock.AssertWasNotCalled(m => m.IsPrivacyPolictyAccepted);
        }

        /// <summary>
        /// Tests that Query calls IsPrivacyPolictyAccepted on the household member when the household member should have accepted the privacy policies.
        /// </summary>
        [Test]
        public void TestThatQueryCallsIsPrivacyPolictyAcceptedOnHouseholdMemberWhenHouseholdMemberShouldHaveAcceptedPrivacyPolicy()
        {
            var fixture = new Fixture();

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            householdMemberMock.Stub(m => m.HasRequiredMembership(Arg<Membership>.Is.Anything))
                .Return(true)
                .Repeat.Any();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(householdMemberMock)
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<object, IView>(Arg<object>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(MockRepository.GenerateMock<IView>())
                .Repeat.Any();

            var householdMemberDataGetQueryHandlerBase = new MyHouseholdMemberDataGetQueryHandler<MyHouseholdMemberDataGetQuery>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, householdMemberMock);
            Assert.That(householdMemberDataGetQueryHandlerBase, Is.Not.Null);
            Assert.That(householdMemberDataGetQueryHandlerBase.ShouldHaveAcceptedPrivacyPolicy, Is.True);

            householdMemberDataGetQueryHandlerBase.Query(householdMemberDataGetQueryHandlerBase.GenerateQuery());

            householdMemberMock.AssertWasCalled(m => m.IsPrivacyPolictyAccepted);
        }

        /// <summary>
        /// Tests that Query throws an IntranetBusinessException when the household member should have accepted the privacy policies but has not.
        /// </summary>
        [Test]
        public void TestThatQueryThrowsIntranetBusinessExceptionWhenHouseholdMemberShouldHaveAcceptedPrivacyPolicyButHasNot()
        {
            var fixture = new Fixture();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock(isPrivacyPolictyAccepted: false);
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(householdMemberMock)
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var householdMemberDataGetQueryHandlerBase = new MyHouseholdMemberDataGetQueryHandler<MyHouseholdMemberDataGetQuery>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, householdMemberMock);
            Assert.That(householdMemberDataGetQueryHandlerBase, Is.Not.Null);
            Assert.That(householdMemberDataGetQueryHandlerBase.ShouldHaveAcceptedPrivacyPolicy, Is.True);

            var exception = Assert.Throws<IntranetBusinessException>(() => householdMemberDataGetQueryHandlerBase.Query(householdMemberDataGetQueryHandlerBase.GenerateQuery()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.HouseholdMemberHasNotAcceptedPrivacyPolicy)));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Query calls HasRequiredMembership on the household member.
        /// </summary>
        [Test]
        [TestCase(Membership.Basic)]
        [TestCase(Membership.Deluxe)]
        [TestCase(Membership.Premium)]
        public void TestThatQueryCallsHasRequiredMembershipOnHouseholdMember(Membership requiredMembership)
        {
            var fixture = new Fixture();

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            householdMemberMock.Stub(m => m.HasRequiredMembership(Arg<Membership>.Is.Anything))
                .Return(true)
                .Repeat.Any();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(householdMemberMock)
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<object, IView>(Arg<object>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(MockRepository.GenerateMock<IView>())
                .Repeat.Any();

            var householdMemberDataGetQueryHandlerBase = new MyHouseholdMemberDataGetQueryHandler<MyHouseholdMemberDataGetQuery>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, householdMemberMock, requiredMembership: requiredMembership);
            Assert.That(householdMemberDataGetQueryHandlerBase, Is.Not.Null);
            Assert.That(householdMemberDataGetQueryHandlerBase.RequiredMembership, Is.EqualTo(requiredMembership));

            householdMemberDataGetQueryHandlerBase.Query(householdMemberDataGetQueryHandlerBase.GenerateQuery());

            householdMemberMock.AssertWasCalled(m => m.HasRequiredMembership(Arg<Membership>.Is.Equal(requiredMembership)));
        }

        /// <summary>
        /// Tests that Query throws an IntranetBusinessException when the household member does not have the required membership.
        /// </summary>
        [Test]
        public void TestThatQueryThrowsIntranetBusinessExceptionWhenHouseholdMemberDoesNotHaveRequiredMembership()
        {
            var fixture = new Fixture();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            householdMemberMock.Stub(m => m.HasRequiredMembership(Arg<Membership>.Is.Anything))
                .Return(false)
                .Repeat.Any();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(householdMemberMock)
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var householdMemberDataGetQueryHandlerBase = new MyHouseholdMemberDataGetQueryHandler<MyHouseholdMemberDataGetQuery>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, householdMemberMock);
            Assert.That(householdMemberDataGetQueryHandlerBase, Is.Not.Null);

            var exception = Assert.Throws<IntranetBusinessException>(() => householdMemberDataGetQueryHandlerBase.Query(householdMemberDataGetQueryHandlerBase.GenerateQuery()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.HouseholdMemberHasNotRequiredMembership)));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Query calls GetData to get the data for the household member.
        /// </summary>
        [Test]
        public void TestThatQueryCallsGetData()
        {
            var fixture = new Fixture();

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            householdMemberMock.Stub(m => m.HasRequiredMembership(Arg<Membership>.Is.Anything))
                .Return(true)
                .Repeat.Any();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(householdMemberMock)
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<object, IView>(Arg<object>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(MockRepository.GenerateMock<IView>())
                .Repeat.Any();

            var householdMemberDataGetQueryHandlerBase = new MyHouseholdMemberDataGetQueryHandler<MyHouseholdMemberDataGetQuery>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, householdMemberMock);
            Assert.That(householdMemberDataGetQueryHandlerBase, Is.Not.Null);
            Assert.That(householdMemberDataGetQueryHandlerBase.GetDataWasCalled, Is.False);

            householdMemberDataGetQueryHandlerBase.Query(householdMemberDataGetQueryHandlerBase.GenerateQuery());

            Assert.That(householdMemberDataGetQueryHandlerBase.GetDataWasCalled, Is.True);
        }

        /// <summary>
        /// Tests that Query calls Map without a culture information on the object mapper which can map objects in the food waste domain when the query for getting some data for a household member is not a query which can get some translatable data for a household member.
        /// </summary>
        [Test]
        public void TestThatQueryCallsMapWithoutCultureInfoOnFoodWasteObjectMapperWhenQueryQueryIsNotHouseholdMemberTranslatableDataGetQueryBase()
        {
            var fixture = new Fixture();

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            householdMemberMock.Stub(m => m.HasRequiredMembership(Arg<Membership>.Is.Anything))
                .Return(true)
                .Repeat.Any();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(householdMemberMock)
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<object, IView>(Arg<object>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(MockRepository.GenerateMock<IView>())
                .Repeat.Any();

            var householdMemberDataGetQueryHandlerBase = new MyHouseholdMemberDataGetQueryHandler<MyHouseholdMemberDataGetQuery>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, householdMemberMock);
            Assert.That(householdMemberDataGetQueryHandlerBase, Is.Not.Null);

            householdMemberDataGetQueryHandlerBase.Query(householdMemberDataGetQueryHandlerBase.GenerateQuery());

            objectMapperMock.AssertWasCalled(m => m.Map<object, IView>(Arg<object>.Is.Equal(householdMemberDataGetQueryHandlerBase.GenerateData()), Arg<CultureInfo>.Is.Null));
        }

        /// <summary>
        /// Tests that Query calls Map with the culture information from translation informations on the object mapper which can map objects in the food waste domain when the query for getting some data for a household member is a query which can get some translatable data for a household member.
        /// </summary>
        [Test]
        public void TestThatQueryCallsMapWithCultureInfoFromTranslationInfoOnFoodWasteObjectMapperWhenQueryQueryIsHouseholdMemberTranslatableDataGetQueryBase()
        {
            var fixture = new Fixture();

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            householdMemberMock.Stub(m => m.HasRequiredMembership(Arg<Membership>.Is.Anything))
                .Return(true)
                .Repeat.Any();

            var translationInfoMock = DomainObjectMockBuilder.BuildTranslationInfoMock();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(translationInfoMock)
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(householdMemberMock)
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<object, IView>(Arg<object>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(MockRepository.GenerateMock<IView>())
                .Repeat.Any();

            var householdMemberDataGetQueryHandlerBase = new MyHouseholdMemberDataGetQueryHandler<MyHouseholdMemberTranslatableDataGetQuery>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, householdMemberMock, translationInfoMock);
            Assert.That(householdMemberDataGetQueryHandlerBase, Is.Not.Null);

            householdMemberDataGetQueryHandlerBase.Query(householdMemberDataGetQueryHandlerBase.GenerateQuery());

            objectMapperMock.AssertWasCalled(m => m.Map<object, IView>(Arg<object>.Is.Equal(householdMemberDataGetQueryHandlerBase.GenerateData()), Arg<CultureInfo>.Is.Equal(translationInfoMock.CultureInfo)));
        }

        /// <summary>
        /// Tests that Query returns the result from Map on the object mapper which can map objects in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatQueryReturnsResultFromMapOnFoodWasteObjectMapper()
        {
            var fixture = new Fixture();

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            householdMemberMock.Stub(m => m.HasRequiredMembership(Arg<Membership>.Is.Anything))
                .Return(true)
                .Repeat.Any();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(householdMemberMock)
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var view = MockRepository.GenerateMock<IView>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<object, IView>(Arg<object>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(view)
                .Repeat.Any();

            var householdMemberDataGetQueryHandlerBase = new MyHouseholdMemberDataGetQueryHandler<MyHouseholdMemberDataGetQuery>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, householdMemberMock);
            Assert.That(householdMemberDataGetQueryHandlerBase, Is.Not.Null);

            var result = householdMemberDataGetQueryHandlerBase.Query(householdMemberDataGetQueryHandlerBase.GenerateQuery());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(view));
        }
    }
}
