using OSDevGrp.OSIntranet.Domain.Kalender;
using NUnit.Framework;
using AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Domain.Kalender
{
    /// <summary>
    /// Tester domæneobjekt for en basiskalenderaftale.
    /// </summary>
    [TestFixture]
    public class AftaleBaseTests
    {
        /// <summary>
        /// Egen klasse til test af en basiskalenderaftale.
        /// </summary>
        private class MyAftale : AftaleBase
        {
            /// <summary>
            /// Danner egen klasse til test af en basiskalenderaftale.
            /// </summary>
            public MyAftale()
                : base(0)
            {
            }
        }

        /// <summary>
        /// Tester, at konstruktøren initierer en basisaftalen.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererAftaleBase()
        {
            var aftale = new MyAftale();
            Assert.That(aftale, Is.Not.Null);
            Assert.That(aftale.Offentligtgørelse, Is.False);
            Assert.That(aftale.Privat, Is.False);
            Assert.That(aftale.Alarm, Is.False);
            Assert.That(aftale.Udført, Is.False);
            Assert.That(aftale.Eksporter, Is.False);
            Assert.That(aftale.Eksporteret, Is.False);
        }

        /// <summary>
        /// Tester, at Offentligtgørelse kan ændres.
        /// </summary>
        [Test]
        public void TestAtOffentligtgørelseÆndres()
        {
            var fixture = new Fixture();

            var aftale = fixture.Create<MyAftale>();
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

            var aftale = fixture.Create<MyAftale>();
            Assert.That(aftale, Is.Not.Null);

            aftale.Privat = true;
            Assert.That(aftale.Privat, Is.True);

            aftale.Privat = false;
            Assert.That(aftale.Privat, Is.False);
        }

        /// <summary>
        /// Tester, at Alarm kan ændres.
        /// </summary>
        [Test]
        public void TestAtAlarmÆndres()
        {
            var fixture = new Fixture();

            var aftale = fixture.Create<MyAftale>();
            Assert.That(aftale, Is.Not.Null);

            aftale.Alarm = true;
            Assert.That(aftale.Alarm, Is.True);

            aftale.Alarm = false;
            Assert.That(aftale.Alarm, Is.False);
        }

        /// <summary>
        /// Tester, at Alarm ændrer Udført, hvis værdien sættes til true.
        /// </summary>
        [Test]
        public void TestAtAlarmÆndrerUdførtHvisValueErTrue()
        {
            var fixture = new Fixture();

            var aftale = fixture.Create<MyAftale>();
            Assert.That(aftale, Is.Not.Null);

            aftale.Udført = true;
            Assert.That(aftale.Udført, Is.True);

            aftale.Alarm = true;
            Assert.That(aftale.Alarm, Is.True);
            Assert.That(aftale.Udført, Is.False);
        }

        /// <summary>
        /// Tester, at Udført kan ændres.
        /// </summary>
        [Test]
        public void TestAtUdførtÆndres()
        {
            var fixture = new Fixture();

            var aftale = fixture.Create<MyAftale>();
            Assert.That(aftale, Is.Not.Null);

            aftale.Udført = true;
            Assert.That(aftale.Udført, Is.True);

            aftale.Udført = false;
            Assert.That(aftale.Udført, Is.False);
        }

        /// <summary>
        /// Tester, at Udført ændrer Alarm, hvis værdien sættes til true.
        /// </summary>
        [Test]
        public void TestAtUdførtÆndrerAlarmHvisValueErTrue()
        {
            var fixture = new Fixture();

            var aftale = fixture.Create<MyAftale>();
            Assert.That(aftale, Is.Not.Null);

            aftale.Alarm = true;
            Assert.That(aftale.Alarm, Is.True);

            aftale.Udført = true;
            Assert.That(aftale.Udført, Is.True);
            Assert.That(aftale.Alarm, Is.False);
        }

        /// <summary>
        /// Tester, at Eksporter kan ændres.
        /// </summary>
        [Test]
        public void TestAtEksporterÆndres()
        {
            var fixture = new Fixture();

            var aftale = fixture.Create<MyAftale>();
            Assert.That(aftale, Is.Not.Null);

            aftale.Eksporter = true;
            Assert.That(aftale.Eksporter, Is.True);

            aftale.Eksporter = false;
            Assert.That(aftale.Eksporter, Is.False);
        }

        /// <summary>
        /// Tester, at Eksporter ændrer Eksporteret, hvis værdien sættes til true.
        /// </summary>
        [Test]
        public void TestAtEksporterÆndrerEksporteretHvisValueErTrue()
        {
            var fixture = new Fixture();

            var aftale = fixture.Create<MyAftale>();
            Assert.That(aftale, Is.Not.Null);

            aftale.Eksporteret = true;
            Assert.That(aftale.Eksporteret, Is.True);

            aftale.Eksporter = true;
            Assert.That(aftale.Eksporter, Is.True);
            Assert.That(aftale.Eksporteret, Is.False);
        }

        /// <summary>
        /// Tester, at Eksporteret kan ændres.
        /// </summary>
        [Test]
        public void TestAtEksporteretÆndres()
        {
            var fixture = new Fixture();

            var aftale = fixture.Create<MyAftale>();
            Assert.That(aftale, Is.Not.Null);

            aftale.Eksporteret = true;
            Assert.That(aftale.Eksporteret, Is.True);

            aftale.Eksporteret = false;
            Assert.That(aftale.Eksporteret, Is.False);
        }

        /// <summary>
        /// Tester, at Eksporteret ændrer Eksporter, hvis værdien sættes til true.
        /// </summary>
        [Test]
        public void TestAtEksporteretÆndrerEksporterHvisValueErTrue()
        {
            var fixture = new Fixture();

            var aftale = fixture.Create<MyAftale>();
            Assert.That(aftale, Is.Not.Null);

            aftale.Eksporter = true;
            Assert.That(aftale.Eksporter, Is.True);

            aftale.Eksporteret = true;
            Assert.That(aftale.Eksporteret, Is.True);
            Assert.That(aftale.Eksporter, Is.False);
        }
    }
}
