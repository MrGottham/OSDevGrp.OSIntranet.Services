using System.Linq;
using OSDevGrp.OSIntranet.Domain.Fælles;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Domain.Fælles
{
    /// <summary>
    /// Tester hjælper til en liste af systemer.
    /// </summary>
    [TestFixture]
    public class SystemlisteHelperTests
    {
        /// <summary>
        /// Tester, at GetById henter og returnerer et givent system.
        /// </summary>
        [Test]
        public void TestAtGetByIdHenterSystem()
        {
            var fixture = new Fixture();
            var systemer = fixture.CreateMany<OSIntranet.Domain.Fælles.System>(3).ToList();
            var systemlisteHelper = new SystemlisteHelper(systemer);
            Assert.That(systemlisteHelper, Is.Not.Null);

            var system = systemlisteHelper.GetById(systemer.ElementAt(1).Nummer);
            Assert.That(system, Is.Not.Null);
            Assert.That(system.Nummer, Is.EqualTo(systemer.ElementAt(1).Nummer));
            Assert.That(system.Titel, Is.Not.Null);
            Assert.That(system.Titel, Is.EqualTo(systemer.ElementAt(1).Titel));
        }

        /// <summary>
        /// Tester, at GetById kaster en IntranetRepositoryException, hvis systemet ikke findes.
        /// </summary>
        [Test]
        public void TestAtGetByIdKasterIntranetRepositoryExceptionHvisSystemIkkeFindes()
        {
            var fixture = new Fixture();
            var systemer = fixture.CreateMany<OSIntranet.Domain.Fælles.System>(3).ToList();
            var systemlisteHelper = new SystemlisteHelper(systemer);
            Assert.That(systemlisteHelper, Is.Not.Null);

            Assert.Throws<IntranetRepositoryException>(() => systemlisteHelper.GetById(-1));
        }
    }
}
