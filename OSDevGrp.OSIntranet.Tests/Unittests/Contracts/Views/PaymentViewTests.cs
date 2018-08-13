using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Views;
using AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Views
{
    /// <summary>
    /// Tests the view for a payment from a stakeholder.
    /// </summary>
    [TestFixture]
    public class PaymentViewTests
    {
        /// <summary>
        /// Tests that the view for a payment from a stakeholder can be initialized.
        /// </summary>
        [Test]
        public void TestThatPaymentViewCanBeInitialized()
        {
            var fixture = new Fixture();
            var paymentView = fixture.Create<PaymentView>();
            DataContractTestHelper.TestAtContractErInitieret(paymentView);
        }

        /// <summary>
        /// Tests that the view for a payment from a stakeholder can be serialized.
        /// </summary>
        [Test]
        public void TestThatPaymentViewCanBeSerialized()
        {
            var fixture = new Fixture();
            var paymentView = fixture.Create<PaymentView>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(paymentView);
        }
    }
}
