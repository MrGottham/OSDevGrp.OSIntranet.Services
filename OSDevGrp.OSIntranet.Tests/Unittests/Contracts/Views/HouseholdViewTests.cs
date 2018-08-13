using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Views;
using AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Views
{
    /// <summary>
    /// Tests the view for a household.
    /// </summary>
    [TestFixture]
    public class HouseholdViewTests
    {
        /// <summary>
        /// Tests that the view for a household can be initialized.
        /// </summary>
        [Test]
        public void TestThatHouseholdViewCanBeInitialized()
        {
            var fixture = new Fixture();
            var householdView = fixture.Create<HouseholdView>();
            DataContractTestHelper.TestAtContractErInitieret(householdView);
        }

        /// <summary>
        /// Tests that the view for a household can be serialized.
        /// </summary>
        [Test]
        public void TestThatHouseholdViewCanBeSerialized()
        {
            var fixture = new Fixture();
            var householdView = fixture.Create<HouseholdView>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(householdView);
        }
    }
}
