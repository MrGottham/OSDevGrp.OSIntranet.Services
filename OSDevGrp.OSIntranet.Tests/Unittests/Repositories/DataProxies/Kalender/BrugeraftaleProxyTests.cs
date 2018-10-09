using System;
using AutoFixture;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Repositories.DataProxies.Kalender;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.Fælles;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.Kalender;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.DataProxies.Kalender
{
    /// <summary>
    /// Tester data proxy for en brugers kalenderaftale.
    /// </summary>
    [TestFixture]
    public class BrugeraftaleProxyTests
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
        /// Tester, at konstruktøren initierer en data proxy for en brugeraftale.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererBrugeraftaleProxy()
        {
            IBrugeraftaleProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.System, Is.Not.Null);
            Assert.That(sut.System.Nummer, Is.EqualTo(0));
            Assert.That(sut.Aftale, Is.Not.Null);
            Assert.That(sut.Aftale.Id, Is.EqualTo(0));
            Assert.That(sut.Bruger, Is.Not.Null);
            Assert.That(sut.Bruger.Id, Is.EqualTo(0));
            Assert.That(sut.Properties, Is.EqualTo(0));
        }

        /// <summary>
        /// Tester, at UniqueId returnerer den unikke idenfikation for brugeraftalen.
        /// </summary>
        [Test]
        public void TestAtUniqueIdReturnererIdentifikation()
        {
            int systemNo = _fixture.Create<int>();
            int calId = _fixture.Create<int>();
            int userId = _fixture.Create<int>();

            IBrugeraftaleProxy sut = CreateSut(systemNo, calId, userId);
            Assert.That(sut, Is.Not.Null);

            string uniqueId = sut.UniqueId;
            Assert.That(uniqueId, Is.Not.Null);
            Assert.That(uniqueId, Is.EqualTo($"{Convert.ToString(systemNo)}-{Convert.ToString(calId)}-{Convert.ToString(userId)}"));
        }

        /// <summary>
        /// Tester, at MapData kaster en ArgumentNullException, hvis data reader er null.
        /// </summary>
        [Test]
        public void TestAtMapDataKasterArgumentNullExceptionHvisDataReaderErNull()
        {
            IBrugeraftaleProxy sut = CreateSut();
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
            IBrugeraftaleProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.MapData(CreateMySqlDataReader(), null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tester, at MapData mapper data proxy for en brugeraftale.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestAtMapDataMapperBrugeraftaleProxy(bool hasProperties)
        {
            IBrugeraftaleProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            int systemNo = _fixture.Create<int>();
            int calId = _fixture.Create<int>();
            int userId = _fixture.Create<int>();
            int? properties = hasProperties ? _fixture.Create<int>() : (int?) null;
            MySqlDataReader dataReader = CreateMySqlDataReader(systemNo, calId, userId, properties);

            ISystemProxy systemProxy = MockRepository.GenerateMock<ISystemProxy>();
            IAftaleProxy appointmentProxy = MockRepository.GenerateMock<IAftaleProxy>();
            IBrugerProxy calenderUserProxy = MockRepository.GenerateMock<IBrugerProxy>();
            IMySqlDataProvider dataProvider = CreateMySqlDataProvider(systemProxy, appointmentProxy, calenderUserProxy);

            sut.MapData(dataReader, dataProvider);

            Assert.That(sut.System, Is.Not.Null);
            Assert.That(sut.System, Is.EqualTo(systemProxy));
            Assert.That(sut.Aftale, Is.Not.Null);
            Assert.That(sut.Aftale, Is.EqualTo(appointmentProxy));
            Assert.That(sut.Bruger, Is.Not.Null);
            Assert.That(sut.Bruger, Is.EqualTo(calenderUserProxy));
            if (hasProperties)
            {
                Assert.That(sut.Properties, Is.EqualTo(properties));
            }
            else
            {
                Assert.That(sut.Properties, Is.EqualTo(0));
            }

            dataReader.AssertWasCalled(m => m.GetOrdinal(Arg<string>.Is.Equal("Properties")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.IsDBNull(Arg<int>.Is.Equal(3)), opt => opt.Repeat.Once());
            if (hasProperties)
            {
                dataReader.AssertWasCalled(m => m.GetInt32(Arg<string>.Is.Equal("Properties")), opt => opt.Repeat.Once());
            }
            else
            {
                dataReader.AssertWasNotCalled(m => m.GetInt32(Arg<string>.Is.Equal("Properties")));
            }

            dataProvider.AssertWasCalled(m => m.Create(
                    Arg<ISystemProxy>.Is.TypeOf,
                    Arg<MySqlDataReader>.Is.Equal(dataReader),
                    Arg<string[]>.Matches(e => e != null && e.Length == 3 &&
                                               e[0] == "SystemNo" &&
                                               e[1] == "SystemTitle" &&
                                               e[2] == "SystemProperties")),
                opt => opt.Repeat.Once());
            dataProvider.AssertWasCalled(m => m.Create(
                    Arg<IAftaleProxy>.Is.TypeOf,
                    Arg<MySqlDataReader>.Is.Equal(dataReader),
                    Arg<string[]>.Matches(e => e != null && e.Length == 10 &&
                                               e[0] == "CalId" &&
                                               e[1] == "Date" &&
                                               e[2] == "FromTime" &&
                                               e[3] == "ToTime" &&
                                               e[4] == "AppointmentProperties" &&
                                               e[5] == "Subject" &&
                                               e[6] == "Note" &&
                                               e[7] == "SystemNo" &&
                                               e[8] == "SystemTitle" &&
                                               e[9] == "SystemProperties")),
                opt => opt.Repeat.Once());
            dataProvider.AssertWasCalled(m => m.Create(
                    Arg<IBrugerProxy>.Is.TypeOf,
                    Arg<MySqlDataReader>.Is.Equal(dataReader),
                    Arg<string[]>.Matches(e => e != null && e.Length == 7 &&
                                               e[0] == "UserId" &&
                                               e[1] == "UserInitials" &&
                                               e[2] == "UserFullname" &&
                                               e[3] == "UserName" &&
                                               e[4] == "SystemNo" &&
                                               e[5] == "SystemTitle" &&
                                               e[6] == "SystemProperties")),
                opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tester, at CreateGetCommand returnerer SQL kommando til foresprøgelse efter brugeraftalen.
        /// </summary>
        [Test]
        public void TestAtCreateGetCommandReturnererSqlCommand()
        {
            int systemNo = _fixture.Create<int>();
            int calId = _fixture.Create<int>();
            int userId = _fixture.Create<int>();

            IBrugeraftaleProxy sut = CreateSut(systemNo, calId, userId);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("SELECT cm.SystemNo,cm.CalId,cm.UserId,cm.Properties,ca.Date,ca.FromTime,ca.ToTime,ca.Properties AS AppointmentProperties,ca.Subject,ca.Note,cu.UserName,cu.Name AS UserFullname,cu.Initials AS UserInitials,s.Title AS SystemTitle,s.Properties AS SystemProperties FROM Calmerge AS cm INNER JOIN Calapps AS ca ON ca.SystemNo=cm.SystemNo AND ca.CalId=cm.CalId INNER JOIN Calusers AS cu ON cu.SystemNo=cm.SystemNo AND cu.UserId=cm.UserId INNER JOIN Systems AS s ON s.SystemNo=cm.SystemNo WHERE cm.SystemNo=@systemNo AND cm.CalId=@calId AND cm.UserId=@userId")
                .AddSmallIntDataParameter("@systemNo", systemNo, 2)
                .AddIntDataParameter("@calId", calId, 8)
                .AddIntDataParameter("@userId", userId, 8)
                .Build()
                .Run(sut.CreateGetCommand());
        }

        /// <summary>
        /// Tester, at CreateInsertCommand returnerer SQL kommando til oprettelse af brugeraftalen.
        /// </summary>
        [Test]
        public void TestAtCreateInsertCommandReturnererSqlCommand()
        {
            int systemNo = _fixture.Create<int>();
            int calId = _fixture.Create<int>();
            int userId = _fixture.Create<int>();
            int properties = _fixture.Create<int>();

            IBrugeraftaleProxy sut = CreateSut(systemNo, calId, userId, properties);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("INSERT INTO Calmerge (SystemNo,CalId,UserId,Properties) VALUES(@systemNo,@calId,@userId,@properties)")
                .AddSmallIntDataParameter("@systemNo", systemNo, 2)
                .AddIntDataParameter("@calId", calId, 8)
                .AddIntDataParameter("@userId", userId, 8)
                .AddSmallIntDataParameter("@properties", properties, 3, true)
                .Build()
                .Run(sut.CreateInsertCommand());
        }

        /// <summary>
        /// Tester, at CreateUpdateCommand returnerer SQL kommando til opdatering af brugeraftalen.
        /// </summary>
        [Test]
        public void TestAtCreateUpdateCommandReturnererSqlCommand()
        {
            int systemNo = _fixture.Create<int>();
            int calId = _fixture.Create<int>();
            int userId = _fixture.Create<int>();
            int properties = _fixture.Create<int>();

            IBrugeraftaleProxy sut = CreateSut(systemNo, calId, userId, properties);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("UPDATE Calmerge SET Properties=@properties WHERE SystemNo=@systemNo AND CalId=@calId AND UserId=@userId")
                .AddSmallIntDataParameter("@systemNo", systemNo, 2)
                .AddIntDataParameter("@calId", calId, 8)
                .AddIntDataParameter("@userId", userId, 8)
                .AddSmallIntDataParameter("@properties", properties, 3, true)
                .Build()
                .Run(sut.CreateUpdateCommand());
        }

        /// <summary>
        /// Tester, at CreateDeleteCommand returnerer SQL kommando til sletning af brugeraftalen.
        /// </summary>
        [Test]
        public void TestAtCreateDeleteCommandReturnererSqlCommand()
        {
            int systemNo = _fixture.Create<int>();
            int calId = _fixture.Create<int>();
            int userId = _fixture.Create<int>();

            IBrugeraftaleProxy sut = CreateSut(systemNo, calId, userId);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("DELETE FROM Calmerge WHERE SystemNo=@systemNo AND CalId=@calId AND UserId=@userId")
                .AddSmallIntDataParameter("@systemNo", systemNo, 2)
                .AddIntDataParameter("@calId", calId, 8)
                .AddIntDataParameter("@userId", userId, 8)
                .Build()
                .Run(sut.CreateDeleteCommand());
        }

        /// <summary>
        /// Creates an instance of the data proxy for a binding between an appointment and a calender user.
        /// </summary>
        /// <returns>Instance of the data proxy for a binding between an appointment and a calender user.</returns>
        private IBrugeraftaleProxy CreateSut()
        {
            return new BrugeraftaleProxy();
        }

        /// <summary>
        /// Creates an instance of the data proxy for a binding between an appointment and a calender user.
        /// </summary>
        /// <returns>Instance of the data proxy for a binding between an appointment and a calender user.</returns>
        private IBrugeraftaleProxy CreateSut(int systemNo, int calId, int userId)
        {
            return new BrugeraftaleProxy(systemNo, calId, userId);
        }

        /// <summary>
        /// Creates an instance of the data proxy for a binding between an appointment and a calender user.
        /// </summary>
        /// <returns>Instance of the data proxy for a binding between an appointment and a calender user.</returns>
        private IBrugeraftaleProxy CreateSut(int systemNo, int calId, int userId, int properties)
        {
            return new BrugeraftaleProxy(systemNo, calId, userId, properties);
        }

        /// <summary>
        /// Creates a stub for the MySQL data reader.
        /// </summary>
        /// <returns>Stub for the MySQL data reader.</returns>
        private MySqlDataReader CreateMySqlDataReader(int? systemNo = null, int? calId = null, int? userId = null, int? properties = null)
        {
            MySqlDataReader mySqlDataReaderMock = MockRepository.GenerateStub<MySqlDataReader>();
            mySqlDataReaderMock.Stub(m => m.GetInt32("SystemNo"))
                .Return(systemNo ?? _fixture.Create<int>())
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetInt32("CalId"))
                .Return(calId ?? _fixture.Create<int>())
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetInt32("UserId"))
                .Return(userId ?? _fixture.Create<int>())
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetOrdinal(Arg<string>.Is.Equal("Properties")))
                .Return(3)
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.IsDBNull(3))
                .Return(properties.HasValue == false)
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetInt32("Properties"))
                .Return(properties ?? _fixture.Create<int>())
                .Repeat.Any();
            return mySqlDataReaderMock;
        }

        /// <summary>
        /// Creates a mockup for the data provider which uses MySQL.
        /// </summary>
        /// <returns>Mockup for the data provider which uses MySQL.</returns>
        private IMySqlDataProvider CreateMySqlDataProvider(ISystemProxy systemProxy = null, IAftaleProxy appointmentProxy = null, IBrugerProxy calenderUserProxy = null)
        {
            IMySqlDataProvider dataProviderMock = MockRepository.GenerateMock<IMySqlDataProvider>();
            dataProviderMock.Stub(m => m.Create(Arg<ISystemProxy>.Is.TypeOf, Arg<MySqlDataReader>.Is.Anything, Arg<string[]>.Is.Anything))
                .Return(systemProxy ?? MockRepository.GenerateMock<ISystemProxy>())
                .Repeat.Any();
            dataProviderMock.Stub(m => m.Create(Arg<IAftaleProxy>.Is.TypeOf, Arg<MySqlDataReader>.Is.Anything, Arg<string[]>.Is.Anything))
                .Return(appointmentProxy ?? MockRepository.GenerateMock<IAftaleProxy>())
                .Repeat.Any();
            dataProviderMock.Stub(m => m.Create(Arg<IBrugerProxy>.Is.TypeOf, Arg<MySqlDataReader>.Is.Anything, Arg<string[]>.Is.Anything))
                .Return(calenderUserProxy ?? MockRepository.GenerateMock<IBrugerProxy>())
                .Repeat.Any();
            return dataProviderMock;
        }
    }
}
