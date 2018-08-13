using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Views;
using AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Views
{
    /// <summary>
    /// Tests the system view for a storage type.
    /// </summary>
    [TestFixture]
    public class StorageTypeSystemViewTests
    {
        /// <summary>
        /// Tests that the system view for a storage type can be initialized.
        /// </summary>
        [Test]
        public void TestThatStorageTypeSystemViewCanBeInitialized()
        {
            Fixture fixture = new Fixture();
            StorageTypeSystemView storageTypeSystemView = fixture.Create<StorageTypeSystemView>();
            DataContractTestHelper.TestAtContractErInitieret(storageTypeSystemView);
        }

        /// <summary>
        /// Tests that the system view for a storage type can be serialized.
        /// </summary>
        [Test]
        public void TestThatStorageTypeSystemViewCanBeSerialized()
        {
            Fixture fixture = new Fixture();
            StorageTypeSystemView storageTypeSystemView = fixture.Create<StorageTypeSystemView>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(storageTypeSystemView);
        }
    }
}
