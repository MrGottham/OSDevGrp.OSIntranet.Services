using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Commands;
using AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Commands
{
    /// <summary>
    /// Tests the command for modifying a foreign key.
    /// </summary>
    [TestFixture]
    public class ForeignKeyModifyCommandTests
    {
        /// <summary>
        /// Tests that the command for modifying a foreign key can be initialized.
        /// </summary>
        [Test]
        public void TestThatForeignKeyModifyCommandCanBeInitialized()
        {
            var fixture = new Fixture();
            var foreignKeyModifyCommand = fixture.Create<ForeignKeyModifyCommand>();
            DataContractTestHelper.TestAtContractErInitieret(foreignKeyModifyCommand);
        }

        /// <summary>
        /// Tests that the command for modifying a foreign key can be serialized.
        /// </summary>
        [Test]
        public void TestThatForeignKeyModifyCommandCanBeSerialized()
        {
            var fixture = new Fixture();
            var foreignKeyModifyCommand = fixture.Create<ForeignKeyModifyCommand>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(foreignKeyModifyCommand);
        }
    }
}
