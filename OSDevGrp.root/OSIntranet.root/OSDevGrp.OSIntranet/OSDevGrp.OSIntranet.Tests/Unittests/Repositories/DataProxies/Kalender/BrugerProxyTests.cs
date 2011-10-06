using System;
using System.Data;
using OSDevGrp.OSIntranet.Domain.Kalender;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.DataProxies.Kalender;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.DataProxies.Kalender
{
    /// <summary>
    /// Tester data proxy for en kalenderbruger under OSWEBDB.
    /// </summary>
    [TestFixture]
    public class BrugerProxyTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en data proxy for en bruger.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererBrugerProxy()
        {
            var fixture = new Fixture();
            fixture.Inject(new BrugerProxy());

            var brugerProxy = fixture.CreateAnonymous<BrugerProxy>();
            Assert.That(brugerProxy, Is.Not.Null);
            Assert.That(brugerProxy.System, Is.Not.Null);
            Assert.That(brugerProxy.System.Nummer, Is.EqualTo(0));
            Assert.That(brugerProxy.Id, Is.EqualTo(0));
            Assert.That(brugerProxy.Initialer, Is.Not.Null);
            Assert.That(brugerProxy.Initialer, Is.EqualTo(typeof(Bruger).ToString()));
            Assert.That(brugerProxy.Navn, Is.Not.Null);
            Assert.That(brugerProxy.Navn, Is.EqualTo(typeof(Bruger).ToString()));
            Assert.That(brugerProxy.DataIsLoaded, Is.False);
        }

        /// <summary>
        /// Tester, at UniqueId returnerer den unikke idenfikation for brugeren.
        /// </summary>
        [Test]
        public void TestAtUniqueIdReturnererIdentifikation()
        {
            var fixture = new Fixture();
            fixture.Inject(new BrugerProxy(1, 2));

            var brugerProxy = fixture.CreateAnonymous<BrugerProxy>();
            Assert.That(brugerProxy, Is.Not.Null);

            var uniqueId = brugerProxy.UniqueId;
            Assert.That(uniqueId, Is.Not.Null);
            Assert.That(uniqueId, Is.EqualTo("1-2"));
        }

        /// <summary>
        /// Tester, at GetSqlQueryForId returnerer SQL foresprøgelse efter brugeren.
        /// </summary>
        [Test]
        public void TestAtGetSqlQueryForIdReturnererSqlQuery()
        {
            var fixture = new Fixture();
            fixture.Inject(new BrugerProxy(1, 2));

            var brugerProxy = fixture.CreateAnonymous<BrugerProxy>();
            Assert.That(brugerProxy, Is.Not.Null);

            var sqlQuery = brugerProxy.GetSqlQueryForId(brugerProxy);
            Assert.That(sqlQuery, Is.Not.Null);
            Assert.That(sqlQuery, Is.EqualTo("SELECT SystemNo,UserId,UserName,Name,Initials FROM Calusers WHERE SystemNo=1 AND UserId=2"));
        }

        /// <summary>
        /// Tester, at GetSqlCommandForInsert returnerer SQL kommando til oprettelse af brugeren.
        /// </summary>
        [Test]
        public void TestAtGetSqlCommandForInsertReturnererSqlCommand()
        {
            var fixture = new Fixture();
            var userName = fixture.CreateAnonymous<string>();
            var navn = fixture.CreateAnonymous<string>();
            var initialer = fixture.CreateAnonymous<string>();
            fixture.Inject(new BrugerProxy(1, 2, initialer, navn));

            var brugerProxy = fixture.CreateAnonymous<BrugerProxy>();
            Assert.That(brugerProxy, Is.Not.Null);
            brugerProxy.UserName = userName;

            var sqlCommand = brugerProxy.GetSqlCommandForInsert();
            Assert.That(sqlCommand, Is.Not.Null);
            Assert.That(sqlCommand, Is.EqualTo(string.Format("INSERT INTO Calusers (SystemNo,UserId,UserName,Name,Initials) VALUES(1,2,'{0}','{1}','{2}')", userName, navn, initialer)));
        }

        /// <summary>
        /// Tester, at GetSqlCommandForUpdate returnerer SQL kommando til opdatering af brugeren.
        /// </summary>
        [Test]
        public void TestAtGetSqlCommandForUpdateReturnererSqlCommand()
        {
            var fixture = new Fixture();
            var userName = fixture.CreateAnonymous<string>();
            var navn = fixture.CreateAnonymous<string>();
            var initialer = fixture.CreateAnonymous<string>();
            fixture.Inject(new BrugerProxy(1, 2, initialer, navn));

            var brugerProxy = fixture.CreateAnonymous<BrugerProxy>();
            Assert.That(brugerProxy, Is.Not.Null);
            brugerProxy.UserName = userName;

            var sqlCommand = brugerProxy.GetSqlCommandForUpdate();
            Assert.That(sqlCommand, Is.Not.Null);
            Assert.That(sqlCommand, Is.EqualTo(string.Format("UPDATE Calusers SET UserName='{0}',Name='{1}',Initials='{2}' WHERE SystemNo=1 AND UserId=2", userName, navn, initialer)));
        }

        /// <summary>
        /// Tester, at GetSqlCommandForDelete returnerer SQL kommando til sletning af brugeren.
        /// </summary>
        [Test]
        public void TestAtGetSqlCommandForDeleteReturnererSqlCommand()
        {
            var fixture = new Fixture();
            fixture.Inject(new BrugerProxy(1, 2));

            var brugerProxy = fixture.CreateAnonymous<BrugerProxy>();
            Assert.That(brugerProxy, Is.Not.Null);

            var sqlCommand = brugerProxy.GetSqlCommandForDelete();
            Assert.That(sqlCommand, Is.Not.Null);
            Assert.That(sqlCommand, Is.EqualTo("DELETE FROM Calusers WHERE SystemNo=1 AND UserId=2"));
        }

        /// <summary>
        /// Tester, at MapData kaster en ArgumentNullException, hvis data reader er null.
        /// </summary>
        [Test]
        public void TestAtMapDataKasterArgumentNullExceptionHvisDataReaderErNull()
        {
            var fixture = new Fixture();
            fixture.Inject(new BrugerProxy());
            fixture.Inject<object>(null);
            fixture.Inject(MockRepository.GenerateMock<IDataProviderBase>());

            var brugerProxy = fixture.CreateAnonymous<BrugerProxy>();
            Assert.That(brugerProxy, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(
                () =>
                brugerProxy.MapData(fixture.CreateAnonymous<object>(), fixture.CreateAnonymous<IDataProviderBase>()));

        }

        /// <summary>
        /// Tester, at MapData kaster en ArgumentNullException, hvis data provider er null.
        /// </summary>
        [Test]
        public void TestAtMapDataKasterArgumentNullExceptionHvisDataProviderErNull()
        {
            var fixture = new Fixture();
            fixture.Inject(new BrugerProxy());
            fixture.Inject(new object());
            fixture.Inject<IDataProviderBase>(null);

            var brugerProxy = fixture.CreateAnonymous<BrugerProxy>();
            Assert.That(brugerProxy, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(
                () =>
                brugerProxy.MapData(fixture.CreateAnonymous<object>(), fixture.CreateAnonymous<IDataProviderBase>()));

        }

        /// <summary>
        /// Tester, at MapData kaster en IntranetRepositoryException, hvis data reader ikke er af typen MySqlDataReader.
        /// </summary>
        [Test]
        public void TestAtMapDataKasterIntranetRepositoryExceptionHvisDataReaderIkkeErMySqlDataReader()
        {
            var fixture = new Fixture();
            fixture.Inject(new BrugerProxy());
            fixture.Inject(MockRepository.GenerateMock<IDataReader>());
            fixture.Inject(MockRepository.GenerateMock<IDataProviderBase>());

            var brugerProxy = fixture.CreateAnonymous<BrugerProxy>();
            Assert.That(brugerProxy, Is.Not.Null);

            Assert.Throws<IntranetRepositoryException>(
                () =>
                brugerProxy.MapData(fixture.CreateAnonymous<IDataReader>(), fixture.CreateAnonymous<IDataProviderBase>()));
        }
    }
}
