﻿using OSDevGrp.OSIntranet.Contracts.Views;
using NUnit.Framework;
using AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Views
{
    /// <summary>
    /// Tester viewobject for et brevhoved.
    /// </summary>
    [TestFixture]
    public class BrevhovedViewTests
    {
        /// <summary>
        /// Tester, at viewobjekt kan initieres.
        /// </summary>
        [Test]
        public void TestAtViewKanInitieres()
        {
            var fixture = new Fixture();
            var view = fixture.Create<BrevhovedView>();
            DataContractTestHelper.TestAtContractErInitieret(view);
        }

        /// <summary>
        /// Tester, at viewobjekt kan serialiseres.
        /// </summary>
        [Test]
        public void TestAtViewKanSerialiseres()
        {
            var fixture = new Fixture();
            var view = fixture.Create<BrevhovedView>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(view);
        }
    }
}
