using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Views;
using AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Views
{
    /// <summary>
    /// Tests the system view for a static text.
    /// </summary>
    [TestFixture]
    public class StaticTextSystemViewTests
    {
        /// <summary>
        /// Tests that the system view for a static text can be initialized.
        /// </summary>
        [Test]
        public void TestThatStaticTextSystemViewCanBeInitialized()
        {
            var fixture = new Fixture();
            var staticTextSystemView = fixture.Create<StaticTextSystemView>();
            DataContractTestHelper.TestAtContractErInitieret(staticTextSystemView);
        }

        /// <summary>
        /// Tests that the system view for a static text can be serialized.
        /// </summary>
        [Test]
        public void TestThatStaticTextSystemViewCanBeSerialized()
        {
            var fixture = new Fixture();
            var staticTextSystemView = fixture.Create<StaticTextSystemView>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(staticTextSystemView);
        }
    }
}
