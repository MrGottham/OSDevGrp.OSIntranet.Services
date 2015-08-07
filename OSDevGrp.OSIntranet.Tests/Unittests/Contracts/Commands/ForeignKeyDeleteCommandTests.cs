using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Commands;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Commands
{
    /// <summary>
    /// Tests the command for deleting a foreign key.
    /// </summary>
    [TestFixture]
    public class ForeignKeyDeleteCommandTests
    {
        /// <summary>
        /// Tests that the command for deleting a foreign key can be initialized.
        /// </summary>
        [Test]
        public void TestThatForeignKeyDeleteCommandCanBeInitialized()
        {
            var fixture = new Fixture();
            var foreignKeyDeleteCommand = fixture.Create<ForeignKeyDeleteCommand>();
            DataContractTestHelper.TestAtContractErInitieret(foreignKeyDeleteCommand);
        }

        /// <summary>
        /// Tests that the command for deleting a foreign key can be serialized.
        /// </summary>
        [Test]
        public void TestThatForeignKeyDeleteCommandCanBeSerialized()
        {
            var fixture = new Fixture();
            var foreignKeyDeleteCommand = fixture.Create<ForeignKeyDeleteCommand>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(foreignKeyDeleteCommand);
        }
    }
}
