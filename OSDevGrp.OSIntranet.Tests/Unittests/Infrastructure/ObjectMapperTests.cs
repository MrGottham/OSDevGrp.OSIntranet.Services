using System;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Domain.Interfaces.Fælles;
using OSDevGrp.OSIntranet.Domain.Interfaces.Kalender;
using OSDevGrp.OSIntranet.Domain.Kalender;
using NUnit.Framework;
using AutoFixture;
using Rhino.Mocks;
using ObjectMapper = OSDevGrp.OSIntranet.Infrastructure.ObjectMapper;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Infrastructure
{
    /// <summary>
    /// Tester ObjectMapper.
    /// </summary>
    [TestFixture]
    public class ObjectMapperTests
    {
        /// <summary>
        /// Tester, at ObjectMapper kan initieres.
        /// </summary>
        [Test]
        public void TestAtObjectMapperKanInitieres()
        {
            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);
        }

        /// <summary>
        /// Tester, at en brugeraftale kan mappes til et kalenderbrugeraftaleview.
        /// </summary>
        [Test]
        public void TestAtBrugeraftaleKanMappesTilKalenderbrugerAftaleView()
        {
            var fixture = new Fixture();
            fixture.Inject(DateTime.Now);
            fixture.Inject(MockRepository.GenerateMock<ISystem>());
            fixture.Inject(MockRepository.GenerateMock<IBruger>());
            fixture.Inject(MockRepository.GenerateMock<IBrugeraftale>());

            var aftale = MockRepository.GenerateMock<IAftale>();
            aftale.Expect(m => m.System)
                .Return(fixture.Create<ISystem>())
                .Repeat.Any();
            aftale.Expect(m => m.Id)
                .Return(fixture.Create<int>())
                .Repeat.Any();
            aftale.Expect(m => m.FraTidspunkt)
                .Return(fixture.Create<DateTime>())
                .Repeat.Any();
            aftale.Expect(m => m.TilTidspunkt)
                .Return(fixture.Create<DateTime>().AddMinutes(15))
                .Repeat.Any();
            aftale.Expect(m => m.Emne)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            aftale.Expect(m => m.Notat)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            aftale.Expect(m => m.Deltagere)
                .Return(fixture.CreateMany<IBrugeraftale>(3))
                .Repeat.Any();
            fixture.Inject(aftale);

            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var brugeraftale = fixture.Create<Brugeraftale>();
            var kalenderbrugerAftaleView = objectMapper.Map<IBrugeraftale, KalenderbrugerAftaleView>(brugeraftale);
            Assert.That(kalenderbrugerAftaleView, Is.Not.Null);
            Assert.That(kalenderbrugerAftaleView.System, Is.Not.Null);
            Assert.That(kalenderbrugerAftaleView.Id, Is.EqualTo(brugeraftale.Aftale.Id));
            Assert.That(kalenderbrugerAftaleView.FraTidspunkt, Is.EqualTo(brugeraftale.Aftale.FraTidspunkt));
            Assert.That(kalenderbrugerAftaleView.TilTidspunkt, Is.EqualTo(brugeraftale.Aftale.TilTidspunkt));
            Assert.That(kalenderbrugerAftaleView.Emne, Is.Not.Null);
            Assert.That(kalenderbrugerAftaleView.Emne, Is.EqualTo(brugeraftale.Aftale.Emne));
            Assert.That(kalenderbrugerAftaleView.Notat, Is.Not.Null);
            Assert.That(kalenderbrugerAftaleView.Notat, Is.EqualTo(brugeraftale.Aftale.Notat));
            Assert.That(kalenderbrugerAftaleView.Offentlig, Is.EqualTo(brugeraftale.Offentligtgørelse));
            Assert.That(kalenderbrugerAftaleView.Privat, Is.EqualTo(brugeraftale.Privat));
            Assert.That(kalenderbrugerAftaleView.Alarm, Is.EqualTo(brugeraftale.Alarm));
            Assert.That(kalenderbrugerAftaleView.Udført, Is.EqualTo(brugeraftale.Udført));
            Assert.That(kalenderbrugerAftaleView.Eksporteres, Is.EqualTo(brugeraftale.Eksporter));
            Assert.That(kalenderbrugerAftaleView.Eksporteret, Is.EqualTo(brugeraftale.Eksporteret));
            Assert.That(kalenderbrugerAftaleView.Deltagere, Is.Not.Null);
        }

        /// <summary>
        /// Tester, at en brugeraftale kan mappes til et kalenderbrugerview.
        /// </summary>
        [Test]
        public void TestAtBrugeraftaleKanMappesTilKalenderbrugerView()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<ISystem>());
            fixture.Inject(MockRepository.GenerateMock<IAftale>());

            var bruger = MockRepository.GenerateMock<IBruger>();
            bruger.Expect(m => m.System)
                .Return(fixture.Create<ISystem>())
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
            fixture.Inject(bruger);

            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var brugeraftale = fixture.Create<Brugeraftale>();
            var kalenderbrugerView = objectMapper.Map<IBrugeraftale, KalenderbrugerView>(brugeraftale);
            Assert.That(kalenderbrugerView, Is.Not.Null);
            Assert.That(kalenderbrugerView.System, Is.Not.Null);
            Assert.That(kalenderbrugerView.Id, Is.EqualTo(brugeraftale.Bruger.Id));
            Assert.That(kalenderbrugerView.Initialer, Is.Not.Null);
            Assert.That(kalenderbrugerView.Initialer, Is.EqualTo(brugeraftale.Bruger.Initialer));
            Assert.That(kalenderbrugerView.Navn, Is.Not.Null);
            Assert.That(kalenderbrugerView.Navn, Is.EqualTo(brugeraftale.Bruger.Navn));
        }

        /// <summary>
        /// Tester, at en kalenderbruger kan mappes til et kalenderbrugerview.
        /// </summary>
        [Test]
        public void TestAtBrugerKanMappesTilKalenderbrugerView()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<ISystem>());

            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var bruger = fixture.Create<Bruger>();
            var kalenderbrugerView = objectMapper.Map<IBruger, KalenderbrugerView>(bruger);
            Assert.That(kalenderbrugerView, Is.Not.Null);
            Assert.That(kalenderbrugerView.System, Is.Not.Null);
            Assert.That(kalenderbrugerView.Id, Is.EqualTo( bruger.Id));
            Assert.That(kalenderbrugerView.Initialer, Is.Not.Null);
            Assert.That(kalenderbrugerView.Initialer, Is.EqualTo(bruger.Initialer));
            Assert.That(kalenderbrugerView.Navn, Is.Not.Null);
            Assert.That(kalenderbrugerView.Navn, Is.EqualTo(bruger.Navn));
        }

        /// <summary>
        /// Tester, at et system kan mappes til et systemview.
        /// </summary>
        [Test]
        public void TestAtSystemKanMappesTilSystemView()
        {
            var fixture = new Fixture();

            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var system = fixture.Create<OSIntranet.Domain.Fælles.System>();
            var systemView = objectMapper.Map<ISystem, SystemView>(system);
            Assert.That(systemView, Is.Not.Null);
            Assert.That(systemView.Nummer, Is.EqualTo(system.Nummer));
            Assert.That(systemView.Titel, Is.Not.Null);
            Assert.That(systemView.Titel, Is.EqualTo(system.Titel));
            Assert.That(systemView.Kalender, Is.EqualTo(system.Kalender));
        }
    }
}