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
        /// <returns>Mockup for a food group.</returns>
        public static IFoodGroup BuildFoodGroupMock()
        {
            var foodGroupMock = MockRepository.GenerateMock<IFoodGroup>();
            foodGroupMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();
            foodGroupMock.Stub(m => m.Translation)
                .Return(BuildTranslationMock())
                .Repeat.Any();
            foodGroupMock.Stub(m => m.Translations)
                .Return(BuildTranslationMockCollection())
                .Repeat.Any();
            return foodGroupMock;
        }

        /// <summary>
        /// Build a collection of mockups for some food groups.
        /// </summary>
        /// <returns>Collection of mockups for some food groups.</returns>
        public static IEnumerable<IFoodGroup> BuildFoodGroupMockCollection()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());
            var result = new List<IFoodGroup>(random.Next(1, 25));
            while (result.Count < result.Capacity)
            {
                result.Add(BuildFoodGroupMock());
            }
            return result;
        }

        /// <summary>
        /// Build a mockup for a translation.
        /// </summary>
        /// <returns>Mockup for a translation.</returns>
        public static ITranslation BuildTranslationMock()
        {
            return BuildTranslationMock(Thread.CurrentThread.CurrentUICulture.Name);
        }

        /// <summary>
        /// Build a collection of mockups for some translations.
        /// </summary>
        /// <returns>Collection of mockups for some translations.</returns>
        public static IEnumerable<ITranslation> BuildTranslationMockCollection()
        {
            return new List<ITranslation>
            {
                BuildTranslationMock("da-DK"),
                BuildTranslationMock("en-US")
            };
        }

        /// <summary>
        /// Build a mockup for a translation.
        /// </summary>
        /// <returns>Mockup for a translation.</returns>
        public static ITranslation BuildTranslationMock(string cultureName)
        {
            var fixture = new Fixture();
            var translationMock = MockRepository.GenerateMock<ITranslation>();
            translationMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();
            translationMock.Stub(m => m.TranslationOfIdentifier)
                .Return(Guid.NewGuid())
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
