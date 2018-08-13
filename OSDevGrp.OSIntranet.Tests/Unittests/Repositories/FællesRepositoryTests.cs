using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Fælles;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf.ChannelFactory;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Queries;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Services;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Views;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories;
using OSDevGrp.OSIntranet.Repositories.DataProxies.Fælles;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using NUnit.Framework;
using AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories
{
    /// <summary>
    /// Tester repository til fælles elementer i domænet.
    /// </summary>
    [TestFixture]
    public class FællesRepositoryTests
    {
        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis ChannelFactory er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisChannelFactoryErNull()
        {
            var fixture = new Fixture();
            fixture.Inject<IChannelFactory>(null);
            fixture.Inject(MockRepository.GenerateMock<IMySqlDataProvider>());
            fixture.Inject(MockRepository.GenerateMock<IDomainObjectBuilder>());
            Assert.Throws<ArgumentNullException>(
                () =>
                new FællesRepository(fixture.Create<IChannelFactory>(),
                                     fixture.Create<IMySqlDataProvider>(),
                                     fixture.Create<IDomainObjectBuilder>()));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis data provider til MySql er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisMySqlDataProviderErNull()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<IChannelFactory>());
            fixture.Inject<IMySqlDataProvider>(null);
            fixture.Inject(MockRepository.GenerateMock<IDomainObjectBuilder>());
            Assert.Throws<ArgumentNullException>(
                () =>
                new FællesRepository(fixture.Create<IChannelFactory>(),
                                     fixture.Create<IMySqlDataProvider>(),
                                     fixture.Create<IDomainObjectBuilder>()));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis domæneobjekt bygger er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisDomainObjectBuilderErNull()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<IChannelFactory>());
            fixture.Inject(MockRepository.GenerateMock<IMySqlDataProvider>());
            fixture.Inject<IDomainObjectBuilder>(null);
            Assert.Throws<ArgumentNullException>(
                () =>
                new FællesRepository(fixture.Create<IChannelFactory>(),
                                     fixture.Create<IMySqlDataProvider>(),
                                     fixture.Create<IDomainObjectBuilder>()));
        }

        /// <summary>
        /// Tester, at BrevhovedGetAll henter alle brevhoveder.
        /// </summary>
        [Test]
        public void TestAtBrevhovedGetAllHenterAlleBrevhoveder()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<IMySqlDataProvider>());

            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFællesRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.BrevhovedGetAll(Arg<BrevhovedGetAllQuery>.Is.Anything))
                .Return(fixture.CreateMany<BrevhovedView>(3));
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed);
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFællesRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);
            fixture.Inject(channelFactory);

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();
            domainObjectBuilder.Expect(
                m => m.BuildMany<BrevhovedView, Brevhoved>(Arg<IEnumerable<BrevhovedView>>.Is.NotNull))
                .Return(fixture.CreateMany<Brevhoved>(3));
            fixture.Inject(domainObjectBuilder);

            var repository = fixture.Create<FællesRepository>();
            Assert.That(repository, Is.Not.Null);

            var brevhoveder = repository.BrevhovedGetAll();
            Assert.That(brevhoveder, Is.Not.Null);
            Assert.That(brevhoveder.Count(), Is.EqualTo(3));

            domainObjectBuilder.AssertWasCalled(
                m => m.BuildMany<BrevhovedView, Brevhoved>(Arg<IEnumerable<BrevhovedView>>.Is.NotNull));
        }

        /// <summary>
        /// Tester, at BrevhovedGetAll kaster en IntranetRepositoryException ved IntranetRepositoryException.
        /// </summary>
        [Test]
        public void TestAtBrevhovedGetAllKasterIntranetRepositoryExceptionVedIntranetRepositoryException()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<IMySqlDataProvider>());
            fixture.Inject(MockRepository.GenerateMock<IDomainObjectBuilder>());

            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFællesRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.BrevhovedGetAll(Arg<BrevhovedGetAllQuery>.Is.Anything))
                .Throw(fixture.Create<IntranetRepositoryException>());
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed);
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFællesRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);
            fixture.Inject(channelFactory);

            var repository = fixture.Create<FællesRepository>();
            Assert.That(repository, Is.Not.Null);

            Assert.Throws<IntranetRepositoryException>(() => repository.BrevhovedGetAll());
        }

        /// <summary>
        /// Tester, at BrevhovedGetAll kaster en IntranetRepositoryException ved FaultException.
        /// </summary>
        [Test]
        public void TestAtBrevhovedGetAllKasterIntranetRepositoryExceptionVedFaultException()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<IMySqlDataProvider>());
            fixture.Inject(MockRepository.GenerateMock<IDomainObjectBuilder>());

            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFællesRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.BrevhovedGetAll(Arg<BrevhovedGetAllQuery>.Is.Anything))
                .Throw(fixture.Create<FaultException>());
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed);
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFællesRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);
            fixture.Inject(channelFactory);

            var repository = fixture.Create<FællesRepository>();
            Assert.That(repository, Is.Not.Null);

            Assert.Throws<IntranetRepositoryException>(() => repository.BrevhovedGetAll());
        }

        /// <summary>
        /// Tester, at BrevhovedGetAll kaster en IntranetRepositoryException ved Exception.
        /// </summary>
        [Test]
        public void TestAtBrevhovedGetAllKasterIntranetRepositoryExceptionVedException()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<IMySqlDataProvider>());
            fixture.Inject(MockRepository.GenerateMock<IDomainObjectBuilder>());

            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFællesRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.BrevhovedGetAll(Arg<BrevhovedGetAllQuery>.Is.Anything))
                .Throw(fixture.Create<Exception>());
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed);
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFællesRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);
            fixture.Inject(channelFactory);

            var repository = fixture.Create<FællesRepository>();
            Assert.That(repository, Is.Not.Null);

            Assert.Throws<IntranetRepositoryException>(() => repository.BrevhovedGetAll());
        }

        /// <summary>
        /// Tester, at SystemGetAll henter systemer.
        /// </summary>
        [Test]
        public void TestAtSystemGetAllHenterSystemer()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<IChannelFactory>());
            fixture.Inject(MockRepository.GenerateMock<IDomainObjectBuilder>());

            var mySqlDataProvider = MockRepository.GenerateMock<IMySqlDataProvider>();
            mySqlDataProvider.Expect(m => m.GetCollection<SystemProxy>(Arg<string>.Is.NotNull))
                .Return(fixture.CreateMany<SystemProxy>(3));
            fixture.Inject(mySqlDataProvider);

            var repository = fixture.Create<FællesRepository>();
            Assert.That(repository, Is.Not.Null);

            var systemer = repository.SystemGetAll();
            Assert.That(systemer, Is.Not.Null);
            Assert.That(systemer.Count(), Is.EqualTo(3));

            mySqlDataProvider.AssertWasCalled(m => m.GetCollection<SystemProxy>(Arg<string>.Is.NotNull));
        }

        /// <summary>
        /// Tester, at SystemGetAll kaster en IntranetRepositoryException ved IntranetRepositoryException.
        /// </summary>
        [Test]
        public void TestAtSystemGetAllKasterIntranetRepositoryExceptionVedIntranetRepositoryException()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<IChannelFactory>());
            fixture.Inject(MockRepository.GenerateMock<IDomainObjectBuilder>());

            var mySqlDataProvider = MockRepository.GenerateMock<IMySqlDataProvider>();
            mySqlDataProvider.Expect(m => m.GetCollection<SystemProxy>(Arg<string>.Is.NotNull))
                .Throw(fixture.Create<IntranetRepositoryException>());
            fixture.Inject(mySqlDataProvider);

            var repository = fixture.Create<FællesRepository>();
            Assert.That(repository, Is.Not.Null);

            Assert.Throws<IntranetRepositoryException>(() => repository.SystemGetAll());
        }

        /// <summary>
        /// Tester, at SystemGetAll kaster en IntranetRepositoryException ved Exception.
        /// </summary>
        [Test]
        public void TestAtSystemGetAllKasterIntranetRepositoryExceptionVedException()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<IChannelFactory>());
            fixture.Inject(MockRepository.GenerateMock<IDomainObjectBuilder>());

            var mySqlDataProvider = MockRepository.GenerateMock<IMySqlDataProvider>();
            mySqlDataProvider.Expect(m => m.GetCollection<SystemProxy>(Arg<string>.Is.NotNull))
                .Throw(fixture.Create<Exception>());
            fixture.Inject(mySqlDataProvider);

            var repository = fixture.Create<FællesRepository>();
            Assert.That(repository, Is.Not.Null);

            Assert.Throws<IntranetRepositoryException>(() => repository.SystemGetAll());
        }
    }
}
