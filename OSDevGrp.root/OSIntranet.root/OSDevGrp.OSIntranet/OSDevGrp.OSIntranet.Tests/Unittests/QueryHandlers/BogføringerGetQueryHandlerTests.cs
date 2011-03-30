using System;
using System.Linq;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.QueryHandlers;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers
{
    /// <summary>
    /// Tester QueryHandler til håndtering af forespørgelsen: BogføringerGetQuery.
    /// </summary>
    [TestFixture]
    public class BogføringerGetQueryHandlerTests : FinansstyringQueryHandlerTestsBase
    {
        /// <summary>
        /// Test, at konstruktøren kaster en ArgumentNullException, hvis repository for finansstyring er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisFinansstyringRepositoryErNull()
        {
            Assert.Throws<ArgumentNullException>(() => new BogføringerGetQueryHandler(null, null, null));
        }

        /// <summary>
        /// Test, at konstruktøren kaster en ArgumentNullException, hvis adresserepository er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisAdresseRepositoryErNull()
        {
            Assert.Throws<ArgumentNullException>(() => new BogføringerGetQueryHandler(GetFinansstyringRepository(), null, null));
        }

        /// <summary>
        /// Test, at konstruktøren kaster en ArgumentNullException, hvis objectmapperen er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisObjectMapperErNull()
        {
            Assert.Throws<ArgumentNullException>(() => new BogføringerGetQueryHandler(GetFinansstyringRepository(), GetAdresseRepository(), null));
        }

        /// <summary>
        /// Test, at Query henter 3 bogføringslinjer.
        /// </summary>
        [Test]
        public void TestAtQueryHenterBogføringslinjer()
        {
            var queryHandler = new BogføringerGetQueryHandler(GetFinansstyringRepository(), GetAdresseRepository(),
                                                              GetObjectMapper());
            var query = new BogføringerGetQuery
                            {
                                Regnskabsnummer = 1,
                                StatusDato = new DateTime(2011, 3, 4),
                                Linjer = 3
                            };
            var bogføringslinjer = queryHandler.Query(query);
            Assert.That(bogføringslinjer, Is.Not.Null);
            Assert.That(bogføringslinjer.Count(), Is.EqualTo(3));
        }

        /// <summary>
        /// Tester, at Query kaster ArgumentNullException, hvis Query er null.
        /// </summary>
        [Test]
        public void TestAtQueryKasterArgumentNullExceptionHvisQueryErNull()
        {
            var queryHandler = new BogføringerGetQueryHandler(GetFinansstyringRepository(), GetAdresseRepository(),
                                                              GetObjectMapper());
            Assert.Throws<ArgumentNullException>(() => queryHandler.Query(null));
        }

        /// <summary>
        /// Test, at Query kaster en IntranetRepositoryException, hvis regnskabet ikke findes.
        /// </summary>
        [Test]
        public void TestAtQueryKasterIntranetRepositoryExceptionHvisRegnskabIkkeFindes()
        {
            var queryHandler = new BogføringerGetQueryHandler(GetFinansstyringRepository(), GetAdresseRepository(),
                                                              GetObjectMapper());
            var query = new BogføringerGetQuery
                            {
                                Regnskabsnummer = -1,
                                StatusDato = new DateTime(2011, 3, 15),
                                Linjer = 3
                            };
            Assert.Throws<IntranetRepositoryException>(() => queryHandler.Query(query));
        }
    }
}
