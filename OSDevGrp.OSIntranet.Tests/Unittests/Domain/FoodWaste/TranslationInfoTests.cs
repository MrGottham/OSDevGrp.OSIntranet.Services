using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.FoodWaste;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste
{
    /// <summary>
    /// Tests the translation information which are used for translation.
    /// </summary>
    [TestFixture]
    public class TranslationInfoTests
    {
        /// <summary>
        /// Tests that the constructor initialize a translation information which can be used for translation.
        /// </summary>
        [Test]
        [TestCase("da-DK")]
        [TestCase("en-US")]
        public void TestThatConstructorInitializeTranslationInfo(string cultureName)
        {
            var translationInfo = new TranslationInfo(cultureName);
            Assert.That(translationInfo, Is.Not.Null);
            Assert.That(translationInfo.Identifier, Is.Null);
            Assert.That(translationInfo.Identifier.HasValue, Is.False);
            Assert.That(translationInfo.CultureName, Is.Not.Null);
            Assert.That(translationInfo.CultureName, Is.Not.Empty);
            Assert.That(translationInfo.CultureName, Is.EqualTo(cultureName));
            Assert.That(translationInfo.CultureInfo, Is.Not.Null);
            Assert.That(translationInfo.CultureInfo.Name, Is.Not.Null);
            Assert.That(translationInfo.CultureInfo.Name, Is.Not.Empty);
            Assert.That(translationInfo.CultureInfo.Name, Is.EqualTo(cultureName));
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException if the culture name is invalid.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestThatConstructorThrowsArgumentNullExceptionForIllegalCultureName(string cultureName)
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new TranslationInfo(cultureName));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("cultureName"));
            Assert.That(exception.InnerException, Is.Null);
        }
    }
}
