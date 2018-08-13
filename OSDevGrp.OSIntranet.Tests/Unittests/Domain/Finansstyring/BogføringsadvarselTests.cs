using System;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.Domain.Finansstyring;
using NUnit.Framework;
using AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Domain.Finansstyring
{
    /// <summary>
    /// Tester en bogføringsadvarsel.
    /// </summary>
    [TestFixture]
    public class BogføringsadvarselTests
    {
        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis advarsel er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisAdvarselErNull()
        {
            var fixture = new Fixture();
            Assert.Throws<ArgumentNullException>(
                () => new Bogføringsadvarsel(null, fixture.Create<Konto>(), 0M));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis advarsel er tom.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisAdvarselErEmpty()
        {
            var fixture = new Fixture();
            Assert.Throws<ArgumentNullException>(
                () => new Bogføringsadvarsel(string.Empty, fixture.Create<Konto>(), 0M));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis kontoen er tom.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisKontoErNull()
        {
            var fixture = new Fixture();
            Assert.Throws<ArgumentNullException>(
                () => new Bogføringsadvarsel(fixture.Create<string>(), null, 0M));
        }

        /// <summary>
        /// Tester, at konstruktøren initierer en bogføringsadvarsel.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererBogføringsadvarsel()
        {
            var fixture = new Fixture();
            var advarsel = fixture.Create<string>();
            var konto = fixture.Create<Konto>();
            var beløb = fixture.Create<decimal>();
            var bogføringsadvarsel = new Bogføringsadvarsel(advarsel, konto, beløb);
            Assert.That(bogføringsadvarsel, Is.Not.Null);
            Assert.That(bogføringsadvarsel.Advarsel, Is.Not.Null);
            Assert.That(bogføringsadvarsel.Advarsel, Is.EqualTo(advarsel));
            Assert.That(bogføringsadvarsel.Konto, Is.Not.Null);
            Assert.That(bogføringsadvarsel.Konto, Is.EqualTo(konto));
            Assert.That(bogføringsadvarsel.Beløb, Is.EqualTo(beløb));
        }
    }
}
