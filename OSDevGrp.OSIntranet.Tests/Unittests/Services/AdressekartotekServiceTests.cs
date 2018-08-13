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
using AutoFixture;
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
            Assert.Throws<ArgumentNullException>(() => new AdressekartotekService(fixture.Create<IQueryBus>()));
        }

        /// <summary>
        /// Tester, at TelefonlisteGet kalder QueryBus.
        /// </summary>
        [Test]
        public void TestAtTelefonlisteGetKalderQueryBus()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            fixture.Inject(queryBus);

            var service = fixture.Create<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            service.TelefonlisteGet(fixture.Create<TelefonlisteGetQuery>());

            queryBus.AssertWasCalled(
                m => m.Query<TelefonlisteGetQuery, IEnumerable<TelefonlisteView>>(Arg<TelefonlisteGetQuery>.Is.NotNull));
        }

        /// <summary>
        /// Tester, at TelefonlisteGet kaster en IntranetRepositoryFault.
        /// </summary>
        [Test]
        public void TestAtTelefonlisteGetKasterIntranetRepositoryFault()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m => m.Query<TelefonlisteGetQuery, IEnumerable<TelefonlisteView>>(Arg<TelefonlisteGetQuery>.Is.NotNull))
                .Throw(fixture.Create<IntranetRepositoryException>());
            fixture.Inject(queryBus);

            var service = fixture.Create<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetRepositoryFault>>(
                () => service.TelefonlisteGet(fixture.Create<TelefonlisteGetQuery>()));
        }

        /// <summary>
        /// Tester, at TelefonlisteGet kaster en IntranetBusinessFault.
        /// </summary>
        [Test]
        public void TestAtTelefonlisteGetKasterIntranetBusinessFault()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m => m.Query<TelefonlisteGetQuery, IEnumerable<TelefonlisteView>>(Arg<TelefonlisteGetQuery>.Is.NotNull))
                .Throw(fixture.Create<IntranetBusinessException>());
            fixture.Inject(queryBus);

            var service = fixture.Create<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetBusinessFault>>(
                () => service.TelefonlisteGet(fixture.Create<TelefonlisteGetQuery>()));
        }

        /// <summary>
        /// Tester, at TelefonlisteGet kaster en IntranetSystemFault.
        /// </summary>
        [Test]
        public void TestAtTelefonlisteGetKasterIntranetSystemFault()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m => m.Query<TelefonlisteGetQuery, IEnumerable<TelefonlisteView>>(Arg<TelefonlisteGetQuery>.Is.NotNull))
                .Throw(fixture.Create<IntranetSystemException>());
            fixture.Inject(queryBus);

            var service = fixture.Create<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetSystemFault>>(
                () => service.TelefonlisteGet(fixture.Create<TelefonlisteGetQuery>()));
        }

        /// <summary>
        /// Tester, at TelefonlisteGet kaster en IntranetSystemFault ved unhandled exception.
        /// </summary>
        [Test]
        public void TestAtTelefonlisteGetKasterIntranetSystemFaultVedUnhandledException()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m => m.Query<TelefonlisteGetQuery, IEnumerable<TelefonlisteView>>(Arg<TelefonlisteGetQuery>.Is.NotNull))
                .Throw(fixture.Create<Exception>());
            fixture.Inject(queryBus);

            var service = fixture.Create<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetSystemFault>>(
                () => service.TelefonlisteGet(fixture.Create<TelefonlisteGetQuery>()));
        }

        /// <summary>
        /// Tester, at PersonlisteGet kalder QueryBus.
        /// </summary>
        [Test]
        public void TestAtPersonlisteGetKalderQueryBus()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            fixture.Inject(queryBus);

            var service = fixture.Create<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            service.PersonlisteGet(fixture.Create<PersonlisteGetQuery>());

            queryBus.AssertWasCalled(
                m => m.Query<PersonlisteGetQuery, IEnumerable<PersonView>>(Arg<PersonlisteGetQuery>.Is.NotNull));
        }

        /// <summary>
        /// Tester, at PersonlisteGet kaster en IntranetRepositoryFault.
        /// </summary>
        [Test]
        public void TestAtPersonlisteGetKasterIntranetRepositoryFault()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m => m.Query<PersonlisteGetQuery, IEnumerable<PersonView>>(Arg<PersonlisteGetQuery>.Is.NotNull))
                .Throw(fixture.Create<IntranetRepositoryException>());
            fixture.Inject(queryBus);

            var service = fixture.Create<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetRepositoryFault>>(
                () => service.PersonlisteGet(fixture.Create<PersonlisteGetQuery>()));
        }

        /// <summary>
        /// Tester, at PersonlisteGet kaster en IntranetBusinessFault.
        /// </summary>
        [Test]
        public void TestAtPersonlisteGetKasterIntranetBusinessFault()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m => m.Query<PersonlisteGetQuery, IEnumerable<PersonView>>(Arg<PersonlisteGetQuery>.Is.NotNull))
                .Throw(fixture.Create<IntranetBusinessException>());
            fixture.Inject(queryBus);

            var service = fixture.Create<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetBusinessFault>>(
                () => service.PersonlisteGet(fixture.Create<PersonlisteGetQuery>()));
        }

        /// <summary>
        /// Tester, at PersonlisteGet kaster en IntranetSystemFault.
        /// </summary>
        [Test]
        public void TestAtPersonlisteGetKasterIntranetSystemFault()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m => m.Query<PersonlisteGetQuery, IEnumerable<PersonView>>(Arg<PersonlisteGetQuery>.Is.NotNull))
                .Throw(fixture.Create<IntranetSystemException>());
            fixture.Inject(queryBus);

            var service = fixture.Create<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetSystemFault>>(
                () => service.PersonlisteGet(fixture.Create<PersonlisteGetQuery>()));
        }

        /// <summary>
        /// Tester, at PersonlisteGet kaster en IntranetSystemFault ved unhandled exception.
        /// </summary>
        [Test]
        public void TestAtPersonlisteGetKasterIntranetSystemFaultVedUnhandledException()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m => m.Query<PersonlisteGetQuery, IEnumerable<PersonView>>(Arg<PersonlisteGetQuery>.Is.NotNull))
                .Throw(fixture.Create<Exception>());
            fixture.Inject(queryBus);

            var service = fixture.Create<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetSystemFault>>(
                () => service.PersonlisteGet(fixture.Create<PersonlisteGetQuery>()));
        }

        /// <summary>
        /// Tester, at PersonGet kalder QueryBus.
        /// </summary>
        [Test]
        public void TestAtPersonGetKalderQueryBus()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            fixture.Inject(queryBus);

            var service = fixture.Create<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            service.PersonGet(fixture.Create<PersonGetQuery>());

            queryBus.AssertWasCalled(m => m.Query<PersonGetQuery, PersonView>(Arg<PersonGetQuery>.Is.NotNull));
        }

        /// <summary>
        /// Tester, at PersonGet kaster en IntranetRepositoryFault.
        /// </summary>
        [Test]
        public void TestAtPersonGetKasterIntranetRepositoryFault()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<PersonGetQuery, PersonView>(Arg<PersonGetQuery>.Is.NotNull))
                .Throw(fixture.Create<IntranetRepositoryException>());
            fixture.Inject(queryBus);

            var service = fixture.Create<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetRepositoryFault>>(
                () => service.PersonGet(fixture.Create<PersonGetQuery>()));
        }

        /// <summary>
        /// Tester, at PersonGet kaster en IntranetBusinessFault.
        /// </summary>
        [Test]
        public void TestAtPersonGetKasterIntranetBusinessFault()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<PersonGetQuery, PersonView>(Arg<PersonGetQuery>.Is.NotNull))
                .Throw(fixture.Create<IntranetBusinessException>());
            fixture.Inject(queryBus);

            var service = fixture.Create<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetBusinessFault>>(
                () => service.PersonGet(fixture.Create<PersonGetQuery>()));
        }

        /// <summary>
        /// Tester, at PersonGet kaster en IntranetSystemFault.
        /// </summary>
        [Test]
        public void TestAtPersonGetKasterIntranetSystemFault()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<PersonGetQuery, PersonView>(Arg<PersonGetQuery>.Is.NotNull))
                .Throw(fixture.Create<IntranetSystemException>());
            fixture.Inject(queryBus);

            var service = fixture.Create<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetSystemFault>>(
                () => service.PersonGet(fixture.Create<PersonGetQuery>()));
        }

        /// <summary>
        /// Tester, at PersonGet kaster en IntranetSystemFault ved unhandled exception.
        /// </summary>
        [Test]
        public void TestAtPersonGetKasterIntranetSystemFaultVedUnhandledException()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<PersonGetQuery, PersonView>(Arg<PersonGetQuery>.Is.NotNull))
                .Throw(fixture.Create<Exception>());
            fixture.Inject(queryBus);

            var service = fixture.Create<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetSystemFault>>(
                () => service.PersonGet(fixture.Create<PersonGetQuery>()));
        }

        /// <summary>
        /// Tester, at FirmalisteGet kalder QueryBus.
        /// </summary>
        [Test]
        public void TestAtFirmalisteGetKalderQueryBus()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            fixture.Inject(queryBus);

            var service = fixture.Create<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            service.FirmalisteGet(fixture.Create<FirmalisteGetQuery>());

            queryBus.AssertWasCalled(
                m => m.Query<FirmalisteGetQuery, IEnumerable<FirmaView>>(Arg<FirmalisteGetQuery>.Is.NotNull));
        }

        /// <summary>
        /// Tester, at FirmalisteGet kaster en IntranetRepositoryFault.
        /// </summary>
        [Test]
        public void TestAtFirmalisteGetKasterIntranetRepositoryFault()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<FirmalisteGetQuery, IEnumerable<FirmaView>>(Arg<FirmalisteGetQuery>.Is.NotNull))
                .Throw(fixture.Create<IntranetRepositoryException>());
            fixture.Inject(queryBus);

            var service = fixture.Create<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetRepositoryFault>>(
                () => service.FirmalisteGet(fixture.Create<FirmalisteGetQuery>()));
        }

        /// <summary>
        /// Tester, at FirmalisteGet kaster en IntranetBusinessFault.
        /// </summary>
        [Test]
        public void TestAtFirmalisteGetKasterIntranetBusinessFault()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<FirmalisteGetQuery, IEnumerable<FirmaView>>(Arg<FirmalisteGetQuery>.Is.NotNull))
                .Throw(fixture.Create<IntranetBusinessException>());
            fixture.Inject(queryBus);

            var service = fixture.Create<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetBusinessFault>>(
                () => service.FirmalisteGet(fixture.Create<FirmalisteGetQuery>()));
        }

        /// <summary>
        /// Tester, at FirmalisteGet kaster en IntranetSystemFault.
        /// </summary>
        [Test]
        public void TestAtFirmalisteGetKasterIntranetSystemFault()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<FirmalisteGetQuery, IEnumerable<FirmaView>>(Arg<FirmalisteGetQuery>.Is.NotNull))
                .Throw(fixture.Create<IntranetSystemException>());
            fixture.Inject(queryBus);

            var service = fixture.Create<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetSystemFault>>(
                () => service.FirmalisteGet(fixture.Create<FirmalisteGetQuery>()));
        }

        /// <summary>
        /// Tester, at FirmalisteGet kaster en IntranetSystemFault ved unhandled exception.
        /// </summary>
        [Test]
        public void TestAtFirmalisteGetKasterIntranetSystemFaultVedUnhandledException()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<FirmalisteGetQuery, IEnumerable<FirmaView>>(Arg<FirmalisteGetQuery>.Is.NotNull))
                .Throw(fixture.Create<Exception>());
            fixture.Inject(queryBus);

            var service = fixture.Create<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetSystemFault>>(
                () => service.FirmalisteGet(fixture.Create<FirmalisteGetQuery>()));
        }

        /// <summary>
        /// Tester, at FirmaGet kalder QueryBus.
        /// </summary>
        [Test]
        public void TestAtFirmaGetKalderQueryBus()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            fixture.Inject(queryBus);

            var service = fixture.Create<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            service.FirmaGet(fixture.Create<FirmaGetQuery>());

            queryBus.AssertWasCalled(m => m.Query<FirmaGetQuery, FirmaView>(Arg<FirmaGetQuery>.Is.NotNull));
        }

        /// <summary>
        /// Tester, at FirmaGet kaster en IntranetRepositoryFault.
        /// </summary>
        [Test]
        public void TestAtFirmaGetKasterIntranetRepositoryFault()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<FirmaGetQuery, FirmaView>(Arg<FirmaGetQuery>.Is.NotNull))
                .Throw(fixture.Create<IntranetRepositoryException>());
            fixture.Inject(queryBus);

            var service = fixture.Create<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetRepositoryFault>>(
                () => service.FirmaGet(fixture.Create<FirmaGetQuery>()));
        }

        /// <summary>
        /// Tester, at FirmaGet kaster en IntranetBusinessFault.
        /// </summary>
        [Test]
        public void TestAtFirmaGetKasterIntranetBusinessFault()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<FirmaGetQuery, FirmaView>(Arg<FirmaGetQuery>.Is.NotNull))
                .Throw(fixture.Create<IntranetBusinessException>());
            fixture.Inject(queryBus);

            var service = fixture.Create<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetBusinessFault>>(
                () => service.FirmaGet(fixture.Create<FirmaGetQuery>()));
        }

        /// <summary>
        /// Tester, at FirmaGet kaster en IntranetSystemFault.
        /// </summary>
        [Test]
        public void TestAtFirmaGetKasterIntranetSystemFault()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<FirmaGetQuery, FirmaView>(Arg<FirmaGetQuery>.Is.NotNull))
                .Throw(fixture.Create<IntranetSystemException>());
            fixture.Inject(queryBus);

            var service = fixture.Create<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetSystemFault>>(
                () => service.FirmaGet(fixture.Create<FirmaGetQuery>()));
        }

        /// <summary>
        /// Tester, at FirmaGet kaster en IntranetSystemFault ved unhandled exception.
        /// </summary>
        [Test]
        public void TestAtFirmaGetKasterIntranetSystemFaultVedUnhandledException()
        {
            var fixture = new Fixture();

            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(m => m.Query<FirmaGetQuery, FirmaView>(Arg<FirmaGetQuery>.Is.NotNull))
                .Throw(fixture.Create<Exception>());
            fixture.Inject(queryBus);

            var service = fixture.Create<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetSystemFault>>(
                () => service.FirmaGet(fixture.Create<FirmaGetQuery>()));
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

            var service = fixture.Create<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            service.PostnumreGet(fixture.Create<PostnumreGetQuery>());

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
                .Throw(fixture.Create<IntranetRepositoryException>());
            fixture.Inject(queryBus);

            var service = fixture.Create<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetRepositoryFault>>(
                () => service.PostnumreGet(fixture.Create<PostnumreGetQuery>()));
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
                .Throw(fixture.Create<IntranetBusinessException>());
            fixture.Inject(queryBus);

            var service = fixture.Create<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetBusinessFault>>(
                () => service.PostnumreGet(fixture.Create<PostnumreGetQuery>()));
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
                .Throw(fixture.Create<IntranetSystemException>());
            fixture.Inject(queryBus);

            var service = fixture.Create<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetSystemFault>>(
                () => service.PostnumreGet(fixture.Create<PostnumreGetQuery>()));
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
                .Throw(fixture.Create<Exception>());
            fixture.Inject(queryBus);

            var service = fixture.Create<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetSystemFault>>(
                () => service.PostnumreGet(fixture.Create<PostnumreGetQuery>()));
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

            var service = fixture.Create<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            service.AdressegrupperGet(fixture.Create<AdressegrupperGetQuery>());

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
                .Throw(fixture.Create<IntranetRepositoryException>());
            fixture.Inject(queryBus);

            var service = fixture.Create<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetRepositoryFault>>(
                () => service.AdressegrupperGet(fixture.Create<AdressegrupperGetQuery>()));
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
                .Throw(fixture.Create<IntranetBusinessException>());
            fixture.Inject(queryBus);

            var service = fixture.Create<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetBusinessFault>>(
                () => service.AdressegrupperGet(fixture.Create<AdressegrupperGetQuery>()));
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
                .Throw(fixture.Create<IntranetSystemException>());
            fixture.Inject(queryBus);

            var service = fixture.Create<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetSystemFault>>(
                () => service.AdressegrupperGet(fixture.Create<AdressegrupperGetQuery>()));
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
                .Throw(fixture.Create<Exception>());
            fixture.Inject(queryBus);

            var service = fixture.Create<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetSystemFault>>(
                () => service.AdressegrupperGet(fixture.Create<AdressegrupperGetQuery>()));
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

            var service = fixture.Create<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            service.BetalingsbetingelserGet(fixture.Create<BetalingsbetingelserGetQuery>());

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
                .Throw(fixture.Create<IntranetRepositoryException>());
            fixture.Inject(queryBus);

            var service = fixture.Create<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetRepositoryFault>>(
                () => service.BetalingsbetingelserGet(fixture.Create<BetalingsbetingelserGetQuery>()));
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
                .Throw(fixture.Create<IntranetBusinessException>());
            fixture.Inject(queryBus);

            var service = fixture.Create<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetBusinessFault>>(
                () => service.BetalingsbetingelserGet(fixture.Create<BetalingsbetingelserGetQuery>()));
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
                .Throw(fixture.Create<IntranetSystemException>());
            fixture.Inject(queryBus);

            var service = fixture.Create<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetSystemFault>>(
                () => service.BetalingsbetingelserGet(fixture.Create<BetalingsbetingelserGetQuery>()));
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
                .Throw(fixture.Create<Exception>());
            fixture.Inject(queryBus);

            var service = fixture.Create<AdressekartotekService>();
            Assert.That(service, Is.Not.Null);

            Assert.Throws<FaultException<IntranetSystemFault>>(
                () => service.BetalingsbetingelserGet(fixture.Create<BetalingsbetingelserGetQuery>()));
        }
    }
}
