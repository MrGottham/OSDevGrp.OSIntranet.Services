using System;
using System.Globalization;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommandHandlers.Dispatchers;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.CommandHandlers.Dispatchers
{
    /// <summary>
    /// Test the dispatcher which can dispatch the welcome letter to a household member.
    /// </summary>
    [TestFixture]
    public class WelcomeLetterDispatcherTests
    {
        /// <summary>
        /// Tests that the constructor initialize the dispatcher which can dispatch the welcome letter to a household member.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeWelcomeLetterDispatcher()
        {
            var communicationRepositoryMock = MockRepository.GenerateMock<ICommunicationRepository>();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var staticTextFieldMergeMock = MockRepository.GenerateMock<IStaticTextFieldMerge>();

            var welcomeLetterDispatcher = new WelcomeLetterDispatcher(communicationRepositoryMock, systemDataRepositoryMock, staticTextFieldMergeMock);
            Assert.That(welcomeLetterDispatcher, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the repository used for communication with internal and external stakeholders in the food waste domain is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenCommunicationRepositoryIsNull()
        {
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var staticTextFieldMergeMock = MockRepository.GenerateMock<IStaticTextFieldMerge>();

            var exception = Assert.Throws<ArgumentNullException>(() => new WelcomeLetterDispatcher(null, systemDataRepositoryMock, staticTextFieldMergeMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("communicationRepository"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the repository which can access system data for the food waste domain is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenSystemDataRepositoryIsNull()
        {
            var communicationRepositoryMock = MockRepository.GenerateMock<ICommunicationRepository>();
            var staticTextFieldMergeMock = MockRepository.GenerateMock<IStaticTextFieldMerge>();

            var exception = Assert.Throws<ArgumentNullException>(() => new WelcomeLetterDispatcher(communicationRepositoryMock, null, staticTextFieldMergeMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("systemDataRepository"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the functionality which can merge fields in a static text is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenStaticTextFieldMergeIsNull()
        {
            var communicationRepositoryMock = MockRepository.GenerateMock<ICommunicationRepository>();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();

            var exception = Assert.Throws<ArgumentNullException>(() => new WelcomeLetterDispatcher(communicationRepositoryMock, systemDataRepositoryMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("staticTextFieldMerge"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Dispatch calls StaticTextGetByStaticTextType on the repository which can access system data for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatDispatchCallsStaticTextGetByStaticTextTypeOnSystemDataRepository()
        {
            var communicationRepositoryMock = MockRepository.GenerateMock<ICommunicationRepository>();
            var staticTextFieldMergeMock = MockRepository.GenerateMock<IStaticTextFieldMerge>();

            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.StaticTextGetByStaticTextType(Arg<StaticTextType>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildStaticTextMock())
                .Repeat.Any();

            var welcomeLetterDispatcher = new WelcomeLetterDispatcher(communicationRepositoryMock, systemDataRepositoryMock, staticTextFieldMergeMock);
            Assert.That(welcomeLetterDispatcher, Is.Not.Null);

            welcomeLetterDispatcher.Dispatch(DomainObjectMockBuilder.BuildStakeholderMock(), DomainObjectMockBuilder.BuildHouseholdMemberMock(), DomainObjectMockBuilder.BuildTranslationInfoMock());

            systemDataRepositoryMock.AssertWasCalled(m => m.StaticTextGetByStaticTextType(Arg<StaticTextType>.Is.Equal(StaticTextType.WelcomeLetter)));
        }

        /// <summary>
        /// Tests that Dispatch calls Translate on the static text for the welcome letter.
        /// </summary>
        [Test]
        public void TestThatDispatchCallsTranslateOnStaticTextForWelcomeLetter()
        {
            var communicationRepositoryMock = MockRepository.GenerateMock<ICommunicationRepository>();
            var staticTextFieldMergeMock = MockRepository.GenerateMock<IStaticTextFieldMerge>();

            var welcomeLetterStaticTextMock = DomainObjectMockBuilder.BuildStaticTextMock();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.StaticTextGetByStaticTextType(Arg<StaticTextType>.Is.Anything))
                .Return(welcomeLetterStaticTextMock)
                .Repeat.Any();

            var translationInfoMock = DomainObjectMockBuilder.BuildTranslationInfoMock();
            
            var welcomeLetterDispatcher = new WelcomeLetterDispatcher(communicationRepositoryMock, systemDataRepositoryMock, staticTextFieldMergeMock);
            Assert.That(welcomeLetterDispatcher, Is.Not.Null);

            welcomeLetterDispatcher.Dispatch(DomainObjectMockBuilder.BuildStakeholderMock(), DomainObjectMockBuilder.BuildHouseholdMemberMock(), translationInfoMock);

            welcomeLetterStaticTextMock.AssertWasCalled(m => m.Translate(Arg<CultureInfo>.Is.Equal(translationInfoMock.CultureInfo)));
        }

        /// <summary>
        /// Tests that Dispatch calls AddMergeFields for the household member on the functionality which can merge fields in a static text.
        /// </summary>
        [Test]
        public void TestThatDispatchCallsAddMergeFieldsForHouseholdMemberOnStaticTextFieldMerge()
        {
            var communicationRepositoryMock = MockRepository.GenerateMock<ICommunicationRepository>();
            var staticTextFieldMergeMock = MockRepository.GenerateMock<IStaticTextFieldMerge>();

            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.StaticTextGetByStaticTextType(Arg<StaticTextType>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildStaticTextMock())
                .Repeat.Any();

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            var translationInfoMock = DomainObjectMockBuilder.BuildTranslationInfoMock();

            var welcomeLetterDispatcher = new WelcomeLetterDispatcher(communicationRepositoryMock, systemDataRepositoryMock, staticTextFieldMergeMock);
            Assert.That(welcomeLetterDispatcher, Is.Not.Null);

            welcomeLetterDispatcher.Dispatch(DomainObjectMockBuilder.BuildStakeholderMock(), householdMemberMock, translationInfoMock);

            staticTextFieldMergeMock.AssertWasCalled(m => m.AddMergeFields(Arg<IHouseholdMember>.Is.Equal(householdMemberMock), Arg<ITranslationInfo>.Is.Equal(translationInfoMock)));
        }

        /// <summary>
        /// Tests that Dispatch calls Merge on the functionality which can merge fields in a static text.
        /// </summary>
        [Test]
        public void TestThatDispatchCallsMergeOnStaticTextFieldMerge()
        {
            var communicationRepositoryMock = MockRepository.GenerateMock<ICommunicationRepository>();
            var staticTextFieldMergeMock = MockRepository.GenerateMock<IStaticTextFieldMerge>();

            var welcomeLetterStaticTextMock = DomainObjectMockBuilder.BuildStaticTextMock();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.StaticTextGetByStaticTextType(Arg<StaticTextType>.Is.Anything))
                .Return(welcomeLetterStaticTextMock)
                .Repeat.Any();

            var translationInfoMock = DomainObjectMockBuilder.BuildTranslationInfoMock();

            var welcomeLetterDispatcher = new WelcomeLetterDispatcher(communicationRepositoryMock, systemDataRepositoryMock, staticTextFieldMergeMock);
            Assert.That(welcomeLetterDispatcher, Is.Not.Null);

            welcomeLetterDispatcher.Dispatch(DomainObjectMockBuilder.BuildStakeholderMock(), DomainObjectMockBuilder.BuildHouseholdMemberMock(), translationInfoMock);

            staticTextFieldMergeMock.AssertWasCalled(m => m.Merge(Arg<IStaticText>.Is.Equal(welcomeLetterStaticTextMock), Arg<ITranslationInfo>.Is.Equal(translationInfoMock)));
        }

        /// <summary>
        /// Tests that Dispatch calls SendMail on the repository used for communication with internal and external stakeholders in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatDispatchCallsSendMailOnCommunicationRepository()
        {
            var communicationRepositoryMock = MockRepository.GenerateMock<ICommunicationRepository>();
            var staticTextFieldMergeMock = MockRepository.GenerateMock<IStaticTextFieldMerge>();

            var welcomeLetterStaticTextMock = DomainObjectMockBuilder.BuildStaticTextMock();
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            systemDataRepositoryMock.Stub(m => m.StaticTextGetByStaticTextType(Arg<StaticTextType>.Is.Anything))
                .Return(welcomeLetterStaticTextMock)
                .Repeat.Any();

            var stakeholderMock = DomainObjectMockBuilder.BuildStakeholderMock();

            var welcomeLetterDispatcher = new WelcomeLetterDispatcher(communicationRepositoryMock, systemDataRepositoryMock, staticTextFieldMergeMock);
            Assert.That(welcomeLetterDispatcher, Is.Not.Null);

            welcomeLetterDispatcher.Dispatch(stakeholderMock, DomainObjectMockBuilder.BuildHouseholdMemberMock(), DomainObjectMockBuilder.BuildTranslationInfoMock());

            communicationRepositoryMock.AssertWasCalled(m => m.SendMail(Arg<string>.Is.Equal(stakeholderMock.MailAddress), Arg<string>.Is.Equal(welcomeLetterStaticTextMock.SubjectTranslation.Value), Arg<string>.Is.Equal(welcomeLetterStaticTextMock.BodyTranslation.Value)));
        }
    }
}
