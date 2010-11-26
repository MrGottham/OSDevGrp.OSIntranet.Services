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
    public class BudgetoplysningerViewTests
    {
        /// <summary>
        /// Tester, at viewobjekt kan initieres.
        /// </summary>
        [Test]
        public void TestAtViewKanInitieres()
        {
            var view = new BudgetoplysningerView
                           {
                               År = 2010,
                               Måned = 10,
                               Budget = -3000M,
                               Bogført = -2700M,
                               Disponibel = 300M
                           };
            Assert.That(view, Is.Not.Null);
            Assert.That(view.År, Is.EqualTo(2010));
            Assert.That(view.Måned, Is.EqualTo(10));
            Assert.That(view.Budget, Is.EqualTo(-3000M));
            Assert.That(view.Bogført, Is.EqualTo(-2700M));
            Assert.That(view.Disponibel, Is.EqualTo(300M));
        }

        /// <summary>
        /// Tester, at viewobjekt kan serialiseres.
        /// </summary>
        [Test]
        public void TestAtViewKanSerialiseres()
        {
            var view = new BudgetoplysningerView
                           {
                               År = 2010,
                               Måned = 10,
                               Budget = -3000M,
                               Bogført = -2700M,
                               Disponibel = 300M
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
