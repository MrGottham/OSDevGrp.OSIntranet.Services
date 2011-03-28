using System;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Enums;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.Contracts.Views;
using AutoMapper;
using NUnit.Framework;
using ObjectMapper = OSDevGrp.OSIntranet.Infrastructure.ObjectMapper;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Infrastructure
{
    /// <summary>
    /// Tester ObjectMapper.
    /// </summary>
    [TestFixture]
    public class ObjectMapperTests
    {
        /// <summary>
        /// Adresseklasse, der kan benyttes til test.
        /// </summary>
        private class OtherAddress : AdresseBase
        {
            /// <summary>
            /// Konstruerer adresseklasse, der kan benyttes til test.
            /// </summary>
            /// <param name="nummer">Nummer på adressen.</param>
            /// <param name="navn">Navn på adressen.</param>
            /// <param name="adressegruppe">Adressegruppe.</param>
            public OtherAddress(int nummer, string navn, Adressegruppe adressegruppe)
                : base(nummer, navn, adressegruppe)
            {
            }
        }

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
        /// Tester, at en basisadresse kan mappes til et telefonlisteview.
        /// </summary>
        [Test]
        public void TestAtAdresseBaseMappesTilTelefonlisteView()
        {
            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var telefonlisteView = objectMapper.Map<AdresseBase, TelefonlisteView>(null);
            Assert.That(telefonlisteView, Is.Null);

            var adressegruppe = new Adressegruppe(1, "Adresser", 0);
            Assert.That(adressegruppe, Is.Not.Null);

            var person = new Person(1, "Ole Sørensen", adressegruppe);
            person.SætTelefon("62 21 49 60", "25 24 49 75");
            telefonlisteView = objectMapper.Map<AdresseBase, TelefonlisteView>(person);
            Assert.That(telefonlisteView, Is.Not.Null);
            Assert.That(telefonlisteView.Nummer, Is.EqualTo(1));
            Assert.That(telefonlisteView.Navn, Is.Not.Null);
            Assert.That(telefonlisteView.Navn, Is.EqualTo("Ole Sørensen"));
            Assert.That(telefonlisteView.PrimærTelefon, Is.Not.Null);
            Assert.That(telefonlisteView.PrimærTelefon, Is.EqualTo("62 21 49 60"));
            Assert.That(telefonlisteView.SekundærTelefon, Is.Not.Null);
            Assert.That(telefonlisteView.SekundærTelefon, Is.EqualTo("25 24 49 75"));

            var firma = new Firma(2, "OS Development Group", adressegruppe);
            firma.SætTelefon("62 21 49 60", "25 24 49 75", null);
            telefonlisteView = objectMapper.Map<AdresseBase, TelefonlisteView>(firma);
            Assert.That(telefonlisteView, Is.Not.Null);
            Assert.That(telefonlisteView.Nummer, Is.EqualTo(2));
            Assert.That(telefonlisteView.Navn, Is.Not.Null);
            Assert.That(telefonlisteView.Navn, Is.EqualTo("OS Development Group"));
            Assert.That(telefonlisteView.PrimærTelefon, Is.Not.Null);
            Assert.That(telefonlisteView.PrimærTelefon, Is.EqualTo("62 21 49 60"));
            Assert.That(telefonlisteView.SekundærTelefon, Is.Not.Null);
            Assert.That(telefonlisteView.SekundærTelefon, Is.EqualTo("25 24 49 75"));

            var andenAdresse = new OtherAddress(3, "Bente Susanne Rasmussen", adressegruppe);
            Assert.Throws<AutoMapperMappingException>(() => objectMapper.Map<AdresseBase, TelefonlisteView>(andenAdresse));
        }

        /// <summary>
        /// Tester, at en basisadresse kan mappes til et adressekontolisteview.
        /// </summary>
        [Test]
        public void TestAtAdresseBaseMappesTilAdressekontolisteView()
        {
            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var debitorlisteView = objectMapper.Map<AdresseBase, AdressekontolisteView>(null);
            Assert.That(debitorlisteView, Is.Null);

            var adressegruppe = new Adressegruppe(1, "Adresser", 0);
            Assert.That(adressegruppe, Is.Not.Null);

            var person = new Person(1, "Ole Sørensen", adressegruppe);
            person.SætTelefon("62 21 49 60", "25 24 49 75");
            person.TilføjBogføringslinje(new Bogføringslinje(1, new DateTime(2010, 12, 31), null, "Saldo", 1500M, 0M));
            person.Calculate(new DateTime(2010, 12, 31));
            debitorlisteView = objectMapper.Map<AdresseBase, AdressekontolisteView>(person);
            Assert.That(debitorlisteView, Is.Not.Null);
            Assert.That(debitorlisteView.Nummer, Is.EqualTo(1));
            Assert.That(debitorlisteView.Navn, Is.Not.Null);
            Assert.That(debitorlisteView.Navn, Is.EqualTo("Ole Sørensen"));
            Assert.That(debitorlisteView.PrimærTelefon, Is.Not.Null);
            Assert.That(debitorlisteView.PrimærTelefon, Is.EqualTo("62 21 49 60"));
            Assert.That(debitorlisteView.SekundærTelefon, Is.Not.Null);
            Assert.That(debitorlisteView.SekundærTelefon, Is.EqualTo("25 24 49 75"));
            Assert.That(debitorlisteView.Saldo, Is.EqualTo(1500M));

            var firma = new Firma(2, "OS Development Group", adressegruppe);
            firma.SætTelefon("62 21 49 60", "25 24 49 75", null);
            firma.TilføjBogføringslinje(new Bogføringslinje(2, new DateTime(2010, 12, 31), null, "Saldo", 2500M, 0M));
            firma.Calculate(new DateTime(2010, 12, 31));
            debitorlisteView = objectMapper.Map<AdresseBase, AdressekontolisteView>(firma);
            Assert.That(debitorlisteView, Is.Not.Null);
            Assert.That(debitorlisteView.Nummer, Is.EqualTo(2));
            Assert.That(debitorlisteView.Navn, Is.Not.Null);
            Assert.That(debitorlisteView.Navn, Is.EqualTo("OS Development Group"));
            Assert.That(debitorlisteView.PrimærTelefon, Is.Not.Null);
            Assert.That(debitorlisteView.PrimærTelefon, Is.EqualTo("62 21 49 60"));
            Assert.That(debitorlisteView.SekundærTelefon, Is.Not.Null);
            Assert.That(debitorlisteView.SekundærTelefon, Is.EqualTo("25 24 49 75"));
            Assert.That(debitorlisteView.Saldo, Is.EqualTo(2500M));

            var andenAdresse = new OtherAddress(3, "Bente Susanne Rasmussen", adressegruppe);
            Assert.Throws<AutoMapperMappingException>(() => objectMapper.Map<AdresseBase, AdressekontolisteView>(andenAdresse));
        }

        /// <summary>
        /// Tester, at en basisadresse kan mappes til et debitorlisteview.
        /// </summary>
        [Test]
        public void TestAtAdresseBaseMappesTilDebitorlisteView()
        {
            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var debitorlisteView = objectMapper.Map<AdresseBase, DebitorlisteView>(null);
            Assert.That(debitorlisteView, Is.Null);

            var adressegruppe = new Adressegruppe(1, "Adresser", 0);
            Assert.That(adressegruppe, Is.Not.Null);

            var person = new Person(1, "Ole Sørensen", adressegruppe);
            person.SætTelefon("62 21 49 60", "25 24 49 75");
            person.TilføjBogføringslinje(new Bogføringslinje(1, new DateTime(2010, 12, 31), null, "Saldo", 1500M, 0M));
            person.Calculate(new DateTime(2010, 12, 31));
            debitorlisteView = objectMapper.Map<AdresseBase, DebitorlisteView>(person);
            Assert.That(debitorlisteView, Is.Not.Null);
            Assert.That(debitorlisteView.Nummer, Is.EqualTo(1));
            Assert.That(debitorlisteView.Navn, Is.Not.Null);
            Assert.That(debitorlisteView.Navn, Is.EqualTo("Ole Sørensen"));
            Assert.That(debitorlisteView.PrimærTelefon, Is.Not.Null);
            Assert.That(debitorlisteView.PrimærTelefon, Is.EqualTo("62 21 49 60"));
            Assert.That(debitorlisteView.SekundærTelefon, Is.Not.Null);
            Assert.That(debitorlisteView.SekundærTelefon, Is.EqualTo("25 24 49 75"));
            Assert.That(debitorlisteView.Saldo, Is.EqualTo(1500M));

            var firma = new Firma(2, "OS Development Group", adressegruppe);
            firma.SætTelefon("62 21 49 60", "25 24 49 75", null);
            firma.TilføjBogføringslinje(new Bogføringslinje(2, new DateTime(2010, 12, 31), null, "Saldo", 2500M, 0M));
            firma.Calculate(new DateTime(2010, 12, 31));
            debitorlisteView = objectMapper.Map<AdresseBase, DebitorlisteView>(firma);
            Assert.That(debitorlisteView, Is.Not.Null);
            Assert.That(debitorlisteView.Nummer, Is.EqualTo(2));
            Assert.That(debitorlisteView.Navn, Is.Not.Null);
            Assert.That(debitorlisteView.Navn, Is.EqualTo("OS Development Group"));
            Assert.That(debitorlisteView.PrimærTelefon, Is.Not.Null);
            Assert.That(debitorlisteView.PrimærTelefon, Is.EqualTo("62 21 49 60"));
            Assert.That(debitorlisteView.SekundærTelefon, Is.Not.Null);
            Assert.That(debitorlisteView.SekundærTelefon, Is.EqualTo("25 24 49 75"));
            Assert.That(debitorlisteView.Saldo, Is.EqualTo(2500M));

            var andenAdresse = new OtherAddress(3, "Bente Susanne Rasmussen", adressegruppe);
            Assert.Throws<AutoMapperMappingException>(() => objectMapper.Map<AdresseBase, DebitorlisteView>(andenAdresse));
        }

        /// <summary>
        /// Tester, at en basisadresse kan mappes til et adressekontoview.
        /// </summary>
        [Test]
        public void TestAtAdresseBaseMappesTilAdressekontoView()
        {
            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var debitorView = objectMapper.Map<AdresseBase, AdressekontoView>(null);
            Assert.That(debitorView, Is.Null);

            var adressegruppe = new Adressegruppe(1, "Adresser", 0);
            Assert.That(adressegruppe, Is.Not.Null);

            var betalingsbetingelse = new Betalingsbetingelse(1, "Kontant");
            Assert.That(betalingsbetingelse, Is.Not.Null);

            var person = new Person(1, "Ole Sørensen", adressegruppe);
            person.SætAdresseoplysninger("Eggertsvænge 2", "c/o:", "5700  Svendborg");
            person.SætTelefon("62 21 49 60", "25 24 49 75");
            person.SætMailadresse("os@dsidata.dk");
            person.SætBetalingsbetingelse(betalingsbetingelse);
            person.TilføjBogføringslinje(new Bogføringslinje(1, new DateTime(2010, 12, 31), null, "Saldo", 1500M, 0M));
            person.Calculate(new DateTime(2010, 12, 31));
            debitorView = objectMapper.Map<AdresseBase, AdressekontoView>(person);
            Assert.That(debitorView, Is.Not.Null);
            Assert.That(debitorView.Nummer, Is.EqualTo(1));
            Assert.That(debitorView.Navn, Is.Not.Null);
            Assert.That(debitorView.Navn, Is.EqualTo("Ole Sørensen"));
            Assert.That(debitorView.Adresse1, Is.Not.Null);
            Assert.That(debitorView.Adresse1, Is.EqualTo("Eggertsvænge 2"));
            Assert.That(debitorView.Adresse2, Is.Not.Null);
            Assert.That(debitorView.Adresse2, Is.EqualTo("c/o:"));
            Assert.That(debitorView.PostnummerBy, Is.Not.Null);
            Assert.That(debitorView.PostnummerBy, Is.EqualTo("5700  Svendborg"));
            Assert.That(debitorView.PrimærTelefon, Is.Not.Null);
            Assert.That(debitorView.PrimærTelefon, Is.EqualTo("62 21 49 60"));
            Assert.That(debitorView.SekundærTelefon, Is.Not.Null);
            Assert.That(debitorView.SekundærTelefon, Is.EqualTo("25 24 49 75"));
            Assert.That(debitorView.Mailadresse, Is.Not.Null);
            Assert.That(debitorView.Mailadresse, Is.EqualTo("os@dsidata.dk"));
            Assert.That(debitorView.Betalingsbetingelse, Is.Not.Null);
            Assert.That(debitorView.Saldo, Is.EqualTo(1500M));

            var firma = new Firma(2, "OS Development Group", adressegruppe);
            firma.SætAdresseoplysninger("Eggertsvænge 2", "c/o:", "5700  Svendborg");
            firma.SætTelefon("62 21 49 60", "25 24 49 75", null);
            firma.SætMailadresse("os@dsidata.dk");
            firma.SætBetalingsbetingelse(betalingsbetingelse);
            firma.TilføjBogføringslinje(new Bogføringslinje(2, new DateTime(2010, 12, 31), null, "Saldo", 2500M, 0M));
            firma.Calculate(new DateTime(2010, 12, 31));
            debitorView = objectMapper.Map<AdresseBase, AdressekontoView>(firma);
            Assert.That(debitorView, Is.Not.Null);
            Assert.That(debitorView.Nummer, Is.EqualTo(2));
            Assert.That(debitorView.Navn, Is.Not.Null);
            Assert.That(debitorView.Navn, Is.EqualTo("OS Development Group"));
            Assert.That(debitorView.Adresse1, Is.Not.Null);
            Assert.That(debitorView.Adresse1, Is.EqualTo("Eggertsvænge 2"));
            Assert.That(debitorView.Adresse2, Is.Not.Null);
            Assert.That(debitorView.Adresse2, Is.EqualTo("c/o:"));
            Assert.That(debitorView.PostnummerBy, Is.Not.Null);
            Assert.That(debitorView.PostnummerBy, Is.EqualTo("5700  Svendborg"));
            Assert.That(debitorView.PrimærTelefon, Is.Not.Null);
            Assert.That(debitorView.PrimærTelefon, Is.EqualTo("62 21 49 60"));
            Assert.That(debitorView.SekundærTelefon, Is.Not.Null);
            Assert.That(debitorView.SekundærTelefon, Is.EqualTo("25 24 49 75"));
            Assert.That(debitorView.Mailadresse, Is.Not.Null);
            Assert.That(debitorView.Mailadresse, Is.EqualTo("os@dsidata.dk"));
            Assert.That(debitorView.Betalingsbetingelse, Is.Not.Null);
            Assert.That(debitorView.Saldo, Is.EqualTo(2500M));

            var andenAdresse = new OtherAddress(3, "Bente Susanne Rasmussen", adressegruppe);
            Assert.Throws<AutoMapperMappingException>(() => objectMapper.Map<AdresseBase, AdressekontoView>(andenAdresse));
        }

        /// <summary>
        /// Tester, at en basisadresse kan mappes til et debitorview.
        /// </summary>
        [Test]
        public void TestAtAdresseBaseMappesTilDebitorView()
        {
            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var debitorView = objectMapper.Map<AdresseBase, DebitorView>(null);
            Assert.That(debitorView, Is.Null);

            var adressegruppe = new Adressegruppe(1, "Adresser", 0);
            Assert.That(adressegruppe, Is.Not.Null);

            var betalingsbetingelse = new Betalingsbetingelse(1, "Kontant");
            Assert.That(betalingsbetingelse, Is.Not.Null);

            var person = new Person(1, "Ole Sørensen", adressegruppe);
            person.SætAdresseoplysninger("Eggertsvænge 2", "c/o:", "5700  Svendborg");
            person.SætTelefon("62 21 49 60", "25 24 49 75");
            person.SætMailadresse("os@dsidata.dk");
            person.SætBetalingsbetingelse(betalingsbetingelse);
            person.TilføjBogføringslinje(new Bogføringslinje(1, new DateTime(2010, 12, 31), null, "Saldo", 1500M, 0M));
            person.Calculate(new DateTime(2010, 12, 31));
            debitorView = objectMapper.Map<AdresseBase, DebitorView>(person);
            Assert.That(debitorView, Is.Not.Null);
            Assert.That(debitorView.Nummer, Is.EqualTo(1));
            Assert.That(debitorView.Navn, Is.Not.Null);
            Assert.That(debitorView.Navn, Is.EqualTo("Ole Sørensen"));
            Assert.That(debitorView.Adresse1, Is.Not.Null);
            Assert.That(debitorView.Adresse1, Is.EqualTo("Eggertsvænge 2"));
            Assert.That(debitorView.Adresse2, Is.Not.Null);
            Assert.That(debitorView.Adresse2, Is.EqualTo("c/o:"));
            Assert.That(debitorView.PostnummerBy, Is.Not.Null);
            Assert.That(debitorView.PostnummerBy, Is.EqualTo("5700  Svendborg"));
            Assert.That(debitorView.PrimærTelefon, Is.Not.Null);
            Assert.That(debitorView.PrimærTelefon, Is.EqualTo("62 21 49 60"));
            Assert.That(debitorView.SekundærTelefon, Is.Not.Null);
            Assert.That(debitorView.SekundærTelefon, Is.EqualTo("25 24 49 75"));
            Assert.That(debitorView.Mailadresse, Is.Not.Null);
            Assert.That(debitorView.Mailadresse, Is.EqualTo("os@dsidata.dk"));
            Assert.That(debitorView.Betalingsbetingelse, Is.Not.Null);
            Assert.That(debitorView.Saldo, Is.EqualTo(1500M));

            var firma = new Firma(2, "OS Development Group", adressegruppe);
            firma.SætAdresseoplysninger("Eggertsvænge 2", "c/o:", "5700  Svendborg");
            firma.SætTelefon("62 21 49 60", "25 24 49 75", null);
            firma.SætMailadresse("os@dsidata.dk");
            firma.SætBetalingsbetingelse(betalingsbetingelse);
            firma.TilføjBogføringslinje(new Bogføringslinje(2, new DateTime(2010, 12, 31), null, "Saldo", 2500M, 0M));
            firma.Calculate(new DateTime(2010, 12, 31));
            debitorView = objectMapper.Map<AdresseBase, DebitorView>(firma);
            Assert.That(debitorView, Is.Not.Null);
            Assert.That(debitorView.Nummer, Is.EqualTo(2));
            Assert.That(debitorView.Navn, Is.Not.Null);
            Assert.That(debitorView.Navn, Is.EqualTo("OS Development Group"));
            Assert.That(debitorView.Adresse1, Is.Not.Null);
            Assert.That(debitorView.Adresse1, Is.EqualTo("Eggertsvænge 2"));
            Assert.That(debitorView.Adresse2, Is.Not.Null);
            Assert.That(debitorView.Adresse2, Is.EqualTo("c/o:"));
            Assert.That(debitorView.PostnummerBy, Is.Not.Null);
            Assert.That(debitorView.PostnummerBy, Is.EqualTo("5700  Svendborg"));
            Assert.That(debitorView.PrimærTelefon, Is.Not.Null);
            Assert.That(debitorView.PrimærTelefon, Is.EqualTo("62 21 49 60"));
            Assert.That(debitorView.SekundærTelefon, Is.Not.Null);
            Assert.That(debitorView.SekundærTelefon, Is.EqualTo("25 24 49 75"));
            Assert.That(debitorView.Mailadresse, Is.Not.Null);
            Assert.That(debitorView.Mailadresse, Is.EqualTo("os@dsidata.dk"));
            Assert.That(debitorView.Betalingsbetingelse, Is.Not.Null);
            Assert.That(debitorView.Saldo, Is.EqualTo(2500M));

            var andenAdresse = new OtherAddress(3, "Bente Susanne Rasmussen", adressegruppe);
            Assert.Throws<AutoMapperMappingException>(() => objectMapper.Map<AdresseBase, DebitorView>(andenAdresse));
        }

        /// <summary>
        /// Tester, at en basisadresse kan mappes til et kreditorview.
        /// </summary>
        [Test]
        public void TestAtAdresseBaseMappesTilKreditorView()
        {
            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var kreditorView = objectMapper.Map<AdresseBase, KreditorView>(null);
            Assert.That(kreditorView, Is.Null);

            var adressegruppe = new Adressegruppe(1, "Adresser", 0);
            Assert.That(adressegruppe, Is.Not.Null);

            var betalingsbetingelse = new Betalingsbetingelse(1, "Kontant");
            Assert.That(betalingsbetingelse, Is.Not.Null);

            var person = new Person(1, "Ole Sørensen", adressegruppe);
            person.SætAdresseoplysninger("Eggertsvænge 2", "c/o:", "5700  Svendborg");
            person.SætTelefon("62 21 49 60", "25 24 49 75");
            person.SætMailadresse("os@dsidata.dk");
            person.SætBetalingsbetingelse(betalingsbetingelse);
            person.TilføjBogføringslinje(new Bogføringslinje(1, new DateTime(2010, 12, 31), null, "Saldo", 0M, 1500M));
            person.Calculate(new DateTime(2010, 12, 31));
            kreditorView = objectMapper.Map<AdresseBase, KreditorView>(person);
            Assert.That(kreditorView, Is.Not.Null);
            Assert.That(kreditorView.Nummer, Is.EqualTo(1));
            Assert.That(kreditorView.Navn, Is.Not.Null);
            Assert.That(kreditorView.Navn, Is.EqualTo("Ole Sørensen"));
            Assert.That(kreditorView.Adresse1, Is.Not.Null);
            Assert.That(kreditorView.Adresse1, Is.EqualTo("Eggertsvænge 2"));
            Assert.That(kreditorView.Adresse2, Is.Not.Null);
            Assert.That(kreditorView.Adresse2, Is.EqualTo("c/o:"));
            Assert.That(kreditorView.PostnummerBy, Is.Not.Null);
            Assert.That(kreditorView.PostnummerBy, Is.EqualTo("5700  Svendborg"));
            Assert.That(kreditorView.PrimærTelefon, Is.Not.Null);
            Assert.That(kreditorView.PrimærTelefon, Is.EqualTo("62 21 49 60"));
            Assert.That(kreditorView.SekundærTelefon, Is.Not.Null);
            Assert.That(kreditorView.SekundærTelefon, Is.EqualTo("25 24 49 75"));
            Assert.That(kreditorView.Mailadresse, Is.Not.Null);
            Assert.That(kreditorView.Mailadresse, Is.EqualTo("os@dsidata.dk"));
            Assert.That(kreditorView.Betalingsbetingelse, Is.Not.Null);
            Assert.That(kreditorView.Saldo, Is.EqualTo(-1500M));

            var firma = new Firma(2, "OS Development Group", adressegruppe);
            firma.SætAdresseoplysninger("Eggertsvænge 2", "c/o:", "5700  Svendborg");
            firma.SætTelefon("62 21 49 60", "25 24 49 75", null);
            firma.SætMailadresse("os@dsidata.dk");
            firma.SætBetalingsbetingelse(betalingsbetingelse);
            firma.TilføjBogføringslinje(new Bogføringslinje(2, new DateTime(2010, 12, 31), null, "Saldo", 0M, 2500M));
            firma.Calculate(new DateTime(2010, 12, 31));
            kreditorView = objectMapper.Map<AdresseBase, KreditorView>(firma);
            Assert.That(kreditorView, Is.Not.Null);
            Assert.That(kreditorView.Nummer, Is.EqualTo(2));
            Assert.That(kreditorView.Navn, Is.Not.Null);
            Assert.That(kreditorView.Navn, Is.EqualTo("OS Development Group"));
            Assert.That(kreditorView.Adresse1, Is.Not.Null);
            Assert.That(kreditorView.Adresse1, Is.EqualTo("Eggertsvænge 2"));
            Assert.That(kreditorView.Adresse2, Is.Not.Null);
            Assert.That(kreditorView.Adresse2, Is.EqualTo("c/o:"));
            Assert.That(kreditorView.PostnummerBy, Is.Not.Null);
            Assert.That(kreditorView.PostnummerBy, Is.EqualTo("5700  Svendborg"));
            Assert.That(kreditorView.PrimærTelefon, Is.Not.Null);
            Assert.That(kreditorView.PrimærTelefon, Is.EqualTo("62 21 49 60"));
            Assert.That(kreditorView.SekundærTelefon, Is.Not.Null);
            Assert.That(kreditorView.SekundærTelefon, Is.EqualTo("25 24 49 75"));
            Assert.That(kreditorView.Mailadresse, Is.Not.Null);
            Assert.That(kreditorView.Mailadresse, Is.EqualTo("os@dsidata.dk"));
            Assert.That(kreditorView.Betalingsbetingelse, Is.Not.Null);
            Assert.That(kreditorView.Saldo, Is.EqualTo(-2500M));

            var andenAdresse = new OtherAddress(3, "Bente Susanne Rasmussen", adressegruppe);
            Assert.Throws<AutoMapperMappingException>(() => objectMapper.Map<AdresseBase, KreditorView>(andenAdresse));
        }

        /// <summary>
        /// Tester, at en basisadresse kan mappes til et kreditorlisteview.
        /// </summary>
        [Test]
        public void TestAtAdresseBaseMappesTilKreditorlisteView()
        {
            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var kreditorlisteView = objectMapper.Map<AdresseBase, KreditorlisteView>(null);
            Assert.That(kreditorlisteView, Is.Null);

            var adressegruppe = new Adressegruppe(1, "Adresser", 0);
            Assert.That(adressegruppe, Is.Not.Null);

            var person = new Person(1, "Ole Sørensen", adressegruppe);
            person.SætTelefon("62 21 49 60", "25 24 49 75");
            person.TilføjBogføringslinje(new Bogføringslinje(1, new DateTime(2010, 12, 31), null, "Saldo", 0M, 1500M));
            person.Calculate(new DateTime(2010, 12, 31));
            kreditorlisteView = objectMapper.Map<AdresseBase, KreditorlisteView>(person);
            Assert.That(kreditorlisteView, Is.Not.Null);
            Assert.That(kreditorlisteView.Nummer, Is.EqualTo(1));
            Assert.That(kreditorlisteView.Navn, Is.Not.Null);
            Assert.That(kreditorlisteView.Navn, Is.EqualTo("Ole Sørensen"));
            Assert.That(kreditorlisteView.PrimærTelefon, Is.Not.Null);
            Assert.That(kreditorlisteView.PrimærTelefon, Is.EqualTo("62 21 49 60"));
            Assert.That(kreditorlisteView.SekundærTelefon, Is.Not.Null);
            Assert.That(kreditorlisteView.SekundærTelefon, Is.EqualTo("25 24 49 75"));
            Assert.That(kreditorlisteView.Saldo, Is.EqualTo(-1500M));

            var firma = new Firma(2, "OS Development Group", adressegruppe);
            firma.SætTelefon("62 21 49 60", "25 24 49 75", null);
            firma.TilføjBogføringslinje(new Bogføringslinje(2, new DateTime(2010, 12, 31), null, "Saldo", 0M, 2500M));
            firma.Calculate(new DateTime(2010, 12, 31));
            kreditorlisteView = objectMapper.Map<AdresseBase, KreditorlisteView>(firma);
            Assert.That(kreditorlisteView, Is.Not.Null);
            Assert.That(kreditorlisteView.Nummer, Is.EqualTo(2));
            Assert.That(kreditorlisteView.Navn, Is.Not.Null);
            Assert.That(kreditorlisteView.Navn, Is.EqualTo("OS Development Group"));
            Assert.That(kreditorlisteView.PrimærTelefon, Is.Not.Null);
            Assert.That(kreditorlisteView.PrimærTelefon, Is.EqualTo("62 21 49 60"));
            Assert.That(kreditorlisteView.SekundærTelefon, Is.Not.Null);
            Assert.That(kreditorlisteView.SekundærTelefon, Is.EqualTo("25 24 49 75"));
            Assert.That(kreditorlisteView.Saldo, Is.EqualTo(-2500M));

            var andenAdresse = new OtherAddress(3, "Bente Susanne Rasmussen", adressegruppe);
            Assert.Throws<AutoMapperMappingException>(() => objectMapper.Map<AdresseBase, KreditorlisteView>(andenAdresse));
        }

        /// <summary>
        /// Tester, at en betalingsbetingelse kan mappes til et betalingsbetingelseview.
        /// </summary>
        [Test]
        public void TestAtBetalingsbetingelseKanMappesTilBetalingsbetingelseView()
        {
            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var betalingsbetingelse = new Betalingsbetingelse(1, "Kontant");
            var betalingsbetingelseView = objectMapper.Map<Betalingsbetingelse, BetalingsbetingelseView>(betalingsbetingelse);
            Assert.That(betalingsbetingelseView, Is.Not.Null);
            Assert.That(betalingsbetingelseView.Nummer, Is.EqualTo(1));
            Assert.That(betalingsbetingelseView.Navn, Is.Not.Null);
            Assert.That(betalingsbetingelseView.Navn, Is.EqualTo("Kontant"));
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
        /// Tester, at en konto kan mappes til et kontoplanview.
        /// </summary>
        [Test]
        public void TestAtKontoKanMappesTilKontoplanView()
        {
            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var regnskab = new Regnskab(1, "Privatregnskab, Ole Sørensen");
            var kontogruppe = new Kontogruppe(1, "Bankkonti", KontogruppeType.Aktiver);
            var konto = new Konto(regnskab, "DANKORT", "Dankort", kontogruppe);
            konto.SætBeskrivelse("Dankort/lønkonto");
            konto.SætNote("Kredit på kr. 5.000,00");
            konto.TilføjKreditoplysninger(new Kreditoplysninger(2010, 10, 5000M));
            konto.TilføjBogføringslinje(new Bogføringslinje(1, new DateTime(2010, 10, 1), null, "Saldo", 2500M, 0M));
            konto.TilføjBogføringslinje(new Bogføringslinje(2, new DateTime(2010, 10, 1), null, "Kvickly", 0M, 250M));
            konto.Calculate(new DateTime(2010, 10, 31));

            var kontoplanView = objectMapper.Map<Konto, KontoplanView>(konto);
            Assert.That(kontoplanView, Is.Not.Null);
            Assert.That(kontoplanView.Regnskab, Is.Not.Null);
            Assert.That(kontoplanView.Kontonummer, Is.Not.Null);
            Assert.That(kontoplanView.Kontonummer, Is.EqualTo("DANKORT"));
            Assert.That(kontoplanView.Kontonavn, Is.Not.Null);
            Assert.That(kontoplanView.Kontonavn, Is.EqualTo("Dankort"));
            Assert.That(kontoplanView.Beskrivelse, Is.Not.Null);
            Assert.That(kontoplanView.Beskrivelse, Is.EqualTo("Dankort/lønkonto"));
            Assert.That(kontoplanView.Notat, Is.Not.Null);
            Assert.That(kontoplanView.Notat, Is.EqualTo("Kredit på kr. 5.000,00"));
            Assert.That(kontoplanView.Kontogruppe, Is.Not.Null);
            Assert.That(kontoplanView.Kredit, Is.EqualTo(5000M));
            Assert.That(kontoplanView.Saldo, Is.EqualTo(2250M));
            Assert.That(kontoplanView.Disponibel, Is.EqualTo(7250M));
        }

        /// <summary>
        /// Tester, at en konto kan mappes til et kontoview.
        /// </summary>
        [Test]
        public void TestAtKontoKanMappesTilKontoView()
        {
            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var regnskab = new Regnskab(1, "Privatregnskab, Ole Sørensen");
            var kontogruppe = new Kontogruppe(1, "Bankkonti", KontogruppeType.Aktiver);
            var konto = new Konto(regnskab, "DANKORT", "Dankort", kontogruppe);
            konto.SætBeskrivelse("Dankort/lønkonto");
            konto.SætNote("Kredit på kr. 5.000,00");
            konto.TilføjKreditoplysninger(new Kreditoplysninger(2010, 10, 5000M));
            konto.TilføjBogføringslinje(new Bogføringslinje(1, new DateTime(2010, 10, 1), null, "Saldo", 2500M, 0M));
            konto.TilføjBogføringslinje(new Bogføringslinje(2, new DateTime(2010, 10, 1), null, "Kvickly", 0M, 250M));
            konto.Calculate(new DateTime(2010, 10, 31));

            var kontopView = objectMapper.Map<Konto, KontoView>(konto);
            Assert.That(kontopView, Is.Not.Null);
            Assert.That(kontopView.Regnskab, Is.Not.Null);
            Assert.That(kontopView.Kontonummer, Is.Not.Null);
            Assert.That(kontopView.Kontonummer, Is.EqualTo("DANKORT"));
            Assert.That(kontopView.Kontonavn, Is.Not.Null);
            Assert.That(kontopView.Kontonavn, Is.EqualTo("Dankort"));
            Assert.That(kontopView.Beskrivelse, Is.Not.Null);
            Assert.That(kontopView.Beskrivelse, Is.EqualTo("Dankort/lønkonto"));
            Assert.That(kontopView.Notat, Is.Not.Null);
            Assert.That(kontopView.Notat, Is.EqualTo("Kredit på kr. 5.000,00"));
            Assert.That(kontopView.Kontogruppe, Is.Not.Null);
            Assert.That(kontopView.Kredit, Is.EqualTo(5000M));
            Assert.That(kontopView.Saldo, Is.EqualTo(2250M));
            Assert.That(kontopView.Disponibel, Is.EqualTo(7250M));
            Assert.That(kontopView.Kreditoplysninger, Is.Not.Null);
            Assert.That(kontopView.Kreditoplysninger.Count(), Is.EqualTo(1));
        }

        /// <summary>
        /// Tester, at kreditoplysninger kan mappes til et kreditoplysningerview.
        /// </summary>
        [Test]
        public void TestAtKreditoplysningerKanMappesTilKreditoplysningerView()
        {
            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var kreditoplysninger = new Kreditoplysninger(2010, 10, 15000M);

            var kreditoplysningerView = objectMapper.Map<Kreditoplysninger, KreditoplysningerView>(kreditoplysninger);
            Assert.That(kreditoplysningerView, Is.Not.Null);
            Assert.That(kreditoplysningerView.År, Is.EqualTo(2010));
            Assert.That(kreditoplysningerView.Måned, Is.EqualTo(10));
            Assert.That(kreditoplysningerView.Kredit, Is.EqualTo(15000M));
        }

        /// <summary>
        /// Tester, at en budgetkonto kan mappes til et budgetkontoplanview.
        /// </summary>
        [Test]
        public void TestAtBudgetkontoKanMappesTilBudgetkontoplanView()
        {
            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var regnskab = new Regnskab(1, "Privatregnskab, Ole Sørensen");
            var budgetkontogruppe = new Budgetkontogruppe(1, "Indtægter");
            var budgetkonto = new Budgetkonto(regnskab, "1000", "Indtægter", budgetkontogruppe);
            budgetkonto.SætBeskrivelse("Salg m.m.");
            budgetkonto.SætNote("Indtægter vedrørende salg m.m.");
            budgetkonto.TilføjBudgetoplysninger(new Budgetoplysninger(2010, 10, 15000M, 0M));
            budgetkonto.TilføjBogføringslinje(new Bogføringslinje(1, new DateTime(2010, 10, 1), null, "Indbetaling",
                                                                  10000M, 0M));
            budgetkonto.TilføjBogføringslinje(new Bogføringslinje(2, new DateTime(2010, 10, 5), null, "Indbetaling",
                                                                  4000M, 0M));
            budgetkonto.Calculate(new DateTime(2010, 10, 31));

            var bugetkontoplanView = objectMapper.Map<Budgetkonto, BudgetkontoplanView>(budgetkonto);
            Assert.That(bugetkontoplanView, Is.Not.Null);
            Assert.That(bugetkontoplanView.Regnskab, Is.Not.Null);
            Assert.That(bugetkontoplanView.Kontonummer, Is.Not.Null);
            Assert.That(bugetkontoplanView.Kontonummer, Is.EqualTo("1000"));
            Assert.That(bugetkontoplanView.Kontonavn, Is.Not.Null);
            Assert.That(bugetkontoplanView.Kontonavn, Is.EqualTo("Indtægter"));
            Assert.That(bugetkontoplanView.Beskrivelse, Is.Not.Null);
            Assert.That(bugetkontoplanView.Beskrivelse, Is.EqualTo("Salg m.m."));
            Assert.That(bugetkontoplanView.Notat, Is.Not.Null);
            Assert.That(bugetkontoplanView.Notat, Is.EqualTo("Indtægter vedrørende salg m.m."));
            Assert.That(bugetkontoplanView.Budgetkontogruppe, Is.Not.Null);
            Assert.That(bugetkontoplanView.Budget, Is.EqualTo(15000M));
            Assert.That(bugetkontoplanView.Bogført, Is.EqualTo(14000M));
            Assert.That(bugetkontoplanView.Disponibel, Is.EqualTo(1000M));
        }

        /// <summary>
        /// Tester, at en budgetkonto kan mappes til et budgetkontview.
        /// </summary>
        [Test]
        public void TestAtBudgetkontoKanMappesTilBudgetkontoView()
        {
            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var regnskab = new Regnskab(1, "Privatregnskab, Ole Sørensen");
            var budgetkontogruppe = new Budgetkontogruppe(1, "Indtægter");
            var budgetkonto = new Budgetkonto(regnskab, "1000", "Indtægter", budgetkontogruppe);
            budgetkonto.SætBeskrivelse("Salg m.m.");
            budgetkonto.SætNote("Indtægter vedrørende salg m.m.");
            budgetkonto.TilføjBudgetoplysninger(new Budgetoplysninger(2010, 10, 15000M, 0M));
            budgetkonto.TilføjBogføringslinje(new Bogføringslinje(1, new DateTime(2010, 10, 1), null, "Indbetaling",
                                                                  10000M, 0M));
            budgetkonto.TilføjBogføringslinje(new Bogføringslinje(2, new DateTime(2010, 10, 5), null, "Indbetaling",
                                                                  4000M, 0M));
            budgetkonto.Calculate(new DateTime(2010, 10, 31));

            var bugetkontoView = objectMapper.Map<Budgetkonto, BudgetkontoView>(budgetkonto);
            Assert.That(bugetkontoView, Is.Not.Null);
            Assert.That(bugetkontoView.Regnskab, Is.Not.Null);
            Assert.That(bugetkontoView.Kontonummer, Is.Not.Null);
            Assert.That(bugetkontoView.Kontonummer, Is.EqualTo("1000"));
            Assert.That(bugetkontoView.Kontonavn, Is.Not.Null);
            Assert.That(bugetkontoView.Kontonavn, Is.EqualTo("Indtægter"));
            Assert.That(bugetkontoView.Beskrivelse, Is.Not.Null);
            Assert.That(bugetkontoView.Beskrivelse, Is.EqualTo("Salg m.m."));
            Assert.That(bugetkontoView.Notat, Is.Not.Null);
            Assert.That(bugetkontoView.Notat, Is.EqualTo("Indtægter vedrørende salg m.m."));
            Assert.That(bugetkontoView.Budgetkontogruppe, Is.Not.Null);
            Assert.That(bugetkontoView.Budget, Is.EqualTo(15000M));
            Assert.That(bugetkontoView.Bogført, Is.EqualTo(14000M));
            Assert.That(bugetkontoView.Disponibel, Is.EqualTo(1000M));
            Assert.That(bugetkontoView.Budgetoplysninger, Is.Not.Null);
            Assert.That(bugetkontoView.Budgetoplysninger.Count(), Is.EqualTo(1));
        }

        /// <summary>
        /// Test, at budgetoplysninger kan mappes til et budgetoplysningerview.
        /// </summary>
        [Test]
        public void TestAtBudgetoplysningerKanMappesTilBudgetoplysningerView()
        {
            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var regnskab = new Regnskab(1, "Privatregnskab, Ole Sørensen");
            var budgetkontogruppe = new Budgetkontogruppe(1, "Udgifter");
            var budgetkonto = new Budgetkonto(regnskab, "1000", "Udgifter", budgetkontogruppe);
            budgetkonto.TilføjBudgetoplysninger(new Budgetoplysninger(2010, 10, 0M, 3000M));
            budgetkonto.TilføjBogføringslinje(new Bogføringslinje(1, new DateTime(2010, 10, 1), null, "Udbetaling", 0M,
                                                                  2500M));
            budgetkonto.TilføjBogføringslinje(new Bogføringslinje(2, new DateTime(2010, 10, 5), null, "Udbetaling", 0M,
                                                                  250M));
            budgetkonto.Calculate(new DateTime(2010, 10, 31));

            var budgetoplysninger = budgetkonto.Budgetoplysninger.SingleOrDefault(m => m.År == 2010 && m.Måned == 10);
            Assert.That(budgetoplysninger, Is.Not.Null);

            var budgetoplysningerView = objectMapper.Map<Budgetoplysninger, BudgetoplysningerView>(budgetoplysninger);
            Assert.That(budgetoplysningerView, Is.Not.Null);
            Assert.That(budgetoplysningerView.År, Is.EqualTo(2010));
            Assert.That(budgetoplysningerView.Måned, Is.EqualTo(10));
            Assert.That(budgetoplysningerView.Budget, Is.EqualTo(-3000M));
            Assert.That(budgetoplysningerView.Bogført, Is.EqualTo(-2750M));
        }

        /// <summary>
        /// Test, at en bogføringslinje kan mappes til et bogføringslinjeview.
        /// </summary>
        [Test]
        public void TestAtBogføringslinjeKanMappesTilBogføringslinjeView()
        {
            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var regnskab = new Regnskab(1, "Privatregnskab, Ole Sørensen");
            var dankort = new Konto(regnskab, "DANKORT", "Dankort", new Kontogruppe(1, "Bankkonti", 0));
            var udgifter = new Budgetkonto(regnskab, "1000", "Udgifter", new Budgetkontogruppe(1, "Udgifter"));
            var adresse = new Person(1, "Ole Sørensen", new Adressegruppe(1, "Personer", 0));

            var bogføringsdato = new DateTime(2011, 3, 15);
            var bogføringslinje = new Bogføringslinje(1, bogføringsdato, "XYZ", "Test", 1000M, 500M);
            dankort.TilføjBogføringslinje(bogføringslinje);
            udgifter.TilføjBogføringslinje(bogføringslinje);
            adresse.TilføjBogføringslinje(bogføringslinje);

            var bogføringslinjeView = objectMapper.Map<Bogføringslinje, BogføringslinjeView>(bogføringslinje);
            Assert.That(bogføringslinjeView, Is.Not.Null);

            Assert.That(bogføringslinjeView.Løbenr, Is.EqualTo(1));
            Assert.That(bogføringslinjeView.Konto, Is.Not.Null);
            Assert.That(bogføringslinjeView.Budgetkonto, Is.Not.Null);
            Assert.That(bogføringslinjeView.Adressekonto, Is.Not.Null);
            Assert.That(bogføringslinjeView.Dato, Is.EqualTo(bogføringsdato));
            Assert.That(bogføringslinjeView.Bilag, Is.Not.Null);
            Assert.That(bogføringslinjeView.Bilag, Is.EqualTo("XYZ"));
            Assert.That(bogføringslinjeView.Tekst, Is.Not.Null);
            Assert.That(bogføringslinjeView.Tekst, Is.EqualTo("Test"));
            Assert.That(bogføringslinjeView.Debit, Is.EqualTo(1000M));
            Assert.That(bogføringslinjeView.Kredit, Is.EqualTo(500M));
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

        /// <summary>
        /// Tester, at en budgetkontogruppe kan mappes til et bugdetkontogruppeview.
        /// </summary>
        [Test]
        public void TestAtBudgetkontogruppeKanMappesTilBudgetkontogruppeView()
        {
            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var budgetkontogruppe = new Budgetkontogruppe(1, "Indtægter");
            var budgetkontogruppeView = objectMapper.Map<Budgetkontogruppe, BudgetkontogruppeView>(budgetkontogruppe);
            Assert.That(budgetkontogruppeView, Is.Not.Null);
            Assert.That(budgetkontogruppeView.Nummer, Is.EqualTo(1));
            Assert.That(budgetkontogruppeView.Navn, Is.Not.Null);
            Assert.That(budgetkontogruppeView.Navn, Is.EqualTo("Indtægter"));
        }
    }
}
