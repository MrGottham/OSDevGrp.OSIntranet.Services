using System;
using System.Linq;
using System.Reflection;
using OSDevGrp.OSIntranet.CommandHandlers;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.Contracts.Commands;
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

            Assert.That(
                Assert.Throws<TargetInvocationException>(
                    () => fixture.CreateAnonymous<BogføringslinjeOpretCommandHandler>()).InnerException,
                Is.TypeOf(typeof (ArgumentNullException)));
        }

        /// <summary>
        /// Tester, at Execute tilføjer en bogføringslinje.
        /// </summary>
        [Test]
        public void TestAtExecuteTilføjerBogføringslinje()
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

            var commandHandler = fixture.CreateAnonymous<BogføringslinjeOpretCommandHandler>();
            Assert.That(commandHandler, Is.Not.Null);

            var command = new BogføringslinjeOpretCommand
                              {
                                  Regnskabsnummer = 1,
                                  Dato = DateTime.Now,
                                  Kontonummer = "DANKORT",
                                  Tekst = "Løn",
                                  Budgetkontonummer = "1000",
                                  Debit = 15000M,
                                  Kredit = 0M,
                                  Adressekonto = 1
                              };
            var result = commandHandler.Execute(command);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Advarsler, Is.Not.Null);
            finansstyringRepository.AssertWasCalled(
                m =>
                m.BogføringslinjeAdd(Arg<DateTime>.Is.Equal(command.Dato),
                                     Arg<string>.Is.Equal(command.Bilag),
                                     Arg<Konto>.Is.NotNull,
                                     Arg<string>.Is.Equal(command.Tekst),
                                     Arg<Budgetkonto>.Is.NotNull,
                                     Arg<decimal>.Is.Equal(command.Debit),
                                     Arg<decimal>.Is.Equal(command.Kredit),
                                     Arg<AdresseBase>.Is.NotNull));
        }

        /// <summary>
        /// Tester, at Execute returnerer advarsel ved overtrækkelse af konto.
        /// </summary>
        [Test]
        public void TestAtExecuteReturnerAdvarselVedOvertrækkelseAfKonto()
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

            var commandHandler = fixture.CreateAnonymous<BogføringslinjeOpretCommandHandler>();
            Assert.That(commandHandler, Is.Not.Null);

            var command = new BogføringslinjeOpretCommand
                              {
                                  Regnskabsnummer = 1,
                                  Dato = DateTime.Now,
                                  Kontonummer = "DANKORT",
                                  Tekst = "Udbetaling",
                                  Debit = 0M,
                                  Kredit = 5000M,
                              };
            var result = commandHandler.Execute(command);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Advarsler, Is.Not.Null);
            Assert.That(result.Advarsler.Count(), Is.EqualTo(1));

            var e = result.Advarsler.GetEnumerator();
            Assert.That(e, Is.Not.Null);
            Assert.That(e.MoveNext(), Is.True);
            Assert.That(e.Current.Advarsel, Is.Not.Null);
            Assert.That(e.Current.Advarsel.Length, Is.GreaterThan(0));
            Assert.That(e.Current.Konto, Is.Not.Null);
            Assert.That(e.Current.Beløb, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at Execute returnerer advarsel ved overtrækkelse af budgetkonto.
        /// </summary>
        [Test]
        public void TestAtExecuteReturnerAdvarselVedOvertrækkelseAfBudgetkonto()
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

            var commandHandler = fixture.CreateAnonymous<BogføringslinjeOpretCommandHandler>();
            Assert.That(commandHandler, Is.Not.Null);

            var command = new BogføringslinjeOpretCommand
                              {
                                  Regnskabsnummer = 1,
                                  Dato = DateTime.Now,
                                  Kontonummer = "DANKORT",
                                  Tekst = "Indkøb",
                                  Budgetkontonummer = "2000",
                                  Debit = 0M,
                                  Kredit = 495M,
                              };
            var result = commandHandler.Execute(command);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Advarsler, Is.Not.Null);
            Assert.That(result.Advarsler.Count(), Is.EqualTo(1));

            var e = result.Advarsler.GetEnumerator();
            Assert.That(e, Is.Not.Null);
            Assert.That(e.MoveNext(), Is.True);
            Assert.That(e.Current.Advarsel, Is.Not.Null);
            Assert.That(e.Current.Advarsel.Length, Is.GreaterThan(0));
            Assert.That(e.Current.Konto, Is.Not.Null);
            Assert.That(e.Current.Beløb, Is.GreaterThan(0));
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

            var commandHandler = fixture.CreateAnonymous<BogføringslinjeOpretCommandHandler>();
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
            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            var konfigurationRepository = MockRepository.GenerateMock<IKonfigurationRepository>();
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();

            fixture.Inject(finansstyringRepository);
            fixture.Inject(adresseRepository);
            fixture.Inject(fællesRepository);
            fixture.Inject(konfigurationRepository);
            fixture.Inject(objectMapper);

            var commandHandler = fixture.CreateAnonymous<BogføringslinjeOpretCommandHandler>();
            Assert.That(commandHandler, Is.Not.Null);

            var command = new BogføringslinjeOpretCommand
                              {
                                  Regnskabsnummer = -1,
                                  Dato = new DateTime(2011, 4, 1),
                                  Bilag = "XYZ",
                                  Kontonummer = "DANKORT",
                                  Tekst = "Test",
                                  Budgetkontonummer = "1000",
                                  Debit = 1000M,
                                  Kredit = 0M,
                                  Adressekonto = 1
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

            var commandHandler = fixture.CreateAnonymous<BogføringslinjeOpretCommandHandler>();
            Assert.That(commandHandler, Is.Not.Null);

            var command = new BogføringslinjeOpretCommand
                              {
                                  Regnskabsnummer = 1,
                                  Dato = DateTime.Now.AddDays(-31),
                                  Bilag = "XYZ",
                                  Kontonummer = "DANKORT",
                                  Tekst = "Test",
                                  Budgetkontonummer = "1000",
                                  Debit = 1000M,
                                  Kredit = 0M,
                                  Adressekonto = 1
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

            var commandHandler = fixture.CreateAnonymous<BogføringslinjeOpretCommandHandler>();
            Assert.That(commandHandler, Is.Not.Null);

            var command = new BogføringslinjeOpretCommand
                              {
                                  Regnskabsnummer = 1,
                                  Dato = DateTime.Now.AddDays(1),
                                  Bilag = "XYZ",
                                  Kontonummer = "DANKORT",
                                  Tekst = "Test",
                                  Budgetkontonummer = "1000",
                                  Debit = 1000M,
                                  Kredit = 0M,
                                  Adressekonto = 1
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

            var commandHandler = fixture.CreateAnonymous<BogføringslinjeOpretCommandHandler>();
            Assert.That(commandHandler, Is.Not.Null);

            var command = new BogføringslinjeOpretCommand
                              {
                                  Regnskabsnummer = 1,
                                  Dato = DateTime.Now,
                                  Bilag = "XYZ",
                                  Kontonummer = null,
                                  Tekst = "Test",
                                  Budgetkontonummer = "1000",
                                  Debit = 1000M,
                                  Kredit = 0M,
                                  Adressekonto = 1
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

            var commandHandler = fixture.CreateAnonymous<BogføringslinjeOpretCommandHandler>();
            Assert.That(commandHandler, Is.Not.Null);

            var command = new BogføringslinjeOpretCommand
                              {
                                  Regnskabsnummer = 1,
                                  Dato = DateTime.Now,
                                  Bilag = "XYZ",
                                  Kontonummer = "UNKNOWN",
                                  Tekst = "Test",
                                  Budgetkontonummer = "1000",
                                  Debit = 1000M,
                                  Kredit = 0M,
                                  Adressekonto = 1
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

            var commandHandler = fixture.CreateAnonymous<BogføringslinjeOpretCommandHandler>();
            Assert.That(commandHandler, Is.Not.Null);

            var command = new BogføringslinjeOpretCommand
                              {
                                  Regnskabsnummer = 1,
                                  Dato = DateTime.Now,
                                  Bilag = "XYZ",
                                  Kontonummer = "DANKORT",
                                  Tekst = null,
                                  Budgetkontonummer = "1000",
                                  Debit = 1000M,
                                  Kredit = 0M,
                                  Adressekonto = 1
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

            var commandHandler = fixture.CreateAnonymous<BogføringslinjeOpretCommandHandler>();
            Assert.That(commandHandler, Is.Not.Null);

            var command = new BogføringslinjeOpretCommand
                              {
                                  Regnskabsnummer = 1,
                                  Dato = DateTime.Now,
                                  Bilag = "XYZ",
                                  Kontonummer = "DANKORT",
                                  Tekst = "Test",
                                  Budgetkontonummer = "UNKNOWN",
                                  Debit = 1000M,
                                  Kredit = 0M,
                                  Adressekonto = 1
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

            var commandHandler = fixture.CreateAnonymous<BogføringslinjeOpretCommandHandler>();
            Assert.That(commandHandler, Is.Not.Null);

            var command = new BogføringslinjeOpretCommand
                              {
                                  Regnskabsnummer = 1,
                                  Dato = DateTime.Now,
                                  Bilag = "XYZ",
                                  Kontonummer = "DANKORT",
                                  Tekst = "Test",
                                  Budgetkontonummer = "1000",
                                  Debit = -1000M,
                                  Kredit = 0M,
                                  Adressekonto = 1
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

            var commandHandler = fixture.CreateAnonymous<BogføringslinjeOpretCommandHandler>();
            Assert.That(commandHandler, Is.Not.Null);

            var command = new BogføringslinjeOpretCommand
                              {
                                  Regnskabsnummer = 1,
                                  Dato = DateTime.Now,
                                  Bilag = "XYZ",
                                  Kontonummer = "DANKORT",
                                  Tekst = "Test",
                                  Budgetkontonummer = "1000",
                                  Debit = 0M,
                                  Kredit = -1000M,
                                  Adressekonto = 1
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

            var commandHandler = fixture.CreateAnonymous<BogføringslinjeOpretCommandHandler>();
            Assert.That(commandHandler, Is.Not.Null);

            var command = new BogføringslinjeOpretCommand
                              {
                                  Regnskabsnummer = 1,
                                  Dato = DateTime.Now,
                                  Bilag = "XYZ",
                                  Kontonummer = "DANKORT",
                                  Tekst = "Test",
                                  Budgetkontonummer = "1000",
                                  Debit = 0M,
                                  Kredit = 0M,
                                  Adressekonto = 1
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

            var commandHandler = fixture.CreateAnonymous<BogføringslinjeOpretCommandHandler>();
            Assert.That(commandHandler, Is.Not.Null);

            var command = new BogføringslinjeOpretCommand
                              {
                                  Regnskabsnummer = 1,
                                  Dato = DateTime.Now,
                                  Bilag = "XYZ",
                                  Kontonummer = "DANKORT",
                                  Tekst = "Test",
                                  Budgetkontonummer = "1000",
                                  Debit = 1000M,
                                  Kredit = 0M,
                                  Adressekonto = -1
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

            var commandHandler = fixture.CreateAnonymous<BogføringslinjeOpretCommandHandler>();
            Assert.That(commandHandler, Is.Not.Null);

            Assert.Throws<IntranetSystemException>(() => commandHandler.HandleException(new BogføringslinjeOpretCommand(), new Exception("Test")));
        }
    }
}
