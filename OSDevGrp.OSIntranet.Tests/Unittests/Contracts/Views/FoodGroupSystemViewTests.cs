using System.Collections.Generic;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Views;
using AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Views
{
    /// <summary>
    /// Tests the system view for a food group.
    /// </summary>
    [TestFixture]
    public class FoodGroupSystemViewTests
    {
        /// <summary>
        /// Tests that the system view for a food group can be initialized.
        /// </summary>
        [Test]
        public void TestThatFoodGroupSystemViewCanBeInitialized()
        {
            var fixture = new Fixture();
            var foodGroupSystemView = fixture.Build<FoodGroupSystemView>()
                .With(m => m.Parent, fixture.Build<FoodGroupSystemView>()
                    .With(m => m.Children, new List<FoodGroupSystemView>())
                    .Create())
                .With(m => m.Children, new List<FoodGroupSystemView>())
                .Create();
            DataContractTestHelper.TestAtContractErInitieret(foodGroupSystemView);
        }

        /// <summary>
        /// Tests that the system view for a food group can be serialized.
        /// </summary>
        [Test]
        public void TestThatFoodGroupSystemViewCanBeSerialized()
        {
            var fixture = new Fixture();
            var foodGroupSystemView = fixture.Build<FoodGroupSystemView>()
                .With(m => m.Parent, fixture.Build<FoodGroupSystemView>()
                    .With(m => m.Children, new List<FoodGroupSystemView>())
                    .Create())
                .With(m => m.Children, new List<FoodGroupSystemView>())
                .Create();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(foodGroupSystemView);
        }
    }
}
