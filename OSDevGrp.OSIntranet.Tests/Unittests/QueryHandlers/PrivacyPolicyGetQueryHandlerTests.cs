using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.QueryHandlers;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers
{
    /// <summary>
    /// Tests the functionality which handles the query for getting the privacy policy.
    /// </summary>
    [TestFixture]
    public class PrivacyPolicyGetQueryHandlerTests
    {
        /// <summary>
        /// Test that the constructor initialize the functionality which handles the query for getting the privacy policy.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializePrivacyPolicyGetQueryHandler()
        {
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            var privacyPolicyGetQueryHandler = new PrivacyPolicyGetQueryHandler(systemDataRepositoryMock, foodWasteObjectMapperMock);
            Assert.That(privacyPolicyGetQueryHandler, Is.Not.Null);
            Assert.That(privacyPolicyGetQueryHandler.StaticTextType, Is.EqualTo(StaticTextType.PrivacyPolicy));
        }
    }
}
