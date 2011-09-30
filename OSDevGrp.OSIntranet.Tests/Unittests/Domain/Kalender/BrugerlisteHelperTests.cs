using System.Linq;
using OSDevGrp.OSIntranet.Domain.Interfaces.Fælles;
using OSDevGrp.OSIntranet.Domain.Kalender;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Domain.Kalender
{
    /// <summary>
    /// Tester hjælper til en liste af brugere i et givent system.
    /// </summary>
    [TestFixture]
    public class BrugerlisteHelperTests
    {
        /// <summary>
        /// Tester, at GetById henter og returnerer en given bruger.
        /// </summary>
        [Test]
        public void TestAtGetByIdHenterBruger()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<ISystem>());
            var brugere = fixture.CreateMany<Bruger>(3).ToList();
            var brugerlisteHelper = new BrugerlisteHelper(brugere);
            Assert.That(brugerlisteHelper, Is.Not.Null);

            var bruger = brugerlisteHelper.GetById(brugere.ElementAt(1).Id);
            Assert.That(bruger, Is.Not.Null);
            Assert.That(bruger.Id, Is.EqualTo(brugere.ElementAt(1).Id));
            Assert.That(bruger.Initialer, Is.Not.Null);
            Assert.That(bruger.Initialer, Is.EqualTo(brugere.ElementAt(1).Initialer));
            Assert.That(bruger.Navn, Is.Not.Null);
            Assert.That(bruger.Navn, Is.EqualTo(brugere.ElementAt(1).Navn));
        }

        /// <summary>
        /// Tester, at GetById kaster en IntranetRepositoryException, hvis brugeren ikke findes.
        /// </summary>
        [Test]
        public void TestAtGetByIdKasterIntranetRepositoryExceptionHvisBrugerIkkeFindes()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<ISystem>());
            var brugere = fixture.CreateMany<Bruger>(3).ToList();
            var brugerlisteHelper = new BrugerlisteHelper(brugere);
            Assert.That(brugerlisteHelper, Is.Not.Null);

            Assert.Throws<IntranetRepositoryException>(() => brugerlisteHelper.GetById(-1));
        }
    }
}
