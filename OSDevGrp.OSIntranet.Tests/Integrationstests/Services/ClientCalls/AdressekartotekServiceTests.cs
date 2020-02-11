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
    /// Integrationstester servicen til adressekartotek med kald fra klienter.
    /// </summary>
    [TestFixture]
    [Category("Integrationstest")]
    public class AdressekartotekServiceTests
    {
        #region Private constants

        private const string ClientEndpointName = "AdressekartotekServiceIntegrationstest";

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
        /// Tester, at telefonliste hentes.
        /// </summary>
        [Test]
        public void TestAtTelefonlisteHentes()
        {
            var client = _channelFactory.CreateChannel<IAdressekartotekService>(ClientEndpointName);
            try
            {
                var query = new TelefonlisteGetQuery();
                var result = client.TelefonlisteGet(query);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count(), Is.GreaterThan(0));
            }
            finally
            {
                ChannelTools.CloseChannel(client);
            }
        }

        /// <summary>
        /// Tester, at personliste hentes.
        /// </summary>
        [Test]
        public void TestAtPersonlisteHentes()
        {
            var client = _channelFactory.CreateChannel<IAdressekartotekService>(ClientEndpointName);
            try
            {
                var query = new PersonlisteGetQuery();
                var result = client.PersonlisteGet(query);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count(), Is.GreaterThan(0));
            }
            finally
            {
                ChannelTools.CloseChannel(client);
            }
        }

        /// <summary>
        /// Tester, at en person hentes.
        /// </summary>
        [Test]
        public void TestAtPersonHentes()
        {
            var client = _channelFactory.CreateChannel<IAdressekartotekService>(ClientEndpointName);
            try
            {
                var query = new PersonGetQuery
                                {
                                    Nummer = 1
                                };
                var result = client.PersonGet(query);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Nummer, Is.EqualTo(query.Nummer));
            }
            finally
            {
                ChannelTools.CloseChannel(client);
            }
        }

        /// <summary>
        /// Tester, at firmaliste hentes.
        /// </summary>
        [Test]
        public void TestAtFirmalisteHentes()
        {
            var client = _channelFactory.CreateChannel<IAdressekartotekService>(ClientEndpointName);
            try
            {
                var query = new FirmalisteGetQuery();
                var result = client.FirmalisteGet(query);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count(), Is.GreaterThan(0));
            }
            finally
            {
                ChannelTools.CloseChannel(client);
            }
        }

        /// <summary>
        /// Tester, at et firma hentes.
        /// </summary>
        [Test]
        public void TestAtFirmaHentes()
        {
            var client = _channelFactory.CreateChannel<IAdressekartotekService>(ClientEndpointName);
            try
            {
                var query = new FirmaGetQuery
                                {
                                    Nummer = 48
                                };
                var result = client.FirmaGet(query);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Nummer, Is.EqualTo(query.Nummer));
            }
            finally
            {
                ChannelTools.CloseChannel(client);
            }
        }

        /// <summary>
        /// Tester, at adressegrupper hentes.
        /// </summary>
        [Test]
        public void TestAtAdressegrupperHentes()
        {
            var client = _channelFactory.CreateChannel<IAdressekartotekService>(ClientEndpointName);
            try
            {
                var query = new AdressegrupperGetQuery();
                var result = client.AdressegrupperGet(query);
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
