using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.Contracts.Views;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Views
{
    /// <summary>
    /// Tester viewobject for en konto.
    /// </summary>
    [TestFixture]
    public class KontoViewTests
    {
        /// <summary>
        /// Tester, at viewobjekt kan initieres.
        /// </summary>
        [Test]
        public void TestAtViewKanInitieres()
        {
            var view = new KontoView
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
                                                     Navn = "Bankkonti",
                                                     ErAktiver = true,
                                                     ErPassiver = false
                                                 },
                               Kredit = 10000M,
                               Saldo = 5000M,
                               Disponibel = 15000M,
                               Kreditoplysninger = new List<KreditoplysningerView>(3)
                                                       {
                                                           new KreditoplysningerView
                                                               {
                                                                   År = 2010,
                                                                   Måned = 8,
                                                                   Kredit = 10000M
                                                               },
                                                           new KreditoplysningerView
                                                               {
                                                                   År = 2010,
                                                                   Måned = 9,
                                                                   Kredit = 10000M
                                                               },
                                                           new KreditoplysningerView
                                                               {
                                                                   År = 2010,
                                                                   Måned = 10,
                                                                   Kredit = 10000M
                                                               }
                                                       }
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
            Assert.That(view.Kreditoplysninger, Is.Not.Null);
            Assert.That(view.Kreditoplysninger.Count(), Is.EqualTo(3));
        }

        /// <summary>
        /// Tester, at viewobjekt kan serialiseres.
        /// </summary>
        [Test]
        public void TestAtViewKanSerialiseres()
        {
            var view = new KontoView
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
                                                     Navn = "Bankkonti",
                                                     ErAktiver = true,
                                                     ErPassiver = false
                                                 },
                               Kredit = 10000M,
                               Saldo = 5000M,
                               Disponibel = 15000M,
                               Kreditoplysninger = new List<KreditoplysningerView>(3)
                                                       {
                                                           new KreditoplysningerView
                                                               {
                                                                   År = 2010,
                                                                   Måned = 8,
                                                                   Kredit = 10000M
                                                               },
                                                           new KreditoplysningerView
                                                               {
                                                                   År = 2010,
                                                                   Måned = 9,
                                                                   Kredit = 10000M
                                                               },
                                                           new KreditoplysningerView
                                                               {
                                                                   År = 2010,
                                                                   Måned = 10,
                                                                   Kredit = 10000M
                                                               }
                                                       }
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
