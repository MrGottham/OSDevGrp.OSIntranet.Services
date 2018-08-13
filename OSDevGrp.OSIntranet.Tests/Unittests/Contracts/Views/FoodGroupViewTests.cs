using System.Collections.Generic;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Views;
using AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Views
{
    /// <summary>
    /// Tests the view for a food group.
    /// </summary>
    [TestFixture]
    public class FoodGroupViewTests
    {
        /// <summary>
        /// Tests that the view for a food group can be initialized.
        /// </summary>
        [Test]
        public void TestThatFoodGroupViewCanBeInitialized()
        {
            var fixture = new Fixture();
            var foodGroupView = fixture.Build<FoodGroupView>()
                .With(m => m.Parent, fixture.Build<FoodGroupView>()
                    .With(m => m.Children, new List<FoodGroupView>())
                    .Create())
                .With(m => m.Children, new List<FoodGroupView>())
                .Create();
            DataContractTestHelper.TestAtContractErInitieret(foodGroupView);
        }

        /// <summary>
        /// Tests that the view for a food group can be serialized.
        /// </summary>
        [Test]
        public void TestThatFoodGroupSystemViewCanBeSerialized()
        {
            var fixture = new Fixture();
            var foodGroupView = fixture.Build<FoodGroupView>()
                .With(m => m.Parent, fixture.Build<FoodGroupView>()
                    .With(m => m.Children, new List<FoodGroupView>())
                    .Create())
                .With(m => m.Children, new List<FoodGroupView>())
                .Create();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(foodGroupView);
        }
    }
}
