using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste
{
    /// <summary>
    /// Tests a payment from a stakeholder.
    /// </summary>
    [TestFixture]
    public class PaymentTests
    {
        /// <summary>
        /// Private class for testing a payment from a stakeholder.
        /// </summary>
        private class MyPayment : Payment
        {
            #region Constructor

            /// <summary>
            /// Creates an instance of the private class for testing a payment from a stakeholder.
            /// </summary>
            /// <param name="stakeholder">Stakeholder who has made the payment.</param>
            /// <param name="dataProvider">Data provider who has handled the payment.</param>
            /// <param name="paymentTime">Date and time for the payment.</param>
            /// <param name="paymentReference">Payment reference from the data provider who has handled the payment.</param>
            /// <param name="paymentReceipt">Payment receipt from the data provider who has handled the payment.</param>
            public MyPayment(IStakeholder stakeholder, IDataProvider dataProvider, DateTime paymentTime, string paymentReference, IEnumerable<byte> paymentReceipt = null)
                : base(stakeholder, dataProvider, paymentTime, paymentReference, paymentReceipt)
            {
            }

            #endregion

            #region Methods

            /// <summary>
            /// Gets or sets the stakeholder who has made the payment.
            /// </summary>
            public new IStakeholder Stakeholder
            {
                get { return base.Stakeholder; }
                set { base.Stakeholder = value; }
            }

            /// <summary>
            /// Gets or sets the data provider who has handled the payment.
            /// </summary>
            public new IDataProvider DataProvider
            {
                get { return base.DataProvider; }
                set { base.DataProvider = value; }
            }

            /// <summary>
            /// Gets or sets the date and time for the payment.
            /// </summary>
            public new DateTime PaymentTime
            {
                get { return base.PaymentTime; }
                set { base.PaymentTime = value; }
            }

            /// <summary>
            /// Gets or sets the payment reference from the data provider who has handled the payment.
            /// </summary>
            public new string PaymentReference
            {
                get { return base.PaymentReference; }
                set { base.PaymentReference = value; }
            }

            /// <summary>
            /// Gets or sets the payment receipt from the data provider who has handled the payment.
            /// </summary>
            public new IEnumerable<byte> PaymentReceipt
            {
                get { return base.PaymentReceipt; }
                set { base.PaymentReceipt = value; }
            }

            /// <summary>
            /// Gets or sets the creation date and time.
            /// </summary>
            public new DateTime CreationTime
            {
                get { return base.CreationTime; }
                set { base.CreationTime = value; }
            }

            #endregion
        }

        /// <summary>
        /// Tests that the constructor initialize a payment from a stakeholder without a payment receipt.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializePaymentWithoutPaymentReceipt()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());
            var stakeholderMock = DomainObjectMockBuilder.BuildStakeholderMock();
            var dataProviderMock = DomainObjectMockBuilder.BuildDataProviderMock(true);
            var paymentTime = DateTime.Now.AddDays(random.Next(1, 7)*-1).AddMinutes(random.Next(120, 240));
            var paymentReference = fixture.Create<string>();

            var payment = new Payment(stakeholderMock, dataProviderMock, paymentTime, paymentReference);
            Assert.That(payment, Is.Not.Null);
            Assert.That(payment.Identifier, Is.Null);
            Assert.That(payment.Identifier.HasValue, Is.False);
            Assert.That(payment.Stakeholder, Is.Not.Null);
            Assert.That(payment.Stakeholder, Is.EqualTo(stakeholderMock));
            Assert.That(payment.DataProvider, Is.Not.Null);
            Assert.That(payment.DataProvider, Is.EqualTo(dataProviderMock));
            Assert.That(payment.PaymentTime, Is.EqualTo(paymentTime));
            Assert.That(payment.PaymentReference, Is.Not.Null);
            Assert.That(payment.PaymentReference, Is.Not.Empty);
            Assert.That(payment.PaymentReference, Is.EqualTo(paymentReference));
            Assert.That(payment.PaymentReceipt, Is.Null);
            Assert.That(payment.CreationTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);
        }

        /// <summary>
        /// Tests that the constructor initialize a payment from a stakeholder with a payment receipt.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializePaymentWithPaymentReceipt()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());
            var stakeholderMock = DomainObjectMockBuilder.BuildStakeholderMock();
            var dataProviderMock = DomainObjectMockBuilder.BuildDataProviderMock(true);
            var paymentTime = DateTime.Now.AddDays(random.Next(1, 7)*-1).AddMinutes(random.Next(120, 240));
            var paymentReference = fixture.Create<string>();
            var paymentReceipt = fixture.CreateMany<byte>(random.Next(1024, 4096));

            var payment = new Payment(stakeholderMock, dataProviderMock, paymentTime, paymentReference, paymentReceipt);
            Assert.That(payment, Is.Not.Null);
            Assert.That(payment.Identifier, Is.Null);
            Assert.That(payment.Identifier.HasValue, Is.False);
            Assert.That(payment.Stakeholder, Is.Not.Null);
            Assert.That(payment.Stakeholder, Is.EqualTo(stakeholderMock));
            Assert.That(payment.DataProvider, Is.Not.Null);
            Assert.That(payment.DataProvider, Is.EqualTo(dataProviderMock));
            Assert.That(payment.PaymentTime, Is.EqualTo(paymentTime));
            Assert.That(payment.PaymentReference, Is.Not.Null);
            Assert.That(payment.PaymentReference, Is.Not.Empty);
            Assert.That(payment.PaymentReference, Is.EqualTo(paymentReference));
            Assert.That(payment.PaymentReceipt, Is.Not.Null);
            Assert.That(payment.PaymentReceipt, Is.Not.Empty);
            Assert.That(payment.PaymentReceipt, Is.EqualTo(paymentReceipt));
            Assert.That(payment.CreationTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the stakeholder who has made the payment is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenStakeholderIsNull()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());

            var exception = Assert.Throws<ArgumentNullException>(() => new Payment(null, DomainObjectMockBuilder.BuildDataProviderMock(true), DateTime.Now.AddDays(random.Next(1, 7)*-1).AddMinutes(random.Next(120, 240)), fixture.Create<string>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("stakeholder"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the data provider who has handled the payment is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenDataProviderIsNull()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());

            var exception = Assert.Throws<ArgumentNullException>(() => new Payment(DomainObjectMockBuilder.BuildStakeholderMock(), null, DateTime.Now.AddDays(random.Next(1, 7)*-1).AddMinutes(random.Next(120, 240)), fixture.Create<string>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("dataProvider"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the payment reference from the data provider who has handled the payment is invalid.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenPaymentReferenceIsInvalid(string invalidValue)
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());

            var exception = Assert.Throws<ArgumentNullException>(() => new Payment(DomainObjectMockBuilder.BuildStakeholderMock(), DomainObjectMockBuilder.BuildDataProviderMock(true), DateTime.Now.AddDays(random.Next(1, 7)*-1).AddMinutes(random.Next(120, 240)), invalidValue));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("paymentReference"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the setter for Stakeholder throws an ArgumentNullException when the value is null.
        /// </summary>
        [Test]
        public void TestThatStakeholderSetterThrowsArgumentNullExceptionWhenValueIsNull()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());

            var payment = new MyPayment(DomainObjectMockBuilder.BuildStakeholderMock(), DomainObjectMockBuilder.BuildDataProviderMock(true), DateTime.Now.AddDays(random.Next(1, 7)*-1).AddMinutes(random.Next(120, 240)), fixture.Create<string>());
            Assert.That(payment, Is.Not.Null);
            Assert.That(payment.Stakeholder, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => payment.Stakeholder = null);
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("value"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the setter for Stakeholder change the value.
        /// </summary>
        [Test]
        public void TestThatStakeholderSetterChangeValue()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());

            var payment = new MyPayment(DomainObjectMockBuilder.BuildStakeholderMock(), DomainObjectMockBuilder.BuildDataProviderMock(true), DateTime.Now.AddDays(random.Next(1, 7)*-1).AddMinutes(random.Next(120, 240)), fixture.Create<string>());
            Assert.That(payment, Is.Not.Null);
            Assert.That(payment.Stakeholder, Is.Not.Null);

            var newStakeholder = DomainObjectMockBuilder.BuildStakeholderMock();
            Assert.That(newStakeholder, Is.Not.Null);
            Assert.That(newStakeholder, Is.Not.EqualTo(payment.Stakeholder));

            payment.Stakeholder = newStakeholder;
            Assert.That(payment.Stakeholder, Is.Not.Null);
            Assert.That(payment.Stakeholder, Is.EqualTo(newStakeholder));
        }

        /// <summary>
        /// Tests that the setter for DataProvider throws an ArgumentNullException when the value is null.
        /// </summary>
        [Test]
        public void TestThatDataProviderSetterThrowsArgumentNullExceptionWhenValueIsNull()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());

            var payment = new MyPayment(DomainObjectMockBuilder.BuildStakeholderMock(), DomainObjectMockBuilder.BuildDataProviderMock(true), DateTime.Now.AddDays(random.Next(1, 7)*-1).AddMinutes(random.Next(120, 240)), fixture.Create<string>());
            Assert.That(payment, Is.Not.Null);
            Assert.That(payment.DataProvider, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => payment.DataProvider = null);
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("value"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the setter for DataProvider change the value.
        /// </summary>
        [Test]
        public void TestThatDataProviderSetterChangeValue()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());

            var payment = new MyPayment(DomainObjectMockBuilder.BuildStakeholderMock(), DomainObjectMockBuilder.BuildDataProviderMock(true), DateTime.Now.AddDays(random.Next(1, 7)*-1).AddMinutes(random.Next(120, 240)), fixture.Create<string>());
            Assert.That(payment, Is.Not.Null);
            Assert.That(payment.DataProvider, Is.Not.Null);

            var newDataProvider = DomainObjectMockBuilder.BuildDataProviderMock(true);
            Assert.That(newDataProvider, Is.Not.Null);
            Assert.That(newDataProvider, Is.Not.EqualTo(payment.DataProvider));

            payment.DataProvider = newDataProvider;
            Assert.That(payment.DataProvider, Is.Not.Null);
            Assert.That(payment.DataProvider, Is.EqualTo(newDataProvider));
        }

        /// <summary>
        /// Tests that the setter for PaymentTime change the value.
        /// </summary>
        [Test]
        public void TestThatPaymentTimeSetterChangeValue()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());

            var payment = new MyPayment(DomainObjectMockBuilder.BuildStakeholderMock(), DomainObjectMockBuilder.BuildDataProviderMock(true), DateTime.Now.AddDays(random.Next(1, 7)*-1).AddMinutes(random.Next(120, 240)), fixture.Create<string>());
            Assert.That(payment, Is.Not.Null);
            Assert.That(payment.PaymentTime, Is.LessThan(DateTime.Now));

            var newPaymentTime = payment.PaymentTime.AddMinutes(random.Next(120, 240)*-1);
            Assert.That(newPaymentTime, Is.Not.EqualTo(payment.PaymentTime));

            payment.PaymentTime = newPaymentTime;
            Assert.That(payment.PaymentTime, Is.EqualTo(newPaymentTime));
        }

        /// <summary>
        /// Tests that the setter for PaymentReference throws an ArgumentNullException when the value is invalid.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void TestThatPaymentReferenceSetterThrowsArgumentNullExceptionWhenValueIsNull(string invalidValue)
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());

            var payment = new MyPayment(DomainObjectMockBuilder.BuildStakeholderMock(), DomainObjectMockBuilder.BuildDataProviderMock(true), DateTime.Now.AddDays(random.Next(1, 7)*-1).AddMinutes(random.Next(120, 240)), fixture.Create<string>());
            Assert.That(payment, Is.Not.Null);
            Assert.That(payment.PaymentReference, Is.Not.Null);
            Assert.That(payment.PaymentReference, Is.Not.Empty);

            var exception = Assert.Throws<ArgumentNullException>(() => payment.PaymentReference = invalidValue);
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("value"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the setter for PaymentReference change the value.
        /// </summary>
        [Test]
        public void TestThatPaymentReferenceSetterChangeValue()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());

            var payment = new MyPayment(DomainObjectMockBuilder.BuildStakeholderMock(), DomainObjectMockBuilder.BuildDataProviderMock(true), DateTime.Now.AddDays(random.Next(1, 7)*-1).AddMinutes(random.Next(120, 240)), fixture.Create<string>());
            Assert.That(payment, Is.Not.Null);
            Assert.That(payment.PaymentReference, Is.Not.Null);
            Assert.That(payment.PaymentReference, Is.Not.Empty);

            var newPaymentReference = fixture.Create<string>();
            Assert.That(newPaymentReference, Is.Not.Null);
            Assert.That(newPaymentReference, Is.Not.Empty);
            Assert.That(newPaymentReference, Is.Not.EqualTo(payment.PaymentReference));

            payment.PaymentReference = newPaymentReference;
            Assert.That(payment.PaymentReference, Is.Not.Null);
            Assert.That(payment.PaymentReference, Is.Not.Empty);
            Assert.That(payment.PaymentReference, Is.EqualTo(newPaymentReference));
        }

        /// <summary>
        /// Tests that the setter for PaymentReceipt change the value to a byte array.
        /// </summary>
        [Test]
        public void TestThatPaymentReceiptSetterChangeValueToByteArray()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());

            var payment = new MyPayment(DomainObjectMockBuilder.BuildStakeholderMock(), DomainObjectMockBuilder.BuildDataProviderMock(true), DateTime.Now.AddDays(random.Next(1, 7)*-1).AddMinutes(random.Next(120, 240)), fixture.Create<string>());
            Assert.That(payment, Is.Not.Null);
            Assert.That(payment.PaymentReceipt, Is.Null);

            var newPaymentReceipt = fixture.CreateMany<byte>(random.Next(1024, 4096)).ToArray();
            Assert.That(newPaymentReceipt, Is.Not.Null);
            Assert.That(newPaymentReceipt, Is.Not.Empty);
            Assert.That(newPaymentReceipt, Is.Not.EqualTo(payment.PaymentReceipt));

            payment.PaymentReceipt = newPaymentReceipt;
            Assert.That(payment.PaymentReceipt, Is.Not.Null);
            Assert.That(payment.PaymentReceipt, Is.Not.Empty);
            Assert.That(payment.PaymentReceipt, Is.EqualTo(newPaymentReceipt));
        }

        /// <summary>
        /// Tests that the setter for PaymentReceipt change the value to null.
        /// </summary>
        [Test]
        public void TestThatPaymentReceiptSetterChangeValueToNull()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());

            var payment = new MyPayment(DomainObjectMockBuilder.BuildStakeholderMock(), DomainObjectMockBuilder.BuildDataProviderMock(true), DateTime.Now.AddDays(random.Next(1, 7)*-1).AddMinutes(random.Next(120, 240)), fixture.Create<string>(), fixture.CreateMany<byte>(random.Next(1024, 4096)).ToArray());
            Assert.That(payment, Is.Not.Null);
            Assert.That(payment.PaymentReceipt, Is.Not.Null);
            Assert.That(payment.PaymentReceipt, Is.Not.Empty);

            payment.PaymentReceipt = null;
            Assert.That(payment.PaymentReceipt, Is.Null);
        }

        /// <summary>
        /// Tests that the setter for CreationTime change the value.
        /// </summary>
        [Test]
        public void TestThatCreationTimeSetterChangeValue()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());

            var payment = new MyPayment(DomainObjectMockBuilder.BuildStakeholderMock(), DomainObjectMockBuilder.BuildDataProviderMock(true), DateTime.Now.AddDays(random.Next(1, 7)*-1).AddMinutes(random.Next(120, 240)), fixture.Create<string>());
            Assert.That(payment, Is.Not.Null);
            Assert.That(payment.CreationTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);

            var newCreationTime = DateTime.Now.AddDays(random.Next(1, 7)*-1);
            Assert.That(newCreationTime, Is.Not.EqualTo(payment.CreationTime));

            payment.CreationTime = newCreationTime;
            Assert.That(payment.CreationTime, Is.EqualTo(newCreationTime));
        }
    }
}
