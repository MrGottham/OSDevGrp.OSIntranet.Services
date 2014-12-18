using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Domain.Interfaces.Fælles;
using OSDevGrp.OSIntranet.Domain.Interfaces.Kalender;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.QueryHandlers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers
{
    /// <summary>
    /// Tester QueryHandler til håndtering af forespørgelsen: KalenderbrugerAftaleGetQuery.
    /// </summary>
    [TestFixture]
    public class KalenderbrugerAftaleGetQueryHandlerTests
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

            var queryHandler = fixture.Create<KalenderbrugerAftaleGetQueryHandler>();
            Assert.That(queryHandler, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => queryHandler.Query(null));
        }

        /// <summary>
        /// Tester, at Query henter en given kalenderaftale til en kalenderbruger med et givent sæt initialer.
        /// </summary>
        [Test]
        public void TestAtQueryHenterKalenderbrugerAftale()
        {
            var fixture = new Fixture();
            fixture.Inject(DateTime.Now);
            fixture.Inject(MockRepository.GenerateMock<IObjectMapper>());

            var r = new Random(fixture.Create<DateTime>().Millisecond);

            fixture.Customize<ISystem>(e => e.FromFactory(() =>
                                                              {
                                                                  var system = MockRepository.GenerateMock<ISystem>();
                                                                  system.Expect(m => m.Nummer)
                                                                      .Return(fixture.Create<int>())
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
                                                                      .Return(fixture.Create<int>())
                                                                      .Repeat.Any();
                                                                  bruger.Expect(m => m.Initialer)
                                                                      .Return(fixture.Create<string>())
                                                                      .Repeat.Any();
                                                                  bruger.Expect(m => m.Navn)
                                                                      .Return(fixture.Create<string>())
                                                                      .Repeat.Any();
                                                                  return bruger;
                                                              }));
            var brugere = fixture.CreateMany<IBruger>(7).ToList();
            fixture.Customize<IAftale>(e => e.FromFactory(() =>
                                                              {
                                                                  var dt = fixture.Create<DateTime>().AddDays(r.Next(100));
                                                                  var a = MockRepository.GenerateMock<IAftale>();
                                                                  a.Expect(m => m.System)
                                                                      .Return(systemer.ElementAt(1))
                                                                      .Repeat.Any();
                                                                  a.Expect(m => m.Id)
                                                                      .Return(fixture.Create<int>())
                                                                      .Repeat.Any();
                                                                  a.Expect(m => m.FraTidspunkt)
                                                                      .Return(dt)
                                                                      .Repeat.Any();
                                                                  a.Expect(m => m.TilTidspunkt)
                                                                      .Return(dt.AddMinutes(15 + (15*r.Next(7))))
                                                                      .Repeat.Any();
                                                                  a.Expect(m => m.Emne)
                                                                      .Return(fixture.Create<string>())
                                                                      .Repeat.Any();
                                                                  a.Expect(m => m.Deltagere)
                                                                      .Return(null)
                                                                      .WhenCalled(m =>
                                                                                      {
                                                                                          var deltagere = new List<IBrugeraftale>();
                                                                                          for (var i = 0; i < 3; i++)
                                                                                          {
                                                                                              var deltager = MockRepository.GenerateMock<IBrugeraftale>();
                                                                                              deltager.Expect(n => n.System)
                                                                                                  .Return(systemer.ElementAt(1))
                                                                                                  .Repeat.Any();
                                                                                              deltager.Expect(n => n.Bruger)
                                                                                                  .Return(brugere.ElementAt(i))
                                                                                                  .Repeat.Any();
                                                                                              deltagere.Add(deltager);
                                                                                          }
                                                                                          m.ReturnValue = deltagere;
                                                                                      })
                                                                      .Repeat.Any();
                                                                  return a;
                                                              }));
            var aftale = fixture.Create<IAftale>();
            var kalenderRepository = MockRepository.GenerateMock<IKalenderRepository>();
            kalenderRepository.Expect(m => m.BrugerGetAllBySystem(Arg<int>.Is.Equal(systemer.ElementAt(1).Nummer)))
                .Return(brugere);
            kalenderRepository.Expect(
                m =>
                m.AftaleGetBySystemAndId(Arg<int>.Is.Equal(systemer.ElementAt(1).Nummer), Arg<int>.Is.Equal(aftale.Id)))
                .Return(aftale);
            fixture.Inject(kalenderRepository);

            fixture.Inject(fixture.CreateMany<KalenderbrugerView>(3));
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            objectMapper.Expect(m => m.Map<IBrugeraftale, KalenderbrugerAftaleView>(Arg<IBrugeraftale>.Is.NotNull))
                .Return(fixture.Create<KalenderbrugerAftaleView>());
            fixture.Inject(objectMapper);

            var queryHandler = fixture.Create<KalenderbrugerAftaleGetQueryHandler>();
            Assert.That(queryHandler, Is.Not.Null);

            var query = new KalenderbrugerAftaleGetQuery
                            {
                                System = systemer.ElementAt(1).Nummer,
                                AftaleId = aftale.Id,
                                Initialer = brugere.ElementAt(1).Initialer
                            };
            var result = queryHandler.Query(query);
            Assert.That(result, Is.Not.Null);

            fællesRepository.AssertWasCalled(m => m.SystemGetAll());
            kalenderRepository.AssertWasCalled(
                m => m.BrugerGetAllBySystem(Arg<int>.Is.Equal(systemer.ElementAt(1).Nummer)));
            kalenderRepository.AssertWasCalled(
                m =>
                m.AftaleGetBySystemAndId(Arg<int>.Is.Equal(systemer.ElementAt(1).Nummer), Arg<int>.Is.Equal(aftale.Id)));
            objectMapper.AssertWasCalled(
                m => m.Map<IBrugeraftale, KalenderbrugerAftaleView>(Arg<IBrugeraftale>.Is.NotNull));
        }

        /// <summary>
        /// Tester, at Query kaster en IntranetRepositoryException, hvis kalenderbrugeren med det givne sæt initialer ikke findes på kalenderaftalen.
        /// </summary>
        [Test]
        public void TestAtQueryKasterIntranetRepositoryExceptionHvisBrugerIkkeFindesPåAftale()
        {
            var fixture = new Fixture();
            fixture.Inject(DateTime.Now);
            fixture.Inject(MockRepository.GenerateMock<IObjectMapper>());

            var r = new Random(fixture.Create<DateTime>().Millisecond);

            fixture.Customize<ISystem>(e => e.FromFactory(() =>
                                                              {
                                                                  var system = MockRepository.GenerateMock<ISystem>();
                                                                  system.Expect(m => m.Nummer)
                                                                      .Return(fixture.Create<int>())
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
                                                                      .Return(fixture.Create<int>())
                                                                      .Repeat.Any();
                                                                  bruger.Expect(m => m.Initialer)
                                                                      .Return(fixture.Create<string>())
                                                                      .Repeat.Any();
                                                                  bruger.Expect(m => m.Navn)
                                                                      .Return(fixture.Create<string>())
                                                                      .Repeat.Any();
                                                                  return bruger;
                                                              }));
            var brugere = fixture.CreateMany<IBruger>(7).ToList();
            fixture.Customize<IAftale>(e => e.FromFactory(() =>
                                                              {
                                                                  var dt = fixture.Create<DateTime>().AddDays(r.Next(100));
                                                                  var a = MockRepository.GenerateMock<IAftale>();
                                                                  a.Expect(m => m.System)
                                                                      .Return(systemer.ElementAt(1))
                                                                      .Repeat.Any();
                                                                  a.Expect(m => m.Id)
                                                                      .Return(fixture.Create<int>())
                                                                      .Repeat.Any();
                                                                  a.Expect(m => m.FraTidspunkt)
                                                                      .Return(dt)
                                                                      .Repeat.Any();
                                                                  a.Expect(m => m.TilTidspunkt)
                                                                      .Return(dt.AddMinutes(15 + (15*r.Next(7))))
                                                                      .Repeat.Any();
                                                                  a.Expect(m => m.Emne)
                                                                      .Return(fixture.Create<string>())
                                                                      .Repeat.Any();
                                                                  a.Expect(m => m.Deltagere)
                                                                      .Return(null)
                                                                      .WhenCalled(m =>
                                                                                      {
                                                                                          var deltagere = new List<IBrugeraftale>();
                                                                                          for (var i = 0; i < 3; i++)
                                                                                          {
                                                                                              var deltager = MockRepository.GenerateMock<IBrugeraftale>();
                                                                                              deltager.Expect(n => n.System)
                                                                                                  .Return(systemer.ElementAt(1))
                                                                                                  .Repeat.Any();
                                                                                              deltager.Expect(n => n.Bruger)
                                                                                                  .Return(brugere.ElementAt(i))
                                                                                                  .Repeat.Any();
                                                                                              deltagere.Add(deltager);
                                                                                          }
                                                                                          m.ReturnValue = deltagere;
                                                                                      })
                                                                      .Repeat.Any();
                                                                  return a;
                                                              }));
            var aftale = fixture.Create<IAftale>();
            var kalenderRepository = MockRepository.GenerateMock<IKalenderRepository>();
            kalenderRepository.Expect(m => m.BrugerGetAllBySystem(Arg<int>.Is.Equal(systemer.ElementAt(1).Nummer)))
                .Return(brugere);
            kalenderRepository.Expect(
                m =>
                m.AftaleGetBySystemAndId(Arg<int>.Is.Equal(systemer.ElementAt(1).Nummer), Arg<int>.Is.Equal(aftale.Id)))
                .Return(aftale);
            fixture.Inject(kalenderRepository);

            fixture.Inject(fixture.CreateMany<KalenderbrugerView>(3));
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            objectMapper.Expect(m => m.Map<IBrugeraftale, KalenderbrugerAftaleView>(Arg<IBrugeraftale>.Is.NotNull))
                .Return(fixture.Create<KalenderbrugerAftaleView>());
            fixture.Inject(objectMapper);

            var queryHandler = fixture.Create<KalenderbrugerAftaleGetQueryHandler>();
            Assert.That(queryHandler, Is.Not.Null);

            var query = new KalenderbrugerAftaleGetQuery
                            {
                                System = systemer.ElementAt(1).Nummer,
                                AftaleId = aftale.Id,
                                Initialer = brugere.ElementAt(3).Initialer
                            };
            Assert.Throws<IntranetRepositoryException>(() => queryHandler.Query(query));

            fællesRepository.AssertWasCalled(m => m.SystemGetAll());
            kalenderRepository.AssertWasCalled(
                m => m.BrugerGetAllBySystem(Arg<int>.Is.Equal(systemer.ElementAt(1).Nummer)));
            kalenderRepository.AssertWasCalled(
                m =>
                m.AftaleGetBySystemAndId(Arg<int>.Is.Equal(systemer.ElementAt(1).Nummer), Arg<int>.Is.Equal(aftale.Id)));
        }
    }
}
