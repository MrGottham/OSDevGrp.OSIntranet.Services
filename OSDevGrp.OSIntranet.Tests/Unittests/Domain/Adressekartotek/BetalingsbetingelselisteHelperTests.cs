using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Domain.Adressekartotek
{
    /// <summary>
    /// Tester hjælper til en liste af betalingsbetingelser.
    /// </summary>
    [TestFixture]
    public class BetalingsbetingelselisteHelperTests
    {
        /// <summary>
        /// Tester, at GetById henter og returnerer en given betalingsbetingelse.
        /// </summary>
        [Test]
        public void TestAtGetByIdHenterBetalingsbetingelse()
        {
            var fixture = new Fixture();
            var betalingsbetingelser = fixture.CreateMany<Betalingsbetingelse>(3).ToList();
            var betalingsbetingelselisteHelper = new BetalingsbetingelselisteHelper(betalingsbetingelser);
            Assert.That(betalingsbetingelselisteHelper, Is.Not.Null);

            var betalingsbetingelse = betalingsbetingelselisteHelper.GetById(betalingsbetingelser.ElementAt(1).Nummer);
            Assert.That(betalingsbetingelse, Is.Not.Null);
            Assert.That(betalingsbetingelse.Nummer, Is.EqualTo(betalingsbetingelser.ElementAt(1).Nummer));
            Assert.That(betalingsbetingelse.Navn, Is.Not.Null);
            Assert.That(betalingsbetingelse.Navn, Is.EqualTo(betalingsbetingelser.ElementAt(1).Navn));
        }

        /// <summary>
        /// Tester, at GetById kaster en IntranetRepositoryException, hvis betalingsbetingelsen ikke findes.
        /// </summary>
        [Test]
        public void TestAtGetByIdKasterIntranetRepositoryExceptionHvisBetalingsbetingelseIkkeFindes()
        {
            var fixture = new Fixture();
            var betalingsbetingelser = fixture.CreateMany<Betalingsbetingelse>(3).ToList();
            var betalingsbetingelselisteHelper = new BetalingsbetingelselisteHelper(betalingsbetingelser);
            Assert.That(betalingsbetingelselisteHelper, Is.Not.Null);

            Assert.Throws<IntranetRepositoryException>(() => betalingsbetingelselisteHelper.GetById(-1));
        }
    }
}
