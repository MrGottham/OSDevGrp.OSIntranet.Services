using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Commands;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Commands
{
    /// <summary>
    /// Tests the command for removing a household member from a given household on the current users household account.
    /// </summary>
    [TestFixture]
    public class HouseholdRemoveHouseholdMemberCommandTests
    {
        /// <summary>
        /// Tests that the command for removing a household member from a given household on the current users household account can be initialized.
        /// </summary>
        [Test]
        public void TestThatHouseholdRemoveHouseholdMemberCommandCanBeInitialized()
        {
            var fixture = new Fixture();
            var householdRemoveHouseholdMemberCommand = fixture.Create<HouseholdRemoveHouseholdMemberCommand>();
            DataContractTestHelper.TestAtContractErInitieret(householdRemoveHouseholdMemberCommand);
        }

        /// <summary>
        /// Tests that the command for removing a household member from a given household on the current users household account can be serialized.
        /// </summary>
        [Test]
        public void TestThatHouseholdRemoveHouseholdMemberCommandCanBeSerialized()
        {
            var fixture = new Fixture();
            var householdRemoveHouseholdMemberCommand = fixture.Create<HouseholdRemoveHouseholdMemberCommand>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(householdRemoveHouseholdMemberCommand);
        }
    }
}
