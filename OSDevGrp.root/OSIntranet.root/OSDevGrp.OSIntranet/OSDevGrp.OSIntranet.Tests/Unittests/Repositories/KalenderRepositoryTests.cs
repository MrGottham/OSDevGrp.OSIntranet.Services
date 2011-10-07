using System;
using System.Linq;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories;
using OSDevGrp.OSIntranet.Repositories.DataProxies.Kalender;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using NUnit.Framework;
using Ploeh.AutoFixture;
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
                () => new KalenderRepository(fixture.CreateAnonymous<IMySqlDataProvider>()));
        }

        /// <summary>
        /// Tester, at BrugerGetAllBySstem henter bruger til et given system under OSWEBDB.
        /// </summary>
        [Test]
        public void TestAtBrugerGetAllBySystemHenterBruger()
        {
            var fixture = new Fixture();

            var mySqlDataProvider = MockRepository.GenerateMock<IMySqlDataProvider>();
            mySqlDataProvider.Expect(m => m.GetCollection<BrugerProxy>(Arg<string>.Is.NotNull))
                .Return(fixture.CreateMany<BrugerProxy>());
            fixture.Inject(mySqlDataProvider);

            var repository = fixture.CreateAnonymous<KalenderRepository>();
            Assert.That(repository, Is.Not.Null);

            var brugere = repository.BrugerGetAllBySystem(fixture.CreateAnonymous<int>());
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
                .Throw(fixture.CreateAnonymous<IntranetRepositoryException>());
            fixture.Inject(mySqlDataProvider);

            var repository = fixture.CreateAnonymous<KalenderRepository>();
            Assert.That(repository, Is.Not.Null);

            Assert.Throws<IntranetRepositoryException>(
                () => repository.BrugerGetAllBySystem(fixture.CreateAnonymous<int>()));

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
                .Throw(fixture.CreateAnonymous<Exception>());
            fixture.Inject(mySqlDataProvider);

            var repository = fixture.CreateAnonymous<KalenderRepository>();
            Assert.That(repository, Is.Not.Null);

            Assert.Throws<IntranetRepositoryException>(
                () => repository.BrugerGetAllBySystem(fixture.CreateAnonymous<int>()));

            mySqlDataProvider.AssertWasCalled(m => m.GetCollection<BrugerProxy>(Arg<string>.Is.NotNull));
        }
    }
}
