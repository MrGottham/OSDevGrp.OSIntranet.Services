using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
            /// <param name="translationForIdentifier">Identifier for the domain object which should be initialized with translations.</param>
            public void Initialize(Guid translationForIdentifier)
            {
                Translations = DomainObjectMockBuilder.BuildTranslationMockCollection(translationForIdentifier);
            }

            /// <summary>
            /// Gets whether the functionality which are executed when the translatable domain object are translated was called.
            /// </summary>
            public bool OnTranslateWasCalled { get; private set; }

            /// <summary>
            /// Clears the basic functionality on a translatable domain object in the food waste domain.
            /// </summary>
            public void Clear()
            {
                Translations = new List<ITranslation>(0);
            }

            /// <summary>
            /// Sets the collection of translations null.
            /// </summary>
            public void SetToNull()
            {
                Translations = null;
            }

            /// <summary>
            /// Functionality which are executed when the translatable domain object are translated
            /// </summary>
            /// <param name="translationCulture">Culture information which are used for translation.</param>
            protected override void OnTranslation(CultureInfo translationCulture)
            {
                base.OnTranslation(translationCulture);
                OnTranslateWasCalled = true;
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
        /// Tests that TranslationAdd throws an ArgumentNullException when the translation is null.
        /// </summary>
        [Test]
        public void TestThatTranslationAddThrowsArgumentNullExceptionWhenTranslationIsNull()
        {
            var translatable = new MyTranslatable();
            Assert.That(translatable, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => translatable.TranslationAdd(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("translation"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that TranslationAdd adds a translation for the domain object.
        /// </summary>
        [Test]
        public void TestThatTranslationAddAddsTranslation()
        {
            var identifier = Guid.NewGuid();
            var translatable = new MyTranslatable
            {
                Identifier = identifier
            };
            Assert.That(translatable, Is.Not.Null);
            Assert.That(translatable.Translations, Is.Not.Null);
            Assert.That(translatable.Translations, Is.Empty);

            var translationMock = DomainObjectMockBuilder.BuildTranslationMock(identifier);
            translatable.TranslationAdd(translationMock);
            Assert.That(translatable.Translations, Is.Not.Null);
            Assert.That(translatable.Translations, Is.Not.Empty);
            Assert.That(translatable.Translations.Contains(translationMock), Is.True);
        }

        /// <summary>
        /// Tests that Translate throws an ArgumentNullException if the culture information which are used for translation is null.
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
        /// Tests that Translate sets Translation to null when identifier on the basic functionality on a translatable domain object is null.
        /// </summary>
        [Test]
        public void TestThatTranslateSetsTranslationToNullWhenIdentifierOnTranslatableBaseIsNull()
        {
            var translatable = new MyTranslatable();
            Assert.That(translatable, Is.Not.Null);
            Assert.That(translatable.Identifier, Is.Null);
            Assert.That(translatable.Identifier.HasValue, Is.False);

            translatable.Initialize(Guid.NewGuid());
            Assert.That(translatable.Translations, Is.Not.Null);
            Assert.That(translatable.Translations, Is.Not.Empty);

            translatable.Translate(CultureInfo.CurrentUICulture);
            Assert.That(translatable.Translation, Is.Null);
        }

        /// <summary>
        /// Tests that Translate sets Translation to null when the collection of translations is null.
        /// </summary>
        [Test]
        public void TestThatTranslateSetsTranslationToNullWhenTranslationCollectionIsNull()
        {
            var translatable = new MyTranslatable
            {
                Identifier = Guid.NewGuid()
            };
            Assert.That(translatable, Is.Not.Null);
            Assert.That(translatable.Identifier, Is.Not.Null);
            Assert.That(translatable.Identifier.HasValue, Is.True);

            translatable.SetToNull();
            Assert.That(translatable.Translations, Is.Null);

            translatable.Translate(CultureInfo.CurrentUICulture);
            Assert.That(translatable.Translation, Is.Null);
        }

        /// <summary>
        /// Tests that Translate sets Translation to null when the collection of translations is empty.
        /// </summary>
        [Test]
        public void TestThatTranslateSetsTranslationToNullWhenTranslationCollectionIsEmpty()
        {
            var translatable = new MyTranslatable
            {
                Identifier = Guid.NewGuid()
            };
            Assert.That(translatable, Is.Not.Null);
            Assert.That(translatable.Identifier, Is.Not.Null);
            Assert.That(translatable.Identifier.HasValue, Is.True);

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
            var translatable = new MyTranslatable
            {
                Identifier = Guid.NewGuid()
            };
            Assert.That(translatable, Is.Not.Null);
            Assert.That(translatable.Identifier, Is.Not.Null);
            Assert.That(translatable.Identifier.HasValue, Is.True);

            // ReSharper disable PossibleInvalidOperationException
            translatable.Initialize(translatable.Identifier.Value);
            // ReSharper restore PossibleInvalidOperationException
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

            var translatable = new MyTranslatable
            {
                Identifier = Guid.NewGuid()
            };
            Assert.That(translatable, Is.Not.Null);
            Assert.That(translatable.Identifier, Is.Not.Null);
            Assert.That(translatable.Identifier.HasValue, Is.True);

            // ReSharper disable PossibleInvalidOperationException
            translatable.Initialize(translatable.Identifier.Value);
            // ReSharper restore PossibleInvalidOperationException
            Assert.That(translatable.Translations, Is.Not.Null);
            Assert.That(translatable.Translations, Is.Not.Empty);

            translatable.Translate(notTranslatedCulture);
            Assert.That(translatable.Translation, Is.Not.Null);
        }

        /// <summary>
        /// Tests that Translate sets Translation to null when the collection of translations does not contain translations for the identifier.
        /// </summary>
        [Test]
        public void TestThatTranslateSetsTranslationToNullWhenTranslationCollectionDoesNotContainIdentifierFromTranslatableBase()
        {
            var translatable = new MyTranslatable
            {
                Identifier = Guid.NewGuid()
            };
            Assert.That(translatable, Is.Not.Null);
            Assert.That(translatable.Identifier, Is.Not.Null);
            Assert.That(translatable.Identifier.HasValue, Is.True);

            translatable.Initialize(Guid.NewGuid());
            Assert.That(translatable.Translations, Is.Not.Null);
            Assert.That(translatable.Translations, Is.Not.Empty);

            translatable.Translate(CultureInfo.CurrentUICulture);
            Assert.That(translatable.Translation, Is.Null);
        }

        /// <summary>
        /// Tests that Translate calls the functionality which are executed when the translatable domain object are translated.
        /// </summary>
        [Test]
        public void TestThatTranslateCallsOnTranslation()
        {
            var translatable = new MyTranslatable
            {
                Identifier = Guid.NewGuid()
            };
            Assert.That(translatable, Is.Not.Null);
            Assert.That(translatable.Identifier, Is.Not.Null);
            Assert.That(translatable.Identifier.HasValue, Is.True);

            // ReSharper disable PossibleInvalidOperationException
            translatable.Initialize(translatable.Identifier.Value);
            // ReSharper restore PossibleInvalidOperationException
            Assert.That(translatable.Translations, Is.Not.Null);
            Assert.That(translatable.Translations, Is.Not.Empty);

            Assert.That(translatable.OnTranslateWasCalled, Is.False);
            translatable.Translate(translatable.Translations.First().TranslationInfo.CultureInfo);
            Assert.That(translatable.OnTranslateWasCalled, Is.True);
        }
    }
}
