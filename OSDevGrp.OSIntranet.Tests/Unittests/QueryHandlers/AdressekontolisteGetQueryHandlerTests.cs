using System;
using System.Linq;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.QueryHandlers;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers
{
    /// <summary>
    /// Tester QueryHandler til håndtering af forespørgelsen: AdressekontolisteGetQuery.
    /// </summary>
    [TestFixture]
    public class AdressekontolisteGetQueryHandlerTests : FinansstyringQueryHandlerTestsBase
    {
        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis objectmapperen er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisObjectMapperErNull()
        {
            var adresseRepository = GetAdresseRepository();
            var finansstyringRepositoruy = GetFinansstyringRepository();
            Assert.Throws<ArgumentNullException>(() => new AdressekontolisteGetQueryHandler(adresseRepository, finansstyringRepositoruy, null));
        }

        /// <summary>
        /// Tester, at Query kaster en ArgumentNullException, hvis Query er null.
        /// </summary>
        [Test]
        public void TestAtQueryKasterArgumentNullExceptionHvisQueryErNull()
        {
            var adresseRepository = GetAdresseRepository();
            var finansstyringRepositoruy = GetFinansstyringRepository();
            var objectMapper = GetObjectMapper();
            var queryHandler = new AdressekontolisteGetQueryHandler(adresseRepository, finansstyringRepositoruy, objectMapper);
            Assert.Throws<ArgumentNullException>(() => queryHandler.Query(null));
        }

        /// <summary>
        /// Tester, at Query henter adressekonti.
        /// </summary>
        [Test]
        public void TestAtQueryHenterAdressekonti()
        {
            var adresseRepository = GetAdresseRepository();
            var finansstyringRepositoruy = GetFinansstyringRepository();
            var objectMapper = GetObjectMapper();
            var queryHandler = new AdressekontolisteGetQueryHandler(adresseRepository, finansstyringRepositoruy, objectMapper);
            var query = new AdressekontolisteGetQuery
                            {
                                Regnskabsnummer = 1,
                                StatusDato = new DateTime(2011, 3, 15)
                            };
            var adressekonti = queryHandler.Query(query);
            Assert.That(adressekonti, Is.Not.Null);
            Assert.That(adressekonti.Count(), Is.EqualTo(3));
        }
    }
}
