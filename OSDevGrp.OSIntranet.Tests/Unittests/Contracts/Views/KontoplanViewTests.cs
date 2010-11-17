using System.IO;
using System.Runtime.Serialization;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Views;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Views
{
    /// <summary>
    /// Tester viewobject for en kontogruppe.Tester viewobject for en kontoplan.
    /// </summary>
    [TestFixture]
    public class KontoplanViewTests
    {
        /// <summary>
        /// Tester, at viewobjekt kan initieres.
        /// </summary>
        [Test]
        public void TestAtViewKanInitieres()
        {
            var view = new KontoplanView
                           {
                               Regnskab = new RegnskabslisteView
                                              {
                                                  Nummer = 1,
                                                  Navn = "Privatregnskab, Ole Sørensen"
                                              },
                               Kontonummer = "DANKORT",
                               Kontonavn = "Dankort",
                               Beskrivelse = "Dankort/Lønkonto",
                               Notat = "Kredit på kr. 10.000,00",
                               Kontogruppe = new KontogruppeView
                                                 {
                                                     Nummer = 1,
                                                     Navn = "Bankkonti½"

                                                 },
                               Kredit = 10000M,
                               Saldo = 5000M,
                               Disponibel = 15000M
                           };
            Assert.That(view, Is.Not.Null);
            Assert.That(view.Regnskab, Is.Not.Null);
            Assert.That(view.Kontonummer, Is.Not.Null);
            Assert.That(view.Kontonummer, Is.EqualTo("DANKORT"));
            Assert.That(view.Kontonavn, Is.Not.Null);
            Assert.That(view.Kontonavn, Is.EqualTo("Dankort"));
            Assert.That(view.Beskrivelse, Is.Not.Null);
            Assert.That(view.Beskrivelse, Is.EqualTo("Dankort/Lønkonto"));
            Assert.That(view.Notat, Is.Not.Null);
            Assert.That(view.Notat, Is.EqualTo("Kredit på kr. 10.000,00"));
            Assert.That(view.Kontogruppe, Is.Not.Null);
            Assert.That(view.Kredit, Is.EqualTo(10000M));
            Assert.That(view.Saldo, Is.EqualTo(5000M));
            Assert.That(view.Disponibel, Is.EqualTo(15000M));
        }

        /// <summary>
        /// Tester, at viewobjekt kan serialiseres.
        /// </summary>
        [Test]
        public void TestAtViewKanSerialiseres()
        {
            var view = new KontoplanView
                           {
                               Regnskab = new RegnskabslisteView
                                              {
                                                  Nummer = 1,
                                                  Navn = "Privatregnskab, Ole Sørensen"
                                              },
                               Kontonummer = "DANKORT",
                               Kontonavn = "Dankort",
                               Beskrivelse = "Dankort/Lønkonto",
                               Notat = "Kredit på kr. 10.000,00",
                               Kontogruppe = new KontogruppeView
                                                 {
                                                     Nummer = 1,
                                                     Navn = "Bankkonti½"

                                                 },
                               Kredit = 10000M,
                               Saldo = 5000M,
                               Disponibel = 15000M
                           };
            Assert.That(view, Is.Not.Null);
            var memoryStream = new MemoryStream();
            try
            {
                var serializer = new DataContractSerializer(view.GetType());
                serializer.WriteObject(memoryStream, view);
                memoryStream.Flush();
                Assert.That(memoryStream.Length, Is.GreaterThan(0));
            }
            finally
            {
                memoryStream.Close();
            }
        }
    }
}
