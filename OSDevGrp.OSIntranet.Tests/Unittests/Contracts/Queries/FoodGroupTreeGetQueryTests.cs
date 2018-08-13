using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Queries;
using AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Queries
{
    /// <summary>
    /// Tests the query for getting the tree of food groups.
    /// </summary>
    [TestFixture]
    public class FoodGroupTreeGetQueryTests
    {
        /// <summary>
        /// Tests that the query for getting the tree of food groups can be initialized.
        /// </summary>
        [Test]
        public void TestThatFoodGroupTreeGetQueryCanBeInitialized()
        {
            var fixture = new Fixture();
            var foodGroupTreeGetQuery = fixture.Create<FoodGroupTreeGetQuery>();
            DataContractTestHelper.TestAtContractErInitieret(foodGroupTreeGetQuery);
        }

        /// <summary>
        /// Tests that the query for getting the tree of food groups can be serialized.
        /// </summary>
        [Test]
        public void TestThatFoodGroupTreeGetQueryCanBeSerialized()
        {
            var fixture = new Fixture();
            var foodGroupTreeGetQuery = fixture.Create<FoodGroupTreeGetQuery>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(foodGroupTreeGetQuery);
        }
    }
}
