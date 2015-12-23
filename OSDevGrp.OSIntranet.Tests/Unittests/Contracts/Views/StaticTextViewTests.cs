using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Views;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Views
{
    /// <summary>
    /// Tests the view for a static text.
    /// </summary>
    [TestFixture]
    public class StaticTextViewTests
    {
        /// <summary>
        /// Tests that the view for a static text can be initialized.
        /// </summary>
        [Test]
        public void TestThatStaticTextViewCanBeInitialized()
        {
            var fixture = new Fixture();
            var staticTextView = fixture.Create<StaticTextView>();
            DataContractTestHelper.TestAtContractErInitieret(staticTextView);
        }

        /// <summary>
        /// Tests that the view for a static text can be serialized.
        /// </summary>
        [Test]
        public void TestThatStaticTextViewCanBeSerialized()
        {
            var fixture = new Fixture();
            var staticTextView = fixture.Create<StaticTextView>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(staticTextView);
        }
    }
}
