using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.Contracts.Views;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Views
{
    /// <summary>
    /// Tester viewobject for en konto.
    /// </summary>
    [TestFixture]
    public class KontoViewTests
    {
        /// <summary>
        /// Tester, at viewobjekt kan initieres.
        /// </summary>
        [Test]
        public void TestAtViewKanInitieres()
        {
            var fixture = new Fixture();
            fixture.Inject<IEnumerable<KreditoplysningerView>>(fixture.CreateMany<KreditoplysningerView>(24).ToList());
            var view = fixture.Create<KontoView>();
            DataContractTestHelper.TestAtContractErInitieret(view);
        }

        /// <summary>
        /// Tester, at viewobjekt kan serialiseres.
        /// </summary>
        [Test]
        public void TestAtViewKanSerialiseres()
        {
            var fixture = new Fixture();
            fixture.Inject<IEnumerable<KreditoplysningerView>>(fixture.CreateMany<KreditoplysningerView>(24).ToList());
            var view = fixture.Create<KontoView>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(view);
        }
    }
}
