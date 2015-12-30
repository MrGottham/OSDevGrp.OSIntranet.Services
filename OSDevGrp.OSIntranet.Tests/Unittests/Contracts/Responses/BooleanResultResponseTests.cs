using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Responses;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Responses
{
    /// <summary>
    /// Tests the response for a boolean result.
    /// </summary>
    [TestFixture]
    public class BooleanResultResponseTests
    {
        /// <summary>
        /// Tests that the response for a boolean result can be initialized.
        /// </summary>
        [Test]
        public void TestThatBooleanResultResponseCanBeInitialized()
        {
            var fixture = new Fixture();
            var booleanResultResponse = fixture.Create<BooleanResultResponse>();
            DataContractTestHelper.TestAtContractErInitieret(booleanResultResponse);
        }

        /// <summary>
        /// Tests that the response for a boolean result can be serialized.
        /// </summary>
        [Test]
        public void TestThatBooleanResultResponseCanBeSerialized()
        {
            var fixture = new Fixture();
            var booleanResultResponse = fixture.Create<BooleanResultResponse>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(booleanResultResponse);
        }
    }
}
