using System.IO;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.Contracts.Views;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Views
{
    /// <summary>
    /// Tester viewobject for kreditoplysninger.
    /// </summary>
    [TestFixture]
    public class KreditoplysningerViewTest
    {
        /// <summary>
        /// Tester, at viewobjekt kan initieres.
        /// </summary>
        [Test]
        public void TestAtViewKanInitieres()
        {
            var view = new KreditoplysningerView
                           {
                               År = 2010,
                               Måned = 10,
                               Kredit = 15000M
                           };
            Assert.That(view, Is.Not.Null);
            Assert.That(view.År, Is.EqualTo(2010));
            Assert.That(view.Måned, Is.EqualTo(10));
            Assert.That(view.Kredit, Is.EqualTo(15000M));
        }

        /// <summary>
        /// Tester, at viewobjekt kan serialiseres.
        /// </summary>
        [Test]
        public void TestAtViewKanSerialiseres()
        {
            var view = new KreditoplysningerView
                           {
                               År = 2010,
                               Måned = 10,
                               Kredit = 15000M
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
