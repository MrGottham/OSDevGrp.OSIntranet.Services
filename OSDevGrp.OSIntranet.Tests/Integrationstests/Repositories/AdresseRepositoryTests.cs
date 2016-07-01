using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.IoC;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Integrationstests.Repositories
{
    /// <summary>
    /// Integrationstester repository for adressekartotek.
    /// </summary>
    [TestFixture]
    [Category("Integrationstest")]
    public class AdresseRepositoryTests
    {
        #region Private variables

        private IAdresseRepository _adresseRepository;

        #endregion

        /// <summary>
        /// Opsætning af tests.
        /// </summary>
        [SetUp]
        public void TestSetUp()
        {
            var container = ContainerFactory.Create();
            _adresseRepository = container.Resolve<IAdresseRepository>();
        }

        /// <summary>
        /// Tester, at AdresseGetAll henter adresser.
        /// </summary>
        [Test]
        public void TestAtAdresseGetAllHenterAdresser()
        {
            var adresser = _adresseRepository.AdresseGetAll();
            Assert.That(adresser, Is.Not.Null);
            Assert.That(adresser.Count(), Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at PostnummerGetAll henter postnumre.
        /// </summary>
        [Test]
        public void TestAtPostnummerGetAllHenterPostnumre()
        {
            var postnumre = _adresseRepository.PostnummerGetAll();
            Assert.That(postnumre, Is.Not.Null);
            Assert.That(postnumre.Count(), Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at AdressegruppeGetAll henter adressegrupper.
        /// </summary>
        [Test]
        public void TestAtAdressegruppeGetAllHenterAdressegrupper()
        {
            var adressegrupper = _adresseRepository.AdressegruppeGetAll();
            Assert.That(adressegrupper, Is.Not.Null);
            Assert.That(adressegrupper.Count(), Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at BetalingsbetingelseGetAll henter betalingsbetingelser.
        /// </summary>
        [Test]
        public void TestAtBetalingsbetingelseGetAllHenterBetalingsbetingelser()
        {
            var betalingsbetingelser = _adresseRepository.BetalingsbetingelseGetAll();
            Assert.That(betalingsbetingelser, Is.Not.Null);
            Assert.That(betalingsbetingelser.Count(), Is.GreaterThan(0));
        }
    }
}
