using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste
{
    /// <summary>
    /// Tests the range which can describe an interval.
    /// </summary>
    [TestFixture]
    public class RangeTests
    {
        /// <summary>
        /// Tests that the constructor initialize the range which can describe an interval.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeRange()
        {
            Fixture fixture = new Fixture();

            int startValue = fixture.Create<int>();
            int endValue = fixture.Create<int>();

            IRange<int> sut = CreateSut(startValue, endValue);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.StartValue, Is.EqualTo(startValue));
            Assert.That(sut.EndValue, Is.EqualTo(endValue));
        }

        /// <summary>
        /// Creates an instance of a range which can describe an interval.
        /// </summary>
        /// <param name="startValue">The start value for the interval.</param>
        /// <param name="endValue">The end value for the interval.</param>
        /// <returns>An instance of a range which can describe an interval.</returns>
        private static IRange<int> CreateSut(int startValue, int endValue)
        {
            return new Range<int>(startValue, endValue);
        }
    }
}
