using System;
using System.Data;
using System.IO;
using System.Linq;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
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
        [TestCase(StakeholderType.HouseholdMember, true)]
        [TestCase(StakeholderType.HouseholdMember, false)]
        public void TestThatGetSqlCommandForInsertReturnsSqlCommandForInsert(StakeholderType stakeholderType, bool hasPaymentReceipt)
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());

            var stakeholderMock = DomainObjectMockBuilder.BuildStakeholderMock(stakeholderType);
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
            var dataProviderIdentifierSqlValue = dataProviderMock.Identifier.Value.ToString("D").ToUpper();
            // ReSharper restore PossibleInvalidOperationException
            var paymentReceiptSqlValue = paymentReceipt == null ? "NULL" : string.Format("'{0}'", Convert.ToBase64String(paymentReceipt));

            var sqlCommand = paymentProxy.GetSqlCommandForInsert();
            Assert.That(sqlCommand, Is.Not.Null);
            Assert.That(sqlCommand, Is.Not.Empty);
            Assert.That(sqlCommand, Is.EqualTo(string.Format("INSERT INTO Payments (PaymentIdentifier,StakeholderIdentifier,StakeholderType,DataProviderIdentifier,PaymentTime,PaymentReference,PaymentReceipt,CreationTime) VALUES('{0}','{1}',{2},'{3}',{4},'{5}',{6},{7})", paymentProxy.UniqueId, stakeholderIdentifierSqlvalue, (int) stakeholderType, dataProviderIdentifierSqlValue, DataRepositoryHelper.GetSqlValueForDateTime(paymentTime), paymentReference, paymentReceiptSqlValue, DataRepositoryHelper.GetSqlValueForDateTime(creationTime))));
        }

        /// <summary>
        /// Tests that GetSqlCommandForUpdate returns the SQL statement to update a payment from a stakeholder.
        /// </summary>
        [Test]
        [TestCase(StakeholderType.HouseholdMember, true)]
        [TestCase(StakeholderType.HouseholdMember, false)]
        public void TestThatGetSqlCommandForUpdateReturnsSqlCommandForUpdate(StakeholderType stakeholderType, bool hasPaymentReceipt)
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());

            var stakeholderMock = DomainObjectMockBuilder.BuildStakeholderMock(stakeholderType);
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
            var dataProviderIdentifierSqlValue = dataProviderMock.Identifier.Value.ToString("D").ToUpper();
            // ReSharper restore PossibleInvalidOperationException
            var paymentReceiptSqlValue = paymentReceipt == null ? "NULL" : string.Format("'{0}'", Convert.ToBase64String(paymentReceipt));

            var sqlCommand = paymentProxy.GetSqlCommandForUpdate();
            Assert.That(sqlCommand, Is.Not.Null);
            Assert.That(sqlCommand, Is.Not.Empty);
            Assert.That(sqlCommand, Is.EqualTo(string.Format("UPDATE Payments SET StakeholderIdentifier='{1}',StakeholderType={2},DataProviderIdentifier='{3}',PaymentTime={4},PaymentReference='{5}',PaymentReceipt={6},CreationTime={7} WHERE PaymentIdentifier='{0}'", paymentProxy.UniqueId, stakeholderIdentifierSqlvalue, (int) stakeholderType, dataProviderIdentifierSqlValue, DataRepositoryHelper.GetSqlValueForDateTime(paymentTime), paymentReference, paymentReceiptSqlValue, DataRepositoryHelper.GetSqlValueForDateTime(creationTime))));
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

        /// <summary>
        /// Tests that MapData throws an ArgumentNullException if the data reader is null.
        /// </summary>
        [Test]
        public void TestThatMapDataThrowsArgumentNullExceptionIfDataReaderIsNull()
        {
            var paymentProxy = new PaymentProxy();
            Assert.That(paymentProxy, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => paymentProxy.MapData(null, MockRepository.GenerateMock<IDataProviderBase>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("dataReader"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that MapData throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatMapDataThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            var paymentProxy = new PaymentProxy();
            Assert.That(paymentProxy, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => paymentProxy.MapData(MockRepository.GenerateStub<MySqlDataReader>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("dataProvider"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that MapData throws an IntranetRepositoryException if the data reader is not type of MySqlDataReader.
        /// </summary>
        [Test]
        public void TestThatMapDataThrowsIntranetRepositoryExceptionIfDataReaderIsNotTypeOfMySqlDataReader()
        {
            var dataReader = MockRepository.GenerateMock<IDataReader>();

            var paymentProxy = new PaymentProxy();
            Assert.That(paymentProxy, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => paymentProxy.MapData(dataReader, MockRepository.GenerateMock<IDataProviderBase>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, "dataReader", dataReader.GetType().Name)));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor initialize a data proxy to a payment from a stakeholder.
        /// </summary>
        [Test]
        [TestCase(StakeholderType.HouseholdMember, true)]
        [TestCase(StakeholderType.HouseholdMember, false)]
        public void TestThatMapDataAndMapRelationsMapsDataIntoProxy(StakeholderType stakeholderType, bool hasPaymentReceipt)
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());
            
            var dataProviderBaseMock = MockRepository.GenerateMock<IDataProviderBase>();
            dataProviderBaseMock.Stub(m => m.Clone())
                .Return(dataProviderBaseMock)
                .Repeat.Any();
            dataProviderBaseMock.Stub(m => m.Get(Arg<HouseholdMemberProxy>.Is.NotNull))
                .WhenCalled(e =>
                {
                    var householdMemberProxy = (HouseholdMemberProxy) e.Arguments.ElementAt(0);
                    Assert.That(householdMemberProxy, Is.Not.Null);
                    Assert.That(householdMemberProxy.Identifier, Is.Not.Null);
                    Assert.That(householdMemberProxy.Identifier.HasValue, Is.True);
                    // ReSharper disable PossibleInvalidOperationException
                    Assert.That(householdMemberProxy.Identifier.Value, Is.EqualTo(Guid.Parse("61AAB0D8-314F-4C46-B604-3FF443CA183A")));
                    // ReSharper restore PossibleInvalidOperationException
                })
                .Return(fixture.Create<HouseholdMemberProxy>())
                .Repeat.Any();
            dataProviderBaseMock.Stub(m => m.Get(Arg<DataProviderProxy>.Is.NotNull))
                .WhenCalled(e =>
                {
                    var dataProviderProxy = (DataProviderProxy) e.Arguments.ElementAt(0);
                    Assert.That(dataProviderProxy, Is.Not.Null);
                    Assert.That(dataProviderProxy.Identifier, Is.Not.Null);
                    Assert.That(dataProviderProxy.Identifier.HasValue, Is.True);
                    // ReSharper disable PossibleInvalidOperationException
                    Assert.That(dataProviderProxy.Identifier.Value, Is.EqualTo(Guid.Parse("9D8B58A7-B8FE-4392-8A60-F3722A2F3C45")));
                    // ReSharper restore PossibleInvalidOperationException
                })
                .Return(fixture.Create<DataProviderProxy>())
                .Repeat.Any();

            var paymentTime = DateTime.Now.AddDays(random.Next(1, 7)*-1).AddMinutes(random.Next(120, 240));
            var paymentReference = fixture.Create<string>();
            var paymentReceipt = hasPaymentReceipt ? fixture.CreateMany<byte>(random.Next(1024, 4096)).ToArray() : null;
            var creationTime = DateTime.Now;
            var dataReader = MockRepository.GenerateStub<MySqlDataReader>();
            dataReader.Stub(m => m.GetString(Arg<string>.Is.Equal("PaymentIdentifier")))
                .Return("C4BDF4A8-668A-4FCC-8816-2C9A53DA8941")
                .Repeat.Any();
            dataReader.Stub(m => m.GetString(Arg<string>.Is.Equal("StakeholderIdentifier")))
                .Return("61AAB0D8-314F-4C46-B604-3FF443CA183A")
                .Repeat.Any();
            dataReader.Stub(m => m.GetInt32(Arg<string>.Is.Equal("StakeholderType")))
                .Return((int) stakeholderType)
                .Repeat.Any();
            dataReader.Stub(m => m.GetString(Arg<string>.Is.Equal("DataProviderIdentifier")))
                .Return("9D8B58A7-B8FE-4392-8A60-F3722A2F3C45")
                .Repeat.Any();
            dataReader.Stub(m => m.GetDateTime(Arg<string>.Is.Equal("PaymentTime")))
                .Return(paymentTime.ToUniversalTime())
                .Repeat.Any();
            dataReader.Stub(m => m.GetString("PaymentReference"))
                .Return(paymentReference)
                .Repeat.Any();
            dataReader.Stub(m => m.GetOrdinal(Arg<string>.Is.Equal("PaymentReceipt")))
                .Return(6)
                .Repeat.Any();
            dataReader.Stub(m => m.IsDBNull(Arg<int>.Is.Equal(6)))
                .Return(!hasPaymentReceipt)
                .Repeat.Any();
            dataReader.Stub(m => m.GetTextReader(6))
                .Return(hasPaymentReceipt ? new StringReader(Convert.ToBase64String(paymentReceipt)) : null)
                .Repeat.Any();
            dataReader.Stub(m => m.GetDateTime(Arg<string>.Is.Equal("CreationTime")))
                .Return(creationTime.ToUniversalTime())
                .Repeat.Any();

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

            paymentProxy.MapData(dataReader, dataProviderBaseMock);
            paymentProxy.MapRelations(dataProviderBaseMock);
            Assert.That(paymentProxy, Is.Not.Null);
            Assert.That(paymentProxy.Identifier, Is.Not.Null);
            Assert.That(paymentProxy.Identifier.HasValue, Is.True);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(paymentProxy.Identifier.Value.ToString("D").ToUpper(), Is.EqualTo(dataReader.GetString("PaymentIdentifier")));
            // ReSharper restore PossibleInvalidOperationException
            Assert.That(paymentProxy.Stakeholder, Is.Not.Null);
            Assert.That(paymentProxy.DataProvider, Is.Not.Null);
            Assert.That(paymentProxy.PaymentTime, Is.EqualTo(paymentTime));
            Assert.That(paymentProxy.PaymentReference, Is.Not.Null);
            Assert.That(paymentProxy.PaymentReference, Is.Not.Empty);
            Assert.That(paymentProxy.PaymentReference, Is.EqualTo(paymentReference));
            if (hasPaymentReceipt)
            {
                Assert.That(paymentProxy.PaymentReceipt, Is.Not.Null);
                Assert.That(paymentProxy.PaymentReceipt, Is.Not.Empty);
                Assert.That(paymentProxy.PaymentReceipt, Is.EqualTo(paymentReceipt));
            }
            else
            {
                Assert.That(paymentProxy.PaymentReceipt, Is.Null);
            }
            Assert.That(paymentProxy.CreationTime, Is.EqualTo(creationTime));

            dataProviderBaseMock.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Times(2));
            switch (stakeholderType)
            {
                case StakeholderType.HouseholdMember:
                    dataProviderBaseMock.AssertWasCalled(m => m.Get(Arg<HouseholdMemberProxy>.Is.NotNull));
                    break;

                default:
                    throw new NotSupportedException(string.Format("The stakeholderType '{0}' is not supported.", stakeholderType));
            }
            dataProviderBaseMock.AssertWasCalled(m => m.Get(Arg<DataProviderProxy>.Is.NotNull));
        }

        /// <summary>
        /// Tests that MapRelations throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatMapRelationsThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            var paymentProxy = new PaymentProxy();
            Assert.That(paymentProxy, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => paymentProxy.MapRelations(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("dataProvider"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that SaveRelations throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            var fixture = new Fixture();

            var paymentProxy = new PaymentProxy();
            Assert.That(paymentProxy, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => paymentProxy.SaveRelations(null, fixture.Create<bool>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("dataProvider"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that SaveRelations throws an IntranetRepositoryException when the identifier for the payment made by a stakeholder is null.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsThrowsIntranetRepositoryExceptionWhenIdentifierIsNull()
        {
            var fixture = new Fixture();

            var paymentProxy = new PaymentProxy
            {
                Identifier = null
            };
            Assert.That(paymentProxy, Is.Not.Null);
            Assert.That(paymentProxy.Identifier, Is.Null);
            Assert.That(paymentProxy.Identifier.HasValue, Is.False);

            var exception = Assert.Throws<IntranetRepositoryException>(() => paymentProxy.SaveRelations(MockRepository.GenerateStub<IDataProviderBase>(), fixture.Create<bool>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, paymentProxy.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that DeleteRelations throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            var paymentProxy = new PaymentProxy();
            Assert.That(paymentProxy, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => paymentProxy.DeleteRelations(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("dataProvider"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that DeleteRelations throws an IntranetRepositoryException when the identifier for the payment made by a stakeholder is null.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsThrowsIntranetRepositoryExceptionWhenIdentifierIsNull()
        {
            var paymentProxy = new PaymentProxy
            {
                Identifier = null
            };
            Assert.That(paymentProxy, Is.Not.Null);
            Assert.That(paymentProxy.Identifier, Is.Null);
            Assert.That(paymentProxy.Identifier.HasValue, Is.False);

            var exception = Assert.Throws<IntranetRepositoryException>(() => paymentProxy.DeleteRelations(MockRepository.GenerateStub<IDataProviderBase>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, paymentProxy.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }
    }
}
