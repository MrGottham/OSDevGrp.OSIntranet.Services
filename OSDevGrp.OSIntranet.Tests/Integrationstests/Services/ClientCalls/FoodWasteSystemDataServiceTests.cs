using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf;
using OSDevGrp.OSIntranet.Contracts.Commands;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Services;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;

namespace OSDevGrp.OSIntranet.Tests.Integrationstests.Services.ClientCalls
{
    /// <summary>
    /// Integration tests the service which can access and modify system data in the food waste domain with client calls.
    /// </summary>
    [TestFixture]
    [Category("Integrationstest")]
    public class FoodWasteSystemDataServiceTests
    {
        #region Private constants

        private const string ClientEndpointName = "FoodWasteSystemDataServiceIntegrationstest";

        #endregion

        #region Private variables

        private ChannelFactory<IFoodWasteSystemDataService> _channelFactory;

        #endregion

        /// <summary>
        /// Opsætning af test.
        /// </summary>
        [SetUp]
        public void TestSetUp()
        {
            _channelFactory = new ChannelFactory<IFoodWasteSystemDataService>(ClientEndpointName);
            if (_channelFactory.Credentials == null)
            {
                return;
            }
            _channelFactory.Credentials.UserName.UserName = "mrgottham@gmail.com";
        }

        /// <summary>
        /// Tests that FoodItemCollectionGet gets the collection of food items.
        /// </summary>
        [Test]
        public void TestThatFoodItemCollectionGetGetsFoodItemCollection()
        {
            var client = _channelFactory.CreateChannel();
            try
            {
                IList<TranslationInfoSystemView> translationInfoCollection = GetTranslationInfoCollection(client);
                Assert.That(translationInfoCollection, Is.Not.Null);
                Assert.That(translationInfoCollection, Is.Not.Empty);

                foreach (var translationInfo in translationInfoCollection)
                {
                    var foodGroupTreeGetQuery = new FoodGroupTreeGetQuery
                    {
                        TranslationInfoIdentifier = translationInfo.TranslationInfoIdentifier
                    };
                    var foodGroupTree = client.FoodGroupTreeGet(foodGroupTreeGetQuery);
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
                        var foodItemCollection = client.FoodItemCollectionGet(foodItemCollectionGetQuery);
                        Assert.That(foodItemCollection, Is.Not.Null);
                        Assert.That(foodItemCollection.FoodItems, Is.Not.Null);
                        Assert.That(foodItemCollection.FoodItems.Count(), Is.GreaterThanOrEqualTo(0));
                        Assert.That(foodItemCollection.DataProvider, Is.Not.Null);
                    }
                }
            }
            finally
            {
                ChannelTools.CloseChannel(client);
            }
        }

        /// <summary>
        /// Tests that FoodItemImportFromDataProvider imports food items from the data provider.
        /// </summary>
        [Test]
        public void TestThatFoodItemImportFromDataProviderImportsFoodItems()
        {
            var client = _channelFactory.CreateChannel();
            try
            {
                IList<TranslationInfoSystemView> translationInfoCollection = GetTranslationInfoCollection(client);
                Assert.That(translationInfoCollection, Is.Not.Null);
                Assert.That(translationInfoCollection, Is.Not.Empty);

                var foodGroupTree = client.FoodGroupTreeGet(new FoodGroupTreeGetQuery { TranslationInfoIdentifier = translationInfoCollection.First().TranslationInfoIdentifier });
                Assert.That(foodGroupTree, Is.Not.Null);

                foreach (var command in TestHelpers.GetFoodItemImportFromDataProviderCommands(foodGroupTree, translationInfoCollection))
                {
                    var result = client.FoodItemImportFromDataProvider(command);
                    Assert.That(result, Is.Not.Null);
                    Assert.That(result.Identifier, Is.Not.EqualTo(default(Guid)));
                    Assert.That(result.Identifier, Is.Not.EqualTo(Guid.Empty));
                    Assert.That(result.EventDate, Is.EqualTo(DateTime.Now).Within(5).Seconds);
                }
            }
            finally
            {
                ChannelTools.CloseChannel(client);
            }
        }

        /// <summary>
        /// Tests that FoodGroupTreeGet gets the tree of food groups.
        /// </summary>
        [Test]
        public void TestThatFoodGroupTreeGetGetsFoodGroupTree()
        {
            var client = _channelFactory.CreateChannel();
            try
            {
                var translationInfoCollection = client.TranslationInfoGetAll(new TranslationInfoCollectionGetQuery());
                Assert.That(translationInfoCollection, Is.Not.Null);
                Assert.That(translationInfoCollection, Is.Not.Empty);

                foreach (var translationInfo in translationInfoCollection)
                {
                    var query = new FoodGroupTreeGetQuery
                    {
                        TranslationInfoIdentifier = translationInfo.TranslationInfoIdentifier
                    };
                    var foodGroupTree = client.FoodGroupTreeGet(query);
                    Assert.That(foodGroupTree, Is.Not.Null);
                    Assert.That(foodGroupTree.FoodGroups, Is.Not.Null);
                    Assert.That(foodGroupTree.FoodGroups.Count(), Is.GreaterThanOrEqualTo(0));
                    Assert.That(foodGroupTree.DataProvider, Is.Not.Null);
                }
            }
            finally
            {
                ChannelTools.CloseChannel(client);
            }
        }

