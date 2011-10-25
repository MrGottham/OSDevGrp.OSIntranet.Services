using System;
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
    /// Integrationstester servicen til kalenderaftaler med kald fra klienter.
    /// </summary>
    [TestFixture]
    [Category("Integrationstest")]
    public class KalenderServiceTests
    {
        #region Private constants

        private const string ClientEndpointName = "KalenderServiceIntegrationstest";

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
        /// Tester, at kalenderaftaler til en given kalenderbruger hentes.
        /// </summary>
        [Test]
        public void TestAtKalenderaftalerHentes()
        {
            var client = _channelFactory.CreateChannel<IKalenderService>(ClientEndpointName);
            try
            {
                var query = new KalenderbrugerAftalerGetQuery
                                {
                                    System = 1,
                                    Initialer = "OS",
                                    FraDato = new DateTime(2010, 1, 1)
                                };
                var result = client.KalenderbrugerAftalerGet(query);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count(), Is.GreaterThan(0));
            }
            finally
            {
                ChannelTools.CloseChannel(client);
            }
        }

        /// <summary>
        /// Tester, at en given kalenderaftale til en given kalenderbruger hentes.
        /// </summary>
        [Test]
        public void TestAtKalenderaftaleHentes()
        {
            var client = _channelFactory.CreateChannel<IKalenderService>(ClientEndpointName);
            try
            {
                var query = new KalenderbrugerAftaleGetQuery
                                {
                                    System = 1,
                                    AftaleId = 1,
                                    Initialer = "OS"
                                };
                var result = client.KalenderbrugerAftaleGet(query);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Id, Is.EqualTo(query.AftaleId));
            }
            finally
            {
                ChannelTools.CloseChannel(client);
            }
        }

        /// <summary>
        /// Tester, at kalenderbrugere til et system under OSWEBDB hentes.
        /// </summary>
        [Test]
        public void TestAtKalenderbrugereHentes()
        {
            var client = _channelFactory.CreateChannel<IKalenderService>(ClientEndpointName);
            try
            {
                var query = new KalenderbrugereGetQuery
                                {
                                    System = 1
                                };
                var result = client.KalenderbrugereGet(query);
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
            var client = _channelFactory.CreateChannel<IKalenderService>(ClientEndpointName);
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
