using System;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.IoC;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf.ChannelFactory;
using OSDevGrp.OSIntranet.Contracts.Commands;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Services;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Integrationstests.Services.ClientCalls
{
    /// <summary>
    /// Integrationstester servicen til finansstyring med kald fra klienter.
    /// </summary>
    [TestFixture]
    [Category("Integrationstest")]
    public class FinansstyringServiceTests
    {
        #region Private constants

        private const string ClientEndpointName = "FinansstyringServiceIntegrationstest";

        #endregion

        #region Private variables

        private IChannelFactory _channelFactory;

        #endregion

        /// <summary>
        /// Opsætning af test.
        /// </summary>
        [SetUp]
        public void TestSetUp()
        {
            var container = ContainerFactory.Create();
            _channelFactory = container.Resolve<IChannelFactory>();
        }

        /// <summary>
        /// Tester, at en kontoplan kan hentes.
        /// </summary>
        [Test]
        public void TestAtKontoplanHentes()
        {
            var client = _channelFactory.CreateChannel<IFinansstyringService>(ClientEndpointName);
            try
            {
                var query = new KontoplanGetQuery
                                {
                                    Regnskabsnummer = 1,
                                    StatusDato = new DateTime(2011, 3, 1)
                                };
                var result = client.KontoplanGet(query);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count(), Is.GreaterThan(0));
            }
            finally
            {
                ChannelTools.CloseChannel(client);
            }
        }

        /// <summary>
        /// Tester, at en konto kan hentes.
        /// </summary>
        [Test]
        public void TestAtKontoHentes()
        {
            var client = _channelFactory.CreateChannel<IFinansstyringService>(ClientEndpointName);
            try
            {
                var query = new KontoGetQuery
                                {
                                    Regnskabsnummer = 1,
                                    Kontonummer = "DANKORT",
                                    StatusDato = new DateTime(2011, 3, 1)
                                };
                var result = client.KontoGet(query);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Regnskab, Is.Not.Null);
                Assert.That(result.Regnskab.Nummer, Is.EqualTo(1));
                Assert.That(result.Kontonummer, Is.Not.Null);
                Assert.That(result.Kontonummer, Is.EqualTo("DANKORT"));
            }
            finally
            {
                ChannelTools.CloseChannel(client);
            }
        }

        /// <summary>
        /// Tester, at en budgetkontoplan kan hentes.
        /// </summary>
        [Test]
        public void TestAtBudgetkontoplanHentes()
        {
            var client = _channelFactory.CreateChannel<IFinansstyringService>(ClientEndpointName);
            try
            {
                var query = new BudgetkontoplanGetQuery
                                {
                                    Regnskabsnummer = 1,
                                    StatusDato = new DateTime(2011, 3, 1)
                                };
                var result = client.BudgetkontoplanGet(query);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count(), Is.GreaterThan(0));
            }
            finally
            {
                ChannelTools.CloseChannel(client);
            }
        }

        /// <summary>
        /// Tester, at en budgetkonto kan hentes.
        /// </summary>
        [Test]
        public void TestAtBudgetkontoHentes()
        {
            var client = _channelFactory.CreateChannel<IFinansstyringService>(ClientEndpointName);
            try
            {
                var query = new BudgetkontoGetQuery
                                {
                                    Regnskabsnummer = 1,
                                    Kontonummer = "1000",
                                    StatusDato = new DateTime(2011, 3, 1)
                                };
                var result = client.BudgetkontoGet(query);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Regnskab, Is.Not.Null);
                Assert.That(result.Regnskab.Nummer, Is.EqualTo(1));
                Assert.That(result.Kontonummer, Is.Not.Null);
                Assert.That(result.Kontonummer, Is.EqualTo("1000"));
            }
            finally
            {
                ChannelTools.CloseChannel(client);
            }
        }

        /// <summary>
        /// Tester, at antal bogføringslinjer kan hentes.
        /// </summary>
        [Test]
        public void TestAtBogføringslinjerKanHentes()
        {
            var client = _channelFactory.CreateChannel<IFinansstyringService>(ClientEndpointName);
            try
            {
                var query = new BogføringerGetQuery
                                {
                                    Regnskabsnummer = 1,
                                    StatusDato = new DateTime(2011, 3, 1),
                                    Linjer = 250
                                };
                var result = client.BogføringerGet(query);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count(), Is.EqualTo(250));
            }
            finally
            {
                ChannelTools.CloseChannel(client);
            }
        }

        /// <summary>
        /// Tester, at bogføringslinjer kan oprettes.
        /// </summary>
        [Test]
        [Ignore("Oprettelse af bogføringslinjer er testet.")]
        public void TestAtBogføringslinjerKanOprettes()
        {
            var client = _channelFactory.CreateChannel<IFinansstyringService>(ClientEndpointName);
            try
            {
                var dato = DateTime.Now;

                var query = new KontoGetQuery
                                {
                                    Regnskabsnummer = 1,
                                    Kontonummer = "DANKORT",
                                    StatusDato = dato
                                };
                var konto = client.KontoGet(query);
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
                var result = client.BogføringslinjeOpret(command);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Løbenr, Is.GreaterThan(0));

                konto = client.KontoGet(query);
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
                result = client.BogføringslinjeOpret(command);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Løbenr, Is.GreaterThan(0));

                konto = client.KontoGet(query);
                Assert.That(konto, Is.Not.Null);
                Assert.That(konto.Saldo, Is.EqualTo(saldoBeforeTest));
            }
            finally
            {
                ChannelTools.CloseChannel(client);
            }
        }
    }
}