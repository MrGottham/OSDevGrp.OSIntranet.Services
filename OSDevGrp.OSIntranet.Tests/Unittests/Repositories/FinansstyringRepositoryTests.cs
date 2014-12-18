using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Fælles;
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
        /// Tester, at RegnskabslisteGet kaster en ArgumentNullException, hvis callbackmetoden til at hente et givent brevhoved er null.
        /// </summary>
        [Test]
        public void TestAtRegnskabslisteGetKasterArgumentNullExceptionHvisGetBrevhovedCallbackErNull()
        {
            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFinansstyringRepositoryService>(new[] {typeof (ICommunicationObject)});
            Expect.Call(((ICommunicationObject) service).State).Return(CommunicationState.Closed).Repeat.Any();
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFinansstyringRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            Assert.Throws<ArgumentNullException>(() => repository.RegnskabslisteGet(null));
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
                m => m.BuildMany<RegnskabListeView, Regnskab>(Arg<IEnumerable<RegnskabListeView>>.Is.NotNull))
                .Return(fixture.CreateMany<Regnskab>(3));

            var getBrevhovedCallback = new Func<int, Brevhoved>(nummer => fixture.Create<Brevhoved>());
            Assert.That(getBrevhovedCallback, Is.Not.Null);
            Assert.That(getBrevhovedCallback(1), Is.Not.Null);

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            var regnskaber = repository.RegnskabslisteGet(getBrevhovedCallback);
            Assert.That(regnskaber, Is.Not.Null);
            Assert.That(regnskaber.Count(), Is.EqualTo(3));

            domainObjectBuilder.AssertWasCalled(
                m => m.BuildMany<RegnskabListeView, Regnskab>(Arg<IEnumerable<RegnskabListeView>>.Is.NotNull));
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
                .Throw(fixture.Create<IntranetRepositoryException>());
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed).Repeat.Any();
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFinansstyringRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var getBrevhovedCallback = new Func<int, Brevhoved>(nummer => fixture.Create<Brevhoved>());
            Assert.That(getBrevhovedCallback, Is.Not.Null);
            Assert.That(getBrevhovedCallback(1), Is.Not.Null);

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            Assert.Throws<IntranetRepositoryException>(() => repository.RegnskabslisteGet(getBrevhovedCallback));
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
                .Throw(fixture.Create<FaultException>());
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed).Repeat.Any();
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFinansstyringRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var getBrevhovedCallback = new Func<int, Brevhoved>(nummer => fixture.Create<Brevhoved>());
            Assert.That(getBrevhovedCallback, Is.Not.Null);
            Assert.That(getBrevhovedCallback(1), Is.Not.Null);

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            Assert.Throws<IntranetRepositoryException>(() => repository.RegnskabslisteGet(getBrevhovedCallback));
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
                .Throw(fixture.Create<Exception>());
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed).Repeat.Any();
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFinansstyringRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var getBrevhovedCallback = new Func<int, Brevhoved>(nummer => fixture.Create<Brevhoved>());
            Assert.That(getBrevhovedCallback, Is.Not.Null);
            Assert.That(getBrevhovedCallback(1), Is.Not.Null);

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            Assert.Throws<IntranetRepositoryException>(() => repository.RegnskabslisteGet(getBrevhovedCallback));
        }

        /// <summary>
        /// Tester, at RegnskabGet kaster en ArgumentNullException, hvis callbackmetoden til at hente et givent brevhoved er null.
        /// </summary>
        [Test]
        public void TestAtRegnskabGetKasterArgumentNullExceptionHvisGetBrevhovedCallbackErNull()
        {
            var fixture = new Fixture();

            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFinansstyringRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.RegnskabGetByNummer(Arg<RegnskabGetByNummerQuery>.Is.Anything))
                .Throw(fixture.Create<IntranetRepositoryException>());
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
            Assert.Throws<ArgumentNullException>(() => repository.RegnskabGet(fixture.Create<int>(), null, null));
        }

        /// <summary>
        /// Tester, at RegnskabGet kaster en ArgumentNullException, hvis callbackmetoden til at hente en given adresse er null.
        /// </summary>
        [Test]
        public void TestAtRegnskabGetKasterArgumentNullExceptionHvisGetAdresseCallbackErNull()
        {
            var fixture = new Fixture();

            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFinansstyringRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.RegnskabGetByNummer(Arg<RegnskabGetByNummerQuery>.Is.Anything))
                .Throw(fixture.Create<IntranetRepositoryException>());
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

            var getBrevhovedCallback = new Func<int, Brevhoved>(nummer => fixture.Create<Brevhoved>());
            Assert.That(getBrevhovedCallback, Is.Not.Null);
            Assert.That(getBrevhovedCallback(1), Is.Not.Null);

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            Assert.Throws<ArgumentNullException>(() => repository.RegnskabGet(fixture.Create<int>(), getBrevhovedCallback, null));
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
            var service = mocker.DynamicMultiMock<IFinansstyringRepositoryService>(new[] {typeof (ICommunicationObject)});
            service.Expect(m => m.RegnskabGetByNummer(Arg<RegnskabGetByNummerQuery>.Is.Anything))
                .Return(fixture.Create<RegnskabView>());
            service.Expect(m => m.KontogruppeGetAll(Arg<KontogruppeGetAllQuery>.Is.Anything))
                .Return(fixture.CreateMany<KontogruppeView>(3));
            service.Expect(m => m.BudgetkontogruppeGetAll(Arg<BudgetkontogruppeGetAllQuery>.Is.Anything))
                .Return(fixture.CreateMany<BudgetkontogruppeView>(3));
            Expect.Call(((ICommunicationObject) service).State).Return(CommunicationState.Closed);
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFinansstyringRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();
            domainObjectBuilder.Expect(
                m => m.BuildMany<KontogruppeView, Kontogruppe>(Arg<IEnumerable<KontogruppeView>>.Is.NotNull))
                .Return(fixture.CreateMany<Kontogruppe>(3));
            domainObjectBuilder.Expect(
                m =>
                m.BuildMany<BudgetkontogruppeView, Budgetkontogruppe>(Arg<IEnumerable<BudgetkontogruppeView>>.Is.NotNull))
                .Return(fixture.CreateMany<Budgetkontogruppe>(3));
            domainObjectBuilder.Expect(m => m.Build<RegnskabView, Regnskab>(Arg<RegnskabView>.Is.NotNull))
                .Return(fixture.Create<Regnskab>());

            var getBrevhovedCallback = new Func<int, Brevhoved>(nummer => fixture.Create<Brevhoved>());
            Assert.That(getBrevhovedCallback, Is.Not.Null);
            Assert.That(getBrevhovedCallback(1), Is.Not.Null);
            var getAdresseCallback = new Func<int, AdresseBase>(nummer => fixture.Create<Person>());
            Assert.That(getAdresseCallback, Is.Not.Null);
            Assert.That(getAdresseCallback(1), Is.Not.Null);

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            var regnskab = repository.RegnskabGet(fixture.Create<int>(), getBrevhovedCallback, getAdresseCallback);
            Assert.That(regnskab, Is.Not.Null);

            domainObjectBuilder.AssertWasCalled(
                m => m.BuildMany<KontogruppeView, Kontogruppe>(Arg<IEnumerable<KontogruppeView>>.Is.NotNull));
            domainObjectBuilder.AssertWasCalled(
                m =>
                m.BuildMany<BudgetkontogruppeView, Budgetkontogruppe>(Arg<IEnumerable<BudgetkontogruppeView>>.Is.NotNull));
            domainObjectBuilder.AssertWasCalled(m => m.Build<RegnskabView, Regnskab>(Arg<RegnskabView>.Is.NotNull));
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
                .Throw(fixture.Create<IntranetRepositoryException>());
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

            var getBrevhovedCallback = new Func<int, Brevhoved>(nummer => fixture.Create<Brevhoved>());
            Assert.That(getBrevhovedCallback, Is.Not.Null);
            Assert.That(getBrevhovedCallback(1), Is.Not.Null);
            var getAdresseCallback = new Func<int, AdresseBase>(nummer => fixture.Create<Person>());
            Assert.That(getAdresseCallback, Is.Not.Null);
            Assert.That(getAdresseCallback(1), Is.Not.Null);

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            Assert.Throws<IntranetRepositoryException>(() => repository.RegnskabGet(fixture.Create<int>(), getBrevhovedCallback, getAdresseCallback));
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
                .Throw(fixture.Create<FaultException>());
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

            var getBrevhovedCallback = new Func<int, Brevhoved>(nummer => fixture.Create<Brevhoved>());
            Assert.That(getBrevhovedCallback, Is.Not.Null);
            Assert.That(getBrevhovedCallback(1), Is.Not.Null);
            var getAdresseCallback = new Func<int, AdresseBase>(nummer => fixture.Create<Person>());
            Assert.That(getAdresseCallback, Is.Not.Null);
            Assert.That(getAdresseCallback(1), Is.Not.Null);

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            Assert.Throws<IntranetRepositoryException>(() => repository.RegnskabGet(fixture.Create<int>(), getBrevhovedCallback, getAdresseCallback));
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
                .Throw(fixture.Create<Exception>());
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

            var getBrevhovedCallback = new Func<int, Brevhoved>(nummer => fixture.Create<Brevhoved>());
            Assert.That(getBrevhovedCallback, Is.Not.Null);
            Assert.That(getBrevhovedCallback(1), Is.Not.Null);
            var getAdresseCallback = new Func<int, AdresseBase>(nummer => fixture.Create<Person>());
            Assert.That(getAdresseCallback, Is.Not.Null);
            Assert.That(getAdresseCallback(1), Is.Not.Null);

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            Assert.Throws<IntranetRepositoryException>(() => repository.RegnskabGet(fixture.Create<int>(), getBrevhovedCallback, getAdresseCallback));
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
                m => m.BuildMany<KontogruppeView, Kontogruppe>(Arg<IEnumerable<KontogruppeView>>.Is.NotNull))
                .Return(fixture.CreateMany<Kontogruppe>(3));

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            var kontogrupper = repository.KontogruppeGetAll();
            Assert.That(kontogrupper, Is.Not.Null);
            Assert.That(kontogrupper.Count(), Is.EqualTo(3));

            domainObjectBuilder.AssertWasCalled(
                m => m.BuildMany<KontogruppeView, Kontogruppe>(Arg<IEnumerable<KontogruppeView>>.Is.NotNull));
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
                .Throw(fixture.Create<IntranetRepositoryException>());
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
                .Throw(fixture.Create<FaultException>());
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
                .Throw(fixture.Create<Exception>());
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
            var service = mocker.DynamicMultiMock<IFinansstyringRepositoryService>(new[] {typeof (ICommunicationObject)});
            service.Expect(m => m.BudgetkontogruppeGetAll(Arg<BudgetkontogruppeGetAllQuery>.Is.Anything))
                .Return(fixture.CreateMany<BudgetkontogruppeView>(3));
            Expect.Call(((ICommunicationObject) service).State).Return(CommunicationState.Closed);
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFinansstyringRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();
            domainObjectBuilder.Expect(
                m =>
                m.BuildMany<BudgetkontogruppeView, Budgetkontogruppe>(Arg<IEnumerable<BudgetkontogruppeView>>.Is.NotNull))
                .Return(fixture.CreateMany<Budgetkontogruppe>(3));

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            var budgetkontogrupper = repository.BudgetkontogruppeGetAll();
            Assert.That(budgetkontogrupper, Is.Not.Null);
            Assert.That(budgetkontogrupper.Count(), Is.EqualTo(3));

            domainObjectBuilder.AssertWasCalled(
                m =>
                m.BuildMany<BudgetkontogruppeView, Budgetkontogruppe>(Arg<IEnumerable<BudgetkontogruppeView>>.Is.NotNull));
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
                .Throw(fixture.Create<IntranetRepositoryException>());
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
                .Throw(fixture.Create<FaultException>());
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
                .Throw(fixture.Create<Exception>());
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
            var fixture = new Fixture();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();
            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            Assert.Throws<ArgumentNullException>(
                () =>
                repository.BogføringslinjeAdd(fixture.Create<DateTime>(), null, null,
                                              fixture.Create<string>(), null,
                                              fixture.Create<decimal>(), fixture.Create<decimal>(),
                                              null));
        }

        /// <summary>
        /// Tester, at BogføringslinjeAdd kaster en ArgumentNullException, hvis tekst er null.
        /// </summary>
        [Test]
        public void TestAtBogføringslinjeAddKasterEnArgumentNullExceptionHvisTekstErNull()
        {
            var fixture = new Fixture();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();
            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            var konto = fixture.Create<Konto>();
            Assert.Throws<ArgumentNullException>(
                () =>
                repository.BogføringslinjeAdd(fixture.Create<DateTime>(), null, konto, null, null,
                                              fixture.Create<decimal>(), fixture.Create<decimal>(),
                                              null));
        }

        /// <summary>
        /// Tester, at BogføringslinjeAdd kalder servicemetode.
        /// </summary>
        [Test]
        public void TestAtBogføringslinjeAddKalderServicemetode()
        {
            var fixture = new Fixture();

            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFinansstyringRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.BogføringslinjeAdd(Arg<BogføringslinjeAddCommand>.Is.NotNull))
                .Return(fixture.Create<BogføringslinjeView>());
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed);
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFinansstyringRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            var konto = fixture.Create<Konto>();
            var budgetkonto = fixture.Create<Budgetkonto>();
            var adressekonto = fixture.Create<Person>();
            var result = repository.BogføringslinjeAdd(fixture.Create<DateTime>(), null, konto,
                                                       fixture.Create<string>(), budgetkonto,
                                                       fixture.Create<decimal>(),
                                                       fixture.Create<decimal>(), adressekonto);
            Assert.That(result, Is.Not.Null);

            Assert.That(konto.Bogføringslinjer.Count(), Is.EqualTo(1));
            Assert.That(budgetkonto.Bogføringslinjer.Count(), Is.EqualTo(1));
            Assert.That(adressekonto.Bogføringslinjer.Count(), Is.EqualTo(1));

            service.AssertWasCalled(m => m.BogføringslinjeAdd(Arg<BogføringslinjeAddCommand>.Is.NotNull));
        }

        /// <summary>
        /// Tester, at BogføringslinjeAdd kaster en IntranetRepositoryException ved IntranetRepositoryException.
        /// </summary>
        [Test]
        public void TestAtBogføringslinjeAddKasterIntranetRepositoryExceptionVedIntranetRepositoryException()
        {
            var fixture = new Fixture();

            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFinansstyringRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.BogføringslinjeAdd(Arg<BogføringslinjeAddCommand>.Is.Anything))
                .Throw(fixture.Create<IntranetRepositoryException>());
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed);
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFinansstyringRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            var konto = fixture.Create<Konto>();
            Assert.Throws<IntranetRepositoryException>(
                () =>
                repository.BogføringslinjeAdd(fixture.Create<DateTime>(), null, konto,
                                              fixture.Create<string>(), null,
                                              fixture.Create<decimal>(), fixture.Create<decimal>(),
                                              null));
        }

        /// <summary>
        /// Tester, at BogføringslinjeAdd kaster en IntranetRepositoryException ved FaultException.
        /// </summary>
        [Test]
        public void TestAtBogføringslinjeAddKasterIntranetRepositoryExceptionVedFaultException()
        {
            var fixture = new Fixture();

            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFinansstyringRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.BogføringslinjeAdd(Arg<BogføringslinjeAddCommand>.Is.Anything))
                .Throw(fixture.Create<FaultException>());
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed);
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFinansstyringRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            var konto = fixture.Create<Konto>();
            Assert.Throws<IntranetRepositoryException>(
                () =>
                repository.BogføringslinjeAdd(fixture.Create<DateTime>(), null, konto,
                                              fixture.Create<string>(), null,
                                              fixture.Create<decimal>(), fixture.Create<decimal>(),
                                              null));
        }

        /// <summary>
        /// Tester, at BogføringslinjeAdd kaster en IntranetRepositoryException ved Exception.
        /// </summary>
        [Test]
        public void TestAtBogføringslinjeAddKasterIntranetRepositoryExceptionVedException()
        {
            var fixture = new Fixture();

            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFinansstyringRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.BogføringslinjeAdd(Arg<BogføringslinjeAddCommand>.Is.Anything))
                .Throw(fixture.Create<Exception>());
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed);
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFinansstyringRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new FinansstyringRepository(channelFactory, domainObjectBuilder);
            var konto = fixture.Create<Konto>();
            Assert.Throws<IntranetRepositoryException>(
                () =>
                repository.BogføringslinjeAdd(fixture.Create<DateTime>(), null, konto,
                                              fixture.Create<string>(), null,
                                              fixture.Create<decimal>(), fixture.Create<decimal>(),
                                              null));
        }
    }
}
