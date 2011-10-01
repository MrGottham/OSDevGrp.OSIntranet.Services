using System;
using System.Linq;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
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
        private class MyDataProxy : IMySqlDataProxy<int>
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

            #region IDataProxyBase Members

            /// <summary>
            /// Mapper data fra en data reader.
            /// </summary>
            /// <param name="dataReader">Data reader for data provideren.</param>
            /// <param name="dataProvider">Data provider, hvorfra data mappes.</param>
            public void MapData(object dataReader, IDataProviderBase dataProvider)
            {
                var mySqlReader = (MySqlDataReader) dataReader;

                SystemNo = mySqlReader.GetInt32("SystemNo");
                Title = mySqlReader.GetString("Title");
            }

            #endregion

            #region IMySqlDataProxy Members

            /// <summary>
            /// Returnerer SQL foresprøgelse til søgning efter en given data proxy på MySql.
            /// </summary>
            /// <param name="id">Unik identifikation af data proxy, som skal fremsøges.</param>
            /// <returns>SQL foresprøgelse.</returns>
            public string GetSqlQueryForId(int id)
            {
                return string.Format("SELECT SystemNo,Title FROM Systems WHERE SystemNo={0}", id);
            }

            #endregion

            #region IMySqlDataProxy<int> Members


            #endregion
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
        /// Tester, at Get henter data proxy fra MySql.
        /// </summary>
        [Test]
        public void TestAtGetHenterDataProxy()
        {
            var fixture = new Fixture();

            var mySqlDataProvider = fixture.CreateAnonymous<MySqlDataProvider>();
            Assert.That(mySqlDataProvider, Is.Not.Null);

            var result = mySqlDataProvider.Get<MyDataProxy, int>(1);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.SystemNo, Is.EqualTo(1));
            Assert.That(result.Title, Is.Not.Null);
            Assert.That(result.Title.Length, Is.GreaterThan(0));
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
        /// Tester, at Get henter data proxy fra MySql.
        /// </summary>
        [Test]
        public void TestAtGetKasterIntranetRepositoryExceptionHvisIdIkkeFindes()
        {
            var fixture = new Fixture();

            var mySqlDataProvider = fixture.CreateAnonymous<MySqlDataProvider>();
            Assert.That(mySqlDataProvider, Is.Not.Null);

            Assert.Throws<IntranetRepositoryException>(() => mySqlDataProvider.Get<MyDataProxy, int>(-1));
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
