using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Resources;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.DataProxies.FoodWaste
{
    /// <summary>
    /// Tests the data proxy to a household member.
    /// </summary>
    [TestFixture]
    public class HouseholdMemberProxyTests
    {
        /// <summary>
        /// Tests that the constructor initialize a data proxy to a household member.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeHouseholdMemberProxy()
        {
            var householdMemberProxy = new HouseholdMemberProxy();
            Assert.That(householdMemberProxy, Is.Not.Null);
            Assert.That(householdMemberProxy.Identifier, Is.Null);
            Assert.That(householdMemberProxy.Identifier.HasValue, Is.False);
            Assert.That(householdMemberProxy.StakeholderType, Is.EqualTo(StakeholderType.HouseholdMember));
            Assert.That(householdMemberProxy.MailAddress, Is.Null);
            Assert.That(householdMemberProxy.Membership, Is.EqualTo(Membership.Basic));
            Assert.That(householdMemberProxy.MembershipExpireTime, Is.Null);
            Assert.That(householdMemberProxy.MembershipExpireTime.HasValue, Is.False);
            Assert.That(householdMemberProxy.ActivationCode, Is.Null);
            Assert.That(householdMemberProxy.ActivationTime, Is.Null);
            Assert.That(householdMemberProxy.ActivationTime.HasValue, Is.False);
            Assert.That(householdMemberProxy.IsActivated, Is.False);
            Assert.That(householdMemberProxy.PrivacyPolicyAcceptedTime, Is.Null);
            Assert.That(householdMemberProxy.PrivacyPolicyAcceptedTime.HasValue, Is.False);
            Assert.That(householdMemberProxy.IsPrivacyPolictyAccepted, Is.False);
            Assert.That(householdMemberProxy.CreationTime, Is.EqualTo(DateTime.MinValue));
            Assert.That(householdMemberProxy.Households, Is.Not.Null);
            Assert.That(householdMemberProxy.Households, Is.Empty);
            Assert.That(householdMemberProxy.Payments, Is.Not.Null);
            Assert.That(householdMemberProxy.Payments, Is.Empty);
        }

        /// <summary>
        /// Tests that getter for UniqueId throws an IntranetRepositoryException when the household member has no identifier.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterThrowsIntranetRepositoryExceptionWhenHouseholdMemberProxyHasNoIdentifier()
        {
            var householdMemberProxy = new HouseholdMemberProxy
            {
                Identifier = null
            };

            // ReSharper disable UnusedVariable
            var exception = Assert.Throws<IntranetRepositoryException>(() => { var uniqueId = householdMemberProxy.UniqueId; });
            // ReSharper restore UnusedVariable
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, householdMemberProxy.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that getter for UniqueId gets the unique identifier for the household member.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterGetsUniqueIdentificationForHouseholdMemberProxy()
        {
            var householdMemberProxy = new HouseholdMemberProxy
            {
                Identifier = Guid.NewGuid()
            };

            var uniqueId = householdMemberProxy.UniqueId;
            Assert.That(uniqueId, Is.Not.Null);
            Assert.That(uniqueId, Is.Not.Empty);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(uniqueId, Is.EqualTo(householdMemberProxy.Identifier.Value.ToString("D").ToUpper()));
            // ReSharper restore PossibleInvalidOperationException
        }

        /// <summary>
        /// Tests that GetSqlQueryForId throws an ArgumentNullException when the given household member is null.
        /// </summary>
        [Test]
        public void TestThatGetSqlQueryForIdThrowsArgumentNullExceptionWhenHouseholdMemberIsNull()
        {
            var householdMemberProxy = new HouseholdMemberProxy();

            // ReSharper disable UnusedVariable
            var exception = Assert.Throws<ArgumentNullException>(() => { var sqlQueryForId = householdMemberProxy.GetSqlQueryForId(null); });
            // ReSharper restore UnusedVariable
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("householdMember"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetSqlQueryForId throws an IntranetRepositoryException when the identifier on the given household member has no value.
        /// </summary>
        [Test]
        public void TestThatGetSqlQueryForIdThrowsIntranetRepositoryExceptionWhenIdentifierOnHouseholdMemberHasNoValue()
        {
            var householdMemberMock = MockRepository.GenerateMock<IHouseholdMember>();
            householdMemberMock.Expect(m => m.Identifier)
                .Return(null)
                .Repeat.Any();

            var householdMemberProxy = new HouseholdMemberProxy();

            // ReSharper disable UnusedVariable
            var exception = Assert.Throws<IntranetRepositoryException>(() => { var sqlQueryForId = householdMemberProxy.GetSqlQueryForId(householdMemberMock); });
            // ReSharper restore UnusedVariable
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, householdMemberMock.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetSqlQueryForId returns the SQL statement for selecting the given household member.
        /// </summary>
        [Test]
        public void TestThatGetSqlQueryForIdReturnsSqlQueryForId()
        {
            var householdMemberMock = MockRepository.GenerateMock<IHouseholdMember>();
            householdMemberMock.Expect(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var householdMemberProxy = new HouseholdMemberProxy();

            var sqlQueryForId = householdMemberProxy.GetSqlQueryForId(householdMemberMock);
            Assert.That(sqlQueryForId, Is.Not.Null);
            Assert.That(sqlQueryForId, Is.Not.Empty);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(sqlQueryForId, Is.EqualTo(string.Format("SELECT HouseholdMemberIdentifier,MailAddress,Membership,MembershipExpireTime,ActivationCode,ActivationTime,PrivacyPolicyAcceptedTime,CreationTime FROM HouseholdMembers WHERE HouseholdMemberIdentifier='{0}'", householdMemberMock.Identifier.Value.ToString("D").ToUpper())));
            // ReSharper restore PossibleInvalidOperationException
        }

        /// <summary>
        /// Tests that GetSqlCommandForInsert returns the SQL statement to insert a household member.
        /// </summary>
        [Test]
        [TestCase(Membership.Basic, true, true, true)]
        [TestCase(Membership.Basic, true, true, false)]
        [TestCase(Membership.Basic, true, false, true)]
        [TestCase(Membership.Basic, true, false, false)]
        [TestCase(Membership.Basic, false, true, true)]
        [TestCase(Membership.Basic, false, true, false)]
        [TestCase(Membership.Basic, false, false, true)]
        [TestCase(Membership.Basic, false, false, false)]
        [TestCase(Membership.Deluxe, true, true, true)]
        [TestCase(Membership.Deluxe, true, true, false)]
        [TestCase(Membership.Deluxe, true, false, true)]
        [TestCase(Membership.Deluxe, true, false, false)]
        [TestCase(Membership.Premium, true, true, true)]
        [TestCase(Membership.Premium, true, true, false)]
        [TestCase(Membership.Premium, true, false, true)]
        [TestCase(Membership.Premium, true, false, false)]
        public void TestThatGetSqlCommandForInsertReturnsSqlCommandForInsert(Membership membership, bool hasMembershipExpireTime, bool hasActivationTime, bool hasPrivacyPolicyAcceptedTime)
        {
            var identifier = Guid.NewGuid();
            var mailAddress = string.Format("test.{0}@osdevgrp.dk", identifier.ToString("D").ToLower());
            var activationCode = (new HouseholdMember(mailAddress)).ActivationCode;
            var membershipExpireTime = hasMembershipExpireTime ? DateTime.Now.AddYears(1) : (DateTime?) null;
            var activationTime = hasActivationTime ? DateTime.Now : (DateTime?) null;
            var privacyPolicyAcceptedTime = hasPrivacyPolicyAcceptedTime ? DateTime.Now : (DateTime?) null;
            var householdMemberProxy = new HouseholdMemberProxy(mailAddress, membership, membershipExpireTime, activationCode, DateTime.Now)
            {
                Identifier = identifier,
                ActivationTime = activationTime,
                PrivacyPolicyAcceptedTime = privacyPolicyAcceptedTime,
            };

            var sqlCommand = householdMemberProxy.GetSqlCommandForInsert();
            Assert.That(sqlCommand, Is.Not.Null);
            Assert.That(sqlCommand, Is.Not.Empty);
            Assert.That(sqlCommand, Is.EqualTo(string.Format("INSERT INTO HouseholdMembers (HouseholdMemberIdentifier,MailAddress,Membership,MembershipExpireTime,ActivationCode,ActivationTime,PrivacyPolicyAcceptedTime,CreationTime) VALUES('{0}','{1}',{2},{3},'{4}',{5},{6},{7})", householdMemberProxy.UniqueId, mailAddress, (int) membership, DataRepositoryHelper.GetSqlValueForDateTime(membershipExpireTime), activationCode, DataRepositoryHelper.GetSqlValueForDateTime(activationTime), DataRepositoryHelper.GetSqlValueForDateTime(privacyPolicyAcceptedTime), DataRepositoryHelper.GetSqlValueForDateTime(householdMemberProxy.CreationTime))));
        }

        /// <summary>
        /// Tests that GetSqlCommandForUpdate returns the SQL statement to update a household member.
        /// </summary>
        [Test]
        [TestCase(Membership.Basic, true, true, true)]
        [TestCase(Membership.Basic, true, true, false)]
        [TestCase(Membership.Basic, true, false, true)]
        [TestCase(Membership.Basic, true, false, false)]
        [TestCase(Membership.Basic, false, true, true)]
        [TestCase(Membership.Basic, false, true, false)]
        [TestCase(Membership.Basic, false, false, true)]
        [TestCase(Membership.Basic, false, false, false)]
        [TestCase(Membership.Deluxe, true, true, true)]
        [TestCase(Membership.Deluxe, true, true, false)]
        [TestCase(Membership.Deluxe, true, false, true)]
        [TestCase(Membership.Deluxe, true, false, false)]
        [TestCase(Membership.Premium, true, true, true)]
        [TestCase(Membership.Premium, true, true, false)]
        [TestCase(Membership.Premium, true, false, true)]
        [TestCase(Membership.Premium, true, false, false)]
        public void TestThatGetSqlCommandForUpdateReturnsSqlCommandForUpdate(Membership membership, bool hasMembershipExpireTime, bool hasActivationTime, bool hasPrivacyPolicyAcceptedTime)
        {
            var identifier = Guid.NewGuid();
            var mailAddress = string.Format("test.{0}@osdevgrp.dk", identifier.ToString("D").ToLower());
            var activationCode = (new HouseholdMember(mailAddress)).ActivationCode;
            var membershipExpireTime = hasMembershipExpireTime ? DateTime.Now.AddYears(1) : (DateTime?) null;
            var activationTime = hasActivationTime ? DateTime.Now : (DateTime?) null;
            var privacyPolicyAcceptedTime = hasPrivacyPolicyAcceptedTime ? DateTime.Now : (DateTime?) null;
            var householdMemberProxy = new HouseholdMemberProxy(mailAddress, membership, membershipExpireTime, activationCode, DateTime.Now)
            {
                Identifier = identifier,
                ActivationTime = activationTime,
                PrivacyPolicyAcceptedTime = privacyPolicyAcceptedTime,
            };

            var sqlCommand = householdMemberProxy.GetSqlCommandForUpdate();
            Assert.That(sqlCommand, Is.Not.Null);
            Assert.That(sqlCommand, Is.Not.Empty);
            Assert.That(sqlCommand, Is.EqualTo(string.Format("UPDATE HouseholdMembers SET MailAddress='{1}',Membership={2},MembershipExpireTime={3},ActivationCode='{4}',ActivationTime={5},PrivacyPolicyAcceptedTime={6},CreationTime={7} WHERE HouseholdMemberIdentifier='{0}'", householdMemberProxy.UniqueId, mailAddress, (int) membership, DataRepositoryHelper.GetSqlValueForDateTime(membershipExpireTime), activationCode, DataRepositoryHelper.GetSqlValueForDateTime(activationTime), DataRepositoryHelper.GetSqlValueForDateTime(privacyPolicyAcceptedTime), DataRepositoryHelper.GetSqlValueForDateTime(householdMemberProxy.CreationTime))));
        }

        /// <summary>
        /// Tests that GetSqlCommandForDelete returns the SQL statement to delete a household member.
        /// </summary>
        [Test]
        public void TestThatGetSqlCommandForDeleteReturnsSqlCommandForDelete()
        {
            var householdMemberProxy = new HouseholdMemberProxy
            {
                Identifier = Guid.NewGuid()
            };

            var sqlCommand = householdMemberProxy.GetSqlCommandForDelete();
            Assert.That(sqlCommand, Is.Not.Null);
            Assert.That(sqlCommand, Is.Not.Empty);
            Assert.That(sqlCommand, Is.EqualTo(string.Format("DELETE FROM HouseholdMembers WHERE HouseholdMemberIdentifier='{0}'", householdMemberProxy.UniqueId)));
        }

        /// <summary>
        /// Tests that MapData throws an ArgumentNullException if the data reader is null.
        /// </summary>
        [Test]
        public void TestThatMapDataThrowsArgumentNullExceptionIfDataReaderIsNull()
        {
            var householdMemberProxy = new HouseholdMemberProxy();
            Assert.That(householdMemberProxy, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdMemberProxy.MapData(null, MockRepository.GenerateMock<IDataProviderBase>()));
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
            var householdMemberProxy = new HouseholdMemberProxy();
            Assert.That(householdMemberProxy, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdMemberProxy.MapData(MockRepository.GenerateStub<MySqlDataReader>(), null));
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

            var householdMemberProxy = new HouseholdMemberProxy();
            Assert.That(householdMemberProxy, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => householdMemberProxy.MapData(dataReader, MockRepository.GenerateMock<IDataProviderBase>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, "dataReader", dataReader.GetType().Name)));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that MapData and MapRelations maps data into the proxy.
        /// </summary>
        [Test]
        [TestCase(Membership.Basic, true, true, true)]
        [TestCase(Membership.Basic, true, true, false)]
        [TestCase(Membership.Basic, true, false, true)]
        [TestCase(Membership.Basic, true, false, false)]
        [TestCase(Membership.Basic, false, true, true)]
        [TestCase(Membership.Basic, false, true, false)]
        [TestCase(Membership.Basic, false, false, true)]
        [TestCase(Membership.Basic, false, false, false)]
        [TestCase(Membership.Deluxe, true, true, true)]
        [TestCase(Membership.Deluxe, true, true, false)]
        [TestCase(Membership.Deluxe, true, false, true)]
        [TestCase(Membership.Deluxe, true, false, false)]
        [TestCase(Membership.Premium, true, true, true)]
        [TestCase(Membership.Premium, true, true, false)]
        [TestCase(Membership.Premium, true, false, true)]
        [TestCase(Membership.Premium, true, false, false)]
        public void TestThatMapDataAndMapRelationsMapsDataIntoProxy(Membership membership, bool hasMembershipExpireTime, bool hasActivationTime, bool hasPrivacyPolicyAcceptedTime)
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());

            var memberOfHouseholdProxyCollection = new List<MemberOfHouseholdProxy>(random.Next(7, 15));
            while (memberOfHouseholdProxyCollection.Count < memberOfHouseholdProxyCollection.Capacity)
            {
                var householdMemberMock = MockRepository.GenerateMock<IHouseholdMember>();
                householdMemberMock.Stub(m => m.Identifier)
                    .Return(Guid.NewGuid())
                    .Repeat.Any();
                var householdMock = MockRepository.GenerateMock<IHousehold>();
                householdMock.Stub(m => m.Identifier)
                    .Return(Guid.NewGuid())
                    .Repeat.Any();
                memberOfHouseholdProxyCollection.Add(new MemberOfHouseholdProxy(householdMemberMock, householdMock));
            }
            var paymentProxyCollection = fixture.CreateMany<PaymentProxy>(5).ToList();
            var dataProviderBaseMock = MockRepository.GenerateMock<IDataProviderBase>();
            dataProviderBaseMock.Stub(m => m.Clone())
                .Return(dataProviderBaseMock)
                .Repeat.Any();
            dataProviderBaseMock.Stub(m => m.GetCollection<MemberOfHouseholdProxy>(Arg<string>.Is.Anything))
                .Return(memberOfHouseholdProxyCollection)
                .Repeat.Any();
            dataProviderBaseMock.Stub(m => m.GetCollection<PaymentProxy>(Arg<string>.Is.Anything))
                .Return(paymentProxyCollection)
                .Repeat.Any();

            var dataReader = MockRepository.GenerateStub<MySqlDataReader>();
            dataReader.Stub(m => m.GetString(Arg<string>.Is.Equal("HouseholdMemberIdentifier")))
                .Return("F69CA810-0A8D-49D3-A50D-BCE0BD38C147")
                .Repeat.Any();
            dataReader.Stub(m => m.GetString(Arg<string>.Is.Equal("MailAddress")))
                .Return("test.f69ca810-0a8d-49d3-a50d-bce0bd38c147@osdevgrp.dk")
                .Repeat.Any();
            dataReader.Stub(m => m.GetInt16(Arg<string>.Is.Equal("Membership")))
                .Return((short) membership)
                .Repeat.Any();
            dataReader.Stub(m => m.GetOrdinal(Arg<string>.Is.Equal("MembershipExpireTime")))
                .Return(3)
                .Repeat.Any();
            dataReader.Stub(m => m.IsDBNull(Arg<int>.Is.Equal(3)))
                .Return(!hasMembershipExpireTime)
                .Repeat.Any();
            dataReader.Stub(m => m.GetDateTime(Arg<int>.Is.Equal(3)))
                .Return(DateTime.Now.AddYears(1).ToUniversalTime())
                .Repeat.Any();
            dataReader.Stub(m => m.GetString(Arg<string>.Is.Equal("ActivationCode")))
                .Return("BA9A4EE7B3E2AE9B5F4CC3E0DE0B6501")
                .Repeat.Any();
            dataReader.Stub(m => m.GetOrdinal(Arg<string>.Is.Equal("ActivationTime")))
                .Return(5)
                .Repeat.Any();
            dataReader.Stub(m => m.IsDBNull(Arg<int>.Is.Equal(5)))
                .Return(!hasActivationTime)
                .Repeat.Any();
            dataReader.Stub(m => m.GetDateTime(Arg<int>.Is.Equal(5)))
                .Return(DateTime.Now.ToUniversalTime())
                .Repeat.Any();
            dataReader.Stub(m => m.GetOrdinal(Arg<string>.Is.Equal("PrivacyPolicyAcceptedTime")))
                .Return(6)
                .Repeat.Any();
            dataReader.Stub(m => m.IsDBNull(Arg<int>.Is.Equal(6)))
                .Return(!hasPrivacyPolicyAcceptedTime)
                .Repeat.Any();
            dataReader.Stub(m => m.GetDateTime(Arg<int>.Is.Equal(6)))
                .Return(DateTime.Now.ToUniversalTime())
                .Repeat.Any();
            dataReader.Stub(m => m.GetDateTime(Arg<string>.Is.Equal("CreationTime")))
                .Return(DateTime.Now.ToUniversalTime())
                .Repeat.Any();

            var householdMemberProxy = new HouseholdMemberProxy();
            Assert.That(householdMemberProxy, Is.Not.Null);
            Assert.That(householdMemberProxy.Identifier, Is.Null);
            Assert.That(householdMemberProxy.Identifier.HasValue, Is.False);
            Assert.That(householdMemberProxy.StakeholderType, Is.EqualTo(StakeholderType.HouseholdMember));
            Assert.That(householdMemberProxy.MailAddress, Is.Null);
            Assert.That(householdMemberProxy.Membership, Is.EqualTo(Membership.Basic));
            Assert.That(householdMemberProxy.MembershipExpireTime, Is.Null);
            Assert.That(householdMemberProxy.MembershipExpireTime.HasValue, Is.False);
            Assert.That(householdMemberProxy.ActivationCode, Is.Null);
            Assert.That(householdMemberProxy.ActivationTime, Is.Null);
            Assert.That(householdMemberProxy.ActivationTime.HasValue, Is.False);
            Assert.That(householdMemberProxy.PrivacyPolicyAcceptedTime, Is.Null);
            Assert.That(householdMemberProxy.PrivacyPolicyAcceptedTime.HasValue, Is.False);
            Assert.That(householdMemberProxy.CreationTime, Is.EqualTo(DateTime.MinValue));
            Assert.That(householdMemberProxy.Households, Is.Not.Null);
            Assert.That(householdMemberProxy.Households, Is.Empty);
            Assert.That(householdMemberProxy.Payments, Is.Not.Null);
            Assert.That(householdMemberProxy.Payments, Is.Empty);

            householdMemberProxy.MapData(dataReader, dataProviderBaseMock);
            householdMemberProxy.MapRelations(dataProviderBaseMock);
            Assert.That(householdMemberProxy, Is.Not.Null);
            Assert.That(householdMemberProxy.Identifier, Is.Not.Null);
            Assert.That(householdMemberProxy.Identifier.HasValue, Is.True);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(householdMemberProxy.Identifier.Value.ToString("D").ToUpper(), Is.EqualTo(dataReader.GetString("HouseholdMemberIdentifier")));
            // ReSharper restore PossibleInvalidOperationException
            Assert.That(householdMemberProxy.StakeholderType, Is.EqualTo(StakeholderType.HouseholdMember));
            Assert.That(householdMemberProxy.MailAddress, Is.Not.Null);
            Assert.That(householdMemberProxy.MailAddress, Is.Not.Empty);
            Assert.That(householdMemberProxy.MailAddress, Is.EqualTo(dataReader.GetString("MailAddress")));
            Assert.That(householdMemberProxy.Membership, Is.EqualTo(membership));
            if (hasMembershipExpireTime)
            {
                Assert.That(householdMemberProxy.MembershipExpireTime, Is.Not.Null);
                Assert.That(householdMemberProxy.MembershipExpireTime.HasValue, Is.True);
                // ReSharper disable PossibleInvalidOperationException
                Assert.That(householdMemberProxy.MembershipExpireTime.Value, Is.EqualTo(DateTime.Now.AddYears(1)).Within(3).Seconds);
                // ReSharper restore PossibleInvalidOperationException
            }
            else
            {
                Assert.That(householdMemberProxy.MembershipExpireTime, Is.Null);
                Assert.That(householdMemberProxy.MembershipExpireTime.HasValue, Is.False);
            }
            Assert.That(householdMemberProxy.ActivationCode, Is.Not.Null);
            Assert.That(householdMemberProxy.ActivationCode, Is.Not.Empty);
            Assert.That(householdMemberProxy.ActivationCode, Is.EqualTo(dataReader.GetString("ActivationCode")));
            if (hasActivationTime)
            {
                Assert.That(householdMemberProxy.ActivationTime, Is.Not.Null);
                Assert.That(householdMemberProxy.ActivationTime.HasValue, Is.True);
                // ReSharper disable PossibleInvalidOperationException
                Assert.That(householdMemberProxy.ActivationTime.Value, Is.EqualTo(DateTime.Now).Within(3).Seconds);
                // ReSharper restore PossibleInvalidOperationException
            }
            else
            {
                Assert.That(householdMemberProxy.ActivationTime, Is.Null);
                Assert.That(householdMemberProxy.ActivationTime.HasValue, Is.False);
            }
            if (hasPrivacyPolicyAcceptedTime)
            {
                Assert.That(householdMemberProxy.PrivacyPolicyAcceptedTime, Is.Not.Null);
                Assert.That(householdMemberProxy.PrivacyPolicyAcceptedTime.HasValue, Is.True);
                // ReSharper disable PossibleInvalidOperationException
                Assert.That(householdMemberProxy.PrivacyPolicyAcceptedTime.Value, Is.EqualTo(DateTime.Now).Within(3).Seconds);
                // ReSharper restore PossibleInvalidOperationException
            }
            else
            {
                Assert.That(householdMemberProxy.PrivacyPolicyAcceptedTime, Is.Null);
                Assert.That(householdMemberProxy.PrivacyPolicyAcceptedTime.HasValue, Is.False);
            }
            Assert.That(householdMemberProxy.CreationTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);
            Assert.That(householdMemberProxy.Households, Is.Not.Null);
            Assert.That(householdMemberProxy.Households, Is.Not.Empty);
            Assert.That(householdMemberProxy.Payments, Is.Not.Null);
            Assert.That(householdMemberProxy.Payments, Is.Not.Empty);
            Assert.That(householdMemberProxy.Payments, Is.EqualTo(paymentProxyCollection));

            dataProviderBaseMock.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Times(2));
            dataProviderBaseMock.AssertWasCalled(m => m.GetCollection<MemberOfHouseholdProxy>(Arg<string>.Is.Equal(string.Format("SELECT MemberOfHouseholdIdentifier,HouseholdMemberIdentifier,HouseholdIdentifier,CreationTime FROM MemberOfHouseholds WHERE HouseholdMemberIdentifier='{0}' ORDER BY CreationTime DESC", householdMemberProxy.UniqueId))));
            dataProviderBaseMock.AssertWasCalled(m => m.GetCollection<PaymentProxy>(Arg<string>.Is.Equal(string.Format("SELECT PaymentIdentifier,StakeholderIdentifier,StakeholderType,DataProviderIdentifier,PaymentTime,PaymentReference,PaymentReceipt,CreationTime FROM Payments WHERE StakeholderIdentifier='{0}' ORDER BY PaymentTime DESC", householdMemberProxy.UniqueId))));
        }

        /// <summary>
        /// Tests that MapRelations throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatMapRelationsThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            var householdMemberProxy = new HouseholdMemberProxy();
            Assert.That(householdMemberProxy, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdMemberProxy.MapRelations(null));
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

            var householdMemberProxy = new HouseholdMemberProxy();
            Assert.That(householdMemberProxy, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdMemberProxy.SaveRelations(null, fixture.Create<bool>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("dataProvider"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that SaveRelations throws an IntranetRepositoryException when the identifier for the household member is null.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsThrowsIntranetRepositoryExceptionWhenIdentifierIsNull()
        {
            var fixture = new Fixture();

            var householdMemberProxy = new HouseholdMemberProxy
            {
                Identifier = null
            };
            Assert.That(householdMemberProxy, Is.Not.Null);
            Assert.That(householdMemberProxy.Identifier, Is.Null);
            Assert.That(householdMemberProxy.Identifier.HasValue, Is.False);

            var exception = Assert.Throws<IntranetRepositoryException>(() => householdMemberProxy.SaveRelations(MockRepository.GenerateStub<IDataProviderBase>(), fixture.Create<bool>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, householdMemberProxy.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that SaveRelations throws an IntranetRepositoryException when one of the households has an identifier equal to null.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsThrowsIntranetRepositoryExceptionWhenOneHouseholdIdentifierIsNull()
        {
            var fixture = new Fixture();
            var dataProviderBaseMock = MockRepository.GenerateMock<IDataProviderBase>();

            var householdMock = MockRepository.GenerateMock<IHousehold>();
            householdMock.Stub(m => m.Identifier)
                .Return(null)
                .Repeat.Any();
            householdMock.Stub(m => m.HouseholdMembers)
                .Return(new List<IHouseholdMember>(0))
                .Repeat.Any();

            var householdMemberProxy = new HouseholdMemberProxy
            {
                Identifier = Guid.NewGuid()
            };
            Assert.That(householdMemberProxy, Is.Not.Null);
            Assert.That(householdMemberProxy.Identifier, Is.Not.Null);
            Assert.That(householdMemberProxy.Identifier.HasValue, Is.True);
            Assert.That(householdMemberProxy.Households, Is.Not.Null);
            Assert.That(householdMemberProxy.Households, Is.Empty);

            householdMemberProxy.HouseholdAdd(householdMock);
            Assert.That(householdMemberProxy.Households, Is.Not.Null);
            Assert.That(householdMemberProxy.Households, Is.Not.Empty);
            Assert.That(householdMemberProxy.Households.Count(), Is.EqualTo(1));
            Assert.That(householdMemberProxy.Households.Contains(householdMock), Is.True);

            var exception = Assert.Throws<IntranetRepositoryException>(() => householdMemberProxy.SaveRelations(dataProviderBaseMock, fixture.Create<bool>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, householdMock.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);

            dataProviderBaseMock.AssertWasNotCalled(m => m.Clone());
            dataProviderBaseMock.AssertWasNotCalled(m => m.GetCollection<MemberOfHouseholdProxy>(Arg<string>.Is.Anything));
        }

        /// <summary>
        /// Tests that SaveRelations inserts the missing bindings between the given household member and their households.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsInsertsMissingMemberOfHouseholds()
        {
            var fixture = new Fixture();

            var householdIdentifier = Guid.NewGuid();
            var householdMock = MockRepository.GenerateMock<IHousehold>();
            householdMock.Stub(m => m.Identifier)
                .Return(householdIdentifier)
                .Repeat.Any();
            householdMock.Stub(m => m.HouseholdMembers)
                .Return(new List<IHouseholdMember>(0))
                .Repeat.Any();

            var householdMemberProxy = new HouseholdMemberProxy
            {
                Identifier = Guid.NewGuid()
            };
            Assert.That(householdMemberProxy, Is.Not.Null);
            Assert.That(householdMemberProxy.Identifier, Is.Not.Null);
            Assert.That(householdMemberProxy.Identifier.HasValue, Is.True);
            Assert.That(householdMemberProxy.Households, Is.Not.Null);
            Assert.That(householdMemberProxy.Households, Is.Empty);

            var dataProviderBaseMock = MockRepository.GenerateMock<IDataProviderBase>();
            dataProviderBaseMock.Stub(m => m.Clone())
                .Return(dataProviderBaseMock)
                .Repeat.Any();
            dataProviderBaseMock.Stub(m => m.GetCollection<MemberOfHouseholdProxy>(Arg<string>.Is.Anything))
                .Return(new List<MemberOfHouseholdProxy>(0))
                .Repeat.Any();
            dataProviderBaseMock.Stub(m => m.Add(Arg<MemberOfHouseholdProxy>.Is.NotNull))
                .WhenCalled(e =>
                {
                    var memberOfHouseholdProxy = (MemberOfHouseholdProxy) e.Arguments.ElementAt(0);
                    Assert.That(memberOfHouseholdProxy, Is.Not.Null);
                    Assert.That(memberOfHouseholdProxy.HouseholdMember, Is.Not.Null);
                    Assert.That(memberOfHouseholdProxy.HouseholdMember, Is.EqualTo(householdMemberProxy));
                    Assert.That(memberOfHouseholdProxy.HouseholdMemberIdentifier, Is.Not.Null);
                    Assert.That(memberOfHouseholdProxy.HouseholdMemberIdentifier.HasValue, Is.True);
                    // ReSharper disable PossibleInvalidOperationException
                    Assert.That(memberOfHouseholdProxy.HouseholdMemberIdentifier.Value, Is.EqualTo(householdMemberProxy.Identifier.Value));
                    // ReSharper restore PossibleInvalidOperationException
                    Assert.That(memberOfHouseholdProxy.Household, Is.Not.Null);
                    Assert.That(memberOfHouseholdProxy.Household, Is.EqualTo(householdMock));
                    Assert.That(memberOfHouseholdProxy.HouseholdIdentifier, Is.Not.Null);
                    Assert.That(memberOfHouseholdProxy.HouseholdIdentifier.HasValue, Is.True);
                    // ReSharper disable PossibleInvalidOperationException
                    Assert.That(memberOfHouseholdProxy.HouseholdIdentifier.Value, Is.EqualTo(householdMock.Identifier.Value));
                    // ReSharper restore PossibleInvalidOperationException
                })
                .Return(fixture.Create<MemberOfHouseholdProxy>())
                .Repeat.Any();

            householdMemberProxy.HouseholdAdd(householdMock);
            Assert.That(householdMemberProxy.Households, Is.Not.Null);
            Assert.That(householdMemberProxy.Households, Is.Not.Empty);
            Assert.That(householdMemberProxy.Households.Count(), Is.EqualTo(1));
            Assert.That(householdMemberProxy.Households.Contains(householdMock), Is.True);

            householdMemberProxy.SaveRelations(dataProviderBaseMock, fixture.Create<bool>());

            dataProviderBaseMock.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Times(2));
            dataProviderBaseMock.AssertWasCalled(m => m.GetCollection<MemberOfHouseholdProxy>(Arg<string>.Is.Equal(string.Format("SELECT MemberOfHouseholdIdentifier,HouseholdMemberIdentifier,HouseholdIdentifier,CreationTime FROM MemberOfHouseholds WHERE HouseholdMemberIdentifier='{0}' ORDER BY CreationTime DESC", householdMemberProxy.UniqueId))), opt => opt.Repeat.Times(1));
            dataProviderBaseMock.AssertWasCalled(m => m.Add(Arg<MemberOfHouseholdProxy>.Is.NotNull), opt => opt.Repeat.Times(1));
        }

        /// <summary>
        /// Tests that SaveRelations does not inserts the existing bindings between the given household member and their households.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsDoesNotInsertsExistingMemberOfHouseholds()
        {
            var fixture = new Fixture();

            var householdIdentifier = Guid.NewGuid();
            var householdMock = MockRepository.GenerateMock<IHousehold>();
            householdMock.Stub(m => m.Identifier)
                .Return(householdIdentifier)
                .Repeat.Any();
            householdMock.Stub(m => m.HouseholdMembers)
                .Return(new List<IHouseholdMember>(0))
                .Repeat.Any();

            var householdMemberProxy = new HouseholdMemberProxy
            {
                Identifier = Guid.NewGuid()
            };
            Assert.That(householdMemberProxy, Is.Not.Null);
            Assert.That(householdMemberProxy.Identifier, Is.Not.Null);
            Assert.That(householdMemberProxy.Identifier.HasValue, Is.True);
            Assert.That(householdMemberProxy.Households, Is.Not.Null);
            Assert.That(householdMemberProxy.Households, Is.Empty);

            var memberOfHouseholdProxy = fixture.Build<MemberOfHouseholdProxy>()
                .With(m => m.HouseholdMemberIdentifier, householdMemberProxy.Identifier)
                .With(m => m.HouseholdIdentifier, householdIdentifier)
                .Create();

            var dataProviderBaseMock = MockRepository.GenerateMock<IDataProviderBase>();
            dataProviderBaseMock.Stub(m => m.Clone())
                .Return(dataProviderBaseMock)
                .Repeat.Any();
            dataProviderBaseMock.Stub(m => m.GetCollection<MemberOfHouseholdProxy>(Arg<string>.Is.Anything))
                .Return(new List<MemberOfHouseholdProxy> {memberOfHouseholdProxy})
                .Repeat.Any();

            householdMemberProxy.HouseholdAdd(householdMock);
            Assert.That(householdMemberProxy.Households, Is.Not.Null);
            Assert.That(householdMemberProxy.Households, Is.Not.Empty);
            Assert.That(householdMemberProxy.Households.Count(), Is.EqualTo(1));
            Assert.That(householdMemberProxy.Households.Contains(householdMock), Is.True);

            householdMemberProxy.SaveRelations(dataProviderBaseMock, fixture.Create<bool>());

            dataProviderBaseMock.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Times(1));
            dataProviderBaseMock.AssertWasCalled(m => m.GetCollection<MemberOfHouseholdProxy>(Arg<string>.Is.Equal(string.Format("SELECT MemberOfHouseholdIdentifier,HouseholdMemberIdentifier,HouseholdIdentifier,CreationTime FROM MemberOfHouseholds WHERE HouseholdMemberIdentifier='{0}' ORDER BY CreationTime DESC", householdMemberProxy.UniqueId))), opt => opt.Repeat.Times(1));
            dataProviderBaseMock.AssertWasNotCalled(m => m.Add(Arg<MemberOfHouseholdProxy>.Is.Anything));
        }

        /// <summary>
        /// Tests that DeleteRelations throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            var householdMemberProxy = new HouseholdMemberProxy();
            Assert.That(householdMemberProxy, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdMemberProxy.DeleteRelations(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("dataProvider"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that DeleteRelations throws an IntranetRepositoryException when the identifier for the household member is null.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsThrowsIntranetRepositoryExceptionWhenIdentifierIsNull()
        {
            var householdMemberProxy = new HouseholdMemberProxy
            {
                Identifier = null
            };
            Assert.That(householdMemberProxy, Is.Not.Null);
            Assert.That(householdMemberProxy.Identifier, Is.Null);
            Assert.That(householdMemberProxy.Identifier.HasValue, Is.False);

            var exception = Assert.Throws<IntranetRepositoryException>(() => householdMemberProxy.DeleteRelations(MockRepository.GenerateStub<IDataProviderBase>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, householdMemberProxy.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that DeleteRelations calls Clone on the data provider two time.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsCallsCloneOnDataProviderTwoTime()
        {
            var dataProviderBaseMock = MockRepository.GenerateMock<IDataProviderBase>();
            dataProviderBaseMock.Stub(m => m.Clone())
                .Return(dataProviderBaseMock)
                .Repeat.Any();
            dataProviderBaseMock.Stub(m => m.GetCollection<MemberOfHouseholdProxy>(Arg<string>.Is.Anything))
                .Return(new List<MemberOfHouseholdProxy>(0))
                .Repeat.Any();
            dataProviderBaseMock.Stub(m => m.GetCollection<PaymentProxy>(Arg<string>.Is.Anything))
                .Return(new List<PaymentProxy>(0))
                .Repeat.Any();

            var householdMemberProxy = new HouseholdMemberProxy
            {
                Identifier = Guid.NewGuid()
            };
            Assert.That(householdMemberProxy, Is.Not.Null);
            Assert.That(householdMemberProxy.Identifier, Is.Not.Null);
            Assert.That(householdMemberProxy.Identifier.HasValue, Is.True);

            householdMemberProxy.DeleteRelations(dataProviderBaseMock);

            dataProviderBaseMock.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Times(2));
        }

        /// <summary>
        /// Tests that DeleteRelations calls GetCollection on the data provider to get the bindings which bind a given household member to all the households on which there is a membership.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsCallsGetCollectionOnDataProviderToGetMemberOfHouseholdProxies()
        {
            var dataProviderBaseMock = MockRepository.GenerateMock<IDataProviderBase>();
            dataProviderBaseMock.Stub(m => m.Clone())
                .Return(dataProviderBaseMock)
                .Repeat.Any();
            dataProviderBaseMock.Stub(m => m.GetCollection<MemberOfHouseholdProxy>(Arg<string>.Is.Anything))
                .Return(new List<MemberOfHouseholdProxy>(0))
                .Repeat.Any();
            dataProviderBaseMock.Stub(m => m.GetCollection<PaymentProxy>(Arg<string>.Is.Anything))
                .Return(new List<PaymentProxy>(0))
                .Repeat.Any();

            var householdMemberProxy = new HouseholdMemberProxy
            {
                Identifier = Guid.NewGuid()
            };
            Assert.That(householdMemberProxy, Is.Not.Null);
            Assert.That(householdMemberProxy.Identifier, Is.Not.Null);
            Assert.That(householdMemberProxy.Identifier.HasValue, Is.True);

            householdMemberProxy.DeleteRelations(dataProviderBaseMock);

            dataProviderBaseMock.AssertWasCalled(m => m.GetCollection<MemberOfHouseholdProxy>(Arg<string>.Is.Equal(string.Format("SELECT MemberOfHouseholdIdentifier,HouseholdMemberIdentifier,HouseholdIdentifier,CreationTime FROM MemberOfHouseholds WHERE HouseholdMemberIdentifier='{0}' ORDER BY CreationTime DESC", householdMemberProxy.UniqueId))));
        }

        /// <summary>
        /// Tests that DeleteRelations calls Delete on the data provider for each binding which bind a given household member to all the households on which there is a membership.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsCallsDeleteOnDataProviderForEachMemberOfHouseholdProxy()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());

            var memberOfHouseholdProxyCollection = new List<MemberOfHouseholdProxy>(random.Next(7, 15));
            while (memberOfHouseholdProxyCollection.Count < memberOfHouseholdProxyCollection.Capacity)
            {
                var householdMemberMock = MockRepository.GenerateMock<IHouseholdMember>();
                householdMemberMock.Stub(m => m.Identifier)
                    .Return(Guid.NewGuid())
                    .Repeat.Any();
                var householdMock = MockRepository.GenerateMock<IHousehold>();
                householdMock.Stub(m => m.Identifier)
                    .Return(Guid.NewGuid())
                    .Repeat.Any();
                memberOfHouseholdProxyCollection.Add(new MemberOfHouseholdProxy(householdMemberMock, householdMock));
            }
            var dataProviderBaseMock = MockRepository.GenerateMock<IDataProviderBase>();
            dataProviderBaseMock.Stub(m => m.Clone())
                .Return(dataProviderBaseMock)
                .Repeat.Any();
            dataProviderBaseMock.Stub(m => m.GetCollection<MemberOfHouseholdProxy>(Arg<string>.Is.Anything))
                .Return(memberOfHouseholdProxyCollection)
                .Repeat.Any();
            dataProviderBaseMock.Stub(m => m.GetCollection<PaymentProxy>(Arg<string>.Is.Anything))
                .Return(new List<PaymentProxy>(0))
                .Repeat.Any();
            dataProviderBaseMock.Stub(m => m.Get(Arg<HouseholdProxy>.Is.NotNull))
                .Return(fixture.Create<HouseholdProxy>())
                .Repeat.Any();
            dataProviderBaseMock.Stub(m => m.Delete(Arg<MemberOfHouseholdProxy>.Is.NotNull))
                .WhenCalled(e =>
                {
                    var memberOfHouseholdProxy = (MemberOfHouseholdProxy) e.Arguments.ElementAt(0);
                    Assert.That(memberOfHouseholdProxy, Is.Not.Null);
                    Assert.That(memberOfHouseholdProxyCollection.Contains(memberOfHouseholdProxy), Is.True);
                })
                .Repeat.Any();

            var householdMemberProxy = new HouseholdMemberProxy
            {
                Identifier = Guid.NewGuid()
            };
            Assert.That(householdMemberProxy, Is.Not.Null);
            Assert.That(householdMemberProxy.Identifier, Is.Not.Null);
            Assert.That(householdMemberProxy.Identifier.HasValue, Is.True);

            householdMemberProxy.DeleteRelations(dataProviderBaseMock);

            dataProviderBaseMock.AssertWasCalled(m => m.Delete(Arg<MemberOfHouseholdProxy>.Is.NotNull), opt => opt.Repeat.Times(memberOfHouseholdProxyCollection.Count));
        }

        /// <summary>
        /// Tests that DeleteRelations calls GetCollection on the data provider to get the payments made by the household member.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsCallsGetCollectionOnDataProviderToGetPayments()
        {
            var dataProviderBaseMock = MockRepository.GenerateMock<IDataProviderBase>();
            dataProviderBaseMock.Stub(m => m.Clone())
                .Return(dataProviderBaseMock)
                .Repeat.Any();
            dataProviderBaseMock.Stub(m => m.GetCollection<MemberOfHouseholdProxy>(Arg<string>.Is.Anything))
                .Return(new List<MemberOfHouseholdProxy>(0))
                .Repeat.Any();
            dataProviderBaseMock.Stub(m => m.GetCollection<PaymentProxy>(Arg<string>.Is.Anything))
                .Return(new List<PaymentProxy>(0))
                .Repeat.Any();

            var householdMemberProxy = new HouseholdMemberProxy
            {
                Identifier = Guid.NewGuid()
            };
            Assert.That(householdMemberProxy, Is.Not.Null);
            Assert.That(householdMemberProxy.Identifier, Is.Not.Null);
            Assert.That(householdMemberProxy.Identifier.HasValue, Is.True);

            householdMemberProxy.DeleteRelations(dataProviderBaseMock);

            dataProviderBaseMock.AssertWasCalled(m => m.GetCollection<PaymentProxy>(Arg<string>.Is.Equal(string.Format("SELECT PaymentIdentifier,StakeholderIdentifier,StakeholderType,DataProviderIdentifier,PaymentTime,PaymentReference,PaymentReceipt,CreationTime FROM Payments WHERE StakeholderIdentifier='{0}' ORDER BY PaymentTime DESC", householdMemberProxy.UniqueId))));
        }

        /// <summary>
        /// Tests that DeleteRelations calls Delete on the data provider for each payment made by the household member.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsCallsDeleteOnDataProviderForEachPayment()
        {
            var fixture = new Fixture();

            var paymentProxyCollection = fixture.CreateMany<PaymentProxy>(5).ToList();
            var dataProviderBaseMock = MockRepository.GenerateMock<IDataProviderBase>();
            dataProviderBaseMock.Stub(m => m.Clone())
                .Return(dataProviderBaseMock)
                .Repeat.Any();
            dataProviderBaseMock.Stub(m => m.GetCollection<MemberOfHouseholdProxy>(Arg<string>.Is.Anything))
                .Return(new List<MemberOfHouseholdProxy>(0))
                .Repeat.Any();
            dataProviderBaseMock.Stub(m => m.GetCollection<PaymentProxy>(Arg<string>.Is.Anything))
                .Return(paymentProxyCollection)
                .Repeat.Any();
            dataProviderBaseMock.Stub(m => m.Delete(Arg<PaymentProxy>.Is.NotNull))
                .WhenCalled(e =>
                {
                    var paymentProxy = (PaymentProxy) e.Arguments.ElementAt(0);
                    Assert.That(paymentProxy, Is.Not.Null);
                    Assert.That(paymentProxyCollection.Contains(paymentProxy), Is.True);
                })
                .Repeat.Any();

            var householdMemberProxy = new HouseholdMemberProxy
            {
                Identifier = Guid.NewGuid()
            };
            Assert.That(householdMemberProxy, Is.Not.Null);
            Assert.That(householdMemberProxy.Identifier, Is.Not.Null);
            Assert.That(householdMemberProxy.Identifier.HasValue, Is.True);

            householdMemberProxy.DeleteRelations(dataProviderBaseMock);

            dataProviderBaseMock.AssertWasCalled(m => m.Delete(Arg<PaymentProxy>.Is.NotNull), opt => opt.Repeat.Times(paymentProxyCollection.Count));
        }
    }
}
