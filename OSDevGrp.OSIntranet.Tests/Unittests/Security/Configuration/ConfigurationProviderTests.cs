using System;
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
            Assert.That(configurationProvider.IssuerTokenName.Address, Is.Not.Null);
            Assert.That(configurationProvider.IssuerTokenName.Address, Is.Not.Empty);
            Assert.That(configurationProvider.IssuerTokenName.Address, Is.EqualTo("http://localhost:8010/sts"));
            Assert.That(configurationProvider.IssuerTokenName.Uri, Is.Not.Null);
            Assert.That(configurationProvider.IssuerTokenName.Uri.AbsoluteUri, Is.Not.Null);
            Assert.That(configurationProvider.IssuerTokenName.Uri.AbsoluteUri, Is.Not.Null);
            Assert.That(configurationProvider.IssuerTokenName.Uri.AbsoluteUri, Is.EqualTo(new Uri("http://localhost:8010/sts")));
            Assert.That(configurationProvider.SigningCertificate, Is.Not.Null);
            Assert.That(configurationProvider.SigningCertificate.SubjetName, Is.Not.Null);
            Assert.That(configurationProvider.SigningCertificate.SubjetName, Is.Not.Empty);
            Assert.That(configurationProvider.SigningCertificate.SubjetName, Is.EqualTo("CN=OSDevGrp.OSIntranet.Tokens"));
            Assert.That(configurationProvider.TrustedRelyingPartyCollection, Is.Not.Null);
            Assert.That(configurationProvider.TrustedRelyingPartyCollection, Is.Not.Empty);
            Assert.That(configurationProvider.TrustedRelyingPartyCollection.Count, Is.EqualTo(2));
            Assert.That(configurationProvider.ClaimCollection, Is.Not.Null);
            Assert.That(configurationProvider.ClaimCollection, Is.Not.Empty);
            Assert.That(configurationProvider.ClaimCollection.Count, Is.EqualTo(3));
        }
    }
}
