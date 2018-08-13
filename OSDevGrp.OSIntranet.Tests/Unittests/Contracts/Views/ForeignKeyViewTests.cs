using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Views;
using AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Views
{
    /// <summary>
    /// Tests the view for foreign key.
    /// </summary>
    [TestFixture]
    public class ForeignKeyViewTests
    {
        /// <summary>
        /// Tests that the view for foreign key can be initialized.
        /// </summary>
        [Test]
        public void TestThatForeignKeyViewCanBeInitialized()
        {
            var fixture = new Fixture();
            var foreignKeyView = fixture.Create<ForeignKeyView>();
            DataContractTestHelper.TestAtContractErInitieret(foreignKeyView);
        }

        /// <summary>
        /// Tests that the view for foreign key can be serialized.
        /// </summary>
        [Test]
        public void TestThatForeignKeyViewCanBeSerialized()
        {
            var fixture = new Fixture();
            var foreignKeyView = fixture.Create<ForeignKeyView>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(foreignKeyView);
        }
    }
}
