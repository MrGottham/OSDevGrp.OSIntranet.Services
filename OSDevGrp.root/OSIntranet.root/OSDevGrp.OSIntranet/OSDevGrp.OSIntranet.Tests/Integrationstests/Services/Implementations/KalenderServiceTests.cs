using System;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.IoC;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Services;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Integrationstests.Services.Implementations
{
    /// <summary>
    /// Integrationstester servicen til kalenderaftaler.
    /// </summary>
    [TestFixture]
    [Category("Integrationstest")]
    public class KalenderServiceTests
    {
        #region Private variables

        private IKalenderService _service;

        #endregion

        /// <summary>
        /// Opsætning af test.
        /// </summary>
        [TestFixtureSetUp]
        public void TestSetUp()
        {
            var container = ContainerFactory.Create();
            _service = container.Resolve<IKalenderService>();
        }

        /// <summary>
        /// Tester, at kalenderaftaler til en given kalenderbruger hentes.
        /// </summary>
        [Test]
        public void TestAtKalenderaftalerHentes()
        {
            var query = new KalenderbrugerAftalerGetQuery
                            {
                                System = 1,
                                Initialer = "OS",
                                FraDato = new DateTime(2010, 1, 1)
                            };
            var result = _service.KalenderbrugerAftalerGet(query);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at en given kalenderaftale til en given kalenderbruger hentes.
        /// </summary>
        [Test]
        public void TestAtKalenderaftaleHentes()
        {
            var query = new KalenderbrugerAftaleGetQuery
                            {
                                System = 1,
                                AftaleId = 1,
                                Initialer = "OS"
                            };
            var result = _service.KalenderbrugerAftaleGet(query);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(query.AftaleId));
        }

        /// <summary>
        /// Tester, at kalenderbrugere til et system under OSWEBDB hentes.
        /// </summary>
        [Test]
        public void TestAtKalenderbrugereHentes()
        {
            var query = new KalenderbrugereGetQuery
                            {
                                System = 1
                            };
            var result = _service.KalenderbrugereGet(query);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at systemer under OSWEBDB hentes.
        /// </summary>
        [Test]
        public void TestAtSystemerHentes()
        {
            var query = new SystemerGetQuery();
            var result = _service.SystemerGet(query);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.GreaterThan(0));
        }
    }
}
