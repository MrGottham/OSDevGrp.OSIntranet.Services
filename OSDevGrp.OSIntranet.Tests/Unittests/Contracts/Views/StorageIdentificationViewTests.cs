using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Views;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Views
{
    /// <summary>
    /// Tests the identification view for a storage.
    /// </summary>
    [TestFixture]
    public class StorageIdentificationViewTests
    {
        #region Private variables

        private Fixture _fixture;

        #endregion

        /// <summary>
        /// Setup each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        /// <summary>
        /// Tests that the identification view for a storage can be initialized.
        /// </summary>
        [Test]
        public void TestThatStorageIdentificationViewCanBeInitialized()
        {
            StorageIdentificationView storageIdentificationView = _fixture.Create<StorageIdentificationView>();
            DataContractTestHelper.TestAtContractErInitieret(storageIdentificationView);
        }

        /// <summary>
        /// Tests that the identification view for a storage can be serialized.
        /// </summary>
        [Test]
        public void TestThatStorageIdentificationViewCanBeSerialized()
        {
            StorageIdentificationView storageIdentificationView = _fixture.Create<StorageIdentificationView>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(storageIdentificationView);
        }
    }
}
