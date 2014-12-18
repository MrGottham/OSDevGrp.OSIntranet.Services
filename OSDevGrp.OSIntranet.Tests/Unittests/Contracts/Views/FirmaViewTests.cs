using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.Contracts.Views;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Views
{
    /// <summary>
    /// Tester viewobject for et firma.
    /// </summary>
    [TestFixture]
    public class FirmaViewTests
    {
        /// <summary>
        /// Tester, at viewobjekt kan initieres.
        /// </summary>
        [Test]
        public void TestAtViewKanInitieres()
        {
            var fixture = new Fixture();
            fixture.Inject<IEnumerable<TelefonlisteView>>(fixture.CreateMany<TelefonlisteView>(5).ToList());
            var view = fixture.Create<FirmaView>();
            DataContractTestHelper.TestAtContractErInitieret(view);
        }

        /// <summary>
        /// Tester, at viewobjekt kan serialiseres.
        /// </summary>
        [Test]
        public void TestAtViewKanSerialiseres()
        {
            var fixture = new Fixture();
            fixture.Inject<IEnumerable<TelefonlisteView>>(fixture.CreateMany<TelefonlisteView>(5).ToList());
            var view = fixture.Create<FirmaView>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(view);
        }
    }
}
