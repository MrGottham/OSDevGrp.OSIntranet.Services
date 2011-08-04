﻿using OSDevGrp.OSIntranet.Contracts.Views;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Views
{
    /// <summary>
    /// Tester viewobject for en budgetkontoplan.
    /// </summary>
    [TestFixture]
    public class BudgetkontoplanViewTests
    {
        /// <summary>
        /// Tester, at viewobjekt kan initieres.
        /// </summary>
        [Test]
        public void TestAtViewKanInitieres()
        {
            var fixture = new Fixture();
            var view = fixture.CreateAnonymous<BudgetkontoplanView>();
            DataContractTestHelper.TestAtContractErInitieret(view);
        }

        /// <summary>
        /// Tester, at viewobjekt kan serialiseres.
        /// </summary>
        [Test]
        public void TestAtViewKanSerialiseres()
        {
            var fixture = new Fixture();
            var view = fixture.CreateAnonymous<BudgetkontoplanView>();
            DataContractTestHelper.TestAtContractKanSerialiseresOgDeserialiseres(view);
        }
    }
}
