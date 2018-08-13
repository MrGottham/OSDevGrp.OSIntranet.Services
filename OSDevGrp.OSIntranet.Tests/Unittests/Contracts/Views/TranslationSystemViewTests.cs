using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Views;
using AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Views
{
    /// <summary>
    /// Tests the system view for a translation of a domain object.
    /// </summary>
    [TestFixture]
    public class TranslationSystemViewTests
    {
        /// <summary>
        /// Tests that the system view for a translation of a domain object can be initialized.
        /// </summary>
        [Test]
        public void TestThatTranslationSystemViewCanBeInitialized()
        {
            var fixture = new Fixture();
            var translationSystemView = fixture.Create<TranslationSystemView>();
            DataContractTestHelper.TestAtContractErInitieret(translationSystemView);
        }

        /// <summary>
        /// Tests that the system view for a translation of a domain object can be serialized.
        /// </summary>
        [Test]
        public void TestThatTranslationSystemViewCanBeSerialized()
        {
            var fixture = new Fixture();
            var translationSystemView = fixture.Create<TranslationSystemView>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(translationSystemView);
        }
    }
}
