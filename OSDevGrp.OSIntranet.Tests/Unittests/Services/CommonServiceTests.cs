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
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Services
{
    /// <summary>
    /// Tester service til fælles elementer.
    /// </summary>
    [TestFixture]
    public class CommonServiceTests
    {
        /// <summary>
        /// Tester, at service til fælles elementer kan hostes.
        /// </summary>
        [Test]
        public void TestAtCommonServiceKanHostes()
        {
            var uri = new Uri("http://localhost:7000/OSIntranet/");
            var host = new ServiceHost(typeof(CommonService), new[] { uri });
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
            var fixture = new Fixture();
            fixture.Inject<IQueryBus>(null);
            Assert.Throws<ArgumentNullException>(() => new CommonService(fixture.CreateAnonymous<IQueryBus>()));
        }

        /// <summary>
        /// Tester, at BrevhovederGet kalder QueryBus.
        /// </summary>
        [Test]
        public void TestAtBrevhovederGetKalderQueryBus()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            fixture.Inject(queryBus);

            var service = fixture.CreateAnonymous<CommonService>();
            Assert.That(service, Is.Not.Null);

            service.BrevhovederGet(fixture.CreateAnonymous<BrevhovederGetQuery>());

            queryBus.AssertWasCalled(
                m => m.Query<BrevhovederGetQuery, IEnumerable<BrevhovedView>>(Arg<BrevhovederGetQuery>.Is.NotNull));
        }

        /// <summary>
        /// Tester, at BrevhovederGet kaster en IntranetRepositoryFault.
        /// </summary>
        [Test]
        public void TestAtBrevhovederGetKasterIntranetRepositoryFault()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m => m.Query<BrevhovederGetQuery, IEnumerable<BrevhovedView>>(Arg<BrevhovederGetQuery>.Is.NotNull))
                .Throw(fixture.CreateAnonymous<IntranetRepositoryException>());
            fixture.Inject(queryBus);

            var service = fixture.CreateAnonymous<CommonService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetRepositoryFault>>(
                () => service.BrevhovederGet(fixture.CreateAnonymous<BrevhovederGetQuery>()));
        }

        /// <summary>
        /// Tester, at BrevhovederGet kaster en IntranetBusinessFault.
        /// </summary>
        [Test]
        public void TestAtBrevhovederGetKasterIntranetBusinessFault()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m => m.Query<BrevhovederGetQuery, IEnumerable<BrevhovedView>>(Arg<BrevhovederGetQuery>.Is.NotNull))
                .Throw(fixture.CreateAnonymous<IntranetBusinessException>());
            fixture.Inject(queryBus);

            var service = fixture.CreateAnonymous<CommonService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetBusinessFault>>(
                () => service.BrevhovederGet(fixture.CreateAnonymous<BrevhovederGetQuery>()));
        }

        /// <summary>
        /// Tester, at BrevhovederGet kaster en IntranetSystemFault.
        /// </summary>
        [Test]
        public void TestAtBrevhovederGetKasterIntranetSystemFault()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m => m.Query<BrevhovederGetQuery, IEnumerable<BrevhovedView>>(Arg<BrevhovederGetQuery>.Is.NotNull))
                .Throw(fixture.CreateAnonymous<IntranetSystemException>());
            fixture.Inject(queryBus);

            var service = fixture.CreateAnonymous<CommonService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetSystemFault>>(
                () => service.BrevhovederGet(fixture.CreateAnonymous<BrevhovederGetQuery>()));
        }

        /// <summary>
        /// Tester, at BrevhovederGet kaster en IntranetSystemFault ved unhandled exception.
        /// </summary>
        [Test]
        public void TestAtBrevhovederGetKasterIntranetSystemFaultVedUnhandledException()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m => m.Query<BrevhovederGetQuery, IEnumerable<BrevhovedView>>(Arg<BrevhovederGetQuery>.Is.NotNull))
                .Throw(fixture.CreateAnonymous<Exception>());
            fixture.Inject(queryBus);

            var service = fixture.CreateAnonymous<CommonService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetSystemFault>>(
                () => service.BrevhovederGet(fixture.CreateAnonymous<BrevhovederGetQuery>()));
        }

        /// <summary>
        /// Tester, at SystemerGet kalder QueryBus.
        /// </summary>
        [Test]
        public void TestAtSystemerGetKalderQueryBus()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            fixture.Inject(queryBus);

            var service = fixture.CreateAnonymous<CommonService>();
            Assert.That(service, Is.Not.Null);

            service.SystemerGet(fixture.CreateAnonymous<SystemerGetQuery>());

            queryBus.AssertWasCalled(
                m => m.Query<SystemerGetQuery, IEnumerable<SystemView>>(Arg<SystemerGetQuery>.Is.NotNull));
        }

        /// <summary>
        /// Tester, at SystemerGet kaster en IntranetRepositoryFault.
        /// </summary>
        [Test]
        public void TestAtSystemerGetKasterIntranetRepositoryFault()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<SystemerGetQuery, IEnumerable<SystemView>>(Arg<SystemerGetQuery>.Is.NotNull))
                .Throw(fixture.CreateAnonymous<IntranetRepositoryException>());
            fixture.Inject(queryBus);

            var service = fixture.CreateAnonymous<CommonService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetRepositoryFault>>(
                () => service.SystemerGet(fixture.CreateAnonymous<SystemerGetQuery>()));
        }

        /// <summary>
        /// Tester, at SystemerGet kaster en IntranetBusinessFault.
        /// </summary>
        [Test]
        public void TestAtSystemerGetKasterIntranetBusinessFault()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<SystemerGetQuery, IEnumerable<SystemView>>(Arg<SystemerGetQuery>.Is.NotNull))
                .Throw(fixture.CreateAnonymous<IntranetBusinessException>());
            fixture.Inject(queryBus);

            var service = fixture.CreateAnonymous<CommonService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetBusinessFault>>(
                () => service.SystemerGet(fixture.CreateAnonymous<SystemerGetQuery>()));
        }

        /// <summary>
        /// Tester, at SystemerGet kaster en IntranetSystemFault.
        /// </summary>
        [Test]
        public void TestAtSystemerGetKasterIntranetSystemFault()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<SystemerGetQuery, IEnumerable<SystemView>>(Arg<SystemerGetQuery>.Is.NotNull))
                .Throw(fixture.CreateAnonymous<IntranetSystemException>());
            fixture.Inject(queryBus);

            var service = fixture.CreateAnonymous<CommonService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetSystemFault>>(
                () => service.SystemerGet(fixture.CreateAnonymous<SystemerGetQuery>()));
        }

        /// <summary>
        /// Tester, at SystemerGet kaster en IntranetSystemFault ved unhandled exception.
        /// </summary>
        [Test]
        public void TestAtSystemerGetKasterIntranetSystemFaultVedUnhandledException()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<SystemerGetQuery, IEnumerable<SystemView>>(Arg<SystemerGetQuery>.Is.NotNull))
                .Throw(fixture.CreateAnonymous<Exception>());
            fixture.Inject(queryBus);

            var service = fixture.CreateAnonymous<CommonService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetSystemFault>>(
                () => service.SystemerGet(fixture.CreateAnonymous<SystemerGetQuery>()));
        }
    }
}
