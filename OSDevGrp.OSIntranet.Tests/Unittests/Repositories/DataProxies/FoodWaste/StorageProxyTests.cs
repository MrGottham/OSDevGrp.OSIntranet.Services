using System;
using AutoFixture;
using MySql.Data.MySqlClient;
using MySql.Data.Types;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Guards;
using OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Resources;
using OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.DataProxies.FoodWaste
{
    /// <summary>
    /// Tests the data proxy to a given storage.
    /// </summary>
    [TestFixture]
    public class StorageProxyTests
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
        /// Tests that the constructor initialize a data proxy to a given storage.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeStorageProxy()
        {
            IStorageProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);
            Assert.That(sut.Household, Is.Null);
            Assert.That(sut.SortOrder, Is.EqualTo(default(int)));
            Assert.That(sut.StorageType, Is.Null);
            Assert.That(sut.Description, Is.Null);
            Assert.That(sut.Temperature, Is.EqualTo(default(int)));
            Assert.That(sut.CreationTime, Is.EqualTo(default(DateTime)));
        }

        /// <summary>
        /// Tests that getter for UniqueId gets the unique identifier for the storage.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterGetsUniqueIdentificationForStorageProxy()
        {
            Guid identifier = Guid.NewGuid();

            IStorageProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Not.Null);

            string uniqueId = sut.UniqueId;
            Assert.That(uniqueId, Is.Not.Null);
            Assert.That(uniqueId, Is.Not.Empty);
            Assert.That(uniqueId, Is.EqualTo(identifier.ToString("D").ToUpper()));
        }

        /// <summary>
        /// Tests that getter of UniqueId throws an IntranetRepositoryException when the storage has no identifier.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterThrowsIntranetRepositoryExceptionWhenStorageHasNoIdentifier()
        {
            IStorageProxy sut = CreateSut();
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
            IStorageProxy sut = CreateSut();
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
            IStorageProxy sut = CreateSut();
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
        public void TestThatMapDataMapsDataIntoProxy(bool hasDescription)
        {
            IStorageProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            Guid identifier = Guid.NewGuid();
            Guid householdIdentifier = Guid.NewGuid();
            int sortOrder = GetLegalSortOrder();
            Guid storageTypeIdentifier = Guid.NewGuid();
            string description = hasDescription ? _fixture.Create<string>() : null;
            IRange<int> temperatureRange = DomainObjectMockBuilder.BuildIntRange();
            int temperature = GetLegalTemperature(temperatureRange);
            DateTime creationTime = DateTime.Now;
            MySqlDataReader dataReader = CreateMySqlDataReader(identifier, householdIdentifier, sortOrder, storageTypeIdentifier, description, temperature, creationTime);

            HouseholdProxy householdProxy = BuildHouseholdProxy();
            StorageTypeProxy storageTypeProxy = BuildStorageTypeProxy(temperatureRange: temperatureRange);
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(householdProxy, storageTypeProxy);

            sut.MapData(dataReader, dataProvider);

            Assert.That(sut.Identifier, Is.Not.Null);
            Assert.That(sut.Identifier, Is.EqualTo(identifier));
            Assert.That(sut.Household, Is.Not.Null);
            Assert.That(sut.Household, Is.EqualTo(householdProxy));
            Assert.That(sut.SortOrder, Is.EqualTo(sortOrder));
            Assert.That(sut.StorageType, Is.Not.Null);
            Assert.That(sut.StorageType, Is.EqualTo(storageTypeProxy));
            if (string.IsNullOrWhiteSpace(description) == false)
            {
                Assert.That(sut.Description, Is.Not.Null);
                Assert.That(sut.Description, Is.Not.Empty);
                Assert.That(sut.Description, Is.EqualTo(description));
            }
            else
            {
                Assert.That(sut.Description, Is.Null);
            }
            Assert.That(sut.Temperature, Is.EqualTo(temperature));
            Assert.That(sut.CreationTime, Is.EqualTo(creationTime).Within(1).Milliseconds);

            dataReader.AssertWasCalled(m => m.GetString(Arg<string>.Is.Equal("StorageIdentifier")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetInt16(Arg<string>.Is.Equal("SortOrder")), opt => opt.Repeat.Once());
            // ReSharper disable StringLiteralTypo
            dataReader.AssertWasCalled(m => m.GetOrdinal(Arg<string>.Is.Equal("Descr")), opt => opt.Repeat.Once());
            // ReSharper restore StringLiteralTypo
            dataReader.AssertWasCalled(m => m.IsDBNull(Arg<int>.Is.Equal(4)), opt => opt.Repeat.Once());
            if (string.IsNullOrWhiteSpace(description) == false)
            {
                dataReader.AssertWasCalled(m => m.GetString(Arg<int>.Is.Equal(4)), opt => opt.Repeat.Once());
            }
            else
            {
                dataReader.AssertWasNotCalled(m => m.GetString(Arg<int>.Is.Equal(4)));
            }
            dataReader.AssertWasCalled(m => m.GetInt16(Arg<string>.Is.Equal("Temperature")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetMySqlDateTime(Arg<string>.Is.Equal("CreationTime")), opt => opt.Repeat.Once());

            dataProvider.AssertWasNotCalled(m => m.Clone());

            dataProvider.AssertWasCalled(m => m.Create(
                    Arg<IHouseholdProxy>.Is.TypeOf,
                    Arg<MySqlDataReader>.Is.Equal(dataReader),
                    Arg<string[]>.Matches(e => e != null && e.Length == 4 &&
                                               e[0] == "HouseholdIdentifier" &&
                                               e[1] == "HouseholdName" &&
                                               // ReSharper disable StringLiteralTypo
                                               e[2] == "HouseholdDescr" &&
                                               // ReSharper restore StringLiteralTypo
                                               e[3] == "HouseholdCreationTime")),
                opt => opt.Repeat.Once());

            dataProvider.AssertWasCalled(m => m.Create(
                    Arg<IStorageTypeProxy>.Is.TypeOf,
                    Arg<MySqlDataReader>.Is.Equal(dataReader),
                    Arg<string[]>.Matches(e => e != null && e.Length == 8 &&
                                               e[0] == "StorageTypeIdentifier" &&
                                               e[1] == "StorageTypeSortOrder" &&
                                               e[2] == "StorageTypeTemperature" &&
                                               e[3] == "StorageTypeTemperatureRangeStartValue" &&
                                               e[4] == "StorageTypeTemperatureRangeEndValue" &&
                                               e[5] == "StorageTypeCreatable" &&
                                               e[6] == "StorageTypeEditable" &&
                                               e[7] == "StorageTypeDeletable")),
                opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that MapRelations throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatMapRelationsThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            IStorageProxy sut = CreateSut();
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
            IStorageProxy sut = CreateSut();
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
            IStorageProxy sut = CreateSut();
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
            IStorageProxy sut = CreateSut();
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
            IStorageProxy sut = CreateSut();
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
            IStorageProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.DeleteRelations(CreateFoodWasteDataProvider()));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, sut.Identifier, "Identifier");
        }

        /// <summary>
        /// Tests that CreateGetCommand returns the SQL command for selecting the given storage.
        /// </summary>
        [Test]
        public void TestThatCreateGetCommandReturnsSqlQueryForId()
        {
            Guid identifier = Guid.NewGuid();

            IStorageProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            // ReSharper disable StringLiteralTypo
            new DbCommandTestBuilder("SELECT s.StorageIdentifier,s.HouseholdIdentifier,s.SortOrder,s.StorageTypeIdentifier,s.Descr,s.Temperature,s.CreationTime,h.Name AS HouseholdName,h.Descr AS HouseholdDescr,h.CreationTime AS HouseholdCreationTime,st.SortOrder AS StorageTypeSortOrder,st.Temperature AS StorageTypeTemperature,st.TemperatureRangeStartValue AS StorageTypeTemperatureRangeStartValue,st.TemperatureRangeEndValue AS StorageTypeTemperatureRangeEndValue,st.Creatable AS StorageTypeCreatable,st.Editable AS StorageTypeEditable,st.Deletable AS StorageTypeDeletable FROM Storages AS s INNER JOIN Households AS h ON h.HouseholdIdentifier=s.HouseholdIdentifier INNER JOIN StorageTypes AS st ON st.StorageTypeIdentifier=s.StorageTypeIdentifier WHERE s.StorageIdentifier=@storageIdentifier")
            // ReSharper restore StringLiteralTypo
                .AddCharDataParameter("@storageIdentifier", identifier)
                .Build()
                .Run(sut.CreateGetCommand());
        }

        /// <summary>
        /// Tests that CreateInsertCommand returns the SQL command to insert this storage type.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatCreateInsertCommandReturnsSqlCommandForInsert(bool hasDescription)
        {
            Guid identifier = Guid.NewGuid();
            Guid householdIdentifier = Guid.NewGuid();
            IHousehold householdMock = DomainObjectMockBuilder.BuildHouseholdMock(householdIdentifier);
            int sortOrder = GetLegalSortOrder();
            Guid storageTypeIdentifier = Guid.NewGuid();
            IStorageType storageTypeMock = DomainObjectMockBuilder.BuildStorageTypeMock(storageTypeIdentifier);
            int temperature = GetLegalTemperature(storageTypeMock.TemperatureRange);
            DateTime creationTime = DateTime.Now;
            string description = hasDescription ? _fixture.Create<string>() : null;

            IStorageProxy sut = CreateSut(identifier, householdMock, sortOrder, storageTypeMock, temperature, creationTime, description);
            Assert.That(sut, Is.Not.Null);

            // ReSharper disable StringLiteralTypo
            new DbCommandTestBuilder("INSERT INTO Storages (StorageIdentifier,HouseholdIdentifier,SortOrder,StorageTypeIdentifier,Descr,Temperature,CreationTime) VALUES(@storageIdentifier,@householdIdentifier,@sortOrder,@storageTypeIdentifier,@descr,@temperature,@creationTime)")
            // ReSharper restore StringLiteralTypo
                .AddCharDataParameter("@storageIdentifier", identifier)
                .AddCharDataParameter("@householdIdentifier", householdIdentifier)
                .AddTinyIntDataParameter("@sortOrder", sortOrder, 4)
                .AddCharDataParameter("@storageTypeIdentifier", storageTypeIdentifier)
                // ReSharper disable StringLiteralTypo
                .AddVarCharDataParameter("@descr", description, 2048, true)
                // ReSharper restore StringLiteralTypo
                .AddTinyIntDataParameter("@temperature", temperature, 4)
                .AddDateTimeDataParameter("@creationTime", creationTime.ToUniversalTime())
                .Build()
                .Run(sut.CreateInsertCommand());
        }

        /// <summary>
        /// Tests that CreateUpdateCommand returns the SQL command to update this storage type.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatCreateUpdateCommandReturnsSqlCommandForUpdate(bool hasDescription)
        {
            Guid identifier = Guid.NewGuid();
            Guid householdIdentifier = Guid.NewGuid();
            IHousehold householdMock = DomainObjectMockBuilder.BuildHouseholdMock(householdIdentifier);
            int sortOrder = GetLegalSortOrder();
            Guid storageTypeIdentifier = Guid.NewGuid();
            IStorageType storageTypeMock = DomainObjectMockBuilder.BuildStorageTypeMock(storageTypeIdentifier);
            int temperature = GetLegalTemperature(storageTypeMock.TemperatureRange);
            DateTime creationTime = DateTime.Now;
            string description = hasDescription ? _fixture.Create<string>() : null;

            IStorageProxy sut = CreateSut(identifier, householdMock, sortOrder, storageTypeMock, temperature, creationTime, description);
            Assert.That(sut, Is.Not.Null);

            // ReSharper disable StringLiteralTypo
            new DbCommandTestBuilder("UPDATE Storages SET HouseholdIdentifier=@householdIdentifier,SortOrder=@sortOrder,StorageTypeIdentifier=@storageTypeIdentifier,Descr=@descr,Temperature=@temperature,CreationTime=@creationTime WHERE StorageIdentifier=@storageIdentifier")
            // ReSharper restore StringLiteralTypo
                .AddCharDataParameter("@storageIdentifier", identifier)
                .AddCharDataParameter("@householdIdentifier", householdIdentifier)
                .AddTinyIntDataParameter("@sortOrder", sortOrder, 4)
                .AddCharDataParameter("@storageTypeIdentifier", storageTypeIdentifier)
                // ReSharper disable StringLiteralTypo
                .AddVarCharDataParameter("@descr", description, 2048, true)
                // ReSharper restore StringLiteralTypo
                .AddTinyIntDataParameter("@temperature", temperature, 4)
                .AddDateTimeDataParameter("@creationTime", creationTime.ToUniversalTime())
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

            IStorageProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            // ReSharper disable StringLiteralTypo
            new DbCommandTestBuilder("DELETE FROM Storages WHERE StorageIdentifier=@storageIdentifier")
            // ReSharper restore StringLiteralTypo
                .AddCharDataParameter("@storageIdentifier", identifier)
                .Build()
                .Run(sut.CreateDeleteCommand());
        }

        /// <summary>
        /// Tests that Create throws an ArgumentNullException if the data reader is null.
        /// </summary>
        [Test]
        public void TestThatCreateThrowsArgumentNullExceptionIfDataReaderIsNull()
        {
            IStorageProxy sut = CreateSut();
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
            IStorageProxy sut = CreateSut();
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
            IStorageProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Create(CreateMySqlDataReader(), CreateFoodWasteDataProvider(), null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "columnNameCollection");
        }

        /// <summary>
        /// Tests that Create creates a data proxy to a given household member with values from the data reader.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatCreateCreatesProxy(bool hasDescription)
        {
            IStorageProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            Guid identifier = Guid.NewGuid();
            Guid householdIdentifier = Guid.NewGuid();
            int sortOrder = GetLegalSortOrder();
            Guid storageTypeIdentifier = Guid.NewGuid();
            string description = hasDescription ? _fixture.Create<string>() : null;
            IRange<int> temperatureRange = DomainObjectMockBuilder.BuildIntRange();
            int temperature = GetLegalTemperature(temperatureRange);
            DateTime creationTime = DateTime.Now;
            MySqlDataReader dataReader = CreateMySqlDataReader(identifier, householdIdentifier, sortOrder, storageTypeIdentifier, description, temperature, creationTime);

            HouseholdProxy householdProxy = BuildHouseholdProxy();
            StorageTypeProxy storageTypeProxy = BuildStorageTypeProxy(temperatureRange: temperatureRange);
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(householdProxy, storageTypeProxy);

            // ReSharper disable StringLiteralTypo
            IStorageProxy result = sut.Create(dataReader, dataProvider, "StorageIdentifier", "HouseholdIdentifier", "SortOrder", "StorageTypeIdentifier", "Descr", "Temperature", "CreationTime", "HouseholdName", "HouseholdDescr", "HouseholdCreationTime", "StorageTypeSortOrder", "StorageTypeTemperature", "StorageTypeTemperatureRangeStartValue", "StorageTypeTemperatureRangeEndValue", "StorageTypeCreatable", "StorageTypeEditable", "StorageTypeDeletable");
            // ReSharper restore StringLiteralTypo
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Identifier, Is.Not.Null);
            Assert.That(result.Identifier, Is.EqualTo(identifier));
            Assert.That(result.Household, Is.Not.Null);
            Assert.That(result.Household, Is.EqualTo(householdProxy));
            Assert.That(result.SortOrder, Is.EqualTo(sortOrder));
            Assert.That(result.StorageType, Is.Not.Null);
            Assert.That(result.StorageType, Is.EqualTo(storageTypeProxy));
            if (string.IsNullOrWhiteSpace(description) == false)
            {
                Assert.That(result.Description, Is.Not.Null);
                Assert.That(result.Description, Is.Not.Empty);
                Assert.That(result.Description, Is.EqualTo(description));
            }
            else
            {
                Assert.That(result.Description, Is.Null);
            }
            Assert.That(result.Temperature, Is.EqualTo(temperature));
            Assert.That(result.CreationTime, Is.EqualTo(creationTime).Within(1).Milliseconds);

            dataReader.AssertWasCalled(m => m.GetString(Arg<string>.Is.Equal("StorageIdentifier")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetInt16(Arg<string>.Is.Equal("SortOrder")), opt => opt.Repeat.Once());
            // ReSharper disable StringLiteralTypo
            dataReader.AssertWasCalled(m => m.GetOrdinal(Arg<string>.Is.Equal("Descr")), opt => opt.Repeat.Once());
            // ReSharper restore StringLiteralTypo
            dataReader.AssertWasCalled(m => m.IsDBNull(Arg<int>.Is.Equal(4)), opt => opt.Repeat.Once());
            if (string.IsNullOrWhiteSpace(description) == false)
            {
                dataReader.AssertWasCalled(m => m.GetString(Arg<int>.Is.Equal(4)), opt => opt.Repeat.Once());
            }
            else
            {
                dataReader.AssertWasNotCalled(m => m.GetString(Arg<int>.Is.Equal(4)));
            }
            dataReader.AssertWasCalled(m => m.GetInt16(Arg<string>.Is.Equal("Temperature")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetMySqlDateTime(Arg<string>.Is.Equal("CreationTime")), opt => opt.Repeat.Once());

            dataProvider.AssertWasNotCalled(m => m.Clone());

            dataProvider.AssertWasCalled(m => m.Create(
                    Arg<IHouseholdProxy>.Is.TypeOf,
                    Arg<MySqlDataReader>.Is.Equal(dataReader),
                    Arg<string[]>.Matches(e => e != null && e.Length == 4 &&
                                               e[0] == "HouseholdIdentifier" &&
                                               e[1] == "HouseholdName" &&
                                               // ReSharper disable StringLiteralTypo
                                               e[2] == "HouseholdDescr" &&
                                               // ReSharper restore StringLiteralTypo
                                               e[3] == "HouseholdCreationTime")),
                opt => opt.Repeat.Once());

            dataProvider.AssertWasCalled(m => m.Create(
                    Arg<IStorageTypeProxy>.Is.TypeOf,
                    Arg<MySqlDataReader>.Is.Equal(dataReader),
                    Arg<string[]>.Matches(e => e != null && e.Length == 8 &&
                                               e[0] == "StorageTypeIdentifier" &&
                                               e[1] == "StorageTypeSortOrder" &&
                                               e[2] == "StorageTypeTemperature" &&
                                               e[3] == "StorageTypeTemperatureRangeStartValue" &&
                                               e[4] == "StorageTypeTemperatureRangeEndValue" &&
                                               e[5] == "StorageTypeCreatable" &&
                                               e[6] == "StorageTypeEditable" &&
                                               e[7] == "StorageTypeDeletable")),
                opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Creates an instance of the data proxy to a given storage which should be used for unit testing.
        /// </summary>
        /// <returns>Instance of the data proxy to a given storage which should be used for unit testing.</returns>
        private IStorageProxy CreateSut(Guid? storageIdentifier = null)
        {
            return new StorageProxy
            {
                Identifier = storageIdentifier
            };
        }

        /// <summary>
        /// Creates an instance of the data proxy to a given storage which should be used for unit testing.
        /// </summary>
        /// <returns>Instance of the data proxy to a given storage which should be used for unit testing.</returns>
        private IStorageProxy CreateSut(Guid storageIdentifier, IHousehold household, int sortOrder, IStorageType storageType, int temperature, DateTime creationTime, string description = null)
        {
            return new StorageProxy(household, sortOrder, storageType, temperature, creationTime, description)
            {
                Identifier = storageIdentifier
            };
        }

        /// <summary>
        /// Creates an instance of a MySQL data reader which should be used for unit testing.
        /// </summary>
        /// <returns>Instance of a MySQL data reader which should be used for unit testing.</returns>
        private MySqlDataReader CreateMySqlDataReader(Guid? storageIdentifier = null, Guid? householdIdentifier = null, int? sortOrder = null, Guid? storageTypeIdentifier = null, string description = null, int? temperature = null, DateTime? creationTime = null)
        {
            MySqlDataReader mySqlDataReaderStub = MockRepository.GenerateStub<MySqlDataReader>();
            mySqlDataReaderStub.Stub(m => m.GetString(Arg<string>.Is.Equal("StorageIdentifier")))
                .Return(storageIdentifier.HasValue ? storageIdentifier.Value.ToString("D").ToUpper() : Guid.NewGuid().ToString("D").ToUpper())
                .Repeat.Any();
            mySqlDataReaderStub.Stub(m => m.GetString(Arg<string>.Is.Equal("HouseholdIdentifier")))
                .Return(householdIdentifier.HasValue ? householdIdentifier.Value.ToString("D").ToUpper() : Guid.NewGuid().ToString("D").ToUpper())
                .Repeat.Any();
            mySqlDataReaderStub.Stub(m => m.GetInt16(Arg<string>.Is.Equal("SortOrder")))
                .Return((short) (sortOrder ?? GetLegalSortOrder()))
                .Repeat.Any();
            mySqlDataReaderStub.Stub(m => m.GetString(Arg<string>.Is.Equal("StorageTypeIdentifier")))
                .Return(storageTypeIdentifier.HasValue ? storageTypeIdentifier.Value.ToString("D").ToUpper() : Guid.NewGuid().ToString("D").ToUpper())
                .Repeat.Any();
            // ReSharper disable StringLiteralTypo
            mySqlDataReaderStub.Stub(m => m.GetOrdinal(Arg<string>.Is.Equal("Descr")))
            // ReSharper restore StringLiteralTypo
                .Return(4)
                .Repeat.Any();
            mySqlDataReaderStub.Stub(m => m.IsDBNull(Arg<int>.Is.Equal(4)))
                .Return(string.IsNullOrWhiteSpace(description))
                .Repeat.Any();
            mySqlDataReaderStub.Stub(m => m.GetString(Arg<int>.Is.Equal(4)))
                .Return(description ?? _fixture.Create<string>())
                .Repeat.Any();
            mySqlDataReaderStub.Stub(m => m.GetInt16(Arg<string>.Is.Equal("Temperature")))
                .Return((short) (temperature ?? GetLegalTemperature(DomainObjectMockBuilder.BuildIntRange())))
                .Repeat.Any();
            mySqlDataReaderStub.Stub(m => m.GetMySqlDateTime(Arg<string>.Is.Equal("CreationTime")))
                .Return(new MySqlDateTime((creationTime ?? DateTime.Now).ToUniversalTime()))
                .Repeat.Any();
            return mySqlDataReaderStub;
        }

        /// <summary>
        /// Creates an instance of a data provider which should be used for unit testing.
        /// </summary>
        /// <returns>Instance of a data provider which should be used for unit testing</returns>
        private IFoodWasteDataProvider CreateFoodWasteDataProvider(HouseholdProxy householdProxy = null, StorageTypeProxy storageTypeProxy = null)
        {
            IFoodWasteDataProvider dataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            dataProviderMock.Stub(m => m.Create(Arg<IHouseholdProxy>.Is.TypeOf, Arg<MySqlDataReader>.Is.Anything, Arg<string[]>.Is.Anything))
                .Return(householdProxy ?? BuildHouseholdProxy())
                .Repeat.Any();
            dataProviderMock.Stub(m => m.Create(Arg<IStorageTypeProxy>.Is.TypeOf, Arg<MySqlDataReader>.Is.Anything, Arg<string[]>.Is.Anything))
                .Return(storageTypeProxy ?? BuildStorageTypeProxy())
                .Repeat.Any();
            return dataProviderMock;
        }

        /// <summary>
        /// Gets a legal sort order for a storage.
        /// </summary>
        /// <returns>Legal sort order for a storage.</returns>
        private int GetLegalSortOrder()
        {
            return _random.Next(1, 100);
        }

        /// <summary>
        /// Gets a legal temperature for a storage.
        /// </summary>
        /// <param name="temperatureRange">Temperature range.</param>
        /// <returns>Legal temperature for a storage.</returns>
        private int GetLegalTemperature(IRange<int> temperatureRange)
        {
            ArgumentNullGuard.NotNull(temperatureRange, nameof(temperatureRange));

            return _random.Next(temperatureRange.StartValue, temperatureRange.EndValue);
        }

        /// <summary>
        /// Creates a data proxy to a household.
        /// </summary>
        /// <param name="identifier">The identifier for the data proxy.</param>
        /// <returns>Data proxy to a household.</returns>
        private HouseholdProxy BuildHouseholdProxy(Guid? identifier = null)
        {
            return new HouseholdProxy
            {
                Identifier = identifier ?? Guid.NewGuid()
            };
        }

        /// <summary>
        /// Creates a data proxy to a household.
        /// </summary>
        /// <param name="identifier">The identifier for the data proxy.</param>
        /// <param name="temperatureRange">The temperature range.</param>
        /// <returns>Data proxy to a household.</returns>
        private StorageTypeProxy BuildStorageTypeProxy(Guid? identifier = null, IRange<int> temperatureRange = null)
        {
            if (temperatureRange == null)
            {
                temperatureRange = DomainObjectMockBuilder.BuildIntRange();
            }

            return new StorageTypeProxy(GetLegalSortOrder(), GetLegalTemperature(temperatureRange), temperatureRange, _fixture.Create<bool>(), _fixture.Create<bool>(), _fixture.Create<bool>())
            {
                Identifier = identifier ?? Guid.NewGuid()
            };
        }
    }
}
