using System;
using System.Collections.Generic;
using System.ServiceModel;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf;
using OSDevGrp.OSIntranet.Contracts.Faults;
using OSDevGrp.OSIntranet.Contracts.Queries;
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
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis QueryBus er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisQueryBusErNull()
        {
            Assert.Throws<ArgumentNullException>(() => new FinansstyringService(null));
        }

        /// <summary>
        /// Tester, at RegnskabslisteGet kalder QueryBus.
        /// </summary>
        [Test]
        public void TestAtRegnskabslisteGetKalderQueryBus()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            var service = new FinansstyringService(queryBus);
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
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m =>
                m.Query<RegnskabslisteGetQuery, IEnumerable<RegnskabslisteView>>(Arg<RegnskabslisteGetQuery>.Is.Anything))
                .Throw(new IntranetRepositoryException("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new RegnskabslisteGetQuery();
            Assert.Throws<FaultException<IntranetRepositoryFault>>(() => service.RegnskabslisteGet(query));
        }

        /// <summary>
        /// Tester, at RegnskabslisteGet kaster en IntranetBusinessFault.
        /// </summary>
        [Test]
        public void TestAtRegnskabslisteGetKasterIntranetBusinessFault()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m =>
                m.Query<RegnskabslisteGetQuery, IEnumerable<RegnskabslisteView>>(Arg<RegnskabslisteGetQuery>.Is.Anything))
                .Throw(new IntranetBusinessException("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new RegnskabslisteGetQuery();
            Assert.Throws<FaultException<IntranetBusinessFault>>(() => service.RegnskabslisteGet(query));
        }

        /// <summary>
        /// Tester, at RegnskabslisteGet kaster en IntranetSystemFault.
        /// </summary>
        [Test]
        public void TestAtRegnskabslisteGetKasterIntranetSystemFault()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m =>
                m.Query<RegnskabslisteGetQuery, IEnumerable<RegnskabslisteView>>(Arg<RegnskabslisteGetQuery>.Is.Anything))
                .Throw(new IntranetSystemException("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new RegnskabslisteGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.RegnskabslisteGet(query));
        }

        /// <summary>
        /// Tester, at RegnskabslisteGet kaster en IntranetSystemFault ved en unhandled exception.
        /// </summary>
        [Test]
        public void TestAtRegnskabslisteGetKasterIntranetSystemFaultVedUnhandledException()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m =>
                m.Query<RegnskabslisteGetQuery, IEnumerable<RegnskabslisteView>>(Arg<RegnskabslisteGetQuery>.Is.Anything))
                .Throw(new Exception("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new RegnskabslisteGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.RegnskabslisteGet(query));
        }

        /// <summary>
        /// Tester, at KontoplanGet kalder QueryBus.
        /// </summary>
        [Test]
        public void TestAtKontoplanGetKalderQueryBus()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            var service = new FinansstyringService(queryBus);
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
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m => m.Query<KontoplanGetQuery, IEnumerable<KontoplanView>>(Arg<KontoplanGetQuery>.Is.Anything)).Throw(
                    new IntranetRepositoryException("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new KontoplanGetQuery();
            Assert.Throws<FaultException<IntranetRepositoryFault>>(() => service.KontoplanGet(query));
        }

        /// <summary>
        /// Tester, at KontoplanGet kaster en IntranetBusinessFault.
        /// </summary>
        [Test]
        public void TestAtKontoplanGetKasterIntranetBusinessFault()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m => m.Query<KontoplanGetQuery, IEnumerable<KontoplanView>>(Arg<KontoplanGetQuery>.Is.Anything)).Throw(
                    new IntranetBusinessException("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new KontoplanGetQuery();
            Assert.Throws<FaultException<IntranetBusinessFault>>(() => service.KontoplanGet(query));
        }

        /// <summary>
        /// Tester, at KontoplanGet kaster en IntranetSystemFault.
        /// </summary>
        [Test]
        public void TestAtKontoplanGetKasterIntranetSystemFault()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m => m.Query<KontoplanGetQuery, IEnumerable<KontoplanView>>(Arg<KontoplanGetQuery>.Is.Anything)).Throw(
                    new IntranetSystemException("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new KontoplanGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.KontoplanGet(query));
        }

        /// <summary>
        /// Tester, at KontoplanGet kaster en IntranetSystemFault ved en unhandled exception.
        /// </summary>
        [Test]
        public void TestAtKontoplanGetKasterIntranetSystemFaultVedUnhandledException()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m => m.Query<KontoplanGetQuery, IEnumerable<KontoplanView>>(Arg<KontoplanGetQuery>.Is.Anything)).Throw(
                    new Exception("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new KontoplanGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.KontoplanGet(query));
        }

        /// <summary>
        /// Tester, at KontoGet kalder QueryBus.
        /// </summary>
        [Test]
        public void TestAtKontoGetKalderQueryBus()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            var service = new FinansstyringService(queryBus);
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
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<KontoGetQuery, KontoView>(Arg<KontoGetQuery>.Is.Anything)).
                Throw(new IntranetRepositoryException("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new KontoGetQuery();
            Assert.Throws<FaultException<IntranetRepositoryFault>>(() => service.KontoGet(query));
        }

        /// <summary>
        /// Tester, at KontoGet kaster en IntranetBusinessFault.
        /// </summary>
        [Test]
        public void TestAtKontoGetKasterIntranetBusinessFault()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<KontoGetQuery, KontoView>(Arg<KontoGetQuery>.Is.Anything)).
                Throw(new IntranetBusinessException("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new KontoGetQuery();
            Assert.Throws<FaultException<IntranetBusinessFault>>(() => service.KontoGet(query));
        }

        /// <summary>
        /// Tester, at KontoGet kaster en IntranetSystemFault.
        /// </summary>
        [Test]
        public void TestAtKontoGetKasterIntranetSystemFault()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<KontoGetQuery, KontoView>(Arg<KontoGetQuery>.Is.Anything))
                .Throw(new IntranetSystemException("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new KontoGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.KontoGet(query));
        }

        /// <summary>
        /// Tester, at KontoGet kaster en IntranetSystemFault ved en unhandled exception.
        /// </summary>
        [Test]
        public void TestAtKontoGetKasterIntranetSystemFaultVedUnhandledException()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<KontoGetQuery, KontoView>(Arg<KontoGetQuery>.Is.Anything))
                .Throw(new Exception("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new KontoGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.KontoGet(query));
        }

        /// <summary>
        /// Tester, at BudgetkontoplanGet kalder QueryBus.
        /// </summary>
        [Test]
        public void TestAtBudgetkontoplanGetKalderQueryBus()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            var service = new FinansstyringService(queryBus);
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
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m =>
                m.Query<BudgetkontoplanGetQuery, IEnumerable<BudgetkontoplanView>>(
                    Arg<BudgetkontoplanGetQuery>.Is.Anything)).
                Throw(new IntranetRepositoryException("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new BudgetkontoplanGetQuery();
            Assert.Throws<FaultException<IntranetRepositoryFault>>(() => service.BudgetkontoplanGet(query));
        }

        /// <summary>
        /// Tester, at BudgetkontoplanGet kaster en IntranetBusinessFault.
        /// </summary>
        [Test]
        public void TestAtBudgetkontoplanGetKasterIntranetBusinessFault()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m =>
                m.Query<BudgetkontoplanGetQuery, IEnumerable<BudgetkontoplanView>>(
                    Arg<BudgetkontoplanGetQuery>.Is.Anything)).
                Throw(new IntranetBusinessException("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new BudgetkontoplanGetQuery();
            Assert.Throws<FaultException<IntranetBusinessFault>>(() => service.BudgetkontoplanGet(query));
        }

        /// <summary>
        /// Tester, at BudgetkontoplanGet kaster en IntranetSystemFault.
        /// </summary>
        [Test]
        public void TestAtBudgetkontoplanGetKasterIntranetSystemFault()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m =>
                m.Query<BudgetkontoplanGetQuery, IEnumerable<BudgetkontoplanView>>(
                    Arg<BudgetkontoplanGetQuery>.Is.Anything)).
                Throw(new IntranetSystemException("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new BudgetkontoplanGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.BudgetkontoplanGet(query));
        }

        /// <summary>
        /// Tester, at BudgetkontoplanGet kaster en IntranetSystemFault ved en unhandled exception.
        /// </summary>
        [Test]
        public void TestAtBudgetkontoplanGetKasterIntranetSystemFaultVedUnhandledException()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m =>
                m.Query<BudgetkontoplanGetQuery, IEnumerable<BudgetkontoplanView>>(
                    Arg<BudgetkontoplanGetQuery>.Is.Anything)).
                Throw(new Exception("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new BudgetkontoplanGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.BudgetkontoplanGet(query));
        }

        /// <summary>
        /// Tester, at BudgetkontoGet kalder QueryBus.
        /// </summary>
        [Test]
        public void TestAtBudgetkontoGetKalderQueryBus()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            var service = new FinansstyringService(queryBus);
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
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<BudgetkontoGetQuery, BudgetkontoView>(Arg<BudgetkontoGetQuery>.Is.Anything)).
                Throw(new IntranetRepositoryException("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new BudgetkontoGetQuery();
            Assert.Throws<FaultException<IntranetRepositoryFault>>(() => service.BudgetkontoGet(query));
        }

        /// <summary>
        /// Tester, at BudgetkontoGet kaster en IntranetBusinessFault.
        /// </summary>
        [Test]
        public void TestAtBudgetkontoGetKasterIntranetBusinessFault()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<BudgetkontoGetQuery, BudgetkontoView>(Arg<BudgetkontoGetQuery>.Is.Anything)).
                Throw(new IntranetBusinessException("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new BudgetkontoGetQuery();
            Assert.Throws<FaultException<IntranetBusinessFault>>(() => service.BudgetkontoGet(query));
        }

        /// <summary>
        /// Tester, at BudgetkontoGet kaster en IntranetSystemFault.
        /// </summary>
        [Test]
        public void TestAtBudgetkontoGetKasterIntranetSystemFault()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<BudgetkontoGetQuery, BudgetkontoView>(Arg<BudgetkontoGetQuery>.Is.Anything)).
                Throw(new IntranetSystemException("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new BudgetkontoGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.BudgetkontoGet(query));
        }

        /// <summary>
        /// Tester, at BudgetkontoGet kaster en IntranetSystemFault ved en unhandled exception.
        /// </summary>
        [Test]
        public void TestAtBudgetkontoGetKasterIntranetSystemFaultVedUnhandledException()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<BudgetkontoGetQuery, BudgetkontoView>(Arg<BudgetkontoGetQuery>.Is.Anything)).
                Throw(new Exception("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new BudgetkontoGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.BudgetkontoGet(query));
        }

        /// <summary>
        /// Tester, at DebitorlisteGet kalder QueryBus.
        /// </summary>
        [Test]
        public void TestAtDebitorlisteGetKalderQueryBus()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            var service = new FinansstyringService(queryBus);
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
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m => m.Query<DebitorlisteGetQuery, IEnumerable<DebitorlisteView>>(Arg<DebitorlisteGetQuery>.Is.Anything))
                .Throw(new IntranetRepositoryException("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new DebitorlisteGetQuery();
            Assert.Throws<FaultException<IntranetRepositoryFault>>(() => service.DebitorlisteGet(query));
        }

        /// <summary>
        /// Tester, at DebitorlisteGet kaster en IntranetBusinessFault.
        /// </summary>
        [Test]
        public void TestAtDebitorlisteGetKasterIntranetBusinessFault()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m => m.Query<DebitorlisteGetQuery, IEnumerable<DebitorlisteView>>(Arg<DebitorlisteGetQuery>.Is.Anything))
                .Throw(new IntranetBusinessException("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new DebitorlisteGetQuery();
            Assert.Throws<FaultException<IntranetBusinessFault>>(() => service.DebitorlisteGet(query));
        }

        /// <summary>
        /// Tester, at DebitorlisteGet kaster en IntranetSystemFault.
        /// </summary>
        [Test]
        public void TestAtDebitorlisteGetKasterIntranetSystemFault()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m => m.Query<DebitorlisteGetQuery, IEnumerable<DebitorlisteView>>(Arg<DebitorlisteGetQuery>.Is.Anything))
                .Throw(new IntranetSystemException("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new DebitorlisteGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.DebitorlisteGet(query));
        }

        /// <summary>
        /// Tester, at DebitorlisteGet kaster en IntranetSystemFault ved en unhandled exception.
        /// </summary>
        [Test]
        public void TestAtDebitorlisteGetKasterIntranetSystemFaultVedUnhandledException()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m => m.Query<DebitorlisteGetQuery, IEnumerable<DebitorlisteView>>(Arg<DebitorlisteGetQuery>.Is.Anything))
                .Throw(new Exception("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new DebitorlisteGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.DebitorlisteGet(query));
        }

        /// <summary>
        /// Tester, at DebitorGet kalder QueryBus.
        /// </summary>
        [Test]
        public void TestAtDebitorGetKalderQueryBus()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            var service = new FinansstyringService(queryBus);
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
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<DebitorGetQuery, DebitorView>(Arg<DebitorGetQuery>.Is.Anything)).Throw(
                new IntranetRepositoryException("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new DebitorGetQuery();
            Assert.Throws<FaultException<IntranetRepositoryFault>>(() => service.DebitorGet(query));
        }

        /// <summary>
        /// Tester, at DebitorGet kaster en IntranetBusinessFault.
        /// </summary>
        [Test]
        public void TestAtDebitorGetKasterIntranetBusinessFault()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<DebitorGetQuery, DebitorView>(Arg<DebitorGetQuery>.Is.Anything)).Throw(
                new IntranetBusinessException("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new DebitorGetQuery();
            Assert.Throws<FaultException<IntranetBusinessFault>>(() => service.DebitorGet(query));
        }

        /// <summary>
        /// Tester, at DebitorGet kaster en IntranetSystemFault.
        /// </summary>
        [Test]
        public void TestAtDebitorGetKasterIntranetSystemFault()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<DebitorGetQuery, DebitorView>(Arg<DebitorGetQuery>.Is.Anything)).Throw(
                new IntranetSystemException("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new DebitorGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.DebitorGet(query));
        }

        /// <summary>
        /// Tester, at DebitorGet kaster en IntranetSystemFault ved en unhandled exception.
        /// </summary>
        [Test]
        public void TestAtDebitorGetKasterIntranetSystemFaultVedUnhandledException()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<DebitorGetQuery, DebitorView>(Arg<DebitorGetQuery>.Is.Anything)).Throw(
                new Exception("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new DebitorGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.DebitorGet(query));
        }

        /// <summary>
        /// Tester, at KreditorlisteGet kalder QueryBus.
        /// </summary>
        [Test]
        public void TestAtKreditorlisteGetKalderQueryBus()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            var service = new FinansstyringService(queryBus);
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
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m =>
                m.Query<KreditorlisteGetQuery, IEnumerable<KreditorlisteView>>(Arg<KreditorlisteGetQuery>.Is.Anything)).
                Throw(new IntranetRepositoryException("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new KreditorlisteGetQuery();
            Assert.Throws<FaultException<IntranetRepositoryFault>>(() => service.KreditorlisteGet(query));
        }

        /// <summary>
        /// Tester, at KreditorlisteGet kaster en IntranetBusinessFault.
        /// </summary>
        [Test]
        public void TestAtKreditorlisteGetKasterIntranetBusinessFault()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m =>
                m.Query<KreditorlisteGetQuery, IEnumerable<KreditorlisteView>>(Arg<KreditorlisteGetQuery>.Is.Anything)).
                Throw(new IntranetBusinessException("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new KreditorlisteGetQuery();
            Assert.Throws<FaultException<IntranetBusinessFault>>(() => service.KreditorlisteGet(query));
        }

        /// <summary>
        /// Tester, at KreditorlisteGet kaster en IntranetSystemFault.
        /// </summary>
        [Test]
        public void TestAtKreditorlisteGetKasterIntranetSystemFault()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m =>
                m.Query<KreditorlisteGetQuery, IEnumerable<KreditorlisteView>>(Arg<KreditorlisteGetQuery>.Is.Anything)).
                Throw(new IntranetSystemException("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new KreditorlisteGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.KreditorlisteGet(query));
        }

        /// <summary>
        /// Tester, at KreditorlisteGet kaster en IntranetSystemFault ved en unhandled exception.
        /// </summary>
        [Test]
        public void TestAtKreditorlisteGetKasterIntranetSystemFaultVedUnhandledException()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m =>
                m.Query<KreditorlisteGetQuery, IEnumerable<KreditorlisteView>>(Arg<KreditorlisteGetQuery>.Is.Anything)).
                Throw(new Exception("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new KreditorlisteGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.KreditorlisteGet(query));
        }

        /// <summary>
        /// Tester, at KreditorGet kalder QueryBus.
        /// </summary>
        [Test]
        public void TestAtKreditorGetKalderQueryBus()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            var service = new FinansstyringService(queryBus);
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
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<KreditorGetQuery, KreditorView>(Arg<KreditorGetQuery>.Is.Anything)).Throw(
                new IntranetRepositoryException("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new KreditorGetQuery();
            Assert.Throws<FaultException<IntranetRepositoryFault>>(() => service.KreditorGet(query));
        }

        /// <summary>
        /// Tester, at KreditorGet kaster en IntranetBusinessFault.
        /// </summary>
        [Test]
        public void TestAtKreditorGetKasterIntranetBusinessFault()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<KreditorGetQuery, KreditorView>(Arg<KreditorGetQuery>.Is.Anything)).Throw(
                new IntranetBusinessException("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new KreditorGetQuery();
            Assert.Throws<FaultException<IntranetBusinessFault>>(() => service.KreditorGet(query));
        }

        /// <summary>
        /// Tester, at KreditorGet kaster en IntranetSystemFault.
        /// </summary>
        [Test]
        public void TestAtKreditorGetKasterIntranetSystemFault()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<KreditorGetQuery, KreditorView>(Arg<KreditorGetQuery>.Is.Anything)).Throw(
                new IntranetSystemException("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new KreditorGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.KreditorGet(query));
        }

        /// <summary>
        /// Tester, at KreditorGet kaster en IntranetSystemFault ved en unhandled exception.
        /// </summary>
        [Test]
        public void TestAtKreditorGetKasterIntranetSystemFaultVedUnhandledException()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<KreditorGetQuery, KreditorView>(Arg<KreditorGetQuery>.Is.Anything)).Throw(
                new Exception("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new KreditorGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.KreditorGet(query));
        }

        /// <summary>
        /// Tester, at AdressekontolisteGet kalder QueryBus.
        /// </summary>
        [Test]
        public void TestAtAdressekontolisteGetKalderQueryBus()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            var service = new FinansstyringService(queryBus);
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
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<AdressekontolisteGetQuery, IEnumerable<AdressekontolisteView>>(Arg<AdressekontolisteGetQuery>.Is.Anything))
                .Throw(new IntranetRepositoryException("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new AdressekontolisteGetQuery();
            Assert.Throws<FaultException<IntranetRepositoryFault>>(() => service.AdressekontolisteGet(query));
        }

        /// <summary>
        /// Tester, at AdressekontolisteGet kaster en IntranetBusinessFault.
        /// </summary>
        [Test]
        public void TestAtAdressekontolisteGetKasterIntranetBusinessFault()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<AdressekontolisteGetQuery, IEnumerable<AdressekontolisteView>>(Arg<AdressekontolisteGetQuery>.Is.Anything))
                .Throw(new IntranetBusinessException("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new AdressekontolisteGetQuery();
            Assert.Throws<FaultException<IntranetBusinessFault>>(() => service.AdressekontolisteGet(query));
        }

        /// <summary>
        /// Tester, at AdressekontolisteGet kaster en IntranetSystemFault.
        /// </summary>
        [Test]
        public void TestAtAdressekontolisteGetKasterIntranetSystemFault()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<AdressekontolisteGetQuery, IEnumerable<AdressekontolisteView>>(Arg<AdressekontolisteGetQuery>.Is.Anything))
                .Throw(new IntranetSystemException("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new AdressekontolisteGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.AdressekontolisteGet(query));
        }

        /// <summary>
        /// Tester, at AdressekontolisteGet kaster en IntranetSystemFault ved en unhandled exception.
        /// </summary>
        [Test]
        public void TestAtAdressekontolisteGetKasterIntranetSystemFaultVedUnhandledException()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<AdressekontolisteGetQuery, IEnumerable<AdressekontolisteView>>(Arg<AdressekontolisteGetQuery>.Is.Anything))
                .Throw(new Exception("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new AdressekontolisteGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.AdressekontolisteGet(query));
        }

        /// <summary>
        /// Tester, at AdressekontoGet kalder QueryBus.
        /// </summary>
        [Test]
        public void TestAtAdressekontoGetKalderQueryBus()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            var service = new FinansstyringService(queryBus);
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
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<AdressekontoGetQuery, AdressekontoView>(Arg<AdressekontoGetQuery>.Is.Anything))
                .Throw(new IntranetRepositoryException("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new AdressekontoGetQuery();
            Assert.Throws<FaultException<IntranetRepositoryFault>>(() => service.AdressekontoGet(query));
        }

        /// <summary>
        /// Tester, at AdressekontoGet kaster en IntranetBusinessFault.
        /// </summary>
        [Test]
        public void TestAtAdressekontoGetKasterIntranetBusinessFault()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<AdressekontoGetQuery, AdressekontoView>(Arg<AdressekontoGetQuery>.Is.Anything))
                .Throw(new IntranetBusinessException("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new AdressekontoGetQuery();
            Assert.Throws<FaultException<IntranetBusinessFault>>(() => service.AdressekontoGet(query));
        }

        /// <summary>
        /// Tester, at AdressekontoGet kaster en IntranetSystemFault.
        /// </summary>
        [Test]
        public void TestAtAdressekontoGetKasterIntranetSystemFault()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<AdressekontoGetQuery, AdressekontoView>(Arg<AdressekontoGetQuery>.Is.Anything))
                .Throw(new IntranetSystemException("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new AdressekontoGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.AdressekontoGet(query));
        }

        /// <summary>
        /// Tester, at AdressekontoGet kaster en IntranetSystemFault ved en unhandled exception.
        /// </summary>
        [Test]
        public void TestAtAdressekontoGetKasterIntranetSystemFaultVedUnhandledException()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<AdressekontoGetQuery, AdressekontoView>(Arg<AdressekontoGetQuery>.Is.Anything))
                .Throw(new Exception("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new AdressekontoGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.AdressekontoGet(query));
        }

        /// <summary>
        /// Tester, at BogføringerGet kalder QueryBus.
        /// </summary>
        [Test]
        public void TestAtBogføringerGetKalderQueryBus()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            var service = new FinansstyringService(queryBus);
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
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<BogføringerGetQuery, IEnumerable<BogføringslinjeView>>(Arg<BogføringerGetQuery>.Is.Anything))
                .Throw(new IntranetRepositoryException("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new BogføringerGetQuery();
            Assert.Throws<FaultException<IntranetRepositoryFault>>(() => service.BogføringerGet(query));
        }

        /// <summary>
        /// Tester, at BogføringerGet kaster en IntranetBusinessFault.
        /// </summary>
        [Test]
        public void TestAtBogføringerGetKasterIntranetBusinessFault()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<BogføringerGetQuery, IEnumerable<BogføringslinjeView>>(Arg<BogføringerGetQuery>.Is.Anything))
                .Throw(new IntranetBusinessException("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new BogføringerGetQuery();
            Assert.Throws<FaultException<IntranetBusinessFault>>(() => service.BogføringerGet(query));
        }

        /// <summary>
        /// Tester, at BogføringerGet kaster en IntranetSystemFault.
        /// </summary>
        [Test]
        public void TestAtBogføringerGetKasterIntranetSystemFault()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<BogføringerGetQuery, IEnumerable<BogføringslinjeView>>(Arg<BogføringerGetQuery>.Is.Anything))
                .Throw(new IntranetSystemException("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new BogføringerGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.BogføringerGet(query));
        }

        /// <summary>
        /// Tester, at BogføringerGet kaster en IntranetSystemFault ved en unhandled exception.
        /// </summary>
        [Test]
        public void TestAtBogføringerGetKasterIntranetSystemFaultVedUnhandledException()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<BogføringerGetQuery, IEnumerable<BogføringslinjeView>>(Arg<BogføringerGetQuery>.Is.Anything))
                .Throw(new Exception("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new BogføringerGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.BogføringerGet(query));
        }

        /// <summary>
        /// Tester, at BetalingsbetingelserGet kalder QueryBus.
        /// </summary>
        [Test]
        public void TestAtBetalingsbetingelserGetKalderQueryBus()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            var service = new FinansstyringService(queryBus);
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
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<BetalingsbetingelserGetQuery, IEnumerable<BetalingsbetingelseView>>(Arg<BetalingsbetingelserGetQuery>.Is.Anything))
                .Throw(new IntranetRepositoryException("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new BetalingsbetingelserGetQuery();
            Assert.Throws<FaultException<IntranetRepositoryFault>>(() => service.BetalingsbetingelserGet(query));
        }

        /// <summary>
        /// Tester, at BetalingsbetingelserGet kaster en IntranetBusinessFault.
        /// </summary>
        [Test]
        public void TestAtBetalingsbetingelserGetKasterIntranetBusinessFault()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<BetalingsbetingelserGetQuery, IEnumerable<BetalingsbetingelseView>>(Arg<BetalingsbetingelserGetQuery>.Is.Anything))
                .Throw(new IntranetBusinessException("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new BetalingsbetingelserGetQuery();
            Assert.Throws<FaultException<IntranetBusinessFault>>(() => service.BetalingsbetingelserGet(query));
        }

        /// <summary>
        /// Tester, at BetalingsbetingelserGet kaster en IntranetSystemFault.
        /// </summary>
        [Test]
        public void TestAtBetalingsbetingelserGetKasterIntranetSystemFault()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<BetalingsbetingelserGetQuery, IEnumerable<BetalingsbetingelseView>>(Arg<BetalingsbetingelserGetQuery>.Is.Anything))
                .Throw(new IntranetSystemException("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new BetalingsbetingelserGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.BetalingsbetingelserGet(query));
        }

        /// <summary>
        /// Tester, at BetalingsbetingelserGet kaster en IntranetSystemFault ved en unhandled exception.
        /// </summary>
        [Test]
        public void TestAtBetalingsbetingelserGetKasterIntranetSystemFaultVedUnhandledException()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<BetalingsbetingelserGetQuery, IEnumerable<BetalingsbetingelseView>>(Arg<BetalingsbetingelserGetQuery>.Is.Anything))
                .Throw(new Exception("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new BetalingsbetingelserGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.BetalingsbetingelserGet(query));
        }

        /// <summary>
        /// Tester, at KontogrupperGet kalder QueryBus.
        /// </summary>
        [Test]
        public void TestAtKontogrupperGetKalderQueryBus()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            var service = new FinansstyringService(queryBus);
            var query = new KontogrupperGetQuery();
            service.KontogrupperGet(query);
            queryBus.AssertWasCalled(m => m.Query<KontogrupperGetQuery, IEnumerable<KontogruppeView>>(Arg<KontogrupperGetQuery>.Is.Anything));
        }

        /// <summary>
        /// Tester, at KontogrupperGet kaster en IntranetRepositoryFault.
        /// </summary>
        [Test]
        public void TestAtKontogrupperGetKasterIntranetRepositoryFault()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<KontogrupperGetQuery, IEnumerable<KontogruppeView>>(Arg<KontogrupperGetQuery>.Is.Anything))
                .Throw(new IntranetRepositoryException("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new KontogrupperGetQuery();
            Assert.Throws<FaultException<IntranetRepositoryFault>>(() => service.KontogrupperGet(query));
        }

        /// <summary>
        /// Tester, at KontogrupperGet kaster en IntranetBusinessFault.
        /// </summary>
        [Test]
        public void TestAtKontogrupperGetKasterIntranetBusinessFault()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<KontogrupperGetQuery, IEnumerable<KontogruppeView>>(Arg<KontogrupperGetQuery>.Is.Anything))
                .Throw(new IntranetBusinessException("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new KontogrupperGetQuery();
            Assert.Throws<FaultException<IntranetBusinessFault>>(() => service.KontogrupperGet(query));
        }

        /// <summary>
        /// Tester, at KontogrupperGet kaster en IntranetSystemFault.
        /// </summary>
        [Test]
        public void TestAtKontogrupperGetKasterIntranetSystemFault()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<KontogrupperGetQuery, IEnumerable<KontogruppeView>>(Arg<KontogrupperGetQuery>.Is.Anything))
                .Throw(new IntranetSystemException("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new KontogrupperGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.KontogrupperGet(query));
        }

        /// <summary>
        /// Tester, at KontogrupperGet kaster en IntranetSystemFault ved en unhandled exception.
        /// </summary>
        [Test]
        public void TestAtKontogrupperGetKasterIntranetSystemFaultVedUnhandledException()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<KontogrupperGetQuery, IEnumerable<KontogruppeView>>(Arg<KontogrupperGetQuery>.Is.Anything))
                .Throw(new Exception("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new KontogrupperGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.KontogrupperGet(query));
        }

        /// <summary>
        /// Tester, at BudgetkontogrupperGet kalder QueryBus.
        /// </summary>
        [Test]
        public void TestAtBudgetkontogrupperGetKalderQueryBus()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            var service = new FinansstyringService(queryBus);
            var query = new BudgetkontogrupperGetQuery();
            service.BudgetkontogrupperGet(query);
            queryBus.AssertWasCalled(m => m.Query<BudgetkontogrupperGetQuery, IEnumerable<BudgetkontogruppeView>>(Arg<BudgetkontogrupperGetQuery>.Is.Anything));
        }

        /// <summary>
        /// Tester, at BudgetkontogrupperGet kaster en IntranetRepositoryFault.
        /// </summary>
        [Test]
        public void TestAtBudgetkontogrupperGetKasterIntranetRepositoryFault()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<BudgetkontogrupperGetQuery, IEnumerable<BudgetkontogruppeView>>(Arg<BudgetkontogrupperGetQuery>.Is.Anything))
                .Throw(new IntranetRepositoryException("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new BudgetkontogrupperGetQuery();
            Assert.Throws<FaultException<IntranetRepositoryFault>>(() => service.BudgetkontogrupperGet(query));
        }

        /// <summary>
        /// Tester, at BudgetkontogrupperGet kaster en IntranetBusinessFault.
        /// </summary>
        [Test]
        public void TestAtBudgetkontogrupperGetKasterIntranetBusinessFault()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<BudgetkontogrupperGetQuery, IEnumerable<BudgetkontogruppeView>>(Arg<BudgetkontogrupperGetQuery>.Is.Anything))
                .Throw(new IntranetBusinessException("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new BudgetkontogrupperGetQuery();
            Assert.Throws<FaultException<IntranetBusinessFault>>(() => service.BudgetkontogrupperGet(query));
        }

        /// <summary>
        /// Tester, at BudgetkontogrupperGet kaster en IntranetSystemFault.
        /// </summary>
        [Test]
        public void TestAtBudgetkontogrupperGetKasterIntranetSystemFault()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<BudgetkontogrupperGetQuery, IEnumerable<BudgetkontogruppeView>>(Arg<BudgetkontogrupperGetQuery>.Is.Anything))
                .Throw(new IntranetSystemException("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new BudgetkontogrupperGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.BudgetkontogrupperGet(query));
        }

        /// <summary>
        /// Tester, at BudgetkontogrupperGet kaster en IntranetSystemFault ved en unhandled exception.
        /// </summary>
        [Test]
        public void TestAtBudgetkontogrupperGetKasterIntranetSystemFaultVedUnhandledException()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<BudgetkontogrupperGetQuery, IEnumerable<BudgetkontogruppeView>>(Arg<BudgetkontogrupperGetQuery>.Is.Anything))
                .Throw(new Exception("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new BudgetkontogrupperGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.BudgetkontogrupperGet(query));
        }
    }
}
