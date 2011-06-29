using System;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.QueryHandlers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers
{
    /// <summary>
    /// Tester basisklasse for en QueryHandler til adressekartoteket.
    /// </summary>
    [TestFixture]
    public class AdressekartotekQueryHandlerBaseTests
    {
        /// <summary>
        /// Egen klasse til test af basisklasse for en QueryHandler til adressekartoteket.
        /// </summary>
        private class MyAdressekartotekQueryHandler : AdressekartotekQueryHandlerBase
        {
            /// <summary>
            /// Danner egen klasse til test af basisklasse for en QueryHandler til adressekartoteket.
            /// </summary>
            /// <param name="adresseRepository">Implementering af repository til adresser.</param>
            /// <param name="objectMapper">Implementering af objectmapper.</param>
            public MyAdressekartotekQueryHandler(IAdresseRepository adresseRepository, IObjectMapper objectMapper)
                : base(adresseRepository, objectMapper)
            {
            }
        }

        /// <summary>
        /// Tester, at konstruktøren initierer AdressekartotekQueryHandlerBase.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererAdressekartotekQueryHandlerBase()
        {
            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            var queryHandler = new MyAdressekartotekQueryHandler(adresseRepository, objectMapper);
            Assert.That(queryHandler, Is.Not.Null);
            Assert.That(queryHandler.Repository, Is.Not.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis repository til adresser er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisAdresseRepositoryErNull()
        {
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            Assert.Throws<ArgumentNullException>(() => new MyAdressekartotekQueryHandler(null, objectMapper));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis objectmapperen er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisObjectMapperErNull()
        {
            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            Assert.Throws<ArgumentNullException>(() => new MyAdressekartotekQueryHandler(adresseRepository, null));
        }

        /// <summary>
        /// Tester, at BetalingsbetingelseGetByNummer henter en given betalingsbetingelse.
        /// </summary>
        [Test]
        public void TestAtBetalingsbetingelseGetByNummerHenterBetalingsbetingelse()
        {
            var fixture = new Fixture();
            var betalingsbetingelser = fixture.CreateMany<Betalingsbetingelse>(3).ToList();

            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepository.Expect(m => m.BetalingsbetingelseGetAll())
                .Return(betalingsbetingelser);
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            var queryHandler = new MyAdressekartotekQueryHandler(adresseRepository, objectMapper);
            Assert.That(queryHandler, Is.Not.Null);

            var betalingsbetingelse = queryHandler.BetalingsbetingelseGetByNummer(betalingsbetingelser.ElementAt(1).Nummer);
            Assert.That(betalingsbetingelse, Is.Not.Null);
            Assert.That(betalingsbetingelse.Nummer, Is.EqualTo(betalingsbetingelser.ElementAt(1).Nummer));
        }

        /// <summary>
        /// Tester, at BetalingsbetingelseGetByNummer kaster en IntranetRepositoryException, hvis betalingsbetingelsen ikke findes.
        /// </summary>
        [Test]
        public void TestAtBetalingsbetingelseGetByNummerKasterIntranetRepositoryExceptionHvisBetalingsbetingelseIkkeFindes()
        {
            var fixture = new Fixture();
            var betalingsbetingelser = fixture.CreateMany<Betalingsbetingelse>(3).ToList();

            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepository.Expect(m => m.BetalingsbetingelseGetAll())
                .Return(betalingsbetingelser);
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            var queryHandler = new MyAdressekartotekQueryHandler(adresseRepository, objectMapper);
            Assert.That(queryHandler, Is.Not.Null);

            Assert.Throws<IntranetRepositoryException>(() => queryHandler.BetalingsbetingelseGetByNummer(-1));
        }
    }
}
