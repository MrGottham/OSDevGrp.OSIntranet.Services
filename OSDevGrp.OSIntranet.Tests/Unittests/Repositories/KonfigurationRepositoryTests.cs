using System;
using System.Collections.Specialized;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories
{
    /// <summary>
    /// Test af konfigurationsrepository.
    /// </summary>
    [TestFixture]
    public class KonfigurationRepositoryTests
    {
        private readonly NameValueCollection _validNameValueCollection = new NameValueCollection
        {
            {"DageForBogføringsperiode", "30"}
        };

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis samlingen af navne og værdier er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisNameValueCollectonErNull()
        {
            Assert.Throws<ArgumentNullException>(() => new KonfigurationRepository(null));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en IntranetRepositoryException, hvis DageForBogføringsperiode ikke er registreret.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterIntranetRepositoryExceptionHvisDageForBogføringsperiodeIkkeErRegistreret()
        {
            var nameValueCollection = new NameValueCollection();
            Assert.Throws<IntranetRepositoryException>(() => new KonfigurationRepository(nameValueCollection));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en IntranetRepositoryException, hvis DageForBogføringsperiode ikke er invalid.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterIntranetRepositoryExceptionHvisDageForBogføringsperiodeErInvalid()
        {
            var nameValueCollection = new NameValueCollection
            {
                {"DageForBogføringsperiode", "X"},
            };
            Assert.Throws<IntranetRepositoryException>(() => new KonfigurationRepository(nameValueCollection));
        }

        /// <summary>
        /// Tester, at DageForBogføringsperiode er 30.
        /// </summary>
        [Test]
        public void TestAtDageForBogføringsperiodeEr30()
        {
            var repository = new KonfigurationRepository(_validNameValueCollection);
            Assert.That(repository, Is.Not.Null);
            Assert.That(repository.DageForBogføringsperiode, Is.EqualTo(30));
        }
    }
}