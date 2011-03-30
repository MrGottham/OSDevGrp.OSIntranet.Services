using System;
using System.Linq;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.QueryHandlers;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers
{
    /// <summary>
    /// Tester QueryHandler til håndtering af forespørgelsen: BogføringerGetQuery.
    /// </summary>
    [TestFixture]
    public class BetalingsbetingelserGetQueryHandlerTests : FinansstyringQueryHandlerTestsBase
    {
        /// <summary>
        /// Test, at konstruktøren kaster en ArgumentNullException, hvis adresserepository er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisAdresseRepositoryErNull()
        {
            Assert.Throws<ArgumentNullException>(() => new BetalingsbetingelserGetQueryHandler(null, null));
        }

        /// <summary>
        /// Test, at konstruktøren kaster en ArgumentNullException, hvis objectmapperen er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisObjectMapperErNull()
        {
            Assert.Throws<ArgumentNullException>(() => new BetalingsbetingelserGetQueryHandler(GetAdresseRepository(), null));
        }

        /// <summary>
        /// Tester, at Query henter betalingsbetingelser.
        /// </summary>
        [Test]
        public void TestAtQueryHenterBetalingsbetingelser()
        {
            var queryHandler = new BetalingsbetingelserGetQueryHandler(GetAdresseRepository(), GetObjectMapper());
            var query = new BetalingsbetingelserGetQuery();
            var betalingsbetingelser = queryHandler.Query(query);
            Assert.That(betalingsbetingelser, Is.Not.Null);
            Assert.That(betalingsbetingelser.Count(), Is.EqualTo(2));
        }

        /// <summary>
        /// Tester, at Query kaster ArgumentNullException, hvis Query er null.
        /// </summary>
        [Test]
        public void TestAtQueryKasterArgumentNullExceptionHvisQueryErNull()
        {
            var queryHandler = new BetalingsbetingelserGetQueryHandler(GetAdresseRepository(), GetObjectMapper());
            Assert.Throws<ArgumentNullException>(() => queryHandler.Query(null));
        }
    }
}
