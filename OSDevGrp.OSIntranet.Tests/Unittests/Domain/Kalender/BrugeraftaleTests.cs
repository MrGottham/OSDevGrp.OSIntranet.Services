using System;
using OSDevGrp.OSIntranet.Domain.Interfaces.Fælles;
using OSDevGrp.OSIntranet.Domain.Interfaces.Kalender;
using OSDevGrp.OSIntranet.Domain.Kalender;
using NUnit.Framework;
using Rhino.Mocks;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Domain.Kalender
{
    /// <summary>
    /// Tester domæneobjektet for en brugers kalenderaftale.
    /// </summary>
    [TestFixture]
    public class BrugeraftaleTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en aftale for en bruger.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererBrugeraftale()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<ISystem>());
            fixture.Inject(MockRepository.GenerateMock<IAftale>());
            fixture.Inject(MockRepository.GenerateMock<IBruger>());

            var system = fixture.Create<ISystem>();
            var aftale = fixture.Create<IAftale>();
            var bruger = fixture.Create<IBruger>();
            var brugeraftale = new Brugeraftale(system, aftale, bruger);
            Assert.That(brugeraftale, Is.Not.Null);
            Assert.That(brugeraftale.System, Is.Not.Null);
            Assert.That(brugeraftale.System, Is.EqualTo(system));
            Assert.That(brugeraftale.Aftale, Is.Not.Null);
            Assert.That(brugeraftale.Aftale, Is.EqualTo(aftale));
            Assert.That(brugeraftale.Bruger, Is.Not.Null);
            Assert.That(brugeraftale.Bruger, Is.EqualTo(bruger));
            Assert.That(brugeraftale.Properties, Is.EqualTo(0));
            Assert.That(brugeraftale.Offentligtgørelse, Is.False);
            Assert.That(brugeraftale.Privat, Is.False);
            Assert.That(brugeraftale.Alarm, Is.False);
            Assert.That(brugeraftale.Udført, Is.False);
            Assert.That(brugeraftale.Eksporter, Is.False);
            Assert.That(brugeraftale.Eksporteret, Is.False);
       }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis systemet er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisSystemErNull()
        {
            var fixture = new Fixture();
            fixture.Inject<ISystem>(null);
            fixture.Inject(MockRepository.GenerateMock<IAftale>());
            fixture.Inject(MockRepository.GenerateMock<IBruger>());

            Assert.Throws<ArgumentNullException>(
                () =>
                new Brugeraftale(fixture.Create<ISystem>(), fixture.Create<IAftale>(),
                                 fixture.Create<IBruger>()));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis aftalen er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisAftaleErNull()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<ISystem>());
            fixture.Inject<IAftale>(null);
            fixture.Inject(MockRepository.GenerateMock<IBruger>());

            Assert.Throws<ArgumentNullException>(
                () =>
                new Brugeraftale(fixture.Create<ISystem>(), fixture.Create<IAftale>(),
                                 fixture.Create<IBruger>()));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis brugeren er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisBrugerErNull()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<ISystem>());
            fixture.Inject(MockRepository.GenerateMock<IAftale>());
            fixture.Inject<IBruger>(null);

            Assert.Throws<ArgumentNullException>(
                () =>
                new Brugeraftale(fixture.Create<ISystem>(), fixture.Create<IAftale>(),
                                 fixture.Create<IBruger>()));
        }
    }
}
