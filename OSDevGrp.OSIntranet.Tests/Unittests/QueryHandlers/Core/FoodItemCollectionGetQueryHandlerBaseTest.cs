using System;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.QueryHandlers.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste;
using AutoFixture;
using Rhino.Mocks;
using System.Globalization;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers.Core
{
    /// <summary>
    /// Tests the functionality which handles a query for getting the collection of food items.
    /// </summary>
    [TestFixture]
    public class FoodItemCollectionGetQueryHandlerBaseTest
    {
        /// <summary>
        /// Private class for testing the functionality which handles a query for getting the collection of food items.
        /// </summary>
        private class MyFoodItemCollectionGetQueryHandler : FoodItemCollectionGetQueryHandlerBase<IView>
        {
            #region Private variables

            private readonly bool _onlyActive;

            #endregion

            #region Constructor

            /// <summary>
            /// Creates a private class for testing the functionality which handles a query for getting the collection of food items.
            /// </summary>
            /// <param name="systemDataRepository">Implementation for a repository which can access system data in the food waste domain.</param>
            /// <param name="foodWasteObjectMapper">Implementation for a object mapper which can map domain object in the food waste domain.</param>
            /// <param name="onlyActive">Indication of whether only active food items should be included.</param>
            public MyFoodItemCollectionGetQueryHandler(ISystemDataRepository systemDataRepository, IFoodWasteObjectMapper foodWasteObjectMapper, bool onlyActive)
                : base(systemDataRepository, foodWasteObjectMapper)
            {
                _onlyActive = onlyActive;
            }

            #endregion

            #region Properties

            /// <summary>
            /// Gets whether only active food items should be included.
            /// </summary>
            protected override bool OnlyActive
            {
                get { return _onlyActive; }
            }

            #endregion

            #region Methods

            /// <summary>
            /// Returns whether only active food items should be included.
            /// </summary>
            /// <returns>Indication of whether only active food items should be included.</returns>
            public bool GetOnlyActive()
            {
                return OnlyActive;
            }

            #endregion
        }

        /// <summary>
        /// Tests that the constructor initialize functionality which handles a query for getting the collection of food items.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatConstructorInitializeFoodItemCollectionGetQueryHandlerBase(bool activeOnly)
        {
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var foodItemCollectionGetQueryHandlerBase = new MyFoodItemCollectionGetQueryHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, activeOnly);
            Assert.That(foodItemCollectionGetQueryHandlerBase, Is.Not.Null);
            Assert.That(foodItemCollectionGetQueryHandlerBase.GetOnlyActive(), Is.EqualTo(activeOnly));
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the repository which can access system data in the food waste domain is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenSystemDataRepositoryIsNull()
        {
            var fixture = new Fixture();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyFoodItemCollectionGetQueryHandler(null, foodWasteObjectMapperMock, fixture.Create<bool>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("systemDataRepository"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the object mapper which can map domain object in the food waste domain is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenFoodWasteObjectMapperIsNull()
        {
            var fixture = new Fixture();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyFoodItemCollectionGetQueryHandler(systemDataRepositoryMock, null, fixture.Create<bool>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("foodWasteObjectMapper"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Query throws an ArgumentNullException when the query for getting the tree of food groups is null.
        /// </summary>
        [Test]
        public void TestThatQueryThrowsArgumentNullExceptionWhenQueryIsNull()
        {
            var fixture = new Fixture();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var foodItemCollectionGetQueryHandlerBase = new MyFoodItemCollectionGetQueryHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, fixture.Create<bool>());
            Assert.That(foodItemCollectionGetQueryHandlerBase, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodItemCollectionGetQueryHandlerBase.Query(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("query"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Query calls Get for the identifier of the translation informations which should be used for translation on the repository which can access system data in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatQueryCallsGetForTranslationInfoIdentifierOnSystemDataRepository()
        {
            var fixture = new Fixture();

            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.DataProviderForFoodItemsGet())
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodItemGetAll())
                .Return(DomainObjectMockBuilder.BuildFoodItemMockCollection())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IFoodItemCollection, IView>(Arg<IFoodItemCollection>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(MockRepository.GenerateMock<IView>())
                .Repeat.Any();

            var foodItemCollectionGetQueryHandlerBase = new MyFoodItemCollectionGetQueryHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, fixture.Create<bool>());
            Assert.That(foodItemCollectionGetQueryHandlerBase, Is.Not.Null);

            var foodItemCollectionGetQuery = fixture.Build<FoodItemCollectionGetQuery>()
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .With(m => m.FoodGroupIdentifier, null)
                .Create();

            foodItemCollectionGetQueryHandlerBase.Query(foodItemCollectionGetQuery);

            systemDataRepositoryMock.AssertWasCalled(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Equal(foodItemCollectionGetQuery.TranslationInfoIdentifier)));
        }

        /// <summary>
        /// Tests that Query calls DataProviderForFoodItemsGet on the repository which can access system data in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatQueryCallsDataProviderForFoodItemsGetOnSystemDataRepository()
        {
            var fixture = new Fixture();

            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.DataProviderForFoodItemsGet())
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodItemGetAll())
                .Return(DomainObjectMockBuilder.BuildFoodItemMockCollection())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IFoodItemCollection, IView>(Arg<IFoodItemCollection>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(MockRepository.GenerateMock<IView>())
                .Repeat.Any();

            var foodItemCollectionGetQueryHandlerBase = new MyFoodItemCollectionGetQueryHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, fixture.Create<bool>());
            Assert.That(foodItemCollectionGetQueryHandlerBase, Is.Not.Null);

            var foodItemCollectionGetQuery = fixture.Build<FoodItemCollectionGetQuery>()
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .With(m => m.FoodGroupIdentifier, null)
                .Create();

            foodItemCollectionGetQueryHandlerBase.Query(foodItemCollectionGetQuery);

            systemDataRepositoryMock.AssertWasCalled(m => m.DataProviderForFoodItemsGet());
        }

        /// <summary>
        /// Tests that Query calls Get for the identifier of the food group on which to get the belonging food items on the repository which can access system data in the food waste domain when it is not null.
        /// </summary>
        [Test]
        public void TestThatQueryCallsGetForFoodGroupIdentifierOnSystemDataRepositoryWhenFoodGroupIdentifierIsNotNull()
        {
            var fixture = new Fixture();

            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.DataProviderForFoodItemsGet())
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<IFoodGroup>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodGroupMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodItemGetAllForFoodGroup(Arg<IFoodGroup>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodItemMockCollection())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IFoodItemCollection, IView>(Arg<IFoodItemCollection>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(MockRepository.GenerateMock<IView>())
                .Repeat.Any();

            var foodItemCollectionGetQueryHandlerBase = new MyFoodItemCollectionGetQueryHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, fixture.Create<bool>());
            Assert.That(foodItemCollectionGetQueryHandlerBase, Is.Not.Null);

            var foodItemCollectionGetQuery = fixture.Build<FoodItemCollectionGetQuery>()
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .With(m => m.FoodGroupIdentifier, Guid.NewGuid())
                .Create();

            foodItemCollectionGetQueryHandlerBase.Query(foodItemCollectionGetQuery);

            systemDataRepositoryMock.AssertWasCalled(m => m.Get<IFoodGroup>(Arg<Guid>.Is.Equal(foodItemCollectionGetQuery.FoodGroupIdentifier)));
        }

        /// <summary>
        /// Tests that Query does not call Get for the identifier of the food group on which to get the belonging food items on the repository which can access system data in the food waste domain when it is null.
        /// </summary>
        [Test]
        public void TestThatQueryDoesNotCallGetForFoodGroupIdentifierOnSystemDataRepositoryWhenFoodGroupIdentifierIsNull()
        {
            var fixture = new Fixture();

            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.DataProviderForFoodItemsGet())
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodItemGetAll())
                .Return(DomainObjectMockBuilder.BuildFoodItemMockCollection())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IFoodItemCollection, IView>(Arg<IFoodItemCollection>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(MockRepository.GenerateMock<IView>())
                .Repeat.Any();

            var foodItemCollectionGetQueryHandlerBase = new MyFoodItemCollectionGetQueryHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, fixture.Create<bool>());
            Assert.That(foodItemCollectionGetQueryHandlerBase, Is.Not.Null);

            var foodItemCollectionGetQuery = fixture.Build<FoodItemCollectionGetQuery>()
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .With(m => m.FoodGroupIdentifier, null)
                .Create();

            foodItemCollectionGetQueryHandlerBase.Query(foodItemCollectionGetQuery);

            systemDataRepositoryMock.AssertWasNotCalled(m => m.Get<IFoodGroup>(Arg<Guid>.Is.Anything));
        }

        /// <summary>
        /// Tests that Query calls FoodItemGetAllForFoodGroup on the repository which can access system data in the food waste domain when the food group identifier in the query is not null.
        /// </summary>
        [Test]
        public void TestThatQueryCallsFoodItemGetAllForFoodGroupOnSystemDataRepositoryWhenFoodGroupIdentifierIsNotNull()
        {
            var fixture = new Fixture();

            var foodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.DataProviderForFoodItemsGet())
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.Get<IFoodGroup>(Arg<Guid>.Is.Anything))
                .Return(foodGroupMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodItemGetAllForFoodGroup(Arg<IFoodGroup>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildFoodItemMockCollection())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IFoodItemCollection, IView>(Arg<IFoodItemCollection>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(MockRepository.GenerateMock<IView>())
                .Repeat.Any();

            var foodItemCollectionGetQueryHandlerBase = new MyFoodItemCollectionGetQueryHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, fixture.Create<bool>());
            Assert.That(foodItemCollectionGetQueryHandlerBase, Is.Not.Null);

            var foodItemCollectionGetQuery = fixture.Build<FoodItemCollectionGetQuery>()
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .With(m => m.FoodGroupIdentifier, Guid.NewGuid())
                .Create();

            foodItemCollectionGetQueryHandlerBase.Query(foodItemCollectionGetQuery);

            systemDataRepositoryMock.AssertWasCalled(m => m.FoodItemGetAllForFoodGroup(Arg<IFoodGroup>.Is.Equal(foodGroupMock)));
        }

        /// <summary>
        /// Tests that Query calls FoodItemGetAll on the repository which can access system data in the food waste domain when the food group identifier in the query is null.
        /// </summary>
        [Test]
        public void TestThatQueryCallsFoodItemGetAllOnSystemDataRepositoryWhenFoodGroupIdentifierIsNull()
        {
            var fixture = new Fixture();

            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.DataProviderForFoodItemsGet())
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodItemGetAll())
                .Return(DomainObjectMockBuilder.BuildFoodItemMockCollection())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IFoodItemCollection, IView>(Arg<IFoodItemCollection>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(MockRepository.GenerateMock<IView>())
                .Repeat.Any();

            var foodItemCollectionGetQueryHandlerBase = new MyFoodItemCollectionGetQueryHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, fixture.Create<bool>());
            Assert.That(foodItemCollectionGetQueryHandlerBase, Is.Not.Null);

            var foodItemCollectionGetQuery = fixture.Build<FoodItemCollectionGetQuery>()
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .With(m => m.FoodGroupIdentifier, null)
                .Create();

            foodItemCollectionGetQueryHandlerBase.Query(foodItemCollectionGetQuery);

            systemDataRepositoryMock.AssertWasCalled(m => m.FoodItemGetAll());
        }

        /// <summary>
        /// Tests that Query calls Translate on the data provider who provides the food items.
        /// </summary>
        [Test]
        public void TestThatQueryCallsTranslateOnDataProviderForFoodItems()
        {
            var fixture = new Fixture();

            var translationInfoMock = DomainObjectMockBuilder.BuildTranslationInfoMock();
            var dataProviderMock = DomainObjectMockBuilder.BuildDataProviderMock();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(translationInfoMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.DataProviderForFoodItemsGet())
                .Return(dataProviderMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodItemGetAll())
                .Return(DomainObjectMockBuilder.BuildFoodItemMockCollection())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IFoodItemCollection, IView>(Arg<IFoodItemCollection>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(MockRepository.GenerateMock<IView>())
                .Repeat.Any();

            var foodItemCollectionGetQueryHandlerBase = new MyFoodItemCollectionGetQueryHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, fixture.Create<bool>());
            Assert.That(foodItemCollectionGetQueryHandlerBase, Is.Not.Null);

            var foodItemCollectionGetQuery = fixture.Build<FoodItemCollectionGetQuery>()
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .With(m => m.FoodGroupIdentifier, null)
                .Create();

            foodItemCollectionGetQueryHandlerBase.Query(foodItemCollectionGetQuery);

            dataProviderMock.AssertWasCalled(m => m.Translate(Arg<CultureInfo>.Is.Equal(translationInfoMock.CultureInfo)));
        }

        /// <summary>
        /// Tests that Query calls Map on the object mapper which can map domain object in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatQueryCallsMapOnFoodWasteObjectMapper()
        {
            var fixture = new Fixture();

            var translationInfoMock = DomainObjectMockBuilder.BuildTranslationInfoMock();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(translationInfoMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.DataProviderForFoodItemsGet())
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodItemGetAll())
                .Return(DomainObjectMockBuilder.BuildFoodItemMockCollection())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IFoodItemCollection, IView>(Arg<IFoodItemCollection>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(MockRepository.GenerateMock<IView>())
                .Repeat.Any();

            var foodItemCollectionGetQueryHandlerBase = new MyFoodItemCollectionGetQueryHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, fixture.Create<bool>());
            Assert.That(foodItemCollectionGetQueryHandlerBase, Is.Not.Null);

            var foodItemCollectionGetQuery = fixture.Build<FoodItemCollectionGetQuery>()
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .With(m => m.FoodGroupIdentifier, null)
                .Create();

            foodItemCollectionGetQueryHandlerBase.Query(foodItemCollectionGetQuery);

            foodWasteObjectMapperMock.AssertWasCalled(m => m.Map<IFoodItemCollection, IView>(Arg<IFoodItemCollection>.Is.NotNull, Arg<CultureInfo>.Is.Equal(translationInfoMock.CultureInfo)));
        }

        /// <summary>
        /// Tests that Query calls Map with a food item collection containing active and inactive food items on the object mapper which can map domain object in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatQueryCallsMapWithFoodItemCollectionContainingActiveAndInactiveFoodItemsOnFoodWasteObjectMapper()
        {
            var fixture = new Fixture();

            var dataProviderMock = DomainObjectMockBuilder.BuildDataProviderMock();
            var foodItemMockCollection = new List<IFoodItem>
            {
                DomainObjectMockBuilder.BuildFoodItemMock(dataProvider: dataProviderMock),
                DomainObjectMockBuilder.BuildFoodItemMock(dataProvider: dataProviderMock),
                DomainObjectMockBuilder.BuildFoodItemMock(dataProvider: dataProviderMock),
                DomainObjectMockBuilder.BuildFoodItemMock(false, dataProviderMock),
                DomainObjectMockBuilder.BuildFoodItemMock(false, dataProviderMock)
            };
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.DataProviderForFoodItemsGet())
                .Return(dataProviderMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodItemGetAll())
                .Return(foodItemMockCollection)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IFoodItemCollection, IView>(Arg<IFoodItemCollection>.Is.NotNull, Arg<CultureInfo>.Is.Anything))
                .WhenCalled(e =>
                {
                    var foodItemCollection = (IFoodItemCollection) e.Arguments.ElementAt(0);
                    Assert.That(foodItemCollection, Is.Not.Null);
                    Assert.That(foodItemCollection.Count, Is.EqualTo(foodItemMockCollection.Count));
                    foreach (var foodItem in foodItemCollection)
                    {
                        Assert.That(foodItem, Is.Not.Null);
                        Assert.That(foodItemMockCollection.Contains(foodItem), Is.True);
                    }
                })
                .Return(MockRepository.GenerateMock<IView>())
                .Repeat.Any();

            var foodItemCollectionGetQueryHandlerBase = new MyFoodItemCollectionGetQueryHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, false);
            Assert.That(foodItemCollectionGetQueryHandlerBase, Is.Not.Null);

            var foodItemCollectionGetQuery = fixture.Build<FoodItemCollectionGetQuery>()
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .With(m => m.FoodGroupIdentifier, null)
                .Create();

            foodItemCollectionGetQueryHandlerBase.Query(foodItemCollectionGetQuery);

            foodWasteObjectMapperMock.AssertWasCalled(m => m.Map<IFoodItemCollection, IView>(Arg<IFoodItemCollection>.Is.NotNull, Arg<CultureInfo>.Is.Anything));
        }

        /// <summary>
        /// Tests that Query calls Map with a food item collection containing only active food items on the object mapper which can map domain object in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatQueryCallsMapWithFoodItemCollectionContainingOnlyActiveFoodItemsOnFoodWasteObjectMapper()
        {
            var fixture = new Fixture();

            var dataProviderMock = DomainObjectMockBuilder.BuildDataProviderMock();
            var foodItemMockCollection = new List<IFoodItem>
            {
                DomainObjectMockBuilder.BuildFoodItemMock(dataProvider: dataProviderMock),
                DomainObjectMockBuilder.BuildFoodItemMock(dataProvider: dataProviderMock),
                DomainObjectMockBuilder.BuildFoodItemMock(dataProvider: dataProviderMock),
                DomainObjectMockBuilder.BuildFoodItemMock(false, dataProviderMock),
                DomainObjectMockBuilder.BuildFoodItemMock(false, dataProviderMock)
            };
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.DataProviderForFoodItemsGet())
                .Return(dataProviderMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodItemGetAll())
                .Return(foodItemMockCollection)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IFoodItemCollection, IView>(Arg<IFoodItemCollection>.Is.NotNull, Arg<CultureInfo>.Is.Anything))
                .WhenCalled(e =>
                {
                    var foodItemCollection = (IFoodItemCollection) e.Arguments.ElementAt(0);
                    Assert.That(foodItemCollection, Is.Not.Null);
                    Assert.That(foodItemCollection.Count, Is.EqualTo(foodItemMockCollection.Count(foodItem => foodItem.IsActive)));
                    foreach (var foodItem in foodItemCollection)
                    {
                        Assert.That(foodItem, Is.Not.Null);
                        Assert.That(foodItem.IsActive, Is.True);
                        Assert.That(foodItemMockCollection.Contains(foodItem), Is.True);
                    }
                })
                .Return(MockRepository.GenerateMock<IView>())
                .Repeat.Any();

            var foodItemCollectionGetQueryHandlerBase = new MyFoodItemCollectionGetQueryHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, true);
            Assert.That(foodItemCollectionGetQueryHandlerBase, Is.Not.Null);

            var foodItemCollectionGetQuery = fixture.Build<FoodItemCollectionGetQuery>()
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .With(m => m.FoodGroupIdentifier, null)
                .Create();

            foodItemCollectionGetQueryHandlerBase.Query(foodItemCollectionGetQuery);

            foodWasteObjectMapperMock.AssertWasCalled(m => m.Map<IFoodItemCollection, IView>(Arg<IFoodItemCollection>.Is.NotNull, Arg<CultureInfo>.Is.Anything));
        }

        /// <summary>
        /// Tests that Query calls Map with a food item collection containing data provider who provides the food items on the object mapper which can map domain object in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatQueryCallsMapWithFoodItemCollectionContainingDataProviderOnFoodWasteObjectMapper()
        {
            var fixture = new Fixture();

            var dataProviderMock = DomainObjectMockBuilder.BuildDataProviderMock();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.DataProviderForFoodItemsGet())
                .Return(dataProviderMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodItemGetAll())
                .Return(DomainObjectMockBuilder.BuildFoodItemMockCollection())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IFoodItemCollection, IView>(Arg<IFoodItemCollection>.Is.NotNull, Arg<CultureInfo>.Is.Anything))
                .WhenCalled(e =>
                {
                    var foodItemCollection = (IFoodItemCollection) e.Arguments.ElementAt(0);
                    Assert.That(foodItemCollection, Is.Not.Null);
                    Assert.That(foodItemCollection.DataProvider, Is.Not.Null);
                    Assert.That(foodItemCollection.DataProvider, Is.EqualTo(dataProviderMock));
                })
                .Return(MockRepository.GenerateMock<IView>())
                .Repeat.Any();

            var foodItemCollectionGetQueryHandlerBase = new MyFoodItemCollectionGetQueryHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, fixture.Create<bool>());
            Assert.That(foodItemCollectionGetQueryHandlerBase, Is.Not.Null);

            var foodItemCollectionGetQuery = fixture.Build<FoodItemCollectionGetQuery>()
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .With(m => m.FoodGroupIdentifier, null)
                .Create();

            foodItemCollectionGetQueryHandlerBase.Query(foodItemCollectionGetQuery);

            foodWasteObjectMapperMock.AssertWasCalled(m => m.Map<IFoodItemCollection, IView>(Arg<IFoodItemCollection>.Is.NotNull, Arg<CultureInfo>.Is.Anything));
        }

        /// <summary>
        /// Tests that Query returns the mapped view from Map on the object mapper which can map domain object in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatQueryReturnsViewFromMapOnFoodWasteObjectMapper()
        {
            var fixture = new Fixture();

            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.DataProviderForFoodItemsGet())
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodItemGetAll())
                .Return(DomainObjectMockBuilder.BuildFoodItemMockCollection())
                .Repeat.Any();

            var view = MockRepository.GenerateMock<IView>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IFoodItemCollection, IView>(Arg<IFoodItemCollection>.Is.NotNull, Arg<CultureInfo>.Is.Anything))
                .Return(view)
                .Repeat.Any();

            var foodItemCollectionGetQueryHandlerBase = new MyFoodItemCollectionGetQueryHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, fixture.Create<bool>());
            Assert.That(foodItemCollectionGetQueryHandlerBase, Is.Not.Null);

            var foodItemCollectionGetQuery = fixture.Build<FoodItemCollectionGetQuery>()
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .With(m => m.FoodGroupIdentifier, null)
                .Create();

            var result = foodItemCollectionGetQueryHandlerBase.Query(foodItemCollectionGetQuery);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(view));
        }
    }
}
