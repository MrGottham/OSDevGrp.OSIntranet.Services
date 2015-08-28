using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Queries;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Queries
{
    /// <summary>
    /// Tests the query for getting a collection of data providers.
    /// </summary>
    [TestFixture]
    public class DataProviderCollectionGetQueryTests
    {
        /// <summary>
        /// Tests that the query for getting a collection of data providers can be initialized.
        /// </summary>
        [Test]
        public void TestThatDataProviderCollectionGetQueryCanBeInitialized()
        {
            var fixture = new Fixture();
            var dataProviderCollectionGetQuery = fixture.Create<DataProviderCollectionGetQuery>();
            DataContractTestHelper.TestAtContractErInitieret(dataProviderCollectionGetQuery);
        }

        /// <summary>
        /// Tests that the query for getting a collection of data providers can be serialized.
        /// </summary>
        [Test]
        public void TestThatDataProviderCollectionGetQueryCanBeSerialized()
        {
            var fixture = new Fixture();
            var dataProviderCollectionGetQuery = fixture.Create<DataProviderCollectionGetQuery>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(dataProviderCollectionGetQuery);
        }
    }
}
