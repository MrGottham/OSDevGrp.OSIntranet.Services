using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommandHandlers.Core;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Resources;
using AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.CommandHandlers.Core
{
    /// <summary>
    /// Tester basisklasse for en CommandHandler til adressekartoteket.
    /// </summary>
    [TestFixture]
    public class AdressekartotekCommandHandlerBaseTests
    {
        /// <summary>
        /// Egen klasse til test af basisklasse for en CommandHandler til adressekartoteket.
        /// </summary>
        private class MyCommandHandler : AdressekartotekCommandHandlerBase
        {
            #region Constructor

            /// <summary>
            /// Danner egen klasse til test af basisklasse for en CommandHandler til adressekartoteket.
            /// </summary>
            /// <param name="adresseRepository">Implementering af repository til adresser.</param>
            /// <param name="objectMapper">Implementering af objectmapper.</param>
            /// <param name="exceptionBuilder">Implementering af en builder, der kan bygge exceptions.</param>
            public MyCommandHandler(IAdresseRepository adresseRepository, IObjectMapper objectMapper, IExceptionBuilder exceptionBuilder)
                : base(adresseRepository, objectMapper, exceptionBuilder)
            {
            }

            #endregion
        }

        /// <summary>
        /// Tester, at konstruktøren initierer AdressekartotekCommandHandlerBase.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererAdressekartotekCommandHandlerBase()
        {
            var adresseRepositoryMock = MockRepository.GenerateMock<IAdresseRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();
            
            var commandHandler = new MyCommandHandler(adresseRepositoryMock, objectMapperMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);
            Assert.That(commandHandler.Repository, Is.Not.Null);
            Assert.That(commandHandler.Repository, Is.EqualTo(adresseRepositoryMock));
            Assert.That(commandHandler.ObjectMapper, Is.Not.Null);
            Assert.That(commandHandler.ObjectMapper, Is.EqualTo(objectMapperMock));
            Assert.That(commandHandler.ExceptionBuilder, Is.Not.Null);
            Assert.That(commandHandler.ExceptionBuilder, Is.EqualTo(exceptionBuilderMock));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis repository til adresser er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisAdresseRepositoryErNull()
        {
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyCommandHandler(null, objectMapperMock, exceptionBuilderMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("adresseRepository"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis objectmapperen er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisObjectMapperErNull()
        {
            var adresseRepositoryMock = MockRepository.GenerateMock<IAdresseRepository>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyCommandHandler(adresseRepositoryMock, null, exceptionBuilderMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("objectMapper"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis builderen, der kan bygge exceptions, er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisExceptionBuilderErNull()
        {
            var adresseRepositoryMock = MockRepository.GenerateMock<IAdresseRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyCommandHandler(adresseRepositoryMock, objectMapperMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("exceptionBuilder"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at AdresseGetByNummer henter en given adresse.
        /// </summary>
        [Test]
        public void TestAtAdresseGetByNummerHenterAdresse()
        {
            var fixture = new Fixture();
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var personer = fixture.CreateMany<Person>(3).ToList();
            var firmaer = fixture.CreateMany<Firma>(3).ToList();
            var adresser = new List<AdresseBase>();
            adresser.AddRange(personer);
            adresser.AddRange(firmaer);
            var adresseRepositoryMock = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepositoryMock.Expect(m => m.AdresseGetAll())
                .Return(adresser)
                .Repeat.Any();

            var commandHandler = new MyCommandHandler(adresseRepositoryMock, objectMapperMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            var adresse = commandHandler.AdresseGetByNummer(adresser.ElementAt(1).Nummer);
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
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var personer = fixture.CreateMany<Person>(3).ToList();
            var firmaer = fixture.CreateMany<Firma>(3).ToList();
            var adresser = new List<AdresseBase>();
            adresser.AddRange(personer);
            adresser.AddRange(firmaer);
            var adresseRepositoryMock = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepositoryMock.Expect(m => m.AdresseGetAll())
                .Return(adresser)
                .Repeat.Any();

            var commandHandler = new MyCommandHandler(adresseRepositoryMock, objectMapperMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => commandHandler.AdresseGetByNummer(-1));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (AdresseBase).Name, -1)));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.TypeOf<InvalidOperationException>());
        }

        /// <summary>
        /// Tester, at PersonGetAll henter alle personer.
        /// </summary>
        [Test]
        public void TestAtPersonGetAllHenterAllePersoner()
        {
            var fixture = new Fixture();
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var personer = fixture.CreateMany<Person>(3).ToList();
            var firmaer = fixture.CreateMany<Firma>(3).ToList();
            var adresser = new List<AdresseBase>();
            adresser.AddRange(personer);
            adresser.AddRange(firmaer);
            var adresseRepositoryMock = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepositoryMock.Expect(m => m.AdresseGetAll())
                .Return(adresser)
                .Repeat.Any();

            var commandHandler = new MyCommandHandler(adresseRepositoryMock, objectMapperMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            var result = commandHandler.PersonGetAll();
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
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var personer = fixture.CreateMany<Person>(3).ToList();
            var firmaer = fixture.CreateMany<Firma>(3).ToList();
            var adresser = new List<AdresseBase>();
            adresser.AddRange(personer);
            adresser.AddRange(firmaer);
            var adresseRepositoryMock = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepositoryMock.Expect(m => m.AdresseGetAll())
                .Return(adresser)
                .Repeat.Any();

            var commandHandler = new MyCommandHandler(adresseRepositoryMock, objectMapperMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            var person = commandHandler.PersonGetByNummer(personer.ElementAt(1).Nummer);
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
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var personer = fixture.CreateMany<Person>(3).ToList();
            var firmaer = fixture.CreateMany<Firma>(3).ToList();
            var adresser = new List<AdresseBase>();
            adresser.AddRange(personer);
            adresser.AddRange(firmaer);
            var adresseRepositoryMock = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepositoryMock.Expect(m => m.AdresseGetAll())
                .Return(adresser)
                .Repeat.Any();

            var commandHandler = new MyCommandHandler(adresseRepositoryMock, objectMapperMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => commandHandler.PersonGetByNummer(-1));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (Person).Name, -1)));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.TypeOf<InvalidOperationException>());
        }

        /// <summary>
        /// Tester, at FirmaGetAll henter alle firmaer.
        /// </summary>
        [Test]
        public void TestAtFirmaGetAllHenterAlleFirmaer()
        {
            var fixture = new Fixture();
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var personer = fixture.CreateMany<Person>(3).ToList();
            var firmaer = fixture.CreateMany<Firma>(3).ToList();
            var adresser = new List<AdresseBase>();
            adresser.AddRange(personer);
            adresser.AddRange(firmaer);
            var adresseRepositoryMock = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepositoryMock.Expect(m => m.AdresseGetAll())
                .Return(adresser)
                .Repeat.Any();

            var commandHandler = new MyCommandHandler(adresseRepositoryMock, objectMapperMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            var result = commandHandler.FirmaGetAll();
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
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var personer = fixture.CreateMany<Person>(3).ToList();
            var firmaer = fixture.CreateMany<Firma>(3).ToList();
            var adresser = new List<AdresseBase>();
            adresser.AddRange(personer);
            adresser.AddRange(firmaer);
            var adresseRepositoryMock = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepositoryMock.Expect(m => m.AdresseGetAll())
                .Return(adresser)
                .Repeat.Any();

            var commandHandler = new MyCommandHandler(adresseRepositoryMock, objectMapperMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            var firma = commandHandler.FirmaGetByNummer(firmaer.ElementAt(1).Nummer);
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
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var personer = fixture.CreateMany<Person>(3).ToList();
            var firmaer = fixture.CreateMany<Firma>(3).ToList();
            var adresser = new List<AdresseBase>();
            adresser.AddRange(personer);
            adresser.AddRange(firmaer);
            var adresseRepositoryMock = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepositoryMock.Expect(m => m.AdresseGetAll())
                .Return(adresser)
                .Repeat.Any();

            var commandHandler = new MyCommandHandler(adresseRepositoryMock, objectMapperMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => commandHandler.FirmaGetByNummer(-1));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (Firma).Name, -1)));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.TypeOf<InvalidOperationException>());
        }
    }
}
