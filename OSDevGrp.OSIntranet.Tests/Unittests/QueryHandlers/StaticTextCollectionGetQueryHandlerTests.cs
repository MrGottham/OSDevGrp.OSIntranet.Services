using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.QueryHandlers;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers
{
    /// <summary>
    /// Tests the functionality which handles the query for getting a collection of static texts.
    /// </summary>
    [TestFixture]
    public class StaticTextCollectionGetQueryHandlerTests
    {
        /// <summary>
        /// Test that the constructor initialize the functionality which handles the query for getting a collection of static texts.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeStaticTextCollectionGetQueryHandler()
        {
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var staticTextCollectionGetQueryHandler = new StaticTextCollectionGetQueryHandler(systemDataRepositoryMock, foodWasteObjectMapperMock);
            Assert.That(staticTextCollectionGetQueryHandler, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the repository which can access system data in the food waste domain is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenSystemDataRepositoryIsNull()
        {
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var exception = Assert.Throws<ArgumentNullException>(() => new StaticTextCollectionGetQueryHandler(null, foodWasteObjectMapperMock));
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
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();

            var exception = Assert.Throws<ArgumentNullException>(() => new StaticTextCollectionGetQueryHandler(systemDataRepositoryMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("foodWasteObjectMapper"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Query throws an ArgumentNullException when the query for getting a collection of data providers is null.
        /// </summary>
        [Test]
        public void TestThatQueryThrowsArgumentNullExceptionIfDataProviderCollectionGetQueryIsNull()
        {
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var staticTextCollectionGetQueryHandler = new StaticTextCollectionGetQueryHandler(systemDataRepositoryMock, foodWasteObjectMapperMock);
            Assert.That(staticTextCollectionGetQueryHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => staticTextCollectionGetQueryHandler.Query(null));
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

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IEnumerable<IStaticText>, IEnumerable<StaticTextSystemView>>(Arg<IEnumerable<IStaticText>>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.CreateMany<StaticTextSystemView>(2).ToList())
                .Repeat.Any();

            var staticTextCollectionGetQueryHandler = new StaticTextCollectionGetQueryHandler(systemDataRepositoryMock, foodWasteObjectMapperMock);
            Assert.That(staticTextCollectionGetQueryHandler, Is.Not.Null);

            var translationInfoIdentifier = Guid.NewGuid();
            var staticTextCollectionGetQuery = fixture.Build<StaticTextCollectionGetQuery>()
                .With(m => m.TranslationInfoIdentifier, translationInfoIdentifier)
                .Create();

            staticTextCollectionGetQueryHandler.Query(staticTextCollectionGetQuery);

            systemDataRepositoryMock.AssertWasCalled(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Equal(translationInfoIdentifier)));
        }

        /// <summary>
        /// Tests that Query calls StaticTextGetAll on the repository which can access system data in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatQueryCallsStaticTextGetAllOnSystemDataRepository()
        {
            var fixture = new Fixture();

            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.StaticTextGetAll())
                .Return(DomainObjectMockBuilder.BuildStaticTextMockCollection())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IEnumerable<IStaticText>, IEnumerable<StaticTextSystemView>>(Arg<IEnumerable<IStaticText>>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.CreateMany<StaticTextSystemView>(2).ToList())
                .Repeat.Any();

            var staticTextCollectionGetQueryHandler = new StaticTextCollectionGetQueryHandler(systemDataRepositoryMock, foodWasteObjectMapperMock);
            Assert.That(staticTextCollectionGetQueryHandler, Is.Not.Null);

            var staticTextCollectionGetQuery = fixture.Build<StaticTextCollectionGetQuery>()
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            staticTextCollectionGetQueryHandler.Query(staticTextCollectionGetQuery);

            systemDataRepositoryMock.AssertWasCalled(m => m.StaticTextGetAll());
        }

        /// <summary>
        /// Tests that Query calls Map on the object mapper which can map domain object in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatQueryCallsMapOnFoodWasteObjectMapper()
        {
            var fixture = new Fixture();

            var translationInfoMock = DomainObjectMockBuilder.BuildTranslationInfoMock();
            var staticTextMockCollection = DomainObjectMockBuilder.BuildStaticTextMockCollection();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(translationInfoMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.StaticTextGetAll())
                .Return(staticTextMockCollection)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IEnumerable<IStaticText>, IEnumerable<StaticTextSystemView>>(Arg<IEnumerable<IStaticText>>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.CreateMany<StaticTextSystemView>(2).ToList())
                .Repeat.Any();

            var staticTextCollectionGetQueryHandler = new StaticTextCollectionGetQueryHandler(systemDataRepositoryMock, foodWasteObjectMapperMock);
            Assert.That(staticTextCollectionGetQueryHandler, Is.Not.Null);

            var staticTextCollectionGetQuery = fixture.Build<StaticTextCollectionGetQuery>()
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            staticTextCollectionGetQueryHandler.Query(staticTextCollectionGetQuery);

            foodWasteObjectMapperMock.AssertWasCalled(m => m.Map<IEnumerable<IStaticText>, IEnumerable<StaticTextSystemView>>(Arg<IEnumerable<IStaticText>>.Is.Equal(staticTextMockCollection), Arg<CultureInfo>.Is.Equal(translationInfoMock.CultureInfo)));
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
            systemDataRepositoryMock.Stub(m => m.StaticTextGetAll())
                .Return(DomainObjectMockBuilder.BuildStaticTextMockCollection())
                .Repeat.Any();

            var staticTextSystemViewCollection = fixture.CreateMany<StaticTextSystemView>(2).ToList();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IEnumerable<IStaticText>, IEnumerable<StaticTextSystemView>>(Arg<IEnumerable<IStaticText>>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(staticTextSystemViewCollection)
                .Repeat.Any();

            var staticTextCollectionGetQueryHandler = new StaticTextCollectionGetQueryHandler(systemDataRepositoryMock, foodWasteObjectMapperMock);
            Assert.That(staticTextCollectionGetQueryHandler, Is.Not.Null);

            var staticTextCollectionGetQuery = fixture.Build<StaticTextCollectionGetQuery>()
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            var result = staticTextCollectionGetQueryHandler.Query(staticTextCollectionGetQuery);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EqualTo(staticTextSystemViewCollection));
        }
    }
}
