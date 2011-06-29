using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.Domain.Finansstyring;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Domain.Finansstyring
{
    /// <summary>
    /// Tester hjælper til en liste af grupper til budgetkonti.
    /// </summary>
    [TestFixture]
    public class BudgetkontogruppelisteHelperTests
    {
        /// <summary>
        /// Tester, at GetById henter og returnerer en given gruppe til budgetkonti.
        /// </summary>
        [Test]
        public void TestAtGetByIdHenterBudgetkontogruppe()
        {
            var fixture = new Fixture();
            var budgetkontogrupper = fixture.CreateMany<Budgetkontogruppe>(3).ToList();
            var budgetkontogruppelisteHelper = new BudgetkontogruppelisteHelper(budgetkontogrupper);
            Assert.That(budgetkontogruppelisteHelper, Is.Not.Null);

            var budgetkontogruppe = budgetkontogruppelisteHelper.GetById(budgetkontogrupper.ElementAt(1).Nummer);
            Assert.That(budgetkontogruppe, Is.Not.Null);
            Assert.That(budgetkontogruppe.Nummer, Is.EqualTo(budgetkontogrupper.ElementAt(1).Nummer));
            Assert.That(budgetkontogruppe.Navn, Is.Not.Null);
            Assert.That(budgetkontogruppe.Navn, Is.EqualTo(budgetkontogrupper.ElementAt(1).Navn));
        }

        /// <summary>
        /// Tester, at GetById kaster en IntranetRepositoryException, hvis gruppen til budgetkonti ikke findes.
        /// </summary>
        [Test]
        public void TestAtGetByIdKasterIntranetRepositoryExceptionHvisBudgetkontogruppenIkkeFindes()
        {
            var fixture = new Fixture();
            var budgetkontogrupper = fixture.CreateMany<Budgetkontogruppe>(3).ToList();
            var budgetkontogruppelisteHelper = new BudgetkontogruppelisteHelper(budgetkontogrupper);
            Assert.That(budgetkontogruppelisteHelper, Is.Not.Null);

            Assert.Throws<IntranetRepositoryException>(() => budgetkontogruppelisteHelper.GetById(-1));
        }
    }
}
