using NUnit.Framework;
using OSDevGrp.OSIntranet.CommonLibrary.IoC;
using OSDevGrp.OSIntranet.Repositories.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.Tests.Integrationstests.Repositories.FoodWaste
{
    /// <summary>
    /// Integration tests for the configuration repository to the food waste domain.
    /// </summary>
    [TestFixture]
    [Category("Integrationstest")]
    public class CommunicationRepositoryTests
    {
        #region Private variables

        private ICommunicationRepository _communicationRepository;

        #endregion

        /// <summary>
        /// Opsætning af tests.
        /// </summary>
        [SetUp]
        public void TestSetUp()
        {
            var container = ContainerFactory.Create();
            _communicationRepository = new CommunicationRepository(container.Resolve<IConfigurationRepository>());
        }

        /// <summary>
        /// Tests that SendMail sends a mail message.
        /// </summary>
        [Test]
        [Ignore("We don't want to send mails anytime we tests.")]
        [TestCase("mrgottham@gmail.com")]
        public void TestThatSendMailSendsMailMessage(string toMailAddress)
        {
            _communicationRepository.SendMail(toMailAddress, "Test", "<html><h1>Test from the Food Waste Project.</h1></html>");
        }
    }
}
