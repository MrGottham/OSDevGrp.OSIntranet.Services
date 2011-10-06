using System;
using System.Data;
using OSDevGrp.OSIntranet.Domain.Kalender;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.DataProxies.Kalender;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.DataProxies.Kalender
{
    /// <summary>
    /// Tester data proxy for en kalenderbruger under OSWEBDB.
    /// </summary>
    [TestFixture]
    public class BrugerProxyTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en data proxy for en bruger.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererBrugerProxy()
        {
            var fixture = new Fixture();
            fixture.Inject(new BrugerProxy());

            var brugerProxy = fixture.CreateAnonymous<BrugerProxy>();
            Assert.That(brugerProxy, Is.Not.Null);
            Assert.That(brugerProxy.System, Is.Not.Null);
            Assert.That(brugerProxy.System.Nummer, Is.EqualTo(0));
            Assert.That(brugerProxy.Id, Is.EqualTo(0));
            Assert.That(brugerProxy.Initialer, Is.Not.Null);
            Assert.That(brugerProxy.Initialer, Is.EqualTo(typeof(Bruger).ToString()));
            Assert.That(brugerProxy.Navn, Is.Not.Null);
            Assert.That(brugerProxy.Navn, Is.EqualTo(typeof(Bruger).ToString()));
            Assert.That(brugerProxy.DataIsLoaded, Is.False);
        }

        /// <summary>
        /// Tester, at UniqueId returnerer den unikke idenfikation for brugeren.
        /// </summary>
        [Test]
        public void TestAtUniqueIdReturnererIdentifikation()
        {
            var fixture = new Fixture();
            fixture.Inject(new BrugerProxy());

            var brugerProxy = fixture.CreateAnonymous<BrugerProxy>();
            Assert.That(brugerProxy, Is.Not.Null);

            var uniqueId = brugerProxy.UniqueId;
            Assert.That(uniqueId, Is.Not.Null);
            Assert.That(uniqueId, Is.EqualTo("0-0"));
        }
    }
}
