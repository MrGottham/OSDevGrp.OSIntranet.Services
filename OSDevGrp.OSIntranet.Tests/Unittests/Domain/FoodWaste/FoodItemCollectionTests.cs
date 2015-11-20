using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste
{
    /// <summary>
    /// Tests the collection of food items.
    /// </summary>
    [TestFixture]
    public class FoodItemCollectionTests
    {
        /// <summary>
        /// Tests that the constructor initialize a collection of food items.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeFoodItemCollection()
        {
            var foodItemMockCollection = DomainObjectMockBuilder.BuildFoodItemMockCollection().ToArray();
            var dataProviderMock = DomainObjectMockBuilder.BuildDataProviderMock();

            var foodItemCollection = new FoodItemCollection(foodItemMockCollection, dataProviderMock);
            Assert.That(foodItemCollection, Is.Not.Null);
            Assert.That(foodItemCollection.DataProvider, Is.Not.Null);
            Assert.That(foodItemCollection.DataProvider, Is.EqualTo(dataProviderMock));
            foreach (var foodItemMock in foodItemMockCollection)
            {
                Assert.That(foodItemCollection.Contains(foodItemMock), Is.True);
            }
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the foods items for the collection is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenFoodItemsIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new FoodItemCollection(null, DomainObjectMockBuilder.BuildDataProviderMock()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("source"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the data provider who has provided the food items is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenDataProviderIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new FoodItemCollection(DomainObjectMockBuilder.BuildFoodItemMockCollection(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("dataProvider"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that RemoveInactiveFoodItems removes inactive food items.
        /// </summary>
        [Test]
        public void TestThatRemoveInactiveFoodItemsRemovesInactiveFoodItems()
        {
            var activeFoodItem1Mock = MockRepository.GenerateMock<IFoodItem>();
            activeFoodItem1Mock.Stub(m => m.IsActive)
                .Return(true)
                .Repeat.Any();
            var activeFoodItem2Mock = MockRepository.GenerateMock<IFoodItem>();
            activeFoodItem2Mock.Stub(m => m.IsActive)
                .Return(true)
                .Repeat.Any();
            var activeFoodItem3Mock = MockRepository.GenerateMock<IFoodItem>();
            activeFoodItem3Mock.Stub(m => m.IsActive)
                .Return(true)
                .Repeat.Any();
            var inactiveFoodItem1Mock = MockRepository.GenerateMock<IFoodItem>();
            inactiveFoodItem1Mock.Stub(m => m.IsActive)
                .Return(false)
                .Repeat.Any();
            var inactiveFoodItem2Mock = MockRepository.GenerateMock<IFoodItem>();
            inactiveFoodItem2Mock.Stub(m => m.IsActive)
                .Return(false)
                .Repeat.Any();
            var inactiveFoodItem3Mock = MockRepository.GenerateMock<IFoodItem>();
            inactiveFoodItem3Mock.Stub(m => m.IsActive)
                .Return(false)
                .Repeat.Any();

            var dataProviderMock = DomainObjectMockBuilder.BuildDataProviderMock();

            var foodItemCollection = new FoodItemCollection(new List<IFoodItem> {activeFoodItem1Mock, inactiveFoodItem1Mock, activeFoodItem2Mock, inactiveFoodItem2Mock, activeFoodItem3Mock, inactiveFoodItem3Mock}, dataProviderMock);
            Assert.That(foodItemCollection, Is.Not.Null);
            Assert.That(foodItemCollection, Is.Not.Empty);
            Assert.That(foodItemCollection.Count, Is.EqualTo(6));
            Assert.That(foodItemCollection.Contains(activeFoodItem1Mock), Is.True);
            Assert.That(foodItemCollection.Contains(activeFoodItem2Mock), Is.True);
            Assert.That(foodItemCollection.Contains(activeFoodItem3Mock), Is.True);
            Assert.That(foodItemCollection.Contains(inactiveFoodItem1Mock), Is.True);
            Assert.That(foodItemCollection.Contains(inactiveFoodItem2Mock), Is.True);
            Assert.That(foodItemCollection.Contains(inactiveFoodItem3Mock), Is.True);

            foodItemCollection.RemoveInactiveFoodItems();
            Assert.That(foodItemCollection, Is.Not.Null);
            Assert.That(foodItemCollection, Is.Not.Empty);
            Assert.That(foodItemCollection.Count, Is.EqualTo(3));
            Assert.That(foodItemCollection.Contains(activeFoodItem1Mock), Is.True);
            Assert.That(foodItemCollection.Contains(activeFoodItem2Mock), Is.True);
            Assert.That(foodItemCollection.Contains(activeFoodItem3Mock), Is.True);
            Assert.That(foodItemCollection.Contains(inactiveFoodItem1Mock), Is.False);
            Assert.That(foodItemCollection.Contains(inactiveFoodItem2Mock), Is.False);
            Assert.That(foodItemCollection.Contains(inactiveFoodItem3Mock), Is.False);
        }

        /// <summary>
        /// Tests that RemoveInactiveFoodItems calls RemoveInactiveFoodGroups on each active food item.
        /// </summary>
        [Test]
        public void TestThatRemoveInactiveFoodItemsCallsRemoveInactiveFoodGroupsOnEachActiveFoodItem()
        {
            var activeFoodItem1Mock = MockRepository.GenerateMock<IFoodItem>();
            activeFoodItem1Mock.Stub(m => m.IsActive)
                .Return(true)
                .Repeat.Any();
            var activeFoodItem2Mock = MockRepository.GenerateMock<IFoodItem>();
            activeFoodItem2Mock.Stub(m => m.IsActive)
                .Return(true)
                .Repeat.Any();
            var activeFoodItem3Mock = MockRepository.GenerateMock<IFoodItem>();
            activeFoodItem3Mock.Stub(m => m.IsActive)
                .Return(true)
                .Repeat.Any();
            var inactiveFoodItem1Mock = MockRepository.GenerateMock<IFoodItem>();
            inactiveFoodItem1Mock.Stub(m => m.IsActive)
                .Return(false)
                .Repeat.Any();
            var inactiveFoodItem2Mock = MockRepository.GenerateMock<IFoodItem>();
            inactiveFoodItem2Mock.Stub(m => m.IsActive)
                .Return(false)
                .Repeat.Any();
            var inactiveFoodItem3Mock = MockRepository.GenerateMock<IFoodItem>();
            inactiveFoodItem3Mock.Stub(m => m.IsActive)
                .Return(false)
                .Repeat.Any();

            var dataProviderMock = DomainObjectMockBuilder.BuildDataProviderMock();

            var foodItemCollection = new FoodItemCollection(new List<IFoodItem> {activeFoodItem1Mock, inactiveFoodItem1Mock, activeFoodItem2Mock, inactiveFoodItem2Mock, activeFoodItem3Mock, inactiveFoodItem3Mock}, dataProviderMock);
            Assert.That(foodItemCollection, Is.Not.Null);
            Assert.That(foodItemCollection, Is.Not.Empty);

            foodItemCollection.RemoveInactiveFoodItems();

            activeFoodItem1Mock.AssertWasCalled(m => m.RemoveInactiveFoodGroups());
            activeFoodItem2Mock.AssertWasCalled(m => m.RemoveInactiveFoodGroups());
            activeFoodItem3Mock.AssertWasCalled(m => m.RemoveInactiveFoodGroups());
        }

        /// <summary>
        /// Tests that RemoveInactiveFoodItems does not call RemoveInactiveFoodGroups on each inactive food item.
        /// </summary>
        [Test]
        public void TestThatRemoveInactiveFoodItemsDoesNotCallRemoveInactiveFoodGroupsOnEachInactiveFoodItem()
        {
            var activeFoodItem1Mock = MockRepository.GenerateMock<IFoodItem>();
            activeFoodItem1Mock.Stub(m => m.IsActive)
                .Return(true)
                .Repeat.Any();
            var activeFoodItem2Mock = MockRepository.GenerateMock<IFoodItem>();
            activeFoodItem2Mock.Stub(m => m.IsActive)
                .Return(true)
                .Repeat.Any();
            var activeFoodItem3Mock = MockRepository.GenerateMock<IFoodItem>();
            activeFoodItem3Mock.Stub(m => m.IsActive)
                .Return(true)
                .Repeat.Any();
            var inactiveFoodItem1Mock = MockRepository.GenerateMock<IFoodItem>();
            inactiveFoodItem1Mock.Stub(m => m.IsActive)
                .Return(false)
                .Repeat.Any();
            var inactiveFoodItem2Mock = MockRepository.GenerateMock<IFoodItem>();
            inactiveFoodItem2Mock.Stub(m => m.IsActive)
                .Return(false)
                .Repeat.Any();
            var inactiveFoodItem3Mock = MockRepository.GenerateMock<IFoodItem>();
            inactiveFoodItem3Mock.Stub(m => m.IsActive)
                .Return(false)
                .Repeat.Any();

            var dataProviderMock = DomainObjectMockBuilder.BuildDataProviderMock();

            var foodItemCollection = new FoodItemCollection(new List<IFoodItem> {activeFoodItem1Mock, inactiveFoodItem1Mock, activeFoodItem2Mock, inactiveFoodItem2Mock, activeFoodItem3Mock, inactiveFoodItem3Mock}, dataProviderMock);
            Assert.That(foodItemCollection, Is.Not.Null);
            Assert.That(foodItemCollection, Is.Not.Empty);

            foodItemCollection.RemoveInactiveFoodItems();

            inactiveFoodItem1Mock.AssertWasNotCalled(m => m.RemoveInactiveFoodGroups());
            inactiveFoodItem2Mock.AssertWasNotCalled(m => m.RemoveInactiveFoodGroups());
            inactiveFoodItem3Mock.AssertWasNotCalled(m => m.RemoveInactiveFoodGroups());
        }
    }
}
