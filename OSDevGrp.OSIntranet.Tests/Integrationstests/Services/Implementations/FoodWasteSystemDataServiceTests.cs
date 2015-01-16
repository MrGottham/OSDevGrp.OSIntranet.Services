using NUnit.Framework;
using OSDevGrp.OSIntranet.CommonLibrary.IoC;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Services;

namespace OSDevGrp.OSIntranet.Tests.Integrationstests.Services.Implementations
{
    /// <summary>
    /// Integration tests the service which can access and modify system data in the food waste domain.
    /// </summary>
    [TestFixture]
    [Category("Integrationstest")]
    public class FoodWasteSystemDataServiceTests
    {
        #region Private variables

        private IFoodWasteSystemDataService _foodWasteSystemDataService;

        #endregion

        /// <summary>
        /// Opsætning af test.
        /// </summary>
        [TestFixtureSetUp]
        public void TestSetUp()
        {
            var container = ContainerFactory.Create();
            _foodWasteSystemDataService = container.Resolve<IFoodWasteSystemDataService>();
        }

        /// <summary>
        /// Tests that TranslationInfoGetAll gets all the tranlation informations.
        /// </summary>
        [Test]
        public void TestThatTranslationInfoGetAllGetsTranslationInfoSystemViewCollection()
        {
            var translationInfoCollectionGetQuery = new TranslationInfoCollectionGetQuery();
            var translationInfoSystemViewCollection = _foodWasteSystemDataService.TranslationInfoGetAll(translationInfoCollectionGetQuery);
            Assert.That(translationInfoSystemViewCollection, Is.Not.Null);
            Assert.That(translationInfoSystemViewCollection, Is.Not.Empty);
        }
    }
}
