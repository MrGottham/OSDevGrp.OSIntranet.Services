using System;
using System.Collections.Specialized;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommonLibrary.Repositories.Interface.Exceptions;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.FoodWaste;
using AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.FoodWaste
{
    /// <summary>
    /// Tests the configuration repository to the food waste domain.
    /// </summary>
    [TestFixture]
    public class ConfigurationRepositoryTests
    {
        /// <summary>
        /// Tests that the constructor initialize the configuration repository to the food waste domain.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeConfigurationRepository()
        {
            var fixture = new Fixture();
            
            var smtpServer = fixture.Create<string>();
            var smtpPort = fixture.Create<int>();
            var useSmtpAuthentication = fixture.Create<bool>();
            var useSmtpSecureConnection = fixture.Create<bool>();
            var smtpUserName = fixture.Create<string>();
            var smtpPassword = fixture.Create<string>();
            var fromMailAddress = fixture.Create<string>();
            var nameValueCollection = new NameValueCollection
            {
                {"SmtpServer", smtpServer},
                {"SmtpPort", Convert.ToString(smtpPort)},
                {"UseSmtpAuthentication", Convert.ToString(useSmtpAuthentication)},
                {"SmtpUserName", smtpUserName},
                {"SmtpPassword", ConfigurationRepository.EncryptValue(smtpPassword)},
                {"UseSmtpSecureConnection", Convert.ToString(useSmtpSecureConnection)},
                {"FromMailAddress", fromMailAddress}
            };

            var configurationRepository = new ConfigurationRepository(nameValueCollection);
            Assert.That(configurationRepository, Is.Not.Null);
            Assert.That(configurationRepository.SmtpServer, Is.Not.Null);
            Assert.That(configurationRepository.SmtpServer, Is.Not.Empty);
            Assert.That(configurationRepository.SmtpServer, Is.EqualTo(smtpServer));
            Assert.That(configurationRepository.SmtpPort, Is.EqualTo(smtpPort));
            Assert.That(configurationRepository.SmtpPort, Is.EqualTo(smtpPort));
            Assert.That(configurationRepository.UseSmtpAuthentication, Is.EqualTo(useSmtpAuthentication));
            Assert.That(configurationRepository.SmtpUserName, Is.Not.Null);
            Assert.That(configurationRepository.SmtpUserName, Is.Not.Empty);
            Assert.That(configurationRepository.SmtpUserName, Is.EqualTo(smtpUserName));
            Assert.That(configurationRepository.SmtpPassword, Is.Not.Null);
            Assert.That(configurationRepository.SmtpPassword, Is.Not.Empty);
            Assert.That(configurationRepository.SmtpPassword, Is.EqualTo(smtpPassword));
            Assert.That(configurationRepository.UseSmtpSecureConnection, Is.EqualTo(useSmtpSecureConnection));
            Assert.That(configurationRepository.FromMailAddress, Is.Not.Null);
            Assert.That(configurationRepository.FromMailAddress, Is.Not.Empty);
            Assert.That(configurationRepository.FromMailAddress, Is.EqualTo(fromMailAddress));
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the name value collection containing the configurations is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenNameValueCollectionIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ConfigurationRepository(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("nameValueCollection"));
        }

        /// <summary>
        /// Tests that the constructor throws an IntranetRepositoryException when a CommonRepositoryException Occurs.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsIntranetRepositoryExceptionWhenCommonRepositoryExceptionOccurs()
        {
            var nameValueCollection = new NameValueCollection();

            var exception = Assert.Throws<IntranetRepositoryException>(() => new ConfigurationRepository(nameValueCollection));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.TypeOf<CommonRepositoryException>());
        }

        /// <summary>
        /// Test EncryptValue and DecryptValue on the configuration repository to the food waste domain.
        /// </summary>
        [Test]
        public void TestThatEncryptValueAndDecryptValueOnConfigurationRepositoryEncryptsAndDescryptsValue()
        {
            var fixture = new Fixture();
            var password = fixture.Create<string>();

            var encryptedPassword = ConfigurationRepository.EncryptValue(password);
            Assert.That(encryptedPassword, Is.Not.Null);
            Assert.That(encryptedPassword, Is.Not.Empty);

            var decryptedPassword = ConfigurationRepository.DecryptValue(encryptedPassword);
            Assert.That(decryptedPassword, Is.Not.Null);
            Assert.That(decryptedPassword, Is.Not.Empty);
            Assert.That(decryptedPassword, Is.EqualTo(password));
        }
    }
}
