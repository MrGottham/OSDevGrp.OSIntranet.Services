using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommandHandlers.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Resources;
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

        /// <summary>
        /// Tests that Equals for two strings returns whether the two string are equal or not.
        /// </summary>
        [Test]
        [TestCase(null, null, StringComparison.Ordinal, true)]
        [TestCase(null, null, StringComparison.OrdinalIgnoreCase, true)]
        [TestCase("", "", StringComparison.Ordinal, true)]
        [TestCase("", "", StringComparison.OrdinalIgnoreCase, true)]
        [TestCase("XXX", "xxx", StringComparison.Ordinal, false)]
        [TestCase("XXX", "xxx", StringComparison.OrdinalIgnoreCase, true)]
        [TestCase("xxx", "XXX", StringComparison.Ordinal, false)]
        [TestCase("xxx", "XXX", StringComparison.OrdinalIgnoreCase, true)]
        [TestCase("XXX", "YYY", StringComparison.Ordinal, false)]
        [TestCase("XXX", "YYY", StringComparison.OrdinalIgnoreCase, false)]
        public void TestThatEqualsForTwoStringsReturnsCompareResult(string xValue, string yValue, StringComparison comparisonType, bool expectedResult)
        {
            var commonValidations = new CommonValidations();
            Assert.That(commonValidations, Is.Not.Null);

            var result = commonValidations.Equals(xValue, yValue, comparisonType);
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Tests that IsLegalEnumValue when calling with legal values throws an ArgumentNullException when the legal values is null.
        /// </summary>
        [Test]
        public void TestThatIsLegalEnumValueWithLegalValuesThrowsArgumentNullExceptionWhenLegalValuesIsNull()
        {
            var fixture = new Fixture();

            var commonValidations = new CommonValidations();
            Assert.That(commonValidations, Is.Not.Null);

            var excpetion = Assert.Throws<ArgumentNullException>(() => commonValidations.IsLegalEnumValue<Membership>(fixture.Create<string>(), null));
            Assert.That(excpetion, Is.Not.Null);
            Assert.That(excpetion.ParamName, Is.Not.Null);
            Assert.That(excpetion.ParamName, Is.Not.Empty);
            Assert.That(excpetion.ParamName, Is.EqualTo("legalValues"));
            Assert.That(excpetion.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that IsLegalEnumValue when calling with legal values throws an IntranetSystemException when the type of enum is not an enum type.
        /// </summary>
        [Test]
        public void TestThatIsLegalEnumValueWithLegalValuesThrowsIntranetSystemExceptionWhenEnumTypeIsNotEnum()
        {
            var fixture = new Fixture();

            var commonValidations = new CommonValidations();
            Assert.That(commonValidations, Is.Not.Null);

            var excpetion = Assert.Throws<IntranetSystemException>(() => commonValidations.IsLegalEnumValue(fixture.Create<string>(), new List<int> {0, 1}));
            Assert.That(excpetion, Is.Not.Null);
            Assert.That(excpetion.Message, Is.Not.Null);
            Assert.That(excpetion.Message, Is.Not.Empty);
            Assert.That(excpetion.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.GenericTypeHasInvalidType, "TEnum", typeof (int).Name)));
            Assert.That(excpetion.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that IsLegalEnumValue when calling with legal values returns whether the given string value are a legal enum value.
        /// </summary>
        [Test]
        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase(" ", false)]
        [TestCase("  ", false)]
        [TestCase("   ", false)]
        [TestCase("XYZ", false)]
        [TestCase("Basic", false)]
        [TestCase("Deluxe", true)]
        [TestCase("Premium", true)]
        public void TestThatIsLegalEnumValueWithLegalValuesReturnsWhetherValueIsLegalEnumValue(string value, bool expectedResult)
        {
            var commonValidations = new CommonValidations();
            Assert.That(commonValidations, Is.Not.Null);

            var result = commonValidations.IsLegalEnumValue(value, new List<Membership> {Membership.Deluxe, Membership.Premium});
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Tests that IsLegalEnumValue when calling without legal values throws an IntranetSystemException when the type of enum is not an enum type.
        /// </summary>
        [Test]
        public void TestThatIsLegalEnumValueWithoutLegalValuesThrowsIntranetSystemExceptionWhenEnumTypeIsNotEnum()
        {
            var fixture = new Fixture();

            var commonValidations = new CommonValidations();
            Assert.That(commonValidations, Is.Not.Null);

            var excpetion = Assert.Throws<IntranetSystemException>(() => commonValidations.IsLegalEnumValue<int>(fixture.Create<string>()));
            Assert.That(excpetion, Is.Not.Null);
            Assert.That(excpetion.Message, Is.Not.Null);
            Assert.That(excpetion.Message, Is.Not.Empty);
            Assert.That(excpetion.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.GenericTypeHasInvalidType, "TEnum", typeof(int).Name)));
            Assert.That(excpetion.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that IsLegalEnumValue when calling without legal values returns whether the given string value are a legal enum value.
        /// </summary>
        [Test]
        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase(" ", false)]
        [TestCase("  ", false)]
        [TestCase("   ", false)]
        [TestCase("XYZ", false)]
        [TestCase("Basic", true)]
        [TestCase("Deluxe", true)]
        [TestCase("Premium", true)]
        public void TestThatIsLegalEnumValueWithoutLegalValuesReturnsWhetherValueIsLegalEnumValue(string value, bool expectedResult)
        {
            var commonValidations = new CommonValidations();
            Assert.That(commonValidations, Is.Not.Null);

            var result = commonValidations.IsLegalEnumValue<Membership>(value);
            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}
