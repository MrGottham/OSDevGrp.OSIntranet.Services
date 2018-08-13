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
using AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers
{
    /// <summary>
    /// Tester QueryHandler til håndtering af forespørgelsen: KalenderbrugerAftalerGetQuery.
    /// </summary>
    [TestFixture]
    public class KalenderbrugerAftalerGetQueryHandlerTests
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

            var queryHandler = fixture.Create<KalenderbrugerAftalerGetQueryHandler>();
            Assert.That(queryHandler, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => queryHandler.Query(null));
        }

        /// <summary>
        /// Tester, at Query henter kalenderaftaler til en kalenderbrugere med et givent sæt initialer.
        /// </summary>
        [Test]
        public void TestAtQueryHenterKalenderbrugerAftaler()
        {
            var fixture = new Fixture();
            fixture.Inject(DateTime.Now);

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
                                                                  var aftale = MockRepository.GenerateMock<IAftale>();
                                                                  aftale.Expect(m => m.System)
                                                                      .Return(systemer.ElementAt(1))
                                                                      .Repeat.Any();
                                                                  aftale.Expect(m => m.Id)
                                                                      .Return(fixture.Create<int>())
                                                                      .Repeat.Any();
                                                                  aftale.Expect(m => m.FraTidspunkt)
                                                                      .Return(dt)
                                                                      .Repeat.Any();
                                                                  aftale.Expect(m => m.TilTidspunkt)
                                                                      .Return(dt.AddMinutes(15 + (15*r.Next(7))))
                                                                      .Repeat.Any();
                                                                  aftale.Expect(m => m.Emne)
                                                                      .Return(fixture.Create<string>())
                                                                      .Repeat.Any();
                                                                  aftale.Expect(m => m.Deltagere)
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
                                                                  return aftale;
                                                              }));
            var aftaler = fixture.CreateMany<IAftale>(250).ToList();
            var kalenderRepository = MockRepository.GenerateMock<IKalenderRepository>();
            kalenderRepository.Expect(m => m.BrugerGetAllBySystem(Arg<int>.Is.Equal(systemer.ElementAt(1).Nummer)))
                .Return(brugere);
            kalenderRepository.Expect(
                m =>
                m.AftaleGetAllBySystem(Arg<int>.Is.Equal(systemer.ElementAt(1).Nummer),
                                       Arg<DateTime>.Is.Equal(fixture.Create<DateTime>())))
                .Return(aftaler);
            fixture.Inject(kalenderRepository);

            fixture.Inject(fixture.CreateMany<KalenderbrugerView>(3));
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            objectMapper.Expect(
                m =>
                m.Map<IEnumerable<IBrugeraftale>, IEnumerable<KalenderbrugerAftaleView>>(
                    Arg<IEnumerable<IBrugeraftale>>.Is.NotNull))
                .Return(fixture.CreateMany<KalenderbrugerAftaleView>(250));
            fixture.Inject(objectMapper);

            var queryHandler = fixture.Create<KalenderbrugerAftalerGetQueryHandler>();
            Assert.That(queryHandler, Is.Not.Null);

            var query = new KalenderbrugerAftalerGetQuery
                            {
                                System = systemer.ElementAt(1).Nummer,
                                FraDato = fixture.Create<DateTime>(),
                                Initialer = brugere.ElementAt(1).Initialer
                            };
            var result = queryHandler.Query(query);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(250));

            fællesRepository.AssertWasCalled(m => m.SystemGetAll());
            kalenderRepository.AssertWasCalled(
                m => m.BrugerGetAllBySystem(Arg<int>.Is.Equal(systemer.ElementAt(1).Nummer)));
            kalenderRepository.AssertWasCalled(
                m =>
                m.AftaleGetAllBySystem(Arg<int>.Is.Equal(systemer.ElementAt(1).Nummer),
                                       Arg<DateTime>.Is.Equal(fixture.Create<DateTime>())));
            objectMapper.AssertWasCalled(
                m =>
                m.Map<IEnumerable<IBrugeraftale>, IEnumerable<KalenderbrugerAftaleView>>(
                    Arg<IEnumerable<IBrugeraftale>>.Is.NotNull));
        }
    }
}
