using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste
{
    /// <summary>
    /// Functionality for building domain object mockups for the food waste domain.
    /// </summary>
    public static class DomainObjectMockBuilder
    {
        /// <summary>
        /// Build a mockup for a food group.
        /// </summary>
        /// <param name="parentMock">Mockup for the parent food group.</param>
        /// <returns>Mockup for a food group.</returns>
        public static IFoodGroup BuildFoodGroupMock(IFoodGroup parentMock = null)
        {
            var identifier = Guid.NewGuid();
            var foodGroupMock = MockRepository.GenerateMock<IFoodGroup>();
            foodGroupMock.Stub(m => m.Identifier)
                .Return(identifier)
                .Repeat.Any();
            foodGroupMock.Stub(m => m.Parent)
                .Return(parentMock)
                .Repeat.Any();
            foodGroupMock.Stub(m => m.Children)
                .Return(parentMock != null ? new List<IFoodGroup>(0) : BuildFoodGroupMockCollection(foodGroupMock))
                .Repeat.Any();
            foodGroupMock.Stub(m => m.Translation)
                .Return(BuildTranslationMock(identifier))
                .Repeat.Any();
            foodGroupMock.Stub(m => m.Translations)
                .Return(BuildTranslationMockCollection(identifier))
                .Repeat.Any();
            foodGroupMock.Stub(m => m.ForeignKeys)
                .Return(BuildForeignKeyMockCollection(identifier, typeof (IFoodGroup)))
                .Repeat.Any();
            return foodGroupMock;
        }

        /// <summary>
        /// Build a collection of mockups for some food groups.
        /// </summary>
        /// <param name="parentMock">Mockup for the parent food group.</param>
        /// <returns>Collection of mockups for some food groups.</returns>
        public static IEnumerable<IFoodGroup> BuildFoodGroupMockCollection(IFoodGroup parentMock = null)
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());
            var result = new List<IFoodGroup>(random.Next(1, 25));
            while (result.Count < result.Capacity)
            {
                result.Add(BuildFoodGroupMock(parentMock));
            }
            return result;
        }

        /// <summary>
        /// Build a collection of mockups for a foreign key to a domain object in the food waste domain.
        /// </summary>
        /// <param name="foreignKeyForIdentifier">Identifier for the domain object which has the foreign keys.</param>
        /// <param name="foreignKeyForType">Type on which has the foreign keys.</param>
        /// <returns>Collection of mockups for a foreign key to a domain object in the food waste domain.</returns>
        public static IEnumerable<IForeignKey> BuildForeignKeyMockCollection(Guid foreignKeyForIdentifier, Type foreignKeyForType)
        {
            return new List<IForeignKey>
            {
                BuildForeignKeyMock(foreignKeyForIdentifier, foreignKeyForType),
                BuildForeignKeyMock(foreignKeyForIdentifier, foreignKeyForType),
                BuildForeignKeyMock(foreignKeyForIdentifier, foreignKeyForType)
            };
        }

        /// <summary>
        /// Build a mockup for a foreign key to a domain object in the food waste domain.
        /// </summary>
        /// <param name="foreignKeyForIdentifier">Identifier for the domain object which has the foreign key.</param>
        /// <param name="foreignKeyForType">Type on which has the foreign key.</param>
        /// <returns>Mockup for a foreign key to a to a domain object in the food waste domain.</returns>
        public static IForeignKey BuildForeignKeyMock(Guid foreignKeyForIdentifier, Type foreignKeyForType)
        {
            if (foreignKeyForType == null)
            {
                throw new ArgumentNullException("foreignKeyForType");
            }
            var fixture = new Fixture();
            var foreignKeyMock = MockRepository.GenerateMock<IForeignKey>();
            foreignKeyMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();
            foreignKeyMock.Stub(m => m.DataProvider)
                .Return(BuildDataProviderMock())
                .Repeat.Any();
            foreignKeyMock.Stub(m => m.ForeignKeyForIdentifier)
                .Return(foreignKeyForIdentifier)
                .Repeat.Any();
            foreignKeyMock.Stub(m => m.ForeignKeyForTypes)
                .Return(new List<Type> {typeof (IDomainObject), typeof (IIdentifiable), foreignKeyForType})
                .Repeat.Any();
            foreignKeyMock.Stub(m => m.ForeignKeyValue)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            return foreignKeyMock;
        }

            /// <summary>
        /// Build a mockup for a data provider.
        /// </summary>
        /// <returns>Mockup for a data provider.</returns>
        public static IDataProvider BuildDataProviderMock()
        {
            var fixture = new Fixture();
            var dataSourceStatementIdentifier = Guid.NewGuid();
            var dataProviderMock = MockRepository.GenerateMock<IDataProvider>();
            dataProviderMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();
            dataProviderMock.Stub(m => m.Translation)
                .Return(BuildTranslationMock(dataSourceStatementIdentifier))
                .Repeat.Any();
            dataProviderMock.Stub(m => m.Translations)
                .Return(BuildTranslationMockCollection(dataSourceStatementIdentifier))
                .Repeat.Any();
            dataProviderMock.Stub(m => m.Name)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            dataProviderMock.Stub(m => m.DataSourceStatementIdentifier)
                .Return(dataSourceStatementIdentifier)
                .Repeat.Any();
            dataProviderMock.Stub(m => m.DataSourceStatement)
                .Return(BuildTranslationMock(dataSourceStatementIdentifier))
                .Repeat.Any();
            dataProviderMock.Stub(m => m.DataSourceStatements)
                .Return(BuildTranslationMockCollection(dataSourceStatementIdentifier))
                .Repeat.Any();
            return dataProviderMock;
        }

        /// <summary>
        /// Build a collection of mockups for some data providers.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IDataProvider> BuildDataProviderMockCollection()
        {
            return new List<IDataProvider>
            {
                BuildDataProviderMock(),
                BuildDataProviderMock(),
                BuildDataProviderMock()
            };
        }

        /// <summary>
        /// Build a mockup for a translation.
        /// </summary>
        /// <param name="translationOfIdentifier">Identifier for the domain object which are translated by the translation.</param>
        /// <returns>Mockup for a translation.</returns>
        public static ITranslation BuildTranslationMock(Guid translationOfIdentifier)
        {
            return BuildTranslationMock(Thread.CurrentThread.CurrentUICulture.Name, translationOfIdentifier);
        }

        /// <summary>
        /// Build a mockup for a translation.
        /// </summary>
        /// <param name="cultureName">Name for the culture which are used for translation.</param>
        /// <param name="translationOfIdentifier">Identifier for the domain object which are translated by the translation.</param>
        /// <returns>Mockup for a translation.</returns>
        public static ITranslation BuildTranslationMock(string cultureName, Guid translationOfIdentifier)
        {
            var fixture = new Fixture();
            var translationMock = MockRepository.GenerateMock<ITranslation>();
            translationMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();
            translationMock.Stub(m => m.TranslationOfIdentifier)
                .Return(translationOfIdentifier)
                .Repeat.Any();
            translationMock.Stub(m => m.TranslationInfo)
                .Return(BuildTranslationInfoMock(cultureName))
                .Repeat.Any();
            translationMock.Stub(m => m.Value)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            return translationMock;
        }

        /// <summary>
        /// Build a collection of mockups for some translations.
        /// </summary>
        /// <param name="translationOfIdentifier">Identifier for the domain object which are translated by the translations.</param>
        /// <returns>Collection of mockups for some translations.</returns>
        public static IEnumerable<ITranslation> BuildTranslationMockCollection(Guid translationOfIdentifier)
        {
            return new List<ITranslation>
            {
                BuildTranslationMock("da-DK", translationOfIdentifier),
                BuildTranslationMock("en-US", translationOfIdentifier)
            };
        }

        /// <summary>
        /// Build a mockup for translation informations for the current cultue.
        /// </summary>
        /// <returns>Mockup for translation informations for the current culture.</returns>
        public static ITranslationInfo BuildTranslationInfoMock()
        {
            return BuildTranslationInfoMock(Thread.CurrentThread.CurrentUICulture.Name);
        }

        /// <summary>
        /// Build a mockup for translation informations.
        /// </summary>
        /// <returns>Mockup for translation informations.</returns>
        public static ITranslationInfo BuildTranslationInfoMock(string cultureName)
        {
            if (string.IsNullOrEmpty(cultureName))
            {
                throw new ArgumentNullException("cultureName");
            }
            var translationInfoMock = MockRepository.GenerateMock<ITranslationInfo>();
            translationInfoMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();
            translationInfoMock.Stub(m => m.CultureName)
                .Return(cultureName)
                .Repeat.Any();
            translationInfoMock.Stub(m => m.CultureInfo)
                .Return(new CultureInfo(cultureName))
                .Repeat.Any();
            return translationInfoMock;
        }

        /// <summary>
        /// Build a collection of mockups for translation informations.
        /// </summary>
        /// <returns>Collection of mockups for translation informations.</returns>
        public static IEnumerable<ITranslationInfo> BuildTranslationInfoMockCollection()
        {
            return new List<ITranslationInfo>
            {
                BuildTranslationInfoMock("da-DK"),
                BuildTranslationInfoMock("en-US")
            };
        }

        /// <summary>
        /// Build a mockup for an identifiable domain object in the food waste domain.
        /// </summary>
        /// <returns>Mockup for an identifiable domain object in the food waste domain.</returns>
        public static IIdentifiable BuildIdentifiableMock()
        {
            var identifibaleMock = MockRepository.GenerateMock<IIdentifiable>();
            identifibaleMock.Expect(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();
            return identifibaleMock;
        }
    }
}
