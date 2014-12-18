using System;
using System.Linq;
using System.Reflection;
using OSDevGrp.OSIntranet.CommandHandlers;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Fælles;
using OSDevGrp.OSIntranet.Contracts.Commands;
using OSDevGrp.OSIntranet.Contracts.Responses;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Domain.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.CommandHandlers
{
    /// <summary>
    /// Tester CommandHandler til håndtering af kommandoen: BogføringslinjeOpretCommand.
    /// </summary>
    [TestFixture]
    public class BogføringslinjeOpretCommandHandlerTests
    {
        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis konfigurationsrepository er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisKonfigurationRepositoryErNull()
        {
            var fixture = new Fixture();

            var finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();

            fixture.Inject(finansstyringRepository);
            fixture.Inject(adresseRepository);
            fixture.Inject(fællesRepository);
            fixture.Inject<IKonfigurationRepository>(null);
            fixture.Inject(objectMapper);

            Assert.That(Assert.Throws<TargetInvocationException>(() => fixture.Create<BogføringslinjeOpretCommandHandler>()).InnerException, Is.TypeOf(typeof (ArgumentNullException)));
        }

        /// <summary>
        /// Tester, at Execute tilføjer en bogføringslinje uden adressekonto.
        /// </summary>
        [Test]
        public void TestAtExecuteTilføjerBogføringslinjeUdenAdressekonto()
        {
            var fixture = new Fixture();
            fixture.Inject<KontoBaseView>(new KontoView());

            var regnskab = fixture.Create<Regnskab>();
            foreach (var konto in fixture.CreateMany<Konto>(25))
            {
                regnskab.TilføjKonto(konto);
            }
            foreach (var budgetkonto in fixture.CreateMany<Budgetkonto>(25))
            {
                regnskab.TilføjKonto(budgetkonto);
            }
            var finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepository.Expect(m => m.RegnskabGet(Arg<int>.Is.Anything, Arg<Func<int, Brevhoved>>.Is.NotNull, Arg<Func<int, AdresseBase>>.Is.NotNull))
                .Return(regnskab);
            finansstyringRepository.Expect(m => m.BogføringslinjeAdd(Arg<DateTime>.Is.Anything, Arg<string>.Is.Anything, Arg<Konto>.Is.NotNull, Arg<string>.Is.NotNull, Arg<Budgetkonto>.Is.NotNull, Arg<decimal>.Is.GreaterThan(0M), Arg<decimal>.Is.GreaterThan(0M), Arg<AdresseBase>.Is.Null))
                .WhenCalled(m =>
                                {
                                    var bogføringslinje = new Bogføringslinje(fixture.Create<int>(), (DateTime) m.Arguments[0], (string) m.Arguments[1], (string) m.Arguments[3], (decimal) m.Arguments[5], (decimal) m.Arguments[6]);
                                    if (m.Arguments[2] as Konto != null)
                                    {
                                        ((Konto) m.Arguments[2]).TilføjBogføringslinje(bogføringslinje);
                                    }
                                    if (m.Arguments[4] as Budgetkonto != null)
                                    {
                                        ((Budgetkonto) m.Arguments[4]).TilføjBogføringslinje(bogføringslinje);
                                    }
                                    m.ReturnValue = bogføringslinje;
                                })
                .Return(null);
            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            var adressekonti = fixture.CreateMany<Person>(25).ToList();
            adresseRepository.Expect(m => m.AdresseGetAll())
                .Return(adressekonti);
            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepository.Expect(m => m.BrevhovedGetAll())
                .Return(fixture.CreateMany<Brevhoved>(3));
            var konfigurationRepository = MockRepository.GenerateMock<IKonfigurationRepository>();
            konfigurationRepository.Expect(m => m.DageForBogføringsperiode)
                .Return(30);
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            fixture.Inject(fixture.CreateMany<BogføringsadvarselResponse>(3));
            objectMapper.Expect(m => m.Map<IBogføringsresultat, BogføringslinjeOpretResponse>(Arg<IBogføringsresultat>.Is.NotNull))
                .Return(fixture.Create<BogføringslinjeOpretResponse>());

            fixture.Inject(finansstyringRepository);
            fixture.Inject(adresseRepository);
            fixture.Inject(fællesRepository);
            fixture.Inject(konfigurationRepository);
            fixture.Inject(objectMapper);

            var commandHandler = fixture.Create<BogføringslinjeOpretCommandHandler>();
            Assert.That(commandHandler, Is.Not.Null);

            fixture.Inject(DateTime.Now);
            var command = new BogføringslinjeOpretCommand
                              {
                                  Regnskabsnummer = regnskab.Nummer,
                                  Dato = fixture.Create<DateTime>(),
                                  Bilag = fixture.Create<string>(),
                                  Kontonummer = regnskab.Konti.OfType<Konto>().ElementAt(1).Kontonummer,
                                  Tekst = fixture.Create<string>(),
                                  Budgetkontonummer = regnskab.Konti.OfType<Budgetkonto>().ElementAt(1).Kontonummer,
                                  Debit = fixture.Create<decimal>(),
                                  Kredit = fixture.Create<decimal>()
                              };
            var result = commandHandler.Execute(command);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Advarsler, Is.Not.Null);
 
            finansstyringRepository.AssertWasCalled(m => m.BogføringslinjeAdd(Arg<DateTime>.Is.Equal(command.Dato), Arg<string>.Is.Equal(command.Bilag), Arg<Konto>.Is.NotNull, Arg<string>.Is.Equal(command.Tekst), Arg<Budgetkonto>.Is.NotNull, Arg<decimal>.Is.Equal(command.Debit), Arg<decimal>.Is.Equal(command.Kredit), Arg<AdresseBase>.Is.Null));
            objectMapper.AssertWasCalled(m => m.Map<IBogføringsresultat, BogføringslinjeOpretResponse>(Arg<IBogføringsresultat>.Is.NotNull));
        }

        /// <summary>
        /// Tester, at Execute tilføjer en bogføringslinje med adressekonto.
        /// </summary>
        [Test]
        public void TestAtExecuteTilføjerBogføringslinjeMedAdressekonto()
        {
            var fixture = new Fixture();
            fixture.Inject<KontoBaseView>(new KontoView());

            var regnskab = fixture.Create<Regnskab>();
            foreach (var konto in fixture.CreateMany<Konto>(25))
            {
                regnskab.TilføjKonto(konto);
            }
            foreach (var budgetkonto in fixture.CreateMany<Budgetkonto>(25))
            {
                regnskab.TilføjKonto(budgetkonto);
            }
            var finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepository.Expect(m => m.RegnskabGet(Arg<int>.Is.Anything, Arg<Func<int, Brevhoved>>.Is.NotNull, Arg<Func<int, AdresseBase>>.Is.NotNull))
                .Return(regnskab);
            finansstyringRepository.Expect(m => m.BogføringslinjeAdd(Arg<DateTime>.Is.Anything, Arg<string>.Is.Anything, Arg<Konto>.Is.NotNull, Arg<string>.Is.NotNull, Arg<Budgetkonto>.Is.NotNull, Arg<decimal>.Is.GreaterThan(0M), Arg<decimal>.Is.GreaterThan(0M), Arg<AdresseBase>.Is.NotNull))
                .WhenCalled(m =>
                                {
                                    var bogføringslinje = new Bogføringslinje(fixture.Create<int>(), (DateTime) m.Arguments[0], (string) m.Arguments[1], (string) m.Arguments[3], (decimal) m.Arguments[5], (decimal) m.Arguments[6]);
                                    if (m.Arguments[2] as Konto != null)
                                    {
                                        ((Konto) m.Arguments[2]).TilføjBogføringslinje(bogføringslinje);
                                    }
                                    if (m.Arguments[4] as Budgetkonto != null)
                                    {
                                        ((Budgetkonto) m.Arguments[4]).TilføjBogføringslinje(bogføringslinje);
                                    }
                                    if (m.Arguments[7] as AdresseBase != null)
                                    {
                                        ((AdresseBase) m.Arguments[7]).TilføjBogføringslinje(bogføringslinje);
                                    }
                                    m.ReturnValue = bogføringslinje;
                                })
                .Return(null);
            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            var adressekonti = fixture.CreateMany<Person>(25).ToList();
            adresseRepository.Expect(m => m.AdresseGetAll())
                .Return(adressekonti);
            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepository.Expect(m => m.BrevhovedGetAll())
                .Return(fixture.CreateMany<Brevhoved>(3));
            var konfigurationRepository = MockRepository.GenerateMock<IKonfigurationRepository>();
            konfigurationRepository.Expect(m => m.DageForBogføringsperiode)
                .Return(30);
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            fixture.Inject(fixture.CreateMany<BogføringsadvarselResponse>(3));
            objectMapper.Expect(m => m.Map<IBogføringsresultat, BogføringslinjeOpretResponse>(Arg<IBogføringsresultat>.Is.NotNull))
                .Return(fixture.Create<BogføringslinjeOpretResponse>());

            fixture.Inject(finansstyringRepository);
            fixture.Inject(adresseRepository);
            fixture.Inject(fællesRepository);
            fixture.Inject(konfigurationRepository);
            fixture.Inject(objectMapper);

            var commandHandler = fixture.Create<BogføringslinjeOpretCommandHandler>();
            Assert.That(commandHandler, Is.Not.Null);

            fixture.Inject(DateTime.Now);
            var command = new BogføringslinjeOpretCommand
                              {
                                  Regnskabsnummer = regnskab.Nummer,
                                  Dato = fixture.Create<DateTime>(),
                                  Bilag = fixture.Create<string>(),
                                  Kontonummer = regnskab.Konti.OfType<Konto>().ElementAt(1).Kontonummer,
                                  Tekst = fixture.Create<string>(),
                                  Budgetkontonummer = regnskab.Konti.OfType<Budgetkonto>().ElementAt(1).Kontonummer,
                                  Debit = fixture.Create<decimal>(),
                                  Kredit = fixture.Create<decimal>(),
                                  Adressekonto = adressekonti.ElementAt(1).Nummer
                              };
            var result = commandHandler.Execute(command);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Advarsler, Is.Not.Null);

            finansstyringRepository.AssertWasCalled(m => m.BogføringslinjeAdd(Arg<DateTime>.Is.Equal(command.Dato), Arg<string>.Is.Equal(command.Bilag), Arg<Konto>.Is.NotNull, Arg<string>.Is.Equal(command.Tekst), Arg<Budgetkonto>.Is.NotNull, Arg<decimal>.Is.Equal(command.Debit), Arg<decimal>.Is.Equal(command.Kredit), Arg<AdresseBase>.Is.NotNull));
            objectMapper.AssertWasCalled(m => m.Map<IBogføringsresultat, BogføringslinjeOpretResponse>(Arg<IBogføringsresultat>.Is.NotNull));
        }

        /// <summary>
        /// Tester, at Execute kaster en ArgumentNullException, hvis kommandoen er null.
        /// </summary>
        [Test]
        public void TestAtExecuteKasterArgumentNullExceptionHvisCommandErNull()
        {
            var fixture = new Fixture();

            var finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            var konfigurationRepository = MockRepository.GenerateMock<IKonfigurationRepository>();
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();

            fixture.Inject(finansstyringRepository);
            fixture.Inject(adresseRepository);
            fixture.Inject(fællesRepository);
            fixture.Inject(konfigurationRepository);
            fixture.Inject(objectMapper);

            var commandHandler = fixture.Create<BogføringslinjeOpretCommandHandler>();
            Assert.That(commandHandler, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => commandHandler.Execute(null));
        }

        /// <summary>
        /// Tester, at Execute kaster en IntranetRepositoryException, hvis regnskabet ikke findes.
        /// </summary>
        [Test]
        public void TestAtExecuteKasterIntranetRepositoryExceptionHvisRegnskabIkkeFindes()
        {
            var fixture = new Fixture();

            var finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepository.Expect(
                m =>
                m.RegnskabGet(Arg<int>.Is.Anything, Arg<Func<int, Brevhoved>>.Is.NotNull,
                              Arg<Func<int, AdresseBase>>.Is.NotNull))
                .Throw(fixture.Create<IntranetRepositoryException>());
            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepository.Expect(m => m.AdresseGetAll())
                .Return(fixture.CreateMany<Person>(25));
            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepository.Expect(m => m.BrevhovedGetAll())
                .Return(fixture.CreateMany<Brevhoved>(3));
            var konfigurationRepository = MockRepository.GenerateMock<IKonfigurationRepository>();
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();

            fixture.Inject(finansstyringRepository);
            fixture.Inject(adresseRepository);
            fixture.Inject(fællesRepository);
            fixture.Inject(konfigurationRepository);
            fixture.Inject(objectMapper);

            var commandHandler = fixture.Create<BogføringslinjeOpretCommandHandler>();
            Assert.That(commandHandler, Is.Not.Null);

            var command = new BogføringslinjeOpretCommand
                              {
                                  Regnskabsnummer = fixture.Create<int>(),
                                  Dato = fixture.Create<DateTime>(),
                                  Bilag = fixture.Create<string>(),
                                  Kontonummer = fixture.Create<string>(),
                                  Tekst = fixture.Create<string>(),
                                  Budgetkontonummer = fixture.Create<string>(),
                                  Debit = fixture.Create<decimal>(),
                                  Kredit = fixture.Create<decimal>(),
                                  Adressekonto = fixture.Create<int>()
                              };
            Assert.Throws<IntranetRepositoryException>(() => commandHandler.Execute(command));
        }

        /// <summary>
        /// Tester, at Execute kaster en IntranetBusinessException, hvis bogføringsdato er for gammel.
        /// </summary>
        [Test]
        public void TestAtExecuteKasterIntranetBusinessExceptionHvisBogføringsdatoErForGammel()
        {
            var fixture = new Fixture();

            var regnskab = fixture.Create<Regnskab>();
            var finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepository.Expect(
                m =>
                m.RegnskabGet(Arg<int>.Is.Anything, Arg<Func<int, Brevhoved>>.Is.NotNull,
                              Arg<Func<int, AdresseBase>>.Is.NotNull))
                .Return(regnskab);
            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepository.Expect(m => m.AdresseGetAll())
                .Return(fixture.CreateMany<Person>(25));
            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepository.Expect(m => m.BrevhovedGetAll())
                .Return(fixture.CreateMany<Brevhoved>(3));
            var konfigurationRepository = MockRepository.GenerateMock<IKonfigurationRepository>();
            konfigurationRepository.Expect(m => m.DageForBogføringsperiode)
                .Return(30);
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();

            fixture.Inject(finansstyringRepository);
            fixture.Inject(adresseRepository);
            fixture.Inject(fællesRepository);
            fixture.Inject(konfigurationRepository);
            fixture.Inject(objectMapper);

            var commandHandler = fixture.Create<BogføringslinjeOpretCommandHandler>();
            Assert.That(commandHandler, Is.Not.Null);

            fixture.Inject(DateTime.Now.AddDays(-31));
            var command = new BogføringslinjeOpretCommand
                              {
                                  Regnskabsnummer = regnskab.Nummer,
                                  Dato = fixture.Create<DateTime>(),
                                  Bilag = fixture.Create<string>(),
                                  Kontonummer = fixture.Create<string>(),
                                  Tekst = fixture.Create<string>(),
                                  Budgetkontonummer = fixture.Create<string>(),
                                  Debit = fixture.Create<decimal>(),
                                  Kredit = fixture.Create<decimal>(),
                                  Adressekonto = fixture.Create<int>()
                              };
            Assert.Throws<IntranetBusinessException>(() => commandHandler.Execute(command));
        }

        /// <summary>
        /// Tester, at Execute kaster en IntranetBusinessException, hvis bogføringsdato er fremme i tiden.
        /// </summary>
        [Test]
        public void TestAtExecuteKasterIntranetBusinessExceptionHvisBogføringsdatoErFremmeITiden()
        {
            var fixture = new Fixture();

            var regnskab = fixture.Create<Regnskab>();
            var finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepository.Expect(
                m =>
                m.RegnskabGet(Arg<int>.Is.Anything, Arg<Func<int, Brevhoved>>.Is.NotNull,
                              Arg<Func<int, AdresseBase>>.Is.NotNull))
                .Return(regnskab);
            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepository.Expect(m => m.AdresseGetAll())
                .Return(fixture.CreateMany<Person>(25));
            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepository.Expect(m => m.BrevhovedGetAll())
                .Return(fixture.CreateMany<Brevhoved>(3));
            var konfigurationRepository = MockRepository.GenerateMock<IKonfigurationRepository>();
            konfigurationRepository.Expect(m => m.DageForBogføringsperiode)
                .Return(30);
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();

            fixture.Inject(finansstyringRepository);
            fixture.Inject(adresseRepository);
            fixture.Inject(fællesRepository);
            fixture.Inject(konfigurationRepository);
            fixture.Inject(objectMapper);

            var commandHandler = fixture.Create<BogføringslinjeOpretCommandHandler>();
            Assert.That(commandHandler, Is.Not.Null);

            fixture.Inject(DateTime.Now.AddDays(1));
            var command = new BogføringslinjeOpretCommand
                              {
                                  Regnskabsnummer = regnskab.Nummer,
                                  Dato = fixture.Create<DateTime>(),
                                  Bilag = fixture.Create<string>(),
                                  Kontonummer = fixture.Create<string>(),
                                  Tekst = fixture.Create<string>(),
                                  Budgetkontonummer = fixture.Create<string>(),
                                  Debit = fixture.Create<decimal>(),
                                  Kredit = fixture.Create<decimal>(),
                                  Adressekonto = fixture.Create<int>()
                              };
            Assert.Throws<IntranetBusinessException>(() => commandHandler.Execute(command));
        }

        /// <summary>
        /// Tester, at Execute kaster en IntranetBusinessException, hvis kontonummer ikke er angivet.
        /// </summary>
        [Test]
        public void TestAtExecuteKasterIntranetBusinessExceptionHvisKontonummerIkkeErAngivet()
        {
            var fixture = new Fixture();

            var regnskab = fixture.Create<Regnskab>();
            var finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepository.Expect(
                m =>
                m.RegnskabGet(Arg<int>.Is.Anything, Arg<Func<int, Brevhoved>>.Is.NotNull,
                              Arg<Func<int, AdresseBase>>.Is.NotNull))
                .Return(regnskab);
            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepository.Expect(m => m.AdresseGetAll())
                .Return(fixture.CreateMany<Person>(25));
            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepository.Expect(m => m.BrevhovedGetAll())
                .Return(fixture.CreateMany<Brevhoved>(3));
            var konfigurationRepository = MockRepository.GenerateMock<IKonfigurationRepository>();
            konfigurationRepository.Expect(m => m.DageForBogføringsperiode)
                .Return(30);
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();

            fixture.Inject(finansstyringRepository);
            fixture.Inject(adresseRepository);
            fixture.Inject(fællesRepository);
            fixture.Inject(konfigurationRepository);
            fixture.Inject(objectMapper);

            var commandHandler = fixture.Create<BogføringslinjeOpretCommandHandler>();
            Assert.That(commandHandler, Is.Not.Null);

            fixture.Inject(DateTime.Now);
            var command = new BogføringslinjeOpretCommand
                              {
                                  Regnskabsnummer = regnskab.Nummer,
                                  Dato = fixture.Create<DateTime>(),
                                  Bilag = fixture.Create<string>(),
                                  Kontonummer = null,
                                  Tekst = fixture.Create<string>(),
                                  Budgetkontonummer = fixture.Create<string>(),
                                  Debit = fixture.Create<decimal>(),
                                  Kredit = fixture.Create<decimal>(),
                                  Adressekonto = fixture.Create<int>()
                              };
            Assert.Throws<IntranetBusinessException>(() => commandHandler.Execute(command));
        }

        /// <summary>
        /// Tester, at Execute kaster en IntranetRepositoryException, hvis konto ikke findes.
        /// </summary>
        [Test]
        public void TestAtExecuteKasterIntranetRepositoryExceptionHvisKontoIkkeFindes()
        {
            var fixture = new Fixture();

            var regnskab = fixture.Create<Regnskab>();
            foreach (var konto in fixture.CreateMany<Konto>(25))
            {
                regnskab.TilføjKonto(konto);
            }
            var finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepository.Expect(
                m =>
                m.RegnskabGet(Arg<int>.Is.Anything, Arg<Func<int, Brevhoved>>.Is.NotNull,
                              Arg<Func<int, AdresseBase>>.Is.NotNull))
                .Return(regnskab);
            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepository.Expect(m => m.AdresseGetAll())
                .Return(fixture.CreateMany<Person>(25));
            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepository.Expect(m => m.BrevhovedGetAll())
                .Return(fixture.CreateMany<Brevhoved>(3));
            var konfigurationRepository = MockRepository.GenerateMock<IKonfigurationRepository>();
            konfigurationRepository.Expect(m => m.DageForBogføringsperiode)
                .Return(30);
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();

            fixture.Inject(finansstyringRepository);
            fixture.Inject(adresseRepository);
            fixture.Inject(fællesRepository);
            fixture.Inject(konfigurationRepository);
            fixture.Inject(objectMapper);

            var commandHandler = fixture.Create<BogføringslinjeOpretCommandHandler>();
            Assert.That(commandHandler, Is.Not.Null);

            fixture.Inject(DateTime.Now);
            var command = new BogføringslinjeOpretCommand
                              {
                                  Regnskabsnummer = regnskab.Nummer,
                                  Dato = fixture.Create<DateTime>(),
                                  Bilag = fixture.Create<string>(),
                                  Kontonummer = fixture.Create<string>(),
                                  Tekst = fixture.Create<string>(),
                                  Budgetkontonummer = fixture.Create<string>(),
                                  Debit = fixture.Create<decimal>(),
                                  Kredit = fixture.Create<decimal>(),
                                  Adressekonto = fixture.Create<int>()
                              };
            Assert.Throws<IntranetRepositoryException>(() => commandHandler.Execute(command));
        }

        /// <summary>
        /// Tester, at Execute kaster en IntranetBusinessException, hvis tekst ikke er angivet.
        /// </summary>
        [Test]
        public void TestAtExecuteKasterIntranetBusinessExceptionHvisTekstIkkeErAngivet()
        {
            var fixture = new Fixture();

            var regnskab = fixture.Create<Regnskab>();
            foreach (var konto in fixture.CreateMany<Konto>(25))
            {
                regnskab.TilføjKonto(konto);
            }
            var finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepository.Expect(
                m =>
                m.RegnskabGet(Arg<int>.Is.Anything, Arg<Func<int, Brevhoved>>.Is.NotNull,
                              Arg<Func<int, AdresseBase>>.Is.NotNull))
                .Return(regnskab);
            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepository.Expect(m => m.AdresseGetAll())
                .Return(fixture.CreateMany<Person>(25));
            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepository.Expect(m => m.BrevhovedGetAll())
                .Return(fixture.CreateMany<Brevhoved>(3));
            var konfigurationRepository = MockRepository.GenerateMock<IKonfigurationRepository>();
            konfigurationRepository.Expect(m => m.DageForBogføringsperiode)
                .Return(30);
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();

            fixture.Inject(finansstyringRepository);
            fixture.Inject(adresseRepository);
            fixture.Inject(fællesRepository);
            fixture.Inject(konfigurationRepository);
            fixture.Inject(objectMapper);

            var commandHandler = fixture.Create<BogføringslinjeOpretCommandHandler>();
            Assert.That(commandHandler, Is.Not.Null);

            fixture.Inject(DateTime.Now);
            var command = new BogføringslinjeOpretCommand
                              {
                                  Regnskabsnummer = regnskab.Nummer,
                                  Dato = fixture.Create<DateTime>(),
                                  Bilag = fixture.Create<string>(),
                                  Kontonummer = regnskab.Konti.OfType<Konto>().ElementAt(1).Kontonummer,
                                  Tekst = null,
                                  Budgetkontonummer = fixture.Create<string>(),
                                  Debit = fixture.Create<decimal>(),
                                  Kredit = fixture.Create<decimal>(),
                                  Adressekonto = fixture.Create<int>()
                              };
            Assert.Throws<IntranetBusinessException>(() => commandHandler.Execute(command));
        }

        /// <summary>
        /// Tester, at Execute kaster en IntranetRepositoryException, hvis budgetkonto ikke findes.
        /// </summary>
        [Test]
        public void TestAtExecuteKasterIntranetRepositoryExceptionHvisBudgetkontoIkkeFindes()
        {
            var fixture = new Fixture();

            var regnskab = fixture.Create<Regnskab>();
            foreach (var konto in fixture.CreateMany<Konto>(25))
            {
                regnskab.TilføjKonto(konto);
            }
            foreach (var budgetkonto in fixture.CreateMany<Budgetkonto>(25))
            {
                regnskab.TilføjKonto(budgetkonto);
            }
            var finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepository.Expect(
                m =>
                m.RegnskabGet(Arg<int>.Is.Anything, Arg<Func<int, Brevhoved>>.Is.NotNull,
                              Arg<Func<int, AdresseBase>>.Is.NotNull))
                .Return(regnskab);
            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepository.Expect(m => m.AdresseGetAll())
                .Return(fixture.CreateMany<Person>(25));
            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepository.Expect(m => m.BrevhovedGetAll())
                .Return(fixture.CreateMany<Brevhoved>(3));
            var konfigurationRepository = MockRepository.GenerateMock<IKonfigurationRepository>();
            konfigurationRepository.Expect(m => m.DageForBogføringsperiode)
                .Return(30);
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();

            fixture.Inject(finansstyringRepository);
            fixture.Inject(adresseRepository);
            fixture.Inject(fællesRepository);
            fixture.Inject(konfigurationRepository);
            fixture.Inject(objectMapper);

            var commandHandler = fixture.Create<BogføringslinjeOpretCommandHandler>();
            Assert.That(commandHandler, Is.Not.Null);

            fixture.Inject(DateTime.Now);
            var command = new BogføringslinjeOpretCommand
                              {
                                  Regnskabsnummer = regnskab.Nummer,
                                  Dato = fixture.Create<DateTime>(),
                                  Bilag = fixture.Create<string>(),
                                  Kontonummer = regnskab.Konti.OfType<Konto>().ElementAt(1).Kontonummer,
                                  Tekst = fixture.Create<string>(),
                                  Budgetkontonummer = fixture.Create<string>(),
                                  Debit = fixture.Create<decimal>(),
                                  Kredit = fixture.Create<decimal>(),
                                  Adressekonto = fixture.Create<int>()
                              };
            Assert.Throws<IntranetRepositoryException>(() => commandHandler.Execute(command));
        }

        /// <summary>
        /// Tester, at Execute kaster en IntranetBusinessException, hvis debitbeløb er under 0.
        /// </summary>
        [Test]
        public void TestAtExecuteKasterIntranetBusinessExceptionHvisDebitBeløbErUnderNul()
        {
            var fixture = new Fixture();

            var regnskab = fixture.Create<Regnskab>();
            foreach (var konto in fixture.CreateMany<Konto>(25))
            {
                regnskab.TilføjKonto(konto);
            }
            foreach (var budgetkonto in fixture.CreateMany<Budgetkonto>(25))
            {
                regnskab.TilføjKonto(budgetkonto);
            }
            var finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepository.Expect(
                m =>
                m.RegnskabGet(Arg<int>.Is.Anything, Arg<Func<int, Brevhoved>>.Is.NotNull,
                              Arg<Func<int, AdresseBase>>.Is.NotNull))
                .Return(regnskab);
            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepository.Expect(m => m.AdresseGetAll())
                .Return(fixture.CreateMany<Person>(25));
            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepository.Expect(m => m.BrevhovedGetAll())
                .Return(fixture.CreateMany<Brevhoved>(3));
            var konfigurationRepository = MockRepository.GenerateMock<IKonfigurationRepository>();
            konfigurationRepository.Expect(m => m.DageForBogføringsperiode)
                .Return(30);
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();

            fixture.Inject(finansstyringRepository);
            fixture.Inject(adresseRepository);
            fixture.Inject(fællesRepository);
            fixture.Inject(konfigurationRepository);
            fixture.Inject(objectMapper);

            var commandHandler = fixture.Create<BogføringslinjeOpretCommandHandler>();
            Assert.That(commandHandler, Is.Not.Null);

            fixture.Inject(DateTime.Now);
            var command = new BogføringslinjeOpretCommand
                              {
                                  Regnskabsnummer = regnskab.Nummer,
                                  Dato = fixture.Create<DateTime>(),
                                  Bilag = fixture.Create<string>(),
                                  Kontonummer = regnskab.Konti.OfType<Konto>().ElementAt(1).Kontonummer,
                                  Tekst = fixture.Create<string>(),
                                  Budgetkontonummer = regnskab.Konti.OfType<Budgetkonto>().ElementAt(1).Kontonummer,
                                  Debit = fixture.Create<decimal>()*-1,
                                  Kredit = fixture.Create<decimal>(),
                                  Adressekonto = fixture.Create<int>()
                              };
            Assert.Throws<IntranetBusinessException>(() => commandHandler.Execute(command));
        }

        /// <summary>
        /// Tester, at Execute kaster en IntranetBusinessException, hvis kreditbeløb er under 0.
        /// </summary>
        [Test]
        public void TestAtExecuteKasterIntranetBusinessExceptionHvisKreditBeløbErUnderNul()
        {
            var fixture = new Fixture();

            var regnskab = fixture.Create<Regnskab>();
            foreach (var konto in fixture.CreateMany<Konto>(25))
            {
                regnskab.TilføjKonto(konto);
            }
            foreach (var budgetkonto in fixture.CreateMany<Budgetkonto>(25))
            {
                regnskab.TilføjKonto(budgetkonto);
            }
            var finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepository.Expect(
                m =>
                m.RegnskabGet(Arg<int>.Is.Anything, Arg<Func<int, Brevhoved>>.Is.NotNull,
                              Arg<Func<int, AdresseBase>>.Is.NotNull))
                .Return(regnskab);
            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepository.Expect(m => m.AdresseGetAll())
                .Return(fixture.CreateMany<Person>(25));
            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepository.Expect(m => m.BrevhovedGetAll())
                .Return(fixture.CreateMany<Brevhoved>(3));
            var konfigurationRepository = MockRepository.GenerateMock<IKonfigurationRepository>();
            konfigurationRepository.Expect(m => m.DageForBogføringsperiode)
                .Return(30);
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();

            fixture.Inject(finansstyringRepository);
            fixture.Inject(adresseRepository);
            fixture.Inject(fællesRepository);
            fixture.Inject(konfigurationRepository);
            fixture.Inject(objectMapper);

            var commandHandler = fixture.Create<BogføringslinjeOpretCommandHandler>();
            Assert.That(commandHandler, Is.Not.Null);

            fixture.Inject(DateTime.Now);
            var command = new BogføringslinjeOpretCommand
                              {
                                  Regnskabsnummer = regnskab.Nummer,
                                  Dato = fixture.Create<DateTime>(),
                                  Bilag = fixture.Create<string>(),
                                  Kontonummer = regnskab.Konti.OfType<Konto>().ElementAt(1).Kontonummer,
                                  Tekst = fixture.Create<string>(),
                                  Budgetkontonummer = regnskab.Konti.OfType<Budgetkonto>().ElementAt(1).Kontonummer,
                                  Debit = fixture.Create<decimal>(),
                                  Kredit = fixture.Create<decimal>()*-1,
                                  Adressekonto = fixture.Create<int>()
                              };
            Assert.Throws<IntranetBusinessException>(() => commandHandler.Execute(command));
        }

        /// <summary>
        /// Tester, at Execute kaster en IntranetBusinessException, hvis debitbeløb og kreditbeløb er 0.
        /// </summary>
        [Test]
        public void TestAtExecuteKasterIntranetBusinessExceptionHvisDebitBeløbOgKreditBeløbErNul()
        {
            var fixture = new Fixture();

            var regnskab = fixture.Create<Regnskab>();
            foreach (var konto in fixture.CreateMany<Konto>(25))
            {
                regnskab.TilføjKonto(konto);
            }
            foreach (var budgetkonto in fixture.CreateMany<Budgetkonto>(25))
            {
                regnskab.TilføjKonto(budgetkonto);
            }
            var finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepository.Expect(
                m =>
                m.RegnskabGet(Arg<int>.Is.Anything, Arg<Func<int, Brevhoved>>.Is.NotNull,
                              Arg<Func<int, AdresseBase>>.Is.NotNull))
                .Return(regnskab);
            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepository.Expect(m => m.AdresseGetAll())
                .Return(fixture.CreateMany<Person>(25));
            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepository.Expect(m => m.BrevhovedGetAll())
                .Return(fixture.CreateMany<Brevhoved>(3));
            var konfigurationRepository = MockRepository.GenerateMock<IKonfigurationRepository>();
            konfigurationRepository.Expect(m => m.DageForBogføringsperiode)
                .Return(30);
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();

            fixture.Inject(finansstyringRepository);
            fixture.Inject(adresseRepository);
            fixture.Inject(fællesRepository);
            fixture.Inject(konfigurationRepository);
            fixture.Inject(objectMapper);

            var commandHandler = fixture.Create<BogføringslinjeOpretCommandHandler>();
            Assert.That(commandHandler, Is.Not.Null);

            fixture.Inject(DateTime.Now);
            fixture.Inject(0M);
            var command = new BogføringslinjeOpretCommand
                              {
                                  Regnskabsnummer = regnskab.Nummer,
                                  Dato = fixture.Create<DateTime>(),
                                  Bilag = fixture.Create<string>(),
                                  Kontonummer = regnskab.Konti.OfType<Konto>().ElementAt(1).Kontonummer,
                                  Tekst = fixture.Create<string>(),
                                  Budgetkontonummer = regnskab.Konti.OfType<Budgetkonto>().ElementAt(1).Kontonummer,
                                  Debit = fixture.Create<decimal>(),
                                  Kredit = fixture.Create<decimal>(),
                                  Adressekonto = fixture.Create<int>()
                              };
            Assert.Throws<IntranetBusinessException>(() => commandHandler.Execute(command));
        }

        /// <summary>
        /// Tester, at Execute kaster en IntranetRepositoryException, hvis adressen ikke findes.
        /// </summary>
        [Test]
        public void TestAtExecuteKasterIntranetRepositoryExceptionHvisAdresseIkkeFindes()
        {
            var fixture = new Fixture();

            var regnskab = fixture.Create<Regnskab>();
            foreach (var konto in fixture.CreateMany<Konto>(25))
            {
                regnskab.TilføjKonto(konto);
            }
            foreach (var budgetkonto in fixture.CreateMany<Budgetkonto>(25))
            {
                regnskab.TilføjKonto(budgetkonto);
            }
            var finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepository.Expect(
                m =>
                m.RegnskabGet(Arg<int>.Is.Anything, Arg<Func<int, Brevhoved>>.Is.NotNull,
                              Arg<Func<int, AdresseBase>>.Is.NotNull))
                .Return(regnskab);
            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            var adressekonti = fixture.CreateMany<Person>(25).ToList();
            adresseRepository.Expect(m => m.AdresseGetAll())
                .Return(adressekonti);
            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepository.Expect(m => m.BrevhovedGetAll())
                .Return(fixture.CreateMany<Brevhoved>(3));
            var konfigurationRepository = MockRepository.GenerateMock<IKonfigurationRepository>();
            konfigurationRepository.Expect(m => m.DageForBogføringsperiode)
                .Return(30);
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();

            fixture.Inject(finansstyringRepository);
            fixture.Inject(adresseRepository);
            fixture.Inject(fællesRepository);
            fixture.Inject(konfigurationRepository);
            fixture.Inject(objectMapper);

            var commandHandler = fixture.Create<BogføringslinjeOpretCommandHandler>();
            Assert.That(commandHandler, Is.Not.Null);

            fixture.Inject(DateTime.Now);
            fixture.Inject(250);
            var command = new BogføringslinjeOpretCommand
                              {
                                  Regnskabsnummer = regnskab.Nummer,
                                  Dato = fixture.Create<DateTime>(),
                                  Bilag = fixture.Create<string>(),
                                  Kontonummer = regnskab.Konti.OfType<Konto>().ElementAt(1).Kontonummer,
                                  Tekst = fixture.Create<string>(),
                                  Budgetkontonummer = regnskab.Konti.OfType<Budgetkonto>().ElementAt(1).Kontonummer,
                                  Debit = fixture.Create<decimal>(),
                                  Kredit = fixture.Create<decimal>(),
                                  Adressekonto = fixture.Create<int>()
                              };
            Assert.Throws<IntranetRepositoryException>(() => commandHandler.Execute(command));
        }

        /// <summary>
        /// Tester, at HandleException kaster IntranetSystemException.
        /// </summary>
        [Test]
        public void TestAtHandleExceptionKasterIntranetSystemException()
        {
            var fixture = new Fixture();

            var finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            var konfigurationRepository = MockRepository.GenerateMock<IKonfigurationRepository>();
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();

            fixture.Inject(finansstyringRepository);
            fixture.Inject(adresseRepository);
            fixture.Inject(fællesRepository);
            fixture.Inject(konfigurationRepository);
            fixture.Inject(objectMapper);

            var commandHandler = fixture.Create<BogføringslinjeOpretCommandHandler>();
            Assert.That(commandHandler, Is.Not.Null);

            Assert.Throws<IntranetSystemException>(
                () =>
                commandHandler.HandleException(fixture.Create<BogføringslinjeOpretCommand>(),
                                               fixture.Create<Exception>()));
        }
    }
}
