using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste
{
    /// <summary>
    /// Tests the data provider.
    /// </summary>
    [TestFixture]
    public class DataProviderTests
    {
        /// <summary>
        /// Tests that the contructor initialize a data provider.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeDataProvider()
        {
            var fixture = new Fixture();
            var name = fixture.Create<string>();
            var dataSourceStatementIdentifier = Guid.NewGuid();

            var dataProvider = new DataProvider(name, dataSourceStatementIdentifier);
            Assert.That(dataProvider, Is.Not.Null);
            Assert.That(dataProvider.Identifier, Is.Null);
            Assert.That(dataProvider.Identifier.HasValue, Is.False);
            Assert.That(dataProvider.Translation, Is.Null);
            Assert.That(dataProvider.Translations, Is.Not.Null);
            Assert.That(dataProvider.Translations, Is.Empty);
            Assert.That(dataProvider.Name, Is.Not.Null);
            Assert.That(dataProvider.Name, Is.Not.Empty);
            Assert.That(dataProvider.Name, Is.EqualTo(name));
            Assert.That(dataProvider.DataSourceStatementIdentifier, Is.EqualTo(dataSourceStatementIdentifier));
            Assert.That(dataProvider.DataSourceStatement, Is.Null);
            Assert.That(dataProvider.DataSourceStatements, Is.Not.Null);
            Assert.That(dataProvider.DataSourceStatements, Is.Empty);
        }

        /// <summary>
        /// Tests that the constructor throw an ArgumentNullException when the name is illegal.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestThatConstructorThrowsArgumentNullExceptionForIllegalName(string illegalValue)
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new DataProvider(illegalValue, Guid.NewGuid()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("name"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Translate sets the translation for the data source statement.
        /// </summary>
        [Test]
        public void TestThatTranslateSetsDataSourceStatement()
        {
            var fixture = new Fixture();
            var dataSourceStatementIdentifier = Guid.NewGuid();

            var dataProvider = new DataProvider(fixture.Create<string>(), dataSourceStatementIdentifier);
            Assert.That(dataProvider, Is.Not.Null);
            Assert.That(dataProvider.DataSourceStatementIdentifier, Is.EqualTo(dataSourceStatementIdentifier));
            Assert.That(dataProvider.DataSourceStatement, Is.Null);
            Assert.That(dataProvider.DataSourceStatements, Is.Not.Null);
            Assert.That(dataProvider.DataSourceStatements, Is.Empty);

            var translationMock = DomainObjectMockBuilder.BuildTranslationMock(dataSourceStatementIdentifier);
            dataProvider.TranslationAdd(translationMock);
            Assert.That(dataProvider.DataSourceStatementIdentifier, Is.EqualTo(dataSourceStatementIdentifier));
            Assert.That(dataProvider.DataSourceStatement, Is.Null);
            Assert.That(dataProvider.DataSourceStatements, Is.Not.Null);
            Assert.That(dataProvider.DataSourceStatements, Is.Not.Empty);

            dataProvider.Translate(translationMock.TranslationInfo.CultureInfo);
            Assert.That(dataProvider.DataSourceStatement, Is.Not.Null);
            Assert.That(dataProvider.DataSourceStatement, Is.EqualTo(translationMock));
        }
    }
}
