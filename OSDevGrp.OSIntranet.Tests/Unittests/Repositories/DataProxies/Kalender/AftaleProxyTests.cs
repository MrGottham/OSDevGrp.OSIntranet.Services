using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using MySql.Data.MySqlClient;
using MySql.Data.Types;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Kalender;
using OSDevGrp.OSIntranet.Domain.Kalender;
using OSDevGrp.OSIntranet.Repositories.DataProxies.Kalender;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.Fælles;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.Kalender;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.DataProxies.Kalender
{
    /// <summary>
    /// Tester data proxy for en kalenderaftale.
    /// </summary>
    [TestFixture]
    public class AftaleProxyTests
    {
        #region Private variables

        private Fixture _fixture;
        private Random _random;

        #endregion

        /// <summary>
        /// Sætter hver test op.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        /// <summary>
        /// Tester, at konstruktøren initierer en data proxy for en aftale.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererAftaleProxy()
        {
            IAftaleProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.System, Is.Not.Null);
            Assert.That(sut.System.Nummer, Is.EqualTo(0));
            Assert.That(sut.Id, Is.EqualTo(0));
            Assert.That(sut.FraTidspunkt, Is.EqualTo(DateTime.MinValue));
            Assert.That(sut.TilTidspunkt, Is.EqualTo(DateTime.MaxValue));
            Assert.That(sut.Emne, Is.Not.Null);
            Assert.That(sut.Emne, Is.EqualTo(typeof(Aftale).Name));
            Assert.That(sut.Properties, Is.EqualTo(0));
            Assert.That(sut.Deltagere, Is.Not.Null);
            Assert.That(sut.Deltagere, Is.Empty);
        }

        /// <summary>
        /// Tests that Deltagere calls Clone when the data provider when MapData has been called and X has not been called.
        /// </summary>
        [Test]
        public void TestThatDeltagereCallsCloneOnDataProviderWhenMapDataHasBeenCalledAndMapRelationsHasNotBeenCalled()
        {
            IAftaleProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            int systemNo = _fixture.Create<int>();
            int calId = _fixture.Create<int>();
            MySqlDataReader dataReader = CreateMySqlDataReader(systemNo, calId, subject: _fixture.Create<string>());

            IMySqlDataProvider dataProvider = CreateMySqlDataProvider();

            sut.MapData(dataReader, dataProvider);

            IEnumerable<IBrugeraftale> result = sut.Deltagere;
            Assert.That(result, Is.Not.Null);

            dataProvider.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that Deltagere gets appointment users when the data provider when MapData has been called and X has not been called.
        /// </summary>
        [Test]
        public void TestThatDeltagereGetAppointmentUsersWhenMapDataHasBeenCalledAndMapRelationsHasNotBeenCalled()
        {
            IAftaleProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            int systemNo = _fixture.Create<int>();
            int calId = _fixture.Create<int>();
            MySqlDataReader dataReader = CreateMySqlDataReader(systemNo, calId, subject: _fixture.Create<string>());

            ISystemProxy systemProxyMock = MockRepository.GenerateMock<ISystemProxy>();
            systemProxyMock.Stub(m => m.Nummer)
                .Return(systemNo)
                .Repeat.Any();
            IEnumerable<BrugeraftaleProxy> appointmentUserCollection = _fixture.CreateMany<BrugeraftaleProxy>(_random.Next(3, 7)).ToList();
            IMySqlDataProvider dataProvider = CreateMySqlDataProvider(systemProxyMock, appointmentUserCollection);

            sut.MapData(dataReader, dataProvider);

            IEnumerable<IBrugeraftale> result = sut.Deltagere;
            // ReSharper disable PossibleMultipleEnumeration
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EqualTo(appointmentUserCollection));
            // ReSharper restore PossibleMultipleEnumeration

            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT cm.SystemNo,cm.CalId,cm.UserId,cm.Properties,ca.Date,ca.FromTime,ca.ToTime,ca.Properties AS AppointmentProperties,ca.Subject,ca.Note,cu.UserName,cu.Name AS UserFullname,cu.Initials AS UserInitials,s.Title AS SystemTitle,s.Properties AS SystemProperties FROM Calmerge AS cm INNER JOIN Calapps AS ca ON ca.SystemNo=cm.SystemNo AND ca.CalId=cm.CalId INNER JOIN Calusers AS cu ON cu.SystemNo=cm.SystemNo AND cu.UserId=cm.UserId INNER JOIN Systems AS s ON s.SystemNo=cm.SystemNo WHERE cm.SystemNo=@systemNo AND cm.CalId=@calId")
                .AddSmallIntDataParameter("@systemNo", systemNo, 2)
                .AddIntDataParameter("@calId", calId, 8)
                .Build();
            dataProvider.AssertWasCalled(m => m.GetCollection<BrugeraftaleProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tester, at UniqueId returnerer den unikke idenfikation for aftalen.
        /// </summary>
        [Test]
        public void TestAtUniqueIdReturnererIdentifikation()
        {
            int systemNo = _fixture.Create<int>();
            int calId = _fixture.Create<int>();

            IAftaleProxy sut = CreateSut(systemNo, calId);
            Assert.That(sut, Is.Not.Null);

            string uniqueId = sut.UniqueId;
            Assert.That(uniqueId, Is.Not.Null);
            Assert.That(uniqueId, Is.EqualTo($"{Convert.ToString(systemNo)}-{Convert.ToString(calId)}"));
        }

        /// <summary>
        /// Tester, at MapData kaster en ArgumentNullException, hvis data reader er null.
        /// </summary>
        [Test]
        public void TestAtMapDataKasterArgumentNullExceptionHvisDataReaderErNull()
        {
            IAftaleProxy sut = CreateSut();
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
            IAftaleProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.MapData(CreateMySqlDataReader(), null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tester, at MapData mapper data proxy for en aftale.
        /// </summary>
        [Test]
        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void TestAtMapDataMapperAftaleProxy(bool hasProperties, bool hasNote)
        {
            IAftaleProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            int systemNo = _fixture.Create<int>();
            int calId = _fixture.Create<int>();
            DateTime fromDateTime = DateTime.Today.AddDays(_random.Next(1, 30)).AddHours(_random.Next(8, 16)) .AddMinutes(_random.Next(0, 3) * 15);
            TimeSpan duration = new TimeSpan(0, 0, _random.Next(1, 3) * 15, 0);
            int? properties = hasProperties ? _fixture.Create<int>() : (int?) null;
            string subject = _fixture.Create<string>();
            string note = hasNote ? _fixture.Create<string>() : null;
            MySqlDataReader dataReader = CreateMySqlDataReader(systemNo, calId, fromDateTime, duration, properties, subject, note);

            ISystemProxy systemProxy = MockRepository.GenerateMock<ISystemProxy>();
            IMySqlDataProvider dataProvider = CreateMySqlDataProvider(systemProxy);

            sut.MapData(dataReader, dataProvider);

            Assert.That(sut.System, Is.Not.Null);
            Assert.That(sut.System, Is.EqualTo(systemProxy));
            Assert.That(sut.Id, Is.EqualTo(calId));
            Assert.That(sut.FraTidspunkt, Is.EqualTo(fromDateTime));
            Assert.That(sut.TilTidspunkt, Is.EqualTo(fromDateTime.Add(duration)));
            if (hasProperties)
            {
                Assert.That(sut.Properties, Is.EqualTo(properties));
            }
            else
            {
                Assert.That(sut.Properties, Is.EqualTo(0));
            }
            Assert.That(sut.Emne, Is.Not.Null);
            Assert.That(sut.Emne, Is.Not.Empty);
            Assert.That(sut.Emne, Is.EqualTo(subject));
            if (hasNote)
            {
                Assert.That(sut.Notat, Is.Not.Null);
                Assert.That(sut.Notat, Is.Not.Empty);
                Assert.That(sut.Notat, Is.EqualTo(note));
            }
            else
            {
                Assert.That(sut.Notat, Is.Null);
            }

            dataReader.AssertWasCalled(m => m.GetInt32(Arg<string>.Is.Equal("CalId")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetMySqlDateTime(Arg<string>.Is.Equal("Date")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetTimeSpan(Arg<string>.Is.Equal("FromTime")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetTimeSpan(Arg<string>.Is.Equal("ToTime")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetOrdinal(Arg<string>.Is.Equal("Properties")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.IsDBNull(Arg<int>.Is.Equal(5)), opt => opt.Repeat.Once());
            if (hasProperties)
            {
                dataReader.AssertWasCalled(m => m.GetInt32(Arg<string>.Is.Equal("Properties")), opt => opt.Repeat.Once());
            }
            else
            {
                dataReader.AssertWasNotCalled(m => m.GetInt32(Arg<string>.Is.Equal("Properties")));
            }
            dataReader.AssertWasCalled(m => m.GetOrdinal(Arg<string>.Is.Equal("Subject")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.IsDBNull(Arg<int>.Is.Equal(6)), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetString(Arg<string>.Is.Equal("Subject")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetOrdinal(Arg<string>.Is.Equal("Note")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.IsDBNull(Arg<int>.Is.Equal(7)), opt => opt.Repeat.Once());
            if (hasNote)
            {
                dataReader.AssertWasCalled(m => m.GetString(Arg<string>.Is.Equal("Note")), opt => opt.Repeat.Once());
            }
            else
            {
                dataReader.AssertWasNotCalled(m => m.GetString(Arg<string>.Is.Equal("Note")));
            }

            dataProvider.AssertWasCalled(m => m.Create(
                    Arg<ISystemProxy>.Is.TypeOf,
                    Arg<MySqlDataReader>.Is.Equal(dataReader),
                    Arg<string[]>.Matches(e => e != null && e.Length == 3 &&
                                               e[0] == "SystemNo" &&
                                               e[1] == "SystemTitle" &&
                                               e[2] == "SystemProperties")),
                opt => opt.Repeat.Once());
            dataProvider.AssertWasNotCalled(m => m.Clone());
        }

        /// <summary>
        /// Tests that MapRelations throws an ArgumentNullException when the data provider is null.
        /// </summary>
        [Test]
        public void TestThatMapRelationsThrowArgumentNullExceptionWhenDataProviderIsNull()
        {
            IAftaleProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.MapRelations(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that MapRelations calls Clone on the data provider.
        /// </summary>
        [Test]
        public void TestThatMapRelationsCallsCloneOnDataProvider()
        {
            IAftaleProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            IMySqlDataProvider dataProvider = CreateMySqlDataProvider();

            sut.MapRelations(dataProvider);

            dataProvider.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that MapRelations maps the appointment users.
        /// </summary>
        [Test]
        public void TestThatMapRelationsMapsAppointmentUsers()
        {
            int systemNo = _fixture.Create<int>();
            int calId = _fixture.Create<int>();

            IAftaleProxy sut = CreateSut(systemNo, calId);
            Assert.That(sut, Is.Not.Null);

            IEnumerable<BrugeraftaleProxy> appointmentUserCollection = _fixture.CreateMany<BrugeraftaleProxy>(_random.Next(3, 7)).ToList();
            IMySqlDataProvider dataProvider = CreateMySqlDataProvider(appointmentUserCollection: appointmentUserCollection);

            sut.MapRelations(dataProvider);

            Assert.That(sut.Deltagere, Is.Not.Null);
            Assert.That(sut.Deltagere, Is.Not.Empty);
            Assert.That(sut.Deltagere, Is.EqualTo(appointmentUserCollection));

            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT cm.SystemNo,cm.CalId,cm.UserId,cm.Properties,ca.Date,ca.FromTime,ca.ToTime,ca.Properties AS AppointmentProperties,ca.Subject,ca.Note,cu.UserName,cu.Name AS UserFullname,cu.Initials AS UserInitials,s.Title AS SystemTitle,s.Properties AS SystemProperties FROM Calmerge AS cm INNER JOIN Calapps AS ca ON ca.SystemNo=cm.SystemNo AND ca.CalId=cm.CalId INNER JOIN Calusers AS cu ON cu.SystemNo=cm.SystemNo AND cu.UserId=cm.UserId INNER JOIN Systems AS s ON s.SystemNo=cm.SystemNo WHERE cm.SystemNo=@systemNo AND cm.CalId=@calId")
                .AddSmallIntDataParameter("@systemNo", systemNo, 2)
                .AddIntDataParameter("@calId", calId, 8)
                .Build();
            dataProvider.AssertWasCalled(m => m.GetCollection<BrugeraftaleProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tester, at CreateGetCommand returnerer SQL kommand til foresprøgelse efter aftalen.
        /// </summary>
        [Test]
        public void TestAtCreateGetCommandReturnererSqlCommand()
        {
            int systemNo = _fixture.Create<int>();
            int calId = _fixture.Create<int>();

            IAftaleProxy sut = CreateSut(systemNo, calId);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("SELECT ca.SystemNo,ca.CalId,ca.Date,ca.FromTime,ca.ToTime,ca.Properties,ca.Subject,ca.Note,s.Title AS SystemTitle,s.Properties AS SystemProperties FROM Calapps AS ca INNER JOIN Systems AS s ON s.SystemNo=ca.SystemNo WHERE ca.SystemNo=@systemNo AND ca.CalId=@calId")
                .AddSmallIntDataParameter("@systemNo", systemNo, 2)
                .AddIntDataParameter("@calId", calId, 8)
                .Build()
                .Run(sut.CreateGetCommand());
        }

        /// <summary>
        /// Tester, at CreateInsertCommand returnerer SQL kommando til oprettelse af aftalen.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestAtCreateInsertCommandReturnererSqlCommand(bool hasNote)
        {
            int systemNo = _fixture.Create<int>();
            int calId = _fixture.Create<int>();
            DateTime fromDateTime = DateTime.Today.AddDays(_random.Next(1, 30)).AddHours(_random.Next(8, 16)).AddMinutes(_random.Next(0, 3) * 15);
            DateTime toDateTime = fromDateTime.AddMinutes(_random.Next(1, 3) * 15);
            string subject = _fixture.Create<string>();
            int properties = _fixture.Create<int>();
            string note = hasNote ? _fixture.Create<string>() : null;

            IAftaleProxy sut = CreateSut(systemNo, calId, fromDateTime, toDateTime, subject, properties, note);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("INSERT INTO Calapps (SystemNo,CalId,Date,FromTime,ToTime,Properties,Subject,Note) VALUES(@systemNo,@calId,@date,@fromTime,@toTime,@properties,@subject,@note)")
                .AddSmallIntDataParameter("@systemNo", systemNo, 2)
                .AddIntDataParameter("@calId", calId, 8)
                .AddDateParameter("@date", fromDateTime)
                .AddTimeParameter("@fromTime", fromDateTime)
                .AddTimeParameter("@toTime", toDateTime)
                .AddSmallIntDataParameter("@properties", properties, 3, true)
                .AddVarCharDataParameter("@subject", subject, 255, true)
                .AddTextDataParameter("@note", note, true)
                .Build()
                .Run(sut.CreateInsertCommand());
        }

        /// <summary>
        /// Tester, at CreateUpdateCommand returnerer SQL kommando til opdatering af aftalen.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestAtCreateUpdateCommandReturnererSqlCommand(bool hasNote)
        {
            int systemNo = _fixture.Create<int>();
            int calId = _fixture.Create<int>();
            DateTime fromDateTime = DateTime.Today.AddDays(_random.Next(1, 30)).AddHours(_random.Next(8, 16)).AddMinutes(_random.Next(0, 3) * 15);
            DateTime toDateTime = fromDateTime.AddMinutes(_random.Next(1, 3) * 15);
            string subject = _fixture.Create<string>();
            int properties = _fixture.Create<int>();
            string note = hasNote ? _fixture.Create<string>() : null;

            IAftaleProxy sut = CreateSut(systemNo, calId, fromDateTime, toDateTime, subject, properties, note);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("UPDATE Calapps SET Date=@date,FromTime=@fromTime,ToTime=@toTime,Properties=@properties,Subject=@subject,Note=@note WHERE SystemNo=@systemNo AND CalId=@calId")
                .AddSmallIntDataParameter("@systemNo", systemNo, 2)
                .AddIntDataParameter("@calId", calId, 8)
                .AddDateParameter("@date", fromDateTime)
                .AddTimeParameter("@fromTime", fromDateTime)
                .AddTimeParameter("@toTime", toDateTime)
                .AddSmallIntDataParameter("@properties", properties, 3, true)
                .AddVarCharDataParameter("@subject", subject, 255, true)
                .AddTextDataParameter("@note", note, true)
                .Build()
                .Run(sut.CreateUpdateCommand());
        }

        /// <summary>
        /// Tester, at CreateDeleteCommand returnerer SQL kommando til sletning af aftalen.
        /// </summary>
        [Test]
        public void TestAtCreateDeleteCommandReturnererSqlCommand()
        {
            int systemNo = _fixture.Create<int>();
            int calId = _fixture.Create<int>();

            IAftaleProxy sut = CreateSut(systemNo, calId);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("DELETE FROM Calapps WHERE SystemNo=@systemNo AND CalId=@calId")
                .AddSmallIntDataParameter("@systemNo", systemNo, 2)
                .AddIntDataParameter("@calId", calId, 8)
                .Build()
                .Run(sut.CreateDeleteCommand());
        }

        /// <summary>
        /// Tests that Create throws ArgumentNullException when the data reader is null.
        /// </summary>
        [Test]
        public void TestThatCreateThrowsArgumentNullExceptionWhenDataReaderIsNull()
        {
            IAftaleProxy sut = CreateSut();
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
            IAftaleProxy sut = CreateSut();
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
            IAftaleProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Create(CreateMySqlDataReader(), CreateMySqlDataProvider(), null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "columnNameCollection");
        }

        /// <summary>
        /// Tests that Create create a calender user data proxy.
        /// </summary>
        [Test]
        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void TestThatCreateCreatesCalenderUserProxy(bool hasProperties, bool hasNote)
        {
            IAftaleProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            int systemNo = _fixture.Create<int>();
            int calId = _fixture.Create<int>();
            DateTime fromDateTime = DateTime.Today.AddDays(_random.Next(1, 30)).AddHours(_random.Next(8, 16)).AddMinutes(_random.Next(0, 3) * 15);
            TimeSpan duration = new TimeSpan(0, 0, _random.Next(1, 3) * 15, 0);
            int? properties = hasProperties ? _fixture.Create<int>() : (int?)null;
            string subject = _fixture.Create<string>();
            string note = hasNote ? _fixture.Create<string>() : null;
            MySqlDataReader dataReader = CreateMySqlDataReader(systemNo, calId, fromDateTime, duration, properties, subject, note);

            ISystemProxy systemProxy = MockRepository.GenerateMock<ISystemProxy>();
            IMySqlDataProvider dataProvider = CreateMySqlDataProvider(systemProxy);

            IAftaleProxy result = sut.Create(dataReader, dataProvider, "CalId", "Date", "FromTime", "ToTime", "Properties", "Subject", "Note", "SystemNo", "SystemTitle", "SystemProperties");
            Assert.That(result.System, Is.Not.Null);
            Assert.That(result.System, Is.EqualTo(systemProxy));
            Assert.That(result.Id, Is.EqualTo(calId));
            Assert.That(result.FraTidspunkt, Is.EqualTo(fromDateTime));
            Assert.That(result.TilTidspunkt, Is.EqualTo(fromDateTime.Add(duration)));
            if (hasProperties)
            {
                Assert.That(result.Properties, Is.EqualTo(properties));
            }
            else
            {
                Assert.That(result.Properties, Is.EqualTo(0));
            }
            Assert.That(result.Emne, Is.Not.Null);
            Assert.That(result.Emne, Is.Not.Empty);
            Assert.That(result.Emne, Is.EqualTo(subject));
            if (hasNote)
            {
                Assert.That(result.Notat, Is.Not.Null);
                Assert.That(result.Notat, Is.Not.Empty);
                Assert.That(result.Notat, Is.EqualTo(note));
            }
            else
            {
                Assert.That(result.Notat, Is.Null);
            }

            dataReader.AssertWasCalled(m => m.GetInt32(Arg<string>.Is.Equal("CalId")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetMySqlDateTime(Arg<string>.Is.Equal("Date")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetTimeSpan(Arg<string>.Is.Equal("FromTime")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetTimeSpan(Arg<string>.Is.Equal("ToTime")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetOrdinal(Arg<string>.Is.Equal("Properties")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.IsDBNull(Arg<int>.Is.Equal(5)), opt => opt.Repeat.Once());
            if (hasProperties)
            {
                dataReader.AssertWasCalled(m => m.GetInt32(Arg<string>.Is.Equal("Properties")), opt => opt.Repeat.Once());
            }
            else
            {
                dataReader.AssertWasNotCalled(m => m.GetInt32(Arg<string>.Is.Equal("Properties")));
            }
            dataReader.AssertWasCalled(m => m.GetOrdinal(Arg<string>.Is.Equal("Subject")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.IsDBNull(Arg<int>.Is.Equal(6)), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetString(Arg<string>.Is.Equal("Subject")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetOrdinal(Arg<string>.Is.Equal("Note")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.IsDBNull(Arg<int>.Is.Equal(7)), opt => opt.Repeat.Once());
            if (hasNote)
            {
                dataReader.AssertWasCalled(m => m.GetString(Arg<string>.Is.Equal("Note")), opt => opt.Repeat.Once());
            }
            else
            {
                dataReader.AssertWasNotCalled(m => m.GetString(Arg<string>.Is.Equal("Note")));
            }

            dataProvider.AssertWasCalled(m => m.Create(
                    Arg<ISystemProxy>.Is.TypeOf,
                    Arg<MySqlDataReader>.Is.Equal(dataReader),
                    Arg<string[]>.Matches(e => e != null && e.Length == 3 &&
                                               e[0] == "SystemNo" &&
                                               e[1] == "SystemTitle" &&
                                               e[2] == "SystemProperties")),
                opt => opt.Repeat.Once());
            dataProvider.AssertWasNotCalled(m => m.Clone());
        }

        /// <summary>
        /// Creates an instance of the data proxy for an appointment.
        /// </summary>
        /// <returns>Instance of the data proxy for an appointment.</returns>
        private IAftaleProxy CreateSut()
        {
            return new AftaleProxy();
        }

        /// <summary>
        /// Creates an instance of the data proxy for an appointment.
        /// </summary>
        /// <returns>Instance of the data proxy for an appointment.</returns>
        private IAftaleProxy CreateSut(int systemNo, int calId)
        {
            return new AftaleProxy(systemNo, calId);
        }

        /// <summary>
        /// Creates an instance of the data proxy for an appointment.
        /// </summary>
        /// <returns>Instance of the data proxy for an appointment.</returns>
        private IAftaleProxy CreateSut(int systemNo, int calId, DateTime fromDateTime, DateTime toDateTime, string subject, int properties, string note)
        {
            return new AftaleProxy(systemNo, calId, fromDateTime, toDateTime, subject, properties)
            {
                Notat = note
            };
        }

        /// <summary>
        /// Creates a stub for the MySQL data reader.
        /// </summary>
        /// <returns>Stub for the MySQL data reader.</returns>
        private MySqlDataReader CreateMySqlDataReader(int? systemNo = null, int? calId = null, DateTime? fromDateTime = null, TimeSpan? duration = null, int? properties = null, string subject = null, string note = null)
        {
            DateTime appointmentFromDateTime = fromDateTime ?? DateTime.Today.AddDays(_random.Next(1, 30)).AddHours(_random.Next(8, 16)).AddMinutes(_random.Next(0, 3) * 15);
            DateTime appointmentToDateTime = appointmentFromDateTime.Add(duration ?? new TimeSpan(0, 0, _random.Next(1, 3) * 15, 0));

            MySqlDataReader mySqlDataReaderMock = MockRepository.GenerateStub<MySqlDataReader>();
            mySqlDataReaderMock.Stub(m => m.GetInt32("SystemNo"))
                .Return(systemNo ?? _fixture.Create<int>())
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetInt32("CalId"))
                .Return(calId ?? _fixture.Create<int>())
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetMySqlDateTime(Arg<string>.Is.Equal("Date")))
                .Return(new MySqlDateTime(appointmentFromDateTime.Date))
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetTimeSpan(Arg<string>.Is.Equal("FromTime")))
                .Return(appointmentFromDateTime.TimeOfDay)
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetTimeSpan(Arg<string>.Is.Equal("ToTime")))
                .Return(appointmentToDateTime.TimeOfDay)
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetOrdinal(Arg<string>.Is.Equal("Properties")))
                .Return(5)
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.IsDBNull(Arg<int>.Is.Equal(5)))
                .Return(properties.HasValue == false)
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetInt32(Arg<string>.Is.Equal("Properties")))
                .Return(properties ?? _fixture.Create<int>())
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetOrdinal(Arg<string>.Is.Equal("Subject")))
                .Return(6)
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.IsDBNull(Arg<int>.Is.Equal(6)))
                .Return(string.IsNullOrWhiteSpace(subject))
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetString(Arg<string>.Is.Equal("Subject")))
                .Return(subject)
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetOrdinal(Arg<string>.Is.Equal("Note")))
                .Return(7)
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.IsDBNull(Arg<int>.Is.Equal(7)))
                .Return(string.IsNullOrWhiteSpace(note))
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetString(Arg<string>.Is.Equal("Note")))
                .Return(note)
                .Repeat.Any();
            return mySqlDataReaderMock;
        }

        /// <summary>
        /// Creates a mockup for the data provider which uses MySQL.
        /// </summary>
        /// <returns>Mockup for the data provider which uses MySQL.</returns>
        private IMySqlDataProvider CreateMySqlDataProvider(ISystemProxy systemProxy = null, IEnumerable<BrugeraftaleProxy> appointmentUserCollection = null)
        {
            IMySqlDataProvider dataProviderMock = MockRepository.GenerateMock<IMySqlDataProvider>();
            dataProviderMock.Stub(m => m.Clone())
                .Return(dataProviderMock)
                .Repeat.Any();
            dataProviderMock.Stub(m => m.Create(Arg<ISystemProxy>.Is.TypeOf, Arg<MySqlDataReader>.Is.Anything, Arg<string[]>.Is.Anything))
                .Return(systemProxy ?? MockRepository.GenerateMock<ISystemProxy>())
                .Repeat.Any();
            dataProviderMock.Stub(m => m.GetCollection<BrugeraftaleProxy>(Arg<MySqlCommand>.Is.Anything))
                .Return(appointmentUserCollection ?? _fixture.CreateMany<BrugeraftaleProxy>(_random.Next(3, 7)).ToList())
                .Repeat.Any();
            return dataProviderMock;
        }
    }
}
