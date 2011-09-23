using System;
using System.ServiceModel;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf;
using OSDevGrp.OSIntranet.Services.Implementations;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Services
{
    /// <summary>
    /// Tester service til adressekartotek.
    /// </summary>
    [TestFixture]
    public class AdressekartotekServiceTests
    {
        /// <summary>
        /// Tester, at service til adressekartotek kan hostes.
        /// </summary>
        [Test]
        public void TestAtAdressekartotekServiceKanHostes()
        {
            var uri = new Uri("http://localhost:7000/OSIntranet/");
            var host = new ServiceHost(typeof(AdressekartotekService), new[] { uri });
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
