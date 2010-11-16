using System;
using System.Linq;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.QueryHandlers;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers
{
    /// <summary>
    /// Tester QueryHandler til håndtering af forespørgelsen: RegnskabslisteGetQuery.
    /// </summary>
    [TestFixture]
    public class RegnskabslisteGetQueryHandlerTests : FinansstyringQueryHandlerTestsBase
    {
        /// <summary>
        /// Test, at konstruktøren kaster en ArgumentNullException, hvis repository for finansstyring er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisFinansstyringRepositoryErNull()
        {
            Assert.Throws<ArgumentNullException>(() => new RegnskabslisteGetQueryHandler(null));
        }

        /// <summary>
        /// Tester, at Query henter regnskaber.
        /// </summary>
        [Test]
        public void TestAtQueryHenterRegnskaber()
        {
            var queryHandler = new RegnskabslisteGetQueryHandler(GetFinansstyringRepository());
            var query = new RegnskabslisteGetQuery();
            Assert.That(queryHandler, Is.Not.Null);
            Assert.That(query, Is.Not.Null);
            var regnskaber = queryHandler.Query(query);
            Assert.That(regnskaber, Is.Not.Null);
            Assert.That(regnskaber.Count(), Is.EqualTo(3));
        }

        /// <summary>
        /// Tester, at Query kaster ArgumentNullException, hvis Query er null.
        /// </summary>
        [Test]
        public void TestAtQueryKasterArgumentNullExceptionHvisQueryErNull()
        {
            var queryHandler = new RegnskabslisteGetQueryHandler(GetFinansstyringRepository());
            Assert.That(queryHandler, Is.Not.Null);
            Assert.Throws<ArgumentNullException>(() => queryHandler.Query(null));
        }
    }
}
