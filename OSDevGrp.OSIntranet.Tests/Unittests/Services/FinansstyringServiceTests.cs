using System;
using System.Collections.Generic;
using System.ServiceModel;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf;
using OSDevGrp.OSIntranet.Contracts.Commands;
using OSDevGrp.OSIntranet.Contracts.Faults;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Responses;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Services.Implementations;
using NUnit.Framework;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Services
{
    /// <summary>
    /// Tester service til finansstyring.
    /// </summary>
    public class FinansstyringServiceTests
    {
        /// <summary>
        /// Tester, at service til finansstyring kan hostes.
        /// </summary>
        [Test]
        public void TestAtFinansstyringServiceKanHostes()
        {
            var uri = new Uri("http://localhost:7000/OSIntranet/");
            var host = new ServiceHost(typeof (FinansstyringService), new [] {uri});
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
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis CommandBus er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisCommandBusErNull()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            Assert.Throws<ArgumentNullException>(() => new FinansstyringService(null, queryBus));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis QueryBus er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisQueryBusErNull()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            Assert.Throws<ArgumentNullException>(() => new FinansstyringService(commandBus, null));
        }

        /// <summary>
        /// Tester, at KontoplanGet kalder QueryBus.
        /// </summary>
        [Test]
        public void TestAtKontoplanGetKalderQueryBus()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new KontoplanGetQuery();
            service.KontoplanGet(query);
            queryBus.AssertWasCalled(
                m => m.Query<KontoplanGetQuery, IEnumerable<KontoplanView>>(Arg<KontoplanGetQuery>.Is.Anything));
        }

        /// <summary>
        /// Tester, at KontoplanGet kaster en IntranetRepositoryFault.
        /// </summary>
        [Test]
        public void TestAtKontoplanGetKasterIntranetRepositoryFault()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m => m.Query<KontoplanGetQuery, IEnumerable<KontoplanView>>(Arg<KontoplanGetQuery>.Is.Anything)).Throw(
                    new IntranetRepositoryException("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new KontoplanGetQuery();
            Assert.Throws<FaultException<IntranetRepositoryFault>>(() => service.KontoplanGet(query));
        }

        /// <summary>
        /// Tester, at KontoplanGet kaster en IntranetBusinessFault.
        /// </summary>
        [Test]
        public void TestAtKontoplanGetKasterIntranetBusinessFault()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m => m.Query<KontoplanGetQuery, IEnumerable<KontoplanView>>(Arg<KontoplanGetQuery>.Is.Anything)).Throw(
                    new IntranetBusinessException("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new KontoplanGetQuery();
            Assert.Throws<FaultException<IntranetBusinessFault>>(() => service.KontoplanGet(query));
        }

        /// <summary>
        /// Tester, at KontoplanGet kaster en IntranetSystemFault.
        /// </summary>
        [Test]
        public void TestAtKontoplanGetKasterIntranetSystemFault()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m => m.Query<KontoplanGetQuery, IEnumerable<KontoplanView>>(Arg<KontoplanGetQuery>.Is.Anything)).Throw(
                    new IntranetSystemException("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new KontoplanGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.KontoplanGet(query));
        }

        /// <summary>
        /// Tester, at KontoplanGet kaster en IntranetSystemFault ved en unhandled exception.
        /// </summary>
        [Test]
        public void TestAtKontoplanGetKasterIntranetSystemFaultVedUnhandledException()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m => m.Query<KontoplanGetQuery, IEnumerable<KontoplanView>>(Arg<KontoplanGetQuery>.Is.Anything)).Throw(
                    new Exception("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new KontoplanGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.KontoplanGet(query));
        }

        /// <summary>
        /// Tester, at KontoGet kalder QueryBus.
        /// </summary>
        [Test]
        public void TestAtKontoGetKalderQueryBus()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new KontoGetQuery();
            service.KontoGet(query);
            queryBus.AssertWasCalled(m => m.Query<KontoGetQuery, KontoView>(Arg<KontoGetQuery>.Is.Anything));
        }

        /// <summary>
        /// Tester, at KontoGet kaster en IntranetRepositoryFault.
        /// </summary>
        [Test]
        public void TestAtKontoGetKasterIntranetRepositoryFault()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<KontoGetQuery, KontoView>(Arg<KontoGetQuery>.Is.Anything)).
                Throw(new IntranetRepositoryException("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new KontoGetQuery();
            Assert.Throws<FaultException<IntranetRepositoryFault>>(() => service.KontoGet(query));
        }

        /// <summary>
        /// Tester, at KontoGet kaster en IntranetBusinessFault.
        /// </summary>
        [Test]
        public void TestAtKontoGetKasterIntranetBusinessFault()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<KontoGetQuery, KontoView>(Arg<KontoGetQuery>.Is.Anything)).
                Throw(new IntranetBusinessException("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new KontoGetQuery();
            Assert.Throws<FaultException<IntranetBusinessFault>>(() => service.KontoGet(query));
        }

        /// <summary>
        /// Tester, at KontoGet kaster en IntranetSystemFault.
        /// </summary>
        [Test]
        public void TestAtKontoGetKasterIntranetSystemFault()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<KontoGetQuery, KontoView>(Arg<KontoGetQuery>.Is.Anything))
                .Throw(new IntranetSystemException("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new KontoGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.KontoGet(query));
        }

        /// <summary>
        /// Tester, at KontoGet kaster en IntranetSystemFault ved en unhandled exception.
        /// </summary>
        [Test]
        public void TestAtKontoGetKasterIntranetSystemFaultVedUnhandledException()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<KontoGetQuery, KontoView>(Arg<KontoGetQuery>.Is.Anything))
                .Throw(new Exception("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new KontoGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.KontoGet(query));
        }

        /// <summary>
        /// Tester, at BudgetkontoplanGet kalder QueryBus.
        /// </summary>
        [Test]
        public void TestAtBudgetkontoplanGetKalderQueryBus()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new BudgetkontoplanGetQuery();
            service.BudgetkontoplanGet(query);
            queryBus.AssertWasCalled(
                m =>
                m.Query<BudgetkontoplanGetQuery, IEnumerable<BudgetkontoplanView>>(
                    Arg<BudgetkontoplanGetQuery>.Is.Anything));
        }

        /// <summary>
        /// Tester, at BudgetkontoplanGet kaster en IntranetRepositoryFault.
        /// </summary>
        [Test]
        public void TestAtBudgetkontoplanGetKasterIntranetRepositoryFault()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m =>
                m.Query<BudgetkontoplanGetQuery, IEnumerable<BudgetkontoplanView>>(
                    Arg<BudgetkontoplanGetQuery>.Is.Anything)).
                Throw(new IntranetRepositoryException("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new BudgetkontoplanGetQuery();
            Assert.Throws<FaultException<IntranetRepositoryFault>>(() => service.BudgetkontoplanGet(query));
        }

        /// <summary>
        /// Tester, at BudgetkontoplanGet kaster en IntranetBusinessFault.
        /// </summary>
        [Test]
        public void TestAtBudgetkontoplanGetKasterIntranetBusinessFault()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m =>
                m.Query<BudgetkontoplanGetQuery, IEnumerable<BudgetkontoplanView>>(
                    Arg<BudgetkontoplanGetQuery>.Is.Anything)).
                Throw(new IntranetBusinessException("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new BudgetkontoplanGetQuery();
            Assert.Throws<FaultException<IntranetBusinessFault>>(() => service.BudgetkontoplanGet(query));
        }

        /// <summary>
        /// Tester, at BudgetkontoplanGet kaster en IntranetSystemFault.
        /// </summary>
        [Test]
        public void TestAtBudgetkontoplanGetKasterIntranetSystemFault()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m =>
                m.Query<BudgetkontoplanGetQuery, IEnumerable<BudgetkontoplanView>>(
                    Arg<BudgetkontoplanGetQuery>.Is.Anything)).
                Throw(new IntranetSystemException("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new BudgetkontoplanGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.BudgetkontoplanGet(query));
        }

        /// <summary>
        /// Tester, at BudgetkontoplanGet kaster en IntranetSystemFault ved en unhandled exception.
        /// </summary>
        [Test]
        public void TestAtBudgetkontoplanGetKasterIntranetSystemFaultVedUnhandledException()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m =>
                m.Query<BudgetkontoplanGetQuery, IEnumerable<BudgetkontoplanView>>(
                    Arg<BudgetkontoplanGetQuery>.Is.Anything)).
                Throw(new Exception("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new BudgetkontoplanGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.BudgetkontoplanGet(query));
        }

        /// <summary>
        /// Tester, at BudgetkontoGet kalder QueryBus.
        /// </summary>
        [Test]
        public void TestAtBudgetkontoGetKalderQueryBus()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new BudgetkontoGetQuery();
            service.BudgetkontoGet(query);
            queryBus.AssertWasCalled(
                m => m.Query<BudgetkontoGetQuery, BudgetkontoView>(Arg<BudgetkontoGetQuery>.Is.Anything));
        }

        /// <summary>
        /// Tester, at BudgetkontoGet kaster en IntranetRepositoryFault.
        /// </summary>
        [Test]
        public void TestAtBudgetkontoGetKasterIntranetRepositoryFault()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<BudgetkontoGetQuery, BudgetkontoView>(Arg<BudgetkontoGetQuery>.Is.Anything)).
                Throw(new IntranetRepositoryException("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new BudgetkontoGetQuery();
            Assert.Throws<FaultException<IntranetRepositoryFault>>(() => service.BudgetkontoGet(query));
        }

        /// <summary>
        /// Tester, at BudgetkontoGet kaster en IntranetBusinessFault.
        /// </summary>
        [Test]
        public void TestAtBudgetkontoGetKasterIntranetBusinessFault()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<BudgetkontoGetQuery, BudgetkontoView>(Arg<BudgetkontoGetQuery>.Is.Anything)).
                Throw(new IntranetBusinessException("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new BudgetkontoGetQuery();
            Assert.Throws<FaultException<IntranetBusinessFault>>(() => service.BudgetkontoGet(query));
        }

        /// <summary>
        /// Tester, at BudgetkontoGet kaster en IntranetSystemFault.
        /// </summary>
        [Test]
        public void TestAtBudgetkontoGetKasterIntranetSystemFault()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<BudgetkontoGetQuery, BudgetkontoView>(Arg<BudgetkontoGetQuery>.Is.Anything)).
                Throw(new IntranetSystemException("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new BudgetkontoGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.BudgetkontoGet(query));
        }

        /// <summary>
        /// Tester, at BudgetkontoGet kaster en IntranetSystemFault ved en unhandled exception.
        /// </summary>
        [Test]
        public void TestAtBudgetkontoGetKasterIntranetSystemFaultVedUnhandledException()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<BudgetkontoGetQuery, BudgetkontoView>(Arg<BudgetkontoGetQuery>.Is.Anything)).
                Throw(new Exception("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new BudgetkontoGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.BudgetkontoGet(query));
        }

        /// <summary>
        /// Tester, at BogføringerGet kalder QueryBus.
        /// </summary>
        [Test]
        public void TestAtBogføringerGetKalderQueryBus()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new BogføringerGetQuery();
            service.BogføringerGet(query);
            queryBus.AssertWasCalled(m => m.Query<BogføringerGetQuery, IEnumerable<BogføringslinjeView>>(Arg<BogføringerGetQuery>.Is.Anything));
        }

        /// <summary>
        /// Tester, at BogføringerGet kaster en IntranetRepositoryFault.
        /// </summary>
        [Test]
        public void TestAtBogføringerGetKasterIntranetRepositoryFault()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<BogføringerGetQuery, IEnumerable<BogføringslinjeView>>(Arg<BogføringerGetQuery>.Is.Anything))
                .Throw(new IntranetRepositoryException("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new BogføringerGetQuery();
            Assert.Throws<FaultException<IntranetRepositoryFault>>(() => service.BogføringerGet(query));
        }

        /// <summary>
        /// Tester, at BogføringerGet kaster en IntranetBusinessFault.
        /// </summary>
        [Test]
        public void TestAtBogføringerGetKasterIntranetBusinessFault()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<BogføringerGetQuery, IEnumerable<BogføringslinjeView>>(Arg<BogføringerGetQuery>.Is.Anything))
                .Throw(new IntranetBusinessException("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new BogføringerGetQuery();
            Assert.Throws<FaultException<IntranetBusinessFault>>(() => service.BogføringerGet(query));
        }

        /// <summary>
        /// Tester, at BogføringerGet kaster en IntranetSystemFault.
        /// </summary>
        [Test]
        public void TestAtBogføringerGetKasterIntranetSystemFault()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<BogføringerGetQuery, IEnumerable<BogføringslinjeView>>(Arg<BogføringerGetQuery>.Is.Anything))
                .Throw(new IntranetSystemException("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new BogføringerGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.BogføringerGet(query));
        }

        /// <summary>
        /// Tester, at BogføringerGet kaster en IntranetSystemFault ved en unhandled exception.
        /// </summary>
        [Test]
        public void TestAtBogføringerGetKasterIntranetSystemFaultVedUnhandledException()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<BogføringerGetQuery, IEnumerable<BogføringslinjeView>>(Arg<BogføringerGetQuery>.Is.Anything))
                .Throw(new Exception("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new BogføringerGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.BogføringerGet(query));
        }

        /// <summary>
        /// Tester, at BogføringslinjeOpret kalder CommandBus.
        /// </summary>
        [Test]
        public void TestAtBogføringslinjeOpretKalderCommandBus()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            var service = new FinansstyringService(commandBus, queryBus);
            var command = new BogføringslinjeOpretCommand();
            service.BogføringslinjeOpret(command);
            commandBus.AssertWasCalled(m => m.Publish<BogføringslinjeOpretCommand, BogføringslinjeOpretResponse>(Arg<BogføringslinjeOpretCommand>.Is.Anything));
        }

        /// <summary>
        /// Tester, at BogføringslinjeOpret kaster en IntranetRepositoryFault.
        /// </summary>
        [Test]
        public void TestAtBogføringslinjeOpretKasterIntranetRepositoryFault()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            commandBus.Expect(m => m.Publish<BogføringslinjeOpretCommand, BogføringslinjeOpretResponse>(Arg<BogføringslinjeOpretCommand>.Is.Anything))
                .Throw(new IntranetRepositoryException("Test", new Exception("Test")));
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            var service = new FinansstyringService(commandBus, queryBus);
            var command = new BogføringslinjeOpretCommand();
            Assert.Throws<FaultException<IntranetRepositoryFault>>(() => service.BogføringslinjeOpret(command));
        }

        /// <summary>
        /// Tester, at BogføringslinjeOpret kaster en IntranetBusinessFault.
        /// </summary>
        [Test]
        public void TestAtBogføringslinjeOpretKasterIntranetBusinessFault()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            commandBus.Expect(m => m.Publish<BogføringslinjeOpretCommand, BogføringslinjeOpretResponse>(Arg<BogføringslinjeOpretCommand>.Is.Anything))
                .Throw(new IntranetBusinessException("Test", new Exception("Test")));
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            var service = new FinansstyringService(commandBus, queryBus);
            var command = new BogføringslinjeOpretCommand();
            Assert.Throws<FaultException<IntranetBusinessFault>>(() => service.BogføringslinjeOpret(command));
        }

        /// <summary>
        /// Tester, at BogføringslinjeOpret kaster en IntranetSystemFault.
        /// </summary>
        [Test]
        public void TestAtBogføringslinjeOpretKasterIntranetSystemFault()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            commandBus.Expect(m => m.Publish<BogføringslinjeOpretCommand, BogføringslinjeOpretResponse>(Arg<BogføringslinjeOpretCommand>.Is.Anything))
                .Throw(new IntranetSystemException("Test", new Exception("Test")));
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            var service = new FinansstyringService(commandBus, queryBus);
            var command = new BogføringslinjeOpretCommand();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.BogføringslinjeOpret(command));
        }

        /// <summary>
        /// Tester, at BogføringslinjeOpret kaster en IntranetSystemFault ved en unhandled exception.
        /// </summary>
        [Test]
        public void TestAtBogføringslinjeOpretKasterIntranetSystemFaultVedUnhandledException()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            commandBus.Expect(m => m.Publish<BogføringslinjeOpretCommand, BogføringslinjeOpretResponse>(Arg<BogføringslinjeOpretCommand>.Is.Anything))
                .Throw(new Exception("Test", new Exception("Test")));
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            var service = new FinansstyringService(commandBus, queryBus);
            var command = new BogføringslinjeOpretCommand();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.BogføringslinjeOpret(command));
        }
    }
}