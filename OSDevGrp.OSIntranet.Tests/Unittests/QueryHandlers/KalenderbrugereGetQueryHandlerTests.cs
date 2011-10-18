using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Domain.Interfaces.Fælles;
using OSDevGrp.OSIntranet.Domain.Interfaces.Kalender;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.QueryHandlers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers
{
    /// <summary>
    /// Tester QueryHandler til håndtering af forespørgelsen: KalenderbrugereGetQuery.
    /// </summary>
    [TestFixture]
    public class KalenderbrugereGetQueryHandlerTests
    {
        /// <summary>
        /// Tester, at Query kaster en ArgumentNullException, hvis Query er null.
        /// </summary>
        [Test]
        public void TestAtQueryKasterArgumentNullExceptionHvisQueryErNull()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<IKalenderRepository>());
            fixture.Inject(MockRepository.GenerateMock<IFællesRepository>());
            fixture.Inject(MockRepository.GenerateMock<IObjectMapper>());

            var queryHandler = fixture.CreateAnonymous<KalenderbrugereGetQueryHandler>();
            Assert.That(queryHandler, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => queryHandler.Query(null));
        }

        /// <summary>
        /// Tester, at Query henter kalenderbrugere til et system under OSWEBDB..
        /// </summary>
        [Test]
        public void TestAtQueryHenterKalenderbrugere()
        {
            var fixture = new Fixture();

            fixture.Customize<ISystem>(e => e.FromFactory(() =>
                                                              {
                                                                  var system = MockRepository.GenerateMock<ISystem>();
                                                                  system.Expect(m => m.Nummer)
                                                                      .Return(fixture.CreateAnonymous<int>())
                                                                      .Repeat.Any();
                                                                  return system;
                                                              }));
            var systemer = fixture.CreateMany<ISystem>(4).ToList();
            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepository.Expect(m => m.SystemGetAll())
                .Return(systemer);
            fixture.Inject(fællesRepository);

            fixture.Customize<IBruger>(e => e.FromFactory(() =>
                                                              {
                                                                  var bruger = MockRepository.GenerateMock<IBruger>();
                                                                  bruger.Expect(m => m.System)
                                                                      .Return(systemer.ElementAt(1))
                                                                      .Repeat.Any();
                                                                  bruger.Expect(m => m.Id)
                                                                      .Return(fixture.CreateAnonymous<int>())
                                                                      .Repeat.Any();
                                                                  bruger.Expect(m => m.Initialer)
                                                                      .Return(fixture.CreateAnonymous<string>())
                                                                      .Repeat.Any();
                                                                  bruger.Expect(m => m.Navn)
                                                                      .Return(fixture.CreateAnonymous<string>())
                                                                      .Repeat.Any();
                                                                  return bruger;
                                                              }));
            var kalenderRepository = MockRepository.GenerateMock<IKalenderRepository>();
            kalenderRepository.Expect(m => m.BrugerGetAllBySystem(Arg<int>.Is.Equal(systemer.ElementAt(1).Nummer)))
                .Return(fixture.CreateMany<IBruger>(7).ToList());
            fixture.Inject(kalenderRepository);

            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            objectMapper.Expect(
                m => m.Map<IEnumerable<IBruger>, IEnumerable<KalenderbrugerView>>(Arg<IEnumerable<IBruger>>.Is.NotNull))
                .Return(fixture.CreateMany<KalenderbrugerView>(7));
            fixture.Inject(objectMapper);

            var queryHandler = fixture.CreateAnonymous<KalenderbrugereGetQueryHandler>();
            Assert.That(queryHandler, Is.Not.Null);

            var query = new KalenderbrugereGetQuery
                            {
                                System = systemer.ElementAt(1).Nummer
                            };
            var result = queryHandler.Query(query);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(7));

            fællesRepository.AssertWasCalled(m => m.SystemGetAll());
            kalenderRepository.Expect(m => m.BrugerGetAllBySystem(Arg<int>.Is.Equal(systemer.ElementAt(1).Nummer)));
            objectMapper.AssertWasCalled(
                m => m.Map<IEnumerable<IBruger>, IEnumerable<KalenderbrugerView>>(Arg<IEnumerable<IBruger>>.Is.NotNull));
        }
    }
}
