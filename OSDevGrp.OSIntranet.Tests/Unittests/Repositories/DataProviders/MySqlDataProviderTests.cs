using System;
using System.Linq;
using OSDevGrp.OSIntranet.Repositories.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.DataProviders
{
    /// <summary>
    /// Tester data provider, som benytter MySql.
    /// </summary>
    [TestFixture]
    public class MySqlDataProviderTests
    {
        /// <summary>
        /// Egen data proxy til test af data provider, som benytter MySql.
        /// </summary>
        private class MyDataProxy : IMySqlDataProxy
        {
            /// <summary>
            /// Systemnummer.
            /// </summary>
            public int SystemNo
            {
                get;
                protected set;
            }

            /// <summary>
            /// Titel.
            /// </summary>
            public string Title
            {
                get;
                protected set;
            }
        }

        /// <summary>
        /// Tester, at konstruktøren initierer en data provider, som benytter MySql.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererMySqlDataProvider()
        {
            var fixture = new Fixture();

            var mySqlDataProvider = fixture.CreateAnonymous<MySqlDataProvider>();
            Assert.That(mySqlDataProvider, Is.Not.Null);
        }

        /// <summary>
        /// Tester, at GetCollection kaster en ArgumentNullException, hvis query er null.
        /// </summary>
        [Test]
        public void TestAtGetCollectionKasterArgumenutNullExceptionHvisQueryErNull()
        {
            var fixture = new Fixture();

            var mySqlDataProvider = fixture.CreateAnonymous<MySqlDataProvider>();
            Assert.That(mySqlDataProvider, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => mySqlDataProvider.GetCollection<MyDataProxy>(null));
        }

        /// <summary>
        /// Tester, at GetCollection, henter data proxies fra MySql.
        /// </summary>
        [Test]
        public void TestAtGetCollectionHenterDataProxies()
        {
            var fixture = new Fixture();

            var mySqlDataProvider = fixture.CreateAnonymous<MySqlDataProvider>();
            Assert.That(mySqlDataProvider, Is.Not.Null);

            var result = mySqlDataProvider.GetCollection<MyDataProxy>("SELECT SystemNo,Title FROM Systems ORDER BY SystemNo");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at GetCollection kaster en ArgumentNullException, hvis query er tom.
        /// </summary>
        [Test]
        public void TestAtGetCollectionKasterArgumenutNullExceptionHvisQueryErEmpty()
        {
            var fixture = new Fixture();

            var mySqlDataProvider = fixture.CreateAnonymous<MySqlDataProvider>();
            Assert.That(mySqlDataProvider, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => mySqlDataProvider.GetCollection<MyDataProxy>(string.Empty));
        }

        /// <summary>
        /// Tester, at GetCollection kaster en MySqlException, hvis query ikke kan udføres.
        /// </summary>
        [Test]
        public void TestAtGetCollectionKasterMySqlExceptionHvisQueryIkkeKanUdføres()
        {
            var fixture = new Fixture();

            var mySqlDataProvider = fixture.CreateAnonymous<MySqlDataProvider>();
            Assert.That(mySqlDataProvider, Is.Not.Null);

            Assert.Throws<MySqlException>(() => mySqlDataProvider.GetCollection<MyDataProxy>(fixture.CreateAnonymous<string>()));
        }

        /// <summary>
        /// Tester, at Get kaster en ArgumentNullException, hvis query er null.
        /// </summary>
        [Test]
        public void TestAtGetKasterArgumenutNullExceptionHvisIdErNull()
        {
            var fixture = new Fixture();

            var mySqlDataProvider = fixture.CreateAnonymous<MySqlDataProvider>();
            Assert.That(mySqlDataProvider, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => mySqlDataProvider.Get<MyDataProxy, object>(null));
        }

        /// <summary>
        /// Tester, at Add kaster en ArgumentNullException, hvis data proxy er null.
        /// </summary>
        [Test]
        public void TestAtAddKasterArgumenutNullExceptionHvisDataProxyErNull()
        {
            var fixture = new Fixture();

            var mySqlDataProvider = fixture.CreateAnonymous<MySqlDataProvider>();
            Assert.That(mySqlDataProvider, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => mySqlDataProvider.Add<MyDataProxy>(null));
        }

        /// <summary>
        /// Tester, at Save kaster en ArgumentNullException, hvis data proxy er null.
        /// </summary>
        [Test]
        public void TestAtSaveKasterArgumenutNullExceptionHvisDataProxyErNull()
        {
            var fixture = new Fixture();

            var mySqlDataProvider = fixture.CreateAnonymous<MySqlDataProvider>();
            Assert.That(mySqlDataProvider, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => mySqlDataProvider.Save<MyDataProxy>(null));
        }

        /// <summary>
        /// Tester, at Delete kaster en ArgumentNullException, hvis data proxy er null.
        /// </summary>
        [Test]
        public void TestAtDeleteKasterArgumenutNullExceptionHvisDataProxyErNull()
        {
            var fixture = new Fixture();

            var mySqlDataProvider = fixture.CreateAnonymous<MySqlDataProvider>();
            Assert.That(mySqlDataProvider, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => mySqlDataProvider.Delete<MyDataProxy>(null));
        }
    }
}
