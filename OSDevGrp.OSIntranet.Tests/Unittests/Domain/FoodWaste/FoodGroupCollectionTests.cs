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
    /// Tests the collection of food groups.
    /// </summary>
    [TestFixture]
    public class FoodGroupCollectionTests
    {
        /// <summary>
        /// Tests that the constructor initialize a collection of food groups.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeFoodGroupCollection()
        {
            var foodGroupMockCollection = DomainObjectMockBuilder.BuildFoodGroupMockCollection().ToArray();
            var dataProviderMock = DomainObjectMockBuilder.BuildDataProviderMock();

            var foodGroupCollection = new FoodGroupCollection(foodGroupMockCollection, dataProviderMock);
            Assert.That(foodGroupCollection, Is.Not.Null);
            Assert.That(foodGroupCollection.DataProvider, Is.Not.Null);
            Assert.That(foodGroupCollection.DataProvider, Is.EqualTo(dataProviderMock));
            foreach (var foodGroupMock in foodGroupMockCollection)
            {
                Assert.That(foodGroupCollection.Contains(foodGroupMock), Is.True);
            }
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the foods groups for the collection is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenFoodGroupsIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new FoodGroupCollection(null, DomainObjectMockBuilder.BuildDataProviderMock()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("source"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the data provider who has provided the food groups is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenDataProviderIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new FoodGroupCollection(DomainObjectMockBuilder.BuildFoodGroupMockCollection(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("dataProvider"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that RemoveInactiveFoodGroups removes inactive food groups.
        /// </summary>
        [Test]
        public void TestThatRemoveInactiveFoodGroupsRemovesInactiveFoodGroups()
        {
            var activeFoodGroup1 = MockRepository.GenerateMock<IFoodGroup>();
            activeFoodGroup1.Stub(m => m.IsActive)
                .Return(true)
                .Repeat.Any();
            var activeFoodGroup2 = MockRepository.GenerateMock<IFoodGroup>();
            activeFoodGroup2.Stub(m => m.IsActive)
                .Return(true)
                .Repeat.Any();
            var activeFoodGroup3 = MockRepository.GenerateMock<IFoodGroup>();
            activeFoodGroup3.Stub(m => m.IsActive)
                .Return(true)
                .Repeat.Any();
            var inactiveFoodGroup1 = MockRepository.GenerateMock<IFoodGroup>();
            inactiveFoodGroup1.Stub(m => m.IsActive)
                .Return(false)
                .Repeat.Any();
            var inactiveFoodGroup2 = MockRepository.GenerateMock<IFoodGroup>();
            inactiveFoodGroup2.Stub(m => m.IsActive)
                .Return(false)
                .Repeat.Any();
            var inactiveFoodGroup3 = MockRepository.GenerateMock<IFoodGroup>();
            inactiveFoodGroup3.Stub(m => m.IsActive)
                .Return(false)
                .Repeat.Any();

            var dataProviderMock = DomainObjectMockBuilder.BuildDataProviderMock();

            var foodGroupCollection = new FoodGroupCollection(new List<IFoodGroup> {activeFoodGroup1, inactiveFoodGroup1, activeFoodGroup2, inactiveFoodGroup2, activeFoodGroup3, inactiveFoodGroup3}, dataProviderMock);
            Assert.That(foodGroupCollection, Is.Not.Null);
            Assert.That(foodGroupCollection, Is.Not.Empty);
            Assert.That(foodGroupCollection.Count, Is.EqualTo(6));

            foodGroupCollection.RemoveInactiveFoodGroups();
            Assert.That(foodGroupCollection, Is.Not.Null);
            Assert.That(foodGroupCollection, Is.Not.Empty);
            Assert.That(foodGroupCollection.Count, Is.EqualTo(3));
            Assert.That(foodGroupCollection.Contains(activeFoodGroup1), Is.True);
            Assert.That(foodGroupCollection.Contains(activeFoodGroup2), Is.True);
            Assert.That(foodGroupCollection.Contains(activeFoodGroup3), Is.True);
        }

        /// <summary>
        /// Tests that RemoveInactiveFoodGroups calls RemoveInactiveChildren on each active food group.
        /// </summary>
        [Test]
        public void TestThatRemoveInactiveFoodGroupsCallsRemoveInactiveChildrenOnEachActiveFoodGroup()
        {
            var activeFoodGroup1 = MockRepository.GenerateMock<IFoodGroup>();
            activeFoodGroup1.Stub(m => m.IsActive)
                .Return(true)
                .Repeat.Any();
            var activeFoodGroup2 = MockRepository.GenerateMock<IFoodGroup>();
            activeFoodGroup2.Stub(m => m.IsActive)
                .Return(true)
                .Repeat.Any();
            var activeFoodGroup3 = MockRepository.GenerateMock<IFoodGroup>();
            activeFoodGroup3.Stub(m => m.IsActive)
                .Return(true)
                .Repeat.Any();
            var inactiveFoodGroup1 = MockRepository.GenerateMock<IFoodGroup>();
            inactiveFoodGroup1.Stub(m => m.IsActive)
                .Return(false)
                .Repeat.Any();
            var inactiveFoodGroup2 = MockRepository.GenerateMock<IFoodGroup>();
            inactiveFoodGroup2.Stub(m => m.IsActive)
                .Return(false)
                .Repeat.Any();
            var inactiveFoodGroup3 = MockRepository.GenerateMock<IFoodGroup>();
            inactiveFoodGroup3.Stub(m => m.IsActive)
                .Return(false)
                .Repeat.Any();

            var dataProviderMock = DomainObjectMockBuilder.BuildDataProviderMock();

            var foodGroupCollection = new FoodGroupCollection(new List<IFoodGroup> {activeFoodGroup1, inactiveFoodGroup1, activeFoodGroup2, inactiveFoodGroup2, activeFoodGroup3, inactiveFoodGroup3}, dataProviderMock);
            Assert.That(foodGroupCollection, Is.Not.Null);
            Assert.That(foodGroupCollection, Is.Not.Empty);

            foodGroupCollection.RemoveInactiveFoodGroups();

            activeFoodGroup1.AssertWasCalled(m => m.RemoveInactiveChildren());
            activeFoodGroup2.AssertWasCalled(m => m.RemoveInactiveChildren());
            activeFoodGroup3.AssertWasCalled(m => m.RemoveInactiveChildren());
        }

        /// <summary>
        /// Tests that RemoveInactiveFoodGroups does not call RemoveInactiveChildren on each inactive food group.
        /// </summary>
        [Test]
        public void TestThatRemoveInactiveFoodGroupsDoesNotCallRemoveInactiveChildrenOnEachInactiveFoodGroup()
        {
            var activeFoodGroup1 = MockRepository.GenerateMock<IFoodGroup>();
            activeFoodGroup1.Stub(m => m.IsActive)
                .Return(true)
                .Repeat.Any();
            var activeFoodGroup2 = MockRepository.GenerateMock<IFoodGroup>();
            activeFoodGroup2.Stub(m => m.IsActive)
                .Return(true)
                .Repeat.Any();
            var activeFoodGroup3 = MockRepository.GenerateMock<IFoodGroup>();
            activeFoodGroup3.Stub(m => m.IsActive)
                .Return(true)
                .Repeat.Any();
            var inactiveFoodGroup1 = MockRepository.GenerateMock<IFoodGroup>();
            inactiveFoodGroup1.Stub(m => m.IsActive)
                .Return(false)
                .Repeat.Any();
            var inactiveFoodGroup2 = MockRepository.GenerateMock<IFoodGroup>();
            inactiveFoodGroup2.Stub(m => m.IsActive)
                .Return(false)
                .Repeat.Any();
            var inactiveFoodGroup3 = MockRepository.GenerateMock<IFoodGroup>();
            inactiveFoodGroup3.Stub(m => m.IsActive)
                .Return(false)
                .Repeat.Any();

            var dataProviderMock = DomainObjectMockBuilder.BuildDataProviderMock();

            var foodGroupCollection = new FoodGroupCollection(new List<IFoodGroup> { activeFoodGroup1, inactiveFoodGroup1, activeFoodGroup2, inactiveFoodGroup2, activeFoodGroup3, inactiveFoodGroup3 }, dataProviderMock);
            Assert.That(foodGroupCollection, Is.Not.Null);
            Assert.That(foodGroupCollection, Is.Not.Empty);

            foodGroupCollection.RemoveInactiveFoodGroups();

            inactiveFoodGroup1.AssertWasNotCalled(m => m.RemoveInactiveChildren());
            inactiveFoodGroup2.AssertWasNotCalled(m => m.RemoveInactiveChildren());
            inactiveFoodGroup3.AssertWasNotCalled(m => m.RemoveInactiveChildren());
        }
    }
}
