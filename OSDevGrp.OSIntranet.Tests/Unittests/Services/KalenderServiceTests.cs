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
    /// Tester service til fælles kalender.
    /// </summary>
    [TestFixture]
    public class KalenderServiceTests
    {
        /// <summary>
        /// Tester, at service til kalender kan hostes.
        /// </summary>
        [Test]
        public void TestAtKalenderServiceKanHostes()
        {
            var uri = new Uri("http://localhost:7000/OSIntranet/");
            var host = new ServiceHost(typeof(KalenderService), new[] { uri });
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
            Assert.Throws<ArgumentNullException>(() => new KalenderService(fixture.CreateAnonymous<IQueryBus>()));
        }

        /// <summary>
        /// Tester, at KalenderbrugerAftalerGet kalder QueryBus.
        /// </summary>
        [Test]
        public void TestAtKalenderbrugerAftalerGetKalderQueryBus()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            fixture.Inject(queryBus);

            var service = fixture.CreateAnonymous<KalenderService>();
            Assert.That(service, Is.Not.Null);

            service.KalenderbrugerAftalerGet(fixture.CreateAnonymous<KalenderbrugerAftalerGetQuery>());

            queryBus.AssertWasCalled(
                m =>
                m.Query<KalenderbrugerAftalerGetQuery, IEnumerable<KalenderbrugerAftaleView>>(
                    Arg<KalenderbrugerAftalerGetQuery>.Is.NotNull));
        }

        /// <summary>
        /// Tester, at KalenderbrugerAftalerGet kaster en IntranetRepositoryFault.
        /// </summary>
        [Test]
        public void TestAtKalenderbrugerAftalerGetKasterIntranetRepositoryFault()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m =>
                m.Query<KalenderbrugerAftalerGetQuery, IEnumerable<KalenderbrugerAftaleView>>(
                    Arg<KalenderbrugerAftalerGetQuery>.Is.NotNull))
                .Throw(fixture.CreateAnonymous<IntranetRepositoryException>());
            fixture.Inject(queryBus);

            var service = fixture.CreateAnonymous<KalenderService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetRepositoryFault>>(
                () => service.KalenderbrugerAftalerGet(fixture.CreateAnonymous<KalenderbrugerAftalerGetQuery>()));
        }

        /// <summary>
        /// Tester, at KalenderbrugerAftalerGet kaster en IntranetBusinessFault.
        /// </summary>
        [Test]
        public void TestAtKalenderbrugerAftalerGetKasterIntranetBusinessFault()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m =>
                m.Query<KalenderbrugerAftalerGetQuery, IEnumerable<KalenderbrugerAftaleView>>(
                    Arg<KalenderbrugerAftalerGetQuery>.Is.NotNull))
                .Throw(fixture.CreateAnonymous<IntranetBusinessException>());
            fixture.Inject(queryBus);

            var service = fixture.CreateAnonymous<KalenderService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetBusinessFault>>(
                () => service.KalenderbrugerAftalerGet(fixture.CreateAnonymous<KalenderbrugerAftalerGetQuery>()));
        }

        /// <summary>
        /// Tester, at KalenderbrugerAftalerGet kaster en IntranetSystemFault.
        /// </summary>
        [Test]
        public void TestAtKalenderbrugerAftalerGetKasterIntranetSystemFault()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m =>
                m.Query<KalenderbrugerAftalerGetQuery, IEnumerable<KalenderbrugerAftaleView>>(
                    Arg<KalenderbrugerAftalerGetQuery>.Is.NotNull))
                .Throw(fixture.CreateAnonymous<IntranetSystemException>());
            fixture.Inject(queryBus);

            var service = fixture.CreateAnonymous<KalenderService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetSystemFault>>(
                () => service.KalenderbrugerAftalerGet(fixture.CreateAnonymous<KalenderbrugerAftalerGetQuery>()));
        }

        /// <summary>
        /// Tester, at KalenderbrugerAftalerGet kaster en IntranetSystemFault ved unhandled exception.
        /// </summary>
        [Test]
        public void TestAtKalenderbrugerAftalerGetKasterIntranetSystemFaultVedUnhandledException()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m =>
                m.Query<KalenderbrugerAftalerGetQuery, IEnumerable<KalenderbrugerAftaleView>>(
                    Arg<KalenderbrugerAftalerGetQuery>.Is.NotNull))
                .Throw(fixture.CreateAnonymous<Exception>());
            fixture.Inject(queryBus);

            var service = fixture.CreateAnonymous<KalenderService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetSystemFault>>(
                () => service.KalenderbrugerAftalerGet(fixture.CreateAnonymous<KalenderbrugerAftalerGetQuery>()));
        }

        /// <summary>
        /// Tester, at KalenderbrugerAftaleGet kalder QueryBus.
        /// </summary>
        [Test]
        public void TestAtKalenderbrugerAftaleGetKalderQueryBus()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            fixture.Inject(queryBus);

            var service = fixture.CreateAnonymous<KalenderService>();
            Assert.That(service, Is.Not.Null);

            service.KalenderbrugerAftaleGet(fixture.CreateAnonymous<KalenderbrugerAftaleGetQuery>());

            queryBus.AssertWasCalled(
                m =>
                m.Query<KalenderbrugerAftaleGetQuery, KalenderbrugerAftaleView>(
                    Arg<KalenderbrugerAftaleGetQuery>.Is.NotNull));
        }

        /// <summary>
        /// Tester, at KalenderbrugerAftaleGet kaster en IntranetRepositoryFault.
        /// </summary>
        [Test]
        public void TestAtKalenderbrugerAftaleGetKasterIntranetRepositoryFault()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m =>
                m.Query<KalenderbrugerAftaleGetQuery, KalenderbrugerAftaleView>(
                    Arg<KalenderbrugerAftaleGetQuery>.Is.NotNull))
                .Throw(fixture.CreateAnonymous<IntranetRepositoryException>());
            fixture.Inject(queryBus);

            var service = fixture.CreateAnonymous<KalenderService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetRepositoryFault>>(
                () => service.KalenderbrugerAftaleGet(fixture.CreateAnonymous<KalenderbrugerAftaleGetQuery>()));
        }

        /// <summary>
        /// Tester, at KalenderbrugerAftaleGet kaster en IntranetBusinessFault.
        /// </summary>
        [Test]
        public void TestAtKalenderbrugerAftaleGetKasterIntranetBusinessFault()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m =>
                m.Query<KalenderbrugerAftaleGetQuery, KalenderbrugerAftaleView>(
                    Arg<KalenderbrugerAftaleGetQuery>.Is.NotNull))
                .Throw(fixture.CreateAnonymous<IntranetBusinessException>());
            fixture.Inject(queryBus);

            var service = fixture.CreateAnonymous<KalenderService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetBusinessFault>>(
                () => service.KalenderbrugerAftaleGet(fixture.CreateAnonymous<KalenderbrugerAftaleGetQuery>()));
        }

        /// <summary>
        /// Tester, at KalenderbrugerAftaleGet kaster en IntranetSystemFault.
        /// </summary>
        [Test]
        public void TestAtKalenderbrugerAftaleGetKasterIntranetSystemFault()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m =>
                m.Query<KalenderbrugerAftaleGetQuery, KalenderbrugerAftaleView>(
                    Arg<KalenderbrugerAftaleGetQuery>.Is.NotNull))
                .Throw(fixture.CreateAnonymous<IntranetSystemException>());
            fixture.Inject(queryBus);

            var service = fixture.CreateAnonymous<KalenderService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetSystemFault>>(
                () => service.KalenderbrugerAftaleGet(fixture.CreateAnonymous<KalenderbrugerAftaleGetQuery>()));
        }

        /// <summary>
        /// Tester, at KalenderbrugerAftalerGet kaster en IntranetSystemFault ved unhandled exception.
        /// </summary>
        [Test]
        public void TestAtKalenderbrugerAftaleGetKasterIntranetSystemFaultVedUnhandledException()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m =>
                m.Query<KalenderbrugerAftaleGetQuery, KalenderbrugerAftaleView>(
                    Arg<KalenderbrugerAftaleGetQuery>.Is.NotNull))
                .Throw(fixture.CreateAnonymous<Exception>());
            fixture.Inject(queryBus);

            var service = fixture.CreateAnonymous<KalenderService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetSystemFault>>(
                () => service.KalenderbrugerAftaleGet(fixture.CreateAnonymous<KalenderbrugerAftaleGetQuery>()));
        }

        /// <summary>
        /// Tester, at KalenderbrugereGet kalder QueryBus.
        /// </summary>
        [Test]
        public void TestAtKalenderbrugereGetKalderQueryBus()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            fixture.Inject(queryBus);

            var service = fixture.CreateAnonymous<KalenderService>();
            Assert.That(service, Is.Not.Null);

            service.KalenderbrugereGet(fixture.CreateAnonymous<KalenderbrugereGetQuery>());

            queryBus.AssertWasCalled(
                m =>
                m.Query<KalenderbrugereGetQuery, IEnumerable<KalenderbrugerView>>(
                    Arg<KalenderbrugereGetQuery>.Is.NotNull));
        }

        /// <summary>
        /// Tester, at KalenderbrugereGet kaster en IntranetRepositoryFault.
        /// </summary>
        [Test]
        public void TestAtKalenderbrugereGetKasterIntranetRepositoryFault()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m =>
                m.Query<KalenderbrugereGetQuery, IEnumerable<KalenderbrugerView>>(
                    Arg<KalenderbrugereGetQuery>.Is.NotNull))
                .Throw(fixture.CreateAnonymous<IntranetRepositoryException>());
            fixture.Inject(queryBus);

            var service = fixture.CreateAnonymous<KalenderService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetRepositoryFault>>(
                () => service.KalenderbrugereGet(fixture.CreateAnonymous<KalenderbrugereGetQuery>()));
        }

        /// <summary>
        /// Tester, at KalenderbrugereGet kaster en IntranetBusinessFault.
        /// </summary>
        [Test]
        public void TestAtKalenderbrugereGetKasterIntranetBusinessFault()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m =>
                m.Query<KalenderbrugereGetQuery, IEnumerable<KalenderbrugerView>>(
                    Arg<KalenderbrugereGetQuery>.Is.NotNull))
                .Throw(fixture.CreateAnonymous<IntranetBusinessException>());
            fixture.Inject(queryBus);

            var service = fixture.CreateAnonymous<KalenderService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetBusinessFault>>(
                () => service.KalenderbrugereGet(fixture.CreateAnonymous<KalenderbrugereGetQuery>()));
        }

        /// <summary>
        /// Tester, at KalenderbrugereGet kaster en IntranetSystemFault.
        /// </summary>
        [Test]
        public void TestAtKalenderbrugereGetKasterIntranetSystemFault()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m =>
                m.Query<KalenderbrugereGetQuery, IEnumerable<KalenderbrugerView>>(
                    Arg<KalenderbrugereGetQuery>.Is.NotNull))
                .Throw(fixture.CreateAnonymous<IntranetSystemException>());
            fixture.Inject(queryBus);

            var service = fixture.CreateAnonymous<KalenderService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetSystemFault>>(
                () => service.KalenderbrugereGet(fixture.CreateAnonymous<KalenderbrugereGetQuery>()));
        }

        /// <summary>
        /// Tester, at KalenderbrugereGet kaster en IntranetSystemFault ved unhandled exception.
        /// </summary>
        [Test]
        public void TestAtKalenderbrugereGetKasterIntranetSystemFaultVedUnhandledException()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m =>
                m.Query<KalenderbrugereGetQuery, IEnumerable<KalenderbrugerView>>(
                    Arg<KalenderbrugereGetQuery>.Is.NotNull))
                .Throw(fixture.CreateAnonymous<Exception>());
            fixture.Inject(queryBus);

            var service = fixture.CreateAnonymous<KalenderService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetSystemFault>>(
                () => service.KalenderbrugereGet(fixture.CreateAnonymous<KalenderbrugereGetQuery>()));
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

            var service = fixture.CreateAnonymous<KalenderService>();
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

            var service = fixture.CreateAnonymous<KalenderService>();
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

            var service = fixture.CreateAnonymous<KalenderService>();
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

            var service = fixture.CreateAnonymous<KalenderService>();
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

            var service = fixture.CreateAnonymous<KalenderService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetSystemFault>>(
                () => service.SystemerGet(fixture.CreateAnonymous<SystemerGetQuery>()));
        }
    }
}
