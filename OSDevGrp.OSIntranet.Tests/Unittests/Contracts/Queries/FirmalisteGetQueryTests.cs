using OSDevGrp.OSIntranet.Contracts.Queries;
using NUnit.Framework;
using AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Queries
{
    /// <summary>
    /// Tester datakontrakt til forespørgelse efter en liste af firmaer.
    /// </summary>
    [TestFixture]
    public class FirmalisteGetQueryTests
    {
        /// <summary>
        /// Tester, at Query kan initieres.
        /// </summary>
        [Test]
        public void TestAtQueryKanInitieres()
        {
            var fixture = new Fixture();
            var query = fixture.Create<FirmalisteGetQuery>();
            DataContractTestHelper.TestAtContractErInitieret(query);
        }

        /// <summary>
        /// Tester, at Query kan serialiseres.
        /// </summary>
        [Test]
        public void TestAtQueryKanSerialiseres()
        {
            var fixture = new Fixture();
            var query = fixture.Create<FirmalisteGetQuery>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(query);
        }
    }
}
