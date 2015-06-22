using System;
using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Resources;
using OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.DataProxies.FoodWaste
{
    /// <summary>
    /// Tests the data proxy to a given foreign key.
    /// </summary>
    [TestFixture]
    public class ForeignKeyProxyTests
    {
        /// <summary>
        /// Tests that the constructor initialize a data proxy to a given foreign key.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeForeignKeyProxy()
        {
            var foreignKeyProxy = new ForeignKeyProxy();
            Assert.That(foreignKeyProxy, Is.Not.Null);
            Assert.That(foreignKeyProxy.Identifier, Is.Null);
            Assert.That(foreignKeyProxy.Identifier.HasValue, Is.False);
            Assert.That(foreignKeyProxy.DataProvider, Is.Null);
            Assert.That(foreignKeyProxy.ForeignKeyForIdentifier, Is.EqualTo(Guid.Empty));
            Assert.That(foreignKeyProxy.ForeignKeyForTypes, Is.Null);
            Assert.That(foreignKeyProxy.ForeignKeyValue, Is.Null);
        }

        /// <summary>
        /// Tests that getter for UniqueId throws an IntranetRepositoryException when the foreign key has no identifier.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterThrowsIntranetRepositoryExceptionWhenForeignKeyProxyHasNoIdentifier()
        {
            var foreignKeyProxy = new ForeignKeyProxy
            {
                Identifier = null
            };

            // ReSharper disable UnusedVariable
            var exception = Assert.Throws<IntranetRepositoryException>(() => { var uniqueId = foreignKeyProxy.UniqueId; });
            // ReSharper restore UnusedVariable
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, foreignKeyProxy.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that getter for UniqueId gets the unique identifier for the foreign key.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterGetsUniqueIdentificationForForeignKeyProxy()
        {
            var foreignKeyProxy = new ForeignKeyProxy
            {
                Identifier = Guid.NewGuid()
            };

            var uniqueId = foreignKeyProxy.UniqueId;
            Assert.That(uniqueId, Is.Not.Null);
            Assert.That(uniqueId, Is.Not.Empty);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(uniqueId, Is.EqualTo(foreignKeyProxy.Identifier.Value.ToString("D").ToUpper()));
            // ReSharper restore PossibleInvalidOperationException
        }

        /// <summary>
        /// Tests that GetSqlQueryForId throws an ArgumentNullException when the given foreign key is null.
        /// </summary>
        [Test]
        public void TestThatGetSqlQueryForIdThrowsArgumentNullExceptionWhenForeignKeyIsNull()
        {
            var foreignKeyProxy = new ForeignKeyProxy();

            // ReSharper disable UnusedVariable
            var exception = Assert.Throws<ArgumentNullException>(() => { var sqlQueryForId = foreignKeyProxy.GetSqlQueryForId(null); });
            // ReSharper restore UnusedVariable
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("foreignKey"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetSqlQueryForId throws an IntranetRepositoryException when the identifier on the given foreign key has no value.
        /// </summary>
        [Test]
        public void TestThatGetSqlQueryForIdThrowsIntranetRepositoryExceptionWhenIdentifierOnForeignKeyHasNoValue()
        {
            var foreignKeyMock = MockRepository.GenerateMock<IForeignKey>();
            foreignKeyMock.Expect(m => m.Identifier)
                .Return(null)
                .Repeat.Any();

            var foreignKeyProxy = new ForeignKeyProxy();

            // ReSharper disable UnusedVariable
            var exception = Assert.Throws<IntranetRepositoryException>(() => { var sqlQueryForId = foreignKeyProxy.GetSqlQueryForId(foreignKeyMock); });
            // ReSharper restore UnusedVariable
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, foreignKeyMock.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetSqlQueryForId returns the SQL statement for selecting the given foreign key.
        /// </summary>
        [Test]
        public void TestThatGetSqlQueryForIdReturnsSqlQueryForId()
        {
            var foreignKeyMock = MockRepository.GenerateMock<IForeignKey>();
            foreignKeyMock.Expect(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foreignKeyProxy = new ForeignKeyProxy();

            var sqlQueryForId = foreignKeyProxy.GetSqlQueryForId(foreignKeyMock);
            Assert.That(sqlQueryForId, Is.Not.Null);
            Assert.That(sqlQueryForId, Is.Not.Empty);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(sqlQueryForId, Is.EqualTo(string.Format("SELECT ForeignKeyIdentifier,DataProviderIdentifier,ForeignKeyForIdentifier,ForeignKeyForTypes,ForeignKeyValue FROM ForeignKeys WHERE ForeignKeyIdentifier='{0}'", foreignKeyMock.Identifier.Value.ToString("D").ToUpper())));
            // ReSharper restore PossibleInvalidOperationException
        }

        /// <summary>
        /// Tests that GetSqlCommandForInsert returns the SQL statement to insert this foreign key.
        /// </summary>
        [Test]
        public void TestThatGetSqlCommandForInsertReturnsSqlCommandForInsert()
        {
            var fixture = new Fixture();

            var foreignKeyProxy = new ForeignKeyProxy(DomainObjectMockBuilder.BuildDataProviderMock(), Guid.NewGuid(), typeof(DataProvider), fixture.Create<string>())
            {
                Identifier = Guid.NewGuid()
            };

            var sqlCommand = foreignKeyProxy.GetSqlCommandForInsert();
            Assert.That(sqlCommand, Is.Not.Null);
            Assert.That(sqlCommand, Is.Not.Empty);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(sqlCommand, Is.EqualTo(string.Format("INSERT INTO ForeignKeys (ForeignKeyIdentifier,DataProviderIdentifier,ForeignKeyForIdentifier,ForeignKeyForTypes,ForeignKeyValue) VALUES('{0}','{1}','{2}','{3}','{4}')", foreignKeyProxy.UniqueId, foreignKeyProxy.DataProvider.Identifier.HasValue ? foreignKeyProxy.DataProvider.Identifier.Value.ToString("D").ToUpper() : Guid.Empty.ToString("D").ToUpper(), foreignKeyProxy.ForeignKeyForIdentifier, string.Join(";", foreignKeyProxy.ForeignKeyForTypes.Select(m => m.Name)), foreignKeyProxy.ForeignKeyValue)));
            // ReSharper restore PossibleInvalidOperationException
        }

        /// <summary>
        /// Tests that GetSqlCommandForUpdate returns the SQL statement to update this foreign key.
        /// </summary>
        [Test]
        public void TestThatGetSqlCommandForUpdateReturnsSqlCommandForUpdate()
        {
            var fixture = new Fixture();

            var foreignKeyProxy = new ForeignKeyProxy(DomainObjectMockBuilder.BuildDataProviderMock(), Guid.NewGuid(), typeof(DataProvider), fixture.Create<string>())
            {
                Identifier = Guid.NewGuid()
            };

            var sqlCommand = foreignKeyProxy.GetSqlCommandForUpdate();
            Assert.That(sqlCommand, Is.Not.Null);
            Assert.That(sqlCommand, Is.Not.Empty);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(sqlCommand, Is.EqualTo(string.Format("UPDATE ForeignKeys SET DataProviderIdentifier='{1}',ForeignKeyForIdentifier='{2}',ForeignKeyForTypes='{3}',ForeignKeyValue='{4}' WHERE ForeignKeyIdentifier='{0}'", foreignKeyProxy.UniqueId, foreignKeyProxy.DataProvider.Identifier.HasValue ? foreignKeyProxy.DataProvider.Identifier.Value.ToString("D").ToUpper() : Guid.Empty.ToString("D").ToUpper(), foreignKeyProxy.ForeignKeyForIdentifier, string.Join(";", foreignKeyProxy.ForeignKeyForTypes.Select(m => m.Name)), foreignKeyProxy.ForeignKeyValue)));
            // ReSharper restore PossibleInvalidOperationException
        }

        /// <summary>
        /// Tests that GetSqlCommandForDelete returns the SQL statement to delete this foreign key.
        /// </summary>
        [Test]
        public void TestThatGetSqlCommandForDeleteReturnsSqlCommandForDelete()
        {
            var fixture = new Fixture();

            var foreignKeyProxy = new ForeignKeyProxy(DomainObjectMockBuilder.BuildDataProviderMock(), Guid.NewGuid(), typeof(DataProvider), fixture.Create<string>())
            {
                Identifier = Guid.NewGuid()
            };

            var sqlCommand = foreignKeyProxy.GetSqlCommandForDelete();
            Assert.That(sqlCommand, Is.Not.Null);
            Assert.That(sqlCommand, Is.Not.Empty);
            Assert.That(sqlCommand, Is.EqualTo(string.Format("DELETE FROM ForeignKeys WHERE ForeignKeyIdentifier='{0}'", foreignKeyProxy.UniqueId)));
        }

        /// <summary>
        /// Tests that MapData throws an ArgumentNullException if the data reader is null.
        /// </summary>
        [Test]
        public void TestThatMapDataThrowsArgumentNullExceptionIfDataReaderIsNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IDataProviderBase>(e => e.FromFactory(() => MockRepository.GenerateMock<IDataProviderBase>()));

            var foreignKeyProxy = new ForeignKeyProxy();
            Assert.That(foreignKeyProxy, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foreignKeyProxy.MapData(null, fixture.Create<IDataProviderBase>()));
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
            var fixture = new Fixture();
            fixture.Customize<MySqlDataReader>(e => e.FromFactory(() => MockRepository.GenerateStub<MySqlDataReader>()));

            var foreignKeyProxy = new ForeignKeyProxy();
            Assert.That(foreignKeyProxy, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foreignKeyProxy.MapData(fixture.Create<MySqlDataReader>(), null));
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
            var fixture = new Fixture();
            fixture.Customize<IDataProviderBase>(e => e.FromFactory(() => MockRepository.GenerateMock<IDataProviderBase>()));

            var dataReader = MockRepository.GenerateMock<IDataReader>();

            var foreignKeyProxy = new ForeignKeyProxy();
            Assert.That(foreignKeyProxy, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => foreignKeyProxy.MapData(dataReader, fixture.Create<IDataProviderBase>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, "dataReader", dataReader.GetType().Name)));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that MapData maps data into the proxy.
        /// </summary>
        [Test]
        public void TestThatMapDataMapsDataIntoProxy()
        {
            var fixture = new Fixture();

            var dataProviderProxyMock = fixture.Create<DataProviderProxy>();
            var dataProviderBaseMock = MockRepository.GenerateMock<IDataProviderBase>();
            dataProviderBaseMock.Stub(m => m.Clone())
                .Return(dataProviderBaseMock)
                .Repeat.Any();
            dataProviderBaseMock.Stub(m => m.Get(Arg<DataProviderProxy>.Is.NotNull))
                .WhenCalled(e =>
                {
                    var proxy = (DataProviderProxy) e.Arguments.ElementAt(0);
                    Assert.That(proxy.Identifier, Is.Not.Null);
                    Assert.That(proxy.Identifier.HasValue, Is.True);
                    // ReSharper disable PossibleInvalidOperationException
                    Assert.That(proxy.Identifier.Value.ToString("D").ToUpper(), Is.EqualTo("797B684C-21D4-4BBD-87A1-40523132AC01"));
                    // ReSharper restore PossibleInvalidOperationException
                })
                .Return(dataProviderProxyMock)
                .Repeat.Any();

            var dataReader = MockRepository.GenerateStub<MySqlDataReader>();
            dataReader.Stub(m => m.GetString(Arg<string>.Is.Equal("ForeignKeyIdentifier")))
                .Return("C92A2126-B9E2-4293-AA1D-2F197FA12042")
                .Repeat.Any();
            dataReader.Stub(m => m.GetString(Arg<string>.Is.Equal("DataProviderIdentifier")))
                .Return("797B684C-21D4-4BBD-87A1-40523132AC01")
                .Repeat.Any();
            dataReader.Stub(m => m.GetString(Arg<string>.Is.Equal("ForeignKeyForIdentifier")))
                .Return("BA05ADE9-895F-4F59-BE2C-5D60BE48BA40")
                .Repeat.Any();
            dataReader.Stub(m => m.GetString(Arg<string>.Is.Equal("ForeignKeyForTypes")))
                .Return("IDomainObject;IIdentifiable;IDataProvider")
                .Repeat.Any();
            dataReader.Stub(m => m.GetString(Arg<string>.Is.Equal("ForeignKeyValue")))
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var foreignKeyProxy = new ForeignKeyProxy();
            Assert.That(foreignKeyProxy, Is.Not.Null);
            Assert.That(foreignKeyProxy.Identifier, Is.Null);
            Assert.That(foreignKeyProxy.Identifier.HasValue, Is.False);
            Assert.That(foreignKeyProxy.DataProvider, Is.Null);
            Assert.That(foreignKeyProxy.ForeignKeyForIdentifier, Is.EqualTo(Guid.Empty));
            Assert.That(foreignKeyProxy.ForeignKeyForTypes, Is.Null);
            Assert.That(foreignKeyProxy.ForeignKeyValue, Is.Null);

            foreignKeyProxy.MapData(dataReader, dataProviderBaseMock);
            Assert.That(foreignKeyProxy, Is.Not.Null);
            Assert.That(foreignKeyProxy.Identifier, Is.Not.Null);
            Assert.That(foreignKeyProxy.Identifier.HasValue, Is.True);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(foreignKeyProxy.Identifier.Value.ToString("D").ToUpper(), Is.EqualTo(dataReader.GetString("ForeignKeyIdentifier")));
            // ReSharper restore PossibleInvalidOperationException
            Assert.That(foreignKeyProxy.DataProvider, Is.Not.Null);
            Assert.That(foreignKeyProxy.DataProvider, Is.EqualTo(dataProviderProxyMock));
            Assert.That(foreignKeyProxy.ForeignKeyForIdentifier.ToString("D").ToUpper(), Is.EqualTo(dataReader.GetString("ForeignKeyForIdentifier")));
            Assert.That(foreignKeyProxy.ForeignKeyForTypes, Is.Not.Null);
            Assert.That(foreignKeyProxy.ForeignKeyForTypes, Is.Not.Empty);
            Assert.That(foreignKeyProxy.ForeignKeyForTypes.Count(), Is.EqualTo(3));
            Assert.That(foreignKeyProxy.ForeignKeyForTypes.Contains(typeof (IDomainObject)), Is.True);
            Assert.That(foreignKeyProxy.ForeignKeyForTypes.Contains(typeof (IIdentifiable)), Is.True);
            Assert.That(foreignKeyProxy.ForeignKeyForTypes.Contains(typeof (IDataProvider)), Is.True);
            Assert.That(foreignKeyProxy.ForeignKeyValue, Is.Not.Null);
            Assert.That(foreignKeyProxy.ForeignKeyValue, Is.Not.Empty);
            Assert.That(foreignKeyProxy.ForeignKeyValue, Is.EqualTo(dataReader.GetString("ForeignKeyValue")));

            dataProviderBaseMock.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Times(1));
            dataProviderBaseMock.AssertWasCalled(m => m.Get(Arg<DataProviderProxy>.Is.NotNull));
        }
    }
}
