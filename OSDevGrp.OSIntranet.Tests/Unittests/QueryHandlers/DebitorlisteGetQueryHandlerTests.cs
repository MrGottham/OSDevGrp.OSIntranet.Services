using System;
using System.Linq;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.QueryHandlers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using NUnit.Framework;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers
{
    /// <summary>
    /// Tester QueryHandler til håndtering af forespørgelsen: DebitorlisteGetQuery.
    /// </summary>
    [TestFixture]
    public class DebitorlisteGetQueryHandlerTests : FinansstyringQueryHandlerTestsBase
    {
        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis konfigurationsrepositoryet er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisKonfigurationRepositoryErNull()
        {
            var adresseRepository = GetAdresseRepository();
            var finansstyringRepositoruy = GetFinansstyringRepository();
            Assert.Throws<ArgumentNullException>(() => new DebitorlisteGetQueryHandler(adresseRepository, finansstyringRepositoruy, null, null));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis objectmapperen er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisObjectMapperErNull()
        {
            var adresseRepository = GetAdresseRepository();
            var finansstyringRepositoruy = GetFinansstyringRepository();
            var konfigurationRepository = MockRepository.GenerateMock<IKonfigurationRepository>();
            Assert.Throws<ArgumentNullException>(() => new DebitorlisteGetQueryHandler(adresseRepository, finansstyringRepositoruy, konfigurationRepository, null));
        }

        /// <summary>
        /// Tester, at Query kaster en ArgumentNullException, hvis Query er null.
        /// </summary>
        [Test]
        public void TestAtQueryKasterArgumentNullExceptionHvisQueryErNull()
        {
            var adresseRepository = GetAdresseRepository();
            var finansstyringRepositoruy = GetFinansstyringRepository();
            var konfigurationRepository = MockRepository.GenerateMock<IKonfigurationRepository>();
            var objectMapper = GetObjectMapper();
            var queryHandler = new DebitorlisteGetQueryHandler(adresseRepository, finansstyringRepositoruy, konfigurationRepository, objectMapper);
            Assert.Throws<ArgumentNullException>(() => queryHandler.Query(null));
        }

        /// <summary>
        /// Tester, at Query henter debitorer.
        /// </summary>
        [Test]
        public void TestAtQueryHenterDebitorer()
        {
            var adresseRepository = GetAdresseRepository();
            var finansstyringRepositoruy = GetFinansstyringRepository();
            var konfigurationRepository = MockRepository.GenerateMock<IKonfigurationRepository>();
            konfigurationRepository.Expect(m => m.DebitorSaldoOverNul).Return(true);
            var objectMapper = GetObjectMapper();
            var queryHandler = new DebitorlisteGetQueryHandler(adresseRepository, finansstyringRepositoruy, konfigurationRepository, objectMapper);
            var query = new DebitorlisteGetQuery
                            {
                                Regnskabsnummer = 1,
                                StatusDato = new DateTime(2011, 3, 15)
                            };
            var debitorer = queryHandler.Query(query);
            Assert.That(debitorer, Is.Not.Null);
            Assert.That(debitorer.Count(), Is.EqualTo(1));
        }
    }
}
