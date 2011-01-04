using System.IO;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.Contracts.Views;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Views
{
    /// <summary>
    /// Tester viewobject for en betalingsbetingelse.
    /// </summary>
    [TestFixture]
    public class BetalingsbetingelseViewTests
    {
        /// <summary>
        /// Tester, at viewobjekt kan initieres.
        /// </summary>
        [Test]
        public void TestAtViewKanInitieres()
        {
            var view = new BetalingsbetingelseView
                           {
                               Nummer = 1,
                               Navn = "Kontant",
                           };
            Assert.That(view, Is.Not.Null);
            Assert.That(view.Nummer, Is.EqualTo(1));
            Assert.That(view.Navn, Is.Not.Null);
            Assert.That(view.Navn, Is.EqualTo("Kontant"));
        }

        /// <summary>
        /// Tester, at viewobjekt kan serialiseres.
        /// </summary>
        [Test]
        public void TestAtViewKanSerialiseres()
        {
            var view = new BetalingsbetingelseView
                           {
                               Nummer = 1,
                               Navn = "Kontant",
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
