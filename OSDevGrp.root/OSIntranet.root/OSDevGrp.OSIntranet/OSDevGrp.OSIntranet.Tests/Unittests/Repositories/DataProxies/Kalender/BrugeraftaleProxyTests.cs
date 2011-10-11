using System;
using System.Data;
using OSDevGrp.OSIntranet.Domain.Kalender;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.DataProxies.Fælles;
using OSDevGrp.OSIntranet.Repositories.DataProxies.Kalender;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.DataProxies.Kalender
{
    /// <summary>
    /// Tester data proxy for en brugers kalenderaftale.
    /// </summary>
    [TestFixture]
    public class BrugeraftaleProxyTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en data proxy for en brugeraftale.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererBrugeraftaleProxy()
        {
            var fixture = new Fixture();
            fixture.Inject(new BrugeraftaleProxy());

            var brugeraftaleProxy = fixture.CreateAnonymous<BrugeraftaleProxy>();
            Assert.That(brugeraftaleProxy, Is.Not.Null);
            Assert.That(brugeraftaleProxy.System, Is.Not.Null);
            Assert.That(brugeraftaleProxy.System.Nummer, Is.EqualTo(0));
            Assert.That(brugeraftaleProxy.Aftale, Is.Not.Null);
            Assert.That(brugeraftaleProxy.Aftale.Id, Is.EqualTo(0));
            Assert.That(brugeraftaleProxy.Bruger, Is.Not.Null);
            Assert.That(brugeraftaleProxy.Bruger.Id, Is.EqualTo(0));
            Assert.That(brugeraftaleProxy.DataIsLoaded, Is.False);
        }
    }
}
