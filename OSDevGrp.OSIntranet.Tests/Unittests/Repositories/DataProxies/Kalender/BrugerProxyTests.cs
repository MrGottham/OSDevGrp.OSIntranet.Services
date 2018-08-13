using System;
using System.Data;
using OSDevGrp.OSIntranet.Domain.Kalender;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.DataProxies.Fælles;
using OSDevGrp.OSIntranet.Repositories.DataProxies.Kalender;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using AutoFixture;
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

            var brugerProxy = fixture.Create<BrugerProxy>();
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

            var brugerProxy = fixture.Create<BrugerProxy>();
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

            var brugerProxy = fixture.Create<BrugerProxy>();
            Assert.That(brugerProxy, Is.Not.Null);

            var sqlQuery = brugerProxy.GetSqlQueryForId(brugerProxy);
            Assert.That(sqlQuery, Is.Not.Null);
            Assert.That(sqlQuery, Is.EqualTo("SELECT SystemNo,UserId,UserName,Name,Initials FROM Calusers WHERE SystemNo=1 AND UserId=2"));
        }

        /// <summary>
        /// Tester, at GetSqlQueryForId kaster ArgumentNullException, hvis data proxy, der skal forespørges efter, er null.
        /// </summary>
        [Test]
        public void TestAtGetSqlQueryForIdKasterArgumentNullExceptionHvisQueryForDataProxyErNull()
        {
            var fixture = new Fixture();
            fixture.Inject(new BrugerProxy(1, 2));

            var brugerProxy = fixture.Create<BrugerProxy>();
            Assert.That(brugerProxy, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => brugerProxy.GetSqlQueryForId(null));
        }

        /// <summary>
        /// Tester, at GetSqlCommandForInsert returnerer SQL kommando til oprettelse af brugeren.
        /// </summary>
        [Test]
        public void TestAtGetSqlCommandForInsertReturnererSqlCommand()
        {
            var fixture = new Fixture();
            var userName = fixture.Create<string>();
            var navn = fixture.Create<string>();
            var initialer = fixture.Create<string>();
            fixture.Inject(new BrugerProxy(1, 2, initialer, navn));

            var brugerProxy = fixture.Create<BrugerProxy>();
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
            var userName = fixture.Create<string>();
            var navn = fixture.Create<string>();
            var initialer = fixture.Create<string>();
            fixture.Inject(new BrugerProxy(1, 2, initialer, navn));

            var brugerProxy = fixture.Create<BrugerProxy>();
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

            var brugerProxy = fixture.Create<BrugerProxy>();
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

            var brugerProxy = fixture.Create<BrugerProxy>();
            Assert.That(brugerProxy, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(
                () =>
                brugerProxy.MapData(fixture.Create<object>(), fixture.Create<IDataProviderBase>()));

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

            var brugerProxy = fixture.Create<BrugerProxy>();
            Assert.That(brugerProxy, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(
                () =>
                brugerProxy.MapData(fixture.Create<object>(), fixture.Create<IDataProviderBase>()));

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

            var brugerProxy = fixture.Create<BrugerProxy>();
            Assert.That(brugerProxy, Is.Not.Null);

            Assert.Throws<IntranetRepositoryException>(
                () =>
                brugerProxy.MapData(fixture.Create<IDataReader>(), fixture.Create<IDataProviderBase>()));
        }

        /// <summary>
        /// Tester, at MapData mapper data proxy for en bruger.
        /// </summary>
        [Test]
        public void TestAtMapDataMapperBrugerProxy()
        {
            var fixture = new Fixture();
            fixture.Inject(new BrugerProxy());
            fixture.Inject(MockRepository.GenerateMock<IDataProviderBase>());

            var dataReader = MockRepository.GenerateStub<MySqlDataReader>();
            dataReader.Expect(m => m.GetInt32(Arg<string>.Is.Equal("SystemNo")))
                .Return(fixture.Create<int>());
            dataReader.Expect(m => m.GetInt32(Arg<string>.Is.Equal("UserId")))
                .Return(fixture.Create<int>());
            dataReader.Expect(m => m.GetString(Arg<string>.Is.Equal("UserName")))
                .Return(fixture.Create<string>());
            dataReader.Expect(m => m.GetString(Arg<string>.Is.Equal("Name")))
                .Return(fixture.Create<string>());
            dataReader.Expect(m => m.GetString(Arg<string>.Is.Equal("Initials")))
                .Return(fixture.Create<string>());
            fixture.Inject(dataReader);

            var brugerProxy = fixture.Create<BrugerProxy>();
            Assert.That(brugerProxy, Is.Not.Null);

            brugerProxy.MapData(fixture.Create<MySqlDataReader>(), fixture.Create<IDataProviderBase>());
            Assert.That(brugerProxy.DataIsLoaded, Is.True);

            dataReader.AssertWasCalled(m => m.GetInt32(Arg<string>.Is.Equal("SystemNo")));
            dataReader.AssertWasCalled(m => m.GetInt32(Arg<string>.Is.Equal("UserId")));
            dataReader.AssertWasCalled(m => m.GetString(Arg<string>.Is.Equal("UserName")));
            dataReader.AssertWasCalled(m => m.GetString(Arg<string>.Is.Equal("Name")));
            dataReader.AssertWasCalled(m => m.GetString(Arg<string>.Is.Equal("Initials")));
        }

        /// <summary>
        /// Tester, at System lazy loades.
        /// </summary>
        [Test]
        public void TestAtSystemLazyLoades()
        {
            var fixture = new Fixture();
            fixture.Inject(new BrugerProxy());

            var dataReader = MockRepository.GenerateStub<MySqlDataReader>();
            dataReader.Expect(m => m.GetInt32(Arg<string>.Is.Equal("SystemNo")))
                .Return(fixture.Create<int>());
            dataReader.Expect(m => m.GetInt32(Arg<string>.Is.Equal("UserId")))
                .Return(fixture.Create<int>());
            dataReader.Expect(m => m.GetString(Arg<string>.Is.Equal("UserName")))
                .Return(fixture.Create<string>());
            dataReader.Expect(m => m.GetString(Arg<string>.Is.Equal("Name")))
                .Return(fixture.Create<string>());
            dataReader.Expect(m => m.GetString(Arg<string>.Is.Equal("Initials")))
                .Return(fixture.Create<string>());
            fixture.Inject(dataReader);

            var dataProvider = MockRepository.GenerateMock<IDataProviderBase>();
            dataProvider.Expect(m => m.Get(Arg<SystemProxy>.Is.NotNull))
                .Return(fixture.Create<SystemProxy>());
            fixture.Inject(dataProvider);

            var brugerProxy = fixture.Create<BrugerProxy>();
            Assert.That(brugerProxy, Is.Not.Null);

            brugerProxy.MapData(fixture.Create<MySqlDataReader>(), fixture.Create<IDataProviderBase>());
            Assert.That(brugerProxy.DataIsLoaded, Is.True);

            Assert.That(brugerProxy.System, Is.Not.Null);

            dataReader.AssertWasCalled(m => m.GetInt32(Arg<string>.Is.Equal("SystemNo")));
            dataReader.AssertWasCalled(m => m.GetInt32(Arg<string>.Is.Equal("UserId")));
            dataReader.AssertWasCalled(m => m.GetString(Arg<string>.Is.Equal("UserName")));
            dataReader.AssertWasCalled(m => m.GetString(Arg<string>.Is.Equal("Name")));
            dataReader.AssertWasCalled(m => m.GetString(Arg<string>.Is.Equal("Initials")));

            dataProvider.AssertWasCalled(m => m.Get(Arg<SystemProxy>.Is.NotNull));
        }
    }
}
