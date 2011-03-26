using System;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.Repositories;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories
{
    /// <summary>
    /// Test af repository til adressekartoteket.
    /// </summary>
    [TestFixture]
    public class AdresseRepositoryTests
    {
        /// <summary>
        /// Tester, at AdresseGetAll henter adresser.
        /// </summary>
        [Test]
        public void TestAtAdresseGetAllHenterAdresser()
        {
            var repository = new AdresseRepository();
            var adresser = repository.AdresseGetAll();
            Assert.That(adresser, Is.Not.Null);
            Assert.That(adresser.OfType<Firma>().Count(), Is.EqualTo(24));
            Assert.That(adresser.OfType<Person>().Count(), Is.EqualTo(113));
        }

        /// <summary>
        /// Tester, at en person mappes korrekt.
        /// </summary>
        [Test]
        public void TestAtPersonMappesKorrekt()
        {
            var repository = new AdresseRepository();
            var adresser = repository.AdresseGetAll();
            Assert.That(adresser, Is.Not.Null);
            Assert.That(adresser.OfType<Person>().Count(), Is.GreaterThan(0));

            var person = adresser.OfType<Person>().SingleOrDefault(m => m.Nummer == 1);
            Assert.That(person, Is.Not.Null);
            Assert.That(person.Nummer, Is.EqualTo(1));
            Assert.That(person.Navn, Is.Not.Null);
            Assert.That(person.Navn, Is.EqualTo("Ole Sørensen"));
            Assert.That(person.Adresse1, Is.Not.Null);
            Assert.That(person.Adresse1, Is.EqualTo("Eggertsvænge 2"));
            Assert.That(person.Adresse2, Is.Null);
            Assert.That(person.PostnrBy, Is.Not.Null);
            Assert.That(person.PostnrBy, Is.EqualTo("5700  Svendborg"));
            Assert.That(person.Telefon, Is.Not.Null);
            Assert.That(person.Telefon, Is.EqualTo("62 21 49 60"));
            Assert.That(person.Mobil, Is.Not.Null);
            Assert.That(person.Mobil, Is.EqualTo("25 24 49 75"));
            Assert.That(person.Fødselsdato, Is.Not.Null);
            Assert.That(person.Fødselsdato, Is.EqualTo(new DateTime(1975, 8, 21, 0, 0, 0).Date).Within(0).Days);
            Assert.That(person.Adressegruppe, Is.Not.Null);
            Assert.That(person.Adressegruppe.Nummer, Is.EqualTo(1));
            Assert.That(person.Adressegruppe.Navn, Is.Not.Null);
            Assert.That(person.Adressegruppe.Navn, Is.EqualTo("Familie (Ole)"));
            Assert.That(person.Adressegruppe.AdressegruppeOswebdb, Is.EqualTo(1));
            Assert.That(person.Bekendtskab, Is.Null);
            Assert.That(person.Mailadresse, Is.Not.Null);
            Assert.That(person.Mailadresse, Is.EqualTo("os@dsidata.dk"));
            Assert.That(person.Webadresse, Is.Not.Null);
            Assert.That(person.Webadresse, Is.EqualTo("www.MrGottham.dk"));
            Assert.That(person.Betalingsbetingelse, Is.Not.Null);
            Assert.That(person.Betalingsbetingelse.Nummer, Is.EqualTo(1));
            Assert.That(person.Betalingsbetingelse.Navn, Is.Not.Null);
            Assert.That(person.Betalingsbetingelse.Navn, Is.EqualTo("Kontant"));
            Assert.That(person.Udlånsfrist, Is.EqualTo(14));
            Assert.That(person.Firma, Is.Not.Null);
            Assert.That(person.Firma.Nummer, Is.EqualTo(48));
            Assert.That(person.Firma.Navn, Is.Not.Null);
            Assert.That(person.Firma.Navn, Is.EqualTo("DSI DATA A/S"));
            Assert.That(person.FilofaxAdresselabel, Is.True);
        }

        /// <summary>
        /// Tester, at et firma mappes korrekt.
        /// </summary>
        [Test]
        public void TestAtFirmaMappesKorrekt()
        {
            var repository = new AdresseRepository();
            var adresser = repository.AdresseGetAll();
            Assert.That(adresser, Is.Not.Null);
            Assert.That(adresser.OfType<Firma>().Count(), Is.GreaterThan(0));

            var firma = adresser.OfType<Firma>().SingleOrDefault(m => m.Nummer == 48);
            Assert.That(firma, Is.Not.Null);
            Assert.That(firma.Nummer, Is.EqualTo(48));
            Assert.That(firma.Navn, Is.Not.Null);
            Assert.That(firma.Navn, Is.EqualTo("DSI DATA A/S"));
            Assert.That(firma.Adresse1, Is.Not.Null);
            Assert.That(firma.Adresse1, Is.EqualTo("Kokkedal Industripark 2"));
            Assert.That(firma.Adresse2, Is.Null);
            Assert.That(firma.PostnrBy, Is.Not.Null);
            Assert.That(firma.PostnrBy, Is.EqualTo("2980  Kokkedal"));
            Assert.That(firma.Telefon1, Is.Not.Null);
            Assert.That(firma.Telefon1, Is.EqualTo("49 18 49 18"));
            Assert.That(firma.Telefon2, Is.Null);
            Assert.That(firma.Telefax, Is.Not.Null);
            Assert.That(firma.Telefax, Is.EqualTo("49 18 49 44"));
            Assert.That(firma.Adressegruppe, Is.Not.Null);
            Assert.That(firma.Adressegruppe.Nummer, Is.EqualTo(3));
            Assert.That(firma.Adressegruppe.Navn, Is.Not.Null);
            Assert.That(firma.Adressegruppe.Navn, Is.EqualTo("Arbejdsrelationer"));
            Assert.That(firma.Adressegruppe.AdressegruppeOswebdb, Is.EqualTo(3));
            Assert.That(firma.Bekendtskab, Is.Not.Null);
            Assert.That(firma.Bekendtskab, Is.EqualTo("Arbejdsplads, Ole"));
            Assert.That(firma.Mailadresse, Is.Not.Null);
            Assert.That(firma.Mailadresse, Is.EqualTo("info@dsidata.dk"));
            Assert.That(firma.Webadresse, Is.Not.Null);
            Assert.That(firma.Webadresse, Is.EqualTo("www.dsidata.dk"));
            Assert.That(firma.Betalingsbetingelse, Is.Not.Null);
            Assert.That(firma.Betalingsbetingelse.Nummer, Is.EqualTo(1));
            Assert.That(firma.Betalingsbetingelse.Navn, Is.Not.Null);
            Assert.That(firma.Betalingsbetingelse.Navn, Is.EqualTo("Kontant"));
            Assert.That(firma.Udlånsfrist, Is.EqualTo(14));
            Assert.That(firma.FilofaxAdresselabel, Is.True);
            Assert.That(firma.Personer, Is.Not.Null);
            Assert.That(firma.Personer.Count, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at PostnummerGetAll henter postnumre.
        /// </summary>
        [Test]
        public void TestAtPostnummerGetAllHenterPostnumre()
        {
            var repository = new AdresseRepository();
            var postnumre = repository.PostnummerGetAll();
            Assert.That(postnumre, Is.Not.Null);
            Assert.That(postnumre.Count, Is.EqualTo(1324));
        }

        /// <summary>
        /// Tester, at postnumre mappes korrekt.
        /// </summary>
        [Test]
        public void TestAtPostnummerMappesKorrekt()
        {
            var repository = new AdresseRepository();
            var postnumre = repository.PostnummerGetAll();
            Assert.That(postnumre, Is.Not.Null);
            Assert.That(postnumre.Count, Is.GreaterThan(0));
            var postnummer =
                postnumre.SingleOrDefault(m => m.Landekode.CompareTo("DK") == 0 && m.Postnr.CompareTo("5700") == 0);
            Assert.That(postnummer, Is.Not.Null);
            Assert.That(postnummer.Landekode, Is.Not.Null);
            Assert.That(postnummer.Landekode, Is.EqualTo("DK"));
            Assert.That(postnummer.Postnr, Is.Not.Null);
            Assert.That(postnummer.Postnr, Is.EqualTo("5700"));
            Assert.That(postnummer.By, Is.Not.Null);
            Assert.That(postnummer.By, Is.EqualTo("Svendborg"));
        }

        /// <summary>
        /// Tester, at AdressegruppeGetAll henter alle adressegrupper.
        /// </summary>
        [Test]
        public void TestAtAdressegruppeGetAllHenterAdressegrupper()
        {
            var repository = new AdresseRepository();
            var adressegrupper = repository.AdressegruppeGetAll();
            Assert.That(adressegrupper, Is.Not.Null);
            Assert.That(adressegrupper.Count, Is.EqualTo(9));
            Assert.That(adressegrupper[0], Is.Not.Null);
            Assert.That(adressegrupper[0].Nummer, Is.EqualTo(1));
            Assert.That(adressegrupper[0].Navn, Is.Not.Null);
            Assert.That(adressegrupper[0].Navn, Is.EqualTo("Familie (Ole)"));
            Assert.That(adressegrupper[0].AdressegruppeOswebdb, Is.EqualTo(1));
            Assert.That(adressegrupper[1], Is.Not.Null);
            Assert.That(adressegrupper[1].Nummer, Is.EqualTo(2));
            Assert.That(adressegrupper[1].Navn, Is.Not.Null);
            Assert.That(adressegrupper[1].Navn, Is.EqualTo("Venner og veninder"));
            Assert.That(adressegrupper[1].AdressegruppeOswebdb, Is.EqualTo(2));
            Assert.That(adressegrupper[2], Is.Not.Null);
            Assert.That(adressegrupper[2].Nummer, Is.EqualTo(3));
            Assert.That(adressegrupper[2].Navn, Is.Not.Null);
            Assert.That(adressegrupper[2].Navn, Is.EqualTo("Arbejdsrelationer"));
            Assert.That(adressegrupper[2].AdressegruppeOswebdb, Is.EqualTo(3));
            Assert.That(adressegrupper[3], Is.Not.Null);
            Assert.That(adressegrupper[3].Nummer, Is.EqualTo(4));
            Assert.That(adressegrupper[3].Navn, Is.Not.Null);
            Assert.That(adressegrupper[3].Navn, Is.EqualTo("Øvrige relationer"));
            Assert.That(adressegrupper[3].AdressegruppeOswebdb, Is.EqualTo(5));
            Assert.That(adressegrupper[4], Is.Not.Null);
            Assert.That(adressegrupper[4].Nummer, Is.EqualTo(5));
            Assert.That(adressegrupper[4].Navn, Is.Not.Null);
            Assert.That(adressegrupper[4].Navn, Is.EqualTo("Familie (Merete)"));
            Assert.That(adressegrupper[4].AdressegruppeOswebdb, Is.EqualTo(6));
            Assert.That(adressegrupper[5], Is.Not.Null);
            Assert.That(adressegrupper[5].Nummer, Is.EqualTo(6));
            Assert.That(adressegrupper[5].Navn, Is.Not.Null);
            Assert.That(adressegrupper[5].Navn, Is.EqualTo("Den Danske Billard Union"));
            Assert.That(adressegrupper[5].AdressegruppeOswebdb, Is.EqualTo(4));
            Assert.That(adressegrupper[6], Is.Not.Null);
            Assert.That(adressegrupper[6].Nummer, Is.EqualTo(7));
            Assert.That(adressegrupper[6].Navn, Is.Not.Null);
            Assert.That(adressegrupper[6].Navn, Is.EqualTo("Sydfyns Billard Klub"));
            Assert.That(adressegrupper[6].AdressegruppeOswebdb, Is.EqualTo(4));
            Assert.That(adressegrupper[7], Is.Not.Null);
            Assert.That(adressegrupper[7].Nummer, Is.EqualTo(8));
            Assert.That(adressegrupper[7].Navn, Is.Not.Null);
            Assert.That(adressegrupper[7].Navn, Is.EqualTo("Billard Union Fyn"));
            Assert.That(adressegrupper[7].AdressegruppeOswebdb, Is.EqualTo(4));
            Assert.That(adressegrupper[8], Is.Not.Null);
            Assert.That(adressegrupper[8].Nummer, Is.EqualTo(9));
            Assert.That(adressegrupper[8].Navn, Is.Not.Null);
            Assert.That(adressegrupper[8].Navn, Is.EqualTo("Billardspillere"));
            Assert.That(adressegrupper[8].AdressegruppeOswebdb, Is.EqualTo(4));
        }

        /// <summary>
        /// Tester, at BetalingsbetingelseGetAllHenterBetalingsbetingelser.
        /// </summary>
        [Test]
        public void TestAtBetalingsbetingelseGetAllHenterBetalingsbetingelser()
        {
            var repository = new AdresseRepository();
            var betalingsbetingelser = repository.BetalingsbetingelseGetAll();
            Assert.That(betalingsbetingelser, Is.Not.Null);
            Assert.That(betalingsbetingelser.Count, Is.EqualTo(2));
            Assert.That(betalingsbetingelser[0], Is.Not.Null);
            Assert.That(betalingsbetingelser[0].Nummer, Is.EqualTo(1));
            Assert.That(betalingsbetingelser[0].Navn, Is.Not.Null);
            Assert.That(betalingsbetingelser[0].Navn, Is.EqualTo("Kontant"));
            Assert.That(betalingsbetingelser[1], Is.Not.Null);
            Assert.That(betalingsbetingelser[1].Nummer, Is.EqualTo(2));
            Assert.That(betalingsbetingelser[1].Navn, Is.Not.Null);
            Assert.That(betalingsbetingelser[1].Navn, Is.EqualTo("Netto + 8 dage"));
        }
    }
}
