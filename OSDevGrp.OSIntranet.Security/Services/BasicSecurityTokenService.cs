using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.ServiceModel;
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.SecurityTokenService;
using OSDevGrp.OSIntranet.Resources;
using OSDevGrp.OSIntranet.Security.Configuration;
using OSDevGrp.OSIntranet.Security.Core;

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
            if (principal.Identity == null || principal.Identity.IsAuthenticated == false)
            {
                var authenticationException = new AuthenticationException();
                throw new InvalidRequestException(authenticationException.Message, authenticationException);
            }
            try
            {
                var appliesTo = request.AppliesTo;
                if (appliesTo == null || appliesTo.Uri == null || string.IsNullOrEmpty(appliesTo.Uri.AbsoluteUri))
                {
                    throw new InvalidRequestException(Resource.GetExceptionMessage(ExceptionMessage.AppliesToMustBeSuppliedInRequestSecurityToken));
                }
                return new Scope(appliesTo.Uri.AbsoluteUri)
                {
                    EncryptingCredentials = GetCredentialsForAppliesTo(appliesTo),
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
            if (scope == null)
            {
                var argumentNullException = new ArgumentNullException("scope");
                throw new InvalidRequestException(argumentNullException.Message, argumentNullException);
            }
            try
            {
                IClaimsIdentity claimsIdentity;
                if (principal.Identities != null && principal.Identities.Any())
                {
                    claimsIdentity = AppendClaims(principal.Identities.First());
                    return CreateOutgoingClaimsIdentity(scope.EncryptingCredentials as X509EncryptingCredentials, claimsIdentity.Claims);
                }
                claimsIdentity = new ClaimsIdentity();
                claimsIdentity.Claims.Add(new Claim(ClaimTypes.Name, principal.Identity.Name));
                return CreateOutgoingClaimsIdentity(scope.EncryptingCredentials as X509EncryptingCredentials, claimsIdentity.Claims);
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
        /// Gets the credentials for the scope.
        /// </summary>
        /// <param name="appliesTo">AppliesTo from the incoming RST.</param>
        /// <returns>Credentials for the scope.</returns>
        private static X509EncryptingCredentials GetCredentialsForAppliesTo(EndpointAddress appliesTo)
        {
            if (appliesTo == null || appliesTo.Uri == null || string.IsNullOrEmpty(appliesTo.Uri.AbsoluteUri))
            {
                throw new InvalidRequestException(Resource.GetExceptionMessage(ExceptionMessage.AppliesToMustBeSuppliedInRequestSecurityToken));
            }
            var appliesToIdentity = appliesTo.Identity as X509CertificateEndpointIdentity;
            if (appliesToIdentity == null || appliesToIdentity.Certificates == null || appliesToIdentity.Certificates.Count <= 0)
            {
                throw new InvalidRequestException(Resource.GetExceptionMessage(ExceptionMessage.AppliesToMustHaveX509CertificateEndpointIdentity));
            }
            if (ConfigurationProvider.Instance.TrustedRelyingPartyCollection.OfType<UriConfigurationElement>().Any(trustedRelyingParty => appliesTo.Uri.AbsoluteUri.StartsWith(trustedRelyingParty.Uri.AbsoluteUri)))
            {
                return new X509EncryptingCredentials(appliesToIdentity.Certificates[0]);
            }
            throw new InvalidRequestException(Resource.GetExceptionMessage(ExceptionMessage.InvalidRelyingPartyAddress, appliesTo.Uri.AbsoluteUri));
        }

        /// <summary>
        /// Appends the claims which should be associated with the claims identity.
        /// </summary>
        /// <param name="claimsIdentity">Claims identity.</param>
        /// <returns>Claims identity with associated claims.</returns>
        private static IClaimsIdentity AppendClaims(IClaimsIdentity claimsIdentity)
        {
            if (claimsIdentity == null)
            {
                var argumentNullException = new ArgumentNullException("claimsIdentity");
                throw new InvalidRequestException(argumentNullException.Message, argumentNullException);
            }
            foreach (var claimConfigurationElement in ConfigurationProvider.Instance.ClaimCollection.OfType<ClaimConfigurationElement>().Where(claimConfigurationElement => claimConfigurationElement.Validate(claimsIdentity)))
            {
                claimsIdentity.Claims.Add(claimConfigurationElement.Claim);
            }
            return claimsIdentity;
        }

        /// <summary>
        /// Creates the outgoing claims identity.
        /// </summary>
        /// <param name="encryptingCredentials">Encrypting credentials for the endpoint to call.</param>
        /// <param name="claims">Claims from the calling claims identity.</param>
        /// <returns>Outgoing claims identity.</returns>
        private static IClaimsIdentity CreateOutgoingClaimsIdentity(X509EncryptingCredentials encryptingCredentials, IEnumerable<Claim> claims)
        {
            if (encryptingCredentials == null)
            {
                var argumentNullException = new ArgumentNullException("encryptingCredentials");
                throw new InvalidRequestException(argumentNullException.Message, argumentNullException);
            }
            if (claims == null)
            {
                var argumentNullException = new ArgumentNullException("claims");
                throw new InvalidRequestException(argumentNullException.Message, argumentNullException);
            }
            var outgoingClaimsIdentity = new ClaimsIdentity(encryptingCredentials.Certificate, encryptingCredentials.Certificate.Issuer);
            foreach (var missingClaim in claims.Where(claim => outgoingClaimsIdentity.Claims.Any(hasClaim => string.Compare(claim.ClaimType, hasClaim.ClaimType, StringComparison.Ordinal) == 0) == false))
            {
                outgoingClaimsIdentity.Claims.Add(new Claim(missingClaim.ClaimType, missingClaim.Value, missingClaim.ValueType, encryptingCredentials.Certificate.Issuer, encryptingCredentials.Certificate.Issuer));
            }
            return outgoingClaimsIdentity;
        }

        #endregion
    }
}
