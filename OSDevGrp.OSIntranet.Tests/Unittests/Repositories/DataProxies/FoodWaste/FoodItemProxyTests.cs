using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Resources;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.DataProxies.FoodWaste
{
    /// <summary>
    /// Tests the data proxy for a food item.
    /// </summary>
    [TestFixture]
    public class FoodItemProxyTests
    {
        /// <summary>
        /// Tests that the constructor initialize a data proxy to a given food item.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeFoodItemProxy()
        {
            var foodItemProxy = new FoodItemProxy();
            Assert.That(foodItemProxy, Is.Not.Null);
            Assert.That(foodItemProxy.Identifier, Is.Null);
            Assert.That(foodItemProxy.Identifier.HasValue, Is.False);
            Assert.That(foodItemProxy.PrimaryFoodGroup, Is.Null);
            Assert.That(foodItemProxy.IsActive, Is.False);
            Assert.That(foodItemProxy.FoodGroups, Is.Not.Null);
            Assert.That(foodItemProxy.FoodGroups, Is.Empty);
            Assert.That(foodItemProxy.Translation, Is.Null);
            Assert.That(foodItemProxy.Translations, Is.Not.Null);
            Assert.That(foodItemProxy.Translations, Is.Empty);
            Assert.That(foodItemProxy.ForeignKeys, Is.Not.Null);
            Assert.That(foodItemProxy.ForeignKeys, Is.Empty);
        }

        /// <summary>
        /// Tests that getter for UniqueId throws an IntranetRepositoryException when the food item has no identifier.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterThrowsIntranetRepositoryExceptionWhenFoodItemProxyHasNoIdentifier()
        {
            var foodItemProxy = new FoodItemProxy
            {
                Identifier = null
            };

            // ReSharper disable UnusedVariable
            var exception = Assert.Throws<IntranetRepositoryException>(() => { var uniqueId = foodItemProxy.UniqueId; });
            // ReSharper restore UnusedVariable
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, foodItemProxy.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that getter for UniqueId gets the unique identifier for the food item.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterGetsUniqueIdentificationForFoodItemProxy()
        {
            var foodItemProxy = new FoodItemProxy
            {
                Identifier = Guid.NewGuid()
            };

            var uniqueId = foodItemProxy.UniqueId;
            Assert.That(uniqueId, Is.Not.Null);
            Assert.That(uniqueId, Is.Not.Empty);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(uniqueId, Is.EqualTo(foodItemProxy.Identifier.Value.ToString("D").ToUpper()));
            // ReSharper restore PossibleInvalidOperationException
        }

        /// <summary>
        /// Tests that GetSqlQueryForId throws an ArgumentNullException when the given food item is null.
        /// </summary>
        [Test]
        public void TestThatGetSqlQueryForIdThrowsArgumentNullExceptionWhenFoodItemIsNull()
        {
            var foodItemProxy = new FoodItemProxy();

            // ReSharper disable UnusedVariable
            var exception = Assert.Throws<ArgumentNullException>(() => { var sqlQueryForId = foodItemProxy.GetSqlQueryForId(null); });
            // ReSharper restore UnusedVariable
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("foodItem"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetSqlQueryForId throws an IntranetRepositoryException when the identifier on the given food item has no value.
        /// </summary>
        [Test]
        public void TestThatGetSqlQueryForIdThrowsIntranetRepositoryExceptionWhenIdentifierOnFoodItemHasNoValue()
        {
            var foodItemMock = MockRepository.GenerateMock<IFoodItem>();
            foodItemMock.Expect(m => m.Identifier)
                .Return(null)
                .Repeat.Any();

            var foodItemProxy = new FoodItemProxy();

            // ReSharper disable UnusedVariable
            var exception = Assert.Throws<IntranetRepositoryException>(() => { var sqlQueryForId = foodItemProxy.GetSqlQueryForId(foodItemMock); });
            // ReSharper restore UnusedVariable
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, foodItemMock.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetSqlQueryForId returns the SQL statement for selecting the given food item.
        /// </summary>
        [Test]
        public void TestThatGetSqlQueryForIdReturnsSqlQueryForId()
        {
            var foodItemMock = MockRepository.GenerateMock<IFoodItem>();
            foodItemMock.Expect(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodItemProxy = new FoodItemProxy();

            var sqlQueryForId = foodItemProxy.GetSqlQueryForId(foodItemMock);
            Assert.That(sqlQueryForId, Is.Not.Null);
            Assert.That(sqlQueryForId, Is.Not.Empty);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(sqlQueryForId, Is.EqualTo(string.Format("SELECT IsActive FROM FoodItems WHERE FoodItemIdentifier='{0}'", foodItemMock.Identifier.Value.ToString("D").ToUpper())));
            // ReSharper restore PossibleInvalidOperationException
        }
    }
}
