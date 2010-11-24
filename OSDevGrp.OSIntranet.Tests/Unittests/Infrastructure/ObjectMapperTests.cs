using OSDevGrp.OSIntranet.CommonLibrary.Domain.Enums;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Infrastructure;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Infrastructure
{
    /// <summary>
    /// Tester ObjectMapper.
    /// </summary>
    [TestFixture]
    public class ObjectMapperTests
    {
        /// <summary>
        /// Tester, at ObjectMapper kan initieres.
        /// </summary>
        [Test]
        public void TestAtObjectMapperKanInitieres()
        {
            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);
        }

        /// <summary>
        /// Tester, at et regnskab kan mappes til et regnskabslisteview.
        /// </summary>
        [Test]
        public void TestAtRegnskabKanMappesTilRegnskabslisteView()
        {
            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var regnskab = new Regnskab(1, "Privatregnskab, Ole Sørensen");
            var regnskabslisteView = objectMapper.Map<Regnskab, RegnskabslisteView>(regnskab);
            Assert.That(regnskabslisteView, Is.Not.Null);
            Assert.That(regnskabslisteView.Nummer, Is.EqualTo(1));
            Assert.That(regnskabslisteView.Navn, Is.Not.Null);
            Assert.That(regnskabslisteView.Navn, Is.EqualTo("Privatregnskab, Ole Sørensen"));
        }

        /// <summary>
        /// Tester, at en kontogruppe kan mappes til et kontogruppeview.
        /// </summary>
        [Test]
        public void TestAtKontogruppeKanMappesTilKontogruppeView()
        {
            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var kontogruppeAktiver = new Kontogruppe(1, "Bankkonti", KontogruppeType.Aktiver);
            var kontogruppeAktiverView = objectMapper.Map<Kontogruppe, KontogruppeView>(kontogruppeAktiver);
            Assert.That(kontogruppeAktiverView, Is.Not.Null);
            Assert.That(kontogruppeAktiverView.Nummer, Is.EqualTo(1));
            Assert.That(kontogruppeAktiverView.Navn, Is.Not.Null);
            Assert.That(kontogruppeAktiverView.Navn, Is.EqualTo("Bankkonti"));
            Assert.That(kontogruppeAktiverView.ErAktiver, Is.True);
            Assert.That(kontogruppeAktiverView.ErPassiver, Is.False);

            var kontogruppePassiver = new Kontogruppe(2, "Kreditorer", KontogruppeType.Passiver);
            var kontogruppePassiverView = objectMapper.Map<Kontogruppe, KontogruppeView>(kontogruppePassiver);
            Assert.That(kontogruppePassiverView, Is.Not.Null);
            Assert.That(kontogruppePassiverView.Nummer, Is.EqualTo(2));
            Assert.That(kontogruppePassiverView.Navn, Is.Not.Null);
            Assert.That(kontogruppePassiverView.Navn, Is.EqualTo("Kreditorer"));
            Assert.That(kontogruppePassiverView.ErAktiver, Is.False);
            Assert.That(kontogruppePassiverView.ErPassiver, Is.True);
        }
    }
}
