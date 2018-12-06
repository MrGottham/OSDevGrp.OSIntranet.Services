using System;
using AutoFixture;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Resources;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.DataProxies.FoodWaste
{
    /// <summary>
    /// Test the data proxy which bind a given household member to a given household.
    /// </summary>
    [TestFixture]
    public class MemberOfHouseholdProxyTests
    {
        #region Private variables

        private Fixture _fixture;

        #endregion

        /// <summary>
        /// Setup each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        /// <summary>
        /// Tests that the constructor without a household member and a household initialize a data proxy which bind a given household member to a given household.
        /// </summary>
        [Test]
        public void TestThatConstructorWithoutHouseholdMemberAndHouseholdInitializeMemberOfHouseholdProxy()
        {
            IMemberOfHouseholdProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);
            Assert.That(sut.HouseholdMember, Is.Null);
            Assert.That(sut.HouseholdMemberIdentifier, Is.Null);
            Assert.That(sut.HouseholdMemberIdentifier.HasValue, Is.False);
            Assert.That(sut.Household, Is.Null);
            Assert.That(sut.HouseholdIdentifier, Is.Null);
            Assert.That(sut.HouseholdIdentifier.HasValue, Is.False);
            Assert.That(sut.CreationTime, Is.EqualTo(DateTime.MinValue));
        }

        /// <summary>
        /// Tests that the constructor with a household member and a household initialize a data proxy which bind a given household member to a given household.
        /// </summary>
        [Test]
        public void TestThatConstructorWithHouseholdMemberAndHouseholdInitializeMemberOfHouseholdProxy()
        {
            Guid identifier = Guid.NewGuid();
            Guid householdMemberIdentifier = Guid.NewGuid();
            HouseholdMemberProxy householdMemberProxy = BuildHouseholdMemberProxy(identifier: householdMemberIdentifier);
            Guid householdIdentifier = Guid.NewGuid();
            HouseholdProxy householdProxy = BuildHouseholdProxy(identifier: householdIdentifier);
            DateTime creationTime = DateTime.Now;

            IMemberOfHouseholdProxy sut = CreateSut(identifier, householdMemberProxy, householdProxy, creationTime);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Not.Null);
            Assert.That(sut.Identifier, Is.EqualTo(identifier));
            Assert.That(sut.HouseholdMember, Is.Not.Null);
            Assert.That(sut.HouseholdMember, Is.EqualTo(householdMemberProxy));
            Assert.That(sut.HouseholdMemberIdentifier, Is.Not.Null);
            Assert.That(sut.HouseholdMemberIdentifier, Is.EqualTo(householdMemberIdentifier));
            Assert.That(sut.Household, Is.Not.Null);
            Assert.That(sut.Household, Is.EqualTo(householdProxy));
            Assert.That(sut.HouseholdIdentifier, Is.Not.Null);
            Assert.That(sut.HouseholdIdentifier, Is.EqualTo(householdIdentifier));
            Assert.That(sut.CreationTime, Is.EqualTo(creationTime));
        }

        /// <summary>
        /// Tests that the constructor with a household member and a household throws an ArgumentNullException when the household member is null.
        /// </summary>
        [Test]
        public void TestThatConstructorWithHouseholdMemberAndHouseholdThrowsArgumentNullExceptionWhenHouseholdMemberIsNull()
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => CreateSut(Guid.NewGuid(), null, BuildHouseholdProxy(), DateTime.Now));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "householdMember");
        }

        /// <summary>
        /// Tests that the constructor with a household member and a household throws an ArgumentNullException when the household is null.
        /// </summary>
        [Test]
        public void TestThatConstructorWithHouseholdMemberAndHouseholdThrowsArgumentNullExceptionWhenHouseholdIsNull()
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => CreateSut(Guid.NewGuid(), BuildHouseholdMemberProxy(), null, DateTime.Now));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "household");
        }

        /// <summary>
        /// Tests that getter for UniqueId throws an IntranetRepositoryException when the binding for a given household member to a given household has no identifier.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterThrowsIntranetRepositoryExceptionWhenMemberOfHouseholdProxyHasNoIdentifier()
        {
            IMemberOfHouseholdProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            // ReSharper disable UnusedVariable
            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => {var uniqueId = sut.UniqueId;});
            // ReSharper restore UnusedVariable

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, sut.Identifier, "Identifier");
        }

        /// <summary>
        /// Tests that getter for UniqueId gets the unique identifier for the binding for a given household member to a given household.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterGetsUniqueIdentificationForMemberOfHouseholdProxy()
        {
            Guid identifier = Guid.NewGuid();

            IMemberOfHouseholdProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

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
            IMemberOfHouseholdProxy sut = CreateSut();
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
            IMemberOfHouseholdProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.MapData(CreateMySqlDataReader(), null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that MapData maps data into the proxy.
        /// </summary>
        [Test]
        public void TestThatMapDataMapsDataIntoProxy()
        {
            IMemberOfHouseholdProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            Guid memberOfHouseholdIdentifier = Guid.NewGuid();
            Guid householdMemberIdentifier = Guid.NewGuid();
            Guid householdIdentifier = Guid.NewGuid();
            DateTime creationTime = DateTime.Now;
            MySqlDataReader dataReader = CreateMySqlDataReader(memberOfHouseholdIdentifier, householdMemberIdentifier, householdIdentifier, creationTime);

            HouseholdMemberProxy householdMemberProxy = BuildHouseholdMemberProxy(identifier: householdMemberIdentifier);
            HouseholdProxy householdProxy = BuildHouseholdProxy(identifier: householdIdentifier);
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(householdMemberProxy, householdProxy);

            sut.MapData(dataReader, dataProvider);

            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Not.Null);
            Assert.That(sut.Identifier, Is.EqualTo(memberOfHouseholdIdentifier));
            Assert.That(sut.HouseholdMember, Is.Not.Null);
            Assert.That(sut.HouseholdMember, Is.EqualTo(householdMemberProxy));
            Assert.That(sut.HouseholdMemberIdentifier, Is.Not.Null);
            Assert.That(sut.HouseholdMemberIdentifier, Is.EqualTo(householdMemberIdentifier));
            // TODO: fix this.
            Assert.That(sut.Household, Is.Null);
            Assert.That(sut.HouseholdIdentifier, Is.Null);
            Assert.That(sut.HouseholdIdentifier.HasValue, Is.False);
            Assert.That(sut.CreationTime, Is.EqualTo(creationTime));

            dataReader.AssertWasCalled(m => m.GetString("MemberOfHouseholdIdentifier"), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetDateTime("CreationTime"), opt => opt.Repeat.Once());

            dataProvider.AssertWasNotCalled(m => m.Clone());

            dataProvider.AssertWasCalled(m => m.Create(
                    Arg<IHouseholdMemberProxy>.Is.TypeOf,
                    Arg<MySqlDataReader>.Is.Equal(dataReader),
                    Arg<string[]>.Matches(e => e != null && e.Length == 8 &&
                                               e[0] == "HouseholdMemberIdentifier" &&
                                               e[1] == "HouseholdMemberMailAddress" &&
                                               e[2] == "HouseholdMemberMembership" &&
                                               e[3] == "HouseholdMemberMembershipExpireTime" &&
                                               e[4] == "HouseholdMemberActivationCode" &&
                                               e[5] == "HouseholdMemberActivationTime" &&
                                               e[6] == "HouseholdMemberPrivacyPolicyAcceptedTime" &&
                                               e[7] == "HouseholdMemberCreationTime")),
                opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that MapRelations throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatMapRelationsThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            IMemberOfHouseholdProxy sut = CreateSut();
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
            IMemberOfHouseholdProxy sut = CreateSut();
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
            IMemberOfHouseholdProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.SaveRelations(null, _fixture.Create<bool>()));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that SaveRelations throws an IntranetRepositoryException when the identifier for the given household member to a given household is null.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsThrowsIntranetRepositoryExceptionWhenIdentifierIsNull()
        {
            IMemberOfHouseholdProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.SaveRelations(CreateFoodWasteDataProvider(), _fixture.Create<bool>()));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, sut.Identifier, "Identifier");
        }

        /// <summary>
        /// Tests that DeleteRelations throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            IMemberOfHouseholdProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.DeleteRelations(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that DeleteRelations throws an IntranetRepositoryException when the identifier for the given household member to a given household is null.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsThrowsIntranetRepositoryExceptionWhenIdentifierIsNull()
        {
            IMemberOfHouseholdProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.DeleteRelations(CreateFoodWasteDataProvider()));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, sut.Identifier, "Identifier");
        }

        /// <summary>
        /// Tests that CreateGetCommand returns the SQL command for selecting the given binding for a given household member to a given household.
        /// </summary>
        [Test]
        public void TestThatCreateGetCommandReturnsSqlCommand()
        {
            Guid identifier = Guid.NewGuid();

            IMemberOfHouseholdProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            // ReSharper disable StringLiteralTypo
            new DbCommandTestBuilder("SELECT moh.MemberOfHouseholdIdentifier,moh.HouseholdMemberIdentifier,hm.MailAddress AS HouseholdMemberMailAddress,hm.Membership AS HouseholdMemberMembership,hm.MembershipExpireTime AS HouseholdMemberMembershipExpireTime,hm.ActivationCode AS HouseholdMemberActivationCode,hm.ActivationTime AS HouseholdMemberActivationTime,hm.PrivacyPolicyAcceptedTime AS HouseholdMemberPrivacyPolicyAcceptedTime,hm.CreationTime AS HouseholdMemberCreationTime,moh.HouseholdIdentifier,h.Name AS HouseholdName,h.Descr AS HouseholdDescr,h.CreationTime AS HouseholdCreationTime,moh.CreationTime FROM MemberOfHouseholds AS moh INNER JOIN HouseholdMembers hm ON hm.HouseholdMemberIdentifier=moh.HouseholdMemberIdentifier INNER JOIN Households h ON h.HouseholdIdentifier=moh.HouseholdIdentifier WHERE moh.MemberOfHouseholdIdentifier=@memberOfHouseholdIdentifier")
            // ReSharper restore StringLiteralTypo
                .AddCharDataParameter("@memberOfHouseholdIdentifier", identifier)
                .Build()
                .Run(sut.CreateGetCommand());
        }

        /// <summary>
        /// Tests that CreateInsertCommand returns the SQL command to insert a binding for a given household member to a given household.
        /// </summary>
        [Test]
        public void TestThatCreateInsertCommandReturnsSqlCommandForInsert()
        {
            Guid identifier = Guid.NewGuid();
            Guid householdMemberIdentifier = Guid.NewGuid();
            HouseholdMemberProxy householdMemberProxy = BuildHouseholdMemberProxy(identifier: householdMemberIdentifier);
            Guid householdIdentifier = Guid.NewGuid();
            HouseholdProxy householdProxy = BuildHouseholdProxy(identifier: householdIdentifier);
            DateTime creationTime = DateTime.Now;

            IMemberOfHouseholdProxy sut = CreateSut(identifier, householdMemberProxy, householdProxy, creationTime);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("INSERT INTO MemberOfHouseholds (MemberOfHouseholdIdentifier,HouseholdMemberIdentifier,HouseholdIdentifier,CreationTime) VALUES(@memberOfHouseholdIdentifier,@householdMemberIdentifier,@householdIdentifier,@creationTime)")
                .AddCharDataParameter("@memberOfHouseholdIdentifier", identifier)
                .AddCharDataParameter("@householdMemberIdentifier", householdMemberIdentifier)
                .AddCharDataParameter("@householdIdentifier", householdIdentifier)
                .AddDateTimeDataParameter("@creationTime", creationTime.ToUniversalTime())
                .Build()
                .Run(sut.CreateInsertCommand());
        }

        /// <summary>
        /// Tests that CreateUpdateCommand returns the SQL command to update a given household member to a given household.
        /// </summary>
        [Test]
        public void TestThatCreateUpdateCommandReturnsSqlCommandForUpdate()
        {
            Guid identifier = Guid.NewGuid();
            Guid householdMemberIdentifier = Guid.NewGuid();
            HouseholdMemberProxy householdMemberProxy = BuildHouseholdMemberProxy(identifier: householdMemberIdentifier);
            Guid householdIdentifier = Guid.NewGuid();
            HouseholdProxy householdProxy = BuildHouseholdProxy(identifier: householdIdentifier);
            DateTime creationTime = DateTime.Now;

            IMemberOfHouseholdProxy sut = CreateSut(identifier, householdMemberProxy, householdProxy, creationTime);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("UPDATE MemberOfHouseholds SET HouseholdMemberIdentifier=@householdMemberIdentifier,HouseholdIdentifier=@householdIdentifier,CreationTime=@creationTime WHERE MemberOfHouseholdIdentifier=@memberOfHouseholdIdentifier")
                .AddCharDataParameter("@memberOfHouseholdIdentifier", identifier)
                .AddCharDataParameter("@householdMemberIdentifier", householdMemberIdentifier)
                .AddCharDataParameter("@householdIdentifier", householdIdentifier)
                .AddDateTimeDataParameter("@creationTime", creationTime.ToUniversalTime())
                .Build()
                .Run(sut.CreateUpdateCommand());
        }

        /// <summary>
        /// Tests that CreateDeleteCommand returns the SQL command to delete a given household member to a given household.
        /// </summary>
        [Test]
        public void TestThatCreateDeleteCommandReturnsSqlCommandForDelete()
        {
            Guid identifier = Guid.NewGuid();

            IMemberOfHouseholdProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("DELETE FROM MemberOfHouseholds WHERE MemberOfHouseholdIdentifier=@memberOfHouseholdIdentifier")
                .AddCharDataParameter("@memberOfHouseholdIdentifier", identifier)
                .Build()
                .Run(sut.CreateDeleteCommand());
        }

        /// <summary>
        /// Creates an instance of a data proxy which bind a given household member to a given household.
        /// </summary>
        /// <returns>Instance of a data proxy which bind a given household member to a given household.</returns>
        private IMemberOfHouseholdProxy CreateSut()
        {
            return new MemberOfHouseholdProxy();
        }

        /// <summary>
        /// Creates an instance of a data proxy which bind a given household member to a given household.
        /// </summary>
        /// <returns>Instance of a data proxy which bind a given household member to a given household.</returns>
        private IMemberOfHouseholdProxy CreateSut(Guid identifier)
        {
            return new MemberOfHouseholdProxy
            {
                Identifier = identifier
            };
        }

        /// <summary>
        /// Creates an instance of a data proxy which bind a given household member to a given household.
        /// </summary>
        /// <returns>Instance of a data proxy which bind a given household member to a given household.</returns>
        private IMemberOfHouseholdProxy CreateSut(Guid identifier, IHouseholdMember householdMember, IHousehold household, DateTime creationTime)
        {
            return new MemberOfHouseholdProxy(householdMember, household, creationTime)
            {
                Identifier = identifier
            };
        }

        /// <summary>
        /// Creates a stub for the MySQL data reader.
        /// </summary>
        /// <returns>Stub for the MySQL data reader.</returns>
        private MySqlDataReader CreateMySqlDataReader(Guid? memberOfHouseholdIdentifier = null, Guid? householdMemberIdentifier = null, Guid? householdIdentifier = null, DateTime? creationTime = null)
        {
            MySqlDataReader mySqlDataReaderMock = MockRepository.GenerateStub<MySqlDataReader>();
            mySqlDataReaderMock.Stub(m => m.GetString(Arg<string>.Is.Equal("MemberOfHouseholdIdentifier")))
                .Return(memberOfHouseholdIdentifier.HasValue ? memberOfHouseholdIdentifier.Value.ToString("D").ToUpper() : Guid.NewGuid().ToString("D").ToUpper())
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetString(Arg<string>.Is.Equal("HouseholdMemberIdentifier")))
                .Return(householdMemberIdentifier.HasValue ? householdMemberIdentifier.Value.ToString("D").ToUpper() : Guid.NewGuid().ToString("D").ToUpper())
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetString(Arg<string>.Is.Equal("HouseholdIdentifier")))
                .Return(householdIdentifier.HasValue ? householdIdentifier.Value.ToString("D").ToUpper() : Guid.NewGuid().ToString("D").ToUpper())
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetDateTime(Arg<string>.Is.Equal("CreationTime")))
                .Return((creationTime ?? DateTime.Now).ToUniversalTime())
                .Repeat.Any();
            return mySqlDataReaderMock;
        }

        /// <summary>
        /// Creates a mockup for the data provider which can access data in the food waste repository.
        /// </summary>
        /// <returns>Mockup for the data provider which can access data in the food waste repository.</returns>
        private IFoodWasteDataProvider CreateFoodWasteDataProvider(HouseholdMemberProxy householdMemberProxy = null, HouseholdProxy householdProxy = null)
        {
            IFoodWasteDataProvider foodWasteDataProvider = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProvider.Stub(m => m.Create(Arg<IHouseholdMemberProxy>.Is.TypeOf, Arg<MySqlDataReader>.Is.Anything, Arg<string[]>.Is.Anything))
                .Return(householdMemberProxy ?? BuildHouseholdMemberProxy())
                .Repeat.Any();
            // TODO: fix this.
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
        /// Creates a data proxy to a household.
        /// </summary>
        /// <param name="hasIdentifier">Indicates whether the data proxy has an identifier.</param>
        /// <param name="identifier">The identifier for the data proxy.</param>
        /// <returns>Data proxy to a household.</returns>
        private HouseholdProxy BuildHouseholdProxy(bool hasIdentifier = true, Guid? identifier = null)
        {
            return new HouseholdProxy
            {
                Identifier = hasIdentifier ? identifier ?? Guid.NewGuid() : (Guid?) null
            };
        }
    }
}
