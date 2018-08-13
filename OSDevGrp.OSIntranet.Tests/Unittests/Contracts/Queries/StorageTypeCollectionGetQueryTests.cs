using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Queries;
using AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Queries
{
    /// <summary>
    /// Tests the query for getting a collection of storage types.
    /// </summary>
    [TestFixture]
    public class StorageTypeCollectionGetQueryTests
    {
        /// <summary>
        /// Tests that the query for getting a collection of storage types can be initialized.
        /// </summary>
        [Test]
        public void TestThatStorageTypeCollectionGetQueryCanBeInitialized()
        {
            Fixture fixture = new Fixture();
            StorageTypeCollectionGetQuery storageTypeCollectionGetQuery = fixture.Create<StorageTypeCollectionGetQuery>();
            DataContractTestHelper.TestAtContractErInitieret(storageTypeCollectionGetQuery);
        }

        /// <summary>
        /// Tests that the query for getting a collection of storage types can be serialized.
        /// </summary>
        [Test]
        public void TestThatStorageTypeCollectionGetQueryCanBeSerialized()
        {
            Fixture fixture = new Fixture();
            StorageTypeCollectionGetQuery storageTypeCollectionGetQuery = fixture.Create<StorageTypeCollectionGetQuery>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(storageTypeCollectionGetQuery);
        }
    }
}
