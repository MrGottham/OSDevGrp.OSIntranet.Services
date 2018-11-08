using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Guards;
using OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Resources;
using OSDevGrp.OSIntranet.Tests.Unittests.Repositories.DataProxies;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.FoodWaste
{
    /// <summary>
    /// Tests the repository which can access system data for the food waste domain.
    /// </summary>
    [TestFixture]
    public class SystemDataRepositoryTests
    {
        #region Private variables

        private Fixture _fixture;
        private IFoodWasteDataProvider _foodWasteDataProviderMock;
        private IFoodWasteObjectMapper _foodWasteObjectMapperMock;

        #endregion

        /// <summary>
        /// Setup each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            _foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
        }

        /// <summary>
        /// Tests that the constructor initialize a repository which can access system data for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeSystemDataRepository()
        {
            ISystemDataRepository sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException if the data provider which can access data in the food waste repository is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionIfFoodWasteDataProviderIsNull()
        {
            IFoodWasteObjectMapper foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => new SystemDataRepository(null, foodWasteObjectMapperMock));
            // ReSharper restore ObjectCreationAsStatement

            TestHelper.AssertArgumentNullExceptionIsValid(result, "foodWasteDataProvider");
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException if the object mapper which can map objects in the food waste domain is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionIfFoodWasteObjectMapperIsNull()
        {
            IFoodWasteDataProvider foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();

            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => new SystemDataRepository(foodWasteDataProviderMock, null));
            // ReSharper restore ObjectCreationAsStatement

            TestHelper.AssertArgumentNullExceptionIsValid(result, "foodWasteObjectMapper");
        }

        /// <summary>
        /// Tests that StorageTypeGetAll calls GetCollection on the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatStorageTypeGetAllCallsGetCollectionOnFoodWasteDataProvider()
        {
            ISystemDataRepository sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            sut.StorageTypeGetAll();

            _foodWasteDataProviderMock.AssertWasCalled(m => m.GetCollection<StorageTypeProxy>(Arg<MySqlCommand>.Matches(cmd => cmd.CommandText == "SELECT StorageTypeIdentifier,SortOrder,Temperature,TemperatureRangeStartValue,TemperatureRangeEndValue,Creatable,Editable,Deletable FROM StorageTypes ORDER BY SortOrder")));
        }

        /// <summary>
        /// Tests that StorageTypeGetAll returns the result from the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatStorageTypeGetAllReturnsResultFromFoodWasteDataProvider()
        {
            Fixture fixture = new Fixture();

            IEnumerable<StorageTypeProxy> storageTypeProxyCollection = BuildStorageTypeProxyCollection(fixture);

            ISystemDataRepository sut = CreateSut(storageTypeProxyCollection: storageTypeProxyCollection);
            Assert.That(sut, Is.Not.Null);

            IEnumerable<IStorageType> result = sut.StorageTypeGetAll();
            // ReSharper disable PossibleMultipleEnumeration
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EqualTo(storageTypeProxyCollection));
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        /// Tests that StorageTypeGetAll throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        [Test]
        public void TestThatStorageTypeGetAllThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs()
        {
            Fixture fixture = new Fixture();
            IntranetRepositoryException exceptionToThrow = fixture.Create<IntranetRepositoryException>();

            ISystemDataRepository sut = CreateSut(exceptionToThrow: exceptionToThrow);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.StorageTypeGetAll());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that StorageTypeGetAll throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        [Test]
        public void TestThatStorageTypeGetAllThrowsIntranetRepositoryExceptionWhenExceptionOccurs()
        {
            Fixture fixture = new Fixture();
            Exception exceptionToThrow = fixture.Create<Exception>();

            ISystemDataRepository sut = CreateSut(exceptionToThrow: exceptionToThrow);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.StorageTypeGetAll());

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, exceptionToThrow, ExceptionMessage.RepositoryError, "StorageTypeGetAll", exceptionToThrow.Message);
        }

        /// <summary>
        /// Tests that FoodItemGetAll calls GetCollection on the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatFoodItemGetAllCallsGetCollectionOnFoodWasteDataProvider()
        {
            ISystemDataRepository sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            sut.FoodItemGetAll();

            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT fi.FoodItemIdentifier,fi.IsActive FROM FoodItems AS fi").Build();
            _foodWasteDataProviderMock.AssertWasCalled(m => m.GetCollection<FoodItemProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that FoodItemGetAll returns the result from the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatFoodItemGetAllReturnsResultFromFoodWasteDataProvider()
        {
            IEnumerable<FoodItemProxy> foodItemProxyCollection = BuildFoodItemProxyCollection();

            ISystemDataRepository sut = CreateSut(foodItemProxyCollection: foodItemProxyCollection);
            Assert.That(sut, Is.Not.Null);

            IEnumerable<IFoodItem> result = sut.FoodItemGetAll();
            // ReSharper disable PossibleMultipleEnumeration
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EqualTo(foodItemProxyCollection));
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        /// Tests that FoodItemGetAll throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        [Test]
        public void TestThatFoodItemGetAllThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs()
        {
            IntranetRepositoryException exceptionToThrow = _fixture.Create<IntranetRepositoryException>();

            ISystemDataRepository sut = CreateSut(exceptionToThrow: exceptionToThrow);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.FoodItemGetAll());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that FoodItemGetAll throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        [Test]
        public void TestThatFoodItemGetAllThrowsIntranetRepositoryExceptionWhenExceptionOccurs()
        {
            Exception exceptionToThrow = _fixture.Create<Exception>();

            ISystemDataRepository sut = CreateSut(exceptionToThrow: exceptionToThrow);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.FoodItemGetAll());

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, exceptionToThrow, ExceptionMessage.RepositoryError, "FoodItemGetAll", exceptionToThrow.Message);
        }

        /// <summary>
        /// Tests that FoodItemGetAllForFoodGroup throws an ArgumentNullException when the food group is null.
        /// </summary>
        [Test]
        public void TestThatFoodItemGetAllForFoodGroupThrowsArgumentNullExceptionWhenFoodGroupIsNull()
        {
            ISystemDataRepository sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result  = Assert.Throws<ArgumentNullException>(() => sut.FoodItemGetAllForFoodGroup(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "foodGroup");
        }

        /// <summary>
        /// Tests that FoodItemGetAllForFoodGroup throws an IntranetRepositoryException when the identifier on the food group has no value.
        /// </summary>
        [Test]
        public void TestThatFoodItemGetAllForFoodGroupThrowsIntranetRepositoryExceptionWhenIdentifierOnFoodGroupHasNoValue()
        {
            IFoodGroup foodGroupMock = BuildFoodGroupMock(null);

            ISystemDataRepository sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.FoodItemGetAllForFoodGroup(foodGroupMock));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, foodGroupMock.Identifier, "Identifier");
        }

        /// <summary>
        /// Tests that FoodItemGetAllForFoodGroup calls GetCollection on the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatFoodItemGetAllForFoodGroupCallsGetCollectionOnFoodWasteDataProvider()
        {
            Guid identifier = Guid.NewGuid();
            IFoodGroup foodGroupMock = BuildFoodGroupMock(identifier);

            ISystemDataRepository sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            sut.FoodItemGetAllForFoodGroup(foodGroupMock);

            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT fi.FoodItemIdentifier,fi.IsActive FROM FoodItems AS fi INNER JOIN FoodItemGroups AS fig ON fig.FoodItemIdentifier=fi.FoodItemIdentifier WHERE fig.FoodGroupIdentifier=@foodGroupIdentifier")
                .AddCharDataParameter("@foodGroupIdentifier", identifier)
                .Build();
            _foodWasteDataProviderMock.AssertWasCalled(m => m.GetCollection<FoodItemProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that FoodItemGetAllForFoodGroup returns the result from the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatFoodItemGetAllForFoodGroupReturnsResultFromFoodWasteDataProvider()
        {
            IEnumerable<FoodItemProxy> foodItemProxyCollection = BuildFoodItemProxyCollection();

            IFoodGroup foodGroupMock = BuildFoodGroupMock();

            ISystemDataRepository sut = CreateSut(foodItemProxyCollection: foodItemProxyCollection);
            Assert.That(sut, Is.Not.Null);

            IEnumerable<IFoodItem> result = sut.FoodItemGetAllForFoodGroup(foodGroupMock);
            // ReSharper disable PossibleMultipleEnumeration
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EqualTo(foodItemProxyCollection));
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        /// Tests that FoodItemGetAllForFoodGroup throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        [Test]
        public void TestThatFoodItemGetAllForFoodGroupThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs()
        {
            IntranetRepositoryException exceptionToThrow = _fixture.Create<IntranetRepositoryException>();

            IFoodGroup foodGroupMock = BuildFoodGroupMock();

            ISystemDataRepository sut = CreateSut(exceptionToThrow: exceptionToThrow);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.FoodItemGetAllForFoodGroup(foodGroupMock));
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that FoodItemGetAllForFoodGroup throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        [Test]
        public void TestThatFoodItemGetAllForFoodGroupThrowsIntranetRepositoryExceptionWhenExceptionOccurs()
        {
            Exception exceptionToThrow = _fixture.Create<Exception>();

            IFoodGroup foodGroupMock = BuildFoodGroupMock();

            ISystemDataRepository sut = CreateSut(exceptionToThrow: exceptionToThrow);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.FoodItemGetAllForFoodGroup(foodGroupMock));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, exceptionToThrow, ExceptionMessage.RepositoryError, "FoodItemGetAllForFoodGroup", exceptionToThrow.Message);
        }

        /// <summary>
        /// Tests that FoodItemGetByForeignKey throws an ArgumentNullException when the data provider is null.
        /// </summary>
        [Test]
        public void TestThatFoodItemGetByForeignKeyThrowsArgumentNullExceptionWhenDataProviderIsNull()
        {
            ISystemDataRepository sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.FoodItemGetByForeignKey(null, _fixture.Create<string>()));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that FoodItemGetByForeignKey throws an ArgumentNullException when the foreign key value is invalid.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestThatFoodItemGetByForeignKeyThrowsArgumentNullExceptionWhenForeignKeyValueIsInvalid(string invalidValue)
        {
            IDataProvider dataProviderMock = BuildDataProviderMock();

            ISystemDataRepository sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.FoodItemGetByForeignKey(dataProviderMock, invalidValue));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "foreignKeyValue");
        }

        /// <summary>
        /// Tests that FoodItemGetByForeignKey throws an IntranetRepositoryException when the identifier on the data provider has no value.
        /// </summary>
        [Test]
        public void TestThatFoodItemGetByForeignKeyThrowsIntranetRepositoryExceptionWhenIdentifierOnDataProviderHasNoValue()
        {
            IDataProvider dataProviderMock = BuildDataProviderMock(null);

            ISystemDataRepository sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.FoodItemGetByForeignKey(dataProviderMock, _fixture.Create<string>()));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, dataProviderMock.Identifier, "Identifier");
        }

        /// <summary>
        /// Tests that FoodItemGetByForeignKey calls GetCollection on the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatFoodItemGetByForeignKeyCallsGetCollectionOnFoodWasteDataProvider()
        {
            Guid identifier = Guid.NewGuid();
            IDataProvider dataProviderMock = BuildDataProviderMock(identifier);

            string foreignKeyValue = _fixture.Create<string>();

            ISystemDataRepository sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            sut.FoodItemGetByForeignKey(dataProviderMock, foreignKeyValue);

            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT fi.FoodItemIdentifier,fi.IsActive FROM FoodItems AS fi INNER JOIN ForeignKeys AS fk ON fk.ForeignKeyForIdentifier=fi.FoodItemIdentifier WHERE fk.DataProviderIdentifier=@dataProviderIdentifier AND fk.ForeignKeyForTypes LIKE @foreignKeyForTypes AND fk.ForeignKeyValue=@foreignKeyValue")
                .AddCharDataParameter("@dataProviderIdentifier", identifier)
                .AddVarCharDataParameter("@foreignKeyForTypes", $"%{typeof(IFoodItem).Name}%", 128)
                .AddVarCharDataParameter("@foreignKeyValue", foreignKeyValue, 128)
                .Build();
            _foodWasteDataProviderMock.AssertWasCalled(m => m.GetCollection<FoodItemProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that FoodItemGetByForeignKey returns null when no food item was found for the data provider and the foreign key value.
        /// </summary>
        [Test]
        public void TestThatFoodItemGetByForeignKeyReturnsNullWhenNoFoodItemWasFound()
        {
            IDataProvider dataProviderMock = BuildDataProviderMock();

            ISystemDataRepository sut = CreateSut(foodItemProxyCollection: new List<FoodItemProxy>(0));
            Assert.That(sut, Is.Not.Null);

            IFoodItem result = sut.FoodItemGetByForeignKey(dataProviderMock, _fixture.Create<string>());
            Assert.That(result, Is.Null);
        }

        /// <summary>
        /// Tests that FoodItemGetByForeignKey returns the food item which was found for the data provider and the foreign key value.
        /// </summary>
        [Test]
        public void TestThatFoodItemGetByForeignKeyReturnsFoodItemWhenItWasFound()
        {
            IDataProvider dataProviderMock = BuildDataProviderMock();

            FoodItemProxy foodItemProxy = new FoodItemProxy(BuildFoodGroupMock());

            ISystemDataRepository sut = CreateSut(foodItemProxyCollection: new List<FoodItemProxy> {foodItemProxy});
            Assert.That(sut, Is.Not.Null);

            IFoodItem result = sut.FoodItemGetByForeignKey(dataProviderMock, _fixture.Create<string>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(foodItemProxy));
        }

        /// <summary>
        /// Tests that FoodItemGetByForeignKey throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        [Test]
        public void TestThatFoodItemGetByForeignKeyThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs()
        {
            IntranetRepositoryException exceptionToThrow = _fixture.Create<IntranetRepositoryException>();

            IDataProvider dataProviderMock = BuildDataProviderMock();

            ISystemDataRepository sut = CreateSut(exceptionToThrow: exceptionToThrow);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.FoodItemGetByForeignKey(dataProviderMock, _fixture.Create<string>()));
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that FoodItemGetByForeignKey throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        [Test]
        public void TestThatFoodItemGetByForeignKeyThrowsIntranetRepositoryExceptionWhenExceptionOccurs()
        {
            Exception exceptionToThrow = _fixture.Create<Exception>();

            IDataProvider dataProviderMock = BuildDataProviderMock();

            ISystemDataRepository sut = CreateSut(exceptionToThrow: exceptionToThrow);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.FoodItemGetByForeignKey(dataProviderMock, _fixture.Create<string>()));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, exceptionToThrow, ExceptionMessage.RepositoryError, "FoodItemGetByForeignKey", exceptionToThrow.Message);
        }

        /// <summary>
        /// Tests that FoodGroupGetAll calls GetCollection on the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatFoodGroupGetAllCallsGetCollectionOnFoodWasteDataProvider()
        {
            ISystemDataRepository sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            sut.FoodGroupGetAll();

            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT fg.FoodGroupIdentifier,fg.ParentIdentifier,fg.IsActive,pfg.ParentIdentifier AS ParentsParentIdentifier,pfg.IsActive AS ParentIsActive FROM FoodGroups AS fg LEFT JOIN FoodGroups AS pfg ON pfg.FoodGroupIdentifier=fg.ParentIdentifier").Build();
            _foodWasteDataProviderMock.AssertWasCalled(m => m.GetCollection<FoodGroupProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that FoodGroupGetAll returns the result from the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatFoodGroupGetAllReturnsResultFromFoodWasteDataProvider()
        {
            IEnumerable<FoodGroupProxy> foodGroupProxyCollection = BuildFoodGroupProxyCollection(_fixture);

            ISystemDataRepository sut = CreateSut(foodGroupProxyCollection: foodGroupProxyCollection);
            Assert.That(sut, Is.Not.Null);

            IEnumerable<IFoodGroup> result = sut.FoodGroupGetAll();
            // ReSharper disable PossibleMultipleEnumeration
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EqualTo(foodGroupProxyCollection));
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        /// Tests that FoodGroupGetAll throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        [Test]
        public void TestThatFoodGroupGetAllThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs()
        {
            IntranetRepositoryException exceptionToThrow = _fixture.Create<IntranetRepositoryException>();

            ISystemDataRepository sut = CreateSut(exceptionToThrow: exceptionToThrow);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.FoodGroupGetAll());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that FoodGroupGetAll throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        [Test]
        public void TestThatFoodGroupGetAllGetThrowsIntranetRepositoryExceptionWhenExceptionOccurs()
        {
            Exception exceptionToThrow = _fixture.Create<Exception>();

            ISystemDataRepository sut = CreateSut(exceptionToThrow: exceptionToThrow);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.FoodGroupGetAll());

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, exceptionToThrow, ExceptionMessage.RepositoryError, "FoodGroupGetAll", exceptionToThrow.Message);
        }

        /// <summary>
        /// Tests that FoodGroupGetAllOnRoot calls GetCollection on the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatFoodGroupGetAllOnRootCallsGetCollectionOnFoodWasteDataProvider()
        {
            ISystemDataRepository sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            sut.FoodGroupGetAllOnRoot();

            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT fg.FoodGroupIdentifier,fg.ParentIdentifier,fg.IsActive,pfg.ParentIdentifier AS ParentsParentIdentifier,pfg.IsActive AS ParentIsActive FROM FoodGroups AS fg LEFT JOIN FoodGroups AS pfg ON pfg.FoodGroupIdentifier=fg.ParentIdentifier WHERE fg.ParentIdentifier IS NULL").Build();
            _foodWasteDataProviderMock.AssertWasCalled(m => m.GetCollection<FoodGroupProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that FoodGroupGetAllOnRoot returns the result from the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatFoodGroupGetAllOnRootReturnsResultFromFoodWasteDataProvider()
        {
            IEnumerable<FoodGroupProxy> foodGroupProxyCollection = BuildFoodGroupProxyCollection(_fixture);

            ISystemDataRepository sut = CreateSut(foodGroupProxyCollection: foodGroupProxyCollection);
            Assert.That(sut, Is.Not.Null);

            IEnumerable<IFoodGroup> result = sut.FoodGroupGetAllOnRoot();
            // ReSharper disable PossibleMultipleEnumeration
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EqualTo(foodGroupProxyCollection));
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        /// Tests that FoodGroupGetAllOnRoot throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        [Test]
        public void TestThatFoodGroupGetAllOnRootThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs()
        {
            IntranetRepositoryException exceptionToThrow = _fixture.Create<IntranetRepositoryException>();

            ISystemDataRepository sut = CreateSut(exceptionToThrow: exceptionToThrow);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.FoodGroupGetAllOnRoot());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that FoodGroupGetAllOnRoot throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        [Test]
        public void TestThatFoodGroupGetAllOnRootGetThrowsIntranetRepositoryExceptionWhenExceptionOccurs()
        {
            Exception exceptionToThrow = _fixture.Create<Exception>();

            ISystemDataRepository sut = CreateSut(exceptionToThrow: exceptionToThrow);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.FoodGroupGetAllOnRoot());

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, exceptionToThrow, ExceptionMessage.RepositoryError, "FoodGroupGetAllOnRoot", exceptionToThrow.Message);
        }

        /// <summary>
        /// Tests that FoodGroupGetByForeignKey throws an ArgumentNullException when the data provider is null.
        /// </summary>
        [Test]
        public void TestThatFoodGroupGetByForeignKeyThrowsArgumentNullExceptionWhenDataProviderIsNull()
        {
            ISystemDataRepository sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.FoodGroupGetByForeignKey(null, _fixture.Create<string>()));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that FoodGroupGetByForeignKey throws an ArgumentNullException when the foreign key value is invalid.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestThatFoodGroupGetByForeignKeyThrowsArgumentNullExceptionWhenForeignKeyValueIsInvalid(string invalidValue)
        {
            IDataProvider dataProviderMock = BuildDataProviderMock();

            ISystemDataRepository sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result  = Assert.Throws<ArgumentNullException>(() => sut.FoodGroupGetByForeignKey(dataProviderMock, invalidValue));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "foreignKeyValue");
        }

        /// <summary>
        /// Tests that FoodGroupGetByForeignKey throws an IntranetRepositoryException when the identifier on the data provider has no value.
        /// </summary>
        [Test]
        public void TestThatFoodGroupGetByForeignKeyThrowsIntranetRepositoryExceptionWhenIdentifierOnDataProviderHasNoValue()
        {
            IDataProvider dataProviderMock = BuildDataProviderMock(null);

            ISystemDataRepository sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.FoodGroupGetByForeignKey(dataProviderMock, _fixture.Create<string>()));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, dataProviderMock.Identifier, "Identifier");
        }

        /// <summary>
        /// Tests that FoodGroupGetByForeignKey calls GetCollection on the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatFoodGroupGetByForeignKeyGetCallsGetCollectionOnFoodWasteDataProvider()
        {
            Guid identifier = Guid.NewGuid();
            IDataProvider dataProviderMock = BuildDataProviderMock(identifier);

            string foreignKeyValue = _fixture.Create<string>();

            ISystemDataRepository sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            sut.FoodGroupGetByForeignKey(dataProviderMock, foreignKeyValue);

            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT fg.FoodGroupIdentifier,fg.ParentIdentifier,fg.IsActive,pfg.ParentIdentifier AS ParentsParentIdentifier,pfg.IsActive AS ParentIsActive FROM FoodGroups AS fg LEFT JOIN FoodGroups AS pfg ON pfg.FoodGroupIdentifier=fg.ParentIdentifier INNER JOIN ForeignKeys AS fk ON fk.ForeignKeyForIdentifier=fg.FoodGroupIdentifier WHERE fk.DataProviderIdentifier=@dataProviderIdentifier AND fk.ForeignKeyForTypes LIKE @foreignKeyForTypes AND fk.ForeignKeyValue=@foreignKeyValue")
                .AddCharDataParameter("@dataProviderIdentifier", identifier)
                .AddVarCharDataParameter("@foreignKeyForTypes", $"%{typeof(IFoodGroup).Name}%", 128)
                .AddVarCharDataParameter("@foreignKeyValue", foreignKeyValue, 128)
                .Build();
            _foodWasteDataProviderMock.AssertWasCalled(m => m.GetCollection<FoodGroupProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that FoodGroupGetByForeignKey returns null when no food group was found for the data provider and the foreign key value.
        /// </summary>
        [Test]
        public void TestThatFoodGroupGetByForeignKeyGetReturnsNullWhenNoFoodGroupWasFound()
        {
            IDataProvider dataProviderMock = BuildDataProviderMock();

            ISystemDataRepository sut = CreateSut(foodGroupProxyCollection: new List<FoodGroupProxy>(0));
            Assert.That(sut, Is.Not.Null);

            IFoodGroup result = sut.FoodGroupGetByForeignKey(dataProviderMock, _fixture.Create<string>());
            Assert.That(result, Is.Null);
        }

        /// <summary>
        /// Tests that FoodGroupGetByForeignKey returns the food group which was found for the data provider and the foreign key value.
        /// </summary>
        [Test]
        public void TestThatFoodGroupGetByForeignKeyGetReturnsFoodGroupWhenItWasFound()
        {
            FoodGroupProxy foodGroupProxy = _fixture.Build<FoodGroupProxy>()
                .With(m => m.Parent, null)
                .Create();

            IDataProvider dataProviderMock = BuildDataProviderMock();

            ISystemDataRepository sut = CreateSut(foodGroupProxyCollection: new List<FoodGroupProxy> {foodGroupProxy});
            Assert.That(sut, Is.Not.Null);

            IFoodGroup result = sut.FoodGroupGetByForeignKey(dataProviderMock, _fixture.Create<string>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(foodGroupProxy));
        }

        /// <summary>
        /// Tests that FoodGroupGetByForeignKey throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        [Test]
        public void TestThatFoodGroupGetByForeignKeyGetThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs()
        {
            IntranetRepositoryException exceptionToThrow = _fixture.Create<IntranetRepositoryException>();

            IDataProvider dataProviderMock = BuildDataProviderMock();

            ISystemDataRepository sut = CreateSut(exceptionToThrow: exceptionToThrow);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.FoodGroupGetByForeignKey(dataProviderMock, _fixture.Create<string>()));
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that FoodGroupGetByForeignKey throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        [Test]
        public void TestThatFoodGroupGetByForeignKeyGetThrowsIntranetRepositoryExceptionWhenExceptionOccurs()
        {
            Exception exceptionToThrow = _fixture.Create<Exception>();

            IDataProvider dataProviderMock = BuildDataProviderMock();

            ISystemDataRepository sut = CreateSut(exceptionToThrow: exceptionToThrow);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.FoodGroupGetByForeignKey(dataProviderMock, _fixture.Create<string>()));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, exceptionToThrow, ExceptionMessage.RepositoryError, "FoodGroupGetByForeignKey", exceptionToThrow.Message);
        }

        /// <summary>
        /// Tests that ForeignKeysForDomainObjectGet throws an ArgumentNullException when the identifiable domain object is null.
        /// </summary>
        [Test]
        public void TestThatForeignKeysForDomainObjectGetThrowsArgumentNullExceptionWhenIdentifiableDomainObjectIsNull()
        {
            ISystemDataRepository sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ForeignKeysForDomainObjectGet(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "identifiableDomainObject");
        }

        /// <summary>
        /// Tests that ForeignKeysForDomainObjectGet throws an IntranetRepositoryException when the identifier on the identifiable domain object has no value.
        /// </summary>
        [Test]
        public void TestThatForeignKeysForDomainObjectGetThrowsIntranetRepositoryExceptionWhenIdentifierOnIdentifiableDomainObjectHasNoValue()
        {
            IIdentifiable identifiableDomainObjectMock = BuildIdentifiableMock(null);

            ISystemDataRepository sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.ForeignKeysForDomainObjectGet(identifiableDomainObjectMock));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, identifiableDomainObjectMock.Identifier, "Identifier");
        }

        /// <summary>
        /// Tests that ForeignKeysForDomainObjectGet calls GetCollection on the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatForeignKeysForDomainObjectGetCallsGetCollectionOnFoodWasteDataProvider()
        {
            Guid identifier = Guid.NewGuid();
            IIdentifiable identifiableDomainObjectMock = BuildIdentifiableMock(identifier);

            ISystemDataRepository sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            sut.ForeignKeysForDomainObjectGet(identifiableDomainObjectMock);

            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT fk.ForeignKeyIdentifier,fk.DataProviderIdentifier,dp.Name AS DataProviderName,dp.HandlesPayments,dp.DataSourceStatementIdentifier,fk.ForeignKeyForIdentifier,fk.ForeignKeyForTypes,fk.ForeignKeyValue FROM ForeignKeys AS fk INNER JOIN DataProviders AS dp ON dp.DataProviderIdentifier=fk.DataProviderIdentifier WHERE fk.ForeignKeyForIdentifier=@foreignKeyForIdentifier")
                .AddCharDataParameter("@foreignKeyForIdentifier", identifier)
                .Build();
            _foodWasteDataProviderMock.AssertWasCalled(m => m.GetCollection<ForeignKeyProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that ForeignKeysForDomainObjectGet returns the result from the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatForeignKeysForDomainObjectGetReturnsResultFromFoodWasteDataProvider()
        {
            IIdentifiable identifiableDomainObjectMock = BuildIdentifiableMock();

            IEnumerable<ForeignKeyProxy> foreignKeyProxyCollection = BuildForeignKeyProxyCollection(_fixture);

            ISystemDataRepository sut = CreateSut(foreignKeyProxyCollection: foreignKeyProxyCollection);
            Assert.That(sut, Is.Not.Null);

            IEnumerable<IForeignKey> result = sut.ForeignKeysForDomainObjectGet(identifiableDomainObjectMock);
            // ReSharper disable PossibleMultipleEnumeration
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EqualTo(foreignKeyProxyCollection));
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        /// Tests that ForeignKeysForDomainObjectGet throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        [Test]
        public void TestThatForeignKeysForDomainObjectGetThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs()
        {
            IntranetRepositoryException exceptionToThrow = _fixture.Create<IntranetRepositoryException>();

            IIdentifiable identifiableDomainObjectMock = BuildIdentifiableMock();

            ISystemDataRepository sut = CreateSut(exceptionToThrow: exceptionToThrow);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.ForeignKeysForDomainObjectGet(identifiableDomainObjectMock));
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that ForeignKeysForDomainObjectGet throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        [Test]
        public void TestThatForeignKeysForDomainObjectGetThrowsIntranetRepositoryExceptionWhenExceptionOccurs()
        {
            Exception exceptionToThrow = _fixture.Create<Exception>();

            IIdentifiable identifiableDomainObjectMock = BuildIdentifiableMock();

            ISystemDataRepository sut = CreateSut(exceptionToThrow: exceptionToThrow);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.ForeignKeysForDomainObjectGet(identifiableDomainObjectMock));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, exceptionToThrow, ExceptionMessage.RepositoryError, "ForeignKeysForDomainObjectGet", exceptionToThrow.Message);
        }

        /// <summary>
        /// Tests that StaticTextGetByStaticTextType calls GetCollection on the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatStaticTextGetByStaticTextTypeCallsGetCollectionOnFoodWasteDataProvider()
        {
            IEnumerable<StaticTextProxy> staticTextProxyCollection = Enum.GetValues(typeof(StaticTextType))
                .Cast<StaticTextType>()
                .Select(staticTextType => new StaticTextProxy(staticTextType, Guid.NewGuid()))
                .ToList();

            ISystemDataRepository sut = CreateSut(staticTextProxyCollection: staticTextProxyCollection);
            Assert.That(sut, Is.Not.Null);

            foreach (StaticTextType staticTextTypeToTest in Enum.GetValues(typeof (StaticTextType)).Cast<StaticTextType>())
            {
                sut.StaticTextGetByStaticTextType(staticTextTypeToTest);

                MySqlCommand cmd = (MySqlCommand) _foodWasteDataProviderMock.GetArgumentsForCallsMadeOn(m => m.GetCollection<StaticTextProxy>(Arg<MySqlCommand>.Is.NotNull))
                    .Last()
                    .ElementAt(0);
                new DbCommandTestBuilder("SELECT StaticTextIdentifier,StaticTextType,SubjectTranslationIdentifier,BodyTranslationIdentifier FROM StaticTexts WHERE StaticTextType=@staticTextType")
                    .AddTinyIntDataParameter("@staticTextType", (int) staticTextTypeToTest, 4)
                    .Build()
                    .Run(cmd);
            }

            _foodWasteDataProviderMock.AssertWasCalled(m => m.GetCollection<StaticTextProxy>(Arg<MySqlCommand>.Is.NotNull), opt => opt.Repeat.Times(staticTextProxyCollection.Count()));
        }

        /// <summary>
        /// Tests that StaticTextGetByStaticTextType returns the result from the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatStaticTextGetByStaticTextTypeReturnsResultFromFoodWasteDataProvider()
        {
            StaticTextType staticTextType = _fixture.Create<StaticTextType>();

            StaticTextProxy staticText = new StaticTextProxy(staticTextType, Guid.NewGuid());
            IEnumerable<StaticTextProxy> staticTextProxyCollection = new List<StaticTextProxy> {staticText};

            ISystemDataRepository sut = CreateSut(staticTextProxyCollection: staticTextProxyCollection);
            Assert.That(sut, Is.Not.Null);

            IStaticText result = sut.StaticTextGetByStaticTextType(staticTextType);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(staticText));
        }

        /// <summary>
        /// Tests that StaticTextGetByStaticTextType throws an IntranetRepositoryException when the static text type was not found.
        /// </summary>
        [Test]
        public void TestThatStaticTextGetByStaticTextTypeThrowsIntranetRepositoryExceptionWhenStaticTextTypeWasNotFound()
        {
            StaticTextType staticTextType = _fixture.Create<StaticTextType>();

            IEnumerable<StaticTextProxy> staticTextProxyCollection = new List<StaticTextProxy>(0);

            ISystemDataRepository sut = CreateSut(staticTextProxyCollection: staticTextProxyCollection);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.StaticTextGetByStaticTextType(staticTextType));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.CantFindObjectById, typeof(IStaticText).Name, staticTextType);
        }

        /// <summary>
        /// Tests that StaticTextGetByStaticTextType throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        [Test]
        public void TestThatStaticTextGetByStaticTextTypeThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs()
        {
            IntranetRepositoryException exceptionToThrow = _fixture.Create<IntranetRepositoryException>();

            ISystemDataRepository sut = CreateSut(exceptionToThrow: exceptionToThrow);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.StaticTextGetByStaticTextType(_fixture.Create<StaticTextType>()));
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that StaticTextGetByStaticTextType throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        [Test]
        public void TestThatStaticTextGetByStaticTextTypeThrowsIntranetRepositoryExceptionWhenExceptionOccurs()
        {
            Exception exceptionToThrow = _fixture.Create<Exception>();

            ISystemDataRepository sut = CreateSut(exceptionToThrow: exceptionToThrow);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result= Assert.Throws<IntranetRepositoryException>(() => sut.StaticTextGetByStaticTextType(_fixture.Create<StaticTextType>()));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, exceptionToThrow, ExceptionMessage.RepositoryError, "StaticTextGetByStaticTextType", exceptionToThrow.Message);
        }

        /// <summary>
        /// Tests that StaticTextGetAll calls GetCollection on the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatStaticTextGetAllCallsGetCollectionOnFoodWasteDataProvider()
        {
            ISystemDataRepository sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            sut.StaticTextGetAll();

            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT StaticTextIdentifier,StaticTextType,SubjectTranslationIdentifier,BodyTranslationIdentifier FROM StaticTexts ORDER BY StaticTextType").Build();
            _foodWasteDataProviderMock.AssertWasCalled(m => m.GetCollection<StaticTextProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that StaticTextGetAll returns the result from the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatStaticTextGetAllReturnsResultFromFoodWasteDataProvider()
        {
            IEnumerable<StaticTextProxy> staticTextProxyCollection = BuildStaticTextProxyCollection(_fixture);

            ISystemDataRepository sut = CreateSut(staticTextProxyCollection: staticTextProxyCollection);
            Assert.That(sut, Is.Not.Null);

            IEnumerable<IStaticText> result = sut.StaticTextGetAll();
            // ReSharper disable PossibleMultipleEnumeration
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EqualTo(result));
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        /// Tests that StaticTextGetAll throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        [Test]
        public void TestThatStaticTextGetAllThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs()
        {
            IntranetRepositoryException exceptionToThrow = _fixture.Create<IntranetRepositoryException>();

            ISystemDataRepository sut = CreateSut(exceptionToThrow: exceptionToThrow);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.StaticTextGetAll());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that StaticTextGetAll throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        [Test]
        public void TestThatStaticTextGetAllThrowsIntranetRepositoryExceptionWhenExceptionOccurs()
        {
            Exception exceptionToThrow = _fixture.Create<Exception>();

            ISystemDataRepository sut = CreateSut(exceptionToThrow: exceptionToThrow);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.StaticTextGetAll());

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, exceptionToThrow, ExceptionMessage.RepositoryError, "StaticTextGetAll", exceptionToThrow.Message);
        }

        /// <summary>
        /// Tests that DataProviderForFoodItemsGet calls Get on the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatDataProviderForFoodItemsGetCallsGetOnFoodWasteDataProvider()
        {
            ISystemDataRepository sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            sut.DataProviderForFoodItemsGet();

            _foodWasteDataProviderMock.AssertWasCalled(m => m.Get(Arg<DataProviderProxy>.Matches(dataProviderProxy =>
                dataProviderProxy != null &&
                // ReSharper disable MergeSequentialChecks
                dataProviderProxy.Identifier != null &&
                // ReSharper restore MergeSequentialChecks
                dataProviderProxy.Identifier.HasValue &&
                dataProviderProxy.Identifier.Value == new Guid("5A1B9283-6406-44DF-91C5-F2FB83CC9A42"))));
        }

        /// <summary>
        /// Tests that DataProviderForFoodItemsGet returns the result from the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatDataProviderForFoodItemsGetReturnsResultFromFoodWasteDataProvider()
        {
            DataProviderProxy dataProviderProxy = _fixture.Create<DataProviderProxy>();

            ISystemDataRepository sut = CreateSut(dataProviderProxy: dataProviderProxy);
            Assert.That(sut, Is.Not.Null);

            IDataProvider result = sut.DataProviderForFoodItemsGet();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(dataProviderProxy));
        }

        /// <summary>
        /// Tests that DataProviderForFoodItemsGet throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        [Test]
        public void TestThatDataProviderForFoodItemsGetThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs()
        {
            IntranetRepositoryException exceptionToThrow = _fixture.Create<IntranetRepositoryException>();

            ISystemDataRepository sut = CreateSut(exceptionToThrow: exceptionToThrow);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.DataProviderForFoodItemsGet());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that DataProviderForFoodItemsGet throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        [Test]
        public void TestThatDataProviderForFoodItemsGetThrowsIntranetRepositoryExceptionWhenExceptionOccurs()
        {
            Exception exceptionToThrow = _fixture.Create<Exception>();

            ISystemDataRepository sut = CreateSut(exceptionToThrow: exceptionToThrow);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.DataProviderForFoodItemsGet());

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, exceptionToThrow, ExceptionMessage.RepositoryError, "DataProviderForFoodItemsGet", exceptionToThrow.Message);
        }

        /// <summary>
        /// Tests that DataProviderForFoodGroupsGet calls Get on the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatDataProviderForFoodGroupsGetCallsGetOnFoodWasteDataProvider()
        {
            ISystemDataRepository sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            sut.DataProviderForFoodGroupsGet();

            _foodWasteDataProviderMock.AssertWasCalled(m => m.Get(Arg<DataProviderProxy>.Matches(dataProviderProxy =>
                dataProviderProxy != null &&
                // ReSharper disable MergeSequentialChecks
                dataProviderProxy.Identifier != null &&
                // ReSharper restore MergeSequentialChecks
                dataProviderProxy.Identifier.HasValue &&
                dataProviderProxy.Identifier.Value == new Guid("5A1B9283-6406-44DF-91C5-F2FB83CC9A42"))));
        }

        /// <summary>
        /// Tests that DataProviderForFoodGroupsGet returns the result from the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatDataProviderForFoodGroupsGetReturnsResultFromFoodWasteDataProvider()
        {
            DataProviderProxy dataProviderProxy = _fixture.Create<DataProviderProxy>();

            ISystemDataRepository sut = CreateSut(dataProviderProxy: dataProviderProxy);
            Assert.That(sut, Is.Not.Null);

            IDataProvider result = sut.DataProviderForFoodGroupsGet();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(dataProviderProxy));
        }

        /// <summary>
        /// Tests that DataProviderForFoodGroupsGet throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        [Test]
        public void TestThatDataProviderForFoodGroupsGetThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs()
        {
            IntranetRepositoryException exceptionToThrow = _fixture.Create<IntranetRepositoryException>();

            ISystemDataRepository sut = CreateSut(exceptionToThrow: exceptionToThrow);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.DataProviderForFoodGroupsGet());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that DataProviderForFoodGroupsGet throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        [Test]
        public void TestThatDataProviderForFoodGroupsGetThrowsIntranetRepositoryExceptionWhenExceptionOccurs()
        {
            Exception exceptionToThrow = _fixture.Create<Exception>();

            ISystemDataRepository sut = CreateSut(exceptionToThrow: exceptionToThrow);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.DataProviderForFoodGroupsGet());

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, exceptionToThrow, ExceptionMessage.RepositoryError, "DataProviderForFoodGroupsGet", exceptionToThrow.Message);
        }

        /// <summary>
        /// Tests that DataProviderGetAll calls GetCollection on the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatDataProviderGetAllCallsGetCollectionOnFoodWasteDataProvider()
        {
            ISystemDataRepository sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            sut.DataProviderGetAll();

            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT DataProviderIdentifier,Name,HandlesPayments,DataSourceStatementIdentifier FROM DataProviders ORDER BY Name").Build();
            _foodWasteDataProviderMock.AssertWasCalled(m => m.GetCollection<DataProviderProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that DataProviderGetAll returns the result from the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatDataProviderGetAllReturnsResultFromFoodWasteDataProvider()
        {
            IEnumerable<DataProviderProxy> dataProviderProxyCollection = BuildDataProviderProxyCollection(_fixture);

            ISystemDataRepository sut = CreateSut(dataProviderProxyCollection: dataProviderProxyCollection);
            Assert.That(sut, Is.Not.Null);

            IEnumerable<IDataProvider> result = sut.DataProviderGetAll();
            // ReSharper disable PossibleMultipleEnumeration
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EqualTo(dataProviderProxyCollection));
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        /// Tests that DataProviderGetAll throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        [Test]
        public void TestThatDataProviderGetAllThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs()
        {
            IntranetRepositoryException exceptionToThrow = _fixture.Create<IntranetRepositoryException>();

            ISystemDataRepository sut = CreateSut(exceptionToThrow: exceptionToThrow);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.DataProviderGetAll());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that DataProviderGetAll throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        [Test]
        public void TestThatDataProviderGetAllThrowsIntranetRepositoryExceptionWhenExceptionOccurs()
        {
            Exception exceptionToThrow = _fixture.Create<Exception>();

            ISystemDataRepository sut = CreateSut(exceptionToThrow: exceptionToThrow);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.DataProviderGetAll());

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, exceptionToThrow, ExceptionMessage.RepositoryError, "DataProviderGetAll", exceptionToThrow.Message);
        }

        /// <summary>
        /// Tests that DataProviderWhoHandlesPaymentsGetAll calls GetCollection on the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatDataProviderWhoHandlesPaymentsGetAllCallsGetCollectionOnFoodWasteDataProvider()
        {
            ISystemDataRepository sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            sut.DataProviderWhoHandlesPaymentsGetAll();

            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT DataProviderIdentifier,Name,HandlesPayments,DataSourceStatementIdentifier FROM DataProviders WHERE HandlesPayments=1 ORDER BY Name").Build();
            _foodWasteDataProviderMock.AssertWasCalled(m => m.GetCollection<DataProviderProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that DataProviderWhoHandlesPaymentsGetAll returns the result from the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatDataProviderWhoHandlesPaymentsGetAllReturnsResultFromFoodWasteDataProvider()
        {
            IEnumerable<DataProviderProxy> dataProviderProxyCollection = BuildDataProviderProxyCollection(_fixture);

            ISystemDataRepository sut = CreateSut(dataProviderProxyCollection: dataProviderProxyCollection);
            Assert.That(sut, Is.Not.Null);

            IEnumerable<IDataProvider> result = sut.DataProviderWhoHandlesPaymentsGetAll();
            // ReSharper disable PossibleMultipleEnumeration
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EqualTo(dataProviderProxyCollection));
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        /// Tests that DataProviderWhoHandlesPaymentsGetAll throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        [Test]
        public void TestThatDataProviderWhoHandlesPaymentsGetAllThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs()
        {
            IntranetRepositoryException exceptionToThrow = _fixture.Create<IntranetRepositoryException>();

            ISystemDataRepository sut = CreateSut(exceptionToThrow: exceptionToThrow);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.DataProviderWhoHandlesPaymentsGetAll());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that DataProviderWhoHandlesPaymentsGetAll throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        [Test]
        public void TestThatDataProviderWhoHandlesPaymentsGetAllThrowsIntranetRepositoryExceptionWhenExceptionOccurs()
        {
            Exception exceptionToThrow = _fixture.Create<Exception>();

            ISystemDataRepository sut = CreateSut(exceptionToThrow: exceptionToThrow);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.DataProviderWhoHandlesPaymentsGetAll());

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, exceptionToThrow, ExceptionMessage.RepositoryError, "DataProviderWhoHandlesPaymentsGetAll", exceptionToThrow.Message);
        }

        /// <summary>
        /// Tests that TranslationsForDomainObjectGet throws an ArgumentNullException when the identifiable domain object is null.
        /// </summary>
        [Test]
        public void TestThatTranslationsForDomainObjectGetThrowsArgumentNullExceptionWhenIdentifiableDomainObjectIsNull()
        {
            ISystemDataRepository sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.TranslationsForDomainObjectGet(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "identifiableDomainObject");
        }

        /// <summary>
        /// Tests that TranslationsForDomainObjectGet throws an IntranetRepositoryException when the identifier on the identifiable domain object has no value.
        /// </summary>
        [Test]
        public void TestThatTranslationsForDomainObjectGetThrowsIntranetRepositoryExceptionWhenIdentifierOnIdentifiableDomainObjectHasNoValue()
        {
            IIdentifiable identifiableDomainObjectMock = BuildIdentifiableMock(null);

            ISystemDataRepository sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.TranslationsForDomainObjectGet(identifiableDomainObjectMock));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, identifiableDomainObjectMock.Identifier, "Identifier");
        }

        /// <summary>
        /// Tests that TranslationsForDomainObjectGet calls GetCollection on the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatTranslationsForDomainObjectGetCallsGetCollectionOnFoodWasteDataProvider()
        {
            Guid identifier = Guid.NewGuid();
            IIdentifiable identifiableDomainObjectMock = BuildIdentifiableMock(identifier);

            ISystemDataRepository sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            sut.TranslationsForDomainObjectGet(identifiableDomainObjectMock);

            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT t.TranslationIdentifier AS TranslationIdentifier,t.OfIdentifier AS OfIdentifier,ti.TranslationInfoIdentifier AS InfoIdentifier,ti.CultureName AS CultureName,t.Value AS Value FROM Translations AS t INNER JOIN TranslationInfos AS ti ON ti.TranslationInfoIdentifier=t.InfoIdentifier WHERE t.OfIdentifier=@ofIdentifier ORDER BY ti.CultureName")
                .AddCharDataParameter("@ofIdentifier", identifier)
                .Build();
            _foodWasteDataProviderMock.AssertWasCalled(m => m.GetCollection<TranslationProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that TranslationsForDomainObjectGet returns the result from the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatTranslationsForDomainObjectGetReturnsResultFromFoodWasteDataProvider()
        {
            IIdentifiable identifiableDomainObjectMock = BuildIdentifiableMock();

            IEnumerable<TranslationProxy> translationProxyCollection = BuildTranslationProxyCollection(_fixture);

            ISystemDataRepository sut = CreateSut(translationProxyCollection: translationProxyCollection);
            Assert.That(sut, Is.Not.Null);

            IEnumerable<ITranslation> result = sut.TranslationsForDomainObjectGet(identifiableDomainObjectMock);
            // ReSharper disable PossibleMultipleEnumeration
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EqualTo(translationProxyCollection));
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        /// Tests that TranslationsForDomainObjectGet throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        [Test]
        public void TestThatTranslationsForDomainObjectGetThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs()
        {
            IntranetRepositoryException exceptionToThrow = _fixture.Create<IntranetRepositoryException>();

            IIdentifiable identifiableDomainObjectMock = BuildIdentifiableMock();

            ISystemDataRepository sut = CreateSut(exceptionToThrow: exceptionToThrow);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.TranslationsForDomainObjectGet(identifiableDomainObjectMock));
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that TranslationsForDomainObjectGet throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        [Test]
        public void TestThatTranslationsForDomainObjectGetThrowsIntranetRepositoryExceptionWhenExceptionOccurs()
        {
            Exception exceptionToThrow = _fixture.Create<Exception>();

            IIdentifiable identifiableDomainObjectMock = BuildIdentifiableMock();

            ISystemDataRepository sut = CreateSut(exceptionToThrow: exceptionToThrow);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.TranslationsForDomainObjectGet(identifiableDomainObjectMock));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, exceptionToThrow, ExceptionMessage.RepositoryError, "TranslationsForDomainObjectGet", exceptionToThrow.Message);
        }

        /// <summary>
        /// Tests that TranslationInfoGetAll calls GetCollection on the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatTranslationInfoGetAllCallsGetCollectionOnFoodWasteDataProvider()
        {
            ISystemDataRepository sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            sut.TranslationInfoGetAll();

            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT TranslationInfoIdentifier,CultureName FROM TranslationInfos ORDER BY CultureName").Build();
            _foodWasteDataProviderMock.AssertWasCalled(m => m.GetCollection<TranslationInfoProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that TranslationInfoGetAll returns the result from the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatTranslationInfoGetAllReturnsResultFromFoodWasteDataProvider()
        {
            IEnumerable<TranslationInfoProxy> translationInfoProxyCollection = BuildTranslationInfoProxyCollection(_fixture);

            ISystemDataRepository sut = CreateSut(translationInfoProxyCollection: translationInfoProxyCollection);
            Assert.That(sut, Is.Not.Null);

            IEnumerable<ITranslationInfo> result = sut.TranslationInfoGetAll();
            // ReSharper disable PossibleMultipleEnumeration
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EqualTo(translationInfoProxyCollection));
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        /// Tests that TranslationInfoGetAll throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        [Test]
        public void TestThatTranslationInfoGetAllThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs()
        {
            IntranetRepositoryException exceptionToThrow = _fixture.Create<IntranetRepositoryException>();

            ISystemDataRepository sut = CreateSut(exceptionToThrow: exceptionToThrow);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.TranslationInfoGetAll());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that TranslationInfoGetAll throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        [Test]
        public void TestThatTranslationInfoGetAllThrowsIntranetRepositoryExceptionWhenExceptionOccurs()
        {
            Exception exceptionToThrow = _fixture.Create<Exception>();

            ISystemDataRepository sut = CreateSut(exceptionToThrow: exceptionToThrow);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.TranslationInfoGetAll());

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, exceptionToThrow, ExceptionMessage.RepositoryError, "TranslationInfoGetAll", exceptionToThrow.Message);
        }

        /// <summary>
        /// Creates an instance of the repository which can access system data for the food waste domain.
        /// </summary>
        /// <returns>Instance of the repository which can access system data for the food waste domain.</returns>
        private ISystemDataRepository CreateSut(IEnumerable<StorageTypeProxy> storageTypeProxyCollection = null, IEnumerable<FoodItemProxy> foodItemProxyCollection = null, IEnumerable<FoodGroupProxy> foodGroupProxyCollection = null, IEnumerable<ForeignKeyProxy> foreignKeyProxyCollection = null, IEnumerable<StaticTextProxy> staticTextProxyCollection = null, IEnumerable<DataProviderProxy> dataProviderProxyCollection = null, IEnumerable<TranslationProxy> translationProxyCollection = null, IEnumerable<TranslationInfoProxy> translationInfoProxyCollection = null, DataProviderProxy dataProviderProxy = null, Exception exceptionToThrow = null)
        {
            if (exceptionToThrow != null)
            {
                _foodWasteDataProviderMock.Stub(m => m.GetCollection<StorageTypeProxy>(Arg<MySqlCommand>.Is.Anything))
                    .Throw(exceptionToThrow)
                    .Repeat.Any();
                _foodWasteDataProviderMock.Stub(m => m.GetCollection<FoodItemProxy>(Arg<MySqlCommand>.Is.Anything))
                    .Throw(exceptionToThrow)
                    .Repeat.Any();
                _foodWasteDataProviderMock.Stub(m => m.GetCollection<FoodGroupProxy>(Arg<MySqlCommand>.Is.Anything))
                    .Throw(exceptionToThrow)
                    .Repeat.Any();
                _foodWasteDataProviderMock.Stub(m => m.GetCollection<ForeignKeyProxy>(Arg<MySqlCommand>.Is.Anything))
                    .Throw(exceptionToThrow)
                    .Repeat.Any();
                _foodWasteDataProviderMock.Stub(m => m.GetCollection<StaticTextProxy>(Arg<MySqlCommand>.Is.Anything))
                    .Throw(exceptionToThrow)
                    .Repeat.Any();
                _foodWasteDataProviderMock.Stub(m => m.GetCollection<DataProviderProxy>(Arg<MySqlCommand>.Is.Anything))
                    .Throw(exceptionToThrow)
                    .Repeat.Any();
                _foodWasteDataProviderMock.Stub(m => m.GetCollection<TranslationProxy>(Arg<MySqlCommand>.Is.Anything))
                    .Throw(exceptionToThrow)
                    .Repeat.Any();
                _foodWasteDataProviderMock.Stub(m => m.GetCollection<TranslationInfoProxy>(Arg<MySqlCommand>.Is.Anything))
                    .Throw(exceptionToThrow)
                    .Repeat.Any();
                _foodWasteDataProviderMock.Stub(m => m.Get(Arg<DataProviderProxy>.Is.Anything))
                    .Throw(exceptionToThrow)
                    .Repeat.Any();
            }
            else
            {
                _foodWasteDataProviderMock.Stub(m => m.GetCollection<StorageTypeProxy>(Arg<MySqlCommand>.Is.Anything))
                    .Return(storageTypeProxyCollection ?? new List<StorageTypeProxy>(0))
                    .Repeat.Any();
                _foodWasteDataProviderMock.Stub(m => m.GetCollection<FoodItemProxy>(Arg<MySqlCommand>.Is.Anything))
                    .Return(foodItemProxyCollection ?? new List<FoodItemProxy>(0))
                    .Repeat.Any();
                _foodWasteDataProviderMock.Stub(m => m.GetCollection<FoodGroupProxy>(Arg<MySqlCommand>.Is.Anything))
                    .Return(foodGroupProxyCollection ?? new List<FoodGroupProxy>(0))
                    .Repeat.Any();
                _foodWasteDataProviderMock.Stub(m => m.GetCollection<ForeignKeyProxy>(Arg<MySqlCommand>.Is.Anything))
                    .Return(foreignKeyProxyCollection ?? new List<ForeignKeyProxy>(0))
                    .Repeat.Any();
                _foodWasteDataProviderMock.Stub(m => m.GetCollection<StaticTextProxy>(Arg<MySqlCommand>.Is.Anything))
                    .Return(staticTextProxyCollection ?? new List<StaticTextProxy>(0))
                    .Repeat.Any();
                _foodWasteDataProviderMock.Stub(m => m.GetCollection<DataProviderProxy>(Arg<MySqlCommand>.Is.Anything))
                    .Return(dataProviderProxyCollection ?? new List<DataProviderProxy>(0))
                    .Repeat.Any();
                _foodWasteDataProviderMock.Stub(m => m.GetCollection<TranslationProxy>(Arg<MySqlCommand>.Is.Anything))
                    .Return(translationProxyCollection ?? new List<TranslationProxy>(0))
                    .Repeat.Any();
                _foodWasteDataProviderMock.Stub(m => m.GetCollection<TranslationInfoProxy>(Arg<MySqlCommand>.Is.Anything))
                    .Return(translationInfoProxyCollection ?? new List<TranslationInfoProxy>(0))
                    .Repeat.Any();
                _foodWasteDataProviderMock.Stub(m => m.Get(Arg<DataProviderProxy>.Is.Anything))
                    .Return(dataProviderProxy ?? new DataProviderProxy())
                    .Repeat.Any();
            }

            return new SystemDataRepository(_foodWasteDataProviderMock, _foodWasteObjectMapperMock);
        }

        /// <summary>
        /// Creates a collection of storage type proxies which can be used for unit testing.
        /// </summary>
        /// <returns>Collection of storage type proxies which can be used for unit testing</returns>
        private static IEnumerable<StorageTypeProxy> BuildStorageTypeProxyCollection(Fixture fixture)
        {
            ArgumentNullGuard.NotNull(fixture, nameof(fixture));

            return fixture.CreateMany<StorageTypeProxy>(5);
        }

        /// <summary>
        /// Creates a collection of food item proxies which can be used for unit testing.
        /// </summary>
        /// <returns>Collection of food item proxies which can be used for unit testing</returns>
        private static IEnumerable<FoodItemProxy> BuildFoodItemProxyCollection()
        {
            return new List<FoodItemProxy>
            {
                new FoodItemProxy(BuildFoodGroupMock()),
                new FoodItemProxy(BuildFoodGroupMock()),
                new FoodItemProxy(BuildFoodGroupMock())
            };
        }

        /// <summary>
        /// Creates a collection of food groups proxies which can be used for unit testing.
        /// </summary>
        /// <returns>Collection of food groups proxies which can be used for unit testing</returns>
        private static IEnumerable<FoodGroupProxy> BuildFoodGroupProxyCollection(Fixture fixture)
        {
            ArgumentNullGuard.NotNull(fixture, nameof(fixture));

            return new List<FoodGroupProxy>
            {
                fixture.Build<FoodGroupProxy>()
                    .With(m => m.Parent, null)
                    .Create(),
                fixture.Build<FoodGroupProxy>()
                    .With(m => m.Parent, null)
                    .Create(),
                fixture.Build<FoodGroupProxy>()
                    .With(m => m.Parent, null)
                    .Create()
            };
        }

        /// <summary>
        /// Creates a collection of foreign key proxies which can be used for unit testing.
        /// </summary>
        /// <returns>Collection of foreign key proxies which can be used for unit testing</returns>
        private static IEnumerable<ForeignKeyProxy> BuildForeignKeyProxyCollection(Fixture fixture)
        {
            ArgumentNullGuard.NotNull(fixture, nameof(fixture));

            return fixture.CreateMany<ForeignKeyProxy>(25).ToList();
        }

        /// <summary>
        /// Creates a collection of static text proxies which can be used for unit testing.
        /// </summary>
        /// <returns>Collection of static text proxies which can be used for unit testing</returns>
        private static IEnumerable<StaticTextProxy> BuildStaticTextProxyCollection(Fixture fixture)
        {
            ArgumentNullGuard.NotNull(fixture, nameof(fixture));

            return new List<StaticTextProxy>
            {
                new StaticTextProxy(fixture.Create<StaticTextType>(), Guid.NewGuid()),
                new StaticTextProxy(fixture.Create<StaticTextType>(), Guid.NewGuid()),
                new StaticTextProxy(fixture.Create<StaticTextType>(), Guid.NewGuid()),
                new StaticTextProxy(fixture.Create<StaticTextType>(), Guid.NewGuid()),
                new StaticTextProxy(fixture.Create<StaticTextType>(), Guid.NewGuid())
            };
        }

        /// <summary>
        /// Creates a collection of data provider proxies which can be used for unit testing.
        /// </summary>
        /// <returns>Collection of data provider proxies which can be used for unit testing</returns>
        private static IEnumerable<DataProviderProxy> BuildDataProviderProxyCollection(Fixture fixture)
        {
            ArgumentNullGuard.NotNull(fixture, nameof(fixture));

            return fixture.CreateMany<DataProviderProxy>(7).ToList();
        }

        /// <summary>
        /// Creates a collection of translation proxies which can be used for unit testing.
        /// </summary>
        /// <returns>Collection of translation proxies which can be used for unit testing</returns>
        private static IEnumerable<TranslationProxy> BuildTranslationProxyCollection(Fixture fixture)
        {
            ArgumentNullGuard.NotNull(fixture, nameof(fixture));

            return fixture.CreateMany<TranslationProxy>(15).ToList();
        }

        /// <summary>
        /// Creates a collection of translation proxies which can be used for unit testing.
        /// </summary>
        /// <returns>Collection of translation proxies which can be used for unit testing</returns>
        private static IEnumerable<TranslationInfoProxy> BuildTranslationInfoProxyCollection(Fixture fixture)
        {
            ArgumentNullGuard.NotNull(fixture, nameof(fixture));

            return fixture.CreateMany<TranslationInfoProxy>(3).ToList();
        }

        /// <summary>
        /// Creates a mockup for a food group.
        /// </summary>
        /// <returns>Mockup for a food group.</returns>
        private static IFoodGroup BuildFoodGroupMock()
        {
            return BuildFoodGroupMock(Guid.NewGuid());
        }

        /// <summary>
        /// Creates a mockup for a food group.
        /// </summary>
        /// <returns>Mockup for a food group.</returns>
        private static IFoodGroup BuildFoodGroupMock(Guid? identifier)
        {
            IFoodGroup foodGroupMock = MockRepository.GenerateMock<IFoodGroup>();
            foodGroupMock.Stub(m => m.Identifier)
                .Return(identifier)
                .Repeat.Any();
            return foodGroupMock;
        }

        /// <summary>
        /// Creates a mockup for a data provider.
        /// </summary>
        /// <returns>Mockup for a data provider.</returns>
        private static IDataProvider BuildDataProviderMock()
        {
            return BuildDataProviderMock(Guid.NewGuid());
        }

        /// <summary>
        /// Creates a mockup for a data provider.
        /// </summary>
        /// <returns>Mockup for a data provider.</returns>
        private static IDataProvider BuildDataProviderMock(Guid? identifier)
        {
            IDataProvider dataProviderMock = MockRepository.GenerateMock<IDataProvider>();
            dataProviderMock.Stub(m => m.Identifier)
                .Return(identifier)
                .Repeat.Any();
            return dataProviderMock;
        }

        /// <summary>
        /// Creates a mockup for an identifiable domain object.
        /// </summary>
        /// <returns>Mockup for an identifiable domain object.</returns>
        private static IIdentifiable BuildIdentifiableMock()
        {
            return BuildIdentifiableMock(Guid.NewGuid());
        }

        /// <summary>
        /// Creates a mockup for an identifiable domain object.
        /// </summary>
        /// <returns>Mockup for an identifiable domain object.</returns>
        private static IIdentifiable BuildIdentifiableMock(Guid? identifier)
        {
            IIdentifiable identifiableMock = MockRepository.GenerateMock<IIdentifiable>();
            identifiableMock.Stub(m => m.Identifier)
                .Return(identifier)
                .Repeat.Any();
            return identifiableMock;
        }
    }
}
