using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.CommonLibrary.IoC;
using OSDevGrp.OSIntranet.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.Domain.Fælles;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Fælles;

namespace OSDevGrp.OSIntranet.Tests.Integrationstests.Repositories
{
    /// <summary>
    /// Integrationstester repository for finansstyring.
    /// </summary>
    [TestFixture]
    [Category("Integrationstest")]
    public class FinansstyringRepositoryTests
    {
        #region Private variables

        private IAdresseRepository _adresseRepository;
        private IFinansstyringRepository _finansstyringRepository;
        private IFællesRepository _fællesRepository;

        #endregion

        /// <summary>
        /// Opsætning af tests.
        /// </summary>
        [SetUp]
        public void TestSetUp()
        {
            var container = ContainerFactory.Create();
            _adresseRepository = container.Resolve<IAdresseRepository>();
            _finansstyringRepository = container.Resolve<IFinansstyringRepository>();
            _fællesRepository = container.Resolve<IFællesRepository>();
        }

        /// <summary>
        /// Tester, at RegnskabslisteGet henter regnskaber.
        /// </summary>
        [Test]
        public void TestAtRegnskabslisteGetHenterRegnskaber()
        {
            var brevhovedlisteHelper = new BrevhovedlisteHelper(_fællesRepository.BrevhovedGetAll());
            var regnskaber = _finansstyringRepository.RegnskabslisteGet(brevhovedlisteHelper.GetById);
            Assert.That(regnskaber, Is.Not.Null);
            Assert.That(regnskaber.Count(), Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at RegnskabGet henter et givent regnskab.
        /// </summary>
        [Test]
        public void TestAtRegnskabGetHenterRegnskab()
        {
            var adresselisteHelper = new AdresselisteHelper(_adresseRepository.AdresseGetAll());
            var brevhovedlisteHelper = new BrevhovedlisteHelper(_fællesRepository.BrevhovedGetAll());
            var regnskab = _finansstyringRepository.RegnskabGet(1, brevhovedlisteHelper.GetById,
                                                                adresselisteHelper.GetById);
            Assert.That(regnskab, Is.Not.Null);
            Assert.That(regnskab.Nummer, Is.EqualTo(1));
        }

        /// <summary>
        /// Tester, at KontogruppeGetAll henter kontogrupper.
        /// </summary>
        [Test]
        public void TestAtKontogruppeGetAllHenterKontogrupper()
        {
            var kontogrupper = _finansstyringRepository.KontogruppeGetAll();
            Assert.That(kontogrupper, Is.Not.Null);
            Assert.That(kontogrupper.Count(), Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at BudgetkontogruppeGetAll henter grupper for budgetkonti.
        /// </summary>
        [Test]
        public void TestAtBudgetkontogruppeGetAllHenterBudgetkontogrupper()
        {
            var budgetkontogrupper = _finansstyringRepository.BudgetkontogruppeGetAll();
            Assert.That(budgetkontogrupper, Is.Not.Null);
            Assert.That(budgetkontogrupper.Count(), Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at BogføringslinjeAdd, tilføjer bogføringslinjer.
        /// </summary>
        [Test]
        [Ignore("Oprettelse af bogføringslinjer er testet.")]
        public void TestAtBogføringslinjeAddTilføjerBogføringslinjer()
        {
            // Hent regnskab og find nødvendige konti.
            var adresselisteHelper = new AdresselisteHelper(_adresseRepository.AdresseGetAll());
            var brevhovedlisteHelper = new BrevhovedlisteHelper(_fællesRepository.BrevhovedGetAll());
            var regnskab = _finansstyringRepository.RegnskabGet(1, brevhovedlisteHelper.GetById,
                                                                adresselisteHelper.GetById);
            var kontoDankort = regnskab.Konti.OfType<Konto>().Single(m => m.Kontonummer == "DANKORT");
            var budgetkontoØvrigeUdgifter = regnskab.Konti.OfType<Budgetkonto>().Single(m => m.Kontonummer == "8990");
            var bogføringer = budgetkontoØvrigeUdgifter.Bogføringslinjer.Count();

            // Opret bogføringer.
            var result1 = _finansstyringRepository.BogføringslinjeAdd(DateTime.Now, null, kontoDankort,
                                                                      "Test fra Repositories", budgetkontoØvrigeUdgifter,
                                                                      5000M, 0M, null);
            Assert.That(result1, Is.Not.Null);
            Assert.That(result1.Løbenummer, Is.GreaterThan(0));
            var result2 = _finansstyringRepository.BogføringslinjeAdd(DateTime.Now, null, kontoDankort,
                                                                      "Test fra Repositories", budgetkontoØvrigeUdgifter,
                                                                      0M, 5000M, null);
            Assert.That(result2, Is.Not.Null);
            Assert.That(result2.Løbenummer, Is.GreaterThan(0));

            // Genindlæs regnskab og find nødvendige konti.
            regnskab = _finansstyringRepository.RegnskabGet(1, brevhovedlisteHelper.GetById, adresselisteHelper.GetById);
            budgetkontoØvrigeUdgifter = regnskab.Konti.OfType<Budgetkonto>().Single(m => m.Kontonummer == "8990");
            Assert.That(budgetkontoØvrigeUdgifter.Bogføringslinjer.Count(), Is.EqualTo(bogføringer + 2));
        }

        [Test] 
        [Ignore("Used for data migration")]
        public void Export()
        {
            IBrevhovedlisteHelper letterHeadHelper = new BrevhovedlisteHelper(_fællesRepository.BrevhovedGetAll());
            IEnumerable<Regnskab> accountingCollection = _finansstyringRepository.RegnskabslisteGet(letterHeadHelper.GetById);

            XmlDocument accountingDocument = new XmlDocument();
            accountingDocument.AppendChild(accountingDocument.CreateXmlDeclaration("1.0", Encoding.UTF8.BodyName, null));

            XmlElement accountingCollectionElement = accountingDocument.CreateElement("Accountings");
            accountingDocument.AppendChild(accountingCollectionElement);

            foreach (Regnskab accounting in accountingCollection.OrderBy(m => m.Nummer))
            {
                XmlElement accountingElement = accountingDocument.CreateElement("Accounting");

                accountingElement.Attributes.Append(accountingDocument.CreateAttribute("number"));
                accountingElement.Attributes["number"].Value = accounting.Nummer.ToString();

                accountingElement.Attributes.Append(accountingDocument.CreateAttribute("name"));
                accountingElement.Attributes["name"].Value = accounting.Navn.Trim();

                accountingElement.Attributes.Append(accountingDocument.CreateAttribute("letterHeadNumber"));
                accountingElement.Attributes["letterHeadNumber"].Value = accounting.Brevhoved.Nummer.ToString();

                accountingCollectionElement.AppendChild(accountingElement);
            }

            using (FileStream fileStream = new FileStream(@"Accountings.xml", FileMode.Create, FileAccess.Write, FileShare.None))
            {
                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Encoding = Encoding.UTF8
                };
                using (XmlWriter xmlWriter = XmlWriter.Create(fileStream, settings))
                {
                    accountingDocument.WriteTo(xmlWriter);

                    xmlWriter.Flush();
                    xmlWriter.Close();
                }
            }
        }
    }
}
