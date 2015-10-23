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
            dataProviderBaseMock.AssertWasCalled(m => m.GetCollection<FoodItemGroupProxy>(Arg<string>.Is.Equal(string.Format("SELECT FoodItemGroupIdentifier,FoodItemIdentifier,FoodGroupIdentifier,IsPrimary FROM FoodItemGroups WHERE FoodItemIdentifier='{0}'", dataReader.GetString("FoodItemIdentifier")))));
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
        /// Tests that SaveRelations throws an IntranetRepositoryException when the identifier for the primary food group is null.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsThrowsIntranetRepositoryExceptionWhenIdentifierIsNullOnPrimaryFoodGroup()
        {
            var fixture = new Fixture();
            fixture.Customize<IDataProviderBase>(e => e.FromFactory(() => MockRepository.GenerateStub<IDataProviderBase>()));

            var primaryFoodGroupMock = MockRepository.GenerateMock<IFoodGroup>();
            primaryFoodGroupMock.Stub(m => m.Identifier)
                .Return(null)
                .Repeat.Any();

            var foodItemProxy = new FoodItemProxy(primaryFoodGroupMock)
            {
                Identifier = Guid.NewGuid()
            };
            Assert.That(foodItemProxy, Is.Not.Null);
            Assert.That(foodItemProxy.Identifier, Is.Not.Null);
            Assert.That(foodItemProxy.Identifier.HasValue, Is.True);
            Assert.That(foodItemProxy.PrimaryFoodGroup, Is.Not.Null);
            Assert.That(foodItemProxy.PrimaryFoodGroup, Is.EqualTo(primaryFoodGroupMock));

            var exception = Assert.Throws<IntranetRepositoryException>(() => foodItemProxy.SaveRelations(fixture.Create<IDataProviderBase>(), fixture.Create<bool>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, primaryFoodGroupMock.Identifier, "PrimaryFoodGroup.Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that SaveRelations throws an IntranetRepositoryException when the identifier for a secondary food group is null.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsThrowsIntranetRepositoryExceptionWhenIdentifierIsNullOnSecondaryFoodGroup()
        {
            var fixture = new Fixture();
            fixture.Customize<IDataProviderBase>(e => e.FromFactory(() => MockRepository.GenerateStub<IDataProviderBase>()));

            var primaryFoodGroupMock = MockRepository.GenerateMock<IFoodGroup>();
            primaryFoodGroupMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var secondaryFoodGroupMock = MockRepository.GenerateMock<IFoodGroup>();
            secondaryFoodGroupMock.Stub(m => m.Identifier)
                .Return(null)
                .Repeat.Any();

            var foodItemProxy = new FoodItemProxy(primaryFoodGroupMock)
            {
                Identifier = Guid.NewGuid()
            };
            Assert.That(foodItemProxy, Is.Not.Null);
            Assert.That(foodItemProxy.Identifier, Is.Not.Null);
            Assert.That(foodItemProxy.Identifier.HasValue, Is.True);
            Assert.That(foodItemProxy.PrimaryFoodGroup, Is.Not.Null);
            Assert.That(foodItemProxy.PrimaryFoodGroup, Is.EqualTo(primaryFoodGroupMock));

            foodItemProxy.FoodGroupAdd(secondaryFoodGroupMock);
            Assert.That(foodItemProxy.FoodGroups, Is.Not.Null);
            Assert.That(foodItemProxy.FoodGroups, Is.Not.Empty);
            Assert.That(foodItemProxy.FoodGroups.Contains(secondaryFoodGroupMock), Is.True);

            var exception = Assert.Throws<IntranetRepositoryException>(() => foodItemProxy.SaveRelations(fixture.Create<IDataProviderBase>(), fixture.Create<bool>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, secondaryFoodGroupMock.Identifier, "FoodGroups[].Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that SaveRelations inserts the missing relations between a food item and it's food groups.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsInsertsMissingFoodItemGroups()
        {
            var fixture = new Fixture();
            var foodItemIdentifier = Guid.NewGuid();

            var primaryFoodGroupMock = MockRepository.GenerateMock<IFoodGroup>();
            primaryFoodGroupMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var secondaryFoodGroupMock = MockRepository.GenerateMock<IFoodGroup>();
            secondaryFoodGroupMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var dataProviderMock = MockRepository.GenerateMock<IDataProviderBase>();
            dataProviderMock.Stub(m => m.Clone())
                .Return(dataProviderMock)
                .Repeat.Any();
            dataProviderMock.Stub(m => m.GetCollection<FoodItemGroupProxy>(Arg<string>.Is.Anything))
                .Return(new List<FoodItemGroupProxy>(0))
                .Repeat.Any();
            dataProviderMock.Stub(m => m.Add(Arg<FoodItemGroupProxy>.Is.NotNull))
                .WhenCalled(e =>
                {
                    var foodItemGroupProxy = (FoodItemGroupProxy) e.Arguments.ElementAt(0);
                    Assert.That(foodItemGroupProxy, Is.Not.Null);
                    Assert.That(foodItemGroupProxy.Identifier, Is.Not.Null);
                    Assert.That(foodItemGroupProxy.Identifier.HasValue, Is.True);
                    Assert.That(foodItemGroupProxy.FoodItemIdentifier, Is.Not.Null);
                    Assert.That(foodItemGroupProxy.FoodItemIdentifier.HasValue, Is.True);
                    // ReSharper disable PossibleInvalidOperationException
                    Assert.That(foodItemGroupProxy.FoodItemIdentifier.Value, Is.EqualTo(foodItemIdentifier));
                    // ReSharper restore PossibleInvalidOperationException
                    Assert.That(foodItemGroupProxy.FoodGroupIdentifier, Is.Not.Null);
                    Assert.That(foodItemGroupProxy.FoodGroupIdentifier.HasValue, Is.True);
                    // ReSharper disable PossibleInvalidOperationException
                    if (foodItemGroupProxy.FoodGroupIdentifier.Value == primaryFoodGroupMock.Identifier.Value)
                    // ReSharper restore PossibleInvalidOperationException
                    {
                        Assert.That(foodItemGroupProxy.IsPrimary, Is.True);
                        e.ReturnValue = foodItemGroupProxy;
                        return;
                    }
                    // ReSharper disable PossibleInvalidOperationException
                    if (foodItemGroupProxy.FoodGroupIdentifier.Value == secondaryFoodGroupMock.Identifier.Value)
                    // ReSharper restore PossibleInvalidOperationException
                    {
                        Assert.That(foodItemGroupProxy.IsPrimary, Is.False);
                        e.ReturnValue = foodItemGroupProxy;
                        return;
                    }
                    Assert.Fail(string.Format("The FoodGroupIdentifier '{0}' is unknown.", foodItemGroupProxy.FoodGroupIdentifier.Value));
                })
                .Return(null)
                .Repeat.Any();

            var foodItemProxy = new FoodItemProxy(primaryFoodGroupMock)
            {
                Identifier = foodItemIdentifier
            };
            Assert.That(foodItemProxy, Is.Not.Null);
            Assert.That(foodItemProxy.Identifier, Is.Not.Null);
            Assert.That(foodItemProxy.Identifier.HasValue, Is.True);
            Assert.That(foodItemProxy.PrimaryFoodGroup, Is.Not.Null);
            Assert.That(foodItemProxy.PrimaryFoodGroup, Is.EqualTo(primaryFoodGroupMock));

            foodItemProxy.FoodGroupAdd(secondaryFoodGroupMock);
            Assert.That(foodItemProxy.FoodGroups, Is.Not.Null);
            Assert.That(foodItemProxy.FoodGroups, Is.Not.Empty);
            Assert.That(foodItemProxy.FoodGroups.Contains(secondaryFoodGroupMock), Is.True);

            foodItemProxy.SaveRelations(dataProviderMock, fixture.Create<bool>());

            dataProviderMock.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Times(3));
            // ReSharper disable PossibleInvalidOperationException
            dataProviderMock.AssertWasCalled(m => m.GetCollection<FoodItemGroupProxy>(Arg<string>.Is.Equal(string.Format("SELECT FoodItemGroupIdentifier,FoodItemIdentifier,FoodGroupIdentifier,IsPrimary FROM FoodItemGroups WHERE FoodItemIdentifier='{0}'", foodItemProxy.Identifier.Value.ToString("D").ToUpper()))));
            // ReSharper restore PossibleInvalidOperationException
            dataProviderMock.AssertWasCalled(m => m.Add(Arg<FoodItemGroupProxy>.Is.NotNull), opt => opt.Repeat.Times(2));
        }

        /// <summary>
        /// Tests that SaveRelations deletes the no longer existing relations between a food item and it's food groups.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsDeletesNoLongerExistingFoodItemGroups()
        {
            var fixture = new Fixture();
            var foodItemIdentifier = Guid.NewGuid();

            var primaryFoodGroupMock = MockRepository.GenerateMock<IFoodGroup>();
            primaryFoodGroupMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var secondaryFoodGroupMock = MockRepository.GenerateMock<IFoodGroup>();
            secondaryFoodGroupMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var noLongerExistingFoodGroupMock = MockRepository.GenerateMock<IFoodGroup>();
            noLongerExistingFoodGroupMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodItemGroupProxyCollection = new List<FoodItemGroupProxy>
            {
                new FoodItemGroupProxy(MockRepository.GenerateMock<IFoodItem>(), primaryFoodGroupMock)
                {
                    IsPrimary = true
                },
                new FoodItemGroupProxy(MockRepository.GenerateMock<IFoodItem>(), secondaryFoodGroupMock)
                {
                    IsPrimary = false
                },
                new FoodItemGroupProxy(MockRepository.GenerateMock<IFoodItem>(), noLongerExistingFoodGroupMock)
                {
                    IsPrimary = false
                }
            };

            var dataProviderMock = MockRepository.GenerateMock<IDataProviderBase>();
            dataProviderMock.Stub(m => m.Clone())
                .Return(dataProviderMock)
                .Repeat.Any();
            dataProviderMock.Stub(m => m.GetCollection<FoodItemGroupProxy>(Arg<string>.Is.Anything))
                .Return(foodItemGroupProxyCollection)
                .Repeat.Any();

            var foodItemProxy = new FoodItemProxy(primaryFoodGroupMock)
            {
                Identifier = foodItemIdentifier
            };
            Assert.That(foodItemProxy, Is.Not.Null);
            Assert.That(foodItemProxy.Identifier, Is.Not.Null);
            Assert.That(foodItemProxy.Identifier.HasValue, Is.True);
            Assert.That(foodItemProxy.PrimaryFoodGroup, Is.Not.Null);
            Assert.That(foodItemProxy.PrimaryFoodGroup, Is.EqualTo(primaryFoodGroupMock));

            foodItemProxy.FoodGroupAdd(secondaryFoodGroupMock);
            Assert.That(foodItemProxy.FoodGroups, Is.Not.Null);
            Assert.That(foodItemProxy.FoodGroups, Is.Not.Empty);
            Assert.That(foodItemProxy.FoodGroups.Contains(secondaryFoodGroupMock), Is.True);

            foodItemProxy.SaveRelations(dataProviderMock, fixture.Create<bool>());

            dataProviderMock.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Times(2));
            // ReSharper disable PossibleInvalidOperationException
            dataProviderMock.AssertWasCalled(m => m.GetCollection<FoodItemGroupProxy>(Arg<string>.Is.Equal(string.Format("SELECT FoodItemGroupIdentifier,FoodItemIdentifier,FoodGroupIdentifier,IsPrimary FROM FoodItemGroups WHERE FoodItemIdentifier='{0}'", foodItemProxy.Identifier.Value.ToString("D").ToUpper()))));
            // ReSharper restore PossibleInvalidOperationException
            dataProviderMock.AssertWasNotCalled(m => m.Delete(Arg<FoodItemGroupProxy>.Is.Equal(foodItemGroupProxyCollection.ElementAt(0))));
            dataProviderMock.AssertWasNotCalled(m => m.Delete(Arg<FoodItemGroupProxy>.Is.Equal(foodItemGroupProxyCollection.ElementAt(1))));
            dataProviderMock.AssertWasCalled(m => m.Delete(Arg<FoodItemGroupProxy>.Is.Equal(foodItemGroupProxyCollection.ElementAt(2))));
        }

        /// <summary>
        /// Tests that SaveRelations updates IsPrimary on relations between a food item and it's food groups.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsUpdatesIsPrimaryOnFoodItemGroups()
        {
            var fixture = new Fixture();
            var foodItemIdentifier = Guid.NewGuid();

            var primaryFoodGroupMock = MockRepository.GenerateMock<IFoodGroup>();
            primaryFoodGroupMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var secondaryFoodGroupMock = MockRepository.GenerateMock<IFoodGroup>();
            secondaryFoodGroupMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var noLongerExistingFoodGroupMock = MockRepository.GenerateMock<IFoodGroup>();
            noLongerExistingFoodGroupMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodItemGroupProxyCollection = new List<FoodItemGroupProxy>
            {
                new FoodItemGroupProxy(MockRepository.GenerateMock<IFoodItem>(), primaryFoodGroupMock)
                {
                    IsPrimary = false
                },
                new FoodItemGroupProxy(MockRepository.GenerateMock<IFoodItem>(), secondaryFoodGroupMock)
                {
                    IsPrimary = true
                }
            };

            var dataProviderMock = MockRepository.GenerateMock<IDataProviderBase>();
            dataProviderMock.Stub(m => m.Clone())
                .Return(dataProviderMock)
                .Repeat.Any();
            dataProviderMock.Stub(m => m.GetCollection<FoodItemGroupProxy>(Arg<string>.Is.Anything))
                .Return(foodItemGroupProxyCollection)
                .Repeat.Any();
            dataProviderMock.Stub(m => m.Save(Arg<FoodItemGroupProxy>.Is.NotNull))
                .WhenCalled(e =>
                {
                    var foodItemGroupProxy = (FoodItemGroupProxy) e.Arguments.ElementAt(0);
                    Assert.That(foodItemGroupProxy, Is.Not.Null);
                    Assert.That(foodItemGroupProxy.FoodGroupIdentifier, Is.Not.Null);
                    Assert.That(foodItemGroupProxy.FoodGroupIdentifier.HasValue, Is.True);
                    // ReSharper disable PossibleInvalidOperationException
                    if (foodItemGroupProxy.FoodGroupIdentifier.Value == primaryFoodGroupMock.Identifier.Value)
                    // ReSharper restore PossibleInvalidOperationException
                    {
                        Assert.That(foodItemGroupProxy.IsPrimary, Is.True);
                        e.ReturnValue = foodItemGroupProxy;
                        return;
                    }
                    // ReSharper disable PossibleInvalidOperationException
                    if (foodItemGroupProxy.FoodGroupIdentifier.Value == secondaryFoodGroupMock.Identifier.Value)
                    // ReSharper restore PossibleInvalidOperationException
                    {
                        Assert.That(foodItemGroupProxy.IsPrimary, Is.False);
                        e.ReturnValue = foodItemGroupProxy;
                        return;
                    }
                    Assert.Fail(string.Format("The FoodGroupIdentifier '{0}' is unknown.", foodItemGroupProxy.FoodGroupIdentifier.Value));
                })
                .Return(null)
                .Repeat.Any();

            var foodItemProxy = new FoodItemProxy(primaryFoodGroupMock)
            {
                Identifier = foodItemIdentifier
            };
            Assert.That(foodItemProxy, Is.Not.Null);
            Assert.That(foodItemProxy.Identifier, Is.Not.Null);
            Assert.That(foodItemProxy.Identifier.HasValue, Is.True);
            Assert.That(foodItemProxy.PrimaryFoodGroup, Is.Not.Null);
            Assert.That(foodItemProxy.PrimaryFoodGroup, Is.EqualTo(primaryFoodGroupMock));

            foodItemProxy.FoodGroupAdd(secondaryFoodGroupMock);
            Assert.That(foodItemProxy.FoodGroups, Is.Not.Null);
            Assert.That(foodItemProxy.FoodGroups, Is.Not.Empty);
            Assert.That(foodItemProxy.FoodGroups.Contains(secondaryFoodGroupMock), Is.True);

            foodItemProxy.SaveRelations(dataProviderMock, fixture.Create<bool>());

            dataProviderMock.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Times(3));
            // ReSharper disable PossibleInvalidOperationException
            dataProviderMock.AssertWasCalled(m => m.GetCollection<FoodItemGroupProxy>(Arg<string>.Is.Equal(string.Format("SELECT FoodItemGroupIdentifier,FoodItemIdentifier,FoodGroupIdentifier,IsPrimary FROM FoodItemGroups WHERE FoodItemIdentifier='{0}'", foodItemProxy.Identifier.Value.ToString("D").ToUpper()))));
            // ReSharper restore PossibleInvalidOperationException
            dataProviderMock.AssertWasCalled(m => m.Save(Arg<FoodItemGroupProxy>.Is.NotNull), opt => opt.Repeat.Times(2));
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

        /// <summary>
        /// Tests that DeleteRelations calls Clone on the data provider tree times.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsCallsCloneOnDataProvider3Times()
        {
            var dataProviderMock = MockRepository.GenerateStub<IDataProviderBase>();
            dataProviderMock.Stub(m => m.Clone())
                .Return(dataProviderMock)
                .Repeat.Any();
            dataProviderMock.Stub(m => m.GetCollection<FoodItemGroupProxy>(Arg<string>.Is.Anything))
                .Return(new List<FoodItemGroupProxy>())
                .Repeat.Any();
            dataProviderMock.Stub(m => m.GetCollection<TranslationProxy>(Arg<string>.Is.Anything))
                .Return(new List<TranslationProxy>())
                .Repeat.Any();
            dataProviderMock.Stub(m => m.GetCollection<ForeignKeyProxy>(Arg<string>.Is.Anything))
                .Return(new List<ForeignKeyProxy>())
                .Repeat.Any();

            var foodItemProxy = new FoodItemProxy
            {
                Identifier = Guid.NewGuid()
            };
            Assert.That(foodItemProxy, Is.Not.Null);
            Assert.That(foodItemProxy.Identifier, Is.Not.Null);
            Assert.That(foodItemProxy.Identifier.HasValue, Is.True);

            foodItemProxy.DeleteRelations(dataProviderMock);

            dataProviderMock.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Times(3));
        }

        /// <summary>
        /// Tests that DeleteRelations calls GetCollection on the data provider to get the releations between the food item and it's food groups.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsCallsGetCollectionOnDataProviderToGetFoodItemGroups()
        {
            var dataProviderMock = MockRepository.GenerateStub<IDataProviderBase>();
            dataProviderMock.Stub(m => m.Clone())
                .Return(dataProviderMock)
                .Repeat.Any();
            dataProviderMock.Stub(m => m.GetCollection<FoodItemGroupProxy>(Arg<string>.Is.Anything))
                .Return(new List<FoodItemGroupProxy>())
                .Repeat.Any();
            dataProviderMock.Stub(m => m.GetCollection<TranslationProxy>(Arg<string>.Is.Anything))
                .Return(new List<TranslationProxy>())
                .Repeat.Any();
            dataProviderMock.Stub(m => m.GetCollection<ForeignKeyProxy>(Arg<string>.Is.Anything))
                .Return(new List<ForeignKeyProxy>())
                .Repeat.Any();

            var foodItemProxy = new FoodItemProxy
            {
                Identifier = Guid.NewGuid()
            };
            Assert.That(foodItemProxy, Is.Not.Null);
            Assert.That(foodItemProxy.Identifier, Is.Not.Null);
            Assert.That(foodItemProxy.Identifier.HasValue, Is.True);

            foodItemProxy.DeleteRelations(dataProviderMock);

            // ReSharper disable PossibleInvalidOperationException
            dataProviderMock.AssertWasCalled(m => m.GetCollection<FoodItemGroupProxy>(Arg<string>.Is.Equal(string.Format("SELECT FoodItemGroupIdentifier,FoodItemIdentifier,FoodGroupIdentifier,IsPrimary FROM FoodItemGroups WHERE FoodItemIdentifier='{0}'", foodItemProxy.Identifier.Value.ToString("D").ToUpper()))));
            // ReSharper restore PossibleInvalidOperationException
        }

        /// <summary>
        /// Tests that DeleteRelations calls Delete on the data provider for each relation between the food item and it's food groups.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsCallsDeleteOnDataProviderForEachFoodItemGroups()
        {
            var fixture = new Fixture();

            var foodItemGroupProxyCollection = fixture.CreateMany<FoodItemGroupProxy>(7).ToList();
            var dataProviderMock = MockRepository.GenerateStub<IDataProviderBase>();
            dataProviderMock.Stub(m => m.Clone())
                .Return(dataProviderMock)
                .Repeat.Any();
            dataProviderMock.Stub(m => m.GetCollection<FoodItemGroupProxy>(Arg<string>.Is.Anything))
                .Return(foodItemGroupProxyCollection)
                .Repeat.Any();
            dataProviderMock.Stub(m => m.GetCollection<TranslationProxy>(Arg<string>.Is.Anything))
                .Return(new List<TranslationProxy>())
                .Repeat.Any();
            dataProviderMock.Stub(m => m.GetCollection<ForeignKeyProxy>(Arg<string>.Is.Anything))
                .Return(new List<ForeignKeyProxy>())
                .Repeat.Any();
            dataProviderMock.Stub(m => m.Delete(Arg<FoodItemGroupProxy>.Is.NotNull))
                .WhenCalled(e =>
                {
                    var foodItemGroupProxy = (FoodItemGroupProxy) e.Arguments.ElementAt(0);
                    Assert.That(foodItemGroupProxy, Is.Not.Null);
                    Assert.That(foodItemGroupProxyCollection.Contains(foodItemGroupProxy), Is.True);
                })
                .Repeat.Any();

            var foodItemProxy = new FoodItemProxy
            {
                Identifier = Guid.NewGuid()
            };
            Assert.That(foodItemProxy, Is.Not.Null);
            Assert.That(foodItemProxy.Identifier, Is.Not.Null);
            Assert.That(foodItemProxy.Identifier.HasValue, Is.True);

            foodItemProxy.DeleteRelations(dataProviderMock);

            dataProviderMock.AssertWasCalled(m => m.Delete(Arg<FoodItemGroupProxy>.Is.NotNull), opt => opt.Repeat.Times(foodItemGroupProxyCollection.Count));
        }

        /// <summary>
        /// Tests that DeleteRelations calls GetCollection on the data provider to get the translation for the data proxy food item.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsCallsGetCollectionOnDataProviderToGetTranslations()
        {
            var dataProviderMock = MockRepository.GenerateStub<IDataProviderBase>();
            dataProviderMock.Stub(m => m.Clone())
                .Return(dataProviderMock)
                .Repeat.Any();
            dataProviderMock.Stub(m => m.GetCollection<FoodItemGroupProxy>(Arg<string>.Is.Anything))
                .Return(new List<FoodItemGroupProxy>())
                .Repeat.Any();
            dataProviderMock.Stub(m => m.GetCollection<TranslationProxy>(Arg<string>.Is.Anything))
                .Return(new List<TranslationProxy>())
                .Repeat.Any();
            dataProviderMock.Stub(m => m.GetCollection<ForeignKeyProxy>(Arg<string>.Is.Anything))
                .Return(new List<ForeignKeyProxy>())
                .Repeat.Any();

            var foodItemProxy = new FoodItemProxy
            {
                Identifier = Guid.NewGuid()
            };
            Assert.That(foodItemProxy, Is.Not.Null);
            Assert.That(foodItemProxy.Identifier, Is.Not.Null);
            Assert.That(foodItemProxy.Identifier.HasValue, Is.True);

            foodItemProxy.DeleteRelations(dataProviderMock);

            // ReSharper disable PossibleInvalidOperationException
            dataProviderMock.AssertWasCalled(m => m.GetCollection<TranslationProxy>(Arg<string>.Is.Equal(string.Format("SELECT t.TranslationIdentifier AS TranslationIdentifier,t.OfIdentifier AS OfIdentifier,ti.TranslationInfoIdentifier AS InfoIdentifier,ti.CultureName AS CultureName,t.Value AS Value FROM Translations AS t, TranslationInfos AS ti WHERE t.OfIdentifier='{0}' AND ti.TranslationInfoIdentifier=t.InfoIdentifier ORDER BY CultureName", foodItemProxy.Identifier.Value.ToString("D").ToUpper()))));
            // ReSharper restore PossibleInvalidOperationException
        }

        /// <summary>
        /// Tests that DeleteRelations calls Delete on the data provider for each translation for the data proxy food item.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsCallsDeleteOnDataProviderForEachTranslations()
        {
            var fixture = new Fixture();

            var translationProxyCollection = fixture.CreateMany<TranslationProxy>(7).ToList();
            var dataProviderMock = MockRepository.GenerateStub<IDataProviderBase>();
            dataProviderMock.Stub(m => m.Clone())
                .Return(dataProviderMock)
                .Repeat.Any();
            dataProviderMock.Stub(m => m.GetCollection<FoodItemGroupProxy>(Arg<string>.Is.Anything))
                .Return(new List<FoodItemGroupProxy>())
                .Repeat.Any();
            dataProviderMock.Stub(m => m.GetCollection<TranslationProxy>(Arg<string>.Is.Anything))
                .Return(translationProxyCollection)
                .Repeat.Any();
            dataProviderMock.Stub(m => m.GetCollection<ForeignKeyProxy>(Arg<string>.Is.Anything))
                .Return(new List<ForeignKeyProxy>())
                .Repeat.Any();
            dataProviderMock.Stub(m => m.Delete(Arg<TranslationProxy>.Is.NotNull))
                .WhenCalled(e =>
                {
                    var translationProxy = (TranslationProxy) e.Arguments.ElementAt(0);
                    Assert.That(translationProxy, Is.Not.Null);
                    Assert.That(translationProxyCollection.Contains(translationProxy), Is.True);
                })
                .Repeat.Any();

            var foodItemProxy = new FoodItemProxy
            {
                Identifier = Guid.NewGuid()
            };
            Assert.That(foodItemProxy, Is.Not.Null);
            Assert.That(foodItemProxy.Identifier, Is.Not.Null);
            Assert.That(foodItemProxy.Identifier.HasValue, Is.True);

            foodItemProxy.DeleteRelations(dataProviderMock);

            dataProviderMock.AssertWasCalled(m => m.Delete(Arg<TranslationProxy>.Is.NotNull), opt => opt.Repeat.Times(translationProxyCollection.Count));
        }

        /// <summary>
        /// Tests that DeleteRelations calls GetCollection on the data provider to get the foreign keys for the data proxy food item.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsCallsGetCollectionOnDataProviderToGetForeignKeys()
        {
            var dataProviderMock = MockRepository.GenerateStub<IDataProviderBase>();
            dataProviderMock.Stub(m => m.Clone())
                .Return(dataProviderMock)
                .Repeat.Any();
            dataProviderMock.Stub(m => m.GetCollection<FoodItemGroupProxy>(Arg<string>.Is.Anything))
                .Return(new List<FoodItemGroupProxy>())
                .Repeat.Any();
            dataProviderMock.Stub(m => m.GetCollection<TranslationProxy>(Arg<string>.Is.Anything))
                .Return(new List<TranslationProxy>())
                .Repeat.Any();
            dataProviderMock.Stub(m => m.GetCollection<ForeignKeyProxy>(Arg<string>.Is.Anything))
                .Return(new List<ForeignKeyProxy>())
                .Repeat.Any();

            var foodItemProxy = new FoodItemProxy
            {
                Identifier = Guid.NewGuid()
            };
            Assert.That(foodItemProxy, Is.Not.Null);
            Assert.That(foodItemProxy.Identifier, Is.Not.Null);
            Assert.That(foodItemProxy.Identifier.HasValue, Is.True);

            foodItemProxy.DeleteRelations(dataProviderMock);

            // ReSharper disable PossibleInvalidOperationException
            dataProviderMock.AssertWasCalled(m => m.GetCollection<ForeignKeyProxy>(string.Format("SELECT ForeignKeyIdentifier,DataProviderIdentifier,ForeignKeyForIdentifier,ForeignKeyForTypes,ForeignKeyValue FROM ForeignKeys WHERE ForeignKeyForIdentifier='{0}' ORDER BY DataProviderIdentifier,ForeignKeyValue", foodItemProxy.Identifier.Value.ToString("D").ToUpper())));
            // ReSharper restore PossibleInvalidOperationException
        }

        /// <summary>
        /// Tests that DeleteRelations calls Delete on the data provider for each translation for the data proxy food item.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsCallsDeleteOnDataProviderForEachForeignKey()
        {
            var fixture = new Fixture();

            var foreignKeyProxyCollection = fixture.CreateMany<ForeignKeyProxy>(7).ToList();
            var dataProviderMock = MockRepository.GenerateStub<IDataProviderBase>();
            dataProviderMock.Stub(m => m.Clone())
                .Return(dataProviderMock)
                .Repeat.Any();
            dataProviderMock.Stub(m => m.GetCollection<FoodItemGroupProxy>(Arg<string>.Is.Anything))
                .Return(new List<FoodItemGroupProxy>())
                .Repeat.Any();
            dataProviderMock.Stub(m => m.GetCollection<TranslationProxy>(Arg<string>.Is.Anything))
                .Return(new List<TranslationProxy>())
                .Repeat.Any();
            dataProviderMock.Stub(m => m.GetCollection<ForeignKeyProxy>(Arg<string>.Is.Anything))
                .Return(foreignKeyProxyCollection)
                .Repeat.Any();
            dataProviderMock.Stub(m => m.Delete(Arg<ForeignKeyProxy>.Is.NotNull))
                .WhenCalled(e =>
                {
                    var foreignKeyProxy = (ForeignKeyProxy) e.Arguments.ElementAt(0);
                    Assert.That(foreignKeyProxy, Is.Not.Null);
                    Assert.That(foreignKeyProxyCollection.Contains(foreignKeyProxy), Is.True);
                })
                .Repeat.Any();

            var foodItemProxy = new FoodItemProxy
            {
                Identifier = Guid.NewGuid()
            };
            Assert.That(foodItemProxy, Is.Not.Null);
            Assert.That(foodItemProxy.Identifier, Is.Not.Null);
            Assert.That(foodItemProxy.Identifier.HasValue, Is.True);

            foodItemProxy.DeleteRelations(dataProviderMock);

            dataProviderMock.AssertWasCalled(m => m.Delete(Arg<ForeignKeyProxy>.Is.NotNull), opt => opt.Repeat.Times(foreignKeyProxyCollection.Count));
        }
    }
}
