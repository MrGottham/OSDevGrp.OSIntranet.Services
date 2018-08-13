using System;
using NUnit.Framework;
using AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Domain.Fælles
{
    /// <summary>
    /// Tester domæneobjekt for system under OSWEBDB.
    /// </summary>
    [TestFixture]
    public class SystemTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer et system.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererSystem()
        {
            var fixture = new Fixture();
            var nummer = fixture.Create<int>();
            var titel = fixture.Create<string>();
            var system = new OSIntranet.Domain.Fælles.System(nummer, titel);
            Assert.That(system, Is.Not.Null);
            Assert.That(system.Nummer, Is.EqualTo(nummer));
            Assert.That(system.Titel, Is.Not.Null);
            Assert.That(system.Titel, Is.EqualTo(titel));
            Assert.That(system.Properties, Is.EqualTo(0));
            Assert.That(system.Kalender, Is.False);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis titlen er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisTitelErNull()
        {
            var fixture = new Fixture();
            var nummer = fixture.Create<int>();
            Assert.Throws<ArgumentNullException>(() => new OSIntranet.Domain.Fælles.System(nummer, null));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis titlen er tom.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisTitelErEmpty()
        {
            var fixture = new Fixture();
            var nummer = fixture.Create<int>();
            Assert.Throws<ArgumentNullException>(() => new OSIntranet.Domain.Fælles.System(nummer, string.Empty));
        }

        /// <summary>
        /// Tester, at Titel kan ændres.
        /// </summary>
        [Test]
        public void TestAtTitelKanÆndres()
        {
            var fixture = new Fixture();

            var system = fixture.Create<OSIntranet.Domain.Fælles.System>();
            Assert.That(system, Is.Not.Null);

            var titel = fixture.Create<string>();
            system.Titel = titel;
            Assert.That(system.Titel, Is.Not.Null);
            Assert.That(system.Titel, Is.EqualTo(titel));
        }

        /// <summary>
        /// Tester, at Titel kaster en ArgumentNullException, hvis værdien er null.
        /// </summary>
        [Test]
        public void TestAtTitelKasterArgumentNullExceptionHvisValueErNull()
        {
            var fixture = new Fixture();

            var system = fixture.Create<OSIntranet.Domain.Fælles.System>();
            Assert.That(system, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => system.Titel = null);
        }

        /// <summary>
        /// Tester, at Titel kaster en ArgumentNullException, hvis værdien er tom.
        /// </summary>
        [Test]
        public void TestAtTitelKasterArgumentNullExceptionHvisValueErEmpty()
        {
            var fixture = new Fixture();

            var system = fixture.Create<OSIntranet.Domain.Fælles.System>();
            Assert.That(system, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => system.Titel = string.Empty);
        }

        /// <summary>
        /// Tester, at Kalender ændres.
        /// </summary>
        [Test]
        public void TestAtKalenderÆndres()
        {
            var fixture = new Fixture();

            var system = fixture.Create<OSIntranet.Domain.Fælles.System>();
            Assert.That(system, Is.Not.Null);

            system.Kalender = true;
            Assert.That(system.Kalender, Is.True);

            system.Kalender = false;
            Assert.That(system.Kalender, Is.False);
        }
    }
}
