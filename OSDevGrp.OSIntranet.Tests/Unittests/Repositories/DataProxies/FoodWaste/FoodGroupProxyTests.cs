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
using OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste;
using AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.DataProxies.FoodWaste
{
    /// <summary>
    /// Tests the data proxy to a given food group.
    /// </summary>
    [TestFixture]
    public class FoodGroupProxyTests
    {
        /// <summary>
        /// Tests that the constructor initialize a data proxy to a given food group.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeFoodGroupProxy()
        {
            var foodGroupProxy = new FoodGroupProxy();
            Assert.That(foodGroupProxy, Is.Not.Null);
            Assert.That(foodGroupProxy.Identifier, Is.Null);
            Assert.That(foodGroupProxy.Identifier.HasValue, Is.False);
            Assert.That(foodGroupProxy.Parent, Is.Null);
            Assert.That(foodGroupProxy.IsActive, Is.False);
            Assert.That(foodGroupProxy.Children, Is.Not.Null);
            Assert.That(foodGroupProxy.Children, Is.Empty);
            Assert.That(foodGroupProxy.Translation, Is.Null);
            Assert.That(foodGroupProxy.Translations, Is.Not.Null);
            Assert.That(foodGroupProxy.Translations, Is.Empty);
            Assert.That(foodGroupProxy.ForeignKeys, Is.Not.Null);
            Assert.That(foodGroupProxy.ForeignKeys, Is.Empty);
        }

        /// <summary>
        /// Tests that getter for UniqueId throws an IntranetRepositoryException when the food group has no identifier.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterThrowsIntranetRepositoryExceptionWhenFoodGroupProxyHasNoIdentifier()
        {
            var foodGroupProxy = new FoodGroupProxy
            {
                Identifier = null
            };

            // ReSharper disable UnusedVariable
            var exception = Assert.Throws<IntranetRepositoryException>(() => { var uniqueId = foodGroupProxy.UniqueId; });
            // ReSharper restore UnusedVariable
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, foodGroupProxy.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that getter for UniqueId gets the unique identifier for the food group.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterGetsUniqueIdentificationForFoodGroupProxy()
        {
            var foodGroupProxy = new FoodGroupProxy
            {
                Identifier = Guid.NewGuid()
            };

            var uniqueId = foodGroupProxy.UniqueId;
            Assert.That(uniqueId, Is.Not.Null);
            Assert.That(uniqueId, Is.Not.Empty);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(uniqueId, Is.EqualTo(foodGroupProxy.Identifier.Value.ToString("D").ToUpper()));
            // ReSharper restore PossibleInvalidOperationException
        }

        /// <summary>
        /// Tests that GetSqlQueryForId throws an ArgumentNullException when the given food group is null.
        /// </summary>
        [Test]
        public void TestThatGetSqlQueryForIdThrowsArgumentNullExceptionWhenFoodGroupIsNull()
        {
            var foodGroupProxy = new FoodGroupProxy();

            // ReSharper disable UnusedVariable
            var exception = Assert.Throws<ArgumentNullException>(() => { var sqlQueryForId = foodGroupProxy.GetSqlQueryForId(null); });
            // ReSharper restore UnusedVariable
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("foodGroup"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetSqlQueryForId throws an IntranetRepositoryException when the identifier on the given food group has no value.
        /// </summary>
        [Test]
        public void TestThatGetSqlQueryForIdThrowsIntranetRepositoryExceptionWhenIdentifierOnFoodGroupHasNoValue()
        {
            var foodGroupMock = MockRepository.GenerateMock<IFoodGroup>();
            foodGroupMock.Expect(m => m.Identifier)
                .Return(null)
                .Repeat.Any();

            var foodGroupProxy = new FoodGroupProxy();

            // ReSharper disable UnusedVariable
            var exception = Assert.Throws<IntranetRepositoryException>(() => { var sqlQueryForId = foodGroupProxy.GetSqlQueryForId(foodGroupMock); });
            // ReSharper restore UnusedVariable
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, foodGroupMock.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetSqlQueryForId returns the SQL statement for selecting the given food group.
        /// </summary>
        [Test]
        public void TestThatGetSqlQueryForIdReturnsSqlQueryForId()
        {
            var foodGroupMock = MockRepository.GenerateMock<IFoodGroup>();
            foodGroupMock.Expect(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodGroupProxy = new FoodGroupProxy();

            var sqlQueryForId = foodGroupProxy.GetSqlQueryForId(foodGroupMock);
            Assert.That(sqlQueryForId, Is.Not.Null);
            Assert.That(sqlQueryForId, Is.Not.Empty);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(sqlQueryForId, Is.EqualTo(string.Format("SELECT FoodGroupIdentifier,ParentIdentifier,IsActive FROM FoodGroups WHERE FoodGroupIdentifier='{0}'", foodGroupMock.Identifier.Value.ToString("D").ToUpper())));
            // ReSharper restore PossibleInvalidOperationException
        }

        /// <summary>
        /// Tests that GetSqlCommandForInsert returns the SQL statement to insert this food group.
        /// </summary>
        [Test]
        public void TestThatGetSqlCommandForInsertReturnsSqlCommandForInsert()
        {
            var fixture = new Fixture();

            var foodGroupProxy = new FoodGroupProxy
            {
                Identifier = Guid.NewGuid(),
                Parent = DomainObjectMockBuilder.BuildFoodGroupMock(),
                IsActive = fixture.Create<bool>()
            };

            var sqlCommand = foodGroupProxy.GetSqlCommandForInsert();
            Assert.That(sqlCommand, Is.Not.Null);
            Assert.That(sqlCommand, Is.Not.Empty);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(sqlCommand, Is.EqualTo(string.Format("INSERT INTO FoodGroups (FoodGroupIdentifier,ParentIdentifier,IsActive) VALUES('{0}','{1}',{2})", foodGroupProxy.UniqueId, foodGroupProxy.Parent.Identifier.Value.ToString("D").ToUpper(), Convert.ToInt32(foodGroupProxy.IsActive))));
            // ReSharper restore PossibleInvalidOperationException
        }

        /// <summary>
        /// Tests that GetSqlCommandForUpdate returns the SQL statement to update this food group.
        /// </summary>
        [Test]
        public void TestThatGetSqlCommandForUpdateReturnsSqlCommandForUpdate()
        {
            var fixture = new Fixture();

            var foodGroupProxy = new FoodGroupProxy
            {
                Identifier = Guid.NewGuid(),
                Parent = DomainObjectMockBuilder.BuildFoodGroupMock(),
                IsActive = fixture.Create<bool>()
            };

            var sqlCommand = foodGroupProxy.GetSqlCommandForUpdate();
            Assert.That(sqlCommand, Is.Not.Null);
            Assert.That(sqlCommand, Is.Not.Empty);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(sqlCommand, Is.EqualTo(string.Format("UPDATE FoodGroups SET ParentIdentifier='{1}',IsActive={2} WHERE FoodGroupIdentifier='{0}'", foodGroupProxy.UniqueId, foodGroupProxy.Parent.Identifier.Value.ToString("D").ToUpper(), Convert.ToInt32(foodGroupProxy.IsActive))));
            // ReSharper restore PossibleInvalidOperationException
        }

        /// <summary>
        /// Tests that GetSqlCommandForDelete returns the SQL statement to delete this food group.
        /// </summary>
        [Test]
        public void TestThatGetSqlCommandForDeleteReturnsSqlCommandForDelete()
        {
            var foodGroupProxy = new FoodGroupProxy
            {
                Identifier = Guid.NewGuid()
            };

            var sqlCommand = foodGroupProxy.GetSqlCommandForDelete();
            Assert.That(sqlCommand, Is.Not.Null);
            Assert.That(sqlCommand, Is.Not.Empty);
            Assert.That(sqlCommand, Is.EqualTo(string.Format("DELETE FROM FoodGroups WHERE FoodGroupIdentifier='{0}'", foodGroupProxy.UniqueId)));
        }

        /// <summary>
        /// Tests that MapData throws an ArgumentNullException if the data reader is null.
        /// </summary>
        [Test]
        public void TestThatMapDataThrowsArgumentNullExceptionIfDataReaderIsNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IDataProviderBase>(e => e.FromFactory(() => MockRepository.GenerateMock<IDataProviderBase>()));

            var foodGroupProxy = new FoodGroupProxy();
            Assert.That(foodGroupProxy, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodGroupProxy.MapData(null, fixture.Create<IDataProviderBase>()));
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

            var foodGroupProxy = new FoodGroupProxy();
            Assert.That(foodGroupProxy, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodGroupProxy.MapData(fixture.Create<MySqlDataReader>(), null));
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

            var foodGroupProxy = new FoodGroupProxy();
            Assert.That(foodGroupProxy, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => foodGroupProxy.MapData(dataReader, fixture.Create<IDataProviderBase>()));
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
        public void TestThatMapDataAndMapRelationsMapsDataIntoProxy()
        {
            var fixture = new Fixture();

            var parentFoodGroupProxy = fixture.Build<FoodGroupProxy>()
                .With(m => m.Parent, null)
                .Create();
            var childrenFoodGroupProxyCollection = new List<FoodGroupProxy>
            {
                fixture.Build<FoodGroupProxy>()
                    .With(m => m.Parent, null)
                    .Create(),
                fixture.Build<FoodGroupProxy>()
                    .With(m => m.Parent, null)
                    .Create(),
                fixture.Build<FoodGroupProxy>()
                    .With(m => m.Parent, null)
                    .Create()
            };
            var translationProxyCollection = fixture.CreateMany<TranslationProxy>(3).ToList();
            var foreignKeyProxyCollection = fixture.CreateMany<ForeignKeyProxy>(5).ToList();

            var dataProviderBaseMock = MockRepository.GenerateMock<IDataProviderBase>();
            dataProviderBaseMock.Stub(m => m.Clone())
                .Return(dataProviderBaseMock)
                .Repeat.Any();
            dataProviderBaseMock.Stub(m => m.Get(Arg<FoodGroupProxy>.Is.NotNull))
                .WhenCalled(e =>
                {
                    var proxy = (FoodGroupProxy) e.Arguments.ElementAt(0);
                    Assert.That(proxy.Identifier, Is.Not.Null);
                    Assert.That(proxy.Identifier.HasValue, Is.True);
                    // ReSharper disable PossibleInvalidOperationException
                    Assert.That(proxy.Identifier.Value.ToString("D").ToUpper(), Is.EqualTo("59C97C78-A496-42B9-A978-7DFCBB2C2039"));
                    // ReSharper restore PossibleInvalidOperationException
                })
                .Return(parentFoodGroupProxy)
                .Repeat.Any();
            dataProviderBaseMock.Stub(m => m.GetCollection<FoodGroupProxy>(Arg<string>.Is.Anything))
                .Return(childrenFoodGroupProxyCollection)
                .Repeat.Any();
            dataProviderBaseMock.Stub(m => m.GetCollection<TranslationProxy>(Arg<string>.Is.Anything))
                .Return(translationProxyCollection)
                .Repeat.Any();
            dataProviderBaseMock.Stub(m => m.GetCollection<ForeignKeyProxy>(Arg<string>.Is.Anything))
                .Return(foreignKeyProxyCollection)
                .Repeat.Any();

            var dataReader = MockRepository.GenerateStub<MySqlDataReader>();
            dataReader.Stub(m => m.GetString(Arg<string>.Is.Equal("FoodGroupIdentifier")))
                .Return("68016998-2A2F-40CB-B691-41379B7C5778")
                .Repeat.Any();
            dataReader.Stub(m => m.GetOrdinal(Arg<string>.Is.Equal("ParentIdentifier")))
                .Return(2)
                .Repeat.Any();
            dataReader.Stub(m => m.IsDBNull(Arg<int>.Is.Equal(2)))
                .Return(false)
                .Repeat.Any();
            dataReader.Stub(m => m.GetString(Arg<int>.Is.Equal(2)))
                .Return("59C97C78-A496-42B9-A978-7DFCBB2C2039")
                .Repeat.Any();
            dataReader.Stub(m => m.GetInt32(Arg<string>.Is.Equal("IsActive")))
                .Return(1)
                .Repeat.Any();

            var foodGroupProxy = new FoodGroupProxy();
            Assert.That(foodGroupProxy, Is.Not.Null);
            Assert.That(foodGroupProxy.Identifier, Is.Null);
            Assert.That(foodGroupProxy.Identifier.HasValue, Is.False);
            Assert.That(foodGroupProxy.Parent, Is.Null);
            Assert.That(foodGroupProxy.IsActive, Is.False);
            Assert.That(foodGroupProxy.Children, Is.Not.Null);
            Assert.That(foodGroupProxy.Children, Is.Empty);
            Assert.That(foodGroupProxy.Translation, Is.Null);
            Assert.That(foodGroupProxy.Translations, Is.Not.Null);
            Assert.That(foodGroupProxy.Translations, Is.Empty);
            Assert.That(foodGroupProxy.ForeignKeys, Is.Not.Null);
            Assert.That(foodGroupProxy.ForeignKeys, Is.Empty);

            foodGroupProxy.MapData(dataReader, dataProviderBaseMock);
            foodGroupProxy.MapRelations(dataProviderBaseMock);
            Assert.That(foodGroupProxy, Is.Not.Null);
            Assert.That(foodGroupProxy.Identifier, Is.Not.Null);
            Assert.That(foodGroupProxy.Identifier.HasValue, Is.True);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(foodGroupProxy.Identifier.Value.ToString("D").ToUpper(), Is.EqualTo(dataReader.GetString("FoodGroupIdentifier")));
            // ReSharper restore PossibleInvalidOperationException
            Assert.That(foodGroupProxy.Parent, Is.Not.Null);
            Assert.That(foodGroupProxy.Parent, Is.EqualTo(parentFoodGroupProxy));
            Assert.That(foodGroupProxy.IsActive, Is.True);
            Assert.That(foodGroupProxy.Children, Is.Not.Null);
            Assert.That(foodGroupProxy.Children, Is.Not.Empty);
            Assert.That(foodGroupProxy.Children.Count(), Is.EqualTo(childrenFoodGroupProxyCollection.Count));
            foreach (var childFoodGroupProxy in childrenFoodGroupProxyCollection)
            {
                Assert.That(foodGroupProxy.Children.Contains(childFoodGroupProxy), Is.True);
            }
            Assert.That(foodGroupProxy.Translation, Is.Null);
            Assert.That(foodGroupProxy.Translations, Is.Not.Null);
            Assert.That(foodGroupProxy.Translations, Is.Not.Empty);
            Assert.That(foodGroupProxy.Translations.Count(), Is.EqualTo(translationProxyCollection.Count));
            foreach (var translationProxy in translationProxyCollection)
            {
                Assert.That(foodGroupProxy.Translations.Contains(translationProxy), Is.True);
            }
            Assert.That(foodGroupProxy.ForeignKeys, Is.Not.Null);
            Assert.That(foodGroupProxy.ForeignKeys, Is.Not.Empty);
            Assert.That(foodGroupProxy.ForeignKeys.Count(), Is.EqualTo(foreignKeyProxyCollection.Count));
            foreach (var foreignKeyProxy in foreignKeyProxyCollection)
            {
                Assert.That(foodGroupProxy.ForeignKeys.Contains(foreignKeyProxy), Is.True);
            }

            dataProviderBaseMock.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Times(4));
            dataProviderBaseMock.AssertWasCalled(m => m.Get(Arg<FoodGroupProxy>.Is.NotNull));
            dataProviderBaseMock.AssertWasCalled(m => m.GetCollection<FoodGroupProxy>(Arg<string>.Is.Equal(string.Format("SELECT FoodGroupIdentifier,ParentIdentifier,IsActive FROM FoodGroups WHERE ParentIdentifier='{0}'", dataReader.GetString("FoodGroupIdentifier")))));
            dataProviderBaseMock.AssertWasCalled(m => m.GetCollection<TranslationProxy>(Arg<string>.Is.Equal(string.Format("SELECT t.TranslationIdentifier AS TranslationIdentifier,t.OfIdentifier AS OfIdentifier,ti.TranslationInfoIdentifier AS InfoIdentifier,ti.CultureName AS CultureName,t.Value AS Value FROM Translations AS t, TranslationInfos AS ti WHERE t.OfIdentifier='{0}' AND ti.TranslationInfoIdentifier=t.InfoIdentifier ORDER BY CultureName", dataReader.GetString("FoodGroupIdentifier")))));
            dataProviderBaseMock.AssertWasCalled(m => m.GetCollection<ForeignKeyProxy>(string.Format("SELECT ForeignKeyIdentifier,DataProviderIdentifier,ForeignKeyForIdentifier,ForeignKeyForTypes,ForeignKeyValue FROM ForeignKeys WHERE ForeignKeyForIdentifier='{0}' ORDER BY DataProviderIdentifier,ForeignKeyValue", dataReader.GetString("FoodGroupIdentifier"))));
        }

        /// <summary>
        /// Tests that MapRelations throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatMapRelationsThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            var fixture = new Fixture();
            fixture.Customize<MySqlDataReader>(e => e.FromFactory(() => MockRepository.GenerateStub<MySqlDataReader>()));

            var foodGroupProxy = new FoodGroupProxy();
            Assert.That(foodGroupProxy, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodGroupProxy.MapRelations(null));
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

            var foodGroupProxy = new FoodGroupProxy();
            Assert.That(foodGroupProxy, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodGroupProxy.SaveRelations(null, fixture.Create<bool>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("dataProvider"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that SaveRelations throws an IntranetRepositoryException when the identifier for the food group is null.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsThrowsIntranetRepositoryExceptionWhenIdentifierIsNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IDataProviderBase>(e => e.FromFactory(() => MockRepository.GenerateStub<IDataProviderBase>()));

            var foodGroupProxy = new FoodGroupProxy
            {
                Identifier = null
            };
            Assert.That(foodGroupProxy, Is.Not.Null);
            Assert.That(foodGroupProxy.Identifier, Is.Null);
            Assert.That(foodGroupProxy.Identifier.HasValue, Is.False);

            var exception = Assert.Throws<IntranetRepositoryException>(() => foodGroupProxy.SaveRelations(fixture.Create<IDataProviderBase>(), fixture.Create<bool>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, foodGroupProxy.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that DeleteRelations throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            var foodGroupProxy = new FoodGroupProxy();
            Assert.That(foodGroupProxy, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodGroupProxy.DeleteRelations(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("dataProvider"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that DeleteRelations throws an IntranetRepositoryException when the identifier for the food group is null.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsThrowsIntranetRepositoryExceptionWhenIdentifierIsNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IDataProviderBase>(e => e.FromFactory(() => MockRepository.GenerateStub<IDataProviderBase>()));

            var foodGroupProxy = new FoodGroupProxy
            {
                Identifier = null
            };
            Assert.That(foodGroupProxy, Is.Not.Null);
            Assert.That(foodGroupProxy.Identifier, Is.Null);
            Assert.That(foodGroupProxy.Identifier.HasValue, Is.False);

            var exception = Assert.Throws<IntranetRepositoryException>(() => foodGroupProxy.DeleteRelations(fixture.Create<IDataProviderBase>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, foodGroupProxy.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that DeleteRelations calls Clone on the data provider four times.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsCallsCloneOnDataProvider4Times()
        {
            var dataProviderMock = MockRepository.GenerateStub<IDataProviderBase>();
            dataProviderMock.Stub(m => m.Clone())
                .Return(dataProviderMock)
                .Repeat.Any();
            dataProviderMock.Stub(m => m.GetCollection<FoodGroupProxy>(Arg<string>.Is.Anything))
                .Return(new List<FoodGroupProxy>())
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

            var foodGroupProxy = new FoodGroupProxy
            {
                Identifier = Guid.NewGuid()
            };
            Assert.That(foodGroupProxy, Is.Not.Null);
            Assert.That(foodGroupProxy.Identifier, Is.Not.Null);
            Assert.That(foodGroupProxy.Identifier.HasValue, Is.True);

            foodGroupProxy.DeleteRelations(dataProviderMock);

            dataProviderMock.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Times(4));
        }

        /// <summary>
        /// Tests that DeleteRelations calls GetCollection on the data provider to get the foods groups which has this the data proxy food group as a parent.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsCallsGetCollectionOnDataProviderToGetChildren()
        {
            var dataProviderMock = MockRepository.GenerateStub<IDataProviderBase>();
            dataProviderMock.Stub(m => m.Clone())
                .Return(dataProviderMock)
                .Repeat.Any();
            dataProviderMock.Stub(m => m.GetCollection<FoodGroupProxy>(Arg<string>.Is.Anything))
                .Return(new List<FoodGroupProxy>())
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

            var foodGroupProxy = new FoodGroupProxy
            {
                Identifier = Guid.NewGuid()
            };
            Assert.That(foodGroupProxy, Is.Not.Null);
            Assert.That(foodGroupProxy.Identifier, Is.Not.Null);
            Assert.That(foodGroupProxy.Identifier.HasValue, Is.True);

            foodGroupProxy.DeleteRelations(dataProviderMock);

            // ReSharper disable PossibleInvalidOperationException
            dataProviderMock.AssertWasCalled(m => m.GetCollection<FoodGroupProxy>(Arg<string>.Is.Equal(string.Format("SELECT FoodGroupIdentifier,ParentIdentifier,IsActive FROM FoodGroups WHERE ParentIdentifier='{0}'", foodGroupProxy.Identifier.Value.ToString("D").ToUpper()))));
            // ReSharper restore PossibleInvalidOperationException
        }

        /// <summary>
        /// Tests that DeleteRelations calls Delete on the data provider for each foods groups which has this the data proxy food group as a parent.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsCallsDeleteOnDataProviderForEachChildren()
        {
            var fixture = new Fixture();

            var childrenFoodGroupProxyCollection = new List<FoodGroupProxy>
            {
                fixture.Build<FoodGroupProxy>()
                    .With(m => m.Parent, null)
                    .Create(),
                fixture.Build<FoodGroupProxy>()
                    .With(m => m.Parent, null)
                    .Create(),
                fixture.Build<FoodGroupProxy>()
                    .With(m => m.Parent, null)
                    .Create()
            };
            var dataProviderMock = MockRepository.GenerateStub<IDataProviderBase>();
            dataProviderMock.Stub(m => m.Clone())
                .Return(dataProviderMock)
                .Repeat.Any();
            dataProviderMock.Stub(m => m.GetCollection<FoodGroupProxy>(Arg<string>.Is.Anything))
                .Return(childrenFoodGroupProxyCollection)
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
            dataProviderMock.Stub(m => m.Delete(Arg<FoodGroupProxy>.Is.NotNull))
                .WhenCalled(e =>
                {
                    var childFoodGroupProxy = (FoodGroupProxy) e.Arguments.ElementAt(0);
                    Assert.That(childFoodGroupProxy, Is.Not.Null);
                    Assert.That(childrenFoodGroupProxyCollection.Contains(childFoodGroupProxy), Is.True);
                })
                .Repeat.Any();

            var foodGroupProxy = new FoodGroupProxy
            {
                Identifier = Guid.NewGuid()
            };
            Assert.That(foodGroupProxy, Is.Not.Null);
            Assert.That(foodGroupProxy.Identifier, Is.Not.Null);
            Assert.That(foodGroupProxy.Identifier.HasValue, Is.True);

            foodGroupProxy.DeleteRelations(dataProviderMock);

            dataProviderMock.AssertWasCalled(m => m.Delete(Arg<FoodGroupProxy>.Is.NotNull), opt => opt.Repeat.Times(childrenFoodGroupProxyCollection.Count));
        }

        /// <summary>
        /// Tests that DeleteRelations calls GetCollection on the data provider to get the releations between the food group and it's food items.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsCallsGetCollectionOnDataProviderToGetFoodItemGroups()
        {
            var dataProviderMock = MockRepository.GenerateStub<IDataProviderBase>();
            dataProviderMock.Stub(m => m.Clone())
                .Return(dataProviderMock)
                .Repeat.Any();
            dataProviderMock.Stub(m => m.GetCollection<FoodGroupProxy>(Arg<string>.Is.Anything))
                .Return(new List<FoodGroupProxy>())
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

            var foodGroupProxy = new FoodGroupProxy
            {
                Identifier = Guid.NewGuid()
            };
            Assert.That(foodGroupProxy, Is.Not.Null);
            Assert.That(foodGroupProxy.Identifier, Is.Not.Null);
            Assert.That(foodGroupProxy.Identifier.HasValue, Is.True);

            foodGroupProxy.DeleteRelations(dataProviderMock);

            // ReSharper disable PossibleInvalidOperationException
            dataProviderMock.AssertWasCalled(m => m.GetCollection<FoodItemGroupProxy>(Arg<string>.Is.Equal(string.Format("SELECT FoodItemGroupIdentifier,FoodItemIdentifier,FoodGroupIdentifier,IsPrimary FROM FoodItemGroups WHERE FoodGroupIdentifier='{0}'", foodGroupProxy.Identifier.Value.ToString("D").ToUpper()))));
            // ReSharper restore PossibleInvalidOperationException
        }

        /// <summary>
        /// Tests that DeleteRelations calls Delete on the data provider for each relation between the food group and it's food items.
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
            dataProviderMock.Stub(m => m.GetCollection<FoodGroupProxy>(Arg<string>.Is.Anything))
                .Return(new List<FoodGroupProxy>())
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

            var foodGroupProxy = new FoodGroupProxy
            {
                Identifier = Guid.NewGuid()
            };
            Assert.That(foodGroupProxy, Is.Not.Null);
            Assert.That(foodGroupProxy.Identifier, Is.Not.Null);
            Assert.That(foodGroupProxy.Identifier.HasValue, Is.True);

            foodGroupProxy.DeleteRelations(dataProviderMock);

            dataProviderMock.AssertWasCalled(m => m.Delete(Arg<FoodItemGroupProxy>.Is.NotNull), opt => opt.Repeat.Times(foodItemGroupProxyCollection.Count));
        }

        /// <summary>
        /// Tests that DeleteRelations calls GetCollection on the data provider to get the translation for the data proxy food group.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsCallsGetCollectionOnDataProviderToGetTranslations()
        {
            var dataProviderMock = MockRepository.GenerateStub<IDataProviderBase>();
            dataProviderMock.Stub(m => m.Clone())
                .Return(dataProviderMock)
                .Repeat.Any();
            dataProviderMock.Stub(m => m.GetCollection<FoodGroupProxy>(Arg<string>.Is.Anything))
                .Return(new List<FoodGroupProxy>())
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

            var foodGroupProxy = new FoodGroupProxy
            {
                Identifier = Guid.NewGuid()
            };
            Assert.That(foodGroupProxy, Is.Not.Null);
            Assert.That(foodGroupProxy.Identifier, Is.Not.Null);
            Assert.That(foodGroupProxy.Identifier.HasValue, Is.True);

            foodGroupProxy.DeleteRelations(dataProviderMock);

            // ReSharper disable PossibleInvalidOperationException
            dataProviderMock.AssertWasCalled(m => m.GetCollection<TranslationProxy>(Arg<string>.Is.Equal(string.Format("SELECT t.TranslationIdentifier AS TranslationIdentifier,t.OfIdentifier AS OfIdentifier,ti.TranslationInfoIdentifier AS InfoIdentifier,ti.CultureName AS CultureName,t.Value AS Value FROM Translations AS t, TranslationInfos AS ti WHERE t.OfIdentifier='{0}' AND ti.TranslationInfoIdentifier=t.InfoIdentifier ORDER BY CultureName", foodGroupProxy.Identifier.Value.ToString("D").ToUpper()))));
            // ReSharper restore PossibleInvalidOperationException
        }

        /// <summary>
        /// Tests that DeleteRelations calls Delete on the data provider for each translation for the data proxy food group.
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
            dataProviderMock.Stub(m => m.GetCollection<FoodGroupProxy>(Arg<string>.Is.Anything))
                .Return(new List<FoodGroupProxy>())
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

            var foodGroupProxy = new FoodGroupProxy
            {
                Identifier = Guid.NewGuid()
            };
            Assert.That(foodGroupProxy, Is.Not.Null);
            Assert.That(foodGroupProxy.Identifier, Is.Not.Null);
            Assert.That(foodGroupProxy.Identifier.HasValue, Is.True);

            foodGroupProxy.DeleteRelations(dataProviderMock);

            dataProviderMock.AssertWasCalled(m => m.Delete(Arg<TranslationProxy>.Is.NotNull), opt => opt.Repeat.Times(translationProxyCollection.Count));
        }

        /// <summary>
        /// Tests that DeleteRelations calls GetCollection on the data provider to get the foreign keys for the data proxy food group.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsCallsGetCollectionOnDataProviderToGetForeignKeys()
        {
            var dataProviderMock = MockRepository.GenerateStub<IDataProviderBase>();
            dataProviderMock.Stub(m => m.Clone())
                .Return(dataProviderMock)
                .Repeat.Any();
            dataProviderMock.Stub(m => m.GetCollection<FoodGroupProxy>(Arg<string>.Is.Anything))
                .Return(new List<FoodGroupProxy>())
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

            var foodGroupProxy = new FoodGroupProxy
            {
                Identifier = Guid.NewGuid()
            };
            Assert.That(foodGroupProxy, Is.Not.Null);
            Assert.That(foodGroupProxy.Identifier, Is.Not.Null);
            Assert.That(foodGroupProxy.Identifier.HasValue, Is.True);

            foodGroupProxy.DeleteRelations(dataProviderMock);

            // ReSharper disable PossibleInvalidOperationException
            dataProviderMock.AssertWasCalled(m => m.GetCollection<ForeignKeyProxy>(string.Format("SELECT ForeignKeyIdentifier,DataProviderIdentifier,ForeignKeyForIdentifier,ForeignKeyForTypes,ForeignKeyValue FROM ForeignKeys WHERE ForeignKeyForIdentifier='{0}' ORDER BY DataProviderIdentifier,ForeignKeyValue", foodGroupProxy.Identifier.Value.ToString("D").ToUpper())));
            // ReSharper restore PossibleInvalidOperationException
        }

        /// <summary>
        /// Tests that DeleteRelations calls Delete on the data provider for each translation for the data proxy food group.
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
            dataProviderMock.Stub(m => m.GetCollection<FoodGroupProxy>(Arg<string>.Is.Anything))
                .Return(new List<FoodGroupProxy>())
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

            var foodGroupProxy = new FoodGroupProxy
            {
                Identifier = Guid.NewGuid()
            };
            Assert.That(foodGroupProxy, Is.Not.Null);
            Assert.That(foodGroupProxy.Identifier, Is.Not.Null);
            Assert.That(foodGroupProxy.Identifier.HasValue, Is.True);

            foodGroupProxy.DeleteRelations(dataProviderMock);

            dataProviderMock.AssertWasCalled(m => m.Delete(Arg<ForeignKeyProxy>.Is.NotNull), opt => opt.Repeat.Times(foreignKeyProxyCollection.Count));
        }
    }
}
