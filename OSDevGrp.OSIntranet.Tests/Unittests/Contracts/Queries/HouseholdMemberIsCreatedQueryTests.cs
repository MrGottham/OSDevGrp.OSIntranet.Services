using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Queries;
using AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Queries
{
    /// <summary>
    /// Tests the query which can check whether the current user has been created as a household member.
    /// </summary>
    [TestFixture]
    public class HouseholdMemberIsCreatedQueryTests
    {
        /// <summary>
        /// Tests that the query which can check whether the current user has been created as a household member can be initialized.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberIsCreatedQueryCanBeInitialized()
        {
            var fixture = new Fixture();
            var householdMemberIsCreatedQuery = fixture.Create<HouseholdMemberIsCreatedQuery>();
            DataContractTestHelper.TestAtContractErInitieret(householdMemberIsCreatedQuery);
        }

        /// <summary>
        /// Tests that the query which can check whether the current user has been created as a household member can be serialized.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberIsCreatedQueryCanBeSerialized()
        {
            var fixture = new Fixture();
            var householdMemberIsCreatedQuery = fixture.Create<HouseholdMemberIsCreatedQuery>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(householdMemberIsCreatedQuery);
        }
    }
}
