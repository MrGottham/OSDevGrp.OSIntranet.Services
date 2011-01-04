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
        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis samlingen af navne og værdier er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisNameValueCollectonErNull()
        {
            Assert.Throws<ArgumentNullException>(() => new KonfigurationRepository(null));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en IntranetRepositoryException, hvis DebitorSaldoOverNul ikke er registreret.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterIntranetRepositoryExceptionHvisDebitorSaldoOverNulIkkeErRegistreret()
        {
            var nameValueCollection = new NameValueCollection();
            Assert.Throws<IntranetRepositoryException>(() => new KonfigurationRepository(nameValueCollection));
        }
    }
}
