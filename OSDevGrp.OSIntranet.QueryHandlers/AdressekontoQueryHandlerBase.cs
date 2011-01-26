using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.QueryHandlers
{
    /// <summary>
    /// Basisklasse for QueryHandler til forespørgelser på adressekonti.
    /// </summary>
    public abstract class AdressekontoQueryHandlerBase
    {
        #region Private variables

        private readonly IAdresseRepository _adresseRepository;
        private readonly IFinansstyringRepository _finansstyringRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner basisklasse for QueryHandler til forespørgelser på adressekonti.
        /// </summary>
        /// <param name="adresseRepository">Implementering af repository til adresser.</param>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        protected AdressekontoQueryHandlerBase(IAdresseRepository adresseRepository, IFinansstyringRepository finansstyringRepository)
        {
            if (adresseRepository == null)
            {
                throw new ArgumentNullException("adresseRepository");
            }
            if (finansstyringRepository == null)
            {
                throw new ArgumentNullException("finansstyringRepository");
            }
            _adresseRepository = adresseRepository;
            _finansstyringRepository = finansstyringRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Henter en liste af adressekonti i et givent regnskab.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer.</param>
        /// <returns>Adressekonti.</returns>
        protected virtual IEnumerable<AdresseBase> AdressekontoGetAllByRegnskabsnummer(int regnskabsnummer)
        {
            var adresser = _adresseRepository.AdresseGetAll();
            var regnskab = _finansstyringRepository.RegnskabGet(regnskabsnummer, nummer =>
                                                                                     {
                                                                                         var adresse = adresser.SingleOrDefault(m => m.Nummer == nummer);
                                                                                         if (adresse == null)
                                                                                         {
                                                                                             throw new IntranetRepositoryException(string.Empty);
                                                                                         }
                                                                                         return adresse;
                                                                                     });
            throw new NotImplementedException();
        }

        #endregion
    }
}
