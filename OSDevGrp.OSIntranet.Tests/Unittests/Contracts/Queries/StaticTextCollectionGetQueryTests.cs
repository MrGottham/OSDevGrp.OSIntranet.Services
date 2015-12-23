using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Queries;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Queries
{
    /// <summary>
    /// Tests the query for getting a collection of static texts.
    /// </summary>
    [TestFixture]
    public class StaticTextCollectionGetQueryTests
    {
        /// <summary>
        /// Tests that the query for getting a collection of static texts can be initialized.
        /// </summary>
        [Test]
        public void TestThatStaticTextCollectionGetQueryCanBeInitialized()
        {
            var fixture = new Fixture();
            var staticTextCollectionGetQuery = fixture.Create<StaticTextCollectionGetQuery>();
            DataContractTestHelper.TestAtContractErInitieret(staticTextCollectionGetQuery);
        }

        /// <summary>
        /// Tests that the query for getting a collection of static texts can be serialized.
        /// </summary>
        [Test]
        public void TestThatStaticTextCollectionGetQueryCanBeSerialized()
        {
            var fixture = new Fixture();
            var staticTextCollectionGetQuery = fixture.Create<StaticTextCollectionGetQuery>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(staticTextCollectionGetQuery);
        }
    }
}
