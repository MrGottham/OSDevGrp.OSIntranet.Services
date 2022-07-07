using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.IoC;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Integrationstests.Repositories
{
    /// <summary>
    /// Integrationstester repository for fælles elementer i domænet.
    /// </summary>
    [TestFixture]
    [Category("Integrationstest")]
    public class FællesRepositoryTests
    {
        #region Private variables

        private IFællesRepository _fællesRepository;

        #endregion

        /// <summary>
        /// Opsætning af tests.
        /// </summary>
        [SetUp]
        public void TestSetUp()
        {
            var container = ContainerFactory.Create();
            _fællesRepository = container.Resolve<IFællesRepository>();
        }

        /// <summary>
        /// Tester, at SystemGetAll henter systemer.
        /// </summary>
        [Test]
        public void TestAtSystemGetAllHenterSystemer()
        {
            var systemer = _fællesRepository.SystemGetAll();
            Assert.That(systemer, Is.Not.Null);
            Assert.That(systemer.Count(), Is.GreaterThan(0));
        }
    }
}