using System;
using System.Linq;
using OSDevGrp.OSIntranet.Domain.Interfaces.Fælles;
using OSDevGrp.OSIntranet.Domain.Interfaces.Kalender;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
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
            /// <param name="fællesRepository">Implementering af repository til fælles elementer i domænet, såsom systemer under OSWEBDB.</param>
            /// <param name="objectMapper">Implementering af objectmapper.</param>
            public MyKalenderQueryHandler(IKalenderRepository kalenderRepository, IFællesRepository fællesRepository, IObjectMapper objectMapper)
                : base(kalenderRepository, fællesRepository, objectMapper)
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
            fixture.Inject(MockRepository.GenerateMock<IFællesRepository>());
            fixture.Inject(MockRepository.GenerateMock<IObjectMapper>());

            var queryHandler = fixture.CreateAnonymous<MyKalenderQueryHandler>();
            Assert.That(queryHandler, Is.Not.Null);
            Assert.That(queryHandler.KalenderRepository, Is.Not.Null);
            Assert.That(queryHandler.FællesRepository, Is.Not.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis repository til kalenderdelen under OSWEBDB er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisKalenderRepositoryErNull()
        {
            var fixture = new Fixture();
            fixture.Inject<IKalenderRepository>(null);
            fixture.Inject(MockRepository.GenerateMock<IFællesRepository>());
            fixture.Inject(MockRepository.GenerateMock<IObjectMapper>());

            Assert.Throws<ArgumentNullException>(
                () =>
                new MyKalenderQueryHandler(fixture.CreateAnonymous<IKalenderRepository>(),
                                           fixture.CreateAnonymous<IFællesRepository>(),
                                           fixture.CreateAnonymous<IObjectMapper>()));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis repository til fælles elementer i domænet er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisFællesRepositoryErNull()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<IKalenderRepository>());
            fixture.Inject<IFællesRepository>(null);
            fixture.Inject(MockRepository.GenerateMock<IObjectMapper>());

            Assert.Throws<ArgumentNullException>(
                () =>
                new MyKalenderQueryHandler(fixture.CreateAnonymous<IKalenderRepository>(),
                                           fixture.CreateAnonymous<IFællesRepository>(),
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
            fixture.Inject(MockRepository.GenerateMock<IFællesRepository>());
            fixture.Inject<IObjectMapper>(null);

            Assert.Throws<ArgumentNullException>(
                () =>
                new MyKalenderQueryHandler(fixture.CreateAnonymous<IKalenderRepository>(),
                                           fixture.CreateAnonymous<IFællesRepository>(),
                                           fixture.CreateAnonymous<IObjectMapper>()));
        }

        /// <summary>
        /// Tester, at SystemGetByNummer henter og returnerer et givent system under OSWEBDB.
        /// </summary>
        [Test]
        public void TestAtSystemGetByNummerHenterSystem()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<IKalenderRepository>());
            fixture.Inject(MockRepository.GenerateMock<IObjectMapper>());

            fixture.Customize<ISystem>(e => e.FromFactory(() => MockRepository.GenerateMock<ISystem>()));
            var systemer = fixture.CreateMany<ISystem>(4).ToList();
            foreach (var system in systemer)
            {
                system.Expect(m => m.Nummer)
                    .Return(fixture.CreateAnonymous<int>())
                    .Repeat.Any();
            }
            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepository.Expect(m => m.SystemGetAll())
                .Return(systemer)
                .Repeat.Any();
            fixture.Inject(fællesRepository);

            var queryHandler = fixture.CreateAnonymous<MyKalenderQueryHandler>();
            Assert.That(queryHandler, Is.Not.Null);

            var result = queryHandler.SystemGetByNummer(systemer.ElementAt(1).Nummer);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Nummer, Is.EqualTo(systemer.ElementAt(1).Nummer));

            fællesRepository.AssertWasCalled(m => m.SystemGetAll());
        }

        /// <summary>
        /// Tester, at SystemGetByNummer kaster en IntranetRepositoryException, hvis systemet ikke findes.
        /// </summary>
        [Test]
        public void TestAtSystemGetByNummerKasterIntranetRepositoryExceptionHvisSystemIkkeFindes()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<IKalenderRepository>());
            fixture.Inject(MockRepository.GenerateMock<IObjectMapper>());

            fixture.Customize<ISystem>(e => e.FromFactory(() => MockRepository.GenerateMock<ISystem>()));
            var systemer = fixture.CreateMany<ISystem>(4).ToList();
            foreach (var system in systemer)
            {
                system.Expect(m => m.Nummer)
                    .Return(fixture.CreateAnonymous<int>())
                    .Repeat.Any();
            }
            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepository.Expect(m => m.SystemGetAll())
                .Return(systemer)
                .Repeat.Any();
            fixture.Inject(fællesRepository);

            var queryHandler = fixture.CreateAnonymous<MyKalenderQueryHandler>();
            Assert.That(queryHandler, Is.Not.Null);

            Assert.Throws<IntranetRepositoryException>(
                () => queryHandler.SystemGetByNummer(fixture.CreateAnonymous<int>()));

            fællesRepository.AssertWasCalled(m => m.SystemGetAll());
        }

        /// <summary>
        /// Tester, at BrugerlisteGetBySystemAndInitialer kaster en ArgumentNullException, hvis systemet, hvorfra brugere skal hentes, er null.
        /// </summary>
        [Test]
        public void TestAtBrugerlisteGetBySystemAndInitialerKasterArgumentNullExceptionHvisSystemErNull()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<IKalenderRepository>());
            fixture.Inject(MockRepository.GenerateMock<IFællesRepository>());
            fixture.Inject(MockRepository.GenerateMock<IObjectMapper>());

            var queryHandler = fixture.CreateAnonymous<MyKalenderQueryHandler>();
            Assert.That(queryHandler, Is.Not.Null);

            fixture.Inject<ISystem>(null);
            Assert.Throws<ArgumentNullException>(
                () =>
                queryHandler.BrugerlisteGetBySystemAndInitialer(fixture.CreateAnonymous<ISystem>(),
                                                                fixture.CreateAnonymous<string>()));
        }

        /// <summary>
        /// Tester, at BrugerlisteGetBySystemAndInitialer kaster en ArgumentNullException, initialer for brugerne er null.
        /// </summary>
        [Test]
        public void TestAtBrugerlisteGetBySystemAndInitialerKasterArgumentNullExceptionHvisInitialerErNull()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<IKalenderRepository>());
            fixture.Inject(MockRepository.GenerateMock<IFællesRepository>());
            fixture.Inject(MockRepository.GenerateMock<IObjectMapper>());

            var queryHandler = fixture.CreateAnonymous<MyKalenderQueryHandler>();
            Assert.That(queryHandler, Is.Not.Null);

            fixture.Inject(MockRepository.GenerateMock<ISystem>());
            fixture.Inject<string>(null);
            Assert.Throws<ArgumentNullException>(
                () =>
                queryHandler.BrugerlisteGetBySystemAndInitialer(fixture.CreateAnonymous<ISystem>(),
                                                                fixture.CreateAnonymous<string>()));
        }

        /// <summary>
        /// Tester, at BrugerlisteGetBySystemAndInitialer kaster en ArgumentNullException, initialer for brugerne er tom.
        /// </summary>
        [Test]
        public void TestAtBrugerlisteGetBySystemAndInitialerKasterArgumentNullExceptionHvisInitialerErEmpty()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<IKalenderRepository>());
            fixture.Inject(MockRepository.GenerateMock<IFællesRepository>());
            fixture.Inject(MockRepository.GenerateMock<IObjectMapper>());

            var queryHandler = fixture.CreateAnonymous<MyKalenderQueryHandler>();
            Assert.That(queryHandler, Is.Not.Null);

            fixture.Inject(MockRepository.GenerateMock<ISystem>());
            fixture.Inject(string.Empty);
            Assert.Throws<ArgumentNullException>(
                () =>
                queryHandler.BrugerlisteGetBySystemAndInitialer(fixture.CreateAnonymous<ISystem>(),
                                                                fixture.CreateAnonymous<string>()));
        }

        /// <summary>
        /// Tester, at BrugerlisteGetBySystemAndInitialer henter en liste af brugere med et givet sæt initialer.
        /// </summary>
        [Test]
        public void TestAtBrugerlisteGetBySystemAndInitialerHenterBrugerlister()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<IFællesRepository>());
            fixture.Inject(MockRepository.GenerateMock<IObjectMapper>());

            var system = MockRepository.GenerateMock<ISystem>();
            system.Expect(m => m.Nummer)
                .Return(fixture.CreateAnonymous<int>())
                .Repeat.Any();
            fixture.Inject(system);

            fixture.Customize<IBruger>(e => e.FromFactory(() => MockRepository.GenerateMock<IBruger>()));
            var brugere = fixture.CreateMany<IBruger>(7).ToList();
            foreach (var bruger in brugere)
            {
                bruger.Expect(m => m.Initialer)
                    .Return(fixture.CreateAnonymous<string>())
                    .Repeat.Any();
            }
            var kalenderRepository = MockRepository.GenerateMock<IKalenderRepository>();
            kalenderRepository.Expect(m => m.BrugerGetAllBySystem(Arg<int>.Is.Equal(system.Nummer)))
                .Return(brugere);
            fixture.Inject(kalenderRepository);

            var queryHandler = fixture.CreateAnonymous<MyKalenderQueryHandler>();
            Assert.That(queryHandler, Is.Not.Null);

            var brugerliste = queryHandler.BrugerlisteGetBySystemAndInitialer(fixture.CreateAnonymous<ISystem>(),
                                                                              brugere.ElementAt(1).Initialer);
            Assert.That(brugerliste, Is.Not.Null);
            Assert.That(brugerliste.Count(), Is.EqualTo(1));

            kalenderRepository.AssertWasCalled(m => m.BrugerGetAllBySystem(Arg<int>.Is.Equal(system.Nummer)));
        }

        /// <summary>
        /// Tester, at BrugerlisteGetBySystemAndInitialer kaster en IntranetRepositoryException, hvis der ikke findes brugere med de givne initialer.
        /// </summary>
        [Test]
        public void TestAtBrugerlisteGetBySystemAndInitialerKasterIntranetRepositoryExceptionHvisInitialerIkkeFindes()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<IFællesRepository>());
            fixture.Inject(MockRepository.GenerateMock<IObjectMapper>());

            var system = MockRepository.GenerateMock<ISystem>();
            system.Expect(m => m.Nummer)
                .Return(fixture.CreateAnonymous<int>())
                .Repeat.Any();
            fixture.Inject(system);

            fixture.Customize<IBruger>(e => e.FromFactory(() => MockRepository.GenerateMock<IBruger>()));
            var brugere = fixture.CreateMany<IBruger>(7).ToList();
            foreach (var bruger in brugere)
            {
                bruger.Expect(m => m.Initialer)
                    .Return(fixture.CreateAnonymous<string>())
                    .Repeat.Any();
            }
            var kalenderRepository = MockRepository.GenerateMock<IKalenderRepository>();
            kalenderRepository.Expect(m => m.BrugerGetAllBySystem(Arg<int>.Is.Equal(system.Nummer)))
                .Return(brugere);
            fixture.Inject(kalenderRepository);

            var queryHandler = fixture.CreateAnonymous<MyKalenderQueryHandler>();
            Assert.That(queryHandler, Is.Not.Null);

            Assert.Throws<IntranetRepositoryException>(
                () =>
                queryHandler.BrugerlisteGetBySystemAndInitialer(fixture.CreateAnonymous<ISystem>(),
                                                                fixture.CreateAnonymous<string>()));

            kalenderRepository.AssertWasCalled(m => m.BrugerGetAllBySystem(Arg<int>.Is.Equal(system.Nummer)));
        }
    }
}
