using System;
using OSDevGrp.OSIntranet.Domain.Interfaces.Fælles;
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

            var system = fixture.CreateAnonymous<ISystem>();
            var id = fixture.CreateAnonymous<int>();
            var fraTidspunkt = fixture.CreateAnonymous<DateTime>();
            var tilTidspunkt = fixture.CreateAnonymous<DateTime>();
            var emne = fixture.CreateAnonymous<string>();
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
                new Aftale(fixture.CreateAnonymous<ISystem>(), fixture.CreateAnonymous<int>(),
                           fixture.CreateAnonymous<DateTime>(), fixture.CreateAnonymous<DateTime>(),
                           fixture.CreateAnonymous<string>()));
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
                new Aftale(fixture.CreateAnonymous<ISystem>(), fixture.CreateAnonymous<int>(),
                           fixture.CreateAnonymous<DateTime>(), fixture.CreateAnonymous<DateTime>(), null));
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
                new Aftale(fixture.CreateAnonymous<ISystem>(), fixture.CreateAnonymous<int>(),
                           fixture.CreateAnonymous<DateTime>(), fixture.CreateAnonymous<DateTime>(), string.Empty));
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
                new Aftale(fixture.CreateAnonymous<ISystem>(), fixture.CreateAnonymous<int>(),
                           lastDateTime.AddMinutes(10), lastDateTime, fixture.CreateAnonymous<string>()));
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
                new Aftale(fixture.CreateAnonymous<ISystem>(), fixture.CreateAnonymous<int>(), lastDateTime,
                           lastDateTime.AddMinutes(-10), fixture.CreateAnonymous<string>()));
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
                new Aftale(fixture.CreateAnonymous<ISystem>(), fixture.CreateAnonymous<int>(), lastDateTime,
                           lastDateTime, fixture.CreateAnonymous<string>()));
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
            fixture.Inject(new Aftale(fixture.CreateAnonymous<ISystem>(), fixture.CreateAnonymous<int>(),
                                      fixture.CreateAnonymous<DateTime>(),
                                      fixture.CreateAnonymous<DateTime>().AddMinutes(15),
                                      fixture.CreateAnonymous<string>()));

            var aftale = fixture.CreateAnonymous<Aftale>();
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
            fixture.Inject(new Aftale(fixture.CreateAnonymous<ISystem>(), fixture.CreateAnonymous<int>(),
                                      fixture.CreateAnonymous<DateTime>(),
                                      fixture.CreateAnonymous<DateTime>().AddMinutes(15),
                                      fixture.CreateAnonymous<string>()));


            var aftale = fixture.CreateAnonymous<Aftale>();
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
            fixture.Inject(new Aftale(fixture.CreateAnonymous<ISystem>(), fixture.CreateAnonymous<int>(),
                                      fixture.CreateAnonymous<DateTime>(),
                                      fixture.CreateAnonymous<DateTime>().AddMinutes(15),
                                      fixture.CreateAnonymous<string>()));


            var aftale = fixture.CreateAnonymous<Aftale>();
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
            fixture.Inject(new Aftale(fixture.CreateAnonymous<ISystem>(), fixture.CreateAnonymous<int>(),
                                      fixture.CreateAnonymous<DateTime>(),
                                      fixture.CreateAnonymous<DateTime>().AddMinutes(15),
                                      fixture.CreateAnonymous<string>()));


            var aftale = fixture.CreateAnonymous<Aftale>();
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
            fixture.Inject(new Aftale(fixture.CreateAnonymous<ISystem>(), fixture.CreateAnonymous<int>(),
                                      fixture.CreateAnonymous<DateTime>(),
                                      fixture.CreateAnonymous<DateTime>().AddMinutes(15),
                                      fixture.CreateAnonymous<string>()));


            var aftale = fixture.CreateAnonymous<Aftale>();
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
            fixture.Inject(new Aftale(fixture.CreateAnonymous<ISystem>(), fixture.CreateAnonymous<int>(),
                                      fixture.CreateAnonymous<DateTime>(),
                                      fixture.CreateAnonymous<DateTime>().AddMinutes(15),
                                      fixture.CreateAnonymous<string>()));


            var aftale = fixture.CreateAnonymous<Aftale>();
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
            fixture.Inject(new Aftale(fixture.CreateAnonymous<ISystem>(), fixture.CreateAnonymous<int>(),
                                      fixture.CreateAnonymous<DateTime>(),
                                      fixture.CreateAnonymous<DateTime>().AddMinutes(15),
                                      fixture.CreateAnonymous<string>()));


            var aftale = fixture.CreateAnonymous<Aftale>();
            Assert.That(aftale, Is.Not.Null);

            var emne = fixture.CreateAnonymous<string>();
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
            fixture.Inject(new Aftale(fixture.CreateAnonymous<ISystem>(), fixture.CreateAnonymous<int>(),
                                      fixture.CreateAnonymous<DateTime>(),
                                      fixture.CreateAnonymous<DateTime>().AddMinutes(15),
                                      fixture.CreateAnonymous<string>()));


            var aftale = fixture.CreateAnonymous<Aftale>();
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
            fixture.Inject(new Aftale(fixture.CreateAnonymous<ISystem>(), fixture.CreateAnonymous<int>(),
                                      fixture.CreateAnonymous<DateTime>(),
                                      fixture.CreateAnonymous<DateTime>().AddMinutes(15),
                                      fixture.CreateAnonymous<string>()));


            var aftale = fixture.CreateAnonymous<Aftale>();
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
            fixture.Inject(new Aftale(fixture.CreateAnonymous<ISystem>(), fixture.CreateAnonymous<int>(),
                                      fixture.CreateAnonymous<DateTime>(),
                                      fixture.CreateAnonymous<DateTime>().AddMinutes(15),
                                      fixture.CreateAnonymous<string>()));


            var aftale = fixture.CreateAnonymous<Aftale>();
            Assert.That(aftale, Is.Not.Null);

            var notat = fixture.CreateAnonymous<string>();
            aftale.Notat = notat;
            Assert.That(aftale.Notat, Is.Not.Null);
            Assert.That(aftale.Notat, Is.EqualTo(notat));
        }

        /// <summary>
        /// Tester, at Offentligtgørelse kan ændres.
        /// </summary>
        [Test]
        public void TestAtOffentligtgørelseÆndres()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<ISystem>());
            fixture.Inject(DateTime.Now);
            fixture.Inject(new Aftale(fixture.CreateAnonymous<ISystem>(), fixture.CreateAnonymous<int>(),
                                      fixture.CreateAnonymous<DateTime>(),
                                      fixture.CreateAnonymous<DateTime>().AddMinutes(15),
                                      fixture.CreateAnonymous<string>()));


            var aftale = fixture.CreateAnonymous<Aftale>();
            Assert.That(aftale, Is.Not.Null);

            aftale.Offentligtgørelse = true;
            Assert.That(aftale.Offentligtgørelse, Is.True);

            aftale.Offentligtgørelse = false;
            Assert.That(aftale.Offentligtgørelse, Is.False);
        }

        /// <summary>
        /// Tester, at Privat kan ændres.
        /// </summary>
        [Test]
        public void TestAtPrivatÆndres()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<ISystem>());
            fixture.Inject(DateTime.Now);
            fixture.Inject(new Aftale(fixture.CreateAnonymous<ISystem>(), fixture.CreateAnonymous<int>(),
                                      fixture.CreateAnonymous<DateTime>(),
                                      fixture.CreateAnonymous<DateTime>().AddMinutes(15),
                                      fixture.CreateAnonymous<string>()));


            var aftale = fixture.CreateAnonymous<Aftale>();
            Assert.That(aftale, Is.Not.Null);

            aftale.Privat = true;
            Assert.That(aftale.Privat, Is.True);

            aftale.Privat = false;
            Assert.That(aftale.Privat, Is.False);
        }
    }
}
