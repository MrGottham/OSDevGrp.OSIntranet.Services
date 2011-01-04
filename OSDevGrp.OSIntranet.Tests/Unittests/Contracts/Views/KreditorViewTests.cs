﻿using System.IO;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.Contracts.Views;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Views
{
    /// <summary>
    /// Tester viewobject for en kreditor.
    /// </summary>
    [TestFixture]
    public class KreditorViewTests
    {
        /// <summary>
        /// Tester, at viewobjekt kan initieres.
        /// </summary>
        [Test]
        public void TestAtViewKanInitieres()
        {
            var view = new KreditorView
                           {
                               Nummer = 1,
                               Navn = "Ole Sørensen",
                               Adresse1 = "Eggertsvænge 2",
                               Adresse2 = "c/o:",
                               PostnummerBy = "5700  Svendborg",
                               PrimærTelefon = "62 21 49 60",
                               SekundærTelefon = "25 24 49 75",
                               Mailadresse = "os@dsidata.dk",
                               Betalingsbetingelse = new BetalingsbetingelseView
                                                         {
                                                             Nummer = 1,
                                                             Navn = "Kontant"
                                                         },
                               Saldo = -1500M
                           };
            Assert.That(view, Is.Not.Null);
            Assert.That(view.Nummer, Is.EqualTo(1));
            Assert.That(view.Navn, Is.Not.Null);
            Assert.That(view.Navn, Is.EqualTo("Ole Sørensen"));
            Assert.That(view.Adresse1, Is.Not.Null);
            Assert.That(view.Adresse1, Is.EqualTo("Eggertsvænge 2"));
            Assert.That(view.Adresse2, Is.Not.Null);
            Assert.That(view.Adresse2, Is.EqualTo("c/o:"));
            Assert.That(view.PostnummerBy, Is.Not.Null);
            Assert.That(view.PostnummerBy, Is.EqualTo("5700  Svendborg"));
            Assert.That(view.PrimærTelefon, Is.Not.Null);
            Assert.That(view.PrimærTelefon, Is.EqualTo("62 21 49 60"));
            Assert.That(view.SekundærTelefon, Is.Not.Null);
            Assert.That(view.SekundærTelefon, Is.EqualTo("25 24 49 75"));
            Assert.That(view.Mailadresse, Is.Not.Null);
            Assert.That(view.Mailadresse, Is.EqualTo("os@dsidata.dk"));
            Assert.That(view.Betalingsbetingelse, Is.Not.Null);
            Assert.That(view.Saldo, Is.EqualTo(-1500M));
        }

        /// <summary>
        /// Tester, at viewobjekt kan serialiseres.
        /// </summary>
        [Test]
        public void TestAtViewKanSerialiseres()
        {
            var view = new KreditorView
                           {
                               Nummer = 1,
                               Navn = "Ole Sørensen",
                               Adresse1 = "Eggertsvænge 2",
                               Adresse2 = "c/o:",
                               PostnummerBy = "5700  Svendborg",
                               PrimærTelefon = "62 21 49 60",
                               SekundærTelefon = "25 24 49 75",
                               Mailadresse = "os@dsidata.dk",
                               Betalingsbetingelse = new BetalingsbetingelseView
                                                         {
                                                             Nummer = 1,
                                                             Navn = "Kontant"
                                                         },
                               Saldo = -1500M
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
