using NUnit.Framework;
using OSDevGrp.OSIntranet.Security.Configuration;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Security.Configuration
{
    /// <summary>
    /// Tests the provider which can deliver configuration data for the basic security token service.
    /// </summary>
    [TestFixture]
    public class ConfigurationProviderTests
    {
        /// <summary>
        /// Tests that Instance returns an instance of the provider which can deliver configuration data for the basic security token service.
        /// </summary>
        [Test]
        public void TestThatInstanceReturnsConfigurationProvider()
        {
            var configurationProvider = ConfigurationProvider.Instance;
            Assert.That(configurationProvider, Is.Not.Null);
            Assert.That(configurationProvider.IssuerTokenName, Is.Not.Null);
            Assert.That(configurationProvider.IssuerTokenName.Value, Is.Not.Null);
            Assert.That(configurationProvider.IssuerTokenName.Value, Is.Not.Empty);
            Assert.That(configurationProvider.IssuerTokenName.Value, Is.EqualTo("http://localhost:8010/sts"));
            Assert.That(configurationProvider.SigningCertificate, Is.Not.Null);
            Assert.That(configurationProvider.SigningCertificate.Value, Is.Not.Null);
            Assert.That(configurationProvider.SigningCertificate.Value, Is.Not.Empty);
            Assert.That(configurationProvider.SigningCertificate.Value, Is.EqualTo("CN=OSDevGrp.OSIntranet.Services"));
            Assert.That(configurationProvider.TrustedRelyingPartyCollection, Is.Not.Null);
            Assert.That(configurationProvider.TrustedRelyingPartyCollection, Is.Not.Empty);
            Assert.That(configurationProvider.TrustedRelyingPartyCollection.Count, Is.EqualTo(2));
            Assert.That(configurationProvider.TrustedRelyingPartyCollection["localhost"], Is.Not.Null);
            Assert.That(configurationProvider.TrustedRelyingPartyCollection["localhost"].Value, Is.Not.Null);
            Assert.That(configurationProvider.TrustedRelyingPartyCollection["localhost"].Value, Is.Not.Empty);
            Assert.That(configurationProvider.TrustedRelyingPartyCollection["localhost"].Value, Is.EqualTo("http://localhost"));
            Assert.That(configurationProvider.TrustedRelyingPartyCollection["mother"], Is.Not.Null);
            Assert.That(configurationProvider.TrustedRelyingPartyCollection["mother"].Value, Is.Not.Null);
            Assert.That(configurationProvider.TrustedRelyingPartyCollection["mother"].Value, Is.Not.Empty);
            Assert.That(configurationProvider.TrustedRelyingPartyCollection["mother"].Value, Is.EqualTo("http://mother"));
        }
    }
}
