using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Domain.Adressekartotek
{
    /// <summary>
    /// Tester hjælper til en liste af adressegrupper.
    /// </summary>
    [TestFixture]
    public class AdressegruppelisteHelperTests
    {
        /// <summary>
        /// Tester, at GetById henter og returnerer en given adressegruppe.
        /// </summary>
        [Test]
        public void TestAtGetByIdHenterAdressegruppe()
        {
            var fixture = new Fixture();
            var adressegrupper = fixture.CreateMany<Adressegruppe>(3).ToList();
            var adressegruppelisteHelper = new AdressegruppelisteHelper(adressegrupper);
            Assert.That(adressegruppelisteHelper, Is.Not.Null);

            var adressegruppe = adressegruppelisteHelper.GetById(adressegrupper.ElementAt(1).Nummer);
            Assert.That(adressegruppe, Is.Not.Null);
            Assert.That(adressegruppe.Nummer, Is.EqualTo(adressegrupper.ElementAt(1).Nummer));
            Assert.That(adressegruppe.Navn, Is.Not.Null);
            Assert.That(adressegruppe.Navn, Is.EqualTo(adressegrupper.ElementAt(1).Navn));
        }

        /// <summary>
        /// Tester, at GetById kaster en IntranetRepositoryException, hvis adressegruppen ikke findes.
        /// </summary>
        [Test]
        public void TestAtGetByIdKasterIntranetRepositoryExceptionHvisAdressegruppeIkkeFindes()
        {
            var fixture = new Fixture();
            var adressegrupper = fixture.CreateMany<Adressegruppe>(3).ToList();
            var adressegruppelisteHelper = new AdressegruppelisteHelper(adressegrupper);
            Assert.That(adressegruppelisteHelper, Is.Not.Null);

            Assert.Throws<IntranetRepositoryException>(() => adressegruppelisteHelper.GetById(-1));
        }
    }
}
