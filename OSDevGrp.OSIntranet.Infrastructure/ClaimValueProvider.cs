using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.IdentityModel.Claims;
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

            var claims = new List<Claim>();
            if (Thread.CurrentPrincipal is IClaimsPrincipal)
            {
                claims.AddRange(GetClaims(Thread.CurrentPrincipal as IClaimsPrincipal));
            }
            if (Thread.CurrentPrincipal is System.Security.Claims.ClaimsPrincipal)
            {
                claims.AddRange(GetClaims(Thread.CurrentPrincipal as System.Security.Claims.ClaimsPrincipal));
            }
            return claims.FirstOrDefault(claim => string.Compare(claim.ClaimType, claimType, StringComparison.Ordinal) == 0);
        }

        /// <summary>
        /// Gets all the claims for a given claims principal.
        /// </summary>
        /// <param name="claimsPrincipal">Implementation of the claims principal in the Microsoft Identity Model.</param>
        /// <returns>All the claims for a given claims principal.</returns>
        private static IEnumerable<Claim> GetClaims(IClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal == null)
            {
                throw new ArgumentNullException("claimsPrincipal");
            }
            return claimsPrincipal.Identities.SelectMany(claimsIdentity => claimsIdentity.Claims).ToArray();
        }

        /// <summary>
        /// Gets all the claims for a given claims principal.
        /// </summary>
        /// <param name="claimsPrincipal">Cliam principal in the System Security.</param>
        /// <returns>All the claims for a given claims principal.</returns>
        private static IEnumerable<Claim> GetClaims(System.Security.Claims.ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal == null)
            {
                throw new ArgumentNullException("claimsPrincipal");
            }
            return claimsPrincipal.Identities
                .SelectMany(claimIdentifiy => claimIdentifiy.Claims)
                .Select(claim => new Claim(claim.Type, claim.Value, claim.ValueType, claim.Issuer, claim.OriginalIssuer))
                .ToArray();
        }

        #endregion
    }
}
