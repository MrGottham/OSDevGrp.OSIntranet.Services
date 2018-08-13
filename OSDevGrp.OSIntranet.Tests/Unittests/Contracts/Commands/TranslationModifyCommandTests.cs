using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Commands;
using AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Commands
{
    /// <summary>
    /// Tests the command for modifying a translation.
    /// </summary>
    [TestFixture]
    public class TranslationModifyCommandTests
    {
        /// <summary>
        /// Tests that the command for modifying a translation can be initialized.
        /// </summary>
        [Test]
        public void TestThatTranslationModifyCommandCanBeInitialized()
        {
            var fixture = new Fixture();
            var translationModifyCommand = fixture.Create<TranslationModifyCommand>();
            DataContractTestHelper.TestAtContractErInitieret(translationModifyCommand);
        }

        /// <summary>
        /// Tests that the command for modifying a translation can be serialized.
        /// </summary>
        [Test]
        public void TestThatTranslationModifyCommandCanBeSerialized()
        {
            var fixture = new Fixture();
            var translationModifyCommand = fixture.Create<TranslationModifyCommand>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(translationModifyCommand);
        }
    }
}
