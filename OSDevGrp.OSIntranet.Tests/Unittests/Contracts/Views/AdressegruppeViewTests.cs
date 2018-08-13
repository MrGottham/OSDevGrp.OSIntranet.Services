using OSDevGrp.OSIntranet.Contracts.Views;
using NUnit.Framework;
using AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Views
{
    /// <summary>
    /// Tester viewobject for en adressegruppe.
    /// </summary>
    [TestFixture]
    public class AdressegruppeViewTests
    {
        /// <summary>
        /// Tester, at viewobjekt kan initieres.
        /// </summary>
        [Test]
        public void TestAtViewKanInitieres()
        {
            var fixture = new Fixture();
            var view = fixture.Create<AdressegruppeView>();
            DataContractTestHelper.TestAtContractErInitieret(view);
        }

        /// <summary>
        /// Tester, at viewobjekt kan serialiseres.
        /// </summary>
        [Test]
        public void TestAtViewKanSerialiseres()
        {
            var fixture = new Fixture();
            var view = fixture.Create<AdressegruppeView>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(view);
        }
    }
}
