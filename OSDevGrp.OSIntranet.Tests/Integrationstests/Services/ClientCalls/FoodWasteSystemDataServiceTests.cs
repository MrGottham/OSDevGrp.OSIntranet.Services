using System.Linq;
using System.ServiceModel;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf;
using OSDevGrp.OSIntranet.Contracts.Services;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Commands;
using System;

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
        [TestFixtureSetUp]
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
    }
}
