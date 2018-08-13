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
    /// Tests the functionality which handles the query for getting a collection of translation informations.
    /// </summary>
    [TestFixture]
    public class TranslationInfoCollectionGetQueryHandlerTests
    {
        /// <summary>
        /// Test that the constructor initialize the functionality which handles the query for getting a collection of translation informations.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeTranslationInfoCollectionGetQueryHandler()
        {
            var fixture = new Fixture();
            fixture.Customize<ISystemDataRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<ISystemDataRepository>()));
            fixture.Customize<IFoodWasteObjectMapper>(e => e.FromFactory(() => MockRepository.GenerateMock<IFoodWasteObjectMapper>()));

            var translationInfoCollectionGetQueryHandler = new TranslationInfoCollectionGetQueryHandler(fixture.Create<ISystemDataRepository>(), fixture.Create<IFoodWasteObjectMapper>());
            Assert.That(translationInfoCollectionGetQueryHandler, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the repository which can access system data in the food waste domain is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenSystemDataRepositoryIsNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IFoodWasteObjectMapper>(e => e.FromFactory(() => MockRepository.GenerateMock<IFoodWasteObjectMapper>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new TranslationInfoCollectionGetQueryHandler(null, fixture.Create<IFoodWasteObjectMapper>()));
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

            var exception = Assert.Throws<ArgumentNullException>(() => new TranslationInfoCollectionGetQueryHandler(fixture.Create<ISystemDataRepository>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("foodWasteObjectMapper"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Query throws an ArgumentNullException when the query for getting a collection of translation informations is null.
        /// </summary>
        [Test]
        public void TestThatQueryThrowsArgumentNullExceptionIfTranslationInfoCollectionGetQueryIsNull()
        {
            var fixture = new Fixture();
            fixture.Customize<ISystemDataRepository>(e => e.FromFactory(() => MockRepository.GenerateMock<ISystemDataRepository>()));
            fixture.Customize<IFoodWasteObjectMapper>(e => e.FromFactory(() => MockRepository.GenerateMock<IFoodWasteObjectMapper>()));

            var translationInfoCollectionGetQueryHandler = new TranslationInfoCollectionGetQueryHandler(fixture.Create<ISystemDataRepository>(), fixture.Create<IFoodWasteObjectMapper>());
            Assert.That(translationInfoCollectionGetQueryHandler, Is.Not.Null);
            
            var exception = Assert.Throws<ArgumentNullException>(() => translationInfoCollectionGetQueryHandler.Query(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("query"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Query calls TranslationInfoGetAll on the repository which can access system data in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatQueryCallsTranslationInfoGetAllOnSystemDataRepository()
        {
            var fixture = new Fixture();

            var translationInfoMockCollection = DomainObjectMockBuilder.BuildTranslationInfoMockCollection().ToList();
            var systemDataRepository = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepository.Stub(m => m.TranslationInfoGetAll())
                .Return(translationInfoMockCollection)
                .Repeat.Any();

            var foodWasteObjectMapper = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapper.Stub(m => m.Map<IEnumerable<ITranslationInfo>, IEnumerable<TranslationInfoSystemView>>(Arg<IEnumerable<ITranslationInfo>>.Is.NotNull, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.CreateMany<TranslationInfoSystemView>(translationInfoMockCollection.Count).ToList())
                .Repeat.Any();

            var translationInfoCollectionGetQueryHandler = new TranslationInfoCollectionGetQueryHandler(systemDataRepository, foodWasteObjectMapper);
            Assert.That(translationInfoCollectionGetQueryHandler, Is.Not.Null);

            translationInfoCollectionGetQueryHandler.Query(fixture.Create<TranslationInfoCollectionGetQuery>());

            systemDataRepository.AssertWasCalled(m => m.TranslationInfoGetAll());
        }

        /// <summary>
        /// Tests that Query calls Map on the object mapper which can map domain object in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatQueryCallsMapOnFoodWasteObjectMapper()
        {
            var fixture = new Fixture();

            var translationInfoMockCollection = DomainObjectMockBuilder.BuildTranslationInfoMockCollection().ToList();
            var systemDataRepository = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepository.Stub(m => m.TranslationInfoGetAll())
                .Return(translationInfoMockCollection)
                .Repeat.Any();

            var foodWasteObjectMapper = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapper.Stub(m => m.Map<IEnumerable<ITranslationInfo>, IEnumerable<TranslationInfoSystemView>>(Arg<IEnumerable<ITranslationInfo>>.Is.NotNull, Arg<CultureInfo>.Is.Anything))
                .WhenCalled(e => ((IEnumerable<ITranslationInfo>) e.Arguments.ElementAt(0)).ToList().ForEach(n => Assert.IsTrue(translationInfoMockCollection.Contains(n))))
                .Return(fixture.CreateMany<TranslationInfoSystemView>(translationInfoMockCollection.Count).ToList())
                .Repeat.Any();

            var translationInfoCollectionGetQueryHandler = new TranslationInfoCollectionGetQueryHandler(systemDataRepository, foodWasteObjectMapper);
            Assert.That(translationInfoCollectionGetQueryHandler, Is.Not.Null);

            translationInfoCollectionGetQueryHandler.Query(fixture.Create<TranslationInfoCollectionGetQuery>());

            foodWasteObjectMapper.AssertWasCalled(m => m.Map<IEnumerable<ITranslationInfo>, IEnumerable<TranslationInfoSystemView>>(Arg<IEnumerable<ITranslationInfo>>.Is.NotNull, Arg<CultureInfo>.Is.Null));
        }

        /// <summary>
        /// Tests that Query returns the result from the object mapper which can map domain object in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatQueryReturnResultFromMapOnFoodWasteObjectMapper()
        {
            var fixture = new Fixture();

            var translationInfoMockCollection = DomainObjectMockBuilder.BuildTranslationInfoMockCollection().ToList();
            var systemDataRepository = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepository.Stub(m => m.TranslationInfoGetAll())
                .Return(translationInfoMockCollection)
                .Repeat.Any();

            var translationInfoSystemViewCollection = fixture.CreateMany<TranslationInfoSystemView>(translationInfoMockCollection.Count).ToList();
            var foodWasteObjectMapper = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapper.Stub(m => m.Map<IEnumerable<ITranslationInfo>, IEnumerable<TranslationInfoSystemView>>(Arg<IEnumerable<ITranslationInfo>>.Is.NotNull, Arg<CultureInfo>.Is.Anything))
                .Return(translationInfoSystemViewCollection)
                .Repeat.Any();

            var translationInfoCollectionGetQueryHandler = new TranslationInfoCollectionGetQueryHandler(systemDataRepository, foodWasteObjectMapper);
            Assert.That(translationInfoCollectionGetQueryHandler, Is.Not.Null);

            var result = translationInfoCollectionGetQueryHandler.Query(fixture.Create<TranslationInfoCollectionGetQuery>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(translationInfoSystemViewCollection));
        }
    }
}
