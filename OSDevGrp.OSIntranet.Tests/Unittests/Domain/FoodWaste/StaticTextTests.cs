using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste
{
    /// <summary>
    /// Tests the static text used by the food waste domain.
    /// </summary>
    [TestFixture]
    public class StaticTextTests
    {
        /// <summary>
        /// Tests that the constructor initialize a static text without a translation identifier for the body.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeStaticTextWithoutBodyTranslationIdentifier()
        {
            var fixture = new Fixture();
            var staticTextType = fixture.Create<StaticTextType>();
            var subjectTranslationIdentifier = Guid.NewGuid();
            var staticText = new StaticText(staticTextType, subjectTranslationIdentifier);
            Assert.That(staticText, Is.Not.Null);
            Assert.That(staticText.Identifier, Is.Null);
            Assert.That(staticText.Identifier.HasValue, Is.False);
            Assert.That(staticText.Translation, Is.Null);
            Assert.That(staticText.Translations, Is.Not.Null);
            Assert.That(staticText.Translations, Is.Empty);
            Assert.That(staticText.SubjectTranslationIdentifier, Is.EqualTo(subjectTranslationIdentifier));
            Assert.That(staticText.SubjectTranslation, Is.Null);
            Assert.That(staticText.SubjectTranslations, Is.Not.Null);
            Assert.That(staticText.SubjectTranslations, Is.Empty);
            Assert.That(staticText.BodyTranslationIdentifier, Is.Null);
            Assert.That(staticText.BodyTranslationIdentifier.HasValue, Is.False);
            Assert.That(staticText.SubjectTranslation, Is.Null);
            Assert.That(staticText.SubjectTranslations, Is.Not.Null);
            Assert.That(staticText.SubjectTranslations, Is.Empty);
        }

        /// <summary>
        /// Tests that the constructor initialize a static text with a translation identifier for the body.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeStaticTextWithBodyTranslationIdentifier()
        {
            var fixture = new Fixture();
            var staticTextType = fixture.Create<StaticTextType>();
            var subjectTranslationIdentifier = Guid.NewGuid();
            var bodyTranslationIdentifier = Guid.NewGuid();
            var staticText = new StaticText(staticTextType, subjectTranslationIdentifier, bodyTranslationIdentifier);
            Assert.That(staticText, Is.Not.Null);
            Assert.That(staticText.Identifier, Is.Null);
            Assert.That(staticText.Identifier.HasValue, Is.False);
            Assert.That(staticText.Translation, Is.Null);
            Assert.That(staticText.Translations, Is.Not.Null);
            Assert.That(staticText.Translations, Is.Empty);
            Assert.That(staticText.SubjectTranslationIdentifier, Is.EqualTo(subjectTranslationIdentifier));
            Assert.That(staticText.SubjectTranslation, Is.Null);
            Assert.That(staticText.SubjectTranslations, Is.Not.Null);
            Assert.That(staticText.SubjectTranslations, Is.Empty);
            Assert.That(staticText.BodyTranslationIdentifier, Is.Not.Null);
            Assert.That(staticText.BodyTranslationIdentifier.HasValue, Is.True);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(staticText.BodyTranslationIdentifier.Value, Is.EqualTo(bodyTranslationIdentifier));
            // ReSharper restore PossibleInvalidOperationException
            Assert.That(staticText.SubjectTranslation, Is.Null);
            Assert.That(staticText.SubjectTranslations, Is.Not.Null);
            Assert.That(staticText.SubjectTranslations, Is.Empty);
        }

        /// <summary>
        /// Tests that the Translate sets the translation for the static text to null.
        /// </summary>
        [Test]
        public void TestThatTranslateSetsTranslationToNull()
        {
            var fixture = new Fixture();
            var subjectTranslationIdentifier = Guid.NewGuid();

            var staticText = new StaticText(fixture.Create<StaticTextType>(), subjectTranslationIdentifier);
            Assert.That(staticText, Is.Not.Null);
            Assert.That(staticText.Translation, Is.Null);
            Assert.That(staticText.Translations, Is.Not.Null);
            Assert.That(staticText.Translations, Is.Empty);

            var translationMock = DomainObjectMockBuilder.BuildTranslationMock(subjectTranslationIdentifier);
            staticText.TranslationAdd(translationMock);
            Assert.That(staticText.Translation, Is.Null);
            Assert.That(staticText.Translations, Is.Not.Null);
            Assert.That(staticText.Translations, Is.Not.Empty);

            staticText.Translate(translationMock.TranslationInfo.CultureInfo);
            Assert.That(staticText.Translation, Is.Null);
        }

        /// <summary>
        /// Tests that the Translate sets the translation for the subject when translation was found.
        /// </summary>
        [Test]
        public void TestThatTranslateSetsSubjectTranslationWhenTranslationWasFound()
        {
            var fixture = new Fixture();
            var subjectTranslationIdentifier = Guid.NewGuid();

            var staticText = new StaticText(fixture.Create<StaticTextType>(), subjectTranslationIdentifier);
            Assert.That(staticText, Is.Not.Null);
            Assert.That(staticText.SubjectTranslationIdentifier, Is.EqualTo(subjectTranslationIdentifier));
            Assert.That(staticText.SubjectTranslation, Is.Null);
            Assert.That(staticText.SubjectTranslations, Is.Not.Null);
            Assert.That(staticText.SubjectTranslations, Is.Empty);

            var translationMock = DomainObjectMockBuilder.BuildTranslationMock(subjectTranslationIdentifier);
            staticText.TranslationAdd(translationMock);
            Assert.That(staticText.SubjectTranslation, Is.Null);
            Assert.That(staticText.SubjectTranslations, Is.Not.Null);
            Assert.That(staticText.SubjectTranslations, Is.Not.Empty);

            staticText.Translate(translationMock.TranslationInfo.CultureInfo);
            Assert.That(staticText.SubjectTranslation, Is.Not.Null);
            Assert.That(staticText.SubjectTranslation, Is.EqualTo(translationMock));
        }

        /// <summary>
        /// Tests that the Translate sets the translation for the subject to null when translation was not found.
        /// </summary>
        [Test]
        public void TestThatTranslateSetsSubjectTranslationToNullWhenTranslationWasNotFound()
        {
            var fixture = new Fixture();
            var subjectTranslationIdentifier = Guid.NewGuid();

            var staticText = new StaticText(fixture.Create<StaticTextType>(), subjectTranslationIdentifier);
            Assert.That(staticText, Is.Not.Null);
            Assert.That(staticText.SubjectTranslationIdentifier, Is.EqualTo(subjectTranslationIdentifier));
            Assert.That(staticText.SubjectTranslation, Is.Null);
            Assert.That(staticText.SubjectTranslations, Is.Not.Null);
            Assert.That(staticText.SubjectTranslations, Is.Empty);

            var translationMock = DomainObjectMockBuilder.BuildTranslationMock(Guid.NewGuid());
            staticText.TranslationAdd(translationMock);
            Assert.That(staticText.SubjectTranslation, Is.Null);
            Assert.That(staticText.SubjectTranslations, Is.Not.Null);
            Assert.That(staticText.SubjectTranslations, Is.Empty);

            staticText.Translate(translationMock.TranslationInfo.CultureInfo);
            Assert.That(staticText.SubjectTranslation, Is.Null);
        }

        /// <summary>
        /// Tests that the Translate sets the translation for the body to null when the translation identifier for the body is null.
        /// </summary>
        [Test]
        public void TestThatTranslateSetsBodyTranslationToNullWhenBodyTranslationIdentifierIsNull()
        {
            var fixture = new Fixture();

            var staticText = new StaticText(fixture.Create<StaticTextType>(), Guid.NewGuid());
            Assert.That(staticText, Is.Not.Null);
            Assert.That(staticText.BodyTranslationIdentifier, Is.Null);
            Assert.That(staticText.BodyTranslationIdentifier.HasValue, Is.False);
            Assert.That(staticText.BodyTranslation, Is.Null);
            Assert.That(staticText.BodyTranslations, Is.Not.Null);
            Assert.That(staticText.BodyTranslations, Is.Empty);

            var translationMock = DomainObjectMockBuilder.BuildTranslationMock(Guid.NewGuid());
            staticText.TranslationAdd(translationMock);
            Assert.That(staticText.BodyTranslation, Is.Null);
            Assert.That(staticText.BodyTranslations, Is.Not.Null);
            Assert.That(staticText.BodyTranslations, Is.Empty);

            staticText.Translate(translationMock.TranslationInfo.CultureInfo);
            Assert.That(staticText.BodyTranslation, Is.Null);
        }

        /// <summary>
        /// Tests that the Translate sets the translation for the body when the translation identifier for the body is not null and translation was found.
        /// </summary>
        [Test]
        public void TestThatTranslateSetsBodyTranslationWhenBodyTranslationIdentifierIsNotNullAndTranslationWasFound()
        {
            var fixture = new Fixture();
            var bodyTranslationIdentifier = Guid.NewGuid();

            var staticText = new StaticText(fixture.Create<StaticTextType>(), Guid.NewGuid(), bodyTranslationIdentifier);
            Assert.That(staticText, Is.Not.Null);
            Assert.That(staticText.BodyTranslationIdentifier, Is.Not.Null);
            Assert.That(staticText.BodyTranslationIdentifier.HasValue, Is.True);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(staticText.BodyTranslationIdentifier.Value, Is.EqualTo(bodyTranslationIdentifier));
            // ReSharper restore PossibleInvalidOperationException
            Assert.That(staticText.BodyTranslation, Is.Null);
            Assert.That(staticText.BodyTranslations, Is.Not.Null);
            Assert.That(staticText.BodyTranslations, Is.Empty);

            var translationMock = DomainObjectMockBuilder.BuildTranslationMock(bodyTranslationIdentifier);
            staticText.TranslationAdd(translationMock);
            Assert.That(staticText.BodyTranslation, Is.Null);
            Assert.That(staticText.BodyTranslations, Is.Not.Null);
            Assert.That(staticText.BodyTranslations, Is.Not.Empty);

            staticText.Translate(translationMock.TranslationInfo.CultureInfo);
            Assert.That(staticText.BodyTranslation, Is.Not.Null);
            Assert.That(staticText.BodyTranslation, Is.EqualTo(translationMock));
        }

        /// <summary>
        /// Tests that the Translate sets the translation for the body to null when the translation identifier for the body is not null and translation was not found.
        /// </summary>
        [Test]
        public void TestThatTranslateSetsBodyTranslationToNullWhenBodyTranslationIdentifierIsNotNullAndTranslationWasNotFound()
        {
            var fixture = new Fixture();
            var bodyTranslationIdentifier = Guid.NewGuid();

            var staticText = new StaticText(fixture.Create<StaticTextType>(), Guid.NewGuid(), bodyTranslationIdentifier);
            Assert.That(staticText, Is.Not.Null);
            Assert.That(staticText.BodyTranslationIdentifier, Is.Not.Null);
            Assert.That(staticText.BodyTranslationIdentifier.HasValue, Is.True);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(staticText.BodyTranslationIdentifier.Value, Is.EqualTo(bodyTranslationIdentifier));
            // ReSharper restore PossibleInvalidOperationException
            Assert.That(staticText.BodyTranslation, Is.Null);
            Assert.That(staticText.BodyTranslations, Is.Not.Null);
            Assert.That(staticText.BodyTranslations, Is.Empty);

            var translationMock = DomainObjectMockBuilder.BuildTranslationMock(Guid.NewGuid());
            staticText.TranslationAdd(translationMock);
            Assert.That(staticText.BodyTranslation, Is.Null);
            Assert.That(staticText.BodyTranslations, Is.Not.Null);
            Assert.That(staticText.BodyTranslations, Is.Empty);

            staticText.Translate(translationMock.TranslationInfo.CultureInfo);
            Assert.That(staticText.BodyTranslation, Is.Null);
        }
    }
}
