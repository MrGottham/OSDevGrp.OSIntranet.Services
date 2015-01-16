using System.Configuration;

namespace OSDevGrp.OSIntranet.Security.Configuration
{
    /// <summary>
    /// Configuration element for a regular expression validator.
    /// </summary>
    public class RegularExpressionConfigurationElement : ConfigurationElement
    {
        #region Properties

        /// <summary>
        /// Gets the cliam type which should deliver the value to the regular expression.
        /// </summary>
        [ConfigurationProperty("valueClaimType", DefaultValue = "urn:osdevgrp:osintranet:security:1.0.0", IsRequired = true)]
        [RegexStringValidator(@"((?<=\()[A-Za-z][A-Za-z0-9\+\.\-]*:([A-Za-z0-9\.\-_~:/\?#\[\]@!\$&'\(\)\*\+,;=]|%[A-Fa-f0-9]{2})+(?=\)))|([A-Za-z][A-Za-z0-9\+\.\-]*:([A-Za-z0-9\.\-_~:/\?#\[\]@!\$&'\(\)\*\+,;=]|%[A-Fa-f0-9]{2})+)")]
        public virtual string ValueClaimType
        {
            get { return (string) this["valueClaimType"]; }
        }

        /// <summary>
        /// Gets the regular expression which sould be used as match condition.
        /// </summary>
        [ConfigurationProperty("matchCondition", DefaultValue = "", IsRequired = true)]
        public virtual string MatchCondition
        {
            get { return (string) this["matchCondition"]; }
        }

        #endregion
    }
}
