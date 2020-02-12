using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.IoC;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Services;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Integrationstests.Services.Implementations
{
    /// <summary>
    /// Integrationstester servicen til adressekartotek.
    /// </summary>
    [TestFixture]
    [Category("Integrationstest")]
    public class AdressekartotekServiceTests
    {
        #region Private variables

        private IAdressekartotekService _service;

        #endregion

        /// <summary>
        /// Opsætning af test.
        /// </summary>
        [SetUp]
        public void TestSetUp()
        {
            var container = ContainerFactory.Create();
            _service = container.Resolve<IAdressekartotekService>();
        }

        /// <summary>
        /// Tester, at telefonliste hentes.
        /// </summary>
        [Test]
        public void TestAtTelefonlisteHentes()
        {
            var query = new TelefonlisteGetQuery();
            var result = _service.TelefonlisteGet(query);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at personliste hentes.
        /// </summary>
        [Test]
        public void TestAtPersonlisteHentes()
        {
            var query = new PersonlisteGetQuery();
            var result = _service.PersonlisteGet(query);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at en person hentes.
        /// </summary>
        [Test]
        public void TestAtPersonHentes()
        {
            var query = new PersonGetQuery
                            {
                                Nummer = 1
                            };
            var result = _service.PersonGet(query);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Nummer, Is.EqualTo(query.Nummer));
        }

        /// <summary>
        /// Tester, at firmaliste hentes.
        /// </summary>
        [Test]
        public void TestAtFirmalisteHentes()
        {
            var query = new FirmalisteGetQuery();
            var result = _service.FirmalisteGet(query);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at et firma hentes.
        /// </summary>
        [Test]
        public void TestAtFirmaHentes()
        {
            var query = new FirmaGetQuery
                            {
                                Nummer = 48
                            };
            var result = _service.FirmaGet(query);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Nummer, Is.EqualTo(query.Nummer));
        }
    }
}
