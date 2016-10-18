using System.Configuration;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using OSDevGrp.OSIntranet.CommonLibrary.IoC.Interfaces.Windsor;
using OSDevGrp.OSIntranet.Repositories.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.Infrastructure.IoC
{
    /// <summary>
    /// Configuration provider for external communication without communication integrations.
    /// </summary>
    public class ExternalCommunicationWithoutExternalCommunicationConfigurationProvider : IConfigurationProvider
    {
        #region Methods

        /// <summary>
        /// Adds configuration to the IoC container.
        /// </summary>
        /// <param name="container">IoC container.</param>
        public void AddConfiguration(IWindsorContainer container)
        {
            container.Register(Component.For<IConfigurationRepository>().UsingFactoryMethod(() => new ConfigurationRepository(ConfigurationManager.AppSettings)).LifeStyle.Transient);
            container.Register(Component.For<ICommunicationRepository>().ImplementedBy<CommunicationRepositoryWithoutExternalCommunication>().LifeStyle.Transient);
        }

        #endregion
    }
}
