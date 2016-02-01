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
        /// <summary>
        /// Tests that service which can access and modify data in a house hold can be hosted.
        /// </summary>
        [Test]
        public void TestThatFoodWasteHouseholdDataServiceCanBeHosted()
        {
            var uri = new Uri("http://localhost:7000/OSIntranet/");
            var host = new ServiceHost(typeof(FoodWasteHouseholdDataService), new[] { uri });
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
            var fixture = new Fixture();
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new FoodWasteHouseholdDataService(null, fixture.Create<IQueryBus>(), fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("commandBus"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the query bus is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionIfQueryBusIsNull()
        {
            var fixture = new Fixture();
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));
            fixture.Customize<IFaultExceptionBuilder<FoodWasteFault>>(e => e.FromFactory(() => MockRepository.GenerateMock<IFaultExceptionBuilder<FoodWasteFault>>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new FoodWasteHouseholdDataService(fixture.Create<ICommandBus>(), null, fixture.Create<IFaultExceptionBuilder<FoodWasteFault>>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("queryBus"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the query bus is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionIfFaultExceptionBuilderIsNull()
        {
            var fixture = new Fixture();
            fixture.Customize<ICommandBus>(e => e.FromFactory(() => MockRepository.GenerateMock<ICommandBus>()));
            fixture.Customize<IQueryBus>(e => e.FromFactory(() => MockRepository.GenerateMock<IQueryBus>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new FoodWasteHouseholdDataService(fixture.Create<ICommandBus>(), fixture.Create<IQueryBus>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("foodWasteFaultExceptionBuilder"));
            Assert.That(exception.InnerException, Is.Null);
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
        public void TestThatHouseholdMemberAcceptPrivacyPolicyThrowsArgumentNullExceptionIfHouseholdMemberAcceptPrivacyPolicyIsNull()
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
    }
}
