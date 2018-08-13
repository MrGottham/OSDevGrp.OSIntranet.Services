using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Views;
using AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Views
{
    /// <summary>
    /// Tests the system view for a data provider.
    /// </summary>
    [TestFixture]
    public class DataProviderSystemViewTests
    {
        /// <summary>
        /// Tests that the system view for a data provider can be initialized.
        /// </summary>
        [Test]
        public void TestThatDataProviderSystemViewCanBeInitialized()
        {
            var fixture = new Fixture();
            var dataProviderSystemView = fixture.Create<DataProviderSystemView>();
            DataContractTestHelper.TestAtContractErInitieret(dataProviderSystemView);
        }

        /// <summary>
        /// Tests that the system view for a data provider can be serialized.
        /// </summary>
        [Test]
        public void TestThatDataProviderSystemViewCanBeSerialized()
        {
            var fixture = new Fixture();
            var dataProviderSystemView = fixture.Create<DataProviderSystemView>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(dataProviderSystemView);
        }
    }
}
