using System.Collections.Generic;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Views;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Views
{
    /// <summary>
    /// Tests the system view for a collection of food items.
    /// </summary>
    [TestFixture]
    public class FoodItemCollectionSystemViewTests
    {
        /// <summary>
        /// Tests that the system view for a collection of food items can be initialized.
        /// </summary>
        [Test]
        public void TestThatFoodItemCollectionSystemViewCanBeInitialized()
        {
            var fixture = new Fixture();
            var foodItemSystemView = fixture.Build<FoodItemSystemView>()
                .With(m => m.FoodGroups, new List<FoodGroupSystemView>(0))
                .Create();
            var foodItemCollectionSystemView = fixture.Build<FoodItemCollectionSystemView>()
                .With(m => m.FoodItems, new List<FoodItemSystemView> {foodItemSystemView})
                .Create();
            DataContractTestHelper.TestAtContractErInitieret(foodItemCollectionSystemView);
        }

        /// <summary>
        /// Tests that the system view for a collection of food items can be serialized.
        /// </summary>
        [Test]
        public void TestThatFoodItemCollectionSystemViewCanBeSerialized()
        {
            var fixture = new Fixture();
            var foodItemSystemView = fixture.Build<FoodItemSystemView>()
                .With(m => m.FoodGroups, new List<FoodGroupSystemView>(0))
                .Create();
            var foodItemCollectionSystemView = fixture.Build<FoodItemCollectionSystemView>()
                .With(m => m.FoodItems, new List<FoodItemSystemView> {foodItemSystemView})
                .Create();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(foodItemCollectionSystemView);
        }
    }
}
