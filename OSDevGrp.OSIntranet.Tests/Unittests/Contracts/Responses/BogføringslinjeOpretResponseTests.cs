﻿using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.Contracts.Responses;
using OSDevGrp.OSIntranet.Contracts.Views;
using NUnit.Framework;
using AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Responses
{
    /// <summary>
    /// Tester datakontrakt til svar fra oprettelse af bogføringslinje.
    /// </summary>
    [TestFixture]
    public class BogføringslinjeOpretResponseTests
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
            fixture.Inject<IEnumerable<BogføringsadvarselResponse>>(fixture.CreateMany<BogføringsadvarselResponse>(3).ToList());
            var response = fixture.Create<BogføringslinjeOpretResponse>();
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
            fixture.Inject<IEnumerable<BogføringsadvarselResponse>>(fixture.CreateMany<BogføringsadvarselResponse>(3).ToList());
            var response = fixture.Create<BogføringslinjeOpretResponse>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(response);
        }
    }
}
