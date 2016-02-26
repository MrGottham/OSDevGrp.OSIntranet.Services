using System;
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
    /// Test the data proxy to a household.
    /// </summary>
    [TestFixture]
    public class HouseholdProxyTests
    {
        /// <summary>
        /// Tests that the constructor initialize a data proxy to a household.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeHouseholdProxy()
        {
            var householdProxy = new HouseholdProxy();
            Assert.That(householdProxy, Is.Not.Null);
            Assert.That(householdProxy.Identifier, Is.Null);
            Assert.That(householdProxy.Identifier.HasValue, Is.False);
            Assert.That(householdProxy.Name, Is.Null);
            Assert.That(householdProxy.Description, Is.Null);
            Assert.That(householdProxy.CreationTime, Is.EqualTo(DateTime.MinValue));
            Assert.That(householdProxy.HouseholdMembers, Is.Not.Null);
            Assert.That(householdProxy.HouseholdMembers, Is.Empty);
        }

        /// <summary>
        /// Tests that getter for UniqueId throws an IntranetRepositoryException when the household has no identifier.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterThrowsIntranetRepositoryExceptionWhenHouseholdProxyHasNoIdentifier()
        {
            var householdProxy = new HouseholdProxy
            {
                Identifier = null
            };

            // ReSharper disable UnusedVariable
            var exception = Assert.Throws<IntranetRepositoryException>(() => { var uniqueId = householdProxy.UniqueId; });
            // ReSharper restore UnusedVariable
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, householdProxy.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that getter for UniqueId gets the unique identifier for the household.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterGetsUniqueIdentificationForHouseholdProxy()
        {
            var householdProxy = new HouseholdProxy
            {
                Identifier = Guid.NewGuid()
            };

            var uniqueId = householdProxy.UniqueId;
            Assert.That(uniqueId, Is.Not.Null);
            Assert.That(uniqueId, Is.Not.Empty);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(uniqueId, Is.EqualTo(householdProxy.Identifier.Value.ToString("D").ToUpper()));
            // ReSharper restore PossibleInvalidOperationException
        }

        /// <summary>
        /// Tests that GetSqlQueryForId throws an ArgumentNullException when the given household is null.
        /// </summary>
        [Test]
        public void TestThatGetSqlQueryForIdThrowsArgumentNullExceptionWhenHouseholdIsNull()
        {
            var householdProxy = new HouseholdProxy();

            // ReSharper disable UnusedVariable
            var exception = Assert.Throws<ArgumentNullException>(() => { var sqlQueryForId = householdProxy.GetSqlQueryForId(null); });
            // ReSharper restore UnusedVariable
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("household"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetSqlQueryForId throws an IntranetRepositoryException when the identifier on the given household has no value.
        /// </summary>
        [Test]
        public void TestThatGetSqlQueryForIdThrowsIntranetRepositoryExceptionWhenIdentifierOnHouseholdHasNoValue()
        {
            var householdMock = MockRepository.GenerateMock<IHousehold>();
            householdMock.Expect(m => m.Identifier)
                .Return(null)
                .Repeat.Any();

            var householdProxy = new HouseholdProxy();

            // ReSharper disable UnusedVariable
            var exception = Assert.Throws<IntranetRepositoryException>(() => { var sqlQueryForId = householdProxy.GetSqlQueryForId(householdMock); });
            // ReSharper restore UnusedVariable
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, householdProxy.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetSqlQueryForId returns the SQL statement for selecting the given household.
        /// </summary>
        [Test]
        public void TestThatGetSqlQueryForIdReturnsSqlQueryForId()
        {
            var householdMock = MockRepository.GenerateMock<IHousehold>();
            householdMock.Expect(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var householdProxy = new HouseholdProxy();

            var sqlQueryForId = householdProxy.GetSqlQueryForId(householdMock);
            Assert.That(sqlQueryForId, Is.Not.Null);
            Assert.That(sqlQueryForId, Is.Not.Empty);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(sqlQueryForId, Is.EqualTo(string.Format("SELECT HouseholdIdentifier,Name,Descr,CreationTime FROM Households WHERE HouseholdIdentifier='{0}'", householdMock.Identifier.Value.ToString("D").ToUpper())));
            // ReSharper restore PossibleInvalidOperationException
        }

        /// <summary>
        /// Tests that GetSqlCommandForInsert returns the SQL statement to insert a household.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatGetSqlCommandForInsertReturnsSqlCommandForInsert(bool hasDescription)
        {
            var fixture = new Fixture();
            
            var identifier = Guid.NewGuid();
            var name = fixture.Create<string>();
            var description = hasDescription ? fixture.Create<string>() : null;
            var creationTime = DateTime.Now;
            var householdProxy = new HouseholdProxy(name, description, creationTime)
            {
                Identifier = identifier
            };

            var descriptionAsSql = hasDescription ? string.Format("'{0}'", description) : "NULL";

            var sqlCommand = householdProxy.GetSqlCommandForInsert();
            Assert.That(sqlCommand, Is.Not.Null);
            Assert.That(sqlCommand, Is.Not.Empty);
            Assert.That(sqlCommand, Is.EqualTo(string.Format("INSERT INTO Households (HouseholdIdentifier,Name,Descr,CreationTime) VALUES('{0}','{1}',{2},{3})", householdProxy.UniqueId, name, descriptionAsSql, DataRepositoryHelper.GetSqlValueForDateTime(creationTime))));
        }

        /// <summary>
        /// Tests that GetSqlCommandForUpdate returns the SQL statement to update a household.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatGetSqlCommandForUpdateReturnsSqlCommandForUpdate(bool hasDescription)
        {
            var fixture = new Fixture();

            var identifier = Guid.NewGuid();
            var name = fixture.Create<string>();
            var description = hasDescription ? fixture.Create<string>() : null;
            var creationTime = DateTime.Now;
            var householdProxy = new HouseholdProxy(name, description, creationTime)
            {
                Identifier = identifier
            };

            var descriptionAsSql = hasDescription ? string.Format("'{0}'", description) : "NULL";

            var sqlCommand = householdProxy.GetSqlCommandForUpdate();
            Assert.That(sqlCommand, Is.Not.Null);
            Assert.That(sqlCommand, Is.Not.Empty);
            Assert.That(sqlCommand, Is.EqualTo(string.Format("UPDATE Households SET Name='{1}',Descr={2},CreationTime={3} WHERE HouseholdIdentifier='{0}'", householdProxy.UniqueId, name, descriptionAsSql, DataRepositoryHelper.GetSqlValueForDateTime(creationTime))));
        }

        /// <summary>
        /// Tests that GetSqlCommandForDelete returns the SQL statement to delete a household.
        /// </summary>
        [Test]
        public void TestThatGetSqlCommandForDeleteReturnsSqlCommandForDelete()
        {
            var householdProxy = new HouseholdProxy
            {
                Identifier = Guid.NewGuid()
            };

            var sqlCommand = householdProxy.GetSqlCommandForDelete();
            Assert.That(sqlCommand, Is.Not.Null);
            Assert.That(sqlCommand, Is.Not.Empty);
            Assert.That(sqlCommand, Is.EqualTo(string.Format("DELETE FROM Households WHERE HouseholdIdentifier='{0}'", householdProxy.UniqueId)));
        }

        /// <summary>
        /// Tests that MapData throws an ArgumentNullException if the data reader is null.
        /// </summary>
        [Test]
        public void TestThatMapDataThrowsArgumentNullExceptionIfDataReaderIsNull()
        {
            var householdProxy = new HouseholdProxy();
            Assert.That(householdProxy, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdProxy.MapData(null, MockRepository.GenerateMock<IDataProviderBase>()));
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
            var householdProxy = new HouseholdProxy();
            Assert.That(householdProxy, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdProxy.MapData(MockRepository.GenerateStub<MySqlDataReader>(), null));
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

            var householdProxy = new HouseholdProxy();
            Assert.That(householdProxy, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => householdProxy.MapData(dataReader, MockRepository.GenerateMock<IDataProviderBase>()));
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
        public void TestThatMapDataAndMapRelationsMapsDataIntoProxy(bool hasDescription)
        {
            var fixture = new Fixture();

            var dataProviderBaseMock = MockRepository.GenerateMock<IDataProviderBase>();

            var dataReader = MockRepository.GenerateStub<MySqlDataReader>();
            dataReader.Stub(m => m.GetString(Arg<string>.Is.Equal("HouseholdIdentifier")))
                .Return("F731712A-0E2F-4A46-B4B3-EFFBE63B8D2B")
                .Repeat.Any();
            dataReader.Stub(m => m.GetString(Arg<string>.Is.Equal("Name")))
                .Return(fixture.Create<string>())
                .Repeat.Any();
            dataReader.Stub(m => m.GetOrdinal(Arg<string>.Is.Equal("Descr")))
                .Return(2)
                .Repeat.Any();
            dataReader.Stub(m => m.IsDBNull(Arg<int>.Is.Equal(2)))
                .Return(!hasDescription)
                .Repeat.Any();
            dataReader.Stub(m => m.GetString(Arg<int>.Is.Equal(2)))
                .Return(hasDescription ? fixture.Create<string>() : null)
                .Repeat.Any();
            dataReader.Stub(m => m.GetDateTime(Arg<string>.Is.Equal("CreationTime")))
                .Return(DateTime.Now.ToUniversalTime())
                .Repeat.Any();

            var householdProxy = new HouseholdProxy();
            Assert.That(householdProxy, Is.Not.Null);
            Assert.That(householdProxy.Identifier, Is.Null);
            Assert.That(householdProxy.Identifier.HasValue, Is.False);
            Assert.That(householdProxy.Name, Is.Null);
            Assert.That(householdProxy.Description, Is.Null);
            Assert.That(householdProxy.CreationTime, Is.EqualTo(DateTime.MinValue));
            Assert.That(householdProxy.HouseholdMembers, Is.Not.Null);
            Assert.That(householdProxy.HouseholdMembers, Is.Empty);

            householdProxy.MapData(dataReader, dataProviderBaseMock);
            householdProxy.MapRelations(dataProviderBaseMock);
            Assert.That(householdProxy, Is.Not.Null);
            Assert.That(householdProxy.Identifier, Is.Not.Null);
            Assert.That(householdProxy.Identifier.HasValue, Is.True);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(householdProxy.Identifier.Value.ToString("D").ToUpper(), Is.EqualTo(dataReader.GetString("HouseholdIdentifier")));
            // ReSharper restore PossibleInvalidOperationException
            Assert.That(householdProxy.Name, Is.Not.Null);
            Assert.That(householdProxy.Name, Is.Not.Empty);
            Assert.That(householdProxy.Name, Is.EqualTo(dataReader.GetString("Name")));
            if (hasDescription)
            {
                Assert.That(householdProxy.Description, Is.Not.Null);
                Assert.That(householdProxy.Description, Is.Not.Empty);
                Assert.That(householdProxy.Description, Is.EqualTo(dataReader.GetString(2)));
            }
            else
            {
                Assert.That(householdProxy.Description, Is.Null);
            }
            Assert.That(householdProxy.CreationTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);
            Assert.That(householdProxy.HouseholdMembers, Is.Not.Null);
            Assert.That(householdProxy.HouseholdMembers, Is.Empty);
        }

        /// <summary>
        /// Tests that MapRelations throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatMapRelationsThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            var householdProxy = new HouseholdProxy();
            Assert.That(householdProxy, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdProxy.MapRelations(null));
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

            var householdProxy = new HouseholdProxy();
            Assert.That(householdProxy, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdProxy.SaveRelations(null, fixture.Create<bool>()));
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

            var householdProxy = new HouseholdProxy
            {
                Identifier = null
            };
            Assert.That(householdProxy, Is.Not.Null);
            Assert.That(householdProxy.Identifier, Is.Null);
            Assert.That(householdProxy.Identifier.HasValue, Is.False);

            var exception = Assert.Throws<IntranetRepositoryException>(() => householdProxy.SaveRelations(MockRepository.GenerateStub<IDataProviderBase>(), fixture.Create<bool>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, householdProxy.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that DeleteRelations throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            var householdProxy = new HouseholdProxy();
            Assert.That(householdProxy, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdProxy.DeleteRelations(null));
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
            var householdProxy = new HouseholdProxy
            {
                Identifier = null
            };
            Assert.That(householdProxy, Is.Not.Null);
            Assert.That(householdProxy.Identifier, Is.Null);
            Assert.That(householdProxy.Identifier.HasValue, Is.False);

            var exception = Assert.Throws<IntranetRepositoryException>(() => householdProxy.DeleteRelations(MockRepository.GenerateStub<IDataProviderBase>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, householdProxy.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }
    }
}
