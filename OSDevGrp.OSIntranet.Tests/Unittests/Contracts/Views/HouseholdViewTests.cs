using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Views;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Views
{
    /// <summary>
    /// Tests the view for a household.
    /// </summary>
    [TestFixture]
    public class HouseholdViewTests
    {
        #region Private variables

        private Fixture _fixture;

        #endregion

        /// <summary>
        /// Setup each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        /// <summary>
        /// Tests that the view for a household can be initialized.
        /// </summary>
        [Test]
        public void TestThatHouseholdViewCanBeInitialized()
        {
            HouseholdView householdView = _fixture.Create<HouseholdView>();
            DataContractTestHelper.TestAtContractErInitieret(householdView);
        }

        /// <summary>
        /// Tests that the view for a household can be serialized.
        /// </summary>
        [Test]
        public void TestThatHouseholdViewCanBeSerialized()
        {
            HouseholdView householdView = _fixture.Create<HouseholdView>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(householdView);
        }
    }
}
