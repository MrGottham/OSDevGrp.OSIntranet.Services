using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Enums;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Views;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories
{
    /// <summary>
    /// Tester domæneobjekt bygger.
    /// </summary>
    [TestFixture]
    public class DomainObjectBuilderTest
    {
        /// <summary>
        /// Tester, at domæneobjekt byggeren kan initieres.
        /// </summary>
        [Test]
        public void TestAtDomainObjectBuilderInitieres()
        {
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);
        }

        /// <summary>
        /// Tester, at GetAdresseBaseCallback kaster en ArgumentNullException, hvis callbackmetoden er null.
        /// </summary>
        [Test]
        public void TestAtGetAdresseBaseCallbackKasterArgumentNullExceptionHvisCallbackMethodErNull()
        {
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => domainObjectBuilder.GetAdresseBaseCallback = null);
        }

        /// <summary>
        /// Tester, at GetAdressegruppeCallback kaster en ArgumentNullException, hvis callbackmetoden er null.
        /// </summary>
        [Test]
        public void TestAtGetAdressegruppeCallbackKasterArgumentNullExceptionHvisCallbackMethodErNull()
        {
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => domainObjectBuilder.GetAdressegruppeCallback = null);
        }

        /// <summary>
        /// Tester, at GetBetalingsbetingelseCallback kaster en ArgumentNullException, hvis callbackmetoden er null.
        /// </summary>
        [Test]
        public void TestAtGetBetalingsbetingelseCallbackKasterArgumentNullExceptionHvisCallbackMethodErNull()
        {
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => domainObjectBuilder.GetBetalingsbetingelseCallback = null);
        }

        /// <summary>
        /// Tester, at GetKontogruppeCallback kaster en ArgumentNullException, hvis callbackmetoden er null.
        /// </summary>
        [Test]
        public void TestAtGetKontogruppeCallbackKasterArgumentNullExceptionHvisCallbackMethodErNull()
        {
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => domainObjectBuilder.GetKontogruppeCallback = null);
        }

        /// <summary>
        /// Tester, at GetBudgetkontogruppeCallback kaster en ArgumentNullException, hvis callbackmetoden er null.
        /// </summary>
        [Test]
        public void TestAtGetBudgetkontogruppeCallbackKasterArgumentNullExceptionHvisCallbackMethodErNull()
        {
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => domainObjectBuilder.GetBudgetkontogruppeCallback = null);
        }

        /// <summary>
        /// Tester, at GetBrevhovedCallback kaster en ArgumentNullException, hvis callbackmetoden er null.
        /// </summary>
        [Test]
        public void TestAtGetBrevhovedCallbackKasterArgumentNullExceptionHvisCallbackMethodErNull()
        {
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => domainObjectBuilder.GetBrevhovedCallback = null);
        }

        /// <summary>
        /// Tester, at SætAdresser kaster en ArgumentNullException, hvis adresser er null.
        /// </summary>
        [Test]
        public void TestAtSætAdresserKasterArgumentNullExceptionHvisAdressegrupperErNull()
        {
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => domainObjectBuilder.SætAdresser(null));
        }

        /// <summary>
        /// Tester, at SætAdressegrupper kaster en ArgumentNullException, hvis adressegrupper er null.
        /// </summary>
        [Test]
        public void TestAtSætAdressegrupperKasterArgumentNullExceptionHvisAdressegrupperErNull()
        {
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => domainObjectBuilder.SætAdressegrupper(null));
        }

        /// <summary>
        /// Tester, at SætBetalingsbetingelser kaster en ArgumentNullException, hvis betalingsbetingelser er null.
        /// </summary>
        [Test]
        public void TestAtSætBetalingsbetingelserKasterArgumentNullExceptionHvisBetalingsbetingelserErNull()
        {
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => domainObjectBuilder.SætBetalingsbetingelser(null));
        }

        /// <summary>
        /// Tester, at Build kaster en ArgumentNullException, hvis objektet er null.
        /// </summary>
        [Test]
        public void TestAtBuildKasterArgumentNullExceptionHvisObjectErNull()
        {
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => domainObjectBuilder.Build<PersonView, Person>(null));
        }

        /// <summary>
        /// Tester, at Build kaster en IntranetRepositoryException, hvis objektet er null.
        /// </summary>
        [Test]
        public void TestAtBuildKasterIntranetRepositoryExceptionVedAutoMapperMappingException()
        {
            var fixture = new Fixture();
            fixture.Inject<FirmaView>(null);
            fixture.Inject<IEnumerable<BogføringslinjeView>>(new List<BogføringslinjeView>());
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            Assert.Throws<IntranetRepositoryException>(() => domainObjectBuilder.Build<PersonView, Postnummer>(fixture.CreateAnonymous<PersonView>()));
        }

        /// <summary>
        /// Tester, at Build bygger person fra et view.
        /// </summary>
        [Test]
        public void TestAtBuildByggerAdresseBaseFraPersonView()
        {
            var fixture = new Fixture();
            var adressegrupper = fixture.CreateMany<Adressegruppe>(3).ToList();
            var betalingsbetingelser = fixture.CreateMany<Betalingsbetingelse>(3).ToList();
            var firmaer = fixture.CreateMany<Firma>(3).ToList();
            fixture.Inject(new AdressegruppeView
                               {
                                   Nummer = adressegrupper.ElementAt(0).Nummer,
                                   Navn = adressegrupper.ElementAt(0).Navn,
                                   AdressegruppeOswebdb = adressegrupper.ElementAt(0).AdressegruppeOswebdb
                               });
            fixture.Inject(new BetalingsbetingelseView
                               {
                                   Nummer = betalingsbetingelser.ElementAt(0).Nummer,
                                   Navn = betalingsbetingelser.ElementAt(0).Navn,
                               });
            fixture.Inject(new AdressereferenceView
                               {
                                   Nummer = firmaer.ElementAt(0).Nummer,
                                   Navn = firmaer.ElementAt(0).Navn
                               });
            fixture.Inject<IEnumerable<BogføringslinjeView>>(new List<BogføringslinjeView>());
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            domainObjectBuilder.SætAdressegrupper(adressegrupper);
            domainObjectBuilder.SætBetalingsbetingelser(betalingsbetingelser);
            domainObjectBuilder.SætAdresser(firmaer);

            var view = fixture.CreateAnonymous<PersonView>();
            var person = (Person) domainObjectBuilder.Build<PersonView, AdresseBase>(view);
            Assert.That(person, Is.Not.Null);
            Assert.That(person.Nummer, Is.EqualTo(view.Nummer));
            Assert.That(person.Navn, Is.Not.Null);
            Assert.That(person.Navn, Is.EqualTo(view.Navn));
            Assert.That(person.Adresse1, Is.EqualTo(view.Adresse1));
            Assert.That(person.Adresse2, Is.EqualTo(view.Adresse2));
            Assert.That(person.PostnrBy, Is.EqualTo(view.PostnummerBy));
            Assert.That(person.Telefon, Is.EqualTo(view.Telefon));
            Assert.That(person.Mobil, Is.EqualTo(view.Mobil));
            Assert.That(person.Fødselsdato, Is.EqualTo(view.Fødselsdato));
            Assert.That(person.Adressegruppe, Is.Not.Null);
            Assert.That(person.Adressegruppe.Nummer, Is.EqualTo(view.Adressegruppe.Nummer));
            Assert.That(person.Bekendtskab, Is.EqualTo(view.Bekendtskab));
            Assert.That(person.Mailadresse, Is.EqualTo(view.Mailadresse));
            Assert.That(person.Webadresse, Is.EqualTo(view.Webadresse));
            Assert.That(person.Betalingsbetingelse, Is.Not.Null);
            Assert.That(person.Betalingsbetingelse.Nummer, Is.EqualTo(view.Betalingsbetingelse.Nummer));
            Assert.That(person.Udlånsfrist, Is.EqualTo(view.Udlånsfrist));
            Assert.That(person.FilofaxAdresselabel, Is.EqualTo(view.FilofaxAdresselabel));
            Assert.That(person.Firma, Is.Not.Null);
            Assert.That(person.Firma.Nummer, Is.EqualTo(view.Firma.Nummer));
            Assert.That(person.Bogføringslinjer, Is.Not.Null);
            Assert.That(person.Bogføringslinjer.Count(), Is.EqualTo(0));
        }

        /// <summary>
        /// Tester, at Build kaster en IntranetRepositoryException ved bygning af person, hvis adressegruppen ikke findes.
        /// </summary>
        [Test]
        public void TestAtBuildKasterIntranetRepositoryExceptionVedBygningAfPersonHvisAdressegruppeIkkeFindes()
        {
            var fixture = new Fixture();
            var adressegrupper = fixture.CreateMany<Adressegruppe>(3).ToList();
            var betalingsbetingelser = fixture.CreateMany<Betalingsbetingelse>(3).ToList();
            var firmaer = fixture.CreateMany<Firma>(3).ToList();
            fixture.Inject(new AdressegruppeView
                               {
                                   Nummer = -1,
                                   Navn = fixture.CreateAnonymous<string>(),
                                   AdressegruppeOswebdb = fixture.CreateAnonymous<int>()
                               });
            fixture.Inject(new BetalingsbetingelseView
                               {
                                   Nummer = betalingsbetingelser.ElementAt(0).Nummer,
                                   Navn = betalingsbetingelser.ElementAt(0).Navn,
                               });
            fixture.Inject(new AdressereferenceView
                               {
                                   Nummer = firmaer.ElementAt(0).Nummer,
                                   Navn = firmaer.ElementAt(0).Navn
                               });
            fixture.Inject<IEnumerable<BogføringslinjeView>>(new List<BogføringslinjeView>());
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            domainObjectBuilder.SætAdressegrupper(adressegrupper);
            domainObjectBuilder.SætBetalingsbetingelser(betalingsbetingelser);
            domainObjectBuilder.SætAdresser(firmaer);

            var view = fixture.CreateAnonymous<PersonView>();
            Assert.Throws<IntranetRepositoryException>(() => domainObjectBuilder.Build<PersonView, AdresseBase>(view));
        }

        /// <summary>
        /// Tester, at Build kaster en IntranetRepositoryException ved bygning af person, hvis betalingsbetingelsen ikke findes.
        /// </summary>
        [Test]
        public void TestAtBuildKasterIntranetRepositoryExceptionVedBygningAfPersonHvisBetalingsbetingelsenIkkeFindes()
        {
            var fixture = new Fixture();
            var adressegrupper = fixture.CreateMany<Adressegruppe>(3).ToList();
            var betalingsbetingelser = fixture.CreateMany<Betalingsbetingelse>(3).ToList();
            var firmaer = fixture.CreateMany<Firma>(3).ToList();
            fixture.Inject(new AdressegruppeView
                               {
                                   Nummer = adressegrupper.ElementAt(0).Nummer,
                                   Navn = adressegrupper.ElementAt(0).Navn,
                                   AdressegruppeOswebdb = adressegrupper.ElementAt(0).AdressegruppeOswebdb
                               });
            fixture.Inject(new BetalingsbetingelseView
                               {
                                   Nummer = -1,
                                   Navn = fixture.CreateAnonymous<string>()
                               });
            fixture.Inject(new AdressereferenceView
                               {
                                   Nummer = firmaer.ElementAt(0).Nummer,
                                   Navn = firmaer.ElementAt(0).Navn
                               });
            fixture.Inject<IEnumerable<BogføringslinjeView>>(new List<BogføringslinjeView>());
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            domainObjectBuilder.SætAdressegrupper(adressegrupper);
            domainObjectBuilder.SætBetalingsbetingelser(betalingsbetingelser);
            domainObjectBuilder.SætAdresser(firmaer);

            var view = fixture.CreateAnonymous<PersonView>();
            Assert.Throws<IntranetRepositoryException>(() => domainObjectBuilder.Build<PersonView, AdresseBase>(view));
        }

        /// <summary>
        /// Tester, at Build kaster en IntranetRepositoryException ved bygning af person, hvis firmaet ikke findes.
        /// </summary>
        [Test]
        public void TestAtBuildKasterIntranetRepositoryExceptionVedBygningAfPersonHvisFirmaIkkeFindes()
        {
            var fixture = new Fixture();
            var adressegrupper = fixture.CreateMany<Adressegruppe>(3).ToList();
            var betalingsbetingelser = fixture.CreateMany<Betalingsbetingelse>(3).ToList();
            var firmaer = fixture.CreateMany<Firma>(3).ToList();
            fixture.Inject(new AdressegruppeView
                               {
                                   Nummer = adressegrupper.ElementAt(0).Nummer,
                                   Navn = adressegrupper.ElementAt(0).Navn,
                                   AdressegruppeOswebdb = adressegrupper.ElementAt(0).AdressegruppeOswebdb
                               });
            fixture.Inject(new BetalingsbetingelseView
                               {
                                   Nummer = betalingsbetingelser.ElementAt(0).Nummer,
                                   Navn = betalingsbetingelser.ElementAt(0).Navn,
                               });
            fixture.Inject(new AdressereferenceView
                               {
                                   Nummer = -1,
                                   Navn = fixture.CreateAnonymous<string>()
                               });
            fixture.Inject<IEnumerable<BogføringslinjeView>>(new List<BogføringslinjeView>());
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            domainObjectBuilder.SætAdressegrupper(adressegrupper);
            domainObjectBuilder.SætBetalingsbetingelser(betalingsbetingelser);
            domainObjectBuilder.SætAdresser(firmaer);

            var view = fixture.CreateAnonymous<PersonView>();
            Assert.Throws<IntranetRepositoryException>(() => domainObjectBuilder.Build<PersonView, AdresseBase>(view));
        }

        /// <summary>
        /// Tester, at Build bygger en liste af firmaer fra et view.
        /// </summary>
        [Test]
        public void TestAtBuildByggerListeAfPersoner()
        {
            var fixture = new Fixture();
            var adressegrupper = fixture.CreateMany<Adressegruppe>(3).ToList();
            var betalingsbetingelser = fixture.CreateMany<Betalingsbetingelse>(3).ToList();
            var firmaer = fixture.CreateMany<Firma>(3).ToList();
            fixture.Inject(new AdressegruppeView
                               {
                                   Nummer = adressegrupper.ElementAt(0).Nummer,
                                   Navn = adressegrupper.ElementAt(0).Navn,
                                   AdressegruppeOswebdb = adressegrupper.ElementAt(0).AdressegruppeOswebdb
                               });
            fixture.Inject(new BetalingsbetingelseView
                               {
                                   Nummer = betalingsbetingelser.ElementAt(0).Nummer,
                                   Navn = betalingsbetingelser.ElementAt(0).Navn,
                               });
            fixture.Inject(new AdressereferenceView
                               {
                                   Nummer = firmaer.ElementAt(0).Nummer,
                                   Navn = firmaer.ElementAt(0).Navn
                               });
            fixture.Inject<IEnumerable<BogføringslinjeView>>(new List<BogføringslinjeView>());
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            domainObjectBuilder.SætAdressegrupper(adressegrupper);
            domainObjectBuilder.SætBetalingsbetingelser(betalingsbetingelser);
            domainObjectBuilder.SætAdresser(firmaer);

            var view = fixture.CreateMany<PersonView>(3);
            var personer = domainObjectBuilder.Build<IEnumerable<PersonView>, IEnumerable<AdresseBase>>(view);
            Assert.That(personer, Is.Not.Null);
            Assert.That(personer.Count(), Is.EqualTo(3));
        }

        /// <summary>
        /// Tester, at Build bygger firma fra et view.
        /// </summary>
        [Test]
        public void TestAtBuildByggerAdresseBaseFraFirmaView()
        {
            var fixture = new Fixture();
            var adressegrupper = fixture.CreateMany<Adressegruppe>(3).ToList();
            var betalingsbetingelser = fixture.CreateMany<Betalingsbetingelse>(3).ToList();
            fixture.Inject(new AdressegruppeView
                               {
                                   Nummer = adressegrupper.ElementAt(0).Nummer,
                                   Navn = adressegrupper.ElementAt(0).Navn,
                                   AdressegruppeOswebdb = adressegrupper.ElementAt(0).AdressegruppeOswebdb
                               });
            fixture.Inject(new BetalingsbetingelseView
                               {
                                   Nummer = betalingsbetingelser.ElementAt(0).Nummer,
                                   Navn = betalingsbetingelser.ElementAt(0).Navn,
                               });
            fixture.Inject<IEnumerable<PersonView>>(new List<PersonView>());
            fixture.Inject<IEnumerable<BogføringslinjeView>>(new List<BogføringslinjeView>());
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            domainObjectBuilder.SætAdressegrupper(adressegrupper);
            domainObjectBuilder.SætBetalingsbetingelser(betalingsbetingelser);

            var view = fixture.CreateAnonymous<FirmaView>();
            var firma = (Firma) domainObjectBuilder.Build<FirmaView, AdresseBase>(view);
            Assert.That(firma, Is.Not.Null);
            Assert.That(firma.Nummer, Is.EqualTo(view.Nummer));
            Assert.That(firma.Navn, Is.Not.Null);
            Assert.That(firma.Navn, Is.EqualTo(view.Navn));
            Assert.That(firma.Adresse1, Is.EqualTo(view.Adresse1));
            Assert.That(firma.Adresse2, Is.EqualTo(view.Adresse2));
            Assert.That(firma.PostnrBy, Is.EqualTo(view.PostnummerBy));
            Assert.That(firma.Telefon1, Is.EqualTo(view.Telefon1));
            Assert.That(firma.Telefon2, Is.EqualTo(view.Telefon2));
            Assert.That(firma.Telefax, Is.EqualTo(view.Telefax));
            Assert.That(firma.Adressegruppe, Is.Not.Null);
            Assert.That(firma.Adressegruppe.Nummer, Is.EqualTo(view.Adressegruppe.Nummer));
            Assert.That(firma.Bekendtskab, Is.EqualTo(view.Bekendtskab));
            Assert.That(firma.Mailadresse, Is.EqualTo(view.Mailadresse));
            Assert.That(firma.Webadresse, Is.EqualTo(view.Webadresse));
            Assert.That(firma.Betalingsbetingelse, Is.Not.Null);
            Assert.That(firma.Betalingsbetingelse.Nummer, Is.EqualTo(view.Betalingsbetingelse.Nummer));
            Assert.That(firma.Udlånsfrist, Is.EqualTo(view.Udlånsfrist));
            Assert.That(firma.FilofaxAdresselabel, Is.EqualTo(view.FilofaxAdresselabel));
            Assert.That(firma.Personer, Is.Not.Null);
            Assert.That(firma.Personer.Count(), Is.EqualTo(0));
            Assert.That(firma.Bogføringslinjer, Is.Not.Null);
            Assert.That(firma.Bogføringslinjer.Count(), Is.EqualTo(0));
        }

        /// <summary>
        /// Tester, at Build kaster en IntranetRepositoryException ved bygning af firma, hvis adressegruppen ikke findes.
        /// </summary>
        [Test]
        public void TestAtBuildKasterIntranetRepositoryExceptionVedBygningAfFirmaHvisAdressegruppeIkkeFindes()
        {
            var fixture = new Fixture();
            var adressegrupper = fixture.CreateMany<Adressegruppe>(3).ToList();
            var betalingsbetingelser = fixture.CreateMany<Betalingsbetingelse>(3).ToList();
            fixture.Inject(new AdressegruppeView
                               {
                                   Nummer = -1,
                                   Navn = fixture.CreateAnonymous<string>(),
                                   AdressegruppeOswebdb = fixture.CreateAnonymous<int>()
                               });
            fixture.Inject(new BetalingsbetingelseView
                               {
                                   Nummer = betalingsbetingelser.ElementAt(0).Nummer,
                                   Navn = betalingsbetingelser.ElementAt(0).Navn,
                               });
            fixture.Inject<IEnumerable<PersonView>>(new List<PersonView>());
            fixture.Inject<IEnumerable<BogføringslinjeView>>(new List<BogføringslinjeView>());
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            domainObjectBuilder.SætAdressegrupper(adressegrupper);
            domainObjectBuilder.SætBetalingsbetingelser(betalingsbetingelser);

            var view = fixture.CreateAnonymous<FirmaView>();
            Assert.Throws<IntranetRepositoryException>(() => domainObjectBuilder.Build<FirmaView, AdresseBase>(view));
        }

        /// <summary>
        /// Tester, at Build kaster en IntranetRepositoryException ved bygning af firma, hvis betalingsbetingelsen ikke findes.
        /// </summary>
        [Test]
        public void TestAtBuildKasterIntranetRepositoryExceptionVedBygningAfFirmaHvisBetalingsbetingelseIkkeFindes()
        {
            var fixture = new Fixture();
            var adressegrupper = fixture.CreateMany<Adressegruppe>(3).ToList();
            var betalingsbetingelser = fixture.CreateMany<Betalingsbetingelse>(3).ToList();
            fixture.Inject(new AdressegruppeView
                               {
                                   Nummer = adressegrupper.ElementAt(0).Nummer,
                                   Navn = adressegrupper.ElementAt(0).Navn,
                                   AdressegruppeOswebdb = adressegrupper.ElementAt(0).AdressegruppeOswebdb
                               });
            fixture.Inject(new BetalingsbetingelseView
                               {
                                   Nummer = -1,
                                   Navn = fixture.CreateAnonymous<string>(),
                               });
            fixture.Inject<IEnumerable<PersonView>>(new List<PersonView>());
            fixture.Inject<IEnumerable<BogføringslinjeView>>(new List<BogføringslinjeView>());
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            domainObjectBuilder.SætAdressegrupper(adressegrupper);
            domainObjectBuilder.SætBetalingsbetingelser(betalingsbetingelser);

            var view = fixture.CreateAnonymous<FirmaView>();
            Assert.Throws<IntranetRepositoryException>(() => domainObjectBuilder.Build<FirmaView, AdresseBase>(view));
        }

        /// <summary>
        /// Tester, at Build bygger en liste af firmaer fra et view.
        /// </summary>
        [Test]
        public void TestAtBuildByggerListeAfFirmaer()
        {
            var fixture = new Fixture();
            var adressegrupper = fixture.CreateMany<Adressegruppe>(3).ToList();
            var betalingsbetingelser = fixture.CreateMany<Betalingsbetingelse>(3).ToList();
            fixture.Inject(new AdressegruppeView
                               {
                                   Nummer = adressegrupper.ElementAt(0).Nummer,
                                   Navn = adressegrupper.ElementAt(0).Navn,
                                   AdressegruppeOswebdb = adressegrupper.ElementAt(0).AdressegruppeOswebdb
                               });
            fixture.Inject(new BetalingsbetingelseView
                               {
                                   Nummer = betalingsbetingelser.ElementAt(0).Nummer,
                                   Navn = betalingsbetingelser.ElementAt(0).Navn,
                               });
            fixture.Inject<IEnumerable<PersonView>>(new List<PersonView>());
            fixture.Inject<IEnumerable<BogføringslinjeView>>(new List<BogføringslinjeView>());
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            domainObjectBuilder.SætAdressegrupper(adressegrupper);
            domainObjectBuilder.SætBetalingsbetingelser(betalingsbetingelser);

            var view = fixture.CreateMany<FirmaView>(3);
            var firmaer = domainObjectBuilder.Build<IEnumerable<FirmaView>, IEnumerable<AdresseBase>>(view);
            Assert.That(firmaer, Is.Not.Null);
            Assert.That(firmaer.Count(), Is.EqualTo(3));
        }

        /// <summary>
        /// Tester, at Build bygger postnummer fra et view.
        /// </summary>
        [Test]
        public void TestAtBuildByggerPostnummerFraPostnummerView()
        {
            var fixture = new Fixture();
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            var view = fixture.CreateAnonymous<PostnummerView>();
            var postnummer = domainObjectBuilder.Build<PostnummerView, Postnummer>(view);
            Assert.That(postnummer, Is.Not.Null);
            Assert.That(postnummer.Landekode, Is.Not.Null);
            Assert.That(postnummer.Landekode, Is.EqualTo(view.Landekode));
            Assert.That(postnummer.Postnr, Is.Not.Null);
            Assert.That(postnummer.Postnr, Is.EqualTo(view.Postnummer));
            Assert.That(postnummer.By, Is.Not.Null);
            Assert.That(postnummer.By, Is.EqualTo(view.Bynavn));
        }

        /// <summary>
        /// Tester, at Build bygger en liste af postnumre fra et view.
        /// </summary>
        [Test]
        public void TestAtBuildByggerListeAfPostnumre()
        {
            var fixture = new Fixture();
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            var view = fixture.CreateMany<PostnummerView>(3);
            var postnumre = domainObjectBuilder.Build<IEnumerable<PostnummerView>, IEnumerable<Postnummer>>(view);
            Assert.That(postnumre, Is.Not.Null);
            Assert.That(postnumre.Count(), Is.EqualTo(3));
        }

        /// <summary>
        /// Tester, at Build bygger adressegruppe fra et view.
        /// </summary>
        [Test]
        public void TestAtBuildByggerAdressegruppeFraAdressegruppeView()
        {
            var fixture = new Fixture();
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            var view = fixture.CreateAnonymous<AdressegruppeView>();
            var adressegruppe = domainObjectBuilder.Build<AdressegruppeView, Adressegruppe>(view);
            Assert.That(adressegruppe, Is.Not.Null);
            Assert.That(adressegruppe.Nummer, Is.EqualTo(view.Nummer));
            Assert.That(adressegruppe.Navn, Is.Not.Null);
            Assert.That(adressegruppe.Navn, Is.EqualTo(view.Navn));
            Assert.That(adressegruppe.AdressegruppeOswebdb, Is.EqualTo(view.AdressegruppeOswebdb));
        }

        /// <summary>
        /// Tester, at Build bygger en liste af adressegrupper fra et view.
        /// </summary>
        [Test]
        public void TestAtBuildByggerListeAfAdressegrupper()
        {
            var fixture = new Fixture();
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            var view = fixture.CreateMany<AdressegruppeView>(3);
            var adressegrupper = domainObjectBuilder.Build<IEnumerable<AdressegruppeView>, IEnumerable<Adressegruppe>>(view);
            Assert.That(adressegrupper, Is.Not.Null);
            Assert.That(adressegrupper.Count(), Is.EqualTo(3));
        }

        /// <summary>
        /// Tester, at Build bygger betalingsbetingelse fra et view.
        /// </summary>
        [Test]
        public void TestAtBuildByggerBetalingsbetingelseFraBetalingsbetingelseView()
        {
            var fixture = new Fixture();
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            var view = fixture.CreateAnonymous<BetalingsbetingelseView>();
            var betalingsbetingelse = domainObjectBuilder.Build<BetalingsbetingelseView, Betalingsbetingelse>(view);
            Assert.That(betalingsbetingelse, Is.Not.Null);
            Assert.That(betalingsbetingelse.Nummer, Is.EqualTo(view.Nummer));
            Assert.That(betalingsbetingelse.Navn, Is.Not.Null);
            Assert.That(betalingsbetingelse.Navn, Is.EqualTo(view.Navn));
        }

        /// <summary>
        /// Tester, at Build bygger en liste af betalingsbetingelser fra et view.
        /// </summary>
        [Test]
        public void TestAtBuildByggerListeAfBetalingsbetingelser()
        {
            var fixture = new Fixture();
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            var view = fixture.CreateMany<BetalingsbetingelseView>(3);
            var betalingsbetingelser = domainObjectBuilder.Build<IEnumerable<BetalingsbetingelseView>, IEnumerable<Betalingsbetingelse>>(view);
            Assert.That(betalingsbetingelser, Is.Not.Null);
            Assert.That(betalingsbetingelser.Count(), Is.EqualTo(3));
        }

        /// <summary>
        /// Tester, at Build bygger kontogruppe som aktiver fra et view.
        /// </summary>
        [Test]
        public void TestAtBuildByggerKontogruppeSomAktiverFraKontogruppeView()
        {
            var fixture = new Fixture();
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            fixture.Inject(DataAccess.Contracts.Enums.KontogruppeType.Aktiver);
            var view = fixture.CreateAnonymous<KontogruppeView>();
            var kontogruppe = domainObjectBuilder.Build<KontogruppeView, Kontogruppe>(view);
            Assert.That(kontogruppe, Is.Not.Null);
            Assert.That(kontogruppe.Nummer, Is.EqualTo(view.Nummer));
            Assert.That(kontogruppe.Navn, Is.Not.Null);
            Assert.That(kontogruppe.Navn, Is.EqualTo(view.Navn));
            Assert.That(kontogruppe.KontogruppeType, Is.EqualTo(KontogruppeType.Aktiver));
        }

        /// <summary>
        /// Tester, at Build bygger kontogruppe som passiver fra et view.
        /// </summary>
        [Test]
        public void TestAtBuildByggerKontogruppeSomPassiverFraKontogruppeView()
        {
            var fixture = new Fixture();
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            fixture.Inject(DataAccess.Contracts.Enums.KontogruppeType.Passiver);
            var view = fixture.CreateAnonymous<KontogruppeView>();
            var kontogruppe = domainObjectBuilder.Build<KontogruppeView, Kontogruppe>(view);
            Assert.That(kontogruppe, Is.Not.Null);
            Assert.That(kontogruppe.Nummer, Is.EqualTo(view.Nummer));
            Assert.That(kontogruppe.Navn, Is.Not.Null);
            Assert.That(kontogruppe.Navn, Is.EqualTo(view.Navn));
            Assert.That(kontogruppe.KontogruppeType, Is.EqualTo(KontogruppeType.Passiver));
        }

        /// <summary>
        /// Tester, at Build kaster en IntranetRepositoryException, hvis typen for kontogruppen ikke er kendt.
        /// </summary>
        [Test]
        public void TestAtBuildKasterIntranetRepositoryExceptionHvisTypenForKontogruppenIkkeErKendt()
        {
            var fixture = new Fixture();
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            fixture.Inject((DataAccess.Contracts.Enums.KontogruppeType)100);
            var view = fixture.CreateAnonymous<KontogruppeView>();
            Assert.Throws<IntranetRepositoryException>(
                () => domainObjectBuilder.Build<KontogruppeView, Kontogruppe>(view));
        }

        /// <summary>
        /// Tester, at Build bygger en liste af kontogrupper fra et view.
        /// </summary>
        [Test]
        public void TestAtBuildByggerListeAfKontogrupper()
        {
            var fixture = new Fixture();
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            var view = fixture.CreateMany<KontogruppeView>(3);
            var kontogrupper = domainObjectBuilder.Build<IEnumerable<KontogruppeView>, IEnumerable<Kontogruppe>>(view);
            Assert.That(kontogrupper, Is.Not.Null);
            Assert.That(kontogrupper.Count(), Is.EqualTo(3));
        }

        /// <summary>
        /// Tester, at Build bygger gruppe til budgetkonti fra et view.
        /// </summary>
        [Test]
        public void TestAtBuildByggerBudgetkontogruppeFraBudgetkontogruppeView()
        {
            var fixture = new Fixture();
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            var view = fixture.CreateAnonymous<BudgetkontogruppeView>();
            var budgetkontogruppe = domainObjectBuilder.Build<BudgetkontogruppeView, Budgetkontogruppe>(view);
            Assert.That(budgetkontogruppe, Is.Not.Null);
            Assert.That(budgetkontogruppe.Nummer, Is.EqualTo(view.Nummer));
            Assert.That(budgetkontogruppe.Navn, Is.Not.Null);
            Assert.That(budgetkontogruppe.Navn, Is.EqualTo(view.Navn));
        }

        /// <summary>
        /// Tester, at Build bygger en liste af grupper til budgetkonti fra et view.
        /// </summary>
        [Test]
        public void TestAtBuildByggerListeAfBudgetkontogrupper()
        {
            var fixture = new Fixture();
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            var view = fixture.CreateMany<BudgetkontogruppeView>(3);
            var budgetkontogrupper = domainObjectBuilder.Build<IEnumerable<BudgetkontogruppeView>, IEnumerable<Budgetkontogruppe>>(view);
            Assert.That(budgetkontogrupper, Is.Not.Null);
            Assert.That(budgetkontogrupper.Count(), Is.EqualTo(3));
        }

        /// <summary>
        /// Tester, at Build bygger regnskab fra et listeview.
        /// </summary>
        [Test]
        public void TestAtBuildByggerRegnskabFraRegnskabListeView()
        {
            var fixture = new Fixture();
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            var view = fixture.CreateAnonymous<RegnskabListeView>();
            var regnskab = domainObjectBuilder.Build<RegnskabListeView, Regnskab>(view);
            Assert.That(regnskab, Is.Not.Null);
            Assert.That(regnskab.Nummer, Is.EqualTo(view.Nummer));
            Assert.That(regnskab.Navn, Is.Not.Null);
            Assert.That(regnskab.Navn, Is.EqualTo(view.Navn));
            Assert.That(regnskab.Brevhoved, Is.Null);
            Assert.That(regnskab.Konti, Is.Not.Null);
            Assert.That(regnskab.Konti.Count(), Is.EqualTo(0));
        }

        /// <summary>
        /// Tester, at Build bygger en liste af regnskaber fra et listeview.
        /// </summary>
        [Test]
        public void TestAtBuildByggerListeAfRegnskaberFraListeView()
        {
            var fixture = new Fixture();
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            var view = fixture.CreateMany<RegnskabListeView>(3);
            var regnskaber = domainObjectBuilder.Build<IEnumerable<RegnskabListeView>, IEnumerable<Regnskab>>(view);
            Assert.That(regnskaber, Is.Not.Null);
            Assert.That(regnskaber.Count(), Is.EqualTo(3));
        }
    }
}
