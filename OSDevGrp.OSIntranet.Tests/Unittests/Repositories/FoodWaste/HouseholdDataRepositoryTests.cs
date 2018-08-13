using System;
using System.Collections.Generic;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Resources;
using AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.FoodWaste
{
    /// <summary>
    /// Tests the repository which can access household data for the food waste domain.
    /// </summary>
    [TestFixture]
    public class HouseholdDataRepositoryTests
    {
        /// <summary>
        /// Tests that the constructor initialize a repository which can access household data for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeHouseholdDataRepository()
        {
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var householdDataRepository = new HouseholdDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(householdDataRepository, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException if the data provider which can access data in the food waste repository is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionIfFoodWasteDataProviderIsNull()
        {
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var exception = Assert.Throws<ArgumentNullException>(() => new HouseholdDataRepository(null, foodWasteObjectMapperMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("foodWasteDataProvider"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException if the object mapper which can map objects in the food waste domain is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionIfFoodWasteObjectMapperIsNull()
        {
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();

            var exception = Assert.Throws<ArgumentNullException>(() => new HouseholdDataRepository(foodWasteDataProviderMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("foodWasteObjectMapper"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that HouseholdMemberGetByMailAddress throws an ArgumentNullException when the mail address is invalid.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestThatHouseholdMemberGetByMailAddressThrowsArgumentNullExceptionWhenMailAddressIsInvalid(string invalidMailAddress)
        {
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var householdDataRepository = new HouseholdDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(householdDataRepository, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdDataRepository.HouseholdMemberGetByMailAddress(invalidMailAddress));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("mailAddress"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that HouseholdMemberGetByMailAddress calls GetCollection on the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberGetByMailAddressCallsGetCollectionOnFoodWasteDataProvider()
        {
            var fixture = new Fixture();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<HouseholdMemberProxy>(Arg<string>.Is.Anything))
                .Return(new List<HouseholdMemberProxy>(0))
                .Repeat.Any();

            var householdDataRepository = new HouseholdDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(householdDataRepository, Is.Not.Null);

            var mailAddress = fixture.Create<string>();
            householdDataRepository.HouseholdMemberGetByMailAddress(mailAddress);

            foodWasteDataProviderMock.AssertWasCalled(m => m.GetCollection<HouseholdMemberProxy>(Arg<string>.Is.Equal(string.Format("SELECT HouseholdMemberIdentifier,MailAddress,Membership,MembershipExpireTime,ActivationCode,ActivationTime,PrivacyPolicyAcceptedTime,CreationTime FROM HouseholdMembers WHERE MailAddress='{0}'", mailAddress))));
        }

        /// <summary>
        /// Tests that HouseholdMemberGetByMailAddress returns household when the household member was found through the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberGetByMailAddressReturnsHouseholdMemberWhenHouseholdMemberWasFoundByFoodWasteDataProvider()
        {
            var fixture = new Fixture();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var householdMemberProxy = new HouseholdMemberProxy();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<HouseholdMemberProxy>(Arg<string>.Is.Anything))
                .Return(new List<HouseholdMemberProxy> {householdMemberProxy})
                .Repeat.Any();

            var householdDataRepository = new HouseholdDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(householdDataRepository, Is.Not.Null);

            var result = householdDataRepository.HouseholdMemberGetByMailAddress(fixture.Create<string>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(householdMemberProxy));
        }

        /// <summary>
        /// Tests that HouseholdMemberGetByMailAddress returns null when the household member was not found through the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberGetByMailAddressReturnsNullWhenHouseholdMemberWasNotFoundByFoodWasteDataProvider()
        {
            var fixture = new Fixture();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<HouseholdMemberProxy>(Arg<string>.Is.Anything))
                .Return(new List<HouseholdMemberProxy>(0))
                .Repeat.Any();

            var householdDataRepository = new HouseholdDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(householdDataRepository, Is.Not.Null);

            var result = householdDataRepository.HouseholdMemberGetByMailAddress(fixture.Create<string>());
            Assert.That(result, Is.Null);
        }

        /// <summary>
        /// Tests that HouseholdMemberGetByMailAddress throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberGetByMailAddressThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs()
        {
            var fixture = new Fixture();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var exceptionToThrow = fixture.Create<IntranetRepositoryException>();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<HouseholdMemberProxy>(Arg<string>.Is.Anything))
                .Throw(exceptionToThrow)
                .Repeat.Any();

            var householdDataRepository = new HouseholdDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(householdDataRepository, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => householdDataRepository.HouseholdMemberGetByMailAddress(fixture.Create<string>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception, Is.EqualTo(exceptionToThrow));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that HouseholdMemberGetByMailAddress throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberGetByMailAddressThrowsIntranetRepositoryExceptionWhenExceptionOccurs()
        {
            var fixture = new Fixture();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var exceptionToThrow = fixture.Create<Exception>();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<HouseholdMemberProxy>(Arg<string>.Is.Anything))
                .Throw(exceptionToThrow)
                .Repeat.Any();

            var householdDataRepository = new HouseholdDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(householdDataRepository, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => householdDataRepository.HouseholdMemberGetByMailAddress(fixture.Create<string>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "HouseholdMemberGetByMailAddress", exceptionToThrow.Message)));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.EqualTo(exceptionToThrow));
        }
    }
}
