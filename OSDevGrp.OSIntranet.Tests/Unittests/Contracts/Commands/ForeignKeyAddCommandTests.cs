using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Commands;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Commands
{
    /// <summary>
    /// Tests the command for adding a foreign key.
    /// </summary>
    [TestFixture]
    public class ForeignKeyAddCommandTests
    {
        /// <summary>
        /// Tests that the command for adding a foreign key can be initialized.
        /// </summary>
        [Test]
        public void TestThatForeignKeyAddCommandCanBeInitialized()
        {
            var fixture = new Fixture();
            var foreignKeyAddCommand = fixture.Create<ForeignKeyAddCommand>();
            DataContractTestHelper.TestAtContractErInitieret(foreignKeyAddCommand);
        }

        /// <summary>
        /// Tests that the command for adding a foreign key can be serialized.
        /// </summary>
        [Test]
        public void TestThatForeignKeyAddCommandCanBeSerialized()
        {
            var fixture = new Fixture();
            var foreignKeyAddCommand = fixture.Create<ForeignKeyAddCommand>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(foreignKeyAddCommand);
        }
    }
}
