using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.Domain.Finansstyring;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Domain.Finansstyring
{
    /// <summary>
    /// Tester hjælper til en liste af kontogrupper.
    /// </summary>
    [TestFixture]
    public class KontogruppelisteHelperTests
    {
        /// <summary>
        /// Tester, at GetById henter og returnerer en given kontogruppe.
        /// </summary>
        [Test]
        public void TestAtGetByIdHenterKontogruppe()
        {
            var fixture = new Fixture();
            var kontogrupper = fixture.CreateMany<Kontogruppe>(3).ToList();
            var kontogruppelisteHelper = new KontogruppelisteHelper(kontogrupper);
            Assert.That(kontogruppelisteHelper, Is.Not.Null);

            var kontogruppe = kontogruppelisteHelper.GetById(kontogrupper.ElementAt(1).Nummer);
            Assert.That(kontogruppe, Is.Not.Null);
            Assert.That(kontogruppe.Nummer, Is.EqualTo(kontogrupper.ElementAt(1).Nummer));
            Assert.That(kontogruppe.Navn, Is.Not.Null);
            Assert.That(kontogruppe.Navn, Is.EqualTo(kontogrupper.ElementAt(1).Navn));
        }

        /// <summary>
        /// Tester, at GetById kaster en IntranetRepositoryException, hvis kontogruppen ikke findes.
        /// </summary>
        [Test]
        public void TestAtGetByIdKasterIntranetRepositoryExceptionHvisKontogruppenIkkeFindes()
        {
            var fixture = new Fixture();
            var kontogrupper = fixture.CreateMany<Kontogruppe>(3).ToList();
            var kontogruppelisteHelper = new KontogruppelisteHelper(kontogrupper);
            Assert.That(kontogruppelisteHelper, Is.Not.Null);

            Assert.Throws<IntranetRepositoryException>(() => kontogruppelisteHelper.GetById(-1));
        }
    }
}
