using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommonLibrary.IoC;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Services;

namespace OSDevGrp.OSIntranet.Tests.Integrationstests.Services.Implementations
{
    /// <summary>
    /// Integration tests the service which can access and modify data on a house hold in the food waste domain.
    /// </summary>
    [TestFixture]
    [Category("Integrationstest")]
    public class FoodWasteHouseHoldServiceTests
    {
        #region Private variables

        private IFoodWasteHouseHoldService _foodWasteHouseHoldService;

        #endregion

        /// <summary>
        /// Opsætning af test.
        /// </summary>
        [TestFixtureSetUp]
        public void TestSetUp()
        {
            var container = ContainerFactory.Create();
            _foodWasteHouseHoldService = container.Resolve<IFoodWasteHouseHoldService>();
        }

        /// <summary>
        /// Tests that FoodGroupTreeGet gets the tree of food groups.
        /// </summary>
        [Test]
        public void TestThatFoodGroupTreeGetGetsFoodGroupTree()
        {
            var translationInfoCollection = _foodWasteHouseHoldService.TranslationInfoGetAll(new TranslationInfoCollectionGetQuery());
            Assert.That(translationInfoCollection, Is.Not.Null);
            Assert.That(translationInfoCollection, Is.Not.Empty);

            foreach (var translationInfo in translationInfoCollection)
            {
                var query = new FoodGroupTreeGetQuery
                {
                    TranslationInfoIdentifier = translationInfo.TranslationInfoIdentifier
                };
                var foodGroupTree = _foodWasteHouseHoldService.FoodGroupTreeGet(query);
                Assert.That(foodGroupTree, Is.Not.Null);
                Assert.That(foodGroupTree.FoodGroups, Is.Not.Null);
                Assert.That(foodGroupTree.FoodGroups.Count(), Is.GreaterThanOrEqualTo(0));
                Assert.That(foodGroupTree.DataProvider, Is.Not.Null);
            }
        }

        /// <summary>
        /// Tests that TranslationInfoGetAll gets all the tranlation informations.
        /// </summary>
        [Test]
        public void TestThatTranslationInfoGetAllGetsTranslationInfoSystemViewCollection()
        {
            var translationInfoCollectionGetQuery = new TranslationInfoCollectionGetQuery();
            var translationInfoSystemViewCollection = _foodWasteHouseHoldService.TranslationInfoGetAll(translationInfoCollectionGetQuery);
            Assert.That(translationInfoSystemViewCollection, Is.Not.Null);
            Assert.That(translationInfoSystemViewCollection, Is.Not.Empty);
        }
    }
}
