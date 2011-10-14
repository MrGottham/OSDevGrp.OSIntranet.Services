using System;
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
        /// Tester, at AftaleGetAllBySystem henter alle aftaler fra og med en given dato.
        /// </summary>
        [Test]
        public void TestAtAftaleGetAllBySystemHenterAftaler()
        {
            var aftaler = _kalenderRepository.AftaleGetAllBySystem(1, new DateTime(2010, 1, 1));
            Assert.That(aftaler, Is.Not.Null);
            Assert.That(aftaler.Count(), Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at AftaleGetBySystemAndId henter en given aftale.
        /// </summary>
        [Test]
        public void TestAtAftaleGetBySystemAndIdHenterAftale()
        {
            var aftale = _kalenderRepository.AftaleGetBySystemAndId(1, 1);
            Assert.That(aftale, Is.Not.Null);
            Assert.That(aftale.System, Is.Not.Null);
            Assert.That(aftale.Deltagere, Is.Not.Null);
            foreach (var deltager in aftale.Deltagere)
            {
                Assert.That(deltager, Is.Not.Null);
                Assert.That(deltager.System, Is.Not.Null);
                Assert.That(deltager.Aftale, Is.Not.Null);
                Assert.That(deltager.Bruger, Is.Not.Null);
            }
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
