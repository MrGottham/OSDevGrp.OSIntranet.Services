using System;
using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Resources;
using AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.DataProxies.FoodWaste
{
    /// <summary>
    /// Tests the data proxy to a given relation between a food item and a food group in the food waste domain.
    /// </summary>
    [TestFixture]
    public class FoodItemGroupProxyTests
    {
        /// <summary>
        /// Tests that the constructor initialize a data proxy to a given relation between a food item and a food group.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeFoodItemGroupProxy()
        {
            var foodItemGroupProxy = new FoodItemGroupProxy();
            Assert.That(foodItemGroupProxy, Is.Not.Null);
            Assert.That(foodItemGroupProxy.Identifier, Is.Null);
            Assert.That(foodItemGroupProxy.Identifier.HasValue, Is.False);
            Assert.That(foodItemGroupProxy.FoodItem, Is.Null);
            Assert.That(foodItemGroupProxy.FoodItemIdentifier, Is.Null);
            Assert.That(foodItemGroupProxy.FoodItemIdentifier.HasValue, Is.False);
            Assert.That(foodItemGroupProxy.FoodGroup, Is.Null);
            Assert.That(foodItemGroupProxy.FoodGroupIdentifier, Is.Null);
            Assert.That(foodItemGroupProxy.FoodGroupIdentifier.HasValue, Is.False);
            Assert.That(foodItemGroupProxy.IsPrimary, Is.False);
        }

        /// <summary>
        /// Tests that getter for UniqueId throws an IntranetRepositoryException when the relation between a food item and a food group has no identifier.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterThrowsIntranetRepositoryExceptionWhenFoodItemGroupProxyHasNoIdentifier()
        {
            var foodItemGroupProxy = new FoodItemGroupProxy
            {
                Identifier = null
            };

            // ReSharper disable UnusedVariable
            var exception = Assert.Throws<IntranetRepositoryException>(() => { var uniqueId = foodItemGroupProxy.UniqueId; });
            // ReSharper restore UnusedVariable
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, foodItemGroupProxy.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that getter for UniqueId gets the unique identifier for the relation between a food item and a food group.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterGetsUniqueIdentificationForFoodItemGroupProxy()
        {
            var foodItemGroupProxy = new FoodItemGroupProxy
            {
                Identifier = Guid.NewGuid()
            };

            var uniqueId = foodItemGroupProxy.UniqueId;
            Assert.That(uniqueId, Is.Not.Null);
            Assert.That(uniqueId, Is.Not.Empty);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(uniqueId, Is.EqualTo(foodItemGroupProxy.Identifier.Value.ToString("D").ToUpper()));
            // ReSharper restore PossibleInvalidOperationException
        }

        /// <summary>
        /// Tests that GetSqlQueryForId throws an ArgumentNullException when the given relation between a food item and a food group is null.
        /// </summary>
        [Test]
        public void TestThatGetSqlQueryForIdThrowsArgumentNullExceptionWhenFoodItemGroupProxyIsNull()
        {
            var foodItemGroupProxy = new FoodItemGroupProxy();

            // ReSharper disable UnusedVariable
            var exception = Assert.Throws<ArgumentNullException>(() => { var sqlQueryForId = foodItemGroupProxy.GetSqlQueryForId(null); });
            // ReSharper restore UnusedVariable
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("foodItemGroup"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetSqlQueryForId throws an IntranetRepositoryException when the identifier on the given relation between a food item and a food group has no value.
        /// </summary>
        [Test]
        public void TestThatGetSqlQueryForIdThrowsIntranetRepositoryExceptionWhenIdentifierOnFoodItemGroupHasNoValue()
        {
            var foodItemGroupMock = MockRepository.GenerateMock<IFoodItemGroupProxy>();
            foodItemGroupMock.Expect(m => m.Identifier)
                .Return(null)
                .Repeat.Any();

            var foodItemGroupProxy = new FoodItemGroupProxy();

            // ReSharper disable UnusedVariable
            var exception = Assert.Throws<IntranetRepositoryException>(() => { var sqlQueryForId = foodItemGroupProxy.GetSqlQueryForId(foodItemGroupMock); });
            // ReSharper restore UnusedVariable
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, foodItemGroupMock.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetSqlQueryForId returns the SQL statement for selecting the given relation between a food item and a food group.
        /// </summary>
        [Test]
        public void TestThatGetSqlQueryForIdReturnsSqlQueryForId()
        {
            var foodItemGroupMock = MockRepository.GenerateMock<IFoodItemGroupProxy>();
            foodItemGroupMock.Expect(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var foodItemGroupProxy = new FoodItemGroupProxy();

            var sqlQueryForId = foodItemGroupProxy.GetSqlQueryForId(foodItemGroupMock);
            Assert.That(sqlQueryForId, Is.Not.Null);
            Assert.That(sqlQueryForId, Is.Not.Empty);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(sqlQueryForId, Is.EqualTo(string.Format("SELECT FoodItemGroupIdentifier,FoodItemIdentifier,FoodGroupIdentifier,IsPrimary FROM FoodItemGroups WHERE FoodItemGroupIdentifier='{0}'", foodItemGroupMock.Identifier.Value.ToString("D").ToUpper())));
            // ReSharper restore PossibleInvalidOperationException
        }

        /// <summary>
        /// Tests that GetSqlCommandForInsert throws an IntranetRepositoryException when the identifier for the food item on the given relation between a food item and a food group has no value.
        /// </summary>
        [Test]
        public void TestThatGetSqlCommandForInsertThrowsIntranetRepositoryExceptionWhenFoodItemIdentifierOnFoodItemGroupHasNoValue()
        {
            var foodItemGroupProxy = new FoodItemGroupProxy
            {
                Identifier = Guid.NewGuid(),
                FoodItemIdentifier = null,
                FoodGroupIdentifier = Guid.NewGuid()
            };

            // ReSharper disable UnusedVariable
            var exception = Assert.Throws<IntranetRepositoryException>(() => { var sqlCommandForInsert = foodItemGroupProxy.GetSqlCommandForInsert(); });
            // ReSharper restore UnusedVariable
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, foodItemGroupProxy.FoodItemIdentifier, "FoodItemIdentifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetSqlCommandForInsert throws an IntranetRepositoryException when the identifier for the food group on the given relation between a food item and a food group has no value.
        /// </summary>
        [Test]
        public void TestThatGetSqlCommandForInsertThrowsIntranetRepositoryExceptionWhenFoodGroupIdentifierOnFoodItemGroupHasNoValue()
        {
            var foodItemGroupProxy = new FoodItemGroupProxy
            {
                Identifier = Guid.NewGuid(),
                FoodItemIdentifier = Guid.NewGuid(),
                FoodGroupIdentifier = null
            };

            // ReSharper disable UnusedVariable
            var exception = Assert.Throws<IntranetRepositoryException>(() => { var sqlCommandForInsert = foodItemGroupProxy.GetSqlCommandForInsert(); });
            // ReSharper restore UnusedVariable
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, foodItemGroupProxy.FoodGroupIdentifier, "FoodGroupIdentifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetSqlCommandForInsert returns the SQL statement to insert this relation between a food item and a food group.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatGetSqlCommandForInsertReturnsSqlCommandForInsert(bool isPrimary)
        {
            var foodItemGroupProxy = new FoodItemGroupProxy
            {
                Identifier = Guid.NewGuid(),
                FoodItemIdentifier = Guid.NewGuid(),
                FoodGroupIdentifier = Guid.NewGuid(),
                IsPrimary = isPrimary
            };

            var sqlCommand = foodItemGroupProxy.GetSqlCommandForInsert();
            Assert.That(sqlCommand, Is.Not.Null);
            Assert.That(sqlCommand, Is.Not.Empty);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(sqlCommand, Is.EqualTo(string.Format("INSERT INTO FoodItemGroups (FoodItemGroupIdentifier,FoodItemIdentifier,FoodGroupIdentifier,IsPrimary) VALUES('{0}','{1}','{2}',{3})", foodItemGroupProxy.UniqueId, foodItemGroupProxy.FoodItemIdentifier.Value.ToString("D").ToUpper(), foodItemGroupProxy.FoodGroupIdentifier.Value.ToString("D").ToUpper(), Convert.ToInt32(foodItemGroupProxy.IsPrimary))));
            // ReSharper restore PossibleInvalidOperationException
        }

        /// <summary>
        /// Tests that GetSqlCommandForUpdate throws an IntranetRepositoryException when the identifier for the food item on the given relation between a food item and a food group has no value.
        /// </summary>
        [Test]
        public void TestThatGetSqlCommandForUpdateThrowsIntranetRepositoryExceptionWhenFoodItemIdentifierOnFoodItemGroupHasNoValue()
        {
            var foodItemGroupProxy = new FoodItemGroupProxy
            {
                Identifier = Guid.NewGuid(),
                FoodItemIdentifier = null,
                FoodGroupIdentifier = Guid.NewGuid()
            };

            // ReSharper disable UnusedVariable
            var exception = Assert.Throws<IntranetRepositoryException>(() => { var sqlCommandForUpdate = foodItemGroupProxy.GetSqlCommandForUpdate(); });
            // ReSharper restore UnusedVariable
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, foodItemGroupProxy.FoodItemIdentifier, "FoodItemIdentifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetSqlCommandForUpdate throws an IntranetRepositoryException when the identifier for the food group on the given relation between a food item and a food group has no value.
        /// </summary>
        [Test]
        public void TestThatGetSqlCommandForUpdateThrowsIntranetRepositoryExceptionWhenFoodGroupIdentifierOnFoodItemGroupHasNoValue()
        {
            var foodItemGroupProxy = new FoodItemGroupProxy
            {
                Identifier = Guid.NewGuid(),
                FoodItemIdentifier = Guid.NewGuid(),
                FoodGroupIdentifier = null
            };

            // ReSharper disable UnusedVariable
            var exception = Assert.Throws<IntranetRepositoryException>(() => { var sqlCommandForUpdate = foodItemGroupProxy.GetSqlCommandForUpdate(); });
            // ReSharper restore UnusedVariable
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, foodItemGroupProxy.FoodGroupIdentifier, "FoodGroupIdentifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetSqlCommandForUpdate returns the SQL statement to update this relation between a food item and a food group.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatGetSqlCommandForUpdateReturnsSqlCommandForUpdate(bool isPrimary)
        {
            var foodItemGroupProxy = new FoodItemGroupProxy
            {
                Identifier = Guid.NewGuid(),
                FoodItemIdentifier = Guid.NewGuid(),
                FoodGroupIdentifier = Guid.NewGuid(),
                IsPrimary = isPrimary
            };

            var sqlCommand = foodItemGroupProxy.GetSqlCommandForUpdate();
            Assert.That(sqlCommand, Is.Not.Null);
            Assert.That(sqlCommand, Is.Not.Empty);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(sqlCommand, Is.EqualTo(string.Format("UPDATE FoodItemGroups SET FoodItemIdentifier='{1}',FoodGroupIdentifier='{2}',IsPrimary={3} WHERE FoodItemGroupIdentifier='{0}'", foodItemGroupProxy.UniqueId, foodItemGroupProxy.FoodItemIdentifier.Value.ToString("D").ToUpper(), foodItemGroupProxy.FoodGroupIdentifier.Value.ToString("D").ToUpper(), Convert.ToInt32(foodItemGroupProxy.IsPrimary))));
            // ReSharper restore PossibleInvalidOperationException
        }

        /// <summary>
        /// Tests that GetSqlCommandForDelete returns the SQL statement to delete this relation between a food item and a food group.
        /// </summary>
        [Test]
        public void TestThatGetSqlCommandForDeleteReturnsSqlCommandForDelete()
        {
            var foodItemGroupProxy = new FoodItemGroupProxy
            {
                Identifier = Guid.NewGuid(),
            };

            var sqlCommand = foodItemGroupProxy.GetSqlCommandForDelete();
            Assert.That(sqlCommand, Is.Not.Null);
            Assert.That(sqlCommand, Is.Not.Empty);
            Assert.That(sqlCommand, Is.EqualTo(string.Format("DELETE FROM FoodItemGroups WHERE FoodItemGroupIdentifier='{0}'", foodItemGroupProxy.UniqueId)));
        }

        /// <summary>
        /// Tests that MapData throws an ArgumentNullException if the data reader is null.
        /// </summary>
        [Test]
        public void TestThatMapDataThrowsArgumentNullExceptionIfDataReaderIsNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IDataProviderBase<MySqlCommand>>(e => e.FromFactory(() => MockRepository.GenerateMock<IDataProviderBase<MySqlCommand>>()));

            var foodItemGroupProxy = new FoodItemGroupProxy();
            Assert.That(foodItemGroupProxy, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodItemGroupProxy.MapData(null, fixture.Create<IDataProviderBase<MySqlCommand>>()));
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

            var foodItemGroupProxy = new FoodItemGroupProxy();
            Assert.That(foodItemGroupProxy, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodItemGroupProxy.MapData(fixture.Create<MySqlDataReader>(), null));
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
            fixture.Customize<IDataProviderBase<MySqlCommand>>(e => e.FromFactory(() => MockRepository.GenerateMock<IDataProviderBase<MySqlCommand>>()));

            var dataReader = MockRepository.GenerateMock<IDataReader>();

            var foodItemGroupProxy = new FoodItemGroupProxy();
            Assert.That(foodItemGroupProxy, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => foodItemGroupProxy.MapData(dataReader, fixture.Create<IDataProviderBase<MySqlCommand>>()));
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
        public void TestThatMapDataAndMapRelationsMapsDataIntoProxy(bool isPrimary)
        {
            var fixture = new Fixture();
            var foodItemProxy = fixture.Build<FoodItemProxy>()
                .With(m => m.Identifier, new Guid("AD62F870-5DFF-4E96-ACE5-D73C06905585"))
                .Create();
            var foodGroupProxy = fixture.Build<FoodGroupProxy>()
                .With(m => m.Identifier, new Guid("32F94B7E-3D9F-43AE-A1B6-97405CB2B3A1"))
                .With(m => m.Parent, null)
                .Create();

            var dataProviderBaseMock = MockRepository.GenerateMock<IDataProviderBase<MySqlCommand>>();
            dataProviderBaseMock.Stub(m => m.Clone())
                .Return(dataProviderBaseMock)
                .Repeat.Any();
            dataProviderBaseMock.Stub(m => m.Get(Arg<FoodItemProxy>.Is.NotNull))
                .WhenCalled(e =>
                {
                    var proxy = (FoodItemProxy) e.Arguments.ElementAt(0);
                    Assert.That(proxy, Is.Not.Null);
                    Assert.That(proxy.Identifier, Is.Not.Null);
                    Assert.That(proxy.Identifier.HasValue, Is.True);
                    // ReSharper disable PossibleInvalidOperationException
                    Assert.That(proxy.Identifier.Value, Is.EqualTo(new Guid("AD62F870-5DFF-4E96-ACE5-D73C06905585")));
                    // ReSharper restore PossibleInvalidOperationException
                })
                .Return(foodItemProxy)
                .Repeat.Any();
            dataProviderBaseMock.Stub(m => m.Get(Arg<FoodGroupProxy>.Is.NotNull))
                .WhenCalled(e =>
                {
                    var proxy = (FoodGroupProxy) e.Arguments.ElementAt(0);
                    Assert.That(proxy, Is.Not.Null);
                    Assert.That(proxy.Identifier, Is.Not.Null);
                    Assert.That(proxy.Identifier.HasValue, Is.True);
                    // ReSharper disable PossibleInvalidOperationException
                    Assert.That(proxy.Identifier.Value, Is.EqualTo(new Guid("32F94B7E-3D9F-43AE-A1B6-97405CB2B3A1")));
                    // ReSharper restore PossibleInvalidOperationException
                })
                .Return(foodGroupProxy)
                .Repeat.Any();

            var dataReader = MockRepository.GenerateStub<MySqlDataReader>();
            dataReader.Stub(m => m.GetString(Arg<string>.Is.Equal("FoodItemGroupIdentifier")))
                .Return("149F64FA-20D8-4F34-A0B3-643AF2F2E3CE")
                .Repeat.Any();
            dataReader.Stub(m => m.GetString(Arg<string>.Is.Equal("FoodItemIdentifier")))
                .Return("AD62F870-5DFF-4E96-ACE5-D73C06905585")
                .Repeat.Any();
            dataReader.Stub(m => m.GetString(Arg<string>.Is.Equal("FoodGroupIdentifier")))
                .Return("32F94B7E-3D9F-43AE-A1B6-97405CB2B3A1")
                .Repeat.Any();
            dataReader.Stub(m => m.GetInt32(Arg<string>.Is.Equal("IsPrimary")))
                .Return(Convert.ToInt32(isPrimary))
                .Repeat.Any();

            var foodItemGroupProxy = new FoodItemGroupProxy();
            Assert.That(foodItemGroupProxy, Is.Not.Null);
            Assert.That(foodItemGroupProxy.Identifier, Is.Null);
            Assert.That(foodItemGroupProxy.Identifier.HasValue, Is.False);

            foodItemGroupProxy.MapData(dataReader, dataProviderBaseMock);
            foodItemGroupProxy.MapRelations(dataProviderBaseMock);
            Assert.That(foodItemGroupProxy, Is.Not.Null);
            Assert.That(foodItemGroupProxy.Identifier, Is.Not.Null);
            Assert.That(foodItemGroupProxy.Identifier.HasValue, Is.True);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(foodItemGroupProxy.Identifier.Value.ToString("D").ToUpper(), Is.EqualTo(dataReader.GetString("FoodItemGroupIdentifier")));
            // ReSharper restore PossibleInvalidOperationException
            Assert.That(foodItemGroupProxy.FoodItem, Is.Not.Null);
            Assert.That(foodItemGroupProxy.FoodItem, Is.TypeOf<FoodItemProxy>());
            Assert.That(foodItemGroupProxy.FoodItemIdentifier, Is.Not.Null);
            Assert.That(foodItemGroupProxy.FoodItemIdentifier.HasValue, Is.True);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(foodItemGroupProxy.FoodItemIdentifier.Value.ToString("D").ToUpper(), Is.EqualTo(dataReader.GetString("FoodItemIdentifier")));
            // ReSharper restore PossibleInvalidOperationException
            Assert.That(foodItemGroupProxy.FoodGroup, Is.Not.Null);
            Assert.That(foodItemGroupProxy.FoodGroup, Is.TypeOf<FoodGroupProxy>());
            Assert.That(foodItemGroupProxy.FoodGroupIdentifier, Is.Not.Null);
            Assert.That(foodItemGroupProxy.FoodGroupIdentifier.HasValue, Is.True);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(foodItemGroupProxy.FoodGroupIdentifier.Value.ToString("D").ToUpper(), Is.EqualTo(dataReader.GetString("FoodGroupIdentifier")));
            // ReSharper restore PossibleInvalidOperationException
            Assert.That(foodItemGroupProxy.IsPrimary, Is.EqualTo(isPrimary));

            dataProviderBaseMock.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Times(2));
            dataProviderBaseMock.AssertWasCalled(m => m.Get(Arg<FoodItemProxy>.Is.NotNull));
            dataProviderBaseMock.AssertWasCalled(m => m.Get(Arg<FoodGroupProxy>.Is.NotNull));
        }

        /// <summary>
        /// Tests that MapRelations throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatMapRelationsThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            var fixture = new Fixture();
            fixture.Customize<MySqlDataReader>(e => e.FromFactory(() => MockRepository.GenerateStub<MySqlDataReader>()));

            var foodItemGroupProxy = new FoodItemGroupProxy();
            Assert.That(foodItemGroupProxy, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodItemGroupProxy.MapRelations(null));
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

            var foodItemGroupProxy = new FoodItemGroupProxy();
            Assert.That(foodItemGroupProxy, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodItemGroupProxy.SaveRelations(null, fixture.Create<bool>()));
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
            fixture.Customize<IDataProviderBase<MySqlCommand>>(e => e.FromFactory(() => MockRepository.GenerateStub<IDataProviderBase<MySqlCommand>>()));

            var foodItemGroupProxy = new FoodItemGroupProxy
            {
                Identifier = null
            };
            Assert.That(foodItemGroupProxy, Is.Not.Null);
            Assert.That(foodItemGroupProxy.Identifier, Is.Null);
            Assert.That(foodItemGroupProxy.Identifier.HasValue, Is.False);

            var exception = Assert.Throws<IntranetRepositoryException>(() => foodItemGroupProxy.SaveRelations(fixture.Create<IDataProviderBase<MySqlCommand>>(), fixture.Create<bool>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, foodItemGroupProxy.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that DeleteRelations throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            var foodItemGroupProxy = new FoodItemGroupProxy();
            Assert.That(foodItemGroupProxy, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodItemGroupProxy.DeleteRelations(null));
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
            fixture.Customize<IDataProviderBase<MySqlCommand>>(e => e.FromFactory(() => MockRepository.GenerateStub<IDataProviderBase<MySqlCommand>>()));

            var foodItemGroupProxy = new FoodItemGroupProxy
            {
                Identifier = null
            };
            Assert.That(foodItemGroupProxy, Is.Not.Null);
            Assert.That(foodItemGroupProxy.Identifier, Is.Null);
            Assert.That(foodItemGroupProxy.Identifier.HasValue, Is.False);

            var exception = Assert.Throws<IntranetRepositoryException>(() => foodItemGroupProxy.DeleteRelations(fixture.Create<IDataProviderBase<MySqlCommand>>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, foodItemGroupProxy.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }
    }
}
