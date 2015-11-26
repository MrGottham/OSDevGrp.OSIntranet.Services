using System;
using System.Globalization;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Resources;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.FoodWaste
{
    /// <summary>
    /// Tests the basic functionality to repositories in the food waste domain.
    /// </summary>
    [TestFixture]
    public class DataRepositoryBaseTests
    {
        /// <summary>
        /// Private class for testing the basic functionality used by repositories in the food waste domain.
        /// </summary>
        private class MyDataRepository : DataRepositoryBase
        {
            #region Constructor

            /// <summary>
            /// Creates a private class for testing the basic functionality used by repositories in the food waste domain.
            /// </summary>
            /// <param name="foodWasteDataProvider">Implementation of a data provider which can access data in the food waste repository.</param>
            /// <param name="foodWasteObjectMapper">Implementation of an object mapper which can map objects in the food waste domain.</param>
            public MyDataRepository(IFoodWasteDataProvider foodWasteDataProvider, IFoodWasteObjectMapper foodWasteObjectMapper) 
                : base(foodWasteDataProvider, foodWasteObjectMapper)
            {
            }

            #endregion

            #region Methods

            /// <summary>
            /// Gets the data provider which can access data in the food waste repository.
            /// </summary>
            /// <returns>Data provider which can access data in the food waste repository.</returns>
            public IFoodWasteDataProvider GetDataProvider()
            {
                return DataProvider;
            }

            /// <summary>
            /// Gets the object mapper which can map objects in the food waste domain.
            /// </summary>
            /// <returns>Object mapper which can map objects in the food waste domain.</returns>
            public IFoodWasteObjectMapper GetObjectMapper()
            {
                return ObjectMapper;
            }

            #endregion
        }

        /// <summary>
        /// Tests that the constructor initialize the basic functionality used by repositories in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeDataRepositoryBase()
        {
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            var foodWasteObjectMapper = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var dataRepositoryBase = new MyDataRepository(foodWasteDataProviderMock, foodWasteObjectMapper);
            Assert.That(dataRepositoryBase, Is.Not.Null);
            Assert.That(dataRepositoryBase.GetDataProvider(), Is.Not.Null);
            Assert.That(dataRepositoryBase.GetDataProvider(), Is.EqualTo(foodWasteDataProviderMock));
            Assert.That(dataRepositoryBase.GetObjectMapper(), Is.Not.Null);
            Assert.That(dataRepositoryBase.GetObjectMapper(), Is.EqualTo(foodWasteObjectMapper));
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException if the data provider which can access data in the food waste repository is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionIfFoodWasteDataProviderIsNull()
        {
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyDataRepository(null, foodWasteObjectMapperMock));
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

            var exception = Assert.Throws<ArgumentNullException>(() => new MyDataRepository(foodWasteDataProviderMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("foodWasteObjectMapper"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Get calls Get on the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatGetCallsGetOnFoodWasteDataProvider()
        {
            TestThatGetCallsGetOnFoodWasteDataProvider<IHouseholdMember, HouseholdMemberProxy>(Guid.NewGuid());
            TestThatGetCallsGetOnFoodWasteDataProvider<IFoodItem, FoodItemProxy>(Guid.NewGuid());
            TestThatGetCallsGetOnFoodWasteDataProvider<IFoodGroup, FoodGroupProxy>(Guid.NewGuid());
            TestThatGetCallsGetOnFoodWasteDataProvider<IForeignKey, ForeignKeyProxy>(Guid.NewGuid());
            TestThatGetCallsGetOnFoodWasteDataProvider<IDataProvider, DataProviderProxy>(Guid.NewGuid());
            TestThatGetCallsGetOnFoodWasteDataProvider<ITranslation, TranslationProxy>(Guid.NewGuid());
            TestThatGetCallsGetOnFoodWasteDataProvider<ITranslationInfo, TranslationInfoProxy>(Guid.NewGuid());
        }

        /// <summary>
        /// Tests that Get returns the received data proxy from the food waste repository.
        /// </summary>
        [Test]
        public void TestThatGetReturnsReceivedDataProxy()
        {
            TestThatGetReturnsReceivedDataProxy<IHouseholdMember, HouseholdMemberProxy>(Guid.NewGuid());
            TestThatGetReturnsReceivedDataProxy<IFoodItem, FoodItemProxy>(Guid.NewGuid());
            TestThatGetReturnsReceivedDataProxy<IFoodGroup, FoodGroupProxy>(Guid.NewGuid());
            TestThatGetReturnsReceivedDataProxy<IForeignKey, ForeignKeyProxy>(Guid.NewGuid());
            TestThatGetReturnsReceivedDataProxy<IDataProvider, DataProviderProxy>(Guid.NewGuid());
            TestThatGetReturnsReceivedDataProxy<ITranslation, TranslationProxy>(Guid.NewGuid());
            TestThatGetReturnsReceivedDataProxy<ITranslationInfo, TranslationInfoProxy>(Guid.NewGuid());
        }

        /// <summary>
        /// Tests that Get throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        [Test]
        public void TestThatGetThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs()
        {
            TestThatGetThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs<IHouseholdMember, HouseholdMemberProxy>(Guid.NewGuid());
            TestThatGetThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs<IFoodItem, FoodItemProxy>(Guid.NewGuid());
            TestThatGetThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs<IFoodGroup, FoodGroupProxy>(Guid.NewGuid());
            TestThatGetThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs<IForeignKey, ForeignKeyProxy>(Guid.NewGuid());
            TestThatGetThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs<IDataProvider, DataProviderProxy>(Guid.NewGuid());
            TestThatGetThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs<ITranslation, TranslationProxy>(Guid.NewGuid());
            TestThatGetThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs<ITranslationInfo, TranslationInfoProxy>(Guid.NewGuid());
        }

        /// <summary>
        /// Tests that Get throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        [Test]
        public void TestThatGetThrowsIntranetRepositoryExceptionWhenExceptionOccurs()
        {
            TestThatGetThrowsIntranetRepositoryExceptionWhenExceptionOccurs<IHouseholdMember, HouseholdMemberProxy>(Guid.NewGuid());
            TestThatGetThrowsIntranetRepositoryExceptionWhenExceptionOccurs<IFoodItem, FoodItemProxy>(Guid.NewGuid());
            TestThatGetThrowsIntranetRepositoryExceptionWhenExceptionOccurs<IFoodGroup, FoodGroupProxy>(Guid.NewGuid());
            TestThatGetThrowsIntranetRepositoryExceptionWhenExceptionOccurs<IForeignKey, ForeignKeyProxy>(Guid.NewGuid());
            TestThatGetThrowsIntranetRepositoryExceptionWhenExceptionOccurs<IDataProvider, DataProviderProxy>(Guid.NewGuid());
            TestThatGetThrowsIntranetRepositoryExceptionWhenExceptionOccurs<ITranslation, TranslationProxy>(Guid.NewGuid());
            TestThatGetThrowsIntranetRepositoryExceptionWhenExceptionOccurs<ITranslationInfo, TranslationInfoProxy>(Guid.NewGuid());
        }

        /// <summary>
        /// Tests that Insert throws an ArgumentNullException when the identifiable domain object is null.
        /// </summary>
        [Test]
        public void TestThatInsertThrowsArgumentNullExceptionWhenIdentifiableIsNull()
        {
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            var foodWasteObjectMapper = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var dataRepositoryBase = new MyDataRepository(foodWasteDataProviderMock, foodWasteObjectMapper);
            Assert.That(dataRepositoryBase, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => dataRepositoryBase.Insert((IIdentifiable) null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("identifiable"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Insert calls Map on the object mapper which can map objects in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatInsertCallsMapOnFoodWasteObjectMapper()
        {
            TestThatInsertCallsMapOnFoodWasteObjectMapper(MockRepository.GenerateMock<IHouseholdMember>(), MockRepository.GenerateMock<IHouseholdMemberProxy>());
            TestThatInsertCallsMapOnFoodWasteObjectMapper(MockRepository.GenerateMock<IFoodItem>(), MockRepository.GenerateMock<IFoodItemProxy>());
            TestThatInsertCallsMapOnFoodWasteObjectMapper(MockRepository.GenerateMock<IFoodGroup>(), MockRepository.GenerateMock<IFoodGroupProxy>());
            TestThatInsertCallsMapOnFoodWasteObjectMapper(MockRepository.GenerateMock<IForeignKey>(), MockRepository.GenerateMock<IForeignKeyProxy>());
            TestThatInsertCallsMapOnFoodWasteObjectMapper(MockRepository.GenerateMock<IDataProvider>(), MockRepository.GenerateMock<IDataProviderProxy>());
            TestThatInsertCallsMapOnFoodWasteObjectMapper(MockRepository.GenerateMock<ITranslation>(), MockRepository.GenerateMock<ITranslationProxy>());
            TestThatInsertCallsMapOnFoodWasteObjectMapper(MockRepository.GenerateMock<ITranslationInfo>(), MockRepository.GenerateMock<ITranslationInfoProxy>());
        }

        /// <summary>
        /// Tests that Insert calls Add on the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatInsertCallsAddOnFoodWasteDataProvider()
        {
            TestThatInsertCallsAddOnFoodWasteDataProvider(MockRepository.GenerateMock<IHouseholdMember>(), MockRepository.GenerateMock<IHouseholdMemberProxy>());
            TestThatInsertCallsAddOnFoodWasteDataProvider(MockRepository.GenerateMock<IFoodItem>(), MockRepository.GenerateMock<IFoodItemProxy>());
            TestThatInsertCallsAddOnFoodWasteDataProvider(MockRepository.GenerateMock<IFoodGroup>(), MockRepository.GenerateMock<IFoodGroupProxy>());
            TestThatInsertCallsAddOnFoodWasteDataProvider(MockRepository.GenerateMock<IForeignKey>(), MockRepository.GenerateMock<IForeignKeyProxy>());
            TestThatInsertCallsAddOnFoodWasteDataProvider(MockRepository.GenerateMock<IDataProvider>(), MockRepository.GenerateMock<IDataProviderProxy>());
            TestThatInsertCallsAddOnFoodWasteDataProvider(MockRepository.GenerateMock<ITranslation>(), MockRepository.GenerateMock<ITranslationProxy>());
            TestThatInsertCallsAddOnFoodWasteDataProvider(MockRepository.GenerateMock<ITranslationInfo>(), MockRepository.GenerateMock<ITranslationInfoProxy>());
        }

        /// <summary>
        /// Tests that Insert returns the inserted data proxy.
        /// </summary>
        [Test]
        public void TestThatInsertReturnsInsertedDataProxy()
        {
            TestThatInsertReturnsInsertedDataProxy(MockRepository.GenerateMock<IHouseholdMember>(), MockRepository.GenerateMock<IHouseholdMemberProxy>());
            TestThatInsertReturnsInsertedDataProxy(MockRepository.GenerateMock<IFoodItem>(), MockRepository.GenerateMock<IFoodItemProxy>());
            TestThatInsertReturnsInsertedDataProxy(MockRepository.GenerateMock<IFoodGroup>(), MockRepository.GenerateMock<IFoodGroupProxy>());
            TestThatInsertReturnsInsertedDataProxy(MockRepository.GenerateMock<IForeignKey>(), MockRepository.GenerateMock<IForeignKeyProxy>());
            TestThatInsertReturnsInsertedDataProxy(MockRepository.GenerateMock<IDataProvider>(), MockRepository.GenerateMock<IDataProviderProxy>());
            TestThatInsertReturnsInsertedDataProxy(MockRepository.GenerateMock<ITranslation>(), MockRepository.GenerateMock<ITranslationProxy>());
            TestThatInsertReturnsInsertedDataProxy(MockRepository.GenerateMock<ITranslationInfo>(), MockRepository.GenerateMock<ITranslationInfoProxy>());
        }

        /// <summary>
        /// Tests that Insert throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        [Test]
        public void TestThatInsertThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs()
        {
            TestThatInsertThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs(MockRepository.GenerateMock<IHouseholdMember>(), MockRepository.GenerateMock<IHouseholdMemberProxy>());
            TestThatInsertThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs(MockRepository.GenerateMock<IFoodItem>(), MockRepository.GenerateMock<IFoodItemProxy>());
            TestThatInsertThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs(MockRepository.GenerateMock<IFoodGroup>(), MockRepository.GenerateMock<IFoodGroupProxy>());
            TestThatInsertThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs(MockRepository.GenerateMock<IForeignKey>(), MockRepository.GenerateMock<IForeignKeyProxy>());
            TestThatInsertThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs(MockRepository.GenerateMock<IDataProvider>(), MockRepository.GenerateMock<IDataProviderProxy>());
            TestThatInsertThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs(MockRepository.GenerateMock<ITranslation>(), MockRepository.GenerateMock<ITranslationProxy>());
            TestThatInsertThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs(MockRepository.GenerateMock<ITranslationInfo>(), MockRepository.GenerateMock<ITranslationInfoProxy>());
        }

        /// <summary>
        /// Tests that Insert throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        [Test]
        public void TestThatInsertThrowsIntranetRepositoryExceptionWhenExceptionOccurs()
        {
            TestThatInsertThrowsIntranetRepositoryExceptionWhenExceptionOccurs(MockRepository.GenerateMock<IHouseholdMember>(), MockRepository.GenerateMock<IHouseholdMemberProxy>());
            TestThatInsertThrowsIntranetRepositoryExceptionWhenExceptionOccurs(MockRepository.GenerateMock<IFoodItem>(), MockRepository.GenerateMock<IFoodItemProxy>());
            TestThatInsertThrowsIntranetRepositoryExceptionWhenExceptionOccurs(MockRepository.GenerateMock<IFoodGroup>(), MockRepository.GenerateMock<IFoodGroupProxy>());
            TestThatInsertThrowsIntranetRepositoryExceptionWhenExceptionOccurs(MockRepository.GenerateMock<IForeignKey>(), MockRepository.GenerateMock<IForeignKeyProxy>());
            TestThatInsertThrowsIntranetRepositoryExceptionWhenExceptionOccurs(MockRepository.GenerateMock<IDataProvider>(), MockRepository.GenerateMock<IDataProviderProxy>());
            TestThatInsertThrowsIntranetRepositoryExceptionWhenExceptionOccurs(MockRepository.GenerateMock<ITranslation>(), MockRepository.GenerateMock<ITranslationProxy>());
            TestThatInsertThrowsIntranetRepositoryExceptionWhenExceptionOccurs(MockRepository.GenerateMock<ITranslationInfo>(), MockRepository.GenerateMock<ITranslationInfoProxy>());
        }

        /// <summary>
        /// Tests that Update throws an ArgumentNullException when the identifiable domain object is null.
        /// </summary>
        [Test]
        public void TestThatUpdateThrowsArgumentNullExceptionWhenIdentifiableIsNull()
        {
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            var foodWasteObjectMapper = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var dataRepositoryBase = new MyDataRepository(foodWasteDataProviderMock, foodWasteObjectMapper);
            Assert.That(dataRepositoryBase, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => dataRepositoryBase.Update((IIdentifiable) null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("identifiable"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Update calls Map on the object mapper which can map objects in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatUpdateCallsMapOnFoodWasteObjectMapper()
        {
            TestThatUpdateCallsMapOnFoodWasteObjectMapper(MockRepository.GenerateMock<IHouseholdMember>(), MockRepository.GenerateMock<IHouseholdMemberProxy>());
            TestThatUpdateCallsMapOnFoodWasteObjectMapper(MockRepository.GenerateMock<IFoodItem>(), MockRepository.GenerateMock<IFoodItemProxy>());
            TestThatUpdateCallsMapOnFoodWasteObjectMapper(MockRepository.GenerateMock<IFoodGroup>(), MockRepository.GenerateMock<IFoodGroupProxy>());
            TestThatUpdateCallsMapOnFoodWasteObjectMapper(MockRepository.GenerateMock<IForeignKey>(), MockRepository.GenerateMock<IForeignKeyProxy>());
            TestThatUpdateCallsMapOnFoodWasteObjectMapper(MockRepository.GenerateMock<IDataProvider>(), MockRepository.GenerateMock<IDataProviderProxy>());
            TestThatUpdateCallsMapOnFoodWasteObjectMapper(MockRepository.GenerateMock<ITranslation>(), MockRepository.GenerateMock<ITranslationProxy>());
            TestThatUpdateCallsMapOnFoodWasteObjectMapper(MockRepository.GenerateMock<ITranslationInfo>(), MockRepository.GenerateMock<ITranslationInfoProxy>());
        }

        /// <summary>
        /// Tests that Update calls Save on the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatUpdateCallsSaveOnFoodWasteDataProvider()
        {
            TestThatUpdateCallsSaveOnFoodWasteDataProvider(MockRepository.GenerateMock<IHouseholdMember>(), MockRepository.GenerateMock<IHouseholdMemberProxy>());
            TestThatUpdateCallsSaveOnFoodWasteDataProvider(MockRepository.GenerateMock<IFoodItem>(), MockRepository.GenerateMock<IFoodItemProxy>());
            TestThatUpdateCallsSaveOnFoodWasteDataProvider(MockRepository.GenerateMock<IFoodGroup>(), MockRepository.GenerateMock<IFoodGroupProxy>());
            TestThatUpdateCallsSaveOnFoodWasteDataProvider(MockRepository.GenerateMock<IForeignKey>(), MockRepository.GenerateMock<IForeignKeyProxy>());
            TestThatUpdateCallsSaveOnFoodWasteDataProvider(MockRepository.GenerateMock<IDataProvider>(), MockRepository.GenerateMock<IDataProviderProxy>());
            TestThatUpdateCallsSaveOnFoodWasteDataProvider(MockRepository.GenerateMock<ITranslation>(), MockRepository.GenerateMock<ITranslationProxy>());
            TestThatUpdateCallsSaveOnFoodWasteDataProvider(MockRepository.GenerateMock<ITranslationInfo>(), MockRepository.GenerateMock<ITranslationInfoProxy>());
        }

        /// <summary>
        /// Tests that Update returns the updated data proxy.
        /// </summary>
        [Test]
        public void TestThatUpdateReturnsUpdatedDataProxy()
        {
            TestThatUpdateReturnsUpdatedDataProxy(MockRepository.GenerateMock<IHouseholdMember>(), MockRepository.GenerateMock<IHouseholdMemberProxy>());
            TestThatUpdateReturnsUpdatedDataProxy(MockRepository.GenerateMock<IFoodItem>(), MockRepository.GenerateMock<IFoodItemProxy>());
            TestThatUpdateReturnsUpdatedDataProxy(MockRepository.GenerateMock<IFoodGroup>(), MockRepository.GenerateMock<IFoodGroupProxy>());
            TestThatUpdateReturnsUpdatedDataProxy(MockRepository.GenerateMock<IForeignKey>(), MockRepository.GenerateMock<IForeignKeyProxy>());
            TestThatUpdateReturnsUpdatedDataProxy(MockRepository.GenerateMock<IDataProvider>(), MockRepository.GenerateMock<IDataProviderProxy>());
            TestThatUpdateReturnsUpdatedDataProxy(MockRepository.GenerateMock<ITranslation>(), MockRepository.GenerateMock<ITranslationProxy>());
            TestThatUpdateReturnsUpdatedDataProxy(MockRepository.GenerateMock<ITranslationInfo>(), MockRepository.GenerateMock<ITranslationInfoProxy>());
        }

        /// <summary>
        /// Tests that Update throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        [Test]
        public void TestThatUpdateThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs()
        {
            TestThatUpdateThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs(MockRepository.GenerateMock<IHouseholdMember>(), MockRepository.GenerateMock<IHouseholdMemberProxy>());
            TestThatUpdateThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs(MockRepository.GenerateMock<IFoodItem>(), MockRepository.GenerateMock<IFoodItemProxy>());
            TestThatUpdateThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs(MockRepository.GenerateMock<IFoodGroup>(), MockRepository.GenerateMock<IFoodGroupProxy>());
            TestThatUpdateThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs(MockRepository.GenerateMock<IForeignKey>(), MockRepository.GenerateMock<IForeignKeyProxy>());
            TestThatUpdateThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs(MockRepository.GenerateMock<IDataProvider>(), MockRepository.GenerateMock<IDataProviderProxy>());
            TestThatUpdateThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs(MockRepository.GenerateMock<ITranslation>(), MockRepository.GenerateMock<ITranslationProxy>());
            TestThatUpdateThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs(MockRepository.GenerateMock<ITranslationInfo>(), MockRepository.GenerateMock<ITranslationInfoProxy>());
        }

        /// <summary>
        /// Tests that Update throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        [Test]
        public void TestThatUpdateThrowsIntranetRepositoryExceptionWhenExceptionOccurs()
        {
            TestThatUpdateThrowsIntranetRepositoryExceptionWhenExceptionOccurs(MockRepository.GenerateMock<IHouseholdMember>(), MockRepository.GenerateMock<IHouseholdMemberProxy>());
            TestThatUpdateThrowsIntranetRepositoryExceptionWhenExceptionOccurs(MockRepository.GenerateMock<IFoodItem>(), MockRepository.GenerateMock<IFoodItemProxy>());
            TestThatUpdateThrowsIntranetRepositoryExceptionWhenExceptionOccurs(MockRepository.GenerateMock<IFoodGroup>(), MockRepository.GenerateMock<IFoodGroupProxy>());
            TestThatUpdateThrowsIntranetRepositoryExceptionWhenExceptionOccurs(MockRepository.GenerateMock<IForeignKey>(), MockRepository.GenerateMock<IForeignKeyProxy>());
            TestThatUpdateThrowsIntranetRepositoryExceptionWhenExceptionOccurs(MockRepository.GenerateMock<IDataProvider>(), MockRepository.GenerateMock<IDataProviderProxy>());
            TestThatUpdateThrowsIntranetRepositoryExceptionWhenExceptionOccurs(MockRepository.GenerateMock<ITranslation>(), MockRepository.GenerateMock<ITranslationProxy>());
            TestThatUpdateThrowsIntranetRepositoryExceptionWhenExceptionOccurs(MockRepository.GenerateMock<ITranslationInfo>(), MockRepository.GenerateMock<ITranslationInfoProxy>());
        }

        /// <summary>
        /// Tests that Delete throws an ArgumentNullException when the identifiable domain object is null.
        /// </summary>
        [Test]
        public void TestThatDeleteThrowsArgumentNullExceptionWhenIdentifiableIsNull()
        {
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            var foodWasteObjectMapper = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var dataRepositoryBase = new MyDataRepository(foodWasteDataProviderMock, foodWasteObjectMapper);
            Assert.That(dataRepositoryBase, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => dataRepositoryBase.Delete((IIdentifiable) null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("identifiable"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Delete calls Map on the object mapper which can map objects in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatDeleteCallsMapOnFoodWasteObjectMapper()
        {
            TestThatDeleteCallsMapOnFoodWasteObjectMapper(MockRepository.GenerateMock<IHouseholdMember>(), MockRepository.GenerateMock<IHouseholdMemberProxy>());
            TestThatDeleteCallsMapOnFoodWasteObjectMapper(MockRepository.GenerateMock<IFoodItem>(), MockRepository.GenerateMock<IFoodItemProxy>());
            TestThatDeleteCallsMapOnFoodWasteObjectMapper(MockRepository.GenerateMock<IFoodGroup>(), MockRepository.GenerateMock<IFoodGroupProxy>());
            TestThatDeleteCallsMapOnFoodWasteObjectMapper(MockRepository.GenerateMock<IForeignKey>(), MockRepository.GenerateMock<IForeignKeyProxy>());
            TestThatDeleteCallsMapOnFoodWasteObjectMapper(MockRepository.GenerateMock<IDataProvider>(), MockRepository.GenerateMock<IDataProviderProxy>());
            TestThatDeleteCallsMapOnFoodWasteObjectMapper(MockRepository.GenerateMock<ITranslation>(), MockRepository.GenerateMock<ITranslationProxy>());
            TestThatDeleteCallsMapOnFoodWasteObjectMapper(MockRepository.GenerateMock<ITranslationInfo>(), MockRepository.GenerateMock<ITranslationInfoProxy>());
        }

        /// <summary>
        /// Tests that Delete calls Delete on the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatDeleteCallsDeleteOnFoodWasteDataProvider()
        {
            TestThatDeleteCallsDeleteOnFoodWasteDataProvider(MockRepository.GenerateMock<IHouseholdMember>(), MockRepository.GenerateMock<IHouseholdMemberProxy>());
            TestThatDeleteCallsDeleteOnFoodWasteDataProvider(MockRepository.GenerateMock<IFoodItem>(), MockRepository.GenerateMock<IFoodItemProxy>());
            TestThatDeleteCallsDeleteOnFoodWasteDataProvider(MockRepository.GenerateMock<IFoodGroup>(), MockRepository.GenerateMock<IFoodGroupProxy>());
            TestThatDeleteCallsDeleteOnFoodWasteDataProvider(MockRepository.GenerateMock<IForeignKey>(), MockRepository.GenerateMock<IForeignKeyProxy>());
            TestThatDeleteCallsDeleteOnFoodWasteDataProvider(MockRepository.GenerateMock<IDataProvider>(), MockRepository.GenerateMock<IDataProviderProxy>());
            TestThatDeleteCallsDeleteOnFoodWasteDataProvider(MockRepository.GenerateMock<ITranslation>(), MockRepository.GenerateMock<ITranslationProxy>());
            TestThatDeleteCallsDeleteOnFoodWasteDataProvider(MockRepository.GenerateMock<ITranslationInfo>(), MockRepository.GenerateMock<ITranslationInfoProxy>());
        }

        /// <summary>
        /// Tests that Delete throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        [Test]
        public void TestThatDeleteThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs()
        {
            TestThatDeleteThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs(MockRepository.GenerateMock<IHouseholdMember>(), MockRepository.GenerateMock<IHouseholdMemberProxy>());
            TestThatDeleteThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs(MockRepository.GenerateMock<IFoodItem>(), MockRepository.GenerateMock<IFoodItemProxy>());
            TestThatDeleteThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs(MockRepository.GenerateMock<IFoodGroup>(), MockRepository.GenerateMock<IFoodGroupProxy>());
            TestThatDeleteThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs(MockRepository.GenerateMock<IForeignKey>(), MockRepository.GenerateMock<IForeignKeyProxy>());
            TestThatDeleteThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs(MockRepository.GenerateMock<IDataProvider>(), MockRepository.GenerateMock<IDataProviderProxy>());
            TestThatDeleteThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs(MockRepository.GenerateMock<ITranslation>(), MockRepository.GenerateMock<ITranslationProxy>());
            TestThatDeleteThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs(MockRepository.GenerateMock<ITranslationInfo>(), MockRepository.GenerateMock<ITranslationInfoProxy>());
        }

        /// <summary>
        /// Tests that Delete throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        [Test]
        public void TestThatDeleteThrowsIntranetRepositoryExceptionWhenExceptionOccurs()
        {
            TestThatDeleteThrowsIntranetRepositoryExceptionWhenExceptionOccurs(MockRepository.GenerateMock<IHouseholdMember>(), MockRepository.GenerateMock<IHouseholdMemberProxy>());
            TestThatDeleteThrowsIntranetRepositoryExceptionWhenExceptionOccurs(MockRepository.GenerateMock<IFoodItem>(), MockRepository.GenerateMock<IFoodItemProxy>());
            TestThatDeleteThrowsIntranetRepositoryExceptionWhenExceptionOccurs(MockRepository.GenerateMock<IFoodGroup>(), MockRepository.GenerateMock<IFoodGroupProxy>());
            TestThatDeleteThrowsIntranetRepositoryExceptionWhenExceptionOccurs(MockRepository.GenerateMock<IForeignKey>(), MockRepository.GenerateMock<IForeignKeyProxy>());
            TestThatDeleteThrowsIntranetRepositoryExceptionWhenExceptionOccurs(MockRepository.GenerateMock<IDataProvider>(), MockRepository.GenerateMock<IDataProviderProxy>());
            TestThatDeleteThrowsIntranetRepositoryExceptionWhenExceptionOccurs(MockRepository.GenerateMock<ITranslation>(), MockRepository.GenerateMock<ITranslationProxy>());
            TestThatDeleteThrowsIntranetRepositoryExceptionWhenExceptionOccurs(MockRepository.GenerateMock<ITranslationInfo>(), MockRepository.GenerateMock<ITranslationInfoProxy>());
        }

        /// <summary>
        /// Tests that Get calls Get on the data provider which can access data in the food waste repository.
        /// </summary>
        private static void TestThatGetCallsGetOnFoodWasteDataProvider<TIdentifiable, TDataProxy>(Guid identifier) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy<TIdentifiable>, new()
        {
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.Get(Arg<TDataProxy>.Is.NotNull))
                .Return(new TDataProxy())
                .Repeat.Any();

            var foodWasteObjectMapper = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var dataRepositoryBase = new MyDataRepository(foodWasteDataProviderMock, foodWasteObjectMapper);
            Assert.That(dataRepositoryBase, Is.Not.Null);

            dataRepositoryBase.Get<TIdentifiable>(identifier);

            foodWasteDataProviderMock.AssertWasCalled(m => m.Get(Arg<TDataProxy>.Is.NotNull));
        }

        /// <summary>
        /// Tests that Get returns the received data proxy from the food waste repository.
        /// </summary>
        private static void TestThatGetReturnsReceivedDataProxy<TIdentifiable, TDataProxy>(Guid identifier) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy<TIdentifiable>, new()
        {
            var dataProxy = new TDataProxy();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.Get(Arg<TDataProxy>.Is.NotNull))
                .Return(dataProxy)
                .Repeat.Any();

            var foodWasteObjectMapper = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var dataRepositoryBase = new MyDataRepository(foodWasteDataProviderMock, foodWasteObjectMapper);
            Assert.That(dataRepositoryBase, Is.Not.Null);

            var result = dataRepositoryBase.Get<TIdentifiable>(identifier);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(dataProxy));
        }

        /// <summary>
        /// Tests that Get throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        private static void TestThatGetThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs<TIdentifiable, TDataProxy>(Guid identifier) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy<TIdentifiable>, new()
        {
            var fixture = new Fixture();
            var exceptionToThrow = fixture.Create<IntranetRepositoryException>();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.Get(Arg<TDataProxy>.Is.NotNull))
                .Throw(exceptionToThrow)
                .Repeat.Any();

            var foodWasteObjectMapper = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var dataRepositoryBase = new MyDataRepository(foodWasteDataProviderMock, foodWasteObjectMapper);
            Assert.That(dataRepositoryBase, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => dataRepositoryBase.Get<TIdentifiable>(identifier));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that Get throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        private static void TestThatGetThrowsIntranetRepositoryExceptionWhenExceptionOccurs<TIdentifiable, TDataProxy>(Guid identifier) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy<TIdentifiable>, new()
        {
            var fixture = new Fixture();
            var exceptionToThrow = fixture.Create<Exception>();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.Get(Arg<TDataProxy>.Is.NotNull))
                .Throw(exceptionToThrow)
                .Repeat.Any();

            var foodWasteObjectMapper = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var dataRepositoryBase = new MyDataRepository(foodWasteDataProviderMock, foodWasteObjectMapper);
            Assert.That(dataRepositoryBase, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => dataRepositoryBase.Get<TIdentifiable>(identifier));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "Get", exceptionToThrow.Message)));
            Assert.That(exception.InnerException, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that Insert calls Map on the object mapper which can map objects in the food waste domain.
        /// </summary>
        private static void TestThatInsertCallsMapOnFoodWasteObjectMapper<TIdentifiable, TDataProxy>(TIdentifiable identifiable, TDataProxy dataProxyMock) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy<TIdentifiable>
        {
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.Add(Arg<TDataProxy>.Is.NotNull))
                .Return(dataProxyMock)
                .Repeat.Any();

            var foodWasteObjectMapper = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapper.Stub(m => m.Map<TIdentifiable, TDataProxy>(Arg<TIdentifiable>.Is.NotNull, Arg<CultureInfo>.Is.Anything))
                .Return(dataProxyMock)
                .Repeat.Any();

            var dataRepositoryBase = new MyDataRepository(foodWasteDataProviderMock, foodWasteObjectMapper);
            Assert.That(dataRepositoryBase, Is.Not.Null);

            dataRepositoryBase.Insert(identifiable);

            foodWasteObjectMapper.AssertWasCalled(m => m.Map<TIdentifiable, TDataProxy>(Arg<TIdentifiable>.Is.Equal(identifiable), Arg<CultureInfo>.Is.Null));
        }

        /// <summary>
        /// Tests that Insert calls Add on the data provider which can access data in the food waste repository.
        /// </summary>
        private static void TestThatInsertCallsAddOnFoodWasteDataProvider<TIdentifiable, TDataProxy>(TIdentifiable identifiable, TDataProxy dataProxyMock) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy<TIdentifiable>
        {
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.Add(Arg<TDataProxy>.Is.NotNull))
                .Return(dataProxyMock)
                .Repeat.Any();

            var foodWasteObjectMapper = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapper.Stub(m => m.Map<TIdentifiable, TDataProxy>(Arg<TIdentifiable>.Is.NotNull, Arg<CultureInfo>.Is.Anything))
                .Return(dataProxyMock)
                .Repeat.Any();

            var dataRepositoryBase = new MyDataRepository(foodWasteDataProviderMock, foodWasteObjectMapper);
            Assert.That(dataRepositoryBase, Is.Not.Null);

            dataRepositoryBase.Insert(identifiable);

            foodWasteDataProviderMock.AssertWasCalled(m => m.Add(Arg<TDataProxy>.Is.Equal(dataProxyMock)));
        }

        /// <summary>
        /// Tests that Insert returns the inserted data proxy.
        /// </summary>
        private static void TestThatInsertReturnsInsertedDataProxy<TIdentifiable, TDataProxy>(TIdentifiable identifiable, TDataProxy dataProxyMock) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy<TIdentifiable>
        {
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.Add(Arg<TDataProxy>.Is.NotNull))
                .Return(dataProxyMock)
                .Repeat.Any();

            var foodWasteObjectMapper = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapper.Stub(m => m.Map<TIdentifiable, TDataProxy>(Arg<TIdentifiable>.Is.NotNull, Arg<CultureInfo>.Is.Anything))
                .Return(dataProxyMock)
                .Repeat.Any();

            var dataRepositoryBase = new MyDataRepository(foodWasteDataProviderMock, foodWasteObjectMapper);
            Assert.That(dataRepositoryBase, Is.Not.Null);

            var result = dataRepositoryBase.Insert(identifiable);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(dataProxyMock));
        }

        /// <summary>
        /// Tests that Insert throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        private static void TestThatInsertThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs<TIdentifiable, TDataProxy>(TIdentifiable identifiable, TDataProxy dataProxyMock) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy<TIdentifiable>
        {
            var fixture = new Fixture();
            var exceptionToThrow = fixture.Create<IntranetRepositoryException>();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.Add(Arg<TDataProxy>.Is.NotNull))
                .Throw(exceptionToThrow)
                .Repeat.Any();

            var foodWasteObjectMapper = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapper.Stub(m => m.Map<TIdentifiable, TDataProxy>(Arg<TIdentifiable>.Is.NotNull, Arg<CultureInfo>.Is.Anything))
                .Return(dataProxyMock)
                .Repeat.Any();

            var dataRepositoryBase = new MyDataRepository(foodWasteDataProviderMock, foodWasteObjectMapper);
            Assert.That(dataRepositoryBase, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => dataRepositoryBase.Insert(identifiable));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that Insert throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        private static void TestThatInsertThrowsIntranetRepositoryExceptionWhenExceptionOccurs<TIdentifiable, TDataProxy>(TIdentifiable identifiable, TDataProxy dataProxyMock) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy<TIdentifiable>
        {
            var fixture = new Fixture();
            var exceptionToThrow = fixture.Create<Exception>();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.Add(Arg<TDataProxy>.Is.NotNull))
                .Throw(exceptionToThrow)
                .Repeat.Any();

            var foodWasteObjectMapper = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapper.Stub(m => m.Map<TIdentifiable, TDataProxy>(Arg<TIdentifiable>.Is.NotNull, Arg<CultureInfo>.Is.Anything))
                .Return(dataProxyMock)
                .Repeat.Any();

            var dataRepositoryBase = new MyDataRepository(foodWasteDataProviderMock, foodWasteObjectMapper);
            Assert.That(dataRepositoryBase, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => dataRepositoryBase.Insert(identifiable));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "Insert", exceptionToThrow.Message)));
            Assert.That(exception.InnerException, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that Update calls Map on the object mapper which can map objects in the food waste domain.
        /// </summary>
        private static void TestThatUpdateCallsMapOnFoodWasteObjectMapper<TIdentifiable, TDataProxy>(TIdentifiable identifiable, TDataProxy dataProxyMock) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy<TIdentifiable>
        {
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.Save(Arg<TDataProxy>.Is.NotNull))
                .Return(dataProxyMock)
                .Repeat.Any();

            var foodWasteObjectMapper = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapper.Stub(m => m.Map<TIdentifiable, TDataProxy>(Arg<TIdentifiable>.Is.NotNull, Arg<CultureInfo>.Is.Anything))
                .Return(dataProxyMock)
                .Repeat.Any();

            var dataRepositoryBase = new MyDataRepository(foodWasteDataProviderMock, foodWasteObjectMapper);
            Assert.That(dataRepositoryBase, Is.Not.Null);

            dataRepositoryBase.Update(identifiable);

            foodWasteObjectMapper.AssertWasCalled(m => m.Map<TIdentifiable, TDataProxy>(Arg<TIdentifiable>.Is.Equal(identifiable), Arg<CultureInfo>.Is.Null));
        }

        /// <summary>
        /// Tests that Update calls Save on the data provider which can access data in the food waste repository.
        /// </summary>
        private static void TestThatUpdateCallsSaveOnFoodWasteDataProvider<TIdentifiable, TDataProxy>(TIdentifiable identifiable, TDataProxy dataProxyMock) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy<TIdentifiable>
        {
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.Save(Arg<TDataProxy>.Is.NotNull))
                .Return(dataProxyMock)
                .Repeat.Any();

            var foodWasteObjectMapper = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapper.Stub(m => m.Map<TIdentifiable, TDataProxy>(Arg<TIdentifiable>.Is.NotNull, Arg<CultureInfo>.Is.Anything))
                .Return(dataProxyMock)
                .Repeat.Any();

            var dataRepositoryBase = new MyDataRepository(foodWasteDataProviderMock, foodWasteObjectMapper);
            Assert.That(dataRepositoryBase, Is.Not.Null);

            dataRepositoryBase.Update(identifiable);

            foodWasteDataProviderMock.AssertWasCalled(m => m.Save(Arg<TDataProxy>.Is.Equal(dataProxyMock)));
        }

        /// <summary>
        /// Tests that Update returns the updated data proxy.
        /// </summary>
        private static void TestThatUpdateReturnsUpdatedDataProxy<TIdentifiable, TDataProxy>(TIdentifiable identifiable, TDataProxy dataProxyMock) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy<TIdentifiable>
        {
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.Save(Arg<TDataProxy>.Is.NotNull))
                .Return(dataProxyMock)
                .Repeat.Any();

            var foodWasteObjectMapper = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapper.Stub(m => m.Map<TIdentifiable, TDataProxy>(Arg<TIdentifiable>.Is.NotNull, Arg<CultureInfo>.Is.Anything))
                .Return(dataProxyMock)
                .Repeat.Any();

            var dataRepositoryBase = new MyDataRepository(foodWasteDataProviderMock, foodWasteObjectMapper);
            Assert.That(dataRepositoryBase, Is.Not.Null);

            var result = dataRepositoryBase.Update(identifiable);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(dataProxyMock));
        }

        /// <summary>
        /// Tests that Update throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        private static void TestThatUpdateThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs<TIdentifiable, TDataProxy>(TIdentifiable identifiable, TDataProxy dataProxyMock) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy<TIdentifiable>
        {
            var fixture = new Fixture();
            var exceptionToThrow = fixture.Create<IntranetRepositoryException>();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.Save(Arg<TDataProxy>.Is.NotNull))
                .Throw(exceptionToThrow)
                .Repeat.Any();

            var foodWasteObjectMapper = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapper.Stub(m => m.Map<TIdentifiable, TDataProxy>(Arg<TIdentifiable>.Is.NotNull, Arg<CultureInfo>.Is.Anything))
                .Return(dataProxyMock)
                .Repeat.Any();

            var dataRepositoryBase = new MyDataRepository(foodWasteDataProviderMock, foodWasteObjectMapper);
            Assert.That(dataRepositoryBase, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => dataRepositoryBase.Update(identifiable));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that Update throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        private static void TestThatUpdateThrowsIntranetRepositoryExceptionWhenExceptionOccurs<TIdentifiable, TDataProxy>(TIdentifiable identifiable, TDataProxy dataProxyMock) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy<TIdentifiable>
        {
            var fixture = new Fixture();
            var exceptionToThrow = fixture.Create<Exception>();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.Save(Arg<TDataProxy>.Is.NotNull))
                .Throw(exceptionToThrow)
                .Repeat.Any();

            var foodWasteObjectMapper = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapper.Stub(m => m.Map<TIdentifiable, TDataProxy>(Arg<TIdentifiable>.Is.NotNull, Arg<CultureInfo>.Is.Anything))
                .Return(dataProxyMock)
                .Repeat.Any();

            var dataRepositoryBase = new MyDataRepository(foodWasteDataProviderMock, foodWasteObjectMapper);
            Assert.That(dataRepositoryBase, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => dataRepositoryBase.Update(identifiable));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "Update", exceptionToThrow.Message)));
            Assert.That(exception.InnerException, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that Delete calls Map on the object mapper which can map objects in the food waste domain.
        /// </summary>
        private static void TestThatDeleteCallsMapOnFoodWasteObjectMapper<TIdentifiable, TDataProxy>(TIdentifiable identifiable, TDataProxy dataProxyMock) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy<TIdentifiable>
        {
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();

            var foodWasteObjectMapper = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapper.Stub(m => m.Map<TIdentifiable, TDataProxy>(Arg<TIdentifiable>.Is.NotNull, Arg<CultureInfo>.Is.Anything))
                .Return(dataProxyMock)
                .Repeat.Any();

            var dataRepositoryBase = new MyDataRepository(foodWasteDataProviderMock, foodWasteObjectMapper);
            Assert.That(dataRepositoryBase, Is.Not.Null);

            dataRepositoryBase.Delete(identifiable);

            foodWasteObjectMapper.AssertWasCalled(m => m.Map<TIdentifiable, TDataProxy>(Arg<TIdentifiable>.Is.Equal(identifiable), Arg<CultureInfo>.Is.Null));
        }

        /// <summary>
        /// Tests that Delete calls Delete on the data provider which can access data in the food waste repository.
        /// </summary>
        private static void TestThatDeleteCallsDeleteOnFoodWasteDataProvider<TIdentifiable, TDataProxy>(TIdentifiable identifiable, TDataProxy dataProxyMock) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy<TIdentifiable>
        {
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();

            var foodWasteObjectMapper = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapper.Stub(m => m.Map<TIdentifiable, TDataProxy>(Arg<TIdentifiable>.Is.NotNull, Arg<CultureInfo>.Is.Anything))
                .Return(dataProxyMock)
                .Repeat.Any();

            var dataRepositoryBase = new MyDataRepository(foodWasteDataProviderMock, foodWasteObjectMapper);
            Assert.That(dataRepositoryBase, Is.Not.Null);

            dataRepositoryBase.Delete(identifiable);

            foodWasteDataProviderMock.AssertWasCalled(m => m.Delete(Arg<TDataProxy>.Is.Equal(dataProxyMock)));
        }

        /// <summary>
        /// Tests that Delete throws an IntranetRepositoryException when an IntranetRepositoryException occurs.
        /// </summary>
        private static void TestThatDeleteThrowsIntranetRepositoryExceptionWhenIntranetRepositoryExceptionOccurs<TIdentifiable, TDataProxy>(TIdentifiable identifiable, TDataProxy dataProxyMock) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy<TIdentifiable>
        {
            var fixture = new Fixture();
            var exceptionToThrow = fixture.Create<IntranetRepositoryException>();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.Delete(Arg<TDataProxy>.Is.NotNull))
                .Throw(exceptionToThrow)
                .Repeat.Any();

            var foodWasteObjectMapper = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapper.Stub(m => m.Map<TIdentifiable, TDataProxy>(Arg<TIdentifiable>.Is.NotNull, Arg<CultureInfo>.Is.Anything))
                .Return(dataProxyMock)
                .Repeat.Any();

            var dataRepositoryBase = new MyDataRepository(foodWasteDataProviderMock, foodWasteObjectMapper);
            Assert.That(dataRepositoryBase, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => dataRepositoryBase.Delete(identifiable));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that Delete throws an IntranetRepositoryException when an Exception occurs.
        /// </summary>
        private static void TestThatDeleteThrowsIntranetRepositoryExceptionWhenExceptionOccurs<TIdentifiable, TDataProxy>(TIdentifiable identifiable, TDataProxy dataProxyMock) where TIdentifiable : IIdentifiable where TDataProxy : class, TIdentifiable, IMySqlDataProxy<TIdentifiable>
        {
            var fixture = new Fixture();
            var exceptionToThrow = fixture.Create<Exception>();
            var foodWasteDataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProviderMock.Stub(m => m.Delete(Arg<TDataProxy>.Is.NotNull))
                .Throw(exceptionToThrow)
                .Repeat.Any();

            var foodWasteObjectMapper = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            foodWasteObjectMapper.Stub(m => m.Map<TIdentifiable, TDataProxy>(Arg<TIdentifiable>.Is.NotNull, Arg<CultureInfo>.Is.Anything))
                .Return(dataProxyMock)
                .Repeat.Any();

            var dataRepositoryBase = new MyDataRepository(foodWasteDataProviderMock, foodWasteObjectMapper);
            Assert.That(dataRepositoryBase, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => dataRepositoryBase.Delete(identifiable));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, "Delete", exceptionToThrow.Message)));
            Assert.That(exception.InnerException, Is.EqualTo(exceptionToThrow));
        }
    }
}
