using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using Microsoft.IdentityModel.Claims;
using OSDevGrp.OSIntranet.Resources;
using OSDevGrp.OSIntranet.Security.Attributes;

namespace OSDevGrp.OSIntranet.Security.AuthorizationPolicies
{
    /// <summary>
    /// Functionality which can handle the authorization policy.
    /// </summary>
    public class AuthorizationPolicyHandler : IAuthorizationPolicyHandler
    {
        #region Methods

        /// <summary>
        /// Gets the custom claim based principal.
        /// </summary>
        /// <param name="claimsIdentities">Collection of claim based identities.</param>
        /// <param name="serviceType">Type for the service on which to use the authorization policy.</param>
        /// <returns>Custom claim based principal.</returns>
        public virtual IClaimsPrincipal GetCustomPrincipal(IEnumerable<IClaimsIdentity> claimsIdentities, Type serviceType)
        {
            if (claimsIdentities == null)
            {
                throw new SecurityException(Resource.GetExceptionMessage(ExceptionMessage.NoIdentityWasFound));
            }
            var claimIdentityArray = claimsIdentities.ToArray();
            if (claimIdentityArray.Length == 0)
            {
                throw new SecurityException(Resource.GetExceptionMessage(ExceptionMessage.NoIdentityWasFound));
            }

            if (serviceType == null || serviceType.GetCustomAttributes(typeof (RequiredClaimTypeAttribute), true).Any() == false)
            {
                return new ClaimsPrincipal(claimIdentityArray.ToArray());
            }
            foreach (var requiredClaimTypeAttribute in serviceType.GetCustomAttributes(typeof (RequiredClaimTypeAttribute), true).Cast<RequiredClaimTypeAttribute>())
            {
                if (string.IsNullOrEmpty(requiredClaimTypeAttribute.RequiredClaimType))
                {
                    continue;
                }
                var hasClaimType = claimIdentityArray.SelectMany(m => m.Claims)
                    .Where(m => string.IsNullOrEmpty(m.ClaimType) == false)
                    .Any(m => string.Compare(m.ClaimType, requiredClaimTypeAttribute.RequiredClaimType, StringComparison.InvariantCulture) == 0);
                if (hasClaimType == false)
                {
                    throw new SecurityException(Resource.GetExceptionMessage(ExceptionMessage.MissingClaimTypeForIdentity));
                }
            }
            return new ClaimsPrincipal(claimIdentityArray.ToArray());
        }

        #endregion
    }
}
