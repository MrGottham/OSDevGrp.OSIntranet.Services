using System;
using System.IO;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.Contracts.Views;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Views
{
    /// <summary>
    /// Tester viewobject for en bogføringslinje.
    /// </summary>
    [TestFixture]
    public class BogføringslinjeViewTests
    {
        /// <summary>
        /// Tester, at viewobjekt kan initieres.
        /// </summary>
        [Test]
        public void TestAtViewKanInitieres()
        {
            var bogføringsdato = new DateTime(2011, 3, 15);
            var view = new BogføringslinjeView
                           {
                               Løbenr = 1,
                               Dato = bogføringsdato,
                               Bilag = "XYZ",
                               Konto = new KontoplanView(),
                               Tekst = "Test",
                               Budgetkonto = new BudgetkontoplanView(),
                               Debit = 1000M,
                               Kredit = -500M,
                               Adressekonto = new AdressekontolisteView()
                           };
            Assert.That(view, Is.Not.Null);
            Assert.That(view.Løbenr, Is.EqualTo(1));
            Assert.That(view.Dato, Is.EqualTo(bogføringsdato));
            Assert.That(view.Bilag, Is.Not.Null);
            Assert.That(view.Bilag, Is.EqualTo("XYZ"));
            Assert.That(view.Konto, Is.Not.Null);
            Assert.That(view.Tekst, Is.Not.Null);
            Assert.That(view.Tekst, Is.EqualTo("Test"));
            Assert.That(view.Budgetkonto, Is.Not.Null);
            Assert.That(view.Debit, Is.EqualTo(1000M));
            Assert.That(view.Kredit, Is.EqualTo(-500M));
            Assert.That(view.Adressekonto, Is.Not.Null);
        }

        /// <summary>
        /// Tester, at viewobjekt kan serialiseres.
        /// </summary>
        [Test]
        public void TestAtViewKanSerialiseres()
        {
            var bogføringsdato = new DateTime(2011, 3, 15);
            var view = new BogføringslinjeView
                           {
                               Løbenr = 1,
                               Dato = bogføringsdato,
                               Bilag = "XYZ",
                               Konto = new KontoplanView(),
                               Tekst = "Test",
                               Budgetkonto = new BudgetkontoplanView(),
                               Debit = 1000M,
                               Kredit = -500M,
                               Adressekonto = new AdressekontolisteView()
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
