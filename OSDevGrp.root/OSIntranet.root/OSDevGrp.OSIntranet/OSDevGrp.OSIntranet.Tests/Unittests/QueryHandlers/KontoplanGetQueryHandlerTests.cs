using System;
using System.Linq;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.QueryHandlers;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers
{
    /// <summary>
    /// Tester QueryHandler til håndtering af forespørgelsen: KontoplanGetQuery.
    /// </summary>
    [TestFixture]
    public class KontoplanGetQueryHandlerTests : FinansstyringQueryHandlerTestsBase
    {
        /// <summary>
        /// Test, at konstruktøren kaster en ArgumentNullException, hvis repository for finansstyring er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisFinansstyringRepositoryErNull()
        {
            Assert.Throws<ArgumentNullException>(() => new KontoplanGetQueryHandler(null, null));
        }

        /// <summary>
        /// Test, at konstruktøren kaster en ArgumentNullException, hvis objectmapper er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisObjectMapperErNull()
        {
            Assert.Throws<ArgumentNullException>(() => new KontoplanGetQueryHandler(GetFinansstyringRepository(), null));
        }

        /// <summary>
        /// Test, at Query henter kontoplan.
        /// </summary>
        [Test]
        public void TestAtQueryHenterKontoplan()
        {
            var queryHandler = new KontoplanGetQueryHandler(GetFinansstyringRepository(), GetObjectMapper());
            var query = new KontoplanGetQuery
                            {
                                Regnskabsnummer = 1,
                                StatusDato = new DateTime(2010, 10, 31)
                            };
            Assert.That(queryHandler, Is.Not.Null);
            Assert.That(query, Is.Not.Null);
            var kontoplan = queryHandler.Query(query);
            Assert.That(kontoplan, Is.Not.Null);
            Assert.That(kontoplan.Count(), Is.EqualTo(2));
        }

        /// <summary>
        /// Test, at Query kaster en IntranetRepositoryException, hvis regnskabet ikke findes.
        /// </summary>
        [Test]
        public void TestAtQueryKasterIntranetRepositoryExceptionHvisRegnskabIkkeFindes()
        {
            var queryHandler = new KontoplanGetQueryHandler(GetFinansstyringRepository(), GetObjectMapper());
            var query = new KontoplanGetQuery
                            {
                                Regnskabsnummer = -1,
                                StatusDato = new DateTime(2010, 10, 31)
                            };
            Assert.That(queryHandler, Is.Not.Null);
            Assert.That(query, Is.Not.Null);
            Assert.Throws<IntranetRepositoryException>(() => queryHandler.Query(query));
        }

        /// <summary>
        /// Tester, at Query kaster ArgumentNullException, hvis Query er null.
        /// </summary>
        [Test]
        public void TestAtQueryKasterArgumentNullExceptionHvisQueryErNull()
        {
            var queryHandler = new KontoplanGetQueryHandler(GetFinansstyringRepository(), GetObjectMapper());
            Assert.That(queryHandler, Is.Not.Null);
            Assert.Throws<ArgumentNullException>(() => queryHandler.Query(null));
        }
    }
}
