using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf.ChannelFactory;
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
            Assert.That(regnskab.Konti.OfType<Konto>().Count(), Is.EqualTo(5));

            var person = new Person(1, "Ole Sørensen", new Adressegruppe(1, "Familie (Ole)", 1));
            regnskab = repository.RegnskabGet(1, nummer => person);
            Assert.That(regnskab, Is.Not.Null);
            Assert.That(regnskab.Nummer, Is.EqualTo(1));
            Assert.That(regnskab.Navn, Is.Not.Null);
            Assert.That(regnskab.Navn, Is.EqualTo("Ole Sørensen"));
            Assert.That(regnskab.Konti, Is.Not.Null);
            Assert.That(regnskab.Konti.Count, Is.GreaterThan(0));
            Assert.That(regnskab.Konti.OfType<Konto>().Count(), Is.EqualTo(5));
            foreach (var konto in regnskab.Konti.OfType<Konto>().ToList())
            {
                Assert.That(konto, Is.Not.Null);
                Assert.That(konto.Kreditoplysninger, Is.Not.Null);
                Assert.That(konto.Kreditoplysninger.Count, Is.GreaterThan(0));
                Assert.That(konto.Bogføringslinjer, Is.Not.Null);
                Assert.That(konto.Bogføringslinjer.Count, Is.GreaterThanOrEqualTo(0));
            }
            Assert.That(regnskab.Konti.OfType<Budgetkonto>().Count(), Is.EqualTo(71));
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
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
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
            Assert.That(konto.Note, Is.Null);
            Assert.That(konto.Kontogruppe, Is.Not.Null);
            Assert.That(konto.Kontogruppe.Nummer, Is.EqualTo(1));
            Assert.That(konto.Kontogruppe.Navn, Is.Not.Null);
            Assert.That(konto.Kontogruppe.Navn, Is.EqualTo("Bankkonti"));
            Assert.That(konto.Kontogruppe.KontogruppeType, Is.EqualTo(KontogruppeType.Aktiver));
            Assert.That(konto.Kreditoplysninger.Where(m => m.År <= 2010 && m.Måned < 11).Count(), Is.EqualTo(122));
            var kreditoplysninger = konto.Kreditoplysninger.SingleOrDefault(m => m.År == 2000 && m.Måned == 12);
            Assert.That(kreditoplysninger, Is.Not.Null);
            Assert.That(kreditoplysninger.År, Is.EqualTo(2000));
            Assert.That(kreditoplysninger.Måned, Is.EqualTo(12));
            Assert.That(kreditoplysninger.Kredit, Is.EqualTo(30000M));
            var calculateTo = new DateTime(2010, 10, 31);
            Assert.That(konto.Bogføringslinjer.Where(m => m.Dato.Date.CompareTo(calculateTo) <= 0).Count(), Is.EqualTo(7458));
        }

        /// <summary>
        /// Tester, at en budgetkonto mappes korrekt.
        /// </summary>
        [Test]
        public void TestAtBudgetkontoMappesKorrekt()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            var repository = new FinansstyringRepository(channelFactory);
            var regnskab = repository.RegnskabGet(1);
            Assert.That(regnskab, Is.Not.Null);
            Assert.That(regnskab.Konti, Is.Not.Null);
            Assert.That(regnskab.Konti.Count, Is.GreaterThan(0));
            var budgetkonto = regnskab.Konti.OfType<Budgetkonto>().SingleOrDefault(m => m.Kontonummer.CompareTo("3000") == 0);
            Assert.That(budgetkonto, Is.Not.Null);
            Assert.That(budgetkonto.Regnskab, Is.Not.Null);
            Assert.That(budgetkonto.Regnskab.Nummer, Is.EqualTo(1));
            Assert.That(budgetkonto.Regnskab.Navn, Is.Not.Null);
            Assert.That(budgetkonto.Regnskab.Navn, Is.EqualTo("Ole Sørensen"));
            Assert.That(budgetkonto.Kontonavn, Is.Not.Null);
            Assert.That(budgetkonto.Kontonavn, Is.EqualTo("Supermarkeder"));
            Assert.That(budgetkonto.Beskrivelse, Is.Null);
            Assert.That(budgetkonto.Note, Is.Null);
            Assert.That(budgetkonto.Budgetkontogruppe, Is.Not.Null);
            Assert.That(budgetkonto.Budgetkontogruppe.Nummer, Is.EqualTo(3));
            Assert.That(budgetkonto.Budgetkontogruppe.Navn, Is.Not.Null);
            Assert.That(budgetkonto.Budgetkontogruppe.Navn, Is.EqualTo("Dagligvareforretninger"));
            Assert.That(budgetkonto.Budgetoplysninger.Where(m => m.År <= 2010 && m.Måned < 11).Count(), Is.EqualTo(122));
            var budgetoplysnigner = budgetkonto.Budgetoplysninger.SingleOrDefault(m => m.År == 2000 && m.Måned == 12);
            Assert.That(budgetoplysnigner, Is.Not.Null);
            Assert.That(budgetoplysnigner.År, Is.EqualTo(2000));
            Assert.That(budgetoplysnigner.Måned, Is.EqualTo(12));
            Assert.That(budgetoplysnigner.Indtægter, Is.EqualTo(0M));
            Assert.That(budgetoplysnigner.Udgifter, Is.EqualTo(1500M));
            var calculateTo = new DateTime(2010, 10, 31);
            Assert.That(budgetkonto.Bogføringslinjer.Where(m => m.Dato.Date.CompareTo(calculateTo) <= 0).Count(), Is.EqualTo(1902));
        }

        /// <summary>
        /// Tester, at RegnskabGet kaster en IntranetRepositoryException, hvis regnskabet ikke findes.
        /// </summary>
        [Test]
        public void TestAtRegnskabGetKasterIntranetRepositoryExceptionHvisRegnskabIkkeFindes()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            var repository = new FinansstyringRepository(channelFactory);
            Assert.Throws<IntranetRepositoryException>(() => repository.RegnskabGet(-1));
        }

        /// <summary>
        /// Tester, at KontogruppeGetAll henter alle kontogrupper.
        /// </summary>
        [Test]
        public void TestAtKontogruppeGetAllHenterAlleKontogrupper()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            var repository = new FinansstyringRepository(channelFactory);
            var kontogrupper = repository.KontogruppeGetAll();
            Assert.That(kontogrupper, Is.Not.Null);
            Assert.That(kontogrupper.Count, Is.EqualTo(2));
            Assert.That(kontogrupper[0], Is.Not.Null);
            Assert.That(kontogrupper[0].Nummer, Is.EqualTo(1));
            Assert.That(kontogrupper[0].Navn, Is.Not.Null);
            Assert.That(kontogrupper[0].Navn, Is.EqualTo("Bankkonti"));
            Assert.That(kontogrupper[0].KontogruppeType, Is.EqualTo(KontogruppeType.Aktiver));
            Assert.That(kontogrupper[1], Is.Not.Null);
            Assert.That(kontogrupper[1].Nummer, Is.EqualTo(2));
            Assert.That(kontogrupper[1].Navn, Is.Not.Null);
            Assert.That(kontogrupper[1].Navn, Is.EqualTo("Kontanter"));
            Assert.That(kontogrupper[1].KontogruppeType, Is.EqualTo(KontogruppeType.Aktiver));
        }

        /// <summary>
        /// Tester, at BudgetkontogruppeGetAll henter alle budgetkontogrupper.
        /// </summary>
        [Test]
        public void TestAtBudgetkontogruppeGetAllHenterAlleBudgetkontogrupper()
        {
            var channelFactory = MockRepository.GenerateMock<IChannelFactory>();
            var repository = new FinansstyringRepository(channelFactory);
            var budgetkontogrupper = repository.BudgetkontogruppeGetAll();
            Assert.That(budgetkontogrupper, Is.Not.Null);
            Assert.That(budgetkontogrupper.Count, Is.EqualTo(8));
            Assert.That(budgetkontogrupper[0], Is.Not.Null);
            Assert.That(budgetkontogrupper[0].Nummer, Is.EqualTo(1));
            Assert.That(budgetkontogrupper[0].Navn, Is.Not.Null);
            Assert.That(budgetkontogrupper[0].Navn, Is.EqualTo("Indtægter"));
            Assert.That(budgetkontogrupper[1], Is.Not.Null);
            Assert.That(budgetkontogrupper[1].Nummer, Is.EqualTo(2));
            Assert.That(budgetkontogrupper[1].Navn, Is.Not.Null);
            Assert.That(budgetkontogrupper[1].Navn, Is.EqualTo("Faste udgifter"));
            Assert.That(budgetkontogrupper[2], Is.Not.Null);
            Assert.That(budgetkontogrupper[2].Nummer, Is.EqualTo(3));
            Assert.That(budgetkontogrupper[2].Navn, Is.Not.Null);
            Assert.That(budgetkontogrupper[2].Navn, Is.EqualTo("Dagligvareforretninger"));
            Assert.That(budgetkontogrupper[3], Is.Not.Null);
            Assert.That(budgetkontogrupper[3].Nummer, Is.EqualTo(4));
            Assert.That(budgetkontogrupper[3].Navn, Is.Not.Null);
            Assert.That(budgetkontogrupper[3].Navn, Is.EqualTo("Specialvareforretninger"));
            Assert.That(budgetkontogrupper[4], Is.Not.Null);
            Assert.That(budgetkontogrupper[4].Nummer, Is.EqualTo(5));
            Assert.That(budgetkontogrupper[4].Navn, Is.Not.Null);
            Assert.That(budgetkontogrupper[4].Navn, Is.EqualTo("Restaurationer"));
            Assert.That(budgetkontogrupper[5], Is.Not.Null);
            Assert.That(budgetkontogrupper[5].Nummer, Is.EqualTo(6));
            Assert.That(budgetkontogrupper[5].Navn, Is.Not.Null);
            Assert.That(budgetkontogrupper[5].Navn, Is.EqualTo("Transportomkostninger"));
            Assert.That(budgetkontogrupper[6], Is.Not.Null);
            Assert.That(budgetkontogrupper[6].Nummer, Is.EqualTo(7));
            Assert.That(budgetkontogrupper[6].Navn, Is.Not.Null);
            Assert.That(budgetkontogrupper[6].Navn, Is.EqualTo("Forlystelser"));
            Assert.That(budgetkontogrupper[7], Is.Not.Null);
            Assert.That(budgetkontogrupper[7].Nummer, Is.EqualTo(8));
            Assert.That(budgetkontogrupper[7].Navn, Is.Not.Null);
            Assert.That(budgetkontogrupper[7].Navn, Is.EqualTo("Øvrige udgifter"));
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
                                                   Kontogruppe = new KontogruppeView
                                                                     {
                                                                         Nummer = 1
                                                                     }
                                               },
                                           new KontoView
                                               {
                                                   Kontonummer = "KONTANTER",
                                                   Kontonavn = "Kontanter",
                                                   Kontogruppe = new KontogruppeView
                                                                     {
                                                                         Nummer = 2
                                                                     }
                                               }
                                       },
                           Budgetkonti = new List<BudgetkontoView>()
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
