using System;
using System.IdentityModel.Claims;
using System.Linq;
using System.ServiceModel;
using Microsoft.IdentityModel.Claims;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.Security.Core
{
    /// <summary>
    /// Functionality which checks authorization access for a secure token on each service operation.
    /// </summary>
    public class SecureTokenAuthorizer : ServiceAuthorizationManager
    {
        #region Private variables

        private readonly IAuthorizationHandler _authorizationHandler;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates functionality which checks authorization access for a secure token on each service operation.
        /// </summary>
        public SecureTokenAuthorizer()
            : this(new AuthorizationHandler())
        {
        }

        /// <summary>
        /// Creates functionality which checks authorization access for a secure token on each service operation.
        /// </summary>
        /// <param name="authorizationHandler">Functionality which can handle authorization.</param>
        public SecureTokenAuthorizer(IAuthorizationHandler authorizationHandler)
        {
            if (authorizationHandler == null)
            {
                throw new ArgumentNullException("authorizationHandler");
            }
            _authorizationHandler = authorizationHandler;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Checks authorization for the given operation context.
        /// </summary>
        /// <param name="operationContext">Operation context.</param>
        /// <returns>True if access is granted otherwise false.</returns>
        public override bool CheckAccess(OperationContext operationContext)
        {
            try
            {
                if (base.CheckAccess(operationContext) == false)
                {
                    return false;
                }
                if (operationContext.Host == null || operationContext.Host.Description == null)
                {
                    return false;
                }
                if (operationContext.ServiceSecurityContext == null || operationContext.ServiceSecurityContext.AuthorizationContext == null || operationContext.ServiceSecurityContext.AuthorizationContext.ClaimSets == null)
                {
                    return false;
                }

                var certificateClaimSets = operationContext.ServiceSecurityContext.AuthorizationContext.ClaimSets
                    .Where(claimSet => claimSet.Issuer as X509CertificateClaimSet != null)
                    .Select(claimSet => (X509CertificateClaimSet) claimSet.Issuer);
                var claimsIdentities = certificateClaimSets
                    .Where(claimSet => claimSet.X509Certificate != null)
                    .Select(claimSet => new ClaimsIdentity(claimSet.X509Certificate, claimSet.X509Certificate.Issuer))
                    .ToList();
                _authorizationHandler.Authorize(claimsIdentities.SelectMany(m => m.Claims), operationContext.Host.Description.ServiceType);

                return true;
            }
            catch (Exception ex)
            {
                throw new FaultException(Resource.GetExceptionMessage(ExceptionMessage.NotAuthorizedToUseService, ex.Message));
            }
        }

        #endregion
    }
}
