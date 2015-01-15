using System;
using System.ServiceModel;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Services.Implementations;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Services
{
    /// <summary>
    /// Tests the service which can access and modify system data in the food waste domain.
    /// </summary>
    [TestFixture]
    public class FoodWasteSystemDataServiceTests
    {
        /// <summary>
        /// Tests that service which can access and modify system data in the food waste domain can be hosted.
        /// </summary>
        [Test]
        public void TestThatFoodWasteSystemDataServiceCanBeHosted()
        {
            var uri = new Uri("http://localhost:7000/OSIntranet/");
            var host = new ServiceHost(typeof (FoodWasteSystemDataService), new[] {uri});
            try
            {
                host.Open();
                Assert.That(host.State, Is.EqualTo(CommunicationState.Opened));
            }
            finally
            {
                ChannelTools.CloseChannel(host);
            }
        }
    }
}
