using Castle.Windsor;
using OSDevGrp.OSIntranet.CommonLibrary.IoC.Interfaces.Windsor;

namespace OSDevGrp.OSIntranet.Infrastructure.IoC
{
    /// <summary>
    /// Konfigurationsprovider til OSIntranet.
    /// </summary>
    public class ConfigurationProvider : IConfigurationProvider
    {
        #region IConfigurationProvider Members

        /// <summary>
        /// Tilføjelse af konfiguration til containeren.
        /// </summary>
        /// <param name="container">Container, hvortil der skal tilføjes konfiguration.</param>
        public void AddConfiguration(IWindsorContainer container)
        {
        }

        #endregion
    }
}
