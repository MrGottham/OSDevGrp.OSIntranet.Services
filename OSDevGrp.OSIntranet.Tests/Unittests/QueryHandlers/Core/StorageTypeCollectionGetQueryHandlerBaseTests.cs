using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Views;
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
    /// Tests the functionality which handles a query for getting a collection of storage types.
    /// </summary>
    [TestFixture]
    public class StorageTypeCollectionGetQueryHandlerBaseTests
    {
        /// <summary>
        /// Private class for testing the functionality which handles a query for getting a collection of storage types.
        /// </summary>
        private class MyStorageTypeCollectionGetQueryHandler : StorageTypeCollectionGetQueryHandlerBase<StorageTypeCollectionGetQuery, StorageTypeIdentificationView>
        {
            #region Constructor

            /// <summary>
            /// Creates an instance of the private class for testing the functionality which handles a query for getting a collection of storage types.
            /// </summary>
            /// <param name="systemDataRepository">Implementation of the repository which can access system data for the food waste domain.</param>
            /// <param name="objectMapper">Implementation of the object mapper which can map objects in the food waste domain.</param>
            public MyStorageTypeCollectionGetQueryHandler(ISystemDataRepository systemDataRepository, IFoodWasteObjectMapper objectMapper)
                : base(systemDataRepository, objectMapper)
            {
            }
            
            #endregion
        }

        #region Private variables

        private ISystemDataRepository _systemDataRepositoryMock;
        private IFoodWasteObjectMapper _objectMapperMock;

        #endregion

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the repository which can access system data for the food waste domain is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenSystemDataRepositoryIsNull()
        {
            IFoodWasteObjectMapper objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new MyStorageTypeCollectionGetQueryHandler(null, objectMapperMock));
            // ReSharper restore ObjectCreationAsStatement

            TestHelper.AssertArgumentNullExceptionIsValid(exception, "systemDataRepository");
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the object mapper which can map objects in the food waste domain is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenFoodWasteObjectMapperIsNull()
        {
            ISystemDataRepository systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();

            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new MyStorageTypeCollectionGetQueryHandler(systemDataRepositoryMock, null));
            // ReSharper restore ObjectCreationAsStatement

            TestHelper.AssertArgumentNullExceptionIsValid(exception, "objectMapper");
        }

        /// <summary>
        /// Tests that Query throws an ArgumentNullException when the query for getting a collection of storage types is null.
        /// </summary>
        [Test]
        public void TestThatQueryThrowsArgumentNullExceptionWhenQueryIsNull()
        {
            Fixture fixture = new Fixture();

            MyStorageTypeCollectionGetQueryHandler sut = CreateSut(fixture);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => sut.Query(null));

            TestHelper.AssertArgumentNullExceptionIsValid(exception, "query");
        }

        /// <summary>
        /// Tests that Query calls Get to get the translation informations on the repository which can access system data for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatQueryCallsGetForTranslationInfoOnSystemDataRepository()
        {
            Fixture fixture = new Fixture();

            Guid translationInfoIdentifier = Guid.NewGuid();
            StorageTypeCollectionGetQuery query = CreateStorageTypeCollectionGetQuery(fixture, translationInfoIdentifier);

            MyStorageTypeCollectionGetQueryHandler sut = CreateSut(fixture);

            sut.Query(query);

            _systemDataRepositoryMock.AssertWasCalled(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Equal(translationInfoIdentifier)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that Query calls StorageTypeGetAll on the repository which can access system data for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatQueryCallsStorageTypeGetAllOnSystemDataRepository()
        {
            Fixture fixture = new Fixture();

            StorageTypeCollectionGetQuery query = CreateStorageTypeCollectionGetQuery(fixture);

            MyStorageTypeCollectionGetQueryHandler sut = CreateSut(fixture);

            sut.Query(query);

            _systemDataRepositoryMock.AssertWasCalled(m => m.StorageTypeGetAll(), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that Query calls Map on the object mapper which can map objects in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatQueryCallsMapOnFoodWasteObjectMapper()
        {
            Fixture fixture = new Fixture();

            StorageTypeCollectionGetQuery query = CreateStorageTypeCollectionGetQuery(fixture);

            ITranslationInfo translationInfo = DomainObjectMockBuilder.BuildTranslationInfoMock();
            IEnumerable<IStorageType> storageTypeCollection = DomainObjectMockBuilder.BuildStorageTypeMockCollection();

            MyStorageTypeCollectionGetQueryHandler sut = CreateSut(fixture, translationInfo, storageTypeCollection);

            sut.Query(query);

            _objectMapperMock.AssertWasCalled(m => m.Map<IEnumerable<IStorageType>, IEnumerable<StorageTypeIdentificationView>>(Arg<IEnumerable<IStorageType>>.Is.Equal(storageTypeCollection), Arg<CultureInfo>.Is.Equal(translationInfo.CultureInfo)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that Query return the mapped result of storage types.
        /// </summary>
        [Test]
        public void TestThatQueryReturnsMappedResult()
        {
            Fixture fixture = new Fixture();

            StorageTypeCollectionGetQuery query = CreateStorageTypeCollectionGetQuery(fixture);

            IEnumerable<StorageTypeIdentificationView> storageTypeIdentificationViewCollection = fixture.CreateMany<StorageTypeIdentificationView>(7);
            MyStorageTypeCollectionGetQueryHandler sut = CreateSut(fixture, storageTypeIdentificationViewCollection: storageTypeIdentificationViewCollection);

            IEnumerable<StorageTypeIdentificationView> result = sut.Query(query);
            // ReSharper disable PossibleMultipleEnumeration
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EqualTo(storageTypeIdentificationViewCollection));
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        /// Creates an instance of the private class for testing the functionality which handles a query for getting a collection of storage types.
        /// </summary>
        /// <param name="fixture">Auto fixture.</param>
        /// <param name="translationInfo">The translation informations which should be used when unit testing.</param>
        /// <param name="storageTypeCollection">The collection of storage types which should be used when unit testning.</param>
        /// <param name="storageTypeIdentificationViewCollection">The collection of the view for storage type identification which should be used when unit testing.</param>
        /// <returns>Instance of the private class for testing the functionality which handles a query for getting a collection of storage types.</returns>
        private MyStorageTypeCollectionGetQueryHandler CreateSut(Fixture fixture, ITranslationInfo translationInfo = null, IEnumerable<IStorageType> storageTypeCollection = null, IEnumerable<StorageTypeIdentificationView> storageTypeIdentificationViewCollection = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            _systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            _systemDataRepositoryMock.Stub(m => m.Get<ITranslationInfo>(Arg<Guid>.Is.Anything))
                .Return(translationInfo ?? DomainObjectMockBuilder.BuildTranslationInfoMock())
                .Repeat.Any();
            _systemDataRepositoryMock.Stub(m => m.StorageTypeGetAll())
                .Return(storageTypeCollection ?? DomainObjectMockBuilder.BuildStorageTypeMockCollection())
                .Repeat.Any();

            _objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            _objectMapperMock.Stub(m => m.Map<IEnumerable<IStorageType>, IEnumerable<StorageTypeIdentificationView>>(Arg<IEnumerable<IStorageType>>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(storageTypeIdentificationViewCollection ?? fixture.CreateMany<StorageTypeIdentificationView>(7).ToList())
                .Repeat.Any();

            return new MyStorageTypeCollectionGetQueryHandler(_systemDataRepositoryMock, _objectMapperMock);
        }

        /// <summary>
        /// Creates the a query for getting a collection of storage types.
        /// </summary>
        /// <param name="fixture">Auto fixture.</param>
        /// <param name="translationInfoIdentifier">The translation informations identifier for the query.</param>
        /// <returns>Query for getting a collection of storage types.</returns>
        private StorageTypeCollectionGetQuery CreateStorageTypeCollectionGetQuery(Fixture fixture, Guid? translationInfoIdentifier = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return fixture.Build<StorageTypeCollectionGetQuery>()
                .With(m => m.TranslationInfoIdentifier, translationInfoIdentifier ?? Guid.NewGuid())
                .Create();
        }
    }
}
