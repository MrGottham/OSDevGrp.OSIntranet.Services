using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf.ChannelFactory;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Queries;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Services;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Views;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using NUnit.Framework;
using AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories
{
    /// <summary>
    /// Test af repository til adressekartoteket.
    /// </summary>
    [TestFixture]
    public class AdresseRepositoryTests
    {
        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis ChannelFactory er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisChannelFactoryErNull()
        {
            Assert.Throws<ArgumentNullException>(() => new AdresseRepository(null, null));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis domæneobjekt byggeren er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisDomainObjectBuilderErNull()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            Assert.Throws<ArgumentNullException>(() => new AdresseRepository(channelFactory, null));
        }

        /// <summary>
        /// Tester, at AdresseGetAll henter adresser.
        /// </summary>
        [Test]
        public void TestAtAdresseGetAllHenterAdresser()
        {
            var fixture = new Fixture();
            fixture.Inject<IEnumerable<PersonView>>(new List<PersonView>());
            fixture.Inject<IEnumerable<BogføringslinjeView>>(new List<BogføringslinjeView>());

            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IAdresseRepositoryService>(new[] {typeof (ICommunicationObject)});
            service.Expect(m => m.PersonGetAll(Arg<PersonGetAllQuery>.Is.Anything))
                .Return(fixture.CreateMany<PersonView>(3));
            service.Expect(m => m.FirmaGetAll(Arg<FirmaGetAllQuery>.Is.Anything))
                .Return(fixture.CreateMany<FirmaView>(3));
            service.Expect(m => m.AdressegruppeGetAll(Arg<AdressegruppeGetAllQuery>.Is.Anything))
                .Return(fixture.CreateMany<AdressegruppeView>(3));
            service.Expect(m => m.BetalingsbetingelseGetAll(Arg<BetalingsbetingelseGetAllQuery>.Is.Anything))
                .Return(fixture.CreateMany<BetalingsbetingelseView>(3));
            Expect.Call(((ICommunicationObject) service).State).Return(CommunicationState.Closed).Repeat.Any();
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IAdresseRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();
            domainObjectBuilder.Expect(
                m => m.BuildMany<AdressegruppeView, Adressegruppe>(Arg<IEnumerable<AdressegruppeView>>.Is.NotNull))
                .Return(fixture.CreateMany<Adressegruppe>(3));
            domainObjectBuilder.Expect(
                m =>
                m.BuildMany<BetalingsbetingelseView, Betalingsbetingelse>(
                    Arg<IEnumerable<BetalingsbetingelseView>>.Is.NotNull))
                .Return(fixture.CreateMany<Betalingsbetingelse>(3));
            domainObjectBuilder.Expect(m => m.BuildMany<FirmaView, AdresseBase>(Arg<IEnumerable<FirmaView>>.Is.NotNull))
                .Return(fixture.CreateMany<Firma>(3));
            domainObjectBuilder.Expect(
                m => m.BuildMany<PersonView, AdresseBase>(Arg<IEnumerable<PersonView>>.Is.NotNull))
                .Return(fixture.CreateMany<Person>(3));

            var repository = new AdresseRepository(channelFactory, domainObjectBuilder);
            var adresser = repository.AdresseGetAll();
            Assert.That(adresser, Is.Not.Null);
            Assert.That(adresser.Count(), Is.EqualTo(6));

            domainObjectBuilder.AssertWasCalled(
                m => m.BuildMany<AdressegruppeView, Adressegruppe>(Arg<IEnumerable<AdressegruppeView>>.Is.NotNull));
            domainObjectBuilder.AssertWasCalled(
                m =>
                m.BuildMany<BetalingsbetingelseView, Betalingsbetingelse>(
                    Arg<IEnumerable<BetalingsbetingelseView>>.Is.NotNull));
            domainObjectBuilder.AssertWasCalled(
                m => m.BuildMany<FirmaView, AdresseBase>(Arg<IEnumerable<FirmaView>>.Is.NotNull));
            domainObjectBuilder.AssertWasCalled(
                m => m.BuildMany<PersonView, AdresseBase>(Arg<IEnumerable<PersonView>>.Is.NotNull));
        }

        /// <summary>
        /// Tester, at AdresseGetAll kaster IntranetRepositoryException ved IntranetRepositoryException.
        /// </summary>
        [Test]
        public void TestAtAdresseGetAllKasterIntranetRepositoryExceptionVedIntranetRepositoryException()
        {
            var fixture = new Fixture();

            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IAdresseRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.PersonGetAll(Arg<PersonGetAllQuery>.Is.Anything))
                .Throw(fixture.Create<IntranetRepositoryException>());
            service.Expect(m => m.FirmaGetAll(Arg<FirmaGetAllQuery>.Is.Anything))
                .Throw(fixture.Create<IntranetRepositoryException>());
            service.Expect(m => m.AdressegruppeGetAll(Arg<AdressegruppeGetAllQuery>.Is.Anything))
                .Return(fixture.CreateMany<AdressegruppeView>(3));
            service.Expect(m => m.BetalingsbetingelseGetAll(Arg<BetalingsbetingelseGetAllQuery>.Is.Anything))
                .Return(fixture.CreateMany<BetalingsbetingelseView>(3));
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed).Repeat.Any();
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IAdresseRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new AdresseRepository(channelFactory, domainObjectBuilder);
            Assert.Throws<IntranetRepositoryException>(() => repository.AdresseGetAll());
        }

        /// <summary>
        /// Tester, at AdresseGetAll kaster IntranetRepositoryException ved FaultException.
        /// </summary>
        [Test]
        public void TestAtAdresseGetAllKasterIntranetRepositoryExceptionVedFaultException()
        {
            var fixture = new Fixture();

            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IAdresseRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.PersonGetAll(Arg<PersonGetAllQuery>.Is.Anything))
                .Throw(fixture.Create<FaultException>());
            service.Expect(m => m.FirmaGetAll(Arg<FirmaGetAllQuery>.Is.Anything))
                .Throw(fixture.Create<FaultException>());
            service.Expect(m => m.AdressegruppeGetAll(Arg<AdressegruppeGetAllQuery>.Is.Anything))
                .Return(fixture.CreateMany<AdressegruppeView>(3));
            service.Expect(m => m.BetalingsbetingelseGetAll(Arg<BetalingsbetingelseGetAllQuery>.Is.Anything))
                .Return(fixture.CreateMany<BetalingsbetingelseView>(3));
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed).Repeat.Any();
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IAdresseRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new AdresseRepository(channelFactory, domainObjectBuilder);
            Assert.Throws<IntranetRepositoryException>(() => repository.AdresseGetAll());
        }

        /// <summary>
        /// Tester, at AdresseGetAll kaster IntranetRepositoryException ved Exception.
        /// </summary>
        [Test]
        public void TestAtAdresseGetAllKasterIntranetRepositoryExceptionVedException()
        {
            var fixture = new Fixture();

            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IAdresseRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.PersonGetAll(Arg<PersonGetAllQuery>.Is.Anything))
                .Throw(fixture.Create<Exception>());
            service.Expect(m => m.FirmaGetAll(Arg<FirmaGetAllQuery>.Is.Anything))
                .Throw(fixture.Create<Exception>());
            service.Expect(m => m.AdressegruppeGetAll(Arg<AdressegruppeGetAllQuery>.Is.Anything))
                .Return(fixture.CreateMany<AdressegruppeView>(3));
            service.Expect(m => m.BetalingsbetingelseGetAll(Arg<BetalingsbetingelseGetAllQuery>.Is.Anything))
                .Return(fixture.CreateMany<BetalingsbetingelseView>(3));
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed).Repeat.Any();
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IAdresseRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new AdresseRepository(channelFactory, domainObjectBuilder);
            Assert.Throws<IntranetRepositoryException>(() => repository.AdresseGetAll());
        }

        /// <summary>
        /// Tester, at PostnummerGetAll henter postnumre.
        /// </summary>
        [Test]
        public void TestAtPostnummerGetAllHenterPostnumre()
        {
            var fixture = new Fixture();

            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IAdresseRepositoryService>(new[] {typeof (ICommunicationObject)});
            service.Expect(m => m.PostnummerGetAll(Arg<PostnummerGetAllQuery>.Is.Anything))
                .Return(fixture.CreateMany<PostnummerView>(3));
            Expect.Call(((ICommunicationObject) service).State).Return(CommunicationState.Closed).Repeat.Any();
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IAdresseRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();
            domainObjectBuilder.Expect(
                m => m.BuildMany<PostnummerView, Postnummer>(Arg<IEnumerable<PostnummerView>>.Is.NotNull))
                .Return(fixture.CreateMany<Postnummer>(3));

            var repository = new AdresseRepository(channelFactory, domainObjectBuilder);
            var postnumre = repository.PostnummerGetAll();
            Assert.That(postnumre, Is.Not.Null);
            Assert.That(postnumre.Count(), Is.EqualTo(3));

            domainObjectBuilder.AssertWasCalled(
                m => m.BuildMany<PostnummerView, Postnummer>(Arg<IEnumerable<PostnummerView>>.Is.NotNull));
        }

        /// <summary>
        /// Tester, at PostnummerGetAll kaster IntranetRepositoryException ved IntranetRepositoryException.
        /// </summary>
        [Test]
        public void TestAtPostnummerGetAllKasterIntranetRepositoryExceptionVedIntranetRepositoryException()
        {
            var fixture = new Fixture();

            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IAdresseRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.PostnummerGetAll(Arg<PostnummerGetAllQuery>.Is.Anything))
                .Throw(fixture.Create<IntranetRepositoryException>());
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed).Repeat.Any();
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IAdresseRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new AdresseRepository(channelFactory, domainObjectBuilder);
            Assert.Throws<IntranetRepositoryException>(() => repository.PostnummerGetAll());
        }

        /// <summary>
        /// Tester, at PostnummerGetAll kaster IntranetRepositoryException ved FaultException.
        /// </summary>
        [Test]
        public void TestAtPostnummerGetAllKasterIntranetRepositoryExceptionVedFaultException()
        {
            var fixture = new Fixture();

            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IAdresseRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.PostnummerGetAll(Arg<PostnummerGetAllQuery>.Is.Anything))
                .Throw(fixture.Create<FaultException>());
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed).Repeat.Any();
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IAdresseRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new AdresseRepository(channelFactory, domainObjectBuilder);
            Assert.Throws<IntranetRepositoryException>(() => repository.PostnummerGetAll());
        }

        /// <summary>
        /// Tester, at PostnummerGetAll kaster IntranetRepositoryException ved Exception.
        /// </summary>
        [Test]
        public void TestAtPostnummerGetAllKasterIntranetRepositoryExceptionVedException()
        {
            var fixture = new Fixture();

            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IAdresseRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.PostnummerGetAll(Arg<PostnummerGetAllQuery>.Is.Anything))
                .Throw(fixture.Create<Exception>());
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed).Repeat.Any();
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IAdresseRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new AdresseRepository(channelFactory, domainObjectBuilder);
            Assert.Throws<IntranetRepositoryException>(() => repository.PostnummerGetAll());
        }

        /// <summary>
        /// Tester, at AdressegruppeGetAll henter alle adressegrupper.
        /// </summary>
        [Test]
        public void TestAtAdressegruppeGetAllHenterAdressegrupper()
        {
            var fixture = new Fixture();

            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IAdresseRepositoryService>(new[] {typeof (ICommunicationObject)});
            service.Expect(m => m.AdressegruppeGetAll(Arg<AdressegruppeGetAllQuery>.Is.Anything))
                .Return(fixture.CreateMany<AdressegruppeView>(3));
            Expect.Call(((ICommunicationObject) service).State).Return(CommunicationState.Closed).Repeat.Any();
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IAdresseRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();
            domainObjectBuilder.Expect(
                m => m.BuildMany<AdressegruppeView, Adressegruppe>(Arg<IEnumerable<AdressegruppeView>>.Is.NotNull))
                .Return(fixture.CreateMany<Adressegruppe>(3));

            var repository = new AdresseRepository(channelFactory, domainObjectBuilder);
            var adressegrupper = repository.AdressegruppeGetAll();
            Assert.That(adressegrupper, Is.Not.Null);
            Assert.That(adressegrupper.Count(), Is.EqualTo(3));

            domainObjectBuilder.AssertWasCalled(
                m => m.BuildMany<AdressegruppeView, Adressegruppe>(Arg<IEnumerable<AdressegruppeView>>.Is.NotNull));
        }

        /// <summary>
        /// Tester, at AdressegruppeGetAll kaster en IntranetRepositoryException ved IntranetRepositoryException.
        /// </summary>
        [Test]
        public void TestAtAdressegruppeGetAllKasterIntranetRepositoryExceptionVedIntranetRepositoryException()
        {
            var fixture = new Fixture();

            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IAdresseRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.AdressegruppeGetAll(Arg<AdressegruppeGetAllQuery>.Is.Anything))
                .Throw(fixture.Create<IntranetRepositoryException>());
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed).Repeat.Any();
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IAdresseRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new AdresseRepository(channelFactory, domainObjectBuilder);
            Assert.Throws<IntranetRepositoryException>(() => repository.AdressegruppeGetAll());
        }

        /// <summary>
        /// Tester, at AdressegruppeGetAll kaster en IntranetRepositoryException ved FaultException.
        /// </summary>
        [Test]
        public void TestAtAdressegruppeGetAllKasterIntranetRepositoryExceptionVedFaultException()
        {
            var fixture = new Fixture();

            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IAdresseRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.AdressegruppeGetAll(Arg<AdressegruppeGetAllQuery>.Is.Anything))
                .Throw(fixture.Create<FaultException>());
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed).Repeat.Any();
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IAdresseRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new AdresseRepository(channelFactory, domainObjectBuilder);
            Assert.Throws<IntranetRepositoryException>(() => repository.AdressegruppeGetAll());
        }

        /// <summary>
        /// Tester, at AdressegruppeGetAll kaster en IntranetRepositoryException ved Exception.
        /// </summary>
        [Test]
        public void TestAtAdressegruppeGetAllKasterIntranetRepositoryExceptionVedException()
        {
            var fixture = new Fixture();

            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IAdresseRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.AdressegruppeGetAll(Arg<AdressegruppeGetAllQuery>.Is.Anything))
                .Throw(fixture.Create<Exception>());
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed).Repeat.Any();
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IAdresseRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new AdresseRepository(channelFactory, domainObjectBuilder);
            Assert.Throws<IntranetRepositoryException>(() => repository.AdressegruppeGetAll());
        }

        /// <summary>
        /// Tester, at BetalingsbetingelseGetAll henter alle betalingsbetingelser.
        /// </summary>
        [Test]
        public void TestAtBetalingsbetingelseGetAllHenterBetalingsbetingelser()
        {
            var fixture = new Fixture();

            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IAdresseRepositoryService>(new[] {typeof (ICommunicationObject)});
            service.Expect(m => m.BetalingsbetingelseGetAll(Arg<BetalingsbetingelseGetAllQuery>.Is.Anything))
                .Return(fixture.CreateMany<BetalingsbetingelseView>(2));
            Expect.Call(((ICommunicationObject) service).State).Return(CommunicationState.Closed).Repeat.Any();
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IAdresseRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();
            domainObjectBuilder.Expect(
                m =>
                m.BuildMany<BetalingsbetingelseView, Betalingsbetingelse>(
                    Arg<IEnumerable<BetalingsbetingelseView>>.Is.NotNull))
                .Return(fixture.CreateMany<Betalingsbetingelse>(2));

            var repository = new AdresseRepository(channelFactory, domainObjectBuilder);
            var betalingsbetingelser = repository.BetalingsbetingelseGetAll();
            Assert.That(betalingsbetingelser, Is.Not.Null);
            Assert.That(betalingsbetingelser.Count(), Is.EqualTo(2));

            domainObjectBuilder.AssertWasCalled(
                m =>
                m.BuildMany<BetalingsbetingelseView, Betalingsbetingelse>(
                    Arg<IEnumerable<BetalingsbetingelseView>>.Is.NotNull));
        }

        /// <summary>
        /// Tester, at BetalingsbetingelserGetAll kaster en IntranetRepositoryException ved IntranetRepositoryException.
        /// </summary>
        [Test]
        public void TestAtBetalingsbetingelserGetAllKasterIntranetRepositoryExceptionVedIntranetRepositoryException()
        {
            var fixture = new Fixture();

            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IAdresseRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.BetalingsbetingelseGetAll(Arg<BetalingsbetingelseGetAllQuery>.Is.Anything))
                .Throw(fixture.Create<IntranetRepositoryException>());
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed).Repeat.Any();
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IAdresseRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new AdresseRepository(channelFactory, domainObjectBuilder);
            Assert.Throws<IntranetRepositoryException>(() => repository.BetalingsbetingelseGetAll());
        }

        /// <summary>
        /// Tester, at BetalingsbetingelserGetAll kaster en IntranetRepositoryException ved FaultException.
        /// </summary>
        [Test]
        public void TestAtBetalingsbetingelserGetAllKasterIntranetRepositoryExceptionVedFaultException()
        {
            var fixture = new Fixture();

            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IAdresseRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.BetalingsbetingelseGetAll(Arg<BetalingsbetingelseGetAllQuery>.Is.Anything))
                .Throw(fixture.Create<FaultException>());
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed).Repeat.Any();
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IAdresseRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new AdresseRepository(channelFactory, domainObjectBuilder);
            Assert.Throws<IntranetRepositoryException>(() => repository.BetalingsbetingelseGetAll());
        }

        /// <summary>
        /// Tester, at BetalingsbetingelserGetAll kaster en IntranetRepositoryException ved Exception.
        /// </summary>
        [Test]
        public void TestAtBetalingsbetingelserGetAllKasterIntranetRepositoryExceptionVedException()
        {
            var fixture = new Fixture();

            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IAdresseRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.BetalingsbetingelseGetAll(Arg<BetalingsbetingelseGetAllQuery>.Is.Anything))
                .Throw(fixture.Create<Exception>());
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed).Repeat.Any();
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IAdresseRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new AdresseRepository(channelFactory, domainObjectBuilder);
            Assert.Throws<IntranetRepositoryException>(() => repository.BetalingsbetingelseGetAll());
        }
    }
}