        /// <summary>
        /// Tests that FoodGroupImportFromDataProvider imports food groups from the data provider.
        /// </summary>
        [Test]
        public void TestThatFoodGroupImportFromDataProviderImportsFoodGroups()
        {
            var client = _channelFactory.CreateChannel();
            try
            {
                foreach (var command in TestHelpers.FoodGroupImportFromDataProviderCommands)
                {
                    var result = client.FoodGroupImportFromDataProvider(command);
                    Assert.That(result, Is.Not.Null);
                    Assert.That(result.Identifier, Is.Not.EqualTo(default(Guid)));
                    Assert.That(result.Identifier, Is.Not.EqualTo(Guid.Empty));
                    Assert.That(result.EventDate, Is.EqualTo(DateTime.Now).Within(5).Seconds);
                }
            }
            finally
            {
                ChannelTools.CloseChannel(client);
            }
        }

        /// <summary>
        /// Tests that DataProviderGetAll gets all the data providers.
        /// </summary>
        [Test]
        public void TestThatDataProviderGetAllGetsDataProviderSystemViewCollection()
        {
            var client = _channelFactory.CreateChannel();
            try
            {
                var dataProviderCollectionGetQuery = new DataProviderCollectionGetQuery();
                var dataProviderSystemViewCollection = client.DataProviderGetAll(dataProviderCollectionGetQuery);
                Assert.That(dataProviderSystemViewCollection, Is.Not.Null);
                Assert.That(dataProviderSystemViewCollection, Is.Not.Empty);
            }
            finally
            {
                ChannelTools.CloseChannel(client);
            }
        }

        /// <summary>
        /// Tests the commands acting on translations.
        /// </summary>
        [Test]
        public void TestTranslationCommands()
        {
            var client = _channelFactory.CreateChannel();
            try
            {
                var translationInfoCollectionGetQuery = new TranslationInfoCollectionGetQuery();
                var translationInfoSystemViewCollection = client.TranslationInfoGetAll(translationInfoCollectionGetQuery);
                Assert.That(translationInfoSystemViewCollection, Is.Not.Null);
                Assert.That(translationInfoSystemViewCollection, Is.Not.Empty);

                var translationAddCommand = new TranslationAddCommand
                {
                    TranslationOfIdentifier = Guid.NewGuid(),
                    TranslationInfoIdentifier = translationInfoSystemViewCollection.First().TranslationInfoIdentifier,
                    TranslationValue = "Test"
                };
                var translationAddResult = client.TranslationAdd(translationAddCommand);
                try
                {
                    var translationModifyCommand = new TranslationModifyCommand
                    {
                        TranslationIdentifier = translationAddResult.Identifier ?? Guid.Empty,
                        TranslationValue = "Test, test"
                    };
                    client.TranslationModify(translationModifyCommand);
                }
                finally
                {
                    var translationDeleteCommand = new TranslationDeleteCommand
                    {
                        TranslationIdentifier = translationAddResult.Identifier ?? Guid.Empty
                    };
                    client.TranslationDelete(translationDeleteCommand);
                }
            }
            finally
            {
                ChannelTools.CloseChannel(client);
            }
        }

        /// <summary>
        /// Tests that StaticTextGetAll gets all the static texts.
        /// </summary>
        [Test]
        public void TestThatStaticTextGetAllGetsStaticTextSystemViewCollection()
        {
            var client = _channelFactory.CreateChannel();
            try
            {
                var translationInfoCollectionGetQuery = new TranslationInfoCollectionGetQuery();
                var translationInfoSystemViewCollection = client.TranslationInfoGetAll(translationInfoCollectionGetQuery);
                Assert.That(translationInfoSystemViewCollection, Is.Not.Null);
                Assert.That(translationInfoSystemViewCollection, Is.Not.Empty);

                foreach (var translationInfoSystemView in translationInfoSystemViewCollection)
                {
                    var staticTextCollectionGetQuery = new StaticTextCollectionGetQuery
                    {
                        TranslationInfoIdentifier = translationInfoSystemView.TranslationInfoIdentifier
                    };
                    var staticTextSystemViewCollection = client.StaticTextGetAll(staticTextCollectionGetQuery);
                    Assert.That(staticTextSystemViewCollection, Is.Not.Null);
                    Assert.That(staticTextSystemViewCollection, Is.Not.Empty);
                    Assert.That(staticTextSystemViewCollection.Count(), Is.EqualTo(Enum.GetValues(typeof (StaticTextType)).Cast<StaticTextType>().Count()));
                }
            }
            finally
            {
                ChannelTools.CloseChannel(client);
            }
        }

        /// <summary>
        /// Tests that TranslationInfoGetAll gets all the tranlation informations.
        /// </summary>
        [Test]
        public void TestThatTranslationInfoGetAllGetsTranslationInfoSystemViewCollection()
        {
            var client = _channelFactory.CreateChannel();
            try
            {
                var translationInfoCollectionGetQuery = new TranslationInfoCollectionGetQuery();
                var translationInfoSystemViewCollection = client.TranslationInfoGetAll(translationInfoCollectionGetQuery);
                Assert.That(translationInfoSystemViewCollection, Is.Not.Null);
                Assert.That(translationInfoSystemViewCollection, Is.Not.Empty);
            }
            finally
            {
                ChannelTools.CloseChannel(client);
            }
        }

        /// <summary>
        /// Gets all the translatios informations.
        /// </summary>
        /// <param name="client">The client to the service which can access and modify system data in the food waste domain.</param>
        /// <returns>Colleciton of all the translatios informations.</returns>
        private IList<TranslationInfoSystemView> GetTranslationInfoCollection(IFoodWasteSystemDataService client)
        {
            TranslationInfoCollectionGetQuery translationInfoCollectionGetQuery = new TranslationInfoCollectionGetQuery();
            return new List<TranslationInfoSystemView>(client.TranslationInfoGetAll(translationInfoCollectionGetQuery));
        }
    }
}
