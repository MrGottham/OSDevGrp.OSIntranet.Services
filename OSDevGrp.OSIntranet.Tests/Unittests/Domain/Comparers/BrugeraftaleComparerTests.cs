using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.Domain.Comparers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Kalender;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Domain.Comparers
{
    /// <summary>
    /// Tester klasse til sammenligning af deltagere på kalenderaftaler.
    /// </summary>
    [TestFixture]
    public class BrugeraftaleComparerTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer klasse til sammenligning af deltagere på kalenderaftaler.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererBrugeraftaleComparer()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<IComparer<IBruger>>());

            var comparer = fixture.Create<BrugeraftaleComparer>();
            Assert.That(comparer, Is.Not.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis klassen til sammenligning af kalenderbrugere er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisBrugerComparerErNull()
        {
            var fixture = new Fixture();
            fixture.Inject<IComparer<IBruger>>(null);

            Assert.Throws<ArgumentNullException>(
                () => new BrugeraftaleComparer(fixture.Create<IComparer<IBruger>>()));
        }

        /// <summary>
        /// Tester, at Compare kaster en ArgumentNullException, hvis X er null.
        /// </summary>
        [Test]
        public void TestAtCompareKasterArgumentNullExceptionHvisXErNull()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<IComparer<IBruger>>());
            fixture.Inject(MockRepository.GenerateMock<IBrugeraftale>());

            var comparer = fixture.Create<BrugeraftaleComparer>();
            Assert.That(comparer, Is.Not.Null);
            Assert.Throws<ArgumentNullException>(() => comparer.Compare(null, fixture.Create<IBrugeraftale>()));
        }

        /// <summary>
        /// Tester, at Compare kaster en ArgumentNullException, hvis Y er null.
        /// </summary>
        [Test]
        public void TestAtCompareKasterArgumentNullExceptionHvisYErNull()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<IComparer<IBruger>>());
            fixture.Inject(MockRepository.GenerateMock<IBrugeraftale>());

            var comparer = fixture.Create<BrugeraftaleComparer>();
            Assert.That(comparer, Is.Not.Null);
            Assert.Throws<ArgumentNullException>(() => comparer.Compare(fixture.Create<IBrugeraftale>(), null));
        }

        /// <summary>
        /// Tester, at Compare returnerer værdi fra sammenligningen af brugere.
        /// </summary>
        [Test]
        public void TestAtCompareReturnerVærdiForSammenligningAfBruger()
        {
            var fixture = new Fixture();

            var brugerAftale = MockRepository.GenerateMock<IBrugeraftale>();
            brugerAftale.Expect(m => m.Bruger)
                .Return(MockRepository.GenerateMock<IBruger>());
            fixture.Inject(brugerAftale);

            var result = fixture.Create<int>();
            var brugerComparer = MockRepository.GenerateMock<IComparer<IBruger>>();
            brugerComparer.Expect(m => m.Compare(Arg<IBruger>.Is.NotNull, Arg<IBruger>.Is.NotNull))
                .Return(result);
            fixture.Inject(brugerComparer);

            var comparer = fixture.Create<BrugeraftaleComparer>();
            Assert.That(comparer, Is.Not.Null);
            Assert.That(comparer.Compare(fixture.Create<IBrugeraftale>(), fixture.Create<IBrugeraftale>()), Is.EqualTo(result));

            brugerComparer.AssertWasCalled(m => m.Compare(Arg<IBruger>.Is.NotNull, Arg<IBruger>.Is.NotNull));
        }
    }
}
