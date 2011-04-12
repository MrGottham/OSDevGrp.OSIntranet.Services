using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.IoC;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Integrationstests.Repositories
{
    /// <summary>
    /// Integrationstester repository for finansstyring.
    /// </summary>
    [TestFixture]
    [Category("Integrationstest")]
    public class FinansstyringRepositoryTests
    {
        #region Private variables

        private IFinansstyringRepository _finansstyringRepository;

        #endregion

        /// <summary>
        /// Opsætning af tests.
        /// </summary>
        [TestFixtureSetUp]
        public void TestSetUp()
        {
            var container = ContainerFactory.Create();
            _finansstyringRepository = container.Resolve<IFinansstyringRepository>();
        }

        /// <summary>
        /// Tester, at RegnskabslisteGet henter regnskaber.
        /// </summary>
        [Test]
        public void TestAtRegnskabslisteGetHenterRegnskaber()
        {
            var regnskaber = _finansstyringRepository.RegnskabslisteGet();
            Assert.That(regnskaber, Is.Not.Null);
            Assert.That(regnskaber.Count(), Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at RegnskabGet henter et givent regnskab.
        /// </summary>
        [Test]
        public void TestAtRegnskabGetHenterRegnskab()
        {
            var regnskab = _finansstyringRepository.RegnskabGet(1);
            Assert.That(regnskab, Is.Not.Null);
            Assert.That(regnskab.Nummer, Is.EqualTo(1));
        }

        /// <summary>
        /// Tester, at KontogruppeGetAll henter kontogrupper.
        /// </summary>
        [Test]
        public void TestAtKontogruppeGetAllHenterKontogrupper()
        {
            var kontogrupper = _finansstyringRepository.KontogruppeGetAll();
            Assert.That(kontogrupper, Is.Not.Null);
            Assert.That(kontogrupper.Count(), Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at BudgetkontogruppeGetAll henter grupper for budgetkonti.
        /// </summary>
        [Test]
        public void TestAtBudgetkontogruppeGetAllHenterBudgetkontogrupper()
        {
            var budgetkontogrupper = _finansstyringRepository.BudgetkontogruppeGetAll();
            Assert.That(budgetkontogrupper, Is.Not.Null);
            Assert.That(budgetkontogrupper.Count(), Is.GreaterThan(0));
        }
    }
}
