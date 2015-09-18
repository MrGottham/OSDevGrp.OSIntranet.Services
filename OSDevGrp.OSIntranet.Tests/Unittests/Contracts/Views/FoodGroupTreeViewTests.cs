using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Views;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Views
{
    /// <summary>
    /// Tests the view for a tree of food groups.
    /// </summary>
    [TestFixture]
    public class FoodGroupTreeViewTests
    {
        /// <summary>
        /// Tests that the view for a tree of food groups can be initialized.
        /// </summary>
        [Test]
        public void TestThatFoodGroupTreeViewCanBeInitialized()
        {
            var fixture = new Fixture();
            fixture.Inject(fixture.Build<FoodGroupView>().With(m => m.Parent, fixture.Create<FoodGroupIdentificationView>()).With(m => m.Children, new List<FoodGroupView>(0)).Create());

            var foodGroupTreeView = fixture.Build<FoodGroupTreeView>()
                .With(m => m.FoodGroups, fixture.CreateMany<FoodGroupView>(7).ToList())
                .With(m => m.DataProvider, fixture.Create<DataProviderView>())
                .Create();
            DataContractTestHelper.TestAtContractErInitieret(foodGroupTreeView);
        }

        /// <summary>
        /// Tests that the view for a food group can be serialized.
        /// </summary>
        [Test]
        public void TestThatFoodGroupTreeViewCanBeSerialized()
        {
            var fixture = new Fixture();
            fixture.Inject(fixture.Build<FoodGroupView>().With(m => m.Parent, fixture.Create<FoodGroupIdentificationView>()).With(m => m.Children, new List<FoodGroupView>(0)).Create());

            var foodGroupTreeView = fixture.Build<FoodGroupTreeView>()
                .With(m => m.FoodGroups, fixture.CreateMany<FoodGroupView>(7).ToList())
                .With(m => m.DataProvider, fixture.Create<DataProviderView>())
                .Create();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(foodGroupTreeView);
        }
    }
}
