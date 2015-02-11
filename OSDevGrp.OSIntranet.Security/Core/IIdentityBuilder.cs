using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Security.Principal;

namespace OSDevGrp.OSIntranet.Security.Core
{
    /// <summary>
    /// Interface for functionality which can build an identity.
    /// </summary>
    public interface IIdentityBuilder
    {
        /// <summary>
        /// Build an identity from a security token.
        /// </summary>
        /// <param name="securityToken">Security token from which to build the identity.</param>
        /// <param name="identityProperties">Dictonary containing properties for the identity.</param>
        /// <returns>Identity build from the security token.</returns>
        IIdentity Build(SecurityToken securityToken, IDictionary<string, string> identityProperties = null);
    }
}
