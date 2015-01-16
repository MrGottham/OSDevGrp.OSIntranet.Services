using System.Configuration;

namespace OSDevGrp.OSIntranet.Security.Configuration
{
    /// <summary>
    /// Provider which can deliver configuration data for the basic security token service.
    /// </summary>
    public class ConfigurationProvider : ConfigurationSection
    {
        #region Private variables

        private static ConfigurationProvider _configurationProvider;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the issuer token name.
        /// </summary>
        [ConfigurationProperty("issuerTokenName", IsRequired = true)]
        public virtual UriConfigurationElement IssuerTokenName
        {
            get { return (UriConfigurationElement) this["issuerTokenName"]; }
        }

        /// <summary>
        /// Gets the subject name for the signing certificate.
        /// </summary>
        [ConfigurationProperty("signingCertificate", IsRequired = true)]
        public virtual CertificateConfigurationElement SigningCertificate
        {
            get { return (CertificateConfigurationElement) this["signingCertificate"]; }
        }

        /// <summary>
        /// Gets the URI's for the trusted relaying parties.
        /// </summary>
        [ConfigurationProperty("trustedRelyingPartyCollection", IsRequired = true)]
        public virtual UriConfigurationElementCollection TrustedRelyingPartyCollection
        {
            get { return (UriConfigurationElementCollection) this["trustedRelyingPartyCollection"]; }
        }

        /// <summary>
        /// Gets an instance of the provider which can deliver configuration data for the basic security token service.
        /// </summary>
        public static ConfigurationProvider Instance
        {
            get
            {
                return _configurationProvider ?? (_configurationProvider = (ConfigurationProvider) ConfigurationManager.GetSection("basicSecurityTokenServiceConfiguration"));
            }
        }

        #endregion
    }
}
