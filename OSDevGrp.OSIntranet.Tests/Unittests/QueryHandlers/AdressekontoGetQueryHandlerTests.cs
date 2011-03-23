using System;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.QueryHandlers;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers
{
    /// <summary>
    /// Tester QueryHandler til håndtering af forespørgelsen: AdressekontoGetQuery.
    /// </summary>
    [TestFixture]
    public class AdressekontoGetQueryHandlerTests : FinansstyringQueryHandlerTestsBase
    {
        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis objectmapperen er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisObjectMapperErNull()
        {
            var adresseRepository = GetAdresseRepository();
            var finansstyringRepositoruy = GetFinansstyringRepository();
            Assert.Throws<ArgumentNullException>(() => new AdressekontoGetQueryHandler(adresseRepository, finansstyringRepositoruy, null));
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
            var queryHandler = new AdressekontoGetQueryHandler(adresseRepository, finansstyringRepositoruy, objectMapper);
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
            var queryHandler = new AdressekontoGetQueryHandler(adresseRepository, finansstyringRepositoruy, objectMapper);
            var query = new AdressekontoGetQuery
                            {
                                Regnskabsnummer = 1,
                                StatusDato = new DateTime(2011, 3, 15),
                                Nummer = 1
                            };
            var adressekonto = queryHandler.Query(query);
            Assert.That(adressekonto, Is.Not.Null);
            Assert.That(adressekonto.Nummer, Is.EqualTo(1));
        }
    }
}
