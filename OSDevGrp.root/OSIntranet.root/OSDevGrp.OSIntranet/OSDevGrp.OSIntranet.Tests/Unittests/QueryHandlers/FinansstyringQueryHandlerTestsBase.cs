using System;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers
{
    /// <summary>
    /// Basisklasse til test af QueryHandlers for finansstyring.
    /// </summary>
    public abstract class FinansstyringQueryHandlerTestsBase
    {
        /// <summary>
        /// Danner og returnerer repository for adressekartotek.
        /// </summary>
        /// <returns>Repository for adressekartotek, der kan benyttes til test.</returns>
        protected static IAdresseRepository GetAdresseRepository()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Danner og returnerer et repository for finansstyring.
        /// </summary>
        /// <returns>Repository for finansstyring, der kan benyttes til test.</returns>
        protected static IFinansstyringRepository GetFinansstyringRepository()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Danner og returnerer en objectmapper.
        /// </summary>
        /// <returns>ObjectMapper.</returns>
        protected static IObjectMapper GetObjectMapper()
        {
            throw new NotImplementedException();
        }
    }
}
