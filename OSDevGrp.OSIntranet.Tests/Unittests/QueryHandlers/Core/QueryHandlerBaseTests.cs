using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.QueryHandlers.Core;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers.Core
{
    /// <summary>
    /// Tester basisklasse for en QueryHandler.
    /// </summary>
    [TestFixture]
    public class QueryHandlerBaseTests
    {
        /// <summary>
        /// Egen klasse til test af basisklasse for en QueryHandler.
        /// </summary>
        private class MyQueryHandler : QueryHandlerBase
        {
            /// <summary>
            /// Danner egen klasse til test af basisklasse for en QueryHandler.
            /// </summary>
            /// <param name="objectMapper">Implementering af objectmapper.</param>
            public MyQueryHandler(IObjectMapper objectMapper)
                : base(objectMapper)
            {
            }
        }

        /// <summary>
        /// Tester, at konstruktøren initierer QueryHandlerBase.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererQueryHandlerBase()
        {
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            var queryHandler = new MyQueryHandler(objectMapper);
            Assert.That(queryHandler, Is.Not.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis objectmapperen er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisObjectMapperErNull()
        {
            Assert.Throws<ArgumentNullException>(() => new MyQueryHandler(null));
        }

        /// <summary>
        /// Tester, at Map kaster en ArgumentNullException, hvis domæneobjektet er null.
        /// </summary>
        [Test]
        public void TestAtMapKasterArgumentNullExceptionHvisDomainObjectErNull()
        {
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            var queryHandler = new MyQueryHandler(objectMapper);
            Assert.That(queryHandler, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => queryHandler.Map<Regnskab, RegnskabslisteView>(null));
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
            var queryHandler = new MyQueryHandler(objectMapper);
            Assert.That(queryHandler, Is.Not.Null);

            var view = queryHandler.Map<Regnskab, RegnskabslisteView>(fixture.CreateAnonymous<Regnskab>());
            Assert.That(view, Is.Not.Null);

            objectMapper.AssertWasCalled(m => m.Map<Regnskab, RegnskabslisteView>(Arg<Regnskab>.Is.NotNull));
        }

        /// <summary>
        /// Tester, at MapMany kaster en ArgumentNullException, hvis domæneobjekter er null.
        /// </summary>
        [Test]
        public void TestAtMapManyKasterArgumentNullExceptionHvisDomainObjectsErNull()
        {
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            var queryHandler = new MyQueryHandler(objectMapper);
            Assert.That(queryHandler, Is.Not.Null);

            IEnumerable<Regnskab> regnskaber = null;
            Assert.Throws<ArgumentNullException>(() => queryHandler.MapMany<Regnskab, RegnskabslisteView>(regnskaber));
        }

        /// <summary>
        /// Tester, at MapMany mapper flere domæneobjekter til views.
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
            var queryHandler = new MyQueryHandler(objectMapper);
            Assert.That(queryHandler, Is.Not.Null);

            var views = queryHandler.MapMany<Regnskab, RegnskabslisteView>(fixture.CreateMany<Regnskab>(3));
            Assert.That(views, Is.Not.Null);
            Assert.That(views.Count(), Is.EqualTo(3));

            objectMapper.AssertWasCalled(
                m =>
                m.Map<IEnumerable<Regnskab>, IEnumerable<RegnskabslisteView>>(Arg<IEnumerable<Regnskab>>.Is.NotNull));
        }
    }
}
