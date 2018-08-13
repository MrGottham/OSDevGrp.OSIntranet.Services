using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Commands;
using AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Commands
{
    /// <summary>
    /// Tests the command for importing a food group from a given data provider.
    /// </summary>
    [TestFixture]
    public class FoodGroupImportFromDataProviderCommandTests
    {
        /// <summary>
        /// Tests that the for importing a food group from a given data provider can be initialized.
        /// </summary>
        [Test]
        public void TestThatFoodGroupImportFromDataProviderCommandCanBeInitialized()
        {
            var fixture = new Fixture();
            var foodGroupImportFromDataProviderCommand = fixture.Create<FoodGroupImportFromDataProviderCommand>();
            DataContractTestHelper.TestAtContractErInitieret(foodGroupImportFromDataProviderCommand);
        }

        /// <summary>
        /// Tests that the for importing a food group from a given data provider can be serialized.
        /// </summary>
        [Test]
        public void TestThatFoodGroupImportFromDataProviderCommandCanBeSerialized()
        {
            var fixture = new Fixture();
            var foodGroupImportFromDataProviderCommand = fixture.Create<FoodGroupImportFromDataProviderCommand>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(foodGroupImportFromDataProviderCommand);
        }
    }
}
