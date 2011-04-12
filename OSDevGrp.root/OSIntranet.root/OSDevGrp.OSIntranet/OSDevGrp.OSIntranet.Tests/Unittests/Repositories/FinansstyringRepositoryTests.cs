using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf.ChannelFactory;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Commands;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Enums;
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
    /// Test af repository til finansstyring.
    /// </summary>
    public class FinansstyringRepositoryTests
    {
        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis ChannelFactory er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisChannelFactoryErNull()
        {
            Assert.Throws<ArgumentNullException>(() => new FinansstyringRepository(null));
        }

        /// <summary>
        /// Tester, at RegnskabslisteGet henter alle regnskaber.
        /// </summary>
        [Test]
        public void TestAtRegnskabslisteGetHenterAlleRegnskaber()
        {
            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFinansstyringRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.RegnskabGetAll(Arg<RegnskabGetAllQuery>.Is.Anything))
                .Return(GetRegnskabsliste());
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed).Repeat.Any();
            mocker.ReplayAll();
            
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFinansstyringRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);
            
            var repository = new FinansstyringRepository(channelFactory);
            var regnskaber = repository.RegnskabslisteGet();
            Assert.That(regnskaber, Is.Not.Null);
            Assert.That(regnskaber.Count, Is.EqualTo(2));
            Assert.That(regnskaber[0].Nummer, Is.EqualTo(1));
            Assert.That(regnskaber[0].Navn, Is.Not.Null);
            Assert.That(regnskaber[0].Navn, Is.EqualTo("Ole Sørensen"));
            Assert.That(regnskaber[1].Nummer, Is.EqualTo(2));
            Assert.That(regnskaber[1].Navn, Is.Not.Null);
            Assert.That(regnskaber[1].Navn, Is.EqualTo("Bryllup"));
        }

        /// <summary>
        /// Tester, at RegnskabslisteGet kaster en IntranetRepositoryException ved IntranetRepositoryException.
        /// </summary>
        [Test]
        public void TestAtRegnskabslisteGetKasterIntranetRepositoryExceptionVedIntranetRepositoryException()
        {
            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFinansstyringRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.RegnskabGetAll(Arg<RegnskabGetAllQuery>.Is.Anything))
                .Throw(new IntranetRepositoryException("Test"));
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed).Repeat.Any();
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFinansstyringRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var repository = new FinansstyringRepository(channelFactory);
            Assert.Throws<IntranetRepositoryException>(() => repository.RegnskabslisteGet());
        }

        /// <summary>
        /// Tester, at RegnskabslisteGet kaster en IntranetRepositoryException ved FaultException.
        /// </summary>
        [Test]
        public void TestAtRegnskabslisteGetKasterIntranetRepositoryExceptionVedFaultException()
        {
            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFinansstyringRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.RegnskabGetAll(Arg<RegnskabGetAllQuery>.Is.Anything))
                .Throw(new FaultException("Test"));
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed).Repeat.Any();
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFinansstyringRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var repository = new FinansstyringRepository(channelFactory);
            Assert.Throws<IntranetRepositoryException>(() => repository.RegnskabslisteGet());
        }

        /// <summary>
        /// Tester, at RegnskabslisteGet kaster en IntranetRepositoryException ved Exception.
        /// </summary>
        [Test]
        public void TestAtRegnskabslisteGetKasterIntranetRepositoryExceptionVedException()
        {
            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFinansstyringRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.RegnskabGetAll(Arg<RegnskabGetAllQuery>.Is.Anything))
                .Throw(new Exception("Test"));
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed).Repeat.Any();
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFinansstyringRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var repository = new FinansstyringRepository(channelFactory);
            Assert.Throws<IntranetRepositoryException>(() => repository.RegnskabslisteGet());
        }

        /// <summary>
        /// Tester, at RegnskabGet henter et givent regnskab.
        /// </summary>
        [Test]
        public void TestAtRegnskabGetHenterRegnskab()
        {
            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFinansstyringRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.RegnskabGetByNummer(Arg<RegnskabGetByNummerQuery>.Is.Anything))
                .Return(GetRegnskab()).Repeat.Any();
            service.Expect(m => m.KontogruppeGetAll(Arg<KontogruppeGetAllQuery>.Is.Anything))
                .Return(GetKontogrupper()).Repeat.Any();
            service.Expect(m => m.BudgetkontogruppeGetAll(Arg<BudgetkontogruppeGetAllQuery>.Is.Anything))
                .Return(GetBudgetkontogrupper()).Repeat.Any();
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed).Repeat.Any();
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFinansstyringRepositoryService>(Arg<string>.Is.Anything))
                .Return(service).Repeat.Any();

            var repository = new FinansstyringRepository(channelFactory);
            var regnskab = repository.RegnskabGet(1);
            Assert.That(regnskab, Is.Not.Null);
            Assert.That(regnskab.Nummer, Is.EqualTo(1));
            Assert.That(regnskab.Navn, Is.Not.Null);
            Assert.That(regnskab.Navn, Is.EqualTo("Ole Sørensen"));
            Assert.That(regnskab.Konti, Is.Not.Null);
            Assert.That(regnskab.Konti.Count, Is.GreaterThan(0));
            Assert.That(regnskab.Konti.OfType<Konto>().Count(), Is.EqualTo(2));

            var person = new Person(1, "Ole Sørensen", new Adressegruppe(1, "Familie (Ole)", 1));
            regnskab = repository.RegnskabGet(1, nummer => person);
            Assert.That(regnskab, Is.Not.Null);
            Assert.That(regnskab.Nummer, Is.EqualTo(1));
            Assert.That(regnskab.Navn, Is.Not.Null);
            Assert.That(regnskab.Navn, Is.EqualTo("Ole Sørensen"));
            Assert.That(regnskab.Konti, Is.Not.Null);
            Assert.That(regnskab.Konti.Count, Is.GreaterThan(0));
            Assert.That(regnskab.Konti.OfType<Konto>().Count(), Is.EqualTo(2));
            foreach (var konto in regnskab.Konti.OfType<Konto>().ToList())
            {
                Assert.That(konto, Is.Not.Null);
                Assert.That(konto.Kreditoplysninger, Is.Not.Null);
                Assert.That(konto.Kreditoplysninger.Count, Is.GreaterThan(0));
                Assert.That(konto.Bogføringslinjer, Is.Not.Null);
                Assert.That(konto.Bogføringslinjer.Count, Is.GreaterThanOrEqualTo(0));
            }
            Assert.That(regnskab.Konti.OfType<Budgetkonto>().Count(), Is.EqualTo(5));
            foreach (var budgetkonto in regnskab.Konti.OfType<Budgetkonto>().ToList())
            {
                Assert.That(budgetkonto, Is.Not.Null);
                Assert.That(budgetkonto.Budgetoplysninger, Is.Not.Null);
                Assert.That(budgetkonto.Budgetoplysninger.Count, Is.GreaterThan(0));
                Assert.That(budgetkonto.Bogføringslinjer, Is.Not.Null);
                Assert.That(budgetkonto.Bogføringslinjer.Count, Is.GreaterThan(0));
            }
            Assert.That(person.Bogføringslinjer, Is.Not.Null);
            Assert.That(person.Bogføringslinjer.Count, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at en konto mappes korrekt.
        /// </summary>
        [Test]
        public void TestAtKontoMappesKorrekt()
        {
            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFinansstyringRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.RegnskabGetByNummer(Arg<RegnskabGetByNummerQuery>.Is.Anything))
                .Return(GetRegnskab());
            service.Expect(m => m.KontogruppeGetAll(Arg<KontogruppeGetAllQuery>.Is.Anything))
                .Return(GetKontogrupper());
            service.Expect(m => m.BudgetkontogruppeGetAll(Arg<BudgetkontogruppeGetAllQuery>.Is.Anything))
                .Return(GetBudgetkontogrupper());
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed);
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFinansstyringRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var repository = new FinansstyringRepository(channelFactory);
            var regnskab = repository.RegnskabGet(1);
            Assert.That(regnskab, Is.Not.Null);
            Assert.That(regnskab.Konti, Is.Not.Null);
            Assert.That(regnskab.Konti.Count, Is.GreaterThan(0));
            var konto = regnskab.Konti.OfType<Konto>().SingleOrDefault(m => m.Kontonummer.CompareTo("DANKORT") == 0);
            Assert.That(konto, Is.Not.Null);
            Assert.That(konto.Regnskab, Is.Not.Null);
            Assert.That(konto.Regnskab.Nummer, Is.EqualTo(1));
            Assert.That(konto.Regnskab.Navn, Is.Not.Null);
            Assert.That(konto.Regnskab.Navn, Is.EqualTo("Ole Sørensen"));
            Assert.That(konto.Kontonavn, Is.Not.Null);
            Assert.That(konto.Kontonavn, Is.EqualTo("Dankort"));
            Assert.That(konto.Beskrivelse, Is.Not.Null);
            Assert.That(konto.Beskrivelse, Is.EqualTo("Dankort/lønkonto"));
            Assert.That(konto.Note, Is.Not.Null);
            Assert.That(konto.Note, Is.EqualTo("Bla, bla og mere bla"));
            Assert.That(konto.Kontogruppe, Is.Not.Null);
            Assert.That(konto.Kontogruppe.Nummer, Is.EqualTo(1));
            Assert.That(konto.Kontogruppe.Navn, Is.Not.Null);
            Assert.That(konto.Kontogruppe.Navn, Is.EqualTo("Bankkonti"));
            Assert.That(konto.Kontogruppe.KontogruppeType, Is.EqualTo(CommonLibrary.Domain.Enums.KontogruppeType.Aktiver));
            Assert.That(konto.Kreditoplysninger.Where(m => m.År <= 2011 && m.Måned <= 4).Count(), Is.EqualTo(1));
            var kreditoplysninger = konto.Kreditoplysninger.SingleOrDefault(m => m.År == 2011 && m.Måned == 4);
            Assert.That(kreditoplysninger, Is.Not.Null);
            Assert.That(kreditoplysninger.År, Is.EqualTo(2011));
            Assert.That(kreditoplysninger.Måned, Is.EqualTo(4));
            Assert.That(kreditoplysninger.Kredit, Is.EqualTo(30000M));
            var calculateTo = new DateTime(2011, 4, 1);
            Assert.That(konto.Bogføringslinjer.Where(m => m.Dato.Date.CompareTo(calculateTo) <= 0).Count(), Is.EqualTo(6));
        }

        /// <summary>
        /// Tester, at en budgetkonto mappes korrekt.
        /// </summary>
        [Test]
        public void TestAtBudgetkontoMappesKorrekt()
        {
            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFinansstyringRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.RegnskabGetByNummer(Arg<RegnskabGetByNummerQuery>.Is.Anything))
                .Return(GetRegnskab());
            service.Expect(m => m.KontogruppeGetAll(Arg<KontogruppeGetAllQuery>.Is.Anything))
                .Return(GetKontogrupper());
            service.Expect(m => m.BudgetkontogruppeGetAll(Arg<BudgetkontogruppeGetAllQuery>.Is.Anything))
                .Return(GetBudgetkontogrupper());
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed);
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFinansstyringRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var repository = new FinansstyringRepository(channelFactory);
            var regnskab = repository.RegnskabGet(1);
            Assert.That(regnskab, Is.Not.Null);
            Assert.That(regnskab.Konti, Is.Not.Null);
            Assert.That(regnskab.Konti.Count, Is.GreaterThan(0));
            var budgetkonto = regnskab.Konti.OfType<Budgetkonto>().SingleOrDefault(m => m.Kontonummer.CompareTo("2000") == 0);
            Assert.That(budgetkonto, Is.Not.Null);
            Assert.That(budgetkonto.Regnskab, Is.Not.Null);
            Assert.That(budgetkonto.Regnskab.Nummer, Is.EqualTo(1));
            Assert.That(budgetkonto.Regnskab.Navn, Is.Not.Null);
            Assert.That(budgetkonto.Regnskab.Navn, Is.EqualTo("Ole Sørensen"));
            Assert.That(budgetkonto.Kontonavn, Is.Not.Null);
            Assert.That(budgetkonto.Kontonavn, Is.EqualTo("Budget"));
            Assert.That(budgetkonto.Beskrivelse, Is.Not.Null);
            Assert.That(budgetkonto.Beskrivelse, Is.EqualTo("Fast budgetterede omkostninger"));
            Assert.That(budgetkonto.Note, Is.Not.Null);
            Assert.That(budgetkonto.Note, Is.EqualTo("Bla, bla og mere bla"));
            Assert.That(budgetkonto.Budgetkontogruppe, Is.Not.Null);
            Assert.That(budgetkonto.Budgetkontogruppe.Nummer, Is.EqualTo(2));
            Assert.That(budgetkonto.Budgetkontogruppe.Navn, Is.Not.Null);
            Assert.That(budgetkonto.Budgetkontogruppe.Navn, Is.EqualTo("Udgifter"));
            Assert.That(budgetkonto.Budgetoplysninger.Where(m => m.År <= 2011 && m.Måned <= 4).Count(), Is.EqualTo(1));
            var budgetoplysnigner = budgetkonto.Budgetoplysninger.SingleOrDefault(m => m.År == 2011 && m.Måned == 4);
            Assert.That(budgetoplysnigner, Is.Not.Null);
            Assert.That(budgetoplysnigner.År, Is.EqualTo(2011));
            Assert.That(budgetoplysnigner.Måned, Is.EqualTo(4));
            Assert.That(budgetoplysnigner.Indtægter, Is.EqualTo(0M));
            Assert.That(budgetoplysnigner.Udgifter, Is.EqualTo(7500M));
            var calculateTo = new DateTime(2011, 4, 1);
            Assert.That(budgetkonto.Bogføringslinjer.Where(m => m.Dato.Date.CompareTo(calculateTo) <= 0).Count(), Is.EqualTo(1));
        }

        /// <summary>
        /// Tester, at RegnskabGet kaster en IntranetRepositoryException ved IntranetRepositoryException.
        /// </summary>
        [Test]
        public void TestAtRegnskabGetKasterIntranetRepositoryExceptionVedIntranetRepositoryException()
        {
            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFinansstyringRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.RegnskabGetByNummer(Arg<RegnskabGetByNummerQuery>.Is.Anything))
                .Throw(new IntranetRepositoryException("Test"));
            service.Expect(m => m.KontogruppeGetAll(Arg<KontogruppeGetAllQuery>.Is.Anything))
                .Return(GetKontogrupper());
            service.Expect(m => m.BudgetkontogruppeGetAll(Arg<BudgetkontogruppeGetAllQuery>.Is.Anything))
                .Return(GetBudgetkontogrupper());
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed);
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFinansstyringRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var repository = new FinansstyringRepository(channelFactory);
            Assert.Throws<IntranetRepositoryException>(() => repository.RegnskabGet(-1));
        }

        /// <summary>
        /// Tester, at RegnskabGet kaster en IntranetRepositoryException ved FaultException.
        /// </summary>
        [Test]
        public void TestAtRegnskabGetKasterIntranetRepositoryExceptionVedFaultException()
        {
            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFinansstyringRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.RegnskabGetByNummer(Arg<RegnskabGetByNummerQuery>.Is.Anything))
                .Throw(new FaultException("Test"));
            service.Expect(m => m.KontogruppeGetAll(Arg<KontogruppeGetAllQuery>.Is.Anything))
                .Return(GetKontogrupper());
            service.Expect(m => m.BudgetkontogruppeGetAll(Arg<BudgetkontogruppeGetAllQuery>.Is.Anything))
                .Return(GetBudgetkontogrupper());
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed);
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFinansstyringRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var repository = new FinansstyringRepository(channelFactory);
            Assert.Throws<IntranetRepositoryException>(() => repository.RegnskabGet(-1));
        }

        /// <summary>
        /// Tester, at RegnskabGet kaster en IntranetRepositoryException ved Exception.
        /// </summary>
        [Test]
        public void TestAtRegnskabGetKasterIntranetRepositoryExceptionVedException()
        {
            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFinansstyringRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.RegnskabGetByNummer(Arg<RegnskabGetByNummerQuery>.Is.Anything))
                .Throw(new Exception("Test"));
            service.Expect(m => m.KontogruppeGetAll(Arg<KontogruppeGetAllQuery>.Is.Anything))
                .Return(GetKontogrupper());
            service.Expect(m => m.BudgetkontogruppeGetAll(Arg<BudgetkontogruppeGetAllQuery>.Is.Anything))
                .Return(GetBudgetkontogrupper());
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed);
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFinansstyringRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var repository = new FinansstyringRepository(channelFactory);
            Assert.Throws<IntranetRepositoryException>(() => repository.RegnskabGet(-1));
        }

        /// <summary>
        /// Tester, at KontogruppeGetAll henter alle kontogrupper.
        /// </summary>
        [Test]
        public void TestAtKontogruppeGetAllHenterAlleKontogrupper()
        {
            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFinansstyringRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.KontogruppeGetAll(Arg<KontogruppeGetAllQuery>.Is.Anything))
                .Return(GetKontogrupper());
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed);
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFinansstyringRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var repository = new FinansstyringRepository(channelFactory);
            var kontogrupper = repository.KontogruppeGetAll();
            Assert.That(kontogrupper, Is.Not.Null);
            Assert.That(kontogrupper.Count, Is.EqualTo(2));
            Assert.That(kontogrupper[0], Is.Not.Null);
            Assert.That(kontogrupper[0].Nummer, Is.EqualTo(1));
            Assert.That(kontogrupper[0].Navn, Is.Not.Null);
            Assert.That(kontogrupper[0].Navn, Is.EqualTo("Bankkonti"));
            Assert.That(kontogrupper[0].KontogruppeType, Is.EqualTo(CommonLibrary.Domain.Enums.KontogruppeType.Aktiver));
            Assert.That(kontogrupper[1], Is.Not.Null);
            Assert.That(kontogrupper[1].Nummer, Is.EqualTo(2));
            Assert.That(kontogrupper[1].Navn, Is.Not.Null);
            Assert.That(kontogrupper[1].Navn, Is.EqualTo("Kontanter"));
            Assert.That(kontogrupper[1].KontogruppeType, Is.EqualTo(CommonLibrary.Domain.Enums.KontogruppeType.Aktiver));
        }

        /// <summary>
        /// Tester, at KontogruppeGetAll kaster en IntranetRepositoryException ved IntranetRepositoryException.
        /// </summary>
        [Test]
        public void TestAtKontogruppeGetAllKasterIntranetRepositoryExceptionVedIntranetRepositoryException()
        {
            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFinansstyringRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.KontogruppeGetAll(Arg<KontogruppeGetAllQuery>.Is.Anything))
                .Throw(new IntranetRepositoryException("Test"));
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed);
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFinansstyringRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var repository = new FinansstyringRepository(channelFactory);
            Assert.Throws<IntranetRepositoryException>(() => repository.KontogruppeGetAll());
        }

        /// <summary>
        /// Tester, at KontogruppeGetAll kaster en IntranetRepositoryException ved FaultException.
        /// </summary>
        [Test]
        public void TestAtKontogruppeGetAllKasterIntranetRepositoryExceptionVedFaultException()
        {
            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFinansstyringRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.KontogruppeGetAll(Arg<KontogruppeGetAllQuery>.Is.Anything))
                .Throw(new FaultException("FaultException"));
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed);
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFinansstyringRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var repository = new FinansstyringRepository(channelFactory);
            Assert.Throws<IntranetRepositoryException>(() => repository.KontogruppeGetAll());
        }

        /// <summary>
        /// Tester, at KontogruppeGetAll kaster en IntranetRepositoryException ved Exception.
        /// </summary>
        [Test]
        public void TestAtKontogruppeGetAllKasterIntranetRepositoryExceptionVedException()
        {
            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFinansstyringRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.KontogruppeGetAll(Arg<KontogruppeGetAllQuery>.Is.Anything))
                .Throw(new Exception("FaultException"));
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed);
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFinansstyringRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var repository = new FinansstyringRepository(channelFactory);
            Assert.Throws<IntranetRepositoryException>(() => repository.KontogruppeGetAll());
        }

        /// <summary>
        /// Tester, at BudgetkontogruppeGetAll henter alle budgetkontogrupper.
        /// </summary>
        [Test]
        public void TestAtBudgetkontogruppeGetAllHenterAlleBudgetkontogrupper()
        {
            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFinansstyringRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.BudgetkontogruppeGetAll(Arg<BudgetkontogruppeGetAllQuery>.Is.Anything))
                .Return(GetBudgetkontogrupper());
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed);
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFinansstyringRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var repository = new FinansstyringRepository(channelFactory);
            var budgetkontogrupper = repository.BudgetkontogruppeGetAll();
            Assert.That(budgetkontogrupper, Is.Not.Null);
            Assert.That(budgetkontogrupper.Count, Is.EqualTo(2));
            Assert.That(budgetkontogrupper[0], Is.Not.Null);
            Assert.That(budgetkontogrupper[0].Nummer, Is.EqualTo(1));
            Assert.That(budgetkontogrupper[0].Navn, Is.Not.Null);
            Assert.That(budgetkontogrupper[0].Navn, Is.EqualTo("Indtægter"));
            Assert.That(budgetkontogrupper[1], Is.Not.Null);
            Assert.That(budgetkontogrupper[1].Nummer, Is.EqualTo(2));
            Assert.That(budgetkontogrupper[1].Navn, Is.Not.Null);
            Assert.That(budgetkontogrupper[1].Navn, Is.EqualTo("Udgifter"));
        }

        /// <summary>
        /// Tester, at BudgetkontogruppeGetAll kaster en IntranetRepositoryException ved IntranetRepositoryException.
        /// </summary>
        [Test]
        public void TestAtBudgetkontogruppeGetAllKasterIntranetRepositoryExceptionVedIntranetRepositoryException()
        {
            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFinansstyringRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.BudgetkontogruppeGetAll(Arg<BudgetkontogruppeGetAllQuery>.Is.Anything))
                .Throw(new IntranetRepositoryException("Test"));
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed);
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFinansstyringRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var repository = new FinansstyringRepository(channelFactory);
            Assert.Throws<IntranetRepositoryException>(() => repository.BudgetkontogruppeGetAll());
        }

        /// <summary>
        /// Tester, at BudgetkontogruppeGetAll kaster en IntranetRepositoryException ved FaultException.
        /// </summary>
        [Test]
        public void TestAtBudgetkontogruppeGetAllKasterIntranetRepositoryExceptionVedFaultException()
        {
            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFinansstyringRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.BudgetkontogruppeGetAll(Arg<BudgetkontogruppeGetAllQuery>.Is.Anything))
                .Throw(new FaultException("Test"));
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed);
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFinansstyringRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var repository = new FinansstyringRepository(channelFactory);
            Assert.Throws<IntranetRepositoryException>(() => repository.BudgetkontogruppeGetAll());
        }

        /// <summary>
        /// Tester, at BudgetkontogruppeGetAll kaster en IntranetRepositoryException ved Exception.
        /// </summary>
        [Test]
        public void TestAtBudgetkontogruppeGetAllKasterIntranetRepositoryExceptionVedException()
        {
            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFinansstyringRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.BudgetkontogruppeGetAll(Arg<BudgetkontogruppeGetAllQuery>.Is.Anything))
                .Throw(new Exception("Test"));
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed);
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFinansstyringRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var repository = new FinansstyringRepository(channelFactory);
            Assert.Throws<IntranetRepositoryException>(() => repository.BudgetkontogruppeGetAll());
        }

        /// <summary>
        /// Tester, at BogføringslinjeAdd kaster en ArgumentNullException, hvis kontoen er null.
        /// </summary>
        [Test]
        public void TestAtBogføringslinjeAddKasterEnArgumentNullExceptionHvisKontoErNull()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            var repository = new FinansstyringRepository(channelFactory);
            Assert.Throws<ArgumentNullException>(() => repository.BogføringslinjeAdd(new DateTime(2011, 4, 1), null, null, null, null, 0M, 0M, null));
        }

        /// <summary>
        /// Tester, at BogføringslinjeAdd kaster en ArgumentNullException, hvis tekst er null.
        /// </summary>
        [Test]
        public void TestAtBogføringslinjeAddKasterEnArgumentNullExceptionHvisTekstErNull()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            var repository = new FinansstyringRepository(channelFactory);
            var regnskab = new Regnskab(1, "Ole Sørensen");
            var bankkonti = new Kontogruppe(1, "Bankkonto", CommonLibrary.Domain.Enums.KontogruppeType.Aktiver);
            var dankort = new Konto(regnskab, "DANKORT", "Dankort", bankkonti);
            Assert.Throws<ArgumentNullException>(() => repository.BogføringslinjeAdd(new DateTime(2011, 4, 1), null, dankort, null, null, 0M, 0M, null));
        }

        /// <summary>
        /// Tester, at BogføringslinjeAdd kalder servicemetode.
        /// </summary>
        [Test]
        public void TestAtBogføringslinjeAddKalderServicemetode()
        {
            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFinansstyringRepositoryService>(new[] { typeof(ICommunicationObject) });
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed);
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFinansstyringRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var repository = new FinansstyringRepository(channelFactory);
            var regnskab = new Regnskab(1, "Ole Sørensen");
            var bankkonti = new Kontogruppe(1, "Bankkonto", CommonLibrary.Domain.Enums.KontogruppeType.Aktiver);
            var dankort = new Konto(regnskab, "DANKORT", "Dankort", bankkonti);
            repository.BogføringslinjeAdd(new DateTime(2011, 4, 1), null, dankort, "Test", null, 0M, 0M, null);
            service.AssertWasCalled(m => m.BogføringslinjeAdd(Arg<BogføringslinjeAddCommand>.Is.Anything));
        }

        /// <summary>
        /// Tester, at BogføringslinjeAdd kaster en IntranetRepositoryException ved IntranetRepositoryException.
        /// </summary>
        [Test]
        public void TestAtBogføringslinjeAddKasterIntranetRepositoryExceptionVedIntranetRepositoryException()
        {
            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFinansstyringRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.BogføringslinjeAdd(Arg<BogføringslinjeAddCommand>.Is.Anything))
                .Throw(new IntranetRepositoryException("Test"));
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed);
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFinansstyringRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var repository = new FinansstyringRepository(channelFactory);
            var regnskab = new Regnskab(1, "Ole Sørensen");
            var bankkonti = new Kontogruppe(1, "Bankkonto", CommonLibrary.Domain.Enums.KontogruppeType.Aktiver);
            var dankort = new Konto(regnskab, "DANKORT", "Dankort", bankkonti);
            Assert.Throws<IntranetRepositoryException>(() => repository.BogføringslinjeAdd(new DateTime(2011, 4, 1), null, dankort, "Test", null, 0M, 0M, null));
        }

        /// <summary>
        /// Tester, at BogføringslinjeAdd kaster en IntranetRepositoryException ved FaultException.
        /// </summary>
        [Test]
        public void TestAtBogføringslinjeAddKasterIntranetRepositoryExceptionVedFaultException()
        {
            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFinansstyringRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.BogføringslinjeAdd(Arg<BogføringslinjeAddCommand>.Is.Anything))
                .Throw(new FaultException("Test"));
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed);
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFinansstyringRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var repository = new FinansstyringRepository(channelFactory);
            var regnskab = new Regnskab(1, "Ole Sørensen");
            var bankkonti = new Kontogruppe(1, "Bankkonto", CommonLibrary.Domain.Enums.KontogruppeType.Aktiver);
            var dankort = new Konto(regnskab, "DANKORT", "Dankort", bankkonti);
            Assert.Throws<IntranetRepositoryException>(() => repository.BogføringslinjeAdd(new DateTime(2011, 4, 1), null, dankort, "Test", null, 0M, 0M, null));
        }

        /// <summary>
        /// Tester, at BogføringslinjeAdd kaster en IntranetRepositoryException ved Exception.
        /// </summary>
        [Test]
        public void TestAtBogføringslinjeAddKasterIntranetRepositoryExceptionVedException()
        {
            var mocker = new MockRepository();
            var service = mocker.DynamicMultiMock<IFinansstyringRepositoryService>(new[] { typeof(ICommunicationObject) });
            service.Expect(m => m.BogføringslinjeAdd(Arg<BogføringslinjeAddCommand>.Is.Anything))
                .Throw(new Exception("Test"));
            Expect.Call(((ICommunicationObject)service).State).Return(CommunicationState.Closed);
            mocker.ReplayAll();

            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            channelFactory.Expect(m => m.CreateChannel<IFinansstyringRepositoryService>(Arg<string>.Is.Anything))
                .Return(service);

            var repository = new FinansstyringRepository(channelFactory);
            var regnskab = new Regnskab(1, "Ole Sørensen");
            var bankkonti = new Kontogruppe(1, "Bankkonto", CommonLibrary.Domain.Enums.KontogruppeType.Aktiver);
            var dankort = new Konto(regnskab, "DANKORT", "Dankort", bankkonti);
            Assert.Throws<IntranetRepositoryException>(() => repository.BogføringslinjeAdd(new DateTime(2011, 4, 1), null, dankort, "Test", null, 0M, 0M, null));
        }

        /// <summary>
        /// Tester, at MapRegnskab kaster en ArgumentNullException, hvis regnskabslisteviewet er null.
        /// </summary>
        [Test]
        public void TestAtMapRegnskabKasterArgumentNullExceptionHvisRegnskabListeViewErNull()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var repository = new FinansstyringRepository(channelFactory);
            var method = repository.GetType().GetMethod("MapRegnskab", BindingFlags.NonPublic | BindingFlags.Static, null, new [] {typeof (RegnskabListeView)}, null);
            Assert.That(method, Is.Not.Null);
            try
            {
                method.Invoke(repository, new object[] {null});
            }
            catch (TargetInvocationException ex)
            {
                Assert.That(ex.InnerException, Is.Not.Null);
                Assert.That(ex.InnerException, Is.TypeOf(typeof (ArgumentNullException)));
            }
        }

        /// <summary>
        /// Tester, at MapRegnskab kaster en ArgumentNullException, hvis regnskabsviewet er null.
        /// </summary>
        [Test]
        public void TestAtMapRegnskabKasterArgumentNullExceptionHvisRegnskabViewErNull()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var repository = new FinansstyringRepository(channelFactory);
            var method = repository.GetType().GetMethod("MapRegnskab", BindingFlags.NonPublic | BindingFlags.Static,
                                                        null, new[]
                                                                  {
                                                                      typeof (RegnskabView),
                                                                      typeof (IEnumerable<Kontogruppe>),
                                                                      typeof (IEnumerable<Budgetkontogruppe>),
                                                                      typeof (Func<int, AdresseBase>)
                                                                  }, null);
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
        /// Tester, at MapRegnskab kaster en ArgumentNullException, hvis kontogrupper er null.
        /// </summary>
        [Test]
        public void TestAtMapRegnskabKasterArgumentNullExceptionHvisKontogrupperErNull()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var repository = new FinansstyringRepository(channelFactory);
            var method = repository.GetType().GetMethod("MapRegnskab", BindingFlags.NonPublic | BindingFlags.Static,
                                                        null, new[]
                                                                  {
                                                                      typeof (RegnskabView),
                                                                      typeof (IEnumerable<Kontogruppe>),
                                                                      typeof (IEnumerable<Budgetkontogruppe>),
                                                                      typeof (Func<int, AdresseBase>)
                                                                  }, null);
            Assert.That(method, Is.Not.Null);
            try
            {
                method.Invoke(repository, new object[] {new RegnskabView(), null, null, null});
            }
            catch (TargetInvocationException ex)
            {
                Assert.That(ex.InnerException, Is.Not.Null);
                Assert.That(ex.InnerException, Is.TypeOf(typeof (ArgumentNullException)));
            }
        }

        /// <summary>
        /// Tester, at MapRegnskab kaster en ArgumentNullException, hvis grupper af budgetkonti er null.
        /// </summary>
        [Test]
        public void TestAtMapRegnskabKasterArgumentNullExceptionHvisBudgetkontogrupperErNull()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var repository = new FinansstyringRepository(channelFactory);
            var method = repository.GetType().GetMethod("MapRegnskab", BindingFlags.NonPublic | BindingFlags.Static,
                                                        null, new[]
                                                                  {
                                                                      typeof (RegnskabView),
                                                                      typeof (IEnumerable<Kontogruppe>),
                                                                      typeof (IEnumerable<Budgetkontogruppe>),
                                                                      typeof (Func<int, AdresseBase>)
                                                                  }, null);
            Assert.That(method, Is.Not.Null);
            try
            {
                method.Invoke(repository, new object[] { new RegnskabView(), new List<Kontogruppe>(), null, null });
            }
            catch (TargetInvocationException ex)
            {
                Assert.That(ex.InnerException, Is.Not.Null);
                Assert.That(ex.InnerException, Is.TypeOf(typeof(ArgumentNullException)));
            }
        }

        /// <summary>
        /// Tester, at MapKonto kaster en ArgumentNullException, hvis regnskab er null.
        /// </summary>
        [Test]
        public void TestAtMapKontoKasterArgumentNullExceptionHvisRengskabErNull()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var repository = new FinansstyringRepository(channelFactory);
            var method = repository.GetType().GetMethod("MapKonto", BindingFlags.NonPublic | BindingFlags.Static,
                                                        null,
                                                        new[]
                                                            {
                                                                typeof (Regnskab), typeof (KontoListeView),
                                                                typeof (IEnumerable<Kontogruppe>)
                                                            }, null);
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
            method = repository.GetType().GetMethod("MapKonto", BindingFlags.NonPublic | BindingFlags.Static,
                                                    null,
                                                    new[]
                                                        {
                                                            typeof (Regnskab), typeof (KontoView),
                                                            typeof (IEnumerable<Kontogruppe>)
                                                        }, null);
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
        /// Tester, at MapKonto kaster en ArgumentNullException, hvis kontolisteviewet er null.
        /// </summary>
        [Test]
        public void TestAtMapKontoKasterArgumentNullExceptionHvisKontoListeViewErNull()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var repository = new FinansstyringRepository(channelFactory);
            var method = repository.GetType().GetMethod("MapKonto", BindingFlags.NonPublic | BindingFlags.Static,
                                                        null,
                                                        new[]
                                                            {
                                                                typeof (Regnskab), typeof (KontoListeView),
                                                                typeof (IEnumerable<Kontogruppe>)
                                                            }, null);
            Assert.That(method, Is.Not.Null);
            try
            {
                method.Invoke(repository, new object[] {new Regnskab(1, "Ole Sørensen"), null, null});
            }
            catch (TargetInvocationException ex)
            {
                Assert.That(ex.InnerException, Is.Not.Null);
                Assert.That(ex.InnerException, Is.TypeOf(typeof(ArgumentNullException)));
            }
        }

        /// <summary>
        /// Tester, at MapKonto kaster en ArgumentNullException, hvis kontoviewet er null.
        /// </summary>
        [Test]
        public void TestAtMapKontoKasterArgumentNullExceptionHvisKontoViewErNull()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var repository = new FinansstyringRepository(channelFactory);
            var method = repository.GetType().GetMethod("MapKonto", BindingFlags.NonPublic | BindingFlags.Static,
                                                        null,
                                                        new[]
                                                            {
                                                                typeof (Regnskab), typeof (KontoView),
                                                                typeof (IEnumerable<Kontogruppe>)
                                                            }, null);
            Assert.That(method, Is.Not.Null);
            try
            {
                method.Invoke(repository, new object[] {new Regnskab(1, "Ole Sørensen"), null, null});
            }
            catch (TargetInvocationException ex)
            {
                Assert.That(ex.InnerException, Is.Not.Null);
                Assert.That(ex.InnerException, Is.TypeOf(typeof (ArgumentNullException)));
            }
        }

        /// <summary>
        /// Tester, at MapKonto kaster en ArgumentNullException, hvis kontogrupper er null.
        /// </summary>
        [Test]
        public void TestAtMapKontoKasterArgumentNullExceptionHvisKontogrupperErNull()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var repository = new FinansstyringRepository(channelFactory);
            var method = repository.GetType().GetMethod("MapKonto", BindingFlags.NonPublic | BindingFlags.Static,
                                                        null,
                                                        new[]
                                                            {
                                                                typeof (Regnskab), typeof (KontoListeView),
                                                                typeof (IEnumerable<Kontogruppe>)
                                                            }, null);
            Assert.That(method, Is.Not.Null);
            try
            {
                method.Invoke(repository, new object[] {new Regnskab(1, "Ole Sørensen"), new KontoListeView(), null});
            }
            catch (TargetInvocationException ex)
            {
                Assert.That(ex.InnerException, Is.Not.Null);
                Assert.That(ex.InnerException, Is.TypeOf(typeof (ArgumentNullException)));
            }
            method = repository.GetType().GetMethod("MapKonto", BindingFlags.NonPublic | BindingFlags.Static,
                                                    null,
                                                    new[]
                                                        {
                                                            typeof (Regnskab), typeof (KontoView),
                                                            typeof (IEnumerable<Kontogruppe>)
                                                        }, null);
            Assert.That(method, Is.Not.Null);
            try
            {
                method.Invoke(repository, new object[] {new Regnskab(1, "Ole Sørensen"), new KontoView(), null});
            }
            catch (TargetInvocationException ex)
            {
                Assert.That(ex.InnerException, Is.Not.Null);
                Assert.That(ex.InnerException, Is.TypeOf(typeof (ArgumentNullException)));
            }
        }

        /// <summary>
        /// Tester, at MapKonto kaster en IntranetRepositoryException, hvis kontogruppen ikke findes.
        /// </summary>
        [Test]
        public void TestAtMapKontoKasterIntranetRepositoryExceptionHvisKontogruppeIkkeFindes()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var repository = new FinansstyringRepository(channelFactory);
            var method = repository.GetType().GetMethod("MapKonto", BindingFlags.NonPublic | BindingFlags.Static,
                                                        null,
                                                        new[]
                                                            {
                                                                typeof (Regnskab), typeof (KontoListeView),
                                                                typeof (IEnumerable<Kontogruppe>)
                                                            }, null);
            Assert.That(method, Is.Not.Null);
            try
            {
                method.Invoke(repository,
                              new object[]
                                  {
                                      new Regnskab(1, "Ole Sørensen"),
                                      new KontoListeView {Kontogruppe = new KontogruppeView {Nummer = 1}},
                                      new List<Kontogruppe>()
                                  });
            }
            catch (TargetInvocationException ex)
            {
                Assert.That(ex.InnerException, Is.Not.Null);
                Assert.That(ex.InnerException, Is.TypeOf(typeof (IntranetRepositoryException)));
            }
        }

        /// <summary>
        /// Tester, at MapKreditoplysninger kaster en ArgumentNullException, hvis kreditoplysningsviewet er null.
        /// </summary>
        [Test]
        public void TestAtMapKreditoplysningerKasterArgumentNullExceptionHvisKontogrupperErNull()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var repository = new FinansstyringRepository(channelFactory);
            var method = repository.GetType().GetMethod("MapKreditoplysninger",
                                                        BindingFlags.NonPublic | BindingFlags.Static);
            Assert.That(method, Is.Not.Null);
            try
            {
                method.Invoke(repository, new object[] {null});
            }
            catch (TargetInvocationException ex)
            {
                Assert.That(ex.InnerException, Is.Not.Null);
                Assert.That(ex.InnerException, Is.TypeOf(typeof (ArgumentNullException)));
            }
        }

        /// <summary>
        /// Tester, at MapBudgetkonto kaster en ArgumentNullException, hvis regnskab er null.
        /// </summary>
        [Test]
        public void TestAtMapBudgetkontoKasterArgumentNullExceptionHvisRengskabErNull()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var repository = new FinansstyringRepository(channelFactory);
            var method = repository.GetType().GetMethod("MapBudgetkonto", BindingFlags.NonPublic | BindingFlags.Static,
                                                        null,
                                                        new[]
                                                            {
                                                                typeof (Regnskab), typeof (BudgetkontoListeView),
                                                                typeof (IEnumerable<Budgetkontogruppe>)
                                                            }, null);
            Assert.That(method, Is.Not.Null);
            try
            {
                method.Invoke(repository, new object[] {null, null, null});
            }
            catch (TargetInvocationException ex)
            {
                Assert.That(ex.InnerException, Is.Not.Null);
                Assert.That(ex.InnerException, Is.TypeOf(typeof (ArgumentNullException)));
            }
            method = repository.GetType().GetMethod("MapBudgetkonto", BindingFlags.NonPublic | BindingFlags.Static,
                                                    null,
                                                    new[]
                                                        {
                                                            typeof (Regnskab), typeof (BudgetkontoView),
                                                            typeof (IEnumerable<Budgetkontogruppe>)
                                                        }, null);
            Assert.That(method, Is.Not.Null);
            try
            {
                method.Invoke(repository, new object[] {null, null, null});
            }
            catch (TargetInvocationException ex)
            {
                Assert.That(ex.InnerException, Is.Not.Null);
                Assert.That(ex.InnerException, Is.TypeOf(typeof (ArgumentNullException)));
            }
        }

        /// <summary>
        /// Tester, at MapBudgetkonto kaster en ArgumentNullException, hvis budgetkontolisteviewet er null.
        /// </summary>
        [Test]
        public void TestAtMapBudgetkontoKasterArgumentNullExceptionHvisBudgetkontoListeViewErNull()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var repository = new FinansstyringRepository(channelFactory);
            var method = repository.GetType().GetMethod("MapBudgetkonto", BindingFlags.NonPublic | BindingFlags.Static,
                                                        null,
                                                        new[]
                                                            {
                                                                typeof (Regnskab), typeof (BudgetkontoListeView),
                                                                typeof (IEnumerable<Budgetkontogruppe>)
                                                            }, null);
            Assert.That(method, Is.Not.Null);
            try
            {
                method.Invoke(repository, new object[] {new Regnskab(1, "Ole Sørensen"), null, null});
            }
            catch (TargetInvocationException ex)
            {
                Assert.That(ex.InnerException, Is.Not.Null);
                Assert.That(ex.InnerException, Is.TypeOf(typeof (ArgumentNullException)));
            }
        }

        /// <summary>
        /// Tester, at MapBudgetkonto kaster en ArgumentNullException, hvis budgetkontoviewet er null.
        /// </summary>
        [Test]
        public void TestAtMapBudgetkontoKasterArgumentNullExceptionHvisBudgetkontoViewErNull()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var repository = new FinansstyringRepository(channelFactory);
            var method = repository.GetType().GetMethod("MapBudgetkonto", BindingFlags.NonPublic | BindingFlags.Static,
                                                        null,
                                                        new[]
                                                            {
                                                                typeof (Regnskab), typeof (BudgetkontoView),
                                                                typeof (IEnumerable<Budgetkontogruppe>)
                                                            }, null);
            Assert.That(method, Is.Not.Null);
            try
            {
                method.Invoke(repository, new object[] { new Regnskab(1, "Ole Sørensen"), null, null });
            }
            catch (TargetInvocationException ex)
            {
                Assert.That(ex.InnerException, Is.Not.Null);
                Assert.That(ex.InnerException, Is.TypeOf(typeof(ArgumentNullException)));
            }
        }

        /// <summary>
        /// Tester, at MapBudgetkonto kaster en ArgumentNullException, hvis grupper af budgetkonti er null.
        /// </summary>
        [Test]
        public void TestAtMapBudgetkontoKasterArgumentNullExceptionHvisBudgetkontogrupperErNull()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var repository = new FinansstyringRepository(channelFactory);
            var method = repository.GetType().GetMethod("MapBudgetkonto", BindingFlags.NonPublic | BindingFlags.Static,
                                                        null,
                                                        new[]
                                                            {
                                                                typeof (Regnskab), typeof (BudgetkontoListeView),
                                                                typeof (IEnumerable<Budgetkontogruppe>)
                                                            }, null);
            Assert.That(method, Is.Not.Null);
            try
            {
                method.Invoke(repository,
                              new object[] {new Regnskab(1, "Ole Sørensen"), new BudgetkontoListeView(), null});
            }
            catch (TargetInvocationException ex)
            {
                Assert.That(ex.InnerException, Is.Not.Null);
                Assert.That(ex.InnerException, Is.TypeOf(typeof (ArgumentNullException)));
            }
            method = repository.GetType().GetMethod("MapBudgetkonto", BindingFlags.NonPublic | BindingFlags.Static,
                                                    null,
                                                    new[]
                                                        {
                                                            typeof (Regnskab), typeof (BudgetkontoView),
                                                            typeof (IEnumerable<Budgetkontogruppe>)
                                                        }, null);
            Assert.That(method, Is.Not.Null);
            try
            {
                method.Invoke(repository,
                              new object[] {new Regnskab(1, "Ole Sørensen"), new BudgetkontoView(), null});
            }
            catch (TargetInvocationException ex)
            {
                Assert.That(ex.InnerException, Is.Not.Null);
                Assert.That(ex.InnerException, Is.TypeOf(typeof (ArgumentNullException)));
            }
        }

        /// <summary>
        /// Tester, at MapBudgetkonto kaster en IntranetRepositoryException, hvis budgetkontogruppen ikke findes.
        /// </summary>
        [Test]
        public void TestAtMapBudgetkontoKasterIntranetRepositoryExceptionHvisBudgetkontogruppeIkkeFindes()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var repository = new FinansstyringRepository(channelFactory);
            var method = repository.GetType().GetMethod("MapBudgetkonto", BindingFlags.NonPublic | BindingFlags.Static,
                                                        null,
                                                        new[]
                                                            {
                                                                typeof (Regnskab), typeof (BudgetkontoListeView),
                                                                typeof (IEnumerable<Budgetkontogruppe>)
                                                            }, null);
            Assert.That(method, Is.Not.Null);
            try
            {
                method.Invoke(repository,
                              new object[]
                                  {
                                      new Regnskab(1, "Ole Sørensen"),
                                      new BudgetkontoListeView {Budgetkontogruppe = new BudgetkontogruppeView {Nummer = 1}}
                                      , new List<Budgetkontogruppe>()
                                  });
            }
            catch (TargetInvocationException ex)
            {
                Assert.That(ex.InnerException, Is.Not.Null);
                Assert.That(ex.InnerException, Is.TypeOf(typeof (IntranetRepositoryException)));
            }
        }

        /// <summary>
        /// Tester, at MapBudgetoplysninger kaster en ArgumentNullException, hvis budgetoplysningsviewet er null.
        /// </summary>
        [Test]
        public void TestAtMapBudgetoplysningerKasterArgumentNullExceptionHvisBudgetoplysningViewErNull()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var repository = new FinansstyringRepository(channelFactory);
            var method = repository.GetType().GetMethod("MapBudgetoplysninger", BindingFlags.NonPublic | BindingFlags.Static);
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
        /// Tester, at MapBogføringslinje kaster en ArgumentNullException, hvis bogføringslinjeviewet er null.
        /// </summary>
        [Test]
        public void TestAtMapBogføringslinjeKasterArgumentNullExceptionHvisBogføringslinjeViewErNull()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var repository = new FinansstyringRepository(channelFactory);
            var method = repository.GetType().GetMethod("MapBogføringslinje", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.That(method, Is.Not.Null);
            try
            {
                method.Invoke(repository, new object[] {null, null, null, null});
            }
            catch (TargetInvocationException ex)
            {
                Assert.That(ex.InnerException, Is.Not.Null);
                Assert.That(ex.InnerException, Is.TypeOf(typeof(ArgumentNullException)));
            }
        }

        /// <summary>
        /// Tester, at MapBogføringslinje kaster en ArgumentNullException, hvis konti er null.
        /// </summary>
        [Test]
        public void TestAtMapBogføringslinjeKasterArgumentNullExceptionHvisKontiErNull()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var repository = new FinansstyringRepository(channelFactory);
            var method = repository.GetType().GetMethod("MapBogføringslinje", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.That(method, Is.Not.Null);
            try
            {
                method.Invoke(repository, new object[] {new BogføringslinjeView(), null, null, null});
            }
            catch (TargetInvocationException ex)
            {
                Assert.That(ex.InnerException, Is.Not.Null);
                Assert.That(ex.InnerException, Is.TypeOf(typeof (ArgumentNullException)));
            }
        }

        /// <summary>
        /// Tester, at MapBogføringslinje kaster en ArgumentNullException, hvis budgetkonti er null.
        /// </summary>
        [Test]
        public void TestAtMapBogføringslinjeKasterArgumentNullExceptionHvisBudgetkontiErNull()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var repository = new FinansstyringRepository(channelFactory);
            var method = repository.GetType().GetMethod("MapBogføringslinje", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.That(method, Is.Not.Null);
            try
            {
                method.Invoke(repository, new object[] { new BogføringslinjeView(), new List<Konto>(), null, null });
            }
            catch (TargetInvocationException ex)
            {
                Assert.That(ex.InnerException, Is.Not.Null);
                Assert.That(ex.InnerException, Is.TypeOf(typeof(ArgumentNullException)));
            }
        }

        /// <summary>
        /// Tester, at MapBogføringslinje kaster en IntranetRepositoryException, hvis konto ikke findes.
        /// </summary>
        [Test]
        public void TestAtMapBogføringslinjeKasterIntranetRepositoryExceptionHvisKontoIkkeFindes()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var repository = new FinansstyringRepository(channelFactory);
            var method = repository.GetType().GetMethod("MapBogføringslinje", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.That(method, Is.Not.Null);
            try
            {
                var view = new BogføringslinjeView
                               {
                                   Løbenummer = 1,
                                   Dato = new DateTime(2011, 4, 1),
                                   Konto = new KontoListeView
                                               {
                                                   Kontonummer = "DANKORT"
                                               },
                                   Tekst = "Test",
                                   Budgetkonto = new BudgetkontoListeView
                                                     {
                                                         Kontonummer = "1000"
                                                     },
                                   Debit = 1000M,
                                   Adresse = new AdressereferenceView
                                                 {
                                                     Nummer = 1
                                                 }
                               };
                method.Invoke(repository, new object[] {view, new List<Konto>(), new List<Budgetkonto>(), null});
            }
            catch (TargetInvocationException ex)
            {
                Assert.That(ex.InnerException, Is.Not.Null);
                Assert.That(ex.InnerException, Is.TypeOf(typeof(IntranetRepositoryException)));
            }
        }

        /// <summary>
        /// Tester, at MapBogføringslinje kaster en IntranetRepositoryException, hvis budgetkonto ikke findes.
        /// </summary>
        [Test]
        public void TestAtMapBogføringslinjeKasterIntranetRepositoryExceptionHvisBudgetkontoIkkeFindes()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var repository = new FinansstyringRepository(channelFactory);
            var method = repository.GetType().GetMethod("MapBogføringslinje", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.That(method, Is.Not.Null);
            try
            {
                var view = new BogføringslinjeView
                {
                    Løbenummer = 1,
                    Dato = new DateTime(2011, 4, 1),
                    Konto = new KontoListeView
                    {
                        Kontonummer = "DANKORT"
                    },
                    Tekst = "Test",
                    Budgetkonto = new BudgetkontoListeView
                    {
                        Kontonummer = "1000"
                    },
                    Debit = 1000M,
                    Adresse = new AdressereferenceView
                    {
                        Nummer = 1
                    }
                };

                var regnskab = new Regnskab(1, "Ole Sørensen");
                var kontogruppe = new Kontogruppe(1, "Bankkonti", CommonLibrary.Domain.Enums.KontogruppeType.Aktiver);
                var kontoDankort = new Konto(regnskab, "DANKORT", "Dankort", kontogruppe);
                method.Invoke(repository, new object[] {view, new List<Konto> {kontoDankort}, new List<Budgetkonto>(), null});
            }
            catch (TargetInvocationException ex)
            {
                Assert.That(ex.InnerException, Is.Not.Null);
                Assert.That(ex.InnerException, Is.TypeOf(typeof(IntranetRepositoryException)));
            }
        }

        /// <summary>
        /// Tester, at MapBogføringslinje kaster en IntranetRepositoryException, hvis adressen ikke findes.
        /// </summary>
        [Test]
        public void TestAtMapBogføringslinjeKasterIntranetRepositoryExceptionHvisAdresseIkkeFindes()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var repository = new FinansstyringRepository(channelFactory);
            var method = repository.GetType().GetMethod("MapBogføringslinje", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.That(method, Is.Not.Null);
            try
            {
                var view = new BogføringslinjeView
                {
                    Løbenummer = 1,
                    Dato = new DateTime(2011, 4, 1),
                    Konto = new KontoListeView
                    {
                        Kontonummer = "DANKORT"
                    },
                    Tekst = "Test",
                    Budgetkonto = new BudgetkontoListeView
                    {
                        Kontonummer = "1000"
                    },
                    Debit = 1000M,
                    Adresse = new AdressereferenceView
                    {
                        Nummer = 1
                    }
                };

                var regnskab = new Regnskab(1, "Ole Sørensen");
                var kontogruppe = new Kontogruppe(1, "Bankkonti", CommonLibrary.Domain.Enums.KontogruppeType.Aktiver);
                var kontoDankort = new Konto(regnskab, "DANKORT", "Dankort", kontogruppe);
                var budgetkontogruppe = new Budgetkontogruppe(1, "Indtægter");
                var budgetkontoIndtægter = new Budgetkonto(regnskab, "1000", "Indtægter", budgetkontogruppe);
                var callback = new Func<int, AdresseBase>(m => null);
                method.Invoke(repository, new object[]
                                              {
                                                  view, new List<Konto> {kontoDankort},
                                                  new List<Budgetkonto> {budgetkontoIndtægter}, callback
                                              });
            }
            catch (TargetInvocationException ex)
            {
                Assert.That(ex.InnerException, Is.Not.Null);
                Assert.That(ex.InnerException, Is.TypeOf(typeof(IntranetRepositoryException)));
            }
        }

        /// <summary>
        /// Tester, at MapKontogruppeType kan mappe kontogruppetype til aktiver.
        /// </summary>
        [Test]
        public void TestAtMapKontogruppeTypeKanMappeKontogruppetypeTilAktiver()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var repository = new FinansstyringRepository(channelFactory);
            var method = repository.GetType().GetMethod("MapKontogruppeType", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.That(method, Is.Not.Null);

            var result = (CommonLibrary.Domain.Enums.KontogruppeType)method.Invoke(repository, new object[] { KontogruppeType.Aktiver });
            Assert.That(result, Is.EqualTo(CommonLibrary.Domain.Enums.KontogruppeType.Aktiver));
        }

        /// <summary>
        /// Tester, at MapKontogruppeType kan mappe kontogruppetype til passiver.
        /// </summary>
        [Test]
        public void TestAtMapKontogruppeTypeKanMappeKontogruppetypeTilPassiver()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var repository = new FinansstyringRepository(channelFactory);
            var method = repository.GetType().GetMethod("MapKontogruppeType", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.That(method, Is.Not.Null);

            var result = (CommonLibrary.Domain.Enums.KontogruppeType) method.Invoke(repository, new object[] { KontogruppeType.Passiver});
            Assert.That(result, Is.EqualTo(CommonLibrary.Domain.Enums.KontogruppeType.Passiver));
        }

        /// <summary>
        /// Tester, at MapKontogruppeType kaster en IntranetRepositoryException, hvis kontogruppetypen ikke kan mappes.
        /// </summary>
        [Test]
        public void TestAtMapKontogruppeTypeKasterIntranetRepositoryExceptionHvisKontogruppetypenIkkeKanMappes()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var repository = new FinansstyringRepository(channelFactory);
            var method = repository.GetType().GetMethod("MapKontogruppeType", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.That(method, Is.Not.Null);
            try
            {
                method.Invoke(repository, new object[] { -1 });
            }
            catch (TargetInvocationException ex)
            {
                Assert.That(ex.InnerException, Is.Not.Null);
                Assert.That(ex.InnerException, Is.TypeOf(typeof(IntranetRepositoryException)));
            }
        }

        /// <summary>
        /// Tester, at MapKontogruppe kaster en ArgumentNullException, hvis kontogruppeviewet er null.
        /// </summary>
        [Test]
        public void TestAtMapKontogruppeKasterArgumentNullExceptionHvisKontogruppeViewErNull()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var repository = new FinansstyringRepository(channelFactory);
            var method = repository.GetType().GetMethod("MapKontogruppe", BindingFlags.NonPublic | BindingFlags.Static);
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
        /// Tester, at MapBudgetkontogruppe kaster en ArgumentNullException, hvis budgetkontogruppeviewet er null.
        /// </summary>
        [Test]
        public void TestAtMapBudgetkontogruppeKasterArgumentNullExceptionHvisBudgetkontogruppeViewErNull()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();

            var repository = new FinansstyringRepository(channelFactory);
            var method = repository.GetType().GetMethod("MapBudgetkontogruppe", BindingFlags.NonPublic | BindingFlags.Static);
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
        /// Danner regnskabsliste til test.
        /// </summary>
        /// <returns>Regnskabsliste til test.</returns>
        private static IList<RegnskabListeView> GetRegnskabsliste()
        {
            return new List<RegnskabListeView>
                       {
                           new RegnskabListeView
                               {
                                   Nummer = 1,
                                   Navn = "Ole Sørensen"
                               },
                           new RegnskabListeView
                               {
                                   Nummer = 2,
                                   Navn = "Bryllup"
                               }
                       };
        }

        /// <summary>
        /// Danner regnskab til test.
        /// </summary>
        /// <returns>Regnskab til test.</returns>
        private static RegnskabView GetRegnskab()
        {
            return new RegnskabView
                       {
                           Nummer = 1,
                           Navn = "Ole Sørensen",
                           Konti = new List<KontoView>
                                       {
                                           new KontoView
                                               {
                                                   Kontonummer = "DANKORT",
                                                   Kontonavn = "Dankort",
                                                   Beskrivelse = "Dankort/lønkonto",
                                                   Note = "Bla, bla og mere bla",
                                                   Kontogruppe = new KontogruppeView
                                                                     {
                                                                         Nummer = 1
                                                                     },
                                                   Kreditoplysninger = new List<KreditoplysningerView>
                                                                           {
                                                                               new KreditoplysningerView
                                                                                   {
                                                                                       År = 2011,
                                                                                       Måned = 4,
                                                                                       Kredit = 30000M
                                                                                   }
                                                                           },
                                                   Bogføringslinjer = new List<BogføringslinjeView>
                                                                          {
                                                                              new BogføringslinjeView
                                                                                  {
                                                                                      Løbenummer = 1,
                                                                                      Dato = new DateTime(2011, 4, 1),
                                                                                      Konto = new KontoListeView
                                                                                                  {
                                                                                                      Kontonummer = "DANKORT"
                                                                                                  },
                                                                                      Tekst = "Saldo",
                                                                                      Debit = 10000M
                                                                                  },
                                                                              new BogføringslinjeView
                                                                                  {
                                                                                      Løbenummer = 3,
                                                                                      Dato = new DateTime(2011, 4, 1),
                                                                                      Konto = new KontoListeView
                                                                                                  {
                                                                                                      Kontonummer = "DANKORT"
                                                                                                  },
                                                                                      Tekst = "Løn",
                                                                                      Budgetkonto =
                                                                                          new BudgetkontoListeView
                                                                                              {
                                                                                                  Kontonummer = "1000"
                                                                                              },
                                                                                      Debit = 25000M
                                                                                  },
                                                                              new BogføringslinjeView
                                                                                  {
                                                                                      Løbenummer = 4,
                                                                                      Dato = new DateTime(2011, 4, 1),
                                                                                      Konto = new KontoListeView
                                                                                                  {
                                                                                                      Kontonummer = "DANKORT"
                                                                                                  },
                                                                                      Tekst = "Indbetaling",
                                                                                      Budgetkonto =
                                                                                          new BudgetkontoListeView
                                                                                              {
                                                                                                  Kontonummer = "1010"
                                                                                              },
                                                                                      Debit = 500M,
                                                                                      Adresse = new AdressereferenceView
                                                                                                    {
                                                                                                        Nummer = 1
                                                                                                    }
                                                                                  },
                                                                              new BogføringslinjeView
                                                                                  {
                                                                                      Løbenummer = 5,
                                                                                      Dato = new DateTime(2011, 4, 1),
                                                                                      Konto = new KontoListeView
                                                                                                  {
                                                                                                      Kontonummer = "DANKORT"
                                                                                                  },
                                                                                      Tekst = "Budget",
                                                                                      Budgetkonto =
                                                                                          new BudgetkontoListeView
                                                                                              {
                                                                                                  Kontonummer = "2000"
                                                                                              },
                                                                                      Kredit = 7500M
                                                                                  },
                                                                              new BogføringslinjeView
                                                                                  {
                                                                                      Løbenummer = 6,
                                                                                      Dato = new DateTime(2011, 4, 1),
                                                                                      Konto = new KontoListeView
                                                                                                  {
                                                                                                      Kontonummer = "DANKORT"
                                                                                                  },
                                                                                      Tekst = "Udlån",
                                                                                      Budgetkonto =
                                                                                          new BudgetkontoListeView
                                                                                              {
                                                                                                  Kontonummer = "2010"
                                                                                              },
                                                                                      Kredit = 2500M,
                                                                                      Adresse = new AdressereferenceView
                                                                                                    {
                                                                                                        Nummer = 1
                                                                                                    }
                                                                                  },
                                                                              new BogføringslinjeView
                                                                                  {
                                                                                      Løbenummer = 7,
                                                                                      Dato = new DateTime(2011, 4, 1),
                                                                                      Konto = new KontoListeView
                                                                                                  {
                                                                                                      Kontonummer = "DANKORT"
                                                                                                  },
                                                                                      Tekst = "Tab",
                                                                                      Budgetkonto =
                                                                                          new BudgetkontoListeView
                                                                                              {
                                                                                                  Kontonummer = "2020"
                                                                                              },
                                                                                      Kredit = 700M
                                                                                  }
                                                                          }
                                               },
                                           new KontoView
                                               {
                                                   Kontonummer = "KONTANTER",
                                                   Kontonavn = "Kontanter",
                                                   Kontogruppe = new KontogruppeView
                                                                     {
                                                                         Nummer = 2
                                                                     },
                                                   Kreditoplysninger = new List<KreditoplysningerView>
                                                                           {
                                                                               new KreditoplysningerView
                                                                                   {
                                                                                       År = 2011,
                                                                                       Måned = 4,
                                                                                       Kredit = 0M
                                                                                   }
                                                                           },
                                                   Bogføringslinjer = new List<BogføringslinjeView>
                                                                          {
                                                                              new BogføringslinjeView
                                                                                  {
                                                                                      Løbenummer = 2,
                                                                                      Dato = new DateTime(2011, 4, 1),
                                                                                      Konto = new KontoListeView
                                                                                                  {
                                                                                                      Kontonummer = "KONTANTER"
                                                                                                  },
                                                                                      Tekst = "Saldo",
                                                                                      Debit = 250M
                                                                                  }
                                                                          }
                                               }
                                       },
                           Budgetkonti = new List<BudgetkontoView>
                                             {
                                                 new BudgetkontoView
                                                     {
                                                         Kontonummer = "1000",
                                                         Kontonavn = "Lønninger",
                                                         Beskrivelse = "Lønindtægter",
                                                         Note = "Bla, bla og mere bla",
                                                         Budgetkontogruppe = new BudgetkontogruppeView
                                                                                 {
                                                                                     Nummer = 1
                                                                                 },
                                                         Budgetoplysninger = new List<BudgetoplysningerView>
                                                                                 {
                                                                                     new BudgetoplysningerView
                                                                                         {
                                                                                             År = 2011,
                                                                                             Måned = 4,
                                                                                             Indtægter = 0M,
                                                                                             Udgifter = 0M
                                                                                         }
                                                                                 }
                                                     },
                                                 new BudgetkontoView
                                                     {
                                                         Kontonummer = "1010",
                                                         Kontonavn = "Øvrige indtægter",
                                                         Budgetkontogruppe = new BudgetkontogruppeView
                                                                                 {
                                                                                     Nummer = 1
                                                                                 },
                                                         Budgetoplysninger = new List<BudgetoplysningerView>
                                                                                 {
                                                                                     new BudgetoplysningerView
                                                                                         {
                                                                                             År = 2011,
                                                                                             Måned = 4,
                                                                                             Indtægter = 0M,
                                                                                             Udgifter = 0M
                                                                                         }
                                                                                 }
                                                     },
                                                 new BudgetkontoView
                                                     {
                                                         Kontonummer = "2000",
                                                         Kontonavn = "Budget",
                                                         Beskrivelse = "Fast budgetterede omkostninger",
                                                         Note = "Bla, bla og mere bla",
                                                         Budgetkontogruppe = new BudgetkontogruppeView
                                                                                 {
                                                                                     Nummer = 2
                                                                                 },
                                                         Budgetoplysninger = new List<BudgetoplysningerView>
                                                                                 {
                                                                                     new BudgetoplysningerView
                                                                                         {
                                                                                             År = 2011,
                                                                                             Måned = 4,
                                                                                             Indtægter = 0M,
                                                                                             Udgifter = 7500M
                                                                                         }
                                                                                 }
                                                     },
                                                 new BudgetkontoView
                                                     {
                                                         Kontonummer = "2010",
                                                         Kontonavn = "Udlån",
                                                         Budgetkontogruppe = new BudgetkontogruppeView
                                                                                 {
                                                                                     Nummer = 2
                                                                                 },
                                                         Budgetoplysninger = new List<BudgetoplysningerView>
                                                                                 {
                                                                                     new BudgetoplysningerView
                                                                                         {
                                                                                             År = 2011,
                                                                                             Måned = 4,
                                                                                             Indtægter = 0M,
                                                                                             Udgifter = 0M
                                                                                         }
                                                                                 }
                                                     },
                                                 new BudgetkontoView
                                                     {
                                                         Kontonummer = "2020",
                                                         Kontonavn = "Øvrige udgifter",
                                                         Budgetkontogruppe = new BudgetkontogruppeView
                                                                                 {
                                                                                     Nummer = 2
                                                                                 },
                                                         Budgetoplysninger = new List<BudgetoplysningerView>
                                                                                 {
                                                                                     new BudgetoplysningerView
                                                                                         {
                                                                                             År = 2011,
                                                                                             Måned = 4,
                                                                                             Indtægter = 0M,
                                                                                             Udgifter = 0M
                                                                                         }
                                                                                 }
                                                     }
                                             }
                       };
        }

        /// <summary>
        /// Danner kontogrupper til test.
        /// </summary>
        /// <returns>Kontogrupper til test.</returns>
        private static IList<KontogruppeView> GetKontogrupper()
        {
            return new List<KontogruppeView>
                       {
                           new KontogruppeView
                               {
                                   Nummer = 1,
                                   Navn = "Bankkonti",
                                   KontogruppeType = KontogruppeType.Aktiver
                               },
                           new KontogruppeView
                               {
                                   Nummer = 2,
                                   Navn = "Kontanter",
                                   KontogruppeType = KontogruppeType.Aktiver
                               }
                       };
        }

        /// <summary>
        /// Danner grupper for budgetkonti til test.
        /// </summary>
        /// <returns>Grupper for budgetkonti til test</returns>
        private static IList<BudgetkontogruppeView> GetBudgetkontogrupper()
        {
            return new List<BudgetkontogruppeView>
                       {
                           new BudgetkontogruppeView
                               {
                                   Nummer = 1,
                                   Navn = "Indtægter"
                               },
                           new BudgetkontogruppeView
                               {
                                   Nummer = 2,
                                   Navn = "Udgifter"
                               }
                       };
        }
    }
}
