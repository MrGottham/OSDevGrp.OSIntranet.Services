using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.IoC;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf.ChannelFactory;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Services;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Integrationstests.Services.ClientCalls
{
    /// <summary>
    /// Integrationstester servicen til fælles elementer med kald fra klienter.
    /// </summary>
    [TestFixture]
    [Category("Integrationstest")]
    public class CommonServiceTests
    {
        #region Private constants

        private const string ClientEndpointName = "CommonServiceIntegrationstest";

        #endregion

        #region Private variables

        private IChannelFactory _channelFactory;

        #endregion

        /// <summary>
        /// Opsætning af test.
        /// </summary>
        [SetUp]
        public void TestSetUp()
        {
            var container = ContainerFactory.Create();
            _channelFactory = container.Resolve<IChannelFactory>();
        }

        /// <summary>
        /// Tester, at brevhoveder hentes.
        /// </summary>
        [Test]
        public void TestAtBrevhovederHentes()
        {
            var client = _channelFactory.CreateChannel<ICommonService>(ClientEndpointName);
            try
            {
                var query = new BrevhovederGetQuery();
                var result = client.BrevhovederGet(query);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count(), Is.GreaterThan(0));
            }
            finally
            {
                ChannelTools.CloseChannel(client);
            }
        }

        /// <summary>
        /// Tester, at systemer under OSWEBDB hentes.
        /// </summary>
        [Test]
        public void TestAtSystemerHentes()
        {
            var client = _channelFactory.CreateChannel<ICommonService>(ClientEndpointName);
            try
            {
                var query = new SystemerGetQuery();
                var result = client.SystemerGet(query);
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
