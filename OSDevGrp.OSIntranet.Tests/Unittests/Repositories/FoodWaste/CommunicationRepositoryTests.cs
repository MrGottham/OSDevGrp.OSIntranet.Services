using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Repositories.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;
using AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.FoodWaste
{
    /// <summary>
    /// Tests the repository used for communication with internal and external stakeholders in the food waste domain.
    /// </summary>
    [TestFixture]
    public class CommunicationRepositoryTests
    {
        /// <summary>
        /// Tests that the constructor initialize the repository used for communication with internal and external stakeholders in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeCommunicationRepository()
        {
            var configurationRepositoryMock = MockRepository.GenerateMock<IConfigurationRepository>();

            var communicationRepository = new CommunicationRepository(configurationRepositoryMock);
            Assert.That(communicationRepository, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the configuration repository to the food waste domain is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenConfigurationRepositoryIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new CommunicationRepository(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("configurationRepository"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that SendMail throws an ArgumentNullException when the mail address for the receiver is invalid.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestThatSendMailThrowsArgumentNullExceptionWhenToMailAddressIsInvalid(string invalidValue)
        {
            var fixture = new Fixture();
            var configurationRepositoryMock = MockRepository.GenerateMock<IConfigurationRepository>();

            var communicationRepository = new CommunicationRepository(configurationRepositoryMock);
            Assert.That(communicationRepository, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => communicationRepository.SendMail(null, fixture.Create<string>(), fixture.Create<string>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("toMailAddress"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that SendMail throws an ArgumentNullException when the subject for the mail is invalid.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestThatSendMailThrowsArgumentNullExceptionWhenSubjectIsInvalid(string invalidValue)
        {
            var fixture = new Fixture();
            var configurationRepositoryMock = MockRepository.GenerateMock<IConfigurationRepository>();

            var communicationRepository = new CommunicationRepository(configurationRepositoryMock);
            Assert.That(communicationRepository, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => communicationRepository.SendMail(fixture.Create<string>(), invalidValue, fixture.Create<string>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("subject"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that SendMail throws an ArgumentNullException when the body for the mail is invalid.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestThatSendMailThrowsArgumentNullExceptionWhenBodyIsInvalid(string invalidValue)
        {
            var fixture = new Fixture();
            var configurationRepositoryMock = MockRepository.GenerateMock<IConfigurationRepository>();

            var communicationRepository = new CommunicationRepository(configurationRepositoryMock);
            Assert.That(communicationRepository, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => communicationRepository.SendMail(fixture.Create<string>(), fixture.Create<string>(), invalidValue));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("body"));
            Assert.That(exception.InnerException, Is.Null);
        }
    }
}
