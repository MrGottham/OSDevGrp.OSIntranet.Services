using NUnit.Framework;
using OSDevGrp.OSIntranet.Security.AuthorizationPolicies;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Security.AuthorizationPolicies
{
    /// <summary>
    /// Tests the functionality which can handle the authorization policy.
    /// </summary>
    [TestFixture]
    public class AuthorizationPolicyHandlerTests
    {
        /// <summary>
        /// Tests that the constructor initalize functionality which can handle the authorization policy.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeAuthorizationPolicyHandler()
        {
            var authorizationPolicyHandler = new AuthorizationPolicyHandler();
            Assert.That(authorizationPolicyHandler, Is.Not.Null);
        }
    }
}
