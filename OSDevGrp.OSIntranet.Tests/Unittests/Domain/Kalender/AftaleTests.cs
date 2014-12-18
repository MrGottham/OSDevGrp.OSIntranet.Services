using System;
using System.Linq;
using OSDevGrp.OSIntranet.Domain.Interfaces.Fælles;
using OSDevGrp.OSIntranet.Domain.Interfaces.Kalender;
using OSDevGrp.OSIntranet.Domain.Kalender;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Domain.Kalender
{
    /// <summary>
    /// Tester domæneobjekt for en kalenderaftale.
    /// </summary>
    [TestFixture]
    public class AftaleTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en kalenderaftale.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererAftale()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<ISystem>());
            var lastDateTime = DateTime.Now;
            fixture.Customize<DateTime>(e => e.FromFactory(() =>
                                                               {
                                                                   var result = lastDateTime;
                                                                   lastDateTime = lastDateTime.AddMinutes(1);
                                                                   return result;
                                                               }));

            var system = fixture.Create<ISystem>();
            var id = fixture.Create<int>();
            var fraTidspunkt = fixture.Create<DateTime>();
            var tilTidspunkt = fixture.Create<DateTime>();
            var emne = fixture.Create<string>();
            var aftale = new Aftale(system, id, fraTidspunkt, tilTidspunkt, emne);
            Assert.That(aftale, Is.Not.Null);
            Assert.That(aftale.System, Is.Not.Null);
            Assert.That(aftale.System, Is.EqualTo(system));
            Assert.That(aftale.Id, Is.EqualTo(id));
            Assert.That(aftale.FraTidspunkt, Is.EqualTo(fraTidspunkt));
            Assert.That(aftale.TilTidspunkt, Is.EqualTo(tilTidspunkt));
            Assert.That(aftale.Emne, Is.Not.Null);
            Assert.That(aftale.Emne, Is.EqualTo(emne));
            Assert.That(aftale.Notat, Is.Null);
            Assert.That(aftale.Deltagere, Is.Not.Null);
            Assert.That(aftale.Deltagere.Count(), Is.EqualTo(0));
            Assert.That(aftale.Properties, Is.EqualTo(0));
            Assert.That(aftale.Offentligtgørelse, Is.False);
            Assert.That(aftale.Privat, Is.False);
            Assert.That(aftale.Alarm, Is.False);
            Assert.That(aftale.Udført, Is.False);
            Assert.That(aftale.Eksporter, Is.False);
            Assert.That(aftale.Eksporteret, Is.False);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis systemet er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisSystemErNull()
        {
            var fixture = new Fixture();
            fixture.Inject<ISystem>(null);
            var lastDateTime = DateTime.Now;
            fixture.Customize<DateTime>(e => e.FromFactory(() =>
                                                               {
                                                                   var result = lastDateTime;
                                                                   lastDateTime = lastDateTime.AddMinutes(1);
                                                                   return result;
                                                               }));

            Assert.Throws<ArgumentNullException>(
                () =>
                new Aftale(fixture.Create<ISystem>(), fixture.Create<int>(),
                           fixture.Create<DateTime>(), fixture.Create<DateTime>(),
                           fixture.Create<string>()));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis emnet er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisEmneErNull()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<ISystem>());
            var lastDateTime = DateTime.Now;
            fixture.Customize<DateTime>(e => e.FromFactory(() =>
                                                               {
                                                                   var result = lastDateTime;
                                                                   lastDateTime = lastDateTime.AddMinutes(1);
                                                                   return result;
                                                               }));

            Assert.Throws<ArgumentNullException>(
                () =>
                new Aftale(fixture.Create<ISystem>(), fixture.Create<int>(),
                           fixture.Create<DateTime>(), fixture.Create<DateTime>(), null));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis emnet er tomt.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisEmneErEmpty()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<ISystem>());
            var lastDateTime = DateTime.Now;
            fixture.Customize<DateTime>(e => e.FromFactory(() =>
                                                               {
                                                                   var result = lastDateTime;
                                                                   lastDateTime = lastDateTime.AddMinutes(1);
                                                                   return result;
                                                               }));

            Assert.Throws<ArgumentNullException>(
                () =>
                new Aftale(fixture.Create<ISystem>(), fixture.Create<int>(),
                           fixture.Create<DateTime>(), fixture.Create<DateTime>(), string.Empty));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en IntranetSystemException, hvis fra dato og tidspunkt er større end til dato og tidspunkt.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterIntranetSystemExceptionHvisFraTidspunktErStørreEndTilTidspunkt()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<ISystem>());
            var lastDateTime = DateTime.Now;

            Assert.Throws<IntranetSystemException>(
                () =>
                new Aftale(fixture.Create<ISystem>(), fixture.Create<int>(),
                           lastDateTime.AddMinutes(10), lastDateTime, fixture.Create<string>()));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en IntranetSystemException, hvis til dato og tidspunkt er mindre end fra dato og tidspunkt.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterIntranetSystemExceptionHvisTilTidspunktErMindreEndFraTidspunkt()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<ISystem>());
            var lastDateTime = DateTime.Now;

            Assert.Throws<IntranetSystemException>(
                () =>
                new Aftale(fixture.Create<ISystem>(), fixture.Create<int>(), lastDateTime,
                           lastDateTime.AddMinutes(-10), fixture.Create<string>()));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en IntranetSystemException, hvis fra dato og tidspunkt er lig med til dato og tidspunkt.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterIntranetSystemExceptionHvisFraTidspunktErLigTilTidspunkt()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<ISystem>());
            var lastDateTime = DateTime.Now;

            Assert.Throws<IntranetSystemException>(
                () =>
                new Aftale(fixture.Create<ISystem>(), fixture.Create<int>(), lastDateTime,
                           lastDateTime, fixture.Create<string>()));
        }

        /// <summary>
        /// Tester, at FraTidspunkt kan ændres.
        /// </summary>
        [Test]
        public void TestAtFraTidspunktÆndres()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<ISystem>());
            fixture.Inject(DateTime.Now);
            fixture.Inject(new Aftale(fixture.Create<ISystem>(), fixture.Create<int>(),
                                      fixture.Create<DateTime>(),
                                      fixture.Create<DateTime>().AddMinutes(15),
                                      fixture.Create<string>()));

            var aftale = fixture.Create<Aftale>();
            Assert.That(aftale, Is.Not.Null);

            var fraTidspunkt = aftale.FraTidspunkt.AddMinutes(-15);
            aftale.FraTidspunkt = fraTidspunkt;
            Assert.That(aftale.FraTidspunkt, Is.EqualTo(fraTidspunkt));
        }

        /// <summary>
        /// Tester, at FraTidspunkt kaster en IntranetSystemException, hvis værdien er større end til dato og tidspunkt.
        /// </summary>
        [Test]
        public void TestAtFraTidspunktKasterIntranetSystemExceptionHvisValueErStørreEndTilTidspunkt()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<ISystem>());
            fixture.Inject(DateTime.Now);
            fixture.Inject(new Aftale(fixture.Create<ISystem>(), fixture.Create<int>(),
                                      fixture.Create<DateTime>(),
                                      fixture.Create<DateTime>().AddMinutes(15),
                                      fixture.Create<string>()));


            var aftale = fixture.Create<Aftale>();
            Assert.That(aftale, Is.Not.Null);

            Assert.Throws<IntranetSystemException>(() => aftale.FraTidspunkt = aftale.TilTidspunkt.AddMinutes(15));
        }

        /// <summary>
        /// Tester, at FraTidspunkt kaster en IntranetSystemException, hvis værdien er lig med til dato og tidspunkt.
        /// </summary>
        [Test]
        public void TestAtFraTidspunktKasterIntranetSystemExceptionHvisValueErLigTilTidspunkt()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<ISystem>());
            fixture.Inject(DateTime.Now);
            fixture.Inject(new Aftale(fixture.Create<ISystem>(), fixture.Create<int>(),
                                      fixture.Create<DateTime>(),
                                      fixture.Create<DateTime>().AddMinutes(15),
                                      fixture.Create<string>()));


            var aftale = fixture.Create<Aftale>();
            Assert.That(aftale, Is.Not.Null);

            Assert.Throws<IntranetSystemException>(() => aftale.FraTidspunkt = aftale.TilTidspunkt);
        }

        /// <summary>
        /// Tester, at TilTidspunkt kan ændres.
        /// </summary>
        [Test]
        public void TestAtTilTidspunktÆndres()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<ISystem>());
            fixture.Inject(DateTime.Now);
            fixture.Inject(new Aftale(fixture.Create<ISystem>(), fixture.Create<int>(),
                                      fixture.Create<DateTime>(),
                                      fixture.Create<DateTime>().AddMinutes(15),
                                      fixture.Create<string>()));


            var aftale = fixture.Create<Aftale>();
            Assert.That(aftale, Is.Not.Null);

            var tilTidspunkt = aftale.TilTidspunkt.AddMinutes(15);
            aftale.TilTidspunkt = tilTidspunkt;
            Assert.That(aftale.TilTidspunkt, Is.EqualTo(tilTidspunkt));
        }

        /// <summary>
        /// Tester, at TilTidspunkt kaster en IntranetSystemException, hvis værdien er mindre end fra dato og tidspunkt.
        /// </summary>
        [Test]
        public void TestAtTilTidspunktKasterIntranetSystemExceptionHvisValueErMindreEndFraTidspunkt()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<ISystem>());
            fixture.Inject(DateTime.Now);
            fixture.Inject(new Aftale(fixture.Create<ISystem>(), fixture.Create<int>(),
                                      fixture.Create<DateTime>(),
                                      fixture.Create<DateTime>().AddMinutes(15),
                                      fixture.Create<string>()));


            var aftale = fixture.Create<Aftale>();
            Assert.That(aftale, Is.Not.Null);

            Assert.Throws<IntranetSystemException>(() => aftale.TilTidspunkt = aftale.FraTidspunkt.AddMinutes(-15));
        }

        /// <summary>
        /// Tester, at TilTidspunkt kaster en IntranetSystemException, hvis værdien er lig med fra dato og tidspunkt.
        /// </summary>
        [Test]
        public void TestAtTilTidspunktKasterIntranetSystemExceptionHvisValueErLigFraTidspunkt()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<ISystem>());
            fixture.Inject(DateTime.Now);
            fixture.Inject(new Aftale(fixture.Create<ISystem>(), fixture.Create<int>(),
                                      fixture.Create<DateTime>(),
                                      fixture.Create<DateTime>().AddMinutes(15),
                                      fixture.Create<string>()));


            var aftale = fixture.Create<Aftale>();
            Assert.That(aftale, Is.Not.Null);

            Assert.Throws<IntranetSystemException>(() => aftale.TilTidspunkt = aftale.FraTidspunkt);
        }

        /// <summary>
        /// Tester, at Emne kan ændres.
        /// </summary>
        [Test]
        public void TestAtEmneÆndres()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<ISystem>());
            fixture.Inject(DateTime.Now);
            fixture.Inject(new Aftale(fixture.Create<ISystem>(), fixture.Create<int>(),
                                      fixture.Create<DateTime>(),
                                      fixture.Create<DateTime>().AddMinutes(15),
                                      fixture.Create<string>()));


            var aftale = fixture.Create<Aftale>();
            Assert.That(aftale, Is.Not.Null);

            var emne = fixture.Create<string>();
            aftale.Emne = emne;
            Assert.That(aftale.Emne, Is.Not.Null);
            Assert.That(aftale.Emne, Is.EqualTo(emne));
        }

        /// <summary>
        /// Tester, at Emne kaster en ArgumentNullException, hvis værdien er null.
        /// </summary>
        [Test]
        public void TestAtEmneKasterArgumentNullExceptionHvisValueErNull()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<ISystem>());
            fixture.Inject(DateTime.Now);
            fixture.Inject(new Aftale(fixture.Create<ISystem>(), fixture.Create<int>(),
                                      fixture.Create<DateTime>(),
                                      fixture.Create<DateTime>().AddMinutes(15),
                                      fixture.Create<string>()));


            var aftale = fixture.Create<Aftale>();
            Assert.That(aftale, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => aftale.Emne = null);
        }

        /// <summary>
        /// Tester, at Emne kaster en ArgumentNullException, hvis værdien er tom.
        /// </summary>
        [Test]
        public void TestAtEmneKasterArgumentNullExceptionHvisValueErEmpty()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<ISystem>());
            fixture.Inject(DateTime.Now);
            fixture.Inject(new Aftale(fixture.Create<ISystem>(), fixture.Create<int>(),
                                      fixture.Create<DateTime>(),
                                      fixture.Create<DateTime>().AddMinutes(15),
                                      fixture.Create<string>()));


            var aftale = fixture.Create<Aftale>();
            Assert.That(aftale, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => aftale.Emne = string.Empty);
        }

        /// <summary>
        /// Tester, at Notat kan ændres.
        /// </summary>
        [Test]
        public void TestAtNotatÆndres()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<ISystem>());
            fixture.Inject(DateTime.Now);
            fixture.Inject(new Aftale(fixture.Create<ISystem>(), fixture.Create<int>(),
                                      fixture.Create<DateTime>(),
                                      fixture.Create<DateTime>().AddMinutes(15),
                                      fixture.Create<string>()));


            var aftale = fixture.Create<Aftale>();
            Assert.That(aftale, Is.Not.Null);

            var notat = fixture.Create<string>();
            aftale.Notat = notat;
            Assert.That(aftale.Notat, Is.Not.Null);
            Assert.That(aftale.Notat, Is.EqualTo(notat));
        }

        /// <summary>
        /// Tester, at TilføjDeltager tilføjer en deltager.
        /// </summary>
        [Test]
        public void TestAtTilføjDeltagerTilføjerEnDeltager()
        {
            var fixture = new Fixture();
            var system = MockRepository.GenerateMock<ISystem>();
            system.Expect(m => m.Nummer)
                .Return(1)
                .Repeat.Any();
            fixture.Inject(system);
            var bruger = MockRepository.GenerateMock<IBruger>();
            bruger.Expect(m => m.System.Nummer)
                .Return(1)
                .Repeat.Any();
            bruger.Expect(m => m.Id)
                .Return(1)
                .Repeat.Any();
            fixture.Inject(bruger);
            fixture.Inject(DateTime.Now);
            fixture.Inject(new Aftale(fixture.Create<ISystem>(), 1, fixture.Create<DateTime>(),
                                      fixture.Create<DateTime>().AddMinutes(15),
                                      fixture.Create<string>()));


            var aftale = fixture.Create<Aftale>();
            Assert.That(aftale, Is.Not.Null);

            var deltager = new Brugeraftale(fixture.Create<ISystem>(), fixture.Create<Aftale>(),
                                            fixture.Create<IBruger>());
            aftale.TilføjDeltager(deltager);
            Assert.That(aftale.Deltagere, Is.Not.Null);
            Assert.That(aftale.Deltagere.Count(), Is.EqualTo(1));
        }

        /// <summary>
        /// Tester, at TilføjDeltager kaster en ArgumentNullException, hvis brugeraftalen er null.
        /// </summary>
        [Test]
        public void TestAtTilføjDeltagerKasterArgumentNullExceptionHvisBrugeraftaleErNull()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<ISystem>());
            fixture.Inject(DateTime.Now);
            fixture.Inject(new Aftale(fixture.Create<ISystem>(), fixture.Create<int>(),
                                      fixture.Create<DateTime>(),
                                      fixture.Create<DateTime>().AddMinutes(15),
                                      fixture.Create<string>()));


            var aftale = fixture.Create<Aftale>();
            Assert.That(aftale, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => aftale.TilføjDeltager(null));
        }

        /// <summary>
        /// Tester, at TilføjDeltager kaster en IntranetSystemException, hvis brugeraftalens system ikke matcher aftalens system.
        /// </summary>
        [Test]
        public void TestAtTilføjDeltagerKasterIntranetSystemExceptionHvisSystemPåBrugeraftaleIkkeMatcherSystemPåAftale()
        {
            var fixture = new Fixture();
            var system = MockRepository.GenerateMock<ISystem>();
            system.Expect(m => m.Nummer)
                .Return(1)
                .Repeat.Any();
            fixture.Inject(system);
            fixture.Inject(DateTime.Now);
            fixture.Inject(new Aftale(fixture.Create<ISystem>(), fixture.Create<int>(),
                                      fixture.Create<DateTime>(),
                                      fixture.Create<DateTime>().AddMinutes(15),
                                      fixture.Create<string>()));


            var aftale = fixture.Create<Aftale>();
            Assert.That(aftale, Is.Not.Null);

            var invalidSystem = MockRepository.GenerateMock<ISystem>();
            invalidSystem.Expect(m => m.Nummer)
                .Return(2);
            var deltager = new Brugeraftale(invalidSystem, fixture.Create<Aftale>(),
                                            fixture.Create<Bruger>());
            Assert.Throws<IntranetSystemException>(() => aftale.TilføjDeltager(deltager));
        }

        /// <summary>
        /// Tester, at TilføjDeltager kaster en IntranetSystemException, hvis brugeraftalens aftalesystem ikke matcher aftalens system.
        /// </summary>
        [Test]
        public void TestAtTilføjDeltagerKasterIntranetSystemExceptionHvisAftaleSystemPåBrugeraftaleIkkeMatcherSystemPåAftale()
        {
            var fixture = new Fixture();
            var system = MockRepository.GenerateMock<ISystem>();
            system.Expect(m => m.Nummer)
                .Return(1)
                .Repeat.Any();
            fixture.Inject(system);
            fixture.Inject(DateTime.Now);
            fixture.Inject(new Aftale(fixture.Create<ISystem>(), fixture.Create<int>(),
                                      fixture.Create<DateTime>(),
                                      fixture.Create<DateTime>().AddMinutes(15),
                                      fixture.Create<string>()));


            var aftale = fixture.Create<Aftale>();
            Assert.That(aftale, Is.Not.Null);

            var invalidSystem = MockRepository.GenerateMock<ISystem>();
            invalidSystem.Expect(m => m.Nummer)
                .Return(2);
            var deltager = new Brugeraftale(fixture.Create<ISystem>(),
                                            new Aftale(invalidSystem, fixture.Create<int>(),
                                                       fixture.Create<DateTime>(),
                                                       fixture.Create<DateTime>().AddMinutes(15),
                                                       fixture.Create<string>()),
                                            fixture.Create<Bruger>());
            Assert.Throws<IntranetSystemException>(() => aftale.TilføjDeltager(deltager));
        }

        /// <summary>
        /// Tester, at TilføjDeltager kaster en IntranetSystemException, hvis brugeraftalens aftale id ikke matcher aftalens id.
        /// </summary>
        [Test]
        public void TestAtTilføjDeltagerKasterIntranetSystemExceptionHvisAftaleIdPåBrugeraftaleIkkeMatcherIdPåAftale()
        {
            var fixture = new Fixture();
            var system = MockRepository.GenerateMock<ISystem>();
            system.Expect(m => m.Nummer)
                .Return(1)
                .Repeat.Any();
            fixture.Inject(system);
            fixture.Inject(DateTime.Now);
            fixture.Inject(new Aftale(fixture.Create<ISystem>(), 1, fixture.Create<DateTime>(),
                                      fixture.Create<DateTime>().AddMinutes(15),
                                      fixture.Create<string>()));


            var aftale = fixture.Create<Aftale>();
            Assert.That(aftale, Is.Not.Null);

            var deltager = new Brugeraftale(fixture.Create<ISystem>(),
                                            new Aftale(fixture.Create<ISystem>(), 2,
                                                       fixture.Create<DateTime>(),
                                                       fixture.Create<DateTime>().AddMinutes(15),
                                                       fixture.Create<string>()),
                                            fixture.Create<Bruger>());
            Assert.Throws<IntranetSystemException>(() => aftale.TilføjDeltager(deltager));
        }

        /// <summary>
        /// Tester, at TilføjDeltager kaster en IntranetSystemException, hvis brugeraftalens brugersystem ikke matcher aftalens system.
        /// </summary>
        [Test]
        public void TestAtTilføjDeltagerKasterIntranetSystemExceptionHvisBrugerSystemPåBrugeraftaleIkkeMatcherSystemPåAftale()
        {
            var fixture = new Fixture();
            var system = MockRepository.GenerateMock<ISystem>();
            system.Expect(m => m.Nummer)
                .Return(1)
                .Repeat.Any();
            fixture.Inject(system);
            fixture.Inject(DateTime.Now);
            fixture.Inject(new Aftale(fixture.Create<ISystem>(), 1, fixture.Create<DateTime>(),
                                      fixture.Create<DateTime>().AddMinutes(15),
                                      fixture.Create<string>()));


            var aftale = fixture.Create<Aftale>();
            Assert.That(aftale, Is.Not.Null);

            var invalidBruger = MockRepository.GenerateMock<IBruger>();
            invalidBruger.Expect(m => m.System.Nummer)
                .Return(2);
            var deltager = new Brugeraftale(fixture.Create<ISystem>(),
                                            new Aftale(fixture.Create<ISystem>(), 1,
                                                       fixture.Create<DateTime>(),
                                                       fixture.Create<DateTime>().AddMinutes(15),
                                                       fixture.Create<string>()), invalidBruger);
            Assert.Throws<IntranetSystemException>(() => aftale.TilføjDeltager(deltager));
        }

        /// <summary>
        /// Tester, at TilføjDeltager kaster en IntranetBusinessException, hvis brugeraftalen allerede eksisterer.
        /// </summary>
        [Test]
        public void TestAtTilføjDeltagerKasterIntranetBusinessExceptionHvisBrugeraftaleAlleredeEksisterer()
        {
            var fixture = new Fixture();
            var system = MockRepository.GenerateMock<ISystem>();
            system.Expect(m => m.Nummer)
                .Return(1)
                .Repeat.Any();
            fixture.Inject(system);
            var bruger = MockRepository.GenerateMock<IBruger>();
            bruger.Expect(m => m.System.Nummer)
                .Return(1)
                .Repeat.Any();
            bruger.Expect(m => m.Id)
                .Return(1)
                .Repeat.Any();
            fixture.Inject(bruger);
            fixture.Inject(DateTime.Now);
            fixture.Inject(new Aftale(fixture.Create<ISystem>(), 1, fixture.Create<DateTime>(),
                                      fixture.Create<DateTime>().AddMinutes(15),
                                      fixture.Create<string>()));


            var aftale = fixture.Create<Aftale>();
            Assert.That(aftale, Is.Not.Null);

            var deltager = new Brugeraftale(fixture.Create<ISystem>(), fixture.Create<Aftale>(),
                                            fixture.Create<IBruger>());
            aftale.TilføjDeltager(deltager);
            Assert.That(aftale.Deltagere, Is.Not.Null);
            Assert.That(aftale.Deltagere.Count(), Is.EqualTo(1));

            Assert.Throws<IntranetBusinessException>(() => aftale.TilføjDeltager(deltager));
        }

        /// <summary>
        /// Tester, at  FjernDeltager tilføjer en deltager.
        /// </summary>
        [Test]
        public void TestAtFjernDeltagerFjernerEnDeltager()
        {
            var fixture = new Fixture();
            var system = MockRepository.GenerateMock<ISystem>();
            system.Expect(m => m.Nummer)
                .Return(1)
                .Repeat.Any();
            fixture.Inject(system);
            var bruger = MockRepository.GenerateMock<IBruger>();
            bruger.Expect(m => m.System.Nummer)
                .Return(1)
                .Repeat.Any();
            bruger.Expect(m => m.Id)
                .Return(1)
                .Repeat.Any();
            fixture.Inject(bruger);
            fixture.Inject(DateTime.Now);
            fixture.Inject(new Aftale(fixture.Create<ISystem>(), 1, fixture.Create<DateTime>(),
                                      fixture.Create<DateTime>().AddMinutes(15),
                                      fixture.Create<string>()));


            var aftale = fixture.Create<Aftale>();
            Assert.That(aftale, Is.Not.Null);

            var deltager = new Brugeraftale(fixture.Create<ISystem>(), fixture.Create<Aftale>(),
                                            fixture.Create<IBruger>());
            aftale.TilføjDeltager(deltager);
            Assert.That(aftale.Deltagere, Is.Not.Null);
            Assert.That(aftale.Deltagere.Count(), Is.EqualTo(1));

            aftale.FjernDeltager(deltager);
            Assert.That(aftale.Deltagere.Count(), Is.EqualTo(0));
        }

        /// <summary>
        /// Tester, at FjernDeltager kaster en ArgumentNullException, hvis brugeraftalen er null.
        /// </summary>
        [Test]
        public void TestAtFjernDeltagerKasterArgumentNullExceptionHvisBrugeraftaleErNull()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<ISystem>());
            fixture.Inject(DateTime.Now);
            fixture.Inject(new Aftale(fixture.Create<ISystem>(), fixture.Create<int>(),
                                      fixture.Create<DateTime>(),
                                      fixture.Create<DateTime>().AddMinutes(15),
                                      fixture.Create<string>()));


            var aftale = fixture.Create<Aftale>();
            Assert.That(aftale, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => aftale.FjernDeltager(null));
        }

        /// <summary>
        /// Tester, at FjernDeltager kaster en IntranetBusinessException, hvis brugeraftalen ikke findes.
        /// </summary>
        [Test]
        public void TestAtFjernDeltagerKasterIntranetBusinessExceptionHvisBrugeraftaleIkkeFindes()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<ISystem>());
            fixture.Inject(DateTime.Now);
            fixture.Inject(new Aftale(fixture.Create<ISystem>(), fixture.Create<int>(),
                                      fixture.Create<DateTime>(),
                                      fixture.Create<DateTime>().AddMinutes(15),
                                      fixture.Create<string>()));
            fixture.Inject(MockRepository.GenerateMock<IBrugeraftale>());

            var aftale = fixture.Create<Aftale>();
            Assert.That(aftale, Is.Not.Null);

            Assert.Throws<IntranetBusinessException>(() => aftale.FjernDeltager(fixture.Create<IBrugeraftale>()));
        }
    }
}
