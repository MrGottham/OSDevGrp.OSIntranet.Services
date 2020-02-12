using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.QueryHandlers.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using NUnit.Framework;
using AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers.Core
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
        /// Tester, at AdresseGetByNummer henter en given adresse.
        /// </summary>
        [Test]
        public void TestAtAdresseGetByNummerHenterAdresse()
        {
            var fixture = new Fixture();
            var personer = fixture.CreateMany<Person>(3).ToList();
            var firmaer = fixture.CreateMany<Firma>(3).ToList();
            var adresser = new List<AdresseBase>();
            adresser.AddRange(personer);
            adresser.AddRange(firmaer);

            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepository.Expect(m => m.AdresseGetAll())
                .Return(adresser);
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            var queryHandler = new MyAdressekartotekQueryHandler(adresseRepository, objectMapper);
            Assert.That(queryHandler, Is.Not.Null);

            var adresse = queryHandler.AdresseGetByNummer(adresser.ElementAt(1).Nummer);
            Assert.That(adresse, Is.Not.Null);
            Assert.That(adresse.Nummer, Is.EqualTo(adresser.ElementAt(1).Nummer));
        }

        /// <summary>
        /// Tester, at AdresseGetByNummer kaster en IntranetRepositoryException, hvis adressen ikke findes.
        /// </summary>
        [Test]
        public void TestAtAdresseGetByNummerKasterIntranetRepositoryExceptionHvisAdresseIkkeFindes()
        {
            var fixture = new Fixture();
            var personer = fixture.CreateMany<Person>(3).ToList();
            var firmaer = fixture.CreateMany<Firma>(3).ToList();
            var adresser = new List<AdresseBase>();
            adresser.AddRange(personer);
            adresser.AddRange(firmaer);

            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepository.Expect(m => m.AdresseGetAll())
                .Return(adresser);
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            var queryHandler = new MyAdressekartotekQueryHandler(adresseRepository, objectMapper);
            Assert.That(queryHandler, Is.Not.Null);

            Assert.Throws<IntranetRepositoryException>(() => queryHandler.AdresseGetByNummer(-1));
        }

        /// <summary>
        /// Tester, at PersonGetAll henter alle personer.
        /// </summary>
        [Test]
        public void TestAtPersonGetAllHenterAllePersoner()
        {
            var fixture = new Fixture();
            var personer = fixture.CreateMany<Person>(3).ToList();
            var firmaer = fixture.CreateMany<Firma>(3).ToList();
            var adresser = new List<AdresseBase>();
            adresser.AddRange(personer);
            adresser.AddRange(firmaer);

            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepository.Expect(m => m.AdresseGetAll())
                .Return(adresser);
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            var queryHandler = new MyAdressekartotekQueryHandler(adresseRepository, objectMapper);
            Assert.That(queryHandler, Is.Not.Null);

            var result = queryHandler.PersonGetAll();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(3));
        }

        /// <summary>
        /// Tester, at PersonGetByNummer henter en given person.
        /// </summary>
        [Test]
        public void TestAtPersonGetByNummerHenterPerson()
        {
            var fixture = new Fixture();
            var personer = fixture.CreateMany<Person>(3).ToList();
            var firmaer = fixture.CreateMany<Firma>(3).ToList();
            var adresser = new List<AdresseBase>();
            adresser.AddRange(personer);
            adresser.AddRange(firmaer);

            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepository.Expect(m => m.AdresseGetAll())
                .Return(adresser);
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            var queryHandler = new MyAdressekartotekQueryHandler(adresseRepository, objectMapper);
            Assert.That(queryHandler, Is.Not.Null);

            var person = queryHandler.PersonGetByNummer(personer.ElementAt(1).Nummer);
            Assert.That(person, Is.Not.Null);
            Assert.That(person.Nummer, Is.EqualTo(personer.ElementAt(1).Nummer));
        }

        /// <summary>
        /// Tester, at PersonGetByNummer kaster en IntranetRepositoryException, hvis personen ikke findes.
        /// </summary>
        [Test]
        public void TestAtPersonGetByNummerKasterIntranetRepositoryExceptionHvisPersonIkkeFindes()
        {
            var fixture = new Fixture();
            var personer = fixture.CreateMany<Person>(3).ToList();
            var firmaer = fixture.CreateMany<Firma>(3).ToList();
            var adresser = new List<AdresseBase>();
            adresser.AddRange(personer);
            adresser.AddRange(firmaer);

            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepository.Expect(m => m.AdresseGetAll())
                .Return(adresser);
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            var queryHandler = new MyAdressekartotekQueryHandler(adresseRepository, objectMapper);
            Assert.That(queryHandler, Is.Not.Null);

            Assert.Throws<IntranetRepositoryException>(() => queryHandler.PersonGetByNummer(-1));
        }

        /// <summary>
        /// Tester, at FirmaGetAll henter alle firmaer.
        /// </summary>
        [Test]
        public void TestAtFirmaGetAllHenterAlleFirmaer()
        {
            var fixture = new Fixture();
            var personer = fixture.CreateMany<Person>(3).ToList();
            var firmaer = fixture.CreateMany<Firma>(3).ToList();
            var adresser = new List<AdresseBase>();
            adresser.AddRange(personer);
            adresser.AddRange(firmaer);

            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepository.Expect(m => m.AdresseGetAll())
                .Return(adresser);
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            var queryHandler = new MyAdressekartotekQueryHandler(adresseRepository, objectMapper);
            Assert.That(queryHandler, Is.Not.Null);

            var result = queryHandler.FirmaGetAll();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(3));
        }

        /// <summary>
        /// Tester, at FirmaGetByNummer henter et givent firma.
        /// </summary>
        [Test]
        public void TestAtFirmaGetByNummerHenterFirma()
        {
            var fixture = new Fixture();
            var personer = fixture.CreateMany<Person>(3).ToList();
            var firmaer = fixture.CreateMany<Firma>(3).ToList();
            var adresser = new List<AdresseBase>();
            adresser.AddRange(personer);
            adresser.AddRange(firmaer);

            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepository.Expect(m => m.AdresseGetAll())
                .Return(adresser);
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            var queryHandler = new MyAdressekartotekQueryHandler(adresseRepository, objectMapper);
            Assert.That(queryHandler, Is.Not.Null);

            var firma = queryHandler.FirmaGetByNummer(firmaer.ElementAt(1).Nummer);
            Assert.That(firma, Is.Not.Null);
            Assert.That(firma.Nummer, Is.EqualTo(firmaer.ElementAt(1).Nummer));
        }

        /// <summary>
        /// Tester, at FirmaGetByNummer kaster en IntranetRepositoryException, hvis firmaet ikke findes.
        /// </summary>
        [Test]
        public void TestAtFirmaGetByNummerKasterIntranetRepositoryExceptionHvisFirmaIkkeFindes()
        {
            var fixture = new Fixture();
            var personer = fixture.CreateMany<Person>(3).ToList();
            var firmaer = fixture.CreateMany<Firma>(3).ToList();
            var adresser = new List<AdresseBase>();
            adresser.AddRange(personer);
            adresser.AddRange(firmaer);

            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepository.Expect(m => m.AdresseGetAll())
                .Return(adresser);
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            var queryHandler = new MyAdressekartotekQueryHandler(adresseRepository, objectMapper);
            Assert.That(queryHandler, Is.Not.Null);

            Assert.Throws<IntranetRepositoryException>(() => queryHandler.FirmaGetByNummer(-1));
        }
    }
}
