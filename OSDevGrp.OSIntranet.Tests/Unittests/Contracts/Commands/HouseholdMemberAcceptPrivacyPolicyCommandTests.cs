using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Commands;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Commands
{
    /// <summary>
    /// Test the command for accepting privacy policy on the current users household member account.
    /// </summary>
    [TestFixture]
    public class HouseholdMemberAcceptPrivacyPolicyCommandTests
    {
        /// <summary>
        /// Tests that the command for accepting privacy policy on the current users household member account can be initialized.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberAcceptPrivacyPolicyCommandCanBeInitialized()
        {
            var fixture = new Fixture();
            var householdMemberAcceptPrivacyPolicyCommand = fixture.Create<HouseholdMemberAcceptPrivacyPolicyCommand>();
            DataContractTestHelper.TestAtContractErInitieret(householdMemberAcceptPrivacyPolicyCommand);
        }

        /// <summary>
        /// Tests that the command for accepting privacy policy on the current users household member account can be serialized.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberAcceptPrivacyPolicyCommandCanBeSerialized()
        {
            var fixture = new Fixture();
            var householdMemberAcceptPrivacyPolicyCommand = fixture.Create<HouseholdMemberAcceptPrivacyPolicyCommand>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(householdMemberAcceptPrivacyPolicyCommand);
        }
    }
}
