using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using AutoFixture;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Fælles;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf.ChannelFactory;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Queries;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Services;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Views;
using OSDevGrp.OSIntranet.Domain.Interfaces.Fælles;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories;
using OSDevGrp.OSIntranet.Repositories.DataProxies.Fælles;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Resources;
using OSDevGrp.OSIntranet.Tests.Unittests.Repositories.DataProxies;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories
{
    /// <summary>
    /// Tester repository til fælles elementer i domænet.
    /// </summary>
    [TestFixture]
    public class FællesRepositoryTests
    {
        #region Private variables

        private Fixture _fixture;
        private Random _random;

        #endregion

        /// <summary>
        /// Sætter hver test op.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis ChannelFactory er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisChannelFactoryErNull()
        {
            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => new FællesRepository(null, CreateMySqlDataProvider(), CreateDomainObjectBuilder()));
            // ReSharper restore ObjectCreationAsStatement

            TestHelper.AssertArgumentNullExceptionIsValid(result, "channelFactory");
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis data provider til MySql er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisMySqlDataProviderErNull()
        {
            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => new FællesRepository(CreateChannelFactory(), null, CreateDomainObjectBuilder()));
            // ReSharper restore ObjectCreationAsStatement

            TestHelper.AssertArgumentNullExceptionIsValid(result, "mySqlDataProvider");
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis domæneobjekt bygger er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisDomainObjectBuilderErNull()
        {
            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => new FællesRepository(CreateChannelFactory(), CreateMySqlDataProvider(), null));
            // ReSharper restore ObjectCreationAsStatement

            TestHelper.AssertArgumentNullExceptionIsValid(result, "domainObjectBuilder");
        }

        /// <summary>
        /// Tester, at BrevhovedGetAll henter alle brevhoveder.
        /// </summary>
        [Test]
        public void TestAtBrevhovedGetAllHenterAlleBrevhoveder()
        {
            IEnumerable<BrevhovedView> letterHeadViewCollection = _fixture.CreateMany<BrevhovedView>(_random.Next(3, 7)).ToList();
            IChannelFactory channelFactory = CreateChannelFactory(letterHeadViewCollection);

            IEnumerable<Brevhoved> letterHeadCollection = _fixture.CreateMany<Brevhoved>(letterHeadViewCollection.Count()).ToList();
            IDomainObjectBuilder domainObjectBuilder = CreateDomainObjectBuilder(letterHeadCollection);

            IFællesRepository sut = new FællesRepository(channelFactory, CreateMySqlDataProvider(), domainObjectBuilder);
            Assert.That(sut, Is.Not.Null);

            IEnumerable<Brevhoved> letterheads = sut.BrevhovedGetAll();
            // ReSharper disable PossibleMultipleEnumeration
            Assert.That(letterheads, Is.Not.Null);
            Assert.That(letterheads, Is.Not.Empty);
            Assert.That(letterheads.Count(), Is.EqualTo(letterHeadCollection.Count()));
            Assert.That(letterheads, Is.EqualTo(letterHeadCollection));
            // ReSharper restore PossibleMultipleEnumeration

            channelFactory.AssertWasCalled(m => m.CreateChannel<IFællesRepositoryService>(Arg<string>.Is.Anything), opt => opt.Repeat.Once());
            domainObjectBuilder.AssertWasCalled(m => m.BuildMany<BrevhovedView, Brevhoved>(Arg<IEnumerable<BrevhovedView>>.Is.Equal(letterHeadViewCollection)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tester, at BrevhovedGetAll kaster en IntranetRepositoryException ved IntranetRepositoryException.
        /// </summary>
        [Test]
        public void TestAtBrevhovedGetAllKasterIntranetRepositoryExceptionVedIntranetRepositoryException()
        {
            IntranetRepositoryException intranetRepositoryException = _fixture.Create<IntranetRepositoryException>();
            IChannelFactory channelFactory = CreateChannelFactory(exception: intranetRepositoryException);

            IFællesRepository sut = new FællesRepository(channelFactory, CreateMySqlDataProvider(), CreateDomainObjectBuilder());
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.BrevhovedGetAll());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(intranetRepositoryException));
        }

        /// <summary>
        /// Tester, at BrevhovedGetAll kaster en IntranetRepositoryException ved FaultException.
        /// </summary>
        [Test]
        public void TestAtBrevhovedGetAllKasterIntranetRepositoryExceptionVedFaultException()
        {
            FaultException faultException = new FaultException(_fixture.Create<FaultReason>());
            IChannelFactory channelFactory = CreateChannelFactory(exception: faultException);

            IFællesRepository sut = new FællesRepository(channelFactory, CreateMySqlDataProvider(), CreateDomainObjectBuilder());
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.BrevhovedGetAll());
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Message, Is.Not.Null);
            Assert.That(result.Message, Is.Not.Empty);
            Assert.That(result.Message, Is.EqualTo(faultException.Message));
            Assert.That(result.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at BrevhovedGetAll kaster en IntranetRepositoryException ved Exception.
        /// </summary>
        [Test]
        public void TestAtBrevhovedGetAllKasterIntranetRepositoryExceptionVedException()
        {
            Exception exception = _fixture.Create<Exception>();
            IChannelFactory channelFactory = CreateChannelFactory(exception: exception);

            IFællesRepository sut = new FællesRepository(channelFactory, CreateMySqlDataProvider(), CreateDomainObjectBuilder());
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.BrevhovedGetAll());

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, exception, ExceptionMessage.RepositoryError, "BrevhovedGetAll", exception.Message);
        }

        /// <summary>
        /// Tester, at SystemGetAll henter systemer.
        /// </summary>
        [Test]
        public void TestAtSystemGetAllHenterSystemer()
        {
            IEnumerable<SystemProxy> systemProxyCollection = _fixture.CreateMany<SystemProxy>(_random.Next(5, 10)).ToList();
            IMySqlDataProvider mySqlDataProvider = CreateMySqlDataProvider(systemProxyCollection);

            IFællesRepository sut = new FællesRepository(CreateChannelFactory(), mySqlDataProvider, CreateDomainObjectBuilder());
            Assert.That(sut, Is.Not.Null);

            IEnumerable<ISystem> result = sut.SystemGetAll();
            // ReSharper disable PossibleMultipleEnumeration
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Count(), Is.EqualTo(systemProxyCollection.Count()));
            Assert.That(result, Is.EqualTo(systemProxyCollection));
            // ReSharper restore PossibleMultipleEnumeration

            IDbCommandTestExecutor expectedCommandTester = new DbCommandTestBuilder("SELECT SystemNo,Title,Properties FROM Systems ORDER BY SystemNo").Build();
            mySqlDataProvider.AssertWasCalled(m => m.GetCollection<SystemProxy>(Arg<MySqlCommand>.Matches(cmd => expectedCommandTester.Run(cmd))), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tester, at SystemGetAll kaster en IntranetRepositoryException ved IntranetRepositoryException.
        /// </summary>
        [Test]
        public void TestAtSystemGetAllKasterIntranetRepositoryExceptionVedIntranetRepositoryException()
        {
            IntranetRepositoryException intranetRepositoryException = _fixture.Create<IntranetRepositoryException>();
            IMySqlDataProvider mySqlDataProvider = CreateMySqlDataProvider(exception: intranetRepositoryException);

            IFællesRepository sut = new FællesRepository(CreateChannelFactory(), mySqlDataProvider, CreateDomainObjectBuilder());
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.SystemGetAll());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(intranetRepositoryException));
        }

        /// <summary>
        /// Tester, at SystemGetAll kaster en IntranetRepositoryException ved Exception.
        /// </summary>
        [Test]
        public void TestAtSystemGetAllKasterIntranetRepositoryExceptionVedException()
        {
            Exception exception = _fixture.Create<Exception>();
            IMySqlDataProvider mySqlDataProvider = CreateMySqlDataProvider(exception: exception);

            IFællesRepository sut = new FællesRepository(CreateChannelFactory(), mySqlDataProvider, CreateDomainObjectBuilder());
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.SystemGetAll());

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, exception, ExceptionMessage.RepositoryError, "SystemGetAll", exception.Message);
        }

        /// <summary>
        /// Creates a mockup of the channel factory.
        /// </summary>
        /// <returns>Mockup of the channel factory.</returns>
        private IChannelFactory CreateChannelFactory(IEnumerable<BrevhovedView> letterHeadViewCollection = null, Exception exception = null)
        {
            MockRepository mocker = new MockRepository();

            IFællesRepositoryService serviceMock = mocker.DynamicMultiMock<IFællesRepositoryService>(typeof(ICommunicationObject));
            if (exception == null)
            {
                serviceMock.Stub(m => m.BrevhovedGetAll(Arg<BrevhovedGetAllQuery>.Is.Anything))
                    .Return(letterHeadViewCollection ?? _fixture.CreateMany<BrevhovedView>(_random.Next(3, 7)).ToList())
                    .Repeat.Any();
            }
            else
            {
                serviceMock.Stub(m => m.BrevhovedGetAll(Arg<BrevhovedGetAllQuery>.Is.Anything))
                    .Throw(exception)
                    .Repeat.Any();
            }
            // ReSharper disable SuspiciousTypeConversion.Global
            serviceMock.Stub(m => ((ICommunicationObject) m).State)
                .Return(CommunicationState.Closed)
                .Repeat.Any();
            // ReSharper restore SuspiciousTypeConversion.Global

            mocker.ReplayAll();

            IChannelFactory channelFactoryMock = MockRepository.GenerateMock<IChannelFactory>();
            channelFactoryMock.Stub(m => m.CreateChannel<IFællesRepositoryService>(Arg<string>.Is.Anything))
                .Return(serviceMock)
                .Repeat.Any();

            return channelFactoryMock;
        }

        /// <summary>
        /// Creates a mockup for the data provider which uses MySQL.
        /// </summary>
        /// <returns>Mockup for the data provider which uses MySQL.</returns>
        private IMySqlDataProvider CreateMySqlDataProvider(IEnumerable<SystemProxy> systemProxyCollection = null, Exception exception = null)
        {
            IMySqlDataProvider mySqlDataProviderMock = MockRepository.GenerateMock<IMySqlDataProvider>();
            if (exception == null)
            {
                mySqlDataProviderMock.Stub(m => m.GetCollection<SystemProxy>(Arg<MySqlCommand>.Is.Anything))
                    .Return(systemProxyCollection ?? _fixture.CreateMany<SystemProxy>(_random.Next(5, 10)).ToList())
                    .Repeat.Any();
            }
            else
            {
                mySqlDataProviderMock.Stub(m => m.GetCollection<SystemProxy>(Arg<MySqlCommand>.Is.Anything))
                    .Throw(exception)
                    .Repeat.Any();
            }
            return mySqlDataProviderMock;
        }

        /// <summary>
        /// Creates a mockup of the domain object builder.
        /// </summary>
        /// <returns>Mockup of the domain object builder.</returns>
        private IDomainObjectBuilder CreateDomainObjectBuilder(IEnumerable<Brevhoved> letterHeadCollection = null)
        {
            IDomainObjectBuilder domainObjectBuilderMock = MockRepository.GenerateMock<IDomainObjectBuilder>();
            domainObjectBuilderMock.Stub(m => m.BuildMany<BrevhovedView, Brevhoved>(Arg<IEnumerable<BrevhovedView>>.Is.Anything))
                .Return(letterHeadCollection ?? _fixture.CreateMany<Brevhoved>(_random.Next(3, 7)).ToList())
                .Repeat.Any();
            return domainObjectBuilderMock;
        }
    }
}
