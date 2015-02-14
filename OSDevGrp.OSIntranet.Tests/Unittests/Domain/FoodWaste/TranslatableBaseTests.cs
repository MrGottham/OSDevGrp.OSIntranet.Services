using System;
using System.Collections.Generic;
using System.Globalization;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;

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
            /// <summary>
            /// Initialize the basic functionality on a translatable domain object in the food waste domain.
            /// </summary>
            public void Initialize()
            {
                Translations = DomainObjectMockBuilder.BuildTranslationCollection();
            }

            /// <summary>
            /// Clears the basic functionality on a translatable domain object in the food waste domain.
            /// </summary>
            public void Clear()
            {
                Translations = new List<ITranslation>(0);
            }
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
            Assert.That(translatable.Translation, Is.Null);
            Assert.That(translatable.Translations, Is.Not.Null);
            Assert.That(translatable.Translations, Is.Empty);
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

        /// <summary>
        /// Tests that Translate sets Translation to null when the collection of translations is empty.
        /// </summary>
        [Test]
        public void TestThatTranslateSetsTranslationToNullWhenTranslationCollectionIsEmpty()
        {
            var translatable = new MyTranslatable();
            Assert.That(translatable, Is.Not.Null);

            translatable.Clear();
            Assert.That(translatable.Translations, Is.Not.Null);
            Assert.That(translatable.Translations, Is.Empty);

            translatable.Translate(CultureInfo.CurrentUICulture);
            Assert.That(translatable.Translation, Is.Null);
        }

        /// <summary>
        /// Tests that Translate sets Translation to a given culture translation when the collection of translations is contains a translation for the culture.
        /// </summary>
        [Test]
        public void TestThatTranslateSetsTranslationWhenTranslationCollectionContainsCultureTranslation()
        {
            var translatable = new MyTranslatable();
            Assert.That(translatable, Is.Not.Null);

            translatable.Initialize();
            Assert.That(translatable.Translations, Is.Not.Null);
            Assert.That(translatable.Translations, Is.Not.Empty);

            foreach (var translation in translatable.Translations)
            {
                translatable.Translate(translation.TranslationInfo.CultureInfo);
                Assert.That(translatable.Translation, Is.Not.Null);
                Assert.That(translatable.Translation, Is.EqualTo(translation));
            }
        }

        /// <summary>
        /// Tests that Translate sets Translation to a default culture translation when the collection of translations does not contains a translation for the culture.
        /// </summary>
        [Test]
        [TestCase("fr-FR")]
        [TestCase("es-ES")]
        public void TestThatTranslateSetsTranslationWhenTranslationCollectionDoesNotContainsCultureTranslation(string notTranslatedCultureName)
        {
            var notTranslatedCulture = new CultureInfo(notTranslatedCultureName);

            var translatable = new MyTranslatable();
            Assert.That(translatable, Is.Not.Null);

            translatable.Initialize();
            Assert.That(translatable.Translations, Is.Not.Null);
            Assert.That(translatable.Translations, Is.Not.Empty);

            translatable.Translate(notTranslatedCulture);
            Assert.That(translatable.Translation, Is.Not.Null);
        }
    }
}
