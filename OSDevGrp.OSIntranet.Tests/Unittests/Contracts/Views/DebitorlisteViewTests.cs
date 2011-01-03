using System.IO;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.Contracts.Views;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Views
{
    /// <summary>
    /// Tester viewobject for en debitorliste.
    /// </summary>
    [TestFixture]
    public class DebitorlisteViewTests
    {
        /// <summary>
        /// Tester, at viewobjekt kan initieres.
        /// </summary>
        [Test]
        public void TestAtViewKanInitieres()
        {
            var view = new DebitorlisteView
                           {
                               Nummer = 1,
                               Navn = "Ole Sørensen",
                               PrimærTelefon = "62 21 49 60",
                               SekundærTelefon = "25 24 49 75",
                               Saldo = 1500M
                           };
            Assert.That(view, Is.Not.Null);
            Assert.That(view.Nummer, Is.EqualTo(1));
            Assert.That(view.Navn, Is.Not.Null);
            Assert.That(view.Navn, Is.EqualTo("Ole Sørensen"));
            Assert.That(view.PrimærTelefon, Is.Not.Null);
            Assert.That(view.PrimærTelefon, Is.EqualTo("62 21 49 60"));
            Assert.That(view.SekundærTelefon, Is.Not.Null);
            Assert.That(view.SekundærTelefon, Is.EqualTo("25 24 49 75"));
            Assert.That(view.Saldo, Is.EqualTo(1500M));
        }

        /// <summary>
        /// Tester, at viewobjekt kan serialiseres.
        /// </summary>
        [Test]
        public void TestAtViewKanSerialiseres()
        {
            var view = new DebitorlisteView
                           {
                               Nummer = 1,
                               Navn = "Ole Sørensen",
                               PrimærTelefon = "62 21 49 60",
                               SekundærTelefon = "25 24 49 75",
                               Saldo = 1500M
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
