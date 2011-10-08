using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.IoC;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Integrationstests.Repositories
{
    /// <summary>
    /// Integrationstester repository til kalenderaftaler under OSWEBDB.
    /// </summary>
    [TestFixture]
    [Category("Integrationstest")]
    public class KalenderRepositoryTests
    {
        #region Private variables

        private IKalenderRepository _kalenderRepository;

        #endregion

        /// <summary>
        /// Opsætning af tests.
        /// </summary>
        [TestFixtureSetUp]
        public void TestSetUp()
        {
            var container = ContainerFactory.Create();
            _kalenderRepository = container.Resolve<IKalenderRepository>();
        }

        /// <summary>
        /// Tester, at BrugerGetAllBySystem henter brugere.
        /// </summary>
        [Test]
        public void TestAtBrugerGetAllBySystemHenterBrugere()
        {
            var brugere = _kalenderRepository.BrugerGetAllBySystem(1);
            Assert.That(brugere, Is.Not.Null);
            Assert.That(brugere.Count(), Is.GreaterThan(0));
        }
    }
}
