using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers.Core
{
    /// <summary>
    /// Tests the functionality which handles a query for getting the tree of food groups.
    /// </summary>
    [TestFixture]
    public class FoodGroupTreeGetQueryHandlerBaseTest
    {
        /// <summary>
        /// Private class for testing the functionality which handles a query for getting the tree of food groups.
        /// </summary>
        private class MyFoodGroupTreeGetQueryHandler : FoodGroupTreeGetQueryHandlerBase<IView>
        {
            #region Private variables

            private readonly bool _onlyActive;
            
            #endregion

            #region Constructor

            /// <summary>
            /// Creates a private class for testing the functionality which handles a query for getting the tree of food groups.
            /// </summary>
            /// <param name="systemDataRepository">Implementation for a repository which can access system data in the food waste domain.</param>
            /// <param name="foodWasteObjectMapper">Implementation for a object mapper which can map domain object in the food waste domain.</param>
            /// <param name="onlyActive">Indication of whether only active food groups should be included.</param>
            public MyFoodGroupTreeGetQueryHandler(ISystemDataRepository systemDataRepository, IFoodWasteObjectMapper foodWasteObjectMapper, bool onlyActive) 
                : base(systemDataRepository, foodWasteObjectMapper)
            {
                _onlyActive = onlyActive;
            }

            #endregion

            #region Properties

            /// <summary>
            /// Gets whether only active food groups should be included.
            /// </summary>
            protected override bool OnlyActive
            {
                get { return _onlyActive; }
            }

            #endregion

            #region Methods

            /// <summary>
            /// Returns whether only active food groups should be included.
            /// </summary>
            /// <returns>Indication of whether only active food groups should be included.</returns>
            public bool GetOnlyActive()
            {
                return OnlyActive;
            }

            #endregion
        }

        /// <summary>
        /// Tests that the constructor initialize functionality which handles a query for getting the tree of food groups.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatConstructorInitializeFoodGroupTreeGetQueryHandlerBase(bool activeOnly)
        {
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var foodGroupTreeGetQueryHandlerBase = new MyFoodGroupTreeGetQueryHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, activeOnly);
            Assert.That(foodGroupTreeGetQueryHandlerBase, Is.Not.Null);
            Assert.That(foodGroupTreeGetQueryHandlerBase.GetOnlyActive(), Is.EqualTo(activeOnly));
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the repository which can access system data in the food waste domain is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenSystemDataRepositoryIsNull()
        {
            var fixture = new Fixture();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyFoodGroupTreeGetQueryHandler(null, foodWasteObjectMapperMock, fixture.Create<bool>()));
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

            var exception = Assert.Throws<ArgumentNullException>(() => new MyFoodGroupTreeGetQueryHandler(systemDataRepositoryMock, null, fixture.Create<bool>()));
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

            var foodGroupTreeGetQueryHandlerBase = new MyFoodGroupTreeGetQueryHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, fixture.Create<bool>());
            Assert.That(foodGroupTreeGetQueryHandlerBase, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodGroupTreeGetQueryHandlerBase.Query(null));
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
            systemDataRepositoryMock.Stub(m => m.DataProviderForFoodGroupsGet())
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodGroupGetAllOnRoot())
                .Return(DomainObjectMockBuilder.BuildFoodGroupMockCollection())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IFoodGroupCollection, IView>(Arg<IFoodGroupCollection>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(MockRepository.GenerateMock<IView>())
                .Repeat.Any();

            var foodGroupTreeGetQueryHandlerBase = new MyFoodGroupTreeGetQueryHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, fixture.Create<bool>());
            Assert.That(foodGroupTreeGetQueryHandlerBase, Is.Not.Null);

            var foodGroupTreeGetQuery = fixture.Build<FoodGroupTreeGetQuery>()
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            foodGroupTreeGetQueryHandlerBase.Query(foodGroupTreeGetQuery);

            systemDataRepositoryMock.AssertWasCalled(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Equal(foodGroupTreeGetQuery.TranslationInfoIdentifier)));
        }

        /// <summary>
        /// Tests that Query calls DataProviderForFoodGroupsGet on the repository which can access system data in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatQueryCallsDataProviderForFoodGroupsGetOnSystemDataRepository()
        {
            var fixture = new Fixture();

            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.DataProviderForFoodGroupsGet())
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodGroupGetAllOnRoot())
                .Return(DomainObjectMockBuilder.BuildFoodGroupMockCollection())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IFoodGroupCollection, IView>(Arg<IFoodGroupCollection>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(MockRepository.GenerateMock<IView>())
                .Repeat.Any();

            var foodGroupTreeGetQueryHandlerBase = new MyFoodGroupTreeGetQueryHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, fixture.Create<bool>());
            Assert.That(foodGroupTreeGetQueryHandlerBase, Is.Not.Null);

            var foodGroupTreeGetQuery = fixture.Build<FoodGroupTreeGetQuery>()
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            foodGroupTreeGetQueryHandlerBase.Query(foodGroupTreeGetQuery);

            systemDataRepositoryMock.AssertWasCalled(m => m.DataProviderForFoodGroupsGet());
        }

        /// <summary>
        /// Tests that Query calls FoodGroupGetAllOnRoot on the repository which can access system data in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatQueryCallsFoodGroupGetAllOnRootOnSystemDataRepository()
        {
            var fixture = new Fixture();

            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.DataProviderForFoodGroupsGet())
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodGroupGetAllOnRoot())
                .Return(DomainObjectMockBuilder.BuildFoodGroupMockCollection())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IFoodGroupCollection, IView>(Arg<IFoodGroupCollection>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(MockRepository.GenerateMock<IView>())
                .Repeat.Any();

            var foodGroupTreeGetQueryHandlerBase = new MyFoodGroupTreeGetQueryHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, fixture.Create<bool>());
            Assert.That(foodGroupTreeGetQueryHandlerBase, Is.Not.Null);

            var foodGroupTreeGetQuery = fixture.Build<FoodGroupTreeGetQuery>()
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            foodGroupTreeGetQueryHandlerBase.Query(foodGroupTreeGetQuery);

            systemDataRepositoryMock.AssertWasCalled(m => m.FoodGroupGetAllOnRoot());
        }

        /// <summary>
        /// Tests that Query calls Translate on the data provider who provides the food groups.
        /// </summary>
        [Test]
        public void TestThatQueryCallsTranslateOnDataProviderForFoodGroups()
        {
            var fixture = new Fixture();

            var translationInfoMock = DomainObjectMockBuilder.BuildTranslationInfoMock();
            var dataProviderMock = DomainObjectMockBuilder.BuildDataProviderMock();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(translationInfoMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.DataProviderForFoodGroupsGet())
                .Return(dataProviderMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodGroupGetAllOnRoot())
                .Return(DomainObjectMockBuilder.BuildFoodGroupMockCollection())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IFoodGroupCollection, IView>(Arg<IFoodGroupCollection>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(MockRepository.GenerateMock<IView>())
                .Repeat.Any();

            var foodGroupTreeGetQueryHandlerBase = new MyFoodGroupTreeGetQueryHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, fixture.Create<bool>());
            Assert.That(foodGroupTreeGetQueryHandlerBase, Is.Not.Null);

            var foodGroupTreeGetQuery = fixture.Build<FoodGroupTreeGetQuery>()
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            foodGroupTreeGetQueryHandlerBase.Query(foodGroupTreeGetQuery);

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
            systemDataRepositoryMock.Stub(m => m.DataProviderForFoodGroupsGet())
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodGroupGetAllOnRoot())
                .Return(DomainObjectMockBuilder.BuildFoodGroupMockCollection())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IFoodGroupCollection, IView>(Arg<IFoodGroupCollection>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(MockRepository.GenerateMock<IView>())
                .Repeat.Any();

            var foodGroupTreeGetQueryHandlerBase = new MyFoodGroupTreeGetQueryHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, fixture.Create<bool>());
            Assert.That(foodGroupTreeGetQueryHandlerBase, Is.Not.Null);

            var foodGroupTreeGetQuery = fixture.Build<FoodGroupTreeGetQuery>()
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            foodGroupTreeGetQueryHandlerBase.Query(foodGroupTreeGetQuery);

            foodWasteObjectMapperMock.AssertWasCalled(m => m.Map<IFoodGroupCollection, IView>(Arg<IFoodGroupCollection>.Is.NotNull, Arg<CultureInfo>.Is.Equal(translationInfoMock.CultureInfo)));
        }

        /// <summary>
        /// Tests that Query calls Map with a food group collection containing active and inactive food groups on the object mapper which can map domain object in the food waste domain when only active food groups is false.
        /// </summary>
        [Test]
        public void TestThatQueryCallsMapWithFoodGroupCollectionContainingActiveAndInactiveFoodGroupsOnFoodWasteObjectMapperWhenOnlyActiveIsFalse()
        {
            var fixture = new Fixture();

            var dataProviderMock = DomainObjectMockBuilder.BuildDataProviderMock();
            var foodGroupMockCollection = new List<IFoodGroup>
            {
                DomainObjectMockBuilder.BuildFoodGroupMock(dataProvider: dataProviderMock),
                DomainObjectMockBuilder.BuildFoodGroupMock(dataProvider: dataProviderMock),
                DomainObjectMockBuilder.BuildFoodGroupMock(dataProvider: dataProviderMock),
                DomainObjectMockBuilder.BuildFoodGroupMock(isActive: false, dataProvider: dataProviderMock),
                DomainObjectMockBuilder.BuildFoodGroupMock(isActive: false, dataProvider: dataProviderMock)
            };
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.DataProviderForFoodGroupsGet())
                .Return(dataProviderMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodGroupGetAllOnRoot())
                .Return(foodGroupMockCollection)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IFoodGroupCollection, IView>(Arg<IFoodGroupCollection>.Is.NotNull, Arg<CultureInfo>.Is.Anything))
                .WhenCalled(e =>
                {
                    var foodGroupCollection = (IFoodGroupCollection) e.Arguments.ElementAt(0);
                    Assert.That(foodGroupCollection, Is.Not.Null);
                    Assert.That(foodGroupCollection.Count, Is.EqualTo(foodGroupMockCollection.Count));
                    foreach (var foodGroup in foodGroupCollection)
                    {
                        Assert.That(foodGroup, Is.Not.Null);
                        Assert.That(foodGroupMockCollection.Contains(foodGroup), Is.True);
                    }
                })
                .Return(MockRepository.GenerateMock<IView>())
                .Repeat.Any();

            var foodGroupTreeGetQueryHandlerBase = new MyFoodGroupTreeGetQueryHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, false);
            Assert.That(foodGroupTreeGetQueryHandlerBase, Is.Not.Null);

            var foodGroupTreeGetQuery = fixture.Build<FoodGroupTreeGetQuery>()
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            foodGroupTreeGetQueryHandlerBase.Query(foodGroupTreeGetQuery);

            foodWasteObjectMapperMock.AssertWasCalled(m => m.Map<IFoodGroupCollection, IView>(Arg<IFoodGroupCollection>.Is.NotNull, Arg<CultureInfo>.Is.Anything));
        }

        /// <summary>
        /// Tests that Query calls Map with a food group collection containing only active food groups on the object mapper which can map domain object in the food waste domain when only active food groups is true.
        /// </summary>
        [Test]
        public void TestThatQueryCallsMapWithFoodGroupCollectionContainingOnlyActiveFoodGroupsOnFoodWasteObjectMapperWhenOnlyActiveIsTrue()
        {
            var fixture = new Fixture();

            var dataProviderMock = DomainObjectMockBuilder.BuildDataProviderMock();
            var foodGroupMockCollection = new List<IFoodGroup>
            {
                DomainObjectMockBuilder.BuildFoodGroupMock(dataProvider: dataProviderMock),
                DomainObjectMockBuilder.BuildFoodGroupMock(dataProvider: dataProviderMock),
                DomainObjectMockBuilder.BuildFoodGroupMock(dataProvider: dataProviderMock),
                DomainObjectMockBuilder.BuildFoodGroupMock(isActive: false, dataProvider: dataProviderMock),
                DomainObjectMockBuilder.BuildFoodGroupMock(isActive: false, dataProvider: dataProviderMock)
            };
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.DataProviderForFoodGroupsGet())
                .Return(dataProviderMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodGroupGetAllOnRoot())
                .Return(foodGroupMockCollection)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IFoodGroupCollection, IView>(Arg<IFoodGroupCollection>.Is.NotNull, Arg<CultureInfo>.Is.Anything))
                .WhenCalled(e =>
                {
                    var foodGroupCollection = (IFoodGroupCollection) e.Arguments.ElementAt(0);
                    Assert.That(foodGroupCollection, Is.Not.Null);
                    Assert.That(foodGroupCollection.Count, Is.EqualTo(foodGroupMockCollection.Count(foodGroup => foodGroup.IsActive)));
                    foreach (var foodGroup in foodGroupCollection)
                    {
                        Assert.That(foodGroup, Is.Not.Null);
                        Assert.That(foodGroup.IsActive, Is.True);
                        Assert.That(foodGroupMockCollection.Contains(foodGroup), Is.True);
                    }
                })
                .Return(MockRepository.GenerateMock<IView>())
                .Repeat.Any();

            var foodGroupTreeGetQueryHandlerBase = new MyFoodGroupTreeGetQueryHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, true);
            Assert.That(foodGroupTreeGetQueryHandlerBase, Is.Not.Null);

            var foodGroupTreeGetQuery = fixture.Build<FoodGroupTreeGetQuery>()
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            foodGroupTreeGetQueryHandlerBase.Query(foodGroupTreeGetQuery);

            foodWasteObjectMapperMock.AssertWasCalled(m => m.Map<IFoodGroupCollection, IView>(Arg<IFoodGroupCollection>.Is.NotNull, Arg<CultureInfo>.Is.Anything));
        }

        /// <summary>
        /// Tests that Query calls Map with a food group collection containing data provider who provides the food groups on the object mapper which can map domain object in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatQueryCallsMapWithFoodGroupCollectionContainingDataProviderOnFoodWasteObjectMapper()
        {
            var fixture = new Fixture();

            var dataProviderMock = DomainObjectMockBuilder.BuildDataProviderMock();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.DataProviderForFoodGroupsGet())
                .Return(dataProviderMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodGroupGetAllOnRoot())
                .Return(DomainObjectMockBuilder.BuildFoodGroupMockCollection())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IFoodGroupCollection, IView>(Arg<IFoodGroupCollection>.Is.NotNull, Arg<CultureInfo>.Is.Anything))
                .WhenCalled(e =>
                {
                    var foodGroupCollection = (IFoodGroupCollection) e.Arguments.ElementAt(0);
                    Assert.That(foodGroupCollection, Is.Not.Null);
                    Assert.That(foodGroupCollection.DataProvider, Is.Not.Null);
                    Assert.That(foodGroupCollection.DataProvider, Is.EqualTo(dataProviderMock));
                })
                .Return(MockRepository.GenerateMock<IView>())
                .Repeat.Any();

            var foodGroupTreeGetQueryHandlerBase = new MyFoodGroupTreeGetQueryHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, fixture.Create<bool>());
            Assert.That(foodGroupTreeGetQueryHandlerBase, Is.Not.Null);

            var foodGroupTreeGetQuery = fixture.Build<FoodGroupTreeGetQuery>()
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            foodGroupTreeGetQueryHandlerBase.Query(foodGroupTreeGetQuery);

            foodWasteObjectMapperMock.AssertWasCalled(m => m.Map<IFoodGroupCollection, IView>(Arg<IFoodGroupCollection>.Is.NotNull, Arg<CultureInfo>.Is.Anything));
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
            systemDataRepositoryMock.Stub(m => m.DataProviderForFoodGroupsGet())
                .Return(DomainObjectMockBuilder.BuildDataProviderMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.FoodGroupGetAllOnRoot())
                .Return(DomainObjectMockBuilder.BuildFoodGroupMockCollection())
                .Repeat.Any();

            var viewMock = MockRepository.GenerateMock<IView>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IFoodGroupCollection, IView>(Arg<IFoodGroupCollection>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(viewMock)
                .Repeat.Any();

            var foodGroupTreeGetQueryHandlerBase = new MyFoodGroupTreeGetQueryHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, fixture.Create<bool>());
            Assert.That(foodGroupTreeGetQueryHandlerBase, Is.Not.Null);

            var foodGroupTreeGetQuery = fixture.Build<FoodGroupTreeGetQuery>()
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            var result = foodGroupTreeGetQueryHandlerBase.Query(foodGroupTreeGetQuery);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(viewMock));
        }
    }
}
