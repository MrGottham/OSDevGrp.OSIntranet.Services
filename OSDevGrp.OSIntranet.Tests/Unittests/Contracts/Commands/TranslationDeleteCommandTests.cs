using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Commands;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Commands
{
    /// <summary>
    /// Tests the command for deleting a translation.
    /// </summary>
    [TestFixture]
    public class TranslationDeleteCommandTests
    {
        /// <summary>
        /// Tests that the command for deleting a translation can be initialized.
        /// </summary>
        [Test]
        public void TestThatTranslationDeleteCommandCanBeInitialized()
        {
            var fixture = new Fixture();
            var translationDeleteCommand = fixture.Create<TranslationDeleteCommand>();
            DataContractTestHelper.TestAtContractErInitieret(translationDeleteCommand);
        }

        /// <summary>
        /// Tests that the command for deleting a translation can be serialized.
        /// </summary>
        [Test]
        public void TestThatTranslationDeleteCommandCanBeSerialized()
        {
            var fixture = new Fixture();
            var translationDeleteCommand = fixture.Create<TranslationDeleteCommand>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(translationDeleteCommand);
        }
    }
}
