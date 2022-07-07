using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Resources;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.DataProxies.FoodWaste
{
    /// <summary>
    /// Tests the data proxy to a given food group.
    /// </summary>
    [TestFixture]
    public class FoodGroupProxyTests
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
        /// Tests that the constructor initialize a data proxy to a given food group.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeFoodGroupProxy()
        {
            IFoodGroupProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);
            Assert.That(sut.Parent, Is.Null);
            Assert.That(sut.IsActive, Is.False);
            Assert.That(sut.Children, Is.Not.Null);
            Assert.That(sut.Children, Is.Empty);
            Assert.That(sut.Translation, Is.Null);
            Assert.That(sut.Translations, Is.Not.Null);
            Assert.That(sut.Translations, Is.Empty);
            Assert.That(sut.ForeignKeys, Is.Not.Null);
            Assert.That(sut.ForeignKeys, Is.Empty);
        }

        /// <summary>
        /// Tests that Parent maps the parent food group into the proxy when Create has been called and MapData has not been called.
        /// </summary>
        [Test]
        public void TestThatParentMapsParentIntoProxyWhenCreateHasBeenCalledAndMapDataHasNotBeenCalled()
        {
            IFoodGroupProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            Guid foodGroupIdentifier = Guid.NewGuid();
            Guid parentGroupIdentifier = Guid.NewGuid();
            MySqlDataReader dataReader = CreateMySqlDataReader(foodGroupIdentifier, parentGroupIdentifier);

            FoodGroupProxy parentFoodGroupProxy = BuildFoodGroupProxy(parentGroupIdentifier);
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(parentFoodGroupProxy);

            IFoodGroupProxy result = sut.Create(dataReader, dataProvider, "FoodGroupIdentifier", "ParentIdentifier", "IsActive");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Parent, Is.Not.Null);
            Assert.That(result.Parent, Is.EqualTo(parentFoodGroupProxy));

            dataProvider.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Times(1));

            dataProvider.AssertWasCalled(m => m.Get(Arg<FoodGroupProxy>.Matches(fg => fg != null && fg.Identifier.HasValue && fg.Identifier == parentGroupIdentifier)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that Children maps foods groups which has this food group as a parent into the proxy when MapData has been called and MapRelations has not been called.
        /// </summary>
        [Test]
        public void TestThatChildrenMapsChildrenIntoProxyWhenMapDataHasBeenCalledAndMapRelationsHasNotBeenCalled()
        {
            IFoodGroupProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            Guid foodGroupIdentifier = Guid.NewGuid();
            MySqlDataReader dataReader = CreateMySqlDataReader(foodGroupIdentifier);

            IEnumerable<FoodGroupProxy> foodGroupProxyCollection = BuildFoodGroupProxyCollection();
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(foodGroupProxyCollection: foodGroupProxyCollection);

            sut.MapData(dataReader, dataProvider);

            Assert.That(sut.Children, Is.Not.Null);
            Assert.That(sut.Children, Is.Not.Empty);
            Assert.That(sut.Children, Is.EqualTo(foodGroupProxyCollection));

            dataProvider.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Times(1));

            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT fg.FoodGroupIdentifier,fg.ParentIdentifier,fg.IsActive,pfg.ParentIdentifier AS ParentsParentIdentifier,pfg.IsActive AS ParentIsActive FROM FoodGroups AS fg LEFT JOIN FoodGroups AS pfg ON pfg.FoodGroupIdentifier=fg.ParentIdentifier WHERE fg.ParentIdentifier=@parentIdentifier")
                .AddCharDataParameter("@parentIdentifier", foodGroupIdentifier, true)
                .Build();
            dataProvider.AssertWasCalled(m => m.GetCollection<FoodGroupProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that Children maps foods groups which has this food group as a parent into the proxy when Create has been called and MapData has not been called.
        /// </summary>
        [Test]
        public void TestThatChildrenMapsChildrenIntoProxyWhenCreateHasBeenCalledAndMapDataHasNotBeenCalled()
        {
            IFoodGroupProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            Guid foodGroupIdentifier = Guid.NewGuid();
            MySqlDataReader dataReader = CreateMySqlDataReader(foodGroupIdentifier);

            IEnumerable<FoodGroupProxy> foodGroupProxyCollection = BuildFoodGroupProxyCollection();
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(foodGroupProxyCollection: foodGroupProxyCollection);

            IFoodGroupProxy result = sut.Create(dataReader, dataProvider, "FoodGroupIdentifier", "ParentIdentifier", "IsActive");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Children, Is.Not.Null);
            Assert.That(result.Children, Is.Not.Empty);
            Assert.That(result.Children, Is.EqualTo(foodGroupProxyCollection));

            dataProvider.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Times(1));

            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT fg.FoodGroupIdentifier,fg.ParentIdentifier,fg.IsActive,pfg.ParentIdentifier AS ParentsParentIdentifier,pfg.IsActive AS ParentIsActive FROM FoodGroups AS fg LEFT JOIN FoodGroups AS pfg ON pfg.FoodGroupIdentifier=fg.ParentIdentifier WHERE fg.ParentIdentifier=@parentIdentifier")
                .AddCharDataParameter("@parentIdentifier", foodGroupIdentifier, true)
                .Build();
            dataProvider.AssertWasCalled(m => m.GetCollection<FoodGroupProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that Translations maps translations for the food group into the proxy when MapData has been called and MapRelations has not been called.
        /// </summary>
        [Test]
        public void TestThatTranslationsMapsTranslationsIntoProxyWhenMapDataHasBeenCalledAndMapRelationsHasNotBeenCalled()
        {
            IFoodGroupProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            Guid foodGroupIdentifier = Guid.NewGuid();
            MySqlDataReader dataReader = CreateMySqlDataReader(foodGroupIdentifier);

            IEnumerable<TranslationProxy> translationProxyCollection = BuildTranslationProxyCollection();
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(translationProxyCollection: translationProxyCollection);

            sut.MapData(dataReader, dataProvider);

            Assert.That(sut.Translation, Is.Null);
            Assert.That(sut.Translations, Is.Not.Null);
            Assert.That(sut.Translations, Is.Not.Empty);
            Assert.That(sut.Translations, Is.EqualTo(translationProxyCollection));

            dataProvider.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Times(1));

            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT t.TranslationIdentifier AS TranslationIdentifier,t.OfIdentifier AS OfIdentifier,ti.TranslationInfoIdentifier AS InfoIdentifier,ti.CultureName AS CultureName,t.Value AS Value FROM Translations AS t INNER JOIN TranslationInfos AS ti ON ti.TranslationInfoIdentifier=t.InfoIdentifier WHERE t.OfIdentifier=@ofIdentifier ORDER BY ti.CultureName")
                .AddCharDataParameter("@ofIdentifier", foodGroupIdentifier)
                .Build();
            dataProvider.AssertWasCalled(m => m.GetCollection<TranslationProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that Translations maps translations for the food group into the proxy when Create has been called and MapData has not been called.
        /// </summary>
        [Test]
        public void TestThatTranslationsMapsTranslationsIntoProxyWhenCreateHasBeenCalledAndMapDataHasNotBeenCalled()
        {
            IFoodGroupProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            Guid foodGroupIdentifier = Guid.NewGuid();
            MySqlDataReader dataReader = CreateMySqlDataReader(foodGroupIdentifier);

            IEnumerable<TranslationProxy> translationProxyCollection = BuildTranslationProxyCollection();
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(translationProxyCollection: translationProxyCollection);

            IFoodGroupProxy result = sut.Create(dataReader, dataProvider, "FoodGroupIdentifier", "ParentIdentifier", "IsActive");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Translation, Is.Null);
            Assert.That(result.Translations, Is.Not.Null);
            Assert.That(result.Translations, Is.Not.Empty);
            Assert.That(result.Translations, Is.EqualTo(translationProxyCollection));

            dataProvider.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Times(1));

            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT t.TranslationIdentifier AS TranslationIdentifier,t.OfIdentifier AS OfIdentifier,ti.TranslationInfoIdentifier AS InfoIdentifier,ti.CultureName AS CultureName,t.Value AS Value FROM Translations AS t INNER JOIN TranslationInfos AS ti ON ti.TranslationInfoIdentifier=t.InfoIdentifier WHERE t.OfIdentifier=@ofIdentifier ORDER BY ti.CultureName")
                .AddCharDataParameter("@ofIdentifier", foodGroupIdentifier)
                .Build();
            dataProvider.AssertWasCalled(m => m.GetCollection<TranslationProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that ForeignKeys maps foreign keys for the food group into the proxy when MapData has been called and MapRelations has not been called.
        /// </summary>
        [Test]
        public void TestThatForeignKeysMapsForeignKeysIntoProxyWhenMapDataHasBeenCalledAndMapRelationsHasNotBeenCalled()
        {
            IFoodGroupProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            Guid foodGroupIdentifier = Guid.NewGuid();
            MySqlDataReader dataReader = CreateMySqlDataReader(foodGroupIdentifier);

            IEnumerable<ForeignKeyProxy> foreignKeyProxyCollection = BuildForeignKeyProxyCollection();
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(foreignKeyProxyCollection: foreignKeyProxyCollection);

            sut.MapData(dataReader, dataProvider);

            Assert.That(sut.ForeignKeys, Is.Not.Null);
            Assert.That(sut.ForeignKeys, Is.Not.Empty);
            Assert.That(sut.ForeignKeys, Is.EqualTo(foreignKeyProxyCollection));

            dataProvider.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Times(1));

            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT fk.ForeignKeyIdentifier,fk.DataProviderIdentifier,dp.Name AS DataProviderName,dp.HandlesPayments,dp.DataSourceStatementIdentifier,fk.ForeignKeyForIdentifier,fk.ForeignKeyForTypes,fk.ForeignKeyValue FROM ForeignKeys AS fk INNER JOIN DataProviders AS dp ON dp.DataProviderIdentifier=fk.DataProviderIdentifier WHERE fk.ForeignKeyForIdentifier=@foreignKeyForIdentifier")
                .AddCharDataParameter("@foreignKeyForIdentifier", foodGroupIdentifier)
                .Build();
            dataProvider.AssertWasCalled(m => m.GetCollection<ForeignKeyProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that ForeignKeys maps foreign keys for the food group into the proxy when Create has been called and MapData has not been called.
        /// </summary>
        [Test]
        public void TestThatForeignKeysMapsForeignKeysIntoProxyWhenCreateHasBeenCalledAndMapDataHasNotBeenCalled()
        {
            IFoodGroupProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            Guid foodGroupIdentifier = Guid.NewGuid();
            MySqlDataReader dataReader = CreateMySqlDataReader(foodGroupIdentifier);

            IEnumerable<ForeignKeyProxy> foreignKeyProxyCollection = BuildForeignKeyProxyCollection();
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(foreignKeyProxyCollection: foreignKeyProxyCollection);

            IFoodGroupProxy result = sut.Create(dataReader, dataProvider, "FoodGroupIdentifier", "ParentIdentifier", "IsActive");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ForeignKeys, Is.Not.Null);
            Assert.That(result.ForeignKeys, Is.Not.Empty);
            Assert.That(result.ForeignKeys, Is.EqualTo(foreignKeyProxyCollection));

            dataProvider.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Times(1));

            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT fk.ForeignKeyIdentifier,fk.DataProviderIdentifier,dp.Name AS DataProviderName,dp.HandlesPayments,dp.DataSourceStatementIdentifier,fk.ForeignKeyForIdentifier,fk.ForeignKeyForTypes,fk.ForeignKeyValue FROM ForeignKeys AS fk INNER JOIN DataProviders AS dp ON dp.DataProviderIdentifier=fk.DataProviderIdentifier WHERE fk.ForeignKeyForIdentifier=@foreignKeyForIdentifier")
                .AddCharDataParameter("@foreignKeyForIdentifier", foodGroupIdentifier)
                .Build();
            dataProvider.AssertWasCalled(m => m.GetCollection<ForeignKeyProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that getter for UniqueId throws an IntranetRepositoryException when the food group has no identifier.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterThrowsIntranetRepositoryExceptionWhenFoodGroupProxyHasNoIdentifier()
        {
            IFoodGroupProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            sut.Identifier = null;

            // ReSharper disable UnusedVariable
            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => {var uniqueId = sut.UniqueId;});
            // ReSharper restore UnusedVariable

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, sut.Identifier, "Identifier");
        }

        /// <summary>
        /// Tests that getter for UniqueId gets the unique identifier for the food group.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterGetsUniqueIdentificationForFoodGroupProxy()
        {
            Guid identifier = Guid.NewGuid();

            IFoodGroupProxy sut = CreateSut(identifier);
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
            IFoodGroupProxy sut = CreateSut();
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
            IFoodGroupProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.MapData(CreateMySqlDataReader(), null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that MapData maps data into the proxy.
        /// </summary>
        [Test]
        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void TestThatMapDataMapsDataIntoProxy(bool hasParent, bool isActive)
        {
            IFoodGroupProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            Guid foodGroupIdentifier = Guid.NewGuid();
            Guid? parentIdentifier = hasParent ? Guid.NewGuid() : (Guid?) null;
            MySqlDataReader dataReader = CreateMySqlDataReader(foodGroupIdentifier, parentIdentifier, isActive);

            FoodGroupProxy parentFoodGroupProxy = BuildFoodGroupProxy(Guid.NewGuid());
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(parentFoodGroupProxy);

            sut.MapData(dataReader, dataProvider);

            Assert.That(sut.Identifier, Is.Not.Null);
            Assert.That(sut.Identifier, Is.EqualTo(foodGroupIdentifier));
            if (parentIdentifier.HasValue)
            {
                Assert.That(sut.Parent, Is.Not.Null);
                Assert.That(sut.Parent, Is.EqualTo(parentFoodGroupProxy));
            }
            else
            {
                Assert.That(sut.Parent, Is.Null);
            }
            Assert.That(sut.IsActive, Is.EqualTo(isActive));

            dataReader.AssertWasCalled(m => m.GetString(Arg<string>.Is.Equal("FoodGroupIdentifier")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetOrdinal(Arg<string>.Is.Equal("ParentIdentifier")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.IsDBNull(Arg<int>.Is.Equal(1)), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetInt32(Arg<string>.Is.Equal("IsActive")), opt => opt.Repeat.Once());
            dataReader.AssertWasNotCalled(m => m.GetString(Arg<string>.Is.Equal("ParentIdentifier")));

            dataProvider.AssertWasNotCalled(m => m.Clone());

            if (parentIdentifier.HasValue)
            {
                dataProvider.AssertWasCalled(m => m.Create(
                        Arg<IFoodGroupProxy>.Is.TypeOf,
                        Arg<MySqlDataReader>.Is.Equal(dataReader),
                        Arg<string[]>.Matches(e => e != null && e.Length == 3 &&
                                                   e[0] == "ParentIdentifier" &&
                                                   e[1] == "ParentsParentIdentifier" &&
                                                   e[2] == "ParentIsActive")),
                    opt => opt.Repeat.Once());
            }
            else
            {
                dataProvider.AssertWasNotCalled(m => m.Create(
                    Arg<IFoodGroupProxy>.Is.TypeOf,
                    Arg<MySqlDataReader>.Is.Anything,
                    Arg<string[]>.Is.Anything));
            }
        }

        /// <summary>
        /// Tests that MapRelations throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatMapRelationsThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            IFoodGroupProxy sut = CreateSut();
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
            IFoodGroupProxy sut = CreateSut();
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
            IFoodGroupProxy sut = CreateSut(Guid.NewGuid());
            Assert.That(sut, Is.Not.Null);

            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider();

            sut.MapRelations(dataProvider);

            dataProvider.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Times(3));
        }

        /// <summary>
        /// Tests that MapRelations maps the foods groups which has this food group as a parent into the proxy.
        /// </summary>
        [Test]
        public void TestThatMapRelationsMapsChildrenIntoProxy()
        {
            Guid identifier = Guid.NewGuid();

            IFoodGroupProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            IEnumerable<FoodGroupProxy> foodGroupProxyCollection = BuildFoodGroupProxyCollection();
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(foodGroupProxyCollection: foodGroupProxyCollection);

            sut.MapRelations(dataProvider);

            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Children, Is.Not.Null);
            Assert.That(sut.Children, Is.Not.Empty);
            Assert.That(sut.Children, Is.EqualTo(foodGroupProxyCollection));

            dataProvider.AssertWasCalled(m => m.GetCollection<FoodGroupProxy>(Arg<MySqlCommand>.Is.NotNull), opt => opt.Repeat.Once());

            MySqlCommand cmd = (MySqlCommand) dataProvider.GetArgumentsForCallsMadeOn(m => m.GetCollection<FoodGroupProxy>(Arg<MySqlCommand>.Is.NotNull)).First().First();
            new DbCommandTestBuilder("SELECT fg.FoodGroupIdentifier,fg.ParentIdentifier,fg.IsActive,pfg.ParentIdentifier AS ParentsParentIdentifier,pfg.IsActive AS ParentIsActive FROM FoodGroups AS fg LEFT JOIN FoodGroups AS pfg ON pfg.FoodGroupIdentifier=fg.ParentIdentifier WHERE fg.ParentIdentifier=@parentIdentifier")
                .AddCharDataParameter("@parentIdentifier", identifier, true)
                .Build()
                .Run(cmd);
        }

        /// <summary>
        /// Tests that MapRelations maps the translations for the food group into the proxy.
        /// </summary>
        [Test]
        public void TestThatMapRelationsMapsTranslationsIntoProxy()
        {
            Guid identifier = Guid.NewGuid();

            IFoodGroupProxy sut = CreateSut(identifier);
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
        /// Tests that MapRelations maps the foreign keys for the food group into the proxy.
        /// </summary>
        [Test]
        public void TestThatMapRelationsMapsForeignKeysIntoProxy()
        {
            Guid identifier = Guid.NewGuid();

            IFoodGroupProxy sut = CreateSut(identifier);
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
            IFoodGroupProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.SaveRelations(null, _fixture.Create<bool>()));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that SaveRelations throws an IntranetRepositoryException when the identifier for the food group is null.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsThrowsIntranetRepositoryExceptionWhenIdentifierIsNull()
        {
            IFoodGroupProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.SaveRelations(CreateFoodWasteDataProvider(), _fixture.Create<bool>()));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, sut.Identifier, "Identifier");
        }

        /// <summary>
        /// Tests that DeleteRelations throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            IFoodGroupProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.DeleteRelations(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that DeleteRelations throws an IntranetRepositoryException when the identifier for the food group is null.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsThrowsIntranetRepositoryExceptionWhenIdentifierIsNull()
        {
            IFoodGroupProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.DeleteRelations(CreateFoodWasteDataProvider()));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, sut.Identifier, "Identifier");
        }

        /// <summary>
        /// Tests that DeleteRelations calls Clone on the data provider four times.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsCallsCloneOnDataProvider4Times()
        {
            IFoodGroupProxy sut = CreateSut(Guid.NewGuid());
            Assert.That(sut, Is.Not.Null);

            IEnumerable<FoodGroupProxy> foodGroupProxyCollection = new List<FoodGroupProxy>(0);
            IEnumerable<FoodItemGroupProxy> foodItemGroupProxyCollection = new List<FoodItemGroupProxy>(0);
            IEnumerable<TranslationProxy> translationProxyCollection = new List<TranslationProxy>(0);
            IEnumerable<ForeignKeyProxy> foreignKeyProxyCollection = new List<ForeignKeyProxy>(0);
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(foodGroupProxyCollection: foodGroupProxyCollection, foodItemGroupProxyCollection: foodItemGroupProxyCollection, translationProxyCollection: translationProxyCollection, foreignKeyProxyCollection: foreignKeyProxyCollection);

            sut.DeleteRelations(dataProvider);

            dataProvider.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Times(4));
        }

        /// <summary>
        /// Tests that DeleteRelations calls GetCollection on the data provider to get the foods groups which has this the data proxy food group as a parent.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsCallsGetCollectionOnDataProviderToGetChildren()
        {
            Guid identifier = Guid.NewGuid();

            IFoodGroupProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider();

            sut.DeleteRelations(dataProvider);

            dataProvider.AssertWasCalled(m => m.GetCollection<FoodGroupProxy>(Arg<MySqlCommand>.Is.NotNull), opt => opt.Repeat.Once());

            MySqlCommand cmd = (MySqlCommand)dataProvider.GetArgumentsForCallsMadeOn(m => m.GetCollection<FoodGroupProxy>(Arg<MySqlCommand>.Is.NotNull)).First().First();
            new DbCommandTestBuilder("SELECT fg.FoodGroupIdentifier,fg.ParentIdentifier,fg.IsActive,pfg.ParentIdentifier AS ParentsParentIdentifier,pfg.IsActive AS ParentIsActive FROM FoodGroups AS fg LEFT JOIN FoodGroups AS pfg ON pfg.FoodGroupIdentifier=fg.ParentIdentifier WHERE fg.ParentIdentifier=@parentIdentifier")
                .AddCharDataParameter("@parentIdentifier", identifier, true)
                .Build()
                .Run(cmd);
        }

        /// <summary>
        /// Tests that DeleteRelations calls Delete on the data provider for each foods groups which has this the data proxy food group as a parent.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsCallsDeleteOnDataProviderForEachChildren()
        {
            IFoodGroupProxy sut = CreateSut(Guid.NewGuid());
            Assert.That(sut, Is.Not.Null);

            IList<FoodGroupProxy> foodGroupProxyCollection = BuildFoodGroupProxyCollection().ToList();
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(foodGroupProxyCollection: foodGroupProxyCollection);

            sut.DeleteRelations(dataProvider);

            dataProvider.AssertWasCalled(m => m.Delete(Arg<IFoodGroupProxy>.Is.NotNull), opt => opt.Repeat.Times(foodGroupProxyCollection.Count));
        }

        /// <summary>
        /// Tests that DeleteRelations calls GetCollection on the data provider to get the relations between the food group and it's food items.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsCallsGetCollectionOnDataProviderToGetFoodItemGroups()
        {
            Guid identifier = Guid.NewGuid();

            IFoodGroupProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider();

            sut.DeleteRelations(dataProvider);

            dataProvider.AssertWasCalled(m => m.GetCollection<FoodItemGroupProxy>(Arg<MySqlCommand>.Is.NotNull), opt => opt.Repeat.Once());

            MySqlCommand cmd = (MySqlCommand) dataProvider.GetArgumentsForCallsMadeOn(m => m.GetCollection<FoodItemGroupProxy>(Arg<MySqlCommand>.Is.NotNull)).First().First();
            new DbCommandTestBuilder("SELECT fig.FoodItemGroupIdentifier,fig.FoodItemIdentifier,fig.FoodGroupIdentifier,fig.IsPrimary,fi.IsActive AS FoodItemIsActive,fg.ParentIdentifier AS FoodGroupParentIdentifier,fg.IsActive AS FoodGroupIsActive,pfg.ParentIdentifier AS FoodGroupParentsParentIdentifier,pfg.IsActive AS FoodGroupParentsParentIsActive FROM FoodItemGroups AS fig INNER JOIN FoodItems AS fi ON fi.FoodItemIdentifier=fig.FoodItemIdentifier INNER JOIN FoodGroups AS fg ON fg.FoodGroupIdentifier=fig.FoodGroupIdentifier LEFT JOIN FoodGroups AS pfg ON pfg.FoodGroupIdentifier=fg.ParentIdentifier WHERE fig.FoodGroupIdentifier=@foodGroupIdentifier")
                .AddCharDataParameter("@foodGroupIdentifier", identifier)
                .Build()
                .Run(cmd);
        }

        /// <summary>
        /// Tests that DeleteRelations calls Delete on the data provider for each relation between the food group and it's food items.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsCallsDeleteOnDataProviderForEachFoodItemGroups()
        {
            IFoodGroupProxy sut = CreateSut(Guid.NewGuid());
            Assert.That(sut, Is.Not.Null);

            IList<FoodItemGroupProxy> foodItemGroupProxyCollection = BuildFoodItemGroupProxyCollection().ToList();
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(foodItemGroupProxyCollection: foodItemGroupProxyCollection);

            sut.DeleteRelations(dataProvider);

            dataProvider.AssertWasCalled(m => m.Delete(Arg<IFoodItemGroupProxy>.Is.NotNull), opt => opt.Repeat.Times(foodItemGroupProxyCollection.Count));
        }

        /// <summary>
        /// Tests that DeleteRelations calls GetCollection on the data provider to get the translation for the data proxy food group.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsCallsGetCollectionOnDataProviderToGetTranslations()
        {
            Guid identifier = Guid.NewGuid();

            IFoodGroupProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider();

            sut.DeleteRelations(dataProvider);

            dataProvider.AssertWasCalled(m => m.GetCollection<TranslationProxy>(Arg<MySqlCommand>.Is.NotNull), opt => opt.Repeat.Once());

            MySqlCommand cmd = (MySqlCommand) dataProvider.GetArgumentsForCallsMadeOn(m => m.GetCollection<TranslationProxy>(Arg<MySqlCommand>.Is.NotNull)).First().First();
            new DbCommandTestBuilder("SELECT t.TranslationIdentifier AS TranslationIdentifier,t.OfIdentifier AS OfIdentifier,ti.TranslationInfoIdentifier AS InfoIdentifier,ti.CultureName AS CultureName,t.Value AS Value FROM Translations AS t INNER JOIN TranslationInfos AS ti ON ti.TranslationInfoIdentifier=t.InfoIdentifier WHERE t.OfIdentifier=@ofIdentifier ORDER BY ti.CultureName")
                .AddCharDataParameter("@ofIdentifier", identifier)
                .Build()
                .Run(cmd);
        }

        /// <summary>
        /// Tests that DeleteRelations calls Delete on the data provider for each translation for the data proxy food group.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsCallsDeleteOnDataProviderForEachTranslations()
        {
            IFoodGroupProxy sut = CreateSut(Guid.NewGuid());
            Assert.That(sut, Is.Not.Null);

            IList<TranslationProxy> translationProxyCollection = BuildTranslationProxyCollection().ToList();
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(translationProxyCollection: translationProxyCollection);

            sut.DeleteRelations(dataProvider);

            dataProvider.AssertWasCalled(m => m.Delete(Arg<ITranslationProxy>.Is.NotNull), opt => opt.Repeat.Times(translationProxyCollection.Count));
        }

        /// <summary>
        /// Tests that DeleteRelations calls GetCollection on the data provider to get the foreign keys for the data proxy food group.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsCallsGetCollectionOnDataProviderToGetForeignKeys()
        {
            Guid identifier = Guid.NewGuid();

            IFoodGroupProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider();

            sut.DeleteRelations(dataProvider);

            dataProvider.AssertWasCalled(m => m.GetCollection<ForeignKeyProxy>(Arg<MySqlCommand>.Is.NotNull), opt => opt.Repeat.Once());

            MySqlCommand cmd = (MySqlCommand) dataProvider.GetArgumentsForCallsMadeOn(m => m.GetCollection<ForeignKeyProxy>(Arg<MySqlCommand>.Is.NotNull)).First().First();
            new DbCommandTestBuilder("SELECT fk.ForeignKeyIdentifier,fk.DataProviderIdentifier,dp.Name AS DataProviderName,dp.HandlesPayments,dp.DataSourceStatementIdentifier,fk.ForeignKeyForIdentifier,fk.ForeignKeyForTypes,fk.ForeignKeyValue FROM ForeignKeys AS fk INNER JOIN DataProviders AS dp ON dp.DataProviderIdentifier=fk.DataProviderIdentifier WHERE fk.ForeignKeyForIdentifier=@foreignKeyForIdentifier")
                .AddCharDataParameter("@foreignKeyForIdentifier", identifier)
                .Build()
                .Run(cmd);
        }

        /// <summary>
        /// Tests that DeleteRelations calls Delete on the data provider for each translation for the data proxy food group.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsCallsDeleteOnDataProviderForEachForeignKey()
        {
            IFoodGroupProxy sut = CreateSut(Guid.NewGuid());
            Assert.That(sut, Is.Not.Null);

            IList<ForeignKeyProxy> foreignKeyProxyCollection = BuildForeignKeyProxyCollection().ToList();
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(foreignKeyProxyCollection: foreignKeyProxyCollection);

            sut.DeleteRelations(dataProvider);

            dataProvider.AssertWasCalled(m => m.Delete(Arg<IForeignKeyProxy>.Is.NotNull), opt => opt.Repeat.Times(foreignKeyProxyCollection.Count));
        }

        /// <summary>
        /// Tests that CreateGetCommand returns the SQL command for selecting the given food group.
        /// </summary>
        [Test]
        public void TestThatCreateGetCommandReturnsSqlCommand()
        {
            Guid identifier = Guid.NewGuid();

            IFoodGroupProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("SELECT fg.FoodGroupIdentifier,fg.ParentIdentifier,fg.IsActive,pfg.ParentIdentifier AS ParentsParentIdentifier,pfg.IsActive AS ParentIsActive FROM FoodGroups AS fg LEFT JOIN FoodGroups AS pfg ON pfg.FoodGroupIdentifier=fg.ParentIdentifier WHERE fg.FoodGroupIdentifier=@foodGroupIdentifier")
                .AddCharDataParameter("@foodGroupIdentifier", identifier)
                .Build()
                .Run(sut.CreateGetCommand());
        }

        /// <summary>
        /// Tests that GetSqlCommandForInsert returns the SQL command to insert this food group.
        /// </summary>
        [Test]
        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void TestThatCreateInsertCommandReturnsSqlCommandForInsert(bool hasParent, bool isActive)
        {
            Guid identifier = Guid.NewGuid();
            Guid? parentIdentifier = hasParent ? Guid.NewGuid() : (Guid?) null;

            IFoodGroupProxy sut = CreateSut(identifier, parentIdentifier.HasValue ? BuildFoodGroupProxy(parentIdentifier.Value) : null, isActive);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("INSERT INTO FoodGroups (FoodGroupIdentifier,ParentIdentifier,IsActive) VALUES(@foodGroupIdentifier,@parentIdentifier,@isActive)")
                .AddCharDataParameter("@foodGroupIdentifier", identifier)
                .AddCharDataParameter("@parentIdentifier", parentIdentifier, true)
                .AddBitDataParameter("@isActive", isActive)
                .Build()
                .Run(sut.CreateInsertCommand());
        }

        /// <summary>
        /// Tests that CreateUpdateCommand returns the SQL command to update this food group.
        /// </summary>
        [Test]
        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void TestThatCreateUpdateCommandReturnsSqlCommandForUpdate(bool hasParent, bool isActive)
        {
            Guid identifier = Guid.NewGuid();
            Guid? parentIdentifier = hasParent ? Guid.NewGuid() : (Guid?)null;

            IFoodGroupProxy sut = CreateSut(identifier, parentIdentifier.HasValue ? BuildFoodGroupProxy(parentIdentifier.Value) : null, isActive);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("UPDATE FoodGroups SET ParentIdentifier=@parentIdentifier,IsActive=@isActive WHERE FoodGroupIdentifier=@foodGroupIdentifier")
                .AddCharDataParameter("@foodGroupIdentifier", identifier)
                .AddCharDataParameter("@parentIdentifier", parentIdentifier, true)
                .AddBitDataParameter("@isActive", isActive)
                .Build()
                .Run(sut.CreateUpdateCommand());
        }

        /// <summary>
        /// Tests that CreateDeleteCommand returns the SQL command to delete this food group.
        /// </summary>
        [Test]
        public void TestThatCreateDeleteCommandReturnsSqlCommandForDelete()
        {
            Guid identifier = Guid.NewGuid();

            IFoodGroupProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("DELETE FROM FoodGroups WHERE FoodGroupIdentifier=@foodGroupIdentifier")
                .AddCharDataParameter("@foodGroupIdentifier", identifier)
                .Build()
                .Run(sut.CreateDeleteCommand());
        }

        /// <summary>
        /// Tests that Create throws an ArgumentNullException if the data reader is null.
        /// </summary>
        [Test]
        public void TestThatCreateThrowsArgumentNullExceptionIfDataReaderIsNull()
        {
            IFoodGroupProxy sut = CreateSut();
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
            IFoodGroupProxy sut = CreateSut();
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
            IFoodGroupProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Create(CreateMySqlDataReader(), CreateFoodWasteDataProvider(), null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "columnNameCollection");
        }

        /// <summary>
        /// Tests that Create creates a data proxy for a given data provider with values from the data reader.
        /// </summary>
        [Test]
        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void TestThatCreateCreatesProxy(bool hasParent, bool isActive)
        {
            IFoodGroupProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            Guid foodGroupIdentifier = Guid.NewGuid();
            Guid? parentIdentifier = hasParent ? Guid.NewGuid() : (Guid?) null;
            MySqlDataReader dataReader = CreateMySqlDataReader(foodGroupIdentifier, parentIdentifier, isActive);

            FoodGroupProxy parentFoodGroupProxy = BuildFoodGroupProxy(parentIdentifier ?? Guid.NewGuid());
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(parentFoodGroupProxy);

            IFoodGroupProxy result = sut.Create(dataReader, dataProvider, "FoodGroupIdentifier", "ParentIdentifier", "IsActive");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Identifier, Is.Not.Null);
            Assert.That(result.Identifier, Is.EqualTo(foodGroupIdentifier));
            if (parentIdentifier.HasValue)
            {
                Assert.That(result.Parent, Is.Not.Null);
                Assert.That(result.Parent, Is.TypeOf<FoodGroupProxy>());
                Assert.That(result.Parent.Identifier, Is.Not.Null);
                Assert.That(result.Parent.Identifier, Is.EqualTo(parentIdentifier));
            }
            else
            {
                Assert.That(result.Parent, Is.Null);
            }
            Assert.That(result.IsActive, Is.EqualTo(isActive));

            dataReader.AssertWasCalled(m => m.GetString(Arg<string>.Is.Equal("FoodGroupIdentifier")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetOrdinal(Arg<string>.Is.Equal("ParentIdentifier")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.IsDBNull(Arg<int>.Is.Equal(1)), opt => opt.Repeat.Once());
            if (parentIdentifier.HasValue)
            {
                dataReader.AssertWasCalled(m => m.GetString(Arg<string>.Is.Equal("ParentIdentifier")), opt => opt.Repeat.Once());
            }
            else
            {
                dataReader.AssertWasNotCalled(m => m.GetString(Arg<string>.Is.Equal("ParentIdentifier")));
            }
            dataReader.AssertWasCalled(m => m.GetInt32(Arg<string>.Is.Equal("IsActive")), opt => opt.Repeat.Once());

            if (parentIdentifier.HasValue)
            {
                dataProvider.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Once());
                dataProvider.AssertWasCalled(m => m.Get(Arg<FoodGroupProxy>.Matches(fg => fg != null && fg.Identifier.HasValue && fg.Identifier == parentIdentifier.Value)), opt => opt.Repeat.Once());
            }
            else
            {
                dataProvider.AssertWasNotCalled(m => m.Clone());
                dataProvider.AssertWasNotCalled(m => m.Get(Arg<FoodGroupProxy>.Is.Anything));
            }
            dataProvider.AssertWasNotCalled(m => m.Create(Arg<IFoodGroupProxy>.Is.TypeOf, Arg<MySqlDataReader>.Is.Anything, Arg<string[]>.Is.Anything));
        }

        /// <summary>
        /// Creates an instance of a data proxy to a given food group.
        /// </summary>
        /// <returns>Instance of a data proxy to a given food group.</returns>
        private IFoodGroupProxy CreateSut()
        {
            return new FoodGroupProxy();
        }

        /// <summary>
        /// Creates an instance of a data proxy to a given food group.
        /// </summary>
        /// <returns>Instance of a data proxy to a given food group.</returns>
        private IFoodGroupProxy CreateSut(Guid identifier)
        {
            return new FoodGroupProxy
            {
                Identifier = identifier
            };
        }

        /// <summary>
        /// Creates an instance of a data proxy to a given food group.
        /// </summary>
        /// <returns>Instance of a data proxy to a given food group.</returns>
        private IFoodGroupProxy CreateSut(Guid identifier, IFoodGroupProxy parent, bool isActive)
        {
            return new FoodGroupProxy
            {
                Identifier = identifier,
                Parent = parent,
                IsActive = isActive
            };
        }

        /// <summary>
        /// Creates a stub for the MySQL data reader.
        /// </summary>
        /// <returns>Stub for the MySQL data reader.</returns>
        private MySqlDataReader CreateMySqlDataReader(Guid? foodGroupIdentifier = null, Guid? parentIdentifier = null, bool? isActive = null)
        {
            MySqlDataReader mySqlDataReaderMock = MockRepository.GenerateStub<MySqlDataReader>();
            mySqlDataReaderMock.Stub(m => m.GetString(Arg<string>.Is.Equal("FoodGroupIdentifier")))
                .Return(foodGroupIdentifier.HasValue ? foodGroupIdentifier.Value.ToString("D").ToUpper() : Guid.NewGuid().ToString("D").ToUpper())
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetOrdinal(Arg<string>.Is.Equal("ParentIdentifier")))
                .Return(1)
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.IsDBNull(Arg<int>.Is.Equal(1)))
                .Return(parentIdentifier.HasValue == false)
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetString(Arg<string>.Is.Equal("ParentIdentifier")))
                .Return(parentIdentifier.HasValue ? parentIdentifier.Value.ToString("D").ToUpper() : Guid.NewGuid().ToString("D").ToUpper())
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
        private IFoodWasteDataProvider CreateFoodWasteDataProvider(FoodGroupProxy foodGroupProxy = null, IEnumerable<FoodGroupProxy> foodGroupProxyCollection = null, IEnumerable<FoodItemGroupProxy> foodItemGroupProxyCollection = null, IEnumerable<TranslationProxy> translationProxyCollection = null, IEnumerable<ForeignKeyProxy> foreignKeyProxyCollection = null)
        {
            IFoodWasteDataProvider foodWasteDataProvider = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProvider.Stub(m => m.Clone())
                .Return(foodWasteDataProvider)
                .Repeat.Any();
            foodWasteDataProvider.Stub(m => m.Get(Arg<FoodGroupProxy>.Is.TypeOf))
                .Return(foodGroupProxy ?? BuildFoodGroupProxy(Guid.NewGuid()))
                .Repeat.Any();
            foodWasteDataProvider.Stub(m => m.GetCollection<FoodGroupProxy>(Arg<MySqlCommand>.Is.Anything))
                .Return(foodGroupProxyCollection ?? BuildFoodGroupProxyCollection())
                .Repeat.Any();
            foodWasteDataProvider.Stub(m => m.GetCollection<FoodItemGroupProxy>(Arg<MySqlCommand>.Is.Anything))
                .Return(foodItemGroupProxyCollection ?? BuildFoodItemGroupProxyCollection())
                .Repeat.Any();
            foodWasteDataProvider.Stub(m => m.GetCollection<TranslationProxy>(Arg<MySqlCommand>.Is.Anything))
                .Return(translationProxyCollection ?? BuildTranslationProxyCollection())
                .Repeat.Any();
            foodWasteDataProvider.Stub(m => m.GetCollection<ForeignKeyProxy>(Arg<MySqlCommand>.Is.Anything))
                .Return(foreignKeyProxyCollection ?? BuildForeignKeyProxyCollection())
                .Repeat.Any();
            foodWasteDataProvider.Stub(m => m.Create(Arg<IFoodGroupProxy>.Is.TypeOf, Arg<MySqlDataReader>.Is.Anything, Arg<string[]>.Is.Anything))
                .Return(foodGroupProxy ?? BuildFoodGroupProxy(Guid.NewGuid()))
                .Repeat.Any();
            return foodWasteDataProvider;
        }

        /// <summary>
        /// Builds a data proxy to a given food group.
        /// </summary>
        /// <param name="foodGroupIdentifier">Identifier for the food group.</param>
        /// <returns>Data proxy to a given food group.</returns>
        private FoodGroupProxy BuildFoodGroupProxy(Guid foodGroupIdentifier)
        {
            return _fixture.Build<FoodGroupProxy>()
                .With(m => m.Identifier, foodGroupIdentifier)
                .With(m => m.Parent, (IFoodGroup) null)
                .Create();
        }

        /// <summary>
        /// Builds a collection of data proxies for some food groups.
        /// </summary>
        /// <returns>Collection of data proxies for some food groups.</returns>
        private IEnumerable<FoodGroupProxy> BuildFoodGroupProxyCollection()
        {
            return _fixture.Build<FoodGroupProxy>()
                .With(m => m.Identifier, Guid.NewGuid())
                .With(m => m.Parent, (IFoodGroup) null)
                .CreateMany(_random.Next(10, 25))
                .ToList();
        }

        /// <summary>
        /// Builds a collection of data proxies for some relations between a food item and a food group.
        /// </summary>
        /// <returns>Collection of data proxies for some relations between a food item and a food group.</returns>
        private IEnumerable<FoodItemGroupProxy> BuildFoodItemGroupProxyCollection()
        {
            return _fixture.CreateMany<FoodItemGroupProxy>(_random.Next(10, 25)).ToList();
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
