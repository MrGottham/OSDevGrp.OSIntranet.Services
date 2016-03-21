using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Commands;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Commands
{
    /// <summary>
    /// Tests the command for updatering a household to the current users household account.
    /// </summary>
    [TestFixture]
    public class HouseholdUpdateCommandTests
    {
        /// <summary>
        /// Tests that the command for updatering a household to the current users household account can be initialized.
        /// </summary>
        [Test]
        public void TestThatHouseholdUpdateCommandCanBeInitialized()
        {
            var fixture = new Fixture();
            var householdUpdateCommand = fixture.Create<HouseholdUpdateCommand>();
            DataContractTestHelper.TestAtContractErInitieret(householdUpdateCommand);
        }

        /// <summary>
        /// Tests that the command for updatering a household to the current users household account can be serialized.
        /// </summary>
        [Test]
        public void TestThatHouseholdUpdateCommandCanBeSerialized()
        {
            var fixture = new Fixture();
            var householdUpdateCommand = fixture.Create<HouseholdUpdateCommand>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(householdUpdateCommand);
        }
    }
}
