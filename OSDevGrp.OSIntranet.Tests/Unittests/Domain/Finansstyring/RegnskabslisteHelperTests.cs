using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.Domain.Finansstyring;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using NUnit.Framework;
using AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Domain.Finansstyring
{
    /// <summary>
    /// Tester hjælper til en regnskabsliste.
    /// </summary>
    [TestFixture]
    public class RegnskabslisteHelperTests
    {
        /// <summary>
        /// Tester, at GetById henter og returnerer et givent regnskab.
        /// </summary>
        [Test]
        public void TestAtGetByIdHenterRegnskab()
        {
            var fixture = new Fixture();
            var regnskaber = fixture.CreateMany<Regnskab>(3).ToList();
            var regnskabslisteHelper = new RegnskabslisteHelper(regnskaber);
            Assert.That(regnskabslisteHelper, Is.Not.Null);

            var regnskab = regnskabslisteHelper.GetById(regnskaber.ElementAt(1).Nummer);
            Assert.That(regnskab, Is.Not.Null);
            Assert.That(regnskab.Nummer, Is.EqualTo(regnskaber.ElementAt(1).Nummer));
            Assert.That(regnskab.Navn, Is.Not.Null);
            Assert.That(regnskab.Navn, Is.EqualTo(regnskaber.ElementAt(1).Navn));
        }

        /// <summary>
        /// Tester, at GetById kaster en IntranetRepositoryException, hvis regnskabet ikke findes.
        /// </summary>
        [Test]
        public void TestAtGetByIdKasterIntranetRepositoryExceptionHvisRegnskabIkkeFindes()
        {
            var fixture = new Fixture();
            var regnskaber = fixture.CreateMany<Regnskab>(3).ToList();
            var regnskabslisteHelper = new RegnskabslisteHelper(regnskaber);
            Assert.That(regnskabslisteHelper, Is.Not.Null);

            Assert.Throws<IntranetRepositoryException>(() => regnskabslisteHelper.GetById(-1));
        }
    }
}
