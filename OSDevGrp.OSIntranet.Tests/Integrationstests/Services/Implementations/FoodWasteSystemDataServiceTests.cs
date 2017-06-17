using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommonLibrary.IoC;
using OSDevGrp.OSIntranet.Contracts.Commands;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Services;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;

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
        [SetUp]
        public void TestSetUp()
        {
            var container = ContainerFactory.Create();
            _foodWasteSystemDataService = container.Resolve<IFoodWasteSystemDataService>();
        }

        /// <summary>
        /// Tests that StorageTypeGetAll gets the collection of all storages types.
        /// </summary>
        [Test]
        public void TestThatStorageTypeGetAllGetGetsStorageTypeSystemViewCollection()
        {
            IList<TranslationInfoSystemView> translationInfoCollection = GetTranslationInfoCollection();
            Assert.That(translationInfoCollection, Is.Not.Null);
            Assert.That(translationInfoCollection, Is.Not.Empty);

            foreach (TranslationInfoSystemView translationInfo in translationInfoCollection)
            {
                StorageTypeCollectionGetQuery storageTypeCollectionGetQuery = new StorageTypeCollectionGetQuery
                {
                    TranslationInfoIdentifier = translationInfo.TranslationInfoIdentifier
                };
                IList<StorageTypeSystemView> storageTypeCollection = new List<StorageTypeSystemView>(_foodWasteSystemDataService.StorageTypeGetAll(storageTypeCollectionGetQuery));
                Assert.That(storageTypeCollection, Is.Not.Null);
                Assert.That(storageTypeCollection, Is.Not.Empty);
                Assert.That(storageTypeCollection.Count, Is.EqualTo(4));
                Assert.That(storageTypeCollection.SingleOrDefault(m => m.StorageTypeIdentifier == StorageType.IdentifierForRefrigerator), Is.Not.Null);
                Assert.That(storageTypeCollection.SingleOrDefault(m => m.StorageTypeIdentifier == StorageType.IdentifierForFreezer), Is.Not.Null);
                Assert.That(storageTypeCollection.SingleOrDefault(m => m.StorageTypeIdentifier == StorageType.IdentifierForKitchenCabinets), Is.Not.Null);
                Assert.That(storageTypeCollection.SingleOrDefault(m => m.StorageTypeIdentifier == StorageType.IdentifierForShoppingBasket), Is.Not.Null);
            }
        }

        /// <summary>
        /// Tests that FoodItemCollectionGet gets the collection of food items.
        /// </summary>
        [Test]
        public void TestThatFoodItemCollectionGetGetsFoodItemCollection()
        {
            IList<TranslationInfoSystemView> translationInfoCollection = GetTranslationInfoCollection();
            Assert.That(translationInfoCollection, Is.Not.Null);
            Assert.That(translationInfoCollection, Is.Not.Empty);

            foreach (var translationInfo in translationInfoCollection)
            {
                var foodGroupTreeGetQuery = new FoodGroupTreeGetQuery
                {
                    TranslationInfoIdentifier = translationInfo.TranslationInfoIdentifier
                };
                var foodGroupTree = _foodWasteSystemDataService.FoodGroupTreeGet(foodGroupTreeGetQuery);
                Assert.That(foodGroupTree, Is.Not.Null);
                Assert.That(foodGroupTree.FoodGroups, Is.Not.Null);

                var foodGroupIdentifiers = new List<Guid?> {null};
                if (foodGroupTree.FoodGroups.Any())
                {
                    foodGroupIdentifiers.Add(foodGroupTree.FoodGroups.Take(1).ElementAt(0).FoodGroupIdentifier);
                }
                foreach (var foodGroupIdentifier in foodGroupIdentifiers)
                {
                    var foodItemCollectionGetQuery = new FoodItemCollectionGetQuery
                    {
                        TranslationInfoIdentifier = translationInfo.TranslationInfoIdentifier,
                        FoodGroupIdentifier = foodGroupIdentifier
                    };
                    var foodItemCollection = _foodWasteSystemDataService.FoodItemCollectionGet(foodItemCollectionGetQuery);
                    Assert.That(foodItemCollection, Is.Not.Null);
                    Assert.That(foodItemCollection.FoodItems, Is.Not.Null);
                    Assert.That(foodItemCollection.FoodItems.Count(), Is.GreaterThanOrEqualTo(0));
                    Assert.That(foodItemCollection.DataProvider, Is.Not.Null);
                }
            }
        }

        /// <summary>
        /// Tests that FoodItemImportFromDataProvider imports food items from the data provider.
        /// </summary>
        [Test]
        public void TestThatFoodItemImportFromDataProviderImportsFoodItems()
        {
            IList<TranslationInfoSystemView> translationInfoCollection = GetTranslationInfoCollection();
            Assert.That(translationInfoCollection, Is.Not.Null);
            Assert.That(translationInfoCollection, Is.Not.Empty);

            var foodGroupTree = _foodWasteSystemDataService.FoodGroupTreeGet(new FoodGroupTreeGetQuery {TranslationInfoIdentifier = translationInfoCollection.First().TranslationInfoIdentifier});
            Assert.That(foodGroupTree, Is.Not.Null);

            foreach (var command in TestHelper.GetFoodItemImportFromDataProviderCommands(foodGroupTree, translationInfoCollection))
            {
                var result = _foodWasteSystemDataService.FoodItemImportFromDataProvider(command);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Identifier, Is.Not.EqualTo(default(Guid)));
                Assert.That(result.Identifier, Is.Not.EqualTo(Guid.Empty));
                Assert.That(result.EventDate, Is.EqualTo(DateTime.Now).Within(5).Seconds);
            }
        }

        /// <summary>
        /// Tests that FoodGroupTreeGet gets the tree of food groups.
        /// </summary>
        [Test]
        public void TestThatFoodGroupTreeGetGetsFoodGroupTree()
        {
            IList<TranslationInfoSystemView> translationInfoCollection = GetTranslationInfoCollection();
            Assert.That(translationInfoCollection, Is.Not.Null);
            Assert.That(translationInfoCollection, Is.Not.Empty);

            foreach (var translationInfo in translationInfoCollection)
            {
                var query = new FoodGroupTreeGetQuery
                {
                    TranslationInfoIdentifier = translationInfo.TranslationInfoIdentifier
                };
                var foodGroupTree = _foodWasteSystemDataService.FoodGroupTreeGet(query);
                Assert.That(foodGroupTree, Is.Not.Null);
                Assert.That(foodGroupTree.FoodGroups, Is.Not.Null);
                Assert.That(foodGroupTree.FoodGroups.Count(), Is.GreaterThanOrEqualTo(0));
                Assert.That(foodGroupTree.DataProvider, Is.Not.Null);
            }
        }

        /// <summary>
        /// Tests that FoodGroupImportFromDataProvider imports food groups from the data provider.
        /// </summary>
        [Test]
        public void TestThatFoodGroupImportFromDataProviderImportsFoodGroups()
        {
            foreach (var command in TestHelper.FoodGroupImportFromDataProviderCommands)
            {
                var result = _foodWasteSystemDataService.FoodGroupImportFromDataProvider(command);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Identifier, Is.Not.EqualTo(default(Guid)));
                Assert.That(result.Identifier, Is.Not.EqualTo(Guid.Empty));
                Assert.That(result.EventDate, Is.EqualTo(DateTime.Now).Within(5).Seconds);
            }
        }

        /// <summary>
        /// Tests that DataProviderGetAll gets all the data providers.
        /// </summary>
        [Test]
        public void TestThatDataProviderGetAllGetsDataProviderSystemViewCollection()
        {
            DataProviderCollectionGetQuery dataProviderCollectionGetQuery = new DataProviderCollectionGetQuery();
            IList<DataProviderSystemView> dataProviderSystemViewCollection = new List<DataProviderSystemView>(_foodWasteSystemDataService.DataProviderGetAll(dataProviderCollectionGetQuery));
            Assert.That(dataProviderSystemViewCollection, Is.Not.Null);
            Assert.That(dataProviderSystemViewCollection, Is.Not.Empty);
        }

        /// <summary>
        /// Tests the commands acting on translations.
        /// </summary>
        [Test]
        public void TestTranslationCommands()
        {
            IList<TranslationInfoSystemView> translationInfoCollection = GetTranslationInfoCollection();
            Assert.That(translationInfoCollection, Is.Not.Null);
            Assert.That(translationInfoCollection, Is.Not.Empty);

            var translationAddCommand = new TranslationAddCommand
            {
                TranslationOfIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = translationInfoCollection.First().TranslationInfoIdentifier,
                TranslationValue = "Test"
            };
            var translationAddResult = _foodWasteSystemDataService.TranslationAdd(translationAddCommand);
            try
            {
                var translationModifyCommand = new TranslationModifyCommand
                {
                    TranslationIdentifier = translationAddResult.Identifier ?? Guid.Empty,
                    TranslationValue = "Test, test"
                };
                _foodWasteSystemDataService.TranslationModify(translationModifyCommand);
            }
            finally
            {
                var translationDeleteCommand = new TranslationDeleteCommand
                {
                    TranslationIdentifier = translationAddResult.Identifier ?? Guid.Empty
                };
                _foodWasteSystemDataService.TranslationDelete(translationDeleteCommand);
            }
        }

        /// <summary>
        /// Tests that StaticTextGetAll gets all the static texts.
        /// </summary>
        [Test]
        public void TestThatStaticTextGetAllGetsStaticTextSystemViewCollection()
        {
            IList<TranslationInfoSystemView> translationInfoCollection = GetTranslationInfoCollection();
            Assert.That(translationInfoCollection, Is.Not.Null);
            Assert.That(translationInfoCollection, Is.Not.Empty);

            foreach (var translationInfoSystemView in translationInfoCollection)
            {
                StaticTextCollectionGetQuery staticTextCollectionGetQuery = new StaticTextCollectionGetQuery
                {
                    TranslationInfoIdentifier = translationInfoSystemView.TranslationInfoIdentifier
                };
                IList<StaticTextSystemView> staticTextSystemViewCollection = new List<StaticTextSystemView>(_foodWasteSystemDataService.StaticTextGetAll(staticTextCollectionGetQuery));
                Assert.That(staticTextSystemViewCollection, Is.Not.Null);
                Assert.That(staticTextSystemViewCollection, Is.Not.Empty);
                Assert.That(staticTextSystemViewCollection.Count, Is.EqualTo(Enum.GetValues(typeof (StaticTextType)).Cast<StaticTextType>().Count()));
            }
        }

        /// <summary>
        /// Tests that TranslationInfoGetAll gets all the tranlation informations.
        /// </summary>
        [Test]
        public void TestThatTranslationInfoGetAllGetsTranslationInfoSystemViewCollection()
        {
            TranslationInfoCollectionGetQuery translationInfoCollectionGetQuery = new TranslationInfoCollectionGetQuery();
            IList<TranslationInfoSystemView> translationInfoSystemViewCollection = new List<TranslationInfoSystemView>(_foodWasteSystemDataService.TranslationInfoGetAll(translationInfoCollectionGetQuery));
            Assert.That(translationInfoSystemViewCollection, Is.Not.Null);
            Assert.That(translationInfoSystemViewCollection, Is.Not.Empty);
        }

        /// <summary>
        /// Gets all the translatios informations.
        /// </summary>
        /// <returns>Colleciton of all the translatios informations.</returns>
        private IList<TranslationInfoSystemView> GetTranslationInfoCollection()
        {
            TranslationInfoCollectionGetQuery translationInfoCollectionGetQuery = new TranslationInfoCollectionGetQuery();
            return new List<TranslationInfoSystemView>(_foodWasteSystemDataService.TranslationInfoGetAll(translationInfoCollectionGetQuery));
        }
    }
}
