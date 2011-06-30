using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Domain.Adressekartotek
{
    /// <summary>
    /// Tester hjælper til en adresseliste.
    /// </summary>
    [TestFixture]
    public class AdresselisteHelperTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer AdresselisteHelper.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererAdresselisteHelper()
        {
            var fixture = new Fixture();
            var adresser = fixture.CreateMany<Person>(3).ToList();
            var adresselisteHelper = new AdresselisteHelper(adresser);
            Assert.That(adresselisteHelper, Is.Not.Null);
            Assert.That(adresselisteHelper.Adresser, Is.Not.Null);
            Assert.That(adresselisteHelper.Adresser.Count(), Is.EqualTo(adresser.Count()));
        }

        /// <summary>
        /// Tester, at GetById henter og returnerer en given adresse.
        /// </summary>
        [Test]
        public void TestAtGetByIdHenterAdresseBase()
        {
            var fixture = new Fixture();
            var adresser = fixture.CreateMany<Person>(3).ToList();
            var adresselisteHelper = new AdresselisteHelper(adresser);
            Assert.That(adresselisteHelper, Is.Not.Null);

            var adresseBase = adresselisteHelper.GetById(adresser.ElementAt(1).Nummer);
            Assert.That(adresseBase, Is.Not.Null);
            Assert.That(adresseBase.Nummer, Is.EqualTo(adresser.ElementAt(1).Nummer));
            Assert.That(adresseBase.Navn, Is.Not.Null);
            Assert.That(adresseBase.Navn, Is.EqualTo(adresser.ElementAt(1).Navn));
        }

        /// <summary>
        /// Tester, at GetById kaster en IntranetRepositoryException, hvis adressen ikke findes.
        /// </summary>
        [Test]
        public void TestAtGetByIdKasterIntranetRepositoryExceptionHvisAdresseBaseIkkeFindes()
        {
            var fixture = new Fixture();
            var adresser = fixture.CreateMany<Person>(3).ToList();
            var adresselisteHelper = new AdresselisteHelper(adresser);
            Assert.That(adresselisteHelper, Is.Not.Null);

            Assert.Throws<IntranetRepositoryException>(() => adresselisteHelper.GetById(-1));
        }
    }
}
