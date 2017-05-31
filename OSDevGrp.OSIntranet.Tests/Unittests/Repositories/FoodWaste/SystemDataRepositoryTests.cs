using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Resources;
using Ploeh.AutoFixture;
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

        private IFoodWasteDataProvider _foodWasteDataProviderMock;
        private IFoodWasteObjectMapper _foodWasteObjectMapperMock;

        #endregion

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
        /// Tests that FoodItemGetAll calls GetCollection on the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatFoodItemGetAllCallsGetCollectionOnFoodWasteDataProvider()
        {
            ISystemDataRepository sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            sut.FoodItemGetAll();

            _foodWasteDataProviderMock.AssertWasCalled(m => m.GetCollection<FoodItemProxy>(Arg<string>.Is.Equal("SELECT FoodItemIdentifier,IsActive FROM FoodItems")));
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
            Fixture fixture = new Fixture();
            IntranetRepositoryException exceptionToThrow = fixture.Create<IntranetRepositoryException>();

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
            Fixture fixture = new Fixture();
            Exception exceptionToThrow = fixture.Create<Exception>();

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

            _foodWasteDataProviderMock.AssertWasCalled(m => m.GetCollection<FoodItemProxy>(Arg<string>.Is.Equal($"SELECT fi.FoodItemIdentifier AS FoodItemIdentifier,fi.IsActive AS IsActive FROM FoodItems AS fi, FoodItemGroups AS fig WHERE fig.FoodItemIdentifier=fi.FoodItemIdentifier AND fig.FoodGroupIdentifier='{identifier.ToString("D").ToUpper()}'")));
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
            Fixture fixture = new Fixture();
            IntranetRepositoryException exceptionToThrow = fixture.Create<IntranetRepositoryException>();

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
            Fixture fixture = new Fixture();
            Exception exceptionToThrow = fixture.Create<Exception>();

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
            Fixture fixture = new Fixture();

            ISystemDataRepository sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.FoodItemGetByForeignKey(null, fixture.Create<string>()));

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
            Fixture fixture = new Fixture();

            IDataProvider dataProviderMock = BuildDataProviderMock(null);

            ISystemDataRepository sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.FoodItemGetByForeignKey(dataProviderMock, fixture.Create<string>()));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, dataProviderMock.Identifier, "Identifier");
        }

        /// <summary>
        /// Tests that FoodItemGetByForeignKey calls GetCollection on the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatFoodItemGetByForeignKeyCallsGetCollectionOnFoodWasteDataProvider()
        {
            Fixture fixture = new Fixture();

            Guid identifer = Guid.NewGuid();
            IDataProvider dataProviderMock = BuildDataProviderMock(identifer);

            string foreignKeyValue = fixture.Create<string>();

            ISystemDataRepository sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            sut.FoodItemGetByForeignKey(dataProviderMock, foreignKeyValue);

            _foodWasteDataProviderMock.AssertWasCalled(m => m.GetCollection<FoodItemProxy>(Arg<string>.Is.Equal($"SELECT fi.FoodItemIdentifier AS FoodItemIdentifier,fi.IsActive AS IsActive FROM FoodItems AS fi, ForeignKeys AS fk WHERE fi.FoodItemIdentifier=fk.ForeignKeyForIdentifier AND fk.DataProviderIdentifier='{identifer.ToString("D").ToUpper()}' AND fk.ForeignKeyForTypes LIKE '%{typeof(IFoodItem).Name}%' AND fk.ForeignKeyValue='{foreignKeyValue}'")));
        }

        /// <summary>
        /// Tests that FoodItemGetByForeignKey returns null when no food item was found for the data provider and the foreign key value.
        /// </summary>
        [Test]
        public void TestThatFoodItemGetByForeignKeyReturnsNullWhenNoFoodItemWasFound()
        {
            Fixture fixture = new Fixture();

            IDataProvider dataProviderMock = BuildDataProviderMock();

            ISystemDataRepository sut = CreateSut(foodItemProxyCollection: new List<FoodItemProxy>(0));
            Assert.That(sut, Is.Not.Null);

            IFoodItem result = sut.FoodItemGetByForeignKey(dataProviderMock, fixture.Create<string>());
            Assert.That(result, Is.Null);
        }

        /// <summary>
        /// Tests that FoodItemGetByForeignKey returns the food item which was found for the data provider and the foreign key value.
        /// </summary>
        [Test]
        public void TestThatFoodItemGetByForeignKeyReturnsFoodItemWhenItWasFound()
        {
            Fixture fixture = new Fixture();

            IDataProvider dataProviderMock = BuildDataProviderMock();

            FoodItemProxy foodItemProxy = new FoodItemProxy(BuildFoodGroupMock());

            ISystemDataRepository sut = CreateSut(foodItemProxyCollection: new List<FoodItemProxy> {foodItemProxy});
            Assert.That(sut, Is.Not.Null);

            IFoodItem result = sut.FoodItemGetByForeignKey(dataProviderMock, fixture.Create<string>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(foodItemProxy));
        }

        /// <summary>
        /// Tests that FoodItemGetByForeignKey throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        [Test]
        public void TestThatFoodItemGetByForeignKeyThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs()
        {
            Fixture fixture = new Fixture();
            IntranetRepositoryException exceptionToThrow = fixture.Create<IntranetRepositoryException>();

            IDataProvider dataProviderMock = BuildDataProviderMock();

            ISystemDataRepository sut = CreateSut(exceptionToThrow: exceptionToThrow);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.FoodItemGetByForeignKey(dataProviderMock, fixture.Create<string>()));
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that FoodItemGetByForeignKey throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        [Test]
        public void TestThatFoodItemGetByForeignKeyThrowsIntranetRepositoryExceptionWhenExceptionOccurs()
        {
            Fixture fixture = new Fixture();
            Exception exceptionToThrow = fixture.Create<Exception>();

            IDataProvider dataProviderMock = BuildDataProviderMock();

            ISystemDataRepository sut = CreateSut(exceptionToThrow: exceptionToThrow);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.FoodItemGetByForeignKey(dataProviderMock, fixture.Create<string>()));

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

            _foodWasteDataProviderMock.AssertWasCalled(m => m.GetCollection<FoodGroupProxy>(Arg<string>.Is.Equal("SELECT FoodGroupIdentifier,ParentIdentifier,IsActive FROM FoodGroups")));
        }

        /// <summary>
        /// Tests that FoodGroupGetAll returns the result from the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatFoodGroupGetAllReturnsResultFromFoodWasteDataProvider()
        {
            Fixture fixture = new Fixture();

            IEnumerable<FoodGroupProxy> foodGroupProxyCollection = BuildFoodGroupProxyCollection(fixture);

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
            Fixture fixture = new Fixture();
            IntranetRepositoryException exceptionToThrow = fixture.Create<IntranetRepositoryException>();

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
            Fixture fixture = new Fixture();
            Exception exceptionToThrow = fixture.Create<Exception>();

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

            _foodWasteDataProviderMock.AssertWasCalled(m => m.GetCollection<FoodGroupProxy>(Arg<string>.Is.Equal("SELECT FoodGroupIdentifier,ParentIdentifier,IsActive FROM FoodGroups WHERE ParentIdentifier IS NULL")));
        }

        /// <summary>
        /// Tests that FoodGroupGetAllOnRoot returns the result from the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatFoodGroupGetAllOnRootReturnsResultFromFoodWasteDataProvider()
        {
            Fixture fixture = new Fixture();

            IEnumerable<FoodGroupProxy> foodGroupProxyCollection = BuildFoodGroupProxyCollection(fixture);

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
            Fixture fixture = new Fixture();
            IntranetRepositoryException exceptionToThrow = fixture.Create<IntranetRepositoryException>();

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
            Fixture fixture = new Fixture();
            Exception exceptionToThrow = fixture.Create<Exception>();

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
            Fixture fixture = new Fixture();

            ISystemDataRepository sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.FoodGroupGetByForeignKey(null, fixture.Create<string>()));

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
            Fixture fixture = new Fixture();

            IDataProvider dataProviderMock = BuildDataProviderMock(null);

            ISystemDataRepository sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.FoodGroupGetByForeignKey(dataProviderMock, fixture.Create<string>()));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, dataProviderMock.Identifier, "Identifier");
        }

        /// <summary>
        /// Tests that FoodGroupGetByForeignKey calls GetCollection on the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatFoodGroupGetByForeignKeyGetCallsGetCollectionOnFoodWasteDataProvider()
        {
            Fixture fixture = new Fixture();

            Guid identifier = Guid.NewGuid();
            IDataProvider dataProviderMock = BuildDataProviderMock(identifier);

            string foreignKeyValue = fixture.Create<string>();

            ISystemDataRepository sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            sut.FoodGroupGetByForeignKey(dataProviderMock, foreignKeyValue);
            
            _foodWasteDataProviderMock.AssertWasCalled(m => m.GetCollection<FoodGroupProxy>(Arg<string>.Is.Equal($"SELECT fg.FoodGroupIdentifier AS FoodGroupIdentifier,fg.ParentIdentifier AS ParentIdentifier,fg.IsActive AS IsActive FROM FoodGroups AS fg, ForeignKeys AS fk WHERE fg.FoodGroupIdentifier=fk.ForeignKeyForIdentifier AND fk.DataProviderIdentifier='{identifier.ToString("D").ToUpper()}' AND fk.ForeignKeyForTypes LIKE '%{typeof(IFoodGroup).Name}%' AND fk.ForeignKeyValue='{foreignKeyValue}'")));
        }

        /// <summary>
        /// Tests that FoodGroupGetByForeignKey returns null when no food group was found for the data provider and the foreign key value.
        /// </summary>
        [Test]
        public void TestThatFoodGroupGetByForeignKeyGetReturnsNullWhenNoFoodGroupWasFound()
        {
            Fixture fixture = new Fixture();

            IDataProvider dataProviderMock = BuildDataProviderMock();

            ISystemDataRepository sut = CreateSut(foodGroupProxyCollection: new List<FoodGroupProxy>(0));
            Assert.That(sut, Is.Not.Null);

            IFoodGroup result = sut.FoodGroupGetByForeignKey(dataProviderMock, fixture.Create<string>());
            Assert.That(result, Is.Null);
        }

        /// <summary>
        /// Tests that FoodGroupGetByForeignKey returns the food group which was found for the data provider and the foreign key value.
        /// </summary>
        [Test]
        public void TestThatFoodGroupGetByForeignKeyGetReturnsFoodGroupWhenItWasFound()
        {
            Fixture fixture = new Fixture();

            FoodGroupProxy foodGroupProxy = fixture.Build<FoodGroupProxy>()
                .With(m => m.Parent, null)
                .Create();

            IDataProvider dataProviderMock = BuildDataProviderMock();

            ISystemDataRepository sut = CreateSut(foodGroupProxyCollection: new List<FoodGroupProxy> {foodGroupProxy});
            Assert.That(sut, Is.Not.Null);

            IFoodGroup result = sut.FoodGroupGetByForeignKey(dataProviderMock, fixture.Create<string>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(foodGroupProxy));
        }

        /// <summary>
        /// Tests that FoodGroupGetByForeignKey throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        [Test]
        public void TestThatFoodGroupGetByForeignKeyGetThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs()
        {
            Fixture fixture = new Fixture();
            IntranetRepositoryException exceptionToThrow = fixture.Create<IntranetRepositoryException>();

            IDataProvider dataProviderMock = BuildDataProviderMock();

            ISystemDataRepository sut = CreateSut(exceptionToThrow: exceptionToThrow);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.FoodGroupGetByForeignKey(dataProviderMock, fixture.Create<string>()));
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that FoodGroupGetByForeignKey throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        [Test]
        public void TestThatFoodGroupGetByForeignKeyGetThrowsIntranetRepositoryExceptionWhenExceptionOccurs()
        {
            Fixture fixture = new Fixture();
            Exception exceptionToThrow = fixture.Create<Exception>();

            IDataProvider dataProviderMock = BuildDataProviderMock();

            ISystemDataRepository sut = CreateSut(exceptionToThrow: exceptionToThrow);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.FoodGroupGetByForeignKey(dataProviderMock, fixture.Create<string>()));

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

            _foodWasteDataProviderMock.AssertWasCalled(m => m.GetCollection<ForeignKeyProxy>(Arg<string>.Is.Equal($"SELECT ForeignKeyIdentifier,DataProviderIdentifier,ForeignKeyForIdentifier,ForeignKeyForTypes,ForeignKeyValue FROM ForeignKeys WHERE ForeignKeyForIdentifier='{identifier.ToString("D").ToUpper()}' ORDER BY DataProviderIdentifier,ForeignKeyValue")));
        }

        /// <summary>
        /// Tests that ForeignKeysForDomainObjectGet returns the result from the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatForeignKeysForDomainObjectGetReturnsResultFromFoodWasteDataProvider()
        {
            Fixture fixture = new Fixture();

            IIdentifiable identifiableDomainObjectMock = BuildIdentifiableMock();

            IEnumerable<ForeignKeyProxy> foreignKeyProxyCollection = BuildForeignKeyProxyCollection(fixture);

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
            Fixture fixture = new Fixture();
            IntranetRepositoryException exceptionToThrow = fixture.Create<IntranetRepositoryException>();

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
            Fixture fixture = new Fixture();
            Exception exceptionToThrow = fixture.Create<Exception>();

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
            foreach (StaticTextType staticTextTypeToTest in Enum.GetValues(typeof (StaticTextType)).Cast<StaticTextType>())
            {
                StaticTextProxy staticText = new StaticTextProxy(staticTextTypeToTest, Guid.NewGuid());
                IEnumerable<StaticTextProxy> staticTextProxyCollection = new List<StaticTextProxy> { staticText };

                ISystemDataRepository sut = CreateSut(staticTextProxyCollection: staticTextProxyCollection);
                Assert.That(sut, Is.Not.Null);

                sut.StaticTextGetByStaticTextType(staticTextTypeToTest);

                _foodWasteDataProviderMock.AssertWasCalled(m => m.GetCollection<StaticTextProxy>(Arg<string>.Is.Equal($"SELECT StaticTextIdentifier,StaticTextType,SubjectTranslationIdentifier,BodyTranslationIdentifier FROM StaticTexts WHERE StaticTextType={(int) staticTextTypeToTest}")));
            }
        }

        /// <summary>
        /// Tests that StaticTextGetByStaticTextType returns the result from the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatStaticTextGetByStaticTextTypeReturnsResultFromFoodWasteDataProvider()
        {
            Fixture fixture = new Fixture();
            StaticTextType staticTextType = fixture.Create<StaticTextType>();

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
            var fixture = new Fixture();

            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<StaticTextProxy>(Arg<string>.Is.Anything))
                .Return(new List<StaticTextProxy>(0))
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var staticTextType = fixture.Create<StaticTextType>();
            var exception = Assert.Throws<IntranetRepositoryException>(() => systemDataRepository.StaticTextGetByStaticTextType(staticTextType));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (IStaticText).Name, staticTextType)));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that StaticTextGetByStaticTextType throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        [Test]
        public void TestThatStaticTextGetByStaticTextTypeThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs()
        {
            var fixture = new Fixture();

            var exceptionToThrow = fixture.Create<IntranetRepositoryException>();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<StaticTextProxy>(Arg<string>.Is.Anything))
                .Throw(exceptionToThrow)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => systemDataRepository.StaticTextGetByStaticTextType(fixture.Create<StaticTextType>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception, Is.EqualTo(exceptionToThrow));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that StaticTextGetByStaticTextType throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        [Test]
        public void TestThatStaticTextGetByStaticTextTypeThrowsIntranetRepositoryExceptionWhenExceptionOccurs()
        {
            var fixture = new Fixture();

            var exceptionToThrow = fixture.Create<Exception>();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<StaticTextProxy>(Arg<string>.Is.Anything))
                .Throw(exceptionToThrow)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => systemDataRepository.StaticTextGetByStaticTextType(fixture.Create<StaticTextType>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "StaticTextGetByStaticTextType", exceptionToThrow.Message)));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that StaticTextGetAll calls GetCollection on the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatStaticTextGetAllCallsGetCollectionOnFoodWasteDataProvider()
        {
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<StaticTextProxy>(Arg<string>.Is.Anything))
                .Return(new List<StaticTextProxy>(0))
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            systemDataRepository.StaticTextGetAll();

            // ReSharper disable AccessToForEachVariableInClosure
            foodWasteDataProviderMock.AssertWasCalled(m => m.GetCollection<StaticTextProxy>(Arg<string>.Is.Equal("SELECT StaticTextIdentifier,StaticTextType,SubjectTranslationIdentifier,BodyTranslationIdentifier FROM StaticTexts ORDER BY StaticTextType")));
            // ReSharper restore AccessToForEachVariableInClosure
        }

        /// <summary>
        /// Tests that StaticTextGetAll returns the result from the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatStaticTextGetAllReturnsResultFromFoodWasteDataProvider()
        {
            var fixture = new Fixture();

            var staticTextCollection = fixture.CreateMany<StaticTextProxy>(7).ToList();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<StaticTextProxy>(Arg<string>.Is.Anything))
                .Return(staticTextCollection)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var result = systemDataRepository.StaticTextGetAll();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EqualTo(staticTextCollection));
        }

        /// <summary>
        /// Tests that StaticTextGetAll throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        [Test]
        public void TestThatStaticTextGetAllThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs()
        {
            var fixture = new Fixture();

            var exceptionToThrow = fixture.Create<IntranetRepositoryException>();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<StaticTextProxy>(Arg<string>.Is.Anything))
                .Throw(exceptionToThrow)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => systemDataRepository.StaticTextGetAll());
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception, Is.EqualTo(exceptionToThrow));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that StaticTextGetAll throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        [Test]
        public void TestThatStaticTextGetAllThrowsIntranetRepositoryExceptionWhenExceptionOccurs()
        {
            var fixture = new Fixture();

            var exceptionToThrow = fixture.Create<Exception>();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<StaticTextProxy>(Arg<string>.Is.Anything))
                .Throw(exceptionToThrow)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => systemDataRepository.StaticTextGetAll());
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "StaticTextGetAll", exceptionToThrow.Message)));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that DataProviderForFoodItemsGet calls Get on the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatDataProviderForFoodItemsGetCallsGetOnFoodWasteDataProvider()
        {
            var fixture = new Fixture();

            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.Get(Arg<DataProviderProxy>.Is.NotNull))
                .WhenCalled(e =>
                {
                    var dataProviderProxy = (DataProviderProxy) e.Arguments.ElementAt(0);
                    Assert.That(dataProviderProxy.Identifier, Is.Not.Null);
                    Assert.That(dataProviderProxy.Identifier.HasValue, Is.True);
                    // ReSharper disable PossibleInvalidOperationException
                    Assert.That(dataProviderProxy.Identifier.Value, Is.EqualTo(new Guid("5A1B9283-6406-44DF-91C5-F2FB83CC9A42")));
                    // ReSharper restore PossibleInvalidOperationException
                })
                .Return(fixture.Create<DataProviderProxy>())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            systemDataRepository.DataProviderForFoodItemsGet();

            foodWasteDataProviderMock.AssertWasCalled(m => m.Get(Arg<DataProviderProxy>.Is.NotNull));
        }

        /// <summary>
        /// Tests that DataProviderForFoodItemsGet returns the result from the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatDataProviderForFoodItemsGetReturnsResultFromFoodWasteDataProvider()
        {
            var fixture = new Fixture();

            var dataProvider = fixture.Create<DataProviderProxy>();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.Get(Arg<DataProviderProxy>.Is.Anything))
                .Return(dataProvider)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var result = systemDataRepository.DataProviderForFoodItemsGet();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(dataProvider));
        }

        /// <summary>
        /// Tests that DataProviderForFoodItemsGet throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        [Test]
        public void TestThatDataProviderForFoodItemsGetThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs()
        {
            var fixture = new Fixture();

            var exceptionToThrow = fixture.Create<IntranetRepositoryException>();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.Get(Arg<DataProviderProxy>.Is.Anything))
                .Throw(exceptionToThrow)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => systemDataRepository.DataProviderForFoodItemsGet());
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception, Is.EqualTo(exceptionToThrow));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that DataProviderForFoodItemsGet throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        [Test]
        public void TestThatDataProviderForFoodItemsGetThrowsIntranetRepositoryExceptionWhenExceptionOccurs()
        {
            var fixture = new Fixture();

            var exceptionToThrow = fixture.Create<Exception>();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.Get(Arg<DataProviderProxy>.Is.Anything))
                .Throw(exceptionToThrow)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => systemDataRepository.DataProviderForFoodItemsGet());
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "DataProviderForFoodItemsGet", exceptionToThrow.Message)));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that DataProviderForFoodGroupsGet calls Get on the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatDataProviderForFoodGroupsGetCallsGetOnFoodWasteDataProvider()
        {
            var fixture = new Fixture();

            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.Get(Arg<DataProviderProxy>.Is.NotNull))
                .WhenCalled(e =>
                {
                    var dataProviderProxy = (DataProviderProxy) e.Arguments.ElementAt(0);
                    Assert.That(dataProviderProxy.Identifier, Is.Not.Null);
                    Assert.That(dataProviderProxy.Identifier.HasValue, Is.True);
                    // ReSharper disable PossibleInvalidOperationException
                    Assert.That(dataProviderProxy.Identifier.Value, Is.EqualTo(new Guid("5A1B9283-6406-44DF-91C5-F2FB83CC9A42")));
                    // ReSharper restore PossibleInvalidOperationException
                })
                .Return(fixture.Create<DataProviderProxy>())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            systemDataRepository.DataProviderForFoodGroupsGet();

            foodWasteDataProviderMock.AssertWasCalled(m => m.Get(Arg<DataProviderProxy>.Is.NotNull));
        }

        /// <summary>
        /// Tests that DataProviderForFoodGroupsGet returns the result from the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatDataProviderForFoodGroupsGetReturnsResultFromFoodWasteDataProvider()
        {
            var fixture = new Fixture();

            var dataProvider = fixture.Create<DataProviderProxy>();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.Get(Arg<DataProviderProxy>.Is.Anything))
                .Return(dataProvider)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var result = systemDataRepository.DataProviderForFoodGroupsGet();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(dataProvider));
        }

        /// <summary>
        /// Tests that DataProviderForFoodGroupsGet throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        [Test]
        public void TestThatDataProviderForFoodGroupsGetThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs()
        {
            var fixture = new Fixture();

            var exceptionToThrow = fixture.Create<IntranetRepositoryException>();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.Get(Arg<DataProviderProxy>.Is.Anything))
                .Throw(exceptionToThrow)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => systemDataRepository.DataProviderForFoodGroupsGet());
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception, Is.EqualTo(exceptionToThrow));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that DataProviderForFoodGroupsGet throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        [Test]
        public void TestThatDataProviderForFoodGroupsGetThrowsIntranetRepositoryExceptionWhenExceptionOccurs()
        {
            var fixture = new Fixture();

            var exceptionToThrow = fixture.Create<Exception>();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.Get(Arg<DataProviderProxy>.Is.Anything))
                .Throw(exceptionToThrow)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => systemDataRepository.DataProviderForFoodGroupsGet());
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "DataProviderForFoodGroupsGet", exceptionToThrow.Message)));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that DataProviderGetAll calls GetCollection on the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatDataProviderGetAllCallsGetCollectionOnFoodWasteDataProvider()
        {
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<DataProviderProxy>(Arg<string>.Is.Anything))
                .Return(new List<DataProviderProxy>(0))
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            systemDataRepository.DataProviderGetAll();

            foodWasteDataProviderMock.AssertWasCalled(m => m.GetCollection<DataProviderProxy>(Arg<string>.Is.Equal("SELECT DataProviderIdentifier,Name,HandlesPayments,DataSourceStatementIdentifier FROM DataProviders ORDER BY Name")));
        }

        /// <summary>
        /// Tests that DataProviderGetAll returns the result from the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatDataProviderGetAllReturnsResultFromFoodWasteDataProvider()
        {
            var fixture = new Fixture();

            var dataProviderCollection = fixture.CreateMany<DataProviderProxy>(25).ToList();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<DataProviderProxy>(Arg<string>.Is.Anything))
                .Return(dataProviderCollection)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var result = systemDataRepository.DataProviderGetAll();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(dataProviderCollection));
        }

        /// <summary>
        /// Tests that DataProviderGetAll throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        [Test]
        public void TestThatDataProviderGetAllThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs()
        {
            var fixture = new Fixture();

            var exceptionToThrow = fixture.Create<IntranetRepositoryException>();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<DataProviderProxy>(Arg<string>.Is.Anything))
                .Throw(exceptionToThrow)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => systemDataRepository.DataProviderGetAll());
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception, Is.EqualTo(exceptionToThrow));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that DataProviderGetAll throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        [Test]
        public void TestThatDataProviderGetAllThrowsIntranetRepositoryExceptionWhenExceptionOccurs()
        {
            var fixture = new Fixture();

            var exceptionToThrow = fixture.Create<Exception>();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<DataProviderProxy>(Arg<string>.Is.Anything))
                .Throw(exceptionToThrow)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => systemDataRepository.DataProviderGetAll());
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "DataProviderGetAll", exceptionToThrow.Message)));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that DataProviderWhoHandlesPaymentsGetAll calls GetCollection on the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatDataProviderWhoHandlesPaymentsGetAllCallsGetCollectionOnFoodWasteDataProvider()
        {
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<DataProviderProxy>(Arg<string>.Is.Anything))
                .Return(new List<DataProviderProxy>(0))
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            systemDataRepository.DataProviderWhoHandlesPaymentsGetAll();

            foodWasteDataProviderMock.AssertWasCalled(m => m.GetCollection<DataProviderProxy>(Arg<string>.Is.Equal("SELECT DataProviderIdentifier,Name,HandlesPayments,DataSourceStatementIdentifier FROM DataProviders WHERE HandlesPayments=1 ORDER BY Name")));
        }

        /// <summary>
        /// Tests that DataProviderWhoHandlesPaymentsGetAll returns the result from the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatDataProviderWhoHandlesPaymentsGetAllReturnsResultFromFoodWasteDataProvider()
        {
            var fixture = new Fixture();

            var dataProviderCollection = fixture.CreateMany<DataProviderProxy>(25).ToList();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<DataProviderProxy>(Arg<string>.Is.Anything))
                .Return(dataProviderCollection)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var result = systemDataRepository.DataProviderWhoHandlesPaymentsGetAll();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(dataProviderCollection));
        }

        /// <summary>
        /// Tests that DataProviderWhoHandlesPaymentsGetAll throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        [Test]
        public void TestThatDataProviderWhoHandlesPaymentsGetAllThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs()
        {
            var fixture = new Fixture();

            var exceptionToThrow = fixture.Create<IntranetRepositoryException>();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<DataProviderProxy>(Arg<string>.Is.Anything))
                .Throw(exceptionToThrow)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => systemDataRepository.DataProviderWhoHandlesPaymentsGetAll());
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception, Is.EqualTo(exceptionToThrow));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that DataProviderWhoHandlesPaymentsGetAll throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        [Test]
        public void TestThatDataProviderWhoHandlesPaymentsGetAllThrowsIntranetRepositoryExceptionWhenExceptionOccurs()
        {
            var fixture = new Fixture();

            var exceptionToThrow = fixture.Create<Exception>();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<DataProviderProxy>(Arg<string>.Is.Anything))
                .Throw(exceptionToThrow)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => systemDataRepository.DataProviderWhoHandlesPaymentsGetAll());
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "DataProviderWhoHandlesPaymentsGetAll", exceptionToThrow.Message)));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that TranslationsForDomainObjectGet throws an ArgumentNullException when the identifiable domain object is null.
        /// </summary>
        [Test]
        public void TestThatTranslationsForDomainObjectGetThrowsArgumentNullExceptionWhenIdentifiableDomainObjectIsNull()
        {
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => systemDataRepository.TranslationsForDomainObjectGet(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("identifiableDomainObject"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that TranslationsForDomainObjectGet throws an IntranetRepositoryException when the identifier on the identifiable domain object has no value.
        /// </summary>
        [Test]
        public void TestThatTranslationsForDomainObjectGetThrowsIntranetRepositoryExceptionWhenIdentifierOnIdentifiableDomainObjectHasNoValue()
        {
            var identifiableDomainObjectMock = MockRepository.GenerateMock<IIdentifiable>();
            identifiableDomainObjectMock.Stub(m => m.Identifier)
                .Return(null)
                .Repeat.Any();

            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => systemDataRepository.TranslationsForDomainObjectGet(identifiableDomainObjectMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, identifiableDomainObjectMock.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that TranslationsForDomainObjectGet calls GetCollection on the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatTranslationsForDomainObjectGetCallsGetCollectionOnFoodWasteDataProvider()
        {
            var identifiableDomainObjectMock = MockRepository.GenerateMock<IIdentifiable>();
            identifiableDomainObjectMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<TranslationProxy>(Arg<string>.Is.Anything))
                .Return(new List<TranslationProxy>(0))
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            systemDataRepository.TranslationsForDomainObjectGet(identifiableDomainObjectMock);

            // ReSharper disable PossibleInvalidOperationException
            foodWasteDataProviderMock.AssertWasCalled(m => m.GetCollection<TranslationProxy>(Arg<string>.Is.Equal(string.Format("SELECT t.TranslationIdentifier AS TranslationIdentifier,t.OfIdentifier AS OfIdentifier,ti.TranslationInfoIdentifier AS InfoIdentifier,ti.CultureName AS CultureName,t.Value AS Value FROM Translations AS t, TranslationInfos AS ti WHERE t.OfIdentifier='{0}' AND ti.TranslationInfoIdentifier=t.InfoIdentifier ORDER BY CultureName", identifiableDomainObjectMock.Identifier.Value.ToString("D").ToUpper()))));
            // ReSharper restore PossibleInvalidOperationException
        }

        /// <summary>
        /// Tests that TranslationsForDomainObjectGet returns the result from the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatTranslationsForDomainObjectGetReturnsResultFromFoodWasteDataProvider()
        {
            var fixture = new Fixture();

            var identifiableDomainObjectMock = MockRepository.GenerateMock<IIdentifiable>();
            identifiableDomainObjectMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var translationCollection = fixture.CreateMany<TranslationProxy>(25).ToList();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<TranslationProxy>(Arg<string>.Is.Anything))
                .Return(translationCollection)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var result = systemDataRepository.TranslationsForDomainObjectGet(identifiableDomainObjectMock);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(translationCollection));
        }

        /// <summary>
        /// Tests that TranslationsForDomainObjectGet throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        [Test]
        public void TestThatTranslationsForDomainObjectGetThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs()
        {
            var fixture = new Fixture();

            var identifiableDomainObjectMock = MockRepository.GenerateMock<IIdentifiable>();
            identifiableDomainObjectMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var exceptionToThrow = fixture.Create<IntranetRepositoryException>();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<TranslationProxy>(Arg<string>.Is.Anything))
                .Throw(exceptionToThrow)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => systemDataRepository.TranslationsForDomainObjectGet(identifiableDomainObjectMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception, Is.EqualTo(exceptionToThrow));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that TranslationsForDomainObjectGet throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        [Test]
        public void TestThatTranslationsForDomainObjectGetThrowsIntranetRepositoryExceptionWhenExceptionOccurs()
        {
            var fixture = new Fixture();

            var identifiableDomainObjectMock = MockRepository.GenerateMock<IIdentifiable>();
            identifiableDomainObjectMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var exceptionToThrow = fixture.Create<Exception>();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<TranslationProxy>(Arg<string>.Is.Anything))
                .Throw(exceptionToThrow)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => systemDataRepository.TranslationsForDomainObjectGet(identifiableDomainObjectMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "TranslationsForDomainObjectGet", exceptionToThrow.Message)));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that TranslationInfoGetAll calls GetCollection on the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatTranslationInfoGetAllCallsGetCollectionOnFoodWasteDataProvider()
        {
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<TranslationInfoProxy>(Arg<string>.Is.Anything))
                .Return(new List<TranslationInfoProxy>(0))
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            systemDataRepository.TranslationInfoGetAll();

            foodWasteDataProviderMock.AssertWasCalled(m => m.GetCollection<TranslationInfoProxy>(Arg<string>.Is.Equal("SELECT TranslationInfoIdentifier,CultureName FROM TranslationInfos ORDER BY CultureName")));
        }

        /// <summary>
        /// Tests that TranslationInfoGetAll returns the result from the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatTranslationInfoGetAllReturnsResultFromFoodWasteDataProvider()
        {
            var fixture = new Fixture();

            var translationInfoCollection = fixture.CreateMany<TranslationInfoProxy>(25).ToList();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<TranslationInfoProxy>(Arg<string>.Is.Anything))
                .Return(translationInfoCollection)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var result = systemDataRepository.TranslationInfoGetAll();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(translationInfoCollection));
        }

        /// <summary>
        /// Tests that TranslationInfoGetAll throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        [Test]
        public void TestThatTranslationInfoGetAllThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs()
        {
            var fixture = new Fixture();

            var exceptionToThrow = fixture.Create<IntranetRepositoryException>();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<TranslationInfoProxy>(Arg<string>.Is.Anything))
                .Throw(exceptionToThrow)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => systemDataRepository.TranslationInfoGetAll());
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception, Is.EqualTo(exceptionToThrow));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that TranslationInfoGetAll throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        [Test]
        public void TestThatTranslationInfoGetAllThrowsIntranetRepositoryExceptionWhenExceptionOccurs()
        {
            var fixture = new Fixture();

            var exceptionToThrow = fixture.Create<Exception>();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<TranslationInfoProxy>(Arg<string>.Is.Anything))
                .Throw(exceptionToThrow)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => systemDataRepository.TranslationInfoGetAll());
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "TranslationInfoGetAll", exceptionToThrow.Message)));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Creates an instance of the repository which can access system data for the food waste domain.
        /// </summary>
        /// <returns>Instance of the repository which can access system data for the food waste domain.</returns>
        private ISystemDataRepository CreateSut(IEnumerable<FoodItemProxy> foodItemProxyCollection = null, IEnumerable<FoodGroupProxy> foodGroupProxyCollection = null, IEnumerable<ForeignKeyProxy> foreignKeyProxyCollection = null, IEnumerable<StaticTextProxy> staticTextProxyCollection = null, Exception exceptionToThrow = null)
        {
            _foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            if (exceptionToThrow != null)
            {
                _foodWasteDataProviderMock.Stub(m => m.GetCollection<FoodItemProxy>(Arg<string>.Is.Anything))
                    .Throw(exceptionToThrow)
                    .Repeat.Any();
                _foodWasteDataProviderMock.Stub(m => m.GetCollection<FoodGroupProxy>(Arg<string>.Is.Anything))
                    .Throw(exceptionToThrow)
                    .Repeat.Any();
                _foodWasteDataProviderMock.Stub(m => m.GetCollection<ForeignKeyProxy>(Arg<string>.Is.Anything))
                    .Throw(exceptionToThrow)
                    .Repeat.Any();
                _foodWasteDataProviderMock.Stub(m => m.GetCollection<StaticTextProxy>(Arg<string>.Is.Anything))
                    .Throw(exceptionToThrow)
                    .Repeat.Any();
            }
            else
            {
                _foodWasteDataProviderMock.Stub(m => m.GetCollection<FoodItemProxy>(Arg<string>.Is.Anything))
                    .Return(foodItemProxyCollection ?? new List<FoodItemProxy>(0))
                    .Repeat.Any();
                _foodWasteDataProviderMock.Stub(m => m.GetCollection<FoodGroupProxy>(Arg<string>.Is.Anything))
                    .Return(foodGroupProxyCollection ?? new List<FoodGroupProxy>(0))
                    .Repeat.Any();
                _foodWasteDataProviderMock.Stub(m => m.GetCollection<ForeignKeyProxy>(Arg<string>.Is.Anything))
                    .Return(foreignKeyProxyCollection ?? new List<ForeignKeyProxy>(0))
                    .Repeat.Any();
                _foodWasteDataProviderMock.Stub(m => m.GetCollection<StaticTextProxy>(Arg<string>.Is.Anything))
                    .Return(staticTextProxyCollection ?? new List<StaticTextProxy>(0))
                    .Repeat.Any();
            }

            _foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            return new SystemDataRepository(_foodWasteDataProviderMock, _foodWasteObjectMapperMock);
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
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

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
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return fixture.CreateMany<ForeignKeyProxy>(25).ToList();
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
