using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Queries;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Queries
{
    /// <summary>
    /// Tests the query which can check whether the current user has accepted the privacy policy.
    /// </summary>
    [TestFixture]
    public class HouseholdMemberHasAcceptedPrivacyPolicyQueryTests
    {
        /// <summary>
        /// Tests that the query which can check whether the current user has accepted the privacy policy can be initialized.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberHasAcceptedPrivacyPolicyQueryCanBeInitialized()
        {
            var fixture = new Fixture();
            var householdMemberHasAcceptedPrivacyPolicyQuery = fixture.Create<HouseholdMemberHasAcceptedPrivacyPolicyQuery>();
            DataContractTestHelper.TestAtContractErInitieret(householdMemberHasAcceptedPrivacyPolicyQuery);
        }

        /// <summary>
        /// Tests that the query which can check whether the current user has accepted the privacy policy can be serialized.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberHasAcceptedPrivacyPolicyQueryCanBeSerialized()
        {
            var fixture = new Fixture();
            var householdMemberHasAcceptedPrivacyPolicyQuery = fixture.Create<HouseholdMemberHasAcceptedPrivacyPolicyQuery>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(householdMemberHasAcceptedPrivacyPolicyQuery);
        }
    }
}
