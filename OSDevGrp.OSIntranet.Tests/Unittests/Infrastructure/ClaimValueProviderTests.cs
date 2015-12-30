using NUnit.Framework;
using OSDevGrp.OSIntranet.Infrastructure;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Infrastructure
{
    /// <summary>
    /// Test the provider which can resolve values from the current users claims.
    /// </summary>
    [TestFixture]
    public class ClaimValueProviderTests
    {
        /// <summary>
        /// Tests that the constructor initialize the provider which can resolve values from the current users claims.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeClaimValueProvider()
        {
            var claimValueProvider = new ClaimValueProvider();
            Assert.That(claimValueProvider, Is.Not.Null);
            Assert.That(claimValueProvider.MailAddress, Is.Not.Null);
        }
    }
}
