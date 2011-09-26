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
    /// Tester service til adressekartotek.
    /// </summary>
    [TestFixture]
    public class AdressekartotekServiceTests
    {
        /// <summary>
        /// Tester, at service til adressekartotek kan hostes.
        /// </summary>
        [Test]
        public void TestAtAdressekartotekServiceKanHostes()
        {
            var uri = new Uri("http://localhost:7000/OSIntranet/");
            var host = new ServiceHost(typeof(AdressekartotekService), new[] { uri });
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
            Assert.Throws<ArgumentNullException>(() => new AdressekartotekService(fixture.CreateAnonymous<IQueryBus>()));
        }

        /// <summary>
        /// Tester, at PostnumreGet kalder QueryBus.
        /// </summary>
        [Test]
        public void TestAtPostnumreGetKalderQueryBus()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            fixture.Inject(queryBus);

            var service = fixture.CreateAnonymous<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            service.PostnumreGet(fixture.CreateAnonymous<PostnumreGetQuery>());

            queryBus.AssertWasCalled(
                m => m.Query<PostnumreGetQuery, IEnumerable<PostnummerView>>(Arg<PostnumreGetQuery>.Is.NotNull));
        }

        /// <summary>
        /// Tester, at PostnumreGet kaster en IntranetRepositoryFault.
        /// </summary>
        [Test]
        public void TestAtPostnumreGetKasterIntranetRepositoryFault()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m => m.Query<PostnumreGetQuery, IEnumerable<PostnummerView>>(Arg<PostnumreGetQuery>.Is.NotNull))
                .Throw(fixture.CreateAnonymous<IntranetRepositoryException>());
            fixture.Inject(queryBus);

            var service = fixture.CreateAnonymous<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetRepositoryFault>>(
                () => service.PostnumreGet(fixture.CreateAnonymous<PostnumreGetQuery>()));
        }

        /// <summary>
        /// Tester, at PostnumreGet kaster en IntranetBusinessFault.
        /// </summary>
        [Test]
        public void TestAtPostnumreGetKasterIntranetBusinessFault()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m => m.Query<PostnumreGetQuery, IEnumerable<PostnummerView>>(Arg<PostnumreGetQuery>.Is.NotNull))
                .Throw(fixture.CreateAnonymous<IntranetBusinessException>());
            fixture.Inject(queryBus);

            var service = fixture.CreateAnonymous<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetBusinessFault>>(
                () => service.PostnumreGet(fixture.CreateAnonymous<PostnumreGetQuery>()));
        }

        /// <summary>
        /// Tester, at PostnumreGet kaster en IntranetSystemFault.
        /// </summary>
        [Test]
        public void TestAtPostnumreGetKasterIntranetSystemFault()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m => m.Query<PostnumreGetQuery, IEnumerable<PostnummerView>>(Arg<PostnumreGetQuery>.Is.NotNull))
                .Throw(fixture.CreateAnonymous<IntranetSystemException>());
            fixture.Inject(queryBus);

            var service = fixture.CreateAnonymous<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetSystemFault>>(
                () => service.PostnumreGet(fixture.CreateAnonymous<PostnumreGetQuery>()));
        }

        /// <summary>
        /// Tester, at PostnumreGet kaster en IntranetSystemFault ved unhandled exception.
        /// </summary>
        [Test]
        public void TestAtPostnumreGetKasterIntranetSystemFaultVedUnhandledException()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m => m.Query<PostnumreGetQuery, IEnumerable<PostnummerView>>(Arg<PostnumreGetQuery>.Is.NotNull))
                .Throw(fixture.CreateAnonymous<Exception>());
            fixture.Inject(queryBus);

            var service = fixture.CreateAnonymous<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetSystemFault>>(
                () => service.PostnumreGet(fixture.CreateAnonymous<PostnumreGetQuery>()));
        }

        /// <summary>
        /// Tester, at AdressegrupperGet kalder QueryBus.
        /// </summary>
        [Test]
        public void TestAtAdressegrupperGetKalderQueryBus()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            fixture.Inject(queryBus);

            var service = fixture.CreateAnonymous<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            service.AdressegrupperGet(fixture.CreateAnonymous<AdressegrupperGetQuery>());

            queryBus.AssertWasCalled(
                m =>
                m.Query<AdressegrupperGetQuery, IEnumerable<AdressegruppeView>>(Arg<AdressegrupperGetQuery>.Is.NotNull));
        }

        /// <summary>
        /// Tester, at AdressegrupperGet kaster en IntranetRepositoryFault.
        /// </summary>
        [Test]
        public void TestAtAdressegrupperGetKasterIntranetRepositoryFault()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m =>
                m.Query<AdressegrupperGetQuery, IEnumerable<AdressegruppeView>>(Arg<AdressegrupperGetQuery>.Is.NotNull))
                .Throw(fixture.CreateAnonymous<IntranetRepositoryException>());
            fixture.Inject(queryBus);

            var service = fixture.CreateAnonymous<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetRepositoryFault>>(
                () => service.AdressegrupperGet(fixture.CreateAnonymous<AdressegrupperGetQuery>()));
        }

        /// <summary>
        /// Tester, at AdressegrupperGet kaster en IntranetBusinessFault.
        /// </summary>
        [Test]
        public void TestAtAdressegrupperGetKasterIntranetBusinessFault()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m =>
                m.Query<AdressegrupperGetQuery, IEnumerable<AdressegruppeView>>(Arg<AdressegrupperGetQuery>.Is.NotNull))
                .Throw(fixture.CreateAnonymous<IntranetBusinessException>());
            fixture.Inject(queryBus);

            var service = fixture.CreateAnonymous<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetBusinessFault>>(
                () => service.AdressegrupperGet(fixture.CreateAnonymous<AdressegrupperGetQuery>()));
        }

        /// <summary>
        /// Tester, at AdressegrupperGet kaster en IntranetSystemFault.
        /// </summary>
        [Test]
        public void TestAtAdressegrupperGetKasterIntranetSystemFault()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m =>
                m.Query<AdressegrupperGetQuery, IEnumerable<AdressegruppeView>>(Arg<AdressegrupperGetQuery>.Is.NotNull))
                .Throw(fixture.CreateAnonymous<IntranetSystemException>());
            fixture.Inject(queryBus);

            var service = fixture.CreateAnonymous<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetSystemFault>>(
                () => service.AdressegrupperGet(fixture.CreateAnonymous<AdressegrupperGetQuery>()));
        }

        /// <summary>
        /// Tester, at AdressegrupperGet kaster en IntranetSystemFault ved unhandled exception.
        /// </summary>
        [Test]
        public void TestAtAdressegrupperGetKasterIntranetSystemFaultVedUnhandledException()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m =>
                m.Query<AdressegrupperGetQuery, IEnumerable<AdressegruppeView>>(Arg<AdressegrupperGetQuery>.Is.NotNull))
                .Throw(fixture.CreateAnonymous<Exception>());
            fixture.Inject(queryBus);

            var service = fixture.CreateAnonymous<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetSystemFault>>(
                () => service.AdressegrupperGet(fixture.CreateAnonymous<AdressegrupperGetQuery>()));
        }

        /// <summary>
        /// Tester, at BetalingsbetingelserGet kalder QueryBus.
        /// </summary>
        [Test]
        public void TestAtBetalingsbetingelserGetKalderQueryBus()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            fixture.Inject(queryBus);

            var service = fixture.CreateAnonymous<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            service.BetalingsbetingelserGet(fixture.CreateAnonymous<BetalingsbetingelserGetQuery>());

            queryBus.AssertWasCalled(
                m =>
                m.Query<BetalingsbetingelserGetQuery, IEnumerable<BetalingsbetingelseView>>(
                    Arg<BetalingsbetingelserGetQuery>.Is.NotNull));
        }

        /// <summary>
        /// Tester, at BetalingsbetingelserGet kaster en IntranetRepositoryFault.
        /// </summary>
        [Test]
        public void TestAtBetalingsbetingelserGetKasterIntranetRepositoryFault()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m =>
                m.Query<BetalingsbetingelserGetQuery, IEnumerable<BetalingsbetingelseView>>(
                    Arg<BetalingsbetingelserGetQuery>.Is.NotNull))
                .Throw(fixture.CreateAnonymous<IntranetRepositoryException>());
            fixture.Inject(queryBus);

            var service = fixture.CreateAnonymous<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetRepositoryFault>>(
                () => service.BetalingsbetingelserGet(fixture.CreateAnonymous<BetalingsbetingelserGetQuery>()));
        }

        /// <summary>
        /// Tester, at BetalingsbetingelserGet kaster en IntranetBusinessFault.
        /// </summary>
        [Test]
        public void TestAtBetalingsbetingelserGetKasterIntranetBusinessFault()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m =>
                m.Query<BetalingsbetingelserGetQuery, IEnumerable<BetalingsbetingelseView>>(
                    Arg<BetalingsbetingelserGetQuery>.Is.NotNull))
                .Throw(fixture.CreateAnonymous<IntranetBusinessException>());
            fixture.Inject(queryBus);

            var service = fixture.CreateAnonymous<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetBusinessFault>>(
                () => service.BetalingsbetingelserGet(fixture.CreateAnonymous<BetalingsbetingelserGetQuery>()));
        }

        /// <summary>
        /// Tester, at BetalingsbetingelserGet kaster en IntranetSystemFault.
        /// </summary>
        [Test]
        public void TestAtBetalingsbetingelserGetKasterIntranetSystemFault()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m =>
                m.Query<BetalingsbetingelserGetQuery, IEnumerable<BetalingsbetingelseView>>(
                    Arg<BetalingsbetingelserGetQuery>.Is.NotNull))
                .Throw(fixture.CreateAnonymous<IntranetSystemException>());
            fixture.Inject(queryBus);

            var service = fixture.CreateAnonymous<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetSystemFault>>(
                () => service.BetalingsbetingelserGet(fixture.CreateAnonymous<BetalingsbetingelserGetQuery>()));
        }

        /// <summary>
        /// Tester, at BetalingsbetingelserGet kaster en IntranetSystemFault ved unhandled exception.
        /// </summary>
        [Test]
        public void TestAtBetalingsbetingelserGetKasterIntranetSystemFaultVedUnhandledException()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m =>
                m.Query<BetalingsbetingelserGetQuery, IEnumerable<BetalingsbetingelseView>>(
                    Arg<BetalingsbetingelserGetQuery>.Is.NotNull))
                .Throw(fixture.CreateAnonymous<Exception>());
            fixture.Inject(queryBus);

            var service = fixture.CreateAnonymous<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetSystemFault>>(
                () => service.BetalingsbetingelserGet(fixture.CreateAnonymous<BetalingsbetingelserGetQuery>()));
        }
    }
}
