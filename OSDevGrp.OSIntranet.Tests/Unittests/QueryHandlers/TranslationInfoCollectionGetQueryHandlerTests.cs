using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.QueryHandlers;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers
{
    /// <summary>
    /// Tests the functionality which handles the query for getting a collection of translation informations.
    /// </summary>
    [TestFixture]
    public class TranslationInfoCollectionGetQueryHandlerTests
    {
        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when repository which can access system data in the food waste domain is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenSystemDataRepositoryIsNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IFoodWasteObjectMapper>(e => e.FromFactory(() => MockRepository.GenerateMock<IFoodWasteObjectMapper>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new TranslationInfoCollectionGetQueryHandler(null, fixture.Create<IFoodWasteObjectMapper>()));
        }
    }
}
