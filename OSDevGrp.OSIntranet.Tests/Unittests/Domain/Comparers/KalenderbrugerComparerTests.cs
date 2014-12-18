using System;
using OSDevGrp.OSIntranet.Domain.Comparers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Kalender;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Domain.Comparers
{
    /// <summary>
    /// Tester klasse til sammenligning af kalenderbrugere.
    /// </summary>
    [TestFixture]
    public class KalenderbrugerComparerTests
    {
        /// <summary>
        /// Tester, at Compare kaster en ArgumentNullException, hvis X er null.
        /// </summary>
        [Test]
        public void TestAtCompareKasterArgumentNullExceptionHvisXErNull()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<IBruger>());

            var comparer = fixture.Create<KalenderbrugerComparer>();
            Assert.That(comparer, Is.Not.Null);
            Assert.Throws<ArgumentNullException>(() => comparer.Compare(null, fixture.Create<IBruger>()));
        }

        /// <summary>
        /// Tester, at Compare kaster en ArgumentNullException, hvis Y er null.
        /// </summary>
        [Test]
        public void TestAtCompareKasterArgumentNullExceptionHvisYErNull()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<IBruger>());

            var comparer = fixture.Create<KalenderbrugerComparer>();
            Assert.That(comparer, Is.Not.Null);
            Assert.Throws<ArgumentNullException>(() => comparer.Compare(fixture.Create<IBruger>(), null));
        }

        /// <summary>
        /// Tester, at Compare returnerer værdi for sammenligning af navn.
        /// </summary>
        [Test]
        public void TestAtCompareReturnererVærdiForSammenligningAfNavn()
        {
            var fixture = new Fixture();

            var brugerX = MockRepository.GenerateMock<IBruger>();
            brugerX.Expect(m => m.Navn)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var brugerY = MockRepository.GenerateMock<IBruger>();
            brugerY.Expect(m => m.Navn)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var result = brugerX.Navn.CompareTo(brugerY.Navn);

            var comparer = fixture.Create<KalenderbrugerComparer>();
            Assert.That(comparer, Is.Not.Null);
            Assert.That(comparer.Compare(brugerX, brugerY), Is.EqualTo(result));
        }

        /// <summary>
        /// Tester, at Compare returnerer værdi for sammenligning af initialer.
        /// </summary>
        [Test]
        public void TestAtCompareReturnererVærdiForSammenligningAfInitialer()
        {
            var fixture = new Fixture();

            var brugerX = MockRepository.GenerateMock<IBruger>();
            brugerX.Expect(m => m.Navn)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            brugerX.Expect(m => m.Initialer)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var brugerY = MockRepository.GenerateMock<IBruger>();
            brugerY.Expect(m => m.Navn)
                .Return(brugerX.Navn)
                .Repeat.Any();
            brugerY.Expect(m => m.Initialer)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var result = brugerX.Initialer.CompareTo(brugerY.Initialer);

            var comparer = fixture.Create<KalenderbrugerComparer>();
            Assert.That(comparer, Is.Not.Null);
            Assert.That(comparer.Compare(brugerX, brugerY), Is.EqualTo(result));
        }

        /// <summary>
        /// Tester, at Compare returnerer værdi for sammenligning af identifikation.
        /// </summary>
        [Test]
        public void TestAtCompareReturnererVærdiForSammenligningAfId()
        {
            var fixture = new Fixture();

            var brugerX = MockRepository.GenerateMock<IBruger>();
            brugerX.Expect(m => m.Navn)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            brugerX.Expect(m => m.Initialer)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            brugerX.Expect(m => m.Id)
                .Return(fixture.Create<int>())
                .Repeat.Any();

            var brugerY = MockRepository.GenerateMock<IBruger>();
            brugerY.Expect(m => m.Navn)
                .Return(brugerX.Navn)
                .Repeat.Any();
            brugerY.Expect(m => m.Initialer)
                .Return(brugerX.Initialer)
                .Repeat.Any();
            brugerY.Expect(m => m.Id)
                .Return(fixture.Create<int>())
                .Repeat.Any();

            var result = brugerX.Id.CompareTo(brugerY.Id);

            var comparer = fixture.Create<KalenderbrugerComparer>();
            Assert.That(comparer, Is.Not.Null);
            Assert.That(comparer.Compare(brugerX, brugerY), Is.EqualTo(result));
        }
    }
}
