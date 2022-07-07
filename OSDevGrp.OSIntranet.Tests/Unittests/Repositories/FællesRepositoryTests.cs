using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using MySql.Data.MySqlClient;
using NUnit.Framework;
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
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis data provider til MySql er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisMySqlDataProviderErNull()
        {
            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => new FællesRepository(null));
            // ReSharper restore ObjectCreationAsStatement

            TestHelper.AssertArgumentNullExceptionIsValid(result, "mySqlDataProvider");
        }

        /// <summary>
        /// Tester, at SystemGetAll henter systemer.
        /// </summary>
        [Test]
        public void TestAtSystemGetAllHenterSystemer()
        {
            IEnumerable<SystemProxy> systemProxyCollection = _fixture.CreateMany<SystemProxy>(_random.Next(5, 10)).ToList();
            IMySqlDataProvider mySqlDataProvider = CreateMySqlDataProvider(systemProxyCollection);

            IFællesRepository sut = new FællesRepository(mySqlDataProvider);
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

            IFællesRepository sut = new FællesRepository(mySqlDataProvider);
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

            IFællesRepository sut = new FællesRepository(mySqlDataProvider);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.SystemGetAll());

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, exception, ExceptionMessage.RepositoryError, "SystemGetAll", exception.Message);
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
    }
}