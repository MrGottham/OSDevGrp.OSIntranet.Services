using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Commands;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Commands
{
    /// <summary>
    /// Tests the command for adding a household member to a given household on the current users household account.
    /// </summary>
    [TestFixture]
    public class HouseholdAddHouseholdMemberCommandTests
    {
        /// <summary>
        /// Tests that the command for adding a household member to a given household on the current users household account can be initialized.
        /// </summary>
        [Test]
        public void TestThatHouseholdAddHouseholdMemberCommandCanBeInitialized()
        {
            var fixture = new Fixture();
            var householdAddHouseholdMemberCommand = fixture.Create<HouseholdAddHouseholdMemberCommand>();
            DataContractTestHelper.TestAtContractErInitieret(householdAddHouseholdMemberCommand);
        }

        /// <summary>
        /// Tests that the command for adding a household member to a given household on the current users household account can be serialized.
        /// </summary>
        [Test]
        public void TestThatHouseholdAddHouseholdMemberCommandCanBeSerialized()
        {
            var fixture = new Fixture();
            var householdAddHouseholdMemberCommand = fixture.Create<HouseholdAddHouseholdMemberCommand>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(householdAddHouseholdMemberCommand);
        }
    }
}
