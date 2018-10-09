using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Kalender;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories;
using OSDevGrp.OSIntranet.Repositories.DataProxies.Kalender;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Resources;
using OSDevGrp.OSIntranet.Tests.Unittests.Repositories.DataProxies;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories
{
    /// <summary>
    /// Tester repository til kalenderaftaler under OSWEBDB.
    /// </summary>
    [TestFixture]
    public class KalenderRepositoryTests
    {
        #region Private variables

        private Fixture _fixture;
        private Random _random;
        private IMySqlDataProvider _dataProviderMock;

        #endregion

        /// <summary>
        /// Setup each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
            _dataProviderMock = MockRepository.GenerateMock<IMySqlDataProvider>();
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis data provideren til MySql er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisMySqlDataProviderErNull()
        {
            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => new KalenderRepository(null));
            // ReSharper restore ObjectCreationAsStatement

            TestHelper.AssertArgumentNullExceptionIsValid(result, "mySqlDataProvider");
        }

        /// <summary>
        /// Tester, at AftaleGetAllBySystem henter kalenderaftaler fra og med en given dato til et given system under OSWEBDB.
        /// </summary>
        [Test]
        public void TestAtAftaleGetAllBySystemHenterAftaler()
        {
            IEnumerable<AftaleProxy> appointmentProxyCollection = _fixture.Build<AftaleProxy>()
                .With(m => m.FraTidspunkt, DateTime.MinValue)
                .With(m => m.TilTidspunkt, DateTime.MaxValue)
                .CreateMany(_random.Next(15, 25))
                .ToList();

            IKalenderRepository sut = CreateSut(appointmentProxyCollection);
            Assert.That(sut, Is.Not.Null);

            int systemNo = _fixture.Create<int>();
            DateTime fromDate = DateTime.Now.AddDays(_random.Next(1, 30) * -1);
            IEnumerable<IAftale> result = sut.AftaleGetAllBySystem(systemNo, fromDate);
            // ReSharper disable PossibleMultipleEnumeration
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EqualTo(appointmentProxyCollection));
            // ReSharper restore PossibleMultipleEnumeration

            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT ca.SystemNo,ca.CalId,ca.Date,ca.FromTime,ca.ToTime,ca.Properties,ca.Subject,ca.Note,s.Title AS SystemTitle,s.Properties AS SystemProperties FROM Calapps AS ca FORCE INDEX(IX_Calapps_SystemNo_Date) INNER JOIN Systems AS s ON s.SystemNo=ca.SystemNo WHERE ca.SystemNo=@systemNo AND ca.Date>=@date ORDER BY ca.Date DESC,ca.FromTime DESC,ca.ToTime DESC,ca.CalId DESC")
                .AddSmallIntDataParameter("@systemNo", systemNo, 2)
                .AddDateParameter("@date", fromDate)
                .Build();
            _dataProviderMock.AssertWasCalled(m => m.GetCollection<AftaleProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tester, at AftaleGetAllBySystem kaster en IntranetRepositoryException ved IntranetRepositoryException.
        /// </summary>
        [Test]
        public void TestAtAftaleGetAllBySystemKasterIntranetRepositoryExceptionVedIntranetRepositoryException()
        {
            IntranetRepositoryException intranetRepositoryException = _fixture.Create<IntranetRepositoryException>();

            IKalenderRepository sut = CreateSut(exception: intranetRepositoryException);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.AftaleGetAllBySystem(_fixture.Create<int>(), DateTime.Now.AddDays(_random.Next(1, 30) * -1)));
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(intranetRepositoryException));
        }

        /// <summary>
        /// Tester, at AftaleGetAllBySystem kaster en IntranetRepositoryException ved Exception.
        /// </summary>
        [Test]
        public void TestAtAftaleGetAllBySystemKasterIntranetRepositoryExceptionVedException()
        {
            Exception exception = _fixture.Create<Exception>();

            IKalenderRepository sut = CreateSut(exception: exception);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.AftaleGetAllBySystem(_fixture.Create<int>(), DateTime.Now.AddDays(_random.Next(1, 30) * -1)));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, exception, ExceptionMessage.RepositoryError, "AftaleGetAllBySystem", exception.Message);
        }

        /// <summary>
        /// Tester, at AftaleGetBySystemAndId henter en given kalenderaftale fra et given system under OSWEBDB.
        /// </summary>
        [Test]
        public void TestAtAftaleGetBySystemAndIdHenterAftale()
        {
            AftaleProxy appointmentProxy = _fixture.Build<AftaleProxy>()
                .With(m => m.FraTidspunkt, DateTime.MinValue)
                .With(m => m.TilTidspunkt, DateTime.MaxValue)
                .Create();

            IKalenderRepository sut = CreateSut(appointmentProxy: appointmentProxy);
            Assert.That(sut, Is.Not.Null);

            int systemNo = _fixture.Create<int>();
            int calId = _fixture.Create<int>();
            IAftale result = sut.AftaleGetBySystemAndId(systemNo, calId);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(appointmentProxy));

            _dataProviderMock.AssertWasCalled(m => m.Get(Arg<AftaleProxy>.Matches(n => n.System != null && n.System.Nummer == systemNo && n.Id == calId)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tester, at AftaleGetBySystemAndId kaster en IntranetRepositoryException ved IntranetRepositoryException.
        /// </summary>
        [Test]
        public void TestAtAftaleGetBySystemAndIdKasterIntranetRepositoryExceptionVedIntranetRepositoryException()
        {
            IntranetRepositoryException intranetRepositoryException = _fixture.Create<IntranetRepositoryException>();

            IKalenderRepository sut = CreateSut(exception: intranetRepositoryException);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.AftaleGetBySystemAndId(_fixture.Create<int>(), _fixture.Create<int>()));
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(intranetRepositoryException));
        }

        /// <summary>
        /// Tester, at AftaleGetBySystemAndId kaster en IntranetRepositoryException ved Exception.
        /// </summary>
        [Test]
        public void TestAtAftaleGetBySystemAndIdKasterIntranetRepositoryExceptionVedException()
        {
            Exception exception = _fixture.Create<Exception>();

            IKalenderRepository sut = CreateSut(exception: exception);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.AftaleGetBySystemAndId(_fixture.Create<int>(), _fixture.Create<int>()));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, exception, ExceptionMessage.RepositoryError, "AftaleGetBySystemAndId", exception.Message);
        }

        /// <summary>
        /// Tester, at BrugerGetAllBySstem henter brugere til et given system under OSWEBDB.
        /// </summary>
        [Test]
        public void TestAtBrugerGetAllBySystemHenterBrugere()
        {
            IEnumerable<BrugerProxy> calenderUserProxyCollection = _fixture.CreateMany<BrugerProxy>(_random.Next(15, 25)).ToList();

            IKalenderRepository sut = CreateSut(calenderUserProxyCollection: calenderUserProxyCollection);
            Assert.That(sut, Is.Not.Null);

            int systemNo = _fixture.Create<int>();
            IEnumerable<IBruger> result = sut.BrugerGetAllBySystem(systemNo);
            // ReSharper disable PossibleMultipleEnumeration
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(calenderUserProxyCollection));
            // ReSharper restore PossibleMultipleEnumeration

            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT cu.SystemNo,cu.UserId,cu.UserName,cu.Name,cu.Initials,s.Title,s.Properties FROM Calusers AS cu INNER JOIN Systems AS s ON s.SystemNo=cu.SystemNo WHERE cu.SystemNo=@systemNo ORDER BY cu.Name,cu.Initials,cu.UserId")
                .AddSmallIntDataParameter("@systemNo", systemNo, 2)
                .Build();
            _dataProviderMock.AssertWasCalled(m => m.GetCollection<BrugerProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tester, at BrugerGetAllBySstem kaster en IntranetRepositoryException ved IntranetRepositoryException.
        /// </summary>
        [Test]
        public void TestAtBrugerGetAllBySystemKasterIntranetRepositoryExceptionVedIntranetRepositoryException()
        {
            IntranetRepositoryException intranetRepositoryException = _fixture.Create<IntranetRepositoryException>();

            IKalenderRepository sut = CreateSut(exception: intranetRepositoryException);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.BrugerGetAllBySystem(_fixture.Create<int>()));
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(intranetRepositoryException));
        }

        /// <summary>
        /// Tester, at BrugerGetAllBySstem kaster en IntranetRepositoryException ved Exception.
        /// </summary>
        [Test]
        public void TestAtBrugerGetAllBySystemKasterIntranetRepositoryExceptionVedException()
        {
            Exception exception = _fixture.Create<Exception>();

            IKalenderRepository sut = CreateSut(exception: exception);
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.BrugerGetAllBySystem(_fixture.Create<int>()));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, exception, ExceptionMessage.RepositoryError, "BrugerGetAllBySystem", exception.Message);
        }

        /// <summary>
        /// Creates an instance of the calender repository which can be used for unit testing.
        /// </summary>
        /// <returns>Instance of the calender repository which can be used for unit testing.</returns>
        private IKalenderRepository CreateSut(IEnumerable<AftaleProxy> appointmentProxyCollection = null, AftaleProxy appointmentProxy = null, IEnumerable<BrugerProxy> calenderUserProxyCollection = null, Exception exception = null)
        {
            if (exception == null)
            {
                _dataProviderMock.Stub(m => m.GetCollection<AftaleProxy>(Arg<MySqlCommand>.Is.Anything))
                    .Return(appointmentProxyCollection ?? _fixture.Build<AftaleProxy>().With(m => m.FraTidspunkt, DateTime.MinValue).With(m => m.TilTidspunkt, DateTime.MaxValue).CreateMany(_random.Next(15, 25)).ToList())
                    .Repeat.Any();
            }
            else
            {
                _dataProviderMock.Stub(m => m.GetCollection<AftaleProxy>(Arg<MySqlCommand>.Is.Anything))
                    .Throw(exception)
                    .Repeat.Any();
            }

            if (exception == null)
            {
                _dataProviderMock.Stub(m => m.Get(Arg<AftaleProxy>.Is.Anything))
                    .Return(appointmentProxy ?? _fixture.Build<AftaleProxy>().With(m => m.FraTidspunkt, DateTime.MinValue).With(m => m.TilTidspunkt, DateTime.MaxValue).Create())
                    .Repeat.Any();
            }
            else
            {
                _dataProviderMock.Stub(m => m.Get(Arg<AftaleProxy>.Is.Anything))
                    .Throw(exception)
                    .Repeat.Any();
            }

            if (exception == null)
            {
                _dataProviderMock.Stub(m => m.GetCollection<BrugerProxy>(Arg<MySqlCommand>.Is.Anything))
                    .Return(calenderUserProxyCollection ?? _fixture.CreateMany<BrugerProxy>(_random.Next(3, 10)).ToList())
                    .Repeat.Any();
            }
            else
            {
                _dataProviderMock.Stub(m => m.GetCollection<BrugerProxy>(Arg<MySqlCommand>.Is.Anything))
                    .Throw(exception)
                    .Repeat.Any();
            }

            return new KalenderRepository(_dataProviderMock);
        }
    }
}
