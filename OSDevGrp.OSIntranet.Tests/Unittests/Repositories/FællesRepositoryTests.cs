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
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using NUnit.Framework;
using Ploeh.AutoFixture;
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
            Assert.Throws<ArgumentNullException>(() => new FællesRepository(null, null));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis domæneobjekt bygger er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisDomainObjectBuilderErNull()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            Assert.Throws<ArgumentNullException>(() => new FællesRepository(channelFactory, null));
        }

        /// <summary>
        /// Tester, at BrevhovedGetAll henter alle brevhoveder.
        /// </summary>
        [Test]
        public void TestAtBrevhovedGetAllHenterAlleBrevhoveder()
        {
            var fixture = new Fixture();

            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFællesRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.BrevhovedGetAll(Arg<BrevhovedGetAllQuery>.Is.Anything))
                .Return(fixture.CreateMany<BrevhovedView>(3));
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed);
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFællesRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();
            domainObjectBuilder.Expect(
                m => m.BuildMany<BrevhovedView, Brevhoved>(Arg<IEnumerable<BrevhovedView>>.Is.NotNull))
                .Return(fixture.CreateMany<Brevhoved>(3));

            var repository = new FællesRepository(channelFactory, domainObjectBuilder);
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

            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFællesRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.BrevhovedGetAll(Arg<BrevhovedGetAllQuery>.Is.Anything))
                .Throw(fixture.CreateAnonymous<IntranetRepositoryException>());
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed);
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFællesRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new FællesRepository(channelFactory, domainObjectBuilder);
            Assert.Throws<IntranetRepositoryException>(() => repository.BrevhovedGetAll());
        }

        /// <summary>
        /// Tester, at BrevhovedGetAll kaster en IntranetRepositoryException ved FaultException.
        /// </summary>
        [Test]
        public void TestAtBrevhovedGetAllKasterIntranetRepositoryExceptionVedFaultException()
        {
            var fixture = new Fixture();

            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFællesRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.BrevhovedGetAll(Arg<BrevhovedGetAllQuery>.Is.Anything))
                .Throw(fixture.CreateAnonymous<FaultException>());
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed);
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFællesRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new FællesRepository(channelFactory, domainObjectBuilder);
            Assert.Throws<IntranetRepositoryException>(() => repository.BrevhovedGetAll());
        }

        /// <summary>
        /// Tester, at BrevhovedGetAll kaster en IntranetRepositoryException ved Exception.
        /// </summary>
        [Test]
        public void TestAtBrevhovedGetAllKasterIntranetRepositoryExceptionVedException()
        {
            var fixture = new Fixture();

            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFællesRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.BrevhovedGetAll(Arg<BrevhovedGetAllQuery>.Is.Anything))
                .Throw(fixture.CreateAnonymous<Exception>());
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed);
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFællesRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var domainObjectBuilder = MockRepository.GenerateMock<IDomainObjectBuilder>();

            var repository = new FællesRepository(channelFactory, domainObjectBuilder);
            Assert.Throws<IntranetRepositoryException>(() => repository.BrevhovedGetAll());
        }
    }
}
