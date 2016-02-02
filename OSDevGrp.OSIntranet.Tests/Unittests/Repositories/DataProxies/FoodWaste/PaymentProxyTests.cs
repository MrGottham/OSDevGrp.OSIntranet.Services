using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Resources;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.DataProxies.FoodWaste
{
    /// <summary>
    /// Tests the data proxy to a payment from a stakeholder.
    /// </summary>
    [TestFixture]
    public class PaymentProxyTests
    {
        /// <summary>
        /// Tests that the constructor initialize a data proxy to a payment from a stakeholder.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializePaymentProxy()
        {
            var paymentProxy = new PaymentProxy();
            Assert.That(paymentProxy, Is.Not.Null);
            Assert.That(paymentProxy.Identifier, Is.Null);
            Assert.That(paymentProxy.Identifier.HasValue, Is.False);
            Assert.That(paymentProxy.Stakeholder, Is.Null);
            Assert.That(paymentProxy.DataProvider, Is.Null);
            Assert.That(paymentProxy.PaymentTime, Is.EqualTo(default(DateTime)));
            Assert.That(paymentProxy.PaymentReference, Is.Null);
            Assert.That(paymentProxy.PaymentReceipt, Is.Null);
            Assert.That(paymentProxy.CreationTime, Is.EqualTo(default(DateTime)));
        }

        /// <summary>
        /// Tests that getter for UniqueId throws an IntranetRepositoryException when the payment from a stakeholder has no identifier.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterThrowsIntranetRepositoryExceptionWhenPaymentProxyHasNoIdentifier()
        {
            var paymentProxy = new PaymentProxy
            {
                Identifier = null
            };

            // ReSharper disable UnusedVariable
            var exception = Assert.Throws<IntranetRepositoryException>(() => { var uniqueId = paymentProxy.UniqueId; });
            // ReSharper restore UnusedVariable
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, paymentProxy.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that getter for UniqueId gets the unique identifier for the payment from a stakeholder.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterGetsUniqueIdentificationForPaymentProxy()
        {
            var paymentProxy = new PaymentProxy
            {
                Identifier = Guid.NewGuid()
            };

            var uniqueId = paymentProxy.UniqueId;
            Assert.That(uniqueId, Is.Not.Null);
            Assert.That(uniqueId, Is.Not.Empty);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(uniqueId, Is.EqualTo(paymentProxy.Identifier.Value.ToString("D").ToUpper()));
            // ReSharper restore PossibleInvalidOperationException
        }

        /// <summary>
        /// Tests that GetSqlQueryForId throws an ArgumentNullException when the given payment from a stakeholder is null.
        /// </summary>
        [Test]
        public void TestThatGetSqlQueryForIdThrowsArgumentNullExceptionWhenPaymentIsNull()
        {
            var paymentProxy = new PaymentProxy();

            // ReSharper disable UnusedVariable
            var exception = Assert.Throws<ArgumentNullException>(() => { var sqlQueryForId = paymentProxy.GetSqlQueryForId(null); });
            // ReSharper restore UnusedVariable
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("payment"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetSqlQueryForId throws an IntranetRepositoryException when the identifier on the given payment from a stakeholder has no value.
        /// </summary>
        [Test]
        public void TestThatGetSqlQueryForIdThrowsIntranetRepositoryExceptionWhenIdentifierOnPaymentHasNoValue()
        {
            var paymentMock = MockRepository.GenerateMock<IPayment>();
            paymentMock.Expect(m => m.Identifier)
                .Return(null)
                .Repeat.Any();

            var paymentProxy = new PaymentProxy();

            // ReSharper disable UnusedVariable
            var exception = Assert.Throws<IntranetRepositoryException>(() => { var sqlQueryForId = paymentProxy.GetSqlQueryForId(paymentMock); });
            // ReSharper restore UnusedVariable
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, paymentMock.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetSqlQueryForId returns the SQL statement for selecting the given payment from a stakeholder is null..
        /// </summary>
        [Test]
        public void TestThatGetSqlQueryForIdReturnsSqlQueryForId()
        {
            var paymentMock = MockRepository.GenerateMock<IPayment>();
            paymentMock.Expect(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var paymentProxy = new PaymentProxy();

            var sqlQueryForId = paymentProxy.GetSqlQueryForId(paymentMock);
            Assert.That(sqlQueryForId, Is.Not.Null);
            Assert.That(sqlQueryForId, Is.Not.Empty);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(sqlQueryForId, Is.EqualTo(string.Format("SELECT PaymentIdentifier,StakeholderIdentifier,StakeholderType,DataProviderIdentifier,PaymentTime,PaymentReference,PaymentReceipt,CreationTime FROM Payments WHERE PaymentIdentifier='{0}'", paymentMock.Identifier.Value.ToString("D").ToUpper())));
            // ReSharper restore PossibleInvalidOperationException
        }

        /*
CREATE TABLE IF NOT EXISTS Payments (
	PaymentIdentifier CHAR(36) NOT NULL,
	StakeholderIdentifier CHAR(36) NOT NULL,
	StakeholderType TINYINT NOT NULL,
	DataProviderIdentifier CHAR(36) NOT NULL,
	PaymentTime DATETIME NOT NULL,
	PaymentReference NVARCHAR(128) NOT NULL,
	PaymentReceipt LONGTEXT NULL,
	CreationTime DATETIME NOT NULL,
         */
    }
}
