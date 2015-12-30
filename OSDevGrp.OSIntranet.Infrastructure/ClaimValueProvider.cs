using System;
using System.Security.Claims;
using System.Threading;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Infrastructure
{
    /// <summary>
    /// Provider which can resolve values from the current users claims.
    /// </summary>
    public class ClaimValueProvider : IClaimValueProvider
    {
        #region Properties

        /// <summary>
        /// Gets the mail address from the current users email claim.
        /// </summary>
        public virtual string MailAddress
        {
            get
            {
                var emailClaim = Find(ClaimTypes.Email);
                return emailClaim == null ? string.Empty : emailClaim.Value;
            }
        }

        #endregion

        #region Methdos

        /// <summary>
        /// Find and returns the claim for a given claim type.
        /// </summary>
        /// <param name="claimType">Claim type.</param>
        /// <returns>Claim for the given claim type.</returns>
        private static Claim Find(string claimType)
        {
            if (string.IsNullOrEmpty(claimType))
            {
                throw new ArgumentNullException("claimType");
            }

            var claimsPrincipal = new ClaimsPrincipal(Thread.CurrentPrincipal);
            return claimsPrincipal.FindFirst(claimType);
        }

        #endregion
    }
}
