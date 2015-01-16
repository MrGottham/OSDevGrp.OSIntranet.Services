using Microsoft.IdentityModel.SecurityTokenService;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Security.Configuration;
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
            Assert.That(basicSecurityTokenServiceConfiguration.TokenIssuerName, Is.Not.Null);
            Assert.That(basicSecurityTokenServiceConfiguration.TokenIssuerName, Is.Not.Empty);
            Assert.That(basicSecurityTokenServiceConfiguration.TokenIssuerName, Is.EqualTo(ConfigurationProvider.Instance.IssuerTokenName.Value));
            Assert.That(basicSecurityTokenServiceConfiguration.SigningCredentials, Is.Not.Null);
            Assert.That(basicSecurityTokenServiceConfiguration.SigningCredentials, Is.TypeOf<X509SigningCredentials>());
            Assert.That(basicSecurityTokenServiceConfiguration.SecurityTokenService, Is.Not.Null);
            Assert.That(basicSecurityTokenServiceConfiguration.SecurityTokenService.Name, Is.Not.Null);
            Assert.That(basicSecurityTokenServiceConfiguration.SecurityTokenService.Name, Is.Not.Empty);
            Assert.That(basicSecurityTokenServiceConfiguration.SecurityTokenService.Name, Is.EqualTo(typeof (BasicSecurityTokenService).Name));
        }
    }
}
