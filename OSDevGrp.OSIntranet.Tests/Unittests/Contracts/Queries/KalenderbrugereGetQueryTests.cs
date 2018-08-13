using OSDevGrp.OSIntranet.Contracts.Queries;
using NUnit.Framework;
using AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Queries
{
    /// <summary>
    /// Tester datakontrakt til forespørgelse efter kalenderbrugere til et givent system under OSWEBDB.
    /// </summary>
    [TestFixture]
    public class KalenderbrugereGetQueryTests
    {
        /// <summary>
        /// Tester, at Query kan initieres.
        /// </summary>
        [Test]
        public void TestAtQueryKanInitieres()
        {
            var fixture = new Fixture();
            var query = fixture.Create<KalenderbrugereGetQuery>();
            DataContractTestHelper.TestAtContractErInitieret(query);
        }

        /// <summary>
        /// Tester, at Query kan serialiseres.
        /// </summary>
        [Test]
        public void TestAtQueryKanSerialiseres()
        {
            var fixture = new Fixture();
            var query = fixture.Create<KalenderbrugereGetQuery>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(query);
        }
    }
}
