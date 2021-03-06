﻿using System;
using System.ServiceModel;
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

                var trustedClaimSets = _authorizationHandler.GetTrustedClaimSets(operationContext.ServiceSecurityContext.AuthorizationContext.ClaimSets);
                _authorizationHandler.Authorize(trustedClaimSets, operationContext.Host.Description.ServiceType);

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
