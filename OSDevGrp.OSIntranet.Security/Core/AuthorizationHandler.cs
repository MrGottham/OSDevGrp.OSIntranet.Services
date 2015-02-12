using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.Linq;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using OSDevGrp.OSIntranet.Resources;
using OSDevGrp.OSIntranet.Security.Attributes;
using OSDevGrp.OSIntranet.Security.Configuration;

namespace OSDevGrp.OSIntranet.Security.Core
{
    /// <summary>
    /// Functionality which can handle authorization.
    /// </summary>
    public class AuthorizationHandler : IAuthorizationHandler
    {
        #region Methods

        /// <summary>
        /// Gets the trusted claim sets from a collection of claim sets.
        /// </summary>
        /// <param name="claimSets">Collection of claim sets from which to get the trusted claim sets.</param>
        /// <returns>Trusted claim sets.</returns>
        public virtual IEnumerable<ClaimSet> GetTrustedClaimSets(IEnumerable<ClaimSet> claimSets)
        {
            if (claimSets == null)
            {
                throw new ArgumentNullException("claimSets");
            }
            var signingCertificate = CertificateHelper.GetCertificate(StoreName.TrustedPeople, StoreLocation.LocalMachine, ConfigurationProvider.Instance.SigningCertificate.SubjetName);
            return claimSets.Where(claimSet => claimSet.Issuer as X509CertificateClaimSet != null && ((X509CertificateClaimSet) claimSet.Issuer).X509Certificate != null && string.Compare(((X509CertificateClaimSet) claimSet.Issuer).X509Certificate.Thumbprint, signingCertificate.Thumbprint, StringComparison.Ordinal) == 0).ToArray();
        }

        /// <summary>
        /// Authorize trusted claim sets against a service type.
        /// </summary>
        /// <param name="claimSets">Collection of trusted claims sets.</param>
        /// <param name="serviceType">Service type.</param>
        public virtual void Authorize(IEnumerable<ClaimSet> claimSets, Type serviceType)
        {
            if (claimSets == null)
            {
                throw new ArgumentNullException("claimSets");
            }
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }

            if (serviceType.GetCustomAttributes(typeof (RequiredClaimTypeAttribute), true).Any() == false)
            {
                return;
            }

            var claimSetArray = claimSets.ToArray();
            if (claimSetArray.Length == 0)
            {
                throw new SecurityException(Resource.GetExceptionMessage(ExceptionMessage.NoClaimsWasFound));
            }

            serviceType.GetCustomAttributes(typeof(RequiredClaimTypeAttribute), true)
                .Cast<RequiredClaimTypeAttribute>()
                .Where(requiredClaimTypeAttribute => string.IsNullOrEmpty(requiredClaimTypeAttribute.RequiredClaimType) == false)
                .ToList()
                .ForEach(requiredClaimTypeAttribute =>
                {
                    var hasClaimType = claimSetArray.Any(claimSet =>
                    {
                        var claims = claimSet.FindClaims(requiredClaimTypeAttribute.RequiredClaimType, Rights.PossessProperty);
                        return claims != null && claims.Any();
                    });
                    if (hasClaimType == false)
                    {
                        throw new SecurityException(Resource.GetExceptionMessage(ExceptionMessage.MissingClaimTypeForIdentity));
                    }
                });
        }

        #endregion
    }
}
