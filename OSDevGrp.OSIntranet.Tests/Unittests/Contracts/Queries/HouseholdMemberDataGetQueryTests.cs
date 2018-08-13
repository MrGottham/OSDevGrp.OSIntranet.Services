using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Queries;
using AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Queries
{
    /// <summary>
    /// Tests the query for getting household member data for the current user.
    /// </summary>
    [TestFixture]
    public class HouseholdMemberDataGetQueryTests
    {
        /// <summary>
        /// Tests that the query for getting household member data for the current user can be initialized.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberDataGetQueryCanBeInitialized()
        {
            var fixture = new Fixture();
            var householdMemberDataGetQuery = fixture.Create<HouseholdMemberDataGetQuery>();
            DataContractTestHelper.TestAtContractErInitieret(householdMemberDataGetQuery);
        }

        /// <summary>
        /// Tests that the query for getting household member data for the current user can be serialized.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberDataGetQueryCanBeSerialized()
        {
            var fixture = new Fixture();
            var householdMemberDataGetQuery = fixture.Create<HouseholdMemberDataGetQuery>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(householdMemberDataGetQuery);
        }
    }
}
