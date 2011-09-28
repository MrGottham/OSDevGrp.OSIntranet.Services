using System;
using NUnit.Framework;
using Ploeh.AutoFixture;

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
            var nummer = fixture.CreateAnonymous<int>();
            var titel = fixture.CreateAnonymous<string>();
            var system = new OSIntranet.Domain.Fælles.System(nummer, titel);
            Assert.That(system, Is.Not.Null);
            Assert.That(system.Nummer, Is.EqualTo(nummer));
            Assert.That(system.Titel, Is.Not.Null);
            Assert.That(system.Titel, Is.EqualTo(titel));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis titlen er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisTitelErNull()
        {
            var fixture = new Fixture();
            var nummer = fixture.CreateAnonymous<int>();
            Assert.Throws<ArgumentNullException>(() => new OSIntranet.Domain.Fælles.System(nummer, null));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis titlen er tom.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisTitelErEmpty()
        {
            var fixture = new Fixture();
            var nummer = fixture.CreateAnonymous<int>();
            Assert.Throws<ArgumentNullException>(() => new OSIntranet.Domain.Fælles.System(nummer, string.Empty));
        }

        /// <summary>
        /// Tester, at Titel kan ændres.
        /// </summary>
        [Test]
        public void TestAtTitelKanÆndres()
        {
            var fixture = new Fixture();

            var system = fixture.CreateAnonymous<OSIntranet.Domain.Fælles.System>();
            Assert.That(system, Is.Not.Null);

            var titel = fixture.CreateAnonymous<string>();
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

            var system = fixture.CreateAnonymous<OSIntranet.Domain.Fælles.System>();
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

            var system = fixture.CreateAnonymous<OSIntranet.Domain.Fælles.System>();
            Assert.That(system, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => system.Titel = string.Empty);
        }
    }
}
