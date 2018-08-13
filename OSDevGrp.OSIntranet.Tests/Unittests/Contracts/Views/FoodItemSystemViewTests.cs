using System.Collections.Generic;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Views;
using AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Views
{
    /// <summary>
    /// Tests the system view for a food item.
    /// </summary>
    [TestFixture]
    public class FoodItemSystemViewTests
    {
        /// <summary>
        /// Tests that the system view for a food item can be initialized.
        /// </summary>
        [Test]
        public void TestThatFoodItemSystemViewCanBeInitialized()
        {
            var fixture = new Fixture();
            var foodGroupSystemView = fixture.Build<FoodGroupSystemView>()
                .With(m => m.Children, new List<FoodGroupSystemView>(0))
                .Create();
            var foodItemSystemView = fixture.Build<FoodItemSystemView>()
                .With(m => m.FoodGroups, new List<FoodGroupSystemView> {foodGroupSystemView})
                .Create();
            DataContractTestHelper.TestAtContractErInitieret(foodItemSystemView);
        }

        /// <summary>
        /// Tests that the system view for a food item can be serialized.
        /// </summary>
        [Test]
        public void TestThatFoodItemSystemViewCanBeSerialized()
        {
            var fixture = new Fixture();
            var foodGroupSystemView = fixture.Build<FoodGroupSystemView>()
                .With(m => m.Children, new List<FoodGroupSystemView>(0))
                .Create();
            var foodItemSystemView = fixture.Build<FoodItemSystemView>()
                .With(m => m.FoodGroups, new List<FoodGroupSystemView> {foodGroupSystemView})
                .Create();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(foodItemSystemView);
        }
    }
}
