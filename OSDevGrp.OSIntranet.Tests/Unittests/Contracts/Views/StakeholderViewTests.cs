using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Views;
using AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Views
{
    /// <summary>
    /// Tests the view for an internal or external stakeholder.
    /// </summary>
    [TestFixture]
    public class StakeholderViewTests
    {
        /// <summary>
        /// Tests that the view for an internal or external stakeholder can be initialized.
        /// </summary>
        [Test]
        public void TestThatStakeholderViewCanBeInitialized()
        {
            var fixture = new Fixture();
            var stakeholderView = fixture.Create<StakeholderView>();
            DataContractTestHelper.TestAtContractErInitieret(stakeholderView);
        }

        /// <summary>
        /// Tests that the view for an internal or external stakeholder can be serialized.
        /// </summary>
        [Test]
        public void TestThatStakeholderViewCanBeSerialized()
        {
            var fixture = new Fixture();
            var stakeholderView = fixture.Create<StakeholderView>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(stakeholderView);
        }
    }
}
