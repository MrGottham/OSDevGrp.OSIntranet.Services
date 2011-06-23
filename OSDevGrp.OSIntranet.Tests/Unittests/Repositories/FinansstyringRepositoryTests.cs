using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf.ChannelFactory;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Commands;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Queries;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Services;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Views;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories
{
    /// <summary>
    /// Test af repository til finansstyring.
    /// </summary>
    public class FinansstyringRepositoryTests
    {
        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis ChannelFactory er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisChannelFactoryErNull()
        {
            Assert.Throws<ArgumentNullException>(() => new FinansstyringRepository(null, null));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis domæneobjekt bygger er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisDomainObjectBuilderErNull()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            Assert.Throws<ArgumentNullException>(() => new FinansstyringRepository(channelFactory, null));
        }

        /// <summary>
        /// Tester, at RegnskabslisteGet henter alle regnskaber.
        /// </summary>
        [Test]
        public void TestAtRegnskabslisteGetHenterAlleRegnskaber()
        {
            var fixture = new Fixture();

            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFinansstyringRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.RegnskabGetAll(Arg<RegnskabGetAllQuery>.Is.Anything))
                .Return(fixture.CreateMany<RegnskabListeView>(3));
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed).Repeat.Any();
            mocker.ReplayAll();
            
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFinansstyringRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();
            domainObjectBuilder.Expect(
                m =>
                m.Build<IEnumerable<RegnskabListeView>, IEnumerable<Regnskab>>(
                    Arg<IEnumerable<RegnskabListeView>>.Is.NotNull)).Return(fixture.CreateMany<Regnskab>(3));

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            var regnskaber = repository.RegnskabslisteGet();
            Assert.That(regnskaber, Is.Not.Null);
            Assert.That(regnskaber.Count(), Is.EqualTo(3));

            domainObjectBuilder.AssertWasCalled(
                m =>
                m.Build<IEnumerable<RegnskabListeView>, IEnumerable<Regnskab>>(
                    Arg<IEnumerable<RegnskabListeView>>.Is.NotNull));
        }

        /// <summary>
        /// Tester, at RegnskabslisteGet kaster en IntranetRepositoryException ved IntranetRepositoryException.
        /// </summary>
        [Test]
        public void TestAtRegnskabslisteGetKasterIntranetRepositoryExceptionVedIntranetRepositoryException()
        {
            var fixture = new Fixture();

            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFinansstyringRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.RegnskabGetAll(Arg<RegnskabGetAllQuery>.Is.Anything))
                .Throw(fixture.CreateAnonymous<IntranetRepositoryException>());
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed).Repeat.Any();
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFinansstyringRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            Assert.Throws<IntranetRepositoryException>(() => repository.RegnskabslisteGet());
        }

        /// <summary>
        /// Tester, at RegnskabslisteGet kaster en IntranetRepositoryException ved FaultException.
        /// </summary>
        [Test]
        public void TestAtRegnskabslisteGetKasterIntranetRepositoryExceptionVedFaultException()
        {
            var fixture = new Fixture();

            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFinansstyringRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.RegnskabGetAll(Arg<RegnskabGetAllQuery>.Is.Anything))
                .Throw(fixture.CreateAnonymous<FaultException>());
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed).Repeat.Any();
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFinansstyringRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            Assert.Throws<IntranetRepositoryException>(() => repository.RegnskabslisteGet());
        }

        /// <summary>
        /// Tester, at RegnskabslisteGet kaster en IntranetRepositoryException ved Exception.
        /// </summary>
        [Test]
        public void TestAtRegnskabslisteGetKasterIntranetRepositoryExceptionVedException()
        {
            var fixture = new Fixture();

            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFinansstyringRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.RegnskabGetAll(Arg<RegnskabGetAllQuery>.Is.Anything))
                .Throw(fixture.CreateAnonymous<Exception>());
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed).Repeat.Any();
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFinansstyringRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            Assert.Throws<IntranetRepositoryException>(() => repository.RegnskabslisteGet());
        }

        /// <summary>
        /// Tester, at RegnskabGet henter et givent regnskab.
        /// </summary>
        [Test]
        public void TestAtRegnskabGetHenterRegnskab()
        {
            var fixture = new Fixture();
            fixture.Inject<IEnumerable<KontoView>>(new List<KontoView>());
            fixture.Inject<IEnumerable<BudgetkontoView>>(new List<BudgetkontoView>());

            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFinansstyringRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.RegnskabGetByNummer(Arg<RegnskabGetByNummerQuery>.Is.Anything))
                .Return(fixture.CreateAnonymous<RegnskabView>());
            service.Expect(m => m.KontogruppeGetAll(Arg<KontogruppeGetAllQuery>.Is.Anything))
                .Return(fixture.CreateMany<KontogruppeView>(3));
            service.Expect(m => m.BudgetkontogruppeGetAll(Arg<BudgetkontogruppeGetAllQuery>.Is.Anything))
                .Return(fixture.CreateMany<BudgetkontogruppeView>(3));
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed);
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFinansstyringRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();
            domainObjectBuilder.Expect(
                m =>
                m.Build<IEnumerable<KontogruppeView>, IEnumerable<Kontogruppe>>(
                    Arg<IEnumerable<KontogruppeView>>.Is.NotNull))
                .Return(fixture.CreateMany<Kontogruppe>(3));
            domainObjectBuilder.Expect(
                m =>
                m.Build<IEnumerable<BudgetkontogruppeView>, IEnumerable<Budgetkontogruppe>>(
                    Arg<IEnumerable<BudgetkontogruppeView>>.Is.NotNull))
                .Return(fixture.CreateMany<Budgetkontogruppe>(3));

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            var regnskab = repository.RegnskabGet(1);
            Assert.That(regnskab, Is.Not.Null);

            domainObjectBuilder.AssertWasCalled(
                m =>
                m.Build<IEnumerable<KontogruppeView>, IEnumerable<Kontogruppe>>(
                    Arg<IEnumerable<KontogruppeView>>.Is.NotNull));
            domainObjectBuilder.AssertWasCalled(
                m =>
                m.Build<IEnumerable<BudgetkontogruppeView>, IEnumerable<Budgetkontogruppe>>(
                    Arg<IEnumerable<BudgetkontogruppeView>>.Is.NotNull));

            /*
            var person = new Person(1, "Ole Sørensen", new Adressegruppe(1, "Familie (Ole)", 1));
            regnskab = repository.RegnskabGet(1, nummer => person);
            Assert.That(regnskab, Is.Not.Null);

            domainObjectBuilder.AssertWasCalled(
                m =>
                m.Build<IEnumerable<KontogruppeView>, IEnumerable<Kontogruppe>>(
                    Arg<IEnumerable<KontogruppeView>>.Is.NotNull));
            */
        }

        /// <summary>
        /// Tester, at RegnskabGet kaster en IntranetRepositoryException ved IntranetRepositoryException.
        /// </summary>
        [Test]
        public void TestAtRegnskabGetKasterIntranetRepositoryExceptionVedIntranetRepositoryException()
        {
            var fixture = new Fixture();

            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFinansstyringRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.RegnskabGetByNummer(Arg<RegnskabGetByNummerQuery>.Is.Anything))
                .Throw(fixture.CreateAnonymous<IntranetRepositoryException>());
            service.Expect(m => m.KontogruppeGetAll(Arg<KontogruppeGetAllQuery>.Is.Anything))
                .Return(fixture.CreateMany<KontogruppeView>(3));
            service.Expect(m => m.BudgetkontogruppeGetAll(Arg<BudgetkontogruppeGetAllQuery>.Is.Anything))
                .Return(fixture.CreateMany<BudgetkontogruppeView>(3));
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed);
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFinansstyringRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            Assert.Throws<IntranetRepositoryException>(() => repository.RegnskabGet(-1));
        }

        /// <summary>
        /// Tester, at RegnskabGet kaster en IntranetRepositoryException ved FaultException.
        /// </summary>
        [Test]
        public void TestAtRegnskabGetKasterIntranetRepositoryExceptionVedFaultException()
        {
            var fixture = new Fixture();

            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFinansstyringRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.RegnskabGetByNummer(Arg<RegnskabGetByNummerQuery>.Is.Anything))
                .Throw(fixture.CreateAnonymous<FaultException>());
            service.Expect(m => m.KontogruppeGetAll(Arg<KontogruppeGetAllQuery>.Is.Anything))
                .Return(fixture.CreateMany<KontogruppeView>(3));
            service.Expect(m => m.BudgetkontogruppeGetAll(Arg<BudgetkontogruppeGetAllQuery>.Is.Anything))
                .Return(fixture.CreateMany<BudgetkontogruppeView>(3));
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed);
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFinansstyringRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            Assert.Throws<IntranetRepositoryException>(() => repository.RegnskabGet(-1));
        }

        /// <summary>
        /// Tester, at RegnskabGet kaster en IntranetRepositoryException ved Exception.
        /// </summary>
        [Test]
        public void TestAtRegnskabGetKasterIntranetRepositoryExceptionVedException()
        {
            var fixture = new Fixture();

            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFinansstyringRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.RegnskabGetByNummer(Arg<RegnskabGetByNummerQuery>.Is.Anything))
                .Throw(fixture.CreateAnonymous<Exception>());
            service.Expect(m => m.KontogruppeGetAll(Arg<KontogruppeGetAllQuery>.Is.Anything))
                .Return(fixture.CreateMany<KontogruppeView>(3));
            service.Expect(m => m.BudgetkontogruppeGetAll(Arg<BudgetkontogruppeGetAllQuery>.Is.Anything))
                .Return(fixture.CreateMany<BudgetkontogruppeView>(3));
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed);
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFinansstyringRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            Assert.Throws<IntranetRepositoryException>(() => repository.RegnskabGet(-1));
        }

        /// <summary>
        /// Tester, at KontogruppeGetAll henter alle kontogrupper.
        /// </summary>
        [Test]
        public void TestAtKontogruppeGetAllHenterAlleKontogrupper()
        {
            var fixture = new Fixture();

            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFinansstyringRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.KontogruppeGetAll(Arg<KontogruppeGetAllQuery>.Is.Anything))
                .Return(fixture.CreateMany<KontogruppeView>(3));
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed);
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFinansstyringRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();
            domainObjectBuilder.Expect(
                m =>
                m.Build<IEnumerable<KontogruppeView>, IEnumerable<Kontogruppe>>(
                    Arg<IEnumerable<KontogruppeView>>.Is.NotNull)).Return(fixture.CreateMany<Kontogruppe>(3));

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            var kontogrupper = repository.KontogruppeGetAll();
            Assert.That(kontogrupper, Is.Not.Null);
            Assert.That(kontogrupper.Count(), Is.EqualTo(3));

            domainObjectBuilder.AssertWasCalled(
                m =>
                m.Build<IEnumerable<KontogruppeView>, IEnumerable<Kontogruppe>>(
                    Arg<IEnumerable<KontogruppeView>>.Is.NotNull));
        }

        /// <summary>
        /// Tester, at KontogruppeGetAll kaster en IntranetRepositoryException ved IntranetRepositoryException.
        /// </summary>
        [Test]
        public void TestAtKontogruppeGetAllKasterIntranetRepositoryExceptionVedIntranetRepositoryException()
        {
            var fixture = new Fixture();

            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFinansstyringRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.KontogruppeGetAll(Arg<KontogruppeGetAllQuery>.Is.Anything))
                .Throw(fixture.CreateAnonymous<IntranetRepositoryException>());
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed);
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFinansstyringRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            Assert.Throws<IntranetRepositoryException>(() => repository.KontogruppeGetAll());
        }

        /// <summary>
        /// Tester, at KontogruppeGetAll kaster en IntranetRepositoryException ved FaultException.
        /// </summary>
        [Test]
        public void TestAtKontogruppeGetAllKasterIntranetRepositoryExceptionVedFaultException()
        {
            var fixture = new Fixture();

            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFinansstyringRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.KontogruppeGetAll(Arg<KontogruppeGetAllQuery>.Is.Anything))
                .Throw(fixture.CreateAnonymous<FaultException>());
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed);
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFinansstyringRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            Assert.Throws<IntranetRepositoryException>(() => repository.KontogruppeGetAll());
        }

        /// <summary>
        /// Tester, at KontogruppeGetAll kaster en IntranetRepositoryException ved Exception.
        /// </summary>
        [Test]
        public void TestAtKontogruppeGetAllKasterIntranetRepositoryExceptionVedException()
        {
            var fixture = new Fixture();

            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFinansstyringRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.KontogruppeGetAll(Arg<KontogruppeGetAllQuery>.Is.Anything))
                .Throw(fixture.CreateAnonymous<Exception>());
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed);
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFinansstyringRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            Assert.Throws<IntranetRepositoryException>(() => repository.KontogruppeGetAll());
        }

        /// <summary>
        /// Tester, at BudgetkontogruppeGetAll henter alle budgetkontogrupper.
        /// </summary>
        [Test]
        public void TestAtBudgetkontogruppeGetAllHenterAlleBudgetkontogrupper()
        {
            var fixture = new Fixture();

            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFinansstyringRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.BudgetkontogruppeGetAll(Arg<BudgetkontogruppeGetAllQuery>.Is.Anything))
                .Return(fixture.CreateMany<BudgetkontogruppeView>(3));
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed);
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFinansstyringRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();
            domainObjectBuilder.Expect(
                m =>
                m.Build<IEnumerable<BudgetkontogruppeView>, IEnumerable<Budgetkontogruppe>>(
                    Arg<IEnumerable<BudgetkontogruppeView>>.Is.NotNull)).Return(fixture.CreateMany<Budgetkontogruppe>(3));

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            var budgetkontogrupper = repository.BudgetkontogruppeGetAll();
            Assert.That(budgetkontogrupper, Is.Not.Null);
            Assert.That(budgetkontogrupper.Count(), Is.EqualTo(3));

            domainObjectBuilder.AssertWasCalled(
                m =>
                m.Build<IEnumerable<BudgetkontogruppeView>, IEnumerable<Budgetkontogruppe>>(
                    Arg<IEnumerable<BudgetkontogruppeView>>.Is.NotNull));
        }

        /// <summary>
        /// Tester, at BudgetkontogruppeGetAll kaster en IntranetRepositoryException ved IntranetRepositoryException.
        /// </summary>
        [Test]
        public void TestAtBudgetkontogruppeGetAllKasterIntranetRepositoryExceptionVedIntranetRepositoryException()
        {
            var fixture = new Fixture();

            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFinansstyringRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.BudgetkontogruppeGetAll(Arg<BudgetkontogruppeGetAllQuery>.Is.Anything))
                .Throw(fixture.CreateAnonymous<IntranetRepositoryException>());
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed);
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFinansstyringRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            Assert.Throws<IntranetRepositoryException>(() => repository.BudgetkontogruppeGetAll());
        }

        /// <summary>
        /// Tester, at BudgetkontogruppeGetAll kaster en IntranetRepositoryException ved FaultException.
        /// </summary>
        [Test]
        public void TestAtBudgetkontogruppeGetAllKasterIntranetRepositoryExceptionVedFaultException()
        {
            var fixture = new Fixture();

            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFinansstyringRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.BudgetkontogruppeGetAll(Arg<BudgetkontogruppeGetAllQuery>.Is.Anything))
                .Throw(fixture.CreateAnonymous<FaultException>());
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed);
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFinansstyringRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            Assert.Throws<IntranetRepositoryException>(() => repository.BudgetkontogruppeGetAll());
        }

        /// <summary>
        /// Tester, at BudgetkontogruppeGetAll kaster en IntranetRepositoryException ved Exception.
        /// </summary>
        [Test]
        public void TestAtBudgetkontogruppeGetAllKasterIntranetRepositoryExceptionVedException()
        {
            var fixture = new Fixture();

            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFinansstyringRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.BudgetkontogruppeGetAll(Arg<BudgetkontogruppeGetAllQuery>.Is.Anything))
                .Throw(fixture.CreateAnonymous<Exception>());
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed);
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFinansstyringRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            Assert.Throws<IntranetRepositoryException>(() => repository.BudgetkontogruppeGetAll());
        }

        /// <summary>
        /// Tester, at BogføringslinjeAdd kaster en ArgumentNullException, hvis kontoen er null.
        /// </summary>
        [Test]
        public void TestAtBogføringslinjeAddKasterEnArgumentNullExceptionHvisKontoErNull()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();
            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            Assert.Throws<ArgumentNullException>(() => repository.BogføringslinjeAdd(new DateTime(2011, 4, 1), null, null, null, null, 0M, 0M, null));
        }

        /// <summary>
        /// Tester, at BogføringslinjeAdd kaster en ArgumentNullException, hvis tekst er null.
        /// </summary>
        [Test]
        public void TestAtBogføringslinjeAddKasterEnArgumentNullExceptionHvisTekstErNull()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();
            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            var regnskab = new Regnskab(1, "Ole Sørensen");
            var bankkonti = new Kontogruppe(1, "Bankkonto", CommonLibrary.Domain.Enums.KontogruppeType.Aktiver);
            var dankort = new Konto(regnskab, "DANKORT", "Dankort", bankkonti);
            Assert.Throws<ArgumentNullException>(() => repository.BogføringslinjeAdd(new DateTime(2011, 4, 1), null, dankort, null, null, 0M, 0M, null));
        }

        /// <summary>
        /// Tester, at BogføringslinjeAdd kalder servicemetode.
        /// </summary>
        [Test]
        public void TestAtBogføringslinjeAddKalderServicemetode()
        {
            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFinansstyringRepositoryService>(new[] { typeof(ICommunicationObject) });
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed);
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFinansstyringRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            var regnskab = new Regnskab(1, "Ole Sørensen");
            var bankkonti = new Kontogruppe(1, "Bankkonto", CommonLibrary.Domain.Enums.KontogruppeType.Aktiver);
            var dankort = new Konto(regnskab, "DANKORT", "Dankort", bankkonti);
            repository.BogføringslinjeAdd(new DateTime(2011, 4, 1), null, dankort, "Test", null, 0M, 0M, null);
            service.AssertWasCalled(m => m.BogføringslinjeAdd(Arg<BogføringslinjeAddCommand>.Is.Anything));
        }

        /// <summary>
        /// Tester, at BogføringslinjeAdd kaster en IntranetRepositoryException ved IntranetRepositoryException.
        /// </summary>
        [Test]
        public void TestAtBogføringslinjeAddKasterIntranetRepositoryExceptionVedIntranetRepositoryException()
        {
            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFinansstyringRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.BogføringslinjeAdd(Arg<BogføringslinjeAddCommand>.Is.Anything))
                .Throw(new IntranetRepositoryException("Test"));
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed);
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFinansstyringRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            var regnskab = new Regnskab(1, "Ole Sørensen");
            var bankkonti = new Kontogruppe(1, "Bankkonto", CommonLibrary.Domain.Enums.KontogruppeType.Aktiver);
            var dankort = new Konto(regnskab, "DANKORT", "Dankort", bankkonti);
            Assert.Throws<IntranetRepositoryException>(() => repository.BogføringslinjeAdd(new DateTime(2011, 4, 1), null, dankort, "Test", null, 0M, 0M, null));
        }

        /// <summary>
        /// Tester, at BogføringslinjeAdd kaster en IntranetRepositoryException ved FaultException.
        /// </summary>
        [Test]
        public void TestAtBogføringslinjeAddKasterIntranetRepositoryExceptionVedFaultException()
        {
            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFinansstyringRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.BogføringslinjeAdd(Arg<BogføringslinjeAddCommand>.Is.Anything))
                .Throw(new FaultException("Test"));
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed);
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFinansstyringRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            var regnskab = new Regnskab(1, "Ole Sørensen");
            var bankkonti = new Kontogruppe(1, "Bankkonto", CommonLibrary.Domain.Enums.KontogruppeType.Aktiver);
            var dankort = new Konto(regnskab, "DANKORT", "Dankort", bankkonti);
            Assert.Throws<IntranetRepositoryException>(() => repository.BogføringslinjeAdd(new DateTime(2011, 4, 1), null, dankort, "Test", null, 0M, 0M, null));
        }

        /// <summary>
        /// Tester, at BogføringslinjeAdd kaster en IntranetRepositoryException ved Exception.
        /// </summary>
        [Test]
        public void TestAtBogføringslinjeAddKasterIntranetRepositoryExceptionVedException()
        {
            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFinansstyringRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.BogføringslinjeAdd(Arg<BogføringslinjeAddCommand>.Is.Anything))
                .Throw(new Exception("Test"));
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed);
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFinansstyringRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            var regnskab = new Regnskab(1, "Ole Sørensen");
            var bankkonti = new Kontogruppe(1, "Bankkonto", CommonLibrary.Domain.Enums.KontogruppeType.Aktiver);
            var dankort = new Konto(regnskab, "DANKORT", "Dankort", bankkonti);
            Assert.Throws<IntranetRepositoryException>(() => repository.BogføringslinjeAdd(new DateTime(2011, 4, 1), null, dankort, "Test", null, 0M, 0M, null));
        }

        /// <summary>
        /// Tester, at MapRegnskab kaster en ArgumentNullException, hvis regnskabsviewet er null.
        /// </summary>
        [Test]
        public void TestAtMapRegnskabKasterArgumentNullExceptionHvisRegnskabViewErNull()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            var method = repository.GetType().GetMethod("MapRegnskab", BindingFlags.NonPublic | BindingFlags.Static,
                                                        null, new[]
                                                                  {
                                                                      typeof (RegnskabView),
                                                                      typeof (IEnumerable<Kontogruppe>),
                                                                      typeof (IEnumerable<Budgetkontogruppe>),
                                                                      typeof (Func<int, AdresseBase>)
                                                                  }, null);
            Assert.That(method, Is.Not.Null);
            Assert.That(
                Assert.Throws<TargetInvocationException>(
                    () => method.Invoke(repository, new object[] {null, null, null, null})).InnerException,
                Is.TypeOf(typeof (ArgumentNullException)));
        }

        /// <summary>
        /// Tester, at MapRegnskab kaster en ArgumentNullException, hvis kontogrupper er null.
        /// </summary>
        [Test]
        public void TestAtMapRegnskabKasterArgumentNullExceptionHvisKontogrupperErNull()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            var method = repository.GetType().GetMethod("MapRegnskab", BindingFlags.NonPublic | BindingFlags.Static,
                                                        null, new[]
                                                                  {
                                                                      typeof (RegnskabView),
                                                                      typeof (IEnumerable<Kontogruppe>),
                                                                      typeof (IEnumerable<Budgetkontogruppe>),
                                                                      typeof (Func<int, AdresseBase>)
                                                                  }, null);
            Assert.That(method, Is.Not.Null);
            Assert.That(
                Assert.Throws<TargetInvocationException>(
                    () => method.Invoke(repository, new object[] {new RegnskabView(), null, null, null})).InnerException,
                Is.TypeOf(typeof (ArgumentNullException)));
        }

        /// <summary>
        /// Tester, at MapRegnskab kaster en ArgumentNullException, hvis grupper af budgetkonti er null.
        /// </summary>
        [Test]
        public void TestAtMapRegnskabKasterArgumentNullExceptionHvisBudgetkontogrupperErNull()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            var method = repository.GetType().GetMethod("MapRegnskab", BindingFlags.NonPublic | BindingFlags.Static,
                                                        null, new[]
                                                                  {
                                                                      typeof (RegnskabView),
                                                                      typeof (IEnumerable<Kontogruppe>),
                                                                      typeof (IEnumerable<Budgetkontogruppe>),
                                                                      typeof (Func<int, AdresseBase>)
                                                                  }, null);
            Assert.That(method, Is.Not.Null);
            Assert.That(
                Assert.Throws<TargetInvocationException>(
                    () =>
                    method.Invoke(repository, new object[] {new RegnskabView(), new List<Kontogruppe>(), null, null})).
                    InnerException, Is.TypeOf(typeof (ArgumentNullException)));
        }

        /// <summary>
        /// Tester, at MapKonto kaster en ArgumentNullException, hvis regnskab er null.
        /// </summary>
        [Test]
        public void TestAtMapKontoKasterArgumentNullExceptionHvisRengskabErNull()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            var method = repository.GetType().GetMethod("MapKonto", BindingFlags.NonPublic | BindingFlags.Static,
                                                        null,
                                                        new[]
                                                            {
                                                                typeof (Regnskab), typeof (KontoListeView),
                                                                typeof (IEnumerable<Kontogruppe>)
                                                            }, null);
            Assert.That(method, Is.Not.Null);
            var m = method;
            Assert.That(
                Assert.Throws<TargetInvocationException>(
                    () => m.Invoke(repository, new object[] {null, null, null})).InnerException,
                Is.TypeOf(typeof (ArgumentNullException)));
            method = repository.GetType().GetMethod("MapKonto", BindingFlags.NonPublic | BindingFlags.Static,
                                                    null,
                                                    new[]
                                                        {
                                                            typeof (Regnskab), typeof (KontoView),
                                                            typeof (IEnumerable<Kontogruppe>)
                                                        }, null);
            Assert.That(method, Is.Not.Null);
            Assert.That(
                Assert.Throws<TargetInvocationException>(
                    () => method.Invoke(repository, new object[] {null, null, null})).InnerException,
                Is.TypeOf(typeof (ArgumentNullException)));
        }

        /// <summary>
        /// Tester, at MapKonto kaster en ArgumentNullException, hvis kontolisteviewet er null.
        /// </summary>
        [Test]
        public void TestAtMapKontoKasterArgumentNullExceptionHvisKontoListeViewErNull()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            var method = repository.GetType().GetMethod("MapKonto", BindingFlags.NonPublic | BindingFlags.Static,
                                                        null,
                                                        new[]
                                                            {
                                                                typeof (Regnskab), typeof (KontoListeView),
                                                                typeof (IEnumerable<Kontogruppe>)
                                                            }, null);
            Assert.That(method, Is.Not.Null);
            Assert.That(
                Assert.Throws<TargetInvocationException>(
                    () => method.Invoke(repository, new object[] {new Regnskab(1, "Ole Sørensen"), null, null})).
                    InnerException, Is.TypeOf(typeof (ArgumentNullException)));
        }

        /// <summary>
        /// Tester, at MapKonto kaster en ArgumentNullException, hvis kontoviewet er null.
        /// </summary>
        [Test]
        public void TestAtMapKontoKasterArgumentNullExceptionHvisKontoViewErNull()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            var method = repository.GetType().GetMethod("MapKonto", BindingFlags.NonPublic | BindingFlags.Static,
                                                        null,
                                                        new[]
                                                            {
                                                                typeof (Regnskab), typeof (KontoView),
                                                                typeof (IEnumerable<Kontogruppe>)
                                                            }, null);
            Assert.That(method, Is.Not.Null);
            Assert.That(
                Assert.Throws<TargetInvocationException>(
                    () => method.Invoke(repository, new object[] {new Regnskab(1, "Ole Sørensen"), null, null})).
                    InnerException, Is.TypeOf(typeof (ArgumentNullException)));
        }

        /// <summary>
        /// Tester, at MapKonto kaster en ArgumentNullException, hvis kontogrupper er null.
        /// </summary>
        [Test]
        public void TestAtMapKontoKasterArgumentNullExceptionHvisKontogrupperErNull()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            var method = repository.GetType().GetMethod("MapKonto", BindingFlags.NonPublic | BindingFlags.Static,
                                                        null,
                                                        new[]
                                                            {
                                                                typeof (Regnskab), typeof (KontoListeView),
                                                                typeof (IEnumerable<Kontogruppe>)
                                                            }, null);
            Assert.That(method, Is.Not.Null);
            var m = method;
            Assert.That(
                Assert.Throws<TargetInvocationException>(
                    () =>
                    m.Invoke(repository, new object[] {new Regnskab(1, "Ole Sørensen"), new KontoListeView(), null}))
                    .InnerException, Is.TypeOf(typeof (ArgumentNullException)));
            method = repository.GetType().GetMethod("MapKonto", BindingFlags.NonPublic | BindingFlags.Static,
                                                    null,
                                                    new[]
                                                        {
                                                            typeof (Regnskab), typeof (KontoView),
                                                            typeof (IEnumerable<Kontogruppe>)
                                                        }, null);
            Assert.That(method, Is.Not.Null);
            Assert.That(
                Assert.Throws<TargetInvocationException>(
                    () =>
                    method.Invoke(repository, new object[] {new Regnskab(1, "Ole Sørensen"), new KontoView(), null})).
                    InnerException, Is.TypeOf(typeof (ArgumentNullException)));
        }

        /// <summary>
        /// Tester, at MapKonto kaster en IntranetRepositoryException, hvis kontogruppen ikke findes.
        /// </summary>
        [Test]
        public void TestAtMapKontoKasterIntranetRepositoryExceptionHvisKontogruppeIkkeFindes()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            var method = repository.GetType().GetMethod("MapKonto", BindingFlags.NonPublic | BindingFlags.Static,
                                                        null,
                                                        new[]
                                                            {
                                                                typeof (Regnskab), typeof (KontoListeView),
                                                                typeof (IEnumerable<Kontogruppe>)
                                                            }, null);
            Assert.That(method, Is.Not.Null);
            Assert.That(
                Assert.Throws<TargetInvocationException>(
                    () =>
                    method.Invoke(repository,
                                  new object[]
                                      {
                                          new Regnskab(1, "Ole Sørensen"),
                                          new KontoListeView {Kontogruppe = new KontogruppeView {Nummer = 1}},
                                          new List<Kontogruppe>()
                                      })).InnerException,
                Is.TypeOf(typeof (IntranetRepositoryException)));
        }

        /// <summary>
        /// Tester, at MapKreditoplysninger kaster en ArgumentNullException, hvis kreditoplysningsviewet er null.
        /// </summary>
        [Test]
        public void TestAtMapKreditoplysningerKasterArgumentNullExceptionHvisKontogrupperErNull()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            var method = repository.GetType().GetMethod("MapKreditoplysninger",
                                                        BindingFlags.NonPublic | BindingFlags.Static);
            Assert.That(method, Is.Not.Null);
            Assert.That(
                Assert.Throws<TargetInvocationException>(() => method.Invoke(repository, new object[] {null})).
                    InnerException, Is.TypeOf(typeof (ArgumentNullException)));
        }

        /// <summary>
        /// Tester, at MapBudgetkonto kaster en ArgumentNullException, hvis regnskab er null.
        /// </summary>
        [Test]
        public void TestAtMapBudgetkontoKasterArgumentNullExceptionHvisRengskabErNull()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            var method = repository.GetType().GetMethod("MapBudgetkonto", BindingFlags.NonPublic | BindingFlags.Static,
                                                        null,
                                                        new[]
                                                            {
                                                                typeof (Regnskab), typeof (BudgetkontoListeView),
                                                                typeof (IEnumerable<Budgetkontogruppe>)
                                                            }, null);
            Assert.That(method, Is.Not.Null);
            var m = method;
            Assert.That(
                Assert.Throws<TargetInvocationException>(() => m.Invoke(repository, new object[] {null, null, null})).
                    InnerException, Is.TypeOf(typeof (ArgumentNullException)));
            method = repository.GetType().GetMethod("MapBudgetkonto", BindingFlags.NonPublic | BindingFlags.Static,
                                                    null,
                                                    new[]
                                                        {
                                                            typeof (Regnskab), typeof (BudgetkontoView),
                                                            typeof (IEnumerable<Budgetkontogruppe>)
                                                        }, null);
            Assert.That(method, Is.Not.Null);
            Assert.That(
                Assert.Throws<TargetInvocationException>(
                    () => method.Invoke(repository, new object[] {null, null, null})).InnerException,
                Is.TypeOf(typeof (ArgumentNullException)));
        }

        /// <summary>
        /// Tester, at MapBudgetkonto kaster en ArgumentNullException, hvis budgetkontolisteviewet er null.
        /// </summary>
        [Test]
        public void TestAtMapBudgetkontoKasterArgumentNullExceptionHvisBudgetkontoListeViewErNull()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            var method = repository.GetType().GetMethod("MapBudgetkonto", BindingFlags.NonPublic | BindingFlags.Static,
                                                        null,
                                                        new[]
                                                            {
                                                                typeof (Regnskab), typeof (BudgetkontoListeView),
                                                                typeof (IEnumerable<Budgetkontogruppe>)
                                                            }, null);
            Assert.That(method, Is.Not.Null);
            Assert.That(
                Assert.Throws<TargetInvocationException>(
                    () => method.Invoke(repository, new object[] {new Regnskab(1, "Ole Sørensen"), null, null})).
                    InnerException, Is.TypeOf(typeof (ArgumentNullException)));
        }

        /// <summary>
        /// Tester, at MapBudgetkonto kaster en ArgumentNullException, hvis budgetkontoviewet er null.
        /// </summary>
        [Test]
        public void TestAtMapBudgetkontoKasterArgumentNullExceptionHvisBudgetkontoViewErNull()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            var method = repository.GetType().GetMethod("MapBudgetkonto", BindingFlags.NonPublic | BindingFlags.Static,
                                                        null,
                                                        new[]
                                                            {
                                                                typeof (Regnskab), typeof (BudgetkontoView),
                                                                typeof (IEnumerable<Budgetkontogruppe>)
                                                            }, null);
            Assert.That(method, Is.Not.Null);
            Assert.That(
                Assert.Throws<TargetInvocationException>(
                    () => method.Invoke(repository, new object[] {new Regnskab(1, "Ole Sørensen"), null, null})).
                    InnerException, Is.TypeOf(typeof (ArgumentNullException)));
        }

        /// <summary>
        /// Tester, at MapBudgetkonto kaster en ArgumentNullException, hvis grupper af budgetkonti er null.
        /// </summary>
        [Test]
        public void TestAtMapBudgetkontoKasterArgumentNullExceptionHvisBudgetkontogrupperErNull()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            var method = repository.GetType().GetMethod("MapBudgetkonto", BindingFlags.NonPublic | BindingFlags.Static,
                                                        null,
                                                        new[]
                                                            {
                                                                typeof (Regnskab), typeof (BudgetkontoListeView),
                                                                typeof (IEnumerable<Budgetkontogruppe>)
                                                            }, null);
            Assert.That(method, Is.Not.Null);
            var m = method;
            Assert.That(
                Assert.Throws<TargetInvocationException>(
                    () =>
                    m.Invoke(repository,
                             new object[] {new Regnskab(1, "Ole Sørensen"), new BudgetkontoListeView(), null})).
                    InnerException, Is.TypeOf(typeof (ArgumentNullException)));
            method = repository.GetType().GetMethod("MapBudgetkonto", BindingFlags.NonPublic | BindingFlags.Static,
                                                    null,
                                                    new[]
                                                        {
                                                            typeof (Regnskab), typeof (BudgetkontoView),
                                                            typeof (IEnumerable<Budgetkontogruppe>)
                                                        }, null);
            Assert.That(method, Is.Not.Null);
            Assert.That(
                Assert.Throws<TargetInvocationException>(
                    () =>
                    method.Invoke(repository,
                                  new object[] {new Regnskab(1, "Ole Sørensen"), new BudgetkontoView(), null})).
                    InnerException, Is.TypeOf(typeof (ArgumentNullException)));
        }

        /// <summary>
        /// Tester, at MapBudgetkonto kaster en IntranetRepositoryException, hvis budgetkontogruppen ikke findes.
        /// </summary>
        [Test]
        public void TestAtMapBudgetkontoKasterIntranetRepositoryExceptionHvisBudgetkontogruppeIkkeFindes()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            var method = repository.GetType().GetMethod("MapBudgetkonto", BindingFlags.NonPublic | BindingFlags.Static,
                                                        null,
                                                        new[]
                                                            {
                                                                typeof (Regnskab), typeof (BudgetkontoListeView),
                                                                typeof (IEnumerable<Budgetkontogruppe>)
                                                            }, null);
            Assert.That(method, Is.Not.Null);
            Assert.That(
                Assert.Throws<TargetInvocationException>(
                    () =>
                    method.Invoke(repository,
                                  new object[]
                                      {
                                          new Regnskab(1, "Ole Sørensen"),
                                          new BudgetkontoListeView
                                              {Budgetkontogruppe = new BudgetkontogruppeView {Nummer = 1}},
                                          new List<Budgetkontogruppe>()
                                      })).InnerException,
                Is.TypeOf(typeof (IntranetRepositoryException)));
        }

        /// <summary>
        /// Tester, at MapBudgetoplysninger kaster en ArgumentNullException, hvis budgetoplysningsviewet er null.
        /// </summary>
        [Test]
        public void TestAtMapBudgetoplysningerKasterArgumentNullExceptionHvisBudgetoplysningViewErNull()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            var method = repository.GetType().GetMethod("MapBudgetoplysninger",
                                                        BindingFlags.NonPublic | BindingFlags.Static);
            Assert.That(method, Is.Not.Null);
            Assert.That(
                Assert.Throws<TargetInvocationException>(() => method.Invoke(repository, new object[] {null})).
                    InnerException, Is.TypeOf(typeof (ArgumentNullException)));
        }

        /// <summary>
        /// Tester, at MapBogføringslinje kaster en ArgumentNullException, hvis bogføringslinjeviewet er null.
        /// </summary>
        [Test]
        public void TestAtMapBogføringslinjeKasterArgumentNullExceptionHvisBogføringslinjeViewErNull()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            var method = repository.GetType().GetMethod("MapBogføringslinje",
                                                        BindingFlags.NonPublic | BindingFlags.Static);
            Assert.That(method, Is.Not.Null);
            Assert.That(
                Assert.Throws<TargetInvocationException>(
                    () => method.Invoke(repository, new object[] {null, null, null, null})).InnerException,
                Is.TypeOf(typeof (ArgumentNullException)));
        }

        /// <summary>
        /// Tester, at MapBogføringslinje kaster en ArgumentNullException, hvis konti er null.
        /// </summary>
        [Test]
        public void TestAtMapBogføringslinjeKasterArgumentNullExceptionHvisKontiErNull()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            var method = repository.GetType().GetMethod("MapBogføringslinje", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.That(method, Is.Not.Null);
            Assert.That(
                Assert.Throws<TargetInvocationException>(
                    () => method.Invoke(repository, new object[] {new BogføringslinjeView(), null, null, null})).
                    InnerException, Is.TypeOf(typeof (ArgumentNullException)));
        }

        /// <summary>
        /// Tester, at MapBogføringslinje kaster en ArgumentNullException, hvis budgetkonti er null.
        /// </summary>
        [Test]
        public void TestAtMapBogføringslinjeKasterArgumentNullExceptionHvisBudgetkontiErNull()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            var method = repository.GetType().GetMethod("MapBogføringslinje", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.That(method, Is.Not.Null);
            Assert.That(
                Assert.Throws<TargetInvocationException>(
                    () =>
                    method.Invoke(repository, new object[] {new BogføringslinjeView(), new List<Konto>(), null, null})).
                    InnerException, Is.TypeOf(typeof (ArgumentNullException)));
        }

        /// <summary>
        /// Tester, at MapBogføringslinje kaster en IntranetRepositoryException, hvis konto ikke findes.
        /// </summary>
        [Test]
        public void TestAtMapBogføringslinjeKasterIntranetRepositoryExceptionHvisKontoIkkeFindes()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            var method = repository.GetType().GetMethod("MapBogføringslinje",
                                                        BindingFlags.NonPublic | BindingFlags.Static);
            Assert.That(method, Is.Not.Null);
            var view = new BogføringslinjeView
                           {
                               Løbenummer = 1,
                               Dato = new DateTime(2011, 4, 1),
                               Konto = new KontoListeView
                                           {
                                               Kontonummer = "DANKORT"
                                           },
                               Tekst = "Test",
                               Budgetkonto = new BudgetkontoListeView
                                                 {
                                                     Kontonummer = "1000"
                                                 },
                               Debit = 1000M,
                               Adresse = new AdressereferenceView
                                             {
                                                 Nummer = 1
                                             }
                           };
            Assert.That(
                Assert.Throws<TargetInvocationException>(
                    () =>
                    method.Invoke(repository, new object[] {view, new List<Konto>(), new List<Budgetkonto>(), null})).
                    InnerException, Is.TypeOf(typeof (IntranetRepositoryException)));
        }

        /// <summary>
        /// Tester, at MapBogføringslinje kaster en IntranetRepositoryException, hvis budgetkonto ikke findes.
        /// </summary>
        [Test]
        public void TestAtMapBogføringslinjeKasterIntranetRepositoryExceptionHvisBudgetkontoIkkeFindes()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            var method = repository.GetType().GetMethod("MapBogføringslinje",
                                                        BindingFlags.NonPublic | BindingFlags.Static);
            Assert.That(method, Is.Not.Null);
            var view = new BogføringslinjeView
                           {
                               Løbenummer = 1,
                               Dato = new DateTime(2011, 4, 1),
                               Konto = new KontoListeView
                                           {
                                               Kontonummer = "DANKORT"
                                           },
                               Tekst = "Test",
                               Budgetkonto = new BudgetkontoListeView
                                                 {
                                                     Kontonummer = "1000"
                                                 },
                               Debit = 1000M,
                               Adresse = new AdressereferenceView
                                             {
                                                 Nummer = 1
                                             }
                           };

            var regnskab = new Regnskab(1, "Ole Sørensen");
            var kontogruppe = new Kontogruppe(1, "Bankkonti", CommonLibrary.Domain.Enums.KontogruppeType.Aktiver);
            var kontoDankort = new Konto(regnskab, "DANKORT", "Dankort", kontogruppe);
            Assert.That(
                Assert.Throws<TargetInvocationException>(
                    () =>
                    method.Invoke(repository,
                                  new object[] {view, new List<Konto> {kontoDankort}, new List<Budgetkonto>(), null})).
                    InnerException, Is.TypeOf(typeof (IntranetRepositoryException)));
        }

        /// <summary>
        /// Tester, at MapBogføringslinje kaster en IntranetRepositoryException, hvis adressen ikke findes.
        /// </summary>
        [Test]
        public void TestAtMapBogføringslinjeKasterIntranetRepositoryExceptionHvisAdresseIkkeFindes()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            var method = repository.GetType().GetMethod("MapBogføringslinje",
                                                        BindingFlags.NonPublic | BindingFlags.Static);
            Assert.That(method, Is.Not.Null);
            var view = new BogføringslinjeView
                           {
                               Løbenummer = 1,
                               Dato = new DateTime(2011, 4, 1),
                               Konto = new KontoListeView
                                           {
                                               Kontonummer = "DANKORT"
                                           },
                               Tekst = "Test",
                               Budgetkonto = new BudgetkontoListeView
                                                 {
                                                     Kontonummer = "1000"
                                                 },
                               Debit = 1000M,
                               Adresse = new AdressereferenceView
                                             {
                                                 Nummer = 1
                                             }
                           };

            var regnskab = new Regnskab(1, "Ole Sørensen");
            var kontogruppe = new Kontogruppe(1, "Bankkonti", CommonLibrary.Domain.Enums.KontogruppeType.Aktiver);
            var kontoDankort = new Konto(regnskab, "DANKORT", "Dankort", kontogruppe);
            var budgetkontogruppe = new Budgetkontogruppe(1, "Indtægter");
            var budgetkontoIndtægter = new Budgetkonto(regnskab, "1000", "Indtægter", budgetkontogruppe);
            var callback = new Func<int, AdresseBase>(m => null);
            Assert.That(
                Assert.Throws<TargetInvocationException>(
                    () =>
                    method.Invoke(repository,
                                  new object[]
                                      {
                                          view, new List<Konto> {kontoDankort},
                                          new List<Budgetkonto> {budgetkontoIndtægter}, callback
                                      })).InnerException,
                Is.TypeOf(typeof (IntranetRepositoryException)));
        }
    }
}
