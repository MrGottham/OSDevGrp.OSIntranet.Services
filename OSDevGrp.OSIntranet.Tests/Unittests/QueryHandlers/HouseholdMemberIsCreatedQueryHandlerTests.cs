using System;
using System.Globalization;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Responses;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.QueryHandlers;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste;
using AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers
{
    /// <summary>
    /// Test the query handler which handles the query which can check whether the current user has been created as a household member.
    /// </summary>
    [TestFixture]
    public class HouseholdMemberIsCreatedQueryHandlerTests
    {
        /// <summary>
        /// Tests that the constructor initialize the query handler which handles the query which can check whether the current user has been created as a household member.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeHouseholdMemberIsCreatedQueryHandler()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var householdMemberIsCreatedQueryHandler = new HouseholdMemberIsCreatedQueryHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock);
            Assert.That(householdMemberIsCreatedQueryHandler, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the repository which can access household data for the food waste domain is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenHouseholdDataRepositoryIsNull()
        {
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var exception = Assert.Throws<ArgumentNullException>(() => new HouseholdMemberIsCreatedQueryHandler(null, claimValueProviderMock, objectMapperMock));
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

            var exception = Assert.Throws<ArgumentNullException>(() => new HouseholdMemberIsCreatedQueryHandler(householdDataRepositoryMock, null, objectMapperMock));
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

            var exception = Assert.Throws<ArgumentNullException>(() => new HouseholdMemberIsCreatedQueryHandler(householdDataRepositoryMock, claimValueProviderMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("foodWasteObjectMapper"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Query throws an ArgumentNullException when the query which can check whether the current user has been created as a household member is null.
        /// </summary>
        [Test]
        public void TestThatQueryThrowsArgumentNullExceptionWhenQueryIsNull()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var householdMemberIsCreatedQueryHandler = new HouseholdMemberIsCreatedQueryHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock);
            Assert.That(householdMemberIsCreatedQueryHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdMemberIsCreatedQueryHandler.Query(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("query"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Query calls the getter of MailAddress on the provider which can resolve values from the current users claims.
        /// </summary>
        [Test]
        public void TestThatQueryCallsMailAddressGettterOnClaimValueProvider()
        {
            var fixture = new Fixture();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<bool, BooleanResultResponse>(Arg<bool>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<BooleanResultResponse>())
                .Repeat.Any();

            var householdMemberIsCreatedQueryHandler = new HouseholdMemberIsCreatedQueryHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock);
            Assert.That(householdMemberIsCreatedQueryHandler, Is.Not.Null);

            householdMemberIsCreatedQueryHandler.Query(fixture.Create<HouseholdMemberIsCreatedQuery>());

            claimValueProviderMock.AssertWasCalled(m => m.MailAddress);
        }

        /// <summary>
        /// Tests that Query calls HouseholdMemberGetByMailAddress on the repository which can access household data for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatQueryCallsHouseholdMemberGetByMailAddressOnHouseholdDataRepository()
        {
            var fixture = new Fixture();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();

            var mailAddress = fixture.Create<string>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(mailAddress)
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<bool, BooleanResultResponse>(Arg<bool>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<BooleanResultResponse>())
                .Repeat.Any();

            var householdMemberIsCreatedQueryHandler = new HouseholdMemberIsCreatedQueryHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock);
            Assert.That(householdMemberIsCreatedQueryHandler, Is.Not.Null);

            householdMemberIsCreatedQueryHandler.Query(fixture.Create<HouseholdMemberIsCreatedQuery>());

            householdDataRepositoryMock.AssertWasCalled(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Equal(mailAddress)));
        }

        /// <summary>
        /// Tests that Query calls Map with true on the object mapper which can map objects in the food waste domain when the household member does exist.
        /// </summary>
        [Test]
        public void TestThatQueryCallsMapWithTrueOnFoodWasteObjectMapperWhenHouseholdMemberDoesExist()
        {
            var fixture = new Fixture();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<bool, BooleanResultResponse>(Arg<bool>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<BooleanResultResponse>())
                .Repeat.Any();

            var householdMemberIsCreatedQueryHandler = new HouseholdMemberIsCreatedQueryHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock);
            Assert.That(householdMemberIsCreatedQueryHandler, Is.Not.Null);

            householdMemberIsCreatedQueryHandler.Query(fixture.Create<HouseholdMemberIsCreatedQuery>());

            objectMapperMock.AssertWasCalled(m => m.Map<bool, BooleanResultResponse>(Arg<bool>.Is.Equal(true), Arg<CultureInfo>.Is.Null));
        }

        /// <summary>
        /// Tests that Query calls Map with false on the object mapper which can map objects in the food waste domain when the household member does not exist.
        /// </summary>
        [Test]
        public void TestThatQueryCallsMapWithFalseOnFoodWasteObjectMapperWhenHouseholdMemberDoesNotExist()
        {
            var fixture = new Fixture();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(null)
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<bool, BooleanResultResponse>(Arg<bool>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<BooleanResultResponse>())
                .Repeat.Any();

            var householdMemberIsCreatedQueryHandler = new HouseholdMemberIsCreatedQueryHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock);
            Assert.That(householdMemberIsCreatedQueryHandler, Is.Not.Null);

            householdMemberIsCreatedQueryHandler.Query(fixture.Create<HouseholdMemberIsCreatedQuery>());

            objectMapperMock.AssertWasCalled(m => m.Map<bool, BooleanResultResponse>(Arg<bool>.Is.Equal(false), Arg<CultureInfo>.Is.Null));
        }

        /// <summary>
        /// Tests that Query returns the result from Map on the object mapper which can map objects in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatQueryReturnsResultFromMapOnFoodWasteObjectMapper()
        {
            var fixture = new Fixture();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var booleanResultResponse = fixture.Create<BooleanResultResponse>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<bool, BooleanResultResponse>(Arg<bool>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(booleanResultResponse)
                .Repeat.Any();

            var householdMemberIsCreatedQueryHandler = new HouseholdMemberIsCreatedQueryHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock);
            Assert.That(householdMemberIsCreatedQueryHandler, Is.Not.Null);

            var result = householdMemberIsCreatedQueryHandler.Query(fixture.Create<HouseholdMemberIsCreatedQuery>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(booleanResultResponse));
        }
    }
}
