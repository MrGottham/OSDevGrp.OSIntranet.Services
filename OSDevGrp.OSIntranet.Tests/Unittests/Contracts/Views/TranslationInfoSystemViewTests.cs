using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Views;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Views
{
    /// <summary>
    /// Tests the system view for translation informations which are used for translation.
    /// </summary>
    [TestFixture]
    public class TranslationInfoSystemViewTests
    {
        /// <summary>
        /// Tests that the system view for translation informations which are used for translation can be initialized.
        /// </summary>
        [Test]
        public void TestThatTranslationInfoSystemViewCanBeInitialized()
        {
            var fixture = new Fixture();
            var translationInfoSystemView = fixture.Create<TranslationInfoSystemView>();
            DataContractTestHelper.TestAtContractErInitieret(translationInfoSystemView);
        }

        /// <summary>
        /// Tests that the system view for translation informations which are used for translation can be serialized.
        /// </summary>
        [Test]
        public void TestThatTranslationInfoSystemViewCanBeSerialized()
        {
            var fixture = new Fixture();
            var translationInfoSystemView = fixture.Create<TranslationInfoSystemView>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(translationInfoSystemView);
        }
    }
}
