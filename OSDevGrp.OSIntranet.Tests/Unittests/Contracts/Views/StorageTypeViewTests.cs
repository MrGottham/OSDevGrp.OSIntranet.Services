using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Views;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Views
{
    /// <summary>
    /// Tests the view for a storage type.
    /// </summary>
    public class StorageTypeViewTests
    {
        /// <summary>
        /// Tests that the view for a storage type can be initialized.
        /// </summary>
        [Test]
        public void TestThatStorageTypeViewCanBeInitialized()
        {
            Fixture fixture = new Fixture();
            StorageTypeView storageTypeView = fixture.Create<StorageTypeView>();
            DataContractTestHelper.TestAtContractErInitieret(storageTypeView);
        }

        /// <summary>
        /// Tests that the view for a storage type can be serialized.
        /// </summary>
        [Test]
        public void TestThatStorageTypeViewCanBeSerialized()
        {
            Fixture fixture = new Fixture();
            StorageTypeView storageTypeView = fixture.Create<StorageTypeView>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(storageTypeView);
        }
    }
}
