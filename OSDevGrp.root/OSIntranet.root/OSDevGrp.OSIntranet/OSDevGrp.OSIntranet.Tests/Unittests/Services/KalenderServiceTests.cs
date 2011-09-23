using System;
using System.ServiceModel;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf;
using OSDevGrp.OSIntranet.Services.Implementations;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Services
{
    /// <summary>
    /// Tester service til fælles kalender.
    /// </summary>
    [TestFixture]
    public class KalenderServiceTests
    {
        /// <summary>
        /// Tester, at service til kalender kan hostes.
        /// </summary>
        [Test]
        public void TestAtKalenderServiceKanHostes()
        {
            var uri = new Uri("http://localhost:7000/OSIntranet/");
            var host = new ServiceHost(typeof(KalenderService), new[] { uri });
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
