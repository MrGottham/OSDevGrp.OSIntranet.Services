﻿using System;
using System.Linq;
using OSDevGrp.OSIntranet.CommandHandlers.Core;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Fælles;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.CommandHandlers.Core
{
    /// <summary>
    /// Tester basisklasse for en CommandHandler til regnskaber.
    /// </summary>
    [TestFixture]
    public class RegnskabCommandHandlerBaseTests
    {
        /// <summary>
        /// Egen klasse til test af basisklasse for en CommandHandler til regnskaber. 
        /// </summary>
        private class MyCommandHandler : RegnskabCommandHandlerBase
        {
            /// <summary>
            /// Danner egen klasse til test af basisklasse for en CommandHandler til regnskaber. 
            /// </summary>
            /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
            /// <param name="adresseRepository">Implementering af repository til adresser.</param>
            /// <param name="fællesRepository">Implementering af repository til fælles elementer i domænet.</param>
            /// <param name="objectMapper">Implementering af objectmapper.</param>
            public MyCommandHandler(IFinansstyringRepository finansstyringRepository, IAdresseRepository adresseRepository, IFællesRepository fællesRepository, IObjectMapper objectMapper)
                : base(finansstyringRepository, adresseRepository, fællesRepository, objectMapper)
            {
            }
        }

        /// <summary>
        /// Tester, at konstruktøren initierer RegnskabQueryHandlerBase.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererRegnskabQueryHandlerBase()
        {
            var finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();

            var commandHandler = new MyCommandHandler(finansstyringRepository, adresseRepository, fællesRepository,
                                                  objectMapper);
            Assert.That(commandHandler, Is.Not.Null);
            Assert.That(commandHandler.Repository, Is.Not.Null);
            Assert.That(commandHandler.ObjectMapper, Is.Not.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis repository til finansstyring er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisFinansstyringRepositoryErNull()
        {
            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            Assert.Throws<ArgumentNullException>(
                () => new MyCommandHandler(null, adresseRepository, fællesRepository, objectMapper));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis repository til adresser er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisAdresseRepositoryErNull()
        {
            var finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            Assert.Throws<ArgumentNullException>(
                () => new MyCommandHandler(finansstyringRepository, null, fællesRepository, objectMapper));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis repository til fælles elementer i domænet er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisFællesRepositoryErNull()
        {
            var finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            Assert.Throws<ArgumentNullException>(
                () => new MyCommandHandler(finansstyringRepository, adresseRepository, null, objectMapper));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis objektmapperen er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisObjectMapperErNull()
        {
            var finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            Assert.Throws<ArgumentNullException>(
                () => new MyCommandHandler(finansstyringRepository, adresseRepository, fællesRepository, null));
        }

        /// <summary>
        /// Tester, at RegnskabGetByNummer henter et givent regnskab.
        /// </summary>
        [Test]
        public void TestAtRegnskabGetByNummerHenterRegnskab()
        {
            var fixture = new Fixture();
            var regnskaber = fixture.CreateMany<Regnskab>(3).ToList();

            var finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepository.Expect(
                m =>
                m.RegnskabGet(Arg<int>.Is.Equal(regnskaber.ElementAt(1).Nummer), Arg<Func<int, Brevhoved>>.Is.NotNull,
                              Arg<Func<int, AdresseBase>>.Is.NotNull))
                .Return(regnskaber.ElementAt(1));
            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepository.Expect(m => m.AdresseGetAll())
                .Return(fixture.CreateMany<Person>(3));
            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepository.Expect(m => m.BrevhovedGetAll())
                .Return(fixture.CreateMany<Brevhoved>(3));
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();

            var commandHandler = new MyCommandHandler(finansstyringRepository, adresseRepository, fællesRepository,
                                                      objectMapper);
            Assert.That(commandHandler, Is.Not.Null);

            var regnskab = commandHandler.RegnskabGetByNummer(regnskaber.ElementAt(1).Nummer);
            Assert.That(regnskab, Is.Not.Null);
            Assert.That(regnskab.Nummer, Is.EqualTo(regnskaber.ElementAt(1).Nummer));
            Assert.That(regnskab.Navn, Is.Not.Null);
            Assert.That(regnskab.Navn, Is.EqualTo(regnskaber.ElementAt(1).Navn));

            finansstyringRepository.AssertWasCalled(
                m =>
                m.RegnskabGet(Arg<int>.Is.Equal(regnskaber.ElementAt(1).Nummer), Arg<Func<int, Brevhoved>>.Is.NotNull,
                              Arg<Func<int, AdresseBase>>.Is.NotNull));
            adresseRepository.AssertWasCalled(m => m.AdresseGetAll());
            fællesRepository.AssertWasCalled(m => m.BrevhovedGetAll());
        }

        /// <summary>
        /// Tester, at KontoGetAllByRegnskab henter et givent regnskab.
        /// </summary>
        [Test]
        public void TestAtKontoGetAllByRegnskabHenterRegnskab()
        {
            var fixture = new Fixture();
            var regnskaber = fixture.CreateMany<Regnskab>(3).ToList();

            fixture.Inject(regnskaber.ElementAt(1));
            var konti = fixture.CreateMany<Konto>(3).ToList();
            var budgetkonti = fixture.CreateMany<Budgetkonto>(3).ToList();
            konti.ForEach(regnskaber.ElementAt(1).TilføjKonto);
            budgetkonti.ForEach(regnskaber.ElementAt(1).TilføjKonto);

            var finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepository.Expect(
                m =>
                m.RegnskabGet(Arg<int>.Is.Equal(regnskaber.ElementAt(1).Nummer), Arg<Func<int, Brevhoved>>.Is.NotNull,
                              Arg<Func<int, AdresseBase>>.Is.NotNull))
                .Return(regnskaber.ElementAt(1));
            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepository.Expect(m => m.AdresseGetAll())
                .Return(fixture.CreateMany<Person>(3));
            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepository.Expect(m => m.BrevhovedGetAll())
                .Return(fixture.CreateMany<Brevhoved>(3));
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();

            var commandHandler = new MyCommandHandler(finansstyringRepository, adresseRepository, fællesRepository,
                                                      objectMapper);
            Assert.That(commandHandler, Is.Not.Null);

            var result = commandHandler.KontoGetAllByRegnskab(regnskaber.ElementAt(1).Nummer);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(3));
        }

        /// <summary>
        /// Tester, at KontoGetByRegnskabAndKontonummer kaster en ArgumentNullException, hvis kontonummeret er null.
        /// </summary>
        [Test]
        public void TestAtKontoGetByRegnskabAndKontonummerKasterEnArgumentNullExceptionHvisKontonummerErNull()
        {
            var fixture = new Fixture();

            var finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();

            var commandHandler = new MyCommandHandler(finansstyringRepository, adresseRepository, fællesRepository,
                                                      objectMapper);
            Assert.That(commandHandler, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(
                () => commandHandler.KontoGetByRegnskabAndKontonummer(fixture.CreateAnonymous<int>(), null));
        }

        /// <summary>
        /// Tester, at KontoGetByRegnskabAndKontonummer henter en given konto fra et givent regnskab.
        /// </summary>
        [Test]
        public void TestAtKontoGetByRegnskabAndKontonummerHenterKontoFraRegnskab()
        {
            var fixture = new Fixture();
            var regnskaber = fixture.CreateMany<Regnskab>(3).ToList();

            fixture.Inject(regnskaber.ElementAt(1));
            var konti = fixture.CreateMany<Konto>(3).ToList();
            var budgetkonti = fixture.CreateMany<Budgetkonto>(3).ToList();
            konti.ForEach(regnskaber.ElementAt(1).TilføjKonto);
            budgetkonti.ForEach(regnskaber.ElementAt(1).TilføjKonto);

            var finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepository.Expect(
                m =>
                m.RegnskabGet(Arg<int>.Is.Equal(regnskaber.ElementAt(1).Nummer), Arg<Func<int, Brevhoved>>.Is.NotNull,
                              Arg<Func<int, AdresseBase>>.Is.NotNull))
                .Return(regnskaber.ElementAt(1));
            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepository.Expect(m => m.AdresseGetAll())
                .Return(fixture.CreateMany<Person>(3));
            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepository.Expect(m => m.BrevhovedGetAll())
                .Return(fixture.CreateMany<Brevhoved>(3));
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();

            var commandHandler = new MyCommandHandler(finansstyringRepository, adresseRepository, fællesRepository,
                                                      objectMapper);
            Assert.That(commandHandler, Is.Not.Null);

            var konto = commandHandler.KontoGetByRegnskabAndKontonummer(konti.ElementAt(1).Regnskab.Nummer,
                                                                        konti.ElementAt(1).Kontonummer);
            Assert.That(konto, Is.Not.Null);
            Assert.That(konto.Kontonummer, Is.Not.Null);
            Assert.That(konto.Kontonummer, Is.EqualTo(konti.ElementAt(1).Kontonummer));
            Assert.That(konto.Kontonavn, Is.Not.Null);
            Assert.That(konto.Kontonavn, Is.EqualTo(konti.ElementAt(1).Kontonavn));
        }

        /// <summary>
        /// Tester, at KontoGetByRegnskabAndKontonummer kaster en IntranetRepositoryException, hvis kontoen ikke findes.
        /// </summary>
        [Test]
        public void TestAtKontoGetByRegnskabAndKontonummerKasterEnIntranetRepositoryExceptionHvisKontoIkkeFindes()
        {
            var fixture = new Fixture();
            var regnskaber = fixture.CreateMany<Regnskab>(3).ToList();

            fixture.Inject(regnskaber.ElementAt(1));
            var konti = fixture.CreateMany<Konto>(3).ToList();
            var budgetkonti = fixture.CreateMany<Budgetkonto>(3).ToList();
            konti.ForEach(regnskaber.ElementAt(1).TilføjKonto);
            budgetkonti.ForEach(regnskaber.ElementAt(1).TilføjKonto);

            var finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepository.Expect(
                m =>
                m.RegnskabGet(Arg<int>.Is.Equal(regnskaber.ElementAt(1).Nummer), Arg<Func<int, Brevhoved>>.Is.NotNull,
                              Arg<Func<int, AdresseBase>>.Is.NotNull))
                .Return(regnskaber.ElementAt(1));
            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepository.Expect(m => m.AdresseGetAll())
                .Return(fixture.CreateMany<Person>(3));
            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepository.Expect(m => m.BrevhovedGetAll())
                .Return(fixture.CreateMany<Brevhoved>(3));
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();

            var commandHandler = new MyCommandHandler(finansstyringRepository, adresseRepository, fællesRepository,
                                                      objectMapper);
            Assert.That(commandHandler, Is.Not.Null);

            Assert.Throws<IntranetRepositoryException>(
                () =>
                commandHandler.KontoGetByRegnskabAndKontonummer(regnskaber.ElementAt(1).Nummer,
                                                                fixture.CreateAnonymous<string>()));
        }

        /// <summary>
        /// Tester, at BudgetkontoGetAllByRegnskab henter et givent regnskab.
        /// </summary>
        [Test]
        public void TestAtBudgetkontoGetAllByRegnskabHenterRegnskab()
        {
            var fixture = new Fixture();
            var regnskaber = fixture.CreateMany<Regnskab>(3).ToList();

            fixture.Inject(regnskaber.ElementAt(1));
            var konti = fixture.CreateMany<Konto>(3).ToList();
            var budgetkonti = fixture.CreateMany<Budgetkonto>(3).ToList();
            konti.ForEach(regnskaber.ElementAt(1).TilføjKonto);
            budgetkonti.ForEach(regnskaber.ElementAt(1).TilføjKonto);

            var finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepository.Expect(
                m =>
                m.RegnskabGet(Arg<int>.Is.Equal(regnskaber.ElementAt(1).Nummer), Arg<Func<int, Brevhoved>>.Is.NotNull,
                              Arg<Func<int, AdresseBase>>.Is.NotNull))
                .Return(regnskaber.ElementAt(1));
            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepository.Expect(m => m.AdresseGetAll())
                .Return(fixture.CreateMany<Person>(3));
            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepository.Expect(m => m.BrevhovedGetAll())
                .Return(fixture.CreateMany<Brevhoved>(3));
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();

            var commandHandler = new MyCommandHandler(finansstyringRepository, adresseRepository, fællesRepository,
                                                      objectMapper);
            Assert.That(commandHandler, Is.Not.Null);

            var result = commandHandler.BudgetkontoGetAllByRegnskab(regnskaber.ElementAt(1).Nummer);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(3));
        }

        /// <summary>
        /// Tester, at BudgetkontoGetByRegnskabAndKontonummer kaster en ArgumentNullException, hvis kontonummeret er null.
        /// </summary>
        [Test]
        public void TestAtBudgetkontoGetByRegnskabAndKontonummerKasterEnArgumentNullExceptionHvisKontonummerErNull()
        {
            var fixture = new Fixture();

            var finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();

            var commandHandler = new MyCommandHandler(finansstyringRepository, adresseRepository, fællesRepository,
                                                      objectMapper);
            Assert.That(commandHandler, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(
                () => commandHandler.BudgetkontoGetByRegnskabAndKontonummer(fixture.CreateAnonymous<int>(), null));
        }

        /// <summary>
        /// Tester, at BudgetkontoGetByRegnskabAndKontonummer henter en given budgetkonto fra et givent regnskab.
        /// </summary>
        [Test]
        public void TestAtBudgetkontoGetByRegnskabAndKontonummerHenterBudgetkontoFraRegnskab()
        {
            var fixture = new Fixture();
            var regnskaber = fixture.CreateMany<Regnskab>(3).ToList();

            fixture.Inject(regnskaber.ElementAt(1));
            var konti = fixture.CreateMany<Konto>(3).ToList();
            var budgetkonti = fixture.CreateMany<Budgetkonto>(3).ToList();
            konti.ForEach(regnskaber.ElementAt(1).TilføjKonto);
            budgetkonti.ForEach(regnskaber.ElementAt(1).TilføjKonto);

            var finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepository.Expect(
                m =>
                m.RegnskabGet(Arg<int>.Is.Equal(regnskaber.ElementAt(1).Nummer), Arg<Func<int, Brevhoved>>.Is.NotNull,
                              Arg<Func<int, AdresseBase>>.Is.NotNull))
                .Return(regnskaber.ElementAt(1));
            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepository.Expect(m => m.AdresseGetAll())
                .Return(fixture.CreateMany<Person>(3));
            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepository.Expect(m => m.BrevhovedGetAll())
                .Return(fixture.CreateMany<Brevhoved>(3));
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();

            var commandHandler = new MyCommandHandler(finansstyringRepository, adresseRepository, fællesRepository,
                                                      objectMapper);
            Assert.That(commandHandler, Is.Not.Null);

            var budgetkonto =
                commandHandler.BudgetkontoGetByRegnskabAndKontonummer(budgetkonti.ElementAt(1).Regnskab.Nummer,
                                                                      budgetkonti.ElementAt(1).Kontonummer);
            Assert.That(budgetkonto, Is.Not.Null);
            Assert.That(budgetkonto.Kontonummer, Is.Not.Null);
            Assert.That(budgetkonto.Kontonummer, Is.EqualTo(budgetkonti.ElementAt(1).Kontonummer));
            Assert.That(budgetkonto.Kontonavn, Is.Not.Null);
            Assert.That(budgetkonto.Kontonavn, Is.EqualTo(budgetkonti.ElementAt(1).Kontonavn));
        }

        /// <summary>
        /// Tester, at BudgetkontoGetByRegnskabAndKontonummer kaster en IntranetRepositoryException, hvis budgetkontoen ikke findes.
        /// </summary>
        [Test]
        public void TestAtBudgetkontoGetByRegnskabAndKontonummerKasterEnIntranetRepositoryExceptionHvisBudgetkontoIkkeFindes()
        {
            var fixture = new Fixture();
            var regnskaber = fixture.CreateMany<Regnskab>(3).ToList();

            fixture.Inject(regnskaber.ElementAt(1));
            var konti = fixture.CreateMany<Konto>(3).ToList();
            var budgetkonti = fixture.CreateMany<Budgetkonto>(3).ToList();
            konti.ForEach(regnskaber.ElementAt(1).TilføjKonto);
            budgetkonti.ForEach(regnskaber.ElementAt(1).TilføjKonto);

            var finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepository.Expect(
                m =>
                m.RegnskabGet(Arg<int>.Is.Equal(regnskaber.ElementAt(1).Nummer), Arg<Func<int, Brevhoved>>.Is.NotNull,
                              Arg<Func<int, AdresseBase>>.Is.NotNull))
                .Return(regnskaber.ElementAt(1));
            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepository.Expect(m => m.AdresseGetAll())
                .Return(fixture.CreateMany<Person>(3));
            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepository.Expect(m => m.BrevhovedGetAll())
                .Return(fixture.CreateMany<Brevhoved>(3));
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();

            var commandHandler = new MyCommandHandler(finansstyringRepository, adresseRepository, fællesRepository,
                                                      objectMapper);
            Assert.That(commandHandler, Is.Not.Null);

            Assert.Throws<IntranetRepositoryException>(
                () =>
                commandHandler.BudgetkontoGetByRegnskabAndKontonummer(regnskaber.ElementAt(1).Nummer,
                                                                      fixture.CreateAnonymous<string>()));
        }
    }
}
