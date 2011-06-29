using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Enums;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Fælles;
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
        /// Tester, at GetRegnskabCallback kaster en ArgumentNullException, hvis callbackmetoden er null.
        /// </summary>
        [Test]
        public void TestAtGetRegnskabCallbackKasterArgumentNullExceptionHvisCallbackMethodErNull()
        {
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => domainObjectBuilder.GetRegnskabCallback = null);
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

            domainObjectBuilder.GetAdressegruppeCallback = (nummer => adressegrupper.Single(m => m.Nummer == nummer));
            domainObjectBuilder.GetBetalingsbetingelseCallback = (nummer => betalingsbetingelser.Single(m => m.Nummer == nummer));
            domainObjectBuilder.GetAdresseBaseCallback = (nummer => firmaer.Single(m => m.Nummer == nummer));

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
        /// Tester, at Build kaster en IntranetRepositoryException ved bygning af person, hvis GetAdressegruppeCallback ikke er registreret.
        /// </summary>
        [Test]
        public void TestAtBuildKasterIntranetRepositoryExceptionVedBygningAfPersonHvisGetAdressegruppeCallbackIkkeErRegistreret()
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

            var view = fixture.CreateAnonymous<PersonView>();
            Assert.Throws<IntranetRepositoryException>(() => domainObjectBuilder.Build<PersonView, AdresseBase>(view));
        }

        /// <summary>
        /// Tester, at Build kaster en IntranetRepositoryException ved bygning af person, hvis GetAdressegruppeCallback kaster en IntranetRepositoryException.
        /// </summary>
        [Test]
        public void TestAtBuildKasterIntranetRepositoryExceptionVedBygningAfPersonHvisGetAdressegruppeCallbackKasterIntranetRepositoryException()
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

            domainObjectBuilder.GetAdressegruppeCallback = (nummer =>
                                                                {
                                                                    throw fixture.CreateAnonymous<IntranetRepositoryException>();
                                                                });

            var view = fixture.CreateAnonymous<PersonView>();
            Assert.Throws<IntranetRepositoryException>(() => domainObjectBuilder.Build<PersonView, AdresseBase>(view));
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

            domainObjectBuilder.GetAdressegruppeCallback = (nummer => adressegrupper.Single(m => m.Nummer == nummer));

            var view = fixture.CreateAnonymous<PersonView>();
            Assert.Throws<IntranetRepositoryException>(() => domainObjectBuilder.Build<PersonView, AdresseBase>(view));
        }

        /// <summary>
        /// Tester, at Build kaster en IntranetRepositoryException ved bygning af person, hvis GetBetalingsbetingelseCallback ikke er registreret.
        /// </summary>
        [Test]
        public void TestAtBuildKasterIntranetRepositoryExceptionVedBygningAfPersonHvisGetBetalingsbetingelseCallbackIkkeErRegistreret()
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
                                   Navn = betalingsbetingelser.ElementAt(0).Navn
                               });
            fixture.Inject(new AdressereferenceView
                               {
                                   Nummer = firmaer.ElementAt(0).Nummer,
                                   Navn = firmaer.ElementAt(0).Navn
                               });
            fixture.Inject<IEnumerable<BogføringslinjeView>>(new List<BogføringslinjeView>());
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            domainObjectBuilder.GetAdressegruppeCallback = (nummer => adressegrupper.Single(m => m.Nummer == nummer));

            var view = fixture.CreateAnonymous<PersonView>();
            Assert.Throws<IntranetRepositoryException>(() => domainObjectBuilder.Build<PersonView, AdresseBase>(view));
        }

        /// <summary>
        /// Tester, at Build kaster en IntranetRepositoryException ved bygning af person, hvis GetBetalingsbetingelseCallback kaster en IntranetRepositoryException.
        /// </summary>
        [Test]
        public void TestAtBuildKasterIntranetRepositoryExceptionVedBygningAfPersonHvisGetBetalingsbetingelseCallbackKasterIntranetRepositoryException()
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
                                   Navn = betalingsbetingelser.ElementAt(0).Navn
                               });
            fixture.Inject(new AdressereferenceView
                               {
                                   Nummer = firmaer.ElementAt(0).Nummer,
                                   Navn = firmaer.ElementAt(0).Navn
                               });
            fixture.Inject<IEnumerable<BogføringslinjeView>>(new List<BogføringslinjeView>());
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            domainObjectBuilder.GetAdressegruppeCallback = (nummer => adressegrupper.Single(m => m.Nummer == nummer));
            domainObjectBuilder.GetBetalingsbetingelseCallback = (nummer =>
                                                                      {
                                                                          throw fixture.CreateAnonymous<IntranetRepositoryException>();
                                                                      });

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

            domainObjectBuilder.GetAdressegruppeCallback = (nummer => adressegrupper.Single(m => m.Nummer == nummer));
            domainObjectBuilder.GetBetalingsbetingelseCallback = (nummer => betalingsbetingelser.Single(m => m.Nummer == nummer));

            var view = fixture.CreateAnonymous<PersonView>();
            Assert.Throws<IntranetRepositoryException>(() => domainObjectBuilder.Build<PersonView, AdresseBase>(view));
        }

        /// <summary>
        /// Tester, at Build kaster en IntranetRepositoryException ved bygning af person, hvis GetAdresseBaseCallback ikke er registreret.
        /// </summary>
        [Test]
        public void TestAtBuildKasterIntranetRepositoryExceptionVedBygningAfPersonHvisGetAdresseBaseCallbackIkkeErRegistreret()
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

            domainObjectBuilder.GetAdressegruppeCallback = (nummer => adressegrupper.Single(m => m.Nummer == nummer));
            domainObjectBuilder.GetBetalingsbetingelseCallback = (nummer => betalingsbetingelser.Single(m => m.Nummer == nummer));

            var view = fixture.CreateAnonymous<PersonView>();
            Assert.Throws<IntranetRepositoryException>(() => domainObjectBuilder.Build<PersonView, AdresseBase>(view));
        }

        /// <summary>
        /// Tester, at Build kaster en IntranetRepositoryException ved bygning af person, hvis GetAdresseBaseCallback kaster en IntranetRepositoryException.
        /// </summary>
        [Test]
        public void TestAtBuildKasterIntranetRepositoryExceptionVedBygningAfPersonHvisGetAdresseBaseCallbackKasterIntranetRepositoryException()
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

            domainObjectBuilder.GetAdressegruppeCallback = (nummer => adressegrupper.Single(m => m.Nummer == nummer));
            domainObjectBuilder.GetBetalingsbetingelseCallback = (nummer => betalingsbetingelser.Single(m => m.Nummer == nummer));
            domainObjectBuilder.GetAdresseBaseCallback = (nummer =>
                                                              {
                                                                  throw fixture.CreateAnonymous<IntranetRepositoryException>();
                                                              });

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

            domainObjectBuilder.GetAdressegruppeCallback = (nummer => adressegrupper.Single(m => m.Nummer == nummer));
            domainObjectBuilder.GetBetalingsbetingelseCallback = (nummer => betalingsbetingelser.Single(m => m.Nummer == nummer));
            domainObjectBuilder.GetAdresseBaseCallback = (nummer => firmaer.Single(m => m.Nummer == nummer));

            var view = fixture.CreateAnonymous<PersonView>();
            Assert.Throws<IntranetRepositoryException>(() => domainObjectBuilder.Build<PersonView, AdresseBase>(view));
        }

        /// <summary>
        /// Tester, at Build bygger en liste af personer fra et view.
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

            domainObjectBuilder.GetAdressegruppeCallback = (nummer => adressegrupper.Single(m => m.Nummer == nummer));
            domainObjectBuilder.GetBetalingsbetingelseCallback = (nummer => betalingsbetingelser.Single(m => m.Nummer == nummer));
            domainObjectBuilder.GetAdresseBaseCallback = (nummer => firmaer.Single(m => m.Nummer == nummer));

            var view = fixture.CreateMany<PersonView>(3);
            var personer = domainObjectBuilder.BuildMany<PersonView, AdresseBase>(view);
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

            domainObjectBuilder.GetAdressegruppeCallback = (nummer => adressegrupper.Single(m => m.Nummer == nummer));
            domainObjectBuilder.GetBetalingsbetingelseCallback = (nummer => betalingsbetingelser.Single(m => m.Nummer == nummer));

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
        /// Tester, at Build kaster en IntranetRepositoryException ved bygning af firma, hvis GetAdressegruppeCallback ikke er registreret.
        /// </summary>
        [Test]
        public void TestAtBuildKasterIntranetRepositoryExceptionVedBygningAfFirmaHvisGetAdressegruppeCallbackIkkeErRegistreret()
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

            var view = fixture.CreateAnonymous<FirmaView>();
            Assert.Throws<IntranetRepositoryException>(() => domainObjectBuilder.Build<FirmaView, AdresseBase>(view));
        }

        /// <summary>
        /// Tester, at Build kaster en IntranetRepositoryException ved bygning af firma, hvis GetAdressegruppeCallback kaster en IntranetRepositoryException.
        /// </summary>
        [Test]
        public void TestAtBuildKasterIntranetRepositoryExceptionVedBygningAfFirmaHvisGetAdressegruppeCallbackKasterIntranetRepositoryException()
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

            domainObjectBuilder.GetAdressegruppeCallback = (nummer =>
                                                                {
                                                                    throw fixture.CreateAnonymous<IntranetRepositoryException>();
                                                                });

            var view = fixture.CreateAnonymous<FirmaView>();
            Assert.Throws<IntranetRepositoryException>(() => domainObjectBuilder.Build<FirmaView, AdresseBase>(view));
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

            domainObjectBuilder.GetAdressegruppeCallback = (nummer => adressegrupper.Single(m => m.Nummer == nummer));

            var view = fixture.CreateAnonymous<FirmaView>();
            Assert.Throws<IntranetRepositoryException>(() => domainObjectBuilder.Build<FirmaView, AdresseBase>(view));
        }

        /// <summary>
        /// Tester, at Build kaster en IntranetRepositoryException ved bygning af firma, hvis GetBetalingsbetingelseCallback ikke er registreret.
        /// </summary>
        [Test]
        public void TestAtBuildKasterIntranetRepositoryExceptionVedBygningAfFirmaHvisGetBetalingsbetingelseCallbackIkkeErRegistreret()
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

            domainObjectBuilder.GetAdressegruppeCallback = (nummer => adressegrupper.Single(m => m.Nummer == nummer));

            var view = fixture.CreateAnonymous<FirmaView>();
            Assert.Throws<IntranetRepositoryException>(() => domainObjectBuilder.Build<FirmaView, AdresseBase>(view));
        }

        /// <summary>
        /// Tester, at Build kaster en IntranetRepositoryException ved bygning af firma, hvis GetBetalingsbetingelseCallback kaster en IntranetRepositoryException.
        /// </summary>
        [Test]
        public void TestAtBuildKasterIntranetRepositoryExceptionVedBygningAfFirmaHvisGetBetalingsbetingelseCallbackKasterIntranetRepositoryException()
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

            domainObjectBuilder.GetAdressegruppeCallback = (nummer => adressegrupper.Single(m => m.Nummer == nummer));
            domainObjectBuilder.GetBetalingsbetingelseCallback = (nummer =>
                                                                      {
                                                                          throw fixture.CreateAnonymous<IntranetRepositoryException>();
                                                                      });

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

            domainObjectBuilder.GetAdressegruppeCallback = (nummer => adressegrupper.Single(m => m.Nummer == nummer));
            domainObjectBuilder.GetBetalingsbetingelseCallback = (nummer => betalingsbetingelser.Single(m => m.Nummer == nummer));

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

            domainObjectBuilder.GetAdressegruppeCallback = (nummer => adressegrupper.Single(m => m.Nummer == nummer));
            domainObjectBuilder.GetBetalingsbetingelseCallback = (nummer => betalingsbetingelser.Single(m => m.Nummer == nummer));

            var view = fixture.CreateMany<FirmaView>(3);
            var firmaer = domainObjectBuilder.BuildMany<FirmaView, AdresseBase>(view);
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
            var postnumre = domainObjectBuilder.BuildMany<PostnummerView, Postnummer>(view);
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
            var adressegrupper = domainObjectBuilder.BuildMany<AdressegruppeView, Adressegruppe>(view);
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
            var betalingsbetingelser = domainObjectBuilder.BuildMany<BetalingsbetingelseView, Betalingsbetingelse>(view);
            Assert.That(betalingsbetingelser, Is.Not.Null);
            Assert.That(betalingsbetingelser.Count(), Is.EqualTo(3));
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
            var regnskaber = domainObjectBuilder.BuildMany<RegnskabListeView, Regnskab>(view);
            Assert.That(regnskaber, Is.Not.Null);
            Assert.That(regnskaber.Count(), Is.EqualTo(3));
        }

        /// <summary>
        /// Tester, at Build bygger regnskab fra et view.
        /// </summary>
        [Test]
        public void TestAtBuildByggerRegnskabFraRegnskabView()
        {
            var fixture = new Fixture();
            var kontogrupper = fixture.CreateMany<Kontogruppe>(3).ToList();
            var budgetkontogrupper = fixture.CreateMany<Budgetkontogruppe>(3).ToList();
            var brevhoveder = fixture.CreateMany<Brevhoved>(3).ToList();
            fixture.Inject(new RegnskabListeView
                               {
                                   Nummer = 1,
                                   Navn = fixture.CreateAnonymous<string>()
                               });
            fixture.Inject<IEnumerable<BogføringslinjeView>>(new List<BogføringslinjeView>());
            fixture.Inject(fixture.CreateMany<KreditoplysningerView>(24));
            fixture.Inject(fixture.CreateMany<BudgetoplysningerView>(24));
            fixture.Inject(new KontogruppeView
                               {
                                   Nummer = kontogrupper.ElementAt(0).Nummer,
                                   Navn = kontogrupper.ElementAt(0).Navn
                               });
            fixture.Inject(new BudgetkontogruppeView
                               {
                                   Nummer = budgetkontogrupper.ElementAt(0).Nummer,
                                   Navn = budgetkontogrupper.ElementAt(0).Navn
                               });
            fixture.Inject(new BrevhovedView
                               {
                                   Nummer = brevhoveder.ElementAt(0).Nummer,
                                   Navn = brevhoveder.ElementAt(0).Navn,
                                   Linje1 = brevhoveder.ElementAt(0).Linje1,
                                   Linje2 = brevhoveder.ElementAt(0).Linje2,
                                   Linje3 = brevhoveder.ElementAt(0).Linje3,
                                   Linje4 = brevhoveder.ElementAt(0).Linje4,
                                   Linje5 = brevhoveder.ElementAt(0).Linje5,
                                   Linje6 = brevhoveder.ElementAt(0).Linje6,
                                   Linje7 = brevhoveder.ElementAt(0).Linje7,
                                   CvrNr = brevhoveder.ElementAt(0).CvrNr
                               });
            fixture.Inject(new RegnskabView
                               {
                                   Nummer = fixture.CreateAnonymous<RegnskabListeView>().Nummer,
                                   Navn = fixture.CreateAnonymous<RegnskabListeView>().Navn,
                                   Konti = fixture.CreateMany<KontoView>(3),
                                   Budgetkonti = fixture.CreateMany<BudgetkontoView>(3),
                                   Brevhoved = fixture.CreateAnonymous<BrevhovedView>()
                               });
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            domainObjectBuilder.GetKontogruppeCallback = (nummer => kontogrupper.Single(m => m.Nummer == nummer));
            domainObjectBuilder.GetBudgetkontogruppeCallback = (nummer => budgetkontogrupper.Single(m => m.Nummer == nummer));
            domainObjectBuilder.GetBrevhovedCallback = (nummer => brevhoveder.Single(m => m.Nummer == nummer));

            var view = fixture.CreateAnonymous<RegnskabView>();
            var regnskab = domainObjectBuilder.Build<RegnskabView, Regnskab>(view);
            Assert.That(regnskab, Is.Not.Null);
            Assert.That(regnskab.Nummer, Is.EqualTo(view.Nummer));
            Assert.That(regnskab.Navn, Is.Not.Null);
            Assert.That(regnskab.Navn, Is.EqualTo(view.Navn));
            Assert.That(regnskab.Brevhoved, Is.Not.Null);
            Assert.That(regnskab.Brevhoved.Nummer, Is.EqualTo(view.Brevhoved.Nummer));
            Assert.That(regnskab.Konti, Is.Not.Null);
            Assert.That(regnskab.Konti.OfType<Konto>().Count(), Is.EqualTo(3));
            Assert.That(regnskab.Konti.OfType<Budgetkonto>().Count(), Is.EqualTo(3));
        }

        /// <summary>
        /// Tester, at Build kaster en IntranetRepositoryException ved bygning af regnskab, hvis GetBrevhovedCallback ikke er registreret.
        /// </summary>
        [Test]
        public void TestAtBuildKasterIntranetRepositoryExceptionVedBygningAfRegnskabHvisGetBrevhovedCallbackIkkeErRegistreret()
        {
            var fixture = new Fixture();
            fixture.Inject<IEnumerable<KontoView>>(new List<KontoView>());
            fixture.Inject<IEnumerable<BudgetkontoView>>(new List<BudgetkontoView>());
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            var view = fixture.CreateAnonymous<RegnskabView>();
            Assert.Throws<IntranetRepositoryException>(() => domainObjectBuilder.Build<RegnskabView, Regnskab>(view));
        }

        /// <summary>
        /// Tester, at Build kaster en IntranetRepositoryException ved bygning af regnskab, hvis GetBrevhovedCallback kaster en IntranetRepositoryException.
        /// </summary>
        [Test]
        public void TestAtBuildKasterIntranetRepositoryExceptionVedBygningAfRegnskabHvisGetBrevhovedCallbackKasterIntranetRepositoryException()
        {
            var fixture = new Fixture();
            fixture.Inject<IEnumerable<KontoView>>(new List<KontoView>());
            fixture.Inject<IEnumerable<BudgetkontoView>>(new List<BudgetkontoView>());
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            domainObjectBuilder.GetBrevhovedCallback = (nummer =>
                                                            {
                                                                throw fixture.CreateAnonymous<IntranetRepositoryException>();
                                                            });

            var view = fixture.CreateAnonymous<RegnskabView>();
            Assert.Throws<IntranetRepositoryException>(() => domainObjectBuilder.Build<RegnskabView, Regnskab>(view));
        }

        /// <summary>
        /// Tester, at Build kaster en IntranetRepositoryException ved bygning af regnskab, hvis brevhovedet ikke findes.
        /// </summary>
        [Test]
        public void TestAtBuildKasterIntranetRepositoryExceptionVedBygningAfRegnskabHvisBrevhovedIkkeFindes()
        {
            var fixture = new Fixture();
            var brevhoveder = fixture.CreateMany<Brevhoved>(3).ToList();
            fixture.Inject(new BrevhovedView
                               {
                                   Nummer = -1,
                                   Navn = fixture.CreateAnonymous<string>(),
                                   Linje1 = fixture.CreateAnonymous<string>(),
                                   Linje2 = fixture.CreateAnonymous<string>(),
                                   Linje3 = fixture.CreateAnonymous<string>(),
                                   Linje4 = fixture.CreateAnonymous<string>(),
                                   Linje5 = fixture.CreateAnonymous<string>(),
                                   Linje6 = fixture.CreateAnonymous<string>(),
                                   Linje7 = fixture.CreateAnonymous<string>(),
                                   CvrNr = fixture.CreateAnonymous<string>()
                               });
            fixture.Inject<IEnumerable<KontoView>>(new List<KontoView>());
            fixture.Inject<IEnumerable<BudgetkontoView>>(new List<BudgetkontoView>());
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            domainObjectBuilder.GetBrevhovedCallback = (nummer => brevhoveder.Single(m => m.Nummer == nummer));

            var view = fixture.CreateAnonymous<RegnskabView>();
            Assert.Throws<IntranetRepositoryException>(() => domainObjectBuilder.Build<RegnskabView, Regnskab>(view));
        }

        /// <summary>
        /// Tester, at Build bygger liste af regnskab fra et view.
        /// </summary>
        [Test]
        public void TestAtBuildByggerListeAfRegnskaber()
        {
            var fixture = new Fixture();
            var kontogrupper = fixture.CreateMany<Kontogruppe>(3).ToList();
            var budgetkontogrupper = fixture.CreateMany<Budgetkontogruppe>(3).ToList();
            var brevhoveder = fixture.CreateMany<Brevhoved>(3).ToList();
            fixture.Inject(new RegnskabListeView
                               {
                                   Nummer = 1,
                                   Navn = fixture.CreateAnonymous<string>()
                               });
            fixture.Inject<IEnumerable<BogføringslinjeView>>(new List<BogføringslinjeView>());
            fixture.Inject(fixture.CreateMany<KreditoplysningerView>(24));
            fixture.Inject(fixture.CreateMany<BudgetoplysningerView>(24));
            fixture.Inject(new KontogruppeView
                               {
                                   Nummer = kontogrupper.ElementAt(0).Nummer,
                                   Navn = kontogrupper.ElementAt(0).Navn
                               });
            fixture.Inject(new BudgetkontogruppeView
                               {
                                   Nummer = budgetkontogrupper.ElementAt(0).Nummer,
                                   Navn = budgetkontogrupper.ElementAt(0).Navn
                               });
            fixture.Inject(new BrevhovedView
                               {
                                   Nummer = brevhoveder.ElementAt(0).Nummer,
                                   Navn = brevhoveder.ElementAt(0).Navn,
                                   Linje1 = brevhoveder.ElementAt(0).Linje1,
                                   Linje2 = brevhoveder.ElementAt(0).Linje2,
                                   Linje3 = brevhoveder.ElementAt(0).Linje3,
                                   Linje4 = brevhoveder.ElementAt(0).Linje4,
                                   Linje5 = brevhoveder.ElementAt(0).Linje5,
                                   Linje6 = brevhoveder.ElementAt(0).Linje6,
                                   Linje7 = brevhoveder.ElementAt(0).Linje7,
                                   CvrNr = brevhoveder.ElementAt(0).CvrNr
                               });
            fixture.Inject(new RegnskabView
                               {
                                   Nummer = fixture.CreateAnonymous<RegnskabListeView>().Nummer,
                                   Navn = fixture.CreateAnonymous<RegnskabListeView>().Navn,
                                   Konti = fixture.CreateMany<KontoView>(3),
                                   Budgetkonti = fixture.CreateMany<BudgetkontoView>(3),
                                   Brevhoved = fixture.CreateAnonymous<BrevhovedView>()
                               });
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            domainObjectBuilder.GetKontogruppeCallback = (nummer => kontogrupper.Single(m => m.Nummer == nummer));
            domainObjectBuilder.GetBudgetkontogruppeCallback = (nummer => budgetkontogrupper.Single(m => m.Nummer == nummer));
            domainObjectBuilder.GetBrevhovedCallback = (nummer => brevhoveder.Single(m => m.Nummer == nummer));

            var view = fixture.CreateMany<RegnskabView>(1);
            var regnskaber = domainObjectBuilder.BuildMany<RegnskabView, Regnskab>(view);
            Assert.That(regnskaber, Is.Not.Null);
            Assert.That(regnskaber.Count(), Is.EqualTo(1));
        }

        /// <summary>
        /// Tester, at Build bygger konto fra et view.
        /// </summary>
        [Test]
        public void TestAtBuildByggerKontoFraKontoView()
        {
            var fixture = new Fixture();
            var regnskaber = fixture.CreateMany<Regnskab>(3).ToList();
            var kontogrupper = fixture.CreateMany<Kontogruppe>(3).ToList();
            fixture.Inject(new RegnskabListeView
                               {
                                   Nummer = regnskaber.ElementAt(0).Nummer,
                                   Navn = regnskaber.ElementAt(0).Navn,
                               });
            fixture.Inject(new KontogruppeView
                               {
                                   Nummer = kontogrupper.ElementAt(0).Nummer,
                                   Navn = kontogrupper.ElementAt(0).Navn,
                                   KontogruppeType = fixture.CreateAnonymous<DataAccess.Contracts.Enums.KontogruppeType>()
                               });
            fixture.Inject(fixture.CreateMany<KreditoplysningerView>(24));
            fixture.Inject(fixture.CreateMany<BogføringslinjeView>(250));
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            domainObjectBuilder.GetRegnskabCallback = (nummer => regnskaber.Single(m => m.Nummer == nummer));
            domainObjectBuilder.GetKontogruppeCallback = (nummer => kontogrupper.Single(m => m.Nummer == nummer));

            var view = fixture.CreateAnonymous<KontoView>();
            var konto = domainObjectBuilder.Build<KontoView, Konto>(view);
            Assert.That(konto, Is.Not.Null);
            Assert.That(konto.Regnskab, Is.Not.Null);
            Assert.That(konto.Regnskab.Nummer, Is.EqualTo(view.Regnskab.Nummer));
            Assert.That(konto.Kontonummer, Is.Not.Null);
            Assert.That(konto.Kontonummer, Is.EqualTo(view.Kontonummer));
            Assert.That(konto.Kontonavn, Is.Not.Null);
            Assert.That(konto.Kontonavn, Is.EqualTo(view.Kontonavn));
            Assert.That(konto.Beskrivelse, Is.EqualTo(view.Beskrivelse));
            Assert.That(konto.Note, Is.EqualTo(view.Note));
            Assert.That(konto.Kontogruppe, Is.Not.Null);
            Assert.That(konto.Kontogruppe.Nummer, Is.EqualTo(view.Kontogruppe.Nummer));
            Assert.That(konto.Kreditoplysninger, Is.Not.Null);
            Assert.That(konto.Kreditoplysninger.Count(), Is.EqualTo(24));
            Assert.That(konto.Bogføringslinjer, Is.Not.Null);
            Assert.That(konto.Bogføringslinjer.Count(), Is.EqualTo(0));
        }

        /// <summary>
        /// Tester, at Build kaster en IntranetRepositoryException ved bygning af konto, hvis GetRegnskabCallback ikke er registreret.
        /// </summary>
        [Test]
        public void TestAtBuildKasterIntranetRepositoryExceptionVedBygningAfKontoHvisGetRegnskabCallbackIkkeErRegistreret()
        {
            var fixture = new Fixture();
            fixture.Inject<IEnumerable<KreditoplysningerView>>(new List<KreditoplysningerView>());
            fixture.Inject<IEnumerable<BogføringslinjeView>>(new List<BogføringslinjeView>());
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            var view = fixture.CreateAnonymous<KontoView>();
            Assert.Throws<IntranetRepositoryException>(() => domainObjectBuilder.Build<KontoView, Konto>(view));
        }

        /// <summary>
        /// Tester, at Build kaster en IntranetRepositoryException ved bygning af konto, hvis GetRegnskabCallback  kaster en IntranetRepositoryException.
        /// </summary>
        [Test]
        public void TestAtBuildKasterIntranetRepositoryExceptionVedBygningAfKontoHvisGetRegnskabCallbackKasterIntranetRepositoryException()
        {
            var fixture = new Fixture();
            fixture.Inject<IEnumerable<KreditoplysningerView>>(new List<KreditoplysningerView>());
            fixture.Inject<IEnumerable<BogføringslinjeView>>(new List<BogføringslinjeView>());
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            domainObjectBuilder.GetRegnskabCallback = (nummer =>
                                                           {
                                                               throw fixture.CreateAnonymous<IntranetRepositoryException>();
                                                           });

            var view = fixture.CreateAnonymous<KontoView>();
            Assert.Throws<IntranetRepositoryException>(() => domainObjectBuilder.Build<KontoView, Konto>(view));
        }

        /// <summary>
        /// Tester, at Build kaster en IntranetRepositoryException ved bygning af konto, hvis regnskabet ikke findes.
        /// </summary>
        [Test]
        public void TestAtBuildKasterIntranetRepositoryExceptionVedBygningAfKontoHvisRegnskabIkkeFindes()
        {
            var fixture = new Fixture();
            var regnskaber = fixture.CreateMany<Regnskab>(3).ToList();
            fixture.Inject(new RegnskabListeView
                               {
                                   Nummer = -1,
                                   Navn = fixture.CreateAnonymous<string>(),
                               });
            fixture.Inject<IEnumerable<KreditoplysningerView>>(new List<KreditoplysningerView>());
            fixture.Inject<IEnumerable<BogføringslinjeView>>(new List<BogføringslinjeView>());
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            domainObjectBuilder.GetRegnskabCallback = (nummer => regnskaber.Single(m => m.Nummer == nummer));

            var view = fixture.CreateAnonymous<KontoView>();
            Assert.Throws<IntranetRepositoryException>(() => domainObjectBuilder.Build<KontoView, Konto>(view));
        }

        /// <summary>
        /// Tester, at Build kaster en IntranetRepositoryException ved bygning af konto, hvis GetKontogruppeCallback ikke er registreret.
        /// </summary>
        [Test]
        public void TestAtBuildKasterIntranetRepositoryExceptionVedBygningAfKontoHvisGetKontogruppeCallbackIkkeErRegistreret()
        {
            var fixture = new Fixture();
            var regnskaber = fixture.CreateMany<Regnskab>(3).ToList();
            fixture.Inject(new RegnskabListeView
                               {
                                   Nummer = regnskaber.ElementAt(0).Nummer,
                                   Navn = regnskaber.ElementAt(0).Navn,
                               });
            fixture.Inject<IEnumerable<KreditoplysningerView>>(new List<KreditoplysningerView>());
            fixture.Inject<IEnumerable<BogføringslinjeView>>(new List<BogføringslinjeView>());
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            domainObjectBuilder.GetRegnskabCallback = (nummer => regnskaber.Single(m => m.Nummer == nummer));

            var view = fixture.CreateAnonymous<KontoView>();
            Assert.Throws<IntranetRepositoryException>(() => domainObjectBuilder.Build<KontoView, Konto>(view));
        }

        /// <summary>
        /// Tester, at Build kaster en IntranetRepositoryException ved bygning af konto, hvis GetKontogruppeCallback kaster en IntranetRepositoryException.
        /// </summary>
        [Test]
        public void TestAtBuildKasterIntranetRepositoryExceptionVedBygningAfKontoHvisGetKontogruppeCallbackKasterIntranetRepositoryException()
        {
            var fixture = new Fixture();
            var regnskaber = fixture.CreateMany<Regnskab>(3).ToList();
            fixture.Inject(new RegnskabListeView
                               {
                                   Nummer = regnskaber.ElementAt(0).Nummer,
                                   Navn = regnskaber.ElementAt(0).Navn,
                               });
            fixture.Inject<IEnumerable<KreditoplysningerView>>(new List<KreditoplysningerView>());
            fixture.Inject<IEnumerable<BogføringslinjeView>>(new List<BogføringslinjeView>());
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            domainObjectBuilder.GetRegnskabCallback = (nummer => regnskaber.Single(m => m.Nummer == nummer));
            domainObjectBuilder.GetKontogruppeCallback = (nummer =>
                                                              {
                                                                  throw fixture.CreateAnonymous<IntranetRepositoryException>();
                                                              });

            var view = fixture.CreateAnonymous<KontoView>();
            Assert.Throws<IntranetRepositoryException>(() => domainObjectBuilder.Build<KontoView, Konto>(view));
        }

        /// <summary>
        /// Tester, at Build kaster en IntranetRepositoryException ved bygning af konto, hvis kontogruppe ikke findes.
        /// </summary>
        [Test]
        public void TestAtBuildKasterIntranetRepositoryExceptionVedBygningAfKontoHvisKontogruppeIkkeFindes()
        {
            var fixture = new Fixture();
            var regnskaber = fixture.CreateMany<Regnskab>(3).ToList();
            fixture.Inject(new RegnskabListeView
                               {
                                   Nummer = regnskaber.ElementAt(0).Nummer,
                                   Navn = regnskaber.ElementAt(0).Navn,
                               });
            var kontogrupper = fixture.CreateMany<Kontogruppe>(3).ToList();
            fixture.Inject(new KontogruppeView
                               {
                                   Nummer = -1,
                                   Navn = fixture.CreateAnonymous<string>(),
                                   KontogruppeType = fixture.CreateAnonymous<DataAccess.Contracts.Enums.KontogruppeType>()
                               });
            fixture.Inject<IEnumerable<KreditoplysningerView>>(new List<KreditoplysningerView>());
            fixture.Inject<IEnumerable<BogføringslinjeView>>(new List<BogføringslinjeView>());
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            domainObjectBuilder.GetRegnskabCallback = (nummer => regnskaber.Single(m => m.Nummer == nummer));
            domainObjectBuilder.GetKontogruppeCallback = (nummer => kontogrupper.Single(m => m.Nummer == nummer));

            var view = fixture.CreateAnonymous<KontoView>();
            Assert.Throws<IntranetRepositoryException>(() => domainObjectBuilder.Build<KontoView, Konto>(view));
        }

        /// <summary>
        /// Tester, at Build bygger liste af konti fra et view.
        /// </summary>
        [Test]
        public void TestAtBuildByggerListeAfKonti()
        {
            var fixture = new Fixture();
            var regnskaber = fixture.CreateMany<Regnskab>(3).ToList();
            var kontogrupper = fixture.CreateMany<Kontogruppe>(3).ToList();
            fixture.Inject(new RegnskabListeView
                               {
                                   Nummer = regnskaber.ElementAt(0).Nummer,
                                   Navn = regnskaber.ElementAt(0).Navn,
                               });
            fixture.Inject(new KontogruppeView
                               {
                                   Nummer = kontogrupper.ElementAt(0).Nummer,
                                   Navn = kontogrupper.ElementAt(0).Navn,
                                   KontogruppeType =
                                       fixture.CreateAnonymous<DataAccess.Contracts.Enums.KontogruppeType>()
                               });
            fixture.Inject(fixture.CreateMany<KreditoplysningerView>(24));
            fixture.Inject(fixture.CreateMany<BogføringslinjeView>(250));
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            domainObjectBuilder.GetRegnskabCallback = (nummer => regnskaber.Single(m => m.Nummer == nummer));
            domainObjectBuilder.GetKontogruppeCallback = (nummer => kontogrupper.Single(m => m.Nummer == nummer));

            var view = fixture.CreateMany<KontoView>(3);
            var konti = domainObjectBuilder.BuildMany<KontoView, Konto>(view);
            Assert.That(konti, Is.Not.Null);
            Assert.That(konti.Count(), Is.EqualTo(3));
        }

        /// <summary>
        /// Tester, at Build bygger kreditoplysninger fra et view.
        /// </summary>
        [Test]
        public void TestAtBuildByggerKreditoplysningerFraKreditoplysningerView()
        {
            var fixture = new Fixture();
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            var view = fixture.CreateAnonymous<KreditoplysningerView>();
            var kreditoplysninger = domainObjectBuilder.Build<KreditoplysningerView, Kreditoplysninger>(view);
            Assert.That(kreditoplysninger, Is.Not.Null);
            Assert.That(kreditoplysninger.Konto, Is.Null);
            Assert.That(kreditoplysninger.År, Is.EqualTo(view.År));
            Assert.That(kreditoplysninger.Måned, Is.EqualTo(view.Måned));
            Assert.That(kreditoplysninger.Kredit, Is.EqualTo(view.Kredit));
        }

        /// <summary>
        /// Tester ,at Build bygger liste af kreditoplysninger fra et view.
        /// </summary>
        [Test]
        public void TestAtBuildByggerListeAfKreditoplysninger()
        {
            var fixture = new Fixture();
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            var view = fixture.CreateMany<KreditoplysningerView>(24);
            var kreditoplysninger = domainObjectBuilder.BuildMany<KreditoplysningerView, Kreditoplysninger>(view);
            Assert.That(kreditoplysninger, Is.Not.Null);
            Assert.That(kreditoplysninger.Count(), Is.EqualTo(24));
        }

        /// <summary>
        /// Tester, at Build bygger budgetkonto fra et view.
        /// </summary>
        [Test]
        public void TestAtBuildByggerBudgetkontoFraBudgetkontoView()
        {
            var fixture = new Fixture();
            var regnskaber = fixture.CreateMany<Regnskab>(3).ToList();
            var budgetkontogrupper = fixture.CreateMany<Budgetkontogruppe>(3).ToList();
            fixture.Inject(new RegnskabListeView
                               {
                                   Nummer = regnskaber.ElementAt(0).Nummer,
                                   Navn = regnskaber.ElementAt(0).Navn,
                               });
            fixture.Inject(new BudgetkontogruppeView
                               {
                                   Nummer = budgetkontogrupper.ElementAt(0).Nummer,
                                   Navn = budgetkontogrupper.ElementAt(0).Navn,
                               });
            fixture.Inject(fixture.CreateMany<BudgetoplysningerView>(24));
            fixture.Inject(fixture.CreateMany<BogføringslinjeView>(250));
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            domainObjectBuilder.GetRegnskabCallback = (nummer => regnskaber.Single(m => m.Nummer == nummer));
            domainObjectBuilder.GetBudgetkontogruppeCallback = (nummer => budgetkontogrupper.Single(m => m.Nummer == nummer));

            var view = fixture.CreateAnonymous<BudgetkontoView>();
            var budgetkonto = domainObjectBuilder.Build<BudgetkontoView, Budgetkonto>(view);
            Assert.That(budgetkonto, Is.Not.Null);
            Assert.That(budgetkonto.Regnskab, Is.Not.Null);
            Assert.That(budgetkonto.Regnskab.Nummer, Is.EqualTo(view.Regnskab.Nummer));
            Assert.That(budgetkonto.Kontonummer, Is.Not.Null);
            Assert.That(budgetkonto.Kontonummer, Is.EqualTo(view.Kontonummer));
            Assert.That(budgetkonto.Kontonavn, Is.Not.Null);
            Assert.That(budgetkonto.Kontonavn, Is.EqualTo(view.Kontonavn));
            Assert.That(budgetkonto.Beskrivelse, Is.EqualTo(view.Beskrivelse));
            Assert.That(budgetkonto.Note, Is.EqualTo(view.Note));
            Assert.That(budgetkonto.Budgetkontogruppe, Is.Not.Null);
            Assert.That(budgetkonto.Budgetkontogruppe.Nummer, Is.EqualTo(view.Budgetkontogruppe.Nummer));
            Assert.That(budgetkonto.Budgetoplysninger, Is.Not.Null);
            Assert.That(budgetkonto.Budgetoplysninger.Count(), Is.EqualTo(24));
            Assert.That(budgetkonto.Bogføringslinjer, Is.Not.Null);
            Assert.That(budgetkonto.Bogføringslinjer.Count(), Is.EqualTo(0));
        }

        /// <summary>
        /// Tester, at Build kaster en IntranetRepositoryException ved bygning af budgetkonto, hvis GetRegnskabCallback ikke er registreret.
        /// </summary>
        [Test]
        public void TestAtBuildKasterIntranetRepositoryExceptionVedBygningAfBudgetkontoHvisGetRegnskabCallbackIkkeErRegistreret()
        {
            var fixture = new Fixture();
            fixture.Inject<IEnumerable<BudgetoplysningerView>>(new List<BudgetoplysningerView>());
            fixture.Inject<IEnumerable<BogføringslinjeView>>(new List<BogføringslinjeView>());
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            var view = fixture.CreateAnonymous<BudgetkontoView>();
            Assert.Throws<IntranetRepositoryException>(() => domainObjectBuilder.Build<BudgetkontoView, Budgetkonto>(view));
        }

        /// <summary>
        /// Tester, at Build kaster en IntranetRepositoryException ved bygning af budgetkonto, hvis GetRegnskabCallback kaster en IntranetRepositoryException.
        /// </summary>
        [Test]
        public void TestAtBuildKasterIntranetRepositoryExceptionVedBygningAfBudgetkontoHvisGetRegnskabCallbackKasterIntranetRepositoryException()
        {
            var fixture = new Fixture();
            fixture.Inject<IEnumerable<BudgetoplysningerView>>(new List<BudgetoplysningerView>());
            fixture.Inject<IEnumerable<BogføringslinjeView>>(new List<BogføringslinjeView>());
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            domainObjectBuilder.GetRegnskabCallback = (nummer =>
                                                           {
                                                               throw fixture.CreateAnonymous<IntranetRepositoryException>();
                                                           });

            var view = fixture.CreateAnonymous<BudgetkontoView>();
            Assert.Throws<IntranetRepositoryException>(() => domainObjectBuilder.Build<BudgetkontoView, Budgetkonto>(view));
        }

        /// <summary>
        /// Tester, at Build kaster en IntranetRepositoryException ved bygning af budgetkonto, hvis regnskabet ikke findes.
        /// </summary>
        [Test]
        public void TestAtBuildKasterIntranetRepositoryExceptionVedBygningAfBudgetkontoHvisRegnskabIkkeFindes()
        {
            var fixture = new Fixture();
            var regnskaber = fixture.CreateMany<Regnskab>(3).ToList();
            fixture.Inject(new RegnskabListeView
                               {
                                   Nummer = -1,
                                   Navn = fixture.CreateAnonymous<string>(),
                               });
            fixture.Inject<IEnumerable<BudgetoplysningerView>>(new List<BudgetoplysningerView>());
            fixture.Inject<IEnumerable<BogføringslinjeView>>(new List<BogføringslinjeView>());
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            domainObjectBuilder.GetRegnskabCallback = (nummer => regnskaber.Single(m => m.Nummer == nummer));

            var view = fixture.CreateAnonymous<BudgetkontoView>();
            Assert.Throws<IntranetRepositoryException>(() => domainObjectBuilder.Build<BudgetkontoView, Budgetkonto>(view));
        }

        /// <summary>
        /// Tester, at Build kaster en IntranetRepositoryException ved bygning af budgetkonto, hvis GetBudgetkontogruppeCallback ikke er registreret.
        /// </summary>
        [Test]
        public void TestAtBuildKasterIntranetRepositoryExceptionVedBygningAfBudgetkontoHvisGetBudgetkontogruppeCallbackIkkeErRegistreret()
        {
            var fixture = new Fixture();
            var regnskaber = fixture.CreateMany<Regnskab>(3).ToList();
            fixture.Inject(new RegnskabListeView
                               {
                                   Nummer = regnskaber.ElementAt(0).Nummer,
                                   Navn = regnskaber.ElementAt(0).Navn,
                               });
            fixture.Inject<IEnumerable<BudgetoplysningerView>>(new List<BudgetoplysningerView>());
            fixture.Inject<IEnumerable<BogføringslinjeView>>(new List<BogføringslinjeView>());
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            domainObjectBuilder.GetRegnskabCallback = (nummer => regnskaber.Single(m => m.Nummer == nummer));

            var view = fixture.CreateAnonymous<BudgetkontoView>();
            Assert.Throws<IntranetRepositoryException>(() => domainObjectBuilder.Build<BudgetkontoView, Budgetkonto>(view));
        }

        /// <summary>
        /// Tester, at Build kaster en IntranetRepositoryException ved bygning af budgetkonto, hvis GetBudgetkontogruppeCallback kaster en IntranetRepositoryException.
        /// </summary>
        [Test]
        public void TestAtBuildKasterIntranetRepositoryExceptionVedBygningAfBudgetkontoHvisGetBudgetkontogruppeCallbackKasterIntranetRepositoryException()
        {
            var fixture = new Fixture();
            var regnskaber = fixture.CreateMany<Regnskab>(3).ToList();
            fixture.Inject(new RegnskabListeView
                               {
                                   Nummer = regnskaber.ElementAt(0).Nummer,
                                   Navn = regnskaber.ElementAt(0).Navn,
                               });
            fixture.Inject<IEnumerable<BudgetoplysningerView>>(new List<BudgetoplysningerView>());
            fixture.Inject<IEnumerable<BogføringslinjeView>>(new List<BogføringslinjeView>());
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            domainObjectBuilder.GetRegnskabCallback = (nummer => regnskaber.Single(m => m.Nummer == nummer));
            domainObjectBuilder.GetBudgetkontogruppeCallback = (nummer =>
                                                                    {
                                                                        throw fixture.CreateAnonymous<IntranetRepositoryException>();
                                                                    });

            var view = fixture.CreateAnonymous<BudgetkontoView>();
            Assert.Throws<IntranetRepositoryException>(() => domainObjectBuilder.Build<BudgetkontoView, Budgetkonto>(view));
        }

        /// <summary>
        /// Tester, at Build kaster en IntranetRepositoryException ved bygning af budgetkonto, hvis gruppen til budgetkontoen ikke findes.
        /// </summary>
        [Test]
        public void TestAtBuildKasterIntranetRepositoryExceptionVedBygningAfBudgetkontoHvisBudgetkontogruppeIkkeFindes()
        {
            var fixture = new Fixture();
            var regnskaber = fixture.CreateMany<Regnskab>(3).ToList();
            var budgetkontogrupper = fixture.CreateMany<Budgetkontogruppe>(3).ToList();
            fixture.Inject(new RegnskabListeView
                               {
                                   Nummer = regnskaber.ElementAt(0).Nummer,
                                   Navn = regnskaber.ElementAt(0).Navn,
                               });
            fixture.Inject(new BudgetkontogruppeView
                               {
                                   Nummer = -1,
                                   Navn = fixture.CreateAnonymous<string>()
                               });
            fixture.Inject<IEnumerable<BudgetoplysningerView>>(new List<BudgetoplysningerView>());
            fixture.Inject<IEnumerable<BogføringslinjeView>>(new List<BogføringslinjeView>());
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            domainObjectBuilder.GetRegnskabCallback = (nummer => regnskaber.Single(m => m.Nummer == nummer));
            domainObjectBuilder.GetBudgetkontogruppeCallback = (nummer => budgetkontogrupper.Single(m => m.Nummer == nummer));

            var view = fixture.CreateAnonymous<BudgetkontoView>();
            Assert.Throws<IntranetRepositoryException>(() => domainObjectBuilder.Build<BudgetkontoView, Budgetkonto>(view));
        }

        /// <summary>
        /// Tester, at Build bygger liste af budgetkonti fra et view.
        /// </summary>
        [Test]
        public void TestAtBuildByggerListeAfBudgetkonti()
        {
            var fixture = new Fixture();
            var regnskaber = fixture.CreateMany<Regnskab>(3).ToList();
            var budgetkontogrupper = fixture.CreateMany<Budgetkontogruppe>(3).ToList();
            fixture.Inject(new RegnskabListeView
                               {
                                   Nummer = regnskaber.ElementAt(0).Nummer,
                                   Navn = regnskaber.ElementAt(0).Navn,
                               });
            fixture.Inject(new BudgetkontogruppeView
                               {
                                   Nummer = budgetkontogrupper.ElementAt(0).Nummer,
                                   Navn = budgetkontogrupper.ElementAt(0).Navn,
                               });
            fixture.Inject(fixture.CreateMany<BudgetoplysningerView>(24));
            fixture.Inject(fixture.CreateMany<BogføringslinjeView>(250));
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            domainObjectBuilder.GetRegnskabCallback = (nummer => regnskaber.Single(m => m.Nummer == nummer));
            domainObjectBuilder.GetBudgetkontogruppeCallback = (nummer => budgetkontogrupper.Single(m => m.Nummer == nummer));

            var view = fixture.CreateMany<BudgetkontoView>(3);
            var budgetkonti = domainObjectBuilder.BuildMany<BudgetkontoView, Budgetkonto>(view);
            Assert.That(budgetkonti, Is.Not.Null);
            Assert.That(budgetkonti.Count(), Is.EqualTo(3));
        }

        /// <summary>
        /// Tester, at Build bygger budgetoplysninger fra et view.
        /// </summary>
        [Test]
        public void TestAtBuildByggerBudgetoplysningerFraKreditoplysningerView()
        {
            var fixture = new Fixture();
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            var view = fixture.CreateAnonymous<BudgetoplysningerView>();
            var budgetoplysninger = domainObjectBuilder.Build<BudgetoplysningerView, Budgetoplysninger>(view);
            Assert.That(budgetoplysninger, Is.Not.Null);
            Assert.That(budgetoplysninger.Budgetkonto, Is.Null);
            Assert.That(budgetoplysninger.År, Is.EqualTo(view.År));
            Assert.That(budgetoplysninger.Måned, Is.EqualTo(view.Måned));
            Assert.That(budgetoplysninger.Indtægter, Is.EqualTo(view.Indtægter));
            Assert.That(budgetoplysninger.Udgifter, Is.EqualTo(view.Udgifter));
        }

        /// <summary>
        /// Tester ,at Build bygger liste af budgetoplysninger fra et view.
        /// </summary>
        [Test]
        public void TestAtBuildByggerListeAfBudgetoplysninger()
        {
            var fixture = new Fixture();
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            var view = fixture.CreateMany<BudgetoplysningerView>(24);
            var budgetoplysninger = domainObjectBuilder.BuildMany<BudgetoplysningerView, Budgetoplysninger>(view);
            Assert.That(budgetoplysninger, Is.Not.Null);
            Assert.That(budgetoplysninger.Count(), Is.EqualTo(24));
        }

        /// <summary>
        /// Tester, at Build bygger bogføringslinje fra et view
        /// </summary>
        [Test]
        public void TestAtBuildByggerBogføringslinjeFraBogføringslinjeView()
        {
            var fixture = new Fixture();
            var konto = fixture.CreateAnonymous<Konto>();
            var budgetkonto = fixture.CreateAnonymous<Budgetkonto>();
            var regnskaber = fixture.CreateMany<Regnskab>(3).ToList();
            regnskaber.ElementAt(0).TilføjKonto(konto);
            regnskaber.ElementAt(0).TilføjKonto(budgetkonto);
            var adresser = fixture.CreateMany<Person>(3).ToList();
            fixture.Inject(new RegnskabListeView
                               {
                                   Nummer = regnskaber.ElementAt(0).Nummer,
                                   Navn = regnskaber.ElementAt(0).Navn
                               });
            fixture.Inject(new KontogruppeView
                               {
                                   Nummer = konto.Kontogruppe.Nummer,
                                   Navn = konto.Kontogruppe.Navn,
                                   KontogruppeType = fixture.CreateAnonymous<DataAccess.Contracts.Enums.KontogruppeType>()
                               });
            fixture.Inject(new KontoListeView
                               {
                                   Regnskab = fixture.CreateAnonymous<RegnskabListeView>(),
                                   Kontonummer = konto.Kontonummer,
                                   Kontonavn = konto.Kontonavn,
                                   Beskrivelse = konto.Beskrivelse,
                                   Note = konto.Note,
                                   Kontogruppe = fixture.CreateAnonymous<KontogruppeView>()
                               });
            fixture.Inject(new BudgetkontogruppeView
                               {
                                   Nummer = budgetkonto.Budgetkontogruppe.Nummer,
                                   Navn = budgetkonto.Budgetkontogruppe.Navn
                               });
            fixture.Inject(new BudgetkontoListeView
                               {
                                   Regnskab = fixture.CreateAnonymous<RegnskabListeView>(),
                                   Kontonummer = budgetkonto.Kontonummer,
                                   Kontonavn = budgetkonto.Kontonavn,
                                   Beskrivelse = budgetkonto.Beskrivelse,
                                   Note = budgetkonto.Note,
                                   Budgetkontogruppe = fixture.CreateAnonymous<BudgetkontogruppeView>()
                               });
            fixture.Inject(new AdressereferenceView
                               {
                                   Nummer = adresser.ElementAt(0).Nummer,
                                   Navn = adresser.ElementAt(0).Navn
                               });
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            domainObjectBuilder.GetRegnskabCallback = (nummer => regnskaber.Single(m => m.Nummer == nummer));
            domainObjectBuilder.GetAdresseBaseCallback = (nummer => adresser.Single(m => m.Nummer == nummer));

            var view = fixture.CreateAnonymous<BogføringslinjeView>();
            var bogføringslinje = domainObjectBuilder.Build<BogføringslinjeView, Bogføringslinje>(view);
            Assert.That(bogføringslinje, Is.Not.Null);
            Assert.That(bogføringslinje.Løbenummer, Is.EqualTo(view.Løbenummer));
            Assert.That(bogføringslinje.Dato, Is.EqualTo(view.Dato));
            Assert.That(bogføringslinje.Bilag, Is.EqualTo(view.Bilag));
            Assert.That(bogføringslinje.Konto, Is.Not.Null);
            Assert.That(bogføringslinje.Konto.Kontonummer, Is.Not.Null);
            Assert.That(bogføringslinje.Konto.Kontonummer, Is.EqualTo(konto.Kontonummer));
            Assert.That(bogføringslinje.Tekst, Is.EqualTo(view.Tekst));
            Assert.That(bogføringslinje.Budgetkonto, Is.Not.Null);
            Assert.That(bogføringslinje.Budgetkonto, Is.Not.Null);
            Assert.That(bogføringslinje.Budgetkonto.Kontonummer, Is.Not.Null);
            Assert.That(bogføringslinje.Budgetkonto.Kontonummer, Is.EqualTo(budgetkonto.Kontonummer));
            Assert.That(bogføringslinje.Debit, Is.EqualTo(view.Debit));
            Assert.That(bogføringslinje.Kredit, Is.EqualTo(view.Kredit));
            Assert.That(bogføringslinje.Adresse, Is.Not.Null);
            Assert.That(bogføringslinje.Adresse.Nummer, Is.EqualTo(view.Adresse.Nummer));
        }

        /// <summary>
        /// Tester, at Build kaster en IntranetRepositoryException ved bygning af bogføringslinje, hvis GetRegnskabCallback ikke er registreret.
        /// </summary>
        [Test]
        public void TestAtBuildKasterIntranetRepositoryExceptionVedBygningAfBogføringslinjeHvisGetRegnskabCallbackIkkeErRegistreret()
        {
            var fixture = new Fixture();
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            var view = fixture.CreateAnonymous<BogføringslinjeView>();
            Assert.Throws<IntranetRepositoryException>(() => domainObjectBuilder.Build<BogføringslinjeView, Bogføringslinje>(view));
        }

        /// <summary>
        /// Tester, at Build kaster en IntranetRepositoryException ved bygning af bogføringslinje, hvis GetRegnskabCallback kaster en IntranetRepositoryException.
        /// </summary>
        [Test]
        public void TestAtBuildKasterIntranetRepositoryExceptionVedBygningAfBogføringslinjeHvisGetRegnskabCallbackKasterIntranetRepositoryException()
        {
            var fixture = new Fixture();
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            domainObjectBuilder.GetRegnskabCallback = (nummer =>
                                                           {
                                                               throw fixture.CreateAnonymous<IntranetRepositoryException>();
                                                           });

            var view = fixture.CreateAnonymous<BogføringslinjeView>();
            Assert.Throws<IntranetRepositoryException>(() => domainObjectBuilder.Build<BogføringslinjeView, Bogføringslinje>(view));
        }

        /// <summary>
        /// Tester, at Build kaster en IntranetRepositoryException ved bygning af bogføringslinje, hvis regnskabet ikke findes.
        /// </summary>
        [Test]
        public void TestAtBuildKasterIntranetRepositoryExceptionVedBygningAfBogføringslinjeHvisRegnskabIkkeFindes()
        {
            var fixture = new Fixture();
            var regnskaber = fixture.CreateMany<Regnskab>(3).ToList();
            fixture.Inject(new RegnskabListeView
                               {
                                   Nummer = -1,
                                   Navn = fixture.CreateAnonymous<string>()
                               });
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            domainObjectBuilder.GetRegnskabCallback = (nummer => regnskaber.Single(m => m.Nummer == nummer));

            var view = fixture.CreateAnonymous<BogføringslinjeView>();
            Assert.Throws<IntranetRepositoryException>(() => domainObjectBuilder.Build<BogføringslinjeView, Bogføringslinje>(view));
        }

        /// <summary>
        /// Tester, at Build kaster en IntranetRepositoryException ved bygning af bogføringslinje, hvis kontoen ikke findes.
        /// </summary>
        [Test]
        public void TestAtBuildKasterIntranetRepositoryExceptionVedBygningAfBogføringslinjeHvisKontoIkkeFindes()
        {
            var fixture = new Fixture();
            var konto = fixture.CreateAnonymous<Konto>();
            var regnskaber = fixture.CreateMany<Regnskab>(3).ToList();
            regnskaber.ElementAt(0).TilføjKonto(konto);
            fixture.Inject(new RegnskabListeView
                               {
                                   Nummer = regnskaber.ElementAt(0).Nummer,
                                   Navn = regnskaber.ElementAt(0).Navn
                               });
            fixture.Inject(new KontoListeView
                               {
                                   Regnskab = fixture.CreateAnonymous<RegnskabListeView>(),
                                   Kontonummer = fixture.CreateAnonymous<string>(),
                                   Kontonavn = fixture.CreateAnonymous<string>(),
                                   Beskrivelse = fixture.CreateAnonymous<string>(),
                                   Note = fixture.CreateAnonymous<string>(),
                                   Kontogruppe = fixture.CreateAnonymous<KontogruppeView>()
                               });
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            domainObjectBuilder.GetRegnskabCallback = (nummer => regnskaber.Single(m => m.Nummer == nummer));

            var view = fixture.CreateAnonymous<BogføringslinjeView>();
            Assert.Throws<IntranetRepositoryException>(() => domainObjectBuilder.Build<BogføringslinjeView, Bogføringslinje>(view));
        }

        /// <summary>
        /// Tester, at Build kaster en IntranetRepositoryException ved bygning af bogføringslinje, hvis budgetkontoen ikke findes.
        /// </summary>
        [Test]
        public void TestAtBuildKasterIntranetRepositoryExceptionVedBygningAfBogføringslinjeHvisBudgetkontoIkkeFindes()
        {
            var fixture = new Fixture();
            var konto = fixture.CreateAnonymous<Konto>();
            var budgetkonto = fixture.CreateAnonymous<Budgetkonto>();
            var regnskaber = fixture.CreateMany<Regnskab>(3).ToList();
            regnskaber.ElementAt(0).TilføjKonto(konto);
            regnskaber.ElementAt(0).TilføjKonto(budgetkonto);
            fixture.Inject(new RegnskabListeView
                               {
                                   Nummer = regnskaber.ElementAt(0).Nummer,
                                   Navn = regnskaber.ElementAt(0).Navn
                               });
            fixture.Inject(new KontogruppeView
                               {
                                   Nummer = konto.Kontogruppe.Nummer,
                                   Navn = konto.Kontogruppe.Navn,
                                   KontogruppeType = fixture.CreateAnonymous<DataAccess.Contracts.Enums.KontogruppeType>()
                               });
            fixture.Inject(new KontoListeView
                               {
                                   Regnskab = fixture.CreateAnonymous<RegnskabListeView>(),
                                   Kontonummer = konto.Kontonummer,
                                   Kontonavn = konto.Kontonavn,
                                   Beskrivelse = konto.Beskrivelse,
                                   Note = konto.Note,
                                   Kontogruppe = fixture.CreateAnonymous<KontogruppeView>()
                               });
            fixture.Inject(new BudgetkontoListeView
                               {
                                   Regnskab = fixture.CreateAnonymous<RegnskabListeView>(),
                                   Kontonummer = fixture.CreateAnonymous<string>(),
                                   Kontonavn = fixture.CreateAnonymous<string>(),
                                   Beskrivelse = fixture.CreateAnonymous<string>(),
                                   Note = fixture.CreateAnonymous<string>(),
                                   Budgetkontogruppe = fixture.CreateAnonymous<BudgetkontogruppeView>()
                               });
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            domainObjectBuilder.GetRegnskabCallback = (nummer => regnskaber.Single(m => m.Nummer == nummer));

            var view = fixture.CreateAnonymous<BogføringslinjeView>();
            Assert.Throws<IntranetRepositoryException>(() => domainObjectBuilder.Build<BogføringslinjeView, Bogføringslinje>(view));
        }

        /// <summary>
        /// Tester, at Build kaster en IntranetRepositoryException ved bygning af bogføringslinje, hvis GetAdresseBaseCallback ikke er registreret.
        /// </summary>
        [Test]
        public void TestAtBuildKasterIntranetRepositoryExceptionVedBygningAfBogføringslinjeHvisGetAdresseBaseCallbackIkkeErRegistreret()
        {
            var fixture = new Fixture();
            var konto = fixture.CreateAnonymous<Konto>();
            var budgetkonto = fixture.CreateAnonymous<Budgetkonto>();
            var regnskaber = fixture.CreateMany<Regnskab>(3).ToList();
            regnskaber.ElementAt(0).TilføjKonto(konto);
            regnskaber.ElementAt(0).TilføjKonto(budgetkonto);
            fixture.Inject(new RegnskabListeView
                               {
                                   Nummer = regnskaber.ElementAt(0).Nummer,
                                   Navn = regnskaber.ElementAt(0).Navn
                               });
            fixture.Inject(new KontogruppeView
                               {
                                   Nummer = konto.Kontogruppe.Nummer,
                                   Navn = konto.Kontogruppe.Navn,
                                   KontogruppeType = fixture.CreateAnonymous<DataAccess.Contracts.Enums.KontogruppeType>()
                               });
            fixture.Inject(new KontoListeView
                               {
                                   Regnskab = fixture.CreateAnonymous<RegnskabListeView>(),
                                   Kontonummer = konto.Kontonummer,
                                   Kontonavn = konto.Kontonavn,
                                   Beskrivelse = konto.Beskrivelse,
                                   Note = konto.Note,
                                   Kontogruppe = fixture.CreateAnonymous<KontogruppeView>()
                               });
            fixture.Inject(new BudgetkontogruppeView
                               {
                                   Nummer = budgetkonto.Budgetkontogruppe.Nummer,
                                   Navn = budgetkonto.Budgetkontogruppe.Navn
                               });
            fixture.Inject(new BudgetkontoListeView
                               {
                                   Regnskab = fixture.CreateAnonymous<RegnskabListeView>(),
                                   Kontonummer = budgetkonto.Kontonummer,
                                   Kontonavn = budgetkonto.Kontonavn,
                                   Beskrivelse = budgetkonto.Beskrivelse,
                                   Note = budgetkonto.Note,
                                   Budgetkontogruppe = fixture.CreateAnonymous<BudgetkontogruppeView>()
                               });
            fixture.Inject(new AdressereferenceView
                               {
                                   Nummer = fixture.CreateAnonymous<int>(),
                                   Navn = fixture.CreateAnonymous<string>()
                               });
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            domainObjectBuilder.GetRegnskabCallback = (nummer => regnskaber.Single(m => m.Nummer == nummer));

            var view = fixture.CreateAnonymous<BogføringslinjeView>();
            Assert.Throws<IntranetRepositoryException>(() => domainObjectBuilder.Build<BogføringslinjeView, Bogføringslinje>(view));
        }

        /// <summary>
        /// Tester, at Build kaster en IntranetRepositoryException ved bygning af bogføringslinje, hvis GetAdresseBaseCallback kaster IntranetRepositoryException.
        /// </summary>
        [Test]
        public void TestAtBuildKasterIntranetRepositoryExceptionVedBygningAfBogføringslinjeHvisGetAdresseBaseCallbackKasterIntranetRepositoryException()
        {
            var fixture = new Fixture();
            var konto = fixture.CreateAnonymous<Konto>();
            var budgetkonto = fixture.CreateAnonymous<Budgetkonto>();
            var regnskaber = fixture.CreateMany<Regnskab>(3).ToList();
            regnskaber.ElementAt(0).TilføjKonto(konto);
            regnskaber.ElementAt(0).TilføjKonto(budgetkonto);
            fixture.Inject(new RegnskabListeView
                               {
                                   Nummer = regnskaber.ElementAt(0).Nummer,
                                   Navn = regnskaber.ElementAt(0).Navn
                               });
            fixture.Inject(new KontogruppeView
                               {
                                   Nummer = konto.Kontogruppe.Nummer,
                                   Navn = konto.Kontogruppe.Navn,
                                   KontogruppeType = fixture.CreateAnonymous<DataAccess.Contracts.Enums.KontogruppeType>()
                               });
            fixture.Inject(new KontoListeView
                               {
                                   Regnskab = fixture.CreateAnonymous<RegnskabListeView>(),
                                   Kontonummer = konto.Kontonummer,
                                   Kontonavn = konto.Kontonavn,
                                   Beskrivelse = konto.Beskrivelse,
                                   Note = konto.Note,
                                   Kontogruppe = fixture.CreateAnonymous<KontogruppeView>()
                               });
            fixture.Inject(new BudgetkontogruppeView
                               {
                                   Nummer = budgetkonto.Budgetkontogruppe.Nummer,
                                   Navn = budgetkonto.Budgetkontogruppe.Navn
                               });
            fixture.Inject(new BudgetkontoListeView
                               {
                                   Regnskab = fixture.CreateAnonymous<RegnskabListeView>(),
                                   Kontonummer = budgetkonto.Kontonummer,
                                   Kontonavn = budgetkonto.Kontonavn,
                                   Beskrivelse = budgetkonto.Beskrivelse,
                                   Note = budgetkonto.Note,
                                   Budgetkontogruppe = fixture.CreateAnonymous<BudgetkontogruppeView>()
                               });
            fixture.Inject(new AdressereferenceView
                               {
                                   Nummer = fixture.CreateAnonymous<int>(),
                                   Navn = fixture.CreateAnonymous<string>()
                               });
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            domainObjectBuilder.GetRegnskabCallback = (nummer => regnskaber.Single(m => m.Nummer == nummer));
            domainObjectBuilder.GetAdresseBaseCallback = (nummer =>
                                                              {
                                                                  throw fixture.CreateAnonymous<IntranetRepositoryException>();
                                                              });

            var view = fixture.CreateAnonymous<BogføringslinjeView>();
            Assert.Throws<IntranetRepositoryException>(() => domainObjectBuilder.Build<BogføringslinjeView, Bogføringslinje>(view));
        }

        /// <summary>
        /// Tester, at Build kaster en IntranetRepositoryException ved bygning af bogføringslinje, hvis adressen ikke findes.
        /// </summary>
        [Test]
        public void TestAtBuildKasterIntranetRepositoryExceptionVedBygningAfBogføringslinjeHvisAdresseBaseIkkeFindes()
        {
            var fixture = new Fixture();
            var konto = fixture.CreateAnonymous<Konto>();
            var budgetkonto = fixture.CreateAnonymous<Budgetkonto>();
            var regnskaber = fixture.CreateMany<Regnskab>(3).ToList();
            regnskaber.ElementAt(0).TilføjKonto(konto);
            regnskaber.ElementAt(0).TilføjKonto(budgetkonto);
            var adresser = fixture.CreateMany<Person>(3).ToList();
            fixture.Inject(new RegnskabListeView
                               {
                                   Nummer = regnskaber.ElementAt(0).Nummer,
                                   Navn = regnskaber.ElementAt(0).Navn
                               });
            fixture.Inject(new KontogruppeView
                               {
                                   Nummer = konto.Kontogruppe.Nummer,
                                   Navn = konto.Kontogruppe.Navn,
                                   KontogruppeType =
                                       fixture.CreateAnonymous<DataAccess.Contracts.Enums.KontogruppeType>()
                               });
            fixture.Inject(new KontoListeView
                               {
                                   Regnskab = fixture.CreateAnonymous<RegnskabListeView>(),
                                   Kontonummer = konto.Kontonummer,
                                   Kontonavn = konto.Kontonavn,
                                   Beskrivelse = konto.Beskrivelse,
                                   Note = konto.Note,
                                   Kontogruppe = fixture.CreateAnonymous<KontogruppeView>()
                               });
            fixture.Inject(new BudgetkontogruppeView
                               {
                                   Nummer = budgetkonto.Budgetkontogruppe.Nummer,
                                   Navn = budgetkonto.Budgetkontogruppe.Navn
                               });
            fixture.Inject(new BudgetkontoListeView
                               {
                                   Regnskab = fixture.CreateAnonymous<RegnskabListeView>(),
                                   Kontonummer = budgetkonto.Kontonummer,
                                   Kontonavn = budgetkonto.Kontonavn,
                                   Beskrivelse = budgetkonto.Beskrivelse,
                                   Note = budgetkonto.Note,
                                   Budgetkontogruppe = fixture.CreateAnonymous<BudgetkontogruppeView>()
                               });
            fixture.Inject(new AdressereferenceView
                               {
                                   Nummer = -1,
                                   Navn = fixture.CreateAnonymous<string>()
                               });
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            domainObjectBuilder.GetRegnskabCallback = (nummer => regnskaber.Single(m => m.Nummer == nummer));
            domainObjectBuilder.GetAdresseBaseCallback = (nummer => adresser.Single(m => m.Nummer == nummer));

            var view = fixture.CreateAnonymous<BogføringslinjeView>();
            Assert.Throws<IntranetRepositoryException>(() => domainObjectBuilder.Build<BogføringslinjeView, Bogføringslinje>(view));
        }

        /// <summary>
        /// Tester, at Build bygger liste af bogføringslinje fra et view
        /// </summary>
        [Test]
        public void TestAtBuildByggerListeAfBogføringslinje()
        {
            var fixture = new Fixture();
            var konto = fixture.CreateAnonymous<Konto>();
            var budgetkonto = fixture.CreateAnonymous<Budgetkonto>();
            var regnskaber = fixture.CreateMany<Regnskab>(3).ToList();
            regnskaber.ElementAt(0).TilføjKonto(konto);
            regnskaber.ElementAt(0).TilføjKonto(budgetkonto);
            var adresser = fixture.CreateMany<Person>(3).ToList();
            fixture.Inject(new RegnskabListeView
                               {
                                   Nummer = regnskaber.ElementAt(0).Nummer,
                                   Navn = regnskaber.ElementAt(0).Navn
                               });
            fixture.Inject(new KontogruppeView
                               {
                                   Nummer = konto.Kontogruppe.Nummer,
                                   Navn = konto.Kontogruppe.Navn,
                                   KontogruppeType = fixture.CreateAnonymous<DataAccess.Contracts.Enums.KontogruppeType>()
                               });
            fixture.Inject(new KontoListeView
                               {
                                   Regnskab = fixture.CreateAnonymous<RegnskabListeView>(),
                                   Kontonummer = konto.Kontonummer,
                                   Kontonavn = konto.Kontonavn,
                                   Beskrivelse = konto.Beskrivelse,
                                   Note = konto.Note,
                                   Kontogruppe = fixture.CreateAnonymous<KontogruppeView>()
                               });
            fixture.Inject(new BudgetkontogruppeView
                               {
                                   Nummer = budgetkonto.Budgetkontogruppe.Nummer,
                                   Navn = budgetkonto.Budgetkontogruppe.Navn
                               });
            fixture.Inject(new BudgetkontoListeView
                               {
                                   Regnskab = fixture.CreateAnonymous<RegnskabListeView>(),
                                   Kontonummer = budgetkonto.Kontonummer,
                                   Kontonavn = budgetkonto.Kontonavn,
                                   Beskrivelse = budgetkonto.Beskrivelse,
                                   Note = budgetkonto.Note,
                                   Budgetkontogruppe = fixture.CreateAnonymous<BudgetkontogruppeView>()
                               });
            fixture.Inject(new AdressereferenceView
                               {
                                   Nummer = adresser.ElementAt(0).Nummer,
                                   Navn = adresser.ElementAt(0).Navn
                               });
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            domainObjectBuilder.GetRegnskabCallback = (nummer => regnskaber.Single(m => m.Nummer == nummer));
            domainObjectBuilder.GetAdresseBaseCallback = (nummer => adresser.Single(m => m.Nummer == nummer));

            var view = fixture.CreateMany<BogføringslinjeView>(250);
            var bogføringslinjer = domainObjectBuilder.BuildMany<BogføringslinjeView, Bogføringslinje>(view);
            Assert.That(bogføringslinjer, Is.Not.Null);
            Assert.That(bogføringslinjer.Count(), Is.EqualTo(250));
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
            Assert.Throws<IntranetRepositoryException>(() => domainObjectBuilder.Build<KontogruppeView, Kontogruppe>(view));
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
            var kontogrupper = domainObjectBuilder.BuildMany<KontogruppeView, Kontogruppe>(view);
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
            var budgetkontogrupper = domainObjectBuilder.BuildMany<BudgetkontogruppeView, Budgetkontogruppe>(view);
            Assert.That(budgetkontogrupper, Is.Not.Null);
            Assert.That(budgetkontogrupper.Count(), Is.EqualTo(3));
        }

        /// <summary>
        /// Tester, at Build bygger brevhoved fra et view.
        /// </summary>
        [Test]
        public void TestAtBuildByggerBrevhovedFraBrevhovedView()
        {
            var fixture = new Fixture();
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            var view = fixture.CreateAnonymous<BrevhovedView>();
            var brevhoved = domainObjectBuilder.Build<BrevhovedView, Brevhoved>(view);
            Assert.That(brevhoved, Is.Not.Null);
            Assert.That(brevhoved.Nummer, Is.EqualTo(view.Nummer));
            Assert.That(brevhoved.Navn, Is.Not.Null);
            Assert.That(brevhoved.Navn, Is.EqualTo(view.Navn));
            Assert.That(brevhoved.Linje1, Is.EqualTo(view.Linje1));
            Assert.That(brevhoved.Linje2, Is.EqualTo(view.Linje2));
            Assert.That(brevhoved.Linje3, Is.EqualTo(view.Linje3));
            Assert.That(brevhoved.Linje4, Is.EqualTo(view.Linje4));
            Assert.That(brevhoved.Linje5, Is.EqualTo(view.Linje5));
            Assert.That(brevhoved.Linje6, Is.EqualTo(view.Linje6));
            Assert.That(brevhoved.Linje7, Is.EqualTo(view.Linje7));
            Assert.That(brevhoved.CvrNr, Is.EqualTo(view.CvrNr));
        }

        /// <summary>
        /// Tester, at Build bygger liste af brevhoveder fra et view.
        /// </summary>
        [Test]
        public void TestAtBuildByggerListeAfBrevhoveder()
        {
            var fixture = new Fixture();
            var domainObjectBuilder = new DomainObjectBuilder();
            Assert.That(domainObjectBuilder, Is.Not.Null);

            var view = fixture.CreateMany<BrevhovedView>(3);
            var brevhoveder = domainObjectBuilder.BuildMany<BrevhovedView, Brevhoved>(view);
            Assert.That(brevhoveder, Is.Not.Null);
            Assert.That(brevhoveder.Count(), Is.EqualTo(3));
        }
    }
}
