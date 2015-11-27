using NUnit.Framework;
using OSDevGrp.OSIntranet.CommonLibrary.IoC;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.Tests.Integrationstests.Repositories.FoodWaste
{
    /// <summary>
    /// Integration tests for the configuration repository to the food waste domain.
    /// </summary>
    [TestFixture]
    [Category("Integrationstest")]
    public class ConfigurationRepositoryTests
    {
        #region Private variables

        private IConfigurationRepository _configurationRepository;

        #endregion

        /// <summary>
        /// Opsætning af tests.
        /// </summary>
        [TestFixtureSetUp]
        public void TestSetUp()
        {
            var container = ContainerFactory.Create();
            _configurationRepository = container.Resolve<IConfigurationRepository>();
        }

        /// <summary>
        /// Tests that SmtpServer has a value.
        /// </summary>
        [Test]
        public void TestThatSmtpServerHasValue()
        {
            var result = _configurationRepository.SmtpServer;
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }

        /// <summary>
        /// Tests that SmtpUserName has a value.
        /// </summary>
        [Test]
        public void TestThatSmtpUserNameHasValue()
        {
            var result = _configurationRepository.SmtpUserName;
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }

        /// <summary>
        /// Tests that SmtpPassword has a value.
        /// </summary>
        [Test]
        public void TestThatSmtpPasswordHasValue()
        {
            var result = _configurationRepository.SmtpPassword;
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }

        /// <summary>
        /// Tests that FromMailAddress has a value.
        /// </summary>
        [Test]
        public void TestThatFromMailAddressHasValue()
        {
            var result = _configurationRepository.FromMailAddress;
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }
    }
}
