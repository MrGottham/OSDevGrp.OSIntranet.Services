using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Commands;
using AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Commands
{
    /// <summary>
    /// Test the command for upgrading the membership on the current users household member account.
    /// </summary>
    [TestFixture]
    public class HouseholdMemberUpgradeMembershipCommandTests
    {
        /// <summary>
        /// Tests that the command for upgrading the membership on the current users household member account can be initialized.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberUpgradeMembershipCommandCanBeInitialized()
        {
            var fixture = new Fixture();
            var householdMemberUpgradeMembershipCommand = fixture.Create<HouseholdMemberUpgradeMembershipCommand>();
            DataContractTestHelper.TestAtContractErInitieret(householdMemberUpgradeMembershipCommand);
        }

        /// <summary>
        /// Tests that the command for upgrading the membership on the current users household member account can be serialized.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberUpgradeMembershipCommandCanBeSerialized()
        {
            var fixture = new Fixture();
            var householdMemberUpgradeMembershipCommand = fixture.Create<HouseholdMemberUpgradeMembershipCommand>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(householdMemberUpgradeMembershipCommand);
        }
    }
}
