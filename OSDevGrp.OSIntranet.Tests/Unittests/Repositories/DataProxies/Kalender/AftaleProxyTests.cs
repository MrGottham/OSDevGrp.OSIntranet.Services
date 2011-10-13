using System;
using System.Data;
using OSDevGrp.OSIntranet.Domain.Kalender;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.DataProxies.Fælles;
using OSDevGrp.OSIntranet.Repositories.DataProxies.Kalender;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using MySql.Data.MySqlClient;
using MySql.Data.Types;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.DataProxies.Kalender
{
    /// <summary>
    /// Tester data proxy for en kalenderaftale.
    /// </summary>
    [TestFixture]
    public class AftaleProxyTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en data proxy for en aftale.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererAftaleProxy()
        {
            var fixture = new Fixture();
            fixture.Inject(new AftaleProxy());

            var aftaleProxy = fixture.CreateAnonymous<AftaleProxy>();
            Assert.That(aftaleProxy, Is.Not.Null);
            Assert.That(aftaleProxy.System, Is.Not.Null);
            Assert.That(aftaleProxy.System.Nummer, Is.EqualTo(0));
            Assert.That(aftaleProxy.Id, Is.EqualTo(0));
            Assert.That(aftaleProxy.FraTidspunkt, Is.EqualTo(DateTime.MinValue));
            Assert.That(aftaleProxy.TilTidspunkt, Is.EqualTo(DateTime.MaxValue));
            Assert.That(aftaleProxy.Emne, Is.Not.Null);
            Assert.That(aftaleProxy.Emne, Is.EqualTo(typeof(Aftale).ToString()));
            Assert.That(aftaleProxy.DataIsLoaded, Is.False);
        }

        /// <summary>
        /// Tester, at UniqueId returnerer den unikke idenfikation for aftalen.
        /// </summary>
        [Test]
        public void TestAtUniqueIdReturnererIdentifikation()
        {
            var fixture = new Fixture();
            fixture.Inject(new AftaleProxy(1, 2));

            var aftaleProxy = fixture.CreateAnonymous<AftaleProxy>();
            Assert.That(aftaleProxy, Is.Not.Null);

            var uniqueId = aftaleProxy.UniqueId;
            Assert.That(uniqueId, Is.Not.Null);
            Assert.That(uniqueId, Is.EqualTo("1-2"));
        }

        /// <summary>
        /// Tester, at GetSqlQueryForId returnerer SQL foresprøgelse efter aftalen.
        /// </summary>
        [Test]
        public void TestAtGetSqlQueryForIdReturnererSqlQuery()
        {
            var fixture = new Fixture();
            fixture.Inject(new AftaleProxy(1, 2));

            var aftaleProxy = fixture.CreateAnonymous<AftaleProxy>();
            Assert.That(aftaleProxy, Is.Not.Null);

            var sqlQuery = aftaleProxy.GetSqlQueryForId(aftaleProxy);
            Assert.That(sqlQuery, Is.Not.Null);
            Assert.That(sqlQuery, Is.EqualTo("SELECT SystemNo,CalId,Date,FromTime,ToTime,Properties,Subject,Note FROM Calapps WHERE SystemNo=1 AND CalId=2"));
        }

        /// <summary>
        /// Tester, at GetSqlQueryForId kaster ArgumentNullException, hvis data proxy, der skal forespørges efter, er null.
        /// </summary>
        [Test]
        public void TestAtGetSqlQueryForIdKasterArgumentNullExceptionHvisQueryForDataProxyErNull()
        {
            var fixture = new Fixture();
            fixture.Inject(new AftaleProxy(1, 2));

            var aftaleProxy = fixture.CreateAnonymous<AftaleProxy>();
            Assert.That(aftaleProxy, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => aftaleProxy.GetSqlQueryForId(null));
        }

        /// <summary>
        /// Tester, at GetSqlCommandForInsert returnerer SQL kommando til oprettelse af aftalen.
        /// </summary>
        [Test]
        public void TestAtGetSqlCommandForInsertReturnererSqlCommand()
        {
            var fixture = new Fixture();
            var emne = fixture.CreateAnonymous<string>();
            var notat = fixture.CreateAnonymous<string>();
            var properties = fixture.CreateAnonymous<int>();
            fixture.Inject(new DateTime(2011, 10, 8, 12, 0, 0));
            fixture.Inject(new AftaleProxy(1, 2, fixture.CreateAnonymous<DateTime>(),
                                           fixture.CreateAnonymous<DateTime>().AddMinutes(15), emne, properties));

            var aftaleProxy = fixture.CreateAnonymous<AftaleProxy>();
            Assert.That(aftaleProxy, Is.Not.Null);
            aftaleProxy.Notat = notat;

            var sqlCommand = aftaleProxy.GetSqlCommandForInsert();
            Assert.That(sqlCommand, Is.Not.Null);
            Assert.That(sqlCommand, Is.EqualTo(string.Format("INSERT INTO Calapps (SystemNo,CalId,Date,FromTime,ToTime,Properties,Subject,Note) VALUES(1,2,'2011-10-08','12:00:00','12:15:00',{0},'{1}','{2}')", properties, emne, notat)));
        }

        /// <summary>
        /// Tester, at GetSqlCommandForUpdate returnerer SQL kommando til opdatering af aftalen.
        /// </summary>
        [Test]
        public void TestAtGetSqlCommandForUpdateReturnererSqlCommand()
        {
            var fixture = new Fixture();
            var emne = fixture.CreateAnonymous<string>();
            var notat = fixture.CreateAnonymous<string>();
            var properties = fixture.CreateAnonymous<int>();
            fixture.Inject(new DateTime(2011, 10, 8, 12, 0, 0));
            fixture.Inject(new AftaleProxy(1, 2, fixture.CreateAnonymous<DateTime>(),
                                           fixture.CreateAnonymous<DateTime>().AddMinutes(15), emne, properties));

            var aftaleProxy = fixture.CreateAnonymous<AftaleProxy>();
            Assert.That(aftaleProxy, Is.Not.Null);
            aftaleProxy.Notat = notat;

            var sqlCommand = aftaleProxy.GetSqlCommandForUpdate();
            Assert.That(sqlCommand, Is.Not.Null);
            Assert.That(sqlCommand, Is.EqualTo(string.Format("UPDATE Calapps SET Date='2011-10-08',FromTime='12:00:00',ToTime='12:15:00',Properties={0},Subject='{1}',Note='{2}' WHERE SystemNo=1 AND CalId=2", properties, emne, notat)));
        }

        /// <summary>
        /// Tester, at GetSqlCommandForDelete returnerer SQL kommando til sletning af aftalen.
        /// </summary>
        [Test]
        public void TestAtGetSqlCommandForDeleteReturnererSqlCommand()
        {
            var fixture = new Fixture();
            fixture.Inject(new AftaleProxy(1, 2));

            var aftaleProxy = fixture.CreateAnonymous<AftaleProxy>();
            Assert.That(aftaleProxy, Is.Not.Null);

            var sqlCommand = aftaleProxy.GetSqlCommandForDelete();
            Assert.That(sqlCommand, Is.Not.Null);
            Assert.That(sqlCommand, Is.EqualTo("DELETE FROM Calapps WHERE SystemNo=1 AND CalId=2"));
        }

        /// <summary>
        /// Tester, at MapData kaster en ArgumentNullException, hvis data reader er null.
        /// </summary>
        [Test]
        public void TestAtMapDataKasterArgumentNullExceptionHvisDataReaderErNull()
        {
            var fixture = new Fixture();
            fixture.Inject(new AftaleProxy());
            fixture.Inject<IDataReader>(null);
            fixture.Inject(MockRepository.GenerateMock<IDataProviderBase>());

            var aftaleProxy = fixture.CreateAnonymous<AftaleProxy>();
            Assert.That(aftaleProxy, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(
                () =>
                aftaleProxy.MapData(fixture.CreateAnonymous<IDataReader>(), fixture.CreateAnonymous<IDataProviderBase>()));
        }

        /// <summary>
        /// Tester, at MapData kaster en ArgumentNullException, hvis data provider er null.
        /// </summary>
        [Test]
        public void TestAtMapDataKasterArgumentNullExceptionHvisDataProviderErNull()
        {
            var fixture = new Fixture();
            fixture.Inject(new AftaleProxy());
            fixture.Inject(MockRepository.GenerateMock<IDataReader>());
            fixture.Inject<IDataProviderBase>(null);

            var aftaleProxy = fixture.CreateAnonymous<AftaleProxy>();
            Assert.That(aftaleProxy, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(
                () =>
                aftaleProxy.MapData(fixture.CreateAnonymous<IDataReader>(), fixture.CreateAnonymous<IDataProviderBase>()));
        }

        /// <summary>
        /// Tester, at MapData kaster en IntranetRepositoryException, hvis data reader ikke er af typen MySqlDataReader.
        /// </summary>
        [Test]
        public void TestAtMapDataKasterIntranetRepositoryExceptionHvisDataReaderIkkeErMySqlDataReader()
        {
            var fixture = new Fixture();
            fixture.Inject(new AftaleProxy());
            fixture.Inject(MockRepository.GenerateMock<IDataReader>());
            fixture.Inject(MockRepository.GenerateMock<IDataProviderBase>());

            var aftaleProxy = fixture.CreateAnonymous<AftaleProxy>();
            Assert.That(aftaleProxy, Is.Not.Null);

            Assert.Throws<IntranetRepositoryException>(
                () =>
                aftaleProxy.MapData(fixture.CreateAnonymous<IDataReader>(), fixture.CreateAnonymous<IDataProviderBase>()));
        }

        /// <summary>
        /// Tester, at MapData mapper data proxy for en aftale.
        /// </summary>
        [Test]
        public void TestAtMapDataMapperAftaleProxy()
        {
            var fixture = new Fixture();
            fixture.Inject(new MySqlDateTime(2011, 10, 8, 0, 0, 0));
            fixture.Inject(new TimeSpan(12, 0, 0));
            fixture.Inject(new AftaleProxy());

            var systemNo = fixture.CreateAnonymous<int>();
            var calId = fixture.CreateAnonymous<int>();

            var dataReader = MockRepository.GenerateStub<MySqlDataReader>();
            dataReader.Expect(m => m.GetInt32(Arg<string>.Is.Equal("SystemNo")))
                .Return(systemNo)
                .Repeat.Any();
            dataReader.Expect(m => m.GetInt32(Arg<string>.Is.Equal("CalId")))
                .Return(calId)
                .Repeat.Any();
            dataReader.Expect(m => m.GetMySqlDateTime(Arg<string>.Is.Equal("Date")))
                .Return(fixture.CreateAnonymous<MySqlDateTime>());
            dataReader.Expect(m => m.GetTimeSpan(Arg<string>.Is.Equal("FromTime")))
                .Return(fixture.CreateAnonymous<TimeSpan>());
            dataReader.Expect(m => m.GetTimeSpan(Arg<string>.Is.Equal("ToTime")))
                .Return(fixture.CreateAnonymous<TimeSpan>().Add(new TimeSpan(0, 15, 0)));
            dataReader.Expect(m => m.GetInt32(Arg<string>.Is.Equal("Properties")))
                .Return(fixture.CreateAnonymous<int>());
            dataReader.Expect(m => m.GetString(Arg<string>.Is.Equal("Subject")))
                .Return(fixture.CreateAnonymous<string>());
            dataReader.Expect(m => m.GetString(Arg<string>.Is.Equal("Note")))
                .Return(fixture.CreateAnonymous<string>());
            fixture.Inject(dataReader);

            fixture.Customize<BrugeraftaleProxy>(m => m.FromFactory(() => new BrugeraftaleProxy(systemNo, calId, fixture.CreateAnonymous<int>())));
            var dataProvider = MockRepository.GenerateMock<IDataProviderBase>();
            dataProvider.Expect(m => m.GetCollection<BrugeraftaleProxy>(Arg<string>.Is.NotNull))
                .Return(fixture.CreateMany<BrugeraftaleProxy>(3));
            fixture.Inject(dataProvider);

            var aftaleProxy = fixture.CreateAnonymous<AftaleProxy>();
            Assert.That(aftaleProxy, Is.Not.Null);

            aftaleProxy.MapData(fixture.CreateAnonymous<MySqlDataReader>(), fixture.CreateAnonymous<IDataProviderBase>());
            Assert.That(aftaleProxy.DataIsLoaded, Is.True);

            dataReader.AssertWasCalled(m => m.GetInt32(Arg<string>.Is.Equal("SystemNo")), opt => opt.Repeat.Times(2));
            dataReader.AssertWasCalled(m => m.GetInt32(Arg<string>.Is.Equal("CalId")), opt => opt.Repeat.Times(2));
            dataReader.AssertWasCalled(m => m.GetMySqlDateTime(Arg<string>.Is.Equal("Date")));
            dataReader.AssertWasCalled(m => m.GetTimeSpan(Arg<string>.Is.Equal("FromTime")));
            dataReader.AssertWasCalled(m => m.GetTimeSpan(Arg<string>.Is.Equal("ToTime")));
            dataReader.AssertWasCalled(m => m.GetInt32(Arg<string>.Is.Equal("Properties")));
            dataReader.AssertWasCalled(m => m.GetString(Arg<string>.Is.Equal("Subject")));
            dataReader.AssertWasCalled(m => m.GetString(Arg<string>.Is.Equal("Note")));

            dataProvider.AssertWasCalled(m => m.GetCollection<BrugeraftaleProxy>(Arg<string>.Is.NotNull));
        }

        /// <summary>
        /// Tester, at System lazy loades.
        /// </summary>
        [Test]
        public void TestAtSystemLazyLoades()
        {
            var fixture = new Fixture();
            fixture.Inject(new MySqlDateTime(2011, 10, 8, 0, 0, 0));
            fixture.Inject(new TimeSpan(12, 0, 0));
            fixture.Inject(new AftaleProxy());

            var systemNo = fixture.CreateAnonymous<int>();
            var calId = fixture.CreateAnonymous<int>();

            var dataReader = MockRepository.GenerateStub<MySqlDataReader>();
            dataReader.Expect(m => m.GetInt32(Arg<string>.Is.Equal("SystemNo")))
                .Return(systemNo)
                .Repeat.Any();
            dataReader.Expect(m => m.GetInt32(Arg<string>.Is.Equal("CalId")))
                .Return(calId)
                .Repeat.Any();
            dataReader.Expect(m => m.GetMySqlDateTime(Arg<string>.Is.Equal("Date")))
                .Return(fixture.CreateAnonymous<MySqlDateTime>());
            dataReader.Expect(m => m.GetTimeSpan(Arg<string>.Is.Equal("FromTime")))
                .Return(fixture.CreateAnonymous<TimeSpan>());
            dataReader.Expect(m => m.GetTimeSpan(Arg<string>.Is.Equal("ToTime")))
                .Return(fixture.CreateAnonymous<TimeSpan>().Add(new TimeSpan(0, 15, 0)));
            dataReader.Expect(m => m.GetInt32(Arg<string>.Is.Equal("Properties")))
                .Return(fixture.CreateAnonymous<int>());
            dataReader.Expect(m => m.GetString(Arg<string>.Is.Equal("Subject")))
                .Return(fixture.CreateAnonymous<string>());
            dataReader.Expect(m => m.GetString(Arg<string>.Is.Equal("Note")))
                .Return(fixture.CreateAnonymous<string>());
            fixture.Inject(dataReader);

            fixture.Customize<BrugeraftaleProxy>(m => m.FromFactory(() => new BrugeraftaleProxy(systemNo, calId, fixture.CreateAnonymous<int>())));
            var dataProvider = MockRepository.GenerateMock<IDataProviderBase>();
            dataProvider.Expect(m => m.GetCollection<BrugeraftaleProxy>(Arg<string>.Is.NotNull))
                .Return(fixture.CreateMany<BrugeraftaleProxy>(3));
            dataProvider.Expect(m => m.Get(Arg<SystemProxy>.Is.NotNull))
                .Return(fixture.CreateAnonymous<SystemProxy>());
            fixture.Inject(dataProvider);

            var aftaleProxy = fixture.CreateAnonymous<AftaleProxy>();
            Assert.That(aftaleProxy, Is.Not.Null);

            aftaleProxy.MapData(fixture.CreateAnonymous<MySqlDataReader>(), fixture.CreateAnonymous<IDataProviderBase>());
            Assert.That(aftaleProxy.DataIsLoaded, Is.True);

            Assert.That(aftaleProxy.System, Is.Not.Null);

            dataReader.AssertWasCalled(m => m.GetInt32(Arg<string>.Is.Equal("SystemNo")), opt => opt.Repeat.Times(2));
            dataReader.AssertWasCalled(m => m.GetInt32(Arg<string>.Is.Equal("CalId")), opt => opt.Repeat.Times(2));
            dataReader.AssertWasCalled(m => m.GetMySqlDateTime(Arg<string>.Is.Equal("Date")));
            dataReader.AssertWasCalled(m => m.GetTimeSpan(Arg<string>.Is.Equal("FromTime")));
            dataReader.AssertWasCalled(m => m.GetTimeSpan(Arg<string>.Is.Equal("ToTime")));
            dataReader.AssertWasCalled(m => m.GetInt32(Arg<string>.Is.Equal("Properties")));
            dataReader.AssertWasCalled(m => m.GetString(Arg<string>.Is.Equal("Subject")));
            dataReader.AssertWasCalled(m => m.GetString(Arg<string>.Is.Equal("Note")));

            dataProvider.AssertWasCalled(m => m.GetCollection<BrugeraftaleProxy>(Arg<string>.Is.NotNull));
            dataProvider.AssertWasCalled(m => m.Get(Arg<SystemProxy>.Is.NotNull));
        }
    }
}
