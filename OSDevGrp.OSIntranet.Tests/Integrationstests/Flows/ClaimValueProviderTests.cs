using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommonLibrary.IoC;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Tests.Integrationstests.Flows
{
    /// <summary>
    /// Integration tests the provider which can resolve values from the current users claims.
    /// </summary>
    [TestFixture]
    [Category("Integrationstest")]
    public class ClaimValueProviderTests
    {
        #region Private variables

        private IClaimValueProvider _claimValueProvider;

        #endregion

        /// <summary>
        /// Opsætning af tests.
        /// </summary>
        [TestFixtureSetUp]
        public void TestSetUp()
        {
            var container = ContainerFactory.Create();
            _claimValueProvider = container.Resolve<IClaimValueProvider>();
        }

        /// <summary>
        /// Tests that cliam values can be resolved.
        /// </summary>
        [Test]
        public void TestThatClaimValuesCanBeResolved()
        {
            var mailAddress = string.Format("test.{0}@osdevgrp.dk", Guid.NewGuid().ToString("D").ToLower());
            using (new ClaimsPrincipalTestExecutor(mailAddress))
            {
                Assert.That(_claimValueProvider.MailAddress, Is.Not.Null);
                Assert.That(_claimValueProvider.MailAddress, Is.Not.Empty);
                Assert.That(_claimValueProvider.MailAddress, Is.EqualTo(mailAddress));
            }
        }
    }
}
