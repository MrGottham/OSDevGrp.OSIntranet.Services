using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Responses;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Resources;
using OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Infrastructure
{
    /// <summary>
    /// Tests the object mapper which can map objects in the food waste domain.
    /// </summary>
    [TestFixture]
    public class FoodWasteObjectMapperTests
    {
        /// <summary>
        /// Tests that the object mapper which can map objects in the food waste domain can be initialized.
        /// </summary>
        [Test]
        public void TestThatFoodWasteObjectMapperCanBeInitialized()
        {
            var foodWasteObjectMapper = new FoodWasteObjectMapper();
            Assert.That(foodWasteObjectMapper, Is.Not.Null);
        }

        /// <summary>
        /// Tests that Map throws an ArgumentNullException if the source object to map is null.
        /// </summary>
        [Test]
        public void TestThatMapThrowsArgumentNullExceptionIfSourceIsNull()
        {
            var foodWasteObjectMapper = new FoodWasteObjectMapper();
            Assert.That(foodWasteObjectMapper, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => foodWasteObjectMapper.Map<object, object>(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("source"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Map throws an IntranetSystemException when the source object is identifiable and the identifier is null.
        /// </summary>
        [Test]
        public void TestThatMapThrowsIntranetSystemExceptionWhenSourceIsIsIdentifiableAndIdentifierIsNull()
        {
            var identifiableMock = MockRepository.GenerateMock<IIdentifiable>();
            identifiableMock.Stub(m => m.Identifier)
                .Return(null)
                .Repeat.Any();

            var foodWasteObjectMapper = new FoodWasteObjectMapper();
            Assert.That(foodWasteObjectMapper, Is.Not.Null);

            var exception = Assert.Throws<IntranetSystemException>(() => foodWasteObjectMapper.Map<object, object>(identifiableMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, identifiableMock.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Map throws an IntranetSystemException when the source object is identifiable and the identifier has no value.
        /// </summary>
        [Test]
        public void TestThatMapThrowsIntranetSystemExceptionWhenSourceIsIsIdentifiableAndIdentifierHasNoValue()
        {
            var identifiableMock = MockRepository.GenerateMock<IIdentifiable>();
            identifiableMock.Stub(m => m.Identifier)
                .Return(null)
                .Repeat.Any();

            var foodWasteObjectMapper = new FoodWasteObjectMapper();
            Assert.That(foodWasteObjectMapper, Is.Not.Null);

            var exception = Assert.Throws<IntranetSystemException>(() => foodWasteObjectMapper.Map<object, object>(identifiableMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, identifiableMock.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Map calls Translate on source if it's a translatable domain object and the translation culture is null.
        /// </summary>
        [Test]
        public void TestThatMapCallsTranslateOnSourceIfTranslatableAndTranslationCultureIsNull()
        {
            var translatableMock = DomainObjectMockBuilder.BuildFoodGroupMock();

            var foodWasteObjectMapper = new FoodWasteObjectMapper();
            Assert.That(foodWasteObjectMapper, Is.Not.Null);

            foodWasteObjectMapper.Map<IFoodGroup, object>(translatableMock);

            translatableMock.AssertWasCalled(m => m.Translate(Arg<CultureInfo>.Is.Equal(Thread.CurrentThread.CurrentUICulture)));
        }

        /// <summary>
        /// Tests that Map calls Translate on source if it's a translatable domain object and the translation culture is not null.
        /// </summary>
        [Test]
        [TestCase("da-DK")]
        [TestCase("en-US")]
        public void TestThatMapCallsTranslateOnSourceIfTranslatableAndTranslationCultureIsNotNull(string cultureName)
        {
            var translatableMock = DomainObjectMockBuilder.BuildFoodGroupMock();
            var translationCulture = new CultureInfo(cultureName);

            var foodWasteObjectMapper = new FoodWasteObjectMapper();
            Assert.That(foodWasteObjectMapper, Is.Not.Null);

            foodWasteObjectMapper.Map<IFoodGroup, object>(translatableMock, translationCulture);

            translatableMock.AssertWasCalled(m => m.Translate(Arg<CultureInfo>.Is.Equal(translationCulture)));
        }

        /// <summary>
        /// Tests that Map calls Translate on the data provider for each foreign key when source is a domain object with foreign keys and the translation culture is null.
        /// </summary>
        [Test]
        public void TestThatMapCallsTranslateOnDataProviderForEachForeignKeyWhenSourceHasForeignKeysAndTranslationCultureIsNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IDataProvider>(e => e.FromFactory(() =>
            {
                var dataProviderMock = MockRepository.GenerateMock<IDataProvider>();
                dataProviderMock.Stub(m => m.Translation)
                    .Return(null)
                    .Repeat.Any();
                return dataProviderMock;
            }));
            fixture.Customize<IForeignKey>(e => e.FromFactory(() =>
            {
                var foreignKeyMock = MockRepository.GenerateMock<IForeignKey>();
                foreignKeyMock.Stub(m => m.DataProvider)
                    .Return(fixture.Create<IDataProvider>())
                    .Repeat.Any();
                return foreignKeyMock;
            }));

            var foodGroupMock = MockRepository.GenerateMock<IFoodGroup>();
            foodGroupMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();
            foodGroupMock.Stub(m => m.ForeignKeys)
                .Return(fixture.CreateMany<IForeignKey>(7).ToList())
                .Repeat.Any();

            var dataProviderMockCollection = foodGroupMock.ForeignKeys.Where(m => m.DataProvider != null).Select(m => m.DataProvider).ToList();
            Assert.That(dataProviderMockCollection, Is.Not.Null);
            Assert.That(dataProviderMockCollection, Is.Not.Empty);

            var foodWasteObjectMapper = new FoodWasteObjectMapper();
            Assert.That(foodWasteObjectMapper, Is.Not.Null);

            foodWasteObjectMapper.Map<IFoodGroup, object>(foodGroupMock);

            dataProviderMockCollection.ForEach(dataProviderMock => dataProviderMock.AssertWasCalled(m => m.Translate(Arg<CultureInfo>.Is.Equal(Thread.CurrentThread.CurrentUICulture))));
        }

        /// <summary>
        /// Tests that Map calls Translate on the data provider for each foreign key when source is a domain object with foreign keys and the translation culture is null.
        /// </summary>
        [Test]
        [TestCase("da-DK")]
        [TestCase("en-US")]
        public void TestThatMapCallsTranslateOnDataProviderForEachForeignKeyWhenSourceHasForeignKeysAndTranslationCultureIsNotNull(string cultureName)
        {
            var fixture = new Fixture();
            fixture.Customize<IDataProvider>(e => e.FromFactory(() =>
            {
                var dataProviderMock = MockRepository.GenerateMock<IDataProvider>();
                dataProviderMock.Stub(m => m.Translation)
                    .Return(null)
                    .Repeat.Any();
                return dataProviderMock;
            }));
            fixture.Customize<IForeignKey>(e => e.FromFactory(() =>
            {
                var foreignKeyMock = MockRepository.GenerateMock<IForeignKey>();
                foreignKeyMock.Stub(m => m.DataProvider)
                    .Return(fixture.Create<IDataProvider>())
                    .Repeat.Any();
                return foreignKeyMock;
            }));

            var foodGroupMock = MockRepository.GenerateMock<IFoodGroup>();
            foodGroupMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();
            foodGroupMock.Stub(m => m.ForeignKeys)
                .Return(fixture.CreateMany<IForeignKey>(7).ToList())
                .Repeat.Any();

            var dataProviderMockCollection = foodGroupMock.ForeignKeys.Where(m => m.DataProvider != null).Select(m => m.DataProvider).ToList();
            Assert.That(dataProviderMockCollection, Is.Not.Null);
            Assert.That(dataProviderMockCollection, Is.Not.Empty);

            var translationCulture = new CultureInfo(cultureName);

            var foodWasteObjectMapper = new FoodWasteObjectMapper();
            Assert.That(foodWasteObjectMapper, Is.Not.Null);

            foodWasteObjectMapper.Map<IFoodGroup, object>(foodGroupMock, translationCulture);

            dataProviderMockCollection.ForEach(dataProviderMock => dataProviderMock.AssertWasCalled(m => m.Translate(Arg<CultureInfo>.Is.Equal(translationCulture))));
        }

        /// <summary>
        /// Tests that Map calls Translate on each translatable domain object in source if source is a collection of translatable domain objects and the translation culture is null.
        /// </summary>
        [Test]
        public void TestThatMapCallsTranslateOnEachTranslatableInSourceIfSourceIsCollectionOfTranslatablesAndTranslationCultureIsNull()
        {
            var translatableMockCollection = DomainObjectMockBuilder.BuildFoodGroupMockCollection().ToList();

            var foodWasteObjectMapper = new FoodWasteObjectMapper();
            Assert.That(foodWasteObjectMapper, Is.Not.Null);

            foodWasteObjectMapper.Map<List<IFoodGroup>, IEnumerable<object>>(translatableMockCollection);

            translatableMockCollection.ForEach(m => m.AssertWasCalled(n => n.Translate(Arg<CultureInfo>.Is.Equal(Thread.CurrentThread.CurrentUICulture))));
        }

        /// <summary>
        /// Tests that Map calls Translate on each translatable domain object in source if source is a collection of translatable domain objects and the translation culture is not null.
        /// </summary>
        [Test]
        [TestCase("da-DK")]
        [TestCase("en-US")]
        public void TestThatMapCallsTranslateOnEachTranslatableInSourceIfSourceIsCollectionOfTranslatablesAndTranslationCultureIsNotNull(string cultureName)
        {
            var translatableMockCollection = DomainObjectMockBuilder.BuildFoodGroupMockCollection().ToList();
            var translationCulture = new CultureInfo(cultureName);

            var foodWasteObjectMapper = new FoodWasteObjectMapper();
            Assert.That(foodWasteObjectMapper, Is.Not.Null);

            foodWasteObjectMapper.Map<List<IFoodGroup>, IEnumerable<object>>(translatableMockCollection, translationCulture);

            translatableMockCollection.ForEach(m => m.AssertWasCalled(n => n.Translate(Arg<CultureInfo>.Is.Equal(translationCulture))));
        }

        /// <summary>
        /// Tests that Map maps HouseholdMember to HouseholdMemberProxy.
        /// </summary>
        [Test]
        public void TestThatMapMapsHouseholdMemberToHouseholdMemberProxy()
        {
            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();

            var foodWasteObjectMapper = new FoodWasteObjectMapper();
            Assert.That(foodWasteObjectMapper, Is.Not.Null);

            var householdMemberProxy = foodWasteObjectMapper.Map<IHouseholdMember, IHouseholdMemberProxy>(householdMemberMock);
            Assert.That(householdMemberProxy, Is.Not.Null);
            Assert.That(householdMemberProxy.Identifier, Is.Not.Null);
            Assert.That(householdMemberProxy.Identifier, Is.EqualTo(householdMemberMock.Identifier));
            Assert.That(householdMemberProxy.MailAddress, Is.Not.Null);
            Assert.That(householdMemberProxy.MailAddress, Is.Not.Empty);
            Assert.That(householdMemberProxy.MailAddress, Is.EqualTo(householdMemberMock.MailAddress));
            Assert.That(householdMemberProxy.ActivationCode, Is.Not.Null);
            Assert.That(householdMemberProxy.ActivationCode, Is.Not.Empty);
            Assert.That(householdMemberProxy.ActivationCode, Is.EqualTo(householdMemberMock.ActivationCode));
            Assert.That(householdMemberProxy.ActivationTime, Is.EqualTo(householdMemberMock.ActivationTime));
            Assert.That(householdMemberProxy.IsActivated, Is.EqualTo(householdMemberMock.IsActivated));
            Assert.That(householdMemberProxy.CreationTime, Is.EqualTo(householdMemberMock.CreationTime));
        }

        /// <summary>
        /// Tests that Map maps FoodItemCollection to FoodItemCollectionView.
        /// </summary>
        [Test]
        public void TestThatMapMapsFoodItemCollectionToFoodItemCollectionView()
        {
            var foodItemCollection = new FoodItemCollection(DomainObjectMockBuilder.BuildFoodItemMockCollection(), DomainObjectMockBuilder.BuildDataProviderMock());

            var foodWasteObjectMapper = new FoodWasteObjectMapper();
            Assert.That(foodWasteObjectMapper, Is.Not.Null);

            var foodItemCollectionView = foodWasteObjectMapper.Map<IFoodItemCollection, FoodItemCollectionView>(foodItemCollection);
            Assert.That(foodItemCollectionView, Is.Not.Null);
            Assert.That(foodItemCollectionView.FoodItems, Is.Not.Null);
            Assert.That(foodItemCollectionView.FoodItems, Is.Not.Empty);
            Assert.That(foodItemCollectionView.FoodItems, Is.TypeOf<List<FoodItemView>>());
            Assert.That(foodItemCollectionView.FoodItems.Count(), Is.EqualTo(foodItemCollection.Count));
            Assert.That(foodItemCollectionView.DataProvider, Is.Not.Null);
            Assert.That(foodItemCollectionView.DataProvider, Is.TypeOf<DataProviderView>());
        }

        /// <summary>
        /// Tests that Map maps FoodItemCollection to FoodItemCollectionSystemView.
        /// </summary>
        [Test]
        public void TestThatMapMapsFoodItemCollectionToFoodItemCollectionSystemView()
        {
            var foodItemCollection = new FoodItemCollection(DomainObjectMockBuilder.BuildFoodItemMockCollection(), DomainObjectMockBuilder.BuildDataProviderMock());

            var foodWasteObjectMapper = new FoodWasteObjectMapper();
            Assert.That(foodWasteObjectMapper, Is.Not.Null);

            var foodItemCollectionSystemView = foodWasteObjectMapper.Map<IFoodItemCollection, FoodItemCollectionSystemView>(foodItemCollection);
            Assert.That(foodItemCollectionSystemView, Is.Not.Null);
            Assert.That(foodItemCollectionSystemView.FoodItems, Is.Not.Null);
            Assert.That(foodItemCollectionSystemView.FoodItems, Is.Not.Empty);
            Assert.That(foodItemCollectionSystemView.FoodItems, Is.TypeOf<List<FoodItemSystemView>>());
            Assert.That(foodItemCollectionSystemView.FoodItems.Count(), Is.EqualTo(foodItemCollection.Count));
            Assert.That(foodItemCollectionSystemView.DataProvider, Is.Not.Null);
            Assert.That(foodItemCollectionSystemView.DataProvider, Is.TypeOf<DataProviderView>());
        }

        /// <summary>
        /// Tests that Map maps FoodItem to FoodItemIdentificationView.
        /// </summary>
        [Test]
        public void TestThatMapMapsFoodItemToFoodItemIdentificationView()
        {
            var foodItemMock = DomainObjectMockBuilder.BuildFoodItemMock();

            var foodWasteObjectMapper = new FoodWasteObjectMapper();
            Assert.That(foodWasteObjectMapper, Is.Not.Null);

            var foodItemIdentificationView = foodWasteObjectMapper.Map<IFoodItem, FoodItemIdentificationView>(foodItemMock);
            Assert.That(foodItemIdentificationView, Is.Not.Null);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(foodItemIdentificationView.FoodItemIdentifier, Is.EqualTo(foodItemMock.Identifier.Value));
            // ReSharper restore PossibleInvalidOperationException
            Assert.That(foodItemIdentificationView.Name, Is.Not.Null);
            Assert.That(foodItemIdentificationView.Name, Is.Not.Empty);
            Assert.That(foodItemIdentificationView.Name, Is.EqualTo(foodItemMock.Translation.Value));
        }

        /// <summary>
        /// Tests that Map maps FoodItem to FoodItemView.
        /// </summary>
        [Test]
        public void TestThatMapMapsFoodItemToFoodItemView()
        {
            var foodItemMock = DomainObjectMockBuilder.BuildFoodItemMock();

            var foodWasteObjectMapper = new FoodWasteObjectMapper();
            Assert.That(foodWasteObjectMapper, Is.Not.Null);

            var foodItemView = foodWasteObjectMapper.Map<IFoodItem, FoodItemView>(foodItemMock);
            Assert.That(foodItemView, Is.Not.Null);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(foodItemView.FoodItemIdentifier, Is.EqualTo(foodItemMock.Identifier.Value));
            // ReSharper restore PossibleInvalidOperationException
            Assert.That(foodItemView.Name, Is.Not.Null);
            Assert.That(foodItemView.Name, Is.Not.Empty);
            Assert.That(foodItemView.Name, Is.EqualTo(foodItemMock.Translation.Value));
            Assert.That(foodItemView.PrimaryFoodGroup, Is.Not.Null);
            Assert.That(foodItemView.PrimaryFoodGroup, Is.TypeOf<FoodGroupIdentificationView>());
            Assert.That(foodItemView.IsActive, Is.EqualTo(foodItemMock.IsActive));
            Assert.That(foodItemView.FoodGroups, Is.Not.Null);
            Assert.That(foodItemView.FoodGroups, Is.Not.Empty);
            Assert.That(foodItemView.FoodGroups, Is.TypeOf<List<FoodGroupIdentificationView>>());
            Assert.That(foodItemView.FoodGroups.Count(), Is.EqualTo(foodItemMock.FoodGroups.Count()));
        }

        /// <summary>
        /// Tests that Map maps FoodItem to FoodItemSystemView.
        /// </summary>
        [Test]
        public void TestThatMapMapsFoodItemToFoodItemSystemView()
        {
            var foodItemMock = DomainObjectMockBuilder.BuildFoodItemMock();

            var foodWasteObjectMapper = new FoodWasteObjectMapper();
            Assert.That(foodWasteObjectMapper, Is.Not.Null);

            var foodItemSystemView = foodWasteObjectMapper.Map<IFoodItem, FoodItemSystemView>(foodItemMock);
            Assert.That(foodItemSystemView, Is.Not.Null);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(foodItemSystemView.FoodItemIdentifier, Is.EqualTo(foodItemMock.Identifier.Value));
            // ReSharper restore PossibleInvalidOperationException
            Assert.That(foodItemSystemView.Name, Is.Not.Null);
            Assert.That(foodItemSystemView.Name, Is.Not.Empty);
            Assert.That(foodItemSystemView.Name, Is.EqualTo(foodItemMock.Translation.Value));
            Assert.That(foodItemSystemView.PrimaryFoodGroup, Is.Not.Null);
            Assert.That(foodItemSystemView.PrimaryFoodGroup, Is.TypeOf<FoodGroupIdentificationView>());
            Assert.That(foodItemSystemView.IsActive, Is.EqualTo(foodItemMock.IsActive));
            Assert.That(foodItemSystemView.FoodGroups, Is.Not.Null);
            Assert.That(foodItemSystemView.FoodGroups, Is.Not.Empty);
            Assert.That(foodItemSystemView.FoodGroups, Is.TypeOf<List<FoodGroupSystemView>>());
            Assert.That(foodItemSystemView.FoodGroups.Count(), Is.EqualTo(foodItemMock.FoodGroups.Count()));
            Assert.That(foodItemSystemView.Translations, Is.Not.Null);
            Assert.That(foodItemSystemView.Translations, Is.Not.Empty);
            Assert.That(foodItemSystemView.Translations, Is.TypeOf<List<TranslationSystemView>>());
            Assert.That(foodItemSystemView.Translations.Count(), Is.EqualTo(foodItemMock.Translations.Count()));
            Assert.That(foodItemSystemView.ForeignKeys, Is.Not.Null);
            Assert.That(foodItemSystemView.ForeignKeys, Is.Not.Empty);
            Assert.That(foodItemSystemView.ForeignKeys, Is.TypeOf<List<ForeignKeySystemView>>());
            Assert.That(foodItemSystemView.ForeignKeys.Count(), Is.EqualTo(foodItemMock.ForeignKeys.Count()));
        }

        /// <summary>
        /// Tests that Map maps FoodItem to FoodItemProxy.
        /// </summary>
        [Test]
        public void TestThatMapMapsFoodItemToFoodItemProxy()
        {
            var foodItemMock = DomainObjectMockBuilder.BuildFoodItemMock();

            var foodWasteObjectMapper = new FoodWasteObjectMapper();
            Assert.That(foodWasteObjectMapper, Is.Not.Null);

            var foodItemProxy = foodWasteObjectMapper.Map<IFoodItem, IFoodItemProxy>(foodItemMock);
            Assert.That(foodItemProxy, Is.Not.Null);
            Assert.That(foodItemProxy.Identifier, Is.Not.Null);
            Assert.That(foodItemProxy.Identifier, Is.EqualTo(foodItemMock.Identifier));
            Assert.That(foodItemProxy.PrimaryFoodGroup, Is.Not.Null);
            Assert.That(foodItemProxy.PrimaryFoodGroup, Is.TypeOf<FoodGroupProxy>());
            Assert.That(foodItemProxy.IsActive, Is.EqualTo(foodItemMock.IsActive));
            Assert.That(foodItemProxy.FoodGroups, Is.Not.Null);
            Assert.That(foodItemProxy.FoodGroups, Is.Not.Empty);
            Assert.That(foodItemProxy.FoodGroups.Count(), Is.EqualTo(foodItemMock.FoodGroups.Count()));
            foreach (var foodGroup in foodItemProxy.FoodGroups)
            {
                Assert.That(foodGroup, Is.Not.Null);
                Assert.That(foodGroup, Is.TypeOf<FoodGroupProxy>());
            }
            Assert.That(foodItemProxy.Translation, Is.Null);
            Assert.That(foodItemProxy.Translations, Is.Not.Null);
            Assert.That(foodItemProxy.Translations, Is.Not.Empty);
            Assert.That(foodItemProxy.Translations.Count(), Is.EqualTo(foodItemMock.Translations.Count()));
            foreach (var translation in foodItemProxy.Translations)
            {
                Assert.That(translation, Is.Not.Null);
                Assert.That(translation, Is.TypeOf<TranslationProxy>());
            }
            Assert.That(foodItemProxy.ForeignKeys, Is.Not.Null);
            Assert.That(foodItemProxy.ForeignKeys, Is.Not.Empty);
            Assert.That(foodItemProxy.ForeignKeys.Count(), Is.EqualTo(foodItemMock.ForeignKeys.Count()));
            foreach (var foreignKey in foodItemProxy.ForeignKeys)
            {
                Assert.That(foreignKey, Is.Not.Null);
                Assert.That(foreignKey, Is.TypeOf<ForeignKeyProxy>());
            }
        }

        /// <summary>
        /// Tests that Map maps FoodGroupCollection to FoodGroupTreeView.
        /// </summary>
        [Test]
        public void TestThatMapMapsFoodGroupCollectionToFoodGroupTreeView()
        {
            var foodGroupMockCollection = new FoodGroupCollection(new List<IFoodGroup> {DomainObjectMockBuilder.BuildFoodGroupMock(), DomainObjectMockBuilder.BuildFoodGroupMock(), DomainObjectMockBuilder.BuildFoodGroupMock()}, DomainObjectMockBuilder.BuildDataProviderMock());

            var foodWasteObjectMapper = new FoodWasteObjectMapper();
            Assert.That(foodWasteObjectMapper, Is.Not.Null);

            var foodGroupTreeView = foodWasteObjectMapper.Map<IFoodGroupCollection, FoodGroupTreeView>(foodGroupMockCollection);
            Assert.That(foodGroupTreeView, Is.Not.Null);
            Assert.That(foodGroupTreeView.FoodGroups, Is.Not.Null);
            Assert.That(foodGroupTreeView.FoodGroups, Is.Not.Empty);
            Assert.That(foodGroupTreeView.FoodGroups, Is.TypeOf<List<FoodGroupView>>());
            Assert.That(foodGroupTreeView.FoodGroups.Count(), Is.EqualTo(foodGroupMockCollection.Count));
            Assert.That(foodGroupTreeView.DataProvider, Is.Not.Null);
            Assert.That(foodGroupTreeView.DataProvider, Is.TypeOf<DataProviderView>());
        }

        /// <summary>
        /// Tests that Map maps FoodGroupCollection to FoodGroupTreeSystemView.
        /// </summary>
        [Test]
        public void TestThatMapMapsFoodGroupCollectionToFoodGroupTreeSystemView()
        {
            var foodGroupMockCollection = new FoodGroupCollection(new List<IFoodGroup> {DomainObjectMockBuilder.BuildFoodGroupMock(), DomainObjectMockBuilder.BuildFoodGroupMock(), DomainObjectMockBuilder.BuildFoodGroupMock()}, DomainObjectMockBuilder.BuildDataProviderMock());

            var foodWasteObjectMapper = new FoodWasteObjectMapper();
            Assert.That(foodWasteObjectMapper, Is.Not.Null);

            var foodGroupTreeSystemView = foodWasteObjectMapper.Map<IFoodGroupCollection, FoodGroupTreeSystemView>(foodGroupMockCollection);
            Assert.That(foodGroupTreeSystemView, Is.Not.Null);
            Assert.That(foodGroupTreeSystemView.FoodGroups, Is.Not.Null);
            Assert.That(foodGroupTreeSystemView.FoodGroups, Is.Not.Empty);
            Assert.That(foodGroupTreeSystemView.FoodGroups, Is.TypeOf<List<FoodGroupSystemView>>());
            Assert.That(foodGroupTreeSystemView.FoodGroups.Count(), Is.EqualTo(foodGroupMockCollection.Count));
            Assert.That(foodGroupTreeSystemView.DataProvider, Is.Not.Null);
            Assert.That(foodGroupTreeSystemView.DataProvider, Is.TypeOf<DataProviderView>());
        }

        /// <summary>
        /// Tests that Map maps FoodGroup to FoodGroupIdentificationView.
        /// </summary>
        [Test]
        public void TestThatMapMapsFoodGroupToFoodGroupIdentificationView()
        {
            var foodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock(DomainObjectMockBuilder.BuildFoodGroupMock());

            var foodWasteObjectMapper = new FoodWasteObjectMapper();
            Assert.That(foodWasteObjectMapper, Is.Not.Null);

            var foodGroupIdentificationView = foodWasteObjectMapper.Map<IFoodGroup, FoodGroupIdentificationView>(foodGroupMock);
            Assert.That(foodGroupIdentificationView, Is.Not.Null);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(foodGroupIdentificationView.FoodGroupIdentifier, Is.EqualTo(foodGroupMock.Identifier.Value));
            // ReSharper restore PossibleInvalidOperationException
            Assert.That(foodGroupIdentificationView.Name, Is.Not.Null);
            Assert.That(foodGroupIdentificationView.Name, Is.Not.Empty);
            Assert.That(foodGroupIdentificationView.Name, Is.EqualTo(foodGroupMock.Translation.Value));
        }

        /// <summary>
        /// Tests that Map maps FoodGroup to FoodGroupView when parent is not null.
        /// </summary>
        [Test]
        public void TestThatMapMapsFoodGroupToFoodGroupViewWhenParentIsNotNull()
        {
            var foodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock(DomainObjectMockBuilder.BuildFoodGroupMock());

            var foodWasteObjectMapper = new FoodWasteObjectMapper();
            Assert.That(foodWasteObjectMapper, Is.Not.Null);

            var foodGroupView = foodWasteObjectMapper.Map<IFoodGroup, FoodGroupView>(foodGroupMock);
            Assert.That(foodGroupView, Is.Not.Null);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(foodGroupView.FoodGroupIdentifier, Is.EqualTo(foodGroupMock.Identifier.Value));
            // ReSharper restore PossibleInvalidOperationException
            Assert.That(foodGroupView.Name, Is.Not.Null);
            Assert.That(foodGroupView.Name, Is.Not.Empty);
            Assert.That(foodGroupView.Name, Is.EqualTo(foodGroupMock.Translation.Value));
            Assert.That(foodGroupView.IsActive, Is.EqualTo(foodGroupMock.IsActive));
            Assert.That(foodGroupView.Parent, Is.Not.Null);
            Assert.That(foodGroupView.Parent, Is.TypeOf<FoodGroupIdentificationView>());
            Assert.That(foodGroupView.Children, Is.Not.Null);
            Assert.That(foodGroupView.Children, Is.Empty);
            Assert.That(foodGroupView.Children, Is.TypeOf<List<FoodGroupView>>());
        }

        /// <summary>
        /// Tests that Map maps FoodGroup to FoodGroupView when parent is null.
        /// </summary>
        [Test]
        public void TestThatMapMapsFoodGroupToFoodGroupViewWhenParentIsNull()
        {
            var foodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock();

            var foodWasteObjectMapper = new FoodWasteObjectMapper();
            Assert.That(foodWasteObjectMapper, Is.Not.Null);

            var foodGroupView = foodWasteObjectMapper.Map<IFoodGroup, FoodGroupView>(foodGroupMock);
            Assert.That(foodGroupView, Is.Not.Null);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(foodGroupView.FoodGroupIdentifier, Is.EqualTo(foodGroupMock.Identifier.Value));
            // ReSharper restore PossibleInvalidOperationException
            Assert.That(foodGroupView.Name, Is.Not.Null);
            Assert.That(foodGroupView.Name, Is.Not.Empty);
            Assert.That(foodGroupView.Name, Is.EqualTo(foodGroupMock.Translation.Value));
            Assert.That(foodGroupView.IsActive, Is.EqualTo(foodGroupMock.IsActive));
            Assert.That(foodGroupView.Parent, Is.Null);
            Assert.That(foodGroupView.Children, Is.Not.Null);
            Assert.That(foodGroupView.Children, Is.Not.Empty);
            Assert.That(foodGroupView.Children, Is.TypeOf<List<FoodGroupView>>());
            Assert.That(foodGroupView.Children.Count(), Is.EqualTo(foodGroupMock.Children.Count()));
        }

        /// <summary>
        /// Tests that Map maps FoodGroup to FoodGroupSystemView when parent is not null.
        /// </summary>
        [Test]
        public void TestThatMapMapsFoodGroupToFoodGroupSystemViewWhenParentIsNotNull()
        {
            var foodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock(DomainObjectMockBuilder.BuildFoodGroupMock());

            var foodWasteObjectMapper = new FoodWasteObjectMapper();
            Assert.That(foodWasteObjectMapper, Is.Not.Null);

            var foodGroupSystemView = foodWasteObjectMapper.Map<IFoodGroup, FoodGroupSystemView>(foodGroupMock);
            Assert.That(foodGroupSystemView, Is.Not.Null);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(foodGroupSystemView.FoodGroupIdentifier, Is.EqualTo(foodGroupMock.Identifier.Value));
            // ReSharper restore PossibleInvalidOperationException
            Assert.That(foodGroupSystemView.Name, Is.Not.Null);
            Assert.That(foodGroupSystemView.Name, Is.Not.Empty);
            Assert.That(foodGroupSystemView.Name, Is.EqualTo(foodGroupMock.Translation.Value));
            Assert.That(foodGroupSystemView.IsActive, Is.EqualTo(foodGroupMock.IsActive));
            Assert.That(foodGroupSystemView.Parent, Is.Not.Null);
            Assert.That(foodGroupSystemView.Parent, Is.TypeOf<FoodGroupIdentificationView>());
            Assert.That(foodGroupSystemView.Translations, Is.Not.Null);
            Assert.That(foodGroupSystemView.Translations, Is.Not.Empty);
            Assert.That(foodGroupSystemView.Translations, Is.TypeOf<List<TranslationSystemView>>());
            Assert.That(foodGroupSystemView.Translations.Count(), Is.EqualTo(foodGroupMock.Translations.Count()));
            Assert.That(foodGroupSystemView.ForeignKeys, Is.Not.Null);
            Assert.That(foodGroupSystemView.ForeignKeys, Is.Not.Empty);
            Assert.That(foodGroupSystemView.ForeignKeys, Is.TypeOf<List<ForeignKeySystemView>>());
            Assert.That(foodGroupSystemView.ForeignKeys.Count(), Is.EqualTo(foodGroupMock.ForeignKeys.Count()));
            Assert.That(foodGroupSystemView.Children, Is.Not.Null);
            Assert.That(foodGroupSystemView.Children, Is.Empty);
            Assert.That(foodGroupSystemView.Children, Is.TypeOf<List<FoodGroupSystemView>>());
        }

        /// <summary>
        /// Tests that Map maps FoodGroup to FoodGroupSystemView when parent is null.
        /// </summary>
        [Test]
        public void TestThatMapMapsFoodGroupToFoodGroupSystemViewWhenParentIsNull()
        {
            var foodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock();

            var foodWasteObjectMapper = new FoodWasteObjectMapper();
            Assert.That(foodWasteObjectMapper, Is.Not.Null);

            var foodGroupSystemView = foodWasteObjectMapper.Map<IFoodGroup, FoodGroupSystemView>(foodGroupMock);
            Assert.That(foodGroupSystemView, Is.Not.Null);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(foodGroupSystemView.FoodGroupIdentifier, Is.EqualTo(foodGroupMock.Identifier.Value));
            // ReSharper restore PossibleInvalidOperationException
            Assert.That(foodGroupSystemView.Name, Is.Not.Null);
            Assert.That(foodGroupSystemView.Name, Is.Not.Empty);
            Assert.That(foodGroupSystemView.Name, Is.EqualTo(foodGroupMock.Translation.Value));
            Assert.That(foodGroupSystemView.IsActive, Is.EqualTo(foodGroupMock.IsActive));
            Assert.That(foodGroupSystemView.Parent, Is.Null);
            Assert.That(foodGroupSystemView.Translations, Is.Not.Null);
            Assert.That(foodGroupSystemView.Translations, Is.Not.Empty);
            Assert.That(foodGroupSystemView.Translations, Is.TypeOf<List<TranslationSystemView>>());
            Assert.That(foodGroupSystemView.Translations.Count(), Is.EqualTo(foodGroupMock.Translations.Count()));
            Assert.That(foodGroupSystemView.ForeignKeys, Is.Not.Null);
            Assert.That(foodGroupSystemView.ForeignKeys, Is.Not.Empty);
            Assert.That(foodGroupSystemView.ForeignKeys, Is.TypeOf<List<ForeignKeySystemView>>());
            Assert.That(foodGroupSystemView.ForeignKeys.Count(), Is.EqualTo(foodGroupMock.ForeignKeys.Count()));
            Assert.That(foodGroupSystemView.Children, Is.Not.Null);
            Assert.That(foodGroupSystemView.Children, Is.Not.Empty);
            Assert.That(foodGroupSystemView.Children, Is.TypeOf<List<FoodGroupSystemView>>());
            Assert.That(foodGroupSystemView.Children.Count(), Is.EqualTo(foodGroupMock.Children.Count()));
        }

        /// <summary>
        /// Tests that Map maps FoodGroup to FoodGroupProxy.
        /// </summary>
        [Test]
        public void TestThatMapMapsFoodGroupToFoodGroupProxy()
        {
            var foodGroupMock = DomainObjectMockBuilder.BuildFoodGroupMock(DomainObjectMockBuilder.BuildFoodGroupMock());

            var foodWasteObjectMapper = new FoodWasteObjectMapper();
            Assert.That(foodWasteObjectMapper, Is.Not.Null);

            var foodGroupProxy = foodWasteObjectMapper.Map<IFoodGroup, IFoodGroupProxy>(foodGroupMock);
            Assert.That(foodGroupProxy.Identifier, Is.Not.Null);
            Assert.That(foodGroupProxy.Identifier, Is.EqualTo(foodGroupMock.Identifier));
            Assert.That(foodGroupProxy.Parent, Is.Not.Null);
            Assert.That(foodGroupProxy.Parent, Is.TypeOf<FoodGroupProxy>());
            Assert.That(foodGroupProxy.Parent.Children, Is.Not.Null);
            Assert.That(foodGroupProxy.Parent.Children, Is.Not.Empty);
            Assert.That(foodGroupProxy.Parent.Children.Count(), Is.EqualTo(foodGroupMock.Parent.Children.Count()));
            foreach (var childFoodGroup in foodGroupProxy.Parent.Children)
            {
                Assert.That(childFoodGroup, Is.Not.Null);
                Assert.That(childFoodGroup, Is.TypeOf<FoodGroupProxy>());
            }
            Assert.That(foodGroupProxy.IsActive, Is.EqualTo(foodGroupMock.IsActive));
            Assert.That(foodGroupProxy.Children, Is.Not.Null);
            Assert.That(foodGroupProxy.Children, Is.Empty);
            Assert.That(foodGroupProxy.Translation, Is.Null);
            Assert.That(foodGroupProxy.Translations, Is.Not.Null);
            Assert.That(foodGroupProxy.Translations, Is.Not.Empty);
            Assert.That(foodGroupProxy.Translations.Count(), Is.EqualTo(foodGroupMock.Translations.Count()));
            foreach (var translation in foodGroupProxy.Translations)
            {
                Assert.That(translation, Is.Not.Null);
                Assert.That(translation, Is.TypeOf<TranslationProxy>());
            }
            Assert.That(foodGroupProxy.ForeignKeys, Is.Not.Null);
            Assert.That(foodGroupProxy.ForeignKeys, Is.Not.Empty);
            Assert.That(foodGroupProxy.ForeignKeys.Count(), Is.EqualTo(foodGroupMock.ForeignKeys.Count()));
            foreach (var foreignKey in foodGroupProxy.ForeignKeys)
            {
                Assert.That(foreignKey, Is.Not.Null);
                Assert.That(foreignKey, Is.TypeOf<ForeignKeyProxy>());
            }
        }

        /// <summary>
        /// Tests that Map maps ForeignKey to ForeignKeyView.
        /// </summary>
        [Test]
        public void TestThatMapMapsForeignKeyToForeignKeyView()
        {
            var foreignKeyMock = DomainObjectMockBuilder.BuildForeignKeyMock(Guid.NewGuid(), typeof(IFoodGroup));

            var foodWasteObjectMapper = new FoodWasteObjectMapper();
            Assert.That(foodWasteObjectMapper, Is.Not.Null);

            var foreignKeyView = foodWasteObjectMapper.Map<IForeignKey, ForeignKeyView>(foreignKeyMock);
            Assert.That(foreignKeyView, Is.Not.Null);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(foreignKeyView.ForeignKeyIdentifier, Is.EqualTo(foreignKeyMock.Identifier.Value));
            // ReSharper restore PossibleInvalidOperationException
            Assert.That(foreignKeyView.DataProvider, Is.Not.Null);
            Assert.That(foreignKeyView.DataProvider, Is.TypeOf<DataProviderView>());
            Assert.That(foreignKeyView.ForeignKeyForIdentifier, Is.EqualTo(foreignKeyMock.ForeignKeyForIdentifier));
            Assert.That(foreignKeyView.ForeignKey, Is.Not.Null);
            Assert.That(foreignKeyView.ForeignKey, Is.Not.Empty);
            Assert.That(foreignKeyView.ForeignKey, Is.EqualTo(foreignKeyMock.ForeignKeyValue));
        }

        /// <summary>
        /// Tests that Map maps ForeignKey to ForeignKeySystemView.
        /// </summary>
        [Test]
        public void TestThatMapMapsForeignKeyToForeignKeySystemView()
        {
            var foreignKeyMock = DomainObjectMockBuilder.BuildForeignKeyMock(Guid.NewGuid(), typeof (IFoodGroup));

            var foodWasteObjectMapper = new FoodWasteObjectMapper();
            Assert.That(foodWasteObjectMapper, Is.Not.Null);

            var foreignKeySystemView = foodWasteObjectMapper.Map<IForeignKey, ForeignKeySystemView>(foreignKeyMock);
            Assert.That(foreignKeySystemView, Is.Not.Null);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(foreignKeySystemView.ForeignKeyIdentifier, Is.EqualTo(foreignKeyMock.Identifier.Value));
            // ReSharper restore PossibleInvalidOperationException
            Assert.That(foreignKeySystemView.DataProvider, Is.Not.Null);
            Assert.That(foreignKeySystemView.DataProvider, Is.TypeOf<DataProviderSystemView>());
            Assert.That(foreignKeySystemView.ForeignKeyForIdentifier, Is.EqualTo(foreignKeyMock.ForeignKeyForIdentifier));
            Assert.That(foreignKeySystemView.ForeignKey, Is.Not.Null);
            Assert.That(foreignKeySystemView.ForeignKey, Is.Not.Empty);
            Assert.That(foreignKeySystemView.ForeignKey, Is.EqualTo(foreignKeyMock.ForeignKeyValue));
        }

        /// <summary>
        /// Tests that Map maps ForeignKey to ForeignKeyProxy.
        /// </summary>
        [Test]
        public void TestThatMapMapsForeignKeyToForeignKeyProxy()
        {
            var foreignKeyMock = DomainObjectMockBuilder.BuildForeignKeyMock(Guid.NewGuid(), typeof (IDataProvider));

            var foodWasteObjectMapper = new FoodWasteObjectMapper();
            Assert.That(foodWasteObjectMapper, Is.Not.Null);

            var foreignKeyProxy = foodWasteObjectMapper.Map<IForeignKey, IForeignKeyProxy>(foreignKeyMock);
            Assert.That(foreignKeyProxy.Identifier, Is.Not.Null);
            Assert.That(foreignKeyProxy.Identifier, Is.EqualTo(foreignKeyMock.Identifier));
            Assert.That(foreignKeyProxy.DataProvider, Is.Not.Null);
            Assert.That(foreignKeyProxy.DataProvider, Is.TypeOf<DataProviderProxy>());
            Assert.That(foreignKeyProxy.ForeignKeyForIdentifier, Is.EqualTo(foreignKeyMock.ForeignKeyForIdentifier));
            Assert.That(foreignKeyProxy.ForeignKeyForTypes, Is.Not.Null);
            Assert.That(foreignKeyProxy.ForeignKeyForTypes, Is.Not.Empty);
            Assert.That(foreignKeyProxy.ForeignKeyForTypes.Count(), Is.EqualTo(foreignKeyMock.ForeignKeyForTypes.Count()));
            foreach (var foreignKeyType in foreignKeyProxy.ForeignKeyForTypes)
            {
                Assert.That(foreignKeyType, Is.Not.Null);
                Assert.That(foreignKeyProxy.ForeignKeyForTypes.Contains(foreignKeyType), Is.True);
            }
            Assert.That(foreignKeyProxy.ForeignKeyValue, Is.Not.Null);
            Assert.That(foreignKeyProxy.ForeignKeyValue, Is.Not.Empty);
            Assert.That(foreignKeyProxy.ForeignKeyValue, Is.EqualTo(foreignKeyMock.ForeignKeyValue));
        }

        /// <summary>
        /// Tests that Map maps StaticText to StaticTextProxy.
        /// </summary>
        [Test]
        public void TestThatMapMapsStaticTextToStaticTextProxy()
        {
            var staticTextMock = DomainObjectMockBuilder.BuildStaticTextMock();

            var foodWasteObjectMapper = new FoodWasteObjectMapper();
            Assert.That(foodWasteObjectMapper, Is.Not.Null);

            var staticTextProxy = foodWasteObjectMapper.Map<IStaticText, IStaticTextProxy>(staticTextMock);
            Assert.That(staticTextProxy.Identifier, Is.Not.Null);
            Assert.That(staticTextProxy.Identifier, Is.EqualTo(staticTextMock.Identifier));
            Assert.That(staticTextProxy.Type, Is.EqualTo(staticTextMock.Type));
            Assert.That(staticTextProxy.Translation, Is.Null);
            Assert.That(staticTextProxy.Translations, Is.Not.Null);
            Assert.That(staticTextProxy.Translations, Is.Not.Empty);
            Assert.That(staticTextProxy.Translations.Count(), Is.EqualTo(staticTextMock.Translations.Count()));
            foreach (var translation in staticTextProxy.Translations)
            {
                Assert.That(translation, Is.Not.Null);
                Assert.That(translation, Is.TypeOf<TranslationProxy>());
            }
            Assert.That(staticTextProxy.SubjectTranslationIdentifier, Is.EqualTo(staticTextMock.SubjectTranslationIdentifier));
            Assert.That(staticTextProxy.SubjectTranslation, Is.Null);
            Assert.That(staticTextProxy.SubjectTranslations, Is.Not.Null);
            Assert.That(staticTextProxy.SubjectTranslations, Is.Not.Empty);
            Assert.That(staticTextProxy.SubjectTranslations.Count(), Is.EqualTo(staticTextMock.SubjectTranslations.Count()));
            foreach (var subjectTranslation in staticTextProxy.SubjectTranslations)
            {
                Assert.That(subjectTranslation, Is.Not.Null);
                Assert.That(subjectTranslation, Is.TypeOf<TranslationProxy>());
            }
            Assert.That(staticTextProxy.BodyTranslationIdentifier, Is.EqualTo(staticTextMock.BodyTranslationIdentifier));
            Assert.That(staticTextProxy.BodyTranslation, Is.Null);
            Assert.That(staticTextProxy.BodyTranslations, Is.Not.Null);
            Assert.That(staticTextProxy.BodyTranslations, Is.Not.Empty);
            Assert.That(staticTextProxy.BodyTranslations.Count(), Is.EqualTo(staticTextMock.BodyTranslations.Count()));
            foreach (var bodyTranslation in staticTextProxy.BodyTranslations)
            {
                Assert.That(bodyTranslation, Is.Not.Null);
                Assert.That(bodyTranslation, Is.TypeOf<TranslationProxy>());
            }
        }

        /// <summary>
        /// Tests that Map maps DataProvider to DataProviderView.
        /// </summary>
        [Test]
        public void TestThatMapMapsDataProviderToDataProviderView()
        {
            var dataProviderMock = DomainObjectMockBuilder.BuildDataProviderMock();

            var foodWasteObjectMapper = new FoodWasteObjectMapper();
            Assert.That(foodWasteObjectMapper, Is.Not.Null);

            var dataProviderView = foodWasteObjectMapper.Map<IDataProvider, DataProviderView>(dataProviderMock);
            Assert.That(dataProviderView.DataProviderIdentifier, Is.Not.Null);
            Assert.That(dataProviderView.DataProviderIdentifier, Is.EqualTo(dataProviderMock.Identifier.HasValue ? dataProviderMock.Identifier.Value : Guid.Empty));
            Assert.That(dataProviderView.Name, Is.Not.Null);
            Assert.That(dataProviderView.Name, Is.Not.Empty);
            Assert.That(dataProviderView.Name, Is.EqualTo(dataProviderMock.Name));
            Assert.That(dataProviderView.DataSourceStatement, Is.Not.Null);
            Assert.That(dataProviderView.DataSourceStatement, Is.Not.Empty);
            Assert.That(dataProviderView.DataSourceStatement, Is.EqualTo(dataProviderMock.DataSourceStatement != null ? dataProviderMock.DataSourceStatement.Value : string.Empty));
        }

        /// <summary>
        /// Tests that Map maps DataProvider to DataProviderSystemView.
        /// </summary>
        [Test]
        public void TestThatMapMapsDataProviderToDataProviderSystemView()
        {
            var dataProviderMock = DomainObjectMockBuilder.BuildDataProviderMock();

            var foodWasteObjectMapper = new FoodWasteObjectMapper();
            Assert.That(foodWasteObjectMapper, Is.Not.Null);

            var dataProviderSystemView = foodWasteObjectMapper.Map<IDataProvider, DataProviderSystemView>(dataProviderMock);
            Assert.That(dataProviderSystemView.DataProviderIdentifier, Is.Not.Null);
            Assert.That(dataProviderSystemView.DataProviderIdentifier, Is.EqualTo(dataProviderMock.Identifier.HasValue ? dataProviderMock.Identifier.Value : Guid.Empty));
            Assert.That(dataProviderSystemView.Name, Is.Not.Null);
            Assert.That(dataProviderSystemView.Name, Is.Not.Empty);
            Assert.That(dataProviderSystemView.Name, Is.EqualTo(dataProviderMock.Name));
            Assert.That(dataProviderSystemView.DataSourceStatementIdentifier, Is.EqualTo(dataProviderMock.DataSourceStatementIdentifier));
            Assert.That(dataProviderSystemView.DataSourceStatements, Is.Not.Null);
            Assert.That(dataProviderSystemView.DataSourceStatements, Is.Not.Empty);
            Assert.That(dataProviderSystemView.DataSourceStatements.Count(), Is.EqualTo(dataProviderMock.DataSourceStatements.Count()));
            foreach (var dataSourceStatement in dataProviderSystemView.DataSourceStatements)
            {
                Assert.That(dataSourceStatement, Is.Not.Null);
                Assert.That(dataSourceStatement, Is.TypeOf<TranslationSystemView>());
            }
        }

        /// <summary>
        /// Tests that Map maps DataProvider to DataProviderProxy.
        /// </summary>
        [Test]
        public void TestThatMapMapsDataProviderToDataProviderProxy()
        {
            var dataProviderMock = DomainObjectMockBuilder.BuildDataProviderMock();

            var foodWasteObjectMapper = new FoodWasteObjectMapper();
            Assert.That(foodWasteObjectMapper, Is.Not.Null);

            var dataProviderProxy = foodWasteObjectMapper.Map<IDataProvider, IDataProviderProxy>(dataProviderMock);
            Assert.That(dataProviderProxy.Identifier, Is.Not.Null);
            Assert.That(dataProviderProxy.Identifier, Is.EqualTo(dataProviderMock.Identifier));
            Assert.That(dataProviderProxy.Translation, Is.Null);
            Assert.That(dataProviderProxy.Translations, Is.Not.Null);
            Assert.That(dataProviderProxy.Translations, Is.Not.Empty);
            Assert.That(dataProviderProxy.Translations.Count(), Is.EqualTo(dataProviderMock.Translations.Count()));
            foreach (var translation in dataProviderProxy.Translations)
            {
                Assert.That(translation, Is.Not.Null);
                Assert.That(translation, Is.TypeOf<TranslationProxy>());
            }
            Assert.That(dataProviderProxy.Name, Is.Not.Null);
            Assert.That(dataProviderProxy.Name, Is.Not.Empty);
            Assert.That(dataProviderProxy.Name, Is.EqualTo(dataProviderMock.Name));
            Assert.That(dataProviderProxy.DataSourceStatementIdentifier, Is.EqualTo(dataProviderMock.DataSourceStatementIdentifier));
            Assert.That(dataProviderProxy.DataSourceStatement, Is.Null);
            Assert.That(dataProviderProxy.DataSourceStatements, Is.Not.Null);
            Assert.That(dataProviderProxy.DataSourceStatements, Is.Not.Empty);
            Assert.That(dataProviderProxy.DataSourceStatements.Count(), Is.EqualTo(dataProviderMock.DataSourceStatements.Count()));
            foreach (var dataSourceStatement in dataProviderProxy.DataSourceStatements)
            {
                Assert.That(dataSourceStatement, Is.Not.Null);
                Assert.That(dataSourceStatement, Is.TypeOf<TranslationProxy>());
            }
        }

        /// <summary>
        /// Tests that Map maps Translation to TranslationSystemView.
        /// </summary>
        [Test]
        public void TestThatMapMapsTranslationToTranslationSystemView()
        {
            var translationMock = DomainObjectMockBuilder.BuildTranslationMock(Guid.NewGuid());

            var foodWasteObjectMapper = new FoodWasteObjectMapper();
            Assert.That(foodWasteObjectMapper, Is.Not.Null);

            var translationInfoSystemView = foodWasteObjectMapper.Map<ITranslation, TranslationSystemView>(translationMock);
            Assert.That(translationInfoSystemView.TranslationIdentifier, Is.Not.Null);
            Assert.That(translationInfoSystemView.TranslationIdentifier, Is.EqualTo(translationMock.Identifier.HasValue ? translationMock.Identifier.Value : Guid.Empty));
            Assert.That(translationInfoSystemView.TranslationOfIdentifier, Is.EqualTo(translationMock.TranslationOfIdentifier));
            Assert.That(translationInfoSystemView.TranslationInfo, Is.Not.Null);
            Assert.That(translationInfoSystemView.TranslationInfo, Is.TypeOf<TranslationInfoSystemView>());
            Assert.That(translationInfoSystemView.Translation, Is.Not.Null);
            Assert.That(translationInfoSystemView.Translation, Is.Not.Empty);
            Assert.That(translationInfoSystemView.Translation, Is.EqualTo(translationMock.Value));
        }

        /// <summary>
        /// Tests that Map maps Translation to TranslationProxy.
        /// </summary>
        [Test]
        public void TestThatMapMapsTranslationToTranslationProxy()
        {
            var translationMock = DomainObjectMockBuilder.BuildTranslationMock(Guid.NewGuid());

            var foodWasteObjectMapper = new FoodWasteObjectMapper();
            Assert.That(foodWasteObjectMapper, Is.Not.Null);

            var translationProxy = foodWasteObjectMapper.Map<ITranslation, ITranslationProxy>(translationMock);
            Assert.That(translationProxy.Identifier, Is.Not.Null);
            Assert.That(translationProxy.Identifier, Is.EqualTo(translationMock.Identifier));
            Assert.That(translationProxy.TranslationOfIdentifier, Is.EqualTo(translationMock.TranslationOfIdentifier));
            Assert.That(translationProxy.TranslationInfo, Is.Not.Null);
            Assert.That(translationProxy.TranslationInfo, Is.TypeOf<TranslationInfoProxy>());
            Assert.That(translationProxy.Value, Is.Not.Null);
            Assert.That(translationProxy.Value, Is.Not.Empty);
            Assert.That(translationProxy.Value, Is.EqualTo(translationMock.Value));
        }

        /// <summary>
        /// Tests that Map maps TranslationInfo to TranslationInfoSystemView.
        /// </summary>
        [Test]
        [TestCase("da-DK")]
        [TestCase("en-US")]
        public void TestThatMapMapsTranslationInfoToTranslationInfoSystemView(string cultureName)
        {
            var translationInfoMock = DomainObjectMockBuilder.BuildTranslationInfoMock(cultureName);

            var foodWasteObjectMapper = new FoodWasteObjectMapper();
            Assert.That(foodWasteObjectMapper, Is.Not.Null);

            var translationInfoSystemView = foodWasteObjectMapper.Map<ITranslationInfo, TranslationInfoSystemView>(translationInfoMock);
            Assert.That(translationInfoSystemView.TranslationInfoIdentifier, Is.Not.Null);
            Assert.That(translationInfoSystemView.TranslationInfoIdentifier, Is.EqualTo(translationInfoMock.Identifier.HasValue ? translationInfoMock.Identifier.Value : Guid.Empty));
            Assert.That(translationInfoSystemView.CultureName, Is.Not.Null);
            Assert.That(translationInfoSystemView.CultureName, Is.Not.Empty);
            Assert.That(translationInfoSystemView.CultureName, Is.EqualTo(cultureName));
        }

        /// <summary>
        /// Tests that Map maps TranslationInfo to TranslationInfoProxy.
        /// </summary>
        [Test]
        [TestCase("da-DK")]
        [TestCase("en-US")]
        public void TestThatMapMapsTranslationInfoToTranslationInfoProxy(string cultureName)
        {
            var translationInfoMock = DomainObjectMockBuilder.BuildTranslationInfoMock(cultureName);

            var foodWasteObjectMapper = new FoodWasteObjectMapper();
            Assert.That(foodWasteObjectMapper, Is.Not.Null);

            var translationInfoProxy = foodWasteObjectMapper.Map<ITranslationInfo, ITranslationInfoProxy>(translationInfoMock);
            Assert.That(translationInfoProxy.Identifier, Is.Not.Null);
            Assert.That(translationInfoProxy.Identifier, Is.EqualTo(translationInfoMock.Identifier));
            Assert.That(translationInfoProxy.CultureName, Is.Not.Null);
            Assert.That(translationInfoProxy.CultureName, Is.Not.Empty);
            Assert.That(translationInfoProxy.CultureName, Is.EqualTo(cultureName));
            Assert.That(translationInfoProxy.CultureInfo, Is.Not.Null);
            Assert.That(translationInfoProxy.CultureInfo.Name, Is.Not.Null);
            Assert.That(translationInfoProxy.CultureInfo.Name, Is.Not.Empty);
            Assert.That(translationInfoProxy.CultureInfo.Name, Is.EqualTo(cultureName));
        }

        /// <summary>
        /// Tests that Map maps an identifiable to ServiceReceiptResponse.
        /// </summary>
        [Test]
        public void TestThatMapMapsIIdentifiableToServiceReceiptResponse()
        {
            var identifiableMock = DomainObjectMockBuilder.BuildIdentifiableMock();

            var foodWasteObjectMapper = new FoodWasteObjectMapper();
            Assert.That(foodWasteObjectMapper, Is.Not.Null);

            var serviceReceiptResponse = foodWasteObjectMapper.Map<IIdentifiable, ServiceReceiptResponse>(identifiableMock);
            Assert.That(serviceReceiptResponse.Identifier, Is.EqualTo(identifiableMock.Identifier));
            Assert.That(serviceReceiptResponse.EventDate, Is.EqualTo(DateTime.Now).Within(3).Seconds);
        }
    }
}
