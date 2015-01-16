using System;
using System.Linq;
using System.ServiceModel;
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.SecurityTokenService;
using OSDevGrp.OSIntranet.Resources;
using OSDevGrp.OSIntranet.Security.Configuration;
using OSDevGrp.OSIntranet.Security.Helper;

namespace OSDevGrp.OSIntranet.Security.Services
{
    /// <summary>
    /// Implementation for a basic security token service.
    /// </summary>
    public class BasicSecurityTokenService : SecurityTokenService
    {
        #region Constructor

        /// <summary>
        /// Creates a basic security token service.
        /// </summary>
        /// <param name="securityTokenServiceConfiguration">Security token service configurationj.</param>
        public BasicSecurityTokenService(SecurityTokenServiceConfiguration securityTokenServiceConfiguration)
            : base(securityTokenServiceConfiguration)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// This method returns the configuration for the token issuance request. The configuration
        /// is represented by the Scope class.
        /// </summary>
        /// <param name="principal">The caller's principal.</param>
        /// <param name="request">The incoming RST.</param>
        /// <returns>The configuration for the token issuance request.</returns>
        protected override Scope GetScope(IClaimsPrincipal principal, RequestSecurityToken request)
        {
            if (principal == null)
            {
                var argumentNullException = new ArgumentNullException("principal");
                throw new InvalidRequestException(argumentNullException.Message, argumentNullException);
            }
            if (request == null)
            {
                var argumentNullException = new ArgumentNullException("request");
                throw new InvalidRequestException(argumentNullException.Message, argumentNullException);
            }
            var appliesTo = request.AppliesTo;
            if (appliesTo == null || appliesTo.Uri == null || string.IsNullOrEmpty(appliesTo.Uri.AbsoluteUri))
            {
                throw new InvalidRequestException(Resource.GetExceptionMessage(ExceptionMessage.AppliesToMustBeSuppliedInRequestSecurityToken));
            }
            try
            {
                return new Scope(appliesTo.Uri.AbsoluteUri)
                {
                    EncryptingCredentials = GetCredentialsForAppliesTo(request.AppliesTo),
                    SigningCredentials = new X509SigningCredentials(CertificateHelper.GetCertificate(StoreName.My, StoreLocation.LocalMachine, ConfigurationProvider.Instance.SigningCertificate.SubjetName))
                };
            }
            catch (InvalidRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidRequestException(ex.Message, ex);
            }
        }

        /// <summary>
        /// This method returns the claims to be included in the issued token.
        /// </summary>
        /// <param name="principal">The caller's principal.</param>
        /// <param name="request">The incoming RST.</param>
        /// <param name="scope">The scope that was previously returned by GetScope method.</param>
        /// <returns>The claims to be included in the issued token.</returns>
        protected override IClaimsIdentity GetOutputClaimsIdentity(IClaimsPrincipal principal, RequestSecurityToken request, Scope scope)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the credentials for 
        /// </summary>
        /// <param name="appliesTo"></param>
        /// <returns></returns>
        private static X509EncryptingCredentials GetCredentialsForAppliesTo(EndpointAddress appliesTo)
        {
            if (appliesTo == null || appliesTo.Uri == null || string.IsNullOrEmpty(appliesTo.Uri.AbsolutePath))
            {
                throw new InvalidRequestException(Resource.GetExceptionMessage(ExceptionMessage.AppliesToMustBeSuppliedInRequestSecurityToken));
            }
            if (ConfigurationProvider.Instance.TrustedRelyingPartyCollection.OfType<UriConfigurationElement>().Any(trustedRelyingParty => appliesTo.Uri.AbsoluteUri.StartsWith(trustedRelyingParty.Uri.AbsoluteUri)))
            {
                return new X509EncryptingCredentials(CertificateHelper.GetCertificate(StoreName.TrustedPeople, StoreLocation.LocalMachine, ConfigurationProvider.Instance.SigningCertificate.SubjetName));
            }
            throw new InvalidRequestException(Resource.GetExceptionMessage(ExceptionMessage.InvalidRelyingPartyAddress, appliesTo.Uri.AbsoluteUri));
        }

        #endregion

        /*
        public class IdentitySTS : SecurityTokenService
        {
            protected override IClaimsIdentity GetOutputClaimsIdentity(IClaimsPrincipal principal,
                RequestSecurityToken request, Scope scope)
            {
                IClaimsIdentity claimsIdentity = new ClaimsIdentity();
                claimsIdentity.Claims.Add(new Claim(ClaimTypes.Name, principal.Identity.Name));
                claimsIdentity.Claims.Add(new Claim(ClaimTypes.Role, "Users"));
                return claimsIdentity;
            }

            protected override Scope GetScope(Microsoft.IdentityModel.Claims.IClaimsPrincipal principal,
                RequestSecurityToken request)
            {
                Scope scope = new Scope(request);
                scope.EncryptingCredentials = this.GetCredentialsForAppliesTo(request.AppliesTo);
                scope.SigningCredentials =
                    new X509SigningCredentials(CertificateUtil.GetCertificate(StoreName.My, StoreLocation.LocalMachine,
                        "CN=IPKey"));
                return scope;
            }

        }
         */
    }
}
