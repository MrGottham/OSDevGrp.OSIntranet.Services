using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Resources;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.DataProxies.FoodWaste
{
    /// <summary>
    /// Tests the data proxy to a given data provider.
    /// </summary>
    [TestFixture]
    public class DataProviderProxyTests
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
        /// Tests that the constructor initialize a data proxy to a given data provider.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeDataProviderProxy()
        {
            IDataProviderProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);
            Assert.That(sut.Translation, Is.Null);
            Assert.That(sut.Translations, Is.Not.Null);
            Assert.That(sut.Translations, Is.Empty);
            Assert.That(sut.Name, Is.Null);
            Assert.That(sut.HandlesPayments, Is.False);
            Assert.That(sut.DataSourceStatementIdentifier, Is.EqualTo(Guid.Empty));
            Assert.That(sut.DataSourceStatement, Is.Null);
            Assert.That(sut.DataSourceStatements, Is.Not.Null);
            Assert.That(sut.DataSourceStatements, Is.Empty);
        }

        /// <summary>
        /// Tests that Translations maps translations and data source statements into the proxy when MapData has been called and MapRelations has not been called.
        /// </summary>
        [Test]
        public void TestThatTranslationsMapsTranslationsAndDataSourceStatementsIntoProxyWhenMapDataHasBeenCalledAndMapRelationsHasNotBeenCalled()
        {
            IDataProviderProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            Guid dataSourceStatementIdentifier = Guid.NewGuid();
            MySqlDataReader dataReader = CreateMySqlDataReader(Guid.NewGuid(), _fixture.Create<string>(), _fixture.Create<bool>(), dataSourceStatementIdentifier);

            IEnumerable<TranslationProxy> translationProxyCollection = BuildTranslationProxyCollection(dataSourceStatementIdentifier);
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(translationProxyCollection);

            sut.MapData(dataReader, dataProvider);

            Assert.That(sut.Translation, Is.Null);
            Assert.That(sut.Translations, Is.Not.Null);
            Assert.That(sut.Translations, Is.Not.Empty);
            Assert.That(sut.Translations, Is.EqualTo(translationProxyCollection));
            Assert.That(sut.DataSourceStatement, Is.Null);
            Assert.That(sut.DataSourceStatements, Is.Not.Null);
            Assert.That(sut.DataSourceStatements, Is.Not.Empty);
            Assert.That(sut.DataSourceStatements, Is.EqualTo(translationProxyCollection));

            dataProvider.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Times(1));

            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT t.TranslationIdentifier AS TranslationIdentifier,t.OfIdentifier AS OfIdentifier,ti.TranslationInfoIdentifier AS InfoIdentifier,ti.CultureName AS CultureName,t.Value AS Value FROM Translations AS t INNER JOIN TranslationInfos AS ti ON ti.TranslationInfoIdentifier=t.InfoIdentifier WHERE t.OfIdentifier=@ofIdentifier ORDER BY ti.CultureName")
                .AddCharDataParameter("@ofIdentifier", dataSourceStatementIdentifier)
                .Build();
            dataProvider.AssertWasCalled(m => m.GetCollection<TranslationProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that Translations maps translations and data source statements into the proxy when Create has been called and MapData has not been called.
        /// </summary>
        [Test]
        public void TestThatTranslationsMapsTranslationsAndDataSourceStatementsIntoProxyWhenCreateHasBeenCalledAndMapDataHasNotBeenCalled()
        {
            IDataProviderProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            Guid dataSourceStatementIdentifier = Guid.NewGuid();
            MySqlDataReader dataReader = CreateMySqlDataReader(Guid.NewGuid(), _fixture.Create<string>(), _fixture.Create<bool>(), dataSourceStatementIdentifier);

            IEnumerable<TranslationProxy> translationProxyCollection = BuildTranslationProxyCollection(dataSourceStatementIdentifier);
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(translationProxyCollection);

            IDataProviderProxy result = sut.Create(dataReader, dataProvider, "DataProviderIdentifier", "Name", "HandlesPayments", "DataSourceStatementIdentifier");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Translation, Is.Null);
            Assert.That(result.Translations, Is.Not.Null);
            Assert.That(result.Translations, Is.Not.Empty);
            Assert.That(result.Translations, Is.EqualTo(translationProxyCollection));
            Assert.That(result.DataSourceStatement, Is.Null);
            Assert.That(result.DataSourceStatements, Is.Not.Null);
            Assert.That(result.DataSourceStatements, Is.Not.Empty);
            Assert.That(result.DataSourceStatements, Is.EqualTo(translationProxyCollection));

            dataProvider.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Times(1));

            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT t.TranslationIdentifier AS TranslationIdentifier,t.OfIdentifier AS OfIdentifier,ti.TranslationInfoIdentifier AS InfoIdentifier,ti.CultureName AS CultureName,t.Value AS Value FROM Translations AS t INNER JOIN TranslationInfos AS ti ON ti.TranslationInfoIdentifier=t.InfoIdentifier WHERE t.OfIdentifier=@ofIdentifier ORDER BY ti.CultureName")
                .AddCharDataParameter("@ofIdentifier", dataSourceStatementIdentifier)
                .Build();
            dataProvider.AssertWasCalled(m => m.GetCollection<TranslationProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that getter for UniqueId throws an IntranetRepositoryException when the data provider has no identifier.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterThrowsIntranetRepositoryExceptionWhenDataProviderProxyHasNoIdentifier()
        {
            IDataProviderProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            sut.Identifier = null;

            // ReSharper disable UnusedVariable
            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => {var uniqueId = sut.UniqueId;});
            // ReSharper restore UnusedVariable

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, sut.Identifier, "Identifier");
        }

        /// <summary>
        /// Tests that getter for UniqueId gets the unique identifier for the data provider.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterGetsUniqueIdentificationForDataProviderProxy()
        {
            Guid identifier = Guid.NewGuid();

            IDataProviderProxy sut = CreateSut(identifier);
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
            IDataProviderProxy sut = CreateSut();
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
            IDataProviderProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.MapData(CreateMySqlDataReader(), null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that MapData maps data into the proxy.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatMapDataMapsDataIntoProxy(bool handlesPayments)
        {
            IDataProviderProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            Guid identifier = Guid.NewGuid();
            string name = _fixture.Create<string>();
            Guid dataSourceStatementIdentifier = Guid.NewGuid();
            MySqlDataReader dataReader = CreateMySqlDataReader(identifier, name, handlesPayments, dataSourceStatementIdentifier);

            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider();

            sut.MapData(dataReader, dataProvider);

            Assert.That(sut.Identifier, Is.Not.Null);
            Assert.That(sut.Identifier, Is.EqualTo(identifier));
            Assert.That(sut.Name, Is.Not.Null);
            Assert.That(sut.Name, Is.Not.Empty);
            Assert.That(sut.Name, Is.EqualTo(name));
            Assert.That(sut.HandlesPayments, Is.EqualTo(handlesPayments));
            Assert.That(sut.DataSourceStatementIdentifier, Is.Not.EqualTo(Guid.Empty));
            Assert.That(sut.DataSourceStatementIdentifier, Is.EqualTo(dataSourceStatementIdentifier));

            dataReader.AssertWasCalled(m => m.GetString(Arg<string>.Is.Equal("DataProviderIdentifier")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetString(Arg<string>.Is.Equal("Name")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetInt32(Arg<string>.Is.Equal("HandlesPayments")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetString(Arg<string>.Is.Equal("DataSourceStatementIdentifier")), opt => opt.Repeat.Once());

            dataProvider.AssertWasNotCalled(m => m.Clone());
        }

        /// <summary>
        /// Tests that MapRelations throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatMapRelationsThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            IDataProviderProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.MapRelations(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that MapRelations maps translations and data source statements into the proxy.
        /// </summary>
        [Test]
        public void TestThatMapRelationsMapTranslationsAndDataSourceStatementsIntoProxy()
        {
            Guid dataSourceStatementIdentifier = Guid.NewGuid();

            IDataProviderProxy sut = CreateSut(Guid.NewGuid(), _fixture.Create<string>(), _fixture.Create<bool>(), dataSourceStatementIdentifier);
            Assert.That(sut, Is.Not.Null);

            IEnumerable<TranslationProxy> translationProxyCollection = BuildTranslationProxyCollection(dataSourceStatementIdentifier);
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(translationProxyCollection);

            sut.MapRelations(dataProvider);

            Assert.That(sut.Translation, Is.Null);
            Assert.That(sut.Translations, Is.Not.Null);
            Assert.That(sut.Translations, Is.Not.Empty);
            Assert.That(sut.Translations, Is.EqualTo(translationProxyCollection));
            Assert.That(sut.DataSourceStatement, Is.Null);
            Assert.That(sut.DataSourceStatements, Is.Not.Null);
            Assert.That(sut.DataSourceStatements, Is.Not.Empty);
            Assert.That(sut.DataSourceStatements, Is.EqualTo(translationProxyCollection));

            dataProvider.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Times(1));

            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT t.TranslationIdentifier AS TranslationIdentifier,t.OfIdentifier AS OfIdentifier,ti.TranslationInfoIdentifier AS InfoIdentifier,ti.CultureName AS CultureName,t.Value AS Value FROM Translations AS t INNER JOIN TranslationInfos AS ti ON ti.TranslationInfoIdentifier=t.InfoIdentifier WHERE t.OfIdentifier=@ofIdentifier ORDER BY ti.CultureName")
                .AddCharDataParameter("@ofIdentifier", dataSourceStatementIdentifier)
                .Build();
            dataProvider.AssertWasCalled(m => m.GetCollection<TranslationProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that SaveRelations throws an NotSupportedException when the data provider is null.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsThrowsNotSupportedExceptionWhenDataProviderIsNull()
        {
            IDataProviderProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            NotSupportedException result = Assert.Throws<NotSupportedException>(() => sut.SaveRelations(null, _fixture.Create<bool>()));

            TestHelper.AssertNotSupportedExceptionIsValid(result);
        }

        /// <summary>
        /// Tests that SaveRelations throws an NotSupportedException when the data provider is not null.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsThrowsNotSupportedExceptionWhenDataProviderIsNotNull()
        {
            IDataProviderProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            NotSupportedException result = Assert.Throws<NotSupportedException>(() => sut.SaveRelations(CreateFoodWasteDataProvider(), _fixture.Create<bool>()));

            TestHelper.AssertNotSupportedExceptionIsValid(result);
        }

        /// <summary>
        /// Tests that DeleteRelations throws an NotSupportedException when the data provider is null.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsThrowsNotSupportedExceptionWhenDataProviderIsNull()
        {
            IDataProviderProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            NotSupportedException result = Assert.Throws<NotSupportedException>(() => sut.DeleteRelations(null));

            TestHelper.AssertNotSupportedExceptionIsValid(result);
        }

        /// <summary>
        /// Tests that SaveRelations throws an NotSupportedException when the data provider is not null.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsThrowsNotSupportedExceptionWhenDataProviderIsNotNull()
        {
            IDataProviderProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            NotSupportedException result = Assert.Throws<NotSupportedException>(() => sut.DeleteRelations(CreateFoodWasteDataProvider()));

            TestHelper.AssertNotSupportedExceptionIsValid(result);
        }

        /// <summary>
        /// Tests that CreateGetCommand returns the SQL command for selecting the given data provider.
        /// </summary>
        [Test]
        public void TestThatCreateGetCommandReturnsSqlCommand()
        {
            Guid identifier = Guid.NewGuid();

            IDataProviderProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("SELECT DataProviderIdentifier,Name,HandlesPayments,DataSourceStatementIdentifier FROM DataProviders WHERE DataProviderIdentifier=@dataProviderIdentifier")
                .AddCharDataParameter("@dataProviderIdentifier", identifier)
                .Build()
                .Run(sut.CreateGetCommand());
        }

        /// <summary>
        /// Tests that CreateInsertCommand returns the SQL command to insert a given data provider.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatCreateInsertCommandReturnsSqlCommandForInsert(bool handlesPayments)
        {
            Guid identifier = Guid.NewGuid();
            string name = _fixture.Create<string>();
            Guid dataSourceStatementIdentifier = Guid.NewGuid();

            IDataProviderProxy sut = CreateSut(identifier, name, handlesPayments, dataSourceStatementIdentifier);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("INSERT INTO DataProviders (DataProviderIdentifier,Name,HandlesPayments,DataSourceStatementIdentifier) VALUES(@dataProviderIdentifier,@name,@handlesPayments,@dataSourceStatementIdentifier)")
                .AddCharDataParameter("@dataProviderIdentifier", identifier)
                .AddVarCharDataParameter("@name", name, 256)
                .AddBitDataParameter("@handlesPayments", handlesPayments)
                .AddCharDataParameter("@dataSourceStatementIdentifier", dataSourceStatementIdentifier)
                .Build()
                .Run(sut.CreateInsertCommand());
        }

        /// <summary>
        /// Tests that CreateUpdateCommand returns the SQL command to update a given data provider.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatCreateUpdateCommandReturnsSqlCommandForUpdate(bool handlesPayments)
        {
            Guid identifier = Guid.NewGuid();
            string name = _fixture.Create<string>();
            Guid dataSourceStatementIdentifier = Guid.NewGuid();

            IDataProviderProxy sut = CreateSut(identifier, name, handlesPayments, dataSourceStatementIdentifier);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("UPDATE DataProviders SET Name=@name,HandlesPayments=@handlesPayments,DataSourceStatementIdentifier=@dataSourceStatementIdentifier WHERE DataProviderIdentifier=@dataProviderIdentifier")
                .AddCharDataParameter("@dataProviderIdentifier", identifier)
                .AddVarCharDataParameter("@name", name, 256)
                .AddBitDataParameter("@handlesPayments", handlesPayments)
                .AddCharDataParameter("@dataSourceStatementIdentifier", dataSourceStatementIdentifier)
                .Build()
                .Run(sut.CreateUpdateCommand());
        }

        /// <summary>
        /// Tests that CreateDeleteCommand returns the SQL command to delete a given data provider.
        /// </summary>
        [Test]
        public void TestThatCreateDeleteCommandReturnsSqlCommandForDelete()
        {
            Guid identifier = Guid.NewGuid();

            IDataProviderProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("DELETE FROM DataProviders WHERE DataProviderIdentifier=@dataProviderIdentifier")
                .AddCharDataParameter("@dataProviderIdentifier", identifier)
                .Build()
                .Run(sut.CreateDeleteCommand());
        }

        /// <summary>
        /// Tests that Create throws an ArgumentNullException if the data reader is null.
        /// </summary>
        [Test]
        public void TestThatCreateThrowsArgumentNullExceptionIfDataReaderIsNull()
        {
            IDataProviderProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Create(null, CreateFoodWasteDataProvider(), _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>()));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataReader");
        }

        /// <summary>
        /// Tests that Create throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatCreateThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            IDataProviderProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Create(CreateMySqlDataReader(), null, _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>()));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that Create throws an ArgumentNullException if the column name collection is null.
        /// </summary>
        [Test]
        public void TestThatCreateThrowsArgumentNullExceptionIfColumnNameCollectionIsNull()
        {
            IDataProviderProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Create(CreateMySqlDataReader(), CreateFoodWasteDataProvider(), null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "columnNameCollection");
        }

        /// <summary>
        /// Tests that Create creates a data proxy for a given data provider with values from the data reader.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatCreateCreatesProxy(bool handlesPayments)
        {
            IDataProviderProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            Guid identifier = Guid.NewGuid();
            string name = _fixture.Create<string>();
            Guid dataSourceStatementIdentifier = Guid.NewGuid();
            MySqlDataReader dataReader = CreateMySqlDataReader(identifier, name, handlesPayments, dataSourceStatementIdentifier);

            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider();

            IDataProviderProxy result = sut.Create(dataReader, dataProvider, "DataProviderIdentifier", "Name", "HandlesPayments", "DataSourceStatementIdentifier");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Identifier, Is.Not.Null);
            Assert.That(result.Identifier, Is.EqualTo(identifier));
            Assert.That(result.Name, Is.Not.Null);
            Assert.That(result.Name, Is.Not.Empty);
            Assert.That(result.Name, Is.EqualTo(name));
            Assert.That(result.HandlesPayments, Is.EqualTo(handlesPayments));
            Assert.That(result.DataSourceStatementIdentifier, Is.Not.EqualTo(Guid.Empty));
            Assert.That(result.DataSourceStatementIdentifier, Is.EqualTo(dataSourceStatementIdentifier));

            dataReader.AssertWasCalled(m => m.GetString(Arg<string>.Is.Equal("DataProviderIdentifier")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetString(Arg<string>.Is.Equal("Name")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetInt32(Arg<string>.Is.Equal("HandlesPayments")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetString(Arg<string>.Is.Equal("DataSourceStatementIdentifier")), opt => opt.Repeat.Once());

            dataProvider.AssertWasNotCalled(m => m.Clone());
        }

        /// <summary>
        /// Creates an instance of a data proxy to a given data provider.
        /// </summary>
        /// <returns>Instance of a data proxy to a given data provider.</returns>
        private IDataProviderProxy CreateSut()
        {
            return new DataProviderProxy();
        }

        /// <summary>
        /// Creates an instance of a data proxy to a given data provider.
        /// </summary>
        /// <returns>Instance of a data proxy to a given data provider.</returns>
        private IDataProviderProxy CreateSut(Guid identifier)
        {
            return new DataProviderProxy
            {
                Identifier = identifier
            };
        }

        /// <summary>
        /// Creates an instance of a data proxy to a given data provider.
        /// </summary>
        /// <returns>Instance of a data proxy to a given data provider.</returns>
        private IDataProviderProxy CreateSut(Guid identifier, string name, bool handlePayments, Guid dataSourceStatementIdentifier)
        {
            return new DataProviderProxy(name, handlePayments, dataSourceStatementIdentifier)
            {
                Identifier = identifier
            };
        }

        /// <summary>
        /// Creates a stub for the MySQL data reader.
        /// </summary>
        /// <returns>Stub for the MySQL data reader.</returns>
        private MySqlDataReader CreateMySqlDataReader(Guid? dataProviderIdentifier = null, string name = null, bool? handlesPayments = null, Guid? dataSourceStatementIdentifier = null)
        {
            MySqlDataReader mySqlDataReaderMock = MockRepository.GenerateStub<MySqlDataReader>();
            mySqlDataReaderMock.Stub(m => m.GetString(Arg<string>.Is.Equal("DataProviderIdentifier")))
                .Return(dataProviderIdentifier.HasValue ? dataProviderIdentifier.Value.ToString("D").ToUpper() : Guid.NewGuid().ToString("D").ToUpper())
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetString(Arg<string>.Is.Equal("Name")))
                .Return(name ?? _fixture.Create<string>())
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetInt32(Arg<string>.Is.Equal("HandlesPayments")))
                .Return(Convert.ToInt32(handlesPayments ??_fixture.Create<bool>()))
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetString(Arg<string>.Is.Equal("DataSourceStatementIdentifier")))
                .Return(dataSourceStatementIdentifier.HasValue ? dataSourceStatementIdentifier.Value.ToString("D").ToUpper() : Guid.NewGuid().ToString("D").ToUpper())
                .Repeat.Any();
            return mySqlDataReaderMock;
        }

        /// <summary>
        /// Creates a mockup for the data provider which can access data in the food waste repository.
        /// </summary>
        /// <returns>Mockup for the data provider which can access data in the food waste repository.</returns>
        private IFoodWasteDataProvider CreateFoodWasteDataProvider(IEnumerable<TranslationProxy> translationProxyCollection = null)
        {
            IFoodWasteDataProvider foodWasteDataProvider = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProvider.Stub(m => m.Clone())
                .Return(foodWasteDataProvider)
                .Repeat.Any();
            foodWasteDataProvider.Stub(m => m.GetCollection<TranslationProxy>(Arg<MySqlCommand>.Is.Anything))
                .Return(translationProxyCollection ?? BuildTranslationProxyCollection(Guid.NewGuid()))
                .Repeat.Any();
            return foodWasteDataProvider;
        }

        /// <summary>
        /// Build a collection of translation data proxies.
        /// </summary>
        /// <returns>Collection of translation data proxies.</returns>
        private IEnumerable<TranslationProxy> BuildTranslationProxyCollection(Guid dataSourceStatementIdentifier)
        {
            _fixture.Customize<TranslationProxy>(composerTransformation => composerTransformation.FromFactory(() => new TranslationProxy(dataSourceStatementIdentifier, _fixture.Create<TranslationInfoProxy>(), _fixture.Create<string>())));

            return _fixture.CreateMany<TranslationProxy>(_random.Next(5, 10)).ToList();
        }
    }
}
