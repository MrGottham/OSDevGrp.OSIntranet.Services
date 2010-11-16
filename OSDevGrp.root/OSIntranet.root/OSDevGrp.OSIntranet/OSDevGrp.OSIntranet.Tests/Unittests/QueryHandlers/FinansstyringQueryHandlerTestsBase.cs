using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers
{
    /// <summary>
    /// Basisklasse til test af QueryHandlers for finansstyring.
    /// </summary>
    public abstract class FinansstyringQueryHandlerTestsBase
    {
        /// <summary>
        /// Danner og returnerer et repository for finansstyring.
        /// </summary>
        /// <returns>Repository for finansstyring, der kan benyttes til test.</returns>
        protected static IFinansstyringRepository GetFinansstyringRepository()
        {
            // Dan testdata for regnskaber.
            var regnskaber = new List<Regnskab>
                                 {
                                     new Regnskab(1, "Privatregnskab, Ole Sørensen"),
                                     new Regnskab(2, "Privatregnskab, Patrick Emil Sørensen"),
                                     new Regnskab(3, "Privatregnskab, Mathias Johannes Sørensen")
                                 };

            // Dan mockup af repository.
            var repository = MockRepository.GenerateMock<IFinansstyringRepository>();
            repository.Expect(m => m.RegnskabslisteGet()).Return(regnskaber);
            repository.Expect(m => m.RegnskabGet(Arg<int>.Is.Equal(1))).Return(regnskaber.Single(m => m.Nummer == 1));
            repository.Expect(m => m.RegnskabGet(Arg<int>.Is.Equal(2))).Return(regnskaber.Single(m => m.Nummer == 2));
            repository.Expect(m => m.RegnskabGet(Arg<int>.Is.Equal(3))).Return(regnskaber.Single(m => m.Nummer == 3));
            return repository;
        }
    }
}
