using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Views;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Views
{
    /// <summary>
    /// Tests the system view for foreign key.
    /// </summary>
    [TestFixture]
    public class ForeignKeySystemViewTests
    {
        /// <summary>
        /// Tests that the system view for foreign key can be initialized.
        /// </summary>
        [Test]
        public void TestThatForeignKeySystemViewCanBeInitialized()
        {
            var fixture = new Fixture();
            var foreignKeySystemView = fixture.Create<ForeignKeySystemView>();
            DataContractTestHelper.TestAtContractErInitieret(foreignKeySystemView);
        }

        /// <summary>
        /// Tests that the system view for foreign key can be serialized.
        /// </summary>
        [Test]
        public void TestThatForeignKeySystemViewCanBeSerialized()
        {
            var fixture = new Fixture();
            var foreignKeySystemView = fixture.Create<ForeignKeySystemView>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(foreignKeySystemView);
        }
    }
}
