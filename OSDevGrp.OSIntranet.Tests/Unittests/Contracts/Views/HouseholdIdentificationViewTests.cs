using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Views;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Views
{
    /// <summary>
    /// Tests the identification view for a household.
    /// </summary>
    [TestFixture]
    public class HouseholdIdentificationViewTests
    {
        /// <summary>
        /// Tests that the identification view for a household can be initialized.
        /// </summary>
        [Test]
        public void TestThatHouseholdIdentificationViewCanBeInitialized()
        {
            var fixture = new Fixture();
            var householdIdentificationView = fixture.Create<HouseholdIdentificationView>();
            DataContractTestHelper.TestAtContractErInitieret(householdIdentificationView);
        }

        /// <summary>
        /// Tests that the identification view for a household can be serialized.
        /// </summary>
        [Test]
        public void TestThatHouseholdIdentificationViewCanBeSerialized()
        {
            var fixture = new Fixture();
            var householdIdentificationView = fixture.Create<HouseholdIdentificationView>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(householdIdentificationView);
        }
    }
}
