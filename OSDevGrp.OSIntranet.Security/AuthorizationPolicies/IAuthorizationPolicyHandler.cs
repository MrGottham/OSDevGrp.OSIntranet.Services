using System;
using System.Collections.Generic;
using Microsoft.IdentityModel.Claims;

namespace OSDevGrp.OSIntranet.Security.AuthorizationPolicies
{
    /// <summary>
    /// Interface for functionality which can handle the authorization policy.
    /// </summary>
    public interface IAuthorizationPolicyHandler
    {
        /// <summary>
        /// Gets the custom claim based principal.
        /// </summary>
        /// <param name="claimsIdentities">Collection of claim based identities.</param>
        /// <param name="serviceType">Type for the service on which to use the authorization policy.</param>
        /// <returns>Custom claim based principal.</returns>
        IClaimsPrincipal GetCustomPrincipal(IEnumerable<IClaimsIdentity> claimsIdentities, Type serviceType);
    }
}
