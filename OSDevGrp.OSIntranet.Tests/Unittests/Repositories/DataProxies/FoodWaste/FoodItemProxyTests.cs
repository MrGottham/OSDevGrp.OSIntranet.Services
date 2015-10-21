using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Resources;
using Ploeh.AutoFixture;
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

        /// <summary>
        /// Tests that GetSqlCommandForInsert returns the SQL statement to insert this food item.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatGetSqlCommandForInsertReturnsSqlCommandForInsert(bool isActive)
        {
            var foodItemProxy = new FoodItemProxy
            {
                Identifier = Guid.NewGuid(),
                IsActive = isActive
            };

            var sqlCommand = foodItemProxy.GetSqlCommandForInsert();
            Assert.That(sqlCommand, Is.Not.Null);
            Assert.That(sqlCommand, Is.Not.Empty);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(sqlCommand, Is.EqualTo(string.Format("INSERT INTO FoodItems (FoodItemIdentifier,IsActive) VALUES('{0}',{1})", foodItemProxy.UniqueId, Convert.ToInt32(isActive))));
            // ReSharper restore PossibleInvalidOperationException
        }

        /// <summary>
        /// Tests that GetSqlCommandForUpdate returns the SQL statement to update this food item.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatGetSqlCommandForUpdateReturnsSqlCommandForUpdate(bool isActive)
        {
            var foodItemProxy = new FoodItemProxy
            {
                Identifier = Guid.NewGuid(),
                IsActive = isActive
            };

            var sqlCommand = foodItemProxy.GetSqlCommandForUpdate();
            Assert.That(sqlCommand, Is.Not.Null);
            Assert.That(sqlCommand, Is.Not.Empty);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(sqlCommand, Is.EqualTo(string.Format("UPDATE FoodItems SET IsActive={1} WHERE FoodItemIdentifier='{0}'", foodItemProxy.UniqueId, Convert.ToInt32(isActive))));
            // ReSharper restore PossibleInvalidOperationException
        }

        /// <summary>
        /// Tests that GetSqlCommandForDelete returns the SQL statement to delete this food item.
        /// </summary>
        [Test]
        public void TestThatGetSqlCommandForDeleteReturnsSqlCommandForDelete()
        {
            var foodItemProxy = new FoodItemProxy
            {
                Identifier = Guid.NewGuid()
            };

            var sqlCommand = foodItemProxy.GetSqlCommandForDelete();
            Assert.That(sqlCommand, Is.Not.Null);
            Assert.That(sqlCommand, Is.Not.Empty);
            Assert.That(sqlCommand, Is.EqualTo(string.Format("DELETE FROM FoodItems WHERE FoodItemIdentifier='{0}'", foodItemProxy.UniqueId)));
        }

        /// <summary>
        /// Tests that MapData throws an ArgumentNullException if the data reader is null.
        /// </summary>
        [Test]
        public void TestThatMapDataThrowsArgumentNullExceptionIfDataReaderIsNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IDataProviderBase>(e => e.FromFactory(() => MockRepository.GenerateMock<IDataProviderBase>()));

            var foodItemProxy = new FoodItemProxy();
            Assert.That(foodItemProxy, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodItemProxy.MapData(null, fixture.Create<IDataProviderBase>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("dataReader"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that MapData throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatMapDataThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            var fixture = new Fixture();
            fixture.Customize<MySqlDataReader>(e => e.FromFactory(() => MockRepository.GenerateStub<MySqlDataReader>()));

            var foodItemProxy = new FoodItemProxy();
            Assert.That(foodItemProxy, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodItemProxy.MapData(fixture.Create<MySqlDataReader>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("dataProvider"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that MapData throws an IntranetRepositoryException if the data reader is not type of MySqlDataReader.
        /// </summary>
        [Test]
        public void TestThatMapDataThrowsIntranetRepositoryExceptionIfDataReaderIsNotTypeOfMySqlDataReader()
        {
            var fixture = new Fixture();
            fixture.Customize<IDataProviderBase>(e => e.FromFactory(() => MockRepository.GenerateMock<IDataProviderBase>()));

            var dataReader = MockRepository.GenerateMock<IDataReader>();

            var foodItemProxy = new FoodItemProxy();
            Assert.That(foodItemProxy, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => foodItemProxy.MapData(dataReader, fixture.Create<IDataProviderBase>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, "dataReader", dataReader.GetType().Name)));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that MapData and MapRelations maps data into the proxy.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatMapDataAndMapRelationsMapsDataIntoProxy(bool isActive)
        {
            var fixture = new Fixture();

            var primaryFoodGroupMock = MockRepository.GenerateMock<IFoodGroup>();
            primaryFoodGroupMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();
            var secondaryFoodGroup1Mock = MockRepository.GenerateMock<IFoodGroup>();
            secondaryFoodGroup1Mock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();
            var secondaryFoodGroup2Mock = MockRepository.GenerateMock<IFoodGroup>();
            secondaryFoodGroup2Mock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var primaryFoodItemGroupProxy = new FoodItemGroupProxy(MockRepository.GenerateMock<IFoodItem>(), primaryFoodGroupMock)
            {
                IsPrimary = true
            };
            var secondaryFoodItemGroupProxy1 = new FoodItemGroupProxy(MockRepository.GenerateMock<IFoodItem>(), secondaryFoodGroup1Mock)
            {
                IsPrimary = false
            };
            var secondaryFoodItemGroupProxy2 = new FoodItemGroupProxy(MockRepository.GenerateMock<IFoodItem>(), secondaryFoodGroup2Mock)
            {
                IsPrimary = false
            };
            var foodItemGroupProxyCollection = new List<FoodItemGroupProxy>
            {
                primaryFoodItemGroupProxy,
                secondaryFoodItemGroupProxy1,
                secondaryFoodItemGroupProxy2
            };

            var translationProxyCollection = fixture.CreateMany<TranslationProxy>(3).ToList();
            var foreignKeyProxyCollection = fixture.CreateMany<ForeignKeyProxy>(5).ToList();

            var dataProviderBaseMock = MockRepository.GenerateMock<IDataProviderBase>();
            dataProviderBaseMock.Stub(m => m.Clone())
                .Return(dataProviderBaseMock)
                .Repeat.Any();
            dataProviderBaseMock.Stub(m => m.GetCollection<FoodItemGroupProxy>(Arg<string>.Is.Anything))
                .Return(foodItemGroupProxyCollection)
                .Repeat.Any();
            dataProviderBaseMock.Stub(m => m.GetCollection<TranslationProxy>(Arg<string>.Is.Anything))
                .Return(translationProxyCollection)
                .Repeat.Any();
            dataProviderBaseMock.Stub(m => m.GetCollection<ForeignKeyProxy>(Arg<string>.Is.Anything))
                .Return(foreignKeyProxyCollection)
                .Repeat.Any();

            var dataReader = MockRepository.GenerateStub<MySqlDataReader>();
            dataReader.Stub(m => m.GetString(Arg<string>.Is.Equal("FoodItemIdentifier")))
                .Return("36D4CFC2-8D45-4411-BBBF-7F95EA7587DE")
                .Repeat.Any();
            dataReader.Stub(m => m.GetInt32(Arg<string>.Is.Equal("IsActive")))
                .Return(Convert.ToInt32(isActive))
                .Repeat.Any();

            var foodItemProxy = new FoodItemProxy();
            Assert.That(foodItemProxy, Is.Not.Null);
            Assert.That(foodItemProxy.Identifier, Is.Null);
            Assert.That(foodItemProxy.Identifier.HasValue, Is.False);

            foodItemProxy.MapData(dataReader, dataProviderBaseMock);
            foodItemProxy.MapRelations(dataProviderBaseMock);
            Assert.That(foodItemProxy, Is.Not.Null);
            Assert.That(foodItemProxy.Identifier, Is.Not.Null);
            Assert.That(foodItemProxy.Identifier.HasValue, Is.True);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(foodItemProxy.Identifier.Value.ToString("D").ToUpper(), Is.EqualTo(dataReader.GetString("FoodItemIdentifier")));
            // ReSharper restore PossibleInvalidOperationException
            Assert.That(foodItemProxy.PrimaryFoodGroup, Is.Not.Null);
            Assert.That(foodItemProxy.PrimaryFoodGroup, Is.EqualTo(primaryFoodGroupMock));
            Assert.That(foodItemProxy.IsActive, Is.EqualTo(isActive));
            Assert.That(foodItemProxy.FoodGroups, Is.Not.Null);
            Assert.That(foodItemProxy.FoodGroups, Is.Not.Empty);
            Assert.That(foodItemProxy.FoodGroups.Count(), Is.EqualTo(foodItemGroupProxyCollection.Count));
            Assert.That(foodItemProxy.FoodGroups.Contains(primaryFoodGroupMock), Is.True);
            Assert.That(foodItemProxy.FoodGroups.Contains(secondaryFoodGroup1Mock), Is.True);
            Assert.That(foodItemProxy.FoodGroups.Contains(secondaryFoodGroup2Mock), Is.True);
            Assert.That(foodItemProxy.Translation, Is.Null);
            Assert.That(foodItemProxy.Translations, Is.Not.Null);
            Assert.That(foodItemProxy.Translations, Is.Not.Empty);
            Assert.That(foodItemProxy.Translations.Count(), Is.EqualTo(translationProxyCollection.Count));
            foreach (var translationProxy in translationProxyCollection)
            {
                Assert.That(foodItemProxy.Translations.Contains(translationProxy), Is.True);
            }
            Assert.That(foodItemProxy.ForeignKeys, Is.Not.Null);
            Assert.That(foodItemProxy.ForeignKeys, Is.Not.Empty);
            Assert.That(foodItemProxy.ForeignKeys.Count(), Is.EqualTo(foreignKeyProxyCollection.Count));
            foreach (var foreignKeyProxy in foreignKeyProxyCollection)
            {
                Assert.That(foodItemProxy.ForeignKeys.Contains(foreignKeyProxy), Is.True);
            }

            dataProviderBaseMock.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Times(3));
            dataProviderBaseMock.AssertWasCalled(m => m.GetCollection<FoodItemGroupProxy>(Arg<string>.Is.Equal(string.Format("SELECT FoodItemGroupIdentifier,FoodItemIdentifier,FoodGroupIdentifier,IsPrimary WHERE FoodItemIdentifier='{0}'", dataReader.GetString("FoodItemIdentifier")))));
            dataProviderBaseMock.AssertWasCalled(m => m.GetCollection<TranslationProxy>(Arg<string>.Is.Equal(string.Format("SELECT t.TranslationIdentifier AS TranslationIdentifier,t.OfIdentifier AS OfIdentifier,ti.TranslationInfoIdentifier AS InfoIdentifier,ti.CultureName AS CultureName,t.Value AS Value FROM Translations AS t, TranslationInfos AS ti WHERE t.OfIdentifier='{0}' AND ti.TranslationInfoIdentifier=t.InfoIdentifier ORDER BY CultureName", dataReader.GetString("FoodItemIdentifier")))));
            dataProviderBaseMock.AssertWasCalled(m => m.GetCollection<ForeignKeyProxy>(string.Format("SELECT ForeignKeyIdentifier,DataProviderIdentifier,ForeignKeyForIdentifier,ForeignKeyForTypes,ForeignKeyValue FROM ForeignKeys WHERE ForeignKeyForIdentifier='{0}' ORDER BY DataProviderIdentifier,ForeignKeyValue", dataReader.GetString("FoodItemIdentifier"))));
        }

        /// <summary>
        /// Tests that MapRelations throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatMapRelationsThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            var fixture = new Fixture();
            fixture.Customize<MySqlDataReader>(e => e.FromFactory(() => MockRepository.GenerateStub<MySqlDataReader>()));

            var foodItemProxy = new FoodItemProxy();
            Assert.That(foodItemProxy, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodItemProxy.MapRelations(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("dataProvider"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that SaveRelations throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            var fixture = new Fixture();

            var foodItemProxy = new FoodItemProxy();
            Assert.That(foodItemProxy, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodItemProxy.SaveRelations(null, fixture.Create<bool>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("dataProvider"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that SaveRelations throws an IntranetRepositoryException when the identifier for the relation between a food item and a food group is null.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsThrowsIntranetRepositoryExceptionWhenIdentifierIsNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IDataProviderBase>(e => e.FromFactory(() => MockRepository.GenerateStub<IDataProviderBase>()));

            var foodItemProxy = new FoodItemProxy
            {
                Identifier = null
            };
            Assert.That(foodItemProxy, Is.Not.Null);
            Assert.That(foodItemProxy.Identifier, Is.Null);
            Assert.That(foodItemProxy.Identifier.HasValue, Is.False);

            var exception = Assert.Throws<IntranetRepositoryException>(() => foodItemProxy.SaveRelations(fixture.Create<IDataProviderBase>(), fixture.Create<bool>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, foodItemProxy.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that DeleteRelations throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            var foodItemProxy = new FoodItemProxy();
            Assert.That(foodItemProxy, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodItemProxy.DeleteRelations(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("dataProvider"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that DeleteRelations throws an IntranetRepositoryException when the identifier for the translation is null.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsThrowsIntranetRepositoryExceptionWhenIdentifierIsNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IDataProviderBase>(e => e.FromFactory(() => MockRepository.GenerateStub<IDataProviderBase>()));

            var foodItemProxy = new FoodItemProxy
            {
                Identifier = null
            };
            Assert.That(foodItemProxy, Is.Not.Null);
            Assert.That(foodItemProxy.Identifier, Is.Null);
            Assert.That(foodItemProxy.Identifier.HasValue, Is.False);

            var exception = Assert.Throws<IntranetRepositoryException>(() => foodItemProxy.DeleteRelations(fixture.Create<IDataProviderBase>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, foodItemProxy.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }
    }
}
