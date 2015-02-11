using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.SecurityTokenService;
using Microsoft.IdentityModel.Tokens;
using OSDevGrp.OSIntranet.Security.Configuration;
using OSDevGrp.OSIntranet.Security.Core;

namespace OSDevGrp.OSIntranet.Security.Services
{
    /// <summary>
    /// Implementation of configuration for the basic security token service.
    /// </summary>
    public class BasicSecurityTokenServiceConfiguration : SecurityTokenServiceConfiguration
    {
        #region Constructor

        /// <summary>
        /// Creates configuration for the basic security token service.
        /// </summary>
        public BasicSecurityTokenServiceConfiguration() :
            base(ConfigurationProvider.Instance.IssuerTokenName.Uri.AbsoluteUri)
        {
            DisableWsdl = true;
            SaveBootstrapTokens = true;
            TokenIssuerName = ConfigurationProvider.Instance.IssuerTokenName.Uri.AbsoluteUri;
            SigningCredentials = new X509SigningCredentials(CertificateHelper.GetCertificate(StoreName.My, StoreLocation.LocalMachine, ConfigurationProvider.Instance.SigningCertificate.SubjetName));

            var userNameSecurityTokenHandler = SecurityTokenHandlers.OfType<UserNameSecurityTokenHandler>().FirstOrDefault();
            while (userNameSecurityTokenHandler != null)
            {
                SecurityTokenHandlers.Remove(userNameSecurityTokenHandler);
                userNameSecurityTokenHandler = SecurityTokenHandlers.OfType<UserNameSecurityTokenHandler>().FirstOrDefault();
            }
            SecurityTokenHandlers.Add(new UserNameAsMailAddressSecurityTokenHandler());
            
            SecurityTokenService = typeof (BasicSecurityTokenService);
        }
        
        #endregion
    }
}
