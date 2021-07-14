using System;
using OSDevGrp.OSIntranet.CommonLibrary.IoC;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf.ChannelFactory;
using OSDevGrp.OSIntranet.Contracts.Commands;
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

                var command = new BogføringslinjeOpretCommand
                                  {
                                      Regnskabsnummer = -1,
                                      Dato = dato,
                                      Kontonummer = string.Empty,
                                      Tekst = "Test fra Services",
                                      Budgetkontonummer = string.Empty,
                                      Debit = 5000M
                                  };
                var result = client.BogføringslinjeOpret(command);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Løbenr, Is.GreaterThan(0));

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
            }
            finally
            {
                ChannelTools.CloseChannel(client);
            }
        }
    }
}