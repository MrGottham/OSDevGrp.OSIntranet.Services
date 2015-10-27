using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Commands;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Commands
{
    /// <summary>
    /// Tests the command for importing a food item from a given data provider.
    /// </summary>
    [TestFixture]
    public class FoodItemImportFromDataProviderCommandTests
    {
        /// <summary>
        /// Tests that the command for importing a food item from a given data provider can be initialized.
        /// </summary>
        [Test]
        public void TestThatFoodItemImportFromDataProviderCommandCanBeInitialized()
        {
            var fixture = new Fixture();
            var foodItemImportFromDataProviderCommand = fixture.Create<FoodItemImportFromDataProviderCommand>();
            DataContractTestHelper.TestAtContractErInitieret(foodItemImportFromDataProviderCommand);
        }

        /// <summary>
        /// Tests that the command for importing a food item from a given data provider can be serialized.
        /// </summary>
        [Test]
        public void TestThatFoodItemImportFromDataProviderCommandCanBeSerialized()
        {
            var fixture = new Fixture();
            var foodItemImportFromDataProviderCommand = fixture.Create<FoodItemImportFromDataProviderCommand>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(foodItemImportFromDataProviderCommand);
        }
    }
}
