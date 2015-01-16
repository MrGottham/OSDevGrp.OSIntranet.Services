using System.Configuration;

namespace OSDevGrp.OSIntranet.Security.Configuration
{
    /// <summary>
    /// Collection of configuration elements containing a claim definition.
    /// </summary>
    public class ClaimConfigurationElementCollection : ConfigurationElementCollection
    {
        #region Methods

        /// <summary>
        /// Creates a new configuration element containing a claim definition.
        /// </summary>
        /// <returns>New configuration element containing a claim definition.</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new ClaimConfigurationElement();
        }

        /// <summary>
        /// Gets the key for a given configuration element containing a claim definition.
        /// </summary>
        /// <param name="element">Configuration element for which the key should be returned.</param>
        /// <returns>Key for the given configuration element containing a claim definition.</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            var claimConfigurationElement = (ClaimConfigurationElement) element;
            if (string.IsNullOrEmpty(claimConfigurationElement.ClaimValue))
            {
                return string.Format("{0}", claimConfigurationElement.ClaimType).GetHashCode();
            }
            return string.Format("{0}|{1}", claimConfigurationElement.ClaimType, claimConfigurationElement.ClaimValue).GetHashCode();
        }

        #endregion
    }
}
