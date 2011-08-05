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
        [TestFixtureSetUp]
        public void TestSetUp()
        {
            var container = ContainerFactory.Create();
            _channelFactory = container.Resolve<IChannelFactory>();
        }

        /// <summary>
        /// Tester, at en regnskabsliste kan hentes.
        /// </summary>
        [Test]
        public void TestAtRegnskabslisteHentes()
        {
            var client = _channelFactory.CreateChannel<IFinansstyringService>(ClientEndpointName);
            try
            {
                var query = new RegnskabslisteGetQuery();
                var result = client.RegnskabslisteGet(query);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count(), Is.GreaterThan(0));
            }
            finally
            {
                ChannelTools.CloseChannel(client);
            }
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
        /// Tester, at en debitorliste kan hentes.
        /// </summary>
        [Test]
        public void TestAtDebitorlisteHentes()
        {
            var client = _channelFactory.CreateChannel<IFinansstyringService>(ClientEndpointName);
            try
            {
                var query = new DebitorlisteGetQuery
                                {
                                    Regnskabsnummer = 1,
                                    StatusDato = new DateTime(2011, 1, 16)
                                };
                var result = client.DebitorlisteGet(query);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count(), Is.GreaterThan(0));
            }
            finally
            {
                ChannelTools.CloseChannel(client);
            }
        }

        /// <summary>
        /// Tester, at en debitor kan hentes.
        /// </summary>
        [Test]
        public void TestAtDebitorHentes()
        {
            var client = _channelFactory.CreateChannel<IFinansstyringService>(ClientEndpointName);
            try
            {
                var query = new DebitorGetQuery
                                {
                                    Regnskabsnummer = 1,
                                    Nummer = 205,
                                    StatusDato = new DateTime(2011, 1, 16)
                                };
                var result = client.DebitorGet(query);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Nummer, Is.EqualTo(205));
                Assert.That(result.Saldo, Is.LessThan(0));
            }
            finally
            {
                ChannelTools.CloseChannel(client);
            }
        }

        /// <summary>
        /// Tester, at en kreditorliste kan hentes.
        /// </summary>
        [Test]
        public void TestAtKreditorlisteKanHentes()
        {
            var client = _channelFactory.CreateChannel<IFinansstyringService>(ClientEndpointName);
            try
            {
                var query = new KreditorlisteGetQuery
                                {
                                    Regnskabsnummer = 1,
                                    StatusDato = new DateTime(2010, 8, 10)
                                };
                var result = client.KreditorlisteGet(query);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count(), Is.GreaterThan(0));
            }
            finally
            {
                ChannelTools.CloseChannel(client);
            }
        }

        /// <summary>
        /// Tester, at en kreditor kan hentes.
        /// </summary>
        [Test]
        public void TestAtKreditorHentes()
        {
            var client = _channelFactory.CreateChannel<IFinansstyringService>(ClientEndpointName);
            try
            {
                var query = new KreditorGetQuery
                                {
                                    Regnskabsnummer = 1,
                                    Nummer = 205,
                                    StatusDato = new DateTime(2010, 8, 10)
                                };
                var result = client.KreditorGet(query);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Nummer, Is.EqualTo(205));
                Assert.That(result.Saldo, Is.GreaterThan(0));
            }
            finally
            {
                ChannelTools.CloseChannel(client);
            }
        }

        /// <summary>
        /// Tester, at en liste af adressekonti kan hentes.
        /// </summary>
        [Test]
        public void TestAtAdressekontolisteKanHentes()
        {
            var client = _channelFactory.CreateChannel<IFinansstyringService>(ClientEndpointName);
            try
            {
                var query = new AdressekontolisteGetQuery
                                {
                                    Regnskabsnummer = 1,
                                    StatusDato = new DateTime(2011, 3, 1)
                                };
                var result = client.AdressekontolisteGet(query);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count(), Is.GreaterThan(0));
            }
            finally
            {
                ChannelTools.CloseChannel(client);
            }
        }

        /// <summary>
        /// Tester, at en adressekonto kan hentes.
        /// </summary>
        [Test]
        public void TestAtAdressekontoKanHentes()
        {
            var client = _channelFactory.CreateChannel<IFinansstyringService>(ClientEndpointName);
            try
            {
                var query = new AdressekontoGetQuery
                                {
                                    Regnskabsnummer = 1,
                                    Nummer = 1,
                                    StatusDato = new DateTime(2011, 3, 1)
                                };
                var result = client.AdressekontoGet(query);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Nummer, Is.EqualTo(1));
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
                                      Regnskabsnummer = 1,
                                      Dato = dato,
                                      Kontonummer = "DANKORT",
                                      Tekst = "Test fra Services",
                                      Budgetkontonummer = "8990",
                                      Debit = 5000M
                                  };
                var result = client.BogføringslinjeOpret(command);
                Assert.That(result, Is.Not.Null);

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

                konto = client.KontoGet(query);
                Assert.That(konto, Is.Not.Null);
                Assert.That(konto.Saldo, Is.EqualTo(saldoBeforeTest));
            }
            finally
            {
                ChannelTools.CloseChannel(client);
            }
        }

        /// <summary>
        /// Tester, at betalingsbetingelser kan hentes.
        /// </summary>
        [Test]
        public void TestAtBetalingsbetingelserKanHentes()
        {
            var client = _channelFactory.CreateChannel<IFinansstyringService>(ClientEndpointName);
            try
            {
                var query = new BetalingsbetingelserGetQuery();
                var result = client.BetalingsbetingelserGet(query);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count(), Is.GreaterThan(0));
            }
            finally
            {
                ChannelTools.CloseChannel(client);
            }
        }

        /// <summary>
        /// Tester, at kontogrupper kan hentes.
        /// </summary>
        [Test]
        public void TestAtKontogrupperKanHentes()
        {
            var client = _channelFactory.CreateChannel<IFinansstyringService>(ClientEndpointName);
            try
            {
                var query = new KontogrupperGetQuery();
                var result = client.KontogrupperGet(query);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count(), Is.GreaterThan(0));
            }
            finally
            {
                ChannelTools.CloseChannel(client);
            }
        }

        /// <summary>
        /// Tester, at grupper til budgetkonti kan hentes.
        /// </summary>
        [Test]
        public void TestAtBudgetkontogrupperKanHentes()
        {
            var client = _channelFactory.CreateChannel<IFinansstyringService>(ClientEndpointName);
            try
            {
                var query = new BudgetkontogrupperGetQuery();
                var result = client.BudgetkontogrupperGet(query);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count(), Is.GreaterThan(0));
            }
            finally
            {
                ChannelTools.CloseChannel(client);
            }
        }

        /// <summary>
        /// Tester, at brevhoveder kan hentes.
        /// </summary>
        [Test]
        public void TestAtBrevhovederKanHentes()
        {
            var client = _channelFactory.CreateChannel<IFinansstyringService>(ClientEndpointName);
            try
            {
                var query = new BrevhovederGetQuery();
                var result = client.BrevhovederGet(query);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count(), Is.GreaterThan(0));
            }
            finally
            {
                ChannelTools.CloseChannel(client);
            }
        }
    }
}
