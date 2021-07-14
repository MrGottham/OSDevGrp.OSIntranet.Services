using System;
using OSDevGrp.OSIntranet.CommonLibrary.IoC;
using OSDevGrp.OSIntranet.Contracts.Commands;
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
        /// Tester, at bogføringslinjer kan oprettes.
        /// </summary>
        [Test]
        [Ignore("Oprettelse af bogføringslinjer er testet.")]
        public void TestAtBogføringslinjerKanOprettes()
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
            var result = _service.BogføringslinjeOpret(command);
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
            result = _service.BogføringslinjeOpret(command);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Løbenr, Is.GreaterThan(0));
        }
    }
}