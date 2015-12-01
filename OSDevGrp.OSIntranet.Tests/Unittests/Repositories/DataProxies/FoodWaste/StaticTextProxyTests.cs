using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.DataProxies.FoodWaste
{
    /// <summary>
    /// Tests the data proxy to a given static text used by the food waste domain.
    /// </summary>
    [TestFixture]
    public class StaticTextProxyTests
    {
        /*
CREATE TABLE IF NOT EXISTS StaticTexts (
	StaticTextIdentifier CHAR(36) NOT NULL,
	StaticTextType TINYINT NOT NULL,
	SubjectTranslationIdentifier CHAR(36) NOT NULL,
	BodyTranslationIdentifier CHAR(36) NULL,
	PRIMARY KEY (StaticTextIdentifier),
	UNIQUE INDEX IX_StaticTexts_StaticTextType (StaticTextType)
);

         */

        /// <summary>
        /// Tests that the constructor initialize a data proxy to a given static text used by the food waste domain.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeStaticTextProxy()
        {
            var staticTextProxy = new StaticTextProxy();
            Assert.That(staticTextProxy, Is.Not.Null);
            Assert.That(staticTextProxy.Identifier, Is.Null);
            Assert.That(staticTextProxy.Identifier.HasValue, Is.False);
            Assert.That(staticTextProxy.Translation, Is.Null);
            Assert.That(staticTextProxy.Translations, Is.Not.Null);
            Assert.That(staticTextProxy.Translations, Is.Empty);
            Assert.That(staticTextProxy.SubjectTranslationIdentifier, Is.EqualTo(default(Guid)));
            Assert.That(staticTextProxy.SubjectTranslation, Is.Null);
            Assert.That(staticTextProxy.SubjectTranslations, Is.Not.Null);
            Assert.That(staticTextProxy.SubjectTranslations, Is.Empty);
            Assert.That(staticTextProxy.BodyTranslationIdentifier, Is.Null);
            Assert.That(staticTextProxy.BodyTranslationIdentifier.HasValue, Is.False);
            Assert.That(staticTextProxy.SubjectTranslation, Is.Null);
            Assert.That(staticTextProxy.SubjectTranslations, Is.Not.Null);
            Assert.That(staticTextProxy.SubjectTranslations, Is.Empty);
        }
    }
}
