using System;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommandHandlers.Validation;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.CommandHandlers.Validation
{
    /// <summary>
    /// Tests the common validations.
    /// </summary>
    [TestFixture]
    public class CommonValidationsTests
    {
        /// <summary>
        /// Tests that the constructor initialize the common validations.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeCommonValidations()
        {
            var commonValidations = new CommonValidations();
            Assert.That(commonValidations, Is.Not.Null);
        }

        /// <summary>
        /// Tests that IsGuidLegal returns true, when the GUID is legal.
        /// </summary>
        [Test]
        public void TestThatIsGuidLegalReturnsTrueWhenGuidIsLegal()
        {
            var commonValidations = new CommonValidations();
            Assert.That(commonValidations, Is.Not.Null);

            Assert.That(commonValidations.IsGuidLegal(Guid.NewGuid()), Is.EqualTo(true));
        }

        /// <summary>
        /// Tests that IsGuidLegal returns false, when the GUID is illegal.
        /// </summary>
        [Test]
        public void TestThatIsGuidLegalReturnsFalseWhenGuidIsIllegal()
        {
            var commonValidations = new CommonValidations();
            Assert.That(commonValidations, Is.Not.Null);

            Assert.That(commonValidations.IsGuidLegal(Guid.Empty), Is.EqualTo(false));
        }

        /// <summary>
        /// Tests that HasValue returns true, when the string has a value.
        /// </summary>
        [Test]
        public void TestThatHasValueReturnsTrueWhenStringHasValue()
        {
            var fixture = new Fixture();

            var commonValidations = new CommonValidations();
            Assert.That(commonValidations, Is.Not.Null);

            Assert.That(commonValidations.HasValue(fixture.Create<string>()), Is.EqualTo(true));
        }

        /// <summary>
        /// Tests that HasValue returns false, when the string has no value.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void TestThatHasValueReturnsFalseWhenStringHasNoValue(string emptyValue)
        {
            var commonValidations = new CommonValidations();
            Assert.That(commonValidations, Is.Not.Null);

            Assert.That(commonValidations.HasValue(emptyValue), Is.EqualTo(false));
        }

        /// <summary>
        /// Tests that ContainsIllegalChar returns false, when the string has no value.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void TestThatContainsIllegalCharReturnsFalseWhenStringHasNoValue(string emptyValue)
        {
            var commonValidations = new CommonValidations();
            Assert.That(commonValidations, Is.Not.Null);

            Assert.That(commonValidations.ContainsIllegalChar(emptyValue), Is.EqualTo(false));
        }

        /// <summary>
        /// Tests that ContainsIllegalChar returns false, when the string does not contain any illegal chars.
        /// </summary>
        [Test]
        public void TestThatContainsIllegalCharReturnsFalseWhenStringDoesNotContainAnyIllegalChars()
        {
            var fixture = new Fixture();
            var value = fixture.Create<string>();

            var commonValidations = new CommonValidations();
            Assert.That(commonValidations, Is.Not.Null);

            Assert.That(commonValidations.ContainsIllegalChar(CommonValidations.IllegalChars.Aggregate(value, (current, illegalChar) => current.Replace(Convert.ToString(illegalChar), null))), Is.EqualTo(false));
        }

        /// <summary>
        /// Tests that ContainsIllegalChar returns true, when the string contains one of the illegal chars.
        /// </summary>
        [Test]
        public void TestThatContainsIllegalCharReturnsTrueWhenStringContainsIllegalChars()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());

            var commonValidations = new CommonValidations();
            Assert.That(commonValidations, Is.Not.Null);

            foreach (var illegalChar in CommonValidations.IllegalChars)
            {
                var value = fixture.Create<string>();
                var pos = random.Next(0, value.Length - 1);

                Assert.That(commonValidations.ContainsIllegalChar(value.Substring(0, pos) + illegalChar + value.Substring(pos)), Is.EqualTo(true));
            }
        }

        /// <summary>
        /// Tests that IsNotNull returns true when the object is not null.
        /// </summary>
        [Test]
        public void TestThatIsNotNullReturnsTrueWhenObjectIsNotNull()
        {
            var fixture = new Fixture();

            var commonValidations = new CommonValidations();
            Assert.That(commonValidations, Is.Not.Null);

            Assert.That(commonValidations.IsNotNull(fixture.Create<object>()), Is.True);
        }

        /// <summary>
        /// Tests that IsNotNull returns false when the object is null.
        /// </summary>
        [Test]
        public void TestThatIsNotNullReturnsFalseWhenObjectIsNull()
        {
            var commonValidations = new CommonValidations();
            Assert.That(commonValidations, Is.Not.Null);

            Assert.That(commonValidations.IsNotNull(null), Is.False);
        }
    }
}
