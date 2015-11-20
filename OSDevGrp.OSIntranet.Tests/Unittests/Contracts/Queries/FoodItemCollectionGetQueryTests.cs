using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Queries;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Queries
{
    /// <summary>
    /// Tests the query for getting the collection of food items.
    /// </summary>
    [TestFixture]
    public class FoodItemCollectionGetQueryTests
    {
        /// <summary>
        /// Tests that the query for getting the collection of food items can be initialized.
        /// </summary>
        [Test]
        public void TestThatFoodItemCollectionGetQueryCanBeInitialized()
        {
            var fixture = new Fixture();
            var foodItemCollectionGetQuery = fixture.Create<FoodItemCollectionGetQuery>();
            DataContractTestHelper.TestAtContractErInitieret(foodItemCollectionGetQuery);
        }

        /// <summary>
        /// Tests that the query for getting the collection of food items can be serialized.
        /// </summary>
        [Test]
        public void TestThatFoodItemCollectionGetQueryCanBeSerialized()
        {
            var fixture = new Fixture();
            var foodItemCollectionGetQuery = fixture.Create<FoodItemCollectionGetQuery>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(foodItemCollectionGetQuery);
        }
    }
}
