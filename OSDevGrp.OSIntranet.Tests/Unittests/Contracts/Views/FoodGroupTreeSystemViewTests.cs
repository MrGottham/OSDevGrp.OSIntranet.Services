using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Views;
using AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Views
{
    /// <summary>
    /// Tests the system view for a tree of food groups.
    /// </summary>
    [TestFixture]
    public class FoodGroupTreeSystemViewTests
    {
        /// <summary>
        /// Tests that the system view for a tree of food groups can be initialized.
        /// </summary>
        [Test]
        public void TestThatFoodGroupTreeSystemViewCanBeInitialized()
        {
            var fixture = new Fixture();
            fixture.Inject(fixture.Build<FoodGroupSystemView>().With(m => m.Parent, fixture.Create<FoodGroupIdentificationView>()).With(m => m.Children, new List<FoodGroupSystemView>(0)).Create());

            var foodGroupTreeSystemView = fixture.Build<FoodGroupTreeSystemView>()
                .With(m => m.FoodGroups, fixture.CreateMany<FoodGroupSystemView>(7).ToList())
                .With(m => m.DataProvider, fixture.Create<DataProviderView>())
                .Create();
            DataContractTestHelper.TestAtContractErInitieret(foodGroupTreeSystemView);
        }

        /// <summary>
        /// Tests that the system view for a food group can be serialized.
        /// </summary>
        [Test]
        public void TestThatFoodGroupTreeSystemViewCanBeSerialized()
        {
            var fixture = new Fixture();
            fixture.Inject(fixture.Build<FoodGroupSystemView>().With(m => m.Parent, fixture.Create<FoodGroupIdentificationView>()).With(m => m.Children, new List<FoodGroupSystemView>(0)).Create());

            var foodGroupTreeSystemView = fixture.Build<FoodGroupTreeSystemView>()
                .With(m => m.FoodGroups, fixture.CreateMany<FoodGroupSystemView>(7).ToList())
                .With(m => m.DataProvider, fixture.Create<DataProviderView>())
                .Create();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(foodGroupTreeSystemView);
        }
    }
}
