using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.Contracts.Responses;
using OSDevGrp.OSIntranet.Contracts.Views;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Responses
{
    /// <summary>
    /// Tester datakontrakt for en bogføringsadvarsel.
    /// </summary>
    [TestFixture]
    public class BogføringsadvarselResponseTests
    {
        /// <summary>
        /// Tester, at Response kan initieres.
        /// </summary>
        [Test]
        public void TestAtResponseKanInitieres()
        {
            var fixture = new Fixture();
            fixture.Inject<IEnumerable<KreditoplysningerView>>(fixture.CreateMany<KreditoplysningerView>(24).ToList());
            fixture.Inject<KontoBaseView>(fixture.Create<KontoView>());
            var response = fixture.Create<BogføringsadvarselResponse>();
            DataContractTestHelper.TestAtContractErInitieret(response);
        }

        /// <summary>
        /// Tester, at Response kan serialiseres.
        /// </summary>
        [Test]
        public void TestAtResponseKanSerialiseres()
        {
            var fixture = new Fixture();
            fixture.Inject<IEnumerable<KreditoplysningerView>>(fixture.CreateMany<KreditoplysningerView>(24).ToList());
            fixture.Inject<KontoBaseView>(fixture.Create<KontoView>());
            var response = fixture.Create<BogføringsadvarselResponse>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(response);
        }
    }
}
