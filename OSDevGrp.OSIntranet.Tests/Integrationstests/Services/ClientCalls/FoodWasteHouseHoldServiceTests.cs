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
