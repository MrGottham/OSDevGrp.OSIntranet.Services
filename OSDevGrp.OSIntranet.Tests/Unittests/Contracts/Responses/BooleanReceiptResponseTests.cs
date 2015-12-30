using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Responses;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Responses
{
    /// <summary>
    /// Tests the response for a boolean receipt.
    /// </summary>
    [TestFixture]
    public class BooleanReceiptResponseTests
    {
        /// <summary>
        /// Tests that the response for a service receipt can be initialized.
        /// </summary>
        [Test]
        public void TestThatBooleanReceiptResponseCanBeInitialized()
        {
            var fixture = new Fixture();
            var serviceReceiptResponse = fixture.Create<BooleanReceiptResponse>();
            DataContractTestHelper.TestAtContractErInitieret(serviceReceiptResponse);
        }

        /// <summary>
        /// Tests that the response for a service receipt can be serialized.
        /// </summary>
        [Test]
        public void TestThatBooleanReceiptResponseCanBeSerialized()
        {
            var fixture = new Fixture();
            var serviceReceiptResponse = fixture.Create<ServiceReceiptResponse>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(serviceReceiptResponse);
        }
    }
}
