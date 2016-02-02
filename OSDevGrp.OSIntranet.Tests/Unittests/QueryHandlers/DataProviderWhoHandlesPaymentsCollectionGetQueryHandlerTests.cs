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
    /// Tests the query handler which handles the query for getting a collection of data providers who handles payments.
    /// </summary>
    [TestFixture]
    public class DataProviderWhoHandlesPaymentsCollectionGetQueryHandlerTests
    {
        /// <summary>
        /// Test that the constructor initialize the query handler which handles the query for getting a collection of data providers who handles payments.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeDataProviderWhoHandlesPaymentsCollectionGetQueryHandler()
        {
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var dataProviderWhoHandlesPaymentsCollectionGetQueryHandler = new DataProviderWhoHandlesPaymentsCollectionGetQueryHandler(systemDataRepositoryMock, objectMapperMock);
            Assert.That(dataProviderWhoHandlesPaymentsCollectionGetQueryHandler, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the repository which can access system data in the food waste domain is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenSystemDataRepositoryIsNull()
        {
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var exception = Assert.Throws<ArgumentNullException>(() => new DataProviderWhoHandlesPaymentsCollectionGetQueryHandler(null, objectMapperMock));
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

            var exception = Assert.Throws<ArgumentNullException>(() => new DataProviderWhoHandlesPaymentsCollectionGetQueryHandler(systemDataRepositoryMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("foodWasteObjectMapper"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Query throws an ArgumentNullException when the query for getting a collection of data providers who handles payments is null.
        /// </summary>
        [Test]
        public void TestThatQueryThrowsArgumentNullExceptionWhenDataProviderWhoHandlesPaymentsCollectionGetQueryIsNull()
        {
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var dataProviderWhoHandlesPaymentsCollectionGetQueryHandler = new DataProviderWhoHandlesPaymentsCollectionGetQueryHandler(systemDataRepositoryMock, objectMapperMock);
            Assert.That(dataProviderWhoHandlesPaymentsCollectionGetQueryHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => dataProviderWhoHandlesPaymentsCollectionGetQueryHandler.Query(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("query"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Query calls Get with the identifier for the translation informations used for translation on the repository which can access system data in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatQueryCallsGetWithTranslationInfoIdentifierOnSystemDataRepository()
        {
            var fixture = new Fixture();

            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.DataProviderWhoHandlesPaymentsGetAll())
                .Return(DomainObjectMockBuilder.BuildDataProviderMockCollection(true))
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IEnumerable<IDataProvider>, IEnumerable<DataProviderView>>(Arg<IEnumerable<IDataProvider>>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.CreateMany<DataProviderView>(3).ToList())
                .Repeat.Any();

            var dataProviderWhoHandlesPaymentsCollectionGetQueryHandler = new DataProviderWhoHandlesPaymentsCollectionGetQueryHandler(systemDataRepositoryMock, objectMapperMock);
            Assert.That(dataProviderWhoHandlesPaymentsCollectionGetQueryHandler, Is.Not.Null);

            var dataProviderWhoHandlesPaymentsCollectionGetQuery = fixture.Build<DataProviderWhoHandlesPaymentsCollectionGetQuery>()
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            dataProviderWhoHandlesPaymentsCollectionGetQueryHandler.Query(dataProviderWhoHandlesPaymentsCollectionGetQuery);

            systemDataRepositoryMock.AssertWasCalled(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Equal(dataProviderWhoHandlesPaymentsCollectionGetQuery.TranslationInfoIdentifier)));
        }

        /// <summary>
        /// Tests that Query calls DataProviderWhoHandlesPaymentsGetAll on the repository which can access system data in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatQueryCallsDataProviderWhoHandlesPaymentsGetAllOnSystemDataRepository()
        {
            var fixture = new Fixture();

            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.DataProviderWhoHandlesPaymentsGetAll())
                .Return(DomainObjectMockBuilder.BuildDataProviderMockCollection(true))
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IEnumerable<IDataProvider>, IEnumerable<DataProviderView>>(Arg<IEnumerable<IDataProvider>>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.CreateMany<DataProviderView>(3).ToList())
                .Repeat.Any();

            var dataProviderWhoHandlesPaymentsCollectionGetQueryHandler = new DataProviderWhoHandlesPaymentsCollectionGetQueryHandler(systemDataRepositoryMock, objectMapperMock);
            Assert.That(dataProviderWhoHandlesPaymentsCollectionGetQueryHandler, Is.Not.Null);

            var dataProviderWhoHandlesPaymentsCollectionGetQuery = fixture.Build<DataProviderWhoHandlesPaymentsCollectionGetQuery>()
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            dataProviderWhoHandlesPaymentsCollectionGetQueryHandler.Query(dataProviderWhoHandlesPaymentsCollectionGetQuery);

            systemDataRepositoryMock.AssertWasCalled(m => m.DataProviderWhoHandlesPaymentsGetAll());
        }

        /// <summary>
        /// Tests that Query calls Map on the object mapper which can map domain object in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatQueryCallsMapOnFoodWasteObjectMapper()
        {
            var fixture = new Fixture();

            var translationInfoMock = DomainObjectMockBuilder.BuildTranslationInfoMock();
            var dataProviderMockCollection = DomainObjectMockBuilder.BuildDataProviderMockCollection(true);
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(translationInfoMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.DataProviderWhoHandlesPaymentsGetAll())
                .Return(dataProviderMockCollection)
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IEnumerable<IDataProvider>, IEnumerable<DataProviderView>>(Arg<IEnumerable<IDataProvider>>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.CreateMany<DataProviderView>(3).ToList())
                .Repeat.Any();

            var dataProviderWhoHandlesPaymentsCollectionGetQueryHandler = new DataProviderWhoHandlesPaymentsCollectionGetQueryHandler(systemDataRepositoryMock, objectMapperMock);
            Assert.That(dataProviderWhoHandlesPaymentsCollectionGetQueryHandler, Is.Not.Null);

            var dataProviderWhoHandlesPaymentsCollectionGetQuery = fixture.Build<DataProviderWhoHandlesPaymentsCollectionGetQuery>()
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            dataProviderWhoHandlesPaymentsCollectionGetQueryHandler.Query(dataProviderWhoHandlesPaymentsCollectionGetQuery);

            objectMapperMock.AssertWasCalled(m => m.Map<IEnumerable<IDataProvider>, IEnumerable<DataProviderView>>(Arg<IEnumerable<IDataProvider>>.Is.Equal(dataProviderMockCollection), Arg<CultureInfo>.Is.Equal(translationInfoMock.CultureInfo)));
        }

        /// <summary>
        /// Tests that Query return the result from Map on the object mapper which can map domain object in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatQueryReturnsResultFromMapOnFoodWasteObjectMapper()
        {
            var fixture = new Fixture();

            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.DataProviderWhoHandlesPaymentsGetAll())
                .Return(DomainObjectMockBuilder.BuildDataProviderMockCollection(true))
                .Repeat.Any();

            var dataProviderViewCollection = fixture.CreateMany<DataProviderView>(3).ToList();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IEnumerable<IDataProvider>, IEnumerable<DataProviderView>>(Arg<IEnumerable<IDataProvider>>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(dataProviderViewCollection)
                .Repeat.Any();

            var dataProviderWhoHandlesPaymentsCollectionGetQueryHandler = new DataProviderWhoHandlesPaymentsCollectionGetQueryHandler(systemDataRepositoryMock, objectMapperMock);
            Assert.That(dataProviderWhoHandlesPaymentsCollectionGetQueryHandler, Is.Not.Null);

            var dataProviderWhoHandlesPaymentsCollectionGetQuery = fixture.Build<DataProviderWhoHandlesPaymentsCollectionGetQuery>()
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            var result = dataProviderWhoHandlesPaymentsCollectionGetQueryHandler.Query(dataProviderWhoHandlesPaymentsCollectionGetQuery);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(dataProviderViewCollection));
        }
    }
}
