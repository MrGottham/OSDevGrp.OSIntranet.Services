using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Responses;
using AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Responses
{
    /// <summary>
    /// Tests the response for a service receipt.
    /// </summary>
    [TestFixture]
    public class ServiceReceiptResponseTests
    {
        /// <summary>
        /// Tests that the response for a service receipt can be initialized.
        /// </summary>
        [Test]
        public void TestThatServiceReceiptResponseCanBeInitialized()
        {
            var fixture = new Fixture();
            var serviceReceiptResponse = fixture.Create<ServiceReceiptResponse>();
            DataContractTestHelper.TestAtContractErInitieret(serviceReceiptResponse);
        }

        /// <summary>
        /// Tests that the response for a service receipt can be serialized.
        /// </summary>
        [Test]
        public void TestThatServiceReceiptResponseCanBeSerialized()
        {
            var fixture = new Fixture();
            var serviceReceiptResponse = fixture.Create<ServiceReceiptResponse>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(serviceReceiptResponse);
        }
    }
}
