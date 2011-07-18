using OSDevGrp.OSIntranet.Contracts.Queries;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Queries
{
    /// <summary>
    /// Tester datakontrakt til forespørgelse efter betalingsbetingelser.
    /// </summary>
    [TestFixture]
    public class BetalingsbetingelserGetQueryTests
    {
        /// <summary>
        /// Tester, at Query kan initieres.
        /// </summary>
        [Test]
        public void TestAtQueryKanInitieres()
        {
            var fixture = new Fixture();
            var query = fixture.CreateAnonymous<BetalingsbetingelserGetQuery>();
            DataContractTestHelper.TestAtContractErInitieret(query);
        }

        /// <summary>
        /// Tester, at Query kan serialiseres.
        /// </summary>
        [Test]
        public void TestAtQueryKanSerialiseres()
        {
            var fixture = new Fixture();
            var query = fixture.CreateAnonymous<BetalingsbetingelserGetQuery>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(query);
        }
    }
}
