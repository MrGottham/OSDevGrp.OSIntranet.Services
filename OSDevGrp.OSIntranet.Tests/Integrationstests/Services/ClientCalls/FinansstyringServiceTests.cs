using System;
using System.Linq;
using System.ServiceModel;
using OSDevGrp.OSIntranet.CommonLibrary.IoC;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf.ChannelFactory;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Services;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Integrationstests.Services.ClientCalls
{
    /// <summary>
    /// Integrationstester servicen til finansstyring med kald fra klienter.
    /// </summary>
    [TestFixture]
    [Category("Integrationstest")]
    public class FinansstyringServiceTests
    {
        #region Private constants

        private const string ClientEndpointName = "FinansstyringServiceIntegrationstest";

        #endregion

        #region Private variables

        private readonly Uri _serviceUri = new Uri("http://localhost:7000/OSIntranet/");
        //private ServiceHost _serviceHost;

        private IChannelFactory _channelFactory;

        #endregion

        /// <summary>
        /// Opsætning af test.
        /// </summary>
        [TestFixtureSetUp]
        public void TestSetUp()
        {
            /*
            _serviceHost = new ServiceHost(typeof (FinansstyringService), new[] {_serviceUri});
            try
            {
                _serviceHost.Open();
                Assert.That(_serviceHost.State, Is.EqualTo(CommunicationState.Opened));
            }
            finally
            {
                ChannelTools.CloseChannel(_serviceHost);
            }
            */
            var container = ContainerFactory.Create();
            _channelFactory = container.Resolve<IChannelFactory>();
        }

        /// <summary>
        /// Tester, at en regnskabsliste kan hentes.
        /// </summary>
        [Test]
        public void TestAtRegnskabslisteHentes()
        {
            var client = _channelFactory.CreateChannel<IFinansstyringService>(ClientEndpointName);
            try
            {
                var query = new RegnskabslisteGetQuery();
                var result = client.RegnskabslisteGet(query);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count(), Is.GreaterThan(0));
            }
            finally
            {
                ChannelTools.CloseChannel(client);
            }
        }
    }
}
