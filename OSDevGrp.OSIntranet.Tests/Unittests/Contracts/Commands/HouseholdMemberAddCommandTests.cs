using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Commands;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Commands
{
    /// <summary>
    /// Tests the command for adding a household member.
    /// </summary>
    [TestFixture]
    public class HouseholdMemberAddCommandTests
    {
        /// <summary>
        /// Tests that the command for adding a household member can be initialized.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberAddCommandCanBeInitialized()
        {
            var fixture = new Fixture();
            var householdMemberAddCommand = fixture.Create<HouseholdMemberAddCommand>();
            DataContractTestHelper.TestAtContractErInitieret(householdMemberAddCommand);
        }

        /// <summary>
        /// Tests that the command for adding a household member can be serialized.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberAddCommandCanBeSerialized()
        {
            var fixture = new Fixture();
            var householdMemberAddCommand = fixture.Create<HouseholdMemberAddCommand>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(householdMemberAddCommand);
        }
    }
}
