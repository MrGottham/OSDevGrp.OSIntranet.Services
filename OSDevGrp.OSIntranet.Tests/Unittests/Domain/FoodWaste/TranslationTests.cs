using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste
{
    /// <summary>
    /// Tests the translation domain objet.
    /// </summary>
    [TestFixture]
    public class TranslationTests
    {
        /// <summary>
        /// Tests that the constructor initialize a translation object.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeTranslation()
        {
            var fixture = new Fixture();
            var translationOfIdentitier = Guid.NewGuid();
            var translationInfoMock = DomainObjectMockBuilder.BuildTranslationInfoMock();
            var value = fixture.Create<string>();

            var translation = new Translation(translationOfIdentitier, translationInfoMock, value);
            Assert.That(translation, Is.Not.Null);
            Assert.That(translation.Identifier, Is.Null);
            Assert.That(translation.Identifier.HasValue, Is.False);
            Assert.That(translation.TranslationOfIdentifier, Is.EqualTo(translationOfIdentitier));
            Assert.That(translation.TranslationInfo, Is.Not.Null);
            Assert.That(translation.TranslationInfo, Is.EqualTo(translationInfoMock));
            Assert.That(translation.Value, Is.Not.Null);
            Assert.That(translation.Value, Is.Not.Empty);
            Assert.That(translation.Value, Is.EqualTo(value));
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the translation informations is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenTranslationInfoIsNull()
        {
            var fixture = new Fixture();

            var exception = Assert.Throws<ArgumentNullException>(() => new Translation(Guid.NewGuid(), null, fixture.Create<string>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("translationInfo"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when value is invalid.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenValueIsInvalid(string invalidValue)
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new Translation(Guid.NewGuid(), DomainObjectMockBuilder.BuildTranslationInfoMock(), invalidValue));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("value"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the setter for Value updates value for the translation.
        /// </summary>
        [Test]
        public void TestThatValueSetterUpdatesValue()
        {
            var fixture = new Fixture();
            var translation = new Translation(Guid.NewGuid(), DomainObjectMockBuilder.BuildTranslationInfoMock(), fixture.Create<string>());

            var newValue = fixture.Create<string>();
            translation.Value = newValue;
            Assert.That(translation.Value, Is.Not.Null);
            Assert.That(translation.Value, Is.Not.Empty);
            Assert.That(translation.Value, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter for Value throws an ArgumentNullException when value is invalid.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestThatValueSetterThrowsArgumentNullExceptionWhenValueIsInvalid(string invalidValue)
        {
            var fixture = new Fixture();
            var translation = new Translation(Guid.NewGuid(), DomainObjectMockBuilder.BuildTranslationInfoMock(), fixture.Create<string>());

            var exception = Assert.Throws<ArgumentNullException>(() => translation.Value = invalidValue);
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("value"));
            Assert.That(exception.InnerException, Is.Null);
        }
    }
}
