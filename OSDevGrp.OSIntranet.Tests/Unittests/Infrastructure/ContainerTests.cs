using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommandHandlers.Core;
using OSDevGrp.OSIntranet.CommandHandlers.Dispatchers;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.CommonLibrary.IoC;
using OSDevGrp.OSIntranet.CommonLibrary.IoC.Interfaces;
using OSDevGrp.OSIntranet.Contracts.Faults;
using OSDevGrp.OSIntranet.Contracts.Services;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.CommandHandlers.Validation;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Guards;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Infrastructure
{
    /// <summary>
    /// Tester container til Inversion of Control.
    /// </summary>
    [TestFixture]
    public class ContainerTests
    {
        #region Private variables

        /// <summary>
        /// Container til Inversion of Control
        /// </summary>
        private IContainer _container;

        #endregion

        /// <summary>
        /// Konfigurering af test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _container = ContainerFactory.Create();
        }

        /// <summary>
        /// Tester, at container konfiguraiton kan indlæses og typer kan resolves.
        /// </summary>
        [Test]
        public void TestAtContainerConfigurationIndlæsesOgTyperKanResolves([Values(typeof(IContainer), typeof(IArgumentNullGuard), typeof(IObjectMapper), typeof(IFoodWasteObjectMapper), typeof(IExceptionBuilder), typeof(IFaultExceptionBuilder<FoodWasteFault>), typeof(IClaimValueProvider), typeof(IDomainObjectValidations), typeof(ISpecification), typeof(ICommandBus), typeof(IQueryBus), typeof(ILogicExecutor), typeof(ICommonValidations), typeof(IStaticTextFieldMerge), typeof(IWelcomeLetterDispatcher), typeof(IMySqlDataProvider), typeof(IFoodWasteDataProvider), typeof(IKalenderRepository), typeof(IFællesRepository), typeof(ISystemDataRepository), typeof(IHouseholdDataRepository), typeof(ICommunicationRepository), typeof(IConfigurationRepository), typeof(IKalenderService), typeof(ICommonService), typeof(IFoodWasteSystemDataService), typeof(IFoodWasteHouseholdDataService))] Type type)
        {
            var resolvedType = _container.Resolve(type);
            Assert.That(resolvedType, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the container resolves the same object each time.
        /// </summary>
        [Test]
        public void TestThatContainerResolvesSameObjectEachTime([Values(typeof(IContainer), typeof(IArgumentNullGuard), typeof(IObjectMapper), typeof(IFoodWasteObjectMapper), typeof(IExceptionBuilder), typeof(IFaultExceptionBuilder<FoodWasteFault>), typeof(IDomainObjectValidations), typeof(ICommonValidations))] Type type)
        {
            var firstResolvedType = _container.Resolve(type);
            Assert.That(firstResolvedType, Is.Not.Null);

            var secondResolvedType = _container.Resolve(type);
            Assert.That(secondResolvedType, Is.Not.Null);

            Assert.AreSame(firstResolvedType, secondResolvedType);
        }

        /// <summary>
        /// Tests that the container does not resolve the same object each time.
        /// </summary>
        [Test]
        public void TestThatContainerDoesNotResolveSameObjectEachTime([Values(typeof(ICommandBus), typeof(IQueryBus), typeof(IClaimValueProvider), typeof(ISpecification), typeof(ILogicExecutor), typeof(IStaticTextFieldMerge), typeof(IMySqlDataProvider), typeof(IFoodWasteDataProvider), typeof(IConfigurationRepository), typeof(ICommunicationRepository))] Type type)
        {
            var firstResolvedType = _container.Resolve(type);
            Assert.That(firstResolvedType, Is.Not.Null);

            var secondResolvedType = _container.Resolve(type);
            Assert.That(secondResolvedType, Is.Not.Null);

            Assert.AreNotSame(firstResolvedType, secondResolvedType);
        }
    }
}