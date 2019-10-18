using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using OSDevGrp.OSIntranet.CommonLibrary.IoC;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;

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

        /// <summary>
        /// Tester, at AdressegruppeGetAll henter adressegrupper.
        /// </summary>
        [Test]
        public void TestAtAdressegruppeGetAllHenterAdressegrupper()
        {
            var adressegrupper = _adresseRepository.AdressegruppeGetAll();
            Assert.That(adressegrupper, Is.Not.Null);
            Assert.That(adressegrupper.Count(), Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at BetalingsbetingelseGetAll henter betalingsbetingelser.
        /// </summary>
        [Test]
        public void TestAtBetalingsbetingelseGetAllHenterBetalingsbetingelser()
        {
            var betalingsbetingelser = _adresseRepository.BetalingsbetingelseGetAll();
            Assert.That(betalingsbetingelser, Is.Not.Null);
            Assert.That(betalingsbetingelser.Count(), Is.GreaterThan(0));
        }

        [Test]
        [Ignore("Used for data migration")]
        public void Export()
        {
            IEnumerable<Postnummer> postalCodeCollection = new List<Postnummer>(0);

            XmlDocument postalCodeDocument = new XmlDocument();
            postalCodeDocument.AppendChild(postalCodeDocument.CreateXmlDeclaration("1.0", Encoding.UTF8.BodyName, null));

            XmlElement postalCodeCollectionElement = postalCodeDocument.CreateElement("PostalCodes");
            postalCodeDocument.AppendChild(postalCodeCollectionElement);

            foreach (Postnummer postalCode in postalCodeCollection)
            {
                XmlElement postalCodeElement = postalCodeDocument.CreateElement("PostalCode");

                postalCodeElement.Attributes.Append(postalCodeDocument.CreateAttribute("countryCode"));
                switch (postalCode.Landekode)
                {
                    case "DK":
                        postalCodeElement.Attributes["countryCode"].Value = postalCode.Landekode;
                        break;

                    case "FR":
                        postalCodeElement.Attributes["countryCode"].Value = "FO";
                        break;

                    default:
                        throw new NotSupportedException(postalCode.Landekode);
                }

                postalCodeElement.Attributes.Append(postalCodeDocument.CreateAttribute("code"));
                postalCodeElement.Attributes["code"].Value = postalCode.Postnr.Trim();

                postalCodeElement.Attributes.Append(postalCodeDocument.CreateAttribute("city"));
                postalCodeElement.Attributes["city"].Value = postalCode.By.Trim();

                postalCodeCollectionElement.AppendChild(postalCodeElement);
            }

            using (FileStream fileStream = new FileStream(@"C:\Users\DFDG_OSO\Desktop\PostalCodes.xml", FileMode.Create, FileAccess.Write, FileShare.None))
            {
                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Encoding = Encoding.UTF8
                };
                using (XmlWriter xmlWriter = XmlWriter.Create(fileStream, settings))
                {
                    postalCodeDocument.WriteTo(xmlWriter);

                    xmlWriter.Flush();
                    xmlWriter.Close();
                }
            }
        }
    }
}
