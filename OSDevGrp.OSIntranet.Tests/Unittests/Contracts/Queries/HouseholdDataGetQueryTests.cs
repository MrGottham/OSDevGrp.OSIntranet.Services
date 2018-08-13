using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Queries;
using AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Queries
{
    /// <summary>
    /// Tests the query for getting household data for one of the current user households.
    /// </summary>
    [TestFixture]
    public class HouseholdDataGetQueryTests
    {
        /// <summary>
        /// Tests that the query for getting household data for one of the current user households can be initialized.
        /// </summary>
        [Test]
        public void TestThatHouseholdDataGetQueryCanBeInitialized()
        {
            var fixture = new Fixture();
            var householdDataGetQuery = fixture.Create<HouseholdDataGetQuery>();
            DataContractTestHelper.TestAtContractErInitieret(householdDataGetQuery);
        }

        /// <summary>
        /// Tests that the getting household data for one of the current user households can be serialized.
        /// </summary>
        [Test]
        public void TestThatHouseholdDataGetQueryCanBeSerialized()
        {
            var fixture = new Fixture();
            var householdDataGetQuery = fixture.Create<HouseholdDataGetQuery>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(householdDataGetQuery);
        }
    }
}
