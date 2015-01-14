using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.FoodWaste;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste
{
    /// <summary>
    /// Tests basic functionality for a translatable domain object in the food waste domain.
    /// </summary>
    [TestFixture]
    public class TranslatableBaseTests
    {
        /// <summary>
        /// Private class for testing the basic functionality on a translatable domain object in the food waste domain.
        /// </summary>
        private class MyTranslatable : TranslatableBase
        {
        }

        /// <summary>
        /// Tests that the constructor initialize basic functionality for a translatable domain object in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeTranslatableBase()
        {
            var translatable = new MyTranslatable();
            Assert.That(translatable, Is.Not.Null);
            Assert.That(translatable.Identifier.HasValue, Is.False);
        }

        /// <summary>
        /// Tests that Translate throws ArgumentNullException if the culture information which are used for translation is null.
        /// </summary>
        [Test]
        public void TestThatTranslateThrowsArgumentNullExceptionIfTranslationCultureIsNull()
        {
            var translatable = new MyTranslatable();
            Assert.That(translatable, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => translatable.Translate(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("translationCulture"));
            Assert.That(exception.InnerException, Is.Null);
        }
    }
}
