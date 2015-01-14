using System;
using System.Collections.Generic;
using System.Globalization;
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
    }
}
