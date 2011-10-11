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
    /// Tester data proxy for en kalenderaftale.
    /// </summary>
    [TestFixture]
    public class AftaleProxyTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en data proxy for en aftale.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererAftaleProxy()
        {
            var fixture = new Fixture();
            fixture.Inject(new AftaleProxy());

            var aftaleProxy = fixture.CreateAnonymous<AftaleProxy>();
            Assert.That(aftaleProxy, Is.Not.Null);
            Assert.That(aftaleProxy.System, Is.Not.Null);
            Assert.That(aftaleProxy.System.Nummer, Is.EqualTo(0));
            Assert.That(aftaleProxy.Id, Is.EqualTo(0));
            Assert.That(aftaleProxy.FraTidspunkt, Is.EqualTo(DateTime.MinValue));
            Assert.That(aftaleProxy.TilTidspunkt, Is.EqualTo(DateTime.MaxValue));
            Assert.That(aftaleProxy.Emne, Is.Not.Null);
            Assert.That(aftaleProxy.Emne, Is.EqualTo(typeof(Aftale).ToString()));
            Assert.That(aftaleProxy.DataIsLoaded, Is.False);
        }
    }
}
