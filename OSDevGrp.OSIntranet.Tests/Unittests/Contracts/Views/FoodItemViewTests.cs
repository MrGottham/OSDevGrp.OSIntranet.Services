using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Views;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Views
{
    /// <summary>
    /// Tests the view for a food item.
    /// </summary>
    [TestFixture]
    public class FoodItemViewTests
    {
        /// <summary>
        /// Tests that the view for a food item can be initialized.
        /// </summary>
        [Test]
        public void TestThatFoodItemViewCanBeInitialized()
        {
            var fixture = new Fixture();
            var foodItemView = fixture.Create<FoodItemView>();
            DataContractTestHelper.TestAtContractErInitieret(foodItemView);
        }

        /// <summary>
        /// Tests that the view for a food item can be serialized.
        /// </summary>
        [Test]
        public void TestThatFoodItemViewCanBeSerialized()
        {
            var fixture = new Fixture();
            var foodItemView = fixture.Create<FoodItemView>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(foodItemView);
        }
    }
}
