using System;
using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Resources;
using AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.DataProxies.FoodWaste
{
    /// <summary>
    /// Test the data proxy which bind a given household member to a given household.
    /// </summary>
    [TestFixture]
    public class MemberOfHouseholdProxyTests
    {
        /// <summary>
        /// Tests that the constructor without a household member and a household initialize a data proxy which bind a given household member to a given household.
        /// </summary>
        [Test]
        public void TestThatConstructorWithoutHouseholdMemberAndHouseholdInitializeMemberOfHouseholdProxy()
        {
            var memberOfHouseholdProxy = new MemberOfHouseholdProxy();
            Assert.That(memberOfHouseholdProxy, Is.Not.Null);
            Assert.That(memberOfHouseholdProxy.Identifier, Is.Null);
            Assert.That(memberOfHouseholdProxy.Identifier.HasValue, Is.False);
            Assert.That(memberOfHouseholdProxy.HouseholdMember, Is.Null);
            Assert.That(memberOfHouseholdProxy.HouseholdMemberIdentifier, Is.Null);
            Assert.That(memberOfHouseholdProxy.HouseholdMemberIdentifier.HasValue, Is.False);
            Assert.That(memberOfHouseholdProxy.Household, Is.Null);
            Assert.That(memberOfHouseholdProxy.HouseholdIdentifier, Is.Null);
            Assert.That(memberOfHouseholdProxy.HouseholdIdentifier.HasValue, Is.False);
            Assert.That(memberOfHouseholdProxy.CreationTime, Is.EqualTo(DateTime.MinValue));
        }

        /// <summary>
        /// Tests that the constructor with a household member and a household initialize a data proxy which bind a given household member to a given household.
        /// </summary>
        [Test]
        public void TestThatConstructorWithHouseholdMemberAndHouseholdInitializeMemberOfHouseholdProxy()
        {
            var householdMemberIdentifier = Guid.NewGuid();
            var householdMemberMock = MockRepository.GenerateMock<IHouseholdMember>();
            householdMemberMock.Stub(m => m.Identifier)
                .Return(householdMemberIdentifier)
                .Repeat.Any();

            var householdIdentifier = Guid.NewGuid();
            var householdMock = MockRepository.GenerateMock<IHousehold>();
            householdMock.Stub(m => m.Identifier)
                .Return(householdIdentifier)
                .Repeat.Any();

            var memberOfHouseholdProxy = new MemberOfHouseholdProxy(householdMemberMock, householdMock);
            Assert.That(memberOfHouseholdProxy, Is.Not.Null);
            Assert.That(memberOfHouseholdProxy.Identifier, Is.Null);
            Assert.That(memberOfHouseholdProxy.Identifier.HasValue, Is.False);
            Assert.That(memberOfHouseholdProxy.HouseholdMember, Is.Not.Null);
            Assert.That(memberOfHouseholdProxy.HouseholdMember, Is.EqualTo(householdMemberMock));
            Assert.That(memberOfHouseholdProxy.HouseholdMemberIdentifier, Is.Not.Null);
            Assert.That(memberOfHouseholdProxy.HouseholdMemberIdentifier.HasValue, Is.True);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(memberOfHouseholdProxy.HouseholdMemberIdentifier.Value, Is.EqualTo(householdMemberIdentifier));
            // ReSharper restore PossibleInvalidOperationException
            Assert.That(memberOfHouseholdProxy.Household, Is.Not.Null);
            Assert.That(memberOfHouseholdProxy.Household, Is.EqualTo(householdMock));
            Assert.That(memberOfHouseholdProxy.HouseholdIdentifier, Is.Not.Null);
            Assert.That(memberOfHouseholdProxy.HouseholdIdentifier.HasValue, Is.True);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(memberOfHouseholdProxy.HouseholdIdentifier.Value, Is.EqualTo(householdIdentifier));
            // ReSharper restore PossibleInvalidOperationException
            Assert.That(memberOfHouseholdProxy.CreationTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);
        }

        /// <summary>
        /// Tests that the constructor with a household member and a household throws an ArgumentNullException when the household member is null.
        /// </summary>
        [Test]
        public void TestThatConstructorWithHouseholdMemberAndHouseholdThrowsArgumentNullExceptionWhenHouseholdMemberIsNull()
        {
            var householdMock = MockRepository.GenerateMock<IHousehold>();
            householdMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var exception = Assert.Throws<ArgumentNullException>(() => new MemberOfHouseholdProxy(null, householdMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("householdMember"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor with a household member and a household throws an ArgumentNullException when the household is null.
        /// </summary>
        [Test]
        public void TestThatConstructorWithHouseholdMemberAndHouseholdThrowsArgumentNullExceptionWhenHouseholdIsNull()
        {
            var householdMemberMock = MockRepository.GenerateMock<IHouseholdMember>();
            householdMemberMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var exception = Assert.Throws<ArgumentNullException>(() => new MemberOfHouseholdProxy(householdMemberMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("household"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the setter for HouseholdMemberIdentifier updates the value of HouseholdMemberIdentifier.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberIdentifierSetterUpdatesValueOfHouseholdMemberIdentifier()
        {
            var householdMemberIdentifier = Guid.NewGuid();
            var householdMemberMock = MockRepository.GenerateMock<IHouseholdMember>();
            householdMemberMock.Stub(m => m.Identifier)
                .Return(householdMemberIdentifier)
                .Repeat.Any();

            var householdMock = MockRepository.GenerateMock<IHousehold>();
            householdMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var memberOfHouseholdProxy = new MemberOfHouseholdProxy(householdMemberMock, householdMock);
            Assert.That(memberOfHouseholdProxy, Is.Not.Null);
            Assert.That(memberOfHouseholdProxy.HouseholdMember, Is.Not.Null);
            Assert.That(memberOfHouseholdProxy.HouseholdMember, Is.EqualTo(householdMemberMock));
            Assert.That(memberOfHouseholdProxy.HouseholdMemberIdentifier, Is.Not.Null);
            Assert.That(memberOfHouseholdProxy.HouseholdMemberIdentifier.HasValue, Is.True);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(memberOfHouseholdProxy.HouseholdMemberIdentifier.Value, Is.EqualTo(householdMemberIdentifier));
            // ReSharper restore PossibleInvalidOperationException

            var newValue = Guid.NewGuid();
            Assert.That(newValue, Is.Not.EqualTo(householdMemberIdentifier));

            memberOfHouseholdProxy.HouseholdMemberIdentifier = newValue;
            Assert.That(memberOfHouseholdProxy.HouseholdMember, Is.Null);
            Assert.That(memberOfHouseholdProxy.HouseholdMemberIdentifier, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(memberOfHouseholdProxy.HouseholdMemberIdentifier.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(memberOfHouseholdProxy.HouseholdMemberIdentifier.Value, Is.EqualTo(newValue));
            // ReSharper restore PossibleInvalidOperationException
        }

        /// <summary>
        /// Tests that the setter for HouseholdMemberIdentifier updates the value of HouseholdMemberIdentifier with null.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberIdentifierSetterUpdatesValueOfHouseholdMemberIdentifierWithNull()
        {
            var householdMemberIdentifier = Guid.NewGuid();
            var householdMemberMock = MockRepository.GenerateMock<IHouseholdMember>();
            householdMemberMock.Stub(m => m.Identifier)
                .Return(householdMemberIdentifier)
                .Repeat.Any();

            var householdMock = MockRepository.GenerateMock<IHousehold>();
            householdMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var memberOfHouseholdProxy = new MemberOfHouseholdProxy(householdMemberMock, householdMock);
            Assert.That(memberOfHouseholdProxy, Is.Not.Null);
            Assert.That(memberOfHouseholdProxy.HouseholdMember, Is.Not.Null);
            Assert.That(memberOfHouseholdProxy.HouseholdMember, Is.EqualTo(householdMemberMock));
            Assert.That(memberOfHouseholdProxy.HouseholdMemberIdentifier, Is.Not.Null);
            Assert.That(memberOfHouseholdProxy.HouseholdMemberIdentifier.HasValue, Is.True);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(memberOfHouseholdProxy.HouseholdMemberIdentifier.Value, Is.EqualTo(householdMemberIdentifier));
            // ReSharper restore PossibleInvalidOperationException

            memberOfHouseholdProxy.HouseholdMemberIdentifier = null;
            Assert.That(memberOfHouseholdProxy.HouseholdMember, Is.Null);
            Assert.That(memberOfHouseholdProxy.HouseholdMemberIdentifier, Is.Null);
            Assert.That(memberOfHouseholdProxy.HouseholdMemberIdentifier.HasValue, Is.False);
        }

        /// <summary>
        /// Tests that the setter for HouseholdIdentifier updates the value of HouseholdIdentifier.
        /// </summary>
        [Test]
        public void TestThatHouseholdIdentifierSetterUpdatesValueOfHouseholdIdentifier()
        {
            var householdMemberMock = MockRepository.GenerateMock<IHouseholdMember>();
            householdMemberMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var householdIdentifier = Guid.NewGuid();
            var householdMock = MockRepository.GenerateMock<IHousehold>();
            householdMock.Stub(m => m.Identifier)
                .Return(householdIdentifier)
                .Repeat.Any();

            var memberOfHouseholdProxy = new MemberOfHouseholdProxy(householdMemberMock, householdMock);
            Assert.That(memberOfHouseholdProxy, Is.Not.Null);
            Assert.That(memberOfHouseholdProxy.Household, Is.Not.Null);
            Assert.That(memberOfHouseholdProxy.HouseholdIdentifier, Is.Not.Null);
            Assert.That(memberOfHouseholdProxy.HouseholdIdentifier.HasValue, Is.True);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(memberOfHouseholdProxy.HouseholdIdentifier.Value, Is.EqualTo(householdIdentifier));
            // ReSharper restore PossibleInvalidOperationException

            var newValue = Guid.NewGuid();
            Assert.That(newValue, Is.Not.EqualTo(householdIdentifier));

            memberOfHouseholdProxy.HouseholdIdentifier = newValue;
            Assert.That(memberOfHouseholdProxy.Household, Is.Null);
            Assert.That(memberOfHouseholdProxy.HouseholdIdentifier, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(memberOfHouseholdProxy.HouseholdIdentifier.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(memberOfHouseholdProxy.HouseholdIdentifier.Value, Is.EqualTo(newValue));
            // ReSharper restore PossibleInvalidOperationException
        }

        /// <summary>
        /// Tests that the setter for HouseholdIdentifier updates the value of HouseholdIdentifier with null.
        /// </summary>
        [Test]
        public void TestThatHouseholdIdentifierSetterUpdatesValueOfHouseholdIdentifierWithNull()
        {
            var householdMemberMock = MockRepository.GenerateMock<IHouseholdMember>();
            householdMemberMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var householdIdentifier = Guid.NewGuid();
            var householdMock = MockRepository.GenerateMock<IHousehold>();
            householdMock.Stub(m => m.Identifier)
                .Return(householdIdentifier)
                .Repeat.Any();

            var memberOfHouseholdProxy = new MemberOfHouseholdProxy(householdMemberMock, householdMock);
            Assert.That(memberOfHouseholdProxy, Is.Not.Null);
            Assert.That(memberOfHouseholdProxy.Household, Is.Not.Null);
            Assert.That(memberOfHouseholdProxy.HouseholdIdentifier, Is.Not.Null);
            Assert.That(memberOfHouseholdProxy.HouseholdIdentifier.HasValue, Is.True);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(memberOfHouseholdProxy.HouseholdIdentifier.Value, Is.EqualTo(householdIdentifier));
            // ReSharper restore PossibleInvalidOperationException

            memberOfHouseholdProxy.HouseholdIdentifier = null;
            Assert.That(memberOfHouseholdProxy.Household, Is.Null);
            Assert.That(memberOfHouseholdProxy.HouseholdIdentifier, Is.Null);
            Assert.That(memberOfHouseholdProxy.HouseholdIdentifier.HasValue, Is.False);
        }

        /// <summary>
        /// Tests that getter for UniqueId throws an IntranetRepositoryException when the binding for a given household member to a given household has no identifier.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterThrowsIntranetRepositoryExceptionWhenMemberOfHouseholdProxyHasNoIdentifier()
        {
            var memberOfHouseholdProxy = new MemberOfHouseholdProxy
            {
                Identifier = null
            };

            // ReSharper disable UnusedVariable
            var exception = Assert.Throws<IntranetRepositoryException>(() => { var uniqueId = memberOfHouseholdProxy.UniqueId; });
            // ReSharper restore UnusedVariable
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, memberOfHouseholdProxy.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that getter for UniqueId gets the unique identifier for the binding for a given household member to a given household.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterGetsUniqueIdentificationForMemberOfHouseholdProxy()
        {
            var memberOfHouseholdProxy = new MemberOfHouseholdProxy
            {
                Identifier = Guid.NewGuid()
            };

            var uniqueId = memberOfHouseholdProxy.UniqueId;
            Assert.That(uniqueId, Is.Not.Null);
            Assert.That(uniqueId, Is.Not.Empty);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(uniqueId, Is.EqualTo(memberOfHouseholdProxy.Identifier.Value.ToString("D").ToUpper()));
            // ReSharper restore PossibleInvalidOperationException
        }

        /// <summary>
        /// Tests that GetSqlQueryForId throws an ArgumentNullException when binding for a given household member to a given household is null.
        /// </summary>
        [Test]
        public void TestThatGetSqlQueryForIdThrowsArgumentNullExceptionWhenMemberOfHouseholdProxyIsNull()
        {
            var memberOfHouseholdProxy = new MemberOfHouseholdProxy();

            // ReSharper disable UnusedVariable
            var exception = Assert.Throws<ArgumentNullException>(() => { var sqlQueryForId = memberOfHouseholdProxy.GetSqlQueryForId(null); });
            // ReSharper restore UnusedVariable
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("memberOfHouseholdProxy"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetSqlQueryForId throws an IntranetRepositoryException when the identifier on the given binding for a given household member to a given household has no value.
        /// </summary>
        [Test]
        public void TestThatGetSqlQueryForIdThrowsIntranetRepositoryExceptionWhenIdentifierOnMemberOfHouseholdProxyHasNoValue()
        {
            var memberOfHouseholdProxyMock = MockRepository.GenerateMock<IMemberOfHouseholdProxy>();
            memberOfHouseholdProxyMock.Expect(m => m.Identifier)
                .Return(null)
                .Repeat.Any();

            var memberOfHouseholdProxy = new MemberOfHouseholdProxy();

            // ReSharper disable UnusedVariable
            var exception = Assert.Throws<IntranetRepositoryException>(() => { var sqlQueryForId = memberOfHouseholdProxy.GetSqlQueryForId(memberOfHouseholdProxyMock); });
            // ReSharper restore UnusedVariable
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, memberOfHouseholdProxyMock.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetSqlQueryForId returns the SQL statement for selecting the given binding for a given household member to a given household.
        /// </summary>
        [Test]
        public void TestThatGetSqlQueryForIdReturnsSqlQueryForId()
        {
            var memberOfHouseholdProxyMock = MockRepository.GenerateMock<IMemberOfHouseholdProxy>();
            memberOfHouseholdProxyMock.Expect(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var memberOfHouseholdProxy = new MemberOfHouseholdProxy();

            var sqlQueryForId = memberOfHouseholdProxy.GetSqlQueryForId(memberOfHouseholdProxyMock);
            Assert.That(sqlQueryForId, Is.Not.Null);
            Assert.That(sqlQueryForId, Is.Not.Empty);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(sqlQueryForId, Is.EqualTo(string.Format("SELECT MemberOfHouseholdIdentifier,HouseholdMemberIdentifier,HouseholdIdentifier,CreationTime FROM MemberOfHouseholds WHERE MemberOfHouseholdIdentifier='{0}'", memberOfHouseholdProxyMock.Identifier.Value.ToString("D").ToUpper())));
            // ReSharper restore PossibleInvalidOperationException
        }

        /// <summary>
        /// Tests that GetSqlCommandForInsert returns the SQL statement to insert a binding for a given household member to a given household.
        /// </summary>
        [Test]
        public void TestThatGetSqlCommandForInsertReturnsSqlCommandForInsert()
        {
            var householdMemberIdentifier = Guid.NewGuid();
            var householdMemberMock = MockRepository.GenerateMock<IHouseholdMember>();
            householdMemberMock.Stub(m => m.Identifier)
                .Return(householdMemberIdentifier)
                .Repeat.Any();

            var householdIdentifier = Guid.NewGuid();
            var householdMock = MockRepository.GenerateMock<IHousehold>();
            householdMock.Stub(m => m.Identifier)
                .Return(householdIdentifier)
                .Repeat.Any();

            var identifier = Guid.NewGuid();
            var creationTime = DateTime.Now;
            var memberOfHouseholdProxy = new MemberOfHouseholdProxy(householdMemberMock, householdMock, creationTime)
            {
                Identifier = identifier
            };

            var sqlCommand = memberOfHouseholdProxy.GetSqlCommandForInsert();
            Assert.That(sqlCommand, Is.Not.Null);
            Assert.That(sqlCommand, Is.Not.Empty);
            Assert.That(sqlCommand, Is.EqualTo(string.Format("INSERT INTO MemberOfHouseholds (MemberOfHouseholdIdentifier,HouseholdMemberIdentifier,HouseholdIdentifier,CreationTime) VALUES('{0}','{1}','{2}',{3})", memberOfHouseholdProxy.UniqueId, householdMemberIdentifier.ToString("D").ToUpper(), householdIdentifier.ToString("D").ToUpper(), DataRepositoryHelper.GetSqlValueForDateTime(creationTime))));
        }

        /// <summary>
        /// Tests that GetSqlCommandForUpdate returns the SQL statement to update a given household member to a given household.
        /// </summary>
        [Test]
        public void TestThatGetSqlCommandForUpdateReturnsSqlCommandForUpdate()
        {
            var householdMemberIdentifier = Guid.NewGuid();
            var householdMemberMock = MockRepository.GenerateMock<IHouseholdMember>();
            householdMemberMock.Stub(m => m.Identifier)
                .Return(householdMemberIdentifier)
                .Repeat.Any();

            var householdIdentifier = Guid.NewGuid();
            var householdMock = MockRepository.GenerateMock<IHousehold>();
            householdMock.Stub(m => m.Identifier)
                .Return(householdIdentifier)
                .Repeat.Any();

            var identifier = Guid.NewGuid();
            var creationTime = DateTime.Now;
            var memberOfHouseholdProxy = new MemberOfHouseholdProxy(householdMemberMock, householdMock, creationTime)
            {
                Identifier = identifier
            };

            var sqlCommand = memberOfHouseholdProxy.GetSqlCommandForUpdate();
            Assert.That(sqlCommand, Is.Not.Null);
            Assert.That(sqlCommand, Is.Not.Empty);
            Assert.That(sqlCommand, Is.EqualTo(string.Format("UPDATE MemberOfHouseholds SET HouseholdMemberIdentifier='{1}',HouseholdIdentifier='{2}',CreationTime={3} WHERE MemberOfHouseholdIdentifier='{0}'", memberOfHouseholdProxy.UniqueId, householdMemberIdentifier.ToString("D").ToUpper(), householdIdentifier.ToString("D").ToUpper(), DataRepositoryHelper.GetSqlValueForDateTime(creationTime))));
        }

        /// <summary>
        /// Tests that GetSqlCommandForDelete returns the SQL statement to delete a given household member to a given household.
        /// </summary>
        [Test]
        public void TestThatGetSqlCommandForDeleteReturnsSqlCommandForDelete()
        {
            var identifier = Guid.NewGuid();
            var memberOfHouseholdProxy = new MemberOfHouseholdProxy
            {
                Identifier = identifier
            };

            var sqlCommand = memberOfHouseholdProxy.GetSqlCommandForDelete();
            Assert.That(sqlCommand, Is.Not.Null);
            Assert.That(sqlCommand, Is.Not.Empty);
            Assert.That(sqlCommand, Is.EqualTo(string.Format("DELETE FROM MemberOfHouseholds WHERE MemberOfHouseholdIdentifier='{0}'", memberOfHouseholdProxy.UniqueId)));
        }

        /// <summary>
        /// Tests that MapData throws an ArgumentNullException if the data reader is null.
        /// </summary>
        [Test]
        public void TestThatMapDataThrowsArgumentNullExceptionIfDataReaderIsNull()
        {
            var memberOfHouseholdProxy = new MemberOfHouseholdProxy();
            Assert.That(memberOfHouseholdProxy, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => memberOfHouseholdProxy.MapData(null, MockRepository.GenerateMock<IDataProviderBase>()));
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
            var memberOfHouseholdProxy = new MemberOfHouseholdProxy();
            Assert.That(memberOfHouseholdProxy, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => memberOfHouseholdProxy.MapData(MockRepository.GenerateStub<MySqlDataReader>(), null));
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

            var memberOfHouseholdProxy = new MemberOfHouseholdProxy();
            Assert.That(memberOfHouseholdProxy, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => memberOfHouseholdProxy.MapData(dataReader, MockRepository.GenerateMock<IDataProviderBase>()));
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
        public void TestThatMapDataAndMapRelationsMapsDataIntoProxy()
        {
            var fixture = new Fixture();

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
                    Assert.That(householdMemberProxy.Identifier.Value, Is.EqualTo(Guid.Parse("F0A03C7D-0E05-4A2E-A360-B4B51242D33B")));
                    // ReSharper restore PossibleInvalidOperationException
                })
                .Return(fixture.Create<HouseholdMemberProxy>())
                .Repeat.Any();
            dataProviderBaseMock.Stub(m => m.Get(Arg<HouseholdProxy>.Is.NotNull))
                .WhenCalled(e =>
                {
                    var householdProxy = (HouseholdProxy)e.Arguments.ElementAt(0);
                    Assert.That(householdProxy, Is.Not.Null);
                    Assert.That(householdProxy.Identifier, Is.Not.Null);
                    Assert.That(householdProxy.Identifier.HasValue, Is.True);
                    // ReSharper disable PossibleInvalidOperationException
                    Assert.That(householdProxy.Identifier.Value, Is.EqualTo(Guid.Parse("723F0DE5-4108-46A8-B952-C4BE521BB5F1")));
                    // ReSharper restore PossibleInvalidOperationException
                })
                .Return(fixture.Create<HouseholdProxy>())
                .Repeat.Any();

            var dataReader = MockRepository.GenerateStub<MySqlDataReader>();
            dataReader.Stub(m => m.GetString(Arg<string>.Is.Equal("MemberOfHouseholdIdentifier")))
                .Return("C8FA8391-4AB0-48F2-AA6E-DBE8CDF53838")
                .Repeat.Any();
            dataReader.Stub(m => m.GetString(Arg<string>.Is.Equal("HouseholdMemberIdentifier")))
                .Return("F0A03C7D-0E05-4A2E-A360-B4B51242D33B")
                .Repeat.Any();
            dataReader.Stub(m => m.GetString(Arg<string>.Is.Equal("HouseholdIdentifier")))
                .Return("723F0DE5-4108-46A8-B952-C4BE521BB5F1")
                .Repeat.Any();
            dataReader.Stub(m => m.GetDateTime(Arg<string>.Is.Equal("CreationTime")))
                .Return(DateTime.Now.ToUniversalTime())
                .Repeat.Any();

            var memberOfHouseholdProxy = new MemberOfHouseholdProxy();
            Assert.That(memberOfHouseholdProxy, Is.Not.Null);
            Assert.That(memberOfHouseholdProxy.Identifier, Is.Null);
            Assert.That(memberOfHouseholdProxy.Identifier.HasValue, Is.False);
            Assert.That(memberOfHouseholdProxy.HouseholdMember, Is.Null);
            Assert.That(memberOfHouseholdProxy.HouseholdMemberIdentifier, Is.Null);
            Assert.That(memberOfHouseholdProxy.HouseholdMemberIdentifier.HasValue, Is.False);
            Assert.That(memberOfHouseholdProxy.Household, Is.Null);
            Assert.That(memberOfHouseholdProxy.HouseholdIdentifier, Is.Null);
            Assert.That(memberOfHouseholdProxy.HouseholdIdentifier.HasValue, Is.False);
            Assert.That(memberOfHouseholdProxy.CreationTime, Is.EqualTo(DateTime.MinValue));

            memberOfHouseholdProxy.MapData(dataReader, dataProviderBaseMock);
            memberOfHouseholdProxy.MapRelations(dataProviderBaseMock);
            Assert.That(memberOfHouseholdProxy, Is.Not.Null);
            Assert.That(memberOfHouseholdProxy.Identifier, Is.Not.Null);
            Assert.That(memberOfHouseholdProxy.Identifier.HasValue, Is.True);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(memberOfHouseholdProxy.Identifier.Value.ToString("D").ToUpper(), Is.EqualTo(dataReader.GetString("MemberOfHouseholdIdentifier")));
            // ReSharper restore PossibleInvalidOperationException
            Assert.That(memberOfHouseholdProxy.HouseholdMember, Is.Not.Null);
            Assert.That(memberOfHouseholdProxy.HouseholdMemberIdentifier, Is.Not.Null);
            Assert.That(memberOfHouseholdProxy.HouseholdMemberIdentifier.HasValue, Is.True);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(memberOfHouseholdProxy.HouseholdMemberIdentifier.Value.ToString("D").ToUpper(), Is.EqualTo(dataReader.GetString("HouseholdMemberIdentifier")));
            // ReSharper restore PossibleInvalidOperationException
            Assert.That(memberOfHouseholdProxy.Household, Is.Not.Null);
            Assert.That(memberOfHouseholdProxy.HouseholdIdentifier, Is.Not.Null);
            Assert.That(memberOfHouseholdProxy.HouseholdIdentifier.HasValue, Is.True);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(memberOfHouseholdProxy.HouseholdIdentifier.Value.ToString("D").ToUpper(), Is.EqualTo(dataReader.GetString("HouseholdIdentifier")));
            // ReSharper restore PossibleInvalidOperationException
            Assert.That(memberOfHouseholdProxy.CreationTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);

            dataProviderBaseMock.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Times(2));
            dataProviderBaseMock.AssertWasCalled(m => m.Get(Arg<HouseholdMemberProxy>.Is.NotNull));
            dataProviderBaseMock.AssertWasCalled(m => m.Get(Arg<HouseholdProxy>.Is.NotNull));
        }

        /// <summary>
        /// Tests that MapRelations throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatMapRelationsThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            var memberOfHouseholdProxy = new MemberOfHouseholdProxy();
            Assert.That(memberOfHouseholdProxy, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => memberOfHouseholdProxy.MapRelations(null));
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

            var memberOfHouseholdProxy = new MemberOfHouseholdProxy();
            Assert.That(memberOfHouseholdProxy, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => memberOfHouseholdProxy.SaveRelations(null, fixture.Create<bool>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("dataProvider"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that SaveRelations throws an IntranetRepositoryException when the identifier for the given household member to a given household is null.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsThrowsIntranetRepositoryExceptionWhenIdentifierIsNull()
        {
            var fixture = new Fixture();

            var memberOfHouseholdProxy = new MemberOfHouseholdProxy
            {
                Identifier = null
            };
            Assert.That(memberOfHouseholdProxy, Is.Not.Null);
            Assert.That(memberOfHouseholdProxy.Identifier, Is.Null);
            Assert.That(memberOfHouseholdProxy.Identifier.HasValue, Is.False);

            var exception = Assert.Throws<IntranetRepositoryException>(() => memberOfHouseholdProxy.SaveRelations(MockRepository.GenerateStub<IDataProviderBase>(), fixture.Create<bool>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, memberOfHouseholdProxy.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that DeleteRelations throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            var memberOfHouseholdProxy = new MemberOfHouseholdProxy();
            Assert.That(memberOfHouseholdProxy, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => memberOfHouseholdProxy.DeleteRelations(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("dataProvider"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that DeleteRelations throws an IntranetRepositoryException when the identifier for the given household member to a given household is null.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsThrowsIntranetRepositoryExceptionWhenIdentifierIsNull()
        {
            var memberOfHouseholdProxy = new MemberOfHouseholdProxy
            {
                Identifier = null
            };
            Assert.That(memberOfHouseholdProxy, Is.Not.Null);
            Assert.That(memberOfHouseholdProxy.Identifier, Is.Null);
            Assert.That(memberOfHouseholdProxy.Identifier.HasValue, Is.False);

            var exception = Assert.Throws<IntranetRepositoryException>(() => memberOfHouseholdProxy.DeleteRelations(MockRepository.GenerateStub<IDataProviderBase>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, memberOfHouseholdProxy.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }
    }
}
