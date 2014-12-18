using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.Domain.Finansstyring;
using OSDevGrp.OSIntranet.Domain.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Domain.Finansstyring
{
    /// <summary>
    /// Tester et bogføringsresultat.
    /// </summary>
    [TestFixture]
    public class BogføringsresultatTests
    {
        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis bogføringslinjen er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisBogføringslinjeErNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Bogføringsresultat(null));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en IntranetSystemException, hvis kontoen på bogføringslinjen er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterIntranetSystemExceptionHvisKontoPåBogføringslinjenErNull()
        {
            var fixture = new Fixture();
            var bogføringslinje = fixture.Create<Bogføringslinje>();
            Assert.Throws<IntranetSystemException>(() => new Bogføringsresultat(bogføringslinje));
        }

        /// <summary>
        /// Tester, at konstruktøren initierer et bogføringsresultat.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererBogføringsresultat()
        {
            var fixture = new Fixture();
            
            var bogføringslinje = fixture.Create<Bogføringslinje>();
            Assert.That(bogføringslinje, Is.Not.Null);

            var konto = fixture.Create<Konto>();
            Assert.That(konto, Is.Not.Null);
            konto.TilføjBogføringslinje(bogføringslinje);

            var bogføringsresultat  = new Bogføringsresultat(bogføringslinje);
            Assert.That(bogføringsresultat, Is.Not.Null);
            Assert.That(bogføringsresultat.Bogføringslinje, Is.Not.Null);
            Assert.That(bogføringsresultat.Bogføringslinje, Is.EqualTo(bogføringslinje));
            Assert.That(bogføringsresultat.Advarsler, Is.Not.Null);
            Assert.That(bogføringsresultat.Advarsler, Is.TypeOf(typeof (List<IBogføringsadvarsel>)));
        }

        /// <summary>
        /// Tester, at konstruktøren danner advarsel ved overtræk på kontoen.
        /// </summary>
        [Test]
        public void TestAtConstructorDannerBogføringsadvarselVedOvertrækPåKonto()
        {
            var fixture = new Fixture();
            fixture.Inject(DateTime.Now);

            var loebenr = fixture.Create<int>();
            var konto = fixture.Create<Konto>();
            Assert.That(konto, Is.Not.Null);
            konto.TilføjKreditoplysninger(new Kreditoplysninger(fixture.Create<DateTime>().Year, fixture.Create<DateTime>().Month, 50000M));
            konto.TilføjBogføringslinje(new Bogføringslinje(loebenr, fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), 0M, 48000M));

            var bogføringslinje = new Bogføringslinje(loebenr + 1, fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), 0M, 5000M);
            Assert.That(bogføringslinje, Is.Not.Null);
            konto.TilføjBogføringslinje(bogføringslinje);

            var bogføringsresultat = new Bogføringsresultat(bogføringslinje);
            Assert.That(bogføringsresultat, Is.Not.Null);
            Assert.That(bogføringsresultat.Bogføringslinje, Is.Not.Null);
            Assert.That(bogføringsresultat.Bogføringslinje, Is.EqualTo(bogføringslinje));
            Assert.That(bogføringsresultat.Advarsler, Is.Not.Null);
            Assert.That(bogføringsresultat.Advarsler, Is.TypeOf(typeof (List<IBogføringsadvarsel>)));
            Assert.That(bogføringsresultat.Advarsler.Count(), Is.EqualTo(1));

            var bogføringsadvarsel = bogføringsresultat.Advarsler.ElementAt(0);
            Assert.That(bogføringsadvarsel, Is.Not.Null);
            Assert.That(bogføringsadvarsel.Advarsel, Is.Not.Null);
            Assert.That(bogføringsadvarsel.Konto, Is.Not.Null);
            Assert.That(bogføringsadvarsel.Konto, Is.EqualTo(konto));
            Assert.That(bogføringsadvarsel.Beløb, Is.EqualTo(3000M));
        }

        /// <summary>
        /// Tester, at konstruktøren danner advarsel ved overtræk på budgetkontoen.
        /// </summary>
        [Test]
        public void TestAtConstructorDannerBogføringsadvarselVedOvertrækPåBudgetkonto()
        {
            var fixture = new Fixture();
            fixture.Inject(DateTime.Now);

            var loebenr = fixture.Create<int>();
            var konto = fixture.Create<Konto>();
            Assert.That(konto, Is.Not.Null);
            konto.TilføjKreditoplysninger(new Kreditoplysninger(fixture.Create<DateTime>().Year, fixture.Create<DateTime>().Month, 50000M));
            konto.TilføjBogføringslinje(new Bogføringslinje(loebenr, fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), 0M, 25000M));

            var budgetkonto = fixture.Create<Budgetkonto>();
            Assert.That(budgetkonto, Is.Not.Null);
            budgetkonto.TilføjBudgetoplysninger(new Budgetoplysninger(fixture.Create<DateTime>().Year, fixture.Create<DateTime>().Month, 0M, 3000M));

            var bogføringslinje = new Bogføringslinje(loebenr + 1, fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), 0M, 5000M);
            Assert.That(bogføringslinje, Is.Not.Null);
            konto.TilføjBogføringslinje(bogføringslinje);
            budgetkonto.TilføjBogføringslinje(bogføringslinje);

            var bogføringsresultat = new Bogføringsresultat(bogføringslinje);
            Assert.That(bogføringsresultat, Is.Not.Null);
            Assert.That(bogføringsresultat.Bogføringslinje, Is.Not.Null);
            Assert.That(bogføringsresultat.Bogføringslinje, Is.EqualTo(bogføringslinje));
            Assert.That(bogføringsresultat.Advarsler, Is.Not.Null);
            Assert.That(bogføringsresultat.Advarsler, Is.TypeOf(typeof(List<IBogføringsadvarsel>)));
            Assert.That(bogføringsresultat.Advarsler.Count(), Is.EqualTo(1));

            var bogføringsadvarsel = bogføringsresultat.Advarsler.ElementAt(0);
            Assert.That(bogføringsadvarsel, Is.Not.Null);
            Assert.That(bogføringsadvarsel.Advarsel, Is.Not.Null);
            Assert.That(bogføringsadvarsel.Konto, Is.Not.Null);
            Assert.That(bogføringsadvarsel.Konto, Is.EqualTo(budgetkonto));
            Assert.That(bogføringsadvarsel.Beløb, Is.EqualTo(2000M));
        }
    }
}
