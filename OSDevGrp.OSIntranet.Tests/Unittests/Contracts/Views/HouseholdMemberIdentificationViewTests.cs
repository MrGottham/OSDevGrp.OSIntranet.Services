using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Views;
using AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Views
{
    /// <summary>
    /// Tests the identification view for a household member.
    /// </summary>
    [TestFixture]
    public class HouseholdMemberIdentificationViewTests
    {
        /// <summary>
        /// Tests that the identification view for a household member can be initialized.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberIdentificationViewCanBeInitialized()
        {
            var fixture = new Fixture();
            var householdMemberIdentificationView = fixture.Create<HouseholdMemberIdentificationView>();
            DataContractTestHelper.TestAtContractErInitieret(householdMemberIdentificationView);
        }

        /// <summary>
        /// Tests that the identification view for a household member can be serialized.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberIdentificationViewCanBeSerialized()
        {
            var fixture = new Fixture();
            var householdMemberIdentificationView = fixture.Create<HouseholdMemberIdentificationView>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(householdMemberIdentificationView);
        }
    }
}
