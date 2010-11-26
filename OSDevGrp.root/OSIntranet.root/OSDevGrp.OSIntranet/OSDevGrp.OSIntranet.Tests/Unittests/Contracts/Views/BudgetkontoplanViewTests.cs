using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Views;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Views
{
    /// <summary>
    /// Tester viewobject for en budgetkontoplan.
    /// </summary>
    [TestFixture]
    public class BudgetkontoplanViewTests
    {
        /// <summary>
        /// Tester, at viewobjekt kan initieres.
        /// </summary>
        [Test]
        public void TestAtViewKanInitieres()
        {
            var view = new BudgetkontoplanView
                           {
                               Regnskab = new RegnskabslisteView
                                              {
                                                  Nummer = 1,
                                                  Navn = "Privatregnskab, Ole Sørensen"
                                              },
                               Kontonummer = "1000",
                               Kontonavn = "Indtægter",
                               Beskrivelse = "Salg m.m.",
                               Notat = "Indtægter i form af salg m.m.",
                               Budgetkontogruppe = new BudgetkontogruppeView
                                                       {
                                                           Nummer = 1,
                                                           Navn = "Indtægter"
                                                       },
                               Budget = 15000M,
                               Bogført = 14000M,
                               Disponibel = 1000M,
                               Budgetoplysninger = new List<BudgetoplysningerView>
                                                       {
                                                           new BudgetoplysningerView
                                                               {
                                                                   År = 2010,
                                                                   Måned = 10,
                                                                   Budget = 15000M,
                                                                   Bogført = 14000M,
                                                                   Disponibel = 1000M
                                                               }
                                                       }
                           };
            Assert.That(view, Is.Not.Null);
            Assert.That(view.Regnskab, Is.Not.Null);
            Assert.That(view.Kontonummer, Is.Not.Null);
            Assert.That(view.Kontonummer, Is.EqualTo("1000"));
            Assert.That(view.Kontonavn, Is.Not.Null);
            Assert.That(view.Kontonavn, Is.EqualTo("Indtægter"));
            Assert.That(view.Beskrivelse, Is.Not.Null);
            Assert.That(view.Beskrivelse, Is.EqualTo("Salg m.m."));
            Assert.That(view.Notat, Is.Not.Null);
            Assert.That(view.Notat, Is.EqualTo("Indtægter i form af salg m.m."));
            Assert.That(view.Budgetkontogruppe, Is.Not.Null);
            Assert.That(view.Budget, Is.EqualTo(15000M));
            Assert.That(view.Bogført, Is.EqualTo(14000M));
            Assert.That(view.Disponibel, Is.EqualTo(1000M));
            Assert.That(view.Budgetoplysninger, Is.Not.Null);
            Assert.That(view.Budgetoplysninger.Count(), Is.EqualTo(1));
        }

        /// <summary>
        /// Tester, at viewobjekt kan serialiseres.
        /// </summary>
        [Test]
        public void TestAtViewKanSerialiseres()
        {
            var view = new BudgetkontoplanView
                           {
                               Regnskab = new RegnskabslisteView
                                              {
                                                  Nummer = 1,
                                                  Navn = "Privatregnskab, Ole Sørensen"
                                              },
                               Kontonummer = "1000",
                               Kontonavn = "Indtægter",
                               Beskrivelse = "Salg m.m.",
                               Notat = "Indtægter i form af salg m.m.",
                               Budgetkontogruppe = new BudgetkontogruppeView
                                                       {
                                                           Nummer = 1,
                                                           Navn = "Indtægter"
                                                       },
                               Budget = 15000M,
                               Bogført = 14000M,
                               Disponibel = 1000M,
                               Budgetoplysninger = new List<BudgetoplysningerView>
                                                       {
                                                           new BudgetoplysningerView
                                                               {
                                                                   År = 2010,
                                                                   Måned = 10,
                                                                   Budget = 15000M,
                                                                   Bogført = 14000M,
                                                                   Disponibel = 1000M
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
