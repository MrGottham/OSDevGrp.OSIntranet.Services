using System.Configuration;

namespace OSDevGrp.OSIntranet.Security.Configuration
{
    /// <summary>
    /// Configuration element for a subject name which refers a certificate.
    /// </summary>
    public class CertificateConfigurationElement : ConfigurationElement
    {
        #region Properties

        /// <summary>
        /// Gets the subject name which refers a certificate.
        /// </summary>
        [ConfigurationProperty("subjectName", DefaultValue="CN=localhost", IsRequired = true)]
        [RegexStringValidator(@"CN=(.*?)(?:,[A-Z]+=|$)")]
        public virtual string SubjetName
        {
            get { return (string) this["subjectName"]; }
        }

        #endregion
    }
}
