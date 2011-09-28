using System;
using OSDevGrp.OSIntranet.Domain.Fælles;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Rhino.Mocks;

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
    }
}
