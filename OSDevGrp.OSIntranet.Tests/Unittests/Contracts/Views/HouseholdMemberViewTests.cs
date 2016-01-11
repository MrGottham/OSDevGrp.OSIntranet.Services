using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Views;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Views
{
    /// <summary>
    /// Tests the view for a household member.
    /// </summary>
    [TestFixture]
    public class HouseholdMemberViewTests
    {
        /// <summary>
        /// Tests that the view for a household member can be initialized.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberViewCanBeInitialized()
        {
            var fixture = new Fixture();
            var householdMemberView = fixture.Create<HouseholdMemberView>();
            DataContractTestHelper.TestAtContractErInitieret(householdMemberView);
        }

        /// <summary>
        /// Tests that the view for a household member can be serialized.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberViewCanBeSerialized()
        {
            var fixture = new Fixture();
            var householdMemberView = fixture.Create<HouseholdMemberView>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(householdMemberView);
        }
    }
}
