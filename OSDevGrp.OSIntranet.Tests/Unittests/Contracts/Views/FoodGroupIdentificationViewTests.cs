using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Views;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Views
{
    /// <summary>
    /// Tests the identification view for a food group.
    /// </summary>
    [TestFixture]
    public class FoodGroupIdentificationViewTests
    {
        /// <summary>
        /// Tests that the identification view for a food group can be initialized.
        /// </summary>
        [Test]
        public void TestThatFoodGroupIdentificationViewCanBeInitialized()
        {
            var fixture = new Fixture();
            var foodGroupIdentificationView = fixture.Create<FoodGroupIdentificationView>();
            DataContractTestHelper.TestAtContractErInitieret(foodGroupIdentificationView);
        }

        /// <summary>
        /// Tests that the identification view for a food group can be serialized.
        /// </summary>
        [Test]
        public void TestThatFoodGroupIdentificationViewCanBeSerialized()
        {
            var fixture = new Fixture();
            var foodGroupIdentificationView = fixture.Create<FoodGroupIdentificationView>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(foodGroupIdentificationView);
        }
    }
}
