using System;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Enums;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Fælles;
using OSDevGrp.OSIntranet.Contracts.Responses;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Domain.Finansstyring;
using OSDevGrp.OSIntranet.Domain.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using AutoMapper;
using NUnit.Framework;
using Ploeh.AutoFixture;
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
        /// Kontoklasse, der kan benyttes til test.
        /// </summary>
        private class OtherKonto : KontoBase
        {
            /// <summary>
            /// Konstruerer kontoklasse, der kan benyttes til test.
            /// </summary>
            /// <param name="regnskab">Regnskab.</param>
            /// <param name="kontonummer">Kontonummer.</param>
            /// <param name="kontonavn">Kontonavn.</param>
            public OtherKonto(Regnskab regnskab, string kontonummer, string kontonavn)
                : base(regnskab, kontonummer, kontonavn)
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
        /// Tester, at Map kaster en ArgumentNullException, hvis source er null.
        /// </summary>
        [Test]
        public void TestAtMapKasterArgumentNullExceptionHvisSourceErNull()
        {
            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => objectMapper.Map<Regnskab, RegnskabslisteView>(null));
        }

        /// <summary>
        /// Tester, at en basisadresse kan mappes til et telefonlisteview.
        /// </summary>
        [Test]
        public void TestAtAdresseBaseMappesTilTelefonlisteView()
        {
            var fixture = new Fixture();

            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var adressegruppe = new Adressegruppe(1, "Adresser", 0);
            Assert.That(adressegruppe, Is.Not.Null);

            var person = fixture.CreateAnonymous<Person>();
            person.SætTelefon(fixture.CreateAnonymous<string>(), fixture.CreateAnonymous<string>());
            var telefonlisteView = objectMapper.Map<AdresseBase, TelefonlisteView>(person);
            Assert.That(telefonlisteView, Is.Not.Null);
            Assert.That(telefonlisteView.Nummer, Is.EqualTo(person.Nummer));
            Assert.That(telefonlisteView.Navn, Is.Not.Null);
            Assert.That(telefonlisteView.Navn, Is.EqualTo(person.Navn));
            Assert.That(telefonlisteView.PrimærTelefon, Is.Not.Null);
            Assert.That(telefonlisteView.PrimærTelefon, Is.EqualTo(person.Telefon));
            Assert.That(telefonlisteView.SekundærTelefon, Is.Not.Null);
            Assert.That(telefonlisteView.SekundærTelefon, Is.EqualTo(person.Mobil));

            var firma = fixture.CreateAnonymous<Firma>();
            firma.SætTelefon(fixture.CreateAnonymous<string>(), fixture.CreateAnonymous<string>(),
                             fixture.CreateAnonymous<string>());
            telefonlisteView = objectMapper.Map<AdresseBase, TelefonlisteView>(firma);
            Assert.That(telefonlisteView, Is.Not.Null);
            Assert.That(telefonlisteView.Nummer, Is.EqualTo(firma.Nummer));
            Assert.That(telefonlisteView.Navn, Is.Not.Null);
            Assert.That(telefonlisteView.Navn, Is.EqualTo(firma.Navn));
            Assert.That(telefonlisteView.PrimærTelefon, Is.Not.Null);
            Assert.That(telefonlisteView.PrimærTelefon, Is.EqualTo(firma.Telefon1));
            Assert.That(telefonlisteView.SekundærTelefon, Is.Not.Null);
            Assert.That(telefonlisteView.SekundærTelefon, Is.EqualTo(firma.Telefon2));

            var andenAdresse = new OtherAddress(fixture.CreateAnonymous<int>(), fixture.CreateAnonymous<string>(),
                                                fixture.CreateAnonymous<Adressegruppe>());
            Assert.Throws<AutoMapperMappingException>(() => objectMapper.Map<AdresseBase, TelefonlisteView>(andenAdresse));
        }

        /// <summary>
        /// Tester, at en basisadresse kan mappes til et adressekontolisteview.
        /// </summary>
        [Test]
        public void TestAtAdresseBaseMappesTilAdressekontolisteView()
        {
            var fixture = new Fixture();
            fixture.Inject(new DateTime(2010, 12, 31));

            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var person = fixture.CreateAnonymous<Person>();
            person.SætTelefon(fixture.CreateAnonymous<string>(), fixture.CreateAnonymous<string>());
            person.TilføjBogføringslinje(new Bogføringslinje(fixture.CreateAnonymous<int>(),
                                                             fixture.CreateAnonymous<DateTime>(),
                                                             fixture.CreateAnonymous<string>(),
                                                             fixture.CreateAnonymous<string>(),
                                                             fixture.CreateAnonymous<decimal>(), 0M));
            person.Calculate(fixture.CreateAnonymous<DateTime>());
            var adressekontolisteView = objectMapper.Map<AdresseBase, AdressekontolisteView>(person);
            Assert.That(adressekontolisteView, Is.Not.Null);
            Assert.That(adressekontolisteView.Nummer, Is.EqualTo(person.Nummer));
            Assert.That(adressekontolisteView.Navn, Is.Not.Null);
            Assert.That(adressekontolisteView.Navn, Is.EqualTo(person.Navn));
            Assert.That(adressekontolisteView.PrimærTelefon, Is.Not.Null);
            Assert.That(adressekontolisteView.PrimærTelefon, Is.EqualTo(person.Telefon));
            Assert.That(adressekontolisteView.SekundærTelefon, Is.Not.Null);
            Assert.That(adressekontolisteView.SekundærTelefon, Is.EqualTo(person.Mobil));
            Assert.That(adressekontolisteView.Saldo, Is.GreaterThan(0M));

            var firma = fixture.CreateAnonymous<Firma>();
            firma.SætTelefon(fixture.CreateAnonymous<string>(), fixture.CreateAnonymous<string>(),
                             fixture.CreateAnonymous<string>());
            firma.TilføjBogføringslinje(new Bogføringslinje(fixture.CreateAnonymous<int>(),
                                                            fixture.CreateAnonymous<DateTime>(),
                                                            fixture.CreateAnonymous<string>(),
                                                            fixture.CreateAnonymous<string>(),
                                                            fixture.CreateAnonymous<decimal>(), 0M));
            firma.Calculate(fixture.CreateAnonymous<DateTime>());
            adressekontolisteView = objectMapper.Map<AdresseBase, AdressekontolisteView>(firma);
            Assert.That(adressekontolisteView, Is.Not.Null);
            Assert.That(adressekontolisteView.Nummer, Is.EqualTo(firma.Nummer));
            Assert.That(adressekontolisteView.Navn, Is.Not.Null);
            Assert.That(adressekontolisteView.Navn, Is.EqualTo(firma.Navn));
            Assert.That(adressekontolisteView.PrimærTelefon, Is.Not.Null);
            Assert.That(adressekontolisteView.PrimærTelefon, Is.EqualTo(firma.Telefon1));
            Assert.That(adressekontolisteView.SekundærTelefon, Is.Not.Null);
            Assert.That(adressekontolisteView.SekundærTelefon, Is.EqualTo(firma.Telefon2));
            Assert.That(adressekontolisteView.Saldo, Is.GreaterThan(0M));

            var andenAdresse = new OtherAddress(fixture.CreateAnonymous<int>(), fixture.CreateAnonymous<string>(),
                                                fixture.CreateAnonymous<Adressegruppe>());
            Assert.Throws<AutoMapperMappingException>(() => objectMapper.Map<AdresseBase, AdressekontolisteView>(andenAdresse));
        }

        /// <summary>
        /// Tester, at en basisadresse kan mappes til et adressekontoview.
        /// </summary>
        [Test]
        public void TestAtAdresseBaseMappesTilAdressekontoView()
        {
            var fixture = new Fixture();
            fixture.Inject(new DateTime(2010, 12, 31));

            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var person = fixture.CreateAnonymous<Person>();
            person.SætAdresseoplysninger(fixture.CreateAnonymous<string>(), fixture.CreateAnonymous<string>(),
                                         fixture.CreateAnonymous<string>());
            person.SætTelefon(fixture.CreateAnonymous<string>(), fixture.CreateAnonymous<string>());
            person.SætMailadresse(fixture.CreateAnonymous<string>());
            person.SætBetalingsbetingelse(fixture.CreateAnonymous<Betalingsbetingelse>());
            person.TilføjBogføringslinje(new Bogføringslinje(fixture.CreateAnonymous<int>(),
                                                             fixture.CreateAnonymous<DateTime>(),
                                                             fixture.CreateAnonymous<string>(),
                                                             fixture.CreateAnonymous<string>(),
                                                             fixture.CreateAnonymous<decimal>(), 0M));
            person.Calculate(fixture.CreateAnonymous<DateTime>());
            var adressekontoView = objectMapper.Map<AdresseBase, AdressekontoView>(person);
            Assert.That(adressekontoView, Is.Not.Null);
            Assert.That(adressekontoView.Nummer, Is.EqualTo(person.Nummer));
            Assert.That(adressekontoView.Navn, Is.Not.Null);
            Assert.That(adressekontoView.Navn, Is.EqualTo(person.Navn));
            Assert.That(adressekontoView.Adresse1, Is.Not.Null);
            Assert.That(adressekontoView.Adresse1, Is.EqualTo(person.Adresse1));
            Assert.That(adressekontoView.Adresse2, Is.Not.Null);
            Assert.That(adressekontoView.Adresse2, Is.EqualTo(person.Adresse2));
            Assert.That(adressekontoView.PostnummerBy, Is.Not.Null);
            Assert.That(adressekontoView.PostnummerBy, Is.EqualTo(person.PostnrBy));
            Assert.That(adressekontoView.PrimærTelefon, Is.Not.Null);
            Assert.That(adressekontoView.PrimærTelefon, Is.EqualTo(person.Telefon));
            Assert.That(adressekontoView.SekundærTelefon, Is.Not.Null);
            Assert.That(adressekontoView.SekundærTelefon, Is.EqualTo(person.Mobil));
            Assert.That(adressekontoView.Mailadresse, Is.Not.Null);
            Assert.That(adressekontoView.Mailadresse, Is.EqualTo(person.Mailadresse));
            Assert.That(adressekontoView.Betalingsbetingelse, Is.Not.Null);
            Assert.That(adressekontoView.Saldo, Is.GreaterThan(0M));

            var firma = fixture.CreateAnonymous<Firma>();
            firma.SætAdresseoplysninger(fixture.CreateAnonymous<string>(), fixture.CreateAnonymous<string>(),
                                        fixture.CreateAnonymous<string>());
            firma.SætTelefon(fixture.CreateAnonymous<string>(), fixture.CreateAnonymous<string>(),
                             fixture.CreateAnonymous<string>());
            firma.SætMailadresse(fixture.CreateAnonymous<string>());
            firma.SætBetalingsbetingelse(fixture.CreateAnonymous<Betalingsbetingelse>());
            firma.TilføjBogføringslinje(new Bogføringslinje(fixture.CreateAnonymous<int>(),
                                                            fixture.CreateAnonymous<DateTime>(),
                                                            fixture.CreateAnonymous<string>(),
                                                            fixture.CreateAnonymous<string>(),
                                                            fixture.CreateAnonymous<decimal>(), 0M));
            firma.Calculate(fixture.CreateAnonymous<DateTime>());
            adressekontoView = objectMapper.Map<AdresseBase, AdressekontoView>(firma);
            Assert.That(adressekontoView, Is.Not.Null);
            Assert.That(adressekontoView.Nummer, Is.EqualTo(firma.Nummer));
            Assert.That(adressekontoView.Navn, Is.Not.Null);
            Assert.That(adressekontoView.Navn, Is.EqualTo(firma.Navn));
            Assert.That(adressekontoView.Adresse1, Is.Not.Null);
            Assert.That(adressekontoView.Adresse1, Is.EqualTo(firma.Adresse1));
            Assert.That(adressekontoView.Adresse2, Is.Not.Null);
            Assert.That(adressekontoView.Adresse2, Is.EqualTo(firma.Adresse2));
            Assert.That(adressekontoView.PostnummerBy, Is.Not.Null);
            Assert.That(adressekontoView.PostnummerBy, Is.EqualTo(firma.PostnrBy));
            Assert.That(adressekontoView.PrimærTelefon, Is.Not.Null);
            Assert.That(adressekontoView.PrimærTelefon, Is.EqualTo(firma.Telefon1));
            Assert.That(adressekontoView.SekundærTelefon, Is.Not.Null);
            Assert.That(adressekontoView.SekundærTelefon, Is.EqualTo(firma.Telefon2));
            Assert.That(adressekontoView.Mailadresse, Is.Not.Null);
            Assert.That(adressekontoView.Mailadresse, Is.EqualTo(firma.Mailadresse));
            Assert.That(adressekontoView.Betalingsbetingelse, Is.Not.Null);
            Assert.That(adressekontoView.Saldo, Is.GreaterThan(0M));

            var andenAdresse = new OtherAddress(fixture.CreateAnonymous<int>(), fixture.CreateAnonymous<string>(),
                                                fixture.CreateAnonymous<Adressegruppe>());
            Assert.Throws<AutoMapperMappingException>(() => objectMapper.Map<AdresseBase, AdressekontoView>(andenAdresse));
        }

        /// <summary>
        /// Tester, at en basisadresse kan mappes til et debitorlisteview.
        /// </summary>
        [Test]
        public void TestAtAdresseBaseMappesTilDebitorlisteView()
        {
            var fixture = new Fixture();
            fixture.Inject(new DateTime(2010, 12, 31));

            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var person = fixture.CreateAnonymous<Person>();
            person.SætTelefon(fixture.CreateAnonymous<string>(), fixture.CreateAnonymous<string>());
            person.TilføjBogføringslinje(new Bogføringslinje(fixture.CreateAnonymous<int>(),
                                                             fixture.CreateAnonymous<DateTime>(),
                                                             fixture.CreateAnonymous<string>(),
                                                             fixture.CreateAnonymous<string>(),
                                                             fixture.CreateAnonymous<decimal>(), 0M));
            person.Calculate(fixture.CreateAnonymous<DateTime>());
            var debitorlisteView = objectMapper.Map<AdresseBase, DebitorlisteView>(person);
            Assert.That(debitorlisteView, Is.Not.Null);
            Assert.That(debitorlisteView.Nummer, Is.EqualTo(person.Nummer));
            Assert.That(debitorlisteView.Navn, Is.Not.Null);
            Assert.That(debitorlisteView.Navn, Is.EqualTo(person.Navn));
            Assert.That(debitorlisteView.PrimærTelefon, Is.Not.Null);
            Assert.That(debitorlisteView.PrimærTelefon, Is.EqualTo(person.Telefon));
            Assert.That(debitorlisteView.SekundærTelefon, Is.Not.Null);
            Assert.That(debitorlisteView.SekundærTelefon, Is.EqualTo(person.Mobil));
            Assert.That(debitorlisteView.Saldo, Is.GreaterThan(0M));

            var firma = fixture.CreateAnonymous<Firma>();
            firma.SætTelefon(fixture.CreateAnonymous<string>(), fixture.CreateAnonymous<string>(),
                             fixture.CreateAnonymous<string>());
            firma.TilføjBogføringslinje(new Bogføringslinje(fixture.CreateAnonymous<int>(),
                                                            fixture.CreateAnonymous<DateTime>(),
                                                            fixture.CreateAnonymous<string>(),
                                                            fixture.CreateAnonymous<string>(),
                                                            fixture.CreateAnonymous<decimal>(), 0M));
            firma.Calculate(fixture.CreateAnonymous<DateTime>());
            debitorlisteView = objectMapper.Map<AdresseBase, DebitorlisteView>(firma);
            Assert.That(debitorlisteView, Is.Not.Null);
            Assert.That(debitorlisteView.Nummer, Is.EqualTo(firma.Nummer));
            Assert.That(debitorlisteView.Navn, Is.Not.Null);
            Assert.That(debitorlisteView.Navn, Is.EqualTo(firma.Navn));
            Assert.That(debitorlisteView.PrimærTelefon, Is.Not.Null);
            Assert.That(debitorlisteView.PrimærTelefon, Is.EqualTo(firma.Telefon1));
            Assert.That(debitorlisteView.SekundærTelefon, Is.Not.Null);
            Assert.That(debitorlisteView.SekundærTelefon, Is.EqualTo(firma.Telefon2));
            Assert.That(debitorlisteView.Saldo, Is.GreaterThan(0M));

            var andenAdresse = new OtherAddress(fixture.CreateAnonymous<int>(), fixture.CreateAnonymous<string>(),
                                                fixture.CreateAnonymous<Adressegruppe>());
            Assert.Throws<AutoMapperMappingException>(() => objectMapper.Map<AdresseBase, DebitorlisteView>(andenAdresse));
        }

        /// <summary>
        /// Tester, at en basisadresse kan mappes til et debitorview.
        /// </summary>
        [Test]
        public void TestAtAdresseBaseMappesTilDebitorView()
        {
            var fixture = new Fixture();
            fixture.Inject(new DateTime(2010, 12, 31));

            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var person = fixture.CreateAnonymous<Person>();
            person.SætAdresseoplysninger(fixture.CreateAnonymous<string>(), fixture.CreateAnonymous<string>(),
                                         fixture.CreateAnonymous<string>());
            person.SætTelefon(fixture.CreateAnonymous<string>(), fixture.CreateAnonymous<string>());
            person.SætMailadresse(fixture.CreateAnonymous<string>());
            person.SætBetalingsbetingelse(fixture.CreateAnonymous<Betalingsbetingelse>());
            person.TilføjBogføringslinje(new Bogføringslinje(fixture.CreateAnonymous<int>(),
                                                             fixture.CreateAnonymous<DateTime>(),
                                                             fixture.CreateAnonymous<string>(),
                                                             fixture.CreateAnonymous<string>(),
                                                             fixture.CreateAnonymous<decimal>(), 0M));
            person.Calculate(fixture.CreateAnonymous<DateTime>());
            var debitorView = objectMapper.Map<AdresseBase, DebitorView>(person);
            Assert.That(debitorView, Is.Not.Null);
            Assert.That(debitorView.Nummer, Is.EqualTo(person.Nummer));
            Assert.That(debitorView.Navn, Is.Not.Null);
            Assert.That(debitorView.Navn, Is.EqualTo(person.Navn));
            Assert.That(debitorView.Adresse1, Is.Not.Null);
            Assert.That(debitorView.Adresse1, Is.EqualTo(person.Adresse1));
            Assert.That(debitorView.Adresse2, Is.Not.Null);
            Assert.That(debitorView.Adresse2, Is.EqualTo(person.Adresse2));
            Assert.That(debitorView.PostnummerBy, Is.Not.Null);
            Assert.That(debitorView.PostnummerBy, Is.EqualTo(person.PostnrBy));
            Assert.That(debitorView.PrimærTelefon, Is.Not.Null);
            Assert.That(debitorView.PrimærTelefon, Is.EqualTo(person.Telefon));
            Assert.That(debitorView.SekundærTelefon, Is.Not.Null);
            Assert.That(debitorView.SekundærTelefon, Is.EqualTo(person.Mobil));
            Assert.That(debitorView.Mailadresse, Is.Not.Null);
            Assert.That(debitorView.Mailadresse, Is.EqualTo(person.Mailadresse));
            Assert.That(debitorView.Betalingsbetingelse, Is.Not.Null);
            Assert.That(debitorView.Saldo, Is.GreaterThan(0M));

            var firma = fixture.CreateAnonymous<Firma>();
            firma.SætAdresseoplysninger(fixture.CreateAnonymous<string>(), fixture.CreateAnonymous<string>(),
                                        fixture.CreateAnonymous<string>());
            firma.SætTelefon(fixture.CreateAnonymous<string>(), fixture.CreateAnonymous<string>(),
                             fixture.CreateAnonymous<string>());
            firma.SætMailadresse(fixture.CreateAnonymous<string>());
            firma.SætBetalingsbetingelse(fixture.CreateAnonymous<Betalingsbetingelse>());
            firma.TilføjBogføringslinje(new Bogføringslinje(fixture.CreateAnonymous<int>(),
                                                            fixture.CreateAnonymous<DateTime>(),
                                                            fixture.CreateAnonymous<string>(),
                                                            fixture.CreateAnonymous<string>(),
                                                            fixture.CreateAnonymous<decimal>(), 0M));
            firma.Calculate(fixture.CreateAnonymous<DateTime>());
            debitorView = objectMapper.Map<AdresseBase, DebitorView>(firma);
            Assert.That(debitorView, Is.Not.Null);
            Assert.That(debitorView.Nummer, Is.EqualTo(firma.Nummer));
            Assert.That(debitorView.Navn, Is.Not.Null);
            Assert.That(debitorView.Navn, Is.EqualTo(firma.Navn));
            Assert.That(debitorView.Adresse1, Is.Not.Null);
            Assert.That(debitorView.Adresse1, Is.EqualTo(firma.Adresse1));
            Assert.That(debitorView.Adresse2, Is.Not.Null);
            Assert.That(debitorView.Adresse2, Is.EqualTo(firma.Adresse2));
            Assert.That(debitorView.PostnummerBy, Is.Not.Null);
            Assert.That(debitorView.PostnummerBy, Is.EqualTo(firma.PostnrBy));
            Assert.That(debitorView.PrimærTelefon, Is.Not.Null);
            Assert.That(debitorView.PrimærTelefon, Is.EqualTo(firma.Telefon1));
            Assert.That(debitorView.SekundærTelefon, Is.Not.Null);
            Assert.That(debitorView.SekundærTelefon, Is.EqualTo(firma.Telefon2));
            Assert.That(debitorView.Mailadresse, Is.Not.Null);
            Assert.That(debitorView.Mailadresse, Is.EqualTo(firma.Mailadresse));
            Assert.That(debitorView.Betalingsbetingelse, Is.Not.Null);
            Assert.That(debitorView.Saldo, Is.GreaterThan(0M));

            var andenAdresse = new OtherAddress(fixture.CreateAnonymous<int>(), fixture.CreateAnonymous<string>(),
                                                fixture.CreateAnonymous<Adressegruppe>());
            Assert.Throws<AutoMapperMappingException>(() => objectMapper.Map<AdresseBase, DebitorView>(andenAdresse));
        }

        /// <summary>
        /// Tester, at en basisadresse kan mappes til et kreditorlisteview.
        /// </summary>
        [Test]
        public void TestAtAdresseBaseMappesTilKreditorlisteView()
        {
            var fixture = new Fixture();
            fixture.Inject(new DateTime(2010, 12, 31));

            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var person = fixture.CreateAnonymous<Person>();
            person.SætTelefon(fixture.CreateAnonymous<string>(), fixture.CreateAnonymous<string>());
            person.TilføjBogføringslinje(new Bogføringslinje(fixture.CreateAnonymous<int>(),
                                                             fixture.CreateAnonymous<DateTime>(),
                                                             fixture.CreateAnonymous<string>(),
                                                             fixture.CreateAnonymous<string>(),
                                                             0M, fixture.CreateAnonymous<decimal>()));
            person.Calculate(fixture.CreateAnonymous<DateTime>());
            var kreditorlisteView = objectMapper.Map<AdresseBase, KreditorlisteView>(person);
            Assert.That(kreditorlisteView, Is.Not.Null);
            Assert.That(kreditorlisteView.Nummer, Is.EqualTo(person.Nummer));
            Assert.That(kreditorlisteView.Navn, Is.Not.Null);
            Assert.That(kreditorlisteView.Navn, Is.EqualTo(person.Navn));
            Assert.That(kreditorlisteView.PrimærTelefon, Is.Not.Null);
            Assert.That(kreditorlisteView.PrimærTelefon, Is.EqualTo(person.Telefon));
            Assert.That(kreditorlisteView.SekundærTelefon, Is.Not.Null);
            Assert.That(kreditorlisteView.SekundærTelefon, Is.EqualTo(person.Mobil));
            Assert.That(kreditorlisteView.Saldo, Is.LessThan(0M));

            var firma = fixture.CreateAnonymous<Firma>();
            firma.SætTelefon(fixture.CreateAnonymous<string>(), fixture.CreateAnonymous<string>(),
                             fixture.CreateAnonymous<string>());
            firma.TilføjBogføringslinje(new Bogføringslinje(fixture.CreateAnonymous<int>(),
                                                            fixture.CreateAnonymous<DateTime>(),
                                                            fixture.CreateAnonymous<string>(),
                                                            fixture.CreateAnonymous<string>(),
                                                            0M, fixture.CreateAnonymous<decimal>()));
            firma.Calculate(fixture.CreateAnonymous<DateTime>());
            kreditorlisteView = objectMapper.Map<AdresseBase, KreditorlisteView>(firma);
            Assert.That(kreditorlisteView, Is.Not.Null);
            Assert.That(kreditorlisteView.Nummer, Is.EqualTo(firma.Nummer));
            Assert.That(kreditorlisteView.Navn, Is.Not.Null);
            Assert.That(kreditorlisteView.Navn, Is.EqualTo(firma.Navn));
            Assert.That(kreditorlisteView.PrimærTelefon, Is.Not.Null);
            Assert.That(kreditorlisteView.PrimærTelefon, Is.EqualTo(firma.Telefon1));
            Assert.That(kreditorlisteView.SekundærTelefon, Is.Not.Null);
            Assert.That(kreditorlisteView.SekundærTelefon, Is.EqualTo(firma.Telefon2));
            Assert.That(kreditorlisteView.Saldo, Is.LessThan(0M));

            var andenAdresse = new OtherAddress(fixture.CreateAnonymous<int>(), fixture.CreateAnonymous<string>(),
                                                fixture.CreateAnonymous<Adressegruppe>());
            Assert.Throws<AutoMapperMappingException>(() => objectMapper.Map<AdresseBase, KreditorlisteView>(andenAdresse));
        }

        /// <summary>
        /// Tester, at en basisadresse kan mappes til et kreditorview.
        /// </summary>
        [Test]
        public void TestAtAdresseBaseMappesTilKreditorView()
        {
            var fixture = new Fixture();
            fixture.Inject(new DateTime(2010, 12, 31));

            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var person = fixture.CreateAnonymous<Person>();
            person.SætAdresseoplysninger(fixture.CreateAnonymous<string>(), fixture.CreateAnonymous<string>(),
                                         fixture.CreateAnonymous<string>());
            person.SætTelefon(fixture.CreateAnonymous<string>(), fixture.CreateAnonymous<string>());
            person.SætMailadresse(fixture.CreateAnonymous<string>());
            person.SætBetalingsbetingelse(fixture.CreateAnonymous<Betalingsbetingelse>());
            person.TilføjBogføringslinje(new Bogføringslinje(fixture.CreateAnonymous<int>(),
                                                             fixture.CreateAnonymous<DateTime>(),
                                                             fixture.CreateAnonymous<string>(),
                                                             fixture.CreateAnonymous<string>(),
                                                             0M, fixture.CreateAnonymous<decimal>()));
            person.Calculate(fixture.CreateAnonymous<DateTime>());
            var kreditorView = objectMapper.Map<AdresseBase, KreditorView>(person);
            Assert.That(kreditorView, Is.Not.Null);
            Assert.That(kreditorView.Nummer, Is.EqualTo(person.Nummer));
            Assert.That(kreditorView.Navn, Is.Not.Null);
            Assert.That(kreditorView.Navn, Is.EqualTo(person.Navn));
            Assert.That(kreditorView.Adresse1, Is.Not.Null);
            Assert.That(kreditorView.Adresse1, Is.EqualTo(person.Adresse1));
            Assert.That(kreditorView.Adresse2, Is.Not.Null);
            Assert.That(kreditorView.Adresse2, Is.EqualTo(person.Adresse2));
            Assert.That(kreditorView.PostnummerBy, Is.Not.Null);
            Assert.That(kreditorView.PostnummerBy, Is.EqualTo(person.PostnrBy));
            Assert.That(kreditorView.PrimærTelefon, Is.Not.Null);
            Assert.That(kreditorView.PrimærTelefon, Is.EqualTo(person.Telefon));
            Assert.That(kreditorView.SekundærTelefon, Is.Not.Null);
            Assert.That(kreditorView.SekundærTelefon, Is.EqualTo(person.Mobil));
            Assert.That(kreditorView.Mailadresse, Is.Not.Null);
            Assert.That(kreditorView.Mailadresse, Is.EqualTo(person.Mailadresse));
            Assert.That(kreditorView.Betalingsbetingelse, Is.Not.Null);
            Assert.That(kreditorView.Saldo, Is.LessThan(0M));

            var firma = fixture.CreateAnonymous<Firma>();
            firma.SætAdresseoplysninger(fixture.CreateAnonymous<string>(), fixture.CreateAnonymous<string>(),
                                        fixture.CreateAnonymous<string>());
            firma.SætTelefon(fixture.CreateAnonymous<string>(), fixture.CreateAnonymous<string>(),
                             fixture.CreateAnonymous<string>());
            firma.SætMailadresse(fixture.CreateAnonymous<string>());
            firma.SætBetalingsbetingelse(fixture.CreateAnonymous<Betalingsbetingelse>());
            firma.TilføjBogføringslinje(new Bogføringslinje(fixture.CreateAnonymous<int>(),
                                                            fixture.CreateAnonymous<DateTime>(),
                                                            fixture.CreateAnonymous<string>(),
                                                            fixture.CreateAnonymous<string>(),
                                                            0M, fixture.CreateAnonymous<decimal>()));
            firma.Calculate(fixture.CreateAnonymous<DateTime>());
            kreditorView = objectMapper.Map<AdresseBase, KreditorView>(firma);
            Assert.That(kreditorView, Is.Not.Null);
            Assert.That(kreditorView.Nummer, Is.EqualTo(firma.Nummer));
            Assert.That(kreditorView.Navn, Is.Not.Null);
            Assert.That(kreditorView.Navn, Is.EqualTo(firma.Navn));
            Assert.That(kreditorView.Adresse1, Is.Not.Null);
            Assert.That(kreditorView.Adresse1, Is.EqualTo(firma.Adresse1));
            Assert.That(kreditorView.Adresse2, Is.Not.Null);
            Assert.That(kreditorView.Adresse2, Is.EqualTo(firma.Adresse2));
            Assert.That(kreditorView.PostnummerBy, Is.Not.Null);
            Assert.That(kreditorView.PostnummerBy, Is.EqualTo(firma.PostnrBy));
            Assert.That(kreditorView.PrimærTelefon, Is.Not.Null);
            Assert.That(kreditorView.PrimærTelefon, Is.EqualTo(firma.Telefon1));
            Assert.That(kreditorView.SekundærTelefon, Is.Not.Null);
            Assert.That(kreditorView.SekundærTelefon, Is.EqualTo(firma.Telefon2));
            Assert.That(kreditorView.Mailadresse, Is.Not.Null);
            Assert.That(kreditorView.Mailadresse, Is.EqualTo(firma.Mailadresse));
            Assert.That(kreditorView.Betalingsbetingelse, Is.Not.Null);
            Assert.That(kreditorView.Saldo, Is.LessThan(0M));

            var andenAdresse = new OtherAddress(fixture.CreateAnonymous<int>(), fixture.CreateAnonymous<string>(),
                                                fixture.CreateAnonymous<Adressegruppe>());
            Assert.Throws<AutoMapperMappingException>(() => objectMapper.Map<AdresseBase, KreditorView>(andenAdresse));
        }

        /// <summary>
        /// Tester, at en betalingsbetingelse kan mappes til et betalingsbetingelseview.
        /// </summary>
        [Test]
        public void TestAtBetalingsbetingelseKanMappesTilBetalingsbetingelseView()
        {
            var fixture = new Fixture();

            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var betalingsbetingelse = fixture.CreateAnonymous<Betalingsbetingelse>();
            var betalingsbetingelseView = objectMapper.Map<Betalingsbetingelse, BetalingsbetingelseView>(betalingsbetingelse);
            Assert.That(betalingsbetingelseView, Is.Not.Null);
            Assert.That(betalingsbetingelseView.Nummer, Is.EqualTo(betalingsbetingelse.Nummer));
            Assert.That(betalingsbetingelseView.Navn, Is.Not.Null);
            Assert.That(betalingsbetingelseView.Navn, Is.EqualTo(betalingsbetingelse.Navn));
        }

        /// <summary>
        /// Tester, at et regnskab kan mappes til et regnskabslisteview.
        /// </summary>
        [Test]
        public void TestAtRegnskabKanMappesTilRegnskabslisteView()
        {
            var fixture = new Fixture();

            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var regnskab = fixture.CreateAnonymous<Regnskab>();
            regnskab.SætBrevhoved(fixture.CreateAnonymous<Brevhoved>());
            var regnskabslisteView = objectMapper.Map<Regnskab, RegnskabslisteView>(regnskab);
            Assert.That(regnskabslisteView, Is.Not.Null);
            Assert.That(regnskabslisteView.Nummer, Is.EqualTo(regnskab.Nummer));
            Assert.That(regnskabslisteView.Navn, Is.Not.Null);
            Assert.That(regnskabslisteView.Navn, Is.EqualTo(regnskab.Navn));
            Assert.That(regnskabslisteView.Brevhoved, Is.Not.Null);
            Assert.That(regnskabslisteView.Brevhoved.Nummer, Is.EqualTo(regnskab.Brevhoved.Nummer));
            Assert.That(regnskabslisteView.Brevhoved.Navn, Is.Not.Null);
            Assert.That(regnskabslisteView.Brevhoved.Navn, Is.EqualTo(regnskab.Brevhoved.Navn));
        }

        /// <summary>
        /// Tester, at en konto castet til en basiskonto kan mappes til et basiskontoview.
        /// </summary>
        [Test]
        public void TestAtKontoBaseForKontoKanMappesTilKontoBaseView()
        {
            var fixture = new Fixture();
            fixture.Inject<KontoBase>(fixture.CreateAnonymous<Konto>());

            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var kontoBase = fixture.CreateAnonymous<KontoBase>();
            Assert.That(kontoBase, Is.Not.Null);
            kontoBase.SætBeskrivelse(fixture.CreateAnonymous<string>());
            kontoBase.SætNote(fixture.CreateAnonymous<string>());

            var kontoBaseView = objectMapper.Map<KontoBase, KontoBaseView>(kontoBase);
            Assert.That(kontoBaseView, Is.Not.Null);
            Assert.That(kontoBaseView.Regnskab, Is.Not.Null);
            Assert.That(kontoBaseView.Kontonummer, Is.Not.Null);
            Assert.That(kontoBaseView.Kontonummer, Is.EqualTo(kontoBase.Kontonummer));
            Assert.That(kontoBaseView.Kontonavn, Is.Not.Null);
            Assert.That(kontoBaseView.Kontonavn, Is.EqualTo(kontoBase.Kontonavn));
            Assert.That(kontoBaseView.Beskrivelse, Is.Not.Null);
            Assert.That(kontoBaseView.Beskrivelse, Is.EqualTo(kontoBase.Beskrivelse));
            Assert.That(kontoBaseView.Notat, Is.Not.Null);
            Assert.That(kontoBaseView.Notat, Is.EqualTo(kontoBase.Note));
        }

        /// <summary>
        /// Tester, at en budgetkonto castet til en basiskonto kan mappes til et basiskontoview.
        /// </summary>
        [Test]
        public void TestAtKontoBaseForBudgetkontoKanMappesTilKontoBaseView()
        {
            var fixture = new Fixture();
            fixture.Inject<KontoBase>(fixture.CreateAnonymous<Budgetkonto>());

            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var kontoBase = fixture.CreateAnonymous<KontoBase>();
            Assert.That(kontoBase, Is.Not.Null);
            kontoBase.SætBeskrivelse(fixture.CreateAnonymous<string>());
            kontoBase.SætNote(fixture.CreateAnonymous<string>());

            var kontoBaseView = objectMapper.Map<KontoBase, KontoBaseView>(kontoBase);
            Assert.That(kontoBaseView, Is.Not.Null);
            Assert.That(kontoBaseView.Regnskab, Is.Not.Null);
            Assert.That(kontoBaseView.Kontonummer, Is.Not.Null);
            Assert.That(kontoBaseView.Kontonummer, Is.EqualTo(kontoBase.Kontonummer));
            Assert.That(kontoBaseView.Kontonavn, Is.Not.Null);
            Assert.That(kontoBaseView.Kontonavn, Is.EqualTo(kontoBase.Kontonavn));
            Assert.That(kontoBaseView.Beskrivelse, Is.Not.Null);
            Assert.That(kontoBaseView.Beskrivelse, Is.EqualTo(kontoBase.Beskrivelse));
            Assert.That(kontoBaseView.Notat, Is.Not.Null);
            Assert.That(kontoBaseView.Notat, Is.EqualTo(kontoBase.Note));
        }

        /// <summary>
        /// Tester, at Map kaster en IntranetSystemException, hvis basiskontoen ikke kan mappes til et basiskontoview.
        /// </summary>
        [Test]
        public void TestAtMapKasterIntranetSystemExceptionHvisKontoBaseIkkeKanMappesTilKontoBaseView()
        {
            var fixture = new Fixture();
            fixture.Inject<KontoBase>(fixture.CreateAnonymous<OtherKonto>());

            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var kontoBase = new OtherKonto(fixture.CreateAnonymous<Regnskab>(), fixture.CreateAnonymous<string>(),
                                           fixture.CreateAnonymous<string>());
            Assert.That(kontoBase, Is.Not.Null);
            kontoBase.SætBeskrivelse(fixture.CreateAnonymous<string>());
            kontoBase.SætNote(fixture.CreateAnonymous<string>());

            Assert.That(
                Assert.Throws<AutoMapperMappingException>(() => objectMapper.Map<KontoBase, KontoBaseView>(kontoBase)).
                    InnerException, Is.TypeOf(typeof (IntranetSystemException)));
        }

        /// <summary>
        /// Tester, at en konto kan mappes til et kontoplanview.
        /// </summary>
        [Test]
        public void TestAtKontoKanMappesTilKontoplanView()
        {
            var fixture = new Fixture();
            fixture.Inject(new DateTime(2010, 12, 31));

            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var konto = fixture.CreateAnonymous<Konto>();
            konto.SætBeskrivelse(fixture.CreateAnonymous<string>());
            konto.SætNote(fixture.CreateAnonymous<string>());
            konto.TilføjKreditoplysninger(new Kreditoplysninger(fixture.CreateAnonymous<DateTime>().Year,
                                                                fixture.CreateAnonymous<DateTime>().Month, 5000M));
            konto.TilføjBogføringslinje(new Bogføringslinje(fixture.CreateAnonymous<int>(),
                                                            fixture.CreateAnonymous<DateTime>(),
                                                            fixture.CreateAnonymous<string>(),
                                                            fixture.CreateAnonymous<string>(), 2500M, 0M));
            konto.TilføjBogføringslinje(new Bogføringslinje(fixture.CreateAnonymous<int>(),
                                                            fixture.CreateAnonymous<DateTime>(),
                                                            fixture.CreateAnonymous<string>(),
                                                            fixture.CreateAnonymous<string>(), 0M, 250M));
            konto.Calculate(fixture.CreateAnonymous<DateTime>());

            var kontoplanView = objectMapper.Map<Konto, KontoplanView>(konto);
            Assert.That(kontoplanView, Is.Not.Null);
            Assert.That(kontoplanView.Regnskab, Is.Not.Null);
            Assert.That(kontoplanView.Kontonummer, Is.Not.Null);
            Assert.That(kontoplanView.Kontonummer, Is.EqualTo(konto.Kontonummer));
            Assert.That(kontoplanView.Kontonavn, Is.Not.Null);
            Assert.That(kontoplanView.Kontonavn, Is.EqualTo(konto.Kontonavn));
            Assert.That(kontoplanView.Beskrivelse, Is.Not.Null);
            Assert.That(kontoplanView.Beskrivelse, Is.EqualTo(konto.Beskrivelse));
            Assert.That(kontoplanView.Notat, Is.Not.Null);
            Assert.That(kontoplanView.Notat, Is.EqualTo(konto.Note));
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
            var fixture = new Fixture();
            fixture.Inject(new DateTime(2010, 12, 31));

            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var konto = fixture.CreateAnonymous<Konto>();
            konto.SætBeskrivelse(fixture.CreateAnonymous<string>());
            konto.SætNote(fixture.CreateAnonymous<string>());
            konto.TilføjKreditoplysninger(new Kreditoplysninger(fixture.CreateAnonymous<DateTime>().Year,
                                                                fixture.CreateAnonymous<DateTime>().Month, 5000M));
            konto.TilføjBogføringslinje(new Bogføringslinje(fixture.CreateAnonymous<int>(),
                                                            fixture.CreateAnonymous<DateTime>(),
                                                            fixture.CreateAnonymous<string>(),
                                                            fixture.CreateAnonymous<string>(), 2500M, 0M));
            konto.TilføjBogføringslinje(new Bogføringslinje(fixture.CreateAnonymous<int>(),
                                                            fixture.CreateAnonymous<DateTime>(),
                                                            fixture.CreateAnonymous<string>(),
                                                            fixture.CreateAnonymous<string>(), 0M, 250M));
            konto.Calculate(fixture.CreateAnonymous<DateTime>());

            var kontoView = objectMapper.Map<Konto, KontoView>(konto);
            Assert.That(kontoView, Is.Not.Null);
            Assert.That(kontoView.Regnskab, Is.Not.Null);
            Assert.That(kontoView.Kontonummer, Is.Not.Null);
            Assert.That(kontoView.Kontonummer, Is.EqualTo(konto.Kontonummer));
            Assert.That(kontoView.Kontonavn, Is.Not.Null);
            Assert.That(kontoView.Kontonavn, Is.EqualTo(konto.Kontonavn));
            Assert.That(kontoView.Beskrivelse, Is.Not.Null);
            Assert.That(kontoView.Beskrivelse, Is.EqualTo(konto.Beskrivelse));
            Assert.That(kontoView.Notat, Is.Not.Null);
            Assert.That(kontoView.Notat, Is.EqualTo(konto.Note));
            Assert.That(kontoView.Kontogruppe, Is.Not.Null);
            Assert.That(kontoView.Kredit, Is.EqualTo(5000M));
            Assert.That(kontoView.Saldo, Is.EqualTo(2250M));
            Assert.That(kontoView.Disponibel, Is.EqualTo(7250M));
            Assert.That(kontoView.Kreditoplysninger, Is.Not.Null);
            Assert.That(kontoView.Kreditoplysninger.Count(), Is.EqualTo(1));
        }

        /// <summary>
        /// Tester, at kreditoplysninger kan mappes til et kreditoplysningerview.
        /// </summary>
        [Test]
        public void TestAtKreditoplysningerKanMappesTilKreditoplysningerView()
        {
            var fixture = new Fixture();

            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var kreditoplysninger = fixture.CreateAnonymous<Kreditoplysninger>();

            var kreditoplysningerView = objectMapper.Map<Kreditoplysninger, KreditoplysningerView>(kreditoplysninger);
            Assert.That(kreditoplysningerView, Is.Not.Null);
            Assert.That(kreditoplysningerView.År, Is.EqualTo(kreditoplysninger.År));
            Assert.That(kreditoplysningerView.Måned, Is.EqualTo(kreditoplysninger.Måned));
            Assert.That(kreditoplysningerView.Kredit, Is.EqualTo(kreditoplysninger.Kredit));
        }

        /// <summary>
        /// Tester, at en budgetkonto kan mappes til et budgetkontoplanview.
        /// </summary>
        [Test]
        public void TestAtBudgetkontoKanMappesTilBudgetkontoplanView()
        {
            var fixture = new Fixture();
            fixture.Inject(new DateTime(2010, 12, 31));

            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var budgetkonto = fixture.CreateAnonymous<Budgetkonto>();
            budgetkonto.SætBeskrivelse(fixture.CreateAnonymous<string>());
            budgetkonto.SætNote(fixture.CreateAnonymous<string>());
            budgetkonto.TilføjBudgetoplysninger(new Budgetoplysninger(fixture.CreateAnonymous<DateTime>().Year,
                                                                      fixture.CreateAnonymous<DateTime>().Month, 15000M,
                                                                      0M));
            budgetkonto.TilføjBogføringslinje(new Bogføringslinje(fixture.CreateAnonymous<int>(),
                                                                  fixture.CreateAnonymous<DateTime>(),
                                                                  fixture.CreateAnonymous<string>(),
                                                                  fixture.CreateAnonymous<string>(), 10000M, 0M));
            budgetkonto.TilføjBogføringslinje(new Bogføringslinje(fixture.CreateAnonymous<int>(),
                                                                  fixture.CreateAnonymous<DateTime>(),
                                                                  fixture.CreateAnonymous<string>(),
                                                                  fixture.CreateAnonymous<string>(), 4000M, 0M));
            budgetkonto.Calculate(fixture.CreateAnonymous<DateTime>());

            var bugetkontoplanView = objectMapper.Map<Budgetkonto, BudgetkontoplanView>(budgetkonto);
            Assert.That(bugetkontoplanView, Is.Not.Null);
            Assert.That(bugetkontoplanView.Regnskab, Is.Not.Null);
            Assert.That(bugetkontoplanView.Kontonummer, Is.Not.Null);
            Assert.That(bugetkontoplanView.Kontonummer, Is.EqualTo(budgetkonto.Kontonummer));
            Assert.That(bugetkontoplanView.Kontonavn, Is.Not.Null);
            Assert.That(bugetkontoplanView.Kontonavn, Is.EqualTo(budgetkonto.Kontonavn));
            Assert.That(bugetkontoplanView.Beskrivelse, Is.Not.Null);
            Assert.That(bugetkontoplanView.Beskrivelse, Is.EqualTo(budgetkonto.Beskrivelse));
            Assert.That(bugetkontoplanView.Notat, Is.Not.Null);
            Assert.That(bugetkontoplanView.Notat, Is.EqualTo(budgetkonto.Note));
            Assert.That(bugetkontoplanView.Budgetkontogruppe, Is.Not.Null);
            Assert.That(bugetkontoplanView.Budget, Is.EqualTo(15000M));
            Assert.That(bugetkontoplanView.Bogført, Is.EqualTo(14000M));
            Assert.That(bugetkontoplanView.Disponibel, Is.EqualTo(0M));
        }

        /// <summary>
        /// Tester, at en budgetkonto kan mappes til et budgetkontview.
        /// </summary>
        [Test]
        public void TestAtBudgetkontoKanMappesTilBudgetkontoView()
        {
            var fixture = new Fixture();
            fixture.Inject(new DateTime(2010, 12, 31));

            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var budgetkonto = fixture.CreateAnonymous<Budgetkonto>();
            budgetkonto.SætBeskrivelse(fixture.CreateAnonymous<string>());
            budgetkonto.SætNote(fixture.CreateAnonymous<string>());
            budgetkonto.TilføjBudgetoplysninger(new Budgetoplysninger(fixture.CreateAnonymous<DateTime>().Year,
                                                                      fixture.CreateAnonymous<DateTime>().Month, 15000M,
                                                                      0M));
            budgetkonto.TilføjBogføringslinje(new Bogføringslinje(fixture.CreateAnonymous<int>(),
                                                                  fixture.CreateAnonymous<DateTime>(),
                                                                  fixture.CreateAnonymous<string>(),
                                                                  fixture.CreateAnonymous<string>(), 10000M, 0M));
            budgetkonto.TilføjBogføringslinje(new Bogføringslinje(fixture.CreateAnonymous<int>(),
                                                                  fixture.CreateAnonymous<DateTime>(),
                                                                  fixture.CreateAnonymous<string>(),
                                                                  fixture.CreateAnonymous<string>(), 4000M, 0M));
            budgetkonto.Calculate(fixture.CreateAnonymous<DateTime>());

            var budgetkontoView = objectMapper.Map<Budgetkonto, BudgetkontoView>(budgetkonto);
            Assert.That(budgetkontoView, Is.Not.Null);
            Assert.That(budgetkontoView.Regnskab, Is.Not.Null);
            Assert.That(budgetkontoView.Kontonummer, Is.Not.Null);
            Assert.That(budgetkontoView.Kontonummer, Is.EqualTo(budgetkonto.Kontonummer));
            Assert.That(budgetkontoView.Kontonavn, Is.Not.Null);
            Assert.That(budgetkontoView.Kontonavn, Is.EqualTo(budgetkonto.Kontonavn));
            Assert.That(budgetkontoView.Beskrivelse, Is.Not.Null);
            Assert.That(budgetkontoView.Beskrivelse, Is.EqualTo(budgetkonto.Beskrivelse));
            Assert.That(budgetkontoView.Notat, Is.Not.Null);
            Assert.That(budgetkontoView.Notat, Is.EqualTo(budgetkonto.Note));
            Assert.That(budgetkontoView.Budgetkontogruppe, Is.Not.Null);
            Assert.That(budgetkontoView.Budget, Is.EqualTo(15000M));
            Assert.That(budgetkontoView.Bogført, Is.EqualTo(14000M));
            Assert.That(budgetkontoView.Disponibel, Is.EqualTo(0M));
            Assert.That(budgetkontoView.Budgetoplysninger, Is.Not.Null);
            Assert.That(budgetkontoView.Budgetoplysninger.Count(), Is.EqualTo(1));
        }

        /// <summary>
        /// Test, at budgetoplysninger kan mappes til et budgetoplysningerview.
        /// </summary>
        [Test]
        public void TestAtBudgetoplysningerKanMappesTilBudgetoplysningerView()
        {
            var fixture = new Fixture();
            fixture.Inject(new DateTime(2010, 12, 31));

            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var budgetkonto = fixture.CreateAnonymous<Budgetkonto>();
            budgetkonto.TilføjBudgetoplysninger(new Budgetoplysninger(fixture.CreateAnonymous<DateTime>().Year, fixture.CreateAnonymous<DateTime>().Month, 0M, 3000M));
            budgetkonto.TilføjBogføringslinje(new Bogføringslinje(fixture.CreateAnonymous<int>(),
                                                                  fixture.CreateAnonymous<DateTime>(),
                                                                  fixture.CreateAnonymous<string>(),
                                                                  fixture.CreateAnonymous<string>(), 0M, 2500M));
            budgetkonto.TilføjBogføringslinje(new Bogføringslinje(fixture.CreateAnonymous<int>(),
                                                                  fixture.CreateAnonymous<DateTime>(),
                                                                  fixture.CreateAnonymous<string>(),
                                                                  fixture.CreateAnonymous<string>(), 0M, 250M));
            budgetkonto.Calculate(fixture.CreateAnonymous<DateTime>());

            var budgetoplysninger = budgetkonto.Budgetoplysninger
                .SingleOrDefault(m =>
                                 m.År == fixture.CreateAnonymous<DateTime>().Year &&
                                 m.Måned == fixture.CreateAnonymous<DateTime>().Month);
            Assert.That(budgetoplysninger, Is.Not.Null);

            var budgetoplysningerView = objectMapper.Map<Budgetoplysninger, BudgetoplysningerView>(budgetoplysninger);
            Assert.That(budgetoplysningerView, Is.Not.Null);
            Assert.That(budgetoplysningerView.År, Is.EqualTo(fixture.CreateAnonymous<DateTime>().Year));
            Assert.That(budgetoplysningerView.Måned, Is.EqualTo(fixture.CreateAnonymous<DateTime>().Month));
            Assert.That(budgetoplysningerView.Budget, Is.EqualTo(-3000M));
            Assert.That(budgetoplysningerView.Bogført, Is.EqualTo(-2750M));
        }

        /// <summary>
        /// Test, at en bogføringslinje kan mappes til et bogføringslinjeview.
        /// </summary>
        [Test]
        public void TestAtBogføringslinjeKanMappesTilBogføringslinjeView()
        {
            var fixture = new Fixture();
            fixture.Inject(new DateTime(2010, 12, 31));

            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var konto = fixture.CreateAnonymous<Konto>();
            var budgetkonto = fixture.CreateAnonymous<Budgetkonto>();
            var adresse = fixture.CreateAnonymous<Person>();

            var bogføringslinje = new Bogføringslinje(fixture.CreateAnonymous<int>(),
                                                      fixture.CreateAnonymous<DateTime>(),
                                                      fixture.CreateAnonymous<string>(),
                                                      fixture.CreateAnonymous<string>(),
                                                      fixture.CreateAnonymous<decimal>(),
                                                      fixture.CreateAnonymous<decimal>());
            konto.TilføjBogføringslinje(bogføringslinje);
            budgetkonto.TilføjBogføringslinje(bogføringslinje);
            adresse.TilføjBogføringslinje(bogføringslinje);

            var bogføringslinjeView = objectMapper.Map<Bogføringslinje, BogføringslinjeView>(bogføringslinje);
            Assert.That(bogføringslinjeView, Is.Not.Null);

            Assert.That(bogføringslinjeView.Løbenr, Is.EqualTo(bogføringslinje.Løbenummer));
            Assert.That(bogføringslinjeView.Konto, Is.Not.Null);
            Assert.That(bogføringslinjeView.Budgetkonto, Is.Not.Null);
            Assert.That(bogføringslinjeView.Adressekonto, Is.Not.Null);
            Assert.That(bogføringslinjeView.Dato, Is.EqualTo(bogføringslinje.Dato));
            Assert.That(bogføringslinjeView.Bilag, Is.Not.Null);
            Assert.That(bogføringslinjeView.Bilag, Is.EqualTo(bogføringslinje.Bilag));
            Assert.That(bogføringslinjeView.Tekst, Is.Not.Null);
            Assert.That(bogføringslinjeView.Tekst, Is.EqualTo(bogføringslinje.Tekst));
            Assert.That(bogføringslinjeView.Debit, Is.EqualTo(bogføringslinje.Debit));
            Assert.That(bogføringslinjeView.Kredit, Is.EqualTo(bogføringslinje.Kredit));
        }

        /// <summary>
        /// Tester, at en bogføringsresultat kan mappes til et bogføringslinjeopretresponse.
        /// </summary>
        [Test]
        public void TestAtBogføringsresultatKanMappesTilBogføringslinjeOpretResponse()
        {
            var fixture = new Fixture();
            
            var regnskab = fixture.CreateAnonymous<Regnskab>();
            Assert.That(regnskab, Is.Not.Null);
            fixture.Inject(regnskab);
            
            var konto = fixture.CreateAnonymous<Konto>();
            Assert.That(konto, Is.Not.Null);
            fixture.Inject(konto);
            
            var budgetkonto = fixture.CreateAnonymous<Budgetkonto>();
            Assert.That(budgetkonto, Is.Not.Null);
            fixture.Inject(budgetkonto);

            var adressekonto = fixture.CreateAnonymous<Person>();
            Assert.That(adressekonto, Is.Not.Null);
            fixture.Inject(adressekonto);
            
            var bogføringslinje = fixture.CreateAnonymous<Bogføringslinje>();
            Assert.That(bogføringslinje, Is.Not.Null);
            konto.TilføjBogføringslinje(bogføringslinje);
            budgetkonto.TilføjBogføringslinje(bogføringslinje);
            adressekonto.TilføjBogføringslinje(bogføringslinje);
            fixture.Inject(bogføringslinje);

            fixture.Inject<IBogføringsresultat>(fixture.CreateAnonymous<Bogføringsresultat>());

            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var bogføringsresultat = fixture.CreateAnonymous<Bogføringsresultat>();
            Assert.That(bogføringsresultat, Is.Not.Null);

            var bogføringslinjeOpretResponse = objectMapper.Map<IBogføringsresultat, BogføringslinjeOpretResponse>(bogføringsresultat);
            Assert.That(bogføringslinjeOpretResponse, Is.Not.Null);
            Assert.That(bogføringslinjeOpretResponse.Løbenr, Is.EqualTo(bogføringslinje.Løbenummer));
            Assert.That(bogføringslinjeOpretResponse.Konto, Is.Not.Null);
            Assert.That(bogføringslinjeOpretResponse.Budgetkonto, Is.Not.Null);
            Assert.That(bogføringslinjeOpretResponse.Adressekonto, Is.Not.Null);
            Assert.That(bogføringslinjeOpretResponse.Dato, Is.EqualTo(bogføringslinje.Dato));
            Assert.That(bogføringslinjeOpretResponse.Bilag, Is.EqualTo(bogføringslinje.Bilag));
            Assert.That(bogføringslinjeOpretResponse.Tekst, Is.Not.Null);
            Assert.That(bogføringslinjeOpretResponse.Tekst, Is.EqualTo(bogføringslinje.Tekst));
            Assert.That(bogføringslinjeOpretResponse.Debit, Is.EqualTo(bogføringslinje.Debit));
            Assert.That(bogføringslinjeOpretResponse.Kredit, Is.EqualTo(bogføringslinje.Kredit));
            Assert.That(bogføringslinjeOpretResponse.Advarsler, Is.Not.Null);
        }

        /// <summary>
        /// Tester, at en bogføringsadvarsel kan mappes til et bogføringsresponse.
        /// </summary>
        [Test]
        public void TestAtBogføringsadvarselKanMappesTilBogføringsadvarselResponse()
        {
            var fixture = new Fixture();
            fixture.Inject<KontoBase>(fixture.CreateAnonymous<Konto>());
            fixture.Inject<IBogføringsadvarsel>(fixture.CreateAnonymous<Bogføringsadvarsel>());

            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var bogføringsadvarsel = fixture.CreateAnonymous<IBogføringsadvarsel>();
            Assert.That(bogføringsadvarsel, Is.Not.Null);

            var bogføringsadvarselReponse = objectMapper.Map<IBogføringsadvarsel, BogføringsadvarselResponse>(bogføringsadvarsel);
            Assert.That(bogføringsadvarselReponse, Is.Not.Null);
            Assert.That(bogføringsadvarselReponse.Advarsel, Is.Not.Null);
            Assert.That(bogføringsadvarselReponse.Advarsel, Is.EqualTo(bogføringsadvarsel.Advarsel));
            Assert.That(bogføringsadvarselReponse.Konto, Is.Not.Null);
            Assert.That(bogføringsadvarselReponse.Beløb, Is.EqualTo(bogføringsadvarsel.Beløb));
        }

        /// <summary>
        /// Tester, at en kontogruppe kan mappes til et kontogruppeview.
        /// </summary>
        [Test]
        public void TestAtKontogruppeKanMappesTilKontogruppeView()
        {
            var fixture = new Fixture();

            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            fixture.Inject(KontogruppeType.Aktiver);
            var kontogruppeAktiver = fixture.CreateAnonymous<Kontogruppe>();
            var kontogruppeAktiverView = objectMapper.Map<Kontogruppe, KontogruppeView>(kontogruppeAktiver);
            Assert.That(kontogruppeAktiverView, Is.Not.Null);
            Assert.That(kontogruppeAktiverView.Nummer, Is.EqualTo(kontogruppeAktiver.Nummer));
            Assert.That(kontogruppeAktiverView.Navn, Is.Not.Null);
            Assert.That(kontogruppeAktiverView.Navn, Is.EqualTo(kontogruppeAktiver.Navn));
            Assert.That(kontogruppeAktiverView.ErAktiver, Is.True);
            Assert.That(kontogruppeAktiverView.ErPassiver, Is.False);

            fixture.Inject(KontogruppeType.Passiver);
            var kontogruppePassiver = fixture.CreateAnonymous<Kontogruppe>();
            var kontogruppePassiverView = objectMapper.Map<Kontogruppe, KontogruppeView>(kontogruppePassiver);
            Assert.That(kontogruppePassiverView, Is.Not.Null);
            Assert.That(kontogruppePassiverView.Nummer, Is.EqualTo(kontogruppePassiver.Nummer));
            Assert.That(kontogruppePassiverView.Navn, Is.Not.Null);
            Assert.That(kontogruppePassiverView.Navn, Is.EqualTo(kontogruppePassiver.Navn));
            Assert.That(kontogruppePassiverView.ErAktiver, Is.False);
            Assert.That(kontogruppePassiverView.ErPassiver, Is.True);
        }

        /// <summary>
        /// Tester, at en budgetkontogruppe kan mappes til et bugdetkontogruppeview.
        /// </summary>
        [Test]
        public void TestAtBudgetkontogruppeKanMappesTilBudgetkontogruppeView()
        {
            var fixture = new Fixture();

            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var budgetkontogruppe = fixture.CreateAnonymous<Budgetkontogruppe>();
            var budgetkontogruppeView = objectMapper.Map<Budgetkontogruppe, BudgetkontogruppeView>(budgetkontogruppe);
            Assert.That(budgetkontogruppeView, Is.Not.Null);
            Assert.That(budgetkontogruppeView.Nummer, Is.EqualTo(budgetkontogruppe.Nummer));
            Assert.That(budgetkontogruppeView.Navn, Is.Not.Null);
            Assert.That(budgetkontogruppeView.Navn, Is.EqualTo(budgetkontogruppe.Navn));
        }

        /// <summary>
        /// Tester, at et brevhoved kan mappes til et view for en reference til et brevhoved.
        /// </summary>
        [Test]
        public void TestAtBrevhovedKanMappesTilBrevhovedreferenceView()
        {
            var fixture = new Fixture();

            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var brevhoved = fixture.CreateAnonymous<Brevhoved>();
            var brevhovedreferenceView = objectMapper.Map<Brevhoved, BrevhovedreferenceView>(brevhoved);
            Assert.That(brevhovedreferenceView, Is.Not.Null);
            Assert.That(brevhovedreferenceView.Nummer, Is.EqualTo(brevhoved.Nummer));
            Assert.That(brevhovedreferenceView.Navn, Is.Not.Null);
            Assert.That(brevhovedreferenceView.Navn, Is.EqualTo(brevhoved.Navn));
        }

        /// <summary>
        /// Tester, at et brevhoved kan mappes til et brevhovedview.
        /// </summary>
        [Test]
        public void TestAtBrevhovedKanMappesTilBrevhovedView()
        {
            var fixture = new Fixture();

            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var brevhoved = fixture.CreateAnonymous<Brevhoved>();
            var brevhovedView = objectMapper.Map<Brevhoved, BrevhovedView>(brevhoved);
            Assert.That(brevhovedView, Is.Not.Null);
            Assert.That(brevhovedView.Nummer, Is.EqualTo(brevhoved.Nummer));
            Assert.That(brevhovedView.Navn, Is.Not.Null);
            Assert.That(brevhovedView.Navn, Is.EqualTo(brevhoved.Navn));
            Assert.That(brevhovedView.Linje1, Is.EqualTo(brevhoved.Linje1));
            Assert.That(brevhovedView.Linje2, Is.EqualTo(brevhoved.Linje2));
            Assert.That(brevhovedView.Linje3, Is.EqualTo(brevhoved.Linje3));
            Assert.That(brevhovedView.Linje4, Is.EqualTo(brevhoved.Linje4));
            Assert.That(brevhovedView.Linje5, Is.EqualTo(brevhoved.Linje5));
            Assert.That(brevhovedView.Linje6, Is.EqualTo(brevhoved.Linje6));
            Assert.That(brevhovedView.Linje7, Is.EqualTo(brevhoved.Linje7));
            Assert.That(brevhovedView.CvrNr, Is.EqualTo(brevhoved.CvrNr));
        }
    }
}
