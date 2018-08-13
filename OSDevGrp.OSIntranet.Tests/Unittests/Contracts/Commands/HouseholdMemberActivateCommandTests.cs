using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Commands;
using AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Commands
{
    /// <summary>
    /// Test the command for activating the current users household member account.
    /// </summary>
    [TestFixture]
    public class HouseholdMemberActivateCommandTests
    {
        /// <summary>
        /// Tests that the command for activating the current users household member account can be initialized.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberActivateCommandCanBeInitialized()
        {
            var fixture = new Fixture();
            var householdMemberActivateCommand = fixture.Create<HouseholdMemberActivateCommand>();
            DataContractTestHelper.TestAtContractErInitieret(householdMemberActivateCommand);
        }

        /// <summary>
        /// Tests that the command for activating the current users household member account can be serialized.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberActivateCommandCanBeSerialized()
        {
            var fixture = new Fixture();
            var householdMemberActivateCommand = fixture.Create<HouseholdMemberActivateCommand>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(householdMemberActivateCommand);
        }
    }
}
