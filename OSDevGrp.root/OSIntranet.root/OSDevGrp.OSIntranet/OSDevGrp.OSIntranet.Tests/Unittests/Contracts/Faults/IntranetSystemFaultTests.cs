using OSDevGrp.OSIntranet.Contracts.Faults;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Faults
{
    /// <summary>
    /// Tester fault for systemfejl i OS Intranet.
    /// </summary>
    [TestFixture]
    public class IntranetSystemFaultTests
    {
        /// <summary>
        /// Tester, at Fault kan initieres.
        /// </summary>
        [Test]
        public void TestAtFaultKanInitieres()
        {
            var fixture = new Fixture();
            var fault = fixture.CreateAnonymous<IntranetSystemFault>();
            DataContractTestHelper.TestAtContractErInitieret(fault);
        }

        /// <summary>
        /// Tester, at Fault kan serialiseres.
        /// </summary>
        [Test]
        public void TestAtFaultKanSerialiseres()
        {
            var fixture = new Fixture();
            var fault = fixture.CreateAnonymous<IntranetSystemFault>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(fault);
        }
    }
}
