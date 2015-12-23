using System.Configuration;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using OSDevGrp.OSIntranet.CommandHandlers.Core;
using OSDevGrp.OSIntranet.CommandHandlers.Dispatchers;
using OSDevGrp.OSIntranet.CommandHandlers.Validation;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.CommonLibrary.IoC.Interfaces.Windsor;
using OSDevGrp.OSIntranet.Contracts.Faults;
using OSDevGrp.OSIntranet.Contracts.Services;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Validation;
using OSDevGrp.OSIntranet.Infrastructure.Validation;
using OSDevGrp.OSIntranet.Repositories;
using OSDevGrp.OSIntranet.Repositories.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;

namespace OSDevGrp.OSIntranet.Infrastructure.IoC
{
    /// <summary>
    /// Konfigurationsprovider til OSIntranet.
    /// </summary>
    public class ConfigurationProvider : IConfigurationProvider
    {
        #region Private constants

        private const string MySqlDataProviderConnectionStringSettingsName = "OSDevGrp.OSIntranet.Repositories.DataProviders.MySqlDataProvider";
        private const string FoodWasteProviderConnectionStringSettingsName = "OSDevGrp.OSIntranet.Repositories.DataProviders.FoodWasteDataProvider";

        #endregion

        #region IConfigurationProvider Members

        /// <summary>
        /// Tilføjelse af konfiguration til containeren.
        /// </summary>
        /// <param name="container">Container, hvortil der skal tilføjes konfiguration.</param>
        public void AddConfiguration(IWindsorContainer container)
        {
            var konfigurationRepository = new KonfigurationRepository(ConfigurationManager.AppSettings);

            container.Register(Component.For<IDomainObjectBuilder>().ImplementedBy<DomainObjectBuilder>().LifeStyle.Singleton);
            container.Register(Component.For<IObjectMapper>().ImplementedBy<ObjectMapper>().LifeStyle.Singleton);
            container.Register(Component.For<IFoodWasteObjectMapper>().ImplementedBy<FoodWasteObjectMapper>().LifeStyle.Singleton);
            container.Register(Component.For<IExceptionBuilder>().ImplementedBy<ExceptionBuilder>().LifeStyle.Singleton);
            container.Register(Component.For<IFaultExceptionBuilder<FoodWasteFault>>().ImplementedBy<FoodWasteFaultExceptionBuilder>().LifeStyle.Singleton);
            container.Register(Component.For<IDomainObjectValidations>().ImplementedBy<DomainObjectValidations>().LifeStyle.Singleton);
            container.Register(Component.For<ISpecification>().ImplementedBy<Specification>().LifeStyle.Transient);
            container.Register(Component.For<ICommandBus>().ImplementedBy<CommandBus>().LifeStyle.Transient);
            container.Register(Component.For<IQueryBus>().ImplementedBy<QueryBus>().LifeStyle.Transient);
            container.Register(Component.For<ICommonValidations>().ImplementedBy<CommonValidations>().LifeStyle.Singleton);
            container.Register(Component.For<ILogicExecutor>().ImplementedBy<LogicExecutor>().LifeStyle.Transient);
            container.Register(Component.For<IStaticTextFieldMerge>().ImplementedBy<StaticTextFieldMerge>().LifeStyle.Transient);
            container.Register(Component.For<IKonfigurationRepository>().Instance(konfigurationRepository).LifeStyle.Transient);

            container.Register(Component.For<IMySqlDataProvider>().Instance(new MySqlDataProvider(ConfigurationManager.ConnectionStrings[MySqlDataProviderConnectionStringSettingsName])).LifeStyle.Transient);
            container.Register(Component.For<IFoodWasteDataProvider>().Instance(new FoodWasteDataProvider(ConfigurationManager.ConnectionStrings[FoodWasteProviderConnectionStringSettingsName])).LifeStyle.Transient);

            container.Register(Classes.FromAssemblyNamed("OSDevGrp.OSIntranet.Repositories").BasedOn(typeof (IRepository)).WithService.FromInterface(typeof (IRepository)));

            container.Register(Classes.FromAssemblyNamed("OSDevGrp.OSIntranet.CommandHandlers").BasedOn(typeof (ICommandHandler)).WithService.FromInterface(typeof (ICommandHandler)));

            container.Register(Classes.FromAssemblyNamed("OSDevGrp.OSIntranet.QueryHandlers").BasedOn(typeof (IQueryHandler)).WithService.FromInterface(typeof (IQueryHandler)));

            container.Register(Classes.FromAssemblyNamed("OSDevGrp.OSIntranet.Services").BasedOn(typeof (IIntranetService)).WithService.FromInterface(typeof (IIntranetService)));
        }

        #endregion
    }
}
