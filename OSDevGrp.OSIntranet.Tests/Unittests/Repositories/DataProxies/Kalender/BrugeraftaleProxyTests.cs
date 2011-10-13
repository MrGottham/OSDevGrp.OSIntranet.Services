using System;
using System.Data;
using OSDevGrp.OSIntranet.Domain.Kalender;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.DataProxies.Fælles;
using OSDevGrp.OSIntranet.Repositories.DataProxies.Kalender;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.DataProxies.Kalender
{
    /// <summary>
    /// Tester data proxy for en brugers kalenderaftale.
    /// </summary>
    [TestFixture]
    public class BrugeraftaleProxyTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en data proxy for en brugeraftale.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererBrugeraftaleProxy()
        {
            var fixture = new Fixture();
            fixture.Inject(new BrugeraftaleProxy());

            var brugeraftaleProxy = fixture.CreateAnonymous<BrugeraftaleProxy>();
            Assert.That(brugeraftaleProxy, Is.Not.Null);
            Assert.That(brugeraftaleProxy.System, Is.Not.Null);
            Assert.That(brugeraftaleProxy.System.Nummer, Is.EqualTo(0));
            Assert.That(brugeraftaleProxy.Aftale, Is.Not.Null);
            Assert.That(brugeraftaleProxy.Aftale.Id, Is.EqualTo(0));
            Assert.That(brugeraftaleProxy.Bruger, Is.Not.Null);
            Assert.That(brugeraftaleProxy.Bruger.Id, Is.EqualTo(0));
            Assert.That(brugeraftaleProxy.DataIsLoaded, Is.False);
        }

        /// <summary>
        /// Tester, at UniqueId returnerer den unikke idenfikation for brugeraftalen.
        /// </summary>
        [Test]
        public void TestAtUniqueIdReturnererIdentifikation()
        {
            var fixture = new Fixture();
            fixture.Inject(new BrugeraftaleProxy(1, 2, 3));

            var brugeraftaleProxy = fixture.CreateAnonymous<BrugeraftaleProxy>();
            Assert.That(brugeraftaleProxy, Is.Not.Null);

            var uniqueId = brugeraftaleProxy.UniqueId;
            Assert.That(uniqueId, Is.Not.Null);
            Assert.That(uniqueId, Is.EqualTo("1-2-3"));
        }

        /// <summary>
        /// Tester, at GetSqlQueryForId returnerer SQL foresprøgelse efter brugeraftalen.
        /// </summary>
        [Test]
        public void TestAtGetSqlQueryForIdReturnererSqlQuery()
        {
            var fixture = new Fixture();
            fixture.Inject(new BrugeraftaleProxy(1, 2, 3));

            var brugeraftaleProxy = fixture.CreateAnonymous<BrugeraftaleProxy>();
            Assert.That(brugeraftaleProxy, Is.Not.Null);

            var sqlQuery = brugeraftaleProxy.GetSqlQueryForId(brugeraftaleProxy);
            Assert.That(sqlQuery, Is.Not.Null);
            Assert.That(sqlQuery, Is.EqualTo("SELECT SystemNo,CalId,UserId,Properties FROM Calmerge WHERE SystemNo=1 AND CalId=2 AND UserId=3"));
        }

        /// <summary>
        /// Tester, at GetSqlQueryForId kaster ArgumentNullException, hvis data proxy, der skal forespørges efter, er null.
        /// </summary>
        [Test]
        public void TestAtGetSqlQueryForIdKasterArgumentNullExceptionHvisQueryForDataProxyErNull()
        {
            var fixture = new Fixture();
            fixture.Inject(new BrugeraftaleProxy(1, 2, 3));

            var brugeraftaleProxy = fixture.CreateAnonymous<BrugeraftaleProxy>();
            Assert.That(brugeraftaleProxy, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => brugeraftaleProxy.GetSqlQueryForId(null));
        }

        /// <summary>
        /// Tester, at GetSqlCommandForInsert returnerer SQL kommando til oprettelse af brugeraftalen.
        /// </summary>
        [Test]
        public void TestAtGetSqlCommandForInsertReturnererSqlCommand()
        {
            var fixture = new Fixture();
            var properties = fixture.CreateAnonymous<int>();
            fixture.Inject(new BrugeraftaleProxy(1, 2, 3, properties));

            var brugeraftaleProxy = fixture.CreateAnonymous<BrugeraftaleProxy>();
            Assert.That(brugeraftaleProxy, Is.Not.Null);

            var sqlCommand = brugeraftaleProxy.GetSqlCommandForInsert();
            Assert.That(sqlCommand, Is.Not.Null);
            Assert.That(sqlCommand, Is.EqualTo(string.Format("INSERT INTO Calmerge (SystemNo,CalId,UserId,Properties) VALUES(1,2,3,{0})", properties)));
        }

        /// <summary>
        /// Tester, at GetSqlCommandForUpdate returnerer SQL kommando til opdatering af brugeraftalen.
        /// </summary>
        [Test]
        public void TestAtGetSqlCommandForUpdateReturnererSqlCommand()
        {
            var fixture = new Fixture();
            var properties = fixture.CreateAnonymous<int>();
            fixture.Inject(new BrugeraftaleProxy(1, 2, 3, properties));

            var brugeraftaleProxy = fixture.CreateAnonymous<BrugeraftaleProxy>();
            Assert.That(brugeraftaleProxy, Is.Not.Null);

            var sqlCommand = brugeraftaleProxy.GetSqlCommandForUpdate();
            Assert.That(sqlCommand, Is.Not.Null);
            Assert.That(sqlCommand, Is.EqualTo(string.Format("UPDATE Calmerge SET Properties={0} WHERE SystemNo=1 AND CalId=2 AND UserId=3", properties)));
        }

        /// <summary>
        /// Tester, at GetSqlCommandForDelete returnerer SQL kommando til sletning af brugeraftalen.
        /// </summary>
        [Test]
        public void TestAtGetSqlCommandForDeleteReturnererSqlCommand()
        {
            var fixture = new Fixture();
            fixture.Inject(new BrugeraftaleProxy(1, 2, 3));

            var brugeraftaleProxy = fixture.CreateAnonymous<BrugeraftaleProxy>();
            Assert.That(brugeraftaleProxy, Is.Not.Null);

            var sqlCommand = brugeraftaleProxy.GetSqlCommandForDelete();
            Assert.That(sqlCommand, Is.Not.Null);
            Assert.That(sqlCommand, Is.EqualTo("DELETE FROM Calmerge WHERE SystemNo=1 AND CalId=2 AND UserId=3"));
        }

        /// <summary>
        /// Tester, at MapData kaster en ArgumentNullException, hvis data reader er null.
        /// </summary>
        [Test]
        public void TestAtMapDataKasterArgumentNullExceptionHvisDataReaderErNull()
        {
            var fixture = new Fixture();
            fixture.Inject(new BrugeraftaleProxy());
            fixture.Inject<IDataReader>(null);
            fixture.Inject(MockRepository.GenerateMock<IDataProviderBase>());

            var brugeraftaleProxy = fixture.CreateAnonymous<BrugeraftaleProxy>();
            Assert.That(brugeraftaleProxy, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(
                () =>
                brugeraftaleProxy.MapData(fixture.CreateAnonymous<IDataReader>(),
                                          fixture.CreateAnonymous<IDataProviderBase>()));
        }

        /// <summary>
        /// Tester, at MapData kaster en ArgumentNullException, hvis data provider er null.
        /// </summary>
        [Test]
        public void TestAtMapDataKasterArgumentNullExceptionHvisDataProviderErNull()
        {
            var fixture = new Fixture();
            fixture.Inject(new BrugeraftaleProxy());
            fixture.Inject(MockRepository.GenerateMock<IDataReader>());
            fixture.Inject<IDataProviderBase>(null);

            var brugeraftaleProxy = fixture.CreateAnonymous<BrugeraftaleProxy>();
            Assert.That(brugeraftaleProxy, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(
                () =>
                brugeraftaleProxy.MapData(fixture.CreateAnonymous<IDataReader>(),
                                          fixture.CreateAnonymous<IDataProviderBase>()));
        }

        /// <summary>
        /// Tester, at MapData kaster en IntranetRepositoryException, hvis data reader ikke er af typen MySqlDataReader.
        /// </summary>
        [Test]
        public void TestAtMapDataKasterIntranetRepositoryExceptionHvisDataReaderIkkeErMySqlDataReader()
        {
            var fixture = new Fixture();
            fixture.Inject(new BrugeraftaleProxy());
            fixture.Inject(MockRepository.GenerateMock<IDataReader>());
            fixture.Inject(MockRepository.GenerateMock<IDataProviderBase>());

            var brugeraftaleProxy = fixture.CreateAnonymous<BrugeraftaleProxy>();
            Assert.That(brugeraftaleProxy, Is.Not.Null);

            Assert.Throws<IntranetRepositoryException>(
                () =>
                brugeraftaleProxy.MapData(fixture.CreateAnonymous<IDataReader>(),
                                          fixture.CreateAnonymous<IDataProviderBase>()));
        }
    }
}
