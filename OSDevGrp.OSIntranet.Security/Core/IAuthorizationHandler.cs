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
        /// Authorize claims against a service type.
        /// </summary>
        /// <param name="claims">Claims from a claims identity.</param>
        /// <param name="serviceType">Service type.</param>
        void Authorize(IEnumerable<Claim> claims, Type serviceType);
    }
}
