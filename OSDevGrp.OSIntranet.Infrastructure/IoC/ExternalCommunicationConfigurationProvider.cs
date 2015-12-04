using System.Configuration;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using OSDevGrp.OSIntranet.CommonLibrary.IoC.Interfaces.Windsor;
using OSDevGrp.OSIntranet.Repositories.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.Infrastructure.IoC
{
    /// <summary>
    /// Configuration provider for external communication.
    /// </summary>
    public class ExternalCommunicationConfigurationProvider : IConfigurationProvider
    {
        #region Methods

        /// <summary>
        /// Adds configuration to the IoC container.
        /// </summary>
        /// <param name="container">IoC container.</param>
        public void AddConfiguration(IWindsorContainer container)
        {
            var configurationRepository = new ConfigurationRepository(ConfigurationManager.AppSettings);

            container.Register(Component.For<IConfigurationRepository>().Instance(configurationRepository).LifeStyle.Transient);
            container.Register(Component.For<ICommunicationRepository>().ImplementedBy<CommunicationRepository>().LifeStyle.Transient);
        }

        #endregion
    }
}
