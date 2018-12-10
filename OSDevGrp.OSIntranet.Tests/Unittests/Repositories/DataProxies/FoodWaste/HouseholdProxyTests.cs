using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Guards;
using OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Resources;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.DataProxies.FoodWaste
{
    /// <summary>
    /// Test the data proxy to a household.
    /// </summary>
    [TestFixture]
    public class HouseholdProxyTests
    {
        #region Private variables

        private Fixture _fixture;
        private Random _random;

        #endregion

        /// <summary>
        /// Setup each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        /// <summary>
        /// Tests that the constructor initialize a data proxy to a household.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeHouseholdProxy()
        {
            IHouseholdProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);
            Assert.That(sut.Name, Is.Null);
            Assert.That(sut.Description, Is.Null);
            Assert.That(sut.CreationTime, Is.EqualTo(default(DateTime)));
            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Empty);
        }

        /// <summary>
        /// Test that HouseholdMembers maps household members into the proxy when MapData has been called and MapRelations has not been called.
        /// </summary>
        [Test]
        public void TestThatHouseholdMembersMapsHouseholdMembersIntoProxyWhenMapDataHasBeenCalledAndMapRelationsHasNotBeenCalled()
        {
            IHouseholdProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            Guid householdIdentifier = Guid.NewGuid();
            MySqlDataReader dataReader = CreateMySqlDataReader(householdIdentifier);

            IHouseholdMemberProxy[] householdMemberProxyCollection = _fixture.Build<HouseholdMemberProxy>()
                .With(m => m.Identifier, Guid.NewGuid())
                .CreateMany(_random.Next(1, 7))
                .Cast<IHouseholdMemberProxy>()
                .ToArray();
            MemberOfHouseholdProxy[] memberOfHouseholdProxyCollection = BuildMemberOfHouseholdProxyCollection(sut, householdMemberProxyCollection).ToArray();
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(memberOfHouseholdProxyCollection);

            sut.MapData(dataReader, dataProvider);

            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Not.Empty);
            Assert.That(sut.HouseholdMembers, Is.EqualTo(householdMemberProxyCollection));

            dataProvider.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Once());

            // ReSharper disable StringLiteralTypo
            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT moh.MemberOfHouseholdIdentifier,moh.HouseholdMemberIdentifier,hm.MailAddress AS HouseholdMemberMailAddress,hm.Membership AS HouseholdMemberMembership,hm.MembershipExpireTime AS HouseholdMemberMembershipExpireTime,hm.ActivationCode AS HouseholdMemberActivationCode,hm.ActivationTime AS HouseholdMemberActivationTime,hm.PrivacyPolicyAcceptedTime AS HouseholdMemberPrivacyPolicyAcceptedTime,hm.CreationTime AS HouseholdMemberCreationTime,moh.HouseholdIdentifier,h.Name AS HouseholdName,h.Descr AS HouseholdDescr,h.CreationTime AS HouseholdCreationTime,moh.CreationTime FROM MemberOfHouseholds AS moh INNER JOIN HouseholdMembers hm ON hm.HouseholdMemberIdentifier=moh.HouseholdMemberIdentifier INNER JOIN Households h ON h.HouseholdIdentifier=moh.HouseholdIdentifier WHERE moh.HouseholdIdentifier=@householdIdentifier")
            // ReSharper restore StringLiteralTypo
                .AddCharDataParameter("@householdIdentifier", householdIdentifier)
                .Build();
            dataProvider.AssertWasCalled(m => m.GetCollection<MemberOfHouseholdProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Test that HouseholdMembers maps household members into the proxy when Create has been called and MapData has not been called.
        /// </summary>
        [Test]
        public void TestThatHouseholdMembersMapsHouseholdMembersIntoProxyWhenCreateHasBeenCalledAndMapDataHasNotBeenCalled()
        {
            IHouseholdProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            Guid householdIdentifier = Guid.NewGuid();
            MySqlDataReader dataReader = CreateMySqlDataReader(householdIdentifier);

            IHouseholdMemberProxy[] householdMemberProxyCollection = _fixture.Build<HouseholdMemberProxy>()
                .With(m => m.Identifier, Guid.NewGuid())
                .CreateMany(_random.Next(1, 7))
                .Cast<IHouseholdMemberProxy>()
                .ToArray();
            MemberOfHouseholdProxy[] memberOfHouseholdProxyCollection = BuildMemberOfHouseholdProxyCollection(sut, householdMemberProxyCollection).ToArray();
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(memberOfHouseholdProxyCollection);

            // ReSharper disable StringLiteralTypo
            IHouseholdProxy result = sut.Create(dataReader, dataProvider, "HouseholdIdentifier", "Name", "Descr", "CreationTime");
            // ReSharper restore StringLiteralTypo
            Assert.That(result, Is.Not.Null);
            Assert.That(result.HouseholdMembers, Is.Not.Null);
            Assert.That(result.HouseholdMembers, Is.Not.Empty);
            Assert.That(result.HouseholdMembers, Is.EqualTo(householdMemberProxyCollection));

            dataProvider.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Once());

            // ReSharper disable StringLiteralTypo
            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT moh.MemberOfHouseholdIdentifier,moh.HouseholdMemberIdentifier,hm.MailAddress AS HouseholdMemberMailAddress,hm.Membership AS HouseholdMemberMembership,hm.MembershipExpireTime AS HouseholdMemberMembershipExpireTime,hm.ActivationCode AS HouseholdMemberActivationCode,hm.ActivationTime AS HouseholdMemberActivationTime,hm.PrivacyPolicyAcceptedTime AS HouseholdMemberPrivacyPolicyAcceptedTime,hm.CreationTime AS HouseholdMemberCreationTime,moh.HouseholdIdentifier,h.Name AS HouseholdName,h.Descr AS HouseholdDescr,h.CreationTime AS HouseholdCreationTime,moh.CreationTime FROM MemberOfHouseholds AS moh INNER JOIN HouseholdMembers hm ON hm.HouseholdMemberIdentifier=moh.HouseholdMemberIdentifier INNER JOIN Households h ON h.HouseholdIdentifier=moh.HouseholdIdentifier WHERE moh.HouseholdIdentifier=@householdIdentifier")
            // ReSharper restore StringLiteralTypo
                .AddCharDataParameter("@householdIdentifier", householdIdentifier)
                .Build();
            dataProvider.AssertWasCalled(m => m.GetCollection<MemberOfHouseholdProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that getter for UniqueId throws an IntranetRepositoryException when the household has no identifier.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterThrowsIntranetRepositoryExceptionWhenHouseholdProxyHasNoIdentifier()
        {
            IHouseholdProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => { sut.UniqueId.ToUpper(); });
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, sut.Identifier, "Identifier");
        }

        /// <summary>
        /// Tests that getter for UniqueId gets the unique identifier for the household.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterGetsUniqueIdentificationForHouseholdProxy()
        {
            Guid identifier = Guid.NewGuid();

            IHouseholdProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Not.Null);
            Assert.That(sut.Identifier, Is.EqualTo(identifier));

            string uniqueId = sut.UniqueId;
            Assert.That(uniqueId, Is.Not.Null);
            Assert.That(uniqueId, Is.Not.Empty);
            Assert.That(uniqueId, Is.EqualTo(identifier.ToString("D").ToUpper()));
        }

        /// <summary>
        /// Tests that MapData throws an ArgumentNullException if the data reader is null.
        /// </summary>
        [Test]
        public void TestThatMapDataThrowsArgumentNullExceptionIfDataReaderIsNull()
        {
            IHouseholdProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.MapData(null, CreateFoodWasteDataProvider()));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataReader");
        }

        /// <summary>
        /// Tests that MapData throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatMapDataThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            IHouseholdProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.MapData(CreateMySqlDataReader(), null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that MapData maps data into the proxy.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatMapDataMapsDataIntoProxy(bool hasDescription)
        {
            IHouseholdProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            Guid identifier = Guid.NewGuid();
            string name = _fixture.Create<string>();
            string description = hasDescription ? _fixture.Create<string>() : null;
            DateTime creationTime = DateTime.Now;
            MySqlDataReader dataReader = CreateMySqlDataReader(identifier, name, description, creationTime);

            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider();

            sut.MapData(dataReader, dataProvider);

            Assert.That(sut.Identifier, Is.Not.Null);
            Assert.That(sut.Identifier, Is.EqualTo(identifier));
            Assert.That(sut.Name, Is.Not.Null);
            Assert.That(sut.Name, Is.Not.Empty);
            Assert.That(sut.Name, Is.EqualTo(name));
            if (string.IsNullOrWhiteSpace(description) == false)
            {
                Assert.That(sut.Description, Is.Not.Null);
                Assert.That(sut.Description, Is.Not.Empty);
                Assert.That(sut.Description, Is.EqualTo(description));
            }
            else
            {
                Assert.That(sut.Description, Is.Null);
            }
            Assert.That(sut.CreationTime, Is.EqualTo(creationTime));

            dataReader.AssertWasCalled(m => m.GetString(Arg<string>.Is.Equal("HouseholdIdentifier")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetString(Arg<string>.Is.Equal("Name")), opt => opt.Repeat.Once());
            // ReSharper disable StringLiteralTypo
            dataReader.AssertWasCalled(m => m.GetOrdinal(Arg<string>.Is.Equal("Descr")), opt => opt.Repeat.Once());
            // ReSharper restore StringLiteralTypo
            dataReader.AssertWasCalled(m => m.IsDBNull(Arg<int>.Is.Equal(2)), opt => opt.Repeat.Once());
            if (string.IsNullOrWhiteSpace(description) == false)
            {
                dataReader.AssertWasCalled(m => m.GetString(Arg<int>.Is.Equal(2)), opt => opt.Repeat.Once());
            }
            else
            {
                dataReader.AssertWasNotCalled(m => m.GetString(Arg<int>.Is.Equal(2)));
            }
            dataReader.AssertWasCalled(m => m.GetDateTime(Arg<string>.Is.Equal("CreationTime")), opt => opt.Repeat.Once());

            dataProvider.AssertWasNotCalled(m => m.Clone());
        }

        /// <summary>
        /// Tests that MapRelations throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatMapRelationsThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            IHouseholdProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.MapRelations(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that MapRelations does not clone the data provider.
        /// </summary>
        [Test]
        public void TestThatMapRelationsDoesNotCloneDataProvider()
        {
            IHouseholdProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider();

            sut.MapRelations(dataProvider);

            dataProvider.AssertWasNotCalled(m => m.Clone());
        }

        /// <summary>
        /// Tests that SaveRelations throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            IHouseholdProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.SaveRelations(null, _fixture.Create<bool>()));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that SaveRelations throws an IntranetRepositoryException when the identifier for the household is null.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsThrowsIntranetRepositoryExceptionWhenIdentifierIsNull()
        {
            IHouseholdProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.SaveRelations(CreateFoodWasteDataProvider(), _fixture.Create<bool>()));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, sut.Identifier, "Identifier");
        }

        /// <summary>
        /// Tests that SaveRelations throws an IntranetRepositoryException when one of the household members has an identifier equal to null.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsThrowsIntranetRepositoryExceptionWhenOneHouseholdMemberIdentifierIsNull()
        {
            Guid householdIdentifier = Guid.NewGuid();

            IHouseholdProxy sut = CreateSut(householdIdentifier);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Empty);

            HouseholdMemberProxy householdMemberProxy = BuildHouseholdMemberProxy(false);

            sut.HouseholdMemberAdd(householdMemberProxy);
            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Not.Empty);
            Assert.That(sut.HouseholdMembers.Contains(householdMemberProxy), Is.True);

            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider();

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.SaveRelations(dataProvider, _fixture.Create<bool>()));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, householdMemberProxy.Identifier, "Identifier");
        }

        /// <summary>
        /// Tests that SaveRelations inserts the missing bindings between the given household and who is member of the household.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsInsertsMissingMemberOfHouseholds()
        {
            Guid householdIdentifier = Guid.NewGuid();

            IHouseholdProxy sut = CreateSut(householdIdentifier);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Empty);

            Guid householdMemberIdentifier = Guid.NewGuid();
            HouseholdMemberProxy householdMemberProxy = BuildHouseholdMemberProxy(identifier: householdMemberIdentifier);

            sut.HouseholdMemberAdd(householdMemberProxy);
            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Not.Empty);
            Assert.That(sut.HouseholdMembers.Contains(householdMemberProxy), Is.True);

            IEnumerable<MemberOfHouseholdProxy> memberOfHouseholdProxyCollection = BuildMemberOfHouseholdProxyCollection(sut);
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(memberOfHouseholdProxyCollection);

            sut.SaveRelations(dataProvider, _fixture.Create<bool>());

            // ReSharper disable StringLiteralTypo
            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT moh.MemberOfHouseholdIdentifier,moh.HouseholdMemberIdentifier,hm.MailAddress AS HouseholdMemberMailAddress,hm.Membership AS HouseholdMemberMembership,hm.MembershipExpireTime AS HouseholdMemberMembershipExpireTime,hm.ActivationCode AS HouseholdMemberActivationCode,hm.ActivationTime AS HouseholdMemberActivationTime,hm.PrivacyPolicyAcceptedTime AS HouseholdMemberPrivacyPolicyAcceptedTime,hm.CreationTime AS HouseholdMemberCreationTime,moh.HouseholdIdentifier,h.Name AS HouseholdName,h.Descr AS HouseholdDescr,h.CreationTime AS HouseholdCreationTime,moh.CreationTime FROM MemberOfHouseholds AS moh INNER JOIN HouseholdMembers hm ON hm.HouseholdMemberIdentifier=moh.HouseholdMemberIdentifier INNER JOIN Households h ON h.HouseholdIdentifier=moh.HouseholdIdentifier WHERE moh.HouseholdIdentifier=@householdIdentifier")
            // ReSharper restore StringLiteralTypo
                .AddCharDataParameter("@householdIdentifier", householdIdentifier)
                .Build();
            dataProvider.AssertWasCalled(m => m.GetCollection<MemberOfHouseholdProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());

            dataProvider.AssertWasCalled(m => m.Add(Arg<MemberOfHouseholdProxy>.Matches(proxy =>
                    proxy != null && proxy.Identifier != null &&
                    proxy.HouseholdMember != null && proxy.HouseholdMember == householdMemberProxy &&
                    proxy.HouseholdMemberIdentifier != null && proxy.HouseholdMemberIdentifier == householdMemberIdentifier &&
                    proxy.Household != null && proxy.Household == sut &&
                    proxy.HouseholdIdentifier != null && proxy.HouseholdIdentifier == householdIdentifier &&
                    proxy.CreationTime >= DateTime.Now.AddSeconds(-3) && proxy.CreationTime <= DateTime.Now.AddSeconds(3)))
                , opt => opt.Repeat.Once());

            dataProvider.AssertWasNotCalled(m => m.Delete(Arg<MemberOfHouseholdProxy>.Is.Anything));
        }

        /// <summary>
        /// Tests that SaveRelations does not inserts the existing bindings between the given household and who is member of the household.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsDoesNotInsertsExistingMemberOfHouseholds()
        {
            Guid householdIdentifier = Guid.NewGuid();

            IHouseholdProxy sut = CreateSut(householdIdentifier);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Empty);

            Guid householdMemberIdentifier = Guid.NewGuid();
            HouseholdMemberProxy householdMemberProxy = BuildHouseholdMemberProxy(identifier: householdMemberIdentifier);

            sut.HouseholdMemberAdd(householdMemberProxy);
            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Not.Empty);
            Assert.That(sut.HouseholdMembers.Contains(householdMemberProxy), Is.True);

            IEnumerable<MemberOfHouseholdProxy> memberOfHouseholdProxyCollection = BuildMemberOfHouseholdProxyCollection(sut, householdMemberProxy);
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(memberOfHouseholdProxyCollection);

            sut.SaveRelations(dataProvider, _fixture.Create<bool>());

            dataProvider.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Once());

            // ReSharper disable StringLiteralTypo
            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT moh.MemberOfHouseholdIdentifier,moh.HouseholdMemberIdentifier,hm.MailAddress AS HouseholdMemberMailAddress,hm.Membership AS HouseholdMemberMembership,hm.MembershipExpireTime AS HouseholdMemberMembershipExpireTime,hm.ActivationCode AS HouseholdMemberActivationCode,hm.ActivationTime AS HouseholdMemberActivationTime,hm.PrivacyPolicyAcceptedTime AS HouseholdMemberPrivacyPolicyAcceptedTime,hm.CreationTime AS HouseholdMemberCreationTime,moh.HouseholdIdentifier,h.Name AS HouseholdName,h.Descr AS HouseholdDescr,h.CreationTime AS HouseholdCreationTime,moh.CreationTime FROM MemberOfHouseholds AS moh INNER JOIN HouseholdMembers hm ON hm.HouseholdMemberIdentifier=moh.HouseholdMemberIdentifier INNER JOIN Households h ON h.HouseholdIdentifier=moh.HouseholdIdentifier WHERE moh.HouseholdIdentifier=@householdIdentifier")
            // ReSharper restore StringLiteralTypo
                .AddCharDataParameter("@householdIdentifier", householdIdentifier)
                .Build();
            dataProvider.AssertWasCalled(m => m.GetCollection<MemberOfHouseholdProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());

            dataProvider.AssertWasNotCalled(m => m.Add(Arg<MemberOfHouseholdProxy>.Is.Anything));
            dataProvider.AssertWasNotCalled(m => m.Delete(Arg<MemberOfHouseholdProxy>.Is.Anything));
        }

        /// <summary>
        /// Tests that SaveRelations deletes the removed bindings between the given household and who is member of the household.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsDeletesRemovedMemberOfHouseholds()
        {
            Guid householdIdentifier = Guid.NewGuid();

            IHouseholdProxy sut = CreateSut(householdIdentifier);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Empty);

            Guid householdMemberIdentifier = Guid.NewGuid();
            HouseholdMemberProxy householdMemberProxy = BuildHouseholdMemberProxy(identifier: householdMemberIdentifier);

            sut.HouseholdMemberAdd(householdMemberProxy);
            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Not.Empty);
            Assert.That(sut.HouseholdMembers.Contains(householdMemberProxy), Is.True);

            IEnumerable<MemberOfHouseholdProxy> memberOfHouseholdProxyCollection = BuildMemberOfHouseholdProxyCollection(sut, householdMemberProxy);
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(memberOfHouseholdProxyCollection);

            sut.SaveRelations(dataProvider, _fixture.Create<bool>());

            sut.HouseholdMemberRemove(householdMemberProxy);
            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Empty);

            sut.SaveRelations(dataProvider, _fixture.Create<bool>());

            dataProvider.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Times(5));

            // ReSharper disable StringLiteralTypo
            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT moh.MemberOfHouseholdIdentifier,moh.HouseholdMemberIdentifier,hm.MailAddress AS HouseholdMemberMailAddress,hm.Membership AS HouseholdMemberMembership,hm.MembershipExpireTime AS HouseholdMemberMembershipExpireTime,hm.ActivationCode AS HouseholdMemberActivationCode,hm.ActivationTime AS HouseholdMemberActivationTime,hm.PrivacyPolicyAcceptedTime AS HouseholdMemberPrivacyPolicyAcceptedTime,hm.CreationTime AS HouseholdMemberCreationTime,moh.HouseholdIdentifier,h.Name AS HouseholdName,h.Descr AS HouseholdDescr,h.CreationTime AS HouseholdCreationTime,moh.CreationTime FROM MemberOfHouseholds AS moh INNER JOIN HouseholdMembers hm ON hm.HouseholdMemberIdentifier=moh.HouseholdMemberIdentifier INNER JOIN Households h ON h.HouseholdIdentifier=moh.HouseholdIdentifier WHERE moh.HouseholdIdentifier=@householdIdentifier")
            // ReSharper restore StringLiteralTypo
                .AddCharDataParameter("@householdIdentifier", householdIdentifier)
                .Build();
            dataProvider.AssertWasCalled(m => m.GetCollection<MemberOfHouseholdProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Times(3));

            dataProvider.AssertWasNotCalled(m => m.Add(Arg<MemberOfHouseholdProxy>.Is.Anything));

            dataProvider.AssertWasCalled(m => m.Delete(Arg<MemberOfHouseholdProxy>.Matches(proxy =>
                    proxy != null && proxy.Identifier != null &&
                    proxy.HouseholdMember != null && proxy.HouseholdMember == householdMemberProxy &&
                    proxy.HouseholdMemberIdentifier != null && proxy.HouseholdMemberIdentifier == householdMemberIdentifier &&
                    proxy.Household != null && proxy.Household == sut &&
                    proxy.HouseholdIdentifier != null && proxy.HouseholdIdentifier == householdIdentifier &&
                    proxy.CreationTime >= DateTime.Now.AddSeconds(-3) && proxy.CreationTime <= DateTime.Now.AddSeconds(3))),
                opt => opt.Repeat.Once());

            dataProvider.AssertWasCalled(m => m.Delete(Arg<IHouseholdMemberProxy>.Is.Equal(householdMemberProxy)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that DeleteRelations throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            IHouseholdProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.DeleteRelations(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that DeleteRelations throws an IntranetRepositoryException when the identifier for the household is null.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsThrowsIntranetRepositoryExceptionWhenIdentifierIsNull()
        {
            IFoodWasteDataProvider dataProviderMock = CreateFoodWasteDataProvider();

            IHouseholdProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.DeleteRelations(dataProviderMock));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, sut.Identifier, "Identifier");
        }

        /// <summary>
        /// Tests that DeleteRelations calls Clone on the data provider one time.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsCallsCloneOnDataProviderOneTime()
        {
            Guid identifier = Guid.NewGuid();

            IHouseholdProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            IEnumerable<MemberOfHouseholdProxy> memberOfHouseholdProxyCollection = BuildMemberOfHouseholdProxyCollection(sut);
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(memberOfHouseholdProxyCollection);

            sut.DeleteRelations(dataProvider);

            dataProvider.AssertWasCalled(m => m.Clone());
        }

        /// <summary>
        /// Tests that DeleteRelations calls GetCollection on the data provider to get the bindings which bind a given household to all the household members who has membership.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsCallsGetCollectionOnDataProviderToGetMemberOfHouseholdProxies()
        {
            Guid identifier = Guid.NewGuid();

            IHouseholdProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            IEnumerable<MemberOfHouseholdProxy> memberOfHouseholdProxyCollection = BuildMemberOfHouseholdProxyCollection(sut);
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(memberOfHouseholdProxyCollection);

            sut.DeleteRelations(dataProvider);

            // ReSharper disable StringLiteralTypo
            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT moh.MemberOfHouseholdIdentifier,moh.HouseholdMemberIdentifier,hm.MailAddress AS HouseholdMemberMailAddress,hm.Membership AS HouseholdMemberMembership,hm.MembershipExpireTime AS HouseholdMemberMembershipExpireTime,hm.ActivationCode AS HouseholdMemberActivationCode,hm.ActivationTime AS HouseholdMemberActivationTime,hm.PrivacyPolicyAcceptedTime AS HouseholdMemberPrivacyPolicyAcceptedTime,hm.CreationTime AS HouseholdMemberCreationTime,moh.HouseholdIdentifier,h.Name AS HouseholdName,h.Descr AS HouseholdDescr,h.CreationTime AS HouseholdCreationTime,moh.CreationTime FROM MemberOfHouseholds AS moh INNER JOIN HouseholdMembers hm ON hm.HouseholdMemberIdentifier=moh.HouseholdMemberIdentifier INNER JOIN Households h ON h.HouseholdIdentifier=moh.HouseholdIdentifier WHERE moh.HouseholdIdentifier=@householdIdentifier")
            // ReSharper restore StringLiteralTypo
                .AddCharDataParameter("@householdIdentifier", identifier)
                .Build();
            dataProvider.AssertWasCalled(m => m.GetCollection<MemberOfHouseholdProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that DeleteRelations calls Delete on the data provider for each binding which bind a given household to all the household members who has membership.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsCallsDeleteOnDataProviderForEachMemberOfHouseholdProxy()
        {
            Guid identifier = Guid.NewGuid();

            IHouseholdProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            IHouseholdMemberProxy[] householdMemberProxyCollection = _fixture.Build<HouseholdMemberProxy>()
                .With(m => m.Identifier, Guid.NewGuid())
                .CreateMany(_random.Next(1, 7))
                .Cast<IHouseholdMemberProxy>()
                .ToArray();
            MemberOfHouseholdProxy[] memberOfHouseholdProxyCollection = BuildMemberOfHouseholdProxyCollection(sut, householdMemberProxyCollection).ToArray();
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(memberOfHouseholdProxyCollection);

            sut.DeleteRelations(dataProvider);

            dataProvider.AssertWasCalled(m => m.Delete(Arg<MemberOfHouseholdProxy>.Is.NotNull), opt => opt.Repeat.Times(memberOfHouseholdProxyCollection.Length));
            dataProvider.AssertWasCalled(m => m.Delete(Arg<IHouseholdMemberProxy>.Is.NotNull), opt => opt.Repeat.Times(householdMemberProxyCollection.Length));
        }

        /// <summary>
        /// Tests that CreateGetCommand returns the SQL command for selecting the given household.
        /// </summary>
        [Test]
        public void TestThatCreateGetCommandReturnsSqlCommand()
        {
            Guid identifier = Guid.NewGuid();

            IHouseholdProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            // ReSharper disable StringLiteralTypo
            new DbCommandTestBuilder("SELECT HouseholdIdentifier,Name,Descr,CreationTime FROM Households WHERE HouseholdIdentifier=@householdIdentifier")
            // ReSharper restore StringLiteralTypo
                .AddCharDataParameter("@householdIdentifier", identifier)
                .Build()
                .Run(sut.CreateGetCommand());
        }

        /// <summary>
        /// Tests that CreateInsertCommand returns the SQL command to insert a household.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatCreateInsertCommandReturnsSqlCommandForInsert(bool hasDescription)
        {
            Guid identifier = Guid.NewGuid();
            string name = _fixture.Create<string>();
            string description = hasDescription ? _fixture.Create<string>() : null;
            DateTime creationTime = DateTime.Now;

            IHouseholdProxy sut = CreateSut(identifier, name, description, creationTime);
            Assert.That(sut, Is.Not.Null);

            // ReSharper disable StringLiteralTypo
            new DbCommandTestBuilder("INSERT INTO Households (HouseholdIdentifier,Name,Descr,CreationTime) VALUES(@householdIdentifier,@name,@descr,@creationTime)")
            // ReSharper restore StringLiteralTypo
                .AddCharDataParameter("@householdIdentifier", identifier)
                .AddVarCharDataParameter("@name", name, 64)
                // ReSharper disable StringLiteralTypo
                .AddVarCharDataParameter("@descr", description, 2048, true)
                // ReSharper restore StringLiteralTypo
                .AddDateTimeDataParameter("@creationTime", creationTime.ToUniversalTime())
                .Build()
                .Run(sut.CreateInsertCommand());
        }

        /// <summary>
        /// Tests that CreateUpdateCommand returns the SQL command to update a household.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatCreateUpdateCommandReturnsSqlCommandForUpdate(bool hasDescription)
        {
            Guid identifier = Guid.NewGuid();
            string name = _fixture.Create<string>();
            string description = hasDescription ? _fixture.Create<string>() : null;
            DateTime creationTime = DateTime.Now;

            IHouseholdProxy sut = CreateSut(identifier, name, description, creationTime);
            Assert.That(sut, Is.Not.Null);

            // ReSharper disable StringLiteralTypo
            new DbCommandTestBuilder("UPDATE Households SET Name=@name,Descr=@descr,CreationTime=@creationTime WHERE HouseholdIdentifier=@householdIdentifier")
            // ReSharper restore StringLiteralTypo
                .AddCharDataParameter("@householdIdentifier", identifier)
                .AddVarCharDataParameter("@name", name, 64)
                // ReSharper disable StringLiteralTypo
                .AddVarCharDataParameter("@descr", description, 2048, true)
                // ReSharper restore StringLiteralTypo
                .AddDateTimeDataParameter("@creationTime", creationTime.ToUniversalTime())
                .Build()
                .Run(sut.CreateUpdateCommand());
        }

        /// <summary>
        /// Tests that CreateDeleteCommand returns the SQL statement to delete a household.
        /// </summary>
        [Test]
        public void TestThatCreateDeleteCommandReturnsSqlCommandForDelete()
        {
            Guid identifier = Guid.NewGuid();

            IHouseholdProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("DELETE FROM Households WHERE HouseholdIdentifier=@householdIdentifier")
                .AddCharDataParameter("@householdIdentifier", identifier)
                .Build()
                .Run(sut.CreateDeleteCommand());
        }

        /// <summary>
        /// Tests that Create throws an ArgumentNullException if the data reader is null.
        /// </summary>
        [Test]
        public void TestThatCreateThrowsArgumentNullExceptionIfDataReaderIsNull()
        {
            IHouseholdProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Create(null, CreateFoodWasteDataProvider(), _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>()));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataReader");
        }

        /// <summary>
        /// Tests that Create throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatCreateThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            IHouseholdProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Create(CreateMySqlDataReader(), null, _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>()));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that Create throws an ArgumentNullException if the column name collection is null.
        /// </summary>
        [Test]
        public void TestThatCreateThrowsArgumentNullExceptionIfColumnNameCollectionIsNull()
        {
            IHouseholdProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Create(CreateMySqlDataReader(), CreateFoodWasteDataProvider(), null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "columnNameCollection");
        }

        /// <summary>
        /// Tests that Create creates a data proxy to a given household member with values from the data reader.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatCreateCreatesProxy(bool hasDescription)
        {
            IHouseholdProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            Guid identifier = Guid.NewGuid();
            string name = _fixture.Create<string>();
            string description = hasDescription ? _fixture.Create<string>() : null;
            DateTime creationTime = DateTime.Now;
            MySqlDataReader dataReader = CreateMySqlDataReader(identifier, name, description, creationTime);

            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider();

            // ReSharper disable StringLiteralTypo
            IHouseholdProxy result = sut.Create(dataReader, dataProvider, "HouseholdIdentifier", "Name", "Descr", "CreationTime");
            // ReSharper restore StringLiteralTypo
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Identifier, Is.Not.Null);
            Assert.That(result.Identifier, Is.EqualTo(identifier));
            Assert.That(result.Name, Is.Not.Null);
            Assert.That(result.Name, Is.Not.Empty);
            Assert.That(result.Name, Is.EqualTo(name));
            if (string.IsNullOrWhiteSpace(description) == false)
            {
                Assert.That(result.Description, Is.Not.Null);
                Assert.That(result.Description, Is.Not.Empty);
                Assert.That(result.Description, Is.EqualTo(description));
            }
            else
            {
                Assert.That(result.Description, Is.Null);
            }
            Assert.That(result.CreationTime, Is.EqualTo(creationTime));

            dataReader.AssertWasCalled(m => m.GetString(Arg<string>.Is.Equal("HouseholdIdentifier")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetString(Arg<string>.Is.Equal("Name")), opt => opt.Repeat.Once());
            // ReSharper disable StringLiteralTypo
            dataReader.AssertWasCalled(m => m.GetOrdinal(Arg<string>.Is.Equal("Descr")), opt => opt.Repeat.Once());
            // ReSharper restore StringLiteralTypo
            dataReader.AssertWasCalled(m => m.IsDBNull(Arg<int>.Is.Equal(2)), opt => opt.Repeat.Once());
            if (string.IsNullOrWhiteSpace(description) == false)
            {
                dataReader.AssertWasCalled(m => m.GetString(Arg<int>.Is.Equal(2)), opt => opt.Repeat.Once());
            }
            else
            {
                dataReader.AssertWasNotCalled(m => m.GetString(Arg<int>.Is.Equal(2)));
            }
            dataReader.AssertWasCalled(m => m.GetDateTime(Arg<string>.Is.Equal("CreationTime")), opt => opt.Repeat.Once());

            dataProvider.AssertWasNotCalled(m => m.Clone());
        }

        /// <summary>
        /// Creates an instance of the data proxy to a given household which should be used for unit testing.
        /// </summary>
        /// <returns>Instance of the data proxy to a given household which should be used for unit testing.</returns>
        private IHouseholdProxy CreateSut(Guid? householdIdentifier = null)
        {
            return new HouseholdProxy
            {
                Identifier = householdIdentifier
            };
        }

        /// <summary>
        /// Creates an instance of the data proxy to a given household which should be used for unit testing.
        /// </summary>
        /// <returns>Instance of the data proxy to a given household which should be used for unit testing.</returns>
        private IHouseholdProxy CreateSut(Guid householdIdentifier, string name, string description, DateTime creationTime)
        {
            return new HouseholdProxy(name, description, creationTime)
            {
                Identifier = householdIdentifier
            };
        }

        /// <summary>
        /// Creates an instance of a MySQL data reader which should be used for unit testing.
        /// </summary>
        /// <returns>Instance of a MySQL data reader which should be used for unit testing.</returns>
        private MySqlDataReader CreateMySqlDataReader(Guid? householdIdentifier = null, string name = null, string description = null, DateTime? creationTime = null)
        {
            MySqlDataReader mySqlDataReaderStub = MockRepository.GenerateStub<MySqlDataReader>();
            mySqlDataReaderStub.Stub(m => m.GetString(Arg<string>.Is.Equal("HouseholdIdentifier")))
                .Return(householdIdentifier.HasValue ? householdIdentifier.Value.ToString("D").ToUpper() : Guid.NewGuid().ToString("D").ToUpper())
                .Repeat.Any();
            mySqlDataReaderStub.Stub(m => m.GetString(Arg<string>.Is.Equal("Name")))
                .Return(name ?? _fixture.Create<string>())
                .Repeat.Any();
            // ReSharper disable StringLiteralTypo
            mySqlDataReaderStub.Stub(m => m.GetOrdinal(Arg<string>.Is.Equal("Descr")))
            // ReSharper restore StringLiteralTypo
                .Return(2)
                .Repeat.Any();
            mySqlDataReaderStub.Stub(m => m.IsDBNull(Arg<int>.Is.Equal(2)))
                .Return(string.IsNullOrWhiteSpace(description))
                .Repeat.Any();
            mySqlDataReaderStub.Stub(m => m.GetString(Arg<int>.Is.Equal(2)))
                .Return(description ?? _fixture.Create<string>())
                .Repeat.Any();
            mySqlDataReaderStub.Stub(m => m.GetDateTime(Arg<string>.Is.Equal("CreationTime")))
                .Return((creationTime ?? DateTime.Now).ToUniversalTime())
                .Repeat.Any();
            return mySqlDataReaderStub;
        }

        /// <summary>
        /// Creates an instance of a data provider which should be used for unit testing.
        /// </summary>
        /// <returns>Instance of a data provider which should be used for unit testing</returns>
        private IFoodWasteDataProvider CreateFoodWasteDataProvider(IEnumerable<MemberOfHouseholdProxy> memberOfHouseholdProxyCollection = null)
        {
            IFoodWasteDataProvider foodWasteDataProvider = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProvider.Stub(m => m.Clone())
                .Return(foodWasteDataProvider)
                .Repeat.Any();
            foodWasteDataProvider.Stub(m => m.GetCollection<MemberOfHouseholdProxy>(Arg<MySqlCommand>.Is.Anything))
                .Return(memberOfHouseholdProxyCollection ?? BuildMemberOfHouseholdProxyCollection(CreateSut(Guid.NewGuid())))
                .Repeat.Any();
            foodWasteDataProvider.Stub(m => m.Add(Arg<MemberOfHouseholdProxy>.Is.TypeOf))
                .WhenCalled(e => e.ReturnValue = (MemberOfHouseholdProxy) e.Arguments.ElementAt(0))
                .Return(null)
                .Repeat.Any();
            return foodWasteDataProvider;
        }

        /// <summary>
        /// Creates a data proxy to a household member.
        /// </summary>
        /// <param name="hasIdentifier">Indicates whether the data proxy has an identifier.</param>
        /// <param name="identifier">The identifier for the data proxy.</param>
        /// <returns>Data proxy to a household member.</returns>
        private HouseholdMemberProxy BuildHouseholdMemberProxy(bool hasIdentifier = true, Guid? identifier = null)
        {
            return new HouseholdMemberProxy
            {
                Identifier = hasIdentifier ? identifier ?? Guid.NewGuid() : (Guid?) null
            };
        }

        /// <summary>
        /// Creates a collection of data proxies which bind a given household member to some households.
        /// </summary>
        /// <returns>Collection of data proxies which bind a given household member to some households.</returns>
        private IEnumerable<MemberOfHouseholdProxy> BuildMemberOfHouseholdProxyCollection(IHouseholdProxy householdProxy, params IHouseholdMemberProxy[] householdMemberProxyCollection)
        {
            ArgumentNullGuard.NotNull(householdProxy, nameof(householdProxy));

            if (householdMemberProxyCollection == null || householdMemberProxyCollection.Any() == false)
            {
                return new List<MemberOfHouseholdProxy>(0);
            }

            return householdMemberProxyCollection
                .Select(householdMemberProxy => new MemberOfHouseholdProxy(householdMemberProxy, householdProxy, DateTime.Now) {Identifier = Guid.NewGuid()})
                .ToList();
        }
    }
}
