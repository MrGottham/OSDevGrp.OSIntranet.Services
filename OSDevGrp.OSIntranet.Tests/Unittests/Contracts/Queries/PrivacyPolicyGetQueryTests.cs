using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Queries;
using AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Queries
{
    /// <summary>
    /// Tests the query for getting the privacy policy.
    /// </summary>
    [TestFixture]
    public class PrivacyPolicyGetQueryTests
    {
        /// <summary>
        /// Tests that the query for getting the privacy policy can be initialized.
        /// </summary>
        [Test]
        public void TestThatPrivacyPolicyGetQueryCanBeInitialized()
        {
            var fixture = new Fixture();
            var privacyPolicyGetQuery = fixture.Create<PrivacyPolicyGetQuery>();
            DataContractTestHelper.TestAtContractErInitieret(privacyPolicyGetQuery);
        }

        /// <summary>
        /// Tests that the query for getting the privacy policy can be serialized.
        /// </summary>
        [Test]
        public void TestThatPrivacyPolicyGetQueryCanBeSerialized()
        {
            var fixture = new Fixture();
            var privacyPolicyGetQuery = fixture.Create<PrivacyPolicyGetQuery>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(privacyPolicyGetQuery);
        }
    }
}
