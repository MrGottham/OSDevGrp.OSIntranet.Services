using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using OSDevGrp.OSIntranet.CommonLibrary.IoC;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Integrationstests.Repositories
{
    /// <summary>
    /// Integrationstester repository for adressekartotek.
    /// </summary>
    [TestFixture]
    [Category("Integrationstest")]
    public class AdresseRepositoryTests
    {
        #region Private variables

        private IAdresseRepository _adresseRepository;

        #endregion

        /// <summary>
        /// Opsætning af tests.
        /// </summary>
        [SetUp]
        public void TestSetUp()
        {
            var container = ContainerFactory.Create();
            _adresseRepository = container.Resolve<IAdresseRepository>();
        }

        /// <summary>
        /// Tester, at AdresseGetAll henter adresser.
        /// </summary>
        [Test]
        public void TestAtAdresseGetAllHenterAdresser()
        {
            var adresser = _adresseRepository.AdresseGetAll();
            Assert.That(adresser, Is.Not.Null);
            Assert.That(adresser.Count(), Is.GreaterThan(0));
        }

        [Test] 
        [Ignore("Used for data migration")]
        public void Export()
        {
            AdresseBase[] addressCollection = _adresseRepository.AdresseGetAll().ToArray();

            XmlDocument addressDocument = new XmlDocument();
            addressDocument.AppendChild(addressDocument.CreateXmlDeclaration("1.0", Encoding.UTF8.BodyName, null));

            XmlElement addressCollectionElement = addressDocument.CreateElement("Addresses");
            addressDocument.AppendChild(addressCollectionElement);

            foreach (Firma company in addressCollection.OfType<Firma>().OrderBy(m => m.Nummer))
            {
                XmlElement companyElement = addressDocument.CreateElement("Company");

                AddAddressBase(company, addressDocument, companyElement);

                if (string.IsNullOrWhiteSpace(company.Telefon1) == false)
                {
                    companyElement.Attributes.Append(addressDocument.CreateAttribute("primaryPhone"));
                    companyElement.Attributes["primaryPhone"].Value = company.Telefon1.Trim();
                }

                if (string.IsNullOrWhiteSpace(company.Telefon2) == false)
                {
                    companyElement.Attributes.Append(addressDocument.CreateAttribute("secondaryPhone"));
                    companyElement.Attributes["secondaryPhone"].Value = company.Telefon2.Trim();
                }

                if (string.IsNullOrWhiteSpace(company.Telefax) == false)
                {
                    companyElement.Attributes.Append(addressDocument.CreateAttribute("fax"));
                    companyElement.Attributes["fax"].Value = company.Telefax.Trim();
                }

                addressCollectionElement.AppendChild(companyElement);
            }

            foreach (Person person in addressCollection.OfType<Person>().OrderBy(m => m.Nummer))
            {
                XmlElement personElement = addressDocument.CreateElement("Person");

                AddAddressBase(person, addressDocument, personElement);

                if (string.IsNullOrWhiteSpace(person.Telefon) == false)
                {
                    personElement.Attributes.Append(addressDocument.CreateAttribute("homePhone"));
                    personElement.Attributes["homePhone"].Value = person.Telefon.Trim();
                }

                if (string.IsNullOrWhiteSpace(person.Mobil) == false)
                {
                    personElement.Attributes.Append(addressDocument.CreateAttribute("mobilePhone"));
                    personElement.Attributes["mobilePhone"].Value = person.Mobil.Trim();
                }

                if (person.Fødselsdato.HasValue)
                {
                    personElement.Attributes.Append(addressDocument.CreateAttribute("birthday"));
                    personElement.Attributes["birthday"].Value = person.Fødselsdato.Value.ToString("yyyy-MM-dd");
                }

                if (person.Firma != null)
                {
                    personElement.Attributes.Append(addressDocument.CreateAttribute("companyIdentifier"));
                    personElement.Attributes["companyIdentifier"].Value = person.Firma.Nummer.ToString();
                }

                addressCollectionElement.AppendChild(personElement);
            }

            using (FileStream fileStream = new FileStream(@"C:\Users\DFDG_OSO\Desktop\Addresses.xml", FileMode.Create, FileAccess.Write, FileShare.None))
            {
                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Encoding = Encoding.UTF8,
                    Indent = true
                };
                using (XmlWriter xmlWriter = XmlWriter.Create(fileStream, settings))
                {
                    addressDocument.WriteTo(xmlWriter);

                    xmlWriter.Flush();
                    xmlWriter.Close();
                }
            }
        }

        private void AddAddressBase(AdresseBase address, XmlDocument document, XmlElement element)
        {
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address));
            }
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }
            
            element.Attributes.Append(document.CreateAttribute("number"));
            element.Attributes["number"].Value = address.Nummer.ToString();

            element.Attributes.Append(document.CreateAttribute("name"));
            element.Attributes["name"].Value = address.Navn.Trim();

            if (string.IsNullOrWhiteSpace(address.Adresse1) == false)
            {
                element.Attributes.Append(document.CreateAttribute("addressLine1"));
                element.Attributes["addressLine1"].Value = address.Adresse1.Trim();
            }

            if (string.IsNullOrWhiteSpace(address.Adresse2) == false)
            {
                element.Attributes.Append(document.CreateAttribute("addressLine2"));
                element.Attributes["addressLine2"].Value = address.Adresse2.Trim();
            }

            if (string.IsNullOrWhiteSpace(address.PostnrBy) == false)
            {
                element.Attributes.Append(document.CreateAttribute("postalCodeAndCity"));
                element.Attributes["postalCodeAndCity"].Value = address.PostnrBy.Trim();
            }

            element.Attributes.Append(document.CreateAttribute("contactGroupIdentifier"));
            element.Attributes["contactGroupIdentifier"].Value = address.Adressegruppe.Nummer.ToString();

            if (string.IsNullOrWhiteSpace(address.Bekendtskab) == false)
            {
                element.Attributes.Append(document.CreateAttribute("acquaintance"));
                element.Attributes["acquaintance"].Value = address.Bekendtskab.Trim();
            }

            if (string.IsNullOrWhiteSpace(address.Mailadresse) == false)
            {
                element.Attributes.Append(document.CreateAttribute("mailAddress"));
                element.Attributes["mailAddress"].Value = address.Mailadresse.Trim();
            }

            if (string.IsNullOrWhiteSpace(address.Webadresse) == false)
            {
                element.Attributes.Append(document.CreateAttribute("homePage"));
                element.Attributes["homePage"].Value = address.Webadresse.Trim();
            }

            element.Attributes.Append(document.CreateAttribute("lendingLimit"));
            element.Attributes["lendingLimit"].Value = address.Udlånsfrist.ToString();

            element.Attributes.Append(document.CreateAttribute("paymentTermIdentifier"));
            element.Attributes["paymentTermIdentifier"].Value = address.Betalingsbetingelse.Nummer.ToString();
        }
    }
}
