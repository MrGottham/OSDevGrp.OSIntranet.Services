using System;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.FoodWaste;
using OSDevGrp.OSIntranet.Resources;
using OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste;
using Ploeh.AutoFixture;
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
            Assert.That(paymentProxy.StakeholderType, Is.Null);
            Assert.That(paymentProxy.StakeholderType.HasValue, Is.False);
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
        /// Tests that GetSqlQueryForId returns the SQL statement for selecting the given payment from a stakeholder is null.
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

        /// <summary>
        /// Tests that GetSqlCommandForInsert returns the SQL statement to insert a payment from a stakeholder.
        /// </summary>
        [Test]
        [TestCase(typeof (IHouseholdMember), true)]
        [TestCase(typeof (IHouseholdMember), false)]
        public void TestThatGetSqlCommandForInsertReturnsSqlCommandForInsert(Type stakeholderType, bool hasPaymentReceipt)
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());

            IStakeholder stakeholderMock = null;
            if (stakeholderType == typeof (IHouseholdMember))
            {
                stakeholderMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            }
            if (stakeholderMock == null)
            {
                throw new NotSupportedException(string.Format("The stakeholderType '{0}' is not supported.", stakeholderType.Name));
            }

            var dataProviderMock = DomainObjectMockBuilder.BuildDataProviderMock(true);
            var paymentTime = DateTime.Now.AddDays(random.Next(1, 7)*-1).AddMinutes(random.Next(120, 240));
            var paymentReference = fixture.Create<string>();
            var paymentReceipt = hasPaymentReceipt ? fixture.CreateMany<byte>(random.Next(1024, 4096)).ToArray() : null;
            var creationTime = DateTime.Now;

            var paymentProxy = new PaymentProxy(stakeholderMock, dataProviderMock, paymentTime, paymentReference, paymentReceipt, creationTime)
            {
                Identifier = Guid.NewGuid()
            };

            // ReSharper disable PossibleInvalidOperationException
            var stakeholderIdentifierSqlvalue = stakeholderMock.Identifier.Value.ToString("D").ToUpper();
            var stakeholderTypeSqlValue = paymentProxy.StakeholderType.Value;
            var dataProviderIdentifierSqlValue = dataProviderMock.Identifier.Value.ToString("D").ToUpper();
            // ReSharper restore PossibleInvalidOperationException
            var paymentReceiptSqlValue = paymentReceipt == null ? "NULL" : string.Format("'{0}'", Convert.ToBase64String(paymentReceipt));

            var sqlCommand = paymentProxy.GetSqlCommandForInsert();
            Assert.That(sqlCommand, Is.Not.Null);
            Assert.That(sqlCommand, Is.Not.Empty);
            Assert.That(sqlCommand, Is.EqualTo(string.Format("INSERT INTO Payments (PaymentIdentifier,StakeholderIdentifier,StakeholderType,DataProviderIdentifier,PaymentTime,PaymentReference,PaymentReceipt,CreationTime) VALUES('{0}','{1}',{2},'{3}','{4}','{5}',{6},'{7}')", paymentProxy.UniqueId, stakeholderIdentifierSqlvalue, stakeholderTypeSqlValue, dataProviderIdentifierSqlValue, DataRepositoryHelper.GetSqlValueForDateTime(paymentTime), paymentReference, paymentReceiptSqlValue, DataRepositoryHelper.GetSqlValueForDateTime(creationTime))));
        }

        /// <summary>
        /// Tests that GetSqlCommandForUpdate returns the SQL statement to update a payment from a stakeholder.
        /// </summary>
        [Test]
        [TestCase(typeof (IHouseholdMember), true)]
        [TestCase(typeof (IHouseholdMember), false)]
        public void TestThatGetSqlCommandForUpdateReturnsSqlCommandForUpdate(Type stakeholderType, bool hasPaymentReceipt)
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());

            IStakeholder stakeholderMock = null;
            if (stakeholderType == typeof (IHouseholdMember))
            {
                stakeholderMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            }
            if (stakeholderMock == null)
            {
                throw new NotSupportedException(string.Format("The stakeholderType '{0}' is not supported.", stakeholderType.Name));
            }

            var dataProviderMock = DomainObjectMockBuilder.BuildDataProviderMock(true);
            var paymentTime = DateTime.Now.AddDays(random.Next(1, 7)*-1).AddMinutes(random.Next(120, 240));
            var paymentReference = fixture.Create<string>();
            var paymentReceipt = hasPaymentReceipt ? fixture.CreateMany<byte>(random.Next(1024, 4096)).ToArray() : null;
            var creationTime = DateTime.Now;

            var paymentProxy = new PaymentProxy(stakeholderMock, dataProviderMock, paymentTime, paymentReference, paymentReceipt, creationTime)
            {
                Identifier = Guid.NewGuid()
            };

            // ReSharper disable PossibleInvalidOperationException
            var stakeholderIdentifierSqlvalue = stakeholderMock.Identifier.Value.ToString("D").ToUpper();
            var stakeholderTypeSqlValue = paymentProxy.StakeholderType.Value;
            var dataProviderIdentifierSqlValue = dataProviderMock.Identifier.Value.ToString("D").ToUpper();
            // ReSharper restore PossibleInvalidOperationException
            var paymentReceiptSqlValue = paymentReceipt == null ? "NULL" : string.Format("'{0}'", Convert.ToBase64String(paymentReceipt));

            var sqlCommand = paymentProxy.GetSqlCommandForUpdate();
            Assert.That(sqlCommand, Is.Not.Null);
            Assert.That(sqlCommand, Is.Not.Empty);
            Assert.That(sqlCommand, Is.EqualTo(string.Format("UPDATE Payments SET StakeholderIdentifier='{1}',StakeholderType={2},DataProviderIdentifier='{3}',PaymentTime='{4}',PaymentReference='{5}',PaymentReceipt={6},CreationTime='{7}' WHERE PaymentIdentifier='{0}'", paymentProxy.UniqueId, stakeholderIdentifierSqlvalue, stakeholderTypeSqlValue, dataProviderIdentifierSqlValue, DataRepositoryHelper.GetSqlValueForDateTime(paymentTime), paymentReference, paymentReceiptSqlValue, DataRepositoryHelper.GetSqlValueForDateTime(creationTime))));
        }

        /// <summary>
        /// Tests that GetSqlCommandForDelete returns the SQL statement to delete a payment from a stakeholder.
        /// </summary>
        [Test]
        public void TestThatGetSqlCommandForDeleteReturnsSqlCommandForDelete()
        {
            var paymentProxy = new PaymentProxy
            {
                Identifier = Guid.NewGuid()
            };

            var sqlCommand = paymentProxy.GetSqlCommandForDelete();
            Assert.That(sqlCommand, Is.Not.Null);
            Assert.That(sqlCommand, Is.Not.Empty);
            Assert.That(sqlCommand, Is.EqualTo(string.Format("DELETE FROM Payments WHERE PaymentIdentifier='{0}'", paymentProxy.UniqueId)));
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
