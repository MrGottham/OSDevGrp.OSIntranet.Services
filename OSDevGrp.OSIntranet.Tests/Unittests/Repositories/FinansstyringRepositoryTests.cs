using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories
{
    /// <summary>
    /// Test af repository til finansstyring.
    /// </summary>
    public class FinansstyringRepositoryTests
    {
        /// <summary>
        /// Tester, at RegnskabslisteGet henter alle regnskaber.
        /// </summary>
        [Test]
        public void TestAtRegnskabslisteGetHenterAlleRegnskaber()
        {
            var repository = new FinansstyringRepository();
            var regnskaber = repository.RegnskabslisteGet();
            Assert.That(regnskaber, Is.Not.Null);
            Assert.That(regnskaber.Count, Is.EqualTo(2));
            Assert.That(regnskaber[0].Nummer, Is.EqualTo(1));
            Assert.That(regnskaber[0].Navn, Is.EqualTo("Ole Sørensen"));
            Assert.That(regnskaber[1].Nummer, Is.EqualTo(2));
            Assert.That(regnskaber[1].Navn, Is.EqualTo("Bryllup"));
        }

        /// <summary>
        /// Tester, at RegnskabGet henter et givent regnskab.
        /// </summary>
        [Test]
        public void TestAtRegnskabGetHenterRegnskab()
        {
            var repository = new FinansstyringRepository();
            var regnskab = repository.RegnskabGet(1);
            Assert.That(regnskab, Is.Not.Null);
            Assert.That(regnskab.Nummer, Is.EqualTo(1));
            Assert.That(regnskab.Navn, Is.Not.Null);
            Assert.That(regnskab.Navn, Is.EqualTo("Ole Sørensen"));
            Assert.That(regnskab.Konti, Is.Not.Null);
            Assert.That(regnskab.Konti.Count, Is.GreaterThan(0));
            Assert.That(regnskab.Konti.OfType<Konto>().Count(), Is.EqualTo(4));
            foreach (var konto in regnskab.Konti.OfType<Konto>().ToList())
            {
                Assert.That(konto, Is.Not.Null);
                Assert.That(konto.Kreditoplysninger, Is.Not.Null);
                Assert.That(konto.Kreditoplysninger.Count, Is.GreaterThan(0));
                Assert.That(konto.Bogføringslinjer, Is.Not.Null);
                Assert.That(konto.Bogføringslinjer.Count, Is.GreaterThan(0));
            }
            Assert.That(regnskab.Konti.OfType<Budgetkonto>().Count(), Is.EqualTo(72));
            foreach (var budgetkonto in regnskab.Konti.OfType<Budgetkonto>().ToList())
            {
                Assert.That(budgetkonto, Is.Not.Null);
                Assert.That(budgetkonto.Budgetoplysninger, Is.Not.Null);
                Assert.That(budgetkonto.Budgetoplysninger.Count, Is.GreaterThan(0));
                Assert.That(budgetkonto.Bogføringslinjer, Is.Not.Null);
                Assert.That(budgetkonto.Bogføringslinjer.Count, Is.GreaterThan(0));
            }
        }

        /// <summary>
        /// Tester, at RegnskabGet kaster en IntranetRepositoryException, hvis regnskabet ikke findes.
        /// </summary>
        [Test]
        public void TestAtRegnskabGetKasterIntranetRepositoryExceptionHvisRegnskabIkkeFindes()
        {
            var repository = new FinansstyringRepository();
            Assert.Throws<IntranetRepositoryException>(() => repository.RegnskabGet(-1));
        }
    }
}
