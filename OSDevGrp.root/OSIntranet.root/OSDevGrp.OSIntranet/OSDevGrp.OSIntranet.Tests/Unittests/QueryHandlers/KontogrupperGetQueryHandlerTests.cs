using System;
using System.Linq;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.QueryHandlers;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers
{
    /// <summary>
    /// Tester QueryHandler til håndtering af forespørgelsen: KontogrupperGetQuery.
    /// </summary>
    [TestFixture]
    public class KontogrupperGetQueryHandlerTests : FinansstyringQueryHandlerTestsBase
    {
        /// <summary>
        /// Test, at konstruktøren kaster en ArgumentNullException, hvis repository for finansstyring er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisFinansstyringRepositoryErNull()
        {
            Assert.Throws<ArgumentNullException>(() => new KontogrupperGetQueryHandler(null, null));
        }

        /// <summary>
        /// Test, at konstruktøren kaster en ArgumentNullException, hvis objectmapperen er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisObjectMapperErNull()
        {
            Assert.Throws<ArgumentNullException>(() => new KontogrupperGetQueryHandler(GetFinansstyringRepository(), null));
        }

        /// <summary>
        /// Tester, at Query henter kontogrupper.
        /// </summary>
        [Test]
        public void TestAtQueryHenterKontogrupper()
        {
            var queryHandler = new KontogrupperGetQueryHandler(GetFinansstyringRepository(), GetObjectMapper());
            var query = new KontogrupperGetQuery();
            var kontogrupper = queryHandler.Query(query);
            Assert.That(kontogrupper, Is.Not.Null);
            Assert.That(kontogrupper.Count(), Is.EqualTo(1));
        }

        /// <summary>
        /// Tester, at Query kaster ArgumentNullException, hvis Query er null.
        /// </summary>
        [Test]
        public void TestAtQueryKasterArgumentNullExceptionHvisQueryErNull()
        {
            var queryHandler = new KontogrupperGetQueryHandler(GetFinansstyringRepository(), GetObjectMapper());
            Assert.Throws<ArgumentNullException>(() => queryHandler.Query(null));
        }
    }
}
