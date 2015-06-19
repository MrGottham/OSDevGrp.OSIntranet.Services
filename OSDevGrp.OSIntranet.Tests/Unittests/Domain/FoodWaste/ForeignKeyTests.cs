using System;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste
{
    /// <summary>
    /// Tests the foreign key for a domain object in the food waste domain.
    /// </summary>
    [TestFixture]
    public class ForeignKeyTests
    {
        /// <summary>
        /// Tests that the constructor initialize a foreign key for a domain object in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeForeignKey()
        {
            var fixture = new Fixture();

            var dataProviderMock = DomainObjectMockBuilder.BuildDataProviderMock();
            var foreignKeyForIdentifier = Guid.NewGuid();
            var foreignKeyForType = typeof (DataProvider);
            var foreignKeyValue = fixture.Create<string>();
            var foreignKey = new ForeignKey(dataProviderMock, foreignKeyForIdentifier, foreignKeyForType, foreignKeyValue);
            Assert.That(foreignKey, Is.Not.Null);
            Assert.That(foreignKey.Identifier, Is.Null);
            Assert.That(foreignKey.Identifier.HasValue, Is.False);
            Assert.That(foreignKey.DataProvider, Is.Not.Null);
            Assert.That(foreignKey.DataProvider, Is.EqualTo(dataProviderMock));
            Assert.That(foreignKey.ForeignKeyForIdentifier, Is.EqualTo(foreignKeyForIdentifier));
            Assert.That(foreignKey.ForeignKeyForTypes, Is.Not.Null);
            Assert.That(foreignKey.ForeignKeyForTypes, Is.Not.Empty);
            Assert.That(foreignKey.ForeignKeyForTypes.Count(), Is.EqualTo(4));
            Assert.That(foreignKey.ForeignKeyForTypes.Contains(typeof (IDomainObject)), Is.True);
            Assert.That(foreignKey.ForeignKeyForTypes.Contains(typeof (IIdentifiable)), Is.True);
            Assert.That(foreignKey.ForeignKeyForTypes.Contains(typeof (ITranslatable)), Is.True);
            Assert.That(foreignKey.ForeignKeyForTypes.Contains(typeof (IDataProvider)), Is.True);
            Assert.That(foreignKey.ForeignKeyValue, Is.Not.Null);
            Assert.That(foreignKey.ForeignKeyValue, Is.Not.Empty);
            Assert.That(foreignKey.ForeignKeyValue, Is.EqualTo(foreignKeyValue));
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the data provider is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenDataProviderIsNull()
        {
            var fixture = new Fixture();

            var exception = Assert.Throws<ArgumentNullException>(() => new ForeignKey(null, Guid.NewGuid(), typeof (DataProvider), fixture.Create<string>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("dataProvider"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the type which has this foreign key is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenForeignKeyForTypeIsNull()
        {
            var fixture = new Fixture();

            var exception = Assert.Throws<ArgumentNullException>(() => new ForeignKey(DomainObjectMockBuilder.BuildDataProviderMock(), Guid.NewGuid(), null, fixture.Create<string>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("foreignKeyForType"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the value of the foreign key value is invalid.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenForeignKeyValueIsInvalid(string invalidValue)
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ForeignKey(DomainObjectMockBuilder.BuildDataProviderMock(), Guid.NewGuid(), typeof (DataProvider), invalidValue));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("foreignKeyValue"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Test that the setter for ForeignKeyValue updates value for the foreign key value.
        /// </summary>
        [Test]
        public void TestThatForeignKeyValueSetterUpdatesValue()
        {
            var fixture = new Fixture();

            var foreignKey = new ForeignKey(DomainObjectMockBuilder.BuildDataProviderMock(), Guid.NewGuid(), typeof(DataProvider), fixture.Create<string>());
            Assert.That(foreignKey, Is.Not.Null);

            var newValue = fixture.Create<string>();
            Assert.That(foreignKey.ForeignKeyValue, Is.Not.Null);
            Assert.That(foreignKey.ForeignKeyValue, Is.Not.Empty);
            Assert.That(foreignKey.ForeignKeyValue, Is.Not.EqualTo(newValue));

            foreignKey.ForeignKeyValue = newValue;
            Assert.That(foreignKey.ForeignKeyValue, Is.Not.Null);
            Assert.That(foreignKey.ForeignKeyValue, Is.Not.Empty);
            Assert.That(foreignKey.ForeignKeyValue, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Test that the setter for ForeignKeyValue throws an ArgumentNullException when the value of the foreign key value is invalid.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestThatForeignKeyValueSetterThrowsArgumentNullExceptionWhenForeignKeyValueIsInvalid(string invalidValue)
        {
            var fixture = new Fixture();

            var foreignKey = new ForeignKey(DomainObjectMockBuilder.BuildDataProviderMock(), Guid.NewGuid(), typeof(DataProvider), fixture.Create<string>());
            Assert.That(foreignKey, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foreignKey.ForeignKeyValue = invalidValue);
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("value"));
            Assert.That(exception.InnerException, Is.Null);
        }
    }
}
