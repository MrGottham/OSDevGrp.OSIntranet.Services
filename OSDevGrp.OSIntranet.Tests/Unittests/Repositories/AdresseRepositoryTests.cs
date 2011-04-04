﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf.ChannelFactory;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Queries;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Services;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Views;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories;
using NUnit.Framework;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories
{
    /// <summary>
    /// Test af repository til adressekartoteket.
    /// </summary>
    [TestFixture]
    public class AdresseRepositoryTests
    {
        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis ChannelFactory er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisChannelFactoryErNull()
        {
            Assert.Throws<ArgumentNullException>(() => new AdresseRepository(null));
        }

        /// <summary>
        /// Tester, at AdresseGetAll henter adresser.
        /// </summary>
        [Test]
        public void TestAtAdresseGetAllHenterAdresser()
        {
            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IAdresseRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.PersonGetAll(Arg<PersonGetAllQuery>.Is.Anything))
                .Return(GetPersoner());
            service.Expect(m => m.FirmaGetAll(Arg<FirmaGetAllQuery>.Is.Anything))
                .Return(GetFirmaer());
            service.Expect(m => m.AdressegruppeGetAll(Arg<AdressegruppeGetAllQuery>.Is.Anything))
                .Return(GetAdressegrupper());
            service.Expect(m => m.BetalingsbetingelseGetAll(Arg<BetalingsbetingelseGetAllQuery>.Is.Anything))
                .Return(GetBetalingsbetingelser());
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed).Repeat.Any();
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IAdresseRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var repository = new AdresseRepository(channelFactory);
            var adresser = repository.AdresseGetAll();
            Assert.That(adresser, Is.Not.Null);
            Assert.That(adresser.OfType<Firma>().Count(), Is.EqualTo(1));
            Assert.That(adresser.OfType<Person>().Count(), Is.EqualTo(2));
        }

        /// <summary>
        /// Tester, at en person mappes korrekt.
        /// </summary>
        [Test]
        public void TestAtPersonMappesKorrekt()
        {
            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IAdresseRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.PersonGetAll(Arg<PersonGetAllQuery>.Is.Anything))
                .Return(GetPersoner());
            service.Expect(m => m.FirmaGetAll(Arg<FirmaGetAllQuery>.Is.Anything))
                .Return(GetFirmaer());
            service.Expect(m => m.AdressegruppeGetAll(Arg<AdressegruppeGetAllQuery>.Is.Anything))
                .Return(GetAdressegrupper());
            service.Expect(m => m.BetalingsbetingelseGetAll(Arg<BetalingsbetingelseGetAllQuery>.Is.Anything))
                .Return(GetBetalingsbetingelser());
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed).Repeat.Any();
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IAdresseRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var repository = new AdresseRepository(channelFactory);
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
            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IAdresseRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.PersonGetAll(Arg<PersonGetAllQuery>.Is.Anything))
                .Return(GetPersoner());
            service.Expect(m => m.FirmaGetAll(Arg<FirmaGetAllQuery>.Is.Anything))
                .Return(GetFirmaer());
            service.Expect(m => m.AdressegruppeGetAll(Arg<AdressegruppeGetAllQuery>.Is.Anything))
                .Return(GetAdressegrupper());
            service.Expect(m => m.BetalingsbetingelseGetAll(Arg<BetalingsbetingelseGetAllQuery>.Is.Anything))
                .Return(GetBetalingsbetingelser());
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed).Repeat.Any();
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IAdresseRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var repository = new AdresseRepository(channelFactory);
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
        /// Tester, at AdresseGetAll kaster IntranetRepositoryException ved IntranetRepositoryException.
        /// </summary>
        [Test]
        public void TestAtAdresseGetAllKasterIntranetRepositoryExceptionVedIntranetRepositoryException()
        {
            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IAdresseRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.PersonGetAll(Arg<PersonGetAllQuery>.Is.Anything))
                .Throw(new IntranetRepositoryException("Test"));
            service.Expect(m => m.FirmaGetAll(Arg<FirmaGetAllQuery>.Is.Anything))
                .Throw(new IntranetRepositoryException("Test"));
            service.Expect(m => m.AdressegruppeGetAll(Arg<AdressegruppeGetAllQuery>.Is.Anything))
                .Return(GetAdressegrupper());
            service.Expect(m => m.BetalingsbetingelseGetAll(Arg<BetalingsbetingelseGetAllQuery>.Is.Anything))
                .Return(GetBetalingsbetingelser());
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed).Repeat.Any();
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IAdresseRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var repository = new AdresseRepository(channelFactory);
            Assert.Throws<IntranetRepositoryException>(() => repository.AdresseGetAll());
        }

        /// <summary>
        /// Tester, at AdresseGetAll kaster IntranetRepositoryException ved FaultException.
        /// </summary>
        [Test]
        public void TestAtAdresseGetAllKasterIntranetRepositoryExceptionVedFaultException()
        {
            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IAdresseRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.PersonGetAll(Arg<PersonGetAllQuery>.Is.Anything))
                .Throw(new FaultException("Test"));
            service.Expect(m => m.FirmaGetAll(Arg<FirmaGetAllQuery>.Is.Anything))
                .Throw(new FaultException("Test"));
            service.Expect(m => m.AdressegruppeGetAll(Arg<AdressegruppeGetAllQuery>.Is.Anything))
                .Return(GetAdressegrupper());
            service.Expect(m => m.BetalingsbetingelseGetAll(Arg<BetalingsbetingelseGetAllQuery>.Is.Anything))
                .Return(GetBetalingsbetingelser());
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed).Repeat.Any();
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IAdresseRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var repository = new AdresseRepository(channelFactory);
            Assert.Throws<IntranetRepositoryException>(() => repository.AdresseGetAll());
        }

        /// <summary>
        /// Tester, at AdresseGetAll kaster IntranetRepositoryException ved Exception.
        /// </summary>
        [Test]
        public void TestAtAdresseGetAllKasterIntranetRepositoryExceptionVedException()
        {
            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IAdresseRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.PersonGetAll(Arg<PersonGetAllQuery>.Is.Anything))
                .Throw(new Exception("Test"));
            service.Expect(m => m.FirmaGetAll(Arg<FirmaGetAllQuery>.Is.Anything))
                .Throw(new Exception("Test"));
            service.Expect(m => m.AdressegruppeGetAll(Arg<AdressegruppeGetAllQuery>.Is.Anything))
                .Return(GetAdressegrupper());
            service.Expect(m => m.BetalingsbetingelseGetAll(Arg<BetalingsbetingelseGetAllQuery>.Is.Anything))
                .Return(GetBetalingsbetingelser());
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed).Repeat.Any();
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IAdresseRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var repository = new AdresseRepository(channelFactory);
            Assert.Throws<IntranetRepositoryException>(() => repository.AdresseGetAll());
        }

        /// <summary>
        /// Tester, at PostnummerGetAll henter postnumre.
        /// </summary>
        [Test]
        public void TestAtPostnummerGetAllHenterPostnumre()
        {
            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IAdresseRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.PostnummerGetAll(Arg<PostnummerGetAllQuery>.Is.Anything))
                .Return(GetPostnumre());
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed).Repeat.Any();
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IAdresseRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var repository = new AdresseRepository(channelFactory);
            var postnumre = repository.PostnummerGetAll();
            Assert.That(postnumre, Is.Not.Null);
            Assert.That(postnumre.Count, Is.EqualTo(3));
        }

        /// <summary>
        /// Tester, at postnumre mappes korrekt.
        /// </summary>
        [Test]
        public void TestAtPostnummerMappesKorrekt()
        {
            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IAdresseRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.PostnummerGetAll(Arg<PostnummerGetAllQuery>.Is.Anything))
                .Return(GetPostnumre());
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed).Repeat.Any();
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IAdresseRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var repository = new AdresseRepository(channelFactory);
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
        /// Tester, at PostnummerGetAll kaster IntranetRepositoryException ved IntranetRepositoryException.
        /// </summary>
        [Test]
        public void TestAtPostnummerGetAllKasterIntranetRepositoryExceptionVedIntranetRepositoryException()
        {
            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IAdresseRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.PostnummerGetAll(Arg<PostnummerGetAllQuery>.Is.Anything))
                .Throw(new IntranetRepositoryException("Test"));
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed).Repeat.Any();
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IAdresseRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var repository = new AdresseRepository(channelFactory);
            Assert.Throws<IntranetRepositoryException>(() => repository.PostnummerGetAll());
        }

        /// <summary>
        /// Tester, at PostnummerGetAll kaster IntranetRepositoryException ved FaultException.
        /// </summary>
        [Test]
        public void TestAtPostnummerGetAllKasterIntranetRepositoryExceptionVedFaultException()
        {
            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IAdresseRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.PostnummerGetAll(Arg<PostnummerGetAllQuery>.Is.Anything))
                .Throw(new FaultException("Test"));
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed).Repeat.Any();
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IAdresseRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var repository = new AdresseRepository(channelFactory);
            Assert.Throws<IntranetRepositoryException>(() => repository.PostnummerGetAll());
        }

        /// <summary>
        /// Tester, at PostnummerGetAll kaster IntranetRepositoryException ved Exception.
        /// </summary>
        [Test]
        public void TestAtPostnummerGetAllKasterIntranetRepositoryExceptionVedException()
        {
            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IAdresseRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.PostnummerGetAll(Arg<PostnummerGetAllQuery>.Is.Anything))
                .Throw(new Exception("Test"));
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed).Repeat.Any();
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IAdresseRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var repository = new AdresseRepository(channelFactory);
            Assert.Throws<IntranetRepositoryException>(() => repository.PostnummerGetAll());
        }

        /// <summary>
        /// Tester, at AdressegruppeGetAll henter alle adressegrupper.
        /// </summary>
        [Test]
        public void TestAtAdressegruppeGetAllHenterAdressegrupper()
        {
            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IAdresseRepositoryService>(new[] {typeof (ICommunicationObject)});
            service.Expect(m => m.AdressegruppeGetAll(Arg<AdressegruppeGetAllQuery>.Is.Anything))
                .Return(GetAdressegrupper());
            Expect.Call(((ICommunicationObject) service).State).Return(CommunicationState.Closed).Repeat.Any();
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IAdresseRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var repository = new AdresseRepository(channelFactory);
            var adressegrupper = repository.AdressegruppeGetAll();
            Assert.That(adressegrupper, Is.Not.Null);
            Assert.That(adressegrupper.Count, Is.EqualTo(3));
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
        }

        /// <summary>
        /// Tester, at AdressegruppeGetAll kaster en IntranetRepositoryException ved IntranetRepositoryException.
        /// </summary>
        [Test]
        public void TestAtAdressegruppeGetAllKasterIntranetRepositoryExceptionVedIntranetRepositoryException()
        {
            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IAdresseRepositoryService>(new[] {typeof (ICommunicationObject)});
            service.Expect(m => m.AdressegruppeGetAll(Arg<AdressegruppeGetAllQuery>.Is.Anything))
                .Throw(new IntranetRepositoryException("Test"));
            Expect.Call(((ICommunicationObject) service).State).Return(CommunicationState.Closed).Repeat.Any();
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IAdresseRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var repository = new AdresseRepository(channelFactory);
            Assert.Throws<IntranetRepositoryException>(() => repository.AdressegruppeGetAll());
        }

        /// <summary>
        /// Tester, at AdressegruppeGetAll kaster en IntranetRepositoryException ved FaultException.
        /// </summary>
        [Test]
        public void TestAtAdressegruppeGetAllKasterIntranetRepositoryExceptionVedFaultException()
        {
            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IAdresseRepositoryService>(new[] {typeof (ICommunicationObject)});
            service.Expect(m => m.AdressegruppeGetAll(Arg<AdressegruppeGetAllQuery>.Is.Anything))
                .Throw(new FaultException("Test"));
            Expect.Call(((ICommunicationObject) service).State).Return(CommunicationState.Closed).Repeat.Any();
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IAdresseRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var repository = new AdresseRepository(channelFactory);
            Assert.Throws<IntranetRepositoryException>(() => repository.AdressegruppeGetAll());
        }

        /// <summary>
        /// Tester, at AdressegruppeGetAll kaster en IntranetRepositoryException ved Exception.
        /// </summary>
        [Test]
        public void TestAtAdressegruppeGetAllKasterIntranetRepositoryExceptionVedException()
        {
            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IAdresseRepositoryService>(new[] {typeof (ICommunicationObject)});
            service.Expect(m => m.AdressegruppeGetAll(Arg<AdressegruppeGetAllQuery>.Is.Anything))
                .Throw(new Exception("Test"));
            Expect.Call(((ICommunicationObject) service).State).Return(CommunicationState.Closed).Repeat.Any();
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IAdresseRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var repository = new AdresseRepository(channelFactory);
            Assert.Throws<IntranetRepositoryException>(() => repository.AdressegruppeGetAll());
        }

        /// <summary>
        /// Tester, at BetalingsbetingelseGetAll henter alle betalingsbetingelser.
        /// </summary>
        [Test]
        public void TestAtBetalingsbetingelseGetAllHenterBetalingsbetingelser()
        {
            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IAdresseRepositoryService>(new[] {typeof (ICommunicationObject)});
            service.Expect(m => m.BetalingsbetingelseGetAll(Arg<BetalingsbetingelseGetAllQuery>.Is.Anything))
                .Return(GetBetalingsbetingelser());
            Expect.Call(((ICommunicationObject) service).State).Return(CommunicationState.Closed).Repeat.Any();
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IAdresseRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var repository = new AdresseRepository(channelFactory);
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

        /// <summary>
        /// Tester, at BetalingsbetingelserGetAll kaster en IntranetRepositoryException ved IntranetRepositoryException.
        /// </summary>
        [Test]
        public void TestAtBetalingsbetingelserGetAllKasterIntranetRepositoryExceptionVedIntranetRepositoryException()
        {
            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IAdresseRepositoryService>(new[] {typeof (ICommunicationObject)});
            service.Expect(m => m.BetalingsbetingelseGetAll(Arg<BetalingsbetingelseGetAllQuery>.Is.Anything))
                .Throw(new IntranetRepositoryException("Test"));
            Expect.Call(((ICommunicationObject) service).State).Return(CommunicationState.Closed).Repeat.Any();
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IAdresseRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var repository = new AdresseRepository(channelFactory);
            Assert.Throws<IntranetRepositoryException>(() => repository.BetalingsbetingelseGetAll());
        }

        /// <summary>
        /// Tester, at BetalingsbetingelserGetAll kaster en IntranetRepositoryException ved FaultException.
        /// </summary>
        [Test]
        public void TestAtBetalingsbetingelserGetAllKasterIntranetRepositoryExceptionVedFaultException()
        {
            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IAdresseRepositoryService>(new[] {typeof (ICommunicationObject)});
            service.Expect(m => m.BetalingsbetingelseGetAll(Arg<BetalingsbetingelseGetAllQuery>.Is.Anything))
                .Throw(new FaultException("Test"));
            Expect.Call(((ICommunicationObject) service).State).Return(CommunicationState.Closed).Repeat.Any();
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IAdresseRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var repository = new AdresseRepository(channelFactory);
            Assert.Throws<IntranetRepositoryException>(() => repository.BetalingsbetingelseGetAll());
        }

        /// <summary>
        /// Tester, at BetalingsbetingelserGetAll kaster en IntranetRepositoryException ved Exception.
        /// </summary>
        [Test]
        public void TestAtBetalingsbetingelserGetAllKasterIntranetRepositoryExceptionVedException()
        {
            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IAdresseRepositoryService>(new[] {typeof (ICommunicationObject)});
            service.Expect(m => m.BetalingsbetingelseGetAll(Arg<BetalingsbetingelseGetAllQuery>.Is.Anything))
                .Throw(new Exception("Test"));
            Expect.Call(((ICommunicationObject) service).State).Return(CommunicationState.Closed).Repeat.Any();
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IAdresseRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var repository = new AdresseRepository(channelFactory);
            Assert.Throws<IntranetRepositoryException>(() => repository.BetalingsbetingelseGetAll());
        }

        /// <summary>
        /// Tester, at MapFirma kaster en ArgumentNullException, hvis firmaviewet er null.
        /// </summary>
        [Test]
        public void TestAtMapFirmaKasterArgumentNullExceptionHvisFirmaViewErNull()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var repository = new AdresseRepository(channelFactory);
            var method = repository.GetType().GetMethod("MapFirma", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.That(method, Is.Not.Null);
            try
            {
                method.Invoke(repository, new object[] {null, null, null});
            }
            catch (TargetInvocationException ex)
            {
                Assert.That(ex.InnerException, Is.Not.Null);
                Assert.That(ex.InnerException, Is.TypeOf(typeof(ArgumentNullException)));
            }
        }

        /// <summary>
        /// Tester, at MapFirma kaster en ArgumentNullException, hvis adressegrupper er null.
        /// </summary>
        [Test]
        public void TestAtMapFirmaKasterArgumentNullExceptionHvisAdressegrupperErNull()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var repository = new AdresseRepository(channelFactory);
            var method = repository.GetType().GetMethod("MapFirma", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.That(method, Is.Not.Null);
            try
            {
                method.Invoke(repository, new object[] { new FirmaView(), null, null });
            }
            catch (TargetInvocationException ex)
            {
                Assert.That(ex.InnerException, Is.Not.Null);
                Assert.That(ex.InnerException, Is.TypeOf(typeof(ArgumentNullException)));
            }
        }

        /// <summary>
        /// Tester, at MapFirma kaster en ArgumentNullException, hvis adressegrupper er null.
        /// </summary>
        [Test]
        public void TestAtMapFirmaKasterArgumentNullExceptionHvisBetalingsbetingelserErNull()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var repository = new AdresseRepository(channelFactory);
            var method = repository.GetType().GetMethod("MapFirma", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.That(method, Is.Not.Null);
            try
            {
                method.Invoke(repository, new object[] { new FirmaView(), new List<Adressegruppe>(), null });
            }
            catch (TargetInvocationException ex)
            {
                Assert.That(ex.InnerException, Is.Not.Null);
                Assert.That(ex.InnerException, Is.TypeOf(typeof(ArgumentNullException)));
            }
        }

        /// <summary>
        /// Tester, at MapFirma kaster en IntranetRepositoryException, hvis adressegruppen ikke findes.
        /// </summary>
        [Test]
        public void TestAtMapFirmaKasterIntranetRepositoryExceptionHvisAdressegruppeIkkeFindes()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var repository = new AdresseRepository(channelFactory);
            var method = repository.GetType().GetMethod("MapFirma", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.That(method, Is.Not.Null);
            try
            {
                var firmaView = new FirmaView
                                    {
                                        Nummer = 1,
                                        Navn = "DSI DATA A/S",
                                        Adressegruppe = new AdressegruppeView
                                                            {
                                                                Nummer = 3
                                                            }
                                    };
                var adressegrupper = new List<Adressegruppe>
                                         {
                                             new Adressegruppe(1, "Familie", 1),
                                             new Adressegruppe(2, "Venner og veninder", 2)
                                         };
                var betalingsbetingelser = new List<Betalingsbetingelse>
                                               {
                                                   new Betalingsbetingelse(1, "Kontant"),
                                                   new Betalingsbetingelse(2, "Netto + 8 dage"),
                                               };
                method.Invoke(repository, new object[] { firmaView, adressegrupper, betalingsbetingelser });
            }
            catch (TargetInvocationException ex)
            {
                Assert.That(ex.InnerException, Is.Not.Null);
                Assert.That(ex.InnerException, Is.TypeOf(typeof(IntranetRepositoryException)));
            }
        }

        /// <summary>
        /// Tester, at MapFirma kaster en IntranetRepositoryException, hvis betalingsbetingelsen ikke findes.
        /// </summary>
        [Test]
        public void TestAtMapFirmaKasterIntranetRepositoryExceptionHvisBetalingsbetingelseIkkeFindes()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var repository = new AdresseRepository(channelFactory);
            var method = repository.GetType().GetMethod("MapFirma", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.That(method, Is.Not.Null);
            try
            {
                var firmaView = new FirmaView
                                    {
                                        Nummer = 1,
                                        Navn = "DSI DATA A/S",
                                        Adressegruppe = new AdressegruppeView
                                                            {
                                                                Nummer = 1
                                                            },
                                        Betalingsbetingelse = new BetalingsbetingelseView
                                                                  {
                                                                      Nummer = 3
                                                                  }
                                    };
                var adressegrupper = new List<Adressegruppe>
                                         {
                                             new Adressegruppe(1, "Familie", 1),
                                             new Adressegruppe(2, "Venner og veninder", 2)
                                         };
                var betalingsbetingelser = new List<Betalingsbetingelse>
                                               {
                                                   new Betalingsbetingelse(1, "Kontant"),
                                                   new Betalingsbetingelse(2, "Netto + 8 dage"),
                                               };
                method.Invoke(repository, new object[] { firmaView, adressegrupper, betalingsbetingelser });
            }
            catch (TargetInvocationException ex)
            {
                Assert.That(ex.InnerException, Is.Not.Null);
                Assert.That(ex.InnerException, Is.TypeOf(typeof(IntranetRepositoryException)));
            }
        }

        /// <summary>
        /// Tester, at MapPerson kaster en ArgumentNullException, hvis personviewet er null.
        /// </summary>
        [Test]
        public void TestAtMapPersonKasterArgumentNullExceptionHvisPersonViewErNull()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var repository = new AdresseRepository(channelFactory);
            var method = repository.GetType().GetMethod("MapPerson", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.That(method, Is.Not.Null);
            try
            {
                method.Invoke(repository, new object[] { null, null, null, null });
            }
            catch (TargetInvocationException ex)
            {
                Assert.That(ex.InnerException, Is.Not.Null);
                Assert.That(ex.InnerException, Is.TypeOf(typeof(ArgumentNullException)));
            }
        }

        /// <summary>
        /// Tester, at MapPerson kaster en ArgumentNullException, hvis personviewet er null.
        /// </summary>
        [Test]
        public void TestAtMapPersonKasterArgumentNullExceptionHvisFirmaerErNull()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var repository = new AdresseRepository(channelFactory);
            var method = repository.GetType().GetMethod("MapPerson", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.That(method, Is.Not.Null);
            try
            {
                method.Invoke(repository, new object[] { new PersonView(), null, null, null });
            }
            catch (TargetInvocationException ex)
            {
                Assert.That(ex.InnerException, Is.Not.Null);
                Assert.That(ex.InnerException, Is.TypeOf(typeof(ArgumentNullException)));
            }
        }

        /// <summary>
        /// Tester, at MapPerson kaster en ArgumentNullException, hvis adressegrupper er null.
        /// </summary>
        [Test]
        public void TestAtMapPersonKasterArgumentNullExceptionHvisAdressegrupperErNull()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var repository = new AdresseRepository(channelFactory);
            var method = repository.GetType().GetMethod("MapPerson", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.That(method, Is.Not.Null);
            try
            {
                method.Invoke(repository, new object[] { new PersonView(), new List<Firma>(), null, null });
            }
            catch (TargetInvocationException ex)
            {
                Assert.That(ex.InnerException, Is.Not.Null);
                Assert.That(ex.InnerException, Is.TypeOf(typeof(ArgumentNullException)));
            }
        }

        /// <summary>
        /// Tester, at MapPerson kaster en ArgumentNullException, hvis betalingsbetingelser er null.
        /// </summary>
        [Test]
        public void TestAtMapPersonKasterArgumentNullExceptionHvisBetalingsbetingelserErNull()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var repository = new AdresseRepository(channelFactory);
            var method = repository.GetType().GetMethod("MapPerson", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.That(method, Is.Not.Null);
            try
            {
                method.Invoke(repository, new object[] { new PersonView(), new List<Firma>(), new List<Adressegruppe>(), null });
            }
            catch (TargetInvocationException ex)
            {
                Assert.That(ex.InnerException, Is.Not.Null);
                Assert.That(ex.InnerException, Is.TypeOf(typeof(ArgumentNullException)));
            }
        }

        /// <summary>
        /// Tester, at MapPerson kaster en IntranetRepositoryException, hvis adressegruppen ikke findes.
        /// </summary>
        [Test]
        public void TestAtMapPersonKasterIntranetRepositoryExceptionHvisAdressegruppeIkkeFindes()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var repository = new AdresseRepository(channelFactory);
            var method = repository.GetType().GetMethod("MapPerson", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.That(method, Is.Not.Null);
            try
            {
                var personView = new PersonView
                                     {
                                         Nummer = 1,
                                         Navn = "Ole Sørensen",
                                         Adressegruppe = new AdressegruppeView
                                                             {
                                                                 Nummer = 3
                                                             }
                                     };
                var adressegrupper = new List<Adressegruppe>
                                         {
                                             new Adressegruppe(1, "Familie", 1),
                                             new Adressegruppe(2, "Venner og veninder", 2)
                                         };
                var betalingsbetingelser = new List<Betalingsbetingelse>
                                               {
                                                   new Betalingsbetingelse(1, "Kontant"),
                                                   new Betalingsbetingelse(2, "Netto + 8 dage"),
                                               };
                method.Invoke(repository, new object[] {personView, new List<Firma>(), adressegrupper, betalingsbetingelser});
            }
            catch (TargetInvocationException ex)
            {
                Assert.That(ex.InnerException, Is.Not.Null);
                Assert.That(ex.InnerException, Is.TypeOf(typeof(IntranetRepositoryException)));
            }
        }

        /// <summary>
        /// Tester, at MapPerson kaster en IntranetRepositoryException, hvis betalingsbetingelsen ikke findes.
        /// </summary>
        [Test]
        public void TestAtMapPersonKasterIntranetRepositoryExceptionHvisBetalingsbetingelseIkkeFindes()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var repository = new AdresseRepository(channelFactory);
            var method = repository.GetType().GetMethod("MapPerson", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.That(method, Is.Not.Null);
            try
            {
                var personView = new PersonView
                                     {
                                         Nummer = 1,
                                         Navn = "Ole Sørensen",
                                         Adressegruppe = new AdressegruppeView
                                                             {
                                                                 Nummer = 1
                                                             },
                                         Betalingsbetingelse = new BetalingsbetingelseView
                                                                   {
                                                                       Nummer = 3
                                                                   }
                                     };
                var adressegrupper = new List<Adressegruppe>
                                         {
                                             new Adressegruppe(1, "Familie", 1),
                                             new Adressegruppe(2, "Venner og veninder", 2)
                                         };
                var betalingsbetingelser = new List<Betalingsbetingelse>
                                               {
                                                   new Betalingsbetingelse(1, "Kontant"),
                                                   new Betalingsbetingelse(2, "Netto + 8 dage"),
                                               };
                method.Invoke(repository, new object[] { personView, new List<Firma>(), adressegrupper, betalingsbetingelser });
            }
            catch (TargetInvocationException ex)
            {
                Assert.That(ex.InnerException, Is.Not.Null);
                Assert.That(ex.InnerException, Is.TypeOf(typeof(IntranetRepositoryException)));
            }
        }

        /// <summary>
        /// Tester, at MapPerson kaster en IntranetRepositoryException, hvis firmaet ikke findes.
        /// </summary>
        [Test]
        public void TestAtMapPersonKasterIntranetRepositoryExceptionHvisFirmaIkkeFindes()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var repository = new AdresseRepository(channelFactory);
            var method = repository.GetType().GetMethod("MapPerson", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.That(method, Is.Not.Null);
            try
            {
                var personView = new PersonView
                                     {
                                         Nummer = 1,
                                         Navn = "Ole Sørensen",
                                         Adressegruppe = new AdressegruppeView
                                                             {
                                                                 Nummer = 1
                                                             },
                                         Betalingsbetingelse = new BetalingsbetingelseView
                                                                   {
                                                                       Nummer = 2
                                                                   },
                                         Firma = new AdressereferenceView
                                         {
                                             Nummer = 48
                                         }
                                     };
                var adressegrupper = new List<Adressegruppe>
                                         {
                                             new Adressegruppe(1, "Familie", 1),
                                             new Adressegruppe(2, "Venner og veninder", 2)
                                         };
                var betalingsbetingelser = new List<Betalingsbetingelse>
                                               {
                                                   new Betalingsbetingelse(1, "Kontant"),
                                                   new Betalingsbetingelse(2, "Netto + 8 dage"),
                                               };
                method.Invoke(repository, new object[] { personView, new List<Firma>(), adressegrupper, betalingsbetingelser });
            }
            catch (TargetInvocationException ex)
            {
                Assert.That(ex.InnerException, Is.Not.Null);
                Assert.That(ex.InnerException, Is.TypeOf(typeof(IntranetRepositoryException)));
            }
        }

        /// <summary>
        /// Tester, at MapPostnummer kaster en ArgumentNullException, hvis postnummerview er null.
        /// </summary>
        [Test]
        public void TestAtMapPostnummerKasterArgumentNullExceptionHvisAdressegrupperErNull()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var repository = new AdresseRepository(channelFactory);
            var method = repository.GetType().GetMethod("MapPostnummer", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.That(method, Is.Not.Null);
            try
            {
                method.Invoke(repository, new object[] { null });
            }
            catch (TargetInvocationException ex)
            {
                Assert.That(ex.InnerException, Is.Not.Null);
                Assert.That(ex.InnerException, Is.TypeOf(typeof(ArgumentNullException)));
            }
        }

        /// <summary>
        /// Tester, at MapAdressegruppe kaster en ArgumentNullException, hvis adressegruppeview er null.
        /// </summary>
        [Test]
        public void TestAtMapAdressegruppeKasterArgumentNullExceptionHvisAdressegrupperErNull()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var repository = new AdresseRepository(channelFactory);
            var method = repository.GetType().GetMethod("MapAdressegruppe", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.That(method, Is.Not.Null);
            try
            {
                method.Invoke(repository, new object[] { null });
            }
            catch (TargetInvocationException ex)
            {
                Assert.That(ex.InnerException, Is.Not.Null);
                Assert.That(ex.InnerException, Is.TypeOf(typeof(ArgumentNullException)));
            }
        }

        /// <summary>
        /// Tester, at MapBetalingsbetingelse kaster en ArgumentNullException, hvis betalingsbetingelsesview er null.
        /// </summary>
        [Test]
        public void TestAtMapBetalingsbetingelseKasterArgumentNullExceptionHvisBetalingsbetingelseErNull()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var repository = new AdresseRepository(channelFactory);
            var method = repository.GetType().GetMethod("MapBetalingsbetingelse", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.That(method, Is.Not.Null);
            try
            {
                method.Invoke(repository, new object[] { null });
            }
            catch (TargetInvocationException ex)
            {
                Assert.That(ex.InnerException, Is.Not.Null);
                Assert.That(ex.InnerException, Is.TypeOf(typeof(ArgumentNullException)));
            }
        }

        /// <summary>
        /// Danner personer til test.
        /// </summary>
        /// <returns>personer til test.</returns>
        public static IList<PersonView> GetPersoner()
        {
            var adressegrupper = GetAdressegrupper();
            var betalingsbetingelser = GetBetalingsbetingelser();
            return new List<PersonView>
                       {
                           new PersonView
                               {
                                   Nummer = 1,
                                   Navn = "Ole Sørensen",
                                   Adresse1 = "Eggertsvænge 2",
                                   PostnummerBy = "5700  Svendborg",
                                   Telefon = "62 21 49 60",
                                   Mobil = "25 24 49 75",
                                   Fødselsdato = new DateTime(1975, 8, 21),
                                   Adressegruppe = adressegrupper.Single(m => m.Nummer == 1),
                                   Mailadresse = "os@dsidata.dk",
                                   Webadresse = "www.MrGottham.dk",
                                   Betalingsbetingelse = betalingsbetingelser.Single(m => m.Nummer == 1),
                                   Udlånsfrist = 14,
                                   Firma = new AdressereferenceView
                                               {
                                                   Nummer = 48,
                                                   Navn = "DSI DATA A/S"
                                               },
                                   FilofaxAdresselabel = true
                               },
                           new PersonView
                               {
                                   Nummer = 2,
                                   Navn = "Bente Rasmussen",
                                   Adresse1 = "Brønshøjvej 74B",
                                   PostnummerBy = "2700  Brønshøj",
                                   Telefon = "26 70 80 48",
                                   Mobil = "26 70 80 48",
                                   Fødselsdato = new DateTime(1958, 8, 20),
                                   Adressegruppe = adressegrupper.Single(m => m.Nummer == 2),
                                   Mailadresse = "ska@adslhome.dk",
                                   Betalingsbetingelse = betalingsbetingelser.Single(m => m.Nummer == 1),
                                   Udlånsfrist = 14,
                                   FilofaxAdresselabel = true
                               }
                       };
        }

        /// <summary>
        /// Danner firmaer til test.
        /// </summary>
        /// <returns>Firmaer til test.</returns>
        public static IList<FirmaView> GetFirmaer()
        {
            var adressegrupper = GetAdressegrupper();
            var betalingsbetingelser = GetBetalingsbetingelser();
            return new List<FirmaView>
                       {
                           new FirmaView
                               {
                                   Nummer = 48,
                                   Navn = "DSI DATA A/S",
                                   Adresse1 = "Kokkedal Industripark 2",
                                   PostnummerBy = "2980  Kokkedal",
                                   Telefon1 = "49 18 49 18",
                                   Telefax = "49 18 49 44",
                                   Adressegruppe = adressegrupper.Single(m => m.Nummer == 3),
                                   Bekendtskab = "Arbejdsplads, Ole",
                                   Mailadresse = "info@dsidata.dk",
                                   Webadresse = "www.dsidata.dk",
                                   Betalingsbetingelse = betalingsbetingelser.Single(m => m.Nummer == 1),
                                   Udlånsfrist = 14,
                                   FilofaxAdresselabel = true
                               }
                       };
        }

        /// <summary>
        /// Danner postnumre til test.
        /// </summary>
        /// <returns>Postnumre til test.</returns>
        public static IList<PostnummerView> GetPostnumre()
        {
            return new List<PostnummerView>
                       {
                           new PostnummerView
                               {
                                   Landekode = "DK",
                                   Postnummer = "2700",
                                   Bynavn = "Brønshøj"
                               },
                           new PostnummerView
                               {
                                   Landekode = "DK",
                                   Postnummer = "5700",
                                   Bynavn = "Svendborg"
                               },
                           new PostnummerView
                               {
                                   Landekode = "DK",
                                   Postnummer = "7860",
                                   Bynavn = "Spøttrup"
                               }
                       };
        }

        /// <summary>
        /// Danner adressegrupper til test.
        /// </summary>
        /// <returns>Adressegrupper til test.</returns>
        private static IList<AdressegruppeView> GetAdressegrupper()
        {
            return new List<AdressegruppeView>
                       {
                           new AdressegruppeView
                               {
                                   Nummer = 1,
                                   Navn = "Familie (Ole)",
                                   AdressegruppeOswebdb = 1,
                               },
                           new AdressegruppeView
                               {
                                   Nummer = 2,
                                   Navn = "Venner og veninder",
                                   AdressegruppeOswebdb = 2,
                               },
                           new AdressegruppeView
                               {
                                   Nummer = 3,
                                   Navn = "Arbejdsrelationer",
                                   AdressegruppeOswebdb = 3,
                               }
                       };
        }

        /// <summary>
        /// Danner betalingsbetingelser til test.
        /// </summary>
        /// <returns>Betalingsbetingelser til test.</returns>
        private static IList<BetalingsbetingelseView> GetBetalingsbetingelser()
        {
            return new List<BetalingsbetingelseView>
                       {
                           new BetalingsbetingelseView
                               {
                                   Nummer = 1,
                                   Navn = "Kontant"
                               },
                           new BetalingsbetingelseView
                               {
                                   Nummer = 2,
                                   Navn = "Netto + 8 dage"
                               }
                       };
        }
    }
}
