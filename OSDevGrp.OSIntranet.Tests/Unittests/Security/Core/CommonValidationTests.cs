using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Security.Core;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Security.Core
{
    /// <summary>
    /// Tests the common validation used by the security logic.
    /// </summary>
    [TestFixture]
    public class CommonValidationTests
    {
        /// <summary>
        /// Tests that the constructor initialize the common validation used by the security logic.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeCommonValidation()
        {
            var commonValidation = new CommonValidation();
            Assert.That(commonValidation, Is.Not.Null);
        }

        /// <summary>
        /// Tests that IsMailAddress throws an ArgumentNullException when value to validate is invalid.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestThatIsMailAddressThrowsArgumentNullExceptionWhenValueIsInvalid(string invalidValue)
        {
            var commonValidation = new CommonValidation();
            Assert.That(commonValidation, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => commonValidation.IsMailAddress(invalidValue));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("value"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that IsMailAddress returns false when value is not a mail address.
        /// </summary>
        [Test]
        [TestCase("XYZ")]
        [TestCase("ZYX.local")]
        public void TestThatIsMailAddressReturnsFalseWhenValueIsNotMailAddress(string noMailAddress)
        {
            var commonValidation = new CommonValidation();
            Assert.That(commonValidation, Is.Not.Null);

            var result = commonValidation.IsMailAddress(noMailAddress);
            Assert.That(result, Is.False);
        }

        /// <summary>
        /// Tests that IsMailAddress returns true when value is a mail address.
        /// </summary>
        [Test]
        [TestCase("mrgottham@gmail.com")]
        [TestCase("mrgottham@osdevgp.dk")]
        public void TestThatIsMailAddressReturnsTrueWhenValueIsMailAddress(string mailAddress)
        {
            var commonValidation = new CommonValidation();
            Assert.That(commonValidation, Is.Not.Null);

            var result = commonValidation.IsMailAddress(mailAddress);
            Assert.That(result, Is.True);
        }
    }
}
