using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Views;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Views
{
    /// <summary>
    /// Tests the view for a collection of food items.
    /// </summary>
    [TestFixture]
    public class FoodItemCollectionViewTests
    {
        /// <summary>
        /// Tests that the view for a collection of food items can be initialized.
        /// </summary>
        [Test]
        public void TestThatFoodItemCollectionViewCanBeInitialized()
        {
            var fixture = new Fixture();
            var foodItemCollectionView = fixture.Create<FoodItemCollectionView>();
            DataContractTestHelper.TestAtContractErInitieret(foodItemCollectionView);
        }

        /// <summary>
        /// Tests that the view for a collection of food items can be serialized.
        /// </summary>
        [Test]
        public void TestThatFoodItemCollectionViewCanBeSerialized()
        {
            var fixture = new Fixture();
            var foodItemCollectionView = fixture.Create<FoodItemCollectionView>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(foodItemCollectionView);
        }
    }
}
