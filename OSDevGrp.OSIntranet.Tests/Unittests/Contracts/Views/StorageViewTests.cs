using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Views;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Views
{
    /// <summary>
    /// Tests the view for a storage.
    /// </summary>
    [TestFixture]
    public class StorageViewTests
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
        /// Tests that the view for a storage can be initialized.
        /// </summary>
        [Test]
        public void TestThatStorageViewCanBeInitialized()
        {
            StorageView storageView = _fixture.Create<StorageView>();
            DataContractTestHelper.TestAtContractErInitieret(storageView);
        }

        /// <summary>
        /// Tests that the view for a storage can be serialized.
        /// </summary>
        [Test]
        public void TestThatStorageViewCanBeSerialized()
        {
            StorageView storageView = _fixture.Create<StorageView>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(storageView);
        }
    }
}
