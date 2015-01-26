using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Security.Authorization;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Security.Authorization
{
    /// <summary>
    /// Tests the functionality which checks authorization access for a secure token on each service operation.
    /// </summary>
    [TestFixture]
    public class SecureTokenAuthorizerTests
    {
        /// <summary>
        /// Tests that the constructor initialize functionality which checks authorization access for a secure token on each service operation..
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeServiceSecureTokenAuthorizer()
        {
            var secureTokenAuthorizer = new SecureTokenAuthorizer();
            Assert.That(secureTokenAuthorizer, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException if the functionality which handles the authorization is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionIfAuthorizationHandlerIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new SecureTokenAuthorizer(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("authorizationHandler"));
            Assert.That(exception.InnerException, Is.Null);
        }
    }
}
