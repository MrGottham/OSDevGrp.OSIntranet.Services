using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Faults;
using AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Faults
{
    /// <summary>
    /// Tests the fault which can be used in the food waste domain.
    /// </summary>
    [TestFixture]
    public class FoodWasteFaultTests
    {
        /// <summary>
        /// Tests that the fault which can be used in the food waste domain can be initialized.
        /// </summary>
        [Test]
        public void TestThatFoodWasteFaultCanBeInitialized()
        {
            var fixture = new Fixture();
            var foodWasteFault = fixture.Create<FoodWasteFault>();
            DataContractTestHelper.TestAtContractErInitieret(foodWasteFault);
        }

        /// <summary>
        /// Tests that the fault which can be used in the food waste domain can be serialized.
        /// </summary>
        [Test]
        public void TestThatFoodWasteFaultGetQueryCanBeSerialized()
        {
            var fixture = new Fixture();
            var foodWasteFault = fixture.Create<FoodWasteFault>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(foodWasteFault);
        }
    }
}
