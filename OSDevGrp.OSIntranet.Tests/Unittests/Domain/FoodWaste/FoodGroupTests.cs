using System;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Resources;

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
            Assert.That(foodGroup.IsActive, Is.False);
            Assert.That(foodGroup.Children, Is.Not.Null);
            Assert.That(foodGroup.Children, Is.Empty);
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
        /// Tests that the setter to Parent throws an ArgumentException when the new value has no identifier.
        /// </summary>
        [Test]
        public void TestThatParentSetterThrowsArgumentExceptionWhenValueHasNoIdentifier()
        {
            var foodGroup = new FoodGroup();
            Assert.That(foodGroup, Is.Not.Null);
            Assert.That(foodGroup.Parent, Is.Null);

            var exception = Assert.Throws<ArgumentException>(() => foodGroup.Parent = new FoodGroup {Identifier = null});
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("value"));
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            // ReSharper disable NotResolvedInText
            Assert.That(exception.Message, Is.EqualTo((new ArgumentException(Resource.GetExceptionMessage(ExceptionMessage.ValueMustBeGivenForProperty, "Identifier"), "value")).Message));
            // ReSharper restore NotResolvedInText
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the setter to Parent throws an ArgumentException when the new value equels the food group which updates Parent.
        /// </summary>
        [Test]
        public void TestThatParentSetterThrowsArgumentExceptionWhenValueEqualsFoodGroupWhichUpdatesParent()
        {
            var foodGroup = new FoodGroup
            {
                Identifier = Guid.NewGuid()
            };
            Assert.That(foodGroup, Is.Not.Null);
            Assert.That(foodGroup.Identifier, Is.Not.Null);
            Assert.That(foodGroup.Identifier.HasValue, Is.True);
            Assert.That(foodGroup.Parent, Is.Null);

            var exception = Assert.Throws<ArgumentException>(() => foodGroup.Parent = foodGroup);
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("value"));
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            // ReSharper disable NotResolvedInText
            Assert.That(exception.Message, Is.EqualTo((new ArgumentException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, foodGroup, "value"), "value")).Message));
            // ReSharper restore NotResolvedInText
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the setter to Parent throws an ArgumentException when the new value makes a circular reference.
        /// </summary>
        [Test]
        public void TestThatParentSetterThrowsArgumentExceptionWhenValueMakesCurcularReference()
        {
            var foodGroupLevel1 = new FoodGroup
            {
                Identifier = Guid.NewGuid()
            };
            Assert.That(foodGroupLevel1, Is.Not.Null);
            Assert.That(foodGroupLevel1.Identifier, Is.Not.Null);
            Assert.That(foodGroupLevel1.Identifier.HasValue, Is.True);
            Assert.That(foodGroupLevel1.Parent, Is.Null);

            var foodGroupLevel2 = new FoodGroup
            {
                Identifier = Guid.NewGuid(),
                Parent = foodGroupLevel1
            };
            Assert.That(foodGroupLevel2, Is.Not.Null);
            Assert.That(foodGroupLevel2.Identifier, Is.Not.Null);
            Assert.That(foodGroupLevel2.Identifier.HasValue, Is.True);
            Assert.That(foodGroupLevel2.Parent, Is.Not.Null);
            Assert.That(foodGroupLevel2.Parent, Is.EqualTo(foodGroupLevel1));

            var foodGroupLevel3 = new FoodGroup
            {
                Identifier = Guid.NewGuid(),
                Parent = foodGroupLevel2
            };
            Assert.That(foodGroupLevel3, Is.Not.Null);
            Assert.That(foodGroupLevel3.Identifier, Is.Not.Null);
            Assert.That(foodGroupLevel3.Identifier.HasValue, Is.True);
            Assert.That(foodGroupLevel3.Parent, Is.Not.Null);
            Assert.That(foodGroupLevel3.Parent, Is.EqualTo(foodGroupLevel2));


            var exception = Assert.Throws<ArgumentException>(() => foodGroupLevel1.Parent = foodGroupLevel3);
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("value"));
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            // ReSharper disable NotResolvedInText
            Assert.That(exception.Message, Is.EqualTo((new ArgumentException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, foodGroupLevel3, "value"), "value").Message)));
            // ReSharper restore NotResolvedInText
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the setter to IsActive sets new value to true.
        /// </summary>
        [Test]
        public void TestThatIsActiveSetterSetsValueToTrue()
        {
            var foodGroup = new FoodGroup
            {
                IsActive = false
            };
            Assert.That(foodGroup, Is.Not.Null);
            Assert.That(foodGroup.IsActive, Is.False);

            foodGroup.IsActive = true;
            Assert.That(foodGroup.IsActive, Is.True);
        }

        /// <summary>
        /// Tests that the setter to IsActive sets new value to false.
        /// </summary>
        [Test]
        public void TestThatIsActiveSetterSetsValueToFalse()
        {
            var foodGroup = new FoodGroup
            {
                IsActive = true
            };
            Assert.That(foodGroup, Is.Not.Null);
            Assert.That(foodGroup.IsActive, Is.True);

            foodGroup.IsActive = false;
            Assert.That(foodGroup.IsActive, Is.False);
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
