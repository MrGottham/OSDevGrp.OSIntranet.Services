using OSDevGrp.OSIntranet.Contracts.Faults;
using NUnit.Framework;
using AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Faults
{
    /// <summary>
    /// Tester fault for en fejl i repositories under OS Intranet.
    /// </summary>
    [TestFixture]
    public class IntranetRepositoryFaultTests
    {
        /// <summary>
        /// Tester, at Fault kan initieres.
        /// </summary>
        [Test]
        public void TestAtFaultKanInitieres()
        {
            var fixture = new Fixture();
            var fault = fixture.Create<IntranetRepositoryFault>();
            DataContractTestHelper.TestAtContractErInitieret(fault);
        }

        /// <summary>
        /// Tester, at Fault kan serialiseres.
        /// </summary>
        [Test]
        public void TestAtFaultKanSerialiseres()
        {
            var fixture = new Fixture();
            var fault = fixture.Create<IntranetRepositoryFault>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(fault);
        }
    }
}
