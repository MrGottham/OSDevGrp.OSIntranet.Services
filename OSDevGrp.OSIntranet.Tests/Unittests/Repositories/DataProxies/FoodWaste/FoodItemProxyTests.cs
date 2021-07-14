using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Guards;
using OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.FoodWaste;
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
        #region Private variables

        private Fixture _fixture;
        private Random _random;

        #endregion

        /// <summary>
        /// Setup each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        /// <summary>
        /// Tests that the constructor initialize a data proxy to a given food item.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeFoodItemProxy()
        {
            IFoodItemProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);
            Assert.That(sut.PrimaryFoodGroup, Is.Null);
            Assert.That(sut.IsActive, Is.False);
            Assert.That(sut.FoodGroups, Is.Not.Null);
            Assert.That(sut.FoodGroups, Is.Empty);
            Assert.That(sut.Translation, Is.Null);
            Assert.That(sut.Translations, Is.Not.Null);
            Assert.That(sut.Translations, Is.Empty);
            Assert.That(sut.ForeignKeys, Is.Not.Null);
            Assert.That(sut.ForeignKeys, Is.Empty);
        }

        /// <summary>
        /// Tests that PrimaryFoodGroup maps the primary food group for the food item into the proxy when MapData has been called and MapRelations has not been called.
        /// </summary>
        [Test]
        public void TestThatPrimaryFoodGroupMapsPrimaryFoodGroupIntoProxyWhenMapDataHasBeenCalledAndMapRelationsHasNotBeenCalled()
        {
            IFoodItemProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            Guid foodItemIdentifier = Guid.NewGuid();
            MySqlDataReader dataReader = CreateMySqlDataReader(foodItemIdentifier, _fixture.Create<bool>());

            FoodGroupProxy primaryFoodGroupProxy = BuildFoodGroupProxy();
            IEnumerable<FoodItemGroupProxy> foodItemGroupProxyCollection = BuildFoodItemGroupProxyCollection(sut, primaryFoodGroupProxy);
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(foodItemGroupProxyCollection);

            sut.MapData(dataReader, dataProvider);

            Assert.That(sut.PrimaryFoodGroup, Is.Not.Null);
            Assert.That(sut.PrimaryFoodGroup, Is.EqualTo(primaryFoodGroupProxy));

            dataProvider.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Once());

            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT fig.FoodItemGroupIdentifier,fig.FoodItemIdentifier,fig.FoodGroupIdentifier,fig.IsPrimary,fi.IsActive AS FoodItemIsActive,fg.ParentIdentifier AS FoodGroupParentIdentifier,fg.IsActive AS FoodGroupIsActive,pfg.ParentIdentifier AS FoodGroupParentsParentIdentifier,pfg.IsActive AS FoodGroupParentsParentIsActive FROM FoodItemGroups AS fig INNER JOIN FoodItems AS fi ON fi.FoodItemIdentifier=fig.FoodItemIdentifier INNER JOIN FoodGroups AS fg ON fg.FoodGroupIdentifier=fig.FoodGroupIdentifier LEFT JOIN FoodGroups AS pfg ON pfg.FoodGroupIdentifier=fg.ParentIdentifier WHERE fig.FoodItemIdentifier=@foodItemIdentifier")
                .AddCharDataParameter("@foodItemIdentifier", foodItemIdentifier)
                .Build();
            dataProvider.AssertWasCalled(m => m.GetCollection<FoodItemGroupProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that PrimaryFoodGroup maps the primary food group for the food item into the proxy when Create has been called and MapData has not been called.
        /// </summary>
        [Test]
        public void TestThatPrimaryFoodGroupMapsPrimaryFoodGroupIntoProxyWhenCreateHasBeenCalledAndMapDataHasNotBeenCalled()
        {
            IFoodItemProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            Guid foodItemIdentifier = Guid.NewGuid();
            MySqlDataReader dataReader = CreateMySqlDataReader(foodItemIdentifier, _fixture.Create<bool>());

            FoodGroupProxy primaryFoodGroupProxy = BuildFoodGroupProxy();
            IEnumerable<FoodItemGroupProxy> foodItemGroupProxyCollection = BuildFoodItemGroupProxyCollection(sut, primaryFoodGroupProxy);
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(foodItemGroupProxyCollection);

            IFoodItemProxy result = sut.Create(dataReader, dataProvider, "FoodItemIdentifier", "IsActive");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.PrimaryFoodGroup, Is.Not.Null);
            Assert.That(result.PrimaryFoodGroup, Is.EqualTo(primaryFoodGroupProxy));

            dataProvider.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Once());

            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT fig.FoodItemGroupIdentifier,fig.FoodItemIdentifier,fig.FoodGroupIdentifier,fig.IsPrimary,fi.IsActive AS FoodItemIsActive,fg.ParentIdentifier AS FoodGroupParentIdentifier,fg.IsActive AS FoodGroupIsActive,pfg.ParentIdentifier AS FoodGroupParentsParentIdentifier,pfg.IsActive AS FoodGroupParentsParentIsActive FROM FoodItemGroups AS fig INNER JOIN FoodItems AS fi ON fi.FoodItemIdentifier=fig.FoodItemIdentifier INNER JOIN FoodGroups AS fg ON fg.FoodGroupIdentifier=fig.FoodGroupIdentifier LEFT JOIN FoodGroups AS pfg ON pfg.FoodGroupIdentifier=fg.ParentIdentifier WHERE fig.FoodItemIdentifier=@foodItemIdentifier")
                .AddCharDataParameter("@foodItemIdentifier", foodItemIdentifier)
                .Build();
            dataProvider.AssertWasCalled(m => m.GetCollection<FoodItemGroupProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that FoodGroups maps the food groups which the food item belong to into the proxy when MapData has been called and MapRelations has not been called.
        /// </summary>
        [Test]
        public void TestThatFoodGroupsMapsFoodGroupsIntoProxyWhenMapDataHasBeenCalledAndMapRelationsHasNotBeenCalled()
        {
            IFoodItemProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            Guid foodItemIdentifier = Guid.NewGuid();
            MySqlDataReader dataReader = CreateMySqlDataReader(foodItemIdentifier, _fixture.Create<bool>());

            IList<FoodItemGroupProxy> foodItemGroupProxyCollection = BuildFoodItemGroupProxyCollection(sut).ToList();
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(foodItemGroupProxyCollection);

            sut.MapData(dataReader, dataProvider);

            Assert.That(sut.FoodGroups, Is.Not.Null);
            Assert.That(sut.FoodGroups, Is.Not.Empty);
            Assert.That(sut.FoodGroups, Is.EqualTo(foodItemGroupProxyCollection.Select(m => m.FoodGroup)));

            dataProvider.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Once());

            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT fig.FoodItemGroupIdentifier,fig.FoodItemIdentifier,fig.FoodGroupIdentifier,fig.IsPrimary,fi.IsActive AS FoodItemIsActive,fg.ParentIdentifier AS FoodGroupParentIdentifier,fg.IsActive AS FoodGroupIsActive,pfg.ParentIdentifier AS FoodGroupParentsParentIdentifier,pfg.IsActive AS FoodGroupParentsParentIsActive FROM FoodItemGroups AS fig INNER JOIN FoodItems AS fi ON fi.FoodItemIdentifier=fig.FoodItemIdentifier INNER JOIN FoodGroups AS fg ON fg.FoodGroupIdentifier=fig.FoodGroupIdentifier LEFT JOIN FoodGroups AS pfg ON pfg.FoodGroupIdentifier=fg.ParentIdentifier WHERE fig.FoodItemIdentifier=@foodItemIdentifier")
                .AddCharDataParameter("@foodItemIdentifier", foodItemIdentifier)
                .Build();
            dataProvider.AssertWasCalled(m => m.GetCollection<FoodItemGroupProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that FoodGroups maps the food groups which the food item belong to into the proxy when Create has been called and MapData has not been called.
        /// </summary>
        [Test]
        public void TestThatFoodGroupsMapsFoodGroupsIntoProxyWhenCreateHasBeenCalledAndMapDataHasNotBeenCalled()
        {
            IFoodItemProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            Guid foodItemIdentifier = Guid.NewGuid();
            MySqlDataReader dataReader = CreateMySqlDataReader(foodItemIdentifier, _fixture.Create<bool>());

            IList<FoodItemGroupProxy> foodItemGroupProxyCollection = BuildFoodItemGroupProxyCollection(sut).ToList();
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(foodItemGroupProxyCollection);

            IFoodItemProxy result = sut.Create(dataReader, dataProvider, "FoodItemIdentifier", "IsActive");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.FoodGroups, Is.Not.Null);
            Assert.That(result.FoodGroups, Is.Not.Empty);
            Assert.That(result.FoodGroups, Is.EqualTo(foodItemGroupProxyCollection.Select(m => m.FoodGroup)));

            dataProvider.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Once());

            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT fig.FoodItemGroupIdentifier,fig.FoodItemIdentifier,fig.FoodGroupIdentifier,fig.IsPrimary,fi.IsActive AS FoodItemIsActive,fg.ParentIdentifier AS FoodGroupParentIdentifier,fg.IsActive AS FoodGroupIsActive,pfg.ParentIdentifier AS FoodGroupParentsParentIdentifier,pfg.IsActive AS FoodGroupParentsParentIsActive FROM FoodItemGroups AS fig INNER JOIN FoodItems AS fi ON fi.FoodItemIdentifier=fig.FoodItemIdentifier INNER JOIN FoodGroups AS fg ON fg.FoodGroupIdentifier=fig.FoodGroupIdentifier LEFT JOIN FoodGroups AS pfg ON pfg.FoodGroupIdentifier=fg.ParentIdentifier WHERE fig.FoodItemIdentifier=@foodItemIdentifier")
                .AddCharDataParameter("@foodItemIdentifier", foodItemIdentifier)
                .Build();
            dataProvider.AssertWasCalled(m => m.GetCollection<FoodItemGroupProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that Translations maps the translations for the food item into the proxy when MapData has been called and MapRelations has not been called.
        /// </summary>
        [Test]
        public void TestThatTranslationsMapsTranslationsIntoProxyWhenMapDataHasBeenCalledAndMapRelationsHasNotBeenCalled()
        {
            IFoodItemProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            Guid foodItemIdentifier = Guid.NewGuid();
            MySqlDataReader dataReader = CreateMySqlDataReader(foodItemIdentifier, _fixture.Create<bool>());

            IEnumerable<TranslationProxy> translationProxyCollection = BuildTranslationProxyCollection();
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(translationProxyCollection: translationProxyCollection);

            sut.MapData(dataReader, dataProvider);
            
            Assert.That(sut.Translation, Is.Null);
            Assert.That(sut.Translations, Is.Not.Null);
            Assert.That(sut.Translations, Is.Not.Empty);
            Assert.That(sut.Translations, Is.EqualTo(translationProxyCollection));

            dataProvider.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Once());

            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT t.TranslationIdentifier AS TranslationIdentifier,t.OfIdentifier AS OfIdentifier,ti.TranslationInfoIdentifier AS InfoIdentifier,ti.CultureName AS CultureName,t.Value AS Value FROM Translations AS t INNER JOIN TranslationInfos AS ti ON ti.TranslationInfoIdentifier=t.InfoIdentifier WHERE t.OfIdentifier=@ofIdentifier ORDER BY ti.CultureName")
                .AddCharDataParameter("@ofIdentifier", foodItemIdentifier)
                .Build();
            dataProvider.AssertWasCalled(m => m.GetCollection<TranslationProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that Translations maps the translations for the food item into the proxy when Create has been called and MapData has not been called.
        /// </summary>
        [Test]
        public void TestThatTranslationsMapsTranslationsIntoProxyWhenCreateHasBeenCalledAndMapDataHasNotBeenCalled()
        {
            IFoodItemProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            Guid foodItemIdentifier = Guid.NewGuid();
            MySqlDataReader dataReader = CreateMySqlDataReader(foodItemIdentifier, _fixture.Create<bool>());

            IEnumerable<TranslationProxy> translationProxyCollection = BuildTranslationProxyCollection();
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(translationProxyCollection: translationProxyCollection);

            IFoodItemProxy result = sut.Create(dataReader, dataProvider, "FoodItemIdentifier", "IsActive");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Translation, Is.Null);
            Assert.That(result.Translations, Is.Not.Null);
            Assert.That(result.Translations, Is.Not.Empty);
            Assert.That(result.Translations, Is.EqualTo(translationProxyCollection));

            dataProvider.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Once());

            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT t.TranslationIdentifier AS TranslationIdentifier,t.OfIdentifier AS OfIdentifier,ti.TranslationInfoIdentifier AS InfoIdentifier,ti.CultureName AS CultureName,t.Value AS Value FROM Translations AS t INNER JOIN TranslationInfos AS ti ON ti.TranslationInfoIdentifier=t.InfoIdentifier WHERE t.OfIdentifier=@ofIdentifier ORDER BY ti.CultureName")
                .AddCharDataParameter("@ofIdentifier", foodItemIdentifier)
                .Build();
            dataProvider.AssertWasCalled(m => m.GetCollection<TranslationProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that ForeignKeys maps the foreign keys for the food item into the proxy when MapData has been called and MapRelations has not been called.
        /// </summary>
        [Test]
        public void TestThatForeignKeysMapsForeignKeysIntoProxyWhenMapDataHasBeenCalledAndMapRelationsHasNotBeenCalled()
        {
            IFoodItemProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            Guid foodItemIdentifier = Guid.NewGuid();
            MySqlDataReader dataReader = CreateMySqlDataReader(foodItemIdentifier, _fixture.Create<bool>());

            IEnumerable<ForeignKeyProxy> foreignKeyProxyCollection = BuildForeignKeyProxyCollection();
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(foreignKeyProxyCollection: foreignKeyProxyCollection);

            sut.MapData(dataReader, dataProvider);

            Assert.That(sut.ForeignKeys, Is.Not.Null);
            Assert.That(sut.ForeignKeys, Is.Not.Empty);
            Assert.That(sut.ForeignKeys, Is.EqualTo(foreignKeyProxyCollection));

            dataProvider.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Once());

            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT fk.ForeignKeyIdentifier,fk.DataProviderIdentifier,dp.Name AS DataProviderName,dp.HandlesPayments,dp.DataSourceStatementIdentifier,fk.ForeignKeyForIdentifier,fk.ForeignKeyForTypes,fk.ForeignKeyValue FROM ForeignKeys AS fk INNER JOIN DataProviders AS dp ON dp.DataProviderIdentifier=fk.DataProviderIdentifier WHERE fk.ForeignKeyForIdentifier=@foreignKeyForIdentifier")
                .AddCharDataParameter("@foreignKeyForIdentifier", foodItemIdentifier)
                .Build();
            dataProvider.AssertWasCalled(m => m.GetCollection<ForeignKeyProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that ForeignKeys maps the foreign keys for the food item into the proxy when Create has been called and MapData has not been called.
        /// </summary>
        [Test]
        public void TestThatForeignKeysMapsForeignKeysIntoProxyWhenCreateHasBeenCalledAndMapDataHasNotBeenCalled()
        {
            IFoodItemProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            Guid foodItemIdentifier = Guid.NewGuid();
            MySqlDataReader dataReader = CreateMySqlDataReader(foodItemIdentifier, _fixture.Create<bool>());

            IEnumerable<ForeignKeyProxy> foreignKeyProxyCollection = BuildForeignKeyProxyCollection();
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(foreignKeyProxyCollection: foreignKeyProxyCollection);

            sut.MapData(dataReader, dataProvider);

            IFoodItemProxy result = sut.Create(dataReader, dataProvider, "FoodItemIdentifier", "IsActive");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ForeignKeys, Is.Not.Null);
            Assert.That(result.ForeignKeys, Is.Not.Empty);
            Assert.That(result.ForeignKeys, Is.EqualTo(foreignKeyProxyCollection));

            dataProvider.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Once());

            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT fk.ForeignKeyIdentifier,fk.DataProviderIdentifier,dp.Name AS DataProviderName,dp.HandlesPayments,dp.DataSourceStatementIdentifier,fk.ForeignKeyForIdentifier,fk.ForeignKeyForTypes,fk.ForeignKeyValue FROM ForeignKeys AS fk INNER JOIN DataProviders AS dp ON dp.DataProviderIdentifier=fk.DataProviderIdentifier WHERE fk.ForeignKeyForIdentifier=@foreignKeyForIdentifier")
                .AddCharDataParameter("@foreignKeyForIdentifier", foodItemIdentifier)
                .Build();
            dataProvider.AssertWasCalled(m => m.GetCollection<ForeignKeyProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that getter for UniqueId throws an IntranetRepositoryException when the food item has no identifier.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterThrowsIntranetRepositoryExceptionWhenFoodItemProxyHasNoIdentifier()
        {
            IFoodItemProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            // ReSharper disable UnusedVariable
            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => {var uniqueId = sut.UniqueId;});
            // ReSharper restore UnusedVariable

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, sut.Identifier, "Identifier");
        }

        /// <summary>
        /// Tests that getter for UniqueId gets the unique identifier for the food item.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterGetsUniqueIdentificationForFoodItemProxy()
        {
            Guid identifier = Guid.NewGuid();

            IFoodItemProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            string uniqueId = sut.UniqueId;
            Assert.That(uniqueId, Is.Not.Null);
            Assert.That(uniqueId, Is.Not.Empty);
            Assert.That(uniqueId, Is.EqualTo(identifier.ToString("D").ToUpper()));
        }

        /// <summary>
        /// Tests that MapData throws an ArgumentNullException if the data reader is null.
        /// </summary>
        [Test]
        public void TestThatMapDataThrowsArgumentNullExceptionIfDataReaderIsNull()
        {
            IFoodItemProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.MapData(null, CreateFoodWasteDataProvider()));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataReader");
        }

        /// <summary>
        /// Tests that MapData throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatMapDataThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            IFoodItemProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.MapData(CreateMySqlDataReader(), null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that MapData maps data into the proxy.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatMapDataMapsDataIntoProxy(bool isActive)
        {
            IFoodItemProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            Guid foodItemIdentifier = Guid.NewGuid();
            MySqlDataReader dataReader = CreateMySqlDataReader(foodItemIdentifier, isActive);

            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider();

            sut.MapData(dataReader, dataProvider);

            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Not.Null);
            Assert.That(sut.Identifier, Is.EqualTo(foodItemIdentifier));
            Assert.That(sut.IsActive, Is.EqualTo(isActive));

            dataReader.AssertWasCalled(m => m.GetString("FoodItemIdentifier"), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetInt32("IsActive"), opt => opt.Repeat.Once());

            dataProvider.AssertWasNotCalled(m => m.Clone());
        }

        /// <summary>
        /// Tests that MapRelations throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatMapRelationsThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            IFoodItemProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.MapRelations(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that MapRelations throws an IntranetRepositoryException when the identifier for the food group is null.
        /// </summary>
        [Test]
        public void TestThatMapRelationsThrowsIntranetRepositoryExceptionWhenIdentifierIsNull()
        {
            IFoodItemProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.MapRelations(CreateFoodWasteDataProvider()));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, sut.Identifier, "Identifier");
        }

        /// <summary>
        /// Tests that MapRelations calls Clone on the data provider 3 times.
        /// </summary>
        [Test]
        public void TestThatMapRelationsCallsCloneOnDataProvider3Times()
        {
            IFoodItemProxy sut = CreateSut(Guid.NewGuid());
            Assert.That(sut, Is.Not.Null);

            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider();

            sut.MapRelations(dataProvider);

            dataProvider.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Times(3));
        }

        /// <summary>
        /// Tests that MapRelations maps the primary food group for the food item into the proxy.
        /// </summary>
        [Test]
        public void TestThatMapRelationsMapsPrimaryFoodGroupIntoProxy()
        {
            Guid identifier = Guid.NewGuid();

            IFoodItemProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            FoodGroupProxy primaryFoodGroupProxy = BuildFoodGroupProxy();
            IEnumerable<FoodItemGroupProxy> foodItemGroupProxyCollection = BuildFoodItemGroupProxyCollection(sut, primaryFoodGroupProxy);
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(foodItemGroupProxyCollection);

            sut.MapRelations(dataProvider);

            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.PrimaryFoodGroup, Is.Not.Null);
            Assert.That(sut.PrimaryFoodGroup, Is.EqualTo(primaryFoodGroupProxy));

            dataProvider.AssertWasCalled(m => m.GetCollection<FoodItemGroupProxy>(Arg<MySqlCommand>.Is.NotNull), opt => opt.Repeat.Once());

            MySqlCommand cmd = (MySqlCommand) dataProvider.GetArgumentsForCallsMadeOn(m => m.GetCollection<FoodItemGroupProxy>(Arg<MySqlCommand>.Is.NotNull)).First().First();
            new DbCommandTestBuilder("SELECT fig.FoodItemGroupIdentifier,fig.FoodItemIdentifier,fig.FoodGroupIdentifier,fig.IsPrimary,fi.IsActive AS FoodItemIsActive,fg.ParentIdentifier AS FoodGroupParentIdentifier,fg.IsActive AS FoodGroupIsActive,pfg.ParentIdentifier AS FoodGroupParentsParentIdentifier,pfg.IsActive AS FoodGroupParentsParentIsActive FROM FoodItemGroups AS fig INNER JOIN FoodItems AS fi ON fi.FoodItemIdentifier=fig.FoodItemIdentifier INNER JOIN FoodGroups AS fg ON fg.FoodGroupIdentifier=fig.FoodGroupIdentifier LEFT JOIN FoodGroups AS pfg ON pfg.FoodGroupIdentifier=fg.ParentIdentifier WHERE fig.FoodItemIdentifier=@foodItemIdentifier")
                .AddCharDataParameter("@foodItemIdentifier", identifier)
                .Build()
                .Run(cmd);
        }

        /// <summary>
        /// Tests that MapRelations maps the food groups which the food item belong to into the proxy.
        /// </summary>
        [Test]
        public void TestThatMapRelationsMapsFoodGroupsIntoProxy()
        {
            Guid identifier = Guid.NewGuid();

            IFoodItemProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            IList<FoodItemGroupProxy> foodItemGroupProxyCollection = BuildFoodItemGroupProxyCollection(sut).ToList();
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(foodItemGroupProxyCollection);

            sut.MapRelations(dataProvider);

            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.FoodGroups, Is.Not.Null);
            Assert.That(sut.FoodGroups, Is.Not.Empty);
            Assert.That(sut.FoodGroups, Is.EqualTo(foodItemGroupProxyCollection.Select(m => m.FoodGroup)));

            dataProvider.AssertWasCalled(m => m.GetCollection<FoodItemGroupProxy>(Arg<MySqlCommand>.Is.NotNull), opt => opt.Repeat.Once());

            MySqlCommand cmd = (MySqlCommand) dataProvider.GetArgumentsForCallsMadeOn(m => m.GetCollection<FoodItemGroupProxy>(Arg<MySqlCommand>.Is.NotNull)).First().First();
            new DbCommandTestBuilder("SELECT fig.FoodItemGroupIdentifier,fig.FoodItemIdentifier,fig.FoodGroupIdentifier,fig.IsPrimary,fi.IsActive AS FoodItemIsActive,fg.ParentIdentifier AS FoodGroupParentIdentifier,fg.IsActive AS FoodGroupIsActive,pfg.ParentIdentifier AS FoodGroupParentsParentIdentifier,pfg.IsActive AS FoodGroupParentsParentIsActive FROM FoodItemGroups AS fig INNER JOIN FoodItems AS fi ON fi.FoodItemIdentifier=fig.FoodItemIdentifier INNER JOIN FoodGroups AS fg ON fg.FoodGroupIdentifier=fig.FoodGroupIdentifier LEFT JOIN FoodGroups AS pfg ON pfg.FoodGroupIdentifier=fg.ParentIdentifier WHERE fig.FoodItemIdentifier=@foodItemIdentifier")
                .AddCharDataParameter("@foodItemIdentifier", identifier)
                .Build()
                .Run(cmd);
        }

        /// <summary>
        /// Tests that MapRelations maps the translations for the food item into the proxy.
        /// </summary>
        [Test]
        public void TestThatMapRelationsMapsTranslationsIntoProxy()
        {
            Guid identifier = Guid.NewGuid();

            IFoodItemProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            IEnumerable<TranslationProxy> translationProxyCollection = BuildTranslationProxyCollection();
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(translationProxyCollection: translationProxyCollection);

            sut.MapRelations(dataProvider);

            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Translation, Is.Null);
            Assert.That(sut.Translations, Is.Not.Null);
            Assert.That(sut.Translations, Is.Not.Empty);
            Assert.That(sut.Translations, Is.EqualTo(translationProxyCollection));

            dataProvider.AssertWasCalled(m => m.GetCollection<TranslationProxy>(Arg<MySqlCommand>.Is.NotNull), opt => opt.Repeat.Once());

            MySqlCommand cmd = (MySqlCommand) dataProvider.GetArgumentsForCallsMadeOn(m => m.GetCollection<TranslationProxy>(Arg<MySqlCommand>.Is.NotNull)).First().First();
            new DbCommandTestBuilder("SELECT t.TranslationIdentifier AS TranslationIdentifier,t.OfIdentifier AS OfIdentifier,ti.TranslationInfoIdentifier AS InfoIdentifier,ti.CultureName AS CultureName,t.Value AS Value FROM Translations AS t INNER JOIN TranslationInfos AS ti ON ti.TranslationInfoIdentifier=t.InfoIdentifier WHERE t.OfIdentifier=@ofIdentifier ORDER BY ti.CultureName")
                .AddCharDataParameter("@ofIdentifier", identifier)
                .Build()
                .Run(cmd);
        }

        /// <summary>
        /// Tests that MapRelations maps the foreign keys for the food item into the proxy.
        /// </summary>
        [Test]
        public void TestThatMapRelationsMapsForeignKeysIntoProxy()
        {
            Guid identifier = Guid.NewGuid();

            IFoodItemProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            IEnumerable<ForeignKeyProxy> foreignKeyProxyCollection = BuildForeignKeyProxyCollection();
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(foreignKeyProxyCollection: foreignKeyProxyCollection);

            sut.MapRelations(dataProvider);

            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.ForeignKeys, Is.Not.Null);
            Assert.That(sut.ForeignKeys, Is.Not.Empty);
            Assert.That(sut.ForeignKeys, Is.EqualTo(foreignKeyProxyCollection));

            dataProvider.AssertWasCalled(m => m.GetCollection<ForeignKeyProxy>(Arg<MySqlCommand>.Is.NotNull), opt => opt.Repeat.Once());

            MySqlCommand cmd = (MySqlCommand) dataProvider.GetArgumentsForCallsMadeOn(m => m.GetCollection<ForeignKeyProxy>(Arg<MySqlCommand>.Is.NotNull)).First().First();
            new DbCommandTestBuilder("SELECT fk.ForeignKeyIdentifier,fk.DataProviderIdentifier,dp.Name AS DataProviderName,dp.HandlesPayments,dp.DataSourceStatementIdentifier,fk.ForeignKeyForIdentifier,fk.ForeignKeyForTypes,fk.ForeignKeyValue FROM ForeignKeys AS fk INNER JOIN DataProviders AS dp ON dp.DataProviderIdentifier=fk.DataProviderIdentifier WHERE fk.ForeignKeyForIdentifier=@foreignKeyForIdentifier")
                .AddCharDataParameter("@foreignKeyForIdentifier", identifier)
                .Build()
                .Run(cmd);
        }

        /// <summary>
        /// Tests that SaveRelations throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            IFoodItemProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.SaveRelations(null, _fixture.Create<bool>()));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that SaveRelations throws an IntranetRepositoryException when the identifier for the food item is null.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsThrowsIntranetRepositoryExceptionWhenIdentifierIsNull()
        {
            IFoodItemProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.SaveRelations(CreateFoodWasteDataProvider(), _fixture.Create<bool>()));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, sut.Identifier, "Identifier");
        }

        /// <summary>
        /// Tests that SaveRelations throws an IntranetRepositoryException when the identifier for the primary food group is null.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsThrowsIntranetRepositoryExceptionWhenIdentifierIsNullOnPrimaryFoodGroup()
        {
            FoodGroupProxy primaryFoodGroupProxy = BuildFoodGroupProxy(false);

            IFoodItemProxy sut = CreateSut(Guid.NewGuid(), _fixture.Create<bool>(), primaryFoodGroupProxy);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.SaveRelations(CreateFoodWasteDataProvider(), _fixture.Create<bool>()));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, primaryFoodGroupProxy.Identifier, "PrimaryFoodGroup.Identifier");
        }

        /// <summary>
        /// Tests that SaveRelations throws an IntranetRepositoryException when the identifier for a secondary food group is null.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsThrowsIntranetRepositoryExceptionWhenIdentifierIsNullOnSecondaryFoodGroup()
        {
            FoodGroupProxy primaryFoodGroupProxy = BuildFoodGroupProxy();
            FoodGroupProxy secondaryFoodGroupProxy = BuildFoodGroupProxy(false);

            IFoodItemProxy sut = CreateSut(Guid.NewGuid(), _fixture.Create<bool>(), primaryFoodGroupProxy);
            Assert.That(sut, Is.Not.Null);

            sut.FoodGroupAdd(secondaryFoodGroupProxy);
            Assert.That(sut.FoodGroups, Is.Not.Null);
            Assert.That(sut.FoodGroups, Is.Not.Empty);
            Assert.That(sut.FoodGroups.Contains(secondaryFoodGroupProxy), Is.True);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.SaveRelations(CreateFoodWasteDataProvider(), _fixture.Create<bool>()));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, secondaryFoodGroupProxy.Identifier, "FoodGroups[].Identifier");
        }

        /// <summary>
        /// Tests that SaveRelations inserts the missing relation between a food item and it's primary food group.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsInsertsMissingFoodItemGroupForPrimaryFoodGroup()
        {
            Guid foodItemIdentifier = Guid.NewGuid();

            FoodGroupProxy primaryFoodGroupProxy = BuildFoodGroupProxy();

            IFoodItemProxy sut = CreateSut(foodItemIdentifier, _fixture.Create<bool>(), primaryFoodGroupProxy);
            Assert.That(sut, Is.Not.Null);

            IEnumerable<FoodItemGroupProxy> foodItemGroupProxyCollection = new List<FoodItemGroupProxy>(0);
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(foodItemGroupProxyCollection);

            sut.SaveRelations(dataProvider, _fixture.Create<bool>());

            dataProvider.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Times(2));

            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT fig.FoodItemGroupIdentifier,fig.FoodItemIdentifier,fig.FoodGroupIdentifier,fig.IsPrimary,fi.IsActive AS FoodItemIsActive,fg.ParentIdentifier AS FoodGroupParentIdentifier,fg.IsActive AS FoodGroupIsActive,pfg.ParentIdentifier AS FoodGroupParentsParentIdentifier,pfg.IsActive AS FoodGroupParentsParentIsActive FROM FoodItemGroups AS fig INNER JOIN FoodItems AS fi ON fi.FoodItemIdentifier=fig.FoodItemIdentifier INNER JOIN FoodGroups AS fg ON fg.FoodGroupIdentifier=fig.FoodGroupIdentifier LEFT JOIN FoodGroups AS pfg ON pfg.FoodGroupIdentifier=fg.ParentIdentifier WHERE fig.FoodItemIdentifier=@foodItemIdentifier")
                .AddCharDataParameter("@foodItemIdentifier", foodItemIdentifier)
                .Build();
            dataProvider.AssertWasCalled(m => m.GetCollection<FoodItemGroupProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());

            dataProvider.AssertWasCalled(m => m.Add(Arg<FoodItemGroupProxy>.Matches(foodGroupProxy =>
                    foodGroupProxy != null &&
                    foodGroupProxy.Identifier.HasValue &&
                    foodGroupProxy.FoodItem != null && foodGroupProxy.FoodItem == sut &&
                    foodGroupProxy.FoodGroup != null && foodGroupProxy.FoodGroup == primaryFoodGroupProxy &&
                    foodGroupProxy.IsPrimary)),
                opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that SaveRelations inserts the missing relations between a food item and it's food groups.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsInsertsMissingFoodItemGroups()
        {
            Guid foodItemIdentifier = Guid.NewGuid();
            
            FoodGroupProxy primaryFoodGroupProxy = BuildFoodGroupProxy();
            FoodGroupProxy foodGroupProxy1 = BuildFoodGroupProxy();
            FoodGroupProxy foodGroupProxy2 = BuildFoodGroupProxy();
            FoodGroupProxy foodGroupProxy3 = BuildFoodGroupProxy();

            IFoodItemProxy sut = CreateSut(foodItemIdentifier, _fixture.Create<bool>(), primaryFoodGroupProxy);
            Assert.That(sut, Is.Not.Null);

            sut.FoodGroupAdd(foodGroupProxy1);
            sut.FoodGroupAdd(foodGroupProxy2);
            sut.FoodGroupAdd(foodGroupProxy3);
            Assert.That(sut.FoodGroups, Is.Not.Null);
            Assert.That(sut.FoodGroups, Is.Not.Empty);
            Assert.That(sut.FoodGroups.Contains(foodGroupProxy1), Is.True);
            Assert.That(sut.FoodGroups.Contains(foodGroupProxy2), Is.True);
            Assert.That(sut.FoodGroups.Contains(foodGroupProxy3), Is.True);

            IEnumerable<FoodItemGroupProxy> foodItemGroupProxyCollection = BuildFoodItemGroupProxyCollection(sut, primaryFoodGroupProxy, foodGroupProxy1, foodGroupProxy2);
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(foodItemGroupProxyCollection);

            sut.SaveRelations(dataProvider, _fixture.Create<bool>());

            dataProvider.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Times(2));

            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT fig.FoodItemGroupIdentifier,fig.FoodItemIdentifier,fig.FoodGroupIdentifier,fig.IsPrimary,fi.IsActive AS FoodItemIsActive,fg.ParentIdentifier AS FoodGroupParentIdentifier,fg.IsActive AS FoodGroupIsActive,pfg.ParentIdentifier AS FoodGroupParentsParentIdentifier,pfg.IsActive AS FoodGroupParentsParentIsActive FROM FoodItemGroups AS fig INNER JOIN FoodItems AS fi ON fi.FoodItemIdentifier=fig.FoodItemIdentifier INNER JOIN FoodGroups AS fg ON fg.FoodGroupIdentifier=fig.FoodGroupIdentifier LEFT JOIN FoodGroups AS pfg ON pfg.FoodGroupIdentifier=fg.ParentIdentifier WHERE fig.FoodItemIdentifier=@foodItemIdentifier")
                .AddCharDataParameter("@foodItemIdentifier", foodItemIdentifier)
                .Build();
            dataProvider.AssertWasCalled(m => m.GetCollection<FoodItemGroupProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());

            dataProvider.AssertWasCalled(m => m.Add(Arg<FoodItemGroupProxy>.Matches(foodGroupProxy =>
                    foodGroupProxy != null &&
                    foodGroupProxy.Identifier.HasValue &&
                    foodGroupProxy.FoodItem != null && foodGroupProxy.FoodItem == sut &&
                    foodGroupProxy.FoodGroup != null && foodGroupProxy.FoodGroup == foodGroupProxy3 &&
                    foodGroupProxy.IsPrimary == false)),
                opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that SaveRelations deletes the no longer existing relations between a food item and it's food groups.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsDeletesNoLongerExistingFoodItemGroups()
        {
            Guid foodItemIdentifier = Guid.NewGuid();

            FoodGroupProxy primaryFoodGroupProxy = BuildFoodGroupProxy();
            FoodGroupProxy foodGroupProxy1 = BuildFoodGroupProxy();
            FoodGroupProxy foodGroupProxy2 = BuildFoodGroupProxy();
            FoodGroupProxy foodGroupProxy3 = BuildFoodGroupProxy();

            IFoodItemProxy sut = CreateSut(foodItemIdentifier, _fixture.Create<bool>(), primaryFoodGroupProxy);
            Assert.That(sut, Is.Not.Null);

            sut.FoodGroupAdd(foodGroupProxy1);
            sut.FoodGroupAdd(foodGroupProxy2);
            Assert.That(sut.FoodGroups, Is.Not.Null);
            Assert.That(sut.FoodGroups, Is.Not.Empty);
            Assert.That(sut.FoodGroups.Contains(foodGroupProxy1), Is.True);
            Assert.That(sut.FoodGroups.Contains(foodGroupProxy2), Is.True);
            Assert.That(sut.FoodGroups.Contains(foodGroupProxy3), Is.False);

            IEnumerable<FoodItemGroupProxy> foodItemGroupProxyCollection = BuildFoodItemGroupProxyCollection(sut, primaryFoodGroupProxy, foodGroupProxy1, foodGroupProxy2, foodGroupProxy3);
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(foodItemGroupProxyCollection);

            sut.SaveRelations(dataProvider, _fixture.Create<bool>());

            dataProvider.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Times(2));

            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT fig.FoodItemGroupIdentifier,fig.FoodItemIdentifier,fig.FoodGroupIdentifier,fig.IsPrimary,fi.IsActive AS FoodItemIsActive,fg.ParentIdentifier AS FoodGroupParentIdentifier,fg.IsActive AS FoodGroupIsActive,pfg.ParentIdentifier AS FoodGroupParentsParentIdentifier,pfg.IsActive AS FoodGroupParentsParentIsActive FROM FoodItemGroups AS fig INNER JOIN FoodItems AS fi ON fi.FoodItemIdentifier=fig.FoodItemIdentifier INNER JOIN FoodGroups AS fg ON fg.FoodGroupIdentifier=fig.FoodGroupIdentifier LEFT JOIN FoodGroups AS pfg ON pfg.FoodGroupIdentifier=fg.ParentIdentifier WHERE fig.FoodItemIdentifier=@foodItemIdentifier")
                .AddCharDataParameter("@foodItemIdentifier", foodItemIdentifier)
                .Build();
            dataProvider.AssertWasCalled(m => m.GetCollection<FoodItemGroupProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());

            dataProvider.AssertWasCalled(m => m.Delete(Arg<FoodItemGroupProxy>.Matches(foodGroupProxy =>
                    foodGroupProxy != null &&
                    foodGroupProxy.FoodItem != null && foodGroupProxy.FoodItem == sut &&
                    foodGroupProxy.FoodGroup != null && foodGroupProxy.FoodGroup == foodGroupProxy3 &&
                    foodGroupProxy.IsPrimary == false)),
                opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that SaveRelations updates IsPrimary on relations between a food item and it's food groups.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsUpdatesIsPrimaryOnFoodItemGroups()
        {
            Guid foodItemIdentifier = Guid.NewGuid();

            FoodGroupProxy primaryFoodGroupProxy = BuildFoodGroupProxy();
            FoodGroupProxy nonPrimaryFoodGroupProxy = BuildFoodGroupProxy();

            IFoodItemProxy sut = CreateSut(foodItemIdentifier, _fixture.Create<bool>(), primaryFoodGroupProxy);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.PrimaryFoodGroup, Is.Not.Null);
            Assert.That(sut.PrimaryFoodGroup, Is.EqualTo(primaryFoodGroupProxy));

            sut.FoodGroupAdd(nonPrimaryFoodGroupProxy);
            Assert.That(sut.FoodGroups, Is.Not.Null);
            Assert.That(sut.FoodGroups, Is.Not.Empty);
            Assert.That(sut.FoodGroups.Contains(nonPrimaryFoodGroupProxy), Is.True);

            IEnumerable<FoodItemGroupProxy> foodItemGroupProxyCollection = BuildFoodItemGroupProxyCollection(sut, nonPrimaryFoodGroupProxy, primaryFoodGroupProxy);
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(foodItemGroupProxyCollection);

            sut.SaveRelations(dataProvider, _fixture.Create<bool>());

            dataProvider.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Times(3));

            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT fig.FoodItemGroupIdentifier,fig.FoodItemIdentifier,fig.FoodGroupIdentifier,fig.IsPrimary,fi.IsActive AS FoodItemIsActive,fg.ParentIdentifier AS FoodGroupParentIdentifier,fg.IsActive AS FoodGroupIsActive,pfg.ParentIdentifier AS FoodGroupParentsParentIdentifier,pfg.IsActive AS FoodGroupParentsParentIsActive FROM FoodItemGroups AS fig INNER JOIN FoodItems AS fi ON fi.FoodItemIdentifier=fig.FoodItemIdentifier INNER JOIN FoodGroups AS fg ON fg.FoodGroupIdentifier=fig.FoodGroupIdentifier LEFT JOIN FoodGroups AS pfg ON pfg.FoodGroupIdentifier=fg.ParentIdentifier WHERE fig.FoodItemIdentifier=@foodItemIdentifier")
                .AddCharDataParameter("@foodItemIdentifier", foodItemIdentifier)
                .Build();
            dataProvider.AssertWasCalled(m => m.GetCollection<FoodItemGroupProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());

            dataProvider.AssertWasCalled(m => m.Save(Arg<FoodItemGroupProxy>.Is.NotNull), opt => opt.Repeat.Times(2));

            FoodItemGroupProxy foodItemGroupProxy = (FoodItemGroupProxy) dataProvider.GetArgumentsForCallsMadeOn(m => m.Save(Arg<FoodItemGroupProxy>.Is.NotNull)).ElementAt(0).First();
            Assert.That(foodItemGroupProxy, Is.Not.Null);
            Assert.That(foodItemGroupProxy.FoodItem, Is.Not.Null);
            Assert.That(foodItemGroupProxy.FoodItem, Is.EqualTo(sut));
            Assert.That(foodItemGroupProxy.FoodGroup, Is.Not.Null);
            Assert.That(foodItemGroupProxy.FoodGroup, Is.EqualTo(primaryFoodGroupProxy));
            Assert.That(foodItemGroupProxy.IsPrimary, Is.True);

            foodItemGroupProxy = (FoodItemGroupProxy) dataProvider.GetArgumentsForCallsMadeOn(m => m.Save(Arg<FoodItemGroupProxy>.Is.NotNull)).ElementAt(1).First();
            Assert.That(foodItemGroupProxy, Is.Not.Null);
            Assert.That(foodItemGroupProxy.FoodItem, Is.Not.Null);
            Assert.That(foodItemGroupProxy.FoodItem, Is.EqualTo(sut));
            Assert.That(foodItemGroupProxy.FoodGroup, Is.Not.Null);
            Assert.That(foodItemGroupProxy.FoodGroup, Is.EqualTo(nonPrimaryFoodGroupProxy));
            Assert.That(foodItemGroupProxy.IsPrimary, Is.False);
        }

        /// <summary>
        /// Tests that DeleteRelations throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            IFoodItemProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.DeleteRelations(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that DeleteRelations throws an IntranetRepositoryException when the identifier for the food item is null.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsThrowsIntranetRepositoryExceptionWhenIdentifierIsNull()
        {
            IFoodItemProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.DeleteRelations(CreateFoodWasteDataProvider()));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, sut.Identifier, "Identifier");
        }

        /// <summary>
        /// Tests that DeleteRelations calls Clone on the data provider tree times.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsCallsCloneOnDataProvider3Times()
        {
            Guid identifier = Guid.NewGuid();
            
            IFoodItemProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            IEnumerable<FoodItemGroupProxy> foodItemGroupProxyCollection = new List<FoodItemGroupProxy>(0);
            IEnumerable<TranslationProxy> translationProxyCollection = new List<TranslationProxy>(0);
            IEnumerable<ForeignKeyProxy> foreignKeyProxyCollection = new List<ForeignKeyProxy>(0);
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(foodItemGroupProxyCollection, translationProxyCollection, foreignKeyProxyCollection);

            sut.DeleteRelations(dataProvider);

            dataProvider.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Times(3));
        }

        /// <summary>
        /// Tests that DeleteRelations calls GetCollection on the data provider to get the relations between the food item and it's food groups.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsCallsGetCollectionOnDataProviderToGetFoodItemGroups()
        {
            Guid identifier = Guid.NewGuid();

            IFoodItemProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider();

            sut.DeleteRelations(dataProvider);

            dataProvider.AssertWasCalled(m => m.GetCollection<FoodItemGroupProxy>(Arg<MySqlCommand>.Is.NotNull), opt => opt.Repeat.Once());

            MySqlCommand cmd = (MySqlCommand) dataProvider.GetArgumentsForCallsMadeOn(m => m.GetCollection<FoodItemGroupProxy>(Arg<MySqlCommand>.Is.NotNull)).First().First();
            new DbCommandTestBuilder("SELECT fig.FoodItemGroupIdentifier,fig.FoodItemIdentifier,fig.FoodGroupIdentifier,fig.IsPrimary,fi.IsActive AS FoodItemIsActive,fg.ParentIdentifier AS FoodGroupParentIdentifier,fg.IsActive AS FoodGroupIsActive,pfg.ParentIdentifier AS FoodGroupParentsParentIdentifier,pfg.IsActive AS FoodGroupParentsParentIsActive FROM FoodItemGroups AS fig INNER JOIN FoodItems AS fi ON fi.FoodItemIdentifier=fig.FoodItemIdentifier INNER JOIN FoodGroups AS fg ON fg.FoodGroupIdentifier=fig.FoodGroupIdentifier LEFT JOIN FoodGroups AS pfg ON pfg.FoodGroupIdentifier=fg.ParentIdentifier WHERE fig.FoodItemIdentifier=@foodItemIdentifier")
                .AddCharDataParameter("@foodItemIdentifier", identifier)
                .Build()
                .Run(cmd);
        }

        /// <summary>
        /// Tests that DeleteRelations calls Delete on the data provider for each relation between the food item and it's food groups.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsCallsDeleteOnDataProviderForEachFoodItemGroups()
        {
            Guid identifier = Guid.NewGuid();

            IFoodItemProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            IList<FoodItemGroupProxy> foodItemGroupProxyCollection = BuildFoodItemGroupProxyCollection(sut).ToList();
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(foodItemGroupProxyCollection);

            sut.DeleteRelations(dataProvider);

            dataProvider.AssertWasCalled(m => m.Delete(Arg<IFoodItemGroupProxy>.Is.NotNull), opt => opt.Repeat.Times(foodItemGroupProxyCollection.Count));
        }

        /// <summary>
        /// Tests that DeleteRelations calls GetCollection on the data provider to get the translation for the data proxy food item.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsCallsGetCollectionOnDataProviderToGetTranslations()
        {
            Guid identifier = Guid.NewGuid();

            IFoodItemProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider();

            sut.DeleteRelations(dataProvider);

            dataProvider.AssertWasCalled(m => m.GetCollection<TranslationProxy>(Arg<MySqlCommand>.Is.NotNull), opt => opt.Repeat.Once());

            MySqlCommand cmd = (MySqlCommand)dataProvider.GetArgumentsForCallsMadeOn(m => m.GetCollection<TranslationProxy>(Arg<MySqlCommand>.Is.NotNull)).First().First();
            new DbCommandTestBuilder("SELECT t.TranslationIdentifier AS TranslationIdentifier,t.OfIdentifier AS OfIdentifier,ti.TranslationInfoIdentifier AS InfoIdentifier,ti.CultureName AS CultureName,t.Value AS Value FROM Translations AS t INNER JOIN TranslationInfos AS ti ON ti.TranslationInfoIdentifier=t.InfoIdentifier WHERE t.OfIdentifier=@ofIdentifier ORDER BY ti.CultureName")
                .AddCharDataParameter("@ofIdentifier", identifier)
                .Build()
                .Run(cmd);
        }

        /// <summary>
        /// Tests that DeleteRelations calls Delete on the data provider for each translation for the data proxy food item.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsCallsDeleteOnDataProviderForEachTranslations()
        {
            IFoodItemProxy sut = CreateSut(Guid.NewGuid());
            Assert.That(sut, Is.Not.Null);

            IList<TranslationProxy> translationProxyCollection = BuildTranslationProxyCollection().ToList();
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(translationProxyCollection: translationProxyCollection);

            sut.DeleteRelations(dataProvider);

            dataProvider.AssertWasCalled(m => m.Delete(Arg<ITranslationProxy>.Is.NotNull), opt => opt.Repeat.Times(translationProxyCollection.Count));
        }

        /// <summary>
        /// Tests that DeleteRelations calls GetCollection on the data provider to get the foreign keys for the data proxy food item.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsCallsGetCollectionOnDataProviderToGetForeignKeys()
        {
            Guid identifier = Guid.NewGuid();

            IFoodItemProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider();

            sut.DeleteRelations(dataProvider);

            dataProvider.AssertWasCalled(m => m.GetCollection<ForeignKeyProxy>(Arg<MySqlCommand>.Is.NotNull), opt => opt.Repeat.Once());

            MySqlCommand cmd = (MySqlCommand)dataProvider.GetArgumentsForCallsMadeOn(m => m.GetCollection<ForeignKeyProxy>(Arg<MySqlCommand>.Is.NotNull)).First().First();
            new DbCommandTestBuilder("SELECT fk.ForeignKeyIdentifier,fk.DataProviderIdentifier,dp.Name AS DataProviderName,dp.HandlesPayments,dp.DataSourceStatementIdentifier,fk.ForeignKeyForIdentifier,fk.ForeignKeyForTypes,fk.ForeignKeyValue FROM ForeignKeys AS fk INNER JOIN DataProviders AS dp ON dp.DataProviderIdentifier=fk.DataProviderIdentifier WHERE fk.ForeignKeyForIdentifier=@foreignKeyForIdentifier")
                .AddCharDataParameter("@foreignKeyForIdentifier", identifier)
                .Build()
                .Run(cmd);
        }

        /// <summary>
        /// Tests that DeleteRelations calls Delete on the data provider for each translation for the data proxy food item.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsCallsDeleteOnDataProviderForEachForeignKey()
        {
            IFoodItemProxy sut = CreateSut(Guid.NewGuid());
            Assert.That(sut, Is.Not.Null);

            IList<ForeignKeyProxy> foreignKeyProxyCollection = BuildForeignKeyProxyCollection().ToList();
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(foreignKeyProxyCollection: foreignKeyProxyCollection);

            sut.DeleteRelations(dataProvider);

            dataProvider.AssertWasCalled(m => m.Delete(Arg<IForeignKeyProxy>.Is.NotNull), opt => opt.Repeat.Times(foreignKeyProxyCollection.Count));
        }

        /// <summary>
        /// Tests that CreateGetCommand returns the SQL command for selecting the given food item.
        /// </summary>
        [Test]
        public void TestThatCreateGetCommandReturnsSqlCommand()
        {
            Guid identifier = Guid.NewGuid();

            IFoodItemProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("SELECT fi.FoodItemIdentifier,fi.IsActive FROM FoodItems AS fi WHERE fi.FoodItemIdentifier=@foodItemIdentifier")
                .AddCharDataParameter("@foodItemIdentifier", identifier)
                .Build()
                .Run(sut.CreateGetCommand());
        }

        /// <summary>
        /// Tests that CreateInsertCommand returns the SQL command to insert this food item.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatCreateInsertCommandReturnsSqlCommandForInsert(bool isActive)
        {
            Guid identifier = Guid.NewGuid();

            IFoodItemProxy sut = CreateSut(identifier, isActive, BuildFoodGroupProxy());
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("INSERT INTO FoodItems (FoodItemIdentifier,IsActive) VALUES(@foodItemIdentifier,@isActive)")
                .AddCharDataParameter("@foodItemIdentifier", identifier)
                .AddBitDataParameter("@isActive", isActive)
                .Build()
                .Run(sut.CreateInsertCommand());
        }

        /// <summary>
        /// Tests that CreateUpdateCommand returns the SQL command to update this food item.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatCreateUpdateCommandReturnsSqlCommandForUpdate(bool isActive)
        {
            Guid identifier = Guid.NewGuid();

            IFoodItemProxy sut = CreateSut(identifier, isActive, BuildFoodGroupProxy());
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("UPDATE FoodItems SET IsActive=@isActive WHERE FoodItemIdentifier=@foodItemIdentifier")
                .AddCharDataParameter("@foodItemIdentifier", identifier)
                .AddBitDataParameter("@isActive", isActive)
                .Build()
                .Run(sut.CreateUpdateCommand());
        }

        /// <summary>
        /// Tests that CreateDeleteCommand returns the SQL command to delete this food item.
        /// </summary>
        [Test]
        public void TestThatCreateDeleteCommandReturnsSqlCommandForDelete()
        {
            Guid identifier = Guid.NewGuid();

            IFoodItemProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("DELETE FROM FoodItems WHERE FoodItemIdentifier=@foodItemIdentifier")
                .AddCharDataParameter("@foodItemIdentifier", identifier)
                .Build()
                .Run(sut.CreateDeleteCommand());
        }

        /// <summary>
        /// Tests that Create throws an ArgumentNullException if the data reader is null.
        /// </summary>
        [Test]
        public void TestThatCreateThrowsArgumentNullExceptionIfDataReaderIsNull()
        {
            IFoodItemProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Create(null, CreateFoodWasteDataProvider(), _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>()));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataReader");
        }

        /// <summary>
        /// Tests that Create throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatCreateThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            IFoodItemProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Create(CreateMySqlDataReader(), null, _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>()));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that Create throws an ArgumentNullException if the column name collection is null.
        /// </summary>
        [Test]
        public void TestThatCreateThrowsArgumentNullExceptionIfColumnNameCollectionIsNull()
        {
            IFoodItemProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Create(CreateMySqlDataReader(), CreateFoodWasteDataProvider(), null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "columnNameCollection");
        }

        /// <summary>
        /// Tests that Create creates a data proxy for a given data provider with values from the data reader.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatCreateCreatesProxy(bool isActive)
        {
            IFoodItemProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            Guid foodItemIdentifier = Guid.NewGuid();
            MySqlDataReader dataReader = CreateMySqlDataReader(foodItemIdentifier, isActive);

            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider();

            IFoodItemProxy result = sut.Create(dataReader, dataProvider, "FoodItemIdentifier", "IsActive");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Identifier, Is.Not.Null);
            Assert.That(result.Identifier, Is.EqualTo(foodItemIdentifier));
            Assert.That(result.IsActive, Is.EqualTo(isActive));

            dataReader.AssertWasCalled(m => m.GetString("FoodItemIdentifier"), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetInt32("IsActive"), opt => opt.Repeat.Once());

            dataProvider.AssertWasNotCalled(m => m.Clone());
        }

        /// <summary>
        /// Creates an instance of a data proxy for a food item.
        /// </summary>
        /// <returns>Instance of a data proxy for a food item.</returns>
        private IFoodItemProxy CreateSut()
        {
            return new FoodItemProxy();
        }

        /// <summary>
        /// Creates an instance of a data proxy for a food item.
        /// </summary>
        /// <returns>Instance of a data proxy for a food item.</returns>
        private IFoodItemProxy CreateSut(Guid identifier)
        {
            return new FoodItemProxy
            {
                Identifier = identifier
            };
        }

        /// <summary>
        /// Creates an instance of a data proxy for a food item.
        /// </summary>
        /// <returns>Instance of a data proxy for a food item.</returns>
        private IFoodItemProxy CreateSut(Guid identifier, bool isActive, FoodGroupProxy primaryFoodGroup)
        {
            return new FoodItemProxy(primaryFoodGroup)
            {
                Identifier = identifier,
                IsActive = isActive,
            };
        }

        /// <summary>
        /// Creates a stub for the MySQL data reader.
        /// </summary>
        /// <returns>Stub for the MySQL data reader.</returns>
        private MySqlDataReader CreateMySqlDataReader(Guid? foodItemIdentifier = null, bool? isActive = null)
        {
            MySqlDataReader mySqlDataReaderMock = MockRepository.GenerateStub<MySqlDataReader>();
            mySqlDataReaderMock.Stub(m => m.GetString(Arg<string>.Is.Equal("FoodItemIdentifier")))
                .Return(foodItemIdentifier.HasValue ? foodItemIdentifier.Value.ToString("D").ToUpper() : Guid.NewGuid().ToString("D").ToUpper())
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetInt32(Arg<string>.Is.Equal("IsActive")))
                .Return(Convert.ToInt32(isActive ?? _fixture.Create<bool>()))
                .Repeat.Any();
            return mySqlDataReaderMock;
        }

        /// <summary>
        /// Creates a mockup for the data provider which can access data in the food waste repository.
        /// </summary>
        /// <returns>Mockup for the data provider which can access data in the food waste repository.</returns>
        private IFoodWasteDataProvider CreateFoodWasteDataProvider(IEnumerable<FoodItemGroupProxy> foodItemGroupProxyCollection = null, IEnumerable<TranslationProxy> translationProxyCollection = null, IEnumerable<ForeignKeyProxy> foreignKeyProxyCollection = null)
        {
            IFoodWasteDataProvider foodWasteDataProvider = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProvider.Stub(m => m.Clone())
                .Return(foodWasteDataProvider)
                .Repeat.Any();
            foodWasteDataProvider.Stub(m => m.GetCollection<FoodItemGroupProxy>(Arg<MySqlCommand>.Is.Anything))
                .Return(foodItemGroupProxyCollection ?? BuildFoodItemGroupProxyCollection(CreateSut()))
                .Repeat.Any();
            foodWasteDataProvider.Stub(m => m.GetCollection<TranslationProxy>(Arg<MySqlCommand>.Is.Anything))
                .Return(translationProxyCollection ?? BuildTranslationProxyCollection())
                .Repeat.Any();
            foodWasteDataProvider.Stub(m => m.GetCollection<ForeignKeyProxy>(Arg<MySqlCommand>.Is.Anything))
                .Return(foreignKeyProxyCollection ?? BuildForeignKeyProxyCollection())
                .Repeat.Any();
            foodWasteDataProvider.Stub(m => m.Add(Arg<FoodItemGroupProxy>.Is.TypeOf))
                .WhenCalled(e => e.ReturnValue = e.Arguments.ElementAt(0))
                .Return(null)
                .Repeat.Any();
            foodWasteDataProvider.Stub(m => m.Save(Arg<FoodItemGroupProxy>.Is.TypeOf))
                .WhenCalled(e => e.ReturnValue = e.Arguments.ElementAt(0))
                .Return(null)
                .Repeat.Any();
            return foodWasteDataProvider;
        }

        /// <summary>
        /// Build a data proxy for a food group.
        /// </summary>
        /// <returns>Data proxy for a food group</returns>
        private FoodGroupProxy BuildFoodGroupProxy(bool hasIdentifier = true, Guid? identifier = null)
        {
            return _fixture.Build<FoodGroupProxy>()
                .With(m => m.Identifier, hasIdentifier ? identifier ?? Guid.NewGuid() : (Guid?) null)
                .With(m => m.Parent, (IFoodGroup) null)
                .Create();
        }

        /// <summary>
        /// Build a collection of data proxies which describe a relation between a food item and a food group.
        /// </summary>
        /// <returns>Collection of data proxies which describe a relation between a food item and a food group</returns>
        private IEnumerable<FoodItemGroupProxy> BuildFoodItemGroupProxyCollection(IFoodItemProxy foodItemProxy, FoodGroupProxy primaryFoodGroupProxy = null, params FoodGroupProxy[] foodGroupProxyCollection)
        {
            ArgumentNullGuard.NotNull(foodItemProxy, nameof(foodItemProxy));

            List<FoodItemGroupProxy> foodItemGroupProxyCollection = new List<FoodItemGroupProxy>
            {
                new FoodItemGroupProxy(foodItemProxy, primaryFoodGroupProxy ?? BuildFoodGroupProxy())
                {
                    IsPrimary = true
                }
            };

            if (foodGroupProxyCollection != null && foodGroupProxyCollection.Any())
            {
                foodItemGroupProxyCollection.AddRange(foodGroupProxyCollection
                    .Select(foodGroup => new FoodItemGroupProxy(foodItemProxy, foodGroup) {IsPrimary = false})
                    .ToList());
                return foodItemGroupProxyCollection;
            }

            foodItemGroupProxyCollection.AddRange(_fixture.Build<FoodGroupProxy>()
                .With(m => m.Parent, (IFoodGroup) null)
                .CreateMany(_random.Next(1, 5))
                .Select(foodGroup => new FoodItemGroupProxy(foodItemProxy, foodGroup) {IsPrimary = false})
                .ToList());
            return foodItemGroupProxyCollection;
        }

        /// <summary>
        /// Build a collection of translation data proxies.
        /// </summary>
        /// <returns>Collection of translation data proxies.</returns>
        private IEnumerable<TranslationProxy> BuildTranslationProxyCollection()
        {
            return _fixture.CreateMany<TranslationProxy>(_random.Next(5, 10)).ToList();
        }

        /// <summary>
        /// Build a collection of foreign key data proxies.
        /// </summary>
        /// <returns>Collection of foreign key data proxies.</returns>
        private IEnumerable<ForeignKeyProxy> BuildForeignKeyProxyCollection()
        {
            return _fixture.CreateMany<ForeignKeyProxy>(_random.Next(5, 10)).ToList();
        }
    }
}
