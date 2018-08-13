using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Contracts;
using OSDevGrp.OSIntranet.Contracts.Commands;
using OSDevGrp.OSIntranet.Contracts.Faults;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Responses;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Services.Implementations;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf;
using OSDevGrp.OSIntranet.Contracts.Services;
using AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Services
{
    /// <summary>
    /// Tests the service which can access and modify system data in the food waste domain.
    /// </summary>
    [TestFixture]
    public class FoodWasteSystemDataServiceTests
    {
        #region Private variables

        private ICommandBus _commandBusMock;
        private IQueryBus _queryBusMock;
        private IFaultExceptionBuilder<FoodWasteFault> _faultExceptionBuilderMock;

        #endregion

        /// <summary>
        /// Tests that service which can access and modify system data in the food waste domain can be hosted.
        /// </summary>
        [Test]
        public void TestThatFoodWasteSystemDataServiceCanBeHosted()
        {
            var uri = new Uri("http://localhost:7000/OSIntranet/");
            var host = new ServiceHost(typeof (FoodWasteSystemDataService), uri);
            try
            {
                host.Open();
                Assert.That(host.State, Is.EqualTo(CommunicationState.Opened));
            }
            finally
            {
                ChannelTools.CloseChannel(host);
            }
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the command bus is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionIfCommandBusIsNull()
        {
            IQueryBus queryBusMock = MockRepository.GenerateMock<IQueryBus>();
            IFaultExceptionBuilder<FoodWasteFault> faultExceptionBuilderMock = MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>();

            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => new FoodWasteSystemDataService(null, queryBusMock, faultExceptionBuilderMock));
            // ReSharper restore ObjectCreationAsStatement

            TestHelper.AssertArgumentNullExceptionIsValid(result, "commandBus");
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the query bus is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionIfQueryBusIsNull()
        {
            ICommandBus commandBusMock = MockRepository.GenerateMock<ICommandBus>();
            IFaultExceptionBuilder<FoodWasteFault> faultExceptionBuilderMock = MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>();

            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => new FoodWasteSystemDataService(commandBusMock, null, faultExceptionBuilderMock));
            // ReSharper restore ObjectCreationAsStatement

            TestHelper.AssertArgumentNullExceptionIsValid(result, "queryBus");
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the query bus is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionIfFaultExceptionBuilderIsNull()
        {
            ICommandBus commandBusMock = MockRepository.GenerateMock<ICommandBus>();
            IQueryBus queryBusMock = MockRepository.GenerateMock<IQueryBus>();

            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => new FoodWasteSystemDataService(commandBusMock, queryBusMock, null));
            // ReSharper restore ObjectCreationAsStatement

            TestHelper.AssertArgumentNullExceptionIsValid(result, "foodWasteFaultExceptionBuilder");
        }

        /// <summary>
        /// Tests that StorageTypeGetAll throws an ArgumentNullException when the query for getting all the storage types is null.
        /// </summary>
        [Test]
        public void TestThatStorageTypeGetAllThrowsArgumentNullExceptionIfStorageTypeCollectionGetQueryIsNull()
        {
            IFoodWasteSystemDataService sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.StorageTypeGetAll(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "query");
        }

        /// <summary>
        /// Tests that StorageTypeGetAll calls Query on the query bus.
        /// </summary>
        [Test]
        public void TestThatStorageTypeGetAllCallsQueryOnQueryBus()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            StorageTypeCollectionGetQuery query = fixture.Create<StorageTypeCollectionGetQuery>();

            IFoodWasteSystemDataService sut = CreateSut<ICommand, StorageTypeCollectionGetQuery, IEnumerable<StorageTypeSystemView>>(fixture.CreateMany<StorageTypeSystemView>(random.Next(1, 25)).ToList());
            Assert.That(sut, Is.Not.Null);

            sut.StorageTypeGetAll(query);

            _queryBusMock.AssertWasCalled(m => m.Query<StorageTypeCollectionGetQuery, IEnumerable<StorageTypeSystemView>>(Arg<StorageTypeCollectionGetQuery>.Is.Equal(query)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that StorageTypeGetAll returns the result from the query bus.
        /// </summary>
        [Test]
        public void TestThatStorageTypeGetAllReturnsResultFromQueryBus()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            IEnumerable<StorageTypeSystemView> storageTypeSystemViewCollection = fixture.CreateMany<StorageTypeSystemView>(random.Next(1, 25)).ToList();

            IFoodWasteSystemDataService sut = CreateSut<ICommand, StorageTypeCollectionGetQuery, IEnumerable<StorageTypeSystemView>>(storageTypeSystemViewCollection);
            Assert.That(sut, Is.Not.Null);

            IEnumerable<StorageTypeSystemView> result = sut.StorageTypeGetAll(fixture.Create<StorageTypeCollectionGetQuery>());
            // ReSharper disable PossibleMultipleEnumeration
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EqualTo(storageTypeSystemViewCollection));
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        /// Tests that StorageTypeGetAll throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatStorageTypeGetAllThrowsFaultExceptionWhenExceptionOccurs()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            Exception exception = fixture.Create<Exception>();
            FaultException<FoodWasteFault> faultException = fixture.Create<FaultException<FoodWasteFault>>();

            IFoodWasteSystemDataService sut = CreateSut<ICommand, StorageTypeCollectionGetQuery, IEnumerable<StorageTypeSystemView>>(fixture.CreateMany<StorageTypeSystemView>(random.Next(1, 25)).ToList(), exception, faultException);
            Assert.That(sut, Is.Not.Null);

            FaultException<FoodWasteFault> result = Assert.Throws<FaultException<FoodWasteFault>>(() => sut.StorageTypeGetAll(fixture.Create<StorageTypeCollectionGetQuery>()));
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(faultException));

            _faultExceptionBuilderMock.AssertWasCalled(m => m.Build(
                    Arg<Exception>.Is.Equal(exception),
                    Arg<string>.Is.Equal(SoapNamespaces.FoodWasteSystemDataServiceName),
                    Arg<MethodBase>.Matches(src => string.Compare(src.Name, "StorageTypeGetAll", StringComparison.Ordinal) == 0)),
                opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that FoodItemCollectionGet throws an ArgumentNullException when the query for getting the collection of food items is null.
        /// </summary>
        [Test]
        public void TestThatFoodItemCollectionGetThrowsArgumentNullExceptionIfFoodItemCollectionGetQueryIsNull()
        {
            IFoodWasteSystemDataService sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.FoodItemCollectionGet(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "query");
        }

        /// <summary>
        /// Tests that FoodItemCollectionGet calls Query on the query bus.
        /// </summary>
        [Test]
        public void TestThatFoodItemCollectionGetCallsQueryOnQueryBus()
        {
            Fixture fixture = new Fixture();

            FoodItemCollectionGetQuery query = fixture.Create<FoodItemCollectionGetQuery>();

            IEnumerable<FoodItemSystemView> foodItemSystemViewCollection = fixture.Build<FoodItemSystemView>()
                .With(m => m.FoodGroups, new List<FoodGroupSystemView>(0))
                .CreateMany(15)
                .ToList();
            FoodItemCollectionSystemView foodItemCollectionSystemView = fixture.Build<FoodItemCollectionSystemView>()
                .With(m => m.FoodItems, foodItemSystemViewCollection)
                .Create();

            IFoodWasteSystemDataService sut = CreateSut<ICommand, FoodItemCollectionGetQuery, FoodItemCollectionSystemView>(foodItemCollectionSystemView);
            Assert.That(sut, Is.Not.Null);

            sut.FoodItemCollectionGet(query);

            _queryBusMock.AssertWasCalled(m => m.Query<FoodItemCollectionGetQuery, FoodItemCollectionSystemView>(Arg<FoodItemCollectionGetQuery>.Is.Equal(query)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that FoodItemCollectionGet returns the result from the query bus.
        /// </summary>
        [Test]
        public void TestThatFoodItemCollectionGetReturnsResultFromQueryBus()
        {
            Fixture fixture = new Fixture();

            IEnumerable<FoodItemSystemView> foodItemSystemViewCollection = fixture.Build<FoodItemSystemView>()
                .With(m => m.FoodGroups, new List<FoodGroupSystemView>(0))
                .CreateMany(15)
                .ToList();
            FoodItemCollectionSystemView foodItemCollectionSystemView = fixture.Build<FoodItemCollectionSystemView>()
                .With(m => m.FoodItems, foodItemSystemViewCollection)
                .Create();

            IFoodWasteSystemDataService sut = CreateSut<ICommand, FoodItemCollectionGetQuery, FoodItemCollectionSystemView>(foodItemCollectionSystemView);
            Assert.That(sut, Is.Not.Null);

            FoodItemCollectionSystemView result = sut.FoodItemCollectionGet(fixture.Create<FoodItemCollectionGetQuery>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(foodItemCollectionSystemView));
        }

        /// <summary>
        /// Tests that FoodItemCollectionGet throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatFoodItemCollectionGetThrowsFaultExceptionWhenExceptionOccurs()
        {
            Fixture fixture = new Fixture();

            IEnumerable<FoodItemSystemView> foodItemSystemViewCollection = fixture.Build<FoodItemSystemView>()
                .With(m => m.FoodGroups, new List<FoodGroupSystemView>(0))
                .CreateMany(15)
                .ToList();
            FoodItemCollectionSystemView foodItemCollectionSystemView = fixture.Build<FoodItemCollectionSystemView>()
                .With(m => m.FoodItems, foodItemSystemViewCollection)
                .Create();

            Exception exception = fixture.Create<Exception>();
            FaultException<FoodWasteFault> faultException = fixture.Create<FaultException<FoodWasteFault>>();

            IFoodWasteSystemDataService sut = CreateSut<ICommand, FoodItemCollectionGetQuery, FoodItemCollectionSystemView>(foodItemCollectionSystemView, exception, faultException);
            Assert.That(sut, Is.Not.Null);

            FaultException<FoodWasteFault> result = Assert.Throws<FaultException<FoodWasteFault>>(() => sut.FoodItemCollectionGet(fixture.Create<FoodItemCollectionGetQuery>()));
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(faultException));

            _faultExceptionBuilderMock.AssertWasCalled(m => m.Build(
                    Arg<Exception>.Is.Equal(exception),
                    Arg<string>.Is.Equal(SoapNamespaces.FoodWasteSystemDataServiceName),
                    Arg<MethodBase>.Matches(src => string.Compare(src.Name, "FoodItemCollectionGet", StringComparison.Ordinal) == 0)),
                opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that FoodItemImportFromDataProvider throws an ArgumentNullException when the command for importing a food item from a given data provider is null.
        /// </summary>
        [Test]
        public void TestThatFoodItemImportFromDataProviderThrowsArgumentNullExceptionIfFoodItemImportFromDataProviderCommandIsNull()
        {
            IFoodWasteSystemDataService sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result  = Assert.Throws<ArgumentNullException>(() => sut.FoodItemImportFromDataProvider(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "command");
        }

        /// <summary>
        /// Tests that FoodItemImportFromDataProvider calls Publish on the command bus.
        /// </summary>
        [Test]
        public void TestThatFoodItemImportFromDataProviderCallsPublishOnCommandBus()
        {
            Fixture fixture = new Fixture();

            FoodItemImportFromDataProviderCommand command = fixture.Create<FoodItemImportFromDataProviderCommand>();

            IFoodWasteSystemDataService sut = CreateSut<FoodItemImportFromDataProviderCommand, IQuery, ServiceReceiptResponse>(fixture.Create<ServiceReceiptResponse>());
            Assert.That(sut, Is.Not.Null);

            sut.FoodItemImportFromDataProvider(command);

            _commandBusMock.AssertWasCalled(m => m.Publish<FoodItemImportFromDataProviderCommand, ServiceReceiptResponse>(Arg<FoodItemImportFromDataProviderCommand>.Is.Equal(command)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that FoodItemImportFromDataProvider returns the result from the command bus.
        /// </summary>
        [Test]
        public void TestThatFoodItemImportFromDataProviderReturnsResultFromCommandBus()
        {
            Fixture fixture = new Fixture();

            ServiceReceiptResponse serviceReceipt = fixture.Create<ServiceReceiptResponse>();

            IFoodWasteSystemDataService sut = CreateSut<FoodItemImportFromDataProviderCommand, IQuery, ServiceReceiptResponse>(serviceReceipt);
            Assert.That(sut, Is.Not.Null);

            ServiceReceiptResponse result = sut.FoodItemImportFromDataProvider(fixture.Create<FoodItemImportFromDataProviderCommand>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(serviceReceipt));
        }

        /// <summary>
        /// Tests that FoodItemImportFromDataProvider throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatFoodItemImportFromDataProviderThrowsFaultExceptionWhenExceptionOccurs()
        {
            Fixture fixture = new Fixture();

            Exception exception = fixture.Create<Exception>();
            FaultException<FoodWasteFault> faultException = fixture.Create<FaultException<FoodWasteFault>>();

            IFoodWasteSystemDataService sut = CreateSut<FoodItemImportFromDataProviderCommand, IQuery, ServiceReceiptResponse>(fixture.Create<ServiceReceiptResponse>(), exception, faultException);
            Assert.That(sut, Is.Not.Null);

            FaultException<FoodWasteFault> result = Assert.Throws<FaultException<FoodWasteFault>>(() => sut.FoodItemImportFromDataProvider(fixture.Create<FoodItemImportFromDataProviderCommand>()));
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(faultException));

            _faultExceptionBuilderMock.AssertWasCalled(m => m.Build(
                    Arg<Exception>.Is.Equal(exception),
                    Arg<string>.Is.Equal(SoapNamespaces.FoodWasteSystemDataServiceName),
                    Arg<MethodBase>.Matches(src => string.Compare(src.Name, "FoodItemImportFromDataProvider", StringComparison.Ordinal) == 0)),
                opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that FoodGroupTreeGet throws an ArgumentNullException when the query for getting all the tree of food groups is null.
        /// </summary>
        [Test]
        public void TestThatFoodGroupTreeGetThrowsArgumentNullExceptionIfFoodGroupTreeGetQueryIsNull()
        {
            IFoodWasteSystemDataService sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.FoodGroupTreeGet(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "query");
        }

        /// <summary>
        /// Tests that FoodGroupTreeGet calls Query on the query bus.
        /// </summary>
        [Test]
        public void TestThatFoodGroupTreeGetCallsQueryOnQueryBus()
        {
            Fixture fixture = new Fixture();

            FoodGroupTreeGetQuery query = fixture.Create<FoodGroupTreeGetQuery>();

            IEnumerable<FoodGroupSystemView> foodGroupSystemViewCollection = fixture.Build<FoodGroupSystemView>()
                .With(m => m.Parent, fixture.Create<FoodGroupIdentificationView>())
                .With(m => m.Children, new List<FoodGroupSystemView>(0))
                .CreateMany(15)
                .ToList();
            FoodGroupTreeSystemView foodGroupTreeSystemView = fixture.Build<FoodGroupTreeSystemView>()
                .With(m => m.FoodGroups, foodGroupSystemViewCollection)
                .Create();

            IFoodWasteSystemDataService sut = CreateSut<ICommand, FoodGroupTreeGetQuery, FoodGroupTreeSystemView>(foodGroupTreeSystemView);
            Assert.That(sut, Is.Not.Null);

            sut.FoodGroupTreeGet(query);

            _queryBusMock.AssertWasCalled(m => m.Query<FoodGroupTreeGetQuery, FoodGroupTreeSystemView>(Arg<FoodGroupTreeGetQuery>.Is.Equal(query)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that FoodGroupTreeGet returns the result from the query bus.
        /// </summary>
        [Test]
        public void TestThatFoodGroupTreeGetReturnsResultFromQueryBus()
        {
            Fixture fixture = new Fixture();

            IEnumerable<FoodGroupSystemView> foodGroupSystemViewCollection = fixture.Build<FoodGroupSystemView>()
                .With(m => m.Parent, fixture.Create<FoodGroupIdentificationView>())
                .With(m => m.Children, new List<FoodGroupSystemView>(0))
                .CreateMany(15)
                .ToList();
            FoodGroupTreeSystemView foodGroupTreeSystemView = fixture.Build<FoodGroupTreeSystemView>()
                .With(m => m.FoodGroups, foodGroupSystemViewCollection)
                .Create();

            IFoodWasteSystemDataService sut = CreateSut<ICommand, FoodGroupTreeGetQuery, FoodGroupTreeSystemView>(foodGroupTreeSystemView);
            Assert.That(sut, Is.Not.Null);

            FoodGroupTreeSystemView result = sut.FoodGroupTreeGet(fixture.Create<FoodGroupTreeGetQuery>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(foodGroupTreeSystemView));
        }

        /// <summary>
        /// Tests that FoodGroupTreeGet throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatFoodGroupTreeGetThrowsFaultExceptionWhenExceptionOccurs()
        {
            Fixture fixture = new Fixture();

            IEnumerable<FoodGroupSystemView> foodGroupSystemViewCollection = fixture.Build<FoodGroupSystemView>()
                .With(m => m.Parent, fixture.Create<FoodGroupIdentificationView>())
                .With(m => m.Children, new List<FoodGroupSystemView>(0))
                .CreateMany(15)
                .ToList();
            FoodGroupTreeSystemView foodGroupTreeSystemView = fixture.Build<FoodGroupTreeSystemView>()
                .With(m => m.FoodGroups, foodGroupSystemViewCollection)
                .Create();

            Exception exception = fixture.Create<Exception>();
            FaultException<FoodWasteFault> faultException = fixture.Create<FaultException<FoodWasteFault>>();

            IFoodWasteSystemDataService sut = CreateSut<ICommand, FoodGroupTreeGetQuery, FoodGroupTreeSystemView>(foodGroupTreeSystemView, exception, faultException);
            Assert.That(sut, Is.Not.Null);

            FaultException<FoodWasteFault> result = Assert.Throws<FaultException<FoodWasteFault>>(() => sut.FoodGroupTreeGet(fixture.Create<FoodGroupTreeGetQuery>()));
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(faultException));

            _faultExceptionBuilderMock.AssertWasCalled(m => m.Build(
                    Arg<Exception>.Is.Equal(exception),
                    Arg<string>.Is.Equal(SoapNamespaces.FoodWasteSystemDataServiceName),
                    Arg<MethodBase>.Matches(src => string.Compare(src.Name, "FoodGroupTreeGet", StringComparison.Ordinal) == 0)),
                opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that FoodGroupImportFromDataProvider throws an ArgumentNullException when the command for importing a food group from a given data provider is null.
        /// </summary>
        [Test]
        public void TestThatFoodGroupImportFromDataProviderThrowsArgumentNullExceptionIfFoodGroupImportFromDataProviderCommandIsNull()
        {
            IFoodWasteSystemDataService sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.FoodGroupImportFromDataProvider(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "command");
        }

        /// <summary>
        /// Tests that FoodGroupImportFromDataProvider calls Publish on the command bus.
        /// </summary>
        [Test]
        public void TestThatFoodGroupImportFromDataProviderCallsPublishOnCommandBus()
        {
            Fixture fixture = new Fixture();

            FoodGroupImportFromDataProviderCommand command = fixture.Create<FoodGroupImportFromDataProviderCommand>();

            IFoodWasteSystemDataService sut = CreateSut<FoodGroupImportFromDataProviderCommand, IQuery, ServiceReceiptResponse>(fixture.Create<ServiceReceiptResponse>());
            Assert.That(sut, Is.Not.Null);

            sut.FoodGroupImportFromDataProvider(command);

            _commandBusMock.AssertWasCalled(m => m.Publish<FoodGroupImportFromDataProviderCommand, ServiceReceiptResponse>(Arg<FoodGroupImportFromDataProviderCommand>.Is.Equal(command)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that FoodGroupImportFromDataProvider returns the result from the command bus.
        /// </summary>
        [Test]
        public void TestThatFoodGroupImportFromDataProviderReturnsResultFromCommandBus()
        {
            Fixture fixture = new Fixture();

            ServiceReceiptResponse serviceReceipt = fixture.Create<ServiceReceiptResponse>();

            IFoodWasteSystemDataService sut = CreateSut<FoodGroupImportFromDataProviderCommand, IQuery, ServiceReceiptResponse>(serviceReceipt);
            Assert.That(sut, Is.Not.Null);
            
            ServiceReceiptResponse result = sut.FoodGroupImportFromDataProvider(fixture.Create<FoodGroupImportFromDataProviderCommand>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(serviceReceipt));
        }

        /// <summary>
        /// Tests that FoodGroupImportFromDataProvider throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatFoodGroupImportFromDataProviderThrowsFaultExceptionWhenExceptionOccurs()
        {
            Fixture fixture = new Fixture();

            Exception exception = fixture.Create<Exception>();
            FaultException<FoodWasteFault> faultException = fixture.Create<FaultException<FoodWasteFault>>();

            IFoodWasteSystemDataService sut = CreateSut<FoodGroupImportFromDataProviderCommand, IQuery, ServiceReceiptResponse>(fixture.Create<ServiceReceiptResponse>(), exception, faultException);
            Assert.That(sut, Is.Not.Null);

            FaultException<FoodWasteFault> result = Assert.Throws<FaultException<FoodWasteFault>>(() => sut.FoodGroupImportFromDataProvider(fixture.Create<FoodGroupImportFromDataProviderCommand>()));
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(faultException));

            _faultExceptionBuilderMock.AssertWasCalled(m => m.Build(
                    Arg<Exception>.Is.Equal(exception),
                    Arg<string>.Is.Equal(SoapNamespaces.FoodWasteSystemDataServiceName),
                    Arg<MethodBase>.Matches(src => string.Compare(src.Name, "FoodGroupImportFromDataProvider", StringComparison.Ordinal) == 0)),
                opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that ForeignKeyAdd throws an ArgumentNullException when the command for adding a foreign key is null.
        /// </summary>
        [Test]
        public void TestThatForeignKeyAddThrowsArgumentNullExceptionIfForeignKeyAddCommandIsNull()
        {
            IFoodWasteSystemDataService sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ForeignKeyAdd(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "command");
        }

        /// <summary>
        /// Tests that ForeignKeyAdd calls Publish on the command bus.
        /// </summary>
        [Test]
        public void TestThatForeignKeyAddCallsPublishOnCommandBus()
        {
            Fixture fixture = new Fixture();

            ForeignKeyAddCommand command = fixture.Create<ForeignKeyAddCommand>();

            IFoodWasteSystemDataService sut = CreateSut<ForeignKeyAddCommand, IQuery, ServiceReceiptResponse>(fixture.Create<ServiceReceiptResponse>());
            Assert.That(sut, Is.Not.Null);

            sut.ForeignKeyAdd(command);

            _commandBusMock.AssertWasCalled(m => m.Publish<ForeignKeyAddCommand, ServiceReceiptResponse>(Arg<ForeignKeyAddCommand>.Is.Equal(command)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that ForeignKeyAdd returns the result from the command bus.
        /// </summary>
        [Test]
        public void TestThatForeignKeyAddReturnsResultFromCommandBus()
        {
            Fixture fixture = new Fixture();

            ServiceReceiptResponse serviceReceipt = fixture.Create<ServiceReceiptResponse>();

            IFoodWasteSystemDataService sut = CreateSut<ForeignKeyAddCommand, IQuery, ServiceReceiptResponse>(serviceReceipt);
            Assert.That(sut, Is.Not.Null);

            ServiceReceiptResponse result = sut.ForeignKeyAdd(fixture.Create<ForeignKeyAddCommand>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(serviceReceipt));
        }

        /// <summary>
        /// Tests that ForeignKeyAdd throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatForeignKeyAddThrowsFaultExceptionWhenExceptionOccurs()
        {
            Fixture fixture = new Fixture();

            Exception exception = fixture.Create<Exception>();
            FaultException<FoodWasteFault> faultException = fixture.Create<FaultException<FoodWasteFault>>();

            IFoodWasteSystemDataService sut = CreateSut<ForeignKeyAddCommand, IQuery, ServiceReceiptResponse>(fixture.Create<ServiceReceiptResponse>(), exception, faultException);
            Assert.That(sut, Is.Not.Null);

            FaultException<FoodWasteFault> result = Assert.Throws<FaultException<FoodWasteFault>>(() => sut.ForeignKeyAdd(fixture.Create<ForeignKeyAddCommand>()));
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(faultException));

            _faultExceptionBuilderMock.AssertWasCalled(m => m.Build(
                    Arg<Exception>.Is.Equal(exception),
                    Arg<string>.Is.Equal(SoapNamespaces.FoodWasteSystemDataServiceName),
                    Arg<MethodBase>.Matches(src => string.Compare(src.Name, "ForeignKeyAdd", StringComparison.Ordinal) == 0)),
                opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that ForeignKeyModify throws an ArgumentNullException when the command for modifying a foreign key is null.
        /// </summary>
        [Test]
        public void TestThatForeignKeyModifyThrowsArgumentNullExceptionIfForeignKeyModifyCommandIsNull()
        {
            IFoodWasteSystemDataService sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ForeignKeyModify(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "command");
        }

        /// <summary>
        /// Tests that ForeignKeyModify calls Publish on the command bus.
        /// </summary>
        [Test]
        public void TestThatForeignKeyModifyCallsPublishOnCommandBus()
        {
            Fixture fixture = new Fixture();

            ForeignKeyModifyCommand command = fixture.Create<ForeignKeyModifyCommand>();

            IFoodWasteSystemDataService sut = CreateSut<ForeignKeyModifyCommand, IQuery, ServiceReceiptResponse>(fixture.Create<ServiceReceiptResponse>());
            Assert.That(sut, Is.Not.Null);

            sut.ForeignKeyModify(command);

            _commandBusMock.AssertWasCalled(m => m.Publish<ForeignKeyModifyCommand, ServiceReceiptResponse>(Arg<ForeignKeyModifyCommand>.Is.Equal(command)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that ForeignKeyModify returns the result from the command bus.
        /// </summary>
        [Test]
        public void TestThatForeignKeyModifyReturnsResultFromCommandBus()
        {
            Fixture fixture = new Fixture();

            ServiceReceiptResponse serviceReceipt = fixture.Create<ServiceReceiptResponse>();

            IFoodWasteSystemDataService sut = CreateSut<ForeignKeyModifyCommand, IQuery, ServiceReceiptResponse>(serviceReceipt);
            Assert.That(sut, Is.Not.Null);

            ServiceReceiptResponse result = sut.ForeignKeyModify(fixture.Create<ForeignKeyModifyCommand>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(serviceReceipt));
        }

        /// <summary>
        /// Tests that ForeignKeyModify throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatForeignKeyModifyThrowsFaultExceptionWhenExceptionOccurs()
        {
            Fixture fixture = new Fixture();

            Exception exception = fixture.Create<Exception>();
            FaultException<FoodWasteFault> faultException = fixture.Create<FaultException<FoodWasteFault>>();

            IFoodWasteSystemDataService sut = CreateSut<ForeignKeyModifyCommand, IQuery, ServiceReceiptResponse>(fixture.Create<ServiceReceiptResponse>(), exception, faultException);
            Assert.That(sut, Is.Not.Null);

            FaultException<FoodWasteFault> result  = Assert.Throws<FaultException<FoodWasteFault>>(() => sut.ForeignKeyModify(fixture.Create<ForeignKeyModifyCommand>()));
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(faultException));

            _faultExceptionBuilderMock.AssertWasCalled(m => m.Build(
                Arg<Exception>.Is.Equal(exception), 
                Arg<string>.Is.Equal(SoapNamespaces.FoodWasteSystemDataServiceName), 
                Arg<MethodBase>.Matches(src => string.Compare(src.Name, "ForeignKeyModify", StringComparison.Ordinal) == 0)),
                opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that ForeignKeyDelete throws an ArgumentNullException when the command for deleting a foreign key is null.
        /// </summary>
        [Test]
        public void TestThatForeignKeyDeleteThrowsArgumentNullExceptionIfForeignKeyDeleteCommandIsNull()
        {
            IFoodWasteSystemDataService sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ForeignKeyDelete(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "command");
        }

        /// <summary>
        /// Tests that ForeignKeyDelete calls Publish on the command bus.
        /// </summary>
        [Test]
        public void TestThatForeignKeyDeleteCallsPublishOnCommandBus()
        {
            Fixture fixture = new Fixture();

            ForeignKeyDeleteCommand command = fixture.Create<ForeignKeyDeleteCommand>();

            IFoodWasteSystemDataService sut = CreateSut<ForeignKeyDeleteCommand, IQuery, ServiceReceiptResponse>(fixture.Create<ServiceReceiptResponse>());
            Assert.That(sut, Is.Not.Null);

            sut.ForeignKeyDelete(command);

            _commandBusMock.AssertWasCalled(m => m.Publish<ForeignKeyDeleteCommand, ServiceReceiptResponse>(Arg<ForeignKeyDeleteCommand>.Is.Equal(command)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that ForeignKeyDelete returns the result from the command bus.
        /// </summary>
        [Test]
        public void TestThatForeignKeyDeleteeturnsResultFromCommandBus()
        {
            Fixture fixture = new Fixture();

            ServiceReceiptResponse serviceReceipt = fixture.Create<ServiceReceiptResponse>();

            IFoodWasteSystemDataService sut = CreateSut<ForeignKeyDeleteCommand, IQuery, ServiceReceiptResponse>(serviceReceipt);
            Assert.That(sut, Is.Not.Null);

            ServiceReceiptResponse result = sut.ForeignKeyDelete(fixture.Create<ForeignKeyDeleteCommand>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(serviceReceipt));
        }

        /// <summary>
        /// Tests that ForeignKeyDelete throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatForeignKeyDeleteThrowsFaultExceptionWhenExceptionOccurs()
        {
            Fixture fixture = new Fixture();

            Exception exception = fixture.Create<Exception>();
            FaultException<FoodWasteFault> faultException = fixture.Create<FaultException<FoodWasteFault>>();

            IFoodWasteSystemDataService sut = CreateSut<ForeignKeyDeleteCommand, IQuery, ServiceReceiptResponse>(fixture.Create<ServiceReceiptResponse>(), exception, faultException);
            Assert.That(sut, Is.Not.Null);

            FaultException<FoodWasteFault> result = Assert.Throws<FaultException<FoodWasteFault>>(() => sut.ForeignKeyDelete(fixture.Create<ForeignKeyDeleteCommand>()));
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(faultException));

            _faultExceptionBuilderMock.AssertWasCalled(m => m.Build(
                    Arg<Exception>.Is.Equal(exception),
                    Arg<string>.Is.Equal(SoapNamespaces.FoodWasteSystemDataServiceName),
                    Arg<MethodBase>.Matches(src => string.Compare(src.Name, "ForeignKeyDelete", StringComparison.Ordinal) == 0)),
                opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that DataProviderGetAll throws an ArgumentNullException when the query for getting all the data providers is null.
        /// </summary>
        [Test]
        public void TestThatDataProviderGetAllThrowsArgumentNullExceptionIfDataProviderCollectionGetQueryIsNull()
        {
            IFoodWasteSystemDataService sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.DataProviderGetAll(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "query");
        }

        /// <summary>
        /// Tests that DataProviderGetAll calls Query on the query bus.
        /// </summary>
        [Test]
        public void TestThatDataProviderGetAllCallsQueryOnQueryBus()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            DataProviderCollectionGetQuery query = fixture.Create<DataProviderCollectionGetQuery>();

            IFoodWasteSystemDataService sut = CreateSut<ICommand, DataProviderCollectionGetQuery, IEnumerable<DataProviderSystemView>>(fixture.CreateMany<DataProviderSystemView>(random.Next(1, 25)).ToList());
            Assert.That(sut, Is.Not.Null);

            sut.DataProviderGetAll(query);

            _queryBusMock.AssertWasCalled(m => m.Query<DataProviderCollectionGetQuery, IEnumerable<DataProviderSystemView>>(Arg<DataProviderCollectionGetQuery>.Is.Equal(query)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that DataProviderGetAll returns the result from the query bus.
        /// </summary>
        [Test]
        public void TestThatDataProviderGetAllReturnsResultFromQueryBus()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            IEnumerable<DataProviderSystemView> dataProviderSystemViewCollection = fixture.CreateMany<DataProviderSystemView>(random.Next(1, 25)).ToList();

            IFoodWasteSystemDataService sut = CreateSut<ICommand, DataProviderCollectionGetQuery, IEnumerable<DataProviderSystemView>>(dataProviderSystemViewCollection);
            Assert.That(sut, Is.Not.Null);

            IEnumerable<DataProviderSystemView> result = sut.DataProviderGetAll(fixture.Create<DataProviderCollectionGetQuery>());
            // ReSharper disable PossibleMultipleEnumeration
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EqualTo(dataProviderSystemViewCollection));
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        /// Tests that DataProviderGetAll throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatDataProviderGetAllThrowsFaultExceptionWhenExceptionOccurs()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            Exception exception = fixture.Create<Exception>();
            FaultException<FoodWasteFault> faultException = fixture.Create<FaultException<FoodWasteFault>>();

            IFoodWasteSystemDataService sut = CreateSut<ICommand, DataProviderCollectionGetQuery, IEnumerable<DataProviderSystemView>>(fixture.CreateMany<DataProviderSystemView>(random.Next(1, 25)).ToList(), exception, faultException);
            Assert.That(sut, Is.Not.Null);

            FaultException<FoodWasteFault> result = Assert.Throws<FaultException<FoodWasteFault>>(() => sut.DataProviderGetAll(fixture.Create<DataProviderCollectionGetQuery>()));
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(faultException));

            _faultExceptionBuilderMock.AssertWasCalled(m => m.Build(
                    Arg<Exception>.Is.Equal(exception),
                    Arg<string>.Is.Equal(SoapNamespaces.FoodWasteSystemDataServiceName),
                    Arg<MethodBase>.Matches(src => string.Compare(src.Name, "DataProviderGetAll", StringComparison.Ordinal) == 0)),
                opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that TranslationAdd throws an ArgumentNullException when the command for adding a translation is null.
        /// </summary>
        [Test]
        public void TestThatTranslationAddThrowsArgumentNullExceptionIfTranslationAddCommandIsNull()
        {
            IFoodWasteSystemDataService sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.TranslationAdd(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "command");
        }

        /// <summary>
        /// Tests that TranslationAdd calls Publish on the command bus.
        /// </summary>
        [Test]
        public void TestThatTranslationAddCallsPublishOnCommandBus()
        {
            Fixture fixture = new Fixture();

            TranslationAddCommand command = fixture.Create<TranslationAddCommand>();

            IFoodWasteSystemDataService sut = CreateSut<TranslationAddCommand, IQuery, ServiceReceiptResponse>(fixture.Create<ServiceReceiptResponse>());
            Assert.That(sut, Is.Not.Null);

            sut.TranslationAdd(command);

            _commandBusMock.AssertWasCalled(m => m.Publish<TranslationAddCommand, ServiceReceiptResponse>(Arg<TranslationAddCommand>.Is.Equal(command)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that TranslationAdd returns the result from the command bus.
        /// </summary>
        [Test]
        public void TestThatTranslationAddReturnsResultFromCommandBus()
        {
            Fixture fixture = new Fixture();

            ServiceReceiptResponse serviceReceipt = fixture.Create<ServiceReceiptResponse>();

            IFoodWasteSystemDataService sut = CreateSut<TranslationAddCommand, IQuery, ServiceReceiptResponse>(serviceReceipt);
            Assert.That(sut, Is.Not.Null);

            ServiceReceiptResponse result = sut.TranslationAdd(fixture.Create<TranslationAddCommand>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(serviceReceipt));
        }

        /// <summary>
        /// Tests that TranslationAdd throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatTranslationAddThrowsFaultExceptionWhenExceptionOccurs()
        {
            Fixture fixture = new Fixture();

            Exception exception = fixture.Create<Exception>();
            FaultException<FoodWasteFault> faultException = fixture.Create<FaultException<FoodWasteFault>>();

            IFoodWasteSystemDataService sut = CreateSut<TranslationAddCommand, IQuery, ServiceReceiptResponse>(fixture.Create<ServiceReceiptResponse>(), exception, faultException);
            Assert.That(sut, Is.Not.Null);

            FaultException<FoodWasteFault> result = Assert.Throws<FaultException<FoodWasteFault>>(() => sut.TranslationAdd(fixture.Create<TranslationAddCommand>()));
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(faultException));

            _faultExceptionBuilderMock.AssertWasCalled(m => m.Build(
                    Arg<Exception>.Is.Equal(exception),
                    Arg<string>.Is.Equal(SoapNamespaces.FoodWasteSystemDataServiceName),
                    Arg<MethodBase>.Matches(src => string.Compare(src.Name, "TranslationAdd", StringComparison.Ordinal) == 0)),
                opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that TranslationModify throws an ArgumentNullException when the command for modifying a translation is null.
        /// </summary>
        [Test]
        public void TestThatTranslationModifyThrowsArgumentNullExceptionIfTranslationModifyCommandIsNull()
        {
            IFoodWasteSystemDataService sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.TranslationModify(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "command");
        }

        /// <summary>
        /// Tests that TranslationModify calls Publish on the command bus.
        /// </summary>
        [Test]
        public void TestThatTranslationModifyCallsPublishOnCommandBus()
        {
            Fixture fixture = new Fixture();

            TranslationModifyCommand command = fixture.Create<TranslationModifyCommand>();

            IFoodWasteSystemDataService sut = CreateSut<TranslationModifyCommand, IQuery, ServiceReceiptResponse>(fixture.Create<ServiceReceiptResponse>());
            Assert.That(sut, Is.Not.Null);

            sut.TranslationModify(command);

            _commandBusMock.AssertWasCalled(m => m.Publish<TranslationModifyCommand, ServiceReceiptResponse>(Arg<TranslationModifyCommand>.Is.Equal(command)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that TranslationModify returns the result from the command bus.
        /// </summary>
        [Test]
        public void TestThatTranslationModifyReturnsResultFromCommandBus()
        {
            Fixture fixture = new Fixture();

            ServiceReceiptResponse serviceReceipt = fixture.Create<ServiceReceiptResponse>();

            IFoodWasteSystemDataService sut = CreateSut<TranslationModifyCommand, IQuery, ServiceReceiptResponse>(serviceReceipt);
            Assert.That(sut, Is.Not.Null);

            ServiceReceiptResponse result = sut.TranslationModify(fixture.Create<TranslationModifyCommand>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(serviceReceipt));
        }

        /// <summary>
        /// Tests that TranslationModify throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatTranslationModifyThrowsFaultExceptionWhenExceptionOccurs()
        {
            Fixture fixture = new Fixture();

            Exception exception = fixture.Create<Exception>();
            FaultException<FoodWasteFault> faultException = fixture.Create<FaultException<FoodWasteFault>>();

            IFoodWasteSystemDataService sut = CreateSut<TranslationModifyCommand, IQuery, ServiceReceiptResponse>(fixture.Create<ServiceReceiptResponse>(), exception, faultException);
            Assert.That(sut, Is.Not.Null);

            FaultException<FoodWasteFault> result = Assert.Throws<FaultException<FoodWasteFault>>(() => sut.TranslationModify(fixture.Create<TranslationModifyCommand>()));
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(faultException));

            _faultExceptionBuilderMock.AssertWasCalled(m => m.Build(
                    Arg<Exception>.Is.Equal(exception),
                    Arg<string>.Is.Equal(SoapNamespaces.FoodWasteSystemDataServiceName),
                    Arg<MethodBase>.Matches(src => string.Compare(src.Name, "TranslationModify", StringComparison.Ordinal) == 0)),
                opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that TranslationDelete throws an ArgumentNullException when the command for deleting a translation is null.
        /// </summary>
        [Test]
        public void TestThatTranslationDeleteThrowsArgumentNullExceptionIfTranslationDeleteCommandIsNull()
        {
            IFoodWasteSystemDataService sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.TranslationDelete(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "command");
        }

        /// <summary>
        /// Tests that TranslationDelete calls Publish on the command bus.
        /// </summary>
        [Test]
        public void TestThatTranslationDeleteCallsPublishOnCommandBus()
        {
            Fixture fixture = new Fixture();

            TranslationDeleteCommand command = fixture.Create<TranslationDeleteCommand>();

            IFoodWasteSystemDataService sut = CreateSut<TranslationDeleteCommand, IQuery, ServiceReceiptResponse>(fixture.Create<ServiceReceiptResponse>());
            Assert.That(sut, Is.Not.Null);

            sut.TranslationDelete(command);

            _commandBusMock.AssertWasCalled(m => m.Publish<TranslationDeleteCommand, ServiceReceiptResponse>(Arg<TranslationDeleteCommand>.Is.Equal(command)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that TranslationDelete returns the result from the command bus.
        /// </summary>
        [Test]
        public void TestThatTranslationDeleteReturnsResultFromCommandBus()
        {
            Fixture fixture = new Fixture();

            ServiceReceiptResponse serviceReceipt = fixture.Create<ServiceReceiptResponse>();

            IFoodWasteSystemDataService sut = CreateSut<TranslationDeleteCommand, IQuery, ServiceReceiptResponse>(serviceReceipt);
            Assert.That(sut, Is.Not.Null);

            ServiceReceiptResponse result = sut.TranslationDelete(fixture.Create<TranslationDeleteCommand>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(serviceReceipt));
        }

        /// <summary>
        /// Tests that TranslationDelete throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatTranslationDeleteThrowsFaultExceptionWhenExceptionOccurs()
        {
            Fixture fixture = new Fixture();

            Exception exception = fixture.Create<Exception>();
            FaultException<FoodWasteFault> faultException = fixture.Create<FaultException<FoodWasteFault>>();

            IFoodWasteSystemDataService sut = CreateSut<TranslationDeleteCommand, IQuery, ServiceReceiptResponse>(fixture.Create<ServiceReceiptResponse>(), exception, faultException);
            Assert.That(sut, Is.Not.Null);

            FaultException<FoodWasteFault> result = Assert.Throws<FaultException<FoodWasteFault>>(() => sut.TranslationDelete(fixture.Create<TranslationDeleteCommand>()));
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(faultException));

            _faultExceptionBuilderMock.AssertWasCalled(m => m.Build(
                    Arg<Exception>.Is.Equal(exception),
                    Arg<string>.Is.Equal(SoapNamespaces.FoodWasteSystemDataServiceName),
                    Arg<MethodBase>.Matches(src => string.Compare(src.Name, "TranslationDelete", StringComparison.Ordinal) == 0)),
                opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that StaticTextGetAll throws an ArgumentNullException when the query for getting all the static texts is null.
        /// </summary>
        [Test]
        public void TestThatStaticTextGetAllThrowsArgumentNullExceptionIfStaticTextCollectionGetQueryIsNull()
        {
            IFoodWasteSystemDataService sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.StaticTextGetAll(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "query");
        }

        /// <summary>
        /// Tests that StaticTextGetAll calls Query on the query bus.
        /// </summary>
        [Test]
        public void TestThatStaticTextGetAllCallsQueryOnQueryBus()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            StaticTextCollectionGetQuery query = fixture.Create<StaticTextCollectionGetQuery>();

            IFoodWasteSystemDataService sut = CreateSut<ICommand, StaticTextCollectionGetQuery, IEnumerable<StaticTextSystemView>>(fixture.CreateMany<StaticTextSystemView>(random.Next(1, 25)).ToList());
            Assert.That(sut, Is.Not.Null);

            sut.StaticTextGetAll(query);

            _queryBusMock.AssertWasCalled(m => m.Query<StaticTextCollectionGetQuery, IEnumerable<StaticTextSystemView>>(Arg<StaticTextCollectionGetQuery>.Is.Equal(query)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that StaticTextGetAll returns the result from the query bus.
        /// </summary>
        [Test]
        public void TestThatStaticTextGetAllReturnsResultFromQueryBus()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            IEnumerable<StaticTextSystemView> staticTextSystemViewCollection = fixture.CreateMany<StaticTextSystemView>(random.Next(1, 25)).ToList();

            IFoodWasteSystemDataService sut = CreateSut<ICommand, StaticTextCollectionGetQuery, IEnumerable<StaticTextSystemView>>(staticTextSystemViewCollection);
            Assert.That(sut, Is.Not.Null);

            IEnumerable<StaticTextSystemView> result = sut.StaticTextGetAll(fixture.Create<StaticTextCollectionGetQuery>());
            // ReSharper disable PossibleMultipleEnumeration
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EqualTo(staticTextSystemViewCollection));
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        /// Tests that StaticTextGetAll throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatStaticTextGetAllThrowsFaultExceptionWhenExceptionOccurs()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            Exception exception = fixture.Create<Exception>();
            FaultException<FoodWasteFault> faultException = fixture.Create<FaultException<FoodWasteFault>>();

            IFoodWasteSystemDataService sut = CreateSut<ICommand, StaticTextCollectionGetQuery, IEnumerable<StaticTextSystemView>>(fixture.CreateMany<StaticTextSystemView>(random.Next(1, 25)).ToList(), exception, faultException);
            Assert.That(sut, Is.Not.Null);

            FaultException<FoodWasteFault> result = Assert.Throws<FaultException<FoodWasteFault>>(() => sut.StaticTextGetAll(fixture.Create<StaticTextCollectionGetQuery>()));
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(faultException));

            _faultExceptionBuilderMock.AssertWasCalled(m => m.Build(
                    Arg<Exception>.Is.Equal(exception),
                    Arg<string>.Is.Equal(SoapNamespaces.FoodWasteSystemDataServiceName),
                    Arg<MethodBase>.Matches(src => string.Compare(src.Name, "StaticTextGetAll", StringComparison.Ordinal) == 0)),
                opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that TranslationInfoGetAll throws an ArgumentNullException when the query for getting all the translation informations which can be used for translations is null.
        /// </summary>
        [Test]
        public void TestThatTranslationInfoGetAllThrowsArgumentNullExceptionIfTranslationInfoCollectionGetQueryIsNull()
        {
            IFoodWasteSystemDataService sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.TranslationInfoGetAll(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "query");
        }

        /// <summary>
        /// Tests that TranslationInfoGetAll calls Query on the query bus.
        /// </summary>
        [Test]
        public void TestThatTranslationInfoGetAllCallsQueryOnQueryBus()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            TranslationInfoCollectionGetQuery query = fixture.Create<TranslationInfoCollectionGetQuery>();

            IFoodWasteSystemDataService sut = CreateSut<ICommand, TranslationInfoCollectionGetQuery, IEnumerable<TranslationInfoSystemView>>(fixture.CreateMany<TranslationInfoSystemView>(random.Next(1, 25)).ToList());
            Assert.That(sut, Is.Not.Null);

            sut.TranslationInfoGetAll(query);

            _queryBusMock.AssertWasCalled(m => m.Query<TranslationInfoCollectionGetQuery, IEnumerable<TranslationInfoSystemView>>(Arg<TranslationInfoCollectionGetQuery>.Is.Equal(query)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that TranslationInfoGetAll returns the result from the query bus.
        /// </summary>
        [Test]
        public void TestThatTranslationInfoGetAllReturnsResultFromQueryBus()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            IEnumerable<TranslationInfoSystemView> translationInfoSystemViewCollection = fixture.CreateMany<TranslationInfoSystemView>(random.Next(1, 25)).ToList();

            IFoodWasteSystemDataService sut = CreateSut<ICommand, TranslationInfoCollectionGetQuery, IEnumerable<TranslationInfoSystemView>>(translationInfoSystemViewCollection);
            Assert.That(sut, Is.Not.Null);

            IEnumerable<TranslationInfoSystemView> result = sut.TranslationInfoGetAll(fixture.Create<TranslationInfoCollectionGetQuery>());
            // ReSharper disable PossibleMultipleEnumeration
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EqualTo(translationInfoSystemViewCollection));
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        /// Tests that TranslationInfoGetAll throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatTranslationInfoGetAllThrowsFaultExceptionWhenExceptionOccurs()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            Exception exception = fixture.Create<Exception>();
            FaultException<FoodWasteFault> faultException = fixture.Create<FaultException<FoodWasteFault>>();

            IFoodWasteSystemDataService sut = CreateSut<ICommand, TranslationInfoCollectionGetQuery, IEnumerable<TranslationInfoSystemView>>(fixture.CreateMany<TranslationInfoSystemView>(random.Next(1, 25)).ToList(), exception, faultException);
            Assert.That(sut, Is.Not.Null);

            FaultException<FoodWasteFault> result = Assert.Throws<FaultException<FoodWasteFault>>(() => sut.TranslationInfoGetAll(fixture.Create<TranslationInfoCollectionGetQuery>()));
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(faultException));

            _faultExceptionBuilderMock.AssertWasCalled(m => m.Build(
                    Arg<Exception>.Is.Equal(exception),
                    Arg<string>.Is.Equal(SoapNamespaces.FoodWasteSystemDataServiceName),
                    Arg<MethodBase>.Matches(src => string.Compare(src.Name, "TranslationInfoGetAll", StringComparison.Ordinal) == 0)),
                opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Creates an instance of the service which can access and modify system data in the food waste domain which can be used for unit testing.
        /// </summary>
        /// <returns>Instance of the service which can access and modify system data in the food waste domain which can be used for unit testing.</returns>
        private IFoodWasteSystemDataService CreateSut()
        {
            _commandBusMock = MockRepository.GenerateMock<ICommandBus>();
            _queryBusMock = MockRepository.GenerateMock<IQueryBus>();
            _faultExceptionBuilderMock = MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>();

            return new FoodWasteSystemDataService(_commandBusMock, _queryBusMock, _faultExceptionBuilderMock);
        }

        /// <summary>
        /// Creates an instance of the service which can access and modify system data in the food waste domain which can be used for unit testing.
        /// </summary>
        /// <typeparam name="TCommand">Type of the command which should be tested.</typeparam>
        /// <typeparam name="TQuery">Type of the query which should be tested.</typeparam>
        /// <typeparam name="TResult">Type of the result which should be tested.</typeparam>
        /// <param name="result">The result of the command or query.</param>
        /// <param name="exception">The exception which should be thrown when command or query are executed.</param>
        /// <param name="faultException">The fault exception which sould be returned for the fault exception builder.</param>
        /// <returns>Instance of the service which can access and modify system data in the food waste domain which can be used for unit testing.</returns>
        private IFoodWasteSystemDataService CreateSut<TCommand, TQuery, TResult>(TResult result, Exception exception = null, FaultException<FoodWasteFault> faultException = null) where TCommand : class, ICommand where TQuery : class, IQuery
        {
            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            _commandBusMock = MockRepository.GenerateMock<ICommandBus>();
            if (exception != null)
            {
                _commandBusMock.Stub(m => m.Publish<TCommand, TResult>(Arg<TCommand>.Is.Anything))
                    .Throw(exception)
                    .Repeat.Any();
            }
            else
            {
                _commandBusMock.Stub(m => m.Publish<TCommand, TResult>(Arg<TCommand>.Is.Anything))
                    .Return(result)
                    .Repeat.Any();
            }

            _queryBusMock = MockRepository.GenerateMock<IQueryBus>();
            if (exception != null)
            {
                _queryBusMock.Stub(m => m.Query<TQuery, TResult>(Arg<TQuery>.Is.Anything))
                    .Throw(exception)
                    .Repeat.Any();
            }
            else
            {
                _queryBusMock.Stub(m => m.Query<TQuery, TResult>(Arg<TQuery>.Is.Anything))
                    .Return(result)
                    .Repeat.Any();
            }

            _faultExceptionBuilderMock = MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>();
            _faultExceptionBuilderMock.Stub(m => m.Build(Arg<Exception>.Is.Anything, Arg<string>.Is.Anything, Arg<MethodBase>.Is.Anything))
                .Return(faultException)
                .Repeat.Any();

            return new FoodWasteSystemDataService(_commandBusMock, _queryBusMock, _faultExceptionBuilderMock);
        }
    }
}
