using System;
using System.Linq;
using System.ServiceModel.Security;
using Microsoft.IdentityModel.SecurityTokenService;
using Microsoft.IdentityModel.Tokens;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Security.Configuration;
using OSDevGrp.OSIntranet.Security.Core;
using OSDevGrp.OSIntranet.Security.Services;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Security.Services
{
    /// <summary>
    /// Tests the configuration for the basic security token service.
    /// </summary>
    [TestFixture]
    public class BasicSecurityTokenServiceConfigurationTests
    {
        /// <summary>
        /// Tests that the constructor initialize configuration for the basic security token service.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeBasicSecurityTokenServiceConfiguration()
        {
            var basicSecurityTokenServiceConfiguration = new BasicSecurityTokenServiceConfiguration();
            Assert.That(basicSecurityTokenServiceConfiguration, Is.Not.Null);
            Assert.That(basicSecurityTokenServiceConfiguration.DisableWsdl, Is.True);
            Assert.That(basicSecurityTokenServiceConfiguration.SaveBootstrapTokens, Is.True);
            Assert.That(basicSecurityTokenServiceConfiguration.TokenIssuerName, Is.Not.Null);
            Assert.That(basicSecurityTokenServiceConfiguration.TokenIssuerName, Is.Not.Empty);
            Assert.That(basicSecurityTokenServiceConfiguration.TokenIssuerName, Is.EqualTo(ConfigurationProvider.Instance.IssuerTokenName.Uri.AbsoluteUri));
            Assert.That(basicSecurityTokenServiceConfiguration.ServiceCertificate, Is.Not.Null);
            Assert.That(basicSecurityTokenServiceConfiguration.ServiceCertificate.Subject, Is.Not.Null);
            Assert.That(basicSecurityTokenServiceConfiguration.ServiceCertificate.Subject, Is.Not.Empty);
            Assert.That(basicSecurityTokenServiceConfiguration.ServiceCertificate.Subject, Is.EqualTo("CN=OSDevGrp.OSIntranet.Services"));
            Assert.That(basicSecurityTokenServiceConfiguration.SigningCredentials, Is.Not.Null);
            Assert.That(basicSecurityTokenServiceConfiguration.SigningCredentials, Is.TypeOf<X509SigningCredentials>());
            Assert.That(basicSecurityTokenServiceConfiguration.CertificateValidationMode, Is.EqualTo(X509CertificateValidationMode.None));
            Assert.That(basicSecurityTokenServiceConfiguration.IssuerNameRegistry, Is.Not.Null);
            Assert.That(basicSecurityTokenServiceConfiguration.IssuerNameRegistry, Is.TypeOf<ConfigurationBasedIssuerNameRegistry>());
            Assert.That(basicSecurityTokenServiceConfiguration.SecurityTokenHandlers, Is.Not.Null);
            Assert.That(basicSecurityTokenServiceConfiguration.SecurityTokenHandlers, Is.Not.Empty);
            Assert.That(basicSecurityTokenServiceConfiguration.SecurityTokenService, Is.Not.Null);
            Assert.That(basicSecurityTokenServiceConfiguration.SecurityTokenService.Name, Is.Not.Null);
            Assert.That(basicSecurityTokenServiceConfiguration.SecurityTokenService.Name, Is.Not.Empty);
            Assert.That(basicSecurityTokenServiceConfiguration.SecurityTokenService.Name, Is.EqualTo(typeof (BasicSecurityTokenService).Name));

            var configurationBasedIssuerNameRegistry = (ConfigurationBasedIssuerNameRegistry) basicSecurityTokenServiceConfiguration.IssuerNameRegistry;
            Assert.That(configurationBasedIssuerNameRegistry.ConfiguredTrustedIssuers, Is.Not.Null);
            Assert.That(configurationBasedIssuerNameRegistry.ConfiguredTrustedIssuers, Is.Not.Empty);
            Assert.That(configurationBasedIssuerNameRegistry.ConfiguredTrustedIssuers.Count, Is.EqualTo(1));
            Assert.That(configurationBasedIssuerNameRegistry.ConfiguredTrustedIssuers.FirstOrDefault(m => string.Compare(m.Key, "3e07c011e0d76365af1391a3be7cda82eaf81d5a", StringComparison.Ordinal) == 0), Is.Not.Null);
            Assert.That(configurationBasedIssuerNameRegistry.ConfiguredTrustedIssuers["3e07c011e0d76365af1391a3be7cda82eaf81d5a"], Is.Not.Null);
            Assert.That(configurationBasedIssuerNameRegistry.ConfiguredTrustedIssuers["3e07c011e0d76365af1391a3be7cda82eaf81d5a"], Is.Not.Empty);
            Assert.That(configurationBasedIssuerNameRegistry.ConfiguredTrustedIssuers["3e07c011e0d76365af1391a3be7cda82eaf81d5a"], Is.EqualTo("CN=OSDevGrp.OSIntranet"));

            var userNameSecurityTokenHandler = basicSecurityTokenServiceConfiguration.SecurityTokenHandlers.OfType<UserNameSecurityTokenHandler>().Single();
            Assert.That(userNameSecurityTokenHandler, Is.Not.Null);
            Assert.That(userNameSecurityTokenHandler, Is.TypeOf<UserNameAsMailAddressSecurityTokenHandler>());
        }
    }
}
