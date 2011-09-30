using System;
using System.Linq;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories
{
    /// <summary>
    /// Tester implementering af en MySql klient.
    /// </summary>
    [TestFixture]
    public class MySqlClientTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en MySql klient.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererMySqlClient()
        {
            using (var mySqlClient = new MySqlClient())
            {
                mySqlClient.Dispose();
            }
        }

        /// <summary>
        /// Tester, at GetCollection henter data.
        /// </summary>
        [Test]
        public void TestAtGetCollectionHenterData()
        {
            var fixture = new Fixture();

            using (var mySqlClient = fixture.CreateAnonymous<MySqlClient>())
            {
                Assert.That(mySqlClient, Is.Not.Null);

                var result = mySqlClient.GetCollection("SELECT SystemNo FROM Systems ORDER BY SystemNo", Builder);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count(), Is.GreaterThan(0));

                mySqlClient.Dispose();
            }
        }

        /// <summary>
        /// Tester, at GetCollection kaster en ArgumentNullException, hvis query er null.
        /// </summary>
        [Test]
        public void TestAtGetCollectionKasterArgumentNullExceptionHvisQueryErNull()
        {
            var fixture = new Fixture();

            using(var mySqlClient = fixture.CreateAnonymous<MySqlClient>())
            {
                Assert.That(mySqlClient, Is.Not.Null);

                Assert.Throws<ArgumentNullException>(() => mySqlClient.GetCollection(null, Builder));

                mySqlClient.Dispose();
            }
        }

        /// <summary>
        /// Tester, at GetCollection kaster en ArgumentNullException, hvis query er tom.
        /// </summary>
        [Test]
        public void TestAtGetCollectionKasterArgumentNullExceptionHvisQueryErEmpty()
        {
            var fixture = new Fixture();

            using (var mySqlClient = fixture.CreateAnonymous<MySqlClient>())
            {
                Assert.That(mySqlClient, Is.Not.Null);

                Assert.Throws<ArgumentNullException>(() => mySqlClient.GetCollection(string.Empty, Builder));

                mySqlClient.Dispose();
            }
        }

        /// <summary>
        /// Tester, at GetCollection kaster en ArgumentNullException, hvis builder er null..
        /// </summary>
        [Test]
        public void TestAtGetCollectionKasterArgumentNullExceptionHvisBuilderErNull()
        {
            var fixture = new Fixture();

            using (var mySqlClient = fixture.CreateAnonymous<MySqlClient>())
            {
                Assert.That(mySqlClient, Is.Not.Null);

                Assert.Throws<ArgumentNullException>(
                    () => mySqlClient.GetCollection<int>(fixture.CreateAnonymous<string>(), null));

                mySqlClient.Dispose();
            }
        }

        /// <summary>
        /// Tester, at GetCollection kaster en IntranetRepositoryException, hvis query ikke kan udføres.
        /// </summary>
        [Test]
        public void TestAtGetCollectionKasterIntranetRepositoryExceptionHvisQueryIkkeKanUdføres()
        {
            var fixture = new Fixture();

            using (var mySqlClient = fixture.CreateAnonymous<MySqlClient>())
            {
                Assert.That(mySqlClient, Is.Not.Null);

                Assert.Throws<IntranetRepositoryException>(() => mySqlClient.GetCollection(fixture.CreateAnonymous<string>(), Builder));

                mySqlClient.Dispose();
            }
        }

        /// <summary>
        /// Objektbygger.
        /// </summary>
        /// <param name="mySqlDataRecord">Data fra MySql.</param>
        /// <returns>Objekt.</returns>
        private static int Builder(IMySqlDataRecord mySqlDataRecord)
        {
            return mySqlDataRecord.GetInt32(0);
        }
    }
}
