using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Security.Authorization;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Security.Authorization
{
    /// <summary>
    /// Tests the manager which checks authorization access checking for service operations.
    /// </summary>
    [TestFixture]
    public class ServiceAuthorizationManagerTests
    {
        /// <summary>
        /// Tests that the constructor initialize a manager which checks authorization access checking for service operations.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeServiceAuthorizationManager()
        {
            var serviceAuthorizationManager = new ServiceAuthorizationManager();
            Assert.That(serviceAuthorizationManager, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException if the functionality which handles the authorization is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionIfAuthorizationHandlerIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ServiceAuthorizationManager(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("authorizationHandler"));
            Assert.That(exception.InnerException, Is.Null);
        }
    }
}
