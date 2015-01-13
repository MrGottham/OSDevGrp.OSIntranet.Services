using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommonLibrary.IoC;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.Tests.Integrationstests.Repositories.FoodWaste
{
    /// <summary>
    /// Integration tests for the repository which can access system data for the food waste domain.
    /// </summary>
    [TestFixture]
    [Category("Integrationstest")]
    public class SystemDataRepositoryTests
    {
        #region Private variables

        private ISystemDataRepository _systemDataRepository;

        #endregion

        /// <summary>
        /// Opsætning af tests.
        /// </summary>
        [TestFixtureSetUp]
        public void TestSetUp()
        {
            var container = ContainerFactory.Create();
            _systemDataRepository = container.Resolve<ISystemDataRepository>();
        }

        /// <summary>
        /// Tests that TranslationInfoGetAll returns all the translation informations.
        /// </summary>
        [Test]
        public void TestThatTranslationInfoGetAllReturnsTranslationInfos()
        {
            var translationInfos = _systemDataRepository.TranslationInfoGetAll();
            Assert.That(translationInfos, Is.Not.Null);
            Assert.That(translationInfos, Is.Not.Empty);
            Assert.That(translationInfos.Count(), Is.EqualTo(2));
        }
    }
}
