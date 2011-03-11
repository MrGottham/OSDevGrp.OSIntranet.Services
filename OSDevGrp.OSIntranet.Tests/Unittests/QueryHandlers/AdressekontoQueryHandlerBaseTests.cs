using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.QueryHandlers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers
{
    /// <summary>
    /// Tester basisklasse for QueryHandler til forespørgelser på adressekonti.
    /// </summary>
    [TestFixture]
    public class AdressekontoQueryHandlerBaseTests : FinansstyringQueryHandlerTestsBase
    {
        /// <summary>
        /// Egen klasse til test af basisklasse for QueryHandler til forespørgelser på adressekonti.
        /// </summary>
        private class MyAdressekontoQueryHandler : AdressekontoQueryHandlerBase
        {
            /// <summary>
            /// Danner klasse til test af basisklasse for QueryHandler til forespørgelser på adressekonti.
            /// </summary>
            /// <param name="adresseRepository">Implementering af repository til adresser.</param>
            /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
            public MyAdressekontoQueryHandler(IAdresseRepository adresseRepository, IFinansstyringRepository finansstyringRepository)
                : base(adresseRepository, finansstyringRepository)
            {
            }

            /// <summary>
            /// Henter en liste af adressekonti i et givent regnskab.
            /// </summary>
            /// <param name="regnskabsnummer">Regnskabsnummer.</param>
            /// <returns>Adressekonti.</returns>
            public new IEnumerable<AdresseBase> AdressekontoGetAllByRegnskabsnummer(int regnskabsnummer)
            {
                return base.AdressekontoGetAllByRegnskabsnummer(regnskabsnummer);
            }
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis repositoryet for adresser er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisAdresseRepositoryErNull()
        {
            Assert.Throws<ArgumentNullException>(() => new MyAdressekontoQueryHandler(null, null));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis repositoryet for finansstyring er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisFinansstyringRepositoryErNull()
        {
            var adresseRepository = GetAdresseRepository();
            Assert.Throws<ArgumentNullException>(() => new MyAdressekontoQueryHandler(adresseRepository, null));
        }

        /// <summary>
        /// Tester, at AdressekontoGetAllByRegnskabsnummer henter adressekonti.
        /// </summary>
        [Test]
        public void TestAtAdressekontoGetAllByRegnskabsnummerHenterAdressekonti()
        {
            var adresseRepository = GetAdresseRepository();
            var finansstyringRepository = GetFinansstyringRepository();
            var adressekontoQueryHandler = new MyAdressekontoQueryHandler(adresseRepository, finansstyringRepository);
            var adressekonti = adressekontoQueryHandler.AdressekontoGetAllByRegnskabsnummer(1);
            Assert.That(adressekonti, Is.Not.Null);
            Assert.That(adressekonti.Count(), Is.EqualTo(3));
        }

        /// <summary>
        /// Tester, at AdressekontoGetAllByRegnskabsnummer kaster en IntranetRepositoryException, hvis regnskabet ikke findes.
        /// </summary>
        [Test]
        public void TestAtAdressekontoGetAllByRegnskabsnummerKasterIntranetRepositoryExceptionHvisRegnskabIkkeFindes()
        {
            var adresseRepository = GetAdresseRepository();
            var finansstyringRepository = GetFinansstyringRepository();
            var adressekontoQueryHandler = new MyAdressekontoQueryHandler(adresseRepository, finansstyringRepository);
            Assert.Throws<IntranetRepositoryException>(() => adressekontoQueryHandler.AdressekontoGetAllByRegnskabsnummer(-1));
        }
    }
}
