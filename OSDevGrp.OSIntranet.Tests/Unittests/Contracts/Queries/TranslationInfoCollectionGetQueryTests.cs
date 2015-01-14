using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Queries;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Queries
{
    /// <summary>
    /// Tests the query for getting a collection of translation informations.
    /// </summary>
    [TestFixture]
    public class TranslationInfoCollectionGetQueryTests
    {
        /// <summary>
        /// Tests that the query for getting a collection of translation informations can be initialized.
        /// </summary>
        [Test]
        public void TestThatTranslationInfoCollectionGetQueryCanBeInitialized()
        {
            var fixture = new Fixture();
            var translationInfoCollectionQuery = fixture.Create<TranslationInfoCollectionGetQuery>();
            DataContractTestHelper.TestAtContractErInitieret(translationInfoCollectionQuery);
        }

        /// <summary>
        /// Tests that the query for getting a collection of translation informations can be serialized.
        /// </summary>
        [Test]
        public void TestThatTranslationInfoCollectionGetQueryCanBeSerialized()
        {
            var fixture = new Fixture();
            var translationInfoCollectionQuery = fixture.Create<TranslationInfoCollectionGetQuery>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(translationInfoCollectionQuery);
        }
    }
}
