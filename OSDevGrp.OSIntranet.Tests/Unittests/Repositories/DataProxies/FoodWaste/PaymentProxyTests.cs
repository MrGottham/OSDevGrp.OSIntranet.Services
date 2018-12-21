using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoFixture;
using MySql.Data.MySqlClient;
using MySql.Data.Types;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Resources;
using OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.DataProxies.FoodWaste
{
    /// <summary>
    /// Tests the data proxy to a payment from a stakeholder.
    /// </summary>
    [TestFixture]
    public class PaymentProxyTests
    {
        #region Private variables

        private Fixture _fixture;
        private Random _random;

        #endregion

        /// <summary>
        /// Setup each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        /// <summary>
        /// Tests that the constructor initialize a data proxy to a payment from a stakeholder.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializePaymentProxy()
        {
            IPaymentProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);
            Assert.That(sut.Stakeholder, Is.Null);
            Assert.That(sut.DataProvider, Is.Null);
            Assert.That(sut.PaymentTime, Is.EqualTo(default(DateTime)));
            Assert.That(sut.PaymentReference, Is.Null);
            Assert.That(sut.PaymentReceipt, Is.Null);
            Assert.That(sut.CreationTime, Is.EqualTo(default(DateTime)));
        }

        /// <summary>
        /// Tests that getter for UniqueId throws an IntranetRepositoryException when the payment from a stakeholder has no identifier.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterThrowsIntranetRepositoryExceptionWhenPaymentProxyHasNoIdentifier()
        {
            IPaymentProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            // ReSharper disable UnusedVariable
            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => {var uniqueId = sut.UniqueId;});
            // ReSharper restore UnusedVariable

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, sut.Identifier, "Identifier");
        }

        /// <summary>
        /// Tests that getter for UniqueId gets the unique identifier for the payment from a stakeholder.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterGetsUniqueIdentificationForPaymentProxy()
        {
            Guid identifier = Guid.NewGuid();

            IPaymentProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            string uniqueId = sut.UniqueId;
            Assert.That(uniqueId, Is.Not.Null);
            Assert.That(uniqueId, Is.Not.Empty);
            Assert.That(uniqueId, Is.EqualTo(identifier.ToString("D").ToUpper()));
        }

        /// <summary>
        /// Tests that MapData throws an ArgumentNullException if the data reader is null.
        /// </summary>
        [Test]
        public void TestThatMapDataThrowsArgumentNullExceptionIfDataReaderIsNull()
        {
            IPaymentProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.MapData(null, CreateFoodWasteDataProvider()));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataReader");
        }

        /// <summary>
        /// Tests that MapData throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatMapDataThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            IPaymentProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.MapData(CreateMySqlDataReader(), null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that MapData maps data into the proxy.
        /// </summary>
        [Test]
        [TestCase(StakeholderType.HouseholdMember, true)]
        [TestCase(StakeholderType.HouseholdMember, false)]
        public void TestThatMapDataMapsDataIntoProxy(StakeholderType stakeholderType, bool hasPaymentReceipt)
        {
            IPaymentProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            Guid paymentIdentifier = Guid.NewGuid();
            Guid stakeholderIdentifier = Guid.NewGuid();
            Guid dataProviderIdentifier = Guid.NewGuid();
            DateTime paymentTime = DateTime.Now.AddDays(_random.Next(1, 7) * -1).AddMinutes(_random.Next(120, 240));
            string paymentReference = _fixture.Create<string>();
            byte[] paymentReceipt = hasPaymentReceipt ? _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray() : null;
            DateTime creationTime = DateTime.Now;
            MySqlDataReader dataReader = CreateMySqlDataReader(paymentIdentifier, stakeholderIdentifier, stakeholderType, dataProviderIdentifier, paymentTime, paymentReference, paymentReceipt, creationTime);

            DataProviderProxy dataProviderProxy = BuildDataProviderProxy();
            HouseholdMemberProxy householdMemberProxy = BuildHouseholdMemberProxy();
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(dataProviderProxy, householdMemberProxy);

            sut.MapData(dataReader, dataProvider);

            Assert.That(sut.Identifier, Is.Not.Null);
            Assert.That(sut.Identifier, Is.EqualTo(paymentIdentifier));
            Assert.That(sut.Stakeholder, Is.Not.Null);
            Assert.That(sut.Stakeholder, Is.EqualTo(householdMemberProxy));
            Assert.That(sut.DataProvider, Is.Not.Null);
            Assert.That(sut.DataProvider, Is.EqualTo(dataProviderProxy));
            Assert.That(sut.PaymentTime, Is.EqualTo(paymentTime).Within(1).Milliseconds);
            Assert.That(sut.PaymentReference, Is.Not.Null);
            Assert.That(sut.PaymentReference, Is.Not.Empty);
            Assert.That(sut.PaymentReference, Is.EqualTo(paymentReference));
            if (paymentReceipt != null)
            {
                Assert.That(sut.PaymentReceipt, Is.Not.Null);
                Assert.That(sut.PaymentReceipt, Is.Not.Empty);
                Assert.That(sut.PaymentReceipt, Is.EqualTo(paymentReceipt));
            }
            else
            {
                Assert.That(sut.PaymentReceipt, Is.Null);
            }
            Assert.That(sut.CreationTime, Is.EqualTo(creationTime).Within(1).Milliseconds);

            dataReader.AssertWasCalled(m => m.GetString(Arg<string>.Is.Equal("PaymentIdentifier")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetInt32(Arg<string>.Is.Equal("StakeholderType")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetMySqlDateTime(Arg<string>.Is.Equal("PaymentTime")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetString(Arg<string>.Is.Equal("PaymentReference")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetOrdinal(Arg<string>.Is.Equal("PaymentReceipt")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.IsDBNull(Arg<int>.Is.Equal(6)), opt => opt.Repeat.Once());
            if (paymentReceipt != null)
            {
                dataReader.AssertWasCalled(m => m.GetTextReader(Arg<int>.Is.Equal(6)), opt => opt.Repeat.Once());
            }
            else
            {
                dataReader.AssertWasNotCalled(m => m.GetTextReader(Arg<int>.Is.Equal(6)));
            }
            dataReader.AssertWasCalled(m => m.GetMySqlDateTime(Arg<string>.Is.Equal("CreationTime")), opt => opt.Repeat.Once());

            dataProvider.AssertWasNotCalled(m => m.Clone());

            switch (stakeholderType)
            {
                case StakeholderType.HouseholdMember:
                    dataProvider.AssertWasCalled(m => m.Create(
                            Arg<IHouseholdMemberProxy>.Is.TypeOf,
                            Arg<MySqlDataReader>.Is.Equal(dataReader),
                            Arg<string[]>.Matches(e => e != null && e.Length == 8 &&
                                                       e[0] == "StakeholderIdentifier" &&
                                                       e[1] == "HouseholdMemberMailAddress" &&
                                                       e[2] == "HouseholdMemberMembership" &&
                                                       e[3] == "HouseholdMemberMembershipExpireTime" &&
                                                       e[4] == "HouseholdMemberActivationCode" &&
                                                       e[5] == "HouseholdMemberActivationTime" &&
                                                       e[6] == "HouseholdMemberPrivacyPolicyAcceptedTime" &&
                                                       e[7] == "HouseholdMemberCreationTime")),
                        opt => opt.Repeat.Once());
                    break;

                default:
                    throw new NotSupportedException($"The stakeholderType '{stakeholderType}' is not supported.");
            }

            dataProvider.AssertWasCalled(m => m.Create(
                    Arg<IDataProviderProxy>.Is.TypeOf,
                    Arg<MySqlDataReader>.Is.Equal(dataReader),
                    Arg<string[]>.Matches(e => e != null && e.Length == 4 &&
                                               e[0] == "DataProviderIdentifier" &&
                                               e[1] == "DataProviderName" &&
                                               e[2] == "DataProviderHandlesPayments" &&
                                               e[3] == "DataProviderDataSourceStatementIdentifier")),
                opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that MapRelations throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatMapRelationsThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            IPaymentProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.MapRelations(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that MapRelations does not clone the data provider.
        /// </summary>
        [Test]
        public void TestThatMapRelationsDoesNotCloneDataProvider()
        {
            IPaymentProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider();

            sut.MapRelations(dataProvider);

            dataProvider.AssertWasNotCalled(m => m.Clone());
        }

        /// <summary>
        /// Tests that SaveRelations throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            IPaymentProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.SaveRelations(null, _fixture.Create<bool>()));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that SaveRelations throws an IntranetRepositoryException when the identifier for the payment made by a stakeholder is null.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsThrowsIntranetRepositoryExceptionWhenIdentifierIsNull()
        {
            IPaymentProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.SaveRelations(CreateFoodWasteDataProvider(), _fixture.Create<bool>()));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, sut.Identifier, "Identifier");
        }

        /// <summary>
        /// Tests that DeleteRelations throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            IPaymentProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.DeleteRelations(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that DeleteRelations throws an IntranetRepositoryException when the identifier for the payment made by a stakeholder is null.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsThrowsIntranetRepositoryExceptionWhenIdentifierIsNull()
        {
            IPaymentProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.DeleteRelations(CreateFoodWasteDataProvider()));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, sut.Identifier, "Identifier");
        }

        /// <summary>
        /// Tests that CreateGetCommand returns the SQL command for selecting the given payment from a stakeholder is null.
        /// </summary>
        [Test]
        public void TestThatCreateGetCommandReturnsSqlCommand()
        {
            Guid identifier = Guid.NewGuid();

            IPaymentProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("SELECT p.PaymentIdentifier,p.StakeholderIdentifier,p.StakeholderType,p.DataProviderIdentifier,p.PaymentTime,p.PaymentReference,p.PaymentReceipt,p.CreationTime,hm.MailAddress AS HouseholdMemberMailAddress,hm.ActivationCode AS HouseholdMemberActivationCode,hm.ActivationTime AS HouseholdMemberActivationTime,hm.CreationTime AS HouseholdMemberCreationTime,hm.Membership AS HouseholdMemberMembership,hm.MembershipExpireTime AS HouseholdMemberMembershipExpireTime,hm.PrivacyPolicyAcceptedTime AS HouseholdMemberPrivacyPolicyAcceptedTime,dp.Name AS DataProviderName,dp.HandlesPayments AS DataProviderHandlesPayments,dp.DataSourceStatementIdentifier AS DataProviderDataSourceStatementIdentifier FROM Payments AS p LEFT JOIN HouseholdMembers AS hm ON hm.HouseholdMemberIdentifier=p.StakeholderIdentifier INNER JOIN DataProviders AS dp ON dp.DataProviderIdentifier=p.DataProviderIdentifier WHERE p.PaymentIdentifier=@paymentIdentifier")
                .AddCharDataParameter("@paymentIdentifier", identifier)
                .Build()
                .Run(sut.CreateGetCommand());
        }

        /// <summary>
        /// Tests that CreateInsertCommand returns the SQL command to insert a payment from a stakeholder.
        /// </summary>
        [Test]
        [TestCase(StakeholderType.HouseholdMember, true)]
        [TestCase(StakeholderType.HouseholdMember, false)]
        public void TestThatCreateInsertCommandReturnsSqlCommandForInsert(StakeholderType stakeholderType, bool hasPaymentReceipt)
        {
            Guid identifier = Guid.NewGuid();
            IStakeholder stakeholder = DomainObjectMockBuilder.BuildStakeholderMock(stakeholderType);
            IDataProvider dataProvider = DomainObjectMockBuilder.BuildDataProviderMock(true);
            DateTime paymentTime = DateTime.Now.AddDays(_random.Next(1, 7) * -1).AddMinutes(_random.Next(120, 240));
            string paymentReference = _fixture.Create<string>();
            byte[] paymentReceipt = hasPaymentReceipt ? _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray() : null;
            DateTime creationTime = DateTime.Now;
            IPaymentProxy sut = CreateSut(identifier, stakeholder, dataProvider, paymentTime, paymentReference, paymentReceipt, creationTime);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("INSERT INTO Payments (PaymentIdentifier,StakeholderIdentifier,StakeholderType,DataProviderIdentifier,PaymentTime,PaymentReference,PaymentReceipt,CreationTime) VALUES(@paymentIdentifier,@stakeholderIdentifier,@stakeholderType,@dataProviderIdentifier,@paymentTime,@paymentReference,@paymentReceipt,@creationTime)")
                .AddCharDataParameter("@paymentIdentifier", identifier)
                .AddCharDataParameter("@stakeholderIdentifier", stakeholder.Identifier)
                .AddTinyIntDataParameter("@stakeholderType", (int) stakeholderType, 4)
                .AddCharDataParameter("@dataProviderIdentifier", dataProvider.Identifier)
                .AddDateTimeDataParameter("@paymentTime", paymentTime.ToUniversalTime())
                .AddVarCharDataParameter("@paymentReference", paymentReference, 128)
                .AddLongTextDataParameter("@paymentReceipt", paymentReceipt, true)
                .AddDateTimeDataParameter("@creationTime", creationTime.ToUniversalTime())
                .Build()
                .Run(sut.CreateInsertCommand());
        }

        /// <summary>
        /// Tests that CreateUpdateCommand returns the SQL command to update a payment from a stakeholder.
        /// </summary>
        [Test]
        [TestCase(StakeholderType.HouseholdMember, true)]
        [TestCase(StakeholderType.HouseholdMember, false)]
        public void TestThatCreateUpdateCommandReturnsSqlCommandForUpdate(StakeholderType stakeholderType, bool hasPaymentReceipt)
        {
            Guid identifier = Guid.NewGuid();
            IStakeholder stakeholder = DomainObjectMockBuilder.BuildStakeholderMock(stakeholderType);
            IDataProvider dataProvider = DomainObjectMockBuilder.BuildDataProviderMock(true);
            DateTime paymentTime = DateTime.Now.AddDays(_random.Next(1, 7) * -1).AddMinutes(_random.Next(120, 240));
            string paymentReference = _fixture.Create<string>();
            byte[] paymentReceipt = hasPaymentReceipt ? _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray() : null;
            DateTime creationTime = DateTime.Now;
            IPaymentProxy sut = CreateSut(identifier, stakeholder, dataProvider, paymentTime, paymentReference, paymentReceipt, creationTime);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("UPDATE Payments SET StakeholderIdentifier=@stakeholderIdentifier,StakeholderType=@stakeholderType,DataProviderIdentifier=@dataProviderIdentifier,PaymentTime=@paymentTime,PaymentReference=@paymentReference,PaymentReceipt=@paymentReceipt,CreationTime=@creationTime WHERE PaymentIdentifier=@paymentIdentifier")
                .AddCharDataParameter("@paymentIdentifier", identifier)
                .AddCharDataParameter("@stakeholderIdentifier", stakeholder.Identifier)
                .AddTinyIntDataParameter("@stakeholderType", (int) stakeholderType, 4)
                .AddCharDataParameter("@dataProviderIdentifier", dataProvider.Identifier)
                .AddDateTimeDataParameter("@paymentTime", paymentTime.ToUniversalTime())
                .AddVarCharDataParameter("@paymentReference", paymentReference, 128)
                .AddLongTextDataParameter("@paymentReceipt", paymentReceipt, true)
                .AddDateTimeDataParameter("@creationTime", creationTime.ToUniversalTime())
                .Build()
                .Run(sut.CreateUpdateCommand());
        }

        /// <summary>
        /// Tests that CreateDeleteCommand returns the SQL command to delete a payment from a stakeholder.
        /// </summary>
        [Test]
        public void TestThatCreateDeleteCommandReturnsSqlCommandForDelete()
        {
            Guid identifier = Guid.NewGuid();

            IPaymentProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("DELETE FROM Payments WHERE PaymentIdentifier=@paymentIdentifier")
                .AddCharDataParameter("@paymentIdentifier", identifier)
                .Build()
                .Run(sut.CreateDeleteCommand());
        }

        /// <summary>
        /// Creates an instance of a data proxy to a payment from a stakeholder.
        /// </summary>
        /// <returns>Instance of a data proxy to a payment from a stakeholder.</returns>
        private IPaymentProxy CreateSut()
        {
            return new PaymentProxy();
        }

        /// <summary>
        /// Creates an instance of a data proxy to a payment from a stakeholder.
        /// </summary>
        /// <returns>Instance of a data proxy to a payment from a stakeholder.</returns>
        private IPaymentProxy CreateSut(Guid identifier)
        {
            return new PaymentProxy
            {
                Identifier = identifier
            };
        }

        /// <summary>
        /// Creates an instance of a data proxy to a payment from a stakeholder.
        /// </summary>
        /// <returns>Instance of a data proxy to a payment from a stakeholder.</returns>
        private IPaymentProxy CreateSut(Guid identifier, IStakeholder stakeholder, IDataProvider dataProvider, DateTime paymentTime, string paymentReference, IEnumerable<byte> paymentReceipt, DateTime creationTime)
        {
            return new PaymentProxy(stakeholder, dataProvider, paymentTime, paymentReference, paymentReceipt, creationTime)
            {
                Identifier = identifier
            };
        }

        /// <summary>
        /// Creates a stub for the MySQL data reader.
        /// </summary>
        /// <returns>Stub for the MySQL data reader.</returns>
        private MySqlDataReader CreateMySqlDataReader(Guid? paymentIdentifier = null, Guid? stakeholderIdentifier = null, StakeholderType? stakeholderType = null, Guid? dataProviderIdentifier = null, DateTime? paymentTime = null, string paymentReference = null, IEnumerable<byte> paymentReceipt = null, DateTime? creationTime = null)
        {
            MySqlDataReader mySqlDataReaderMock = MockRepository.GenerateStub<MySqlDataReader>();
            mySqlDataReaderMock.Stub(m => m.GetString(Arg<string>.Is.Equal("PaymentIdentifier")))
                .Return(paymentIdentifier.HasValue ? paymentIdentifier.Value.ToString("D").ToUpper() : Guid.NewGuid().ToString("D").ToUpper())
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetString(Arg<string>.Is.Equal("StakeholderIdentifier")))
                .Return(stakeholderIdentifier.HasValue ? stakeholderIdentifier.Value.ToString("D").ToUpper() : Guid.NewGuid().ToString("D").ToUpper())
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetInt32(Arg<string>.Is.Equal("StakeholderType")))
                .Return((int) (stakeholderType ?? _fixture.Create<StakeholderType>()))
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetString(Arg<string>.Is.Equal("DataProviderIdentifier")))
                .Return(dataProviderIdentifier.HasValue ? dataProviderIdentifier.Value.ToString("D").ToUpper() : Guid.NewGuid().ToString("D").ToUpper())
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetMySqlDateTime(Arg<string>.Is.Equal("PaymentTime")))
                .Return(new MySqlDateTime((paymentTime ?? DateTime.Now.AddDays(_random.Next(1, 7) * -1).AddMinutes(_random.Next(120, 240))).ToUniversalTime()))
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetString(Arg<string>.Is.Equal("PaymentReference")))
                .Return(paymentReference ?? _fixture.Create<string>())
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetOrdinal(Arg<string>.Is.Equal("PaymentReceipt")))
                .Return(6)
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.IsDBNull(Arg<int>.Is.Equal(6)))
                .Return(paymentReceipt == null)
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetTextReader(Arg<int>.Is.Equal(6)))
                .Return(paymentReceipt != null ? new StringReader(Convert.ToBase64String(paymentReceipt.ToArray())) : null)
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetMySqlDateTime(Arg<string>.Is.Equal("CreationTime")))
                .Return(new MySqlDateTime((creationTime ?? DateTime.Now.AddDays(_random.Next(1, 7) * -1).AddMinutes(_random.Next(120, 240))).ToUniversalTime()))
                .Repeat.Any();
            return mySqlDataReaderMock;
        }

        /// <summary>
        /// Creates a mockup for the data provider which can access data in the food waste repository.
        /// </summary>
        /// <returns>Mockup for the data provider which can access data in the food waste repository.</returns>
        private IFoodWasteDataProvider CreateFoodWasteDataProvider(DataProviderProxy dataProviderProxy = null, HouseholdMemberProxy householdMemberProxy = null)
        {
            IFoodWasteDataProvider foodWasteDataProvider = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProvider.Stub(m => m.Create(Arg<IDataProviderProxy>.Is.TypeOf, Arg<MySqlDataReader>.Is.Anything, Arg<string[]>.Is.Anything))
                .Return(dataProviderProxy ?? BuildDataProviderProxy())
                .Repeat.Any();
            foodWasteDataProvider.Stub(m => m.Create(Arg<IHouseholdMemberProxy>.Is.TypeOf, Arg<MySqlDataReader>.Is.Anything, Arg<string[]>.Is.Anything))
                .Return(householdMemberProxy ?? BuildHouseholdMemberProxy())
                .Repeat.Any();
            return foodWasteDataProvider;
        }

        /// <summary>
        /// Creates a data proxy to a given data provider.
        /// </summary>
        /// <param name="identifier">The identifier for the data proxy.</param>
        /// <returns>Data proxy to a given data provider.</returns>
        private DataProviderProxy BuildDataProviderProxy(Guid? identifier = null)
        {
            return new DataProviderProxy
            {
                Identifier = identifier ?? Guid.NewGuid()
            };
        }

        /// <summary>
        /// Creates a data proxy to a given household member.
        /// </summary>
        /// <param name="identifier">The identifier for the data proxy.</param>
        /// <returns>Data proxy to a given household member.</returns>
        private HouseholdMemberProxy BuildHouseholdMemberProxy(Guid? identifier = null)
        {
            return new HouseholdMemberProxy
            {
                Identifier = identifier ?? Guid.NewGuid()
            };
        }
    }
}
