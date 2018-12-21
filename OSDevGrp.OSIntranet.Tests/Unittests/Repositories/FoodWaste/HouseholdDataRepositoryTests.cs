using System;
using System.Collections.Generic;
using AutoFixture;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Resources;
using OSDevGrp.OSIntranet.Tests.Unittests.Repositories.DataProxies;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.FoodWaste
{
    /// <summary>
    /// Tests the repository which can access household data for the food waste domain.
    /// </summary>
    [TestFixture]
    public class HouseholdDataRepositoryTests
    {
        #region Private variables

        private IFoodWasteDataProvider _foodWasteDataProviderMock;
        private IFoodWasteObjectMapper _foodWasteObjectMapperMock;
        private Fixture _fixture;

        #endregion

        /// <summary>
        /// Setup each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            _foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            _fixture = new Fixture();
        }

        /// <summary>
        /// Tests that the constructor initialize a repository which can access household data for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeHouseholdDataRepository()
        {
            IHouseholdDataRepository sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException if the data provider which can access data in the food waste repository is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionIfFoodWasteDataProviderIsNull()
        {
            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => new HouseholdDataRepository(null, _foodWasteObjectMapperMock));
            // ReSharper restore ObjectCreationAsStatement

            TestHelper.AssertArgumentNullExceptionIsValid(result, "foodWasteDataProvider");
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException if the object mapper which can map objects in the food waste domain is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionIfFoodWasteObjectMapperIsNull()
        {
            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException result =  Assert.Throws<ArgumentNullException>(() => new HouseholdDataRepository(_foodWasteDataProviderMock, null));
            // ReSharper restore ObjectCreationAsStatement

            TestHelper.AssertArgumentNullExceptionIsValid(result, "foodWasteObjectMapper");
        }

        /// <summary>
        /// Tests that HouseholdMemberGetByMailAddress throws an ArgumentNullException when the mail address is invalid.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("  ")]
        public void TestThatHouseholdMemberGetByMailAddressThrowsArgumentNullExceptionWhenMailAddressIsInvalid(string invalidMailAddress)
        {
            IHouseholdDataRepository sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.HouseholdMemberGetByMailAddress(invalidMailAddress));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "mailAddress");
        }

        /// <summary>
        /// Tests that HouseholdMemberGetByMailAddress calls GetCollection on the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberGetByMailAddressCallsGetCollectionOnFoodWasteDataProvider()
        {
            IHouseholdDataRepository sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            string mailAddress = _fixture.Create<string>();
            sut.HouseholdMemberGetByMailAddress(mailAddress);

            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT HouseholdMemberIdentifier,MailAddress,Membership,MembershipExpireTime,ActivationCode,ActivationTime,PrivacyPolicyAcceptedTime,CreationTime FROM HouseholdMembers WHERE MailAddress=@mailAddress")
                .AddVarCharDataParameter("@mailAddress", mailAddress, 128)
                .Build();
            _foodWasteDataProviderMock.AssertWasCalled(m => m.GetCollection<HouseholdMemberProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that HouseholdMemberGetByMailAddress returns household when the household member was found through the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberGetByMailAddressReturnsHouseholdMemberWhenHouseholdMemberWasFoundByFoodWasteDataProvider()
        {
            HouseholdMemberProxy householdMemberProxy = new HouseholdMemberProxy();

            IHouseholdDataRepository sut = CreateSut(new List<HouseholdMemberProxy> {householdMemberProxy});
            Assert.That(sut, Is.Not.Null);

            IHouseholdMember result = sut.HouseholdMemberGetByMailAddress(_fixture.Create<string>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(householdMemberProxy));
        }

        /// <summary>
        /// Tests that HouseholdMemberGetByMailAddress returns null when the household member was not found through the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberGetByMailAddressReturnsNullWhenHouseholdMemberWasNotFoundByFoodWasteDataProvider()
        {
            IHouseholdDataRepository sut = CreateSut(new List<HouseholdMemberProxy>(0));
            Assert.That(sut, Is.Not.Null);

            IHouseholdMember result = sut.HouseholdMemberGetByMailAddress(_fixture.Create<string>());
            Assert.That(result, Is.Null);
        }

        /// <summary>
        /// Tests that HouseholdMemberGetByMailAddress throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberGetByMailAddressThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs()
        {
            IntranetRepositoryException exceptionToThrow = _fixture.Create<IntranetRepositoryException>();

            IHouseholdDataRepository sut = CreateSut(exception: exceptionToThrow);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.HouseholdMemberGetByMailAddress(_fixture.Create<string>()));
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that HouseholdMemberGetByMailAddress throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberGetByMailAddressThrowsIntranetRepositoryExceptionWhenExceptionOccurs()
        {
            Exception exceptionToThrow = _fixture.Create<Exception>();

            IHouseholdDataRepository sut = CreateSut(exception: exceptionToThrow);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.HouseholdMemberGetByMailAddress(_fixture.Create<string>()));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, exceptionToThrow, ExceptionMessage.RepositoryError, "HouseholdMemberGetByMailAddress", exceptionToThrow.Message);
        }

        /// <summary>
        /// Creates an instance of the <see cref="HouseholdDataRepository"/> for unit testing.
        /// </summary>
        /// <returns>Instance of the <see cref="HouseholdDataRepository"/> for unit testing.</returns>
        private IHouseholdDataRepository CreateSut(IEnumerable<HouseholdMemberProxy> householdMemberProxyCollection = null, Exception exception = null)
        {
            if (exception == null)
            {
                _foodWasteDataProviderMock.Stub(m => m.GetCollection<HouseholdMemberProxy>(Arg<MySqlCommand>.Is.Anything))
                    .Return(householdMemberProxyCollection ?? new List<HouseholdMemberProxy>(0))
                    .Repeat.Any();
            }
            else
            {
                _foodWasteDataProviderMock.Stub(m => m.GetCollection<HouseholdMemberProxy>(Arg<MySqlCommand>.Is.Anything))
                    .Throw(exception)
                    .Repeat.Any();
            }

            return new HouseholdDataRepository(_foodWasteDataProviderMock, _foodWasteObjectMapperMock);
        }
    }
}
