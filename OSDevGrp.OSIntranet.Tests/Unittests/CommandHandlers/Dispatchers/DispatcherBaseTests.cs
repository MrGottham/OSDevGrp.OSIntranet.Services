using System;
using System.Globalization;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommandHandlers.Dispatchers;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.CommandHandlers.Dispatchers
{
    /// <summary>
    /// Tests the basic functionaly for a dispatcher which can dispatch data for a given domain object in the food waste domain.
    /// </summary>
    [TestFixture]
    public class DispatcherBaseTests
    {
        /// <summary>
        /// Private class for testing the basic functionaly for a dispatcher which can dispatch data for a given domain object in the food waste domain.
        /// </summary>
        /// <typeparam name="TDomainObject">Type of the domain object which provides data to dispatch.</typeparam>
        private class MyDispatcher<TDomainObject> : DispatcherBase<TDomainObject> where TDomainObject : IIdentifiable
        {
            #region Constructor

            /// <summary>
            /// Creates an instance of the private class for testing the basic functionaly for a dispatcher which can dispatch data for a given domain object in the food waste domain.
            /// </summary>
            /// <param name="communicationRepository">Implementation of a repository used for communication with internal and external stakeholders in the food waste domain.</param>
            public MyDispatcher(ICommunicationRepository communicationRepository) 
                : base(communicationRepository)
            {
            }

            #endregion

            #region Properties

            /// <summary>
            /// Repository used for communication with internal and external stakeholders in the food waste domain.
            /// </summary>
            public new ICommunicationRepository CommunicationRepository
            {
                get { return base.CommunicationRepository; }
            }

            /// <summary>
            /// Gets whether HandleCommunication has been called.
            /// </summary>
            public bool HandleCommunicationHasBeenCalled { get; private set; }

            #endregion

            #region Methods

            /// <summary>
            /// Handles the communication so data will be dispatched to the stakeholder.
            /// </summary>
            /// <param name="stakeholder">Stakeholder which data should be dispatched to.</param>
            /// <param name="domainObject">Domain object which provides data to dispatch.</param>
            /// <param name="translationInfo">Translation informations used to translate the data to dispatch.</param>
            protected override void HandleCommunication(IStakeholder stakeholder, TDomainObject domainObject, ITranslationInfo translationInfo)
            {
                Assert.That(stakeholder, Is.Not.Null);
                Assert.That(domainObject, Is.Not.Null);
                Assert.That(translationInfo, Is.Not.Null);

                HandleCommunicationHasBeenCalled = true;
            }

            #endregion
        }

        /// <summary>
        /// Tests that the constructor initialize the basic functionaly for a dispatcher which can dispatch data for a given domain object in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeDispatcherBase()
        {
            var communicationRepository = MockRepository.GenerateMock<ICommunicationRepository>();

            var dispatcherBase = new MyDispatcher<IIdentifiable>(communicationRepository);
            Assert.That(dispatcherBase, Is.Not.Null);
            Assert.That(dispatcherBase.CommunicationRepository, Is.Not.Null);
            Assert.That(dispatcherBase.CommunicationRepository, Is.EqualTo(communicationRepository));
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the repository used for communication with internal and external stakeholders in the food waste domain is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenCommunicationRepositoryIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new MyDispatcher<IIdentifiable>(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("communicationRepository"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Dispatch throws an ArgumentNullException when the stakeholder which data should be dispatched to is null.
        /// </summary>
        [Test]
        public void TestThatDispatchThrowsArgumentNullExceptionWhenStakeholderIsNull()
        {
            var communicationRepository = MockRepository.GenerateMock<ICommunicationRepository>();

            var dispatcherBase = new MyDispatcher<IIdentifiable>(communicationRepository);
            Assert.That(dispatcherBase, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => dispatcherBase.Dispatch(null, MockRepository.GenerateMock<IStakeholder>(), DomainObjectMockBuilder.BuildTranslationInfoMock()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("stakeholder"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Dispatch throws an ArgumentNullException when the domain object which provides data to dispatch is null.
        /// </summary>
        [Test]
        public void TestThatDispatchThrowsArgumentNullExceptionWhenDomainObjectIsNull()
        {
            var communicationRepository = MockRepository.GenerateMock<ICommunicationRepository>();

            var dispatcherBase = new MyDispatcher<IIdentifiable>(communicationRepository);
            Assert.That(dispatcherBase, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => dispatcherBase.Dispatch(DomainObjectMockBuilder.BuildStakeholderMock(), null, DomainObjectMockBuilder.BuildTranslationInfoMock()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("domainObject"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Dispatch throws an ArgumentNullException when the translation informations used to translate the data to dispatch is null.
        /// </summary>
        [Test]
        public void TestThatDispatchThrowsArgumentNullExceptionWhenTranslationInfoIsNull()
        {
            var communicationRepository = MockRepository.GenerateMock<ICommunicationRepository>();

            var dispatcherBase = new MyDispatcher<IIdentifiable>(communicationRepository);
            Assert.That(dispatcherBase, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => dispatcherBase.Dispatch(DomainObjectMockBuilder.BuildStakeholderMock(), MockRepository.GenerateMock<IIdentifiable>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("translationInfo"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Dispatch calls Translate when the domain object which provides data to dispatch is translatable.
        /// </summary>
        [Test]
        public void TestThatDispatchCallsTranslateWhenDomainObjectIsTranslatable()
        {
            var communicationRepository = MockRepository.GenerateMock<ICommunicationRepository>();
            
            var domainObjectMock = MockRepository.GenerateMock<ITranslatable>();
            var translationInfoMock = DomainObjectMockBuilder.BuildTranslationInfoMock();

            var dispatcherBase = new MyDispatcher<ITranslatable>(communicationRepository);
            Assert.That(dispatcherBase, Is.Not.Null);

            dispatcherBase.Dispatch(DomainObjectMockBuilder.BuildStakeholderMock(), domainObjectMock, translationInfoMock);

            domainObjectMock.AssertWasCalled(m => m.Translate(Arg<CultureInfo>.Is.Equal(translationInfoMock.CultureInfo)));
        }

        /// <summary>
        /// Tests that Dispatch calls HandleCommunication.
        /// </summary>
        [Test]
        public void TestThatDispatchCallsHandleCommunication()
        {
            var communicationRepository = MockRepository.GenerateMock<ICommunicationRepository>();

            var dispatcherBase = new MyDispatcher<IIdentifiable>(communicationRepository);
            Assert.That(dispatcherBase, Is.Not.Null);
            Assert.That(dispatcherBase.HandleCommunicationHasBeenCalled, Is.False);

            dispatcherBase.Dispatch(DomainObjectMockBuilder.BuildStakeholderMock(), MockRepository.GenerateMock<IIdentifiable>(), DomainObjectMockBuilder.BuildTranslationInfoMock());
            Assert.That(dispatcherBase.HandleCommunicationHasBeenCalled, Is.True);
        }
    }
}
