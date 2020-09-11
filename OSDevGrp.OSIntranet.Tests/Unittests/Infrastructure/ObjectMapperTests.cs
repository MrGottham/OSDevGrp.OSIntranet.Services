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
using OSDevGrp.OSIntranet.Domain.Interfaces.Fælles;
using OSDevGrp.OSIntranet.Domain.Interfaces.Kalender;
using OSDevGrp.OSIntranet.Domain.Kalender;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Resources;
using AutoFixture;
using Rhino.Mocks;
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

            var person = fixture.Create<Person>();
            person.SætTelefon(fixture.Create<string>(), fixture.Create<string>());
            var telefonlisteView = objectMapper.Map<AdresseBase, TelefonlisteView>(person);
            Assert.That(telefonlisteView, Is.Not.Null);
            Assert.That(telefonlisteView.Nummer, Is.EqualTo(person.Nummer));
            Assert.That(telefonlisteView.Navn, Is.Not.Null);
            Assert.That(telefonlisteView.Navn, Is.EqualTo(person.Navn));
            Assert.That(telefonlisteView.PrimærTelefon, Is.Not.Null);
            Assert.That(telefonlisteView.PrimærTelefon, Is.EqualTo(person.Telefon));
            Assert.That(telefonlisteView.SekundærTelefon, Is.Not.Null);
            Assert.That(telefonlisteView.SekundærTelefon, Is.EqualTo(person.Mobil));

            var firma = fixture.Create<Firma>();
            firma.SætTelefon(fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>());
            telefonlisteView = objectMapper.Map<AdresseBase, TelefonlisteView>(firma);
            Assert.That(telefonlisteView, Is.Not.Null);
            Assert.That(telefonlisteView.Nummer, Is.EqualTo(firma.Nummer));
            Assert.That(telefonlisteView.Navn, Is.Not.Null);
            Assert.That(telefonlisteView.Navn, Is.EqualTo(firma.Navn));
            Assert.That(telefonlisteView.PrimærTelefon, Is.Not.Null);
            Assert.That(telefonlisteView.PrimærTelefon, Is.EqualTo(firma.Telefon1));
            Assert.That(telefonlisteView.SekundærTelefon, Is.Not.Null);
            Assert.That(telefonlisteView.SekundærTelefon, Is.EqualTo(firma.Telefon2));

            var andenAdresse = new OtherAddress(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<Adressegruppe>());
            var exception = Assert.Throws<IntranetSystemException>(() => objectMapper.Map<AdresseBase, TelefonlisteView>(andenAdresse));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.CantAutoMapType, andenAdresse.GetType().Name)));
            Assert.That(exception.InnerException, Is.Null);
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

            var person = fixture.Create<Person>();
            person.SætTelefon(fixture.Create<string>(), fixture.Create<string>());
            person.TilføjBogføringslinje(new Bogføringslinje(fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<decimal>(), 0M));
            person.Calculate(fixture.Create<DateTime>());
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

            var firma = fixture.Create<Firma>();
            firma.SætTelefon(fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>());
            firma.TilføjBogføringslinje(new Bogføringslinje(fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<decimal>(), 0M));
            firma.Calculate(fixture.Create<DateTime>());
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

            var andenAdresse = new OtherAddress(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<Adressegruppe>());
            var exception = Assert.Throws<IntranetSystemException>(() => objectMapper.Map<AdresseBase, AdressekontolisteView>(andenAdresse));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.CantAutoMapType, andenAdresse.GetType().Name)));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at en basisadresse kan mappes til et adressekontoview.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestAtAdresseBaseMappesTilAdressekontoView(bool harBetalingsbetingelse)
        {
            var fixture = new Fixture();
            fixture.Inject(new DateTime(2010, 12, 31));

            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            Person person = fixture.Create<Person>();
            person.SætAdresseoplysninger(fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>());
            person.SætTelefon(fixture.Create<string>(), fixture.Create<string>());
            person.SætMailadresse(fixture.Create<string>());
            if (harBetalingsbetingelse)
            {
                person.SætBetalingsbetingelse(fixture.Create<Betalingsbetingelse>());
            }
            person.TilføjBogføringslinje(new Bogføringslinje(fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<decimal>(), 0M));
            person.Calculate(fixture.Create<DateTime>());
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
            Assert.That(adressekontoView.Betalingsbetingelse, harBetalingsbetingelse ? Is.Not.Null : Is.Null);
            Assert.That(adressekontoView.Saldo, Is.GreaterThan(0M));

            Firma firma = fixture.Create<Firma>();
            firma.SætAdresseoplysninger(fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>());
            firma.SætTelefon(fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>());
            firma.SætMailadresse(fixture.Create<string>());
            if (harBetalingsbetingelse)
            {
                firma.SætBetalingsbetingelse(fixture.Create<Betalingsbetingelse>());
            }
            firma.TilføjBogføringslinje(new Bogføringslinje(fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<decimal>(), 0M));
            firma.Calculate(fixture.Create<DateTime>());
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
            Assert.That(adressekontoView.Betalingsbetingelse, harBetalingsbetingelse ? Is.Not.Null : Is.Null);
            Assert.That(adressekontoView.Saldo, Is.GreaterThan(0M));

            var andenAdresse = new OtherAddress(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<Adressegruppe>());
            var exception = Assert.Throws<IntranetSystemException>(() => objectMapper.Map<AdresseBase, AdressekontoView>(andenAdresse));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.CantAutoMapType, andenAdresse.GetType().Name)));
            Assert.That(exception.InnerException, Is.Null);
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

            var person = fixture.Create<Person>();
            person.SætTelefon(fixture.Create<string>(), fixture.Create<string>());
            person.TilføjBogføringslinje(new Bogføringslinje(fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<decimal>(), 0M));
            person.Calculate(fixture.Create<DateTime>());
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

            var firma = fixture.Create<Firma>();
            firma.SætTelefon(fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>());
            firma.TilføjBogføringslinje(new Bogføringslinje(fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<decimal>(), 0M));
            firma.Calculate(fixture.Create<DateTime>());
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

            var andenAdresse = new OtherAddress(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<Adressegruppe>());
            var exception = Assert.Throws<IntranetSystemException>(() => objectMapper.Map<AdresseBase, DebitorlisteView>(andenAdresse));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.CantAutoMapType, andenAdresse.GetType().Name)));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at en basisadresse kan mappes til et debitorview.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestAtAdresseBaseMappesTilDebitorView(bool harBetalingsbetingelse)
        {
            var fixture = new Fixture();
            fixture.Inject(new DateTime(2010, 12, 31));

            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            Person person = fixture.Create<Person>();
            person.SætAdresseoplysninger(fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>());
            person.SætTelefon(fixture.Create<string>(), fixture.Create<string>());
            person.SætMailadresse(fixture.Create<string>());
            if (harBetalingsbetingelse)
            {
                person.SætBetalingsbetingelse(fixture.Create<Betalingsbetingelse>());
            }
            person.TilføjBogføringslinje(new Bogføringslinje(fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<decimal>(), 0M));
            person.Calculate(fixture.Create<DateTime>());
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
            Assert.That(debitorView.Betalingsbetingelse, harBetalingsbetingelse ? Is.Not.Null : Is.Null);
            Assert.That(debitorView.Saldo, Is.GreaterThan(0M));

            Firma firma = fixture.Create<Firma>();
            firma.SætAdresseoplysninger(fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>());
            firma.SætTelefon(fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>());
            firma.SætMailadresse(fixture.Create<string>());
            if (harBetalingsbetingelse)
            {
                firma.SætBetalingsbetingelse(fixture.Create<Betalingsbetingelse>());
            }
            firma.TilføjBogføringslinje(new Bogføringslinje(fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<decimal>(), 0M));
            firma.Calculate(fixture.Create<DateTime>());
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
            Assert.That(debitorView.Betalingsbetingelse, harBetalingsbetingelse ? Is.Not.Null : Is.Null);
            Assert.That(debitorView.Saldo, Is.GreaterThan(0M));

            var andenAdresse = new OtherAddress(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<Adressegruppe>());
            var exception = Assert.Throws<IntranetSystemException>(() => objectMapper.Map<AdresseBase, DebitorView>(andenAdresse));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.CantAutoMapType, andenAdresse.GetType().Name)));
            Assert.That(exception.InnerException, Is.Null);
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

            var person = fixture.Create<Person>();
            person.SætTelefon(fixture.Create<string>(), fixture.Create<string>());
            person.TilføjBogføringslinje(new Bogføringslinje(fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), 0M, fixture.Create<decimal>()));
            person.Calculate(fixture.Create<DateTime>());
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

            var firma = fixture.Create<Firma>();
            firma.SætTelefon(fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>());
            firma.TilføjBogføringslinje(new Bogføringslinje(fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), 0M, fixture.Create<decimal>()));
            firma.Calculate(fixture.Create<DateTime>());
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

            var andenAdresse = new OtherAddress(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<Adressegruppe>());
            var exception = Assert.Throws<IntranetSystemException>(() => objectMapper.Map<AdresseBase, KreditorlisteView>(andenAdresse));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.CantAutoMapType, andenAdresse.GetType().Name)));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at en basisadresse kan mappes til et kreditorview.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestAtAdresseBaseMappesTilKreditorView(bool harBetalingsbetingelse)
        {
            var fixture = new Fixture();
            fixture.Inject(new DateTime(2010, 12, 31));

            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            Person person = fixture.Create<Person>();
            person.SætAdresseoplysninger(fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>());
            person.SætTelefon(fixture.Create<string>(), fixture.Create<string>());
            person.SætMailadresse(fixture.Create<string>());
            if (harBetalingsbetingelse)
            {
                person.SætBetalingsbetingelse(fixture.Create<Betalingsbetingelse>());
            }
            person.TilføjBogføringslinje(new Bogføringslinje(fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), 0M, fixture.Create<decimal>()));
            person.Calculate(fixture.Create<DateTime>());
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
            Assert.That(kreditorView.Betalingsbetingelse, harBetalingsbetingelse ? Is.Not.Null : Is.Null);
            Assert.That(kreditorView.Saldo, Is.LessThan(0M));

            Firma firma = fixture.Create<Firma>();
            firma.SætAdresseoplysninger(fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>());
            firma.SætTelefon(fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>());
            firma.SætMailadresse(fixture.Create<string>());
            if (harBetalingsbetingelse)
            {
                firma.SætBetalingsbetingelse(fixture.Create<Betalingsbetingelse>());
            }
            firma.TilføjBogføringslinje(new Bogføringslinje(fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), 0M, fixture.Create<decimal>()));
            firma.Calculate(fixture.Create<DateTime>());
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
            Assert.That(kreditorView.Betalingsbetingelse, harBetalingsbetingelse ? Is.Not.Null : Is.Null);
            Assert.That(kreditorView.Saldo, Is.LessThan(0M));

            var andenAdresse = new OtherAddress(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<Adressegruppe>());
            var exception = Assert.Throws<IntranetSystemException>(() => objectMapper.Map<AdresseBase, KreditorView>(andenAdresse));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.CantAutoMapType, andenAdresse.GetType().Name)));
            Assert.That(exception.InnerException, Is.Null);
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

            var betalingsbetingelse = fixture.Create<Betalingsbetingelse>();
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

            var regnskab = fixture.Create<Regnskab>();
            regnskab.SætBrevhoved(fixture.Create<Brevhoved>());
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
            fixture.Inject<KontoBase>(fixture.Create<Konto>());

            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var kontoBase = fixture.Create<KontoBase>();
            Assert.That(kontoBase, Is.Not.Null);
            kontoBase.SætBeskrivelse(fixture.Create<string>());
            kontoBase.SætNote(fixture.Create<string>());

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
            fixture.Inject<KontoBase>(fixture.Create<Budgetkonto>());

            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var kontoBase = fixture.Create<KontoBase>();
            Assert.That(kontoBase, Is.Not.Null);
            kontoBase.SætBeskrivelse(fixture.Create<string>());
            kontoBase.SætNote(fixture.Create<string>());

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
            fixture.Inject<KontoBase>(fixture.Create<OtherKonto>());

            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var kontoBase = new OtherKonto(fixture.Create<Regnskab>(), fixture.Create<string>(), fixture.Create<string>());
            Assert.That(kontoBase, Is.Not.Null);
            kontoBase.SætBeskrivelse(fixture.Create<string>());
            kontoBase.SætNote(fixture.Create<string>());

            var exception = Assert.Throws<IntranetSystemException>(() => objectMapper.Map<KontoBase, KontoBaseView>(kontoBase));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.CantAutoMapType, kontoBase.GetType().Name)));
            Assert.That(exception.InnerException, Is.Null);
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

            var minLobenr = fixture.Create<int>();
            var konto = fixture.Create<Konto>();
            konto.SætBeskrivelse(fixture.Create<string>());
            konto.SætNote(fixture.Create<string>());
            konto.TilføjKreditoplysninger(new Kreditoplysninger(fixture.Create<DateTime>().Year, fixture.Create<DateTime>().Month, 5000M));
            konto.TilføjBogføringslinje(new Bogføringslinje(minLobenr + 1, fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), 2500M, 0M));
            konto.TilføjBogføringslinje(new Bogføringslinje(minLobenr, fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), 0M, 250M));
            konto.Calculate(fixture.Create<DateTime>());

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

            var minLobenr = fixture.Create<int>();
            var konto = fixture.Create<Konto>();
            konto.SætBeskrivelse(fixture.Create<string>());
            konto.SætNote(fixture.Create<string>());
            konto.TilføjKreditoplysninger(new Kreditoplysninger(fixture.Create<DateTime>().Year, fixture.Create<DateTime>().Month, 5000M));
            konto.TilføjBogføringslinje(new Bogføringslinje(minLobenr + 1, fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), 2500M, 0M));
            konto.TilføjBogføringslinje(new Bogføringslinje(minLobenr, fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), 0M, 250M));
            konto.Calculate(fixture.Create<DateTime>());

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

            var kreditoplysninger = fixture.Create<Kreditoplysninger>();

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

            var budgetThisMonth = fixture.Create<decimal>();
            var bogførtLinje1ThisMonth = fixture.Create<decimal>();
            var bogførtLinje2ThisMonth = fixture.Create<decimal>();

            var budgetLastMonth = fixture.Create<decimal>();
            var bogførtLinje1LastMonth = fixture.Create<decimal>();
            var bogførtLinje2LastMonth = fixture.Create<decimal>();

            var budgetPrevYearSameMonth = fixture.Create<decimal>();
            var bogførtLinje1PrevYearSameMonth = fixture.Create<decimal>();
            var bogførtLinje2PrevYearSameMonth = fixture.Create<decimal>();

            var budgetPrevYearPrevMonth = fixture.Create<decimal>();
            var bogførtLinje1PrevYearPrevMonth = fixture.Create<decimal>();
            var bogførtLinje2PrevYearPrevMonth = fixture.Create<decimal>();

            var minLobenr = fixture.Create<int>();
            var budgetkonto = fixture.Create<Budgetkonto>();
            budgetkonto.SætBeskrivelse(fixture.Create<string>());
            budgetkonto.SætNote(fixture.Create<string>());
            budgetkonto.TilføjBudgetoplysninger(new Budgetoplysninger(fixture.Create<DateTime>().Year, fixture.Create<DateTime>().Month, budgetThisMonth, 0M));
            budgetkonto.TilføjBudgetoplysninger(new Budgetoplysninger(fixture.Create<DateTime>().AddMonths(-1).Year, fixture.Create<DateTime>().AddMonths(-1).Month, budgetLastMonth, 0M));
            budgetkonto.TilføjBudgetoplysninger(new Budgetoplysninger(fixture.Create<DateTime>().AddYears(-1).Year, fixture.Create<DateTime>().AddYears(-1).Month, budgetPrevYearSameMonth, 0M));
            budgetkonto.TilføjBudgetoplysninger(new Budgetoplysninger(fixture.Create<DateTime>().AddYears(-1).AddMonths(-1).Year, fixture.Create<DateTime>().AddYears(-1).AddMonths(-1).Month, budgetPrevYearPrevMonth, 0M));
            budgetkonto.TilføjBogføringslinje(new Bogføringslinje(minLobenr + 7, fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), bogførtLinje1ThisMonth, 0M));
            budgetkonto.TilføjBogføringslinje(new Bogføringslinje(minLobenr + 6, fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), bogførtLinje2ThisMonth, 0M));
            budgetkonto.TilføjBogføringslinje(new Bogføringslinje(minLobenr + 5, fixture.Create<DateTime>().AddMonths(-1), fixture.Create<string>(), fixture.Create<string>(), bogførtLinje1LastMonth, 0M));
            budgetkonto.TilføjBogføringslinje(new Bogføringslinje(minLobenr + 4, fixture.Create<DateTime>().AddMonths(-1), fixture.Create<string>(), fixture.Create<string>(), bogførtLinje2LastMonth, 0M));
            budgetkonto.TilføjBogføringslinje(new Bogføringslinje(minLobenr + 3, fixture.Create<DateTime>().AddYears(-1), fixture.Create<string>(), fixture.Create<string>(), bogførtLinje1PrevYearSameMonth, 0M));
            budgetkonto.TilføjBogføringslinje(new Bogføringslinje(minLobenr + 2, fixture.Create<DateTime>().AddYears(-1), fixture.Create<string>(), fixture.Create<string>(), bogførtLinje2PrevYearSameMonth, 0M));
            budgetkonto.TilføjBogføringslinje(new Bogføringslinje(minLobenr + 1, fixture.Create<DateTime>().AddYears(-1).AddMonths(-1), fixture.Create<string>(), fixture.Create<string>(), bogførtLinje1PrevYearPrevMonth, 0M));
            budgetkonto.TilføjBogføringslinje(new Bogføringslinje(minLobenr, fixture.Create<DateTime>().AddYears(-1).AddMonths(-1), fixture.Create<string>(), fixture.Create<string>(), bogførtLinje2PrevYearPrevMonth, 0M));
            budgetkonto.Calculate(fixture.Create<DateTime>());

            var budgetkontoplanView = objectMapper.Map<Budgetkonto, BudgetkontoplanView>(budgetkonto);
            Assert.That(budgetkontoplanView, Is.Not.Null);
            Assert.That(budgetkontoplanView.Regnskab, Is.Not.Null);
            Assert.That(budgetkontoplanView.Kontonummer, Is.Not.Null);
            Assert.That(budgetkontoplanView.Kontonummer, Is.EqualTo(budgetkonto.Kontonummer));
            Assert.That(budgetkontoplanView.Kontonavn, Is.Not.Null);
            Assert.That(budgetkontoplanView.Kontonavn, Is.EqualTo(budgetkonto.Kontonavn));
            Assert.That(budgetkontoplanView.Beskrivelse, Is.Not.Null);
            Assert.That(budgetkontoplanView.Beskrivelse, Is.EqualTo(budgetkonto.Beskrivelse));
            Assert.That(budgetkontoplanView.Notat, Is.Not.Null);
            Assert.That(budgetkontoplanView.Notat, Is.EqualTo(budgetkonto.Note));
            Assert.That(budgetkontoplanView.Budgetkontogruppe, Is.Not.Null);
            Assert.That(budgetkontoplanView.Budget, Is.EqualTo(budgetThisMonth));
            Assert.That(budgetkontoplanView.BudgetSidsteMåned, Is.EqualTo(budgetLastMonth));
            Assert.That(budgetkontoplanView.BudgetÅrTilDato, Is.EqualTo(budgetThisMonth + budgetLastMonth));
            Assert.That(budgetkontoplanView.BudgetSidsteÅr, Is.EqualTo(budgetPrevYearSameMonth + budgetPrevYearPrevMonth));
            Assert.That(budgetkontoplanView.Bogført, Is.EqualTo(bogførtLinje1ThisMonth + bogførtLinje2ThisMonth));
            Assert.That(budgetkontoplanView.BogførtSidsteMåned, Is.EqualTo(bogførtLinje1LastMonth + bogførtLinje2LastMonth));
            Assert.That(budgetkontoplanView.BogførtÅrTilDato, Is.EqualTo(bogførtLinje1ThisMonth + bogførtLinje2ThisMonth + bogførtLinje1LastMonth + bogførtLinje2LastMonth));
            Assert.That(budgetkontoplanView.BogførtSidsteÅr, Is.EqualTo(bogførtLinje1PrevYearSameMonth + bogførtLinje2PrevYearSameMonth + bogførtLinje1PrevYearPrevMonth + bogførtLinje2PrevYearPrevMonth));
            Assert.That(budgetkontoplanView.Disponibel, Is.EqualTo(0M));
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

            var budgetThisMonth = fixture.Create<decimal>();
            var bogførtLinje1ThisMonth = fixture.Create<decimal>();
            var bogførtLinje2ThisMonth = fixture.Create<decimal>();

            var budgetLastMonth = fixture.Create<decimal>();
            var bogførtLinje1LastMonth = fixture.Create<decimal>();
            var bogførtLinje2LastMonth = fixture.Create<decimal>();

            var budgetPrevYearSameMonth = fixture.Create<decimal>();
            var bogførtLinje1PrevYearSameMonth = fixture.Create<decimal>();
            var bogførtLinje2PrevYearSameMonth = fixture.Create<decimal>();

            var budgetPrevYearPrevMonth = fixture.Create<decimal>();
            var bogførtLinje1PrevYearPrevMonth = fixture.Create<decimal>();
            var bogførtLinje2PrevYearPrevMonth = fixture.Create<decimal>();

            var minLobenr = fixture.Create<int>();
            var budgetkonto = fixture.Create<Budgetkonto>();
            budgetkonto.SætBeskrivelse(fixture.Create<string>());
            budgetkonto.SætNote(fixture.Create<string>());
            budgetkonto.TilføjBudgetoplysninger(new Budgetoplysninger(fixture.Create<DateTime>().Year, fixture.Create<DateTime>().Month, budgetThisMonth, 0M));
            budgetkonto.TilføjBudgetoplysninger(new Budgetoplysninger(fixture.Create<DateTime>().AddMonths(-1).Year, fixture.Create<DateTime>().AddMonths(-1).Month, budgetLastMonth, 0M));
            budgetkonto.TilføjBudgetoplysninger(new Budgetoplysninger(fixture.Create<DateTime>().AddYears(-1).Year, fixture.Create<DateTime>().AddYears(-1).Month, budgetPrevYearSameMonth, 0M));
            budgetkonto.TilføjBudgetoplysninger(new Budgetoplysninger(fixture.Create<DateTime>().AddYears(-1).AddMonths(-1).Year, fixture.Create<DateTime>().AddYears(-1).AddMonths(-1).Month, budgetPrevYearPrevMonth, 0M));
            budgetkonto.TilføjBogføringslinje(new Bogføringslinje(minLobenr + 7, fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), bogførtLinje1ThisMonth, 0M));
            budgetkonto.TilføjBogføringslinje(new Bogføringslinje(minLobenr + 6, fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), bogførtLinje2ThisMonth, 0M));
            budgetkonto.TilføjBogføringslinje(new Bogføringslinje(minLobenr + 5, fixture.Create<DateTime>().AddMonths(-1), fixture.Create<string>(), fixture.Create<string>(), bogførtLinje1LastMonth, 0M));
            budgetkonto.TilføjBogføringslinje(new Bogføringslinje(minLobenr + 4, fixture.Create<DateTime>().AddMonths(-1), fixture.Create<string>(), fixture.Create<string>(), bogførtLinje2LastMonth, 0M));
            budgetkonto.TilføjBogføringslinje(new Bogføringslinje(minLobenr + 3, fixture.Create<DateTime>().AddYears(-1), fixture.Create<string>(), fixture.Create<string>(), bogførtLinje1PrevYearSameMonth, 0M));
            budgetkonto.TilføjBogføringslinje(new Bogføringslinje(minLobenr + 2, fixture.Create<DateTime>().AddYears(-1), fixture.Create<string>(), fixture.Create<string>(), bogførtLinje2PrevYearSameMonth, 0M));
            budgetkonto.TilføjBogføringslinje(new Bogføringslinje(minLobenr + 1, fixture.Create<DateTime>().AddYears(-1).AddMonths(-1), fixture.Create<string>(), fixture.Create<string>(), bogførtLinje1PrevYearPrevMonth, 0M));
            budgetkonto.TilføjBogføringslinje(new Bogføringslinje(minLobenr, fixture.Create<DateTime>().AddYears(-1).AddMonths(-1), fixture.Create<string>(), fixture.Create<string>(), bogførtLinje2PrevYearPrevMonth, 0M));
            budgetkonto.Calculate(fixture.Create<DateTime>());

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
            Assert.That(budgetkontoView.Budget, Is.EqualTo(budgetThisMonth));
            Assert.That(budgetkontoView.BudgetSidsteMåned, Is.EqualTo(budgetLastMonth));
            Assert.That(budgetkontoView.BudgetÅrTilDato, Is.EqualTo(budgetThisMonth + budgetLastMonth));
            Assert.That(budgetkontoView.BudgetSidsteÅr, Is.EqualTo(budgetPrevYearSameMonth + budgetPrevYearPrevMonth));
            Assert.That(budgetkontoView.Bogført, Is.EqualTo(bogførtLinje1ThisMonth + bogførtLinje2ThisMonth));
            Assert.That(budgetkontoView.BogførtSidsteMåned, Is.EqualTo(bogførtLinje1LastMonth + bogførtLinje2LastMonth));
            Assert.That(budgetkontoView.BogførtÅrTilDato, Is.EqualTo(bogførtLinje1ThisMonth + bogførtLinje2ThisMonth + bogførtLinje1LastMonth + bogførtLinje2LastMonth));
            Assert.That(budgetkontoView.BogførtSidsteÅr, Is.EqualTo(bogførtLinje1PrevYearSameMonth + bogførtLinje2PrevYearSameMonth + bogførtLinje1PrevYearPrevMonth + bogførtLinje2PrevYearPrevMonth));
            Assert.That(budgetkontoView.Disponibel, Is.EqualTo(0M));
            Assert.That(budgetkontoView.Budgetoplysninger, Is.Not.Null);
            Assert.That(budgetkontoView.Budgetoplysninger.Count(), Is.EqualTo(4));
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

            var minLobenr = fixture.Create<int>();
            var budgetkonto = fixture.Create<Budgetkonto>();
            budgetkonto.TilføjBudgetoplysninger(new Budgetoplysninger(fixture.Create<DateTime>().Year, fixture.Create<DateTime>().Month, 0M, 3000M));
            budgetkonto.TilføjBogføringslinje(new Bogføringslinje(minLobenr + 1, fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), 0M, 2500M));
            budgetkonto.TilføjBogføringslinje(new Bogføringslinje(minLobenr, fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), 0M, 250M));
            budgetkonto.Calculate(fixture.Create<DateTime>());

            var budgetoplysninger = budgetkonto.Budgetoplysninger.SingleOrDefault(m => m.År == fixture.Create<DateTime>().Year && m.Måned == fixture.Create<DateTime>().Month);
            Assert.That(budgetoplysninger, Is.Not.Null);

            var budgetoplysningerView = objectMapper.Map<Budgetoplysninger, BudgetoplysningerView>(budgetoplysninger);
            Assert.That(budgetoplysningerView, Is.Not.Null);
            Assert.That(budgetoplysningerView.År, Is.EqualTo(fixture.Create<DateTime>().Year));
            Assert.That(budgetoplysningerView.Måned, Is.EqualTo(fixture.Create<DateTime>().Month));
            Assert.That(budgetoplysningerView.Budget, Is.EqualTo(-3000M));
            Assert.That(budgetoplysningerView.Bogført, Is.EqualTo(-2750M));
        }

        /// <summary>
        /// Test, at en bogføringslinje kan mappes til et bogføringslinjeview.
        /// </summary>
        [Test]
        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void TestAtBogføringslinjeKanMappesTilBogføringslinjeView(bool harBudgetkonto, bool harAdressekonto)
        {
            var fixture = new Fixture();
            fixture.Inject(new DateTime(2010, 12, 31));

            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var konto = fixture.Create<Konto>();
            var budgetkonto = fixture.Create<Budgetkonto>();
            var adresse = fixture.Create<Person>();

            var bogføringslinje = new Bogføringslinje(fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<decimal>(), fixture.Create<decimal>());
            konto.TilføjBogføringslinje(bogføringslinje);
            if (harBudgetkonto)
            {
                budgetkonto.TilføjBogføringslinje(bogføringslinje);
            }
            if (harAdressekonto)
            {
                adresse.TilføjBogføringslinje(bogføringslinje);
            }

            var bogføringslinjeView = objectMapper.Map<Bogføringslinje, BogføringslinjeView>(bogføringslinje);
            Assert.That(bogføringslinjeView, Is.Not.Null);

            Assert.That(bogføringslinjeView.Løbenr, Is.EqualTo(bogføringslinje.Løbenummer));
            Assert.That(bogføringslinjeView.Konto, Is.Not.Null);
            Assert.That(bogføringslinjeView.Budgetkonto, harBudgetkonto ? Is.Not.Null : Is.Null);
            Assert.That(bogføringslinjeView.Adressekonto, harAdressekonto ? Is.Not.Null : Is.Null);
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
        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void TestAtBogføringsresultatKanMappesTilBogføringslinjeOpretResponse(bool harBudgetkonto, bool harAdressekonto)
        {
            var fixture = new Fixture();
            
            var regnskab = fixture.Create<Regnskab>();
            Assert.That(regnskab, Is.Not.Null);
            fixture.Inject(regnskab);
            
            var konto = fixture.Create<Konto>();
            Assert.That(konto, Is.Not.Null);
            fixture.Inject(konto);
            
            var budgetkonto = fixture.Create<Budgetkonto>();
            Assert.That(budgetkonto, Is.Not.Null);
            fixture.Inject(budgetkonto);

            var adressekonto = fixture.Create<Person>();
            Assert.That(adressekonto, Is.Not.Null);
            fixture.Inject(adressekonto);
            
            var bogføringslinje = fixture.Create<Bogføringslinje>();
            Assert.That(bogføringslinje, Is.Not.Null);
            konto.TilføjBogføringslinje(bogføringslinje);
            if (harBudgetkonto)
            {
                budgetkonto.TilføjBogføringslinje(bogføringslinje);
            }
            if (harAdressekonto)
            {
                adressekonto.TilføjBogføringslinje(bogføringslinje);
            }
            fixture.Inject(bogføringslinje);

            fixture.Inject<IBogføringsresultat>(fixture.Create<Bogføringsresultat>());

            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var bogføringsresultat = fixture.Create<Bogføringsresultat>();
            Assert.That(bogføringsresultat, Is.Not.Null);

            var bogføringslinjeOpretResponse = objectMapper.Map<IBogføringsresultat, BogføringslinjeOpretResponse>(bogføringsresultat);
            Assert.That(bogføringslinjeOpretResponse, Is.Not.Null);
            Assert.That(bogføringslinjeOpretResponse.Løbenr, Is.EqualTo(bogføringslinje.Løbenummer));
            Assert.That(bogføringslinjeOpretResponse.Konto, Is.Not.Null);
            Assert.That(bogføringslinjeOpretResponse.Budgetkonto, harBudgetkonto ? Is.Not.Null : Is.Null);
            Assert.That(bogføringslinjeOpretResponse.Adressekonto, harAdressekonto ? Is.Not.Null : Is.Null);
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
            fixture.Inject<KontoBase>(fixture.Create<Konto>());
            fixture.Inject<IBogføringsadvarsel>(fixture.Create<Bogføringsadvarsel>());

            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var bogføringsadvarsel = fixture.Create<IBogføringsadvarsel>();
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
            var kontogruppeAktiver = fixture.Create<Kontogruppe>();
            var kontogruppeAktiverView = objectMapper.Map<Kontogruppe, KontogruppeView>(kontogruppeAktiver);
            Assert.That(kontogruppeAktiverView, Is.Not.Null);
            Assert.That(kontogruppeAktiverView.Nummer, Is.EqualTo(kontogruppeAktiver.Nummer));
            Assert.That(kontogruppeAktiverView.Navn, Is.Not.Null);
            Assert.That(kontogruppeAktiverView.Navn, Is.EqualTo(kontogruppeAktiver.Navn));
            Assert.That(kontogruppeAktiverView.ErAktiver, Is.True);
            Assert.That(kontogruppeAktiverView.ErPassiver, Is.False);

            fixture.Inject(KontogruppeType.Passiver);
            var kontogruppePassiver = fixture.Create<Kontogruppe>();
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

            var budgetkontogruppe = fixture.Create<Budgetkontogruppe>();
            var budgetkontogruppeView = objectMapper.Map<Budgetkontogruppe, BudgetkontogruppeView>(budgetkontogruppe);
            Assert.That(budgetkontogruppeView, Is.Not.Null);
            Assert.That(budgetkontogruppeView.Nummer, Is.EqualTo(budgetkontogruppe.Nummer));
            Assert.That(budgetkontogruppeView.Navn, Is.Not.Null);
            Assert.That(budgetkontogruppeView.Navn, Is.EqualTo(budgetkontogruppe.Navn));
        }

        /// <summary>
        /// Tester, at en brugeraftale kan mappes til et kalenderbrugeraftaleview.
        /// </summary>
        [Test]
        public void TestAtBrugeraftaleKanMappesTilKalenderbrugerAftaleView()
        {
            var fixture = new Fixture();
            fixture.Inject(DateTime.Now);
            fixture.Inject(MockRepository.GenerateMock<ISystem>());
            fixture.Inject(MockRepository.GenerateMock<IBruger>());
            fixture.Inject(MockRepository.GenerateMock<IBrugeraftale>());

            var aftale = MockRepository.GenerateMock<IAftale>();
            aftale.Expect(m => m.System)
                .Return(fixture.Create<ISystem>())
                .Repeat.Any();
            aftale.Expect(m => m.Id)
                .Return(fixture.Create<int>())
                .Repeat.Any();
            aftale.Expect(m => m.FraTidspunkt)
                .Return(fixture.Create<DateTime>())
                .Repeat.Any();
            aftale.Expect(m => m.TilTidspunkt)
                .Return(fixture.Create<DateTime>().AddMinutes(15))
                .Repeat.Any();
            aftale.Expect(m => m.Emne)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            aftale.Expect(m => m.Notat)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            aftale.Expect(m => m.Deltagere)
                .Return(fixture.CreateMany<IBrugeraftale>(3))
                .Repeat.Any();
            fixture.Inject(aftale);

            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var brugeraftale = fixture.Create<Brugeraftale>();
            var kalenderbrugerAftaleView = objectMapper.Map<IBrugeraftale, KalenderbrugerAftaleView>(brugeraftale);
            Assert.That(kalenderbrugerAftaleView, Is.Not.Null);
            Assert.That(kalenderbrugerAftaleView.System, Is.Not.Null);
            Assert.That(kalenderbrugerAftaleView.Id, Is.EqualTo(brugeraftale.Aftale.Id));
            Assert.That(kalenderbrugerAftaleView.FraTidspunkt, Is.EqualTo(brugeraftale.Aftale.FraTidspunkt));
            Assert.That(kalenderbrugerAftaleView.TilTidspunkt, Is.EqualTo(brugeraftale.Aftale.TilTidspunkt));
            Assert.That(kalenderbrugerAftaleView.Emne, Is.Not.Null);
            Assert.That(kalenderbrugerAftaleView.Emne, Is.EqualTo(brugeraftale.Aftale.Emne));
            Assert.That(kalenderbrugerAftaleView.Notat, Is.Not.Null);
            Assert.That(kalenderbrugerAftaleView.Notat, Is.EqualTo(brugeraftale.Aftale.Notat));
            Assert.That(kalenderbrugerAftaleView.Offentlig, Is.EqualTo(brugeraftale.Offentligtgørelse));
            Assert.That(kalenderbrugerAftaleView.Privat, Is.EqualTo(brugeraftale.Privat));
            Assert.That(kalenderbrugerAftaleView.Alarm, Is.EqualTo(brugeraftale.Alarm));
            Assert.That(kalenderbrugerAftaleView.Udført, Is.EqualTo(brugeraftale.Udført));
            Assert.That(kalenderbrugerAftaleView.Eksporteres, Is.EqualTo(brugeraftale.Eksporter));
            Assert.That(kalenderbrugerAftaleView.Eksporteret, Is.EqualTo(brugeraftale.Eksporteret));
            Assert.That(kalenderbrugerAftaleView.Deltagere, Is.Not.Null);
        }

        /// <summary>
        /// Tester, at en brugeraftale kan mappes til et kalenderbrugerview.
        /// </summary>
        [Test]
        public void TestAtBrugeraftaleKanMappesTilKalenderbrugerView()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<ISystem>());
            fixture.Inject(MockRepository.GenerateMock<IAftale>());

            var bruger = MockRepository.GenerateMock<IBruger>();
            bruger.Expect(m => m.System)
                .Return(fixture.Create<ISystem>())
                .Repeat.Any();
            bruger.Expect(m => m.Id)
                .Return(fixture.Create<int>())
                .Repeat.Any();
            bruger.Expect(m => m.Initialer)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            bruger.Expect(m => m.Navn)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            fixture.Inject(bruger);

            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var brugeraftale = fixture.Create<Brugeraftale>();
            var kalenderbrugerView = objectMapper.Map<IBrugeraftale, KalenderbrugerView>(brugeraftale);
            Assert.That(kalenderbrugerView, Is.Not.Null);
            Assert.That(kalenderbrugerView.System, Is.Not.Null);
            Assert.That(kalenderbrugerView.Id, Is.EqualTo(brugeraftale.Bruger.Id));
            Assert.That(kalenderbrugerView.Initialer, Is.Not.Null);
            Assert.That(kalenderbrugerView.Initialer, Is.EqualTo(brugeraftale.Bruger.Initialer));
            Assert.That(kalenderbrugerView.Navn, Is.Not.Null);
            Assert.That(kalenderbrugerView.Navn, Is.EqualTo(brugeraftale.Bruger.Navn));
        }

        /// <summary>
        /// Tester, at en kalenderbruger kan mappes til et kalenderbrugerview.
        /// </summary>
        [Test]
        public void TestAtBrugerKanMappesTilKalenderbrugerView()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<ISystem>());

            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var bruger = fixture.Create<Bruger>();
            var kalenderbrugerView = objectMapper.Map<IBruger, KalenderbrugerView>(bruger);
            Assert.That(kalenderbrugerView, Is.Not.Null);
            Assert.That(kalenderbrugerView.System, Is.Not.Null);
            Assert.That(kalenderbrugerView.Id, Is.EqualTo( bruger.Id));
            Assert.That(kalenderbrugerView.Initialer, Is.Not.Null);
            Assert.That(kalenderbrugerView.Initialer, Is.EqualTo(bruger.Initialer));
            Assert.That(kalenderbrugerView.Navn, Is.Not.Null);
            Assert.That(kalenderbrugerView.Navn, Is.EqualTo(bruger.Navn));
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

            var brevhoved = fixture.Create<Brevhoved>();
            var brevhovedreferenceView = objectMapper.Map<Brevhoved, BrevhovedreferenceView>(brevhoved);
            Assert.That(brevhovedreferenceView, Is.Not.Null);
            Assert.That(brevhovedreferenceView.Nummer, Is.EqualTo(brevhoved.Nummer));
            Assert.That(brevhovedreferenceView.Navn, Is.Not.Null);
            Assert.That(brevhovedreferenceView.Navn, Is.EqualTo(brevhoved.Navn));
        }

        /// <summary>
        /// Tester, at et system kan mappes til et systemview.
        /// </summary>
        [Test]
        public void TestAtSystemKanMappesTilSystemView()
        {
            var fixture = new Fixture();

            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var system = fixture.Create<OSIntranet.Domain.Fælles.System>();
            var systemView = objectMapper.Map<ISystem, SystemView>(system);
            Assert.That(systemView, Is.Not.Null);
            Assert.That(systemView.Nummer, Is.EqualTo(system.Nummer));
            Assert.That(systemView.Titel, Is.Not.Null);
            Assert.That(systemView.Titel, Is.EqualTo(system.Titel));
            Assert.That(systemView.Kalender, Is.EqualTo(system.Kalender));
        }
    }
}
