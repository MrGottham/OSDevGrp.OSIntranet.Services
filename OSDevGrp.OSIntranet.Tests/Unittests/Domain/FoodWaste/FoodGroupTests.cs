using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.FoodWaste;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste
{
    /// <summary>
    /// Tests the food group.
    /// </summary>
    [TestFixture]
    public class FoodGroupTests
    {
        /// <summary>
        /// Tests that the constructor initialize a food group.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeForeignKey()
        {
            var foodGroup = new FoodGroup();
            Assert.That(foodGroup, Is.Not.Null);
            Assert.That(foodGroup.Identifier, Is.Null);
            Assert.That(foodGroup.Identifier.HasValue, Is.False);
            Assert.That(foodGroup.Translation, Is.Null);
            Assert.That(foodGroup.Translations, Is.Not.Null);
            Assert.That(foodGroup.Translations, Is.Empty);
            Assert.That(foodGroup.ForeignKeys, Is.Not.Null);
            Assert.That(foodGroup.ForeignKeys, Is.Empty);
        }

        /// <summary>
        /// Tests that TranslationAdd throws an ArgumentNullException when the translation if null.
        /// </summary>
        [Test]
        public void TestThatTranslationAddThrowsArgumentNullExceptionWhenTranslationIsNull()
        {
            var foodGroup = new FoodGroup();
            Assert.That(foodGroup, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodGroup.TranslationAdd(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("translation"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that ForeignKeyAdd throws an ArgumentNullException when the foreign key if null.
        /// </summary>
        [Test]
        public void TestThatForeignKeyAddThrowsArgumentNullExceptionWhenForeignKeyIsNull()
        {
            var foodGroup = new FoodGroup();
            Assert.That(foodGroup, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodGroup.ForeignKeyAdd(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("foreignKey"));
            Assert.That(exception.InnerException, Is.Null);
        }
    }
}
