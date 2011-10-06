﻿using System;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.CommonLibrary.IoC;
using OSDevGrp.OSIntranet.CommonLibrary.IoC.Interfaces;
using OSDevGrp.OSIntranet.Contracts.Services;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using NUnit.Framework;

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
        [TestFixtureSetUp]
        public void SetUp()
        {
            _container = ContainerFactory.Create();
        }

        /// <summary>
        /// Tester, at container konfiguraiton kan indlæses og typer kan resolves.
        /// </summary>
        [Test]
        public void TestAtContainerConfigurationIndlæsesOgTyperKanResolves([Values(typeof(IContainer), typeof(IDomainObjectBuilder), typeof(IObjectMapper), typeof(ICommandBus), typeof(IQueryBus), typeof(IMySqlDataProvider), typeof(IAdresseRepository), typeof(IFinansstyringRepository), typeof(IKalenderRepository), typeof(IFællesRepository) , typeof(IKonfigurationRepository), typeof(IAdressekartotekService), typeof(IFinansstyringService), typeof(IKalenderService), typeof(ICommonService))] Type type)
        {
            var resolvedType = _container.Resolve(type);
            Assert.That(resolvedType, Is.Not.Null);
        }
    }
}
