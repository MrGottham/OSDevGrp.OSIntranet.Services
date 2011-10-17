using System;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.QueryHandlers.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers.Core
{
    /// <summary>
    /// Tester basisklasse for en QueryHandler til kalenderdelen under OSWEBDB.
    /// </summary>
    [TestFixture]
    public class KalenderQueryHandlerBaseTests
    {
        /// <summary>
        /// Egen klasse til test af basisklasse for en QueryHandler til kalenderdelen under OSWEBDB.
        /// </summary>
        private class MyKalenderQueryHandler : KalenderQueryHandlerBase
        {
            /// <summary>
            /// Danner egen klasse til test af basisklasse for en QueryHandler til kalenderdelen under OSWEBDB.
            /// </summary>
            /// <param name="kalenderRepository">Implementering af repository til kalenderdelen under OSWEBDB.</param>
            /// <param name="objectMapper">Implementering af objectmapper.</param>
            public MyKalenderQueryHandler(IKalenderRepository kalenderRepository, IObjectMapper objectMapper)
                : base(kalenderRepository, objectMapper)
            {
            }
        }

        /// <summary>
        /// Tester, at konstruktøren initierer KalenderQueryHandlerBase.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererKalenderQueryHandlerBase()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<IKalenderRepository>());
            fixture.Inject(MockRepository.GenerateMock<IObjectMapper>());

            var queryHandler = fixture.CreateAnonymous<MyKalenderQueryHandler>();
            Assert.That(queryHandler, Is.Not.Null);
            Assert.That(queryHandler.Repository, Is.Not.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis repository til kalenderdelen under OSWEBDB er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisKalenderRepositoryErNull()
        {
            var fixture = new Fixture();
            fixture.Inject<IKalenderRepository>(null);
            fixture.Inject(MockRepository.GenerateMock<IObjectMapper>());

            Assert.Throws<ArgumentNullException>(
                () =>
                new MyKalenderQueryHandler(fixture.CreateAnonymous<IKalenderRepository>(),
                                           fixture.CreateAnonymous<IObjectMapper>()));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis objectmapperen er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisObjectMapperErNull()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<IKalenderRepository>());
            fixture.Inject<IObjectMapper>(null);

            Assert.Throws<ArgumentNullException>(
                () =>
                new MyKalenderQueryHandler(fixture.CreateAnonymous<IKalenderRepository>(),
                                           fixture.CreateAnonymous<IObjectMapper>()));
        }
    }
}
