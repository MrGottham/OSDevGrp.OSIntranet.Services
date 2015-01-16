using System;
using System.Configuration;

namespace OSDevGrp.OSIntranet.Security.Configuration
{
    /// <summary>
    /// Configuration element for an URI address.
    /// </summary>
    public class UriConfigurationElement : ConfigurationElement
    {
        #region Properties

        /// <summary>
        /// Gets the Uri address.
        /// </summary>
        [ConfigurationProperty("address", DefaultValue = "http://localhost", IsRequired = true)]
        [RegexStringValidator(@"((?<=\()[A-Za-z][A-Za-z0-9\+\.\-]*:([A-Za-z0-9\.\-_~:/\?#\[\]@!\$&'\(\)\*\+,;=]|%[A-Fa-f0-9]{2})+(?=\)))|([A-Za-z][A-Za-z0-9\+\.\-]*:([A-Za-z0-9\.\-_~:/\?#\[\]@!\$&'\(\)\*\+,;=]|%[A-Fa-f0-9]{2})+)")]
        public virtual string Address
        {
            get { return (string) this["address"]; }
        }

        /// <summary>
        /// Gets the Uri.
        /// </summary>
        public virtual Uri Uri
        {
            get
            {
                return new Uri(Address);
            }
        }

        #endregion
    }
}
