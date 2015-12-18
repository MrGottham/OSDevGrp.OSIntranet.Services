﻿using System;
using System.Data;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
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
            Assert.That(householdMemberProxy.MailAddress, Is.Null);
            Assert.That(householdMemberProxy.ActivationCode, Is.Null);
            Assert.That(householdMemberProxy.ActivationTime, Is.Null);
            Assert.That(householdMemberProxy.ActivationTime.HasValue, Is.False);
            Assert.That(householdMemberProxy.IsActivated, Is.False);
            Assert.That(householdMemberProxy.CreationTime, Is.EqualTo(DateTime.MinValue));
            Assert.That(householdMemberProxy.Households, Is.Not.Null);
            Assert.That(householdMemberProxy.Households, Is.Empty);
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
        public void TestThatUniqueIdGetterGetsUniqueIdentificationForFoodItemProxy()
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
            Assert.That(sqlQueryForId, Is.EqualTo(string.Format("SELECT HouseholdMemberIdentifier,MailAddress,ActivationCode,ActivationTime,CreationTime FROM HouseholdMembers WHERE HouseholdMemberIdentifier='{0}'", householdMemberMock.Identifier.Value.ToString("D").ToUpper())));
            // ReSharper restore PossibleInvalidOperationException
        }

        /// <summary>
        /// Tests that GetSqlCommandForInsert returns the SQL statement to insert a household member.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatGetSqlCommandForInsertReturnsSqlCommandForInsert(bool hasActivationTime)
        {
            var identifier = Guid.NewGuid();
            var mailAddress = string.Format("test.{0}@osdevgrp.dk", identifier.ToString("D").ToLower());
            var activationTime = hasActivationTime ? DateTime.Now : (DateTime?) null;
            var householdMemberProxy = new HouseholdMemberProxy(mailAddress)
            {
                Identifier = identifier,
                ActivationTime = activationTime,
            };

            var sqlCommand = householdMemberProxy.GetSqlCommandForInsert();
            Assert.That(sqlCommand, Is.Not.Null);
            Assert.That(sqlCommand, Is.Not.Empty);
            Assert.That(sqlCommand, Is.EqualTo(string.Format("INSERT INTO HouseholdMembers (HouseholdMemberIdentifier,MailAddress,ActivationCode,ActivationTime,CreationTime) VALUES('{0}','{1}','{2}',{3},{4})", householdMemberProxy.UniqueId, mailAddress, householdMemberProxy.ActivationCode, DataRepositoryHelper.GetSqlValueForDateTime(activationTime.HasValue ? activationTime.Value.ToUniversalTime() : (DateTime?) null), DataRepositoryHelper.GetSqlValueForDateTime(householdMemberProxy.CreationTime.ToUniversalTime()))));
        }

        /// <summary>
        /// Tests that GetSqlCommandForUpdate returns the SQL statement to update a household member.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatGetSqlCommandForUpdateReturnsSqlCommandForUpdate(bool hasActivationTime)
        {
            var identifier = Guid.NewGuid();
            var mailAddress = string.Format("test.{0}@osdevgrp.dk", identifier.ToString("D").ToLower());
            var activationTime = hasActivationTime ? DateTime.Now : (DateTime?) null;
            var householdMemberProxy = new HouseholdMemberProxy(mailAddress)
            {
                Identifier = identifier,
                ActivationTime = activationTime,
            };

            var sqlCommand = householdMemberProxy.GetSqlCommandForUpdate();
            Assert.That(sqlCommand, Is.Not.Null);
            Assert.That(sqlCommand, Is.Not.Empty);
            Assert.That(sqlCommand, Is.EqualTo(string.Format("UPDATE HouseholdMembers SET MailAddress='{1}',ActivationCode='{2}',ActivationTime={3},CreationTime={4} WHERE HouseholdMemberIdentifier='{0}'", householdMemberProxy.UniqueId, mailAddress, householdMemberProxy.ActivationCode, DataRepositoryHelper.GetSqlValueForDateTime(activationTime.HasValue ? activationTime.Value.ToUniversalTime() : (DateTime?) null), DataRepositoryHelper.GetSqlValueForDateTime(householdMemberProxy.CreationTime.ToUniversalTime()))));
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
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatMapDataAndMapRelationsMapsDataIntoProxy(bool hasActivationTime)
        {
            var dataProviderBaseMock = MockRepository.GenerateMock<IDataProviderBase>();

            var dataReader = MockRepository.GenerateStub<MySqlDataReader>();
            dataReader.Stub(m => m.GetString(Arg<string>.Is.Equal("HouseholdMemberIdentifier")))
                .Return("F69CA810-0A8D-49D3-A50D-BCE0BD38C147")
                .Repeat.Any();
            dataReader.Stub(m => m.GetString(Arg<string>.Is.Equal("MailAddress")))
                .Return("test.f69ca810-0a8d-49d3-a50d-bce0bd38c147@osdevgrp.dk")
                .Repeat.Any();
            dataReader.Stub(m => m.GetString(Arg<string>.Is.Equal("ActivationCode")))
                .Return("BA9A4EE7B3E2AE9B5F4CC3E0DE0B6501")
                .Repeat.Any();
            dataReader.Stub(m => m.GetOrdinal("ActivationTime"))
                .Return(3)
                .Repeat.Any();
            dataReader.Stub(m => m.IsDBNull(Arg<int>.Is.Equal(3)))
                .Return(!hasActivationTime)
                .Repeat.Any();
            dataReader.Stub(m => m.GetDateTime(Arg<int>.Is.Equal(3)))
                .Return(DateTime.Now.ToUniversalTime())
                .Repeat.Any();
            dataReader.Stub(m => m.GetDateTime(Arg<string>.Is.Equal("CreationTime")))
                .Return(DateTime.Now.ToUniversalTime())
                .Repeat.Any();

            var householdMemberProxy = new HouseholdMemberProxy();
            Assert.That(householdMemberProxy, Is.Not.Null);
            Assert.That(householdMemberProxy.Identifier, Is.Null);
            Assert.That(householdMemberProxy.Identifier.HasValue, Is.False);
            Assert.That(householdMemberProxy.MailAddress, Is.Null);
            Assert.That(householdMemberProxy.ActivationCode, Is.Null);
            Assert.That(householdMemberProxy.ActivationTime, Is.Null);
            Assert.That(householdMemberProxy.ActivationTime.HasValue, Is.False);
            Assert.That(householdMemberProxy.IsActivated, Is.False);
            Assert.That(householdMemberProxy.CreationTime, Is.EqualTo(DateTime.MinValue));
            Assert.That(householdMemberProxy.Households, Is.Not.Null);
            Assert.That(householdMemberProxy.Households, Is.Empty);

            householdMemberProxy.MapData(dataReader, dataProviderBaseMock);
            householdMemberProxy.MapRelations(dataProviderBaseMock);
            Assert.That(householdMemberProxy, Is.Not.Null);
            Assert.That(householdMemberProxy.Identifier, Is.Not.Null);
            Assert.That(householdMemberProxy.Identifier.HasValue, Is.True);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(householdMemberProxy.Identifier.Value.ToString("D").ToUpper(), Is.EqualTo(dataReader.GetString("HouseholdMemberIdentifier")));
            // ReSharper restore PossibleInvalidOperationException
            Assert.That(householdMemberProxy.MailAddress, Is.Not.Null);
            Assert.That(householdMemberProxy.MailAddress, Is.Not.Empty);
            Assert.That(householdMemberProxy.MailAddress, Is.EqualTo(dataReader.GetString("MailAddress")));
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
            Assert.That(householdMemberProxy.CreationTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);
            Assert.That(householdMemberProxy.Households, Is.Not.Null);
            Assert.That(householdMemberProxy.Households, Is.Empty);
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
    }
}
