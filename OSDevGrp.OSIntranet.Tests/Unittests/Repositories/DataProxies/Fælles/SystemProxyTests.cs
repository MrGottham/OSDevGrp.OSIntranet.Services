using System;
using System.Data;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.DataProxies.Fælles;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.DataProxies.Fælles
{
    /// <summary>
    /// Tester data proxy for et system under OSWEBDB.
    /// </summary>
    [TestFixture]
    public class SystemProxyTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en data proxy for et system under OSWEBDB.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererSystemProxy()
        {
            var fixture = new Fixture();
            fixture.Inject(new SystemProxy());

            var systemProxy = fixture.Create<SystemProxy>();
            Assert.That(systemProxy, Is.Not.Null);
            Assert.That(systemProxy.Nummer, Is.EqualTo(0));
            Assert.That(systemProxy.Titel, Is.Not.Null);
            Assert.That(systemProxy.Titel, Is.EqualTo(typeof(OSIntranet.Domain.Fælles.System).ToString()));
            Assert.That(systemProxy.Properties, Is.EqualTo(0));
            Assert.That(systemProxy.Kalender, Is.False);
            Assert.That(systemProxy.DataIsLoaded, Is.False);
        }

        /// <summary>
        /// Tester, at UniqueId returnerer den unikke identifikation af systemet under OSWEBDB.
        /// </summary>
        [Test]
        public void TestAtUniqueIdReturnererIdentifikation()
        {
            var fixture = new Fixture();
            fixture.Inject(new SystemProxy());

            var systemProxy = fixture.Create<SystemProxy>();
            Assert.That(systemProxy, Is.Not.Null);

            Assert.That(systemProxy.UniqueId, Is.Not.Null);
            Assert.That(systemProxy.UniqueId, Is.EqualTo("0"));
        }

        /// <summary>
        /// Tester, at GetSqlQueryForId returnerer SQL foresprøgelse efter systemet under OSWEBDB.
        /// </summary>
        [Test]
        public void TestAtGetSqlQueryForIdReturnererSqlQuery()
        {
            var fixture = new Fixture();
            fixture.Inject(new SystemProxy(1));

            var systemProxy = fixture.Create<SystemProxy>();
            Assert.That(systemProxy, Is.Not.Null);

            var sqlQuery = systemProxy.GetSqlQueryForId(systemProxy);
            Assert.That(sqlQuery, Is.Not.Null);
            Assert.That(sqlQuery, Is.EqualTo("SELECT SystemNo,Title,Properties FROM Systems WHERE SystemNo=1"));
        }

        /// <summary>
        /// Tester, at GetSqlQueryForId kaster ArgumentNullException, hvis data proxy, der skal forespørges efter, er null.
        /// </summary>
        [Test]
        public void TestAtGetSqlQueryForIdKasterArgumentNullExceptionHvisQueryForDataProxyErNull()
        {
            var fixture = new Fixture();
            fixture.Inject(new SystemProxy(1));

            var systemProxy = fixture.Create<SystemProxy>();
            Assert.That(systemProxy, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => systemProxy.GetSqlQueryForId(null));
        }

        /// <summary>
        /// Tester, at GetSqlCommandForInsert returnerer SQL kommando til oprettelse af systemet under OSWEBDB.
        /// </summary>
        [Test]
        public void TestAtGetSqlCommandForInsertReturnererSqlCommand()
        {
            var fixture = new Fixture();
            var title = fixture.Create<string>();
            var properties = fixture.Create<int>();
            fixture.Inject(new SystemProxy(1, title, properties));

            var systemProxy = fixture.Create<SystemProxy>();
            Assert.That(systemProxy, Is.Not.Null);

            var sqlCommand = systemProxy.GetSqlCommandForInsert();
            Assert.That(sqlCommand, Is.Not.Null);
            Assert.That(sqlCommand, Is.EqualTo(string.Format("INSERT INTO Systems (SystemNo,Title,Properties) VALUES(1,'{0}',{1})", title, properties)));
        }

        /// <summary>
        /// Tester, at GetSqlCommandForUpdate returnerer SQL kommando til opdatering af systemet under OSWEBDB.
        /// </summary>
        [Test]
        public void TestAtGetSqlCommandForUpdateReturnererSqlCommand()
        {
            var fixture = new Fixture();
            var title = fixture.Create<string>();
            var properties = fixture.Create<int>();
            fixture.Inject(new SystemProxy(1, title, properties));

            var systemProxy = fixture.Create<SystemProxy>();
            Assert.That(systemProxy, Is.Not.Null);

            var sqlCommand = systemProxy.GetSqlCommandForUpdate();
            Assert.That(sqlCommand, Is.Not.Null);
            Assert.That(sqlCommand, Is.EqualTo(string.Format("UPDATE Systems SET Title='{0}',Properties={1} WHERE SystemNo=1", title, properties)));
        }

        /// <summary>
        /// Tester, at GetSqlCommandForDelete returnerer SQL kommando til sletning af systemet under OSWEBDB.
        /// </summary>
        [Test]
        public void TestAtGetSqlCommandForDeleteReturnererSqlCommand()
        {
            var fixture = new Fixture();
            var title = fixture.Create<string>();
            fixture.Inject(new SystemProxy(1, title));

            var systemProxy = fixture.Create<SystemProxy>();
            Assert.That(systemProxy, Is.Not.Null);

            var sqlCommand = systemProxy.GetSqlCommandForDelete();
            Assert.That(sqlCommand, Is.Not.Null);
            Assert.That(sqlCommand, Is.EqualTo("DELETE FROM Systems WHERE SystemNo=1"));
        }

        /// <summary>
        /// Tester, at MapData kaster en ArgumentNullException, hvis data reader er null.
        /// </summary>
        [Test]
        public void TestAtMapDataKasterArgumentNullExceptionHvisDataReaderErNull()
        {
            var fixture = new Fixture();
            fixture.Inject(new SystemProxy());
            fixture.Inject<object>(null);
            fixture.Inject(MockRepository.GenerateMock<IDataProviderBase>());

            var systemProxy = fixture.Create<SystemProxy>();
            Assert.That(systemProxy, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(
                () =>
                systemProxy.MapData(fixture.Create<object>(), fixture.Create<IDataProviderBase>()));
        }

        /// <summary>
        /// Tester, at MapData kaster en ArgumentNullException, hvis data provider er null.
        /// </summary>
        [Test]
        public void TestAtMapDataKasterArgumentNullExceptionHvisDataProviderErNull()
        {
            var fixture = new Fixture();
            fixture.Inject(new SystemProxy());
            fixture.Inject(new object());
            fixture.Inject<IDataProviderBase>(null);

            var systemProxy = fixture.Create<SystemProxy>();
            Assert.That(systemProxy, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(
                () =>
                systemProxy.MapData(fixture.Create<object>(), fixture.Create<IDataProviderBase>()));
        }

        /// <summary>
        /// Tester, at MapData kaster en IntranetRepositoryException, hvis data reader ikke er af typen MySqlDataReader.
        /// </summary>
        [Test]
        public void TestAtMapDataKasterIntranetRepositoryExceptionHvisDataReaderIkkeErMySqlDataReader()
        {
            var fixture = new Fixture();
            fixture.Inject(new SystemProxy());
            fixture.Inject(MockRepository.GenerateMock<IDataReader>());
            fixture.Inject(MockRepository.GenerateMock<IDataProviderBase>());

            var systemProxy = fixture.Create<SystemProxy>();
            Assert.That(systemProxy, Is.Not.Null);

            Assert.Throws<IntranetRepositoryException>(
                () =>
                systemProxy.MapData(fixture.Create<IDataReader>(), fixture.Create<IDataProviderBase>()));
        }

        /// <summary>
        /// Tester, at MapData mapper data proxy for et system under OSWEBDB.
        /// </summary>
        [Test]
        public void TestAtMapDataMapperSystemProxy()
        {
            var fixture = new Fixture();
            fixture.Inject(new SystemProxy());
            fixture.Inject(MockRepository.GenerateMock<IDataProviderBase>());

            var dataReader = MockRepository.GenerateStub<MySqlDataReader>();
            dataReader.Expect(m => m.GetInt32(Arg<string>.Is.Equal("SystemNo")))
                .Return(fixture.Create<int>());
            dataReader.Expect(m => m.GetString(Arg<string>.Is.Equal("Title")))
                .Return(fixture.Create<string>());
            dataReader.Expect(m => m.GetInt32(Arg<string>.Is.Equal("Properties")))
                .Return(fixture.Create<int>());
            fixture.Inject(dataReader);

            var systemProxy = fixture.Create<SystemProxy>();
            Assert.That(systemProxy, Is.Not.Null);

            systemProxy.MapData(fixture.Create<MySqlDataReader>(), fixture.Create<IDataProviderBase>());
            Assert.That(systemProxy.DataIsLoaded, Is.True);

            dataReader.AssertWasCalled(m => m.GetInt32(Arg<string>.Is.Equal("SystemNo")));
            dataReader.AssertWasCalled(m => m.GetString(Arg<string>.Is.Equal("Title")));
            dataReader.AssertWasCalled(m => m.GetInt32(Arg<string>.Is.Equal("Properties")));
        }
    }
}
