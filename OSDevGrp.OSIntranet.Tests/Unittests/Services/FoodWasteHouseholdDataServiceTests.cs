using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf;
using OSDevGrp.OSIntranet.Contracts;
using OSDevGrp.OSIntranet.Contracts.Commands;
using OSDevGrp.OSIntranet.Contracts.Faults;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Responses;
using OSDevGrp.OSIntranet.Contracts.Services;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Services.Implementations;
using AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Services
{
    /// <summary>
    /// Tests the service which can access and modify data on a house hold in the food waste domain.
    /// </summary>
    [TestFixture]
    public class FoodWasteHouseholdDataServiceTests
    {
        #region Private variables

        private ICommandBus _commandBusMock;
        private IQueryBus _queryBusMock;
        private IFaultExceptionBuilder<FoodWasteFault> _faultExceptionBuilderMock;

        #endregion

        /// <summary>
        /// Tests that service which can access and modify data in a house hold can be hosted.
        /// </summary>
        [Test]
        public void TestThatFoodWasteHouseholdDataServiceCanBeHosted()
        {
            var uri = new Uri("http://localhost:7000/OSIntranet/");
            var host = new ServiceHost(typeof(FoodWasteHouseholdDataService), uri);
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
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => new FoodWasteHouseholdDataService(null, queryBusMock, faultExceptionBuilderMock));
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
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => new FoodWasteHouseholdDataService(commandBusMock, null, faultExceptionBuilderMock));
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
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => new FoodWasteHouseholdDataService(commandBusMock, queryBusMock, null));
            // ReSharper restore ObjectCreationAsStatement

            TestHelper.AssertArgumentNullExceptionIsValid(result, "foodWasteFaultExceptionBuilder");
        }

        /// <summary>
        /// Tests that StorageTypeGetAll throws an ArgumentNullException when the query for getting all the storage types is null.
        /// </summary>
        [Test]
        public void TestThatStorageTypeGetAllThrowsArgumentNullExceptionIfStorageTypeCollectionGetQueryIsNull()
        {
            IFoodWasteHouseholdDataService sut = CreateSut();
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

            IFoodWasteHouseholdDataService sut = CreateSut<ICommand, StorageTypeCollectionGetQuery, IEnumerable<StorageTypeView>>(fixture.CreateMany<StorageTypeView>(random.Next(1, 25)).ToList());
            Assert.That(sut, Is.Not.Null);

            sut.StorageTypeGetAll(query);

            _queryBusMock.AssertWasCalled(m => m.Query<StorageTypeCollectionGetQuery, IEnumerable<StorageTypeView>>(Arg<StorageTypeCollectionGetQuery>.Is.Equal(query)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that StorageTypeGetAll returns the result from the query bus.
        /// </summary>
        [Test]
        public void TestThatStorageTypeGetAllReturnsResultFromQueryBus()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            IEnumerable<StorageTypeView> storageTypeSystemViewCollection = fixture.CreateMany<StorageTypeView>(random.Next(1, 25)).ToList();

            IFoodWasteHouseholdDataService sut = CreateSut<ICommand, StorageTypeCollectionGetQuery, IEnumerable<StorageTypeView>>(storageTypeSystemViewCollection);
            Assert.That(sut, Is.Not.Null);

            IEnumerable<StorageTypeView> result = sut.StorageTypeGetAll(fixture.Create<StorageTypeCollectionGetQuery>());
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

            IFoodWasteHouseholdDataService sut = CreateSut<ICommand, StorageTypeCollectionGetQuery, IEnumerable<StorageTypeView>>(fixture.CreateMany<StorageTypeView>(random.Next(1, 25)).ToList(), exception, faultException);
            Assert.That(sut, Is.Not.Null);

            FaultException<FoodWasteFault> result = Assert.Throws<FaultException<FoodWasteFault>>(() => sut.StorageTypeGetAll(fixture.Create<StorageTypeCollectionGetQuery>()));
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(faultException));

            _faultExceptionBuilderMock.AssertWasCalled(m => m.Build(
                    Arg<Exception>.Is.Equal(exception),
                    Arg<string>.Is.Equal(SoapNamespaces.FoodWasteHouseholdDataServiceName),
                    Arg<MethodBase>.Matches(src => string.Compare(src.Name, "StorageTypeGetAll", StringComparison.Ordinal) == 0)),
                opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that HouseholdDataGet throws an ArgumentNullException when the query for getting household data for one of the current callers households is null.
        /// </summary>
        [Test]
        public void TestThatHouseholdDataGetThrowsArgumentNullExceptionIfHouseholdDataGetQueryIsNull()
        {
            IFoodWasteHouseholdDataService sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.HouseholdDataGet(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "query");
        }

        /// <summary>
        /// Tests that HouseholdDataGet calls Query on the query bus.
        /// </summary>
        [Test]
        public void TestThatHouseholdDataGetCallsQueryOnQueryBus()
        {
            Fixture fixture = new Fixture();

            HouseholdDataGetQuery query = fixture.Create<HouseholdDataGetQuery>();

            IFoodWasteHouseholdDataService sut = CreateSut<ICommand, HouseholdDataGetQuery, HouseholdView>(fixture.Create<HouseholdView>());
            Assert.That(sut, Is.Not.Null);

            sut.HouseholdDataGet(query);

            _queryBusMock.AssertWasCalled(m => m.Query<HouseholdDataGetQuery, HouseholdView>(Arg<HouseholdDataGetQuery>.Is.Equal(query)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that HouseholdDataGet returns the result from the query bus.
        /// </summary>
        [Test]
        public void TestThatHouseholdDataGetReturnsResultFromQueryBus()
        {
            Fixture fixture = new Fixture();

            HouseholdView householdView = fixture.Create<HouseholdView>();

            IFoodWasteHouseholdDataService sut = CreateSut<ICommand, HouseholdDataGetQuery, HouseholdView>(householdView);
            Assert.That(sut, Is.Not.Null);

            HouseholdView result = sut.HouseholdDataGet(fixture.Create<HouseholdDataGetQuery>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(householdView));
        }

        /// <summary>
        /// Tests that HouseholdDataGet throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatHouseholdDataGetThrowsFaultExceptionWhenExceptionOccurs()
        {
            Fixture fixture = new Fixture();

            Exception exception = fixture.Create<Exception>();
            FaultException<FoodWasteFault> faultException = fixture.Create<FaultException<FoodWasteFault>>();

            IFoodWasteHouseholdDataService sut = CreateSut<ICommand, HouseholdDataGetQuery, HouseholdView>(fixture.Create<HouseholdView>(), exception, faultException);
            Assert.That(sut, Is.Not.Null);

            FaultException<FoodWasteFault> result = Assert.Throws<FaultException<FoodWasteFault>>(() => sut.HouseholdDataGet(fixture.Create<HouseholdDataGetQuery>()));
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(faultException));

            _faultExceptionBuilderMock.AssertWasCalled(m => m.Build(
                    Arg<Exception>.Is.Equal(exception),
                    Arg<string>.Is.Equal(SoapNamespaces.FoodWasteHouseholdDataServiceName),
                    Arg<MethodBase>.Matches(src => string.Compare(src.Name, "HouseholdDataGet", StringComparison.Ordinal) == 0)),
                opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that HouseholdAdd throws an ArgumentNullException when the command for adding a household to the current users household account is null.
        /// </summary>
        [Test]
        public void TestThatHouseholdAddThrowsArgumentNullExceptionIfHouseholdAddCommandIsNull()
        {
            IFoodWasteHouseholdDataService sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.HouseholdAdd(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "command");
        }

        /// <summary>
        /// Tests that HouseholdAdd calls Publish on the command bus.
        /// </summary>
        [Test]
        public void TestThatHouseholdAddCallsPublishOnCommandBus()
        {
            Fixture fixture = new Fixture();

            HouseholdAddCommand command = fixture.Create<HouseholdAddCommand>();

            IFoodWasteHouseholdDataService sut = CreateSut<HouseholdAddCommand, IQuery, ServiceReceiptResponse>(fixture.Create<ServiceReceiptResponse>());
            Assert.That(sut, Is.Not.Null);

            sut.HouseholdAdd(command);

            _commandBusMock.AssertWasCalled(m => m.Publish<HouseholdAddCommand, ServiceReceiptResponse>(Arg<HouseholdAddCommand>.Is.Equal(command)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that HouseholdAdd returns the result from the command bus.
        /// </summary>
        [Test]
        public void TestThatHouseholdAddReturnsResultFromCommand()
        {
            Fixture fixture = new Fixture();

            ServiceReceiptResponse serviceReceipt = fixture.Create<ServiceReceiptResponse>();

            IFoodWasteHouseholdDataService sut = CreateSut<HouseholdAddCommand, IQuery, ServiceReceiptResponse>(serviceReceipt);
            Assert.That(sut, Is.Not.Null);

            ServiceReceiptResponse result = sut.HouseholdAdd(fixture.Create<HouseholdAddCommand>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(serviceReceipt));
        }

        /// <summary>
        /// Tests that HouseholdAdd throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatHouseholdAddThrowsFaultExceptionWhenExceptionOccurs()
        {
            Fixture fixture = new Fixture();

            Exception exception = fixture.Create<Exception>();
            FaultException<FoodWasteFault> faultException = fixture.Create<FaultException<FoodWasteFault>>();

            IFoodWasteHouseholdDataService sut = CreateSut<HouseholdAddCommand, IQuery, ServiceReceiptResponse>(fixture.Create<ServiceReceiptResponse>(), exception, faultException);
            Assert.That(sut, Is.Not.Null);

            FaultException<FoodWasteFault> result = Assert.Throws<FaultException<FoodWasteFault>>(() => sut.HouseholdAdd(fixture.Create<HouseholdAddCommand>()));
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(faultException));

            _faultExceptionBuilderMock.AssertWasCalled(m => m.Build(
                    Arg<Exception>.Is.Equal(exception),
                    Arg<string>.Is.Equal(SoapNamespaces.FoodWasteHouseholdDataServiceName),
                    Arg<MethodBase>.Matches(src => string.Compare(src.Name, "HouseholdAdd", StringComparison.Ordinal) == 0)),
                opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that HouseholdUpdate throws an ArgumentNullException when the command for updatering a household on the current callers household account is null.
        /// </summary>
        [Test]
        public void TestThatHouseholdUpdateThrowsArgumentNullExceptionIfHouseholdUpdateCommandIsNull()
        {
            IFoodWasteHouseholdDataService sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.HouseholdUpdate(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "command");
        }

        /// <summary>
        /// Tests that HouseholdUpdate calls Publish on the command bus.
        /// </summary>
        [Test]
        public void TestThatHouseholdUpdateCallsPublishOnCommandBus()
        {
            Fixture fixture = new Fixture();

            HouseholdUpdateCommand command = fixture.Create<HouseholdUpdateCommand>();

            IFoodWasteHouseholdDataService sut = CreateSut<HouseholdUpdateCommand, IQuery, ServiceReceiptResponse>(fixture.Create<ServiceReceiptResponse>());
            Assert.That(sut, Is.Not.Null);

            sut.HouseholdUpdate(command);

            _commandBusMock.AssertWasCalled(m => m.Publish<HouseholdUpdateCommand, ServiceReceiptResponse>(Arg<HouseholdUpdateCommand>.Is.Equal(command)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that HouseholdUpdate returns the result from the command bus.
        /// </summary>
        [Test]
        public void TestThatHouseholdUpdateReturnsResultFromCommand()
        {
            Fixture fixture = new Fixture();

            ServiceReceiptResponse serviceReceipt = fixture.Create<ServiceReceiptResponse>();

            IFoodWasteHouseholdDataService sut = CreateSut<HouseholdUpdateCommand, IQuery, ServiceReceiptResponse>(serviceReceipt);
            Assert.That(sut, Is.Not.Null);

            ServiceReceiptResponse result = sut.HouseholdUpdate(fixture.Create<HouseholdUpdateCommand>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(serviceReceipt));
        }

        /// <summary>
        /// Tests that HouseholdUpdate throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatHouseholdUpdateThrowsFaultExceptionWhenExceptionOccurs()
        {
            Fixture fixture = new Fixture();

            Exception exception = fixture.Create<Exception>();
            FaultException<FoodWasteFault> faultException = fixture.Create<FaultException<FoodWasteFault>>();

            IFoodWasteHouseholdDataService sut = CreateSut<HouseholdUpdateCommand, IQuery, ServiceReceiptResponse>(fixture.Create<ServiceReceiptResponse>(), exception, faultException);
            Assert.That(sut, Is.Not.Null);

            FaultException<FoodWasteFault> result = Assert.Throws<FaultException<FoodWasteFault>>(() => sut.HouseholdUpdate(fixture.Create<HouseholdUpdateCommand>()));
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(faultException));

            _faultExceptionBuilderMock.AssertWasCalled(m => m.Build(
                    Arg<Exception>.Is.Equal(exception),
                    Arg<string>.Is.Equal(SoapNamespaces.FoodWasteHouseholdDataServiceName),
                    Arg<MethodBase>.Matches(src => string.Compare(src.Name, "HouseholdUpdate", StringComparison.Ordinal) == 0)),
                opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that HouseholdAddHouseholdMember throws an ArgumentNullException when the command for adding a household member to a given household on the current users household account is null.
        /// </summary>
        [Test]
        public void TestThatHouseholdAddHouseholdMemberThrowsArgumentNullExceptionIfHouseholdAddHouseholdMemberCommandIsNull()
        {
            IFoodWasteHouseholdDataService sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.HouseholdAddHouseholdMember(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "command");
        }

        /// <summary>
        /// Tests that HouseholdAddHouseholdMember calls Publish on the command bus.
        /// </summary>
        [Test]
        public void TestThatHouseholdAddHouseholdMemberCallsPublishOnCommandBus()
        {
            Fixture fixture = new Fixture();

            HouseholdAddHouseholdMemberCommand command = fixture.Create<HouseholdAddHouseholdMemberCommand>();

            IFoodWasteHouseholdDataService sut = CreateSut<HouseholdAddHouseholdMemberCommand, IQuery, ServiceReceiptResponse>(fixture.Create<ServiceReceiptResponse>());
            Assert.That(sut, Is.Not.Null);

            sut.HouseholdAddHouseholdMember(command);

            _commandBusMock.AssertWasCalled(m => m.Publish<HouseholdAddHouseholdMemberCommand, ServiceReceiptResponse>(Arg<HouseholdAddHouseholdMemberCommand>.Is.Equal(command)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that HouseholdAddHouseholdMember returns the result from the command bus.
        /// </summary>
        [Test]
        public void TestThatHouseholdAddHouseholdMemberReturnsResultFromCommand()
        {
            Fixture fixture = new Fixture();

            ServiceReceiptResponse serviceReceipt = fixture.Create<ServiceReceiptResponse>();

            IFoodWasteHouseholdDataService sut = CreateSut<HouseholdAddHouseholdMemberCommand, IQuery, ServiceReceiptResponse>(serviceReceipt);
            Assert.That(sut, Is.Not.Null);

            ServiceReceiptResponse result = sut.HouseholdAddHouseholdMember(fixture.Create<HouseholdAddHouseholdMemberCommand>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(serviceReceipt));
        }

        /// <summary>
        /// Tests that HouseholdAddHouseholdMember throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatHouseholdAddHouseholdMemberThrowsFaultExceptionWhenExceptionOccurs()
        {
            Fixture fixture = new Fixture();

            Exception exception = fixture.Create<Exception>();
            FaultException<FoodWasteFault> faultException = fixture.Create<FaultException<FoodWasteFault>>();

            IFoodWasteHouseholdDataService sut = CreateSut<HouseholdAddHouseholdMemberCommand, IQuery, ServiceReceiptResponse>(fixture.Create<ServiceReceiptResponse>(), exception, faultException);
            Assert.That(sut, Is.Not.Null);

            FaultException<FoodWasteFault> result = Assert.Throws<FaultException<FoodWasteFault>>(() => sut.HouseholdAddHouseholdMember(fixture.Create<HouseholdAddHouseholdMemberCommand>()));
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(faultException));

            _faultExceptionBuilderMock.AssertWasCalled(m => m.Build(
                    Arg<Exception>.Is.Equal(exception),
                    Arg<string>.Is.Equal(SoapNamespaces.FoodWasteHouseholdDataServiceName),
                    Arg<MethodBase>.Matches(src => string.Compare(src.Name, "HouseholdAddHouseholdMember", StringComparison.Ordinal) == 0)),
                opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that HouseholdRemoveHouseholdMember throws an ArgumentNullException when the command for removing a household member from a given household on the current users household account is null.
        /// </summary>
        [Test]
        public void TestThatHouseholdRemoveHouseholdMemberThrowsArgumentNullExceptionIfHouseholdRemoveHouseholdMemberCommandIsNull()
        {
            IFoodWasteHouseholdDataService sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.HouseholdRemoveHouseholdMember(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "command");
        }

        /// <summary>
        /// Tests that HouseholdRemoveHouseholdMember calls Publish on the command bus.
        /// </summary>
        [Test]
        public void TestThatHouseholdRemoveHouseholdMemberCallsPublishOnCommandBus()
        {
            Fixture fixture = new Fixture();

            HouseholdRemoveHouseholdMemberCommand command = fixture.Create<HouseholdRemoveHouseholdMemberCommand>();

            IFoodWasteHouseholdDataService sut = CreateSut<HouseholdRemoveHouseholdMemberCommand, IQuery, ServiceReceiptResponse>(fixture.Create<ServiceReceiptResponse>());
            Assert.That(sut, Is.Not.Null);

            sut.HouseholdRemoveHouseholdMember(command);

            _commandBusMock.AssertWasCalled(m => m.Publish<HouseholdRemoveHouseholdMemberCommand, ServiceReceiptResponse>(Arg<HouseholdRemoveHouseholdMemberCommand>.Is.Equal(command)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that HouseholdRemoveHouseholdMember returns the result from the command bus.
        /// </summary>
        [Test]
        public void TestThatHouseholdRemoveHouseholdMemberReturnsResultFromCommand()
        {
            Fixture fixture = new Fixture();

            ServiceReceiptResponse serviceReceipt = fixture.Create<ServiceReceiptResponse>();

            IFoodWasteHouseholdDataService sut = CreateSut<HouseholdRemoveHouseholdMemberCommand, IQuery, ServiceReceiptResponse>(serviceReceipt);
            Assert.That(sut, Is.Not.Null);

            ServiceReceiptResponse result = sut.HouseholdRemoveHouseholdMember(fixture.Create<HouseholdRemoveHouseholdMemberCommand>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(serviceReceipt));
        }

        /// <summary>
        /// Tests that HouseholdRemoveHouseholdMember throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatHouseholdRemoveHouseholdMemberThrowsFaultExceptionWhenExceptionOccurs()
        {
            Fixture fixture = new Fixture();

            Exception exception = fixture.Create<Exception>();
            FaultException<FoodWasteFault> faultException = fixture.Create<FaultException<FoodWasteFault>>();

            IFoodWasteHouseholdDataService sut = CreateSut<HouseholdRemoveHouseholdMemberCommand, IQuery, ServiceReceiptResponse>(fixture.Create<ServiceReceiptResponse>(), exception, faultException);
            Assert.That(sut, Is.Not.Null);

            FaultException<FoodWasteFault> result = Assert.Throws<FaultException<FoodWasteFault>>(() => sut.HouseholdRemoveHouseholdMember(fixture.Create<HouseholdRemoveHouseholdMemberCommand>()));
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(faultException));

            _faultExceptionBuilderMock.AssertWasCalled(m => m.Build(
                    Arg<Exception>.Is.Equal(exception),
                    Arg<string>.Is.Equal(SoapNamespaces.FoodWasteHouseholdDataServiceName),
                    Arg<MethodBase>.Matches(src => string.Compare(src.Name, "HouseholdRemoveHouseholdMember", StringComparison.Ordinal) == 0)),
                opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that HouseholdMemberIsCreated throws an ArgumentNullException when the query which can check whether the current caller has been created as a household member is null.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberIsCreatedThrowsArgumentNullExceptionIfHouseholdMemberIsCreatedQueryIsNull()
        {
            IFoodWasteHouseholdDataService sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.HouseholdMemberIsCreated(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "query");
        }

        /// <summary>
        /// Tests that HouseholdMemberIsCreated calls Query on the query bus.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberIsCreatedCallsQueryOnQueryBus()
        {
            Fixture fixture = new Fixture();

            HouseholdMemberIsCreatedQuery query = fixture.Create<HouseholdMemberIsCreatedQuery>();

            IFoodWasteHouseholdDataService sut = CreateSut<ICommand, HouseholdMemberIsCreatedQuery, BooleanResultResponse>(fixture.Create<BooleanResultResponse>());
            Assert.That(sut, Is.Not.Null);

            sut.HouseholdMemberIsCreated(query);

            _queryBusMock.AssertWasCalled(m => m.Query<HouseholdMemberIsCreatedQuery, BooleanResultResponse>(Arg<HouseholdMemberIsCreatedQuery>.Is.Equal(query)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that HouseholdMemberIsCreated returns the result from the query bus.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberIsCreatedReturnsResultFromQueryBus()
        {
            Fixture fixture = new Fixture();

            BooleanResultResponse booleanResult = fixture.Create<BooleanResultResponse>();

            IFoodWasteHouseholdDataService sut = CreateSut<ICommand, HouseholdMemberIsCreatedQuery, BooleanResultResponse>(booleanResult);
            Assert.That(sut, Is.Not.Null);

            BooleanResultResponse result = sut.HouseholdMemberIsCreated(fixture.Create<HouseholdMemberIsCreatedQuery>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(booleanResult));
        }

        /// <summary>
        /// Tests that HouseholdMemberIsCreated throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberIsCreatedThrowsFaultExceptionWhenExceptionOccurs()
        {
            Fixture fixture = new Fixture();

            Exception exception = fixture.Create<Exception>();
            FaultException<FoodWasteFault> faultException = fixture.Create<FaultException<FoodWasteFault>>();

            IFoodWasteHouseholdDataService sut = CreateSut<ICommand, HouseholdMemberIsCreatedQuery, BooleanResultResponse>(fixture.Create<BooleanResultResponse>(), exception, faultException);
            Assert.That(sut, Is.Not.Null);

            FaultException<FoodWasteFault> result = Assert.Throws<FaultException<FoodWasteFault>>(() => sut.HouseholdMemberIsCreated(fixture.Create<HouseholdMemberIsCreatedQuery>()));
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(faultException));

            _faultExceptionBuilderMock.AssertWasCalled(m => m.Build(
                    Arg<Exception>.Is.Equal(exception),
                    Arg<string>.Is.Equal(SoapNamespaces.FoodWasteHouseholdDataServiceName),
                    Arg<MethodBase>.Matches(src => string.Compare(src.Name, "HouseholdMemberIsCreated", StringComparison.Ordinal) == 0)),
                opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that HouseholdMemberIsActivated throws an ArgumentNullException when the query which can check whether the current caller has been activated is null.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberIsActivatedThrowsArgumentNullExceptionIfHouseholdMemberIsActivatedQueryIsNull()
        {
            IFoodWasteHouseholdDataService sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.HouseholdMemberIsActivated(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "query");
        }

        /// <summary>
        /// Tests that HouseholdMemberIsActivated calls Query on the query bus.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberIsActivatedCallsQueryOnQueryBus()
        {
            Fixture fixture = new Fixture();

            HouseholdMemberIsActivatedQuery query = fixture.Create<HouseholdMemberIsActivatedQuery>();

            IFoodWasteHouseholdDataService sut = CreateSut<ICommand, HouseholdMemberIsActivatedQuery, BooleanResultResponse>(fixture.Create<BooleanResultResponse>());
            Assert.That(sut, Is.Not.Null);

            sut.HouseholdMemberIsActivated(query);

            _queryBusMock.AssertWasCalled(m => m.Query<HouseholdMemberIsActivatedQuery, BooleanResultResponse>(Arg<HouseholdMemberIsActivatedQuery>.Is.Equal(query)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that HouseholdMemberIsActivated returns the result from the query bus.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberIsActivatedReturnsResultFromQueryBus()
        {
            Fixture fixture = new Fixture();

            BooleanResultResponse booleanResult = fixture.Create<BooleanResultResponse>();

            IFoodWasteHouseholdDataService sut = CreateSut<ICommand, HouseholdMemberIsActivatedQuery, BooleanResultResponse>(booleanResult);
            Assert.That(sut, Is.Not.Null);

            BooleanResultResponse result = sut.HouseholdMemberIsActivated(fixture.Create<HouseholdMemberIsActivatedQuery>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(booleanResult));
        }

        /// <summary>
        /// Tests that HouseholdMemberIsActivated throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberIsActivatedThrowsFaultExceptionWhenExceptionOccurs()
        {
            Fixture fixture = new Fixture();

            Exception exception = fixture.Create<Exception>();
            FaultException<FoodWasteFault> faultException = fixture.Create<FaultException<FoodWasteFault>>();

            IFoodWasteHouseholdDataService sut = CreateSut<ICommand, HouseholdMemberIsActivatedQuery, BooleanResultResponse>(fixture.Create<BooleanResultResponse>(), exception, faultException);
            Assert.That(sut, Is.Not.Null);

            FaultException<FoodWasteFault> result = Assert.Throws<FaultException<FoodWasteFault>>(() => sut.HouseholdMemberIsActivated(fixture.Create<HouseholdMemberIsActivatedQuery>()));
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(faultException));

            _faultExceptionBuilderMock.AssertWasCalled(m => m.Build(
                    Arg<Exception>.Is.Equal(exception),
                    Arg<string>.Is.Equal(SoapNamespaces.FoodWasteHouseholdDataServiceName),
                    Arg<MethodBase>.Matches(src => string.Compare(src.Name, "HouseholdMemberIsActivated", StringComparison.Ordinal) == 0)),
                opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that HouseholdMemberHasAcceptedPrivacyPolicy throws an ArgumentNullException when the query which can check whether the current caller has accepted the privacy policy is null.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberHasAcceptedPrivacyPolicyThrowsArgumentNullExceptionIfHouseholdMemberHasAcceptedPrivacyPolicyQueryIsNull()
        {
            IFoodWasteHouseholdDataService sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.HouseholdMemberHasAcceptedPrivacyPolicy(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "query");
        }

        /// <summary>
        /// Tests that HouseholdMemberHasAcceptedPrivacyPolicy calls Query on the query bus.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberHasAcceptedPrivacyPolicyCallsQueryOnQueryBus()
        {
            Fixture fixture = new Fixture();

            HouseholdMemberHasAcceptedPrivacyPolicyQuery query = fixture.Create<HouseholdMemberHasAcceptedPrivacyPolicyQuery>();

            IFoodWasteHouseholdDataService sut = CreateSut<ICommand, HouseholdMemberHasAcceptedPrivacyPolicyQuery, BooleanResultResponse>(fixture.Create<BooleanResultResponse>());
            Assert.That(sut, Is.Not.Null);

            sut.HouseholdMemberHasAcceptedPrivacyPolicy(query);

            _queryBusMock.AssertWasCalled(m => m.Query<HouseholdMemberHasAcceptedPrivacyPolicyQuery, BooleanResultResponse>(Arg<HouseholdMemberHasAcceptedPrivacyPolicyQuery>.Is.Equal(query)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that HouseholdMemberHasAcceptedPrivacyPolicy returns the result from the query bus.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberHasAcceptedPrivacyPolicyReturnsResultFromQueryBus()
        {
            Fixture fixture = new Fixture();

            BooleanResultResponse booleanResult = fixture.Create<BooleanResultResponse>();

            IFoodWasteHouseholdDataService sut = CreateSut<ICommand, HouseholdMemberHasAcceptedPrivacyPolicyQuery, BooleanResultResponse>(booleanResult);
            Assert.That(sut, Is.Not.Null);

            BooleanResultResponse result = sut.HouseholdMemberHasAcceptedPrivacyPolicy(fixture.Create<HouseholdMemberHasAcceptedPrivacyPolicyQuery>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(booleanResult));
        }

        /// <summary>
        /// Tests that HouseholdMemberHasAcceptedPrivacyPolicy throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberHasAcceptedPrivacyPolicyThrowsFaultExceptionWhenExceptionOccurs()
        {
            Fixture fixture = new Fixture();

            Exception exception = fixture.Create<Exception>();
            FaultException<FoodWasteFault> faultException = fixture.Create<FaultException<FoodWasteFault>>();

            IFoodWasteHouseholdDataService sut = CreateSut<ICommand, HouseholdMemberHasAcceptedPrivacyPolicyQuery, BooleanResultResponse>(fixture.Create<BooleanResultResponse>(), exception, faultException);
            Assert.That(sut, Is.Not.Null);

            FaultException<FoodWasteFault> result = Assert.Throws<FaultException<FoodWasteFault>>(() => sut.HouseholdMemberHasAcceptedPrivacyPolicy(fixture.Create<HouseholdMemberHasAcceptedPrivacyPolicyQuery>()));
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(faultException));

            _faultExceptionBuilderMock.AssertWasCalled(m => m.Build(
                    Arg<Exception>.Is.Equal(exception),
                    Arg<string>.Is.Equal(SoapNamespaces.FoodWasteHouseholdDataServiceName),
                    Arg<MethodBase>.Matches(src => string.Compare(src.Name, "HouseholdMemberHasAcceptedPrivacyPolicy", StringComparison.Ordinal) == 0)),
                opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that HouseholdMemberDataGet throws an ArgumentNullException when the query which can get household member data for the current caller is null.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberDataGetThrowsArgumentNullExceptionIfHouseholdMemberDataGetQueryIsNull()
        {
            IFoodWasteHouseholdDataService sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.HouseholdMemberDataGet(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "query");
        }

        /// <summary>
        /// Tests that HouseholdMemberDataGet calls Query on the query bus.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberDataGetCallsQueryOnQueryBus()
        {
            Fixture fixture = new Fixture();

            HouseholdMemberDataGetQuery query = fixture.Create<HouseholdMemberDataGetQuery>();

            IFoodWasteHouseholdDataService sut = CreateSut<ICommand, HouseholdMemberDataGetQuery, HouseholdMemberView>(fixture.Create<HouseholdMemberView>());
            Assert.That(sut, Is.Not.Null);

            sut.HouseholdMemberDataGet(query);

            _queryBusMock.AssertWasCalled(m => m.Query<HouseholdMemberDataGetQuery, HouseholdMemberView>(Arg<HouseholdMemberDataGetQuery>.Is.Equal(query)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that HouseholdMemberDataGet returns the result from the query bus.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberDataGetReturnsResultFromQueryBus()
        {
            Fixture fixture = new Fixture();

            HouseholdMemberView householdMemberView = fixture.Create<HouseholdMemberView>();

            IFoodWasteHouseholdDataService sut = CreateSut<ICommand, HouseholdMemberDataGetQuery, HouseholdMemberView>(householdMemberView);
            Assert.That(sut, Is.Not.Null);

            HouseholdMemberView result = sut.HouseholdMemberDataGet(fixture.Create<HouseholdMemberDataGetQuery>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(householdMemberView));
        }

        /// <summary>
        /// Tests that HouseholdMemberDataGet throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberDataGetThrowsFaultExceptionWhenExceptionOccurs()
        {
            Fixture fixture = new Fixture();

            Exception exception = fixture.Create<Exception>();
            FaultException<FoodWasteFault> faultException = fixture.Create<FaultException<FoodWasteFault>>();

            IFoodWasteHouseholdDataService sut = CreateSut<ICommand, HouseholdMemberDataGetQuery, HouseholdMemberView>(fixture.Create<HouseholdMemberView>(), exception, faultException);
            Assert.That(sut, Is.Not.Null);

            FaultException<FoodWasteFault> result = Assert.Throws<FaultException<FoodWasteFault>>(() => sut.HouseholdMemberDataGet(fixture.Create<HouseholdMemberDataGetQuery>()));
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(faultException));

            _faultExceptionBuilderMock.AssertWasCalled(m => m.Build(
                    Arg<Exception>.Is.Equal(exception),
                    Arg<string>.Is.Equal(SoapNamespaces.FoodWasteHouseholdDataServiceName),
                    Arg<MethodBase>.Matches(src => string.Compare(src.Name, "HouseholdMemberDataGet", StringComparison.Ordinal) == 0)),
                opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that HouseholdMemberActivate throws an ArgumentNullException when the command for activating the current callers household member account is null.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberActivateThrowsArgumentNullExceptionIfHouseholdMemberActivateCommandIsNull()
        {
            IFoodWasteHouseholdDataService sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.HouseholdMemberActivate(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "command");
        }

        /// <summary>
        /// Tests that HouseholdMemberActivate calls Publish on the command bus.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberActivateCallsPublishOnCommandBus()
        {
            Fixture fixture = new Fixture();

            HouseholdMemberActivateCommand command = fixture.Create<HouseholdMemberActivateCommand>();

            IFoodWasteHouseholdDataService sut = CreateSut<HouseholdMemberActivateCommand, IQuery, ServiceReceiptResponse>(fixture.Create<ServiceReceiptResponse>());
            Assert.That(sut, Is.Not.Null);

            sut.HouseholdMemberActivate(command);

            _commandBusMock.AssertWasCalled(m => m.Publish<HouseholdMemberActivateCommand, ServiceReceiptResponse>(Arg<HouseholdMemberActivateCommand>.Is.Equal(command)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that HouseholdMemberActivate returns the result from the command bus.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberActivateReturnsResultFromCommand()
        {
            Fixture fixture = new Fixture();

            ServiceReceiptResponse serviceReceipt = fixture.Create<ServiceReceiptResponse>();

            IFoodWasteHouseholdDataService sut = CreateSut<HouseholdMemberActivateCommand, IQuery, ServiceReceiptResponse>(serviceReceipt);
            Assert.That(sut, Is.Not.Null);

            ServiceReceiptResponse result = sut.HouseholdMemberActivate(fixture.Create<HouseholdMemberActivateCommand>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(serviceReceipt));
        }

        /// <summary>
        /// Tests that HouseholdMemberActivate throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberActivateThrowsFaultExceptionWhenExceptionOccurs()
        {
            Fixture fixture = new Fixture();

            Exception exception = fixture.Create<Exception>();
            FaultException<FoodWasteFault> faultException = fixture.Create<FaultException<FoodWasteFault>>();

            IFoodWasteHouseholdDataService sut = CreateSut<HouseholdMemberActivateCommand, IQuery, ServiceReceiptResponse>(fixture.Create<ServiceReceiptResponse>(), exception, faultException);
            Assert.That(sut, Is.Not.Null);

            FaultException<FoodWasteFault> result = Assert.Throws<FaultException<FoodWasteFault>>(() => sut.HouseholdMemberActivate(fixture.Create<HouseholdMemberActivateCommand>()));
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(faultException));

            _faultExceptionBuilderMock.AssertWasCalled(m => m.Build(
                    Arg<Exception>.Is.Equal(exception),
                    Arg<string>.Is.Equal(SoapNamespaces.FoodWasteHouseholdDataServiceName),
                    Arg<MethodBase>.Matches(src => string.Compare(src.Name, "HouseholdMemberActivate", StringComparison.Ordinal) == 0)),
                opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that HouseholdMemberAcceptPrivacyPolicy throws an ArgumentNullException when the command for accepting privacy policy on the current callers household member account is null.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberAcceptPrivacyPolicyThrowsArgumentNullExceptionIfHouseholdMemberAcceptPrivacyPolicyCommandIsNull()
        {
            IFoodWasteHouseholdDataService sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.HouseholdMemberAcceptPrivacyPolicy(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "command");
        }

        /// <summary>
        /// Tests that HouseholdMemberAcceptPrivacyPolicy calls Publish on the command bus.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberAcceptPrivacyPolicyCallsPublishOnCommandBus()
        {
            Fixture fixture = new Fixture();

            HouseholdMemberAcceptPrivacyPolicyCommand command = fixture.Create<HouseholdMemberAcceptPrivacyPolicyCommand>();

            IFoodWasteHouseholdDataService sut = CreateSut<HouseholdMemberAcceptPrivacyPolicyCommand, IQuery, ServiceReceiptResponse>(fixture.Create<ServiceReceiptResponse>());
            Assert.That(sut, Is.Not.Null);

            sut.HouseholdMemberAcceptPrivacyPolicy(command);

            _commandBusMock.AssertWasCalled(m => m.Publish<HouseholdMemberAcceptPrivacyPolicyCommand, ServiceReceiptResponse>(Arg<HouseholdMemberAcceptPrivacyPolicyCommand>.Is.Equal(command)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that HouseholdMemberAcceptPrivacyPolicy returns the result from the command bus.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberAcceptPrivacyPolicyReturnsResultFromCommand()
        {
            Fixture fixture = new Fixture();

            ServiceReceiptResponse serviceReceipt = fixture.Create<ServiceReceiptResponse>();

            IFoodWasteHouseholdDataService sut = CreateSut<HouseholdMemberAcceptPrivacyPolicyCommand, IQuery, ServiceReceiptResponse>(serviceReceipt);
            Assert.That(sut, Is.Not.Null);

            ServiceReceiptResponse result = sut.HouseholdMemberAcceptPrivacyPolicy(fixture.Create<HouseholdMemberAcceptPrivacyPolicyCommand>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(serviceReceipt));
        }

        /// <summary>
        /// Tests that HouseholdMemberAcceptPrivacyPolicy throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberAcceptPrivacyPolicyThrowsFaultExceptionWhenExceptionOccurs()
        {
            Fixture fixture = new Fixture();

            Exception exception = fixture.Create<Exception>();
            FaultException<FoodWasteFault> faultException = fixture.Create<FaultException<FoodWasteFault>>();

            IFoodWasteHouseholdDataService sut = CreateSut<HouseholdMemberAcceptPrivacyPolicyCommand, IQuery, ServiceReceiptResponse>(fixture.Create<ServiceReceiptResponse>(), exception, faultException);
            Assert.That(sut, Is.Not.Null);

            FaultException<FoodWasteFault> result = Assert.Throws<FaultException<FoodWasteFault>>(() => sut.HouseholdMemberAcceptPrivacyPolicy(fixture.Create<HouseholdMemberAcceptPrivacyPolicyCommand>()));
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(faultException));

            _faultExceptionBuilderMock.AssertWasCalled(m => m.Build(
                    Arg<Exception>.Is.Equal(exception),
                    Arg<string>.Is.Equal(SoapNamespaces.FoodWasteHouseholdDataServiceName),
                    Arg<MethodBase>.Matches(src => string.Compare(src.Name, "HouseholdMemberAcceptPrivacyPolicy", StringComparison.Ordinal) == 0)),
                opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that HouseholdMemberUpgradeMembership throws an ArgumentNullException when the command for upgrading the membership on the current callers household member account is null.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberUpgradeMembershipThrowsArgumentNullExceptionIfHouseholdMemberUpgradeMembershipCommandIsNull()
        {
            IFoodWasteHouseholdDataService sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.HouseholdMemberUpgradeMembership(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "command");
        }

        /// <summary>
        /// Tests that HouseholdMemberUpgradeMembership calls Publish on the command bus.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberUpgradeMembershipCallsPublishOnCommandBus()
        {
            Fixture fixture = new Fixture();

            HouseholdMemberUpgradeMembershipCommand command = fixture.Create<HouseholdMemberUpgradeMembershipCommand>();

            IFoodWasteHouseholdDataService sut = CreateSut<HouseholdMemberUpgradeMembershipCommand, IQuery, ServiceReceiptResponse>(fixture.Create<ServiceReceiptResponse>());
            Assert.That(sut, Is.Not.Null);

            sut.HouseholdMemberUpgradeMembership(command);

            _commandBusMock.AssertWasCalled(m => m.Publish<HouseholdMemberUpgradeMembershipCommand, ServiceReceiptResponse>(Arg<HouseholdMemberUpgradeMembershipCommand>.Is.Equal(command)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that HouseholdMemberUpgradeMembership returns the result from the command bus.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberUpgradeMembershipReturnsResultFromCommand()
        {
            Fixture fixture = new Fixture();

            ServiceReceiptResponse serviceReceipt = fixture.Create<ServiceReceiptResponse>();

            IFoodWasteHouseholdDataService sut = CreateSut<HouseholdMemberUpgradeMembershipCommand, IQuery, ServiceReceiptResponse>(serviceReceipt);
            Assert.That(sut, Is.Not.Null);

            ServiceReceiptResponse result = sut.HouseholdMemberUpgradeMembership(fixture.Create<HouseholdMemberUpgradeMembershipCommand>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(serviceReceipt));
        }

        /// <summary>
        /// Tests that HouseholdMemberUpgradeMembership throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberUpgradeMembershipThrowsFaultExceptionWhenExceptionOccurs()
        {
            Fixture fixture = new Fixture();

            Exception exception = fixture.Create<Exception>();
            FaultException<FoodWasteFault> faultException = fixture.Create<FaultException<FoodWasteFault>>();

            IFoodWasteHouseholdDataService sut = CreateSut<HouseholdMemberUpgradeMembershipCommand, IQuery, ServiceReceiptResponse>(fixture.Create<ServiceReceiptResponse>(), exception, faultException);
            Assert.That(sut, Is.Not.Null);

            FaultException<FoodWasteFault> result = Assert.Throws<FaultException<FoodWasteFault>>(() => sut.HouseholdMemberUpgradeMembership(fixture.Create<HouseholdMemberUpgradeMembershipCommand>()));
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(faultException));

            _faultExceptionBuilderMock.AssertWasCalled(m => m.Build(
                    Arg<Exception>.Is.Equal(exception),
                    Arg<string>.Is.Equal(SoapNamespaces.FoodWasteHouseholdDataServiceName),
                    Arg<MethodBase>.Matches(src => string.Compare(src.Name, "HouseholdMemberUpgradeMembership", StringComparison.Ordinal) == 0)),
                opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that FoodItemCollectionGet throws an ArgumentNullException when the query for getting all the collection of food items is null.
        /// </summary>
        [Test]
        public void TestThatFoodItemCollectionGetThrowsArgumentNullExceptionIfFoodItemColletionGetQueryIsNull()
        {
            IFoodWasteHouseholdDataService sut = CreateSut();
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

            IFoodWasteHouseholdDataService sut = CreateSut<ICommand, FoodItemCollectionGetQuery, FoodItemCollectionView>(fixture.Create<FoodItemCollectionView>());
            Assert.That(sut, Is.Not.Null);

            sut.FoodItemCollectionGet(query);

            _queryBusMock.AssertWasCalled(m => m.Query<FoodItemCollectionGetQuery, FoodItemCollectionView>(Arg<FoodItemCollectionGetQuery>.Is.Equal(query)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that FoodItemCollectionGet returns the result from the query bus.
        /// </summary>
        [Test]
        public void TestThatFoodItemCollectionGetReturnsResultFromQueryBus()
        {
            Fixture fixture = new Fixture();

            FoodItemCollectionView foodItemCollectionView = fixture.Create<FoodItemCollectionView>();

            IFoodWasteHouseholdDataService sut = CreateSut<ICommand, FoodItemCollectionGetQuery, FoodItemCollectionView>(foodItemCollectionView);
            Assert.That(sut, Is.Not.Null);

            FoodItemCollectionView result = sut.FoodItemCollectionGet(fixture.Create<FoodItemCollectionGetQuery>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(foodItemCollectionView));
        }

        /// <summary>
        /// Tests that FoodItemCollectionGet throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatFoodItemCollectionGetThrowsFaultExceptionWhenExceptionOccurs()
        {
            Fixture fixture = new Fixture();

            Exception exception = fixture.Create<Exception>();
            FaultException<FoodWasteFault> faultException = fixture.Create<FaultException<FoodWasteFault>>();

            IFoodWasteHouseholdDataService sut = CreateSut<ICommand, FoodItemCollectionGetQuery, FoodItemCollectionView>(fixture.Create<FoodItemCollectionView>(), exception, faultException);
            Assert.That(sut, Is.Not.Null);

            FaultException<FoodWasteFault> result  = Assert.Throws<FaultException<FoodWasteFault>>(() => sut.FoodItemCollectionGet(fixture.Create<FoodItemCollectionGetQuery>()));
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(faultException));

            _faultExceptionBuilderMock.AssertWasCalled(m => m.Build(
                    Arg<Exception>.Is.Equal(exception),
                    Arg<string>.Is.Equal(SoapNamespaces.FoodWasteHouseholdDataServiceName),
                    Arg<MethodBase>.Matches(src => string.Compare(src.Name, "FoodItemCollectionGet", StringComparison.Ordinal) == 0)),
                opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that FoodGroupTreeGet throws an ArgumentNullException when the query for getting all the tree of food groups is null.
        /// </summary>
        [Test]
        public void TestThatFoodGroupTreeGetThrowsArgumentNullExceptionIfFoodGroupTreeGetQueryIsNull()
        {
            IFoodWasteHouseholdDataService sut = CreateSut();
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
            Random random = new Random(fixture.Create<int>());

            FoodGroupTreeGetQuery query = fixture.Create<FoodGroupTreeGetQuery>();

            IEnumerable<FoodGroupView> foodGroupViewCollection = fixture.Build<FoodGroupView>()
                .With(m => m.Children, new List<FoodGroupView>(0))
                .CreateMany(random.Next(5, 10))
                .ToList();
            FoodGroupTreeView foodGroupTreeView = fixture.Build<FoodGroupTreeView>()
                .With(m => m.FoodGroups, foodGroupViewCollection)
                .Create();

            IFoodWasteHouseholdDataService sut = CreateSut<ICommand, FoodGroupTreeGetQuery, FoodGroupTreeView>(foodGroupTreeView);
            Assert.That(sut, Is.Not.Null);

            sut.FoodGroupTreeGet(query);

            _queryBusMock.AssertWasCalled(m => m.Query<FoodGroupTreeGetQuery, FoodGroupTreeView>(Arg<FoodGroupTreeGetQuery>.Is.Equal(query)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that FoodGroupTreeGet returns the result from the query bus.
        /// </summary>
        [Test]
        public void TestThatFoodGroupTreeGetReturnsResultFromQueryBus()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            IEnumerable<FoodGroupView> foodGroupViewCollection = fixture.Build<FoodGroupView>()
                .With(m => m.Children, new List<FoodGroupView>(0))
                .CreateMany(random.Next(5, 10))
                .ToList();
            FoodGroupTreeView foodGroupTreeView = fixture.Build<FoodGroupTreeView>()
                .With(m => m.FoodGroups, foodGroupViewCollection)
                .Create();

            IFoodWasteHouseholdDataService sut = CreateSut<ICommand, FoodGroupTreeGetQuery, FoodGroupTreeView>(foodGroupTreeView);
            Assert.That(sut, Is.Not.Null);

            FoodGroupTreeView result = sut.FoodGroupTreeGet(fixture.Create<FoodGroupTreeGetQuery>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(foodGroupTreeView));
        }

        /// <summary>
        /// Tests that FoodGroupTreeGet throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatFoodGroupTreeGetThrowsFaultExceptionWhenExceptionOccurs()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            IEnumerable<FoodGroupView> foodGroupViewCollection = fixture.Build<FoodGroupView>()
                .With(m => m.Children, new List<FoodGroupView>(0))
                .CreateMany(random.Next(5, 10))
                .ToList();
            FoodGroupTreeView foodGroupTreeView = fixture.Build<FoodGroupTreeView>()
                .With(m => m.FoodGroups, foodGroupViewCollection)
                .Create();

            Exception exception = fixture.Create<Exception>();
            FaultException<FoodWasteFault> faultException = fixture.Create<FaultException<FoodWasteFault>>();

            IFoodWasteHouseholdDataService sut = CreateSut<ICommand, FoodGroupTreeGetQuery, FoodGroupTreeView>(foodGroupTreeView, exception, faultException);
            Assert.That(sut, Is.Not.Null);

            FaultException<FoodWasteFault> result = Assert.Throws<FaultException<FoodWasteFault>>(() => sut.FoodGroupTreeGet(fixture.Create<FoodGroupTreeGetQuery>()));
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(faultException));

            _faultExceptionBuilderMock.AssertWasCalled(m => m.Build(
                    Arg<Exception>.Is.Equal(exception),
                    Arg<string>.Is.Equal(SoapNamespaces.FoodWasteHouseholdDataServiceName),
                    Arg<MethodBase>.Matches(src => string.Compare(src.Name, "FoodGroupTreeGet", StringComparison.Ordinal) == 0)),
                opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that PrivacyPolicyGet throws an ArgumentNullException when the query for getting the privacy policy is null.
        /// </summary>
        [Test]
        public void TestThatPrivacyPolicyGetThrowsArgumentNullExceptionIfPrivacyPolicyGetQueryIsNull()
        {
            IFoodWasteHouseholdDataService sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.PrivacyPolicyGet(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "query");
        }

        /// <summary>
        /// Tests that PrivacyPolicyGet calls Query on the query bus.
        /// </summary>
        [Test]
        public void TestThatPrivacyPolicyGetCallsQueryOnQueryBus()
        {
            Fixture fixture = new Fixture();

            PrivacyPolicyGetQuery query = fixture.Create<PrivacyPolicyGetQuery>();

            IFoodWasteHouseholdDataService sut = CreateSut<ICommand, PrivacyPolicyGetQuery, StaticTextView>(fixture.Create<StaticTextView>());
            Assert.That(sut, Is.Not.Null);

            sut.PrivacyPolicyGet(query);

            _queryBusMock.AssertWasCalled(m => m.Query<PrivacyPolicyGetQuery, StaticTextView>(Arg<PrivacyPolicyGetQuery>.Is.Equal(query)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that PrivacyPolicyGet returns the result from the query bus.
        /// </summary>
        [Test]
        public void TestThatPrivacyPolicyGetReturnsResultFromQueryBus()
        {
            Fixture fixture = new Fixture();

            StaticTextView staticTextView = fixture.Create<StaticTextView>();

            IFoodWasteHouseholdDataService sut = CreateSut<ICommand, PrivacyPolicyGetQuery, StaticTextView>(staticTextView);
            Assert.That(sut, Is.Not.Null);

            StaticTextView result = sut.PrivacyPolicyGet(fixture.Create<PrivacyPolicyGetQuery>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(staticTextView));
        }

        /// <summary>
        /// Tests that PrivacyPolicyGet throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatPrivacyPolicyGetThrowsFaultExceptionWhenExceptionOccurs()
        {
            Fixture fixture = new Fixture();

            Exception exception = fixture.Create<Exception>();
            FaultException<FoodWasteFault> faultException = fixture.Create<FaultException<FoodWasteFault>>();

            IFoodWasteHouseholdDataService sut = CreateSut<ICommand, PrivacyPolicyGetQuery, StaticTextView>(fixture.Create<StaticTextView>(), exception, faultException);
            Assert.That(sut, Is.Not.Null);

            FaultException<FoodWasteFault> result = Assert.Throws<FaultException<FoodWasteFault>>(() => sut.PrivacyPolicyGet(fixture.Create<PrivacyPolicyGetQuery>()));
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(faultException));

            _faultExceptionBuilderMock.AssertWasCalled(m => m.Build(
                    Arg<Exception>.Is.Equal(exception),
                    Arg<string>.Is.Equal(SoapNamespaces.FoodWasteHouseholdDataServiceName),
                    Arg<MethodBase>.Matches(src => string.Compare(src.Name, "PrivacyPolicyGet", StringComparison.Ordinal) == 0)),
                opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that DataProviderWhoHandlesPaymentsCollectionGet throws an ArgumentNullException when the query for getting a collection of data providers who handles payments is null.
        /// </summary>
        [Test]
        public void TestThatDataProviderWhoHandlesPaymentsCollectionGetThrowsArgumentNullExceptionIfDataProviderWhoHandlesPaymentsCollectionGetQueryIsNull()
        {
            IFoodWasteHouseholdDataService sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.DataProviderWhoHandlesPaymentsCollectionGet(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "query");
        }

        /// <summary>
        /// Tests that DataProviderWhoHandlesPaymentsCollectionGet calls Query on the query bus.
        /// </summary>
        [Test]
        public void TestThatDataProviderWhoHandlesPaymentsCollectionGetCallsQueryOnQueryBus()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            DataProviderWhoHandlesPaymentsCollectionGetQuery query = fixture.Create<DataProviderWhoHandlesPaymentsCollectionGetQuery>();

            IFoodWasteHouseholdDataService sut = CreateSut<ICommand, DataProviderWhoHandlesPaymentsCollectionGetQuery, IEnumerable<DataProviderView>>(fixture.CreateMany<DataProviderView>(random.Next(1, 25)).ToList());
            Assert.That(sut, Is.Not.Null);

            sut.DataProviderWhoHandlesPaymentsCollectionGet(query);

            _queryBusMock.AssertWasCalled(m => m.Query<DataProviderWhoHandlesPaymentsCollectionGetQuery, IEnumerable<DataProviderView>>(Arg<DataProviderWhoHandlesPaymentsCollectionGetQuery>.Is.Equal(query)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that DataProviderWhoHandlesPaymentsCollectionGet returns the result from the query bus.
        /// </summary>
        [Test]
        public void TestThatDataProviderWhoHandlesPaymentsCollectionGetReturnsResultFromQueryBus()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            IEnumerable<DataProviderView> dataProviderViewCollection = fixture.CreateMany<DataProviderView>(random.Next(1, 25)).ToList();

            IFoodWasteHouseholdDataService sut = CreateSut<ICommand, DataProviderWhoHandlesPaymentsCollectionGetQuery, IEnumerable<DataProviderView>>(dataProviderViewCollection);
            Assert.That(sut, Is.Not.Null);

            IEnumerable<DataProviderView> result = sut.DataProviderWhoHandlesPaymentsCollectionGet(fixture.Create<DataProviderWhoHandlesPaymentsCollectionGetQuery>());
            // ReSharper disable PossibleMultipleEnumeration
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EqualTo(dataProviderViewCollection));
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        /// Tests that DataProviderWhoHandlesPaymentsCollectionGet throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatDataProviderWhoHandlesPaymentsCollectionGetThrowsFaultExceptionWhenExceptionOccurs()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            Exception exception = fixture.Create<Exception>();
            FaultException<FoodWasteFault> faultException = fixture.Create<FaultException<FoodWasteFault>>();

            IFoodWasteHouseholdDataService sut = CreateSut<ICommand, DataProviderWhoHandlesPaymentsCollectionGetQuery, IEnumerable<DataProviderView>>(fixture.CreateMany<DataProviderView>(random.Next(1, 25)).ToList(), exception, faultException);
            Assert.That(sut, Is.Not.Null);

            FaultException<FoodWasteFault> result = Assert.Throws<FaultException<FoodWasteFault>>(() => sut.DataProviderWhoHandlesPaymentsCollectionGet(fixture.Create<DataProviderWhoHandlesPaymentsCollectionGetQuery>()));
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(faultException));

            _faultExceptionBuilderMock.AssertWasCalled(m => m.Build(
                    Arg<Exception>.Is.Equal(exception),
                    Arg<string>.Is.Equal(SoapNamespaces.FoodWasteHouseholdDataServiceName),
                    Arg<MethodBase>.Matches(src => string.Compare(src.Name, "DataProviderWhoHandlesPaymentsCollectionGet", StringComparison.Ordinal) == 0)),
                opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that TranslationInfoGetAll throws an ArgumentNullException when the query for getting all the translation informations which can be used for translations is null.
        /// </summary>
        [Test]
        public void TestThatTranslationInfoGetAllThrowsArgumentNullExceptionIfTranslationInfoCollectionGetQueryIsNull()
        {
            IFoodWasteHouseholdDataService sut = CreateSut();
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

            IFoodWasteHouseholdDataService sut = CreateSut<ICommand, TranslationInfoCollectionGetQuery, IEnumerable<TranslationInfoSystemView>>(fixture.CreateMany<TranslationInfoSystemView>(random.Next(1, 25)).ToList());
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

            IFoodWasteHouseholdDataService sut = CreateSut<ICommand, TranslationInfoCollectionGetQuery, IEnumerable<TranslationInfoSystemView>>(translationInfoSystemViewCollection);
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

            IFoodWasteHouseholdDataService sut = CreateSut<ICommand, TranslationInfoCollectionGetQuery, IEnumerable<TranslationInfoSystemView>>(fixture.CreateMany<TranslationInfoSystemView>(random.Next(1, 25)).ToList(), exception, faultException);
            Assert.That(sut, Is.Not.Null);

            FaultException<FoodWasteFault> result = Assert.Throws<FaultException<FoodWasteFault>>(() => sut.TranslationInfoGetAll(fixture.Create<TranslationInfoCollectionGetQuery>()));
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(faultException));

            _faultExceptionBuilderMock.AssertWasCalled(m => m.Build(
                    Arg<Exception>.Is.Equal(exception),
                    Arg<string>.Is.Equal(SoapNamespaces.FoodWasteHouseholdDataServiceName),
                    Arg<MethodBase>.Matches(src => string.Compare(src.Name, "TranslationInfoGetAll", StringComparison.Ordinal) == 0)),
                opt => opt.Repeat.Once());

        }

        /// <summary>
        /// Creates an instance of the service which can access and modify data on a house hold in the food waste domain which can be used for unit testing.
        /// </summary>
        /// <returns>Instance of the service which can access and modify data on a house hold in the food waste domain which can be used for unit testing.</returns>
        private IFoodWasteHouseholdDataService CreateSut()
        {
            _commandBusMock = MockRepository.GenerateMock<ICommandBus>();
            _queryBusMock = MockRepository.GenerateMock<IQueryBus>();
            _faultExceptionBuilderMock = MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>();

            return new FoodWasteHouseholdDataService(_commandBusMock, _queryBusMock, _faultExceptionBuilderMock);
        }

        /// <summary>
        /// Creates an instance of the service which can access and modify data on a house hold in the food waste domain which can be used for unit testing.
        /// </summary>
        /// <typeparam name="TCommand">Type of the command which should be tested.</typeparam>
        /// <typeparam name="TQuery">Type of the query which should be tested.</typeparam>
        /// <typeparam name="TResult">Type of the result which should be tested.</typeparam>
        /// <param name="result">The result of the command or query.</param>
        /// <param name="exception">The exception which should be thrown when command or query are executed.</param>
        /// <param name="faultException">The fault exception which sould be returned for the fault exception builder.</param>
        /// <returns>Instance of the service which can access and modify data on a house hold in the food waste domain which can be used for unit testing.</returns>
        private IFoodWasteHouseholdDataService CreateSut<TCommand, TQuery, TResult>(TResult result, Exception exception = null, FaultException<FoodWasteFault> faultException = null) where TCommand : class, ICommand where TQuery : class, IQuery
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

            return new FoodWasteHouseholdDataService(_commandBusMock, _queryBusMock, _faultExceptionBuilderMock);
        }
    }
}
