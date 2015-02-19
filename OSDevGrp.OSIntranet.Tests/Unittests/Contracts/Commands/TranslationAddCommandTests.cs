using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Commands;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Commands
{
    /// <summary>
    /// Tests the command for adding a translation.
    /// </summary>
    [TestFixture]
    public class TranslationAddCommandTests
    {
        /// <summary>
        /// Tests that the command for adding a translation can be initialized.
        /// </summary>
        [Test]
        public void TestThatTranslationAddCommandCanBeInitialized()
        {
            var fixture = new Fixture();
            var translationAddCommand = fixture.Create<TranslationAddCommand>();
            DataContractTestHelper.TestAtContractErInitieret(translationAddCommand);
        }

        /// <summary>
        /// Tests that the command for adding a translation can be serialized.
        /// </summary>
        [Test]
        public void TestThatTranslationAddCommandCanBeSerialized()
        {
            var fixture = new Fixture();
            var translationAddCommand = fixture.Create<TranslationAddCommand>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(translationAddCommand);
        }
    }
}
