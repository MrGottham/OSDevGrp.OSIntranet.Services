using System.Configuration;

namespace OSDevGrp.OSIntranet.Security.Configuration
{
    /// <summary>
    /// Collection of configuration elements for some URI addresses.
    /// </summary>
    public class UriConfigurationElementCollection : ConfigurationElementCollection
    {
        #region Method

        /// <summary>
        /// Creates a new configuration element for an URI address.
        /// </summary>
        /// <returns>New configuration element for an URI address</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new UriConfigurationElement();
        }

        /// <summary>
        /// Gets the key for a given configuration element.
        /// </summary>
        /// <param name="element">Configuration element for which the key should be return.</param>
        /// <returns>Key for the given configuration elemenet.</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((UriConfigurationElement) element).Address;
        }

        #endregion
    }
}
