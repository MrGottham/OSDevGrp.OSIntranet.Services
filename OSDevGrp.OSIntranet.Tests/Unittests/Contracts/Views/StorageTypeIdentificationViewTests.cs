using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Views;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Views
{
    /// <summary>
    /// Tests the identification view for a storage type.
    /// </summary>
    [TestFixture]
    public class StorageTypeIdentificationViewTests
    {
        /// <summary>
        /// Tests that the identification view for a storage type can be initialized.
        /// </summary>
        [Test]
        public void TestThatStorageTypeIdentificationViewCanBeInitialized()
        {
            Fixture fixture = new Fixture();
            StorageTypeIdentificationView storageTypeIdentificationView = fixture.Create<StorageTypeIdentificationView>();
            DataContractTestHelper.TestAtContractErInitieret(storageTypeIdentificationView);
        }

        /// <summary>
        /// Tests that the identification view for a storage type can be serialized.
        /// </summary>
        [Test]
        public void TestThatStorageTypeIdentificationViewCanBeSerialized()
        {
            Fixture fixture = new Fixture();
            StorageTypeIdentificationView storageTypeIdentificationView = fixture.Create<StorageTypeIdentificationView>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(storageTypeIdentificationView);
        }
    }
}
