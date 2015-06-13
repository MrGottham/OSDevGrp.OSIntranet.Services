using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.DataProxies.FoodWaste
{
    /// <summary>
    /// Tests the data proxy to a given data provider.
    /// </summary>
    [TestFixture]
    public class DataProviderProxyTests
    {
        /// <summary>
        /// Tests that the constructor initialize a data proxy to a given data provider.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeDataProviderProxy()
        {
            var dataProviderProxy = new DataProviderProxy();
            Assert.That(dataProviderProxy, Is.Not.Null);
            Assert.That(dataProviderProxy.Identifier, Is.Null);
            Assert.That(dataProviderProxy.Identifier.HasValue, Is.False);
            Assert.That(dataProviderProxy.Name, Is.Null);
            Assert.That(dataProviderProxy.DataSourceStatementIdentifier, Is.EqualTo(Guid.Empty));
            Assert.That(dataProviderProxy.DataSourceStatements, Is.Not.Null);
            Assert.That(dataProviderProxy.DataSourceStatements, Is.Empty);
        }

        /// <summary>
        /// Tests that getter for UniqueId throws an IntranetRepositoryException when the data provider has no identifier.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterThrowsIntranetRepositoryExceptionWhenDataProviderProxyHasNoIdentifier()
        {
            var dataProviderProxy = new DataProviderProxy
            {
                Identifier = null
            };

            // ReSharper disable UnusedVariable
            var exception = Assert.Throws<IntranetRepositoryException>(() => { var uniqueId = dataProviderProxy.UniqueId; });
            // ReSharper restore UnusedVariable
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, dataProviderProxy.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that getter for UniqueId gets the unique identifier for the data provider.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterGetsUniqueIdentificationForDataProviderProxy()
        {
            var dataProviderProxy = new DataProviderProxy
            {
                Identifier = Guid.NewGuid()
            };

            var uniqueId = dataProviderProxy.UniqueId;
            Assert.That(uniqueId, Is.Not.Null);
            Assert.That(uniqueId, Is.Not.Empty);
            Assert.That(uniqueId, Is.EqualTo(dataProviderProxy.Identifier.ToString().ToUpper()));
        }
    }
}
