using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
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
    /// Tests the data proxy to a given storage type.
    /// </summary>
    [TestFixture]
    public class StorageTypeProxyTests
    {
        #region Private variables

        private Fixture _fixture;

        #endregion

        /// <summary>
        /// Setup each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        /// <summary>
        /// Tests that the constructor initialize a data proxy to a given storage type.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeStorageTypeProxy()
        {
            IStorageTypeProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);
            Assert.That(sut.SortOrder, Is.EqualTo(default(int)));
            Assert.That(sut.Temperature, Is.EqualTo(default(int)));
            Assert.That(sut.TemperatureRange, Is.Null);
            Assert.That(sut.Creatable, Is.EqualTo(default(bool)));
            Assert.That(sut.Editable, Is.EqualTo(default(bool)));
            Assert.That(sut.Deletable, Is.EqualTo(default(bool)));
            Assert.That(sut.Translation, Is.Null);
            Assert.That(sut.Translations, Is.Not.Null);
            Assert.That(sut.Translations, Is.Empty);
        }

        /// <summary>
        /// Tests that Translations maps the translations for the food item into the proxy when MapData has been called and MapRelations has not been called.
        /// </summary>
        [Test]
        public void TestThatTranslationsMapsTranslationsIntoProxyWhenMapDataHasBeenCalledAndMapRelationsHasNotBeenCalled()
        {
            IStorageTypeProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            Guid storageTypeIdentifier = Guid.NewGuid();
            MySqlDataReader dataReader = CreateMySqlDataReader(storageTypeIdentifier);

            IEnumerable<TranslationProxy> translationProxyCollection = _fixture.CreateMany<TranslationProxy>().ToList();
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(translationProxyCollection);

            sut.MapData(dataReader, dataProvider);

            Assert.That(sut.Translation, Is.Null);
            Assert.That(sut.Translations, Is.Not.Null);
            Assert.That(sut.Translations, Is.Not.Empty);
            Assert.That(sut.Translations, Is.EqualTo(translationProxyCollection));

            dataProvider.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Once());

            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT t.TranslationIdentifier AS TranslationIdentifier,t.OfIdentifier AS OfIdentifier,ti.TranslationInfoIdentifier AS InfoIdentifier,ti.CultureName AS CultureName,t.Value AS Value FROM Translations AS t INNER JOIN TranslationInfos AS ti ON ti.TranslationInfoIdentifier=t.InfoIdentifier WHERE t.OfIdentifier=@ofIdentifier ORDER BY ti.CultureName")
                .AddCharDataParameter("@ofIdentifier", storageTypeIdentifier)
                .Build();
            dataProvider.AssertWasCalled(m => m.GetCollection<TranslationProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that Translations maps the translations for the food item into the proxy when Create has been called and MapData has not been called.
        /// </summary>
        [Test]
        public void TestThatTranslationsMapsTranslationsIntoProxyWhenCreateHasBeenCalledAndMapDataHasNotBeenCalled()
        {
            IStorageTypeProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            Guid storageTypeIdentifier = Guid.NewGuid();
            MySqlDataReader dataReader = CreateMySqlDataReader(storageTypeIdentifier);

            IEnumerable<TranslationProxy> translationProxyCollection = _fixture.CreateMany<TranslationProxy>().ToList();
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(translationProxyCollection);

            IStorageTypeProxy result = sut.Create(dataReader, dataProvider, "StorageTypeIdentifier", "SortOrder", "Temperature", "TemperatureRangeStartValue", "TemperatureRangeEndValue", "Creatable", "Editable", "Deletable");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Translation, Is.Null);
            Assert.That(result.Translations, Is.Not.Null);
            Assert.That(result.Translations, Is.Not.Empty);
            Assert.That(result.Translations, Is.EqualTo(translationProxyCollection));

            dataProvider.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Once());

            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT t.TranslationIdentifier AS TranslationIdentifier,t.OfIdentifier AS OfIdentifier,ti.TranslationInfoIdentifier AS InfoIdentifier,ti.CultureName AS CultureName,t.Value AS Value FROM Translations AS t INNER JOIN TranslationInfos AS ti ON ti.TranslationInfoIdentifier=t.InfoIdentifier WHERE t.OfIdentifier=@ofIdentifier ORDER BY ti.CultureName")
                .AddCharDataParameter("@ofIdentifier", storageTypeIdentifier)
                .Build();
            dataProvider.AssertWasCalled(m => m.GetCollection<TranslationProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that getter for UniqueId gets the unique identifier for the storage type.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterGetsUniqueIdentificationForStorageTypeProxy()
        {
            Guid identifier = Guid.NewGuid();

            IStorageTypeProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Not.Null);
            Assert.That(sut.Identifier.Value, Is.EqualTo(identifier));

            string uniqueId = sut.UniqueId;
            Assert.That(uniqueId, Is.Not.Null);
            Assert.That(uniqueId, Is.Not.Empty);
            Assert.That(uniqueId, Is.EqualTo(identifier.ToString("D").ToUpper()));
        }

        /// <summary>
        /// Tests that getter of UniqueId throws an IntranetRepositoryException when the storage type has no identifier.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterThrowsIntranetRepositoryExceptionWhenStorageTypeHasNoIdentifier()
        {
            IStorageTypeProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.UniqueId.ToUpper());
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, sut.Identifier, "Identifier");
        }

        /// <summary>
        /// Tests that MapData throws an ArgumentNullException if the data reader is null.
        /// </summary>
        [Test]
        public void TestThatMapDataThrowsArgumentNullExceptionIfDataReaderIsNull()
        {
            IStorageTypeProxy sut = CreateSut();
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
            IStorageTypeProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.MapData(CreateMySqlDataReader(), null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that MapData maps data into the proxy.
        /// </summary>
        [Test]
        [TestCase(true, true, true)]
        [TestCase(true, true, false)]
        [TestCase(true, false, true)]
        [TestCase(true, false, false)]
        [TestCase(false, true, true)]
        [TestCase(false, true, false)]
        [TestCase(false, false, true)]
        [TestCase(false, false, false)]
        public void TestThatMapDataMapsDataIntoProxy(bool creatable, bool editable, bool deletable)
        {
            IStorageTypeProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            Guid storageTypeIdentifier = Guid.NewGuid();
            int sortOrder = _fixture.Create<int>();
            int temperature = _fixture.Create<int>();
            IRange<int> temperatureRange = DomainObjectMockBuilder.BuildIntRange();
            MySqlDataReader dataReader = CreateMySqlDataReader(storageTypeIdentifier, sortOrder, temperature, temperatureRange, creatable, editable, deletable);

            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider();

            sut.MapData(dataReader, dataProvider);

            Assert.That(sut.Identifier, Is.Not.Null);
            Assert.That(sut.Identifier, Is.EqualTo(storageTypeIdentifier));
            Assert.That(sut.SortOrder, Is.EqualTo(sortOrder));
            Assert.That(sut.Temperature, Is.EqualTo(temperature));
            Assert.That(sut.TemperatureRange, Is.Not.Null);
            Assert.That(sut.TemperatureRange.StartValue, Is.EqualTo(temperatureRange.StartValue));
            Assert.That(sut.TemperatureRange.EndValue, Is.EqualTo(temperatureRange.EndValue));
            Assert.That(sut.Creatable, Is.EqualTo(creatable));
            Assert.That(sut.Editable, Is.EqualTo(editable));
            Assert.That(sut.Deletable, Is.EqualTo(deletable));

            dataReader.AssertWasCalled(m => m.GetString(Arg<string>.Is.Equal("StorageTypeIdentifier")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetInt16(Arg<string>.Is.Equal("SortOrder")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetInt16(Arg<string>.Is.Equal("Temperature")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetInt16(Arg<string>.Is.Equal("TemperatureRangeStartValue")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetInt16(Arg<string>.Is.Equal("TemperatureRangeEndValue")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetInt32(Arg<string>.Is.Equal("Creatable")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetInt32(Arg<string>.Is.Equal("Editable")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetInt32(Arg<string>.Is.Equal("Deletable")), opt => opt.Repeat.Once());

            dataProvider.AssertWasNotCalled(m => m.Clone());
        }

        /// <summary>
        /// Tests that MapRelations throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatMapRelationsThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            IStorageTypeProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.MapRelations(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that MapRelations throws an IntranetRepositoryException when the identifier for the food group is null.
        /// </summary>
        [Test]
        public void TestThatMapRelationsThrowsIntranetRepositoryExceptionWhenIdentifierIsNull()
        {
            IStorageTypeProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.MapRelations(CreateFoodWasteDataProvider()));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, sut.Identifier, "Identifier");
        }

        /// <summary>
        /// Tests that MapRelations calls Clone on the data provider one time.
        /// </summary>
        [Test]
        public void TestThatMapRelationsCallsCloneOnDataProviderOneTime()
        {
            IStorageTypeProxy sut = CreateSut(Guid.NewGuid());
            Assert.That(sut, Is.Not.Null);

            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider();

            sut.MapRelations(dataProvider);

            dataProvider.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that MapRelations calls GetCollection on the data provider to get all translations for the storage type.
        /// </summary>
        [Test]
        public void TestThatMapRelationsCallsGetCollectionOnDataProviderToGetTranslations()
        {
            Guid storageTypeIdentifier = Guid.NewGuid();

            IStorageTypeProxy sut = CreateSut(storageTypeIdentifier);
            Assert.That(sut, Is.Not.Null);

            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider();

            sut.MapRelations(dataProvider);

            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT t.TranslationIdentifier AS TranslationIdentifier,t.OfIdentifier AS OfIdentifier,ti.TranslationInfoIdentifier AS InfoIdentifier,ti.CultureName AS CultureName,t.Value AS Value FROM Translations AS t INNER JOIN TranslationInfos AS ti ON ti.TranslationInfoIdentifier=t.InfoIdentifier WHERE t.OfIdentifier=@ofIdentifier ORDER BY ti.CultureName")
                .AddCharDataParameter("@ofIdentifier", storageTypeIdentifier)
                .Build();
            dataProvider.AssertWasCalled(m => m.GetCollection<TranslationProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that MapRelations maps translations for the storage type.
        /// </summary>
        [Test]
        public void TestThatMapRelationsMapsTranslationsIntoProxy()
        {
            IStorageTypeProxy sut = CreateSut(Guid.NewGuid());
            Assert.That(sut, Is.Not.Null);

            IEnumerable<TranslationProxy> translationProxyCollection = _fixture.CreateMany<TranslationProxy>().ToList();
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(translationProxyCollection);

            sut.MapRelations(dataProvider);

            Assert.That(sut.Translation, Is.Null);
            Assert.That(sut.Translations, Is.Not.Null);
            Assert.That(sut.Translations, Is.Not.Empty);
            Assert.That(sut.Translations, Is.EqualTo(translationProxyCollection));
        }

        /// <summary>
        /// Tests that SaveRelations throws an NotSupportedException when the data provider is null.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsThrowsNotSupportedExceptionWhenDataProviderIsNull()
        {
            IStorageTypeProxy sut = CreateSut();
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
           IStorageTypeProxy sut = CreateSut();
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
            IStorageTypeProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            NotSupportedException result = Assert.Throws<NotSupportedException>(() => sut.DeleteRelations(null));

            TestHelper.AssertNotSupportedExceptionIsValid(result);
        }

        /// <summary>
        /// Tests that DeleteRelations throws an NotSupportedException when the data provider is not null.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsThrowsNotSupportedExceptionWhenDataProviderIsNotNull()
        {
            IStorageTypeProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            NotSupportedException result = Assert.Throws<NotSupportedException>(() => sut.DeleteRelations(CreateFoodWasteDataProvider()));

            TestHelper.AssertNotSupportedExceptionIsValid(result);
        }

        /// <summary>
        /// Tests that CreateGetCommand returns the SQL command for selecting the given storage type.
        /// </summary>
        [Test]
        public void TestThatCreateGetCommandReturnsSqlCommand()
        {
            Guid identifier = Guid.NewGuid();

            IStorageTypeProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("SELECT StorageTypeIdentifier,SortOrder,Temperature,TemperatureRangeStartValue,TemperatureRangeEndValue,Creatable,Editable,Deletable FROM StorageTypes WHERE StorageTypeIdentifier=@storageTypeIdentifier")
                .AddCharDataParameter("@storageTypeIdentifier", identifier)
                .Build()
                .Run(sut.CreateGetCommand());
        }

        /// <summary>
        /// Tests that CreateInsertCommand returns the SQL command to insert this storage type.
        /// </summary>
        [Test]
        [TestCase(true, true, true)]
        [TestCase(true, true, false)]
        [TestCase(true, false, true)]
        [TestCase(true, false, false)]
        [TestCase(false, true, true)]
        [TestCase(false, true, false)]
        [TestCase(false, false, true)]
        [TestCase(false, false, false)]
        public void TestThatCreateInsertCommandReturnsSqlCommandForInsert(bool creatable, bool editable, bool deletable)
        {
            Guid identifier = Guid.NewGuid();
            int sortOrder = _fixture.Create<int>();
            int temperature = _fixture.Create<int>();
            IRange<int> temperatureRange = DomainObjectMockBuilder.BuildIntRange();

            IStorageTypeProxy sut = CreateSut(identifier, sortOrder, temperature, temperatureRange, creatable, editable, deletable);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("INSERT INTO StorageTypes (StorageTypeIdentifier,SortOrder,Temperature,TemperatureRangeStartValue,TemperatureRangeEndValue,Creatable,Editable,Deletable) VALUES(@storageTypeIdentifier,@sortOrder,@temperature,@temperatureRangeStartValue,@temperatureRangeEndValue,@creatable,@editable,@deletable)")
                .AddCharDataParameter("@storageTypeIdentifier", identifier)
                .AddTinyIntDataParameter("@sortOrder", sortOrder, 4)
                .AddTinyIntDataParameter("@temperature", temperature, 4)
                .AddTinyIntDataParameter("@temperatureRangeStartValue", temperatureRange.StartValue, 4)
                .AddTinyIntDataParameter("@temperatureRangeEndValue", temperatureRange.EndValue, 4)
                .AddBitDataParameter("@creatable", creatable)
                .AddBitDataParameter("@editable", editable)
                .AddBitDataParameter("@deletable", deletable)
                .Build()
                .Run(sut.CreateInsertCommand());
        }

        /// <summary>
        /// Tests that CreateUpdateCommand returns the SQL command to update this storage type.
        /// </summary>
        [Test]
        [TestCase(true, true, true)]
        [TestCase(true, true, false)]
        [TestCase(true, false, true)]
        [TestCase(true, false, false)]
        [TestCase(false, true, true)]
        [TestCase(false, true, false)]
        [TestCase(false, false, true)]
        [TestCase(false, false, false)]
        public void TestThatCreateUpdateCommandReturnsSqlCommandForUpdate(bool creatable, bool editable, bool deletable)
        {
            Guid identifier = Guid.NewGuid();
            int sortOrder = _fixture.Create<int>();
            int temperature = _fixture.Create<int>();
            IRange<int> temperatureRange = DomainObjectMockBuilder.BuildIntRange();

            IStorageTypeProxy sut = CreateSut(identifier, sortOrder, temperature, temperatureRange, creatable, editable, deletable);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("UPDATE StorageTypes SET SortOrder=@sortOrder,Temperature=@temperature,TemperatureRangeStartValue=@temperatureRangeStartValue,TemperatureRangeEndValue=@temperatureRangeEndValue,Creatable=@creatable,Editable=@editable,Deletable=@deletable WHERE StorageTypeIdentifier=@storageTypeIdentifier")
                .AddCharDataParameter("@storageTypeIdentifier", identifier)
                .AddTinyIntDataParameter("@sortOrder", sortOrder, 4)
                .AddTinyIntDataParameter("@temperature", temperature, 4)
                .AddTinyIntDataParameter("@temperatureRangeStartValue", temperatureRange.StartValue, 4)
                .AddTinyIntDataParameter("@temperatureRangeEndValue", temperatureRange.EndValue, 4)
                .AddBitDataParameter("@creatable", creatable)
                .AddBitDataParameter("@editable", editable)
                .AddBitDataParameter("@deletable", deletable)
                .Build()
                .Run(sut.CreateUpdateCommand());
        }

        /// <summary>
        /// Tests that CreateDeleteCommand returns the SQL command to delete this storage type.
        /// </summary>
        [Test]
        public void TestThatCreateDeleteCommandReturnsSqlCommandForDelete()
        {
            Guid identifier = Guid.NewGuid();

            IStorageTypeProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("DELETE FROM StorageTypes WHERE StorageTypeIdentifier=@storageTypeIdentifier")
                .AddCharDataParameter("@storageTypeIdentifier", identifier)
                .Build()
                .Run(sut.CreateDeleteCommand());
        }

        /// <summary>
        /// Tests that Create throws an ArgumentNullException if the data reader is null.
        /// </summary>
        [Test]
        public void TestThatCreateThrowsArgumentNullExceptionIfDataReaderIsNull()
        {
            IStorageTypeProxy sut = CreateSut();
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
            IStorageTypeProxy sut = CreateSut();
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
            IStorageTypeProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Create(CreateMySqlDataReader(), CreateFoodWasteDataProvider(), null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "columnNameCollection");
        }

        /// <summary>
        /// Tests that Create creates a data proxy for a given data provider with values from the data reader.
        /// </summary>
        [Test]
        [TestCase(true, true, true)]
        [TestCase(true, true, false)]
        [TestCase(true, false, true)]
        [TestCase(true, false, false)]
        [TestCase(false, true, true)]
        [TestCase(false, true, false)]
        [TestCase(false, false, true)]
        [TestCase(false, false, false)]
        public void TestThatCreateCreatesProxy(bool creatable, bool editable, bool deletable)
        {
            IStorageTypeProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            Guid storageTypeIdentifier = Guid.NewGuid();
            int sortOrder = _fixture.Create<int>();
            int temperature = _fixture.Create<int>();
            IRange<int> temperatureRange = DomainObjectMockBuilder.BuildIntRange();
            MySqlDataReader dataReader = CreateMySqlDataReader(storageTypeIdentifier, sortOrder, temperature, temperatureRange, creatable, editable, deletable);

            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider();

            IStorageTypeProxy result = sut.Create(dataReader, dataProvider, "StorageTypeIdentifier", "SortOrder", "Temperature", "TemperatureRangeStartValue", "TemperatureRangeEndValue", "Creatable", "Editable", "Deletable");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Identifier, Is.Not.Null);
            Assert.That(result.Identifier, Is.EqualTo(storageTypeIdentifier));
            Assert.That(result.SortOrder, Is.EqualTo(sortOrder));
            Assert.That(result.Temperature, Is.EqualTo(temperature));
            Assert.That(result.TemperatureRange, Is.Not.Null);
            Assert.That(result.TemperatureRange.StartValue, Is.EqualTo(temperatureRange.StartValue));
            Assert.That(result.TemperatureRange.EndValue, Is.EqualTo(temperatureRange.EndValue));
            Assert.That(result.Creatable, Is.EqualTo(creatable));
            Assert.That(result.Editable, Is.EqualTo(editable));
            Assert.That(result.Deletable, Is.EqualTo(deletable));

            dataReader.AssertWasCalled(m => m.GetString(Arg<string>.Is.Equal("StorageTypeIdentifier")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetInt16(Arg<string>.Is.Equal("SortOrder")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetInt16(Arg<string>.Is.Equal("Temperature")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetInt16(Arg<string>.Is.Equal("TemperatureRangeStartValue")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetInt16(Arg<string>.Is.Equal("TemperatureRangeEndValue")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetInt32(Arg<string>.Is.Equal("Creatable")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetInt32(Arg<string>.Is.Equal("Editable")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetInt32(Arg<string>.Is.Equal("Deletable")), opt => opt.Repeat.Once());

            dataProvider.AssertWasNotCalled(m => m.Clone());
        }

        /// <summary>
        /// Creates an instance of the data proxy to a given storage type which should be used for unit testing.
        /// </summary>
        /// <returns>Instance of the data proxy to a given storage type which should be used for unit testing.</returns>
        private IStorageTypeProxy CreateSut(Guid? storageTypeIdentifier = null)
        {
            return new StorageTypeProxy
            {
                Identifier = storageTypeIdentifier
            };
        }

        /// <summary>
        /// Creates an instance of the data proxy to a given storage type which should be used for unit testing.
        /// </summary>
        /// <returns>Instance of the data proxy to a given storage type which should be used for unit testing.</returns>
        private IStorageTypeProxy CreateSut(Guid storageTypeIdentifier, int sortOrder, int temperature, IRange<int> temperatureRange, bool creatable, bool editable, bool deletable)
        {
            return new StorageTypeProxy(sortOrder, temperature, temperatureRange, creatable, editable, deletable)
            {
                Identifier = storageTypeIdentifier
            };
        }

        /// <summary>
        /// Creates an instance of a MySQL data reader which should be used for unit testing.
        /// </summary>
        /// <returns>Instance of a MySQL data reader which should be used for unit testing.</returns>
        private MySqlDataReader CreateMySqlDataReader(Guid? storageTypeIdentifier = null, int? sortOrder = null, int? temperature = null, IRange<int> temperatureRange = null, bool? creatable = null, bool? editable = null, bool? deletable = null)
        {
            IRange<int> range = temperatureRange ?? DomainObjectMockBuilder.BuildIntRange();

            MySqlDataReader mySqlDataReaderStub = MockRepository.GenerateStub<MySqlDataReader>();
            mySqlDataReaderStub.Stub(m => m.GetString(Arg<string>.Is.Equal("StorageTypeIdentifier")))
                .Return(storageTypeIdentifier.HasValue ? storageTypeIdentifier.Value.ToString("D").ToUpper() : Guid.NewGuid().ToString("D").ToUpper())
                .Repeat.Any();
            mySqlDataReaderStub.Stub(m => m.GetInt16(Arg<string>.Is.Equal("SortOrder")))
                .Return((short) (sortOrder ?? _fixture.Create<short>()))
                .Repeat.Any();
            mySqlDataReaderStub.Stub(m => m.GetInt16(Arg<string>.Is.Equal("Temperature")))
                .Return((short) (temperature ?? _fixture.Create<short>()))
                .Repeat.Any();
            mySqlDataReaderStub.Stub(m => m.GetInt16("TemperatureRangeStartValue"))
                .Return((short) range.StartValue)
                .Repeat.Any();
            mySqlDataReaderStub.Stub(m => m.GetInt16("TemperatureRangeEndValue"))
                .Return((short) range.EndValue)
                .Repeat.Any();
            mySqlDataReaderStub.Stub(m => m.GetInt32(Arg<string>.Is.Equal("Creatable")))
                .Return(Convert.ToInt32(creatable ?? _fixture.Create<bool>()))
                .Repeat.Any();
            mySqlDataReaderStub.Stub(m => m.GetInt32(Arg<string>.Is.Equal("Editable")))
                .Return(Convert.ToInt32(editable ?? _fixture.Create<bool>()))
                .Repeat.Any();
            mySqlDataReaderStub.Stub(m => m.GetInt32(Arg<string>.Is.Equal("Deletable")))
                .Return(Convert.ToInt32(deletable ?? _fixture.Create<bool>()))
                .Repeat.Any();
            return mySqlDataReaderStub;
        }

        /// <summary>
        /// Creates an instance of a data provider which should be used for unit testing.
        /// </summary>
        /// <returns>Instance of a data provider which should be used for unit testing</returns>
        private IFoodWasteDataProvider CreateFoodWasteDataProvider(IEnumerable<TranslationProxy> translationProxyCollection = null)
        {
            IFoodWasteDataProvider dataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            dataProviderMock.Stub(m => m.Clone())
                .Return(dataProviderMock)
                .Repeat.Any();
            dataProviderMock.Stub(m => m.GetCollection<TranslationProxy>(Arg<MySqlCommand>.Is.Anything))
                .Return(translationProxyCollection ?? _fixture.CreateMany<TranslationProxy>().ToList())
                .Repeat.Any();
            return dataProviderMock;
        }
    }
}
