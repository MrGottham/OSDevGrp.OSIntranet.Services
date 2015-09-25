using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;

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
        /// Test that the setter of IsActive whether the food item is active.
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
    }
}
