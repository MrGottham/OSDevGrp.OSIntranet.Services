using System;
using System.Collections.Generic;
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
using OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste;
using AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.DataProxies.FoodWaste
{
    /// <summary>
    /// Tests the data proxy to a given storage type.
    /// </summary>
    [TestFixture]
    public class StorageTypeProxyTests
    {
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
        /// Tests that getter for UniqueId gets the unique identifier for the storage type.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterGetsUniqueIdentificationForStorageTypeProxy()
        {
            Guid identifier = Guid.NewGuid();

            IStorageTypeProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.Identifier.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
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
        /// Tests that GetSqlQueryForId throws an ArgumentNullException when the given storage type is null.
        /// </summary>
        [Test]
        public void TestThatGetSqlQueryForIdThrowsArgumentNullExceptionWhenStorageTypeIsNull()
        {
            IStorageTypeProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => ((StorageTypeProxy) sut).GetSqlQueryForId(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "storageType");
        }

        /// <summary>
        /// Tests that GetSqlQueryForId throws an IntranetRepositoryException when the identifier on the given storage type has no value.
        /// </summary>
        [Test]
        public void TestThatGetSqlQueryForIdThrowsIntranetRepositoryExceptionWhenIdentifierOnStorageTypeHasNoValue()
        {
            IStorageType storageTypeMock = MockRepository.GenerateMock<IStorageType>();
            storageTypeMock.Stub(m => m.Identifier)
                .Return(null)
                .Repeat.Any();

            IStorageTypeProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => ((StorageTypeProxy) sut).GetSqlQueryForId(storageTypeMock));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, storageTypeMock.Identifier, "Identifier");
        }

        /// <summary>
        /// Tests that GetSqlQueryForId returns the SQL statement for selecting the given storage type.
        /// </summary>
        [Test]
        public void TestThatGetSqlQueryForIdReturnsSqlQueryForId()
        {
            Guid identifier = Guid.NewGuid();

            IStorageType storageTypeMock = MockRepository.GenerateMock<IStorageType>();
            storageTypeMock.Stub(m => m.Identifier)
                .Return(identifier)
                .Repeat.Any();

            IStorageTypeProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            string result = ((StorageTypeProxy) sut).GetSqlQueryForId(storageTypeMock);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EqualTo($"SELECT StorageTypeIdentifier,SortOrder,Temperature,TemperatureRangeStartValue,TemperatureRangeEndValue,Creatable,Editable,Deletable FROM StorageTypes WHERE StorageTypeIdentifier='{identifier.ToString("D").ToUpper()}'"));
        }

        /// <summary>
        /// Tests that GetSqlCommandForInsert returns the SQL statement to insert this storage type.
        /// </summary>
        [Test]
        public void TestThatGetSqlCommandForInsertReturnsSqlCommandForInsert()
        {
            Fixture fixture = new Fixture();

            Guid identifier = Guid.NewGuid();
            int sortOrder = fixture.Create<int>();
            int temperatur = fixture.Create<int>();
            IRange<int> temperaturRange = DomainObjectMockBuilder.BuildIntRange();
            bool creatable = fixture.Create<bool>();
            bool editable = fixture.Create<bool>();
            bool deletable = fixture.Create<bool>();

            IStorageTypeProxy sut = CreateSut(identifier, sortOrder, temperatur, temperaturRange, creatable, editable, deletable);
            Assert.That(sut, Is.Not.Null);

            string result = ((StorageTypeProxy) sut).GetSqlCommandForInsert();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EqualTo($"INSERT INTO StorageTypes (StorageTypeIdentifier,SortOrder,Temperature,TemperatureRangeStartValue,TemperatureRangeEndValue,Creatable,Editable,Deletable) VALUES('{identifier.ToString("D").ToUpper()}',{sortOrder},{temperatur},{temperaturRange.StartValue},{temperaturRange.EndValue},{Convert.ToInt32(creatable)},{Convert.ToInt32(editable)},{Convert.ToInt32(deletable)})"));
        }

        /// <summary>
        /// Tests that GetSqlCommandForUpdate returns the SQL statement to update this storage type.
        /// </summary>
        [Test]
        public void TestThatGetSqlCommandForUpdateReturnsSqlCommandForUpdate()
        {
            Fixture fixture = new Fixture();

            Guid identifier = Guid.NewGuid();
            int sortOrder = fixture.Create<int>();
            int temperatur = fixture.Create<int>();
            IRange<int> temperaturRange = DomainObjectMockBuilder.BuildIntRange();
            bool creatable = fixture.Create<bool>();
            bool editable = fixture.Create<bool>();
            bool deletable = fixture.Create<bool>();

            IStorageTypeProxy sut = CreateSut(identifier, sortOrder, temperatur, temperaturRange, creatable, editable, deletable);
            Assert.That(sut, Is.Not.Null);

            string result = ((StorageTypeProxy) sut).GetSqlCommandForUpdate();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EqualTo($"UPDATE StorageTypes SET SortOrder={sortOrder},Temperature={temperatur},TemperatureRangeStartValue={temperaturRange.StartValue},TemperatureRangeEndValue={temperaturRange.EndValue},Creatable={Convert.ToInt32(creatable)},Editable={Convert.ToInt32(editable)},Deletable={Convert.ToInt32(deletable)} WHERE StorageTypeIdentifier='{identifier.ToString("D").ToUpper()}'"));
        }

        /// <summary>
        /// Tests that GetSqlCommandForDelete returns the SQL statement to delete this storage type.
        /// </summary>
        [Test]
        public void TestThatGetSqlCommandForDeleteReturnsSqlCommandForDelete()
        {
            Guid identifier = Guid.NewGuid();

            IStorageTypeProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            string result = ((StorageTypeProxy) sut).GetSqlCommandForDelete();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EqualTo($"DELETE FROM StorageTypes WHERE StorageTypeIdentifier='{identifier.ToString("D").ToUpper()}'"));
        }

        /// <summary>
        /// Tests that MapData throws an ArgumentNullException if the data reader is null.
        /// </summary>
        [Test]
        public void TestThatMapDataThrowsArgumentNullExceptionIfDataReaderIsNull()
        {
            IStorageTypeProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.MapData(null, MockRepository.GenerateMock<IDataProviderBase<MySqlCommand>>()));

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

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.MapData(MockRepository.GenerateStub<MySqlDataReader>(), null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that MapData throws an IntranetRepositoryException if the data reader is not type of MySqlDataReader.
        /// </summary>
        [Test]
        public void TestThatMapDataThrowsIntranetRepositoryExceptionIfDataReaderIsNotTypeOfMySqlDataReader()
        {
            IDataReader dataReader = MockRepository.GenerateMock<IDataReader>();

            IStorageTypeProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.MapData(dataReader, MockRepository.GenerateMock<IDataProviderBase<MySqlCommand>>()));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, "dataReader", dataReader.GetType().Name);
        }

        /// <summary>
        /// Tests that MapData maps data into the proxy.
        /// </summary>
        [Test]
        public void TestThatMapDataMapsDataIntoProxy()
        {
            Fixture fixture = new Fixture();

            Guid storageTypeIdentifier = Guid.NewGuid();
            int sortOrder = fixture.Create<int>();
            int temperatur = fixture.Create<int>();
            IRange<int> temperaturRange = DomainObjectMockBuilder.BuildIntRange();
            bool creatable = fixture.Create<bool>();
            bool editable = fixture.Create<bool>();
            bool deletable = fixture.Create<bool>();

            MySqlDataReader mySqlDataReaderStub = CreateMySqlDataReaderStub(storageTypeIdentifier, sortOrder, temperatur, temperaturRange, creatable, editable, deletable);

            IDataProviderBase<MySqlCommand> dataProviderMock = CreateDataProviderMock(fixture);

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

            sut.MapData(mySqlDataReaderStub, dataProviderMock);

            Assert.That(sut.Identifier, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.Identifier.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.Identifier.Value, Is.EqualTo(storageTypeIdentifier));
            Assert.That(sut.SortOrder, Is.EqualTo(sortOrder));
            Assert.That(sut.Temperature, Is.EqualTo(temperatur));
            Assert.That(sut.TemperatureRange, Is.Not.Null);
            Assert.That(sut.TemperatureRange.StartValue, Is.EqualTo(temperaturRange.StartValue));
            Assert.That(sut.TemperatureRange.EndValue, Is.EqualTo(temperaturRange.EndValue));
            Assert.That(sut.Creatable, Is.EqualTo(creatable));
            Assert.That(sut.Editable, Is.EqualTo(editable));
            Assert.That(sut.Deletable, Is.EqualTo(deletable));
            Assert.That(sut.Translation, Is.Null);
            Assert.That(sut.Translations, Is.Not.Null);
            Assert.That(sut.Translations, Is.Empty);
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
        /// Tests that MapRelations calls Clone on the data provider one time.
        /// </summary>
        [Test]
        public void TestThatMapRelationsCallsCloneOnDataProviderOneTime()
        {
            Fixture fixture = new Fixture();

            IDataProviderBase<MySqlCommand> dataProviderMock = CreateDataProviderMock(fixture);

            IStorageTypeProxy sut = CreateSut(storageTypeIdentifier: Guid.NewGuid());
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.Identifier.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse

            sut.MapRelations(dataProviderMock);

            dataProviderMock.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that MapRelations calls GetCollection on the data provider to get all translations for the storage type.
        /// </summary>
        [Test]
        public void TestThatMapRelationsCallsGetCollectionOnDataProviderToGetTranslations()
        {
            Fixture fixture = new Fixture();

            Guid storageTypeIdentifier = Guid.NewGuid();

            IDataProviderBase<MySqlCommand> dataProviderMock = CreateDataProviderMock(fixture);

            IStorageTypeProxy sut = CreateSut(storageTypeIdentifier: storageTypeIdentifier);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.Identifier.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(sut.Identifier.Value, Is.EqualTo(storageTypeIdentifier));
            // ReSharper restore PossibleInvalidOperationException

            sut.MapRelations(dataProviderMock);

            dataProviderMock.AssertWasCalled(m => m.GetCollection<TranslationProxy>(Arg<MySqlCommand>.Matches(cmd => cmd.CommandText == $"SELECT t.TranslationIdentifier AS TranslationIdentifier,t.OfIdentifier AS OfIdentifier,ti.TranslationInfoIdentifier AS InfoIdentifier,ti.CultureName AS CultureName,t.Value AS Value FROM Translations AS t, TranslationInfos AS ti WHERE t.OfIdentifier='{storageTypeIdentifier.ToString("D").ToUpper()}' AND ti.TranslationInfoIdentifier=t.InfoIdentifier ORDER BY CultureName")));
        }

        /// <summary>
        /// Tests that MapRelations maps translations for the storage type.
        /// </summary>
        [Test]
        public void TestThatMapRelationsMapsTranslationsIntoProxy()
        {
            Fixture fixture = new Fixture();

            Guid storageTypeIdentifier = Guid.NewGuid();

            IEnumerable<TranslationProxy> translationProxyCollection = fixture.CreateMany<TranslationProxy>().ToList();
            Assert.That(translationProxyCollection, Is.Not.Null);
            Assert.That(translationProxyCollection, Is.Not.Empty);

            IDataProviderBase<MySqlCommand> dataProviderMock = CreateDataProviderMock(fixture, translationProxyCollection: translationProxyCollection);

            IStorageTypeProxy sut = CreateSut(storageTypeIdentifier: storageTypeIdentifier);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.Identifier.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.Translation, Is.Null);
            Assert.That(sut.Translations, Is.Not.Null);
            Assert.That(sut.Translations, Is.Empty);

            sut.MapRelations(dataProviderMock);

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
            Fixture fixture = new Fixture();

            IStorageTypeProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            NotSupportedException result = Assert.Throws<NotSupportedException>(() => sut.SaveRelations(null, fixture.Create<bool>()));

            TestHelper.AssertNotSupportedExceptionIsValid(result);
        }

        /// <summary>
        /// Tests that SaveRelations throws an NotSupportedException when the data provider is not null.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsThrowsNotSupportedExceptionWhenDataProviderIsNotNull()
        {
            Fixture fixture = new Fixture();

            IStorageTypeProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            NotSupportedException result = Assert.Throws<NotSupportedException>(() => sut.SaveRelations(CreateDataProviderMock(fixture), fixture.Create<bool>()));

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
            Fixture fixture = new Fixture();

            IStorageTypeProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            NotSupportedException result = Assert.Throws<NotSupportedException>(() => sut.DeleteRelations(CreateDataProviderMock(fixture)));

            TestHelper.AssertNotSupportedExceptionIsValid(result);
        }

        /// <summary>
        /// Creates an instance of the data proxy to a given storage type which should be used for unit testing.
        /// </summary>
        /// <returns>Instance of the data proxy to a given storage type which should be used for unit testing.</returns>
        private static IStorageTypeProxy CreateSut(Guid? storageTypeIdentifier = null)
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
        private static IStorageTypeProxy CreateSut(Guid storageTypeIdentifier, int sortOrder, int temperature, IRange<int> temperatureRange, bool creatable, bool editable, bool deletable)
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
        private static MySqlDataReader CreateMySqlDataReaderStub(Guid storageTypeIdentifier, int sortOrder, int temperature, IRange<int> temperatureRange, bool creatable, bool editable, bool deletable)
        {
            if (temperatureRange == null)
            {
                throw new ArgumentNullException(nameof(temperatureRange));
            }

            MySqlDataReader mySqlDataReaderStub = MockRepository.GenerateStub<MySqlDataReader>();
            mySqlDataReaderStub.Stub(m => m.GetString(Arg<string>.Is.Equal("StorageTypeIdentifier")))
                .Return(storageTypeIdentifier.ToString("D").ToUpper())
                .Repeat.Any();
            mySqlDataReaderStub.Stub(m => m.GetInt16(Arg<string>.Is.Equal("SortOrder")))
                .Return((short) sortOrder)
                .Repeat.Any();
            mySqlDataReaderStub.Stub(m => m.GetInt16(Arg<string>.Is.Equal("Temperature")))
                .Return((short) temperature)
                .Repeat.Any();
            mySqlDataReaderStub.Stub(m => m.GetInt16("TemperatureRangeStartValue"))
                .Return((short) temperatureRange.StartValue)
                .Repeat.Any();
            mySqlDataReaderStub.Stub(m => m.GetInt16("TemperatureRangeEndValue"))
                .Return((short) temperatureRange.EndValue)
                .Repeat.Any();
            mySqlDataReaderStub.Stub(m => m.GetInt32(Arg<string>.Is.Equal("Creatable")))
                .Return(Convert.ToInt32(creatable))
                .Repeat.Any();
            mySqlDataReaderStub.Stub(m => m.GetInt32(Arg<string>.Is.Equal("Editable")))
                .Return(Convert.ToInt32(editable))
                .Repeat.Any();
            mySqlDataReaderStub.Stub(m => m.GetInt32(Arg<string>.Is.Equal("Deletable")))
                .Return(Convert.ToInt32(deletable))
                .Repeat.Any();
            return mySqlDataReaderStub;
        }

        /// <summary>
        /// Creates an instance of a data provider which should be used for unit testing.
        /// </summary>
        /// <returns>Instance of a data provider which should be used for unit testing</returns>
        private static IDataProviderBase<MySqlCommand> CreateDataProviderMock(Fixture fixture, IEnumerable<TranslationProxy> translationProxyCollection = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            IDataProviderBase<MySqlCommand> dataProviderMock = MockRepository.GenerateMock<IDataProviderBase<MySqlCommand>>();
            dataProviderMock.Stub(m => m.Clone())
                .Return(dataProviderMock)
                .Repeat.Any();
            dataProviderMock.Stub(m => m.GetCollection<TranslationProxy>(Arg<MySqlCommand>.Is.Anything))
                .Return(translationProxyCollection ?? fixture.CreateMany<TranslationProxy>().ToList())
                .Repeat.Any();
            return dataProviderMock;
        }
    }
}
