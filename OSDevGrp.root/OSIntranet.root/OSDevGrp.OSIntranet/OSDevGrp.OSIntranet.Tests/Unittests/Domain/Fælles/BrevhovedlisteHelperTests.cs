using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Fælles;
using OSDevGrp.OSIntranet.Domain.Fælles;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Domain.Fælles
{
    /// <summary>
    /// Tester hjælper til en liste af brevhoveder.
    /// </summary>
    [TestFixture]
    public class BrevhovedlisteHelperTests
    {
        /// <summary>
        /// Tester, at GetById henter og returnerer et givent brevhoved.
        /// </summary>
        [Test]
        public void TestAtGetByIdHenterBrevhoved()
        {
            var fixture = new Fixture();
            var brevhoveder = fixture.CreateMany<Brevhoved>(3).ToList();
            var brevhovedlisteHelper = new BrevhovedlisteHelper(brevhoveder);
            Assert.That(brevhovedlisteHelper, Is.Not.Null);

            var brevhoved = brevhovedlisteHelper.GetById(brevhoveder.ElementAt(1).Nummer);
            Assert.That(brevhoved, Is.Not.Null);
            Assert.That(brevhoved.Nummer, Is.EqualTo(brevhoveder.ElementAt(1).Nummer));
            Assert.That(brevhoved.Navn, Is.Not.Null);
            Assert.That(brevhoved.Navn, Is.EqualTo(brevhoveder.ElementAt(1).Navn));
        }

        /// <summary>
        /// Tester, at GetById kaster en IntranetRepositoryException, hvis brevhovedet ikke findes.
        /// </summary>
        [Test]
        public void TestAtGetByIdKasterIntranetRepositoryExceptionHvisBrevhovedetIkkeFindes()
        {
            var fixture = new Fixture();
            var brevhoveder = fixture.CreateMany<Brevhoved>(3).ToList();
            var brevhovedlisteHelper = new BrevhovedlisteHelper(brevhoveder);
            Assert.That(brevhovedlisteHelper, Is.Not.Null);

            Assert.Throws<IntranetRepositoryException>(() => brevhovedlisteHelper.GetById(-1));
        }
    }
}
