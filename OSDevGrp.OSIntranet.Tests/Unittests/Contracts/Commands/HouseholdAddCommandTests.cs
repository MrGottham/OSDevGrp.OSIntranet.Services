using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Commands;
using AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Commands
{
    /// <summary>
    /// Tests the command for adding a household to the current users household account.
    /// </summary>
    [TestFixture]
    public class HouseholdAddCommandTests
    {
        /// <summary>
        /// Tests that the command for adding a household to the current users household account can be initialized.
        /// </summary>
        [Test]
        public void TestThatHouseholdAddCommandCanBeInitialized()
        {
            var fixture = new Fixture();
            var householdAddCommand = fixture.Create<HouseholdAddCommand>();
            DataContractTestHelper.TestAtContractErInitieret(householdAddCommand);
        }

        /// <summary>
        /// Tests that the command for adding a household to the current users household account can be serialized.
        /// </summary>
        [Test]
        public void TestThatHouseholdAddCommandCanBeSerialized()
        {
            var fixture = new Fixture();
            var householdAddCommand = fixture.Create<HouseholdAddCommand>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(householdAddCommand);
        }
    }
}
