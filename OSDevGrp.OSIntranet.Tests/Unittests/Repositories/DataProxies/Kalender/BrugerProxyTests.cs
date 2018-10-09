using System;
using AutoFixture;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Kalender;
using OSDevGrp.OSIntranet.Repositories.DataProxies.Kalender;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.Fælles;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.Kalender;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.DataProxies.Kalender
{
    /// <summary>
    /// Tester data proxy for en kalenderbruger under OSWEBDB.
    /// </summary>
    [TestFixture]
    public class BrugerProxyTests
    {
        #region Private variables

        private Fixture _fixture;

        #endregion

        /// <summary>
        /// Sætter hver test op.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        /// <summary>
        /// Tester, at konstruktøren initierer en data proxy for en bruger.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererBrugerProxy()
        {
            IBrugerProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.System, Is.Not.Null);
            Assert.That(sut.System.Nummer, Is.EqualTo(0));
            Assert.That(sut.Id, Is.EqualTo(0));
            Assert.That(sut.UserName, Is.Null);
            Assert.That(sut.Navn, Is.Not.Null);
            Assert.That(sut.Navn, Is.Not.Empty);
            Assert.That(sut.Navn, Is.EqualTo(typeof(Bruger).Name));
            Assert.That(sut.Initialer, Is.Not.Null);
            Assert.That(sut.Initialer, Is.Not.Empty);
            Assert.That(sut.Initialer, Is.EqualTo(typeof(Bruger).Name));
        }

        /// <summary>
        /// Tester, at UniqueId returnerer den unikke idenfikation for brugeren.
        /// </summary>
        [Test]
        public void TestAtUniqueIdReturnererIdentifikation()
        {
            int systemNo = _fixture.Create<int>();
            int userId = _fixture.Create<int>();

            IBrugerProxy sut = CreateSut(systemNo, userId);
            Assert.That(sut, Is.Not.Null);

            string uniqueId = sut.UniqueId;
            Assert.That(uniqueId, Is.Not.Null);
            Assert.That(uniqueId, Is.Not.Empty);
            Assert.That(uniqueId, Is.EqualTo($"{Convert.ToString(systemNo)}-{Convert.ToString(userId)}"));
        }

        /// <summary>
        /// Tester, at MapData kaster en ArgumentNullException, hvis data reader er null.
        /// </summary>
        [Test]
        public void TestAtMapDataKasterArgumentNullExceptionHvisDataReaderErNull()
        {
            IBrugerProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.MapData(null, CreateMySqlDataProvider()));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataReader");
        }

        /// <summary>
        /// Tester, at MapData kaster en ArgumentNullException, hvis data provider er null.
        /// </summary>
        [Test]
        public void TestAtMapDataKasterArgumentNullExceptionHvisDataProviderErNull()
        {
            IBrugerProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.MapData(CreateMySqlDataReader(), null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tester, at MapData mapper data proxy for en bruger.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestAtMapDataMapperBrugerProxy(bool hasUserName)
        {
            IBrugerProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            int systemNo = _fixture.Create<int>();
            int userId = _fixture.Create<int>();
            string userName = hasUserName ? _fixture.Create<string>() : null;
            string name = _fixture.Create<string>();
            string initials = _fixture.Create<string>();
            MySqlDataReader dataReader = CreateMySqlDataReader(systemNo, userId, userName, name, initials);

            ISystemProxy systemProxy = MockRepository.GenerateMock<ISystemProxy>();
            IMySqlDataProvider dataProvider = CreateMySqlDataProvider(systemProxy);

            sut.MapData(dataReader, dataProvider);

            Assert.That(sut.System, Is.Not.Null);
            Assert.That(sut.System, Is.EqualTo(systemProxy));
            Assert.That(sut.Id, Is.EqualTo(userId));
            if (hasUserName)
            {
                Assert.That(sut.UserName, Is.Not.Null);
                Assert.That(sut.UserName, Is.Not.Empty);
                Assert.That(sut.UserName, Is.EqualTo(userName));
            }
            else
            {
                Assert.That(sut.UserName, Is.Null);
            }
            Assert.That(sut.Navn, Is.Not.Null);
            Assert.That(sut.Navn, Is.Not.Empty);
            Assert.That(sut.Navn, Is.EqualTo(name));
            Assert.That(sut.Initialer, Is.Not.Null);
            Assert.That(sut.Initialer, Is.Not.Empty);
            Assert.That(sut.Initialer, Is.EqualTo(initials));

            dataReader.AssertWasCalled(m => m.GetInt32(Arg<string>.Is.Equal("UserId")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetOrdinal(Arg<string>.Is.Equal("UserName")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.IsDBNull(Arg<int>.Is.Equal(2)), opt => opt.Repeat.Once());
            if (hasUserName)
            {
                dataReader.AssertWasCalled(m => m.GetString(Arg<string>.Is.Equal("UserName")), opt => opt.Repeat.Once());
            }
            else
            {
                dataReader.AssertWasNotCalled(m => m.GetString(Arg<string>.Is.Equal("UserName")));
            }
            dataReader.AssertWasCalled(m => m.GetString(Arg<string>.Is.Equal("Name")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetString(Arg<string>.Is.Equal("Initials")), opt => opt.Repeat.Once());

            dataProvider.AssertWasCalled(m => m.Create(
                    Arg<ISystemProxy>.Is.TypeOf,
                    Arg<MySqlDataReader>.Is.Equal(dataReader),
                    Arg<string[]>.Matches(e => e != null && e.Length == 3 &&
                                               e[0] == "SystemNo" &&
                                               e[1] == "Title" &&
                                               e[2] == "Properties")),
                opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tester, at CreateGetCommand returnerer SQL kommand til foresprøgelse efter brugeren.
        /// </summary>
        [Test]
        public void TestAtCreateGetCommandReturnererSqlCommand()
        {
            int systemNo = _fixture.Create<int>();
            int userId = _fixture.Create<int>();

            IBrugerProxy sut = CreateSut(systemNo, userId);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("SELECT cu.SystemNo,cu.UserId,cu.UserName,cu.Name,cu.Initials,s.Title,s.Properties FROM Calusers AS cu INNER JOIN Systems AS s ON s.SystemNo=cu.SystemNo WHERE cu.SystemNo=@systemNo AND cu.UserId=@userId")
                .AddSmallIntDataParameter("@systemNo", systemNo, 2)
                .AddIntDataParameter("@userId", userId, 8)
                .Build()
                .Run(sut.CreateGetCommand());
        }

        /// <summary>
        /// Tester, at CreateInsertCommand returnerer SQL kommando til oprettelse af brugeren.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestAtCreateInsertCommandReturnererSqlCommand(bool hasUserName)
        {
            int systemNo = _fixture.Create<int>();
            int userId = _fixture.Create<int>();
            string userName = hasUserName ? _fixture.Create<string>() : null;
            string name = _fixture.Create<string>();
            string initials = _fixture.Create<string>();

            IBrugerProxy sut = CreateSut(systemNo, userId, userName, name, initials);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("INSERT INTO Calusers (SystemNo,UserId,UserName,Name,Initials) VALUES(@systemNo,@userId,@userName,@name,@initials)")
                .AddSmallIntDataParameter("@systemNo", systemNo, 2)
                .AddIntDataParameter("@userId", userId, 8)
                .AddVarCharDataParameter("@userName", userName, 16, true)
                .AddVarCharDataParameter("@name", name, 40)
                .AddVarCharDataParameter("@initials", initials, 8)
                .Build()
                .Run(sut.CreateInsertCommand());
        }

        /// <summary>
        /// Tester, at CreateUpdateCommand returnerer SQL kommando til opdatering af brugeren.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestAtCreateUpdateCommandReturnererSqlCommand(bool hasUserName)
        {
            int systemNo = _fixture.Create<int>();
            int userId = _fixture.Create<int>();
            string userName = hasUserName ? _fixture.Create<string>() : null;
            string name = _fixture.Create<string>();
            string initials = _fixture.Create<string>();

            IBrugerProxy sut = CreateSut(systemNo, userId, userName, name, initials);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("UPDATE Calusers SET UserName=@userName,Name=@name,Initials=@initials WHERE SystemNo=@systemNo AND UserId=@userId")
                .AddSmallIntDataParameter("@systemNo", systemNo, 2)
                .AddIntDataParameter("@userId", userId, 8)
                .AddVarCharDataParameter("@userName", userName, 16, true)
                .AddVarCharDataParameter("@name", name, 40)
                .AddVarCharDataParameter("@initials", initials, 8)
                .Build()
                .Run(sut.CreateUpdateCommand());
        }

        /// <summary>
        /// Tester, at CreateDeleteCommand returnerer SQL kommando til sletning af brugeren.
        /// </summary>
        [Test]
        public void TestAtCreateDeleteCommandForDeleteReturnererSqlCommand()
        {
            int systemNo = _fixture.Create<int>();
            int userId = _fixture.Create<int>();

            IBrugerProxy sut = CreateSut(systemNo, userId);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("DELETE FROM Calusers WHERE SystemNo=@systemNo AND UserId=@userId")
                .AddSmallIntDataParameter("@systemNo", systemNo, 2)
                .AddIntDataParameter("@userId", userId, 8)
                .Build()
                .Run(sut.CreateDeleteCommand());
        }

        /// <summary>
        /// Tests that Create throws ArgumentNullException when the data reader is null.
        /// </summary>
        [Test]
        public void TestThatCreateThrowsArgumentNullExceptionWhenDataReaderIsNull()
        {
            IBrugerProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Create(null, CreateMySqlDataProvider(), "SystemNo", "Title", "Properties"));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataReader");
        }

        /// <summary>
        /// Tests that Create throws ArgumentNullException when the data provider is null.
        /// </summary>
        [Test]
        public void TestThatCreateThrowsArgumentNullExceptionWhenDataProviderIsNull()
        {
            IBrugerProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Create(CreateMySqlDataReader(), null, "SystemNo", "Title", "Properties"));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that Create throws ArgumentNullException when the collection of column names is null.
        /// </summary>
        [Test]
        public void TestThatCreateThrowsArgumentNullExceptionWhenColumnNameCollectionIsNull()
        {
            IBrugerProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Create(CreateMySqlDataReader(), CreateMySqlDataProvider(), null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "columnNameCollection");
        }

        /// <summary>
        /// Tests that Create create a calender user data proxy.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatCreateCreatesCalenderUserProxy(bool hasUserName)
        {
            IBrugerProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            int systemNo = _fixture.Create<int>();
            int userId = _fixture.Create<int>();
            string userName = hasUserName ? _fixture.Create<string>() : null;
            string name = _fixture.Create<string>();
            string initials = _fixture.Create<string>();
            MySqlDataReader dataReader = CreateMySqlDataReader(systemNo, userId, userName, name, initials);

            ISystemProxy systemProxy = MockRepository.GenerateMock<ISystemProxy>();
            IMySqlDataProvider dataProvider = CreateMySqlDataProvider(systemProxy);

            IBrugerProxy result = sut.Create(dataReader, dataProvider, "UserId", "Initials", "Name", "UserName", "SystemNo", "Title", "Properties");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.System, Is.Not.Null);
            Assert.That(result.System, Is.EqualTo(systemProxy));
            Assert.That(result.Id, Is.EqualTo(userId));
            if (hasUserName)
            {
                Assert.That(result.UserName, Is.Not.Null);
                Assert.That(result.UserName, Is.Not.Empty);
                Assert.That(result.UserName, Is.EqualTo(userName));
            }
            else
            {
                Assert.That(result.UserName, Is.Null);
            }
            Assert.That(result.Navn, Is.Not.Null);
            Assert.That(result.Navn, Is.Not.Empty);
            Assert.That(result.Navn, Is.EqualTo(name));
            Assert.That(result.Initialer, Is.Not.Null);
            Assert.That(result.Initialer, Is.Not.Empty);
            Assert.That(result.Initialer, Is.EqualTo(initials));

            dataReader.AssertWasCalled(m => m.GetInt32(Arg<string>.Is.Equal("UserId")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetOrdinal(Arg<string>.Is.Equal("UserName")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.IsDBNull(Arg<int>.Is.Equal(2)), opt => opt.Repeat.Once());
            if (hasUserName)
            {
                dataReader.AssertWasCalled(m => m.GetString(Arg<string>.Is.Equal("UserName")), opt => opt.Repeat.Once());
            }
            else
            {
                dataReader.AssertWasNotCalled(m => m.GetString(Arg<string>.Is.Equal("UserName")));
            }
            dataReader.AssertWasCalled(m => m.GetString(Arg<string>.Is.Equal("Name")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetString(Arg<string>.Is.Equal("Initials")), opt => opt.Repeat.Once());

            dataProvider.AssertWasCalled(m => m.Create(
                    Arg<ISystemProxy>.Is.TypeOf,
                    Arg<MySqlDataReader>.Is.Equal(dataReader),
                    Arg<string[]>.Matches(e => e != null && e.Length == 3 &&
                                               e[0] == "SystemNo" &&
                                               e[1] == "Title" &&
                                               e[2] == "Properties")),
                opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Creates an instance of the data proxy for a calender user within OSWEBDB.
        /// </summary>
        /// <returns>Instance of the data proxy for a calender user within OSWEBDB.</returns>
        private IBrugerProxy CreateSut()
        {
            return new BrugerProxy();
        }

        /// <summary>
        /// Creates an instance of the data proxy for a calender user within OSWEBDB.
        /// </summary>
        /// <returns>Instance of the data proxy for a calender user within OSWEBDB.</returns>
        private IBrugerProxy CreateSut(int systemNo, int userId)
        {
            return new BrugerProxy(systemNo, userId);
        }

        /// <summary>
        /// Creates an instance of the data proxy for a calender user within OSWEBDB.
        /// </summary>
        /// <returns>Instance of the data proxy for a calender user within OSWEBDB.</returns>
        private IBrugerProxy CreateSut(int systemNo, int userId, string userName, string name, string initials)
        {
            return new BrugerProxy(systemNo, userId, initials, name)
            {
                UserName = userName
            };
        }

        /// <summary>
        /// Creates a stub for the MySQL data reader.
        /// </summary>
        /// <returns>Stub for the MySQL data reader.</returns>
        private MySqlDataReader CreateMySqlDataReader(int? systemNo = null, int? userId = null, string userName = null, string name = null, string initials = null)
        {
            MySqlDataReader mySqlDataReaderMock = MockRepository.GenerateStub<MySqlDataReader>();
            mySqlDataReaderMock.Stub(m => m.GetInt32(Arg<string>.Is.Equal("SystemNo")))
                .Return(systemNo ?? _fixture.Create<int>())
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetInt32(Arg<string>.Is.Equal("UserId")))
                .Return(userId ?? _fixture.Create<int>())
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetOrdinal(Arg<string>.Is.Equal("UserName")))
                .Return(2)
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.IsDBNull(Arg<int>.Is.Equal(2)))
                .Return(string.IsNullOrWhiteSpace(userName))
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetString(Arg<string>.Is.Equal("UserName")))
                .Return(userName)
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetString(Arg<string>.Is.Equal("Name")))
                .Return(name ?? _fixture.Create<string>())
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetString(Arg<string>.Is.Equal("Initials")))
                .Return(initials ?? _fixture.Create<string>())
                .Repeat.Any();
            return mySqlDataReaderMock;
        }

        /// <summary>
        /// Creates a mockup for the data provider which uses MySQL.
        /// </summary>
        /// <returns>Mockup for the data provider which uses MySQL.</returns>
        private IMySqlDataProvider CreateMySqlDataProvider(ISystemProxy systemProxy = null)
        {
            IMySqlDataProvider dataProviderMock = MockRepository.GenerateMock<IMySqlDataProvider>();
            dataProviderMock.Stub(m => m.Create(Arg<ISystemProxy>.Is.TypeOf, Arg<MySqlDataReader>.Is.Anything, Arg<string[]>.Is.Anything))
                .Return(systemProxy ?? MockRepository.GenerateMock<ISystemProxy>())
                .Repeat.Any();
            return dataProviderMock;
        }
    }
}
