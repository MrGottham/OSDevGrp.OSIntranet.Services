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
using Ploeh.AutoFixture;
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
                .CreateMany(15);
            FoodItemCollectionSystemView foodItemCollectionSystemView = fixture.Build<FoodItemCollectionSystemView>()
                .With(m => m.FoodItems, foodItemSystemViewCollection)
                .Create();

            IFoodWasteSystemDataService sut = CreateSut<FoodItemCollectionGetQuery, FoodItemCollectionSystemView>(foodItemCollectionSystemView);
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
                .CreateMany(15);
            FoodItemCollectionSystemView foodItemCollectionSystemView = fixture.Build<FoodItemCollectionSystemView>()
                .With(m => m.FoodItems, foodItemSystemViewCollection)
                .Create();

            IFoodWasteSystemDataService sut = CreateSut<FoodItemCollectionGetQuery, FoodItemCollectionSystemView>(foodItemCollectionSystemView);
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
                .CreateMany(15);
            FoodItemCollectionSystemView foodItemCollectionSystemView = fixture.Build<FoodItemCollectionSystemView>()
                .With(m => m.FoodItems, foodItemSystemViewCollection)
                .Create();

            Exception exception = fixture.Create<Exception>();
            FaultException<FoodWasteFault> faultException = fixture.Create<FaultException<FoodWasteFault>>();

            IFoodWasteSystemDataService sut = CreateSut<FoodItemCollectionGetQuery, FoodItemCollectionSystemView>(foodItemCollectionSystemView, exception, faultException);
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
            var fixture = new Fixture();
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var foodWasteSystemDataService = new FoodWasteSystemDataService(fixture.Create<ICommandBus>(), fixture.Create<IQueryBus>(), fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteSystemDataService, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodWasteSystemDataService.FoodItemImportFromDataProvider(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("command"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that FoodItemImportFromDataProvider calls Publish on the command bus.
        /// </summary>
        [Test]
        public void TestThatFoodItemImportFromDataProviderCallsPublishOnCommandBus()
        {
            var fixture = new Fixture();
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var commandBusMock = MockRepository.GenerateMock<ICommandBus>();
            commandBusMock.Stub(m => m.Publish<FoodItemImportFromDataProviderCommand, ServiceReceiptResponse>(Arg<FoodItemImportFromDataProviderCommand>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var foodWasteSystemDataService = new FoodWasteSystemDataService(commandBusMock, fixture.Create<IQueryBus>(), fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteSystemDataService, Is.Not.Null);

            var command = fixture.Create<FoodItemImportFromDataProviderCommand>();

            foodWasteSystemDataService.FoodItemImportFromDataProvider(command);

            commandBusMock.AssertWasCalled(m => m.Publish<FoodItemImportFromDataProviderCommand, ServiceReceiptResponse>(Arg<FoodItemImportFromDataProviderCommand>.Is.Equal(command)));
        }

        /// <summary>
        /// Tests that FoodItemImportFromDataProvider returns the result from the command bus.
        /// </summary>
        [Test]
        public void TestThatFoodItemImportFromDataProviderReturnsResultFromCommandBus()
        {
            var fixture = new Fixture();
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var serviceReceipt = fixture.Create<ServiceReceiptResponse>();
            var commandBusMock = MockRepository.GenerateMock<ICommandBus>();
            commandBusMock.Stub(m => m.Publish<FoodItemImportFromDataProviderCommand, ServiceReceiptResponse>(Arg<FoodItemImportFromDataProviderCommand>.Is.Anything))
                .Return(serviceReceipt)
                .Repeat.Any();

            var foodWasteSystemDataService = new FoodWasteSystemDataService(commandBusMock, fixture.Create<IQueryBus>(), fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteSystemDataService, Is.Not.Null);

            var command = fixture.Create<FoodItemImportFromDataProviderCommand>();

            var result = foodWasteSystemDataService.FoodItemImportFromDataProvider(command);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(serviceReceipt));
        }

        /// <summary>
        /// Tests that FoodItemImportFromDataProvider throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatFoodItemImportFromDataProviderThrowsFaultExceptionWhenExceptionOccurs()
        {
            var fixture = new Fixture();
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));

            var exception = fixture.Create<Exception>();
            var commandBusMock = MockRepository.GenerateMock<ICommandBus>();
            commandBusMock.Stub(m => m.Publish<FoodItemImportFromDataProviderCommand, ServiceReceiptResponse>(Arg<FoodItemImportFromDataProviderCommand>.Is.Anything))
                .Throw(exception)
                .Repeat.Any();

            var foodWasteFaultExceptionBuilderMock = MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>();
            foodWasteFaultExceptionBuilderMock.Stub(m => m.Build(Arg<Exception>.Is.NotNull, Arg<string>.Is.NotNull, Arg<MethodBase>.Is.NotNull))
                .Return(fixture.Create<FaultException<FoodWasteFault>>())
                .Repeat.Any();

            var foodWasteSystemDataService = new FoodWasteSystemDataService(commandBusMock, fixture.Create<IQueryBus>(), foodWasteFaultExceptionBuilderMock);
            Assert.That(foodWasteSystemDataService, Is.Not.Null);

            var command = fixture.Create<FoodItemImportFromDataProviderCommand>();

            var faultException = Assert.Throws<FaultException<FoodWasteFault>>(() => foodWasteSystemDataService.FoodItemImportFromDataProvider(command));
            Assert.That(faultException, Is.Not.Null);
            Assert.That(faultException.Detail, Is.Not.Null);

            foodWasteFaultExceptionBuilderMock.AssertWasCalled(m => m.Build(Arg<Exception>.Is.Equal(exception), Arg<string>.Is.Equal(SoapNamespaces.FoodWasteSystemDataServiceName), Arg<MethodBase>.Is.NotNull));
        }

        /// <summary>
        /// Tests that FoodGroupTreeGet throws an ArgumentNullException when the query for getting all the tree of food groups is null.
        /// </summary>
        [Test]
        public void TestThatFoodGroupTreeGetThrowsArgumentNullExceptionIfFoodGroupTreeGetQueryIsNull()
        {
            var fixture = new Fixture();
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var foodWasteSystemDataService = new FoodWasteSystemDataService(fixture.Create<ICommandBus>(), fixture.Create<IQueryBus>(), fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteSystemDataService, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodWasteSystemDataService.FoodGroupTreeGet(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("query"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that FoodGroupTreeGet calls Query on the query bus.
        /// </summary>
        [Test]
        public void TestThatFoodGroupTreeGetCallsQueryOnQueryBus()
        {
            var fixture = new Fixture();
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));
            fixture.Inject(fixture.Build<FoodGroupSystemView>().With(m => m.Parent, fixture.Create<FoodGroupIdentificationView>()).With(m => m.Children, new List<FoodGroupSystemView>(0)).Create());

            var queryBusMock = MockRepository.GenerateMock<IQueryBus>();
            queryBusMock.Stub(m => m.Query<FoodGroupTreeGetQuery, FoodGroupTreeSystemView>(Arg<FoodGroupTreeGetQuery>.Is.NotNull))
                .Return(fixture.Create<FoodGroupTreeSystemView>())
                .Repeat.Any();

            var foodWasteSystemDataService = new FoodWasteSystemDataService(fixture.Create<ICommandBus>(), queryBusMock, fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteSystemDataService, Is.Not.Null);

            var query = fixture.Create<FoodGroupTreeGetQuery>();
            foodWasteSystemDataService.FoodGroupTreeGet(query);

            queryBusMock.AssertWasCalled(m => m.Query<FoodGroupTreeGetQuery, FoodGroupTreeSystemView>(Arg<FoodGroupTreeGetQuery>.Is.Equal(query)));
        }

        /// <summary>
        /// Tests that FoodGroupTreeGet returns the result from the query bus.
        /// </summary>
        [Test]
        public void TestThatFoodGroupTreeGetReturnsResultFromQueryBus()
        {
            var fixture = new Fixture();
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));
            fixture.Inject(fixture.Build<FoodGroupSystemView>().With(m => m.Parent, fixture.Create<FoodGroupIdentificationView>()).With(m => m.Children, new List<FoodGroupSystemView>(0)).Create());

            var foodGroupTreeSystemView = fixture.Create<FoodGroupTreeSystemView>();
            var queryBusMock = MockRepository.GenerateMock<IQueryBus>();
            queryBusMock.Stub(m => m.Query<FoodGroupTreeGetQuery, FoodGroupTreeSystemView>(Arg<FoodGroupTreeGetQuery>.Is.NotNull))
                .Return(foodGroupTreeSystemView)
                .Repeat.Any();

            var foodWasteSystemDataService = new FoodWasteSystemDataService(fixture.Create<ICommandBus>(), queryBusMock, fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteSystemDataService, Is.Not.Null);

            var result = foodWasteSystemDataService.FoodGroupTreeGet(fixture.Create<FoodGroupTreeGetQuery>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(foodGroupTreeSystemView));
        }

        /// <summary>
        /// Tests that FoodGroupTreeGet throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatFoodGroupTreeGetThrowsFaultExceptionWhenExceptionOccurs()
        {
            var fixture = new Fixture();
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));

            var exception = fixture.Create<Exception>();
            var queryBusMock = MockRepository.GenerateMock<IQueryBus>();
            queryBusMock.Stub(m => m.Query<FoodGroupTreeGetQuery, FoodGroupTreeSystemView>(Arg<FoodGroupTreeGetQuery>.Is.NotNull))
                .Throw(exception)
                .Repeat.Any();

            var foodWasteFaultExceptionBuilderMock = MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>();
            foodWasteFaultExceptionBuilderMock.Stub(m => m.Build(Arg<Exception>.Is.NotNull, Arg<string>.Is.NotNull, Arg<MethodBase>.Is.NotNull))
                .Return(fixture.Create<FaultException<FoodWasteFault>>())
                .Repeat.Any();

            var foodWasteSystemDataService = new FoodWasteSystemDataService(fixture.Create<ICommandBus>(), queryBusMock, foodWasteFaultExceptionBuilderMock);
            Assert.That(foodWasteSystemDataService, Is.Not.Null);

            var faultException = Assert.Throws<FaultException<FoodWasteFault>>(() => foodWasteSystemDataService.FoodGroupTreeGet(fixture.Create<FoodGroupTreeGetQuery>()));
            Assert.That(faultException, Is.Not.Null);
            Assert.That(faultException.Detail, Is.Not.Null);

            foodWasteFaultExceptionBuilderMock.AssertWasCalled(m => m.Build(Arg<Exception>.Is.Equal(exception), Arg<string>.Is.Equal(SoapNamespaces.FoodWasteSystemDataServiceName), Arg<MethodBase>.Is.NotNull));
        }

        /// <summary>
        /// Tests that FoodGroupImportFromDataProvider throws an ArgumentNullException when the command for importing a food group from a given data provider is null.
        /// </summary>
        [Test]
        public void TestThatFoodGroupImportFromDataProviderThrowsArgumentNullExceptionIfFoodGroupImportFromDataProviderCommandIsNull()
        {
            var fixture = new Fixture();
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var foodWasteSystemDataService = new FoodWasteSystemDataService(fixture.Create<ICommandBus>(), fixture.Create<IQueryBus>(), fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteSystemDataService, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodWasteSystemDataService.FoodGroupImportFromDataProvider(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("command"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that FoodGroupImportFromDataProvider calls Publish on the command bus.
        /// </summary>
        [Test]
        public void TestThatFoodGroupImportFromDataProviderCallsPublishOnCommandBus()
        {
            var fixture = new Fixture();
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var commandBusMock = MockRepository.GenerateMock<ICommandBus>();
            commandBusMock.Stub(m => m.Publish<FoodGroupImportFromDataProviderCommand, ServiceReceiptResponse>(Arg<FoodGroupImportFromDataProviderCommand>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var foodWasteSystemDataService = new FoodWasteSystemDataService(commandBusMock, fixture.Create<IQueryBus>(), fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteSystemDataService, Is.Not.Null);

            var command = fixture.Create<FoodGroupImportFromDataProviderCommand>();

            foodWasteSystemDataService.FoodGroupImportFromDataProvider(command);

            commandBusMock.AssertWasCalled(m => m.Publish<FoodGroupImportFromDataProviderCommand, ServiceReceiptResponse>(Arg<FoodGroupImportFromDataProviderCommand>.Is.Equal(command)));
        }

        /// <summary>
        /// Tests that FoodGroupImportFromDataProvider returns the result from the command bus.
        /// </summary>
        [Test]
        public void TestThatFoodGroupImportFromDataProviderReturnsResultFromCommandBus()
        {
            var fixture = new Fixture();
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var serviceReceipt = fixture.Create<ServiceReceiptResponse>();
            var commandBusMock = MockRepository.GenerateMock<ICommandBus>();
            commandBusMock.Stub(m => m.Publish<FoodGroupImportFromDataProviderCommand, ServiceReceiptResponse>(Arg<FoodGroupImportFromDataProviderCommand>.Is.Anything))
                .Return(serviceReceipt)
                .Repeat.Any();

            var foodWasteSystemDataService = new FoodWasteSystemDataService(commandBusMock, fixture.Create<IQueryBus>(), fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteSystemDataService, Is.Not.Null);

            var command = fixture.Create<FoodGroupImportFromDataProviderCommand>();

            var result = foodWasteSystemDataService.FoodGroupImportFromDataProvider(command);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(serviceReceipt));
        }

        /// <summary>
        /// Tests that FoodGroupImportFromDataProvider throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatFoodGroupImportFromDataProviderThrowsFaultExceptionWhenExceptionOccurs()
        {
            var fixture = new Fixture();
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));

            var exception = fixture.Create<Exception>();
            var commandBusMock = MockRepository.GenerateMock<ICommandBus>();
            commandBusMock.Stub(m => m.Publish<FoodGroupImportFromDataProviderCommand, ServiceReceiptResponse>(Arg<FoodGroupImportFromDataProviderCommand>.Is.Anything))
                .Throw(exception)
                .Repeat.Any();

            var foodWasteFaultExceptionBuilderMock = MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>();
            foodWasteFaultExceptionBuilderMock.Stub(m => m.Build(Arg<Exception>.Is.NotNull, Arg<string>.Is.NotNull, Arg<MethodBase>.Is.NotNull))
                .Return(fixture.Create<FaultException<FoodWasteFault>>())
                .Repeat.Any();

            var foodWasteSystemDataService = new FoodWasteSystemDataService(commandBusMock, fixture.Create<IQueryBus>(), foodWasteFaultExceptionBuilderMock);
            Assert.That(foodWasteSystemDataService, Is.Not.Null);

            var command = fixture.Create<FoodGroupImportFromDataProviderCommand>();

            var faultException = Assert.Throws<FaultException<FoodWasteFault>>(() => foodWasteSystemDataService.FoodGroupImportFromDataProvider(command));
            Assert.That(faultException, Is.Not.Null);
            Assert.That(faultException.Detail, Is.Not.Null);

            foodWasteFaultExceptionBuilderMock.AssertWasCalled(m => m.Build(Arg<Exception>.Is.Equal(exception), Arg<string>.Is.Equal(SoapNamespaces.FoodWasteSystemDataServiceName), Arg<MethodBase>.Is.NotNull));
        }

        /// <summary>
        /// Tests that ForeignKeyAdd throws an ArgumentNullException when the command for adding a foreign key is null.
        /// </summary>
        [Test]
        public void TestThatForeignKeyAddThrowsArgumentNullExceptionIfForeignKeyAddCommandIsNull()
        {
            var fixture = new Fixture();
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var foodWasteSystemDataService = new FoodWasteSystemDataService(fixture.Create<ICommandBus>(), fixture.Create<IQueryBus>(), fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteSystemDataService, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodWasteSystemDataService.ForeignKeyAdd(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("command"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that ForeignKeyAdd calls Publish on the command bus.
        /// </summary>
        [Test]
        public void TestThatForeignKeyAddCallsPublishOnCommandBus()
        {
            var fixture = new Fixture();
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var commandBusMock = MockRepository.GenerateMock<ICommandBus>();
            commandBusMock.Stub(m => m.Publish<ForeignKeyAddCommand, ServiceReceiptResponse>(Arg<ForeignKeyAddCommand>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var foodWasteSystemDataService = new FoodWasteSystemDataService(commandBusMock, fixture.Create<IQueryBus>(), fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteSystemDataService, Is.Not.Null);

            var command = fixture.Create<ForeignKeyAddCommand>();

            foodWasteSystemDataService.ForeignKeyAdd(command);

            commandBusMock.AssertWasCalled(m => m.Publish<ForeignKeyAddCommand, ServiceReceiptResponse>(Arg<ForeignKeyAddCommand>.Is.Equal(command)));
        }

        /// <summary>
        /// Tests that ForeignKeyAdd returns the result from the command bus.
        /// </summary>
        [Test]
        public void TestThatForeignKeyAddReturnsResultFromCommandBus()
        {
            var fixture = new Fixture();
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var serviceReceipt = fixture.Create<ServiceReceiptResponse>();
            var commandBusMock = MockRepository.GenerateMock<ICommandBus>();
            commandBusMock.Stub(m => m.Publish<ForeignKeyAddCommand, ServiceReceiptResponse>(Arg<ForeignKeyAddCommand>.Is.Anything))
                .Return(serviceReceipt)
                .Repeat.Any();

            var foodWasteSystemDataService = new FoodWasteSystemDataService(commandBusMock, fixture.Create<IQueryBus>(), fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteSystemDataService, Is.Not.Null);

            var command = fixture.Create<ForeignKeyAddCommand>();

            var result = foodWasteSystemDataService.ForeignKeyAdd(command);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(serviceReceipt));
        }

        /// <summary>
        /// Tests that ForeignKeyAdd throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatForeignKeyAddThrowsFaultExceptionWhenExceptionOccurs()
        {
            var fixture = new Fixture();
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));

            var exception = fixture.Create<Exception>();
            var commandBusMock = MockRepository.GenerateMock<ICommandBus>();
            commandBusMock.Stub(m => m.Publish<ForeignKeyAddCommand, ServiceReceiptResponse>(Arg<ForeignKeyAddCommand>.Is.Anything))
                .Throw(exception)
                .Repeat.Any();

            var foodWasteFaultExceptionBuilderMock = MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>();
            foodWasteFaultExceptionBuilderMock.Stub(m => m.Build(Arg<Exception>.Is.NotNull, Arg<string>.Is.NotNull, Arg<MethodBase>.Is.NotNull))
                .Return(fixture.Create<FaultException<FoodWasteFault>>())
                .Repeat.Any();

            var foodWasteSystemDataService = new FoodWasteSystemDataService(commandBusMock, fixture.Create<IQueryBus>(), foodWasteFaultExceptionBuilderMock);
            Assert.That(foodWasteSystemDataService, Is.Not.Null);

            var command = fixture.Create<ForeignKeyAddCommand>();

            var faultException = Assert.Throws<FaultException<FoodWasteFault>>(() => foodWasteSystemDataService.ForeignKeyAdd(command));
            Assert.That(faultException, Is.Not.Null);
            Assert.That(faultException.Detail, Is.Not.Null);

            foodWasteFaultExceptionBuilderMock.AssertWasCalled(m => m.Build(Arg<Exception>.Is.Equal(exception), Arg<string>.Is.Equal(SoapNamespaces.FoodWasteSystemDataServiceName), Arg<MethodBase>.Is.NotNull));
        }

        /// <summary>
        /// Tests that ForeignKeyModify throws an ArgumentNullException when the command for modifying a foreign key is null.
        /// </summary>
        [Test]
        public void TestThatForeignKeyModifyThrowsArgumentNullExceptionIfForeignKeyModifyCommandIsNull()
        {
            var fixture = new Fixture();
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var foodWasteSystemDataService = new FoodWasteSystemDataService(fixture.Create<ICommandBus>(), fixture.Create<IQueryBus>(), fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteSystemDataService, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodWasteSystemDataService.ForeignKeyModify(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("command"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that ForeignKeyModify calls Publish on the command bus.
        /// </summary>
        [Test]
        public void TestThatForeignKeyModifyCallsPublishOnCommandBus()
        {
            var fixture = new Fixture();
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var commandBusMock = MockRepository.GenerateMock<ICommandBus>();
            commandBusMock.Stub(m => m.Publish<ForeignKeyModifyCommand, ServiceReceiptResponse>(Arg<ForeignKeyModifyCommand>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var foodWasteSystemDataService = new FoodWasteSystemDataService(commandBusMock, fixture.Create<IQueryBus>(), fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteSystemDataService, Is.Not.Null);

            var command = fixture.Create<ForeignKeyModifyCommand>();

            foodWasteSystemDataService.ForeignKeyModify(command);

            commandBusMock.AssertWasCalled(m => m.Publish<ForeignKeyModifyCommand, ServiceReceiptResponse>(Arg<ForeignKeyModifyCommand>.Is.Equal(command)));
        }

        /// <summary>
        /// Tests that ForeignKeyModify returns the result from the command bus.
        /// </summary>
        [Test]
        public void TestThatForeignKeyModifyReturnsResultFromCommandBus()
        {
            var fixture = new Fixture();
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var serviceReceipt = fixture.Create<ServiceReceiptResponse>();
            var commandBusMock = MockRepository.GenerateMock<ICommandBus>();
            commandBusMock.Stub(m => m.Publish<ForeignKeyModifyCommand, ServiceReceiptResponse>(Arg<ForeignKeyModifyCommand>.Is.Anything))
                .Return(serviceReceipt)
                .Repeat.Any();

            var foodWasteSystemDataService = new FoodWasteSystemDataService(commandBusMock, fixture.Create<IQueryBus>(), fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteSystemDataService, Is.Not.Null);

            var command = fixture.Create<ForeignKeyModifyCommand>();

            var result = foodWasteSystemDataService.ForeignKeyModify(command);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(serviceReceipt));
        }

        /// <summary>
        /// Tests that ForeignKeyModify throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatForeignKeyModifyThrowsFaultExceptionWhenExceptionOccurs()
        {
            var fixture = new Fixture();
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));

            var exception = fixture.Create<Exception>();
            var commandBusMock = MockRepository.GenerateMock<ICommandBus>();
            commandBusMock.Stub(m => m.Publish<ForeignKeyModifyCommand, ServiceReceiptResponse>(Arg<ForeignKeyModifyCommand>.Is.Anything))
                .Throw(exception)
                .Repeat.Any();

            var foodWasteFaultExceptionBuilderMock = MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>();
            foodWasteFaultExceptionBuilderMock.Stub(m => m.Build(Arg<Exception>.Is.NotNull, Arg<string>.Is.NotNull, Arg<MethodBase>.Is.NotNull))
                .Return(fixture.Create<FaultException<FoodWasteFault>>())
                .Repeat.Any();

            var foodWasteSystemDataService = new FoodWasteSystemDataService(commandBusMock, fixture.Create<IQueryBus>(), foodWasteFaultExceptionBuilderMock);
            Assert.That(foodWasteSystemDataService, Is.Not.Null);

            var command = fixture.Create<ForeignKeyModifyCommand>();

            var faultException = Assert.Throws<FaultException<FoodWasteFault>>(() => foodWasteSystemDataService.ForeignKeyModify(command));
            Assert.That(faultException, Is.Not.Null);
            Assert.That(faultException.Detail, Is.Not.Null);

            foodWasteFaultExceptionBuilderMock.AssertWasCalled(m => m.Build(Arg<Exception>.Is.Equal(exception), Arg<string>.Is.Equal(SoapNamespaces.FoodWasteSystemDataServiceName), Arg<MethodBase>.Is.NotNull));
        }

        /// <summary>
        /// Tests that ForeignKeyDelete throws an ArgumentNullException when the command for deleting a foreign key is null.
        /// </summary>
        [Test]
        public void TestThatForeignKeyDeleteThrowsArgumentNullExceptionIfForeignKeyDeleteCommandIsNull()
        {
            var fixture = new Fixture();
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var foodWasteSystemDataService = new FoodWasteSystemDataService(fixture.Create<ICommandBus>(), fixture.Create<IQueryBus>(), fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteSystemDataService, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodWasteSystemDataService.ForeignKeyDelete(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("command"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that ForeignKeyDelete calls Publish on the command bus.
        /// </summary>
        [Test]
        public void TestThatForeignKeyDeleteCallsPublishOnCommandBus()
        {
            var fixture = new Fixture();
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var commandBusMock = MockRepository.GenerateMock<ICommandBus>();
            commandBusMock.Stub(m => m.Publish<ForeignKeyDeleteCommand, ServiceReceiptResponse>(Arg<ForeignKeyDeleteCommand>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var foodWasteSystemDataService = new FoodWasteSystemDataService(commandBusMock, fixture.Create<IQueryBus>(), fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteSystemDataService, Is.Not.Null);

            var command = fixture.Create<ForeignKeyDeleteCommand>();

            foodWasteSystemDataService.ForeignKeyDelete(command);

            commandBusMock.AssertWasCalled(m => m.Publish<ForeignKeyDeleteCommand, ServiceReceiptResponse>(Arg<ForeignKeyDeleteCommand>.Is.Equal(command)));
        }

        /// <summary>
        /// Tests that ForeignKeyDelete returns the result from the command bus.
        /// </summary>
        [Test]
        public void TestThatForeignKeyDeleteeturnsResultFromCommandBus()
        {
            var fixture = new Fixture();
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var serviceReceipt = fixture.Create<ServiceReceiptResponse>();
            var commandBusMock = MockRepository.GenerateMock<ICommandBus>();
            commandBusMock.Stub(m => m.Publish<ForeignKeyDeleteCommand, ServiceReceiptResponse>(Arg<ForeignKeyDeleteCommand>.Is.Anything))
                .Return(serviceReceipt)
                .Repeat.Any();

            var foodWasteSystemDataService = new FoodWasteSystemDataService(commandBusMock, fixture.Create<IQueryBus>(), fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteSystemDataService, Is.Not.Null);

            var command = fixture.Create<ForeignKeyDeleteCommand>();

            var result = foodWasteSystemDataService.ForeignKeyDelete(command);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(serviceReceipt));
        }

        /// <summary>
        /// Tests that ForeignKeyDelete throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatForeignKeyDeleteThrowsFaultExceptionWhenExceptionOccurs()
        {
            var fixture = new Fixture();
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));

            var exception = fixture.Create<Exception>();
            var commandBusMock = MockRepository.GenerateMock<ICommandBus>();
            commandBusMock.Stub(m => m.Publish<ForeignKeyDeleteCommand, ServiceReceiptResponse>(Arg<ForeignKeyDeleteCommand>.Is.Anything))
                .Throw(exception)
                .Repeat.Any();

            var foodWasteFaultExceptionBuilderMock = MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>();
            foodWasteFaultExceptionBuilderMock.Stub(m => m.Build(Arg<Exception>.Is.NotNull, Arg<string>.Is.NotNull, Arg<MethodBase>.Is.NotNull))
                .Return(fixture.Create<FaultException<FoodWasteFault>>())
                .Repeat.Any();

            var foodWasteSystemDataService = new FoodWasteSystemDataService(commandBusMock, fixture.Create<IQueryBus>(), foodWasteFaultExceptionBuilderMock);
            Assert.That(foodWasteSystemDataService, Is.Not.Null);

            var command = fixture.Create<ForeignKeyDeleteCommand>();

            var faultException = Assert.Throws<FaultException<FoodWasteFault>>(() => foodWasteSystemDataService.ForeignKeyDelete(command));
            Assert.That(faultException, Is.Not.Null);
            Assert.That(faultException.Detail, Is.Not.Null);

            foodWasteFaultExceptionBuilderMock.AssertWasCalled(m => m.Build(Arg<Exception>.Is.Equal(exception), Arg<string>.Is.Equal(SoapNamespaces.FoodWasteSystemDataServiceName), Arg<MethodBase>.Is.NotNull));
        }

        /// <summary>
        /// Tests that DataProviderGetAll throws an ArgumentNullException when the query for getting all the data providers is null.
        /// </summary>
        [Test]
        public void TestThatDataProviderGetAllThrowsArgumentNullExceptionIfDataProviderCollectionGetQueryIsNull()
        {
            var fixture = new Fixture();
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var foodWasteSystemDataService = new FoodWasteSystemDataService(fixture.Create<ICommandBus>(), fixture.Create<IQueryBus>(), fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteSystemDataService, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodWasteSystemDataService.DataProviderGetAll(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("query"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that DataProviderGetAll calls Query on the query bus.
        /// </summary>
        [Test]
        public void TestThatDataProviderGetAllCallsQueryOnQueryBus()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var queryBusMock = MockRepository.GenerateMock<IQueryBus>();
            queryBusMock.Stub(m => m.Query<DataProviderCollectionGetQuery, IEnumerable<DataProviderSystemView>>(Arg<DataProviderCollectionGetQuery>.Is.NotNull))
                .Return(fixture.CreateMany<DataProviderSystemView>(random.Next(1, 25)).ToList())
                .Repeat.Any();

            var foodWasteSystemDataService = new FoodWasteSystemDataService(fixture.Create<ICommandBus>(), queryBusMock, fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteSystemDataService, Is.Not.Null);

            var query = fixture.Create<DataProviderCollectionGetQuery>();
            foodWasteSystemDataService.DataProviderGetAll(query);

            queryBusMock.AssertWasCalled(m => m.Query<DataProviderCollectionGetQuery, IEnumerable<DataProviderSystemView>>(Arg<DataProviderCollectionGetQuery>.Is.Equal(query)));
        }

        /// <summary>
        /// Tests that DataProviderGetAll returns the result from the query bus.
        /// </summary>
        [Test]
        public void TestThatDataProviderGetAllReturnsResultFromQueryBus()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var dataProviderSystemViewCollection = fixture.CreateMany<DataProviderSystemView>(random.Next(1, 25)).ToList();
            var queryBusMock = MockRepository.GenerateMock<IQueryBus>();
            queryBusMock.Stub(m => m.Query<DataProviderCollectionGetQuery, IEnumerable<DataProviderSystemView>>(Arg<DataProviderCollectionGetQuery>.Is.NotNull))
                .Return(dataProviderSystemViewCollection)
                .Repeat.Any();

            var foodWasteSystemDataService = new FoodWasteSystemDataService(fixture.Create<ICommandBus>(), queryBusMock, fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteSystemDataService, Is.Not.Null);

            var result = foodWasteSystemDataService.DataProviderGetAll(fixture.Create<DataProviderCollectionGetQuery>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EqualTo(dataProviderSystemViewCollection));
        }

        /// <summary>
        /// Tests that DataProviderGetAll throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatDataProviderGetAllThrowsFaultExceptionWhenExceptionOccurs()
        {
            var fixture = new Fixture();
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));

            var exception = fixture.Create<Exception>();
            var queryBusMock = MockRepository.GenerateMock<IQueryBus>();
            queryBusMock.Stub(m => m.Query<DataProviderCollectionGetQuery, IEnumerable<DataProviderSystemView>>(Arg<DataProviderCollectionGetQuery>.Is.NotNull))
                .Throw(exception)
                .Repeat.Any();

            var foodWasteFaultExceptionBuilderMock = MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>();
            foodWasteFaultExceptionBuilderMock.Stub(m => m.Build(Arg<Exception>.Is.NotNull, Arg<string>.Is.NotNull, Arg<MethodBase>.Is.NotNull))
                .Return(fixture.Create<FaultException<FoodWasteFault>>())
                .Repeat.Any();

            var foodWasteSystemDataService = new FoodWasteSystemDataService(fixture.Create<ICommandBus>(), queryBusMock, foodWasteFaultExceptionBuilderMock);
            Assert.That(foodWasteSystemDataService, Is.Not.Null);

            var faultException = Assert.Throws<FaultException<FoodWasteFault>>(() => foodWasteSystemDataService.DataProviderGetAll(fixture.Create<DataProviderCollectionGetQuery>()));
            Assert.That(faultException, Is.Not.Null);
            Assert.That(faultException.Detail, Is.Not.Null);

            foodWasteFaultExceptionBuilderMock.AssertWasCalled(m => m.Build(Arg<Exception>.Is.Equal(exception), Arg<string>.Is.Equal(SoapNamespaces.FoodWasteSystemDataServiceName), Arg<MethodBase>.Is.NotNull));
        }

        /// <summary>
        /// Tests that TranslationAdd throws an ArgumentNullException when the command for adding a translation is null.
        /// </summary>
        [Test]
        public void TestThatTranslationAddThrowsArgumentNullExceptionIfTranslationAddCommandIsNull()
        {
            var fixture = new Fixture();
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var foodWasteSystemDataService = new FoodWasteSystemDataService(fixture.Create<ICommandBus>(), fixture.Create<IQueryBus>(), fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteSystemDataService, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodWasteSystemDataService.TranslationAdd(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("command"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that TranslationAdd calls Publish on the command bus.
        /// </summary>
        [Test]
        public void TestThatTranslationAddCallsPublishOnCommandBus()
        {
            var fixture = new Fixture();
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var commandBusMock = MockRepository.GenerateMock<ICommandBus>();
            commandBusMock.Stub(m => m.Publish<TranslationAddCommand, ServiceReceiptResponse>(Arg<TranslationAddCommand>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var foodWasteSystemDataService = new FoodWasteSystemDataService(commandBusMock, fixture.Create<IQueryBus>(), fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteSystemDataService, Is.Not.Null);

            var command = fixture.Create<TranslationAddCommand>();

            foodWasteSystemDataService.TranslationAdd(command);

            commandBusMock.AssertWasCalled(m => m.Publish<TranslationAddCommand, ServiceReceiptResponse>(Arg<TranslationAddCommand>.Is.Equal(command)));
        }

        /// <summary>
        /// Tests that TranslationAdd returns the result from the command bus.
        /// </summary>
        [Test]
        public void TestThatTranslationAddReturnsResultFromCommandBus()
        {
            var fixture = new Fixture();
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var serviceReceipt = fixture.Create<ServiceReceiptResponse>();
            var commandBusMock = MockRepository.GenerateMock<ICommandBus>();
            commandBusMock.Stub(m => m.Publish<TranslationAddCommand, ServiceReceiptResponse>(Arg<TranslationAddCommand>.Is.Anything))
                .Return(serviceReceipt)
                .Repeat.Any();

            var foodWasteSystemDataService = new FoodWasteSystemDataService(commandBusMock, fixture.Create<IQueryBus>(), fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteSystemDataService, Is.Not.Null);

            var command = fixture.Create<TranslationAddCommand>();

            var result = foodWasteSystemDataService.TranslationAdd(command);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(serviceReceipt));
        }

        /// <summary>
        /// Tests that TranslationAdd throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatTranslationAddThrowsFaultExceptionWhenExceptionOccurs()
        {
            var fixture = new Fixture();
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));

            var exception = fixture.Create<Exception>();
            var commandBusMock = MockRepository.GenerateMock<ICommandBus>();
            commandBusMock.Stub(m => m.Publish<TranslationAddCommand, ServiceReceiptResponse>(Arg<TranslationAddCommand>.Is.Anything))
                .Throw(exception)
                .Repeat.Any();

            var foodWasteFaultExceptionBuilderMock = MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>();
            foodWasteFaultExceptionBuilderMock.Stub(m => m.Build(Arg<Exception>.Is.NotNull, Arg<string>.Is.NotNull, Arg<MethodBase>.Is.NotNull))
                .Return(fixture.Create<FaultException<FoodWasteFault>>())
                .Repeat.Any();

            var foodWasteSystemDataService = new FoodWasteSystemDataService(commandBusMock, fixture.Create<IQueryBus>(), foodWasteFaultExceptionBuilderMock);
            Assert.That(foodWasteSystemDataService, Is.Not.Null);

            var command = fixture.Create<TranslationAddCommand>();

            var faultException = Assert.Throws<FaultException<FoodWasteFault>>(() => foodWasteSystemDataService.TranslationAdd(command));
            Assert.That(faultException, Is.Not.Null);
            Assert.That(faultException.Detail, Is.Not.Null);

            foodWasteFaultExceptionBuilderMock.AssertWasCalled(m => m.Build(Arg<Exception>.Is.Equal(exception), Arg<string>.Is.Equal(SoapNamespaces.FoodWasteSystemDataServiceName), Arg<MethodBase>.Is.NotNull));
        }

        /// <summary>
        /// Tests that TranslationModify throws an ArgumentNullException when the command for modifying a translation is null.
        /// </summary>
        [Test]
        public void TestThatTranslationModifyThrowsArgumentNullExceptionIfTranslationModifyCommandIsNull()
        {
            var fixture = new Fixture();
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var foodWasteSystemDataService = new FoodWasteSystemDataService(fixture.Create<ICommandBus>(), fixture.Create<IQueryBus>(), fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteSystemDataService, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodWasteSystemDataService.TranslationModify(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("command"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that TranslationModify calls Publish on the command bus.
        /// </summary>
        [Test]
        public void TestThatTranslationModifyCallsPublishOnCommandBus()
        {
            var fixture = new Fixture();
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var commandBusMock = MockRepository.GenerateMock<ICommandBus>();
            commandBusMock.Stub(m => m.Publish<TranslationModifyCommand, ServiceReceiptResponse>(Arg<TranslationModifyCommand>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var foodWasteSystemDataService = new FoodWasteSystemDataService(commandBusMock, fixture.Create<IQueryBus>(), fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteSystemDataService, Is.Not.Null);

            var command = fixture.Create<TranslationModifyCommand>();

            foodWasteSystemDataService.TranslationModify(command);

            commandBusMock.AssertWasCalled(m => m.Publish<TranslationModifyCommand, ServiceReceiptResponse>(Arg<TranslationModifyCommand>.Is.Equal(command)));
        }

        /// <summary>
        /// Tests that TranslationModify returns the result from the command bus.
        /// </summary>
        [Test]
        public void TestThatTranslationModifyReturnsResultFromCommandBus()
        {
            var fixture = new Fixture();
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var serviceReceipt = fixture.Create<ServiceReceiptResponse>();
            var commandBusMock = MockRepository.GenerateMock<ICommandBus>();
            commandBusMock.Stub(m => m.Publish<TranslationModifyCommand, ServiceReceiptResponse>(Arg<TranslationModifyCommand>.Is.Anything))
                .Return(serviceReceipt)
                .Repeat.Any();

            var foodWasteSystemDataService = new FoodWasteSystemDataService(commandBusMock, fixture.Create<IQueryBus>(), fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteSystemDataService, Is.Not.Null);

            var command = fixture.Create<TranslationModifyCommand>();

            var result = foodWasteSystemDataService.TranslationModify(command);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(serviceReceipt));
        }

        /// <summary>
        /// Tests that TranslationModify throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatTranslationModifyThrowsFaultExceptionWhenExceptionOccurs()
        {
            var fixture = new Fixture();
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));

            var exception = fixture.Create<Exception>();
            var commandBusMock = MockRepository.GenerateMock<ICommandBus>();
            commandBusMock.Stub(m => m.Publish<TranslationModifyCommand, ServiceReceiptResponse>(Arg<TranslationModifyCommand>.Is.Anything))
                .Throw(exception)
                .Repeat.Any();

            var foodWasteFaultExceptionBuilderMock = MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>();
            foodWasteFaultExceptionBuilderMock.Stub(m => m.Build(Arg<Exception>.Is.NotNull, Arg<string>.Is.NotNull, Arg<MethodBase>.Is.NotNull))
                .Return(fixture.Create<FaultException<FoodWasteFault>>())
                .Repeat.Any();

            var foodWasteSystemDataService = new FoodWasteSystemDataService(commandBusMock, fixture.Create<IQueryBus>(), foodWasteFaultExceptionBuilderMock);
            Assert.That(foodWasteSystemDataService, Is.Not.Null);

            var command = fixture.Create<TranslationModifyCommand>();

            var faultException = Assert.Throws<FaultException<FoodWasteFault>>(() => foodWasteSystemDataService.TranslationModify(command));
            Assert.That(faultException, Is.Not.Null);
            Assert.That(faultException.Detail, Is.Not.Null);

            foodWasteFaultExceptionBuilderMock.AssertWasCalled(m => m.Build(Arg<Exception>.Is.Equal(exception), Arg<string>.Is.Equal(SoapNamespaces.FoodWasteSystemDataServiceName), Arg<MethodBase>.Is.NotNull));
        }

        /// <summary>
        /// Tests that TranslationDelete throws an ArgumentNullException when the command for deleting a translation is null.
        /// </summary>
        [Test]
        public void TestThatTranslationDeleteThrowsArgumentNullExceptionIfTranslationDeleteCommandIsNull()
        {
            var fixture = new Fixture();
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var foodWasteSystemDataService = new FoodWasteSystemDataService(fixture.Create<ICommandBus>(), fixture.Create<IQueryBus>(), fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteSystemDataService, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodWasteSystemDataService.TranslationDelete(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("command"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that TranslationDelete calls Publish on the command bus.
        /// </summary>
        [Test]
        public void TestThatTranslationDeleteCallsPublishOnCommandBus()
        {
            var fixture = new Fixture();
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var commandBusMock = MockRepository.GenerateMock<ICommandBus>();
            commandBusMock.Stub(m => m.Publish<TranslationDeleteCommand, ServiceReceiptResponse>(Arg<TranslationDeleteCommand>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var foodWasteSystemDataService = new FoodWasteSystemDataService(commandBusMock, fixture.Create<IQueryBus>(), fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteSystemDataService, Is.Not.Null);

            var command = fixture.Create<TranslationDeleteCommand>();

            foodWasteSystemDataService.TranslationDelete(command);

            commandBusMock.AssertWasCalled(m => m.Publish<TranslationDeleteCommand, ServiceReceiptResponse>(Arg<TranslationDeleteCommand>.Is.Equal(command)));
        }

        /// <summary>
        /// Tests that TranslationDelete returns the result from the command bus.
        /// </summary>
        [Test]
        public void TestThatTranslationDeleteReturnsResultFromCommandBus()
        {
            var fixture = new Fixture();
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var serviceReceipt = fixture.Create<ServiceReceiptResponse>();
            var commandBusMock = MockRepository.GenerateMock<ICommandBus>();
            commandBusMock.Stub(m => m.Publish<TranslationDeleteCommand, ServiceReceiptResponse>(Arg<TranslationDeleteCommand>.Is.Anything))
                .Return(serviceReceipt)
                .Repeat.Any();

            var foodWasteSystemDataService = new FoodWasteSystemDataService(commandBusMock, fixture.Create<IQueryBus>(), fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteSystemDataService, Is.Not.Null);

            var command = fixture.Create<TranslationDeleteCommand>();

            var result = foodWasteSystemDataService.TranslationDelete(command);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(serviceReceipt));
        }

        /// <summary>
        /// Tests that TranslationDelete throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatTranslationDeleteThrowsFaultExceptionWhenExceptionOccurs()
        {
            var fixture = new Fixture();
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));

            var exception = fixture.Create<Exception>();
            var commandBusMock = MockRepository.GenerateMock<ICommandBus>();
            commandBusMock.Stub(m => m.Publish<TranslationDeleteCommand, ServiceReceiptResponse>(Arg<TranslationDeleteCommand>.Is.Anything))
                .Throw(exception)
                .Repeat.Any();

            var foodWasteFaultExceptionBuilderMock = MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>();
            foodWasteFaultExceptionBuilderMock.Stub(m => m.Build(Arg<Exception>.Is.NotNull, Arg<string>.Is.NotNull, Arg<MethodBase>.Is.NotNull))
                .Return(fixture.Create<FaultException<FoodWasteFault>>())
                .Repeat.Any();

            var foodWasteSystemDataService = new FoodWasteSystemDataService(commandBusMock, fixture.Create<IQueryBus>(), foodWasteFaultExceptionBuilderMock);
            Assert.That(foodWasteSystemDataService, Is.Not.Null);

            var command = fixture.Create<TranslationDeleteCommand>();

            var faultException = Assert.Throws<FaultException<FoodWasteFault>>(() => foodWasteSystemDataService.TranslationDelete(command));
            Assert.That(faultException, Is.Not.Null);
            Assert.That(faultException.Detail, Is.Not.Null);

            foodWasteFaultExceptionBuilderMock.AssertWasCalled(m => m.Build(Arg<Exception>.Is.Equal(exception), Arg<string>.Is.Equal(SoapNamespaces.FoodWasteSystemDataServiceName), Arg<MethodBase>.Is.NotNull));
        }

        /// <summary>
        /// Tests that StaticTextGetAll throws an ArgumentNullException when the query for getting all the static texts is null.
        /// </summary>
        [Test]
        public void TestThatStaticTextGetAllThrowsArgumentNullExceptionIfStaticTextCollectionGetQueryIsNull()
        {
            var fixture = new Fixture();
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var foodWasteSystemDataService = new FoodWasteSystemDataService(fixture.Create<ICommandBus>(), fixture.Create<IQueryBus>(), fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteSystemDataService, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodWasteSystemDataService.StaticTextGetAll(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("query"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that StaticTextGetAll calls Query on the query bus.
        /// </summary>
        [Test]
        public void TestThatStaticTextGetAllCallsQueryOnQueryBus()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var queryBusMock = MockRepository.GenerateMock<IQueryBus>();
            queryBusMock.Stub(m => m.Query<StaticTextCollectionGetQuery, IEnumerable<StaticTextSystemView>>(Arg<StaticTextCollectionGetQuery>.Is.NotNull))
                .Return(fixture.CreateMany<StaticTextSystemView>(random.Next(1, 25)).ToList())
                .Repeat.Any();

            var foodWasteSystemDataService = new FoodWasteSystemDataService(fixture.Create<ICommandBus>(), queryBusMock, fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteSystemDataService, Is.Not.Null);

            var query = fixture.Create<StaticTextCollectionGetQuery>();
            foodWasteSystemDataService.StaticTextGetAll(query);

            queryBusMock.AssertWasCalled(m => m.Query<StaticTextCollectionGetQuery, IEnumerable<StaticTextSystemView>>(Arg<StaticTextCollectionGetQuery>.Is.Equal(query)));
        }

        /// <summary>
        /// Tests that StaticTextGetAll returns the result from the query bus.
        /// </summary>
        [Test]
        public void TestThatStaticTextGetAllReturnsResultFromQueryBus()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var translationInfoSystemViewCollection = fixture.CreateMany<StaticTextSystemView>(random.Next(1, 25)).ToList();
            var queryBusMock = MockRepository.GenerateMock<IQueryBus>();
            queryBusMock.Stub(m => m.Query<StaticTextCollectionGetQuery, IEnumerable<StaticTextSystemView>>(Arg<StaticTextCollectionGetQuery>.Is.NotNull))
                .Return(translationInfoSystemViewCollection)
                .Repeat.Any();

            var foodWasteSystemDataService = new FoodWasteSystemDataService(fixture.Create<ICommandBus>(), queryBusMock, fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteSystemDataService, Is.Not.Null);

            var result = foodWasteSystemDataService.StaticTextGetAll(fixture.Create<StaticTextCollectionGetQuery>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EqualTo(translationInfoSystemViewCollection));
        }

        /// <summary>
        /// Tests that StaticTextGetAll throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatStaticTextGetAllThrowsFaultExceptionWhenExceptionOccurs()
        {
            var fixture = new Fixture();
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));

            var exception = fixture.Create<Exception>();
            var queryBusMock = MockRepository.GenerateMock<IQueryBus>();
            queryBusMock.Stub(m => m.Query<StaticTextCollectionGetQuery, IEnumerable<StaticTextSystemView>>(Arg<StaticTextCollectionGetQuery>.Is.NotNull))
                .Throw(exception)
                .Repeat.Any();

            var foodWasteFaultExceptionBuilderMock = MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>();
            foodWasteFaultExceptionBuilderMock.Stub(m => m.Build(Arg<Exception>.Is.NotNull, Arg<string>.Is.NotNull, Arg<MethodBase>.Is.NotNull))
                .Return(fixture.Create<FaultException<FoodWasteFault>>())
                .Repeat.Any();

            var foodWasteSystemDataService = new FoodWasteSystemDataService(fixture.Create<ICommandBus>(), queryBusMock, foodWasteFaultExceptionBuilderMock);
            Assert.That(foodWasteSystemDataService, Is.Not.Null);

            var faultException = Assert.Throws<FaultException<FoodWasteFault>>(() => foodWasteSystemDataService.StaticTextGetAll(fixture.Create<StaticTextCollectionGetQuery>()));
            Assert.That(faultException, Is.Not.Null);
            Assert.That(faultException.Detail, Is.Not.Null);

            foodWasteFaultExceptionBuilderMock.AssertWasCalled(m => m.Build(Arg<Exception>.Is.Equal(exception), Arg<string>.Is.Equal(SoapNamespaces.FoodWasteSystemDataServiceName), Arg<MethodBase>.Is.NotNull));
        }

        /// <summary>
        /// Tests that TranslationInfoGetAll throws an ArgumentNullException when the query for getting all the translation informations which can be used for translations is null.
        /// </summary>
        [Test]
        public void TestThatTranslationInfoGetAllThrowsArgumentNullExceptionIfTranslationInfoCollectionGetQueryIsNull()
        {
            var fixture = new Fixture();
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var foodWasteSystemDataService = new FoodWasteSystemDataService(fixture.Create<ICommandBus>(), fixture.Create<IQueryBus>(), fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteSystemDataService, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodWasteSystemDataService.TranslationInfoGetAll(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("query"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that TranslationInfoGetAll calls Query on the query bus.
        /// </summary>
        [Test]
        public void TestThatTranslationInfoGetAllCallsQueryOnQueryBus()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var queryBusMock = MockRepository.GenerateMock<IQueryBus>();
            queryBusMock.Stub(m => m.Query<TranslationInfoCollectionGetQuery, IEnumerable<TranslationInfoSystemView>>(Arg<TranslationInfoCollectionGetQuery>.Is.NotNull))
                .Return(fixture.CreateMany<TranslationInfoSystemView>(random.Next(1, 25)).ToList())
                .Repeat.Any();

            var foodWasteSystemDataService = new FoodWasteSystemDataService(fixture.Create<ICommandBus>(), queryBusMock, fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteSystemDataService, Is.Not.Null);

            var query = fixture.Create<TranslationInfoCollectionGetQuery>();
            foodWasteSystemDataService.TranslationInfoGetAll(query);

            queryBusMock.AssertWasCalled(m => m.Query<TranslationInfoCollectionGetQuery, IEnumerable<TranslationInfoSystemView>>(Arg<TranslationInfoCollectionGetQuery>.Is.Equal(query)));
        }

        /// <summary>
        /// Tests that TranslationInfoGetAll returns the result from the query bus.
        /// </summary>
        [Test]
        public void TestThatTranslationInfoGetAllReturnsResultFromQueryBus()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var translationInfoSystemViewCollection = fixture.CreateMany<TranslationInfoSystemView>(random.Next(1, 25)).ToList();
            var queryBusMock = MockRepository.GenerateMock<IQueryBus>();
            queryBusMock.Stub(m => m.Query<TranslationInfoCollectionGetQuery, IEnumerable<TranslationInfoSystemView>>(Arg<TranslationInfoCollectionGetQuery>.Is.NotNull))
                .Return(translationInfoSystemViewCollection)
                .Repeat.Any();

            var foodWasteSystemDataService = new FoodWasteSystemDataService(fixture.Create<ICommandBus>(), queryBusMock, fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteSystemDataService, Is.Not.Null);

            var result = foodWasteSystemDataService.TranslationInfoGetAll(fixture.Create<TranslationInfoCollectionGetQuery>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EqualTo(translationInfoSystemViewCollection));
        }

        /// <summary>
        /// Tests that TranslationInfoGetAll throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatTranslationInfoGetAllThrowsFaultExceptionWhenExceptionOccurs()
        {
            var fixture = new Fixture();
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));

            var exception = fixture.Create<Exception>();
            var queryBusMock = MockRepository.GenerateMock<IQueryBus>();
            queryBusMock.Stub(m => m.Query<TranslationInfoCollectionGetQuery, IEnumerable<TranslationInfoSystemView>>(Arg<TranslationInfoCollectionGetQuery>.Is.NotNull))
                .Throw(exception)
                .Repeat.Any();

            var foodWasteFaultExceptionBuilderMock = MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>();
            foodWasteFaultExceptionBuilderMock.Stub(m => m.Build(Arg<Exception>.Is.NotNull, Arg<string>.Is.NotNull, Arg<MethodBase>.Is.NotNull))
                .Return(fixture.Create<FaultException<FoodWasteFault>>())
                .Repeat.Any();

            var foodWasteSystemDataService = new FoodWasteSystemDataService(fixture.Create<ICommandBus>(), queryBusMock, foodWasteFaultExceptionBuilderMock);
            Assert.That(foodWasteSystemDataService, Is.Not.Null);

            var faultException = Assert.Throws<FaultException<FoodWasteFault>>(() => foodWasteSystemDataService.TranslationInfoGetAll(fixture.Create<TranslationInfoCollectionGetQuery>()));
            Assert.That(faultException, Is.Not.Null);
            Assert.That(faultException.Detail, Is.Not.Null);

            foodWasteFaultExceptionBuilderMock.AssertWasCalled(m => m.Build(Arg<Exception>.Is.Equal(exception), Arg<string>.Is.Equal(SoapNamespaces.FoodWasteSystemDataServiceName), Arg<MethodBase>.Is.NotNull));
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
        /// <typeparam name="TQuery">Type of the query which should be tested.</typeparam>
        /// <typeparam name="TView">Type of the view which should be tested.</typeparam>
        /// <param name="queryResult">The result of the query.</param>
        /// <param name="queryException">The exception which should be thrown when query are executed.</param>
        /// <param name="faultException">The fault exception which sould be returned for the fault exception builder.</param>
        /// <returns>Instance of the service which can access and modify system data in the food waste domain which can be used for unit testing.</returns>
        private IFoodWasteSystemDataService CreateSut<TQuery, TView>(TView queryResult, Exception queryException = null, FaultException<FoodWasteFault> faultException = null) where TQuery : class, IQuery
        {
            if (queryResult == null)
            {
                throw new ArgumentNullException(nameof(queryResult));
            }

            _commandBusMock = MockRepository.GenerateMock<ICommandBus>();

            _queryBusMock = MockRepository.GenerateMock<IQueryBus>();
            if (queryException != null)
            {
                _queryBusMock.Stub(m => m.Query<TQuery, TView>(Arg<TQuery>.Is.Anything))
                    .Throw(queryException)
                    .Repeat.Any();
            }
            else
            {
                _queryBusMock.Stub(m => m.Query<TQuery, TView>(Arg<TQuery>.Is.Anything))
                    .Return(queryResult)
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
