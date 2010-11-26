using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.Contracts.Views;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Views
{
    /// <summary>
    /// Tester viewobject for en budgetplan.
    /// </summary>
    [TestFixture]
    public class BudgetplanViewTests
    {
        /// <summary>
        /// Tester, at viewobjekt kan initieres.
        /// </summary>
        [Test]
        public void TestAtViewKanInitieres()
        {
            var view = new BudgetplanView
                           {
                               Nummer = 1,
                               Navn = "Indtægter",
                               Budget = 15000M,
                               Bogført = 14000M,
                               Disponibel = 1000M,
                               Budgetkonti = new List<BudgetkontoplanView>(),
                               Budgetoplysninger = new List<BudgetoplysningerView>()
                           };
            Assert.That(view, Is.Not.Null);
            Assert.That(view.Nummer, Is.EqualTo(1));
            Assert.That(view.Navn, Is.Not.Null);
            Assert.That(view.Navn, Is.EqualTo("Indtægter"));
            Assert.That(view.Budget, Is.EqualTo(15000M));
            Assert.That(view.Bogført, Is.EqualTo(14000M));
            Assert.That(view.Disponibel, Is.EqualTo(1000M));
            Assert.That(view.Budgetkonti, Is.Not.Null);
            Assert.That(view.Budgetkonti.Count(), Is.EqualTo(0));
            Assert.That(view.Budgetoplysninger, Is.Not.Null);
            Assert.That(view.Budgetoplysninger.Count(), Is.EqualTo(0));
        }

        /// <summary>
        /// Tester, at viewobjekt kan serialiseres.
        /// </summary>
        [Test]
        public void TestAtViewKanSerialiseres()
        {
            var view = new BudgetplanView
                           {
                               Nummer = 1,
                               Navn = "Indtægter",
                               Budget = 15000M,
                               Bogført = 14000M,
                               Disponibel = 1000M,
                               Budgetkonti = new List<BudgetkontoplanView>(),
                               Budgetoplysninger = new List<BudgetoplysningerView>()
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
