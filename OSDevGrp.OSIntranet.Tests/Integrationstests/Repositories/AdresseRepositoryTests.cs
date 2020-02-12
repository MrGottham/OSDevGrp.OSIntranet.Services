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

        [Test]
        [Ignore("Used for data migration")]
        public void Export()
        {
            IEnumerable<Betalingsbetingelse> paymentTermCollection = new List<Betalingsbetingelse>(0);

            XmlDocument paymentTermDocument = new XmlDocument();
            paymentTermDocument.AppendChild(paymentTermDocument.CreateXmlDeclaration("1.0", Encoding.UTF8.BodyName, null));

            XmlElement paymentTermCollectionElement = paymentTermDocument.CreateElement("PaymentTerms");
            paymentTermDocument.AppendChild(paymentTermCollectionElement);

            foreach (Betalingsbetingelse paymentTerm in paymentTermCollection.OrderBy(m => m.Nummer))
            {
                XmlElement paymentTermElement = paymentTermDocument.CreateElement("PaymentTerm");

                paymentTermElement.Attributes.Append(paymentTermDocument.CreateAttribute("number"));
                paymentTermElement.Attributes["number"].Value = paymentTerm.Nummer.ToString();

                paymentTermElement.Attributes.Append(paymentTermDocument.CreateAttribute("name"));
                paymentTermElement.Attributes["name"].Value = paymentTerm.Navn.Trim();

                paymentTermCollectionElement.AppendChild(paymentTermElement);
            }

            using (FileStream fileStream = new FileStream(@"PaymentTerms.xml", FileMode.Create, FileAccess.Write, FileShare.None))
            {
                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Encoding = Encoding.UTF8
                };
                using (XmlWriter xmlWriter = XmlWriter.Create(fileStream, settings))
                {
                    paymentTermDocument.WriteTo(xmlWriter);

                    xmlWriter.Flush();
                    xmlWriter.Close();
                }
            }
        }
    }
}
