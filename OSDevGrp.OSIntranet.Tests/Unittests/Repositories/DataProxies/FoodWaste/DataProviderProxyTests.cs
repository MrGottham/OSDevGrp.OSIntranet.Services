using System;
using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Resources;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.DataProxies.FoodWaste
{
    /// <summary>
    /// Tests the data proxy to a given data provider.
    /// </summary>
    [TestFixture]
    public class DataProviderProxyTests
    {
        /// <summary>
        /// Tests that the constructor initialize a data proxy to a given data provider.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeDataProviderProxy()
        {
            var dataProviderProxy = new DataProviderProxy();
            Assert.That(dataProviderProxy, Is.Not.Null);
            Assert.That(dataProviderProxy.Identifier, Is.Null);
            Assert.That(dataProviderProxy.Identifier.HasValue, Is.False);
            Assert.That(dataProviderProxy.Name, Is.Null);
            Assert.That(dataProviderProxy.DataSourceStatementIdentifier, Is.EqualTo(Guid.Empty));
            Assert.That(dataProviderProxy.DataSourceStatements, Is.Not.Null);
            Assert.That(dataProviderProxy.DataSourceStatements, Is.Empty);
        }

        /// <summary>
        /// Tests that getter for UniqueId throws an IntranetRepositoryException when the data provider has no identifier.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterThrowsIntranetRepositoryExceptionWhenDataProviderProxyHasNoIdentifier()
        {
            var dataProviderProxy = new DataProviderProxy
            {
                Identifier = null
            };

            // ReSharper disable UnusedVariable
            var exception = Assert.Throws<IntranetRepositoryException>(() => { var uniqueId = dataProviderProxy.UniqueId; });
            // ReSharper restore UnusedVariable
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, dataProviderProxy.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that getter for UniqueId gets the unique identifier for the data provider.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterGetsUniqueIdentificationForDataProviderProxy()
        {
            var dataProviderProxy = new DataProviderProxy
            {
                Identifier = Guid.NewGuid()
            };

            var uniqueId = dataProviderProxy.UniqueId;
            Assert.That(uniqueId, Is.Not.Null);
            Assert.That(uniqueId, Is.Not.Empty);
            Assert.That(uniqueId, Is.EqualTo(dataProviderProxy.Identifier.ToString().ToUpper()));
        }

        /// <summary>
        /// Tests that GetSqlQueryForId throws an ArgumentNullException when the given data provider is null.
        /// </summary>
        [Test]
        public void TestThatGetSqlQueryForIdThrowsArgumentNullExceptionWhenDataProviderIsNull()
        {
            var dataProviderProxy = new DataProviderProxy();

            // ReSharper disable UnusedVariable
            var exception = Assert.Throws<ArgumentNullException>(() => { var sqlQueryForId = dataProviderProxy.GetSqlQueryForId(null); });
            // ReSharper restore UnusedVariable
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("dataProvider"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetSqlQueryForId throws an IntranetRepositoryException when the identifier on the given data provider has no value.
        /// </summary>
        [Test]
        public void TestThatGetSqlQueryForIdThrowsIntranetRepositoryExceptionWhenIdentifierOnDataProviderHasNoValue()
        {
            var dataProviderMock = MockRepository.GenerateMock<IDataProvider>();
            dataProviderMock.Expect(m => m.Identifier)
                .Return(null)
                .Repeat.Any();

            var dataProviderProxy = new DataProviderProxy();

            // ReSharper disable UnusedVariable
            var exception = Assert.Throws<IntranetRepositoryException>(() => { var sqlQueryForId = dataProviderProxy.GetSqlQueryForId(dataProviderMock); });
            // ReSharper restore UnusedVariable
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, dataProviderMock.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetSqlQueryForId returns the SQL statement for selecting the given data provider.
        /// </summary>
        [Test]
        public void TestThatGetSqlQueryForIdReturnsSqlQueryForId()
        {
            var dataProviderMock = MockRepository.GenerateMock<IDataProvider>();
            dataProviderMock.Expect(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var dataProviderProxy = new DataProviderProxy();

            var sqlQueryForId = dataProviderProxy.GetSqlQueryForId(dataProviderMock);
            Assert.That(sqlQueryForId, Is.Not.Null);
            Assert.That(sqlQueryForId, Is.Not.Empty);
            Assert.That(sqlQueryForId, Is.EqualTo(string.Format("SELECT DataProviderIdentifier,Name,DataSourceStatementIdentifier FROM DataProviders WHERE DataProviderIdentifier='{0}'", dataProviderMock.Identifier.ToString().ToUpper())));
        }

        /// <summary>
        /// Tests that GetSqlCommandForInsert returns the SQL statement to insert this data provider.
        /// </summary>
        [Test]
        public void TestThatGetSqlCommandForInsertReturnsSqlCommandForInsert()
        {
            var fixture = new Fixture();

            var dataProviderProxy = new DataProviderProxy(fixture.Create<string>(), Guid.NewGuid())
            {
                Identifier = Guid.NewGuid()
            };

            var sqlCommand = dataProviderProxy.GetSqlCommandForInsert();
            Assert.That(sqlCommand, Is.Not.Null);
            Assert.That(sqlCommand, Is.Not.Empty);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(sqlCommand, Is.EqualTo(string.Format("INSERT INTO DataProviders (DataProviderIdentifier,Name,DataSourceStatementIdentifier) VALUES('{0}','{1}','{2}')", dataProviderProxy.UniqueId, dataProviderProxy.Name, dataProviderProxy.DataSourceStatementIdentifier.ToString().ToUpper())));
            // ReSharper restore PossibleInvalidOperationException
        }

        /// <summary>
        /// Tests that GetSqlCommandForUpdate returns the SQL statement to update data provider.
        /// </summary>
        [Test]
        public void TestThatGetSqlCommandForUpdateReturnsSqlCommandForUpdate()
        {
            var fixture = new Fixture();

            var dataProviderProxy = new DataProviderProxy(fixture.Create<string>(), Guid.NewGuid())
            {
                Identifier = Guid.NewGuid()
            };

            var sqlCommand = dataProviderProxy.GetSqlCommandForUpdate();
            Assert.That(sqlCommand, Is.Not.Null);
            Assert.That(sqlCommand, Is.Not.Empty);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(sqlCommand, Is.EqualTo(string.Format("UPDATE DataProviders SET Name='{1}',DataSourceStatementIdentifier='{2}' WHERE DataProviderIdentifier='{0}'", dataProviderProxy.UniqueId, dataProviderProxy.Name, dataProviderProxy.DataSourceStatementIdentifier.ToString().ToUpper())));
            // ReSharper restore PossibleInvalidOperationException
        }

        /// <summary>
        /// Tests that GetSqlCommandForDelete returns the SQL statement to delete data provider.
        /// </summary>
        [Test]
        public void TestThatGetSqlCommandForDeleteReturnsSqlCommandForDelete()
        {
            var fixture = new Fixture();

            var dataProviderProxy = new DataProviderProxy(fixture.Create<string>(), Guid.NewGuid())
            {
                Identifier = Guid.NewGuid()
            };

            var sqlCommand = dataProviderProxy.GetSqlCommandForDelete();
            Assert.That(sqlCommand, Is.Not.Null);
            Assert.That(sqlCommand, Is.Not.Empty);
            Assert.That(sqlCommand, Is.EqualTo(string.Format("DELETE FROM DataProviders WHERE DataProviderIdentifier='{0}'", dataProviderProxy.UniqueId)));
        }

        /// <summary>
        /// Tests that MapData throws an ArgumentNullException if the data reader is null.
        /// </summary>
        [Test]
        public void TestThatMapDataThrowsArgumentNullExceptionIfDataReaderIsNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IDataProviderBase>(e => e.FromFactory(() => MockRepository.GenerateMock<IDataProviderBase>()));

            var dataProviderProxy = new DataProviderProxy();
            Assert.That(dataProviderProxy, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => dataProviderProxy.MapData(null, fixture.Create<IDataProviderBase>()));
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

            var dataProviderProxy = new DataProviderProxy();
            Assert.That(dataProviderProxy, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => dataProviderProxy.MapData(fixture.Create<MySqlDataReader>(), null));
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

            var dataProviderProxy = new DataProviderProxy();
            Assert.That(dataProviderProxy, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => dataProviderProxy.MapData(dataReader, fixture.Create<IDataProviderBase>()));
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

            var translationProxyCollection = fixture.CreateMany<TranslationProxy>(2).ToList();
            var dataProviderBaseMock = MockRepository.GenerateMock<IDataProviderBase>();
            dataProviderBaseMock.Stub(m => m.Clone())
                .Return(dataProviderBaseMock)
                .Repeat.Any();
            dataProviderBaseMock.Stub(m => m.GetCollection<TranslationProxy>(Arg<string>.Is.NotNull))
                .Return(translationProxyCollection)
                .Repeat.Any();
                

            var dataReader = MockRepository.GenerateStub<MySqlDataReader>();
            dataReader.Stub(m => m.GetString(Arg<string>.Is.Equal("DataProviderIdentifier")))
                .Return("93660448-B33D-4732-8AC4-9A2FCB51C4BD")
                .Repeat.Any();
            dataReader.Stub(m => m.GetString(Arg<string>.Is.Equal("Name")))
                .Return(fixture.Create<string>())
                .Repeat.Any();
            dataReader.Stub(m => m.GetString(Arg<string>.Is.Equal("DataSourceStatementIdentifier")))
                .Return("1AFF5DC2-26B4-4E0B-ACFF-71E669B5CDCB")
                .Repeat.Any();

            var dataProviderProxy = new DataProviderProxy();
            Assert.That(dataProviderProxy, Is.Not.Null);
            Assert.That(dataProviderProxy.Identifier, Is.Null);
            Assert.That(dataProviderProxy.Identifier.HasValue, Is.False);
            Assert.That(dataProviderProxy.Name, Is.Null);
            Assert.That(dataProviderProxy.DataSourceStatementIdentifier, Is.EqualTo(Guid.Empty));
            Assert.That(dataProviderProxy.DataSourceStatements, Is.Not.Null);
            Assert.That(dataProviderProxy.DataSourceStatements, Is.Empty);

            dataProviderProxy.MapData(dataReader, dataProviderBaseMock);
            Assert.That(dataProviderProxy.Identifier, Is.Not.Null);
            Assert.That(dataProviderProxy.Identifier.HasValue, Is.True);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(dataProviderProxy.Identifier.Value.ToString("D").ToUpper(), Is.EqualTo(dataReader.GetString("DataProviderIdentifier")));
            // ReSharper restore PossibleInvalidOperationException
            Assert.That(dataProviderProxy.Name, Is.Not.Null);
            Assert.That(dataProviderProxy.Name, Is.Not.Empty);
            Assert.That(dataProviderProxy.Name, Is.EqualTo(dataReader.GetString("Name")));
            Assert.That(dataProviderProxy.DataSourceStatementIdentifier, Is.Not.EqualTo(Guid.Empty));
            Assert.That(dataProviderProxy.DataSourceStatementIdentifier.ToString("D").ToUpper(), Is.EqualTo(dataReader.GetString("DataSourceStatementIdentifier")));
            Assert.That(dataProviderProxy.DataSourceStatements, Is.Not.Null);
            Assert.That(dataProviderProxy.DataSourceStatements, Is.Not.Empty);
            Assert.That(dataProviderProxy.DataSourceStatements, Is.EqualTo(translationProxyCollection));
            
            dataProviderBaseMock.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Times(1));
            dataProviderBaseMock.AssertWasCalled(m => m.GetCollection<TranslationProxy>(Arg<string>.Is.Equal(string.Format("SELECT t.TranslationIdentifier AS TranslationIdentifier,t.OfIdentifier AS OfIdentifier,ti.TranslationInfoIdentifier AS InfoIdentifier,ti.CultureName AS CultureName,t.Value AS Value FROM Translations AS t, TranslationInfos AS ti WHERE t.OfIdentifier='{0}' AND ti.TranslationInfoIdentifier=t.InfoIdentifier ORDER BY CultureName", dataReader.GetString("DataSourceStatementIdentifier").ToUpper()))));
        }

        /// <summary>
        /// Tests that AddDataSourceStatement throws an ArgumentNullException when the data proxy for the data source statement is null.
        /// </summary>
        [Test]
        public void TestThatAddDataSourceStatementThrowsArgumentNullExceptionWhenDataSourceStatementProxyIsNull()
        {
            var dataProviderProxy = new DataProviderProxy();
            Assert.That(dataProviderProxy, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => dataProviderProxy.AddDataSourceStatement(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("dataSourceStatementProxy"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that AddDataSourceStatement adds the data proxy for the data source statement.
        /// </summary>
        [Test]
        public void TestThatAddDataSourceStatementAddsDataSourceStatementProxy()
        {
            var dataSourceStatementMock = MockRepository.GenerateMock<ITranslationProxy>();

            var dataProviderProxy = new DataProviderProxy();
            Assert.That(dataProviderProxy, Is.Not.Null);
            Assert.That(dataProviderProxy.DataSourceStatements, Is.Not.Null);
            Assert.That(dataProviderProxy.DataSourceStatements, Is.Empty);

            dataProviderProxy.AddDataSourceStatement(dataSourceStatementMock);
            Assert.That(dataProviderProxy.DataSourceStatements, Is.Not.Null);
            Assert.That(dataProviderProxy.DataSourceStatements, Is.Not.Empty);
            Assert.That(dataProviderProxy.DataSourceStatements.Contains(dataSourceStatementMock), Is.True);
        }
    }
}
