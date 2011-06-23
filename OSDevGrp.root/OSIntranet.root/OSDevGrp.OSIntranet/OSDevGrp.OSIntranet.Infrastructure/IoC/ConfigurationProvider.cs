using System.Configuration;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.CommonLibrary.IoC.Interfaces.Windsor;
using OSDevGrp.OSIntranet.Contracts.Services;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Repositories;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

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
            var konfigurationRepository = new KonfigurationRepository(ConfigurationManager.AppSettings);

            container.Register(Component.For<IObjectMapper>().ImplementedBy<ObjectMapper>().LifeStyle.Singleton);
            container.Register(Component.For<ICommandBus>().ImplementedBy<CommandBus>().LifeStyle.Transient);
            container.Register(Component.For<IQueryBus>().ImplementedBy<QueryBus>().LifeStyle.Transient);
            container.Register(Component.For<IDomainObjectBuilder>().ImplementedBy<DomainObjectBuilder>().LifeStyle.Transient);
            container.Register(Component.For<IKonfigurationRepository>().Instance(konfigurationRepository).LifeStyle.Transient);

            container.Register(AllTypes.FromAssemblyNamed("OSDevGrp.OSIntranet.Repositories")
                                   .BasedOn(typeof (IRepository)).WithService.FromInterface(typeof (IRepository)));

            container.Register(AllTypes.FromAssemblyNamed("OSDevGrp.OSIntranet.CommandHandlers")
                                   .BasedOn(typeof(ICommandHandler)).WithService.FromInterface(typeof(ICommandHandler)));

            container.Register(AllTypes.FromAssemblyNamed("OSDevGrp.OSIntranet.QueryHandlers")
                                   .BasedOn(typeof (IQueryHandler)).WithService.FromInterface(typeof (IQueryHandler)));

            container.Register(AllTypes.FromAssemblyNamed("OSDevGrp.OSIntranet.Services")
                                   .BasedOn(typeof (IIntranetService)).WithService.FromInterface(typeof (IIntranetService)));
        }

        #endregion
    }
}
