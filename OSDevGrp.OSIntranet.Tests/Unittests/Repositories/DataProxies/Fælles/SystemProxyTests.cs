using System;
using OSDevGrp.OSIntranet.Repositories.DataProxies.Fælles;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using AutoFixture;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.Fælles;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.DataProxies.Fælles
{
    /// <summary>
    /// Tester data proxy for et system under OSWEBDB.
    /// </summary>
    [TestFixture]
    public class SystemProxyTests
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
        /// Tester, at konstruktøren initierer en data proxy for et system under OSWEBDB.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererSystemProxy()
        {
            ISystemProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Nummer, Is.EqualTo(0));
            Assert.That(sut.Titel, Is.Not.Null);
            Assert.That(sut.Titel, Is.Not.Empty);
            Assert.That(sut.Titel, Is.EqualTo(typeof(OSIntranet.Domain.Fælles.System).Name));
            Assert.That(sut.Properties, Is.EqualTo(0));
            Assert.That(sut.Kalender, Is.False);
        }

        /// <summary>
        /// Tester, at UniqueId returnerer den unikke identifikation af systemet under OSWEBDB.
        /// </summary>
        [Test]
        public void TestAtUniqueIdReturnererIdentifikation()
        {
            int nummer = _fixture.Create<int>();

            ISystemProxy sut = CreateSut(nummer);
            Assert.That(sut, Is.Not.Null);

            Assert.That(sut.UniqueId, Is.Not.Null);
            Assert.That(sut.UniqueId, Is.Not.Null);
            Assert.That(sut.UniqueId, Is.EqualTo(Convert.ToString(nummer)));
        }

        /// <summary>
        /// Tester, at MapData kaster en ArgumentNullException, hvis data reader er null.
        /// </summary>
        [Test]
        public void TestAtMapDataKasterArgumentNullExceptionHvisDataReaderErNull()
        {
            ISystemProxy sut = CreateSut();
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
            ISystemProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.MapData(CreateMySqlDataReader(), null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tester, at MapData mapper data proxy for et system under OSWEBDB.
        /// </summary>
        [Test]
        public void TestAtMapDataMapperSystemProxy()
        {
            ISystemProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            int number = _fixture.Create<int>();
            string title = _fixture.Create<string>();
            int properties = _fixture.Create<int>();
            MySqlDataReader dataReader = CreateMySqlDataReader(number, title, properties);

            sut.MapData(dataReader, CreateMySqlDataProvider());

            Assert.That(sut.Nummer, Is.EqualTo(number));
            Assert.That(sut.Titel, Is.EqualTo(title));
            Assert.That(sut.Properties, Is.EqualTo(properties));

            dataReader.AssertWasCalled(m => m.GetInt32(Arg<string>.Is.Equal("SystemNo")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetString(Arg<string>.Is.Equal("Title")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetInt32(Arg<string>.Is.Equal("Properties")), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tester, at CreateGetCommand returnerer SQL kommando med foresprøgelse efter systemet under OSWEBDB.
        /// </summary>
        [Test]
        public void TestAtCreateGetCommandReturnererSqlCommand()
        {
            int nummer = _fixture.Create<int>();

            ISystemProxy sut = CreateSut(nummer);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("SELECT SystemNo,Title,Properties FROM Systems WHERE SystemNo=@systemNo")
                .AddSmallIntDataParameter("@systemNo", nummer, 2)
                .Build()
                .Run(sut.CreateGetCommand());
        }

        /// <summary>
        /// Tester, at CreateInsertCommand returnerer SQL kommando til oprettelse af systemet under OSWEBDB.
        /// </summary>
        [Test]
        public void TestAtCreateInsertCommandReturnererSqlCommand()
        {
            int nummer = _fixture.Create<int>();
            string title = _fixture.Create<string>();
            int properties = _fixture.Create<int>();

            ISystemProxy sut = CreateSut(nummer, title, properties);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("INSERT INTO Systems (SystemNo,Title,Properties) VALUES(@systemNo,@title,@properties)")
                .AddSmallIntDataParameter("@systemNo", nummer, 2)
                .AddVarCharDataParameter("@title", title, 50)
                .AddSmallIntDataParameter("@properties", properties, 5)
                .Build()
                .Run(sut.CreateInsertCommand());
        }

        /// <summary>
        /// Tester, at CreateUpdateCommand returnerer SQL kommando til opdatering af systemet under OSWEBDB.
        /// </summary>
        [Test]
        public void TestAtCreateUpdateCommandReturnererSqlCommand()
        {
            int nummer = _fixture.Create<int>();
            string title = _fixture.Create<string>();
            int properties = _fixture.Create<int>();

            ISystemProxy sut = CreateSut(nummer, title, properties);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("UPDATE Systems SET Title=@title,Properties=@properties WHERE SystemNo=@systemNo")
                .AddSmallIntDataParameter("@systemNo", nummer, 2)
                .AddVarCharDataParameter("@title", title, 50)
                .AddSmallIntDataParameter("@properties", properties, 5)
                .Build()
                .Run(sut.CreateUpdateCommand());
        }

        /// <summary>
        /// Tester, at CreateDeleteCommand returnerer SQL kommando til sletning af systemet under OSWEBDB.
        /// </summary>
        [Test]
        public void TestAtCreateDeleteCommandReturnererSqlCommand()
        {
            int nummer = _fixture.Create<int>();

            ISystemProxy sut = CreateSut(nummer);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("DELETE FROM Systems WHERE SystemNo=@systemNo")
                .AddSmallIntDataParameter("@systemNo", nummer, 2)
                .Build()
                .Run(sut.CreateDeleteCommand());
        }

        /// <summary>
        /// Danner en instans af en data proxy for et system under OSWEBDB til brug for unit testing. 
        /// </summary>
        /// <returns>Instans af en data proxy for et system under OSWEBDB til brug for unit testing.</returns>
        private ISystemProxy CreateSut()
        {
            return new SystemProxy();
        }

        /// <summary>
        /// Danner en instans af en data proxy for et system under OSWEBDB til brug for unit testing. 
        /// </summary>
        /// <param name="nummer">Unik identifikation af systemet under OSWEBDB.</param>
        /// <returns>Instans af en data proxy for et system under OSWEBDB til brug for unit testing.</returns>
        private ISystemProxy CreateSut(int nummer)
        {
            return new SystemProxy(nummer);
        }

        /// <summary>
        /// Danner en instans af en data proxy for et system under OSWEBDB til brug for unit testing. 
        /// </summary>
        /// <param name="nummer">Unik identifikation af systemet under OSWEBDB.</param>
        /// <param name="title">Titel for systemet under OSWEBDB.</param>
        /// <param name="properties">Egenskaber for systemet under OSWEBDB.</param>
        /// <returns>Instans af en data proxy for et system under OSWEBDB til brug for unit testing.</returns>
        private ISystemProxy CreateSut(int nummer, string title, int properties)
        {
            return new SystemProxy(nummer, title, properties);
        }

        /// <summary>
        /// Creates a stub for the MySQL data reader.
        /// </summary>
        /// <returns>Stub for the MySQL data reader.</returns>
        private MySqlDataReader CreateMySqlDataReader(int? number = null, string title = null, int? properties = null)
        {
            MySqlDataReader mySqlDataReaderMock = MockRepository.GenerateStub<MySqlDataReader>();
            mySqlDataReaderMock.Stub(m => m.GetInt32(Arg<string>.Is.Equal("SystemNo")))
                .Return(number ?? _fixture.Create<int>())
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetString(Arg<string>.Is.Equal("Title")))
                .Return(title ?? _fixture.Create<string>())
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetInt32(Arg<string>.Is.Equal("Properties")))
                .Return(properties ?? _fixture.Create<int>())
                .Repeat.Any();
            return mySqlDataReaderMock;
        }

        /// <summary>
        /// Creates a mockup for the data provider which uses MySQL.
        /// </summary>
        /// <returns>Mockup for the data provider which uses MySQL.</returns>
        private IMySqlDataProvider CreateMySqlDataProvider()
        {
            return MockRepository.GenerateMock<IMySqlDataProvider>();
        }
    }
}
