using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces.Core;
using OSDevGrp.OSIntranet.CommandHandlers.Core;
using OSDevGrp.OSIntranet.Contracts.Commands;
using OSDevGrp.OSIntranet.Contracts.Responses;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.CommandHandlers.Core
{
    /// <summary>
    /// Tester extensions til basisklassen CommandHandlerBase.
    /// </summary>
    [TestFixture]
    public class CommandHandlerBaseExtensionsTests
    {
        /// <summary>
        /// Egen klasse til test af extensions til basisklassen CommandHandlerBase.
        /// </summary>
        private class MyCommandHandler : CommandHandlerBase
        {
        }

        /// <summary>
        /// Tester, at Map kaster en ArgumentNullException, hvis commandhandler er null.
        /// </summary>
        [Test]
        public void TestAtMapKasterArgumentNullExceptionHvisCommandHandlerErNull()
        {
            Assert.Throws<ArgumentNullException>(() => CommandHandlerBaseExtensions.Map<Regnskab, RegnskabslisteView>(null, null, null));
        }

        /// <summary>
        /// Tester, at Map kaster en ArgumentNullException, hvis objectmapperen er null.
        /// </summary>
        [Test]
        public void TestAtMapKasterArgumentNullExceptionHvisObjectMapperErNull()
        {
            var commandHandler = new MyCommandHandler();
            Assert.That(commandHandler, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => commandHandler.Map<Regnskab, RegnskabslisteView>(null, null));
        }

        /// <summary>
        /// Tester, at Map kaster en ArgumentNullException, hvis domæneobjekt er null.
        /// </summary>
        [Test]
        public void TestAtMapKasterArgumentNullExceptionHvisDomainObjectErNull()
        {
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();

            var commandHandler = new MyCommandHandler();
            Assert.That(commandHandler, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => commandHandler.Map<Regnskab, RegnskabslisteView>(objectMapper, null));
        }

        /// <summary>
        /// Tester, at Map mapper et domæneobjekt til et view.
        /// </summary>
        [Test]
        public void TestAtMapMapperDomainObjectTilView()
        {
            var fixture = new Fixture();

            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            objectMapper.Expect(m => m.Map<Regnskab, RegnskabslisteView>(Arg<Regnskab>.Is.NotNull))
                .Return(fixture.CreateAnonymous<RegnskabslisteView>());

            var commandHandler = new MyCommandHandler();
            Assert.That(commandHandler, Is.Not.Null);

            var domainObject = fixture.CreateAnonymous<Regnskab>();
            var view = commandHandler.Map<Regnskab, RegnskabslisteView>(objectMapper, domainObject);
            Assert.That(view, Is.Not.Null);

            objectMapper.AssertWasCalled(m => m.Map<Regnskab, RegnskabslisteView>(Arg<Regnskab>.Is.NotNull));
        }

        /// <summary>
        /// Tester, at MapMany kaster en ArgumentNullException, hvis commandhandler er null.
        /// </summary>
        [Test]
        public void TestAtMapManyKasterArgumentNullExceptionHvisCommandHandlerErNull()
        {
            Assert.Throws<ArgumentNullException>(() => CommandHandlerBaseExtensions.MapMany<Regnskab, RegnskabslisteView>(null, null, null));
        }

        /// <summary>
        /// Tester, at MapMany kaster en ArgumentNullException, hvis objectmapperen er null.
        /// </summary>
        [Test]
        public void TestAtMapManyKasterArgumentNullExceptionHvisObjectMapperErNull()
        {
            var commandHandler = new MyCommandHandler();
            Assert.That(commandHandler, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => commandHandler.MapMany<Regnskab, RegnskabslisteView>(null, null));
        }

        /// <summary>
        /// Tester, at MapMany kaster en ArgumentNullException, hvis domæneobjekter er null.
        /// </summary>
        [Test]
        public void TestAtMapManyKasterArgumentNullExceptionHvisDomainObjectsErNull()
        {
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();

            var commandHandler = new MyCommandHandler();
            Assert.That(commandHandler, Is.Not.Null);

            IEnumerable<Regnskab> regnskaber = null;
            Assert.Throws<ArgumentNullException>(() => commandHandler.MapMany<Regnskab, RegnskabslisteView>(objectMapper, regnskaber));
        }

        /// <summary>
        /// Tester, at MapMany mapper domæneobjekter til et views.
        /// </summary>
        [Test]
        public void TestAtMapManyMapperDomainObjectsTilViews()
        {
            var fixture = new Fixture();

            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            objectMapper.Expect(
                m =>
                m.Map<IEnumerable<Regnskab>, IEnumerable<RegnskabslisteView>>(Arg<IEnumerable<Regnskab>>.Is.NotNull))
                .Return(fixture.CreateMany<RegnskabslisteView>(3));

            var commandHandler = new MyCommandHandler();
            Assert.That(commandHandler, Is.Not.Null);

            var domainObjects = fixture.CreateMany<Regnskab>(3);
            var views = commandHandler.MapMany<Regnskab, RegnskabslisteView>(objectMapper, domainObjects);
            Assert.That(views, Is.Not.Null);
            Assert.That(views.Count(), Is.Not.Null);

            objectMapper.AssertWasCalled(
                m =>
                m.Map<IEnumerable<Regnskab>, IEnumerable<RegnskabslisteView>>(Arg<IEnumerable<Regnskab>>.Is.NotNull));
        }

        /// <summary>
        /// Tester, ExceptionHandler, der håndterer exception fra commandhandleres uden returværdi, kaster en ArgumentNullException, hvis commandhandler er null.
        /// </summary>
        [Test]
        public void TestAtExceptionHandlerUdenResponseKasterArgumentNullExceptionHvisCommandHandlerErNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => CommandHandlerBaseExtensions.ExceptionHandler<BogføringslinjeOpretCommand>(null, null, null));
        }

        /// <summary>
        /// Tester, ExceptionHandler, der håndterer exception fra commandhandleres uden returværdi, kaster en ArgumentNullException, hvis exception er null.
        /// </summary>
        [Test]
        public void TestAtExceptionHandlerUdenResponseKasterArgumentNullExceptionHvisExceptionErNull()
        {
            var fixture = new Fixture();

            var commandHandler = new MyCommandHandler();
            Assert.That(commandHandler, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(
                () => commandHandler.ExceptionHandler(fixture.CreateAnonymous<BogføringslinjeOpretCommand>(), null));
        }

        /// <summary>
        /// Tester, ExceptionHandler, der håndterer exception fra commandhandleres uden returværdi, kaster en IntranetSystemException.
        /// </summary>
        [Test]
        public void TestAtExceptionHandlerUdenResponseKasterIntranetSystemException()
        {
            var fixture = new Fixture();

            var commandHandler = new MyCommandHandler();
            Assert.That(commandHandler, Is.Not.Null);

            Assert.Throws<IntranetSystemException>(
                () =>
                commandHandler.ExceptionHandler(fixture.CreateAnonymous<BogføringslinjeOpretCommand>(),
                                                fixture.CreateAnonymous<Exception>()));
        }

        /// <summary>
        /// Tester, ExceptionHandler, der håndterer exception fra commandhandleres med returværdi, kaster en ArgumentNullException, hvis commandhandler er null.
        /// </summary>
        [Test]
        public void TestAtExceptionHandlerMedResponseKasterArgumentNullExceptionHvisCommandHandlerErNull()
        {
            Assert.Throws<ArgumentNullException>(
                () =>
                CommandHandlerBaseExtensions.ExceptionHandler<BogføringslinjeOpretCommand, BogføringslinjeOpretResponse>
                    (null, null, null));
        }

        /// <summary>
        /// Tester, ExceptionHandler, der håndterer exception fra commandhandleres med returværdi, kaster en ArgumentNullException, hvis exception er null.
        /// </summary>
        [Test]
        public void TestAtExceptionHandlerMedResponseKasterArgumentNullExceptionHvisExceptionErNull()
        {
            var fixture = new Fixture();

            var commandHandler = new MyCommandHandler();
            Assert.That(commandHandler, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(
                () =>
                commandHandler.ExceptionHandler<BogføringslinjeOpretCommand, BogføringslinjeOpretResponse>(
                    fixture.CreateAnonymous<BogføringslinjeOpretCommand>(), null));
        }

        /// <summary>
        /// Tester, ExceptionHandler, der håndterer exception fra commandhandleres med returværdi, kaster en IntranetSystemException.
        /// </summary>
        [Test]
        public void TestAtExceptionHandlerMedResponseKasterIntranetSystemException()
        {
            var fixture = new Fixture();

            var commandHandler = new MyCommandHandler();
            Assert.That(commandHandler, Is.Not.Null);

            Assert.Throws<IntranetSystemException>(
                () =>
                commandHandler.ExceptionHandler<BogføringslinjeOpretCommand, BogføringslinjeOpretResponse>(
                    fixture.CreateAnonymous<BogføringslinjeOpretCommand>(), fixture.CreateAnonymous<Exception>()));
        }

    }
}
