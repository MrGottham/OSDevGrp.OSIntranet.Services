using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Queries;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Queries
{
    /// <summary>
    /// Tests the query which can check whether the current user has been activated.
    /// </summary>
    [TestFixture]
    public class HouseholdMemberIsActivatedQueryTests
    {
        /// <summary>
        /// Tests that the query which can check whether the current user has been activated can be initialized.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberIsActivatedQueryCanBeInitialized()
        {
            var fixture = new Fixture();
            var householdMemberIsActivatedQuery = fixture.Create<HouseholdMemberIsActivatedQuery>();
            DataContractTestHelper.TestAtContractErInitieret(householdMemberIsActivatedQuery);
        }

        /// <summary>
        /// Tests that the query which can check whether the current user has been activated can be serialized.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberIsActivatedQueryCanBeSerialized()
        {
            var fixture = new Fixture();
            var householdMemberIsActivatedQuery = fixture.Create<HouseholdMemberIsActivatedQuery>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(householdMemberIsActivatedQuery);
        }
    }
}
