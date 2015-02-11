using System;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.IdentityModel.Claims;

namespace OSDevGrp.OSIntranet.Security.Configuration
{
    /// <summary>
    /// Configuration element for a claim.
    /// </summary>
    public class ClaimConfigurationElement : ConfigurationElement
    {
        #region Properties

        /// <summary>
        /// Gets the claim type.
        /// </summary>
        [ConfigurationProperty("claimType", DefaultValue = "http://osdevgrp.local/osintranet/security", IsRequired = true)]
        [RegexStringValidator(@"((?<=\()[A-Za-z][A-Za-z0-9\+\.\-]*:([A-Za-z0-9\.\-_~:/\?#\[\]@!\$&'\(\)\*\+,;=]|%[A-Fa-f0-9]{2})+(?=\)))|([A-Za-z][A-Za-z0-9\+\.\-]*:([A-Za-z0-9\.\-_~:/\?#\[\]@!\$&'\(\)\*\+,;=]|%[A-Fa-f0-9]{2})+)")]
        public virtual string ClaimType
        {
            get { return (string) this["claimType"]; }
        }

        /// <summary>
        /// Gets the claim value.
        /// </summary>
        [ConfigurationProperty("claimValue", DefaultValue = "", IsRequired = true)]
        public virtual string ClaimValue
        {
            get { return (string) this["claimValue"]; }
        }

        /// <summary>
        /// Conditions indicating whether the claim should be associated with a principal.
        /// </summary>
        [ConfigurationProperty("conditions", IsRequired = true)]
        public virtual RegularExpressionConfigurationElementCollection Conditions
        {
            get { return (RegularExpressionConfigurationElementCollection)this["conditions"]; }
        }

        /// <summary>
        /// Gets the claim.
        /// </summary>
        public virtual Claim Claim
        {
            get
            {
                return new Claim(ClaimType, ClaimValue, ClaimValueTypes.String, ConfigurationProvider.Instance.IssuerTokenName.Address);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Validates whether the claim should be associated with a given claims identity.
        /// </summary>
        /// <param name="claimsIdentity">Claims identity.</param>
        /// <returns>True if the cliam should be associated with the given claims identify otherwise false.</returns>
        public virtual bool Validate(IClaimsIdentity claimsIdentity)
        {
            if (claimsIdentity == null || claimsIdentity.Claims == null)
            {
                return false;
            }
            if (claimsIdentity.Claims.Any(c => string.Compare(c.ClaimType, ClaimType, StringComparison.Ordinal) == 0))
            {
                return false;
            }
            foreach (var condition in Conditions.OfType<RegularExpressionConfigurationElement>())
            {
                var currentCondition = condition;
                foreach (var valueClaim in claimsIdentity.Claims.Where(c => string.Compare(c.ClaimType, currentCondition.ValueClaimType, StringComparison.Ordinal) == 0))
                {
                    if (string.IsNullOrEmpty(valueClaim.Value))
                    {
                        continue;
                    }
                    var regex = new Regex(currentCondition.MatchCondition, RegexOptions.None);
                    if (regex.IsMatch(valueClaim.Value))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion
    }
}
