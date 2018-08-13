using System;
using System.Linq;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories;
using OSDevGrp.OSIntranet.Repositories.DataProxies.Kalender;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using NUnit.Framework;
using AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories
{
    /// <summary>
    /// Tester repository til kalenderaftaler under OSWEBDB.
    /// </summary>
    [TestFixture]
    public class KalenderRepositoryTests
    {
        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis data provideren til MySql er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisMySqlDataProviderErNull()
        {
            var fixture = new Fixture();
            fixture.Inject<IMySqlDataProvider>(null);

            Assert.Throws<ArgumentNullException>(
                () => new KalenderRepository(fixture.Create<IMySqlDataProvider>()));
        }

        /// <summary>
        /// Tester, at AftaleGetAllBySystem henter kalenderaftaler fra og med en given dato til et given system under OSWEBDB.
        /// </summary>
        [Test]
        public void TestAtAftaleGetAllBySystemHenterAftaler()
        {
            var fixture = new Fixture();
            fixture.Inject(new AftaleProxy());

            var mySqlDataProvider = MockRepository.GenerateMock<IMySqlDataProvider>();
            mySqlDataProvider.Expect(m => m.GetCollection<AftaleProxy>(Arg<string>.Is.NotNull))
                .Return(fixture.CreateMany<AftaleProxy>(25));
            fixture.Inject(mySqlDataProvider);

            var repository = fixture.Create<KalenderRepository>();
            Assert.That(repository, Is.Not.Null);

            var aftaler = repository.AftaleGetAllBySystem(fixture.Create<int>(),
                                                          fixture.Create<DateTime>());
            Assert.That(aftaler, Is.Not.Null);
            Assert.That(aftaler.Count(), Is.EqualTo(25));

            mySqlDataProvider.AssertWasCalled(m => m.GetCollection<AftaleProxy>(Arg<string>.Is.NotNull));
        }

        /// <summary>
        /// Tester, at AftaleGetAllBySystem kaster en IntranetRepositoryException ved IntranetRepositoryException.
        /// </summary>
        [Test]
        public void TestAtAftaleGetAllBySystemKasterIntranetRepositoryExceptionVedIntranetRepositoryException()
        {
            var fixture = new Fixture();
            fixture.Inject(new AftaleProxy());

            var mySqlDataProvider = MockRepository.GenerateMock<IMySqlDataProvider>();
            mySqlDataProvider.Expect(m => m.GetCollection<AftaleProxy>(Arg<string>.Is.NotNull))
                .Throw(fixture.Create<IntranetRepositoryException>());
            fixture.Inject(mySqlDataProvider);

            var repository = fixture.Create<KalenderRepository>();
            Assert.That(repository, Is.Not.Null);

            Assert.Throws<IntranetRepositoryException>(
                () =>
                repository.AftaleGetAllBySystem(fixture.Create<int>(), fixture.Create<DateTime>()));
        }

        /// <summary>
        /// Tester, at AftaleGetAllBySystem kaster en IntranetRepositoryException ved Exception.
        /// </summary>
        [Test]
        public void TestAtAftaleGetAllBySystemKasterIntranetRepositoryExceptionVedException()
        {
            var fixture = new Fixture();
            fixture.Inject(new AftaleProxy());

            var mySqlDataProvider = MockRepository.GenerateMock<IMySqlDataProvider>();
            mySqlDataProvider.Expect(m => m.GetCollection<AftaleProxy>(Arg<string>.Is.NotNull))
                .Throw(fixture.Create<Exception>());
            fixture.Inject(mySqlDataProvider);

            var repository = fixture.Create<KalenderRepository>();
            Assert.That(repository, Is.Not.Null);

            Assert.Throws<IntranetRepositoryException>(
                () =>
                repository.AftaleGetAllBySystem(fixture.Create<int>(), fixture.Create<DateTime>()));
        }

        /// <summary>
        /// Tester, at AftaleGetBySystemAndId henter en given kalenderaftale fra et given system under OSWEBDB.
        /// </summary>
        [Test]
        public void TestAtAftaleGetBySystemAndIdHenterAftale()
        {
            var fixture = new Fixture();
            fixture.Inject(new AftaleProxy());

            var mySqlDataProvider = MockRepository.GenerateMock<IMySqlDataProvider>();
            mySqlDataProvider.Expect(m => m.Get(Arg<AftaleProxy>.Is.NotNull))
                .Return(fixture.Create<AftaleProxy>());
            fixture.Inject(mySqlDataProvider);

            var repository = fixture.Create<KalenderRepository>();
            Assert.That(repository, Is.Not.Null);

            var aftale = repository.AftaleGetBySystemAndId(fixture.Create<int>(),
                                                           fixture.Create<int>());
            Assert.That(aftale, Is.Not.Null);

            mySqlDataProvider.AssertWasCalled(m => m.Get(Arg<AftaleProxy>.Is.NotNull));
        }

        /// <summary>
        /// Tester, at AftaleGetBySystemAndId kaster en IntranetRepositoryException ved IntranetRepositoryException.
        /// </summary>
        [Test]
        public void TestAtAftaleGetBySystemAndIdKasterIntranetRepositoryExceptionVedIntranetRepositoryException()
        {
            var fixture = new Fixture();
            fixture.Inject(new AftaleProxy());

            var mySqlDataProvider = MockRepository.GenerateMock<IMySqlDataProvider>();
            mySqlDataProvider.Expect(m => m.Get(Arg<AftaleProxy>.Is.NotNull))
                .Throw(fixture.Create<IntranetRepositoryException>());
            fixture.Inject(mySqlDataProvider);

            var repository = fixture.Create<KalenderRepository>();
            Assert.That(repository, Is.Not.Null);

            Assert.Throws<IntranetRepositoryException>(
                () => repository.AftaleGetBySystemAndId(fixture.Create<int>(), fixture.Create<int>()));
        }

        /// <summary>
        /// Tester, at AftaleGetBySystemAndId kaster en IntranetRepositoryException ved Exception.
        /// </summary>
        [Test]
        public void TestAtAftaleGetBySystemAndIdKasterIntranetRepositoryExceptionVedException()
        {
            var fixture = new Fixture();
            fixture.Inject(new AftaleProxy());

            var mySqlDataProvider = MockRepository.GenerateMock<IMySqlDataProvider>();
            mySqlDataProvider.Expect(m => m.Get(Arg<AftaleProxy>.Is.NotNull))
                .Throw(fixture.Create<Exception>());
            fixture.Inject(mySqlDataProvider);

            var repository = fixture.Create<KalenderRepository>();
            Assert.That(repository, Is.Not.Null);

            Assert.Throws<IntranetRepositoryException>(
                () => repository.AftaleGetBySystemAndId(fixture.Create<int>(), fixture.Create<int>()));
        }

        /// <summary>
        /// Tester, at BrugerGetAllBySstem henter brugere til et given system under OSWEBDB.
        /// </summary>
        [Test]
        public void TestAtBrugerGetAllBySystemHenterBrugere()
        {
            var fixture = new Fixture();

            var mySqlDataProvider = MockRepository.GenerateMock<IMySqlDataProvider>();
            mySqlDataProvider.Expect(m => m.GetCollection<BrugerProxy>(Arg<string>.Is.NotNull))
                .Return(fixture.CreateMany<BrugerProxy>(3));
            fixture.Inject(mySqlDataProvider);

            var repository = fixture.Create<KalenderRepository>();
            Assert.That(repository, Is.Not.Null);

            var brugere = repository.BrugerGetAllBySystem(fixture.Create<int>());
            Assert.That(brugere, Is.Not.Null);
            Assert.That(brugere.Count(), Is.EqualTo(3));

            mySqlDataProvider.AssertWasCalled(m => m.GetCollection<BrugerProxy>(Arg<string>.Is.NotNull));
        }

        /// <summary>
        /// Tester, at BrugerGetAllBySstem kaster en IntranetRepositoryException ved IntranetRepositoryException.
        /// </summary>
        [Test]
        public void TestAtBrugerGetAllBySystemKasterIntranetRepositoryExceptionVedIntranetRepositoryException()
        {
            var fixture = new Fixture();

            var mySqlDataProvider = MockRepository.GenerateMock<IMySqlDataProvider>();
            mySqlDataProvider.Expect(m => m.GetCollection<BrugerProxy>(Arg<string>.Is.NotNull))
                .Throw(fixture.Create<IntranetRepositoryException>());
            fixture.Inject(mySqlDataProvider);

            var repository = fixture.Create<KalenderRepository>();
            Assert.That(repository, Is.Not.Null);

            Assert.Throws<IntranetRepositoryException>(
                () => repository.BrugerGetAllBySystem(fixture.Create<int>()));

            mySqlDataProvider.AssertWasCalled(m => m.GetCollection<BrugerProxy>(Arg<string>.Is.NotNull));
        }

        /// <summary>
        /// Tester, at BrugerGetAllBySstem kaster en IntranetRepositoryException ved Exception.
        /// </summary>
        [Test]
        public void TestAtBrugerGetAllBySystemKasterIntranetRepositoryExceptionVedException()
        {
            var fixture = new Fixture();

            var mySqlDataProvider = MockRepository.GenerateMock<IMySqlDataProvider>();
            mySqlDataProvider.Expect(m => m.GetCollection<BrugerProxy>(Arg<string>.Is.NotNull))
                .Throw(fixture.Create<Exception>());
            fixture.Inject(mySqlDataProvider);

            var repository = fixture.Create<KalenderRepository>();
            Assert.That(repository, Is.Not.Null);

            Assert.Throws<IntranetRepositoryException>(
                () => repository.BrugerGetAllBySystem(fixture.Create<int>()));

            mySqlDataProvider.AssertWasCalled(m => m.GetCollection<BrugerProxy>(Arg<string>.Is.NotNull));
        }
    }
}
