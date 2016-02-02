using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Queries;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Queries
{
    /// <summary>
    /// Tests the query for getting a collection of data providers who handles payments.
    /// </summary>
    [TestFixture]
    public class DataProviderWhoHandlesPaymentsCollectionGetQueryTests
    {
        /// <summary>
        /// Tests that the query for getting a collection of data providers who handles payments can be initialized.
        /// </summary>
        [Test]
        public void TestThatDataProviderWhoHandlesPaymentsCollectionGetQueryCanBeInitialized()
        {
            var fixture = new Fixture();
            var dataProviderWhoHandlesPaymentsCollectionGetQuery = fixture.Create<DataProviderWhoHandlesPaymentsCollectionGetQuery>();
            DataContractTestHelper.TestAtContractErInitieret(dataProviderWhoHandlesPaymentsCollectionGetQuery);
        }

        /// <summary>
        /// Tests that the query for getting a collection of data providers who handles payments can be serialized.
        /// </summary>
        [Test]
        public void TestThatDataProviderWhoHandlesPaymentsCollectionGetQueryCanBeSerialized()
        {
            var fixture = new Fixture();
            var dataProviderWhoHandlesPaymentsCollectionGetQuery = fixture.Create<DataProviderWhoHandlesPaymentsCollectionGetQuery>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(dataProviderWhoHandlesPaymentsCollectionGetQuery);
        }
    }
}
