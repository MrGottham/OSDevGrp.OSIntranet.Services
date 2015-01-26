using NUnit.Framework;
using OSDevGrp.OSIntranet.CommonLibrary.IoC;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf.ChannelFactory;
using OSDevGrp.OSIntranet.Contracts.Services;
using OSDevGrp.OSIntranet.Contracts.Queries;

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

        private IChannelFactory _channelFactory;

        #endregion

        /// <summary>
        /// Opsætning af test.
        /// </summary>
        [TestFixtureSetUp]
        public void TestSetUp()
        {
            var container = ContainerFactory.Create();
            _channelFactory = container.Resolve<IChannelFactory>();
        }

        /// <summary>
        /// Tests that TranslationInfoGetAll gets all the tranlation informations.
        /// </summary>
        [Test]
        public void TestThatTranslationInfoGetAllGetsTranslationInfoSystemViewCollection()
        {
            var client = _channelFactory.CreateChannel<IFoodWasteSystemDataService>(ClientEndpointName);
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
