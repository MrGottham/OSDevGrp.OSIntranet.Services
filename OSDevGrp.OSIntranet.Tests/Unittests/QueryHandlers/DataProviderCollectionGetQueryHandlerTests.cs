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
using AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers
{
    /// <summary>
    /// Tests the functionality which handles the query for getting a collection of data providers.
    /// </summary>
    [TestFixture]
    public class DataProviderCollectionGetQueryHandlerTests
    {
        /// <summary>
        /// Test that the constructor initialize the functionality which handles the query for getting a collection of data providers.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeDataProviderCollectionGetQueryHandler()
        {
            var fixture = new Fixture();
            fixture.Customize<ISystemDataRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<ISystemDataRepository>()));
            fixture.Customize<IFoodWasteObjectMapper>(e => e.FromFactory(() => MockRepository.GenerateMock<IFoodWasteObjectMapper>()));

            var dataProviderCollectionGetQueryHandler = new DataProviderCollectionGetQueryHandler(fixture.Create<ISystemDataRepository>(), fixture.Create<IFoodWasteObjectMapper>());
            Assert.That(dataProviderCollectionGetQueryHandler, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the repository which can access system data in the food waste domain is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenSystemDataRepositoryIsNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IFoodWasteObjectMapper>(e => e.FromFactory(() => MockRepository.GenerateMock<IFoodWasteObjectMapper>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new DataProviderCollectionGetQueryHandler(null, fixture.Create<IFoodWasteObjectMapper>()));
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
            fixture.Customize<ISystemDataRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<ISystemDataRepository>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new DataProviderCollectionGetQueryHandler(fixture.Create<ISystemDataRepository>(), null));
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
            var fixture = new Fixture();
            fixture.Customize<ISystemDataRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<ISystemDataRepository>()));
            fixture.Customize<IFoodWasteObjectMapper>(e => e.FromFactory(() => MockRepository.GenerateMock<IFoodWasteObjectMapper>()));

            var dataProviderCollectionGetQueryHandler = new DataProviderCollectionGetQueryHandler(fixture.Create<ISystemDataRepository>(), fixture.Create<IFoodWasteObjectMapper>());
            Assert.That(dataProviderCollectionGetQueryHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => dataProviderCollectionGetQueryHandler.Query(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("query"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Query calls DataProviderGetAll on the repository which can access system data in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatQueryCallsDataProviderGetAllOnSystemDataRepository()
        {
            var fixture = new Fixture();

            var dataProviderMockCollection = DomainObjectMockBuilder.BuildDataProviderMockCollection().ToList();
            var systemDataRepository = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepository.Stub(m => m.DataProviderGetAll())
                .Return(dataProviderMockCollection)
                .Repeat.Any();

            var foodWasteObjectMapper = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapper.Stub(m => m.Map<IEnumerable<IDataProvider>, IEnumerable<DataProviderSystemView>>(Arg<IEnumerable<IDataProvider>>.Is.NotNull, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.CreateMany<DataProviderSystemView>(dataProviderMockCollection.Count).ToList())
                .Repeat.Any();

            var dataProviderCollectionGetQueryHandler = new DataProviderCollectionGetQueryHandler(systemDataRepository, foodWasteObjectMapper);
            Assert.That(dataProviderCollectionGetQueryHandler, Is.Not.Null);

            dataProviderCollectionGetQueryHandler.Query(fixture.Create<DataProviderCollectionGetQuery>());

            systemDataRepository.AssertWasCalled(m => m.DataProviderGetAll());
        }

        /// <summary>
        /// Tests that Query calls Map on the object mapper which can map domain object in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatQueryCallsMapOnFoodWasteObjectMapper()
        {
            var fixture = new Fixture();

            var dataProviderMockCollection = DomainObjectMockBuilder.BuildDataProviderMockCollection().ToList();
            var systemDataRepository = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepository.Stub(m => m.DataProviderGetAll())
                .Return(dataProviderMockCollection)
                .Repeat.Any();

            var foodWasteObjectMapper = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapper.Stub(m => m.Map<IEnumerable<IDataProvider>, IEnumerable<DataProviderSystemView>>(Arg<IEnumerable<IDataProvider>>.Is.NotNull, Arg<CultureInfo>.Is.Anything))
                .WhenCalled(e => ((IEnumerable<IDataProvider>) e.Arguments.ElementAt(0)).ToList().ForEach(n => Assert.IsTrue(dataProviderMockCollection.Contains(n))))
                .Return(fixture.CreateMany<DataProviderSystemView>(dataProviderMockCollection.Count).ToList())
                .Repeat.Any();

            var dataProviderCollectionGetQueryHandler = new DataProviderCollectionGetQueryHandler(systemDataRepository, foodWasteObjectMapper);
            Assert.That(dataProviderCollectionGetQueryHandler, Is.Not.Null);

            dataProviderCollectionGetQueryHandler.Query(fixture.Create<DataProviderCollectionGetQuery>());

            foodWasteObjectMapper.AssertWasCalled(m => m.Map<IEnumerable<IDataProvider>, IEnumerable<DataProviderSystemView>>(Arg<IEnumerable<IDataProvider>>.Is.NotNull, Arg<CultureInfo>.Is.Null));
        }

        /// <summary>
        /// Tests that Query returns the result from the object mapper which can map domain object in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatQueryReturnResultFromMapOnFoodWasteObjectMapper()
        {
            var fixture = new Fixture();

            var dataProviderMockCollection = DomainObjectMockBuilder.BuildDataProviderMockCollection().ToList();
            var systemDataRepository = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepository.Stub(m => m.DataProviderGetAll())
                .Return(dataProviderMockCollection)
                .Repeat.Any();

            var dataProviderSystemViewCollection = fixture.CreateMany<DataProviderSystemView>(dataProviderMockCollection.Count).ToList();
            var foodWasteObjectMapper = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapper.Stub(m => m.Map<IEnumerable<IDataProvider>, IEnumerable<DataProviderSystemView>>(Arg<IEnumerable<IDataProvider>>.Is.NotNull, Arg<CultureInfo>.Is.Anything))
                .Return(dataProviderSystemViewCollection)
                .Repeat.Any();

            var dataProviderCollectionGetQueryHandler = new DataProviderCollectionGetQueryHandler(systemDataRepository, foodWasteObjectMapper);
            Assert.That(dataProviderCollectionGetQueryHandler, Is.Not.Null);

            var result = dataProviderCollectionGetQueryHandler.Query(fixture.Create<DataProviderCollectionGetQuery>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(dataProviderSystemViewCollection));
        }
    }
}
