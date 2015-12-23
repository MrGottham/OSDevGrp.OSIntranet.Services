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
        /// <summary>
        /// Tests that the constructor initialize a repository which can access system data for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeSystemDataRepository()
        {
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException if the data provider which can access data in the food waste repository is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionIfFoodWasteDataProviderIsNull()
        {
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var exception = Assert.Throws<ArgumentNullException>(() => new SystemDataRepository(null, foodWasteObjectMapperMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("foodWasteDataProvider"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException if the object mapper which can map objects in the food waste domain is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionIfFoodWasteObjectMapperIsNull()
        {
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();

            var exception = Assert.Throws<ArgumentNullException>(() => new SystemDataRepository(foodWasteDataProviderMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("foodWasteObjectMapper"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that FoodItemGetAll calls GetCollection on the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatFoodItemGetAllCallsGetCollectionOnFoodWasteDataProvider()
        {
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<FoodItemProxy>(Arg<string>.Is.Anything))
                .Return(new List<FoodItemProxy>(0))
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            systemDataRepository.FoodItemGetAll();

            foodWasteDataProviderMock.AssertWasCalled(m => m.GetCollection<FoodItemProxy>(Arg<string>.Is.Equal("SELECT FoodItemIdentifier,IsActive FROM FoodItems")));
        }

        /// <summary>
        /// Tests that FoodItemGetAll returns the result from the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatFoodItemGetAllReturnsResultFromFoodWasteDataProvider()
        {
            var foodItemCollection = new List<FoodItemProxy>
            {
                new FoodItemProxy(MockRepository.GenerateMock<IFoodGroup>()),
                new FoodItemProxy(MockRepository.GenerateMock<IFoodGroup>()),
                new FoodItemProxy(MockRepository.GenerateMock<IFoodGroup>())
            };
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<FoodItemProxy>(Arg<string>.Is.Anything))
                .Return(foodItemCollection)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var result = systemDataRepository.FoodItemGetAll();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(foodItemCollection));
        }

        /// <summary>
        /// Tests that FoodItemGetAll throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        [Test]
        public void TestThatFoodItemGetAllThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs()
        {
            var fixture = new Fixture();

            var exceptionToThrow = fixture.Create<IntranetRepositoryException>();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<FoodItemProxy>(Arg<string>.Is.Anything))
                .Throw(exceptionToThrow)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => systemDataRepository.FoodItemGetAll());
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception, Is.EqualTo(exceptionToThrow));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that FoodItemGetAll throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        [Test]
        public void TestThatFoodItemGetAllThrowsIntranetRepositoryExceptionWhenExceptionOccurs()
        {
            var fixture = new Fixture();

            var exceptionToThrow = fixture.Create<Exception>();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<FoodItemProxy>(Arg<string>.Is.Anything))
                .Throw(exceptionToThrow)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => systemDataRepository.FoodItemGetAll());
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "FoodItemGetAll", exceptionToThrow.Message)));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that FoodItemGetAllForFoodGroup throws an ArgumentNullException when the food group is null.
        /// </summary>
        [Test]
        public void TestThatFoodItemGetAllForFoodGroupThrowsArgumentNullExceptionWhenFoodGroupIsNull()
        {
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => systemDataRepository.FoodItemGetAllForFoodGroup(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("foodGroup"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that FoodItemGetAllForFoodGroup throws an IntranetRepositoryException when the identifier on the food group has no value.
        /// </summary>
        [Test]
        public void TestThatFoodItemGetAllForFoodGroupThrowsIntranetRepositoryExceptionWhenIdentifierOnFoodGroupHasNoValue()
        {
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var foodGroupMock = MockRepository.GenerateMock<IFoodGroup>();
            foodGroupMock.Stub(m => m.Identifier)
                .Return(null)
                .Repeat.Any();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => systemDataRepository.FoodItemGetAllForFoodGroup(foodGroupMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, foodGroupMock.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that FoodItemGetAllForFoodGroup calls GetCollection on the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatFoodItemGetAllForFoodGroupCallsGetCollectionOnFoodWasteDataProvider()
        {
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<FoodItemProxy>(Arg<string>.Is.Anything))
                .Return(new List<FoodItemProxy>(0))
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var foodGroupMock = MockRepository.GenerateMock<IFoodGroup>();
            foodGroupMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            systemDataRepository.FoodItemGetAllForFoodGroup(foodGroupMock);

            // ReSharper disable PossibleInvalidOperationException
            foodWasteDataProviderMock.AssertWasCalled(m => m.GetCollection<FoodItemProxy>(Arg<string>.Is.Equal(string.Format("SELECT fi.FoodItemIdentifier AS FoodItemIdentifier,fi.IsActive AS IsActive FROM FoodItems AS fi, FoodItemGroups AS fig WHERE fig.FoodItemIdentifier=fi.FoodItemIdentifier AND fig.FoodGroupIdentifier='{0}'", foodGroupMock.Identifier.Value.ToString("D").ToUpper()))));
            // ReSharper restore PossibleInvalidOperationException
        }

        /// <summary>
        /// Tests that FoodItemGetAllForFoodGroup returns the result from the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatFoodItemGetAllForFoodGroupReturnsResultFromFoodWasteDataProvider()
        {
            var foodItemCollection = new List<FoodItemProxy>
            {
                new FoodItemProxy(MockRepository.GenerateMock<IFoodGroup>()),
                new FoodItemProxy(MockRepository.GenerateMock<IFoodGroup>()),
                new FoodItemProxy(MockRepository.GenerateMock<IFoodGroup>())
            };
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<FoodItemProxy>(Arg<string>.Is.Anything))
                .Return(foodItemCollection)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var foodGroupMock = MockRepository.GenerateMock<IFoodGroup>();
            foodGroupMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var result = systemDataRepository.FoodItemGetAllForFoodGroup(foodGroupMock);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(foodItemCollection));
        }

        /// <summary>
        /// Tests that FoodItemGetAllForFoodGroup throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        [Test]
        public void TestThatFoodItemGetAllForFoodGroupThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs()
        {
            var fixture = new Fixture();

            var exceptionToThrow = fixture.Create<IntranetRepositoryException>();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<FoodItemProxy>(Arg<string>.Is.Anything))
                .Throw(exceptionToThrow)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var foodGroupMock = MockRepository.GenerateMock<IFoodGroup>();
            foodGroupMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => systemDataRepository.FoodItemGetAllForFoodGroup(foodGroupMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception, Is.EqualTo(exceptionToThrow));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that FoodItemGetAllForFoodGroup throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        [Test]
        public void TestThatFoodItemGetAllForFoodGroupThrowsIntranetRepositoryExceptionWhenExceptionOccurs()
        {
            var fixture = new Fixture();

            var exceptionToThrow = fixture.Create<Exception>();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<FoodItemProxy>(Arg<string>.Is.Anything))
                .Throw(exceptionToThrow)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var foodGroupMock = MockRepository.GenerateMock<IFoodGroup>();
            foodGroupMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => systemDataRepository.FoodItemGetAllForFoodGroup(foodGroupMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "FoodItemGetAllForFoodGroup", exceptionToThrow.Message)));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that FoodItemGetByForeignKey throws an ArgumentNullException when the data provider is null.
        /// </summary>
        [Test]
        public void TestThatFoodItemGetByForeignKeyThrowsArgumentNullExceptionWhenDataProviderIsNull()
        {
            var fixture = new Fixture();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => systemDataRepository.FoodItemGetByForeignKey(null, fixture.Create<string>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("dataProvider"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that FoodItemGetByForeignKey throws an ArgumentNullException when the foreign key value is invalid.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestThatFoodItemGetByForeignKeyThrowsArgumentNullExceptionWhenForeignKeyValueIsInvalid(string invalidValue)
        {
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var dataProviderMock = MockRepository.GenerateMock<IDataProvider>();
            dataProviderMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => systemDataRepository.FoodItemGetByForeignKey(dataProviderMock, invalidValue));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("foreignKeyValue"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that FoodItemGetByForeignKey throws an IntranetRepositoryException when the identifier on the data provider has no value.
        /// </summary>
        [Test]
        public void TestThatFoodItemGetByForeignKeyThrowsIntranetRepositoryExceptionWhenIdentifierOnDataProviderHasNoValue()
        {
            var fixture = new Fixture();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var dataProviderMock = MockRepository.GenerateMock<IDataProvider>();
            dataProviderMock.Stub(m => m.Identifier)
                .Return(null)
                .Repeat.Any();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => systemDataRepository.FoodItemGetByForeignKey(dataProviderMock, fixture.Create<string>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, dataProviderMock.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that FoodItemGetByForeignKey calls GetCollection on the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatFoodItemGetByForeignKeyCallsGetCollectionOnFoodWasteDataProvider()
        {
            var fixture = new Fixture();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<FoodItemProxy>(Arg<string>.Is.Anything))
                .Return(new List<FoodItemProxy>(0))
                .Repeat.Any();

            var dataProviderMock = MockRepository.GenerateMock<IDataProvider>();
            dataProviderMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foreignKeyValue = fixture.Create<string>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            systemDataRepository.FoodItemGetByForeignKey(dataProviderMock, foreignKeyValue);

            // ReSharper disable PossibleInvalidOperationException
            foodWasteDataProviderMock.AssertWasCalled(m => m.GetCollection<FoodItemProxy>(Arg<string>.Is.Equal(string.Format("SELECT fi.FoodItemIdentifier AS FoodItemIdentifier,fi.IsActive AS IsActive FROM FoodItems AS fi, ForeignKeys AS fk WHERE fi.FoodItemIdentifier=fk.ForeignKeyForIdentifier AND fk.DataProviderIdentifier='{0}' AND fk.ForeignKeyForTypes LIKE '%{1}%' AND fk.ForeignKeyValue='{2}'", dataProviderMock.Identifier.Value.ToString("D").ToUpper(), typeof (IFoodItem).Name, foreignKeyValue))));
            // ReSharper restore PossibleInvalidOperationException
        }

        /// <summary>
        /// Tests that FoodItemGetByForeignKey returns null when no food item was found for the data provider and the foreign key value.
        /// </summary>
        [Test]
        public void TestThatFoodItemGetByForeignKeyReturnsNullWhenNoFoodItemWasFound()
        {
            var fixture = new Fixture();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<FoodItemProxy>(Arg<string>.Is.Anything))
                .Return(new List<FoodItemProxy>(0))
                .Repeat.Any();

            var dataProviderMock = MockRepository.GenerateMock<IDataProvider>();
            dataProviderMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var result = systemDataRepository.FoodItemGetByForeignKey(dataProviderMock, fixture.Create<string>());
            Assert.That(result, Is.Null);
        }

        /// <summary>
        /// Tests that FoodItemGetByForeignKey returns the food item which was found for the data provider and the foreign key value.
        /// </summary>
        [Test]
        public void TestThatFoodItemGetByForeignKeyReturnsFoodItemWhenItWasFound()
        {
            var fixture = new Fixture();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var foodItemProxy = new FoodItemProxy(MockRepository.GenerateMock<IFoodGroup>());
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<FoodItemProxy>(Arg<string>.Is.Anything))
                .Return(new List<FoodItemProxy> {foodItemProxy})
                .Repeat.Any();

            var dataProviderMock = MockRepository.GenerateMock<IDataProvider>();
            dataProviderMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var result = systemDataRepository.FoodItemGetByForeignKey(dataProviderMock, fixture.Create<string>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(foodItemProxy));
        }

        /// <summary>
        /// Tests that FoodItemGetByForeignKey throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        [Test]
        public void TestThatFoodItemGetByForeignKeyThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs()
        {
            var fixture = new Fixture();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var exceptionToThrow = fixture.Create<IntranetRepositoryException>();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<FoodItemProxy>(Arg<string>.Is.Anything))
                .Throw(exceptionToThrow)
                .Repeat.Any();

            var dataProviderMock = MockRepository.GenerateMock<IDataProvider>();
            dataProviderMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => systemDataRepository.FoodItemGetByForeignKey(dataProviderMock, fixture.Create<string>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception, Is.EqualTo(exceptionToThrow));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that FoodItemGetByForeignKey throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        [Test]
        public void TestThatFoodItemGetByForeignKeyThrowsIntranetRepositoryExceptionWhenExceptionOccurs()
        {
            var fixture = new Fixture();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var exceptionToThrow = fixture.Create<Exception>();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<FoodItemProxy>(Arg<string>.Is.Anything))
                .Throw(exceptionToThrow)
                .Repeat.Any();

            var dataProviderMock = MockRepository.GenerateMock<IDataProvider>();
            dataProviderMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => systemDataRepository.FoodItemGetByForeignKey(dataProviderMock, fixture.Create<string>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "FoodItemGetByForeignKey", exceptionToThrow.Message)));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that FoodGroupGetAll calls GetCollection on the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatFoodGroupGetAllCallsGetCollectionOnFoodWasteDataProvider()
        {
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<FoodGroupProxy>(Arg<string>.Is.Anything))
                .Return(new List<FoodGroupProxy>(0))
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            systemDataRepository.FoodGroupGetAll();

            foodWasteDataProviderMock.AssertWasCalled(m => m.GetCollection<FoodGroupProxy>(Arg<string>.Is.Equal("SELECT FoodGroupIdentifier,ParentIdentifier,IsActive FROM FoodGroups")));
        }

        /// <summary>
        /// Tests that FoodGroupGetAll returns the result from the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatFoodGroupGetAllReturnsResultFromFoodWasteDataProvider()
        {
            var fixture = new Fixture();

            var foodGroupCollection = new List<FoodGroupProxy>
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
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<FoodGroupProxy>(Arg<string>.Is.Anything))
                .Return(foodGroupCollection)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var result = systemDataRepository.FoodGroupGetAll();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(foodGroupCollection));
        }

        /// <summary>
        /// Tests that FoodGroupGetAll throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        [Test]
        public void TestThatFoodGroupGetAllThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs()
        {
            var fixture = new Fixture();

            var exceptionToThrow = fixture.Create<IntranetRepositoryException>();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<FoodGroupProxy>(Arg<string>.Is.Anything))
                .Throw(exceptionToThrow)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => systemDataRepository.FoodGroupGetAll());
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception, Is.EqualTo(exceptionToThrow));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that FoodGroupGetAll throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        [Test]
        public void TestThatFoodGroupGetAllGetThrowsIntranetRepositoryExceptionWhenExceptionOccurs()
        {
            var fixture = new Fixture();

            var exceptionToThrow = fixture.Create<Exception>();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<FoodGroupProxy>(Arg<string>.Is.Anything))
                .Throw(exceptionToThrow)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => systemDataRepository.FoodGroupGetAll());
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "FoodGroupGetAll", exceptionToThrow.Message)));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that FoodGroupGetAllOnRoot calls GetCollection on the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatFoodGroupGetAllOnRootCallsGetCollectionOnFoodWasteDataProvider()
        {
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<FoodGroupProxy>(Arg<string>.Is.Anything))
                .Return(new List<FoodGroupProxy>(0))
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            systemDataRepository.FoodGroupGetAllOnRoot();

            foodWasteDataProviderMock.AssertWasCalled(m => m.GetCollection<FoodGroupProxy>(Arg<string>.Is.Equal("SELECT FoodGroupIdentifier,ParentIdentifier,IsActive FROM FoodGroups WHERE ParentIdentifier IS NULL")));
        }

        /// <summary>
        /// Tests that FoodGroupGetAllOnRoot returns the result from the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatFoodGroupGetAllOnRootReturnsResultFromFoodWasteDataProvider()
        {
            var fixture = new Fixture();

            var foodGroupCollection = new List<FoodGroupProxy>
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
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<FoodGroupProxy>(Arg<string>.Is.Anything))
                .Return(foodGroupCollection)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var result = systemDataRepository.FoodGroupGetAllOnRoot();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(foodGroupCollection));
        }

        /// <summary>
        /// Tests that FoodGroupGetAllOnRoot throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        [Test]
        public void TestThatFoodGroupGetAllOnRootThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs()
        {
            var fixture = new Fixture();

            var exceptionToThrow = fixture.Create<IntranetRepositoryException>();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<FoodGroupProxy>(Arg<string>.Is.Anything))
                .Throw(exceptionToThrow)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => systemDataRepository.FoodGroupGetAllOnRoot());
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception, Is.EqualTo(exceptionToThrow));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that FoodGroupGetAllOnRoot throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        [Test]
        public void TestThatFoodGroupGetAllOnRootGetThrowsIntranetRepositoryExceptionWhenExceptionOccurs()
        {
            var fixture = new Fixture();

            var exceptionToThrow = fixture.Create<Exception>();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<FoodGroupProxy>(Arg<string>.Is.Anything))
                .Throw(exceptionToThrow)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => systemDataRepository.FoodGroupGetAllOnRoot());
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "FoodGroupGetAllOnRoot", exceptionToThrow.Message)));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that FoodGroupGetByForeignKey throws an ArgumentNullException when the data provider is null.
        /// </summary>
        [Test]
        public void TestThatFoodGroupGetByForeignKeyThrowsArgumentNullExceptionWhenDataProviderIsNull()
        {
            var fixture = new Fixture();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => systemDataRepository.FoodGroupGetByForeignKey(null, fixture.Create<string>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("dataProvider"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that FoodGroupGetByForeignKey throws an ArgumentNullException when the foreign key value is invalid.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestThatFoodGroupGetByForeignKeyThrowsArgumentNullExceptionWhenForeignKeyValueIsInvalid(string invalidValue)
        {
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var dataProviderMock = MockRepository.GenerateMock<IDataProvider>();
            dataProviderMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => systemDataRepository.FoodGroupGetByForeignKey(dataProviderMock, invalidValue));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("foreignKeyValue"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that FoodGroupGetByForeignKey throws an IntranetRepositoryException when the identifier on the data provider has no value.
        /// </summary>
        [Test]
        public void TestThatFoodGroupGetByForeignKeyThrowsIntranetRepositoryExceptionWhenIdentifierOnDataProviderHasNoValue()
        {
            var fixture = new Fixture();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var dataProviderMock = MockRepository.GenerateMock<IDataProvider>();
            dataProviderMock.Stub(m => m.Identifier)
                .Return(null)
                .Repeat.Any();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => systemDataRepository.FoodGroupGetByForeignKey(dataProviderMock, fixture.Create<string>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, dataProviderMock.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that FoodGroupGetByForeignKey calls GetCollection on the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatFoodGroupGetByForeignKeyGetCallsGetCollectionOnFoodWasteDataProvider()
        {
            var fixture = new Fixture();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<FoodGroupProxy>(Arg<string>.Is.Anything))
                .Return(new List<FoodGroupProxy>(0))
                .Repeat.Any();

            var dataProviderMock = MockRepository.GenerateMock<IDataProvider>();
            dataProviderMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foreignKeyValue = fixture.Create<string>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            systemDataRepository.FoodGroupGetByForeignKey(dataProviderMock, foreignKeyValue);
            
            // ReSharper disable PossibleInvalidOperationException
            foodWasteDataProviderMock.AssertWasCalled(m => m.GetCollection<FoodGroupProxy>(Arg<string>.Is.Equal(string.Format("SELECT fg.FoodGroupIdentifier AS FoodGroupIdentifier,fg.ParentIdentifier AS ParentIdentifier,fg.IsActive AS IsActive FROM FoodGroups AS fg, ForeignKeys AS fk WHERE fg.FoodGroupIdentifier=fk.ForeignKeyForIdentifier AND fk.DataProviderIdentifier='{0}' AND fk.ForeignKeyForTypes LIKE '%{1}%' AND fk.ForeignKeyValue='{2}'", dataProviderMock.Identifier.Value.ToString("D").ToUpper(), typeof (IFoodGroup).Name, foreignKeyValue))));
            // ReSharper restore PossibleInvalidOperationException
        }

        /// <summary>
        /// Tests that FoodGroupGetByForeignKey returns null when no food group was found for the data provider and the foreign key value.
        /// </summary>
        [Test]
        public void TestThatFoodGroupGetByForeignKeyGetReturnsNullWhenNoFoodGroupWasFound()
        {
            var fixture = new Fixture();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<FoodGroupProxy>(Arg<string>.Is.Anything))
                .Return(new List<FoodGroupProxy>(0))
                .Repeat.Any();

            var dataProviderMock = MockRepository.GenerateMock<IDataProvider>();
            dataProviderMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var result = systemDataRepository.FoodGroupGetByForeignKey(dataProviderMock, fixture.Create<string>());
            Assert.That(result, Is.Null);
        }

        /// <summary>
        /// Tests that FoodGroupGetByForeignKey returns the food group which was found for the data provider and the foreign key value.
        /// </summary>
        [Test]
        public void TestThatFoodGroupGetByForeignKeyGetReturnsFoodGroupWhenItWasFound()
        {
            var fixture = new Fixture();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var foodGroupProxy = fixture.Build<FoodGroupProxy>()
                .With(m => m.Parent, null)
                .Create();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<FoodGroupProxy>(Arg<string>.Is.Anything))
                .Return(new List<FoodGroupProxy> {foodGroupProxy})
                .Repeat.Any();

            var dataProviderMock = MockRepository.GenerateMock<IDataProvider>();
            dataProviderMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var result = systemDataRepository.FoodGroupGetByForeignKey(dataProviderMock, fixture.Create<string>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(foodGroupProxy));
        }

        /// <summary>
        /// Tests that FoodGroupGetByForeignKey throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        [Test]
        public void TestThatFoodGroupGetByForeignKeyGetThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs()
        {
            var fixture = new Fixture();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var exceptionToThrow = fixture.Create<IntranetRepositoryException>();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<FoodGroupProxy>(Arg<string>.Is.Anything))
                .Throw(exceptionToThrow)
                .Repeat.Any();

            var dataProviderMock = MockRepository.GenerateMock<IDataProvider>();
            dataProviderMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => systemDataRepository.FoodGroupGetByForeignKey(dataProviderMock, fixture.Create<string>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception, Is.EqualTo(exceptionToThrow));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that FoodGroupGetByForeignKey throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        [Test]
        public void TestThatFoodGroupGetByForeignKeyGetThrowsIntranetRepositoryExceptionWhenExceptionOccurs()
        {
            var fixture = new Fixture();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var exceptionToThrow = fixture.Create<Exception>();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<FoodGroupProxy>(Arg<string>.Is.Anything))
                .Throw(exceptionToThrow)
                .Repeat.Any();

            var dataProviderMock = MockRepository.GenerateMock<IDataProvider>();
            dataProviderMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => systemDataRepository.FoodGroupGetByForeignKey(dataProviderMock, fixture.Create<string>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "FoodGroupGetByForeignKey", exceptionToThrow.Message)));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that ForeignKeysForDomainObjectGet throws an ArgumentNullException when the identifiable domain object is null.
        /// </summary>
        [Test]
        public void TestThatForeignKeysForDomainObjectGetThrowsArgumentNullExceptionWhenIdentifiableDomainObjectIsNull()
        {
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => systemDataRepository.ForeignKeysForDomainObjectGet(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("identifiableDomainObject"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that ForeignKeysForDomainObjectGet throws an IntranetRepositoryException when the identifier on the identifiable domain object has no value.
        /// </summary>
        [Test]
        public void TestThatForeignKeysForDomainObjectGetThrowsIntranetRepositoryExceptionWhenIdentifierOnIdentifiableDomainObjectHasNoValue()
        {
            var identifiableDomainObjectMock = MockRepository.GenerateMock<IIdentifiable>();
            identifiableDomainObjectMock.Stub(m => m.Identifier)
                .Return(null)
                .Repeat.Any();

            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => systemDataRepository.ForeignKeysForDomainObjectGet(identifiableDomainObjectMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, identifiableDomainObjectMock.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that ForeignKeysForDomainObjectGet calls GetCollection on the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatForeignKeysForDomainObjectGetCallsGetCollectionOnFoodWasteDataProvider()
        {
            var identifiableDomainObjectMock = MockRepository.GenerateMock<IIdentifiable>();
            identifiableDomainObjectMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<ForeignKeyProxy>(Arg<string>.Is.Anything))
                .Return(new List<ForeignKeyProxy>(0))
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            systemDataRepository.ForeignKeysForDomainObjectGet(identifiableDomainObjectMock);

            // ReSharper disable PossibleInvalidOperationException
            foodWasteDataProviderMock.AssertWasCalled(m => m.GetCollection<ForeignKeyProxy>(Arg<string>.Is.Equal(string.Format("SELECT ForeignKeyIdentifier,DataProviderIdentifier,ForeignKeyForIdentifier,ForeignKeyForTypes,ForeignKeyValue FROM ForeignKeys WHERE ForeignKeyForIdentifier='{0}' ORDER BY DataProviderIdentifier,ForeignKeyValue", identifiableDomainObjectMock.Identifier.Value.ToString("D").ToUpper()))));
            // ReSharper restore PossibleInvalidOperationException
        }

        /// <summary>
        /// Tests that ForeignKeysForDomainObjectGet returns the result from the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatForeignKeysForDomainObjectGetReturnsResultFromFoodWasteDataProvider()
        {
            var fixture = new Fixture();

            var identifiableDomainObjectMock = MockRepository.GenerateMock<IIdentifiable>();
            identifiableDomainObjectMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foreignKeyCollection = fixture.CreateMany<ForeignKeyProxy>(25).ToList();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<ForeignKeyProxy>(Arg<string>.Is.Anything))
                .Return(foreignKeyCollection)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var result = systemDataRepository.ForeignKeysForDomainObjectGet(identifiableDomainObjectMock);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(foreignKeyCollection));
        }

        /// <summary>
        /// Tests that ForeignKeysForDomainObjectGet throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        [Test]
        public void TestThatForeignKeysForDomainObjectGetThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs()
        {
            var fixture = new Fixture();

            var identifiableDomainObjectMock = MockRepository.GenerateMock<IIdentifiable>();
            identifiableDomainObjectMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var exceptionToThrow = fixture.Create<IntranetRepositoryException>();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<ForeignKeyProxy>(Arg<string>.Is.Anything))
                .Throw(exceptionToThrow)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => systemDataRepository.ForeignKeysForDomainObjectGet(identifiableDomainObjectMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception, Is.EqualTo(exceptionToThrow));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that ForeignKeysForDomainObjectGet throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        [Test]
        public void TestThatForeignKeysForDomainObjectGetThrowsIntranetRepositoryExceptionWhenExceptionOccurs()
        {
            var fixture = new Fixture();

            var identifiableDomainObjectMock = MockRepository.GenerateMock<IIdentifiable>();
            identifiableDomainObjectMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var exceptionToThrow = fixture.Create<Exception>();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<ForeignKeyProxy>(Arg<string>.Is.Anything))
                .Throw(exceptionToThrow)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => systemDataRepository.ForeignKeysForDomainObjectGet(identifiableDomainObjectMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "ForeignKeysForDomainObjectGet", exceptionToThrow.Message)));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that StaticTextGetByStaticTextType calls GetCollection on the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatStaticTextGetByStaticTextTypeCallsGetCollectionOnFoodWasteDataProvider()
        {
            foreach (var staticTextTypeToTest in Enum.GetValues(typeof (StaticTextType)).Cast<StaticTextType>())
            {
                var staticTextProxy = new StaticTextProxy(staticTextTypeToTest, Guid.NewGuid());
                var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
                foodWasteDataProviderMock.Stub(m => m.GetCollection<StaticTextProxy>(Arg<string>.Is.Anything))
                    .Return(new List<StaticTextProxy> {staticTextProxy})
                    .Repeat.Any();

                var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

                var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
                Assert.That(systemDataRepository, Is.Not.Null);

                systemDataRepository.StaticTextGetByStaticTextType(staticTextTypeToTest);

                // ReSharper disable AccessToForEachVariableInClosure
                foodWasteDataProviderMock.AssertWasCalled(m => m.GetCollection<StaticTextProxy>(Arg<string>.Is.Equal(string.Format("SELECT StaticTextIdentifier,StaticTextType,SubjectTranslationIdentifier,BodyTranslationIdentifier FROM StaticTexts WHERE StaticTextType={0}", (int) staticTextTypeToTest))));
                // ReSharper restore AccessToForEachVariableInClosure
            }
        }

        /// <summary>
        /// Tests that StaticTextGetByStaticTextType returns the result from the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatStaticTextGetByStaticTextTypeReturnsResultFromFoodWasteDataProvider()
        {
            var fixture = new Fixture();

            var staticTextType = fixture.Create<StaticTextType>();
            var staticTextProxy = new StaticTextProxy(staticTextType, Guid.NewGuid());
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.GetCollection<StaticTextProxy>(Arg<string>.Is.Anything))
                .Return(new List<StaticTextProxy> {staticTextProxy})
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var systemDataRepository = new SystemDataRepository(foodWasteDataProviderMock, foodWasteObjectMapperMock);
            Assert.That(systemDataRepository, Is.Not.Null);

            var result = systemDataRepository.StaticTextGetByStaticTextType(staticTextType);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(staticTextProxy));
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

            foodWasteDataProviderMock.AssertWasCalled(m => m.GetCollection<DataProviderProxy>(Arg<string>.Is.Equal("SELECT DataProviderIdentifier,Name,DataSourceStatementIdentifier FROM DataProviders ORDER BY Name")));
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
    }
}
