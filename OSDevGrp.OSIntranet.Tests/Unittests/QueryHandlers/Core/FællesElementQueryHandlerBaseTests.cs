using System;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Fælles;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.QueryHandlers.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using NUnit.Framework;
using AutoFixture;
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

        /// <summary>
        /// Tester, at BrevhovedGetByNummer henter en given brevhoved.
        /// </summary>
        [Test]
        public void TestAtBrevhovedGetByNummerHenterBrevhoved()
        {
            var fixture = new Fixture();
            var brevhoveder = fixture.CreateMany<Brevhoved>(3).ToList();

            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepository.Expect(m => m.BrevhovedGetAll())
                .Return(brevhoveder);
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            var queryHandler = new MyFællesElementQueryHandler(fællesRepository, objectMapper);
            Assert.That(queryHandler, Is.Not.Null);

            var brevhoved = queryHandler.BrevhovedGetByNummer(brevhoveder.ElementAt(1).Nummer);
            Assert.That(brevhoved, Is.Not.Null);
            Assert.That(brevhoved.Nummer, Is.EqualTo(brevhoveder.ElementAt(1).Nummer));
        }

        /// <summary>
        /// Tester, at BrevhovedGetByNummer kaster en IntranetRepositoryException, hvis brevhoved ikke findes.
        /// </summary>
        [Test]
        public void TestAtBrevhovedGetByNummerKasterIntranetRepositoryExceptionHvisBrevhovedIkkeFindes()
        {
            var fixture = new Fixture();
            var brevhoveder = fixture.CreateMany<Brevhoved>(3).ToList();

            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepository.Expect(m => m.BrevhovedGetAll())
                .Return(brevhoveder);
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            var queryHandler = new MyFællesElementQueryHandler(fællesRepository, objectMapper);
            Assert.That(queryHandler, Is.Not.Null);

            Assert.Throws<IntranetRepositoryException>(() => queryHandler.BrevhovedGetByNummer(-1));
        }
    }
}
