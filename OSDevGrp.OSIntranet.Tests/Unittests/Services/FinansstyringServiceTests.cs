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
        /// Tester, at RegnskabslisteGet kalder QueryBus.
        /// </summary>
        [Test]
        public void TestAtRegnskabslisteGetKalderQueryBus()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new RegnskabslisteGetQuery();
            service.RegnskabslisteGet(query);
            queryBus.AssertWasCalled(
                m =>
                m.Query<RegnskabslisteGetQuery, IEnumerable<RegnskabslisteView>>(Arg<RegnskabslisteGetQuery>.Is.Anything));
        }

        /// <summary>
        /// Tester, at RegnskabslisteGet kaster en IntranetRepositoryFault.
        /// </summary>
        [Test]
        public void TestAtRegnskabslisteGetKasterIntranetRepositoryFault()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m =>
                m.Query<RegnskabslisteGetQuery, IEnumerable<RegnskabslisteView>>(Arg<RegnskabslisteGetQuery>.Is.Anything))
                .Throw(new IntranetRepositoryException("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new RegnskabslisteGetQuery();
            Assert.Throws<FaultException<IntranetRepositoryFault>>(() => service.RegnskabslisteGet(query));
        }

        /// <summary>
        /// Tester, at RegnskabslisteGet kaster en IntranetBusinessFault.
        /// </summary>
        [Test]
        public void TestAtRegnskabslisteGetKasterIntranetBusinessFault()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m =>
                m.Query<RegnskabslisteGetQuery, IEnumerable<RegnskabslisteView>>(Arg<RegnskabslisteGetQuery>.Is.Anything))
                .Throw(new IntranetBusinessException("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new RegnskabslisteGetQuery();
            Assert.Throws<FaultException<IntranetBusinessFault>>(() => service.RegnskabslisteGet(query));
        }

        /// <summary>
        /// Tester, at RegnskabslisteGet kaster en IntranetSystemFault.
        /// </summary>
        [Test]
        public void TestAtRegnskabslisteGetKasterIntranetSystemFault()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m =>
                m.Query<RegnskabslisteGetQuery, IEnumerable<RegnskabslisteView>>(Arg<RegnskabslisteGetQuery>.Is.Anything))
                .Throw(new IntranetSystemException("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new RegnskabslisteGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.RegnskabslisteGet(query));
        }

        /// <summary>
        /// Tester, at RegnskabslisteGet kaster en IntranetSystemFault ved en unhandled exception.
        /// </summary>
        [Test]
        public void TestAtRegnskabslisteGetKasterIntranetSystemFaultVedUnhandledException()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m =>
                m.Query<RegnskabslisteGetQuery, IEnumerable<RegnskabslisteView>>(Arg<RegnskabslisteGetQuery>.Is.Anything))
                .Throw(new Exception("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new RegnskabslisteGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.RegnskabslisteGet(query));
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
        /// Tester, at DebitorlisteGet kalder QueryBus.
        /// </summary>
        [Test]
        public void TestAtDebitorlisteGetKalderQueryBus()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new DebitorlisteGetQuery();
            service.DebitorlisteGet(query);
            queryBus.AssertWasCalled(
                m => m.Query<DebitorlisteGetQuery, IEnumerable<DebitorlisteView>>(Arg<DebitorlisteGetQuery>.Is.Anything));
        }

        /// <summary>
        /// Tester, at DebitorlisteGet kaster en IntranetRepositoryFault.
        /// </summary>
        [Test]
        public void TestAtDebitorlisteGetKasterIntranetRepositoryFault()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m => m.Query<DebitorlisteGetQuery, IEnumerable<DebitorlisteView>>(Arg<DebitorlisteGetQuery>.Is.Anything))
                .Throw(new IntranetRepositoryException("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new DebitorlisteGetQuery();
            Assert.Throws<FaultException<IntranetRepositoryFault>>(() => service.DebitorlisteGet(query));
        }

        /// <summary>
        /// Tester, at DebitorlisteGet kaster en IntranetBusinessFault.
        /// </summary>
        [Test]
        public void TestAtDebitorlisteGetKasterIntranetBusinessFault()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m => m.Query<DebitorlisteGetQuery, IEnumerable<DebitorlisteView>>(Arg<DebitorlisteGetQuery>.Is.Anything))
                .Throw(new IntranetBusinessException("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new DebitorlisteGetQuery();
            Assert.Throws<FaultException<IntranetBusinessFault>>(() => service.DebitorlisteGet(query));
        }

        /// <summary>
        /// Tester, at DebitorlisteGet kaster en IntranetSystemFault.
        /// </summary>
        [Test]
        public void TestAtDebitorlisteGetKasterIntranetSystemFault()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m => m.Query<DebitorlisteGetQuery, IEnumerable<DebitorlisteView>>(Arg<DebitorlisteGetQuery>.Is.Anything))
                .Throw(new IntranetSystemException("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new DebitorlisteGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.DebitorlisteGet(query));
        }

        /// <summary>
        /// Tester, at DebitorlisteGet kaster en IntranetSystemFault ved en unhandled exception.
        /// </summary>
        [Test]
        public void TestAtDebitorlisteGetKasterIntranetSystemFaultVedUnhandledException()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m => m.Query<DebitorlisteGetQuery, IEnumerable<DebitorlisteView>>(Arg<DebitorlisteGetQuery>.Is.Anything))
                .Throw(new Exception("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new DebitorlisteGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.DebitorlisteGet(query));
        }

        /// <summary>
        /// Tester, at DebitorGet kalder QueryBus.
        /// </summary>
        [Test]
        public void TestAtDebitorGetKalderQueryBus()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new DebitorGetQuery();
            service.DebitorGet(query);
            queryBus.AssertWasCalled(m => m.Query<DebitorGetQuery, DebitorView>(Arg<DebitorGetQuery>.Is.Anything));
        }

        /// <summary>
        /// Tester, at DebitorGet kaster en IntranetRepositoryFault.
        /// </summary>
        [Test]
        public void TestAtDebitorGetKasterIntranetRepositoryFault()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<DebitorGetQuery, DebitorView>(Arg<DebitorGetQuery>.Is.Anything)).Throw(
                new IntranetRepositoryException("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new DebitorGetQuery();
            Assert.Throws<FaultException<IntranetRepositoryFault>>(() => service.DebitorGet(query));
        }

        /// <summary>
        /// Tester, at DebitorGet kaster en IntranetBusinessFault.
        /// </summary>
        [Test]
        public void TestAtDebitorGetKasterIntranetBusinessFault()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<DebitorGetQuery, DebitorView>(Arg<DebitorGetQuery>.Is.Anything)).Throw(
                new IntranetBusinessException("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new DebitorGetQuery();
            Assert.Throws<FaultException<IntranetBusinessFault>>(() => service.DebitorGet(query));
        }

        /// <summary>
        /// Tester, at DebitorGet kaster en IntranetSystemFault.
        /// </summary>
        [Test]
        public void TestAtDebitorGetKasterIntranetSystemFault()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<DebitorGetQuery, DebitorView>(Arg<DebitorGetQuery>.Is.Anything)).Throw(
                new IntranetSystemException("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new DebitorGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.DebitorGet(query));
        }

        /// <summary>
        /// Tester, at DebitorGet kaster en IntranetSystemFault ved en unhandled exception.
        /// </summary>
        [Test]
        public void TestAtDebitorGetKasterIntranetSystemFaultVedUnhandledException()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<DebitorGetQuery, DebitorView>(Arg<DebitorGetQuery>.Is.Anything)).Throw(
                new Exception("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new DebitorGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.DebitorGet(query));
        }

        /// <summary>
        /// Tester, at KreditorlisteGet kalder QueryBus.
        /// </summary>
        [Test]
        public void TestAtKreditorlisteGetKalderQueryBus()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new KreditorlisteGetQuery();
            service.KreditorlisteGet(query);
            queryBus.AssertWasCalled(
                m =>
                m.Query<KreditorlisteGetQuery, IEnumerable<KreditorlisteView>>(Arg<KreditorlisteGetQuery>.Is.Anything));
        }

        /// <summary>
        /// Tester, at KreditorlisteGet kaster en IntranetRepositoryFault.
        /// </summary>
        [Test]
        public void TestAtKreditorlisteGetKasterIntranetRepositoryFault()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m =>
                m.Query<KreditorlisteGetQuery, IEnumerable<KreditorlisteView>>(Arg<KreditorlisteGetQuery>.Is.Anything)).
                Throw(new IntranetRepositoryException("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new KreditorlisteGetQuery();
            Assert.Throws<FaultException<IntranetRepositoryFault>>(() => service.KreditorlisteGet(query));
        }

        /// <summary>
        /// Tester, at KreditorlisteGet kaster en IntranetBusinessFault.
        /// </summary>
        [Test]
        public void TestAtKreditorlisteGetKasterIntranetBusinessFault()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m =>
                m.Query<KreditorlisteGetQuery, IEnumerable<KreditorlisteView>>(Arg<KreditorlisteGetQuery>.Is.Anything)).
                Throw(new IntranetBusinessException("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new KreditorlisteGetQuery();
            Assert.Throws<FaultException<IntranetBusinessFault>>(() => service.KreditorlisteGet(query));
        }

        /// <summary>
        /// Tester, at KreditorlisteGet kaster en IntranetSystemFault.
        /// </summary>
        [Test]
        public void TestAtKreditorlisteGetKasterIntranetSystemFault()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m =>
                m.Query<KreditorlisteGetQuery, IEnumerable<KreditorlisteView>>(Arg<KreditorlisteGetQuery>.Is.Anything)).
                Throw(new IntranetSystemException("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new KreditorlisteGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.KreditorlisteGet(query));
        }

        /// <summary>
        /// Tester, at KreditorlisteGet kaster en IntranetSystemFault ved en unhandled exception.
        /// </summary>
        [Test]
        public void TestAtKreditorlisteGetKasterIntranetSystemFaultVedUnhandledException()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m =>
                m.Query<KreditorlisteGetQuery, IEnumerable<KreditorlisteView>>(Arg<KreditorlisteGetQuery>.Is.Anything)).
                Throw(new Exception("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new KreditorlisteGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.KreditorlisteGet(query));
        }

        /// <summary>
        /// Tester, at KreditorGet kalder QueryBus.
        /// </summary>
        [Test]
        public void TestAtKreditorGetKalderQueryBus()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new KreditorGetQuery();
            service.KreditorGet(query);
            queryBus.AssertWasCalled(m => m.Query<KreditorGetQuery, KreditorView>(Arg<KreditorGetQuery>.Is.Anything));
        }

        /// <summary>
        /// Tester, at KreditorGet kaster en IntranetRepositoryFault.
        /// </summary>
        [Test]
        public void TestAtKreditorGetKasterIntranetRepositoryFault()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<KreditorGetQuery, KreditorView>(Arg<KreditorGetQuery>.Is.Anything)).Throw(
                new IntranetRepositoryException("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new KreditorGetQuery();
            Assert.Throws<FaultException<IntranetRepositoryFault>>(() => service.KreditorGet(query));
        }

        /// <summary>
        /// Tester, at KreditorGet kaster en IntranetBusinessFault.
        /// </summary>
        [Test]
        public void TestAtKreditorGetKasterIntranetBusinessFault()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<KreditorGetQuery, KreditorView>(Arg<KreditorGetQuery>.Is.Anything)).Throw(
                new IntranetBusinessException("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new KreditorGetQuery();
            Assert.Throws<FaultException<IntranetBusinessFault>>(() => service.KreditorGet(query));
        }

        /// <summary>
        /// Tester, at KreditorGet kaster en IntranetSystemFault.
        /// </summary>
        [Test]
        public void TestAtKreditorGetKasterIntranetSystemFault()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<KreditorGetQuery, KreditorView>(Arg<KreditorGetQuery>.Is.Anything)).Throw(
                new IntranetSystemException("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new KreditorGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.KreditorGet(query));
        }

        /// <summary>
        /// Tester, at KreditorGet kaster en IntranetSystemFault ved en unhandled exception.
        /// </summary>
        [Test]
        public void TestAtKreditorGetKasterIntranetSystemFaultVedUnhandledException()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<KreditorGetQuery, KreditorView>(Arg<KreditorGetQuery>.Is.Anything)).Throw(
                new Exception("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new KreditorGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.KreditorGet(query));
        }

        /// <summary>
        /// Tester, at AdressekontolisteGet kalder QueryBus.
        /// </summary>
        [Test]
        public void TestAtAdressekontolisteGetKalderQueryBus()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new AdressekontolisteGetQuery();
            service.AdressekontolisteGet(query);
            queryBus.AssertWasCalled(m => m.Query<AdressekontolisteGetQuery, IEnumerable<AdressekontolisteView>>(Arg<AdressekontolisteGetQuery>.Is.Anything));
        }

        /// <summary>
        /// Tester, at AdressekontolisteGet kaster en IntranetRepositoryFault.
        /// </summary>
        [Test]
        public void TestAtAdressekontolisteGetKasterIntranetRepositoryFault()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<AdressekontolisteGetQuery, IEnumerable<AdressekontolisteView>>(Arg<AdressekontolisteGetQuery>.Is.Anything))
                .Throw(new IntranetRepositoryException("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new AdressekontolisteGetQuery();
            Assert.Throws<FaultException<IntranetRepositoryFault>>(() => service.AdressekontolisteGet(query));
        }

        /// <summary>
        /// Tester, at AdressekontolisteGet kaster en IntranetBusinessFault.
        /// </summary>
        [Test]
        public void TestAtAdressekontolisteGetKasterIntranetBusinessFault()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<AdressekontolisteGetQuery, IEnumerable<AdressekontolisteView>>(Arg<AdressekontolisteGetQuery>.Is.Anything))
                .Throw(new IntranetBusinessException("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new AdressekontolisteGetQuery();
            Assert.Throws<FaultException<IntranetBusinessFault>>(() => service.AdressekontolisteGet(query));
        }

        /// <summary>
        /// Tester, at AdressekontolisteGet kaster en IntranetSystemFault.
        /// </summary>
        [Test]
        public void TestAtAdressekontolisteGetKasterIntranetSystemFault()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<AdressekontolisteGetQuery, IEnumerable<AdressekontolisteView>>(Arg<AdressekontolisteGetQuery>.Is.Anything))
                .Throw(new IntranetSystemException("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new AdressekontolisteGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.AdressekontolisteGet(query));
        }

        /// <summary>
        /// Tester, at AdressekontolisteGet kaster en IntranetSystemFault ved en unhandled exception.
        /// </summary>
        [Test]
        public void TestAtAdressekontolisteGetKasterIntranetSystemFaultVedUnhandledException()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<AdressekontolisteGetQuery, IEnumerable<AdressekontolisteView>>(Arg<AdressekontolisteGetQuery>.Is.Anything))
                .Throw(new Exception("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new AdressekontolisteGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.AdressekontolisteGet(query));
        }

        /// <summary>
        /// Tester, at AdressekontoGet kalder QueryBus.
        /// </summary>
        [Test]
        public void TestAtAdressekontoGetKalderQueryBus()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new AdressekontoGetQuery();
            service.AdressekontoGet(query);
            queryBus.AssertWasCalled(m => m.Query<AdressekontoGetQuery, AdressekontoView>(Arg<AdressekontoGetQuery>.Is.Anything));
        }

        /// <summary>
        /// Tester, at AdressekontoGet kaster en IntranetRepositoryFault.
        /// </summary>
        [Test]
        public void TestAtAdressekontoGetKasterIntranetRepositoryFault()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<AdressekontoGetQuery, AdressekontoView>(Arg<AdressekontoGetQuery>.Is.Anything))
                .Throw(new IntranetRepositoryException("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new AdressekontoGetQuery();
            Assert.Throws<FaultException<IntranetRepositoryFault>>(() => service.AdressekontoGet(query));
        }

        /// <summary>
        /// Tester, at AdressekontoGet kaster en IntranetBusinessFault.
        /// </summary>
        [Test]
        public void TestAtAdressekontoGetKasterIntranetBusinessFault()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<AdressekontoGetQuery, AdressekontoView>(Arg<AdressekontoGetQuery>.Is.Anything))
                .Throw(new IntranetBusinessException("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new AdressekontoGetQuery();
            Assert.Throws<FaultException<IntranetBusinessFault>>(() => service.AdressekontoGet(query));
        }

        /// <summary>
        /// Tester, at AdressekontoGet kaster en IntranetSystemFault.
        /// </summary>
        [Test]
        public void TestAtAdressekontoGetKasterIntranetSystemFault()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<AdressekontoGetQuery, AdressekontoView>(Arg<AdressekontoGetQuery>.Is.Anything))
                .Throw(new IntranetSystemException("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new AdressekontoGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.AdressekontoGet(query));
        }

        /// <summary>
        /// Tester, at AdressekontoGet kaster en IntranetSystemFault ved en unhandled exception.
        /// </summary>
        [Test]
        public void TestAtAdressekontoGetKasterIntranetSystemFaultVedUnhandledException()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<AdressekontoGetQuery, AdressekontoView>(Arg<AdressekontoGetQuery>.Is.Anything))
                .Throw(new Exception("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new AdressekontoGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.AdressekontoGet(query));
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

        /// <summary>
        /// Tester, at BetalingsbetingelserGet kalder QueryBus.
        /// </summary>
        [Test]
        public void TestAtBetalingsbetingelserGetKalderQueryBus()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new BetalingsbetingelserGetQuery();
            service.BetalingsbetingelserGet(query);
            queryBus.AssertWasCalled(m => m.Query<BetalingsbetingelserGetQuery, IEnumerable<BetalingsbetingelseView>>(Arg<BetalingsbetingelserGetQuery>.Is.Anything));
        }

        /// <summary>
        /// Tester, at BetalingsbetingelserGet kaster en IntranetRepositoryFault.
        /// </summary>
        [Test]
        public void TestAtBetalingsbetingelserGetKasterIntranetRepositoryFault()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<BetalingsbetingelserGetQuery, IEnumerable<BetalingsbetingelseView>>(Arg<BetalingsbetingelserGetQuery>.Is.Anything))
                .Throw(new IntranetRepositoryException("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new BetalingsbetingelserGetQuery();
            Assert.Throws<FaultException<IntranetRepositoryFault>>(() => service.BetalingsbetingelserGet(query));
        }

        /// <summary>
        /// Tester, at BetalingsbetingelserGet kaster en IntranetBusinessFault.
        /// </summary>
        [Test]
        public void TestAtBetalingsbetingelserGetKasterIntranetBusinessFault()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<BetalingsbetingelserGetQuery, IEnumerable<BetalingsbetingelseView>>(Arg<BetalingsbetingelserGetQuery>.Is.Anything))
                .Throw(new IntranetBusinessException("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new BetalingsbetingelserGetQuery();
            Assert.Throws<FaultException<IntranetBusinessFault>>(() => service.BetalingsbetingelserGet(query));
        }

        /// <summary>
        /// Tester, at BetalingsbetingelserGet kaster en IntranetSystemFault.
        /// </summary>
        [Test]
        public void TestAtBetalingsbetingelserGetKasterIntranetSystemFault()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<BetalingsbetingelserGetQuery, IEnumerable<BetalingsbetingelseView>>(Arg<BetalingsbetingelserGetQuery>.Is.Anything))
                .Throw(new IntranetSystemException("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new BetalingsbetingelserGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.BetalingsbetingelserGet(query));
        }

        /// <summary>
        /// Tester, at BetalingsbetingelserGet kaster en IntranetSystemFault ved en unhandled exception.
        /// </summary>
        [Test]
        public void TestAtBetalingsbetingelserGetKasterIntranetSystemFaultVedUnhandledException()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<BetalingsbetingelserGetQuery, IEnumerable<BetalingsbetingelseView>>(Arg<BetalingsbetingelserGetQuery>.Is.Anything))
                .Throw(new Exception("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus, queryBus);
            var query = new BetalingsbetingelserGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.BetalingsbetingelserGet(query));
        }
    }
}
