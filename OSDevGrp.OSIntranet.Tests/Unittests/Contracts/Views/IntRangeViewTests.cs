using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Views;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Views
{
    /// <summary>
    /// Tests the view for an integer range.
    /// </summary>
    [TestFixture]
    public class IntRangeViewTests
    {
        /// <summary>
        /// Tests that the view for an integer range can be initialized.
        /// </summary>
        [Test]
        public void TestThatIntRangeViewCanBeInitialized()
        {
            Fixture fixture = new Fixture();
            IntRangeView intRangeView = fixture.Create<IntRangeView>();
            DataContractTestHelper.TestAtContractErInitieret(intRangeView);
        }

        /// <summary>
        /// Tests that the view for an integer range can be serialized.
        /// </summary>
        [Test]
        public void TestThatIntRangeViewCanBeSerialized()
        {
            Fixture fixture = new Fixture();
            IntRangeView intRangeView = fixture.Create<IntRangeView>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(intRangeView);
        }
    }
}
