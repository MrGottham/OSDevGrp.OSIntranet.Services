using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Resources;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.DataProxies.FoodWaste
{
    /// <summary>
    /// Tests the data proxy to a given static text used by the food waste domain.
    /// </summary>
    [TestFixture]
    public class StaticTextProxyTests
    {
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

        /// <summary>
        /// Tests that getter for UniqueId throws an IntranetRepositoryException when the static text has no identifier.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterThrowsIntranetRepositoryExceptionWhenStaticTextProxyHasNoIdentifier()
        {
            var staticTextProxy = new StaticTextProxy
            {
                Identifier = null
            };

            // ReSharper disable UnusedVariable
            var exception = Assert.Throws<IntranetRepositoryException>(() => { var uniqueId = staticTextProxy.UniqueId; });
            // ReSharper restore UnusedVariable
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, staticTextProxy.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that getter for UniqueId gets the unique identifier for the static text.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterGetsUniqueIdentificationForStaticTextProxy()
        {
            var staticTextProxy = new StaticTextProxy
            {
                Identifier = Guid.NewGuid()
            };

            var uniqueId = staticTextProxy.UniqueId;
            Assert.That(uniqueId, Is.Not.Null);
            Assert.That(uniqueId, Is.Not.Empty);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(uniqueId, Is.EqualTo(staticTextProxy.Identifier.Value.ToString("D").ToUpper()));
            // ReSharper restore PossibleInvalidOperationException
        }

        /// <summary>
        /// Tests that GetSqlQueryForId throws an ArgumentNullException when the given static text is null.
        /// </summary>
        [Test]
        public void TestThatGetSqlQueryForIdThrowsArgumentNullExceptionWhenStaticTextIsNull()
        {
            var staticTextProxy = new StaticTextProxy();

            // ReSharper disable UnusedVariable
            var exception = Assert.Throws<ArgumentNullException>(() => { var sqlQueryForId = staticTextProxy.GetSqlQueryForId(null); });
            // ReSharper restore UnusedVariable
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("staticText"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetSqlQueryForId throws an IntranetRepositoryException when the identifier on the given static text has no value.
        /// </summary>
        [Test]
        public void TestThatGetSqlQueryForIdThrowsIntranetRepositoryExceptionWhenIdentifierOnStaticTextHasNoValue()
        {
            var staticTextMock = MockRepository.GenerateMock<IStaticText>();
            staticTextMock.Expect(m => m.Identifier)
                .Return(null)
                .Repeat.Any();

            var staticTextProxy = new StaticTextProxy();

            // ReSharper disable UnusedVariable
            var exception = Assert.Throws<IntranetRepositoryException>(() => { var sqlQueryForId = staticTextProxy.GetSqlQueryForId(staticTextMock); });
            // ReSharper restore UnusedVariable
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, staticTextMock.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetSqlQueryForId returns the SQL statement for selecting the given static text.
        /// </summary>
        [Test]
        public void TestThatGetSqlQueryForIdReturnsSqlQueryForId()
        {
            var staticTextMock = MockRepository.GenerateMock<IStaticText>();
            staticTextMock.Expect(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var staticTextProxy = new StaticTextProxy();

            var sqlQueryForId = staticTextProxy.GetSqlQueryForId(staticTextMock);
            Assert.That(sqlQueryForId, Is.Not.Null);
            Assert.That(sqlQueryForId, Is.Not.Empty);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(sqlQueryForId, Is.EqualTo(string.Format("SELECT StaticTextIdentifier,StaticTextType,SubjectTranslationIdentifier,BodyTranslationIdentifier FROM StaticTexts WHERE StaticTextIdentifier='{0}'", staticTextMock.Identifier.Value.ToString("D").ToUpper())));
            // ReSharper restore PossibleInvalidOperationException
        }

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

    }
}
