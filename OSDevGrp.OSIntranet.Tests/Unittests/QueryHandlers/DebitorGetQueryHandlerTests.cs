using System;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.QueryHandlers;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers
{
    /// <summary>
    /// Tester QueryHandler til håndtering af forespørgelsen: DebitorGetQueryHandler.
    /// </summary>
    [TestFixture]
    public class DebitorGetQueryHandlerTests : FinansstyringQueryHandlerTestsBase
    {
        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis objectmapperen er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisObjectMapperErNull()
        {
            var adresseRepository = GetAdresseRepository();
            var finansstyringRepositoruy = GetFinansstyringRepository();
            Assert.Throws<ArgumentNullException>(() => new DebitorGetQueryHandler(adresseRepository, finansstyringRepositoruy, null));
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
            var queryHandler = new DebitorGetQueryHandler(adresseRepository, finansstyringRepositoruy, objectMapper);
            var query = new DebitorGetQuery
                            {
                                Regnskabsnummer = 1,
                                StatusDato = new DateTime(2011, 3, 15),
                                Nummer = 2
                            };
            var debitor = queryHandler.Query(query);
            Assert.That(debitor, Is.Not.Null);
            Assert.That(debitor.Nummer, Is.EqualTo(2));
            Assert.That(debitor.Navn, Is.Not.Null);
            Assert.That(debitor.Navn, Is.EqualTo("Bente Susanne Rasmussen"));
            Assert.That(debitor.Saldo, Is.EqualTo(1000M));
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
            var queryHandler = new DebitorGetQueryHandler(adresseRepository, finansstyringRepositoruy, objectMapper);
            Assert.Throws<ArgumentNullException>(() => queryHandler.Query(null));
        }
    }
}
