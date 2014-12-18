using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.Contracts.Views;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Views
{
    /// <summary>
    /// Tester viewobject for en kalenderbruger.
    /// </summary>
    [TestFixture]
    public class KalenderbrugerAftaleViewTests
    {
        /// <summary>
        /// Tester, at viewobjekt kan initieres.
        /// </summary>
        [Test]
        public void TestAtViewKanInitieres()
        {
            var fixture = new Fixture();
            fixture.Inject<IEnumerable<KalenderbrugerView>>(fixture.CreateMany<KalenderbrugerView>(3).ToList());
            var view = fixture.Create<KalenderbrugerAftaleView>();
            DataContractTestHelper.TestAtContractErInitieret(view);
        }

        /// <summary>
        /// Tester, at viewobjekt kan serialiseres.
        /// </summary>
        [Test]
        public void TestAtViewKanSerialiseres()
        {
            var fixture = new Fixture();
            fixture.Inject<IEnumerable<KalenderbrugerView>>(fixture.CreateMany<KalenderbrugerView>(3).ToList());
            var view = fixture.Create<KalenderbrugerAftaleView>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(view);
        }
    }
}
