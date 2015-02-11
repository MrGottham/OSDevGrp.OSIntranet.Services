using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.Linq;
using System.Security;
using OSDevGrp.OSIntranet.Resources;
using OSDevGrp.OSIntranet.Security.Attributes;

namespace OSDevGrp.OSIntranet.Security.Core
{
    /// <summary>
    /// Functionality which can handle authorization.
    /// </summary>
    public class AuthorizationHandler : IAuthorizationHandler
    {
        #region Methods

        /// <summary>
        /// Authorize claims against a service type.
        /// </summary>
        /// <param name="claims">Claims from a claims identity.</param>
        /// <param name="serviceType">Service type.</param>
        public virtual void Authorize(IEnumerable<Claim> claims, Type serviceType)
        {
            if (claims == null)
            {
                throw new ArgumentNullException("claims");
            }
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }

            var claimArray = claims.ToArray();
            if (claimArray.Length == 0)
            {
                throw new SecurityException(Resource.GetExceptionMessage(ExceptionMessage.NoClaimsWasFound));
            }

            if (serviceType.GetCustomAttributes(typeof (RequiredClaimTypeAttribute), true).Any() == false)
            {
                return;
            }

            serviceType.GetCustomAttributes(typeof (RequiredClaimTypeAttribute), true)
                .Cast<RequiredClaimTypeAttribute>()
                .Where(requiredClaimTypeAttribute => string.IsNullOrEmpty(requiredClaimTypeAttribute.RequiredClaimType) == false)
                .ToList()
                .ForEach(requiredClaimTypeAttribute =>
                {
                    var hasClaimType = claimArray.Any(m => string.Compare(m.ClaimType, requiredClaimTypeAttribute.RequiredClaimType, StringComparison.Ordinal) == 0);
                    if (hasClaimType == false)
                    {
                        throw new SecurityException(Resource.GetExceptionMessage(ExceptionMessage.MissingClaimTypeForIdentity));
                    }
                });
        }

        #endregion
    }
}
