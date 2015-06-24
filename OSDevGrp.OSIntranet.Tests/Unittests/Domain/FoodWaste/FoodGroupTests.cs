using System;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;

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
            Assert.That(foodGroup.Parent, Is.Null);
            Assert.That(foodGroup.Translation, Is.Null);
            Assert.That(foodGroup.Translations, Is.Not.Null);
            Assert.That(foodGroup.Translations, Is.Empty);
            Assert.That(foodGroup.ForeignKeys, Is.Not.Null);
            Assert.That(foodGroup.ForeignKeys, Is.Empty);
        }

        /// <summary>
        /// Tests that the setter to Parent sets new value which are not null.
        /// </summary>
        [Test]
        public void TestThatParentSetterSetsValueWhichAreNotNull()
        {
            var foodGroup = new FoodGroup();
            Assert.That(foodGroup, Is.Not.Null);
            Assert.That(foodGroup.Parent, Is.Null);

            var parentFoodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock();

            foodGroup.Parent = parentFoodGroupMock;
            Assert.That(foodGroup.Parent, Is.Not.Null);
            Assert.That(foodGroup.Parent, Is.EqualTo(parentFoodGroupMock));
        }

        /// <summary>
        /// Tests that the setter to Parent sets new value which are null.
        /// </summary>
        [Test]
        public void TestThatParentSetterSetsValueWhichAreNull()
        {
            var foodGroup = new FoodGroup
            {
                Parent = DomainObjectMockBuilder.BuildFoodGroupMock()
            };
            Assert.That(foodGroup, Is.Not.Null);
            Assert.That(foodGroup.Parent, Is.Not.Null);

            foodGroup.Parent = null;
            Assert.That(foodGroup.Parent, Is.Null);
        }

        /// <summary>
        /// Tests that the setter to Parent throws an IntranetSystemException when the new value has no identifier.
        /// </summary>
        [Test]
        public void TestThatParentSetterThrowsIntranetSystemExceptionWhenValueHasNoIdentifier()
        {
            var foodGroup = new FoodGroup();
            Assert.That(foodGroup, Is.Not.Null);
            Assert.That(foodGroup.Parent, Is.Null);

            var exception = Assert.Throws<IntranetSystemException>(() => foodGroup.Parent = new FoodGroup {Identifier = null});
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(""));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Test that the setter to Parent throws IntranetSystemException when new value equals the food group which updates Parent.
        /// </summary>
        [Test]
        public void TestThatParentSetterThrowsIntranetSystemExceptionWhenValueEqualsFoodGroupWhichUpdatesParent()
        {
            var foodGroup = new FoodGroup
            {
                Parent = DomainObjectMockBuilder.BuildFoodGroupMock()
            };
            Assert.That(foodGroup, Is.Not.Null);
            Assert.That(foodGroup.Parent, Is.Not.Null);
        }

        /// <summary>
        /// Tests that TranslationAdd adds a translation.
        /// </summary>
        [Test]
        public void TestThatTranslationAddAddsTranslation()
        {
            var foodGroup = new FoodGroup
            {
                Identifier = Guid.NewGuid()
            };
            Assert.That(foodGroup, Is.Not.Null);
            Assert.That(foodGroup.Translations, Is.Not.Null);
            Assert.That(foodGroup.Translations, Is.Empty);
            
            // ReSharper disable PossibleInvalidOperationException
            var translationMock = DomainObjectMockBuilder.BuildTranslationMock(foodGroup.Identifier.Value);
            // ReSharper restore PossibleInvalidOperationException

            foodGroup.TranslationAdd(translationMock);
            Assert.That(foodGroup.Translations, Is.Not.Null);
            Assert.That(foodGroup.Translations, Is.Not.Empty);
            Assert.That(foodGroup.Translations.Count(), Is.EqualTo(1));
            Assert.That(foodGroup.Translations.Contains(translationMock), Is.True);
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
        /// Tests that ForeignKeyAdd adds a foreign key.
        /// </summary>
        [Test]
        public void TestThatForeignKeyAddAddsForeignKey()
        {
            var foodGroup = new FoodGroup
            {
                Identifier = Guid.NewGuid()
            };
            Assert.That(foodGroup, Is.Not.Null);
            Assert.That(foodGroup.ForeignKeys, Is.Not.Null);
            Assert.That(foodGroup.ForeignKeys, Is.Empty);

            // ReSharper disable PossibleInvalidOperationException
            var foreignKeyMock = DomainObjectMockBuilder.BuildForeignKeyMock(foodGroup.Identifier.Value, foodGroup.GetType());
            // ReSharper restore PossibleInvalidOperationException

            foodGroup.ForeignKeyAdd(foreignKeyMock);
            Assert.That(foodGroup.ForeignKeys, Is.Not.Null);
            Assert.That(foodGroup.ForeignKeys, Is.Not.Empty);
            Assert.That(foodGroup.ForeignKeys.Count(), Is.EqualTo(1));
            Assert.That(foodGroup.ForeignKeys.Contains(foreignKeyMock), Is.True);
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
