using System;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.QueryHandlers.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste;
using AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers.Core
{
    /// <summary>
    /// Tests the functionality which handles a query for getting a specific static text.
    /// </summary>
    [TestFixture]
    public class StaticTextGetQueryHandlerBaseTests
    {
        /// <summary>
        /// Private class for a query which can get a specific static text.
        /// </summary>
        private class MyStaticTextGetQuery : StaticTextGetQueryBase
        {
        }

        /// <summary>
        /// Private class for testing the functionality which handles a query for getting a specific static text.
        /// </summary>
        private class MyStaticTextGetQueryHandler : StaticTextGetQueryHandlerBase<MyStaticTextGetQuery>
        {
            #region Private variables

            private readonly StaticTextType _staticTextType;

            #endregion

            #region Constructor

            /// <summary>
            /// Creates an instance of the private class for testing the functionality which handles a query for getting a specific static text.
            /// </summary>
            /// <param name="staticTextType">Type of the static text to get.</param>
            /// <param name="systemDataRepository">Implementation for a repository which can access system data in the food waste domain.</param>
            /// <param name="foodWasteObjectMapper">Implementation for a object mapper which can map domain object in the food waste domain.</param>
            public MyStaticTextGetQueryHandler(StaticTextType staticTextType, ISystemDataRepository systemDataRepository, IFoodWasteObjectMapper foodWasteObjectMapper)
                : base(systemDataRepository, foodWasteObjectMapper)
            {
                _staticTextType = staticTextType;
            }

            #endregion

            #region Properties

            /// <summary>
            /// Gets the type for the specific static text to get.
            /// </summary>
            public override StaticTextType StaticTextType
            {
                get { return _staticTextType; }
            }

            #endregion
        }

        /// <summary>
        /// Test that the constructor initialize the functionality which handles a query for getting a specific static text.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeStaticTextGetQueryHandlerBase()
        {
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            foreach (var staticTextTypeToTest in Enum.GetValues(typeof (StaticTextType)).Cast<StaticTextType>())
            {
                var staticTextGetQueryHandlerBase = new MyStaticTextGetQueryHandler(staticTextTypeToTest, systemDataRepositoryMock, foodWasteObjectMapperMock);
                Assert.That(staticTextGetQueryHandlerBase, Is.Not.Null);
                Assert.That(staticTextGetQueryHandlerBase.StaticTextType, Is.EqualTo(staticTextTypeToTest));
            }
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the repository which can access system data in the food waste domain is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenSystemDataRepositoryIsNull()
        {
            var fixture = new Fixture();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyStaticTextGetQueryHandler(fixture.Create<StaticTextType>(), null, foodWasteObjectMapperMock));
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

            var exception = Assert.Throws<ArgumentNullException>(() => new MyStaticTextGetQueryHandler(fixture.Create<StaticTextType>(), systemDataRepositoryMock, null));
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
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var staticTextGetQueryHandlerBase = new MyStaticTextGetQueryHandler(fixture.Create<StaticTextType>(), systemDataRepositoryMock, foodWasteObjectMapperMock);
            Assert.That(staticTextGetQueryHandlerBase, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => staticTextGetQueryHandlerBase.Query(null));
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
            systemDataRepositoryMock.Stub(m => m.StaticTextGetByStaticTextType(Arg<StaticTextType>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildStaticTextMock())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IStaticText, StaticTextView>(Arg<IStaticText>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<StaticTextView>())
                .Repeat.Any();

            var staticTextGetQueryHandlerBase = new MyStaticTextGetQueryHandler(fixture.Create<StaticTextType>(), systemDataRepositoryMock, foodWasteObjectMapperMock);
            Assert.That(staticTextGetQueryHandlerBase, Is.Not.Null);

            var translationInfoIdentifier = Guid.NewGuid();
            var staticTextGetQuery = fixture.Build<MyStaticTextGetQuery>()
                .With(m => m.TranslationInfoIdentifier, translationInfoIdentifier)
                .Create();

            staticTextGetQueryHandlerBase.Query(staticTextGetQuery);

            systemDataRepositoryMock.AssertWasCalled(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Equal(translationInfoIdentifier)));
        }

        /// <summary>
        /// Tests that Query calls StaticTextGetByStaticTextType on the repository which can access system data in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatQueryStaticTextGetByStaticTextTypeOnSystemDataRepository()
        {
            var fixture = new Fixture();
            
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.StaticTextGetByStaticTextType(Arg<StaticTextType>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildStaticTextMock())
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IStaticText, StaticTextView>(Arg<IStaticText>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<StaticTextView>())
                .Repeat.Any();

            foreach (var staticTextTypeToTest in Enum.GetValues(typeof(StaticTextType)).Cast<StaticTextType>())
            {
                var staticTextGetQueryHandlerBase = new MyStaticTextGetQueryHandler(staticTextTypeToTest, systemDataRepositoryMock, foodWasteObjectMapperMock);
                Assert.That(staticTextGetQueryHandlerBase, Is.Not.Null);

                var staticTextGetQuery = fixture.Build<MyStaticTextGetQuery>()
                    .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                    .Create();

                staticTextGetQueryHandlerBase.Query(staticTextGetQuery);

                // ReSharper disable AccessToForEachVariableInClosure
                systemDataRepositoryMock.AssertWasCalled(m => m.StaticTextGetByStaticTextType(Arg<StaticTextType>.Is.Equal(staticTextTypeToTest)));
                // ReSharper restore AccessToForEachVariableInClosure
            }
        }

        /// <summary>
        /// Tests that Query calls Map on the object mapper which can map domain object in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatQueryCallsMapOnFoodWasteObjectMapper()
        {
            var fixture = new Fixture();

            var translationInfoMock = DomainObjectMockBuilder.BuildTranslationInfoMock();
            var staticTextMock = DomainObjectMockBuilder.BuildStaticTextMock();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(translationInfoMock)
                .Repeat.Any();
            systemDataRepositoryMock.Stub(m => m.StaticTextGetByStaticTextType(Arg<StaticTextType>.Is.Anything))
                .Return(staticTextMock)
                .Repeat.Any();

            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IStaticText, StaticTextView>(Arg<IStaticText>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<StaticTextView>())
                .Repeat.Any();

            var staticTextGetQueryHandlerBase = new MyStaticTextGetQueryHandler(fixture.Create<StaticTextType>(), systemDataRepositoryMock, foodWasteObjectMapperMock);
            Assert.That(staticTextGetQueryHandlerBase, Is.Not.Null);

            var staticTextGetQuery = fixture.Build<MyStaticTextGetQuery>()
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            staticTextGetQueryHandlerBase.Query(staticTextGetQuery);

            foodWasteObjectMapperMock.AssertWasCalled(m => m.Map<IStaticText, StaticTextView>(Arg<IStaticText>.Is.Equal(staticTextMock), Arg<CultureInfo>.Is.Equal(translationInfoMock.CultureInfo)));
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
            systemDataRepositoryMock.Stub(m => m.StaticTextGetByStaticTextType(Arg<StaticTextType>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildStaticTextMock())
                .Repeat.Any();

            var staticTextView = fixture.Create<StaticTextView>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapperMock.Stub(m => m.Map<IStaticText, StaticTextView>(Arg<IStaticText>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(staticTextView)
                .Repeat.Any();

            var staticTextGetQueryHandlerBase = new MyStaticTextGetQueryHandler(fixture.Create<StaticTextType>(), systemDataRepositoryMock, foodWasteObjectMapperMock);
            Assert.That(staticTextGetQueryHandlerBase, Is.Not.Null);

            var staticTextGetQuery = fixture.Build<MyStaticTextGetQuery>()
                .With(m => m.TranslationInfoIdentifier, Guid.NewGuid())
                .Create();

            var result = staticTextGetQueryHandlerBase.Query(staticTextGetQuery);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(staticTextView));
        }
    }
}
