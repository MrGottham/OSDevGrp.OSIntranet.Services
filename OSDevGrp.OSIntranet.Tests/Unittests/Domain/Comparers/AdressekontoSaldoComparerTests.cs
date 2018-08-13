using System;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.Domain.Comparers;
using NUnit.Framework;
using AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Domain.Comparers
{
    /// <summary>
    /// Tester klasse til sammenligning af adressekonti.
    /// </summary>
    [TestFixture]
    public class AdressekontoSaldoComparerTests
    {
        /// <summary>
        /// Tester, at Compare kaster en ArgumentNullException, hvis X er null.
        /// </summary>
        [Test]
        public void TestAtCompareKasterArgumentNullExceptionHvisXErNull()
        {
            var comparer = new AdressekontoSaldoComparer();
            Assert.That(comparer, Is.Not.Null);
            Assert.Throws<ArgumentNullException>(() => comparer.Compare(null, null));
        }

        /// <summary>
        /// Tester, at Compare kaster en ArgumentNullException, hvis Y er null.
        /// </summary>
        [Test]
        public void TestAtCompareKasterArgumentNullExceptionHvisYErNull()
        {
            var fixture = new Fixture();
            var x = fixture.Create<Person>();

            var comparer = new AdressekontoSaldoComparer();
            Assert.That(comparer, Is.Not.Null);
            Assert.Throws<ArgumentNullException>(() => comparer.Compare(x, null));
        }

        /// <summary>
        /// Tester, at Compare returner 1, hvis saldo for X er større end saldo for Y.
        /// </summary>
        [Test]
        public void TestAtCompareReturnererEnHvisSaldoForXErStørreEndSaldoForY()
        {
            var fixture = new Fixture();
            fixture.Inject(new DateTime(2011, 6, 30));
            var x = fixture.Create<Person>();
            x.TilføjBogføringslinje(new Bogføringslinje(fixture.Create<int>(),
                                                        fixture.Create<DateTime>(),
                                                        fixture.Create<string>(),
                                                        fixture.Create<string>(), 5000M, 0M));
            x.Calculate(fixture.Create<DateTime>());
            var y = fixture.Create<Person>();
            y.TilføjBogføringslinje(new Bogføringslinje(fixture.Create<int>(),
                                                        fixture.Create<DateTime>(),
                                                        fixture.Create<string>(),
                                                        fixture.Create<string>(), 2500M, 0M));
            y.Calculate(fixture.Create<DateTime>());

            var comparer = new AdressekontoSaldoComparer();
            Assert.That(comparer, Is.Not.Null);
            Assert.That(comparer.Compare(x, y), Is.EqualTo(1));
        }

        /// <summary>
        /// Tester, at Compare returner -1, hvis saldo for Y er større end saldo for X.
        /// </summary>
        [Test]
        public void TestAtCompareReturnererMinusEnHvisSaldoForYErStørreEndSaldoForX()
        {
            var fixture = new Fixture();
            fixture.Inject(new DateTime(2011, 6, 30));
            var x = fixture.Create<Person>();
            x.TilføjBogføringslinje(new Bogføringslinje(fixture.Create<int>(),
                                                        fixture.Create<DateTime>(),
                                                        fixture.Create<string>(),
                                                        fixture.Create<string>(), 2500M, 0M));
            x.Calculate(fixture.Create<DateTime>());
            var y = fixture.Create<Person>();
            y.TilføjBogføringslinje(new Bogføringslinje(fixture.Create<int>(),
                                                        fixture.Create<DateTime>(),
                                                        fixture.Create<string>(),
                                                        fixture.Create<string>(), 5000M, 0M));
            y.Calculate(fixture.Create<DateTime>());

            var comparer = new AdressekontoSaldoComparer();
            Assert.That(comparer, Is.Not.Null);
            Assert.That(comparer.Compare(x, y), Is.EqualTo(-1));
        }

        /// <summary>
        /// Tester, at Compare returner værdi fra anden sortering, hvis saldo for X er lig saldo for Y.
        /// </summary>
        [Test]
        public void TestAtCompareReturnererVærdiFraAndenSorteringHvisSaldoForXErLigSaldoForY()
        {
            var fixture = new Fixture();
            fixture.Inject(new DateTime(2011, 6, 30));
            var x = fixture.Create<Person>();
            x.TilføjBogføringslinje(new Bogføringslinje(fixture.Create<int>(),
                                                        fixture.Create<DateTime>(),
                                                        fixture.Create<string>(),
                                                        fixture.Create<string>(), 5000M, 0M));
            x.Calculate(fixture.Create<DateTime>());
            var y = fixture.Create<Person>();
            y.TilføjBogføringslinje(new Bogføringslinje(fixture.Create<int>(),
                                                        fixture.Create<DateTime>(),
                                                        fixture.Create<string>(),
                                                        fixture.Create<string>(), 5000M, 0M));
            y.Calculate(fixture.Create<DateTime>());

            var comparer = new AdressekontoSaldoComparer();
            Assert.That(comparer, Is.Not.Null);
            Assert.That(comparer.Compare(x, y), Is.Not.EqualTo(0));
        }
    }
}
