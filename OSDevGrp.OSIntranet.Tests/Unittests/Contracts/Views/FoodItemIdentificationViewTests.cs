using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Views;
using AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Views
{
    /// <summary>
    /// Tests the identification view for a food item.
    /// </summary>
    [TestFixture]
    public class FoodItemIdentificationViewTests
    {
        /// <summary>
        /// Tests that the identification view for a food item can be initialized.
        /// </summary>
        [Test]
        public void TestThatFoodItemIdentificationViewCanBeInitialized()
        {
            var fixture = new Fixture();
            var foodItemIdentificationView = fixture.Create<FoodItemIdentificationView>();
            DataContractTestHelper.TestAtContractErInitieret(foodItemIdentificationView);
        }

        /// <summary>
        /// Tests that the identification view for a food item can be serialized.
        /// </summary>
        [Test]
        public void TestThatFoodItemIdentificationViewCanBeSerialized()
        {
            var fixture = new Fixture();
            var foodItemIdentificationView = fixture.Create<FoodItemIdentificationView>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(foodItemIdentificationView);
        }
    }
}
