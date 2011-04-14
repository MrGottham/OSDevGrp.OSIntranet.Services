using System;
using System.Linq;
using OSDevGrp.OSIntranet.CommandHandlers;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.Contracts.Commands;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers;
using NUnit.Framework;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.CommandHandlers
{
    /// <summary>
    /// Tester CommandHandler til håndtering af kommandoen: BogføringslinjeOpretCommand.
    /// </summary>
    [TestFixture]
    public class BogføringslinjeOpretCommandHandlerTests : FinansstyringQueryHandlerTestsBase
    {
        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis repository for finansstyring er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisFinansstyringRepositoryErNull()
        {
            Assert.Throws<ArgumentNullException>(() => new BogføringslinjeOpretCommandHandler(null, null, null));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis repository for adressekartoteket er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisAdresseRepositoryErNull()
        {
            Assert.Throws<ArgumentNullException>(() => new BogføringslinjeOpretCommandHandler(GetFinansstyringRepository(), null, null));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis konfigurationsrepository er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisKonfigurationRepositoryErNull()
        {
            Assert.Throws<ArgumentNullException>(() => new BogføringslinjeOpretCommandHandler(GetFinansstyringRepository(), GetAdresseRepository(), null));
        }

        /// <summary>
        /// Tester, at Execute tilføjer en bogføringslinje.
        /// </summary>
        [Test]
        public void TestAtExecuteTilføjerBogføringslinje()
        {
            var finansstyringRepository = GetFinansstyringRepository();
            var konfigurationRepository = MockRepository.GenerateMock<IKonfigurationRepository>();
            konfigurationRepository.Expect(m => m.DageForBogføringsperiode).Return(30);
            var commandHandler = new BogføringslinjeOpretCommandHandler(finansstyringRepository, GetAdresseRepository(),
                                                                        konfigurationRepository);
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
            var finansstyringRepository = GetFinansstyringRepository();
            var konfigurationRepository = MockRepository.GenerateMock<IKonfigurationRepository>();
            konfigurationRepository.Expect(m => m.DageForBogføringsperiode).Return(30);
            var commandHandler = new BogføringslinjeOpretCommandHandler(finansstyringRepository, GetAdresseRepository(),
                                                                        konfigurationRepository);
            var command = new BogføringslinjeOpretCommand
                              {
                                  Regnskabsnummer = 1,
                                  Dato = DateTime.Now,
                                  Kontonummer = "DANKORT",
                                  Tekst = "Udbetaling",
                                  Debit = 0M,
                                  Kredit = 25000M,
                              };
            var result = commandHandler.Execute(command);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Advarsler, Is.Not.Null);
            Assert.That(result.Advarsler.Count(), Is.EqualTo(1));
        }

        /// <summary>
        /// Tester, at Execute kaster en ArgumentNullException, hvis kommandoen er null.
        /// </summary>
        [Test]
        public void TestAtExecuteKasterArgumentNullExceptionHvisCommandErNull()
        {
            var konfigurationRepository = MockRepository.GenerateMock<IKonfigurationRepository>();
            var commandHandler = new BogføringslinjeOpretCommandHandler(GetFinansstyringRepository(), GetAdresseRepository(), konfigurationRepository);
            Assert.Throws<ArgumentNullException>(() => commandHandler.Execute(null));
        }

        /// <summary>
        /// Tester, at Execute kaster en IntranetRepositoryException, hvis regnskabet ikke findes.
        /// </summary>
        [Test]
        public void TestAtExecuteKasterIntranetRepositoryExceptionHvisRegnskabIkkeFindes()
        {
            var konfigurationRepository = MockRepository.GenerateMock<IKonfigurationRepository>();
            var commandHandler = new BogføringslinjeOpretCommandHandler(GetFinansstyringRepository(), GetAdresseRepository(), konfigurationRepository);
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
            var konfigurationRepository = MockRepository.GenerateMock<IKonfigurationRepository>();
            konfigurationRepository.Expect(m => m.DageForBogføringsperiode).Return(30);
            var commandHandler = new BogføringslinjeOpretCommandHandler(GetFinansstyringRepository(), GetAdresseRepository(), konfigurationRepository);
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
            var konfigurationRepository = MockRepository.GenerateMock<IKonfigurationRepository>();
            konfigurationRepository.Expect(m => m.DageForBogføringsperiode).Return(30);
            var commandHandler = new BogføringslinjeOpretCommandHandler(GetFinansstyringRepository(), GetAdresseRepository(), konfigurationRepository);
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
            var konfigurationRepository = MockRepository.GenerateMock<IKonfigurationRepository>();
            konfigurationRepository.Expect(m => m.DageForBogføringsperiode).Return(30);
            var commandHandler = new BogføringslinjeOpretCommandHandler(GetFinansstyringRepository(), GetAdresseRepository(), konfigurationRepository);
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
            var konfigurationRepository = MockRepository.GenerateMock<IKonfigurationRepository>();
            konfigurationRepository.Expect(m => m.DageForBogføringsperiode).Return(30);
            var commandHandler = new BogføringslinjeOpretCommandHandler(GetFinansstyringRepository(), GetAdresseRepository(), konfigurationRepository);
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
            var konfigurationRepository = MockRepository.GenerateMock<IKonfigurationRepository>();
            konfigurationRepository.Expect(m => m.DageForBogføringsperiode).Return(30);
            var commandHandler = new BogføringslinjeOpretCommandHandler(GetFinansstyringRepository(), GetAdresseRepository(), konfigurationRepository);
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
            var konfigurationRepository = MockRepository.GenerateMock<IKonfigurationRepository>();
            konfigurationRepository.Expect(m => m.DageForBogføringsperiode).Return(30);
            var commandHandler = new BogføringslinjeOpretCommandHandler(GetFinansstyringRepository(), GetAdresseRepository(), konfigurationRepository);
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
            var konfigurationRepository = MockRepository.GenerateMock<IKonfigurationRepository>();
            konfigurationRepository.Expect(m => m.DageForBogføringsperiode).Return(30);
            var commandHandler = new BogføringslinjeOpretCommandHandler(GetFinansstyringRepository(), GetAdresseRepository(), konfigurationRepository);
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
            var konfigurationRepository = MockRepository.GenerateMock<IKonfigurationRepository>();
            konfigurationRepository.Expect(m => m.DageForBogføringsperiode).Return(30);
            var commandHandler = new BogføringslinjeOpretCommandHandler(GetFinansstyringRepository(), GetAdresseRepository(), konfigurationRepository);
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
            var konfigurationRepository = MockRepository.GenerateMock<IKonfigurationRepository>();
            konfigurationRepository.Expect(m => m.DageForBogføringsperiode).Return(30);
            var commandHandler = new BogføringslinjeOpretCommandHandler(GetFinansstyringRepository(), GetAdresseRepository(), konfigurationRepository);
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
            var konfigurationRepository = MockRepository.GenerateMock<IKonfigurationRepository>();
            konfigurationRepository.Expect(m => m.DageForBogføringsperiode).Return(30);
            var commandHandler = new BogføringslinjeOpretCommandHandler(GetFinansstyringRepository(), GetAdresseRepository(), konfigurationRepository);
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
            var konfigurationRepository = MockRepository.GenerateMock<IKonfigurationRepository>();
            var commandHandler = new BogføringslinjeOpretCommandHandler(GetFinansstyringRepository(), GetAdresseRepository(), konfigurationRepository);
            Assert.Throws<IntranetSystemException>(() => commandHandler.HandleException(new BogføringslinjeOpretCommand(), new Exception("Test")));
        }
    }
}
