using System;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.FoodWaste;

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
    }
}
