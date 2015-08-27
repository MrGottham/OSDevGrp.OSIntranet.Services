using System;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommonLibrary.IoC;
using OSDevGrp.OSIntranet.Contracts.Commands;
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
        /// Tests that FoodGroupImportFromDataProvider imports food groups from the data provider.
        /// </summary>
        [Test]
        public void TestThatFoodGroupImportFromDataProviderImportsFoodGroups()
        {
            foreach (var command in TestHelpers.FoodGroupImportFromDataProviderCommands)
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
            var dataProviderCollectionGetQuery = new DataProviderCollectionGetQuery();
            var dataProviderSystemViewCollection = _foodWasteSystemDataService.DataProviderGetAll(dataProviderCollectionGetQuery);
            Assert.That(dataProviderSystemViewCollection, Is.Not.Null);
            Assert.That(dataProviderSystemViewCollection, Is.Not.Empty);
        }

        /// <summary>
        /// Tests the commands acting on translations.
        /// </summary>
        [Test]
        public void TestTranslationCommands()
        {
            var translationInfoCollectionGetQuery = new TranslationInfoCollectionGetQuery();
            var translationInfoSystemViewCollection = _foodWasteSystemDataService.TranslationInfoGetAll(translationInfoCollectionGetQuery);
            Assert.That(translationInfoSystemViewCollection, Is.Not.Null);
            Assert.That(translationInfoSystemViewCollection, Is.Not.Empty);

            var translationAddCommand = new TranslationAddCommand
            {
                TranslationOfIdentifier = Guid.NewGuid(),
                TranslationInfoIdentifier = translationInfoSystemViewCollection.First().TranslationInfoIdentifier,
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
