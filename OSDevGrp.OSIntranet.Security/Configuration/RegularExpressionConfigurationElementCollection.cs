using System.Configuration;

namespace OSDevGrp.OSIntranet.Security.Configuration
{
    /// <summary>
    /// Collection of configuration element containing a regular expression validator.
    /// </summary>
    public class RegularExpressionConfigurationElementCollection : ConfigurationElementCollection
    {
        #region Methods

        /// <summary>
        /// Creates a new configuration element containing a regular expression validator.
        /// </summary>
        /// <returns>new configuration element containing a regular expression validator.</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new RegularExpressionConfigurationElement();
        }

        /// <summary>
        /// Gets the key for a given configuration element containing a regular expression validator.
        /// </summary>
        /// <param name="element">Configuration element for which the key should be returned.</param>
        /// <returns>Key for a given configuration element containing a regular expression validator.</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            var regularExpressionConfigurationElement = (RegularExpressionConfigurationElement) element;
            return string.Format("{0}|{1}", regularExpressionConfigurationElement.ValueClaimType, regularExpressionConfigurationElement.MatchCondition).GetHashCode();
        }

        #endregion
    }
}
