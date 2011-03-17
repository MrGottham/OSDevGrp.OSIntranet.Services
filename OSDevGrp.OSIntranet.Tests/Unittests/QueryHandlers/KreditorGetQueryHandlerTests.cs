using System;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.QueryHandlers;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers
{
    /// <summary>
    /// Tester QueryHandler til håndtering af forespørgelsen: KreditorGetQueryHandler.
    /// </summary>
    [TestFixture]
    public class KreditorGetQueryHandlerTests : FinansstyringQueryHandlerTestsBase
    {
        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis objectmapperen er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisObjectMapperErNull()
        {
            var adresseRepository = GetAdresseRepository();
            var finansstyringRepositoruy = GetFinansstyringRepository();
            Assert.Throws<ArgumentNullException>(() => new KreditorGetQueryHandler(adresseRepository, finansstyringRepositoruy, null));
        }

        /// <summary>
        /// Tester, at Query henter en debitor.
        /// </summary>
        [Test]
        public void TestAtQueryHenterDebitor()
        {
            var adresseRepository = GetAdresseRepository();
            var finansstyringRepositoruy = GetFinansstyringRepository();
            var objectMapper = GetObjectMapper();
            var queryHandler = new KreditorGetQueryHandler(adresseRepository, finansstyringRepositoruy, objectMapper);
            var query = new KreditorGetQuery()
                            {
                                Regnskabsnummer = 1,
                                StatusDato = new DateTime(2011, 3, 15),
                                Nummer = 1
                            };
            var kreditor = queryHandler.Query(query);
            Assert.That(kreditor, Is.Not.Null);
            Assert.That(kreditor.Nummer, Is.EqualTo(1));
            Assert.That(kreditor.Navn, Is.Not.Null);
            Assert.That(kreditor.Navn, Is.EqualTo("Ole Sørensen"));
            Assert.That(kreditor.Saldo, Is.EqualTo(-1000M));
        }

        /// <summary>
        /// Tester, at Query kaster en ArgumentNullExcpetion, hvis query er null.
        /// </summary>
        [Test]
        public void TestAtQueryKasterArgumentNullExceptionHvisQueryErNull()
        {
            var adresseRepository = GetAdresseRepository();
            var finansstyringRepositoruy = GetFinansstyringRepository();
            var objectMapper = GetObjectMapper();
            var queryHandler = new KreditorGetQueryHandler(adresseRepository, finansstyringRepositoruy, objectMapper);
            Assert.Throws<ArgumentNullException>(() => queryHandler.Query(null));
        }
    }
}
