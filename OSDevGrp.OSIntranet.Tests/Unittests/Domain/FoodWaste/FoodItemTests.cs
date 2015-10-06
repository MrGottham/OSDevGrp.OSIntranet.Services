using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Castle.Core.Internal;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste
{
    /// <summary>
    /// Tests the food item.
    /// </summary>
    [TestFixture]
    public class FoodItemTests
    {
        /// <summary>
        /// Private class used to test the food item.
        /// </summary>
        private class MyFoodItem : FoodItem
        {
            #region Properties

            /// <summary>
            /// Gets or sets the primary food group for the food item.
            /// </summary>
            public new IFoodGroup PrimaryFoodGroup
            {
                get { return base.PrimaryFoodGroup; }
                set { base.PrimaryFoodGroup = value; }
            }

            /// <summary>
            /// Gets the food groups which this food item belong to.
            /// </summary>
            public new IEnumerable<IFoodGroup> FoodGroups
            {
                get { return base.FoodGroups; }
                set { base.FoodGroups = value; }
            }

            /// <summary>
            /// Gets or sets the foreign keys for the food item.
            /// </summary>
            public new IEnumerable<IForeignKey> ForeignKeys
            {
                get { return base.ForeignKeys; }
                set { base.ForeignKeys = value; }
            }

            #endregion
        }

        /// <summary>
        /// Tests that the constructor initialize a food item.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeFoodItem()
        {
            var primaryFoodGroup = DomainObjectMockBuilder.BuildFoodGroupMock();

            var foodItem = new FoodItem(primaryFoodGroup);
            Assert.That(foodItem, Is.Not.Null);
            Assert.That(foodItem.Identifier, Is.Null);
            Assert.That(foodItem.Identifier.HasValue, Is.False);
            Assert.That(foodItem.PrimaryFoodGroup, Is.Not.Null);
            Assert.That(foodItem.PrimaryFoodGroup, Is.EqualTo(primaryFoodGroup));
            Assert.That(foodItem.IsActive, Is.False);
            Assert.That(foodItem.FoodGroups, Is.Not.Null);
            Assert.That(foodItem.FoodGroups, Is.Not.Empty);
            Assert.That(foodItem.FoodGroups.Count(), Is.EqualTo(1));
            Assert.That(foodItem.FoodGroups.Contains(primaryFoodGroup), Is.True);
            Assert.That(foodItem.Translation, Is.Null);
            Assert.That(foodItem.Translations, Is.Not.Null);
            Assert.That(foodItem.Translations, Is.Empty);
            Assert.That(foodItem.ForeignKeys, Is.Not.Null);
            Assert.That(foodItem.ForeignKeys, Is.Empty);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the primary food group is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenFoodGroupIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new FoodItem(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("primaryFoodGroup"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Test that the setter of PrimaryFoodGroup throws an ArgumentNullException when the value is null.
        /// </summary>
        [Test]
        public void TestThatPrimaryFoodGroupSetterThrowsArgumentNullExceptionWhenValueIsNull()
        {
            var foodItem = new MyFoodItem();
            Assert.That(foodItem, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodItem.PrimaryFoodGroup = null);
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("value"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Test that the setter of PrimaryFoodGroup sets the primary food group.
        /// </summary>
        [Test]
        public void TestThatPrimaryFoodGroupSetterSetsPrimaryFoodGroup()
        {
            var foodItem = new MyFoodItem();
            Assert.That(foodItem, Is.Not.Null);

            var primaryFoodGroup = DomainObjectMockBuilder.BuildFoodGroupMock();
            Assert.That(primaryFoodGroup, Is.Not.Null);

            foodItem.PrimaryFoodGroup = primaryFoodGroup;
            Assert.That(foodItem.PrimaryFoodGroup, Is.Not.Null);
            Assert.That(foodItem.PrimaryFoodGroup, Is.EqualTo(primaryFoodGroup));

            Assert.That(foodItem.FoodGroups, Is.Not.Null);
            Assert.That(foodItem.FoodGroups, Is.Not.Empty);
            Assert.That(foodItem.FoodGroups.Count(), Is.EqualTo(1));
            Assert.That(foodItem.FoodGroups.Contains(primaryFoodGroup), Is.True);
        }

        /// <summary>
        /// Test that the setter of IsActive sets whether the food item is active.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatIsActiveSetterSetsIsActive(bool testValue)
        {
            var foodItem = new FoodItem(DomainObjectMockBuilder.BuildFoodGroupMock())
            {
                IsActive = !testValue
            };
            Assert.That(foodItem, Is.Not.Null);
            Assert.That(foodItem.IsActive, Is.Not.EqualTo(testValue));

            foodItem.IsActive = testValue;
            Assert.That(foodItem.IsActive, Is.EqualTo(testValue));
        }

        /// <summary>
        /// Test that the setter of FoodGroups throws an ArgumentNullException when the value is null.
        /// </summary>
        [Test]
        public void TestThatFoodGroupsSetterThrowsArgumentNullExceptionWhenValueIsNull()
        {
            var foodItem = new MyFoodItem();
            Assert.That(foodItem, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodItem.FoodGroups = null);
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("value"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Test that the setter of FoodGroups sets the food groups which this food item belong to when the primary food group is null.
        /// </summary>
        [Test]
        public void TestThatFoodGroupsSetterSetsFoodGroupsWhenPrimaryFoodGroupIsNull()
        {
            var foodItem = new MyFoodItem();
            Assert.That(foodItem, Is.Not.Null);
            Assert.That(foodItem.PrimaryFoodGroup, Is.Null);
            Assert.That(foodItem.FoodGroups, Is.Not.Null);
            Assert.That(foodItem.FoodGroups, Is.Empty);

            var foodGroupMockCollection = DomainObjectMockBuilder.BuildFoodGroupMockCollection().ToArray();
            foodItem.FoodGroups = foodGroupMockCollection;
            Assert.That(foodItem.PrimaryFoodGroup, Is.Not.Null);
            Assert.That(foodItem.PrimaryFoodGroup, Is.EqualTo(foodGroupMockCollection.First()));
            Assert.That(foodItem.FoodGroups, Is.Not.Null);
            Assert.That(foodItem.FoodGroups, Is.Not.Empty);
            Assert.That(foodItem.FoodGroups, Is.EqualTo(foodGroupMockCollection));
        }

        /// <summary>
        /// Test that the setter of FoodGroups sets the food groups which this food item belong to when the primary food group is not null and the food group collection does not contain the primary food group.
        /// </summary>
        [Test]
        public void TestThatFoodGroupsSetterSetsFoodGroupsWhenPrimaryFoodGroupIsNotNullAndValueDoesNotContainPrimaryFoodGroup()
        {
            var primaryFoodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock();
            var foodItem = new MyFoodItem
            {
                PrimaryFoodGroup = primaryFoodGroupMock
            };
            Assert.That(foodItem, Is.Not.Null);
            Assert.That(foodItem.PrimaryFoodGroup, Is.Not.Null);
            Assert.That(foodItem.PrimaryFoodGroup, Is.EqualTo(primaryFoodGroupMock));
            Assert.That(foodItem.FoodGroups, Is.Not.Null);
            Assert.That(foodItem.FoodGroups, Is.Not.Empty);
            Assert.That(foodItem.FoodGroups.Count(), Is.EqualTo(1));
            Assert.That(foodItem.FoodGroups.Contains(primaryFoodGroupMock), Is.True);

            var foodGroupMockCollection = DomainObjectMockBuilder.BuildFoodGroupMockCollection().ToArray();
            Assert.That(foodGroupMockCollection, Is.Not.Null);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(foodGroupMockCollection.Any(foodGroupMock => foodGroupMock.Identifier.Value == primaryFoodGroupMock.Identifier.Value), Is.False);
            // ReSharper restore PossibleInvalidOperationException

            foodItem.FoodGroups = foodGroupMockCollection;
            Assert.That(foodItem.PrimaryFoodGroup, Is.Not.Null);
            Assert.That(foodItem.PrimaryFoodGroup, Is.EqualTo(primaryFoodGroupMock));
            Assert.That(foodItem.FoodGroups, Is.Not.Null);
            Assert.That(foodItem.FoodGroups, Is.Not.Empty);
            Assert.That(foodItem.FoodGroups.Count(), Is.EqualTo(1 + foodGroupMockCollection.Count()));
            Assert.That(foodItem.FoodGroups.Contains(primaryFoodGroupMock), Is.True);
            foodGroupMockCollection.ForEach(foodGroupMock => Assert.That(foodItem.FoodGroups.Contains(foodGroupMock), Is.True));
        }

        /// <summary>
        /// Test that the setter of FoodGroups sets the food groups which this food item belong to when the primary food group is not null and the food group collection does contain the primary food group.
        /// </summary>
        [Test]
        public void TestThatFoodGroupsSetterSetsFoodGroupsWhenPrimaryFoodGroupIsNotNullAndValueDoesContainPrimaryFoodGroup()
        {
            var primaryFoodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock();
            var foodItem = new MyFoodItem
            {
                PrimaryFoodGroup = primaryFoodGroupMock
            };
            Assert.That(foodItem, Is.Not.Null);
            Assert.That(foodItem.PrimaryFoodGroup, Is.Not.Null);
            Assert.That(foodItem.PrimaryFoodGroup, Is.EqualTo(primaryFoodGroupMock));
            Assert.That(foodItem.FoodGroups, Is.Not.Null);
            Assert.That(foodItem.FoodGroups, Is.Not.Empty);
            Assert.That(foodItem.FoodGroups.Count(), Is.EqualTo(1));
            Assert.That(foodItem.FoodGroups.Contains(primaryFoodGroupMock), Is.True);

            var foodGroupMockCollection = DomainObjectMockBuilder.BuildFoodGroupMockCollection().ToList();
            Assert.That(foodGroupMockCollection, Is.Not.Null);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(foodGroupMockCollection.Any(foodGroupMock => foodGroupMock.Identifier.Value == primaryFoodGroupMock.Identifier.Value), Is.False);
            // ReSharper restore PossibleInvalidOperationException
            foodGroupMockCollection.Add(primaryFoodGroupMock);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(foodGroupMockCollection.Any(foodGroupMock => foodGroupMock.Identifier.Value == primaryFoodGroupMock.Identifier.Value), Is.True);
            // ReSharper restore PossibleInvalidOperationException

            foodItem.FoodGroups = foodGroupMockCollection;
            Assert.That(foodItem.PrimaryFoodGroup, Is.Not.Null);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(foodItem.PrimaryFoodGroup, Is.EqualTo(foodGroupMockCollection.Single(foodGroupMock => foodGroupMock.Identifier.Value == primaryFoodGroupMock.Identifier.Value)));
            // ReSharper restore PossibleInvalidOperationException
            Assert.That(foodItem.FoodGroups, Is.Not.Null);
            Assert.That(foodItem.FoodGroups, Is.Not.Empty);
            Assert.That(foodItem.FoodGroups, Is.EqualTo(foodGroupMockCollection));
        }

        /// <summary>
        /// Test that the setter of ForeignKeys throws an ArgumentNullException when the value is null.
        /// </summary>
        [Test]
        public void TestThatForeignKeysSetterThrowsArgumentNullExceptionWhenValueIsNull()
        {
            var foodItem = new MyFoodItem();
            Assert.That(foodItem, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodItem.ForeignKeys = null);
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("value"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Test that the setter of ForeignKeys sets the foreign keys for the food item.
        /// </summary>
        [Test]
        public void TestThatForeignKeysSetterSetForeignKeys()
        {
            var foodItem = new MyFoodItem
            {
                Identifier = Guid.NewGuid()
            };
            Assert.That(foodItem, Is.Not.Null);
            Assert.That(foodItem.Identifier, Is.Not.Null);
            Assert.That(foodItem.Identifier.HasValue, Is.True);
            Assert.That(foodItem.ForeignKeys, Is.Not.Null);
            Assert.That(foodItem.ForeignKeys, Is.Empty);

            // ReSharper disable PossibleInvalidOperationException
            var foreignKeyMockCollection = DomainObjectMockBuilder.BuildForeignKeyMockCollection(foodItem.Identifier.Value, foodItem.GetType());
            // ReSharper restore PossibleInvalidOperationException
            Assert.That(foreignKeyMockCollection, Is.Not.Null);
            Assert.That(foreignKeyMockCollection, Is.Not.Empty);

            foodItem.ForeignKeys = foreignKeyMockCollection;
            Assert.That(foodItem.ForeignKeys, Is.Not.Null);
            Assert.That(foodItem.ForeignKeys, Is.Not.Empty);
            Assert.That(foodItem.ForeignKeys, Is.EqualTo(foreignKeyMockCollection));
        }

        /// <summary>
        /// Test that FoodGroupAdd throws an ArgumentNullException when the food group is null.
        /// </summary>
        [Test]
        public void TestThatFoodGroupAddThrowsArgumentNullExceptionWhenFoodGroupIsNull()
        {
            var foodItem = new FoodItem(DomainObjectMockBuilder.BuildFoodGroupMock());
            Assert.That(foodItem, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodItem.FoodGroupAdd(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("foodGroup"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Test that FoodGroupAdd adds a food group to the food item.
        /// </summary>
        [Test]
        public void TestThatFoodGroupAddAddsFoodGroup()
        {
            var primaryFoodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock();
            Assert.That(primaryFoodGroupMock, Is.Not.Null);
            Assert.That(primaryFoodGroupMock.Identifier, Is.Not.Null);
            Assert.That(primaryFoodGroupMock.Identifier.HasValue, Is.True);

            var foodItem = new FoodItem(primaryFoodGroupMock);
            Assert.That(foodItem, Is.Not.Null);
            Assert.That(foodItem.FoodGroups, Is.Not.Null);
            Assert.That(foodItem.FoodGroups, Is.Not.Empty);
            Assert.That(foodItem.FoodGroups.Count(), Is.EqualTo(1));
            Assert.That(foodItem.FoodGroups.Contains(primaryFoodGroupMock), Is.True);

            var foodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock();
            Assert.That(foodGroupMock, Is.Not.Null);
            Assert.That(foodGroupMock.Identifier, Is.Not.Null);
            Assert.That(foodGroupMock.Identifier.HasValue, Is.True);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(foodGroupMock.Identifier.Value, Is.Not.EqualTo(primaryFoodGroupMock.Identifier.Value));
            // ReSharper restore PossibleInvalidOperationException

            foodItem.FoodGroupAdd(foodGroupMock);
            Assert.That(foodItem.FoodGroups, Is.Not.Null);
            Assert.That(foodItem.FoodGroups, Is.Not.Empty);
            Assert.That(foodItem.FoodGroups.Count(), Is.EqualTo(2));
            Assert.That(foodItem.FoodGroups.Contains(primaryFoodGroupMock), Is.True);
            Assert.That(foodItem.FoodGroups.Contains(foodGroupMock), Is.True);
        }

        /// <summary>
        /// Test that TranslationAdd throws an ArgumentNullException when the translation is null.
        /// </summary>
        [Test]
        public void TestThatTranslationAddThrowsArgumentNullExceptionWhenTranslationIsNull()
        {
            var foodItem = new FoodItem(DomainObjectMockBuilder.BuildFoodGroupMock());
            Assert.That(foodItem, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodItem.TranslationAdd(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("translation"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Test that TranslationAdd adds a translation to the food item.
        /// </summary>
        [Test]
        public void TestThatTranslationAddAddsTranslation()
        {
            var foodItem = new FoodItem(DomainObjectMockBuilder.BuildFoodGroupMock())
            {
                Identifier = Guid.NewGuid()
            };
            Assert.That(foodItem, Is.Not.Null);
            Assert.That(foodItem.Identifier, Is.Not.Null);
            Assert.That(foodItem.Identifier.HasValue, Is.True);
            Assert.That(foodItem.Translations, Is.Not.Null);
            Assert.That(foodItem.Translations, Is.Empty);

            // ReSharper disable PossibleInvalidOperationException
            var translationMock = DomainObjectMockBuilder.BuildTranslationMock(foodItem.Identifier.Value);
            // ReSharper restore PossibleInvalidOperationException
            Assert.That(translationMock, Is.Not.Null);

            foodItem.TranslationAdd(translationMock);
            Assert.That(foodItem.Translations, Is.Not.Null);
            Assert.That(foodItem.Translations, Is.Not.Empty);
            Assert.That(foodItem.Translations.Count(), Is.EqualTo(1));
            Assert.That(foodItem.Translations.Contains(translationMock), Is.True);
        }

        /// <summary>
        /// Test that ForeignKeyAdd throws an ArgumentNullException when the foreign key is null.
        /// </summary>
        [Test]
        public void TestThatForeignKeyAddThrowsArgumentNullExceptionWhenForeignKeyIsNull()
        {
            var foodItem = new FoodItem(DomainObjectMockBuilder.BuildFoodGroupMock());
            Assert.That(foodItem, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodItem.ForeignKeyAdd(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("foreignKey"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Test that ForeignKeyAdd adds a foreign key to the food item.
        /// </summary>
        [Test]
        public void TestThatForeignKeyAddAddsForeignKey()
        {
            var foodItem = new FoodItem(DomainObjectMockBuilder.BuildFoodGroupMock())
            {
                Identifier = Guid.NewGuid()
            };
            Assert.That(foodItem, Is.Not.Null);
            Assert.That(foodItem.Identifier, Is.Not.Null);
            Assert.That(foodItem.Identifier.HasValue, Is.True);
            Assert.That(foodItem.ForeignKeys, Is.Not.Null);
            Assert.That(foodItem.ForeignKeys, Is.Empty);

            // ReSharper disable PossibleInvalidOperationException
            var foreignKeyMock = DomainObjectMockBuilder.BuildForeignKeyMock(foodItem.Identifier.Value, foodItem.GetType());
            // ReSharper restore PossibleInvalidOperationException
            Assert.That(foreignKeyMock, Is.Not.Null);

            foodItem.ForeignKeyAdd(foreignKeyMock);
            Assert.That(foodItem.ForeignKeys, Is.Not.Null);
            Assert.That(foodItem.ForeignKeys, Is.Not.Empty);
            Assert.That(foodItem.ForeignKeys.Count(), Is.EqualTo(1));
            Assert.That(foodItem.ForeignKeys.Contains(foreignKeyMock), Is.True);
        }

        /// <summary>
        /// Tests that Translate calls Translate on each food group which this food item belong to.
        /// </summary>
        [Test]
        public void TestThatTranslateCallsTranslateOnEachFoodGroup()
        {
            var foodGroupMockCollection = DomainObjectMockBuilder.BuildFoodGroupMockCollection().ToArray();
            Assert.That(foodGroupMockCollection, Is.Not.Null);
            Assert.That(foodGroupMockCollection, Is.Not.Empty);

            var foodItem = new MyFoodItem
            {
                FoodGroups = foodGroupMockCollection
            };
            Assert.That(foodItem, Is.Not.Null);
            Assert.That(foodItem.PrimaryFoodGroup, Is.Not.Null);
            Assert.That(foodItem.FoodGroups, Is.Not.Null);
            Assert.That(foodItem.FoodGroups, Is.Not.Empty);
            Assert.That(foodItem.FoodGroups, Is.EqualTo(foodGroupMockCollection));

            var cultureInfo = CultureInfo.CurrentUICulture;
            foodItem.Translate(cultureInfo);

            foodItem.PrimaryFoodGroup.AssertWasCalled(m => m.Translate(Arg<CultureInfo>.Is.Equal(cultureInfo)));
            foodGroupMockCollection.ForEach(foodGroupMock => foodGroupMock.AssertWasCalled(m => m.Translate(Arg<CultureInfo>.Is.Equal(cultureInfo))));
        }
    }
}
