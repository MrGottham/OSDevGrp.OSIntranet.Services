using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Queries;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Queries
{
    /// <summary>
    /// Tests the for getting household member data for the current user
    /// </summary>
    [TestFixture]
    public class HouseholdMemberDataGetQueryTests
    {
        /// <summary>
        /// Tests that for getting household member data for the current user can be initialized.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberHasAcceptedPrivacyPolicyQueryCanBeInitialized()
        {
            var fixture = new Fixture();
            var householdMemberDataGetQuery = fixture.Create<HouseholdMemberDataGetQuery>();
            DataContractTestHelper.TestAtContractErInitieret(householdMemberDataGetQuery);
        }

        /// <summary>
        /// Tests that for getting household member data for the current user can be serialized.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberHasAcceptedPrivacyPolicyQueryCanBeSerialized()
        {
            var fixture = new Fixture();
            var householdMemberDataGetQuery = fixture.Create<HouseholdMemberDataGetQuery>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(householdMemberDataGetQuery);
        }
    }
}
