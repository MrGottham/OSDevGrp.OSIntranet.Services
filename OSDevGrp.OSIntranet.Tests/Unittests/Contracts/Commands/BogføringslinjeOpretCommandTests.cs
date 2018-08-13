using OSDevGrp.OSIntranet.Contracts.Commands;
using NUnit.Framework;
using AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Commands
{
    /// <summary>
    /// Tester datakontrakt til oprettelse af bogføringslinje.
    /// </summary>
    [TestFixture]
    public class BogføringslinjeOpretCommandTests
    {
        /// <summary>
        /// Tester, at Command kan initieres.
        /// </summary>
        [Test]
        public void TestAtCommandKanInitieres()
        {
            var fixture = new Fixture();
            var command = fixture.Create<BogføringslinjeOpretCommand>();
            DataContractTestHelper.TestAtContractErInitieret(command);
        }

        /// <summary>
        /// Tester, at Command kan serialiseres.
        /// </summary>
        [Test]
        public void TestAtCommandKanSerialiseres()
        {
            var fixture = new Fixture();
            var command = fixture.Create<BogføringslinjeOpretCommand>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(command);
        }
    }
}
