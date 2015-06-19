using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Views;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Views
{
    /// <summary>
    /// Tests the view for a data provider.
    /// </summary>
    [TestFixture]
    public class DataProviderViewTests
    {
        /// <summary>
        /// Tests that the view for a data provider can be initialized.
        /// </summary>
        [Test]
        public void TestThatDataProviderViewCanBeInitialized()
        {
            var fixture = new Fixture();
            var dataProviderView = fixture.Create<DataProviderView>();
            DataContractTestHelper.TestAtContractErInitieret(dataProviderView);
        }

        /// <summary>
        /// Tests that the view for a data provider can be serialized.
        /// </summary>
        [Test]
        public void TestThatDataProviderViewCanBeSerialized()
        {
            var fixture = new Fixture();
            var dataProviderView = fixture.Create<DataProviderView>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(dataProviderView);
        }
    }
}
