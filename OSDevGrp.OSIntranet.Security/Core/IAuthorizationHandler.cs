using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;

namespace OSDevGrp.OSIntranet.Security.Core
{
    /// <summary>
    /// Interface for functionality which can handle authorization.
    /// </summary>
    public interface IAuthorizationHandler
    {
        /// <summary>
        /// Gets the trusted claim sets from a collection of claim sets.
        /// </summary>
        /// <param name="claimSets">Collection of claim sets from which to get the trusted claim sets.</param>
        /// <returns>Trusted claim sets.</returns>
        IEnumerable<ClaimSet> GetTrustedClaimSets(IEnumerable<ClaimSet> claimSets); 

        /// <summary>
        /// Authorize trusted claim sets against a service type.
        /// </summary>
        /// <param name="claimSets">Collection of trusted claims sets.</param>
        /// <param name="serviceType">Service type.</param>
        void Authorize(IEnumerable<ClaimSet> claimSets, Type serviceType);
    }
}
