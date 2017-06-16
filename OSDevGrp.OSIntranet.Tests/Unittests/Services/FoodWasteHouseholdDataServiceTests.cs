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
using Ploeh.AutoFixture;
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
            var fixture = new Fixture();
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var foodWasteHouseholdDataService = new FoodWasteHouseholdDataService(fixture.Create<ICommandBus>(), fixture.Create<IQueryBus>(), fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteHouseholdDataService, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodWasteHouseholdDataService.HouseholdRemoveHouseholdMember(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("command"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that HouseholdRemoveHouseholdMember calls Publish on the command bus.
        /// </summary>
        [Test]
        public void TestThatHouseholdRemoveHouseholdMemberCallsPublishOnCommandBus()
        {
            var fixture = new Fixture();
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            commandBus.Stub(m => m.Publish<HouseholdRemoveHouseholdMemberCommand, ServiceReceiptResponse>(Arg<HouseholdRemoveHouseholdMemberCommand>.Is.NotNull))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var foodWasteHouseholdDataService = new FoodWasteHouseholdDataService(commandBus, fixture.Create<IQueryBus>(), fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteHouseholdDataService, Is.Not.Null);

            var command = fixture.Create<HouseholdRemoveHouseholdMemberCommand>();
            foodWasteHouseholdDataService.HouseholdRemoveHouseholdMember(command);

            commandBus.AssertWasCalled(m => m.Publish<HouseholdRemoveHouseholdMemberCommand, ServiceReceiptResponse>(Arg<HouseholdRemoveHouseholdMemberCommand>.Is.Equal(command)));
        }

        /// <summary>
        /// Tests that HouseholdRemoveHouseholdMember returns the result from the command bus.
        /// </summary>
        [Test]
        public void TestThatHouseholdRemoveHouseholdMemberReturnsResultFromCommand()
        {
            var fixture = new Fixture();
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var serviceReceiptResponse = fixture.Create<ServiceReceiptResponse>();
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            commandBus.Stub(m => m.Publish<HouseholdRemoveHouseholdMemberCommand, ServiceReceiptResponse>(Arg<HouseholdRemoveHouseholdMemberCommand>.Is.NotNull))
                .Return(serviceReceiptResponse)
                .Repeat.Any();

            var foodWasteHouseholdDataService = new FoodWasteHouseholdDataService(commandBus, fixture.Create<IQueryBus>(), fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteHouseholdDataService, Is.Not.Null);

            var result = foodWasteHouseholdDataService.HouseholdRemoveHouseholdMember(fixture.Create<HouseholdRemoveHouseholdMemberCommand>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(serviceReceiptResponse));
        }

        /// <summary>
        /// Tests that HouseholdRemoveHouseholdMember throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatHouseholdRemoveHouseholdMemberThrowsFaultExceptionWhenExceptionOccurs()
        {
            var fixture = new Fixture();
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));

            var exception = fixture.Create<Exception>();
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            commandBus.Stub(m => m.Publish<HouseholdRemoveHouseholdMemberCommand, ServiceReceiptResponse>(Arg<HouseholdRemoveHouseholdMemberCommand>.Is.NotNull))
                .Throw(exception)
                .Repeat.Any();

            var foodWasteFaultExceptionBuilderMock = MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>();
            foodWasteFaultExceptionBuilderMock.Stub(m => m.Build(Arg<Exception>.Is.NotNull, Arg<string>.Is.NotNull, Arg<MethodBase>.Is.NotNull))
                .Return(fixture.Create<FaultException<FoodWasteFault>>())
                .Repeat.Any();

            var foodWasteHouseholdDataService = new FoodWasteHouseholdDataService(commandBus, fixture.Create<IQueryBus>(), foodWasteFaultExceptionBuilderMock);
            Assert.That(foodWasteHouseholdDataService, Is.Not.Null);

            var faultException = Assert.Throws<FaultException<FoodWasteFault>>(() => foodWasteHouseholdDataService.HouseholdRemoveHouseholdMember(fixture.Create<HouseholdRemoveHouseholdMemberCommand>()));
            Assert.That(faultException, Is.Not.Null);
            Assert.That(faultException.Detail, Is.Not.Null);

            foodWasteFaultExceptionBuilderMock.AssertWasCalled(m => m.Build(Arg<Exception>.Is.Equal(exception), Arg<string>.Is.Equal(SoapNamespaces.FoodWasteHouseholdDataServiceName), Arg<MethodBase>.Is.NotNull));
        }

        /// <summary>
        /// Tests that HouseholdMemberIsCreated throws an ArgumentNullException when the query which can check whether the current caller has been created as a household member is null.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberIsCreatedThrowsArgumentNullExceptionIfHouseholdMemberIsCreatedQueryIsNull()
        {
            var fixture = new Fixture();
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var foodWasteHouseholdDataService = new FoodWasteHouseholdDataService(fixture.Create<ICommandBus>(), fixture.Create<IQueryBus>(), fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteHouseholdDataService, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodWasteHouseholdDataService.HouseholdMemberIsCreated(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("query"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that HouseholdMemberIsCreated calls Query on the query bus.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberIsCreatedCallsQueryOnQueryBus()
        {
            var fixture = new Fixture();
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var queryBusMock = MockRepository.GenerateMock<IQueryBus>();
            queryBusMock.Stub(m => m.Query<HouseholdMemberIsCreatedQuery, BooleanResultResponse>(Arg<HouseholdMemberIsCreatedQuery>.Is.NotNull))
                .Return(fixture.Create<BooleanResultResponse>())
                .Repeat.Any();

            var foodWasteHouseholdDataService = new FoodWasteHouseholdDataService(fixture.Create<ICommandBus>(), queryBusMock, fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteHouseholdDataService, Is.Not.Null);

            var query = fixture.Create<HouseholdMemberIsCreatedQuery>();
            foodWasteHouseholdDataService.HouseholdMemberIsCreated(query);

            queryBusMock.AssertWasCalled(m => m.Query<HouseholdMemberIsCreatedQuery, BooleanResultResponse>(Arg<HouseholdMemberIsCreatedQuery>.Is.Equal(query)));
        }

        /// <summary>
        /// Tests that HouseholdMemberIsCreated returns the result from the query bus.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberIsCreatedReturnsResultFromQueryBus()
        {
            var fixture = new Fixture();
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var booleanResultResponse = fixture.Create<BooleanResultResponse>();
            var queryBusMock = MockRepository.GenerateMock<IQueryBus>();
            queryBusMock.Stub(m => m.Query<HouseholdMemberIsCreatedQuery, BooleanResultResponse>(Arg<HouseholdMemberIsCreatedQuery>.Is.NotNull))
                .Return(booleanResultResponse)
                .Repeat.Any();

            var foodWasteHouseholdDataService = new FoodWasteHouseholdDataService(fixture.Create<ICommandBus>(), queryBusMock, fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteHouseholdDataService, Is.Not.Null);

            var result = foodWasteHouseholdDataService.HouseholdMemberIsCreated(fixture.Create<HouseholdMemberIsCreatedQuery>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(booleanResultResponse));
        }

        /// <summary>
        /// Tests that HouseholdMemberIsCreated throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberIsCreatedThrowsFaultExceptionWhenExceptionOccurs()
        {
            var fixture = new Fixture();
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));

            var exception = fixture.Create<Exception>();
            var queryBusMock = MockRepository.GenerateMock<IQueryBus>();
            queryBusMock.Stub(m => m.Query<HouseholdMemberIsCreatedQuery, BooleanResultResponse>(Arg<HouseholdMemberIsCreatedQuery>.Is.NotNull))
                .Throw(exception)
                .Repeat.Any();

            var foodWasteFaultExceptionBuilderMock = MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>();
            foodWasteFaultExceptionBuilderMock.Stub(m => m.Build(Arg<Exception>.Is.NotNull, Arg<string>.Is.NotNull, Arg<MethodBase>.Is.NotNull))
                .Return(fixture.Create<FaultException<FoodWasteFault>>())
                .Repeat.Any();

            var foodWasteHouseholdDataService = new FoodWasteHouseholdDataService(fixture.Create<ICommandBus>(), queryBusMock, foodWasteFaultExceptionBuilderMock);
            Assert.That(foodWasteHouseholdDataService, Is.Not.Null);

            var faultException = Assert.Throws<FaultException<FoodWasteFault>>(() => foodWasteHouseholdDataService.HouseholdMemberIsCreated(fixture.Create<HouseholdMemberIsCreatedQuery>()));
            Assert.That(faultException, Is.Not.Null);
            Assert.That(faultException.Detail, Is.Not.Null);

            foodWasteFaultExceptionBuilderMock.AssertWasCalled(m => m.Build(Arg<Exception>.Is.Equal(exception), Arg<string>.Is.Equal(SoapNamespaces.FoodWasteHouseholdDataServiceName), Arg<MethodBase>.Is.NotNull));
        }

        /// <summary>
        /// Tests that HouseholdMemberIsActivated throws an ArgumentNullException when the query which can check whether the current caller has been activated is null.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberIsActivatedThrowsArgumentNullExceptionIfHouseholdMemberIsActivatedQueryIsNull()
        {
            var fixture = new Fixture();
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var foodWasteHouseholdDataService = new FoodWasteHouseholdDataService(fixture.Create<ICommandBus>(), fixture.Create<IQueryBus>(), fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteHouseholdDataService, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodWasteHouseholdDataService.HouseholdMemberIsActivated(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("query"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that HouseholdMemberIsActivated calls Query on the query bus.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberIsActivatedCallsQueryOnQueryBus()
        {
            var fixture = new Fixture();
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var queryBusMock = MockRepository.GenerateMock<IQueryBus>();
            queryBusMock.Stub(m => m.Query<HouseholdMemberIsActivatedQuery, BooleanResultResponse>(Arg<HouseholdMemberIsActivatedQuery>.Is.NotNull))
                .Return(fixture.Create<BooleanResultResponse>())
                .Repeat.Any();

            var foodWasteHouseholdDataService = new FoodWasteHouseholdDataService(fixture.Create<ICommandBus>(), queryBusMock, fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteHouseholdDataService, Is.Not.Null);

            var query = fixture.Create<HouseholdMemberIsActivatedQuery>();
            foodWasteHouseholdDataService.HouseholdMemberIsActivated(query);

            queryBusMock.AssertWasCalled(m => m.Query<HouseholdMemberIsActivatedQuery, BooleanResultResponse>(Arg<HouseholdMemberIsActivatedQuery>.Is.Equal(query)));
        }

        /// <summary>
        /// Tests that HouseholdMemberIsActivated returns the result from the query bus.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberIsActivatedReturnsResultFromQueryBus()
        {
            var fixture = new Fixture();
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var booleanResultResponse = fixture.Create<BooleanResultResponse>();
            var queryBusMock = MockRepository.GenerateMock<IQueryBus>();
            queryBusMock.Stub(m => m.Query<HouseholdMemberIsActivatedQuery, BooleanResultResponse>(Arg<HouseholdMemberIsActivatedQuery>.Is.NotNull))
                .Return(booleanResultResponse)
                .Repeat.Any();

            var foodWasteHouseholdDataService = new FoodWasteHouseholdDataService(fixture.Create<ICommandBus>(), queryBusMock, fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteHouseholdDataService, Is.Not.Null);

            var result = foodWasteHouseholdDataService.HouseholdMemberIsActivated(fixture.Create<HouseholdMemberIsActivatedQuery>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(booleanResultResponse));
        }

        /// <summary>
        /// Tests that HouseholdMemberIsActivated throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberIsActivatedThrowsFaultExceptionWhenExceptionOccurs()
        {
            var fixture = new Fixture();
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));

            var exception = fixture.Create<Exception>();
            var queryBusMock = MockRepository.GenerateMock<IQueryBus>();
            queryBusMock.Stub(m => m.Query<HouseholdMemberIsActivatedQuery, BooleanResultResponse>(Arg<HouseholdMemberIsActivatedQuery>.Is.NotNull))
                .Throw(exception)
                .Repeat.Any();

            var foodWasteFaultExceptionBuilderMock = MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>();
            foodWasteFaultExceptionBuilderMock.Stub(m => m.Build(Arg<Exception>.Is.NotNull, Arg<string>.Is.NotNull, Arg<MethodBase>.Is.NotNull))
                .Return(fixture.Create<FaultException<FoodWasteFault>>())
                .Repeat.Any();

            var foodWasteHouseholdDataService = new FoodWasteHouseholdDataService(fixture.Create<ICommandBus>(), queryBusMock, foodWasteFaultExceptionBuilderMock);
            Assert.That(foodWasteHouseholdDataService, Is.Not.Null);

            var faultException = Assert.Throws<FaultException<FoodWasteFault>>(() => foodWasteHouseholdDataService.HouseholdMemberIsActivated(fixture.Create<HouseholdMemberIsActivatedQuery>()));
            Assert.That(faultException, Is.Not.Null);
            Assert.That(faultException.Detail, Is.Not.Null);

            foodWasteFaultExceptionBuilderMock.AssertWasCalled(m => m.Build(Arg<Exception>.Is.Equal(exception), Arg<string>.Is.Equal(SoapNamespaces.FoodWasteHouseholdDataServiceName), Arg<MethodBase>.Is.NotNull));
        }

        /// <summary>
        /// Tests that HouseholdMemberHasAcceptedPrivacyPolicy throws an ArgumentNullException when the query which can check whether the current caller has accepted the privacy policy is null.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberHasAcceptedPrivacyPolicyThrowsArgumentNullExceptionIfHouseholdMemberHasAcceptedPrivacyPolicyQueryIsNull()
        {
            var fixture = new Fixture();
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var foodWasteHouseholdDataService = new FoodWasteHouseholdDataService(fixture.Create<ICommandBus>(), fixture.Create<IQueryBus>(), fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteHouseholdDataService, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodWasteHouseholdDataService.HouseholdMemberHasAcceptedPrivacyPolicy(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("query"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that HouseholdMemberHasAcceptedPrivacyPolicy calls Query on the query bus.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberHasAcceptedPrivacyPolicyCallsQueryOnQueryBus()
        {
            var fixture = new Fixture();
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var queryBusMock = MockRepository.GenerateMock<IQueryBus>();
            queryBusMock.Stub(m => m.Query<HouseholdMemberHasAcceptedPrivacyPolicyQuery, BooleanResultResponse>(Arg<HouseholdMemberHasAcceptedPrivacyPolicyQuery>.Is.NotNull))
                .Return(fixture.Create<BooleanResultResponse>())
                .Repeat.Any();

            var foodWasteHouseholdDataService = new FoodWasteHouseholdDataService(fixture.Create<ICommandBus>(), queryBusMock, fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteHouseholdDataService, Is.Not.Null);

            var query = fixture.Create<HouseholdMemberHasAcceptedPrivacyPolicyQuery>();
            foodWasteHouseholdDataService.HouseholdMemberHasAcceptedPrivacyPolicy(query);

            queryBusMock.AssertWasCalled(m => m.Query<HouseholdMemberHasAcceptedPrivacyPolicyQuery, BooleanResultResponse>(Arg<HouseholdMemberHasAcceptedPrivacyPolicyQuery>.Is.Equal(query)));
        }

        /// <summary>
        /// Tests that HouseholdMemberHasAcceptedPrivacyPolicy returns the result from the query bus.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberHasAcceptedPrivacyPolicyReturnsResultFromQueryBus()
        {
            var fixture = new Fixture();
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var booleanResultResponse = fixture.Create<BooleanResultResponse>();
            var queryBusMock = MockRepository.GenerateMock<IQueryBus>();
            queryBusMock.Stub(m => m.Query<HouseholdMemberHasAcceptedPrivacyPolicyQuery, BooleanResultResponse>(Arg<HouseholdMemberHasAcceptedPrivacyPolicyQuery>.Is.NotNull))
                .Return(booleanResultResponse)
                .Repeat.Any();

            var foodWasteHouseholdDataService = new FoodWasteHouseholdDataService(fixture.Create<ICommandBus>(), queryBusMock, fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteHouseholdDataService, Is.Not.Null);

            var result = foodWasteHouseholdDataService.HouseholdMemberHasAcceptedPrivacyPolicy(fixture.Create<HouseholdMemberHasAcceptedPrivacyPolicyQuery>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(booleanResultResponse));
        }

        /// <summary>
        /// Tests that HouseholdMemberHasAcceptedPrivacyPolicy throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberHasAcceptedPrivacyPolicyThrowsFaultExceptionWhenExceptionOccurs()
        {
            var fixture = new Fixture();
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));

            var exception = fixture.Create<Exception>();
            var queryBusMock = MockRepository.GenerateMock<IQueryBus>();
            queryBusMock.Stub(m => m.Query<HouseholdMemberHasAcceptedPrivacyPolicyQuery, BooleanResultResponse>(Arg<HouseholdMemberHasAcceptedPrivacyPolicyQuery>.Is.NotNull))
                .Throw(exception)
                .Repeat.Any();

            var foodWasteFaultExceptionBuilderMock = MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>();
            foodWasteFaultExceptionBuilderMock.Stub(m => m.Build(Arg<Exception>.Is.NotNull, Arg<string>.Is.NotNull, Arg<MethodBase>.Is.NotNull))
                .Return(fixture.Create<FaultException<FoodWasteFault>>())
                .Repeat.Any();

            var foodWasteHouseholdDataService = new FoodWasteHouseholdDataService(fixture.Create<ICommandBus>(), queryBusMock, foodWasteFaultExceptionBuilderMock);
            Assert.That(foodWasteHouseholdDataService, Is.Not.Null);

            var faultException = Assert.Throws<FaultException<FoodWasteFault>>(() => foodWasteHouseholdDataService.HouseholdMemberHasAcceptedPrivacyPolicy(fixture.Create<HouseholdMemberHasAcceptedPrivacyPolicyQuery>()));
            Assert.That(faultException, Is.Not.Null);
            Assert.That(faultException.Detail, Is.Not.Null);

            foodWasteFaultExceptionBuilderMock.AssertWasCalled(m => m.Build(Arg<Exception>.Is.Equal(exception), Arg<string>.Is.Equal(SoapNamespaces.FoodWasteHouseholdDataServiceName), Arg<MethodBase>.Is.NotNull));
        }

        /// <summary>
        /// Tests that HouseholdMemberDataGet throws an ArgumentNullException when the query which can get household member data for the current caller is null.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberDataGetThrowsArgumentNullExceptionIfHouseholdMemberDataGetQueryIsNull()
        {
            var fixture = new Fixture();
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var foodWasteHouseholdDataService = new FoodWasteHouseholdDataService(fixture.Create<ICommandBus>(), fixture.Create<IQueryBus>(), fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteHouseholdDataService, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodWasteHouseholdDataService.HouseholdMemberDataGet(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("query"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that HouseholdMemberDataGet calls Query on the query bus.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberDataGetCallsQueryOnQueryBus()
        {
            var fixture = new Fixture();
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var queryBusMock = MockRepository.GenerateMock<IQueryBus>();
            queryBusMock.Stub(m => m.Query<HouseholdMemberDataGetQuery, HouseholdMemberView>(Arg<HouseholdMemberDataGetQuery>.Is.NotNull))
                .Return(fixture.Create<HouseholdMemberView>())
                .Repeat.Any();

            var foodWasteHouseholdDataService = new FoodWasteHouseholdDataService(fixture.Create<ICommandBus>(), queryBusMock, fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteHouseholdDataService, Is.Not.Null);

            var query = fixture.Create<HouseholdMemberDataGetQuery>();
            foodWasteHouseholdDataService.HouseholdMemberDataGet(query);

            queryBusMock.AssertWasCalled(m => m.Query<HouseholdMemberDataGetQuery, HouseholdMemberView>(Arg<HouseholdMemberDataGetQuery>.Is.Equal(query)));
        }

        /// <summary>
        /// Tests that HouseholdMemberDataGet returns the result from the query bus.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberDataGetReturnsResultFromQueryBus()
        {
            var fixture = new Fixture();
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var householdMemberView = fixture.Create<HouseholdMemberView>();
            var queryBusMock = MockRepository.GenerateMock<IQueryBus>();
            queryBusMock.Stub(m => m.Query<HouseholdMemberDataGetQuery, HouseholdMemberView>(Arg<HouseholdMemberDataGetQuery>.Is.NotNull))
                .Return(householdMemberView)
                .Repeat.Any();

            var foodWasteHouseholdDataService = new FoodWasteHouseholdDataService(fixture.Create<ICommandBus>(), queryBusMock, fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteHouseholdDataService, Is.Not.Null);

            var result = foodWasteHouseholdDataService.HouseholdMemberDataGet(fixture.Create<HouseholdMemberDataGetQuery>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(householdMemberView));
        }

        /// <summary>
        /// Tests that HouseholdMemberDataGet throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberDataGetThrowsFaultExceptionWhenExceptionOccurs()
        {
            var fixture = new Fixture();
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));

            var exception = fixture.Create<Exception>();
            var queryBusMock = MockRepository.GenerateMock<IQueryBus>();
            queryBusMock.Stub(m => m.Query<HouseholdMemberDataGetQuery, HouseholdMemberView>(Arg<HouseholdMemberDataGetQuery>.Is.NotNull))
                .Throw(exception)
                .Repeat.Any();

            var foodWasteFaultExceptionBuilderMock = MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>();
            foodWasteFaultExceptionBuilderMock.Stub(m => m.Build(Arg<Exception>.Is.NotNull, Arg<string>.Is.NotNull, Arg<MethodBase>.Is.NotNull))
                .Return(fixture.Create<FaultException<FoodWasteFault>>())
                .Repeat.Any();

            var foodWasteHouseholdDataService = new FoodWasteHouseholdDataService(fixture.Create<ICommandBus>(), queryBusMock, foodWasteFaultExceptionBuilderMock);
            Assert.That(foodWasteHouseholdDataService, Is.Not.Null);

            var faultException = Assert.Throws<FaultException<FoodWasteFault>>(() => foodWasteHouseholdDataService.HouseholdMemberDataGet(fixture.Create<HouseholdMemberDataGetQuery>()));
            Assert.That(faultException, Is.Not.Null);
            Assert.That(faultException.Detail, Is.Not.Null);

            foodWasteFaultExceptionBuilderMock.AssertWasCalled(m => m.Build(Arg<Exception>.Is.Equal(exception), Arg<string>.Is.Equal(SoapNamespaces.FoodWasteHouseholdDataServiceName), Arg<MethodBase>.Is.NotNull));
        }

        /// <summary>
        /// Tests that HouseholdMemberActivate throws an ArgumentNullException when the command for activating the current callers household member account is null.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberActivateThrowsArgumentNullExceptionIfHouseholdMemberActivateCommandIsNull()
        {
            var fixture = new Fixture();
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var foodWasteHouseholdDataService = new FoodWasteHouseholdDataService(fixture.Create<ICommandBus>(), fixture.Create<IQueryBus>(), fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteHouseholdDataService, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodWasteHouseholdDataService.HouseholdMemberActivate(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("command"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that HouseholdMemberActivate calls Publish on the command bus.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberActivateCallsPublishOnCommandBus()
        {
            var fixture = new Fixture();
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            commandBus.Stub(m => m.Publish<HouseholdMemberActivateCommand, ServiceReceiptResponse>(Arg<HouseholdMemberActivateCommand>.Is.NotNull))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var foodWasteHouseholdDataService = new FoodWasteHouseholdDataService(commandBus, fixture.Create<IQueryBus>(), fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteHouseholdDataService, Is.Not.Null);

            var command = fixture.Create<HouseholdMemberActivateCommand>();
            foodWasteHouseholdDataService.HouseholdMemberActivate(command);

            commandBus.AssertWasCalled(m => m.Publish<HouseholdMemberActivateCommand, ServiceReceiptResponse>(Arg<HouseholdMemberActivateCommand>.Is.Equal(command)));
        }

        /// <summary>
        /// Tests that HouseholdMemberActivate returns the result from the command bus.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberActivateReturnsResultFromCommand()
        {
            var fixture = new Fixture();
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var serviceReceiptResponse = fixture.Create<ServiceReceiptResponse>();
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            commandBus.Stub(m => m.Publish<HouseholdMemberActivateCommand, ServiceReceiptResponse>(Arg<HouseholdMemberActivateCommand>.Is.NotNull))
                .Return(serviceReceiptResponse)
                .Repeat.Any();

            var foodWasteHouseholdDataService = new FoodWasteHouseholdDataService(commandBus, fixture.Create<IQueryBus>(), fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteHouseholdDataService, Is.Not.Null);

            var result = foodWasteHouseholdDataService.HouseholdMemberActivate(fixture.Create<HouseholdMemberActivateCommand>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(serviceReceiptResponse));
        }

        /// <summary>
        /// Tests that HouseholdMemberActivate throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberActivateThrowsFaultExceptionWhenExceptionOccurs()
        {
            var fixture = new Fixture();
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));

            var exception = fixture.Create<Exception>();
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            commandBus.Stub(m => m.Publish<HouseholdMemberActivateCommand, ServiceReceiptResponse>(Arg<HouseholdMemberActivateCommand>.Is.NotNull))
                .Throw(exception)
                .Repeat.Any();

            var foodWasteFaultExceptionBuilderMock = MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>();
            foodWasteFaultExceptionBuilderMock.Stub(m => m.Build(Arg<Exception>.Is.NotNull, Arg<string>.Is.NotNull, Arg<MethodBase>.Is.NotNull))
                .Return(fixture.Create<FaultException<FoodWasteFault>>())
                .Repeat.Any();

            var foodWasteHouseholdDataService = new FoodWasteHouseholdDataService(commandBus, fixture.Create<IQueryBus>(), foodWasteFaultExceptionBuilderMock);
            Assert.That(foodWasteHouseholdDataService, Is.Not.Null);

            var faultException = Assert.Throws<FaultException<FoodWasteFault>>(() => foodWasteHouseholdDataService.HouseholdMemberActivate(fixture.Create<HouseholdMemberActivateCommand>()));
            Assert.That(faultException, Is.Not.Null);
            Assert.That(faultException.Detail, Is.Not.Null);

            foodWasteFaultExceptionBuilderMock.AssertWasCalled(m => m.Build(Arg<Exception>.Is.Equal(exception), Arg<string>.Is.Equal(SoapNamespaces.FoodWasteHouseholdDataServiceName), Arg<MethodBase>.Is.NotNull));
        }

        /// <summary>
        /// Tests that HouseholdMemberAcceptPrivacyPolicy throws an ArgumentNullException when the command for accepting privacy policy on the current callers household member account is null.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberAcceptPrivacyPolicyThrowsArgumentNullExceptionIfHouseholdMemberAcceptPrivacyPolicyCommandIsNull()
        {
            var fixture = new Fixture();
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var foodWasteHouseholdDataService = new FoodWasteHouseholdDataService(fixture.Create<ICommandBus>(), fixture.Create<IQueryBus>(), fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteHouseholdDataService, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodWasteHouseholdDataService.HouseholdMemberAcceptPrivacyPolicy(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("command"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that HouseholdMemberAcceptPrivacyPolicy calls Publish on the command bus.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberAcceptPrivacyPolicyCallsPublishOnCommandBus()
        {
            var fixture = new Fixture();
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            commandBus.Stub(m => m.Publish<HouseholdMemberAcceptPrivacyPolicyCommand, ServiceReceiptResponse>(Arg<HouseholdMemberAcceptPrivacyPolicyCommand>.Is.NotNull))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var foodWasteHouseholdDataService = new FoodWasteHouseholdDataService(commandBus, fixture.Create<IQueryBus>(), fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteHouseholdDataService, Is.Not.Null);

            var command = fixture.Create<HouseholdMemberAcceptPrivacyPolicyCommand>();
            foodWasteHouseholdDataService.HouseholdMemberAcceptPrivacyPolicy(command);

            commandBus.AssertWasCalled(m => m.Publish<HouseholdMemberAcceptPrivacyPolicyCommand, ServiceReceiptResponse>(Arg<HouseholdMemberAcceptPrivacyPolicyCommand>.Is.Equal(command)));
        }

        /// <summary>
        /// Tests that HouseholdMemberAcceptPrivacyPolicy returns the result from the command bus.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberAcceptPrivacyPolicyReturnsResultFromCommand()
        {
            var fixture = new Fixture();
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var serviceReceiptResponse = fixture.Create<ServiceReceiptResponse>();
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            commandBus.Stub(m => m.Publish<HouseholdMemberAcceptPrivacyPolicyCommand, ServiceReceiptResponse>(Arg<HouseholdMemberAcceptPrivacyPolicyCommand>.Is.NotNull))
                .Return(serviceReceiptResponse)
                .Repeat.Any();

            var foodWasteHouseholdDataService = new FoodWasteHouseholdDataService(commandBus, fixture.Create<IQueryBus>(), fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteHouseholdDataService, Is.Not.Null);

            var result = foodWasteHouseholdDataService.HouseholdMemberAcceptPrivacyPolicy(fixture.Create<HouseholdMemberAcceptPrivacyPolicyCommand>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(serviceReceiptResponse));
        }

        /// <summary>
        /// Tests that HouseholdMemberAcceptPrivacyPolicy throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberAcceptPrivacyPolicyThrowsFaultExceptionWhenExceptionOccurs()
        {
            var fixture = new Fixture();
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));

            var exception = fixture.Create<Exception>();
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            commandBus.Stub(m => m.Publish<HouseholdMemberAcceptPrivacyPolicyCommand, ServiceReceiptResponse>(Arg<HouseholdMemberAcceptPrivacyPolicyCommand>.Is.NotNull))
                .Throw(exception)
                .Repeat.Any();

            var foodWasteFaultExceptionBuilderMock = MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>();
            foodWasteFaultExceptionBuilderMock.Stub(m => m.Build(Arg<Exception>.Is.NotNull, Arg<string>.Is.NotNull, Arg<MethodBase>.Is.NotNull))
                .Return(fixture.Create<FaultException<FoodWasteFault>>())
                .Repeat.Any();

            var foodWasteHouseholdDataService = new FoodWasteHouseholdDataService(commandBus, fixture.Create<IQueryBus>(), foodWasteFaultExceptionBuilderMock);
            Assert.That(foodWasteHouseholdDataService, Is.Not.Null);

            var faultException = Assert.Throws<FaultException<FoodWasteFault>>(() => foodWasteHouseholdDataService.HouseholdMemberAcceptPrivacyPolicy(fixture.Create<HouseholdMemberAcceptPrivacyPolicyCommand>()));
            Assert.That(faultException, Is.Not.Null);
            Assert.That(faultException.Detail, Is.Not.Null);

            foodWasteFaultExceptionBuilderMock.AssertWasCalled(m => m.Build(Arg<Exception>.Is.Equal(exception), Arg<string>.Is.Equal(SoapNamespaces.FoodWasteHouseholdDataServiceName), Arg<MethodBase>.Is.NotNull));
        }

        /// <summary>
        /// Tests that HouseholdMemberUpgradeMembership throws an ArgumentNullException when the command for upgrading the membership on the current callers household member account is null.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberUpgradeMembershipThrowsArgumentNullExceptionIfHouseholdMemberUpgradeMembershipCommandIsNull()
        {
            var fixture = new Fixture();
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var foodWasteHouseholdDataService = new FoodWasteHouseholdDataService(fixture.Create<ICommandBus>(), fixture.Create<IQueryBus>(), fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteHouseholdDataService, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodWasteHouseholdDataService.HouseholdMemberUpgradeMembership(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("command"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that HouseholdMemberUpgradeMembership calls Publish on the command bus.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberUpgradeMembershipCallsPublishOnCommandBus()
        {
            var fixture = new Fixture();
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            commandBus.Stub(m => m.Publish<HouseholdMemberUpgradeMembershipCommand, ServiceReceiptResponse>(Arg<HouseholdMemberUpgradeMembershipCommand>.Is.NotNull))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var foodWasteHouseholdDataService = new FoodWasteHouseholdDataService(commandBus, fixture.Create<IQueryBus>(), fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteHouseholdDataService, Is.Not.Null);

            var command = fixture.Create<HouseholdMemberUpgradeMembershipCommand>();
            foodWasteHouseholdDataService.HouseholdMemberUpgradeMembership(command);

            commandBus.AssertWasCalled(m => m.Publish<HouseholdMemberUpgradeMembershipCommand, ServiceReceiptResponse>(Arg<HouseholdMemberUpgradeMembershipCommand>.Is.Equal(command)));
        }

        /// <summary>
        /// Tests that HouseholdMemberUpgradeMembership returns the result from the command bus.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberUpgradeMembershipReturnsResultFromCommand()
        {
            var fixture = new Fixture();
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var serviceReceiptResponse = fixture.Create<ServiceReceiptResponse>();
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            commandBus.Stub(m => m.Publish<HouseholdMemberUpgradeMembershipCommand, ServiceReceiptResponse>(Arg<HouseholdMemberUpgradeMembershipCommand>.Is.NotNull))
                .Return(serviceReceiptResponse)
                .Repeat.Any();

            var foodWasteHouseholdDataService = new FoodWasteHouseholdDataService(commandBus, fixture.Create<IQueryBus>(), fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteHouseholdDataService, Is.Not.Null);

            var result = foodWasteHouseholdDataService.HouseholdMemberUpgradeMembership(fixture.Create<HouseholdMemberUpgradeMembershipCommand>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(serviceReceiptResponse));
        }

        /// <summary>
        /// Tests that HouseholdMemberUpgradeMembership throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberUpgradeMembershipThrowsFaultExceptionWhenExceptionOccurs()
        {
            var fixture = new Fixture();
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));

            var exception = fixture.Create<Exception>();
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            commandBus.Stub(m => m.Publish<HouseholdMemberUpgradeMembershipCommand, ServiceReceiptResponse>(Arg<HouseholdMemberUpgradeMembershipCommand>.Is.NotNull))
                .Throw(exception)
                .Repeat.Any();

            var foodWasteFaultExceptionBuilderMock = MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>();
            foodWasteFaultExceptionBuilderMock.Stub(m => m.Build(Arg<Exception>.Is.NotNull, Arg<string>.Is.NotNull, Arg<MethodBase>.Is.NotNull))
                .Return(fixture.Create<FaultException<FoodWasteFault>>())
                .Repeat.Any();

            var foodWasteHouseholdDataService = new FoodWasteHouseholdDataService(commandBus, fixture.Create<IQueryBus>(), foodWasteFaultExceptionBuilderMock);
            Assert.That(foodWasteHouseholdDataService, Is.Not.Null);

            var faultException = Assert.Throws<FaultException<FoodWasteFault>>(() => foodWasteHouseholdDataService.HouseholdMemberUpgradeMembership(fixture.Create<HouseholdMemberUpgradeMembershipCommand>()));
            Assert.That(faultException, Is.Not.Null);
            Assert.That(faultException.Detail, Is.Not.Null);

            foodWasteFaultExceptionBuilderMock.AssertWasCalled(m => m.Build(Arg<Exception>.Is.Equal(exception), Arg<string>.Is.Equal(SoapNamespaces.FoodWasteHouseholdDataServiceName), Arg<MethodBase>.Is.NotNull));
        }

        /// <summary>
        /// Tests that FoodItemCollectionGet throws an ArgumentNullException when the query for getting all the collection of food items is null.
        /// </summary>
        [Test]
        public void TestThatFoodItemCollectionGetThrowsArgumentNullExceptionIfFoodItemColletionGetQueryIsNull()
        {
            var fixture = new Fixture();
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var foodWasteHouseholdDataService = new FoodWasteHouseholdDataService(fixture.Create<ICommandBus>(), fixture.Create<IQueryBus>(), fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteHouseholdDataService, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodWasteHouseholdDataService.FoodItemCollectionGet(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("query"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that FoodItemCollectionGet calls Query on the query bus.
        /// </summary>
        [Test]
        public void TestThatFoodItemCollectionGetCallsQueryOnQueryBus()
        {
            var fixture = new Fixture();
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var queryBusMock = MockRepository.GenerateMock<IQueryBus>();
            queryBusMock.Stub(m => m.Query<FoodItemCollectionGetQuery, FoodItemCollectionView>(Arg<FoodItemCollectionGetQuery>.Is.NotNull))
                .Return(fixture.Create<FoodItemCollectionView>())
                .Repeat.Any();

            var foodWasteHouseholdDataService = new FoodWasteHouseholdDataService(fixture.Create<ICommandBus>(), queryBusMock, fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteHouseholdDataService, Is.Not.Null);

            var query = fixture.Create<FoodItemCollectionGetQuery>();
            foodWasteHouseholdDataService.FoodItemCollectionGet(query);

            queryBusMock.AssertWasCalled(m => m.Query<FoodItemCollectionGetQuery, FoodItemCollectionView>(Arg<FoodItemCollectionGetQuery>.Is.Equal(query)));
        }

        /// <summary>
        /// Tests that FoodItemCollectionGet returns the result from the query bus.
        /// </summary>
        [Test]
        public void TestThatFoodItemCollectionGetReturnsResultFromQueryBus()
        {
            var fixture = new Fixture();
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var foodItemCollectionView = fixture.Create<FoodItemCollectionView>();
            var queryBusMock = MockRepository.GenerateMock<IQueryBus>();
            queryBusMock.Stub(m => m.Query<FoodItemCollectionGetQuery, FoodItemCollectionView>(Arg<FoodItemCollectionGetQuery>.Is.NotNull))
                .Return(foodItemCollectionView)
                .Repeat.Any();

            var foodWasteHouseholdDataService = new FoodWasteHouseholdDataService(fixture.Create<ICommandBus>(), queryBusMock, fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteHouseholdDataService, Is.Not.Null);

            var result = foodWasteHouseholdDataService.FoodItemCollectionGet(fixture.Create<FoodItemCollectionGetQuery>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(foodItemCollectionView));
        }

        /// <summary>
        /// Tests that FoodItemCollectionGet throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatFoodItemCollectionGetThrowsFaultExceptionWhenExceptionOccurs()
        {
            var fixture = new Fixture();
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));

            var exception = fixture.Create<Exception>();
            var queryBusMock = MockRepository.GenerateMock<IQueryBus>();
            queryBusMock.Stub(m => m.Query<FoodItemCollectionGetQuery, FoodItemCollectionView>(Arg<FoodItemCollectionGetQuery>.Is.NotNull))
                .Throw(exception)
                .Repeat.Any();

            var foodWasteFaultExceptionBuilderMock = MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>();
            foodWasteFaultExceptionBuilderMock.Stub(m => m.Build(Arg<Exception>.Is.NotNull, Arg<string>.Is.NotNull, Arg<MethodBase>.Is.NotNull))
                .Return(fixture.Create<FaultException<FoodWasteFault>>())
                .Repeat.Any();

            var foodWasteHouseholdDataService = new FoodWasteHouseholdDataService(fixture.Create<ICommandBus>(), queryBusMock, foodWasteFaultExceptionBuilderMock);
            Assert.That(foodWasteHouseholdDataService, Is.Not.Null);

            var faultException = Assert.Throws<FaultException<FoodWasteFault>>(() => foodWasteHouseholdDataService.FoodItemCollectionGet(fixture.Create<FoodItemCollectionGetQuery>()));
            Assert.That(faultException, Is.Not.Null);
            Assert.That(faultException.Detail, Is.Not.Null);

            foodWasteFaultExceptionBuilderMock.AssertWasCalled(m => m.Build(Arg<Exception>.Is.Equal(exception), Arg<string>.Is.Equal(SoapNamespaces.FoodWasteHouseholdDataServiceName), Arg<MethodBase>.Is.NotNull));
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

            var foodWasteHouseholdDataService = new FoodWasteHouseholdDataService(fixture.Create<ICommandBus>(), fixture.Create<IQueryBus>(), fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteHouseholdDataService, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodWasteHouseholdDataService.FoodGroupTreeGet(null));
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
            fixture.Inject(fixture.Build<FoodGroupView>().With(m => m.Parent, fixture.Create<FoodGroupIdentificationView>()).With(m => m.Children, new List<FoodGroupView>(0)).Create());

            var queryBusMock = MockRepository.GenerateMock<IQueryBus>();
            queryBusMock.Stub(m => m.Query<FoodGroupTreeGetQuery, FoodGroupTreeView>(Arg<FoodGroupTreeGetQuery>.Is.NotNull))
                .Return(fixture.Create<FoodGroupTreeView>())
                .Repeat.Any();

            var foodWasteHouseholdDataService = new FoodWasteHouseholdDataService(fixture.Create<ICommandBus>(), queryBusMock, fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteHouseholdDataService, Is.Not.Null);

            var query = fixture.Create<FoodGroupTreeGetQuery>();
            foodWasteHouseholdDataService.FoodGroupTreeGet(query);

            queryBusMock.AssertWasCalled(m => m.Query<FoodGroupTreeGetQuery, FoodGroupTreeView>(Arg<FoodGroupTreeGetQuery>.Is.Equal(query)));
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
            fixture.Inject(fixture.Build<FoodGroupView>().With(m => m.Parent, fixture.Create<FoodGroupIdentificationView>()).With(m => m.Children, new List<FoodGroupView>(0)).Create());

            var foodGroupTreeView = fixture.Create<FoodGroupTreeView>();
            var queryBusMock = MockRepository.GenerateMock<IQueryBus>();
            queryBusMock.Stub(m => m.Query<FoodGroupTreeGetQuery, FoodGroupTreeView>(Arg<FoodGroupTreeGetQuery>.Is.NotNull))
                .Return(foodGroupTreeView)
                .Repeat.Any();

            var foodWasteHouseholdDataService = new FoodWasteHouseholdDataService(fixture.Create<ICommandBus>(), queryBusMock, fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteHouseholdDataService, Is.Not.Null);

            var result = foodWasteHouseholdDataService.FoodGroupTreeGet(fixture.Create<FoodGroupTreeGetQuery>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(foodGroupTreeView));
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
            queryBusMock.Stub(m => m.Query<FoodGroupTreeGetQuery, FoodGroupTreeView>(Arg<FoodGroupTreeGetQuery>.Is.NotNull))
                .Throw(exception)
                .Repeat.Any();

            var foodWasteFaultExceptionBuilderMock = MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>();
            foodWasteFaultExceptionBuilderMock.Stub(m => m.Build(Arg<Exception>.Is.NotNull, Arg<string>.Is.NotNull, Arg<MethodBase>.Is.NotNull))
                .Return(fixture.Create<FaultException<FoodWasteFault>>())
                .Repeat.Any();

            var foodWasteHouseholdDataService = new FoodWasteHouseholdDataService(fixture.Create<ICommandBus>(), queryBusMock, foodWasteFaultExceptionBuilderMock);
            Assert.That(foodWasteHouseholdDataService, Is.Not.Null);

            var faultException = Assert.Throws<FaultException<FoodWasteFault>>(() => foodWasteHouseholdDataService.FoodGroupTreeGet(fixture.Create<FoodGroupTreeGetQuery>()));
            Assert.That(faultException, Is.Not.Null);
            Assert.That(faultException.Detail, Is.Not.Null);

            foodWasteFaultExceptionBuilderMock.AssertWasCalled(m => m.Build(Arg<Exception>.Is.Equal(exception), Arg<string>.Is.Equal(SoapNamespaces.FoodWasteHouseholdDataServiceName), Arg<MethodBase>.Is.NotNull));
        }

        /// <summary>
        /// Tests that PrivacyPolicyGet throws an ArgumentNullException when the query for getting the privacy policy is null.
        /// </summary>
        [Test]
        public void TestThatPrivacyPolicyGetThrowsArgumentNullExceptionIfPrivacyPolicyGetQueryIsNull()
        {
            var fixture = new Fixture();
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var foodWasteHouseholdDataService = new FoodWasteHouseholdDataService(fixture.Create<ICommandBus>(), fixture.Create<IQueryBus>(), fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteHouseholdDataService, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodWasteHouseholdDataService.PrivacyPolicyGet(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("query"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that PrivacyPolicyGet calls Query on the query bus.
        /// </summary>
        [Test]
        public void TestThatPrivacyPolicyGetCallsQueryOnQueryBus()
        {
            var fixture = new Fixture();
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var queryBusMock = MockRepository.GenerateMock<IQueryBus>();
            queryBusMock.Stub(m => m.Query<PrivacyPolicyGetQuery, StaticTextView>(Arg<PrivacyPolicyGetQuery>.Is.NotNull))
                .Return(fixture.Create<StaticTextView>())
                .Repeat.Any();

            var foodWasteHouseholdDataService = new FoodWasteHouseholdDataService(fixture.Create<ICommandBus>(), queryBusMock, fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteHouseholdDataService, Is.Not.Null);

            var query = fixture.Create<PrivacyPolicyGetQuery>();
            foodWasteHouseholdDataService.PrivacyPolicyGet(query);

            queryBusMock.AssertWasCalled(m => m.Query<PrivacyPolicyGetQuery, StaticTextView>(Arg<PrivacyPolicyGetQuery>.Is.Equal(query)));
        }

        /// <summary>
        /// Tests that PrivacyPolicyGet returns the result from the query bus.
        /// </summary>
        [Test]
        public void TestThatPrivacyPolicyGetReturnsResultFromQueryBus()
        {
            var fixture = new Fixture();
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var staticTextView = fixture.Create<StaticTextView>();
            var queryBusMock = MockRepository.GenerateMock<IQueryBus>();
            queryBusMock.Stub(m => m.Query<PrivacyPolicyGetQuery, StaticTextView>(Arg<PrivacyPolicyGetQuery>.Is.NotNull))
                .Return(staticTextView)
                .Repeat.Any();

            var foodWasteHouseholdDataService = new FoodWasteHouseholdDataService(fixture.Create<ICommandBus>(), queryBusMock, fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteHouseholdDataService, Is.Not.Null);

            var result = foodWasteHouseholdDataService.PrivacyPolicyGet(fixture.Create<PrivacyPolicyGetQuery>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(staticTextView));
        }

        /// <summary>
        /// Tests that PrivacyPolicyGet throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatPrivacyPolicyGetThrowsFaultExceptionWhenExceptionOccurs()
        {
            var fixture = new Fixture();
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));

            var exception = fixture.Create<Exception>();
            var queryBusMock = MockRepository.GenerateMock<IQueryBus>();
            queryBusMock.Stub(m => m.Query<PrivacyPolicyGetQuery, StaticTextView>(Arg<PrivacyPolicyGetQuery>.Is.NotNull))
                .Throw(exception)
                .Repeat.Any();

            var foodWasteFaultExceptionBuilderMock = MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>();
            foodWasteFaultExceptionBuilderMock.Stub(m => m.Build(Arg<Exception>.Is.NotNull, Arg<string>.Is.NotNull, Arg<MethodBase>.Is.NotNull))
                .Return(fixture.Create<FaultException<FoodWasteFault>>())
                .Repeat.Any();

            var foodWasteHouseholdDataService = new FoodWasteHouseholdDataService(fixture.Create<ICommandBus>(), queryBusMock, foodWasteFaultExceptionBuilderMock);
            Assert.That(foodWasteHouseholdDataService, Is.Not.Null);

            var faultException = Assert.Throws<FaultException<FoodWasteFault>>(() => foodWasteHouseholdDataService.PrivacyPolicyGet(fixture.Create<PrivacyPolicyGetQuery>()));
            Assert.That(faultException, Is.Not.Null);
            Assert.That(faultException.Detail, Is.Not.Null);

            foodWasteFaultExceptionBuilderMock.AssertWasCalled(m => m.Build(Arg<Exception>.Is.Equal(exception), Arg<string>.Is.Equal(SoapNamespaces.FoodWasteHouseholdDataServiceName), Arg<MethodBase>.Is.NotNull));
        }

        /// <summary>
        /// Tests that DataProviderWhoHandlesPaymentsCollectionGet throws an ArgumentNullException when the query for getting a collection of data providers who handles payments is null.
        /// </summary>
        [Test]
        public void TestThatDataProviderWhoHandlesPaymentsCollectionGetThrowsArgumentNullExceptionIfDataProviderWhoHandlesPaymentsCollectionGetQueryIsNull()
        {
            var fixture = new Fixture();
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var foodWasteHouseholdDataService = new FoodWasteHouseholdDataService(fixture.Create<ICommandBus>(), fixture.Create<IQueryBus>(), fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteHouseholdDataService, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodWasteHouseholdDataService.DataProviderWhoHandlesPaymentsCollectionGet(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("query"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that DataProviderWhoHandlesPaymentsCollectionGet calls Query on the query bus.
        /// </summary>
        [Test]
        public void TestThatDataProviderWhoHandlesPaymentsCollectionGetCallsQueryOnQueryBus()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var queryBusMock = MockRepository.GenerateMock<IQueryBus>();
            queryBusMock.Stub(m => m.Query<DataProviderWhoHandlesPaymentsCollectionGetQuery, IEnumerable<DataProviderView>>(Arg<DataProviderWhoHandlesPaymentsCollectionGetQuery>.Is.NotNull))
                .Return(fixture.CreateMany<DataProviderView>(random.Next(1, 25)).ToList())
                .Repeat.Any();

            var foodWasteHouseholdDataService = new FoodWasteHouseholdDataService(fixture.Create<ICommandBus>(), queryBusMock, fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteHouseholdDataService, Is.Not.Null);

            var query = fixture.Create<DataProviderWhoHandlesPaymentsCollectionGetQuery>();
            foodWasteHouseholdDataService.DataProviderWhoHandlesPaymentsCollectionGet(query);

            queryBusMock.AssertWasCalled(m => m.Query<DataProviderWhoHandlesPaymentsCollectionGetQuery, IEnumerable<DataProviderView>>(Arg<DataProviderWhoHandlesPaymentsCollectionGetQuery>.Is.Equal(query)));
        }

        /// <summary>
        /// Tests that DataProviderWhoHandlesPaymentsCollectionGet returns the result from the query bus.
        /// </summary>
        [Test]
        public void TestThatDataProviderWhoHandlesPaymentsCollectionGetReturnsResultFromQueryBus()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var dataProviderViewCollection = fixture.CreateMany<DataProviderView>(random.Next(1, 25)).ToList();
            var queryBusMock = MockRepository.GenerateMock<IQueryBus>();
            queryBusMock.Stub(m => m.Query<DataProviderWhoHandlesPaymentsCollectionGetQuery, IEnumerable<DataProviderView>>(Arg<DataProviderWhoHandlesPaymentsCollectionGetQuery>.Is.NotNull))
                .Return(dataProviderViewCollection)
                .Repeat.Any();

            var foodWasteHouseholdDataService = new FoodWasteHouseholdDataService(fixture.Create<ICommandBus>(), queryBusMock, fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteHouseholdDataService, Is.Not.Null);

            var result = foodWasteHouseholdDataService.DataProviderWhoHandlesPaymentsCollectionGet(fixture.Create<DataProviderWhoHandlesPaymentsCollectionGetQuery>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EqualTo(dataProviderViewCollection));
        }

        /// <summary>
        /// Tests that DataProviderWhoHandlesPaymentsCollectionGet throws an FaultException if an error occurs.
        /// </summary>
        [Test]
        public void TestThatDataProviderWhoHandlesPaymentsCollectionGetThrowsFaultExceptionWhenExceptionOccurs()
        {
            var fixture = new Fixture();
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));

            var exception = fixture.Create<Exception>();
            var queryBusMock = MockRepository.GenerateMock<IQueryBus>();
            queryBusMock.Stub(m => m.Query<DataProviderWhoHandlesPaymentsCollectionGetQuery, IEnumerable<DataProviderView>>(Arg<DataProviderWhoHandlesPaymentsCollectionGetQuery>.Is.NotNull))
                .Throw(exception)
                .Repeat.Any();

            var foodWasteFaultExceptionBuilderMock = MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>();
            foodWasteFaultExceptionBuilderMock.Stub(m => m.Build(Arg<Exception>.Is.NotNull, Arg<string>.Is.NotNull, Arg<MethodBase>.Is.NotNull))
                .Return(fixture.Create<FaultException<FoodWasteFault>>())
                .Repeat.Any();

            var foodWasteHouseholdDataService = new FoodWasteHouseholdDataService(fixture.Create<ICommandBus>(), queryBusMock, foodWasteFaultExceptionBuilderMock);
            Assert.That(foodWasteHouseholdDataService, Is.Not.Null);

            var faultException = Assert.Throws<FaultException<FoodWasteFault>>(() => foodWasteHouseholdDataService.DataProviderWhoHandlesPaymentsCollectionGet(fixture.Create<DataProviderWhoHandlesPaymentsCollectionGetQuery>()));
            Assert.That(faultException, Is.Not.Null);
            Assert.That(faultException.Detail, Is.Not.Null);

            foodWasteFaultExceptionBuilderMock.AssertWasCalled(m => m.Build(Arg<Exception>.Is.Equal(exception), Arg<string>.Is.Equal(SoapNamespaces.FoodWasteHouseholdDataServiceName), Arg<MethodBase>.Is.NotNull));
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

            var foodWasteHouseholdDataService = new FoodWasteHouseholdDataService(fixture.Create<ICommandBus>(), fixture.Create<IQueryBus>(), fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteHouseholdDataService, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodWasteHouseholdDataService.TranslationInfoGetAll(null));
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

            var foodWasteHouseholdDataService = new FoodWasteHouseholdDataService(fixture.Create<ICommandBus>(), queryBusMock, fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteHouseholdDataService, Is.Not.Null);

            var query = fixture.Create<TranslationInfoCollectionGetQuery>();
            foodWasteHouseholdDataService.TranslationInfoGetAll(query);

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

            var foodWasteHouseholdDataService = new FoodWasteHouseholdDataService(fixture.Create<ICommandBus>(), queryBusMock, fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>());
            Assert.That(foodWasteHouseholdDataService, Is.Not.Null);

            var result = foodWasteHouseholdDataService.TranslationInfoGetAll(fixture.Create<TranslationInfoCollectionGetQuery>());
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

            var foodWasteHouseholdDataService = new FoodWasteHouseholdDataService(fixture.Create<ICommandBus>(), queryBusMock, foodWasteFaultExceptionBuilderMock);
            Assert.That(foodWasteHouseholdDataService, Is.Not.Null);

            var faultException = Assert.Throws<FaultException<FoodWasteFault>>(() => foodWasteHouseholdDataService.TranslationInfoGetAll(fixture.Create<TranslationInfoCollectionGetQuery>()));
            Assert.That(faultException, Is.Not.Null);
            Assert.That(faultException.Detail, Is.Not.Null);

            foodWasteFaultExceptionBuilderMock.AssertWasCalled(m => m.Build(Arg<Exception>.Is.Equal(exception), Arg<string>.Is.Equal(SoapNamespaces.FoodWasteHouseholdDataServiceName), Arg<MethodBase>.Is.NotNull));
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
