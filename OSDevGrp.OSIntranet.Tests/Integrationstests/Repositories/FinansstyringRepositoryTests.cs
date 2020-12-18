using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.CommonLibrary.IoC;
using OSDevGrp.OSIntranet.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.Domain.Fælles;
using OSDevGrp.OSIntranet.Domain.Interfaces.Adressekartotek;
using OSDevGrp.OSIntranet.Domain.Interfaces.Fælles;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

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
            IAdresselisteHelper addressHelper = new AdresselisteHelper(_adresseRepository.AdresseGetAll());
            IEnumerable<Regnskab> accountingCollection = _finansstyringRepository
                .RegnskabslisteGet(letterHeadHelper.GetById)
                .Select(accounting => _finansstyringRepository.RegnskabGet(accounting.Nummer, letterHeadHelper.GetById, addressHelper.GetById))
                .ToArray();

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

                foreach (Konto account in accounting.Konti.OfType<Konto>())
                {
                    XmlElement accountElement = accountingDocument.CreateElement("Account");

                    accountElement.Attributes.Append(accountingDocument.CreateAttribute("accountNumber"));
                    accountElement.Attributes["accountNumber"].Value = account.Kontonummer.Trim().ToUpper();

                    accountElement.Attributes.Append(accountingDocument.CreateAttribute("accountName"));
                    accountElement.Attributes["accountName"].Value = account.Kontonavn.Trim();

                    if (string.IsNullOrWhiteSpace(account.Beskrivelse) == false)
                    {
                        accountElement.Attributes.Append(accountingDocument.CreateAttribute("description"));
                        accountElement.Attributes["description"].Value = account.Beskrivelse.Trim();
                    }

                    if (string.IsNullOrWhiteSpace(account.Note) == false)
                    {
                        accountElement.Attributes.Append(accountingDocument.CreateAttribute("note"));
                        accountElement.Attributes["note"].Value = account.Note.Trim();
                    }

                    accountElement.Attributes.Append(accountingDocument.CreateAttribute("accountGroup"));
                    accountElement.Attributes["accountGroup"].Value = account.Kontogruppe.Nummer.ToString();

                    foreach (Kreditoplysninger creditInfo in account.Kreditoplysninger.OrderBy(m => m.År).ThenBy(m => m.Måned))
                    {
                        XmlElement creditInfoElement = accountingDocument.CreateElement("CreditInfo");

                        creditInfoElement.Attributes.Append(accountingDocument.CreateAttribute("year"));
                        creditInfoElement.Attributes["year"].Value = creditInfo.År.ToString();

                        creditInfoElement.Attributes.Append(accountingDocument.CreateAttribute("month"));
                        creditInfoElement.Attributes["month"].Value = creditInfo.Måned.ToString();

                        creditInfoElement.Attributes.Append(accountingDocument.CreateAttribute("credit"));
                        creditInfoElement.Attributes["credit"].Value = creditInfo.Kredit.ToString("0.00", CultureInfo.InvariantCulture);

                        accountElement.AppendChild(creditInfoElement);
                    }

                    accountingElement.AppendChild(accountElement);
                }

                foreach (Budgetkonto budgetAccount in accounting.Konti.OfType<Budgetkonto>())
                {
                    XmlElement budgetAccountElement = accountingDocument.CreateElement("BudgetAccount");

                    budgetAccountElement.Attributes.Append(accountingDocument.CreateAttribute("accountNumber"));
                    budgetAccountElement.Attributes["accountNumber"].Value = budgetAccount.Kontonummer.Trim().ToUpper();

                    budgetAccountElement.Attributes.Append(accountingDocument.CreateAttribute("accountName"));
                    budgetAccountElement.Attributes["accountName"].Value = budgetAccount.Kontonavn;

                    if (string.IsNullOrWhiteSpace(budgetAccount.Beskrivelse) == false)
                    {
                        budgetAccountElement.Attributes.Append(accountingDocument.CreateAttribute("description"));
                        budgetAccountElement.Attributes["description"].Value = budgetAccount.Beskrivelse.Trim();
                    }

                    if (string.IsNullOrWhiteSpace(budgetAccount.Note) == false)
                    {
                        budgetAccountElement.Attributes.Append(accountingDocument.CreateAttribute("note"));
                        budgetAccountElement.Attributes["note"].Value = budgetAccount.Note.Trim();
                    }

                    budgetAccountElement.Attributes.Append(accountingDocument.CreateAttribute("budgetAccountGroup"));
                    budgetAccountElement.Attributes["budgetAccountGroup"].Value = budgetAccount.Budgetkontogruppe.Nummer.ToString();

                    foreach (Budgetoplysninger budgetInfo in budgetAccount.Budgetoplysninger.OrderBy(m => m.År).ThenBy(m => m.Måned))
                    {
                        XmlElement budgetInfoElement = accountingDocument.CreateElement("BudgetInfo");

                        budgetInfoElement.Attributes.Append(accountingDocument.CreateAttribute("year"));
                        budgetInfoElement.Attributes["year"].Value = budgetInfo.År.ToString();

                        budgetInfoElement.Attributes.Append(accountingDocument.CreateAttribute("month"));
                        budgetInfoElement.Attributes["month"].Value = budgetInfo.Måned.ToString();

                        budgetInfoElement.Attributes.Append(accountingDocument.CreateAttribute("income"));
                        budgetInfoElement.Attributes["income"].Value = budgetInfo.Indtægter.ToString("0.00", CultureInfo.InvariantCulture);

                        budgetInfoElement.Attributes.Append(accountingDocument.CreateAttribute("expenses"));
                        budgetInfoElement.Attributes["expenses"].Value = budgetInfo.Udgifter.ToString("0.00", CultureInfo.InvariantCulture);

                        budgetAccountElement.AppendChild(budgetInfoElement);
                    }

                    accountingElement.AppendChild(budgetAccountElement);
                }

                AdresseBase[] contactAccountCollection = accounting.Konti.OfType<Konto>()
                    .SelectMany(account => account.Bogføringslinjer)
                    .Where(postingLine => postingLine.Adresse != null)
                    .Select(postingLine => postingLine.Adresse.Nummer)
                    .Distinct()
                    .Select(number => addressHelper.GetById(number))
                    .ToArray();
                foreach (AdresseBase contactAccount in contactAccountCollection)
                {
                    XmlElement contactAccountElement = accountingDocument.CreateElement("ContactAccount");

                    contactAccountElement.Attributes.Append(accountingDocument.CreateAttribute("accountNumber"));
                    contactAccountElement.Attributes["accountNumber"].Value = contactAccount.Nummer.ToString();

                    contactAccountElement.Attributes.Append(accountingDocument.CreateAttribute("accountName"));
                    contactAccountElement.Attributes["accountName"].Value = contactAccount.Navn.Trim();

                    if (contactAccount is Person person)
                    {
                        if (string.IsNullOrWhiteSpace(person.Mobil) == false && string.IsNullOrWhiteSpace(person.Telefon) == false)
                        {
                            contactAccountElement.Attributes.Append(accountingDocument.CreateAttribute("primaryPhone"));
                            contactAccountElement.Attributes["primaryPhone"].Value = person.Telefon.Trim();

                            if (string.CompareOrdinal(person.Mobil, person.Telefon) != 0)
                            {
                                contactAccountElement.Attributes.Append(accountingDocument.CreateAttribute("secondaryPhone"));
                                contactAccountElement.Attributes["secondaryPhone"].Value = person.Mobil.Trim();
                            }
                        }
                        else if (string.IsNullOrWhiteSpace(person.Mobil) == false && string.IsNullOrWhiteSpace(person.Telefon))
                        {
                            contactAccountElement.Attributes.Append(accountingDocument.CreateAttribute("primaryPhone"));
                            contactAccountElement.Attributes["primaryPhone"].Value = person.Mobil.Trim();
                        }
                        else if (string.IsNullOrWhiteSpace(person.Mobil) && string.IsNullOrWhiteSpace(person.Telefon) == false)
                        {
                            contactAccountElement.Attributes.Append(accountingDocument.CreateAttribute("primaryPhone"));
                            contactAccountElement.Attributes["primaryPhone"].Value = person.Telefon.Trim();
                        }
                    }

                    if (contactAccount is Firma company)
                    {
                        if (string.IsNullOrWhiteSpace(company.Telefon1) == false && string.IsNullOrWhiteSpace(company.Telefon2) == false)
                        {
                            contactAccountElement.Attributes.Append(accountingDocument.CreateAttribute("primaryPhone"));
                            contactAccountElement.Attributes["primaryPhone"].Value = company.Telefon1.Trim();

                            if (string.CompareOrdinal(company.Telefon1, company.Telefon2) != 0)
                            {
                                contactAccountElement.Attributes.Append(accountingDocument.CreateAttribute("secondaryPhone"));
                                contactAccountElement.Attributes["secondaryPhone"].Value = company.Telefon2.Trim();
                            }
                        }
                        else if (string.IsNullOrWhiteSpace(company.Telefon1) == false && string.IsNullOrWhiteSpace(company.Telefon2))
                        {
                            contactAccountElement.Attributes.Append(accountingDocument.CreateAttribute("primaryPhone"));
                            contactAccountElement.Attributes["primaryPhone"].Value = company.Telefon1.Trim();
                        }
                        else if (string.IsNullOrWhiteSpace(company.Telefon1) && string.IsNullOrWhiteSpace(company.Telefon2) == false)
                        {
                            contactAccountElement.Attributes.Append(accountingDocument.CreateAttribute("primaryPhone"));
                            contactAccountElement.Attributes["primaryPhone"].Value = company.Telefon2.Trim();
                        }
                    }

                    if (string.IsNullOrWhiteSpace(contactAccount.Mailadresse) == false)
                    {
                        contactAccountElement.Attributes.Append(accountingDocument.CreateAttribute("mailAddress"));
                        contactAccountElement.Attributes["mailAddress"].Value = contactAccount.Mailadresse.Trim();
                    }

                    contactAccountElement.Attributes.Append(accountingDocument.CreateAttribute("paymentTermNumber"));
                    contactAccountElement.Attributes["paymentTermNumber"].Value = contactAccount.Betalingsbetingelse.Nummer.ToString();

                    accountingElement.AppendChild(contactAccountElement);
                }

                accountingCollectionElement.AppendChild(accountingElement);
            }

            using (FileStream fileStream = new FileStream("Accountings.xml", FileMode.Create, FileAccess.Write, FileShare.None))
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
