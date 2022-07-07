using System;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.QueryHandlers.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using NUnit.Framework;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers.Core
{
    /// <summary>
    /// Tester basisklasse for en QueryHandler til fælles elementer i domænet, såsom brevhoved.
    /// </summary>
    [TestFixture]
    public class FællesElementQueryHandlerBaseTests
    {
        /// <summary>
        /// Egen klasse til test af basisklasse for en QueryHandler til fælles elementer i domænet, såsom brevhoved.
        /// </summary>
        private class MyFællesElementQueryHandler : FællesElementQueryHandlerBase
        {
            /// <summary>
            /// Danner egen klasse til test af basisklasse for en QueryHandler til fælles elementer i domænet, såsom brevhoved.
            /// </summary>
            /// <param name="fællesRepository">Implementering af repository til fælles elementer i domænet.</param>
            /// <param name="objectMapper">Implementering af objectmapper.</param>
            public MyFællesElementQueryHandler(IFællesRepository fællesRepository, IObjectMapper objectMapper)
                : base(fællesRepository, objectMapper)
            {
            }
        }

        /// <summary>
        /// Tester, at konstruktøren initierer FællesElementQueryHandlerBase.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererFællesElementQueryHandlerBase()
        {
            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            var queryHandler = new MyFællesElementQueryHandler(fællesRepository, objectMapper);
            Assert.That(queryHandler, Is.Not.Null);
            Assert.That(queryHandler.Repository, Is.Not.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis repository til fælles elementer er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisFællesRepositoryErNull()
        {
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            Assert.Throws<ArgumentNullException>(() => new MyFællesElementQueryHandler(null, objectMapper));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis objectmapperen er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisObjectMapperErNull()
        {
            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            Assert.Throws<ArgumentNullException>(() => new MyFællesElementQueryHandler(fællesRepository, null));
        }
    }
}
