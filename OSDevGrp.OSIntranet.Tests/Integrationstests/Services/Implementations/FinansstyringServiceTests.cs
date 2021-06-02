using System;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.IoC;
using OSDevGrp.OSIntranet.Contracts.Commands;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Services;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Integrationstests.Services.Implementations
{
    /// <summary>
    /// Integrationstester servicen til finansstyring.
    /// </summary>
    [TestFixture]
    [Category("Integrationstest")]
    public class FinansstyringServiceTests
    {
        #region Private variables

        private IFinansstyringService _service;

        #endregion

        /// <summary>
        /// Opsætning af test.
        /// </summary>
        [SetUp]
        public void TestSetUp()
        {
            var container = ContainerFactory.Create();
            _service = container.Resolve<IFinansstyringService>();
        }

        /// <summary>
        /// Tester, at en kontoplan kan hentes.
        /// </summary>
        [Test]
        public void TestAtKontoplanHentes()
        {
            var query = new KontoplanGetQuery
                            {
                                Regnskabsnummer = 1,
                                StatusDato = new DateTime(2011, 3, 1)
                            };
            var result = _service.KontoplanGet(query);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at en konto kan hentes.
        /// </summary>
        [Test]
        public void TestAtKontoHentes()
        {
            var query = new KontoGetQuery
                            {
                                Regnskabsnummer = 1,
                                Kontonummer = "DANKORT",
                                StatusDato = new DateTime(2011, 3, 1)
                            };
            var result = _service.KontoGet(query);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Regnskab, Is.Not.Null);
            Assert.That(result.Regnskab.Nummer, Is.EqualTo(1));
            Assert.That(result.Kontonummer, Is.Not.Null);
            Assert.That(result.Kontonummer, Is.EqualTo("DANKORT"));
        }

        /// <summary>
        /// Tester, at en budgetkontoplan kan hentes.
        /// </summary>
        [Test]
        public void TestAtBudgetkontoplanHentes()
        {
            var query = new BudgetkontoplanGetQuery
                            {
                                Regnskabsnummer = 1,
                                StatusDato = new DateTime(2011, 3, 1)
                            };
            var result = _service.BudgetkontoplanGet(query);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at en budgetkonto kan hentes.
        /// </summary>
        [Test]
        public void TestAtBudgetkontoHentes()
        {
            var query = new BudgetkontoGetQuery
                            {
                                Regnskabsnummer = 1,
                                Kontonummer = "1000",
                                StatusDato = new DateTime(2011, 3, 1)
                            };
            var result = _service.BudgetkontoGet(query);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Regnskab, Is.Not.Null);
            Assert.That(result.Regnskab.Nummer, Is.EqualTo(1));
            Assert.That(result.Kontonummer, Is.Not.Null);
            Assert.That(result.Kontonummer, Is.EqualTo("1000"));
        }

        /// <summary>
        /// Tester, at en debitorliste kan hentes.
        /// </summary>
        [Test]
        public void TestAtDebitorlisteHentes()
        {
            var query = new DebitorlisteGetQuery
                            {
                                Regnskabsnummer = 1,
                                StatusDato = new DateTime(2011, 1, 16)
                            };
            var result = _service.DebitorlisteGet(query);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at en debitor kan hentes.
        /// </summary>
        [Test]
        public void TestAtDebitorHentes()
        {
            var query = new DebitorGetQuery
                            {
                                Regnskabsnummer = 1,
                                Nummer = 205,
                                StatusDato = new DateTime(2011, 1, 16)
                            };
            var result = _service.DebitorGet(query);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Nummer, Is.EqualTo(205));
            Assert.That(result.Saldo, Is.LessThan(0));
        }

        /// <summary>
        /// Tester, at en kreditorliste kan hentes.
        /// </summary>
        [Test]
        public void TestAtKreditorlisteKanHentes()
        {
            var query = new KreditorlisteGetQuery
                            {
                                Regnskabsnummer = 1,
                                StatusDato = new DateTime(2010, 8, 10)
                            };
            var result = _service.KreditorlisteGet(query);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at en kreditor kan hentes.
        /// </summary>
        [Test]
        public void TestAtKreditorHentes()
        {
            var query = new KreditorGetQuery
                            {
                                Regnskabsnummer = 1,
                                Nummer = 205,
                                StatusDato = new DateTime(2010, 8, 10)
                            };
            var result = _service.KreditorGet(query);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Nummer, Is.EqualTo(205));
            Assert.That(result.Saldo, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at en liste af adressekonti kan hentes.
        /// </summary>
        [Test]
        public void TestAtAdressekontolisteKanHentes()
        {
            var query = new AdressekontolisteGetQuery
                            {
                                Regnskabsnummer = 1,
                                StatusDato = new DateTime(2011, 3, 1)
                            };
            var result = _service.AdressekontolisteGet(query);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at en adressekonto kan hentes.
        /// </summary>
        [Test]
        public void TestAtAdressekontoKanHentes()
        {
            var query = new AdressekontoGetQuery
                            {
                                Regnskabsnummer = 1,
                                Nummer = 1,
                                StatusDato = new DateTime(2011, 3, 1)
                            };
            var result = _service.AdressekontoGet(query);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Nummer, Is.EqualTo(1));
        }

        /// <summary>
        /// Tester, at antal bogføringslinjer kan hentes.
        /// </summary>
        [Test]
        public void TestAtBogføringslinjerKanHentes()
        {
            var query = new BogføringerGetQuery
                            {
                                Regnskabsnummer = 1,
                                StatusDato = new DateTime(2011, 3, 1),
                                Linjer = 250
                            };
            var result = _service.BogføringerGet(query);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(250));
        }

        /// <summary>
        /// Tester, at bogføringslinjer kan oprettes.
        /// </summary>
        [Test]
        [Ignore("Oprettelse af bogføringslinjer er testet.")]
        public void TestAtBogføringslinjerKanOprettes()
        {
            var dato = DateTime.Now;

            var query = new KontoGetQuery
                            {
                                Regnskabsnummer = 1,
                                Kontonummer = "DANKORT",
                                StatusDato = dato
                            };
            var konto = _service.KontoGet(query);
            Assert.That(konto, Is.Not.Null);
            var saldoBeforeTest = konto.Saldo;

            var command = new BogføringslinjeOpretCommand
                              {
                                  Regnskabsnummer = konto.Regnskab.Nummer,
                                  Dato = dato,
                                  Kontonummer = konto.Kontonummer,
                                  Tekst = "Test fra Services",
                                  Budgetkontonummer = "8990",
                                  Debit = 5000M
                              };
            var result = _service.BogføringslinjeOpret(command);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Løbenr, Is.GreaterThan(0));

            konto = _service.KontoGet(query);
            Assert.That(konto, Is.Not.Null);
            Assert.That(konto.Saldo, Is.EqualTo(saldoBeforeTest + command.Debit));

            command = new BogføringslinjeOpretCommand
                          {
                              Regnskabsnummer = command.Regnskabsnummer,
                              Dato = command.Dato,
                              Kontonummer = command.Kontonummer,
                              Tekst = command.Tekst,
                              Budgetkontonummer = command.Budgetkontonummer,
                              Kredit = command.Debit
                          };
            result = _service.BogføringslinjeOpret(command);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Løbenr, Is.GreaterThan(0));

            konto = _service.KontoGet(query);
            Assert.That(konto, Is.Not.Null);
            Assert.That(konto.Saldo, Is.EqualTo(saldoBeforeTest));
        }
    }
}
