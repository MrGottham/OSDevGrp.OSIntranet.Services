using System.Security.Principal;

namespace OSDevGrp.OSIntranet.Security.Core
{
    /// <summary>
    /// Interface for a provider which can provide an identity.
    /// </summary>
    public interface IIdentityProvider
    {
        /// <summary>
        /// Gets the primary identity.
        /// </summary>
        IIdentity PrimaryIdentity { get; }
    }
}
