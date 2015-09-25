using System.Linq;
using System.ServiceModel;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf;
using OSDevGrp.OSIntranet.Contracts.Services;
using OSDevGrp.OSIntranet.Contracts.Queries;

namespace OSDevGrp.OSIntranet.Tests.Integrationstests.Services.ClientCalls
{
    /// <summary>
    /// Integration tests the service which can access and modify data on a house hold in the food waste domain.
    /// </summary>
    [TestFixture]
    [Category("Integrationstest")]
    public class FoodWasteHouseHoldServiceTests
    {
        #region Private constants

        private const string ClientEndpointName = "FoodWasteHouseHoldServiceIntegrationstest";

        #endregion

        #region Private variables

        private ChannelFactory<IFoodWasteHouseHoldService> _channelFactory;

        #endregion

        /// <summary>
        /// Opsætning af test.
        /// </summary>
        [TestFixtureSetUp]
        public void TestSetUp()
        {
            _channelFactory = new ChannelFactory<IFoodWasteHouseHoldService>(ClientEndpointName);
            if (_channelFactory.Credentials == null)
            {
                return;
            }
            _channelFactory.Credentials.UserName.UserName = "ole.sorensen@osdevgrp.dk";
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
