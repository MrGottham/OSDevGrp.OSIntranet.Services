using System;
using System.ServiceModel;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf;
using OSDevGrp.OSIntranet.Services.Implementations;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Services
{
    /// <summary>
    /// Tester service til fælles elementer.
    /// </summary>
    [TestFixture]
    public class CommonServiceTests
    {
        /// <summary>
        /// Tester, at service til fælles elementer kan hostes.
        /// </summary>
        [Test]
        public void TestAtCommonServiceKanHostes()
        {
            var uri = new Uri("http://localhost:7000/OSIntranet/");
            var host = new ServiceHost(typeof(CommonService), new[] { uri });
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
