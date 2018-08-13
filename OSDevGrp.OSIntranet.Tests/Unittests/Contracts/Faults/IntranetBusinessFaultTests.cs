using OSDevGrp.OSIntranet.Contracts.Faults;
using NUnit.Framework;
using AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Faults
{
    /// <summary>
    /// Tester fault for forretningslogik under OS Intranet.
    /// </summary>
    [TestFixture]
    public class IntranetBusinessFaultTests
    {
        /// <summary>
        /// Tester, at Fault kan initieres.
        /// </summary>
        [Test]
        public void TestAtFaultKanInitieres()
        {
            var fixture = new Fixture();
            var fault = fixture.Create<IntranetBusinessFault>();
            DataContractTestHelper.TestAtContractErInitieret(fault);
        }

        /// <summary>
        /// Tester, at Fault kan serialiseres.
        /// </summary>
        [Test]
        public void TestAtFaultKanSerialiseres()
        {
            var fixture = new Fixture();
            var fault = fixture.Create<IntranetBusinessFault>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(fault);
        }
    }
}
