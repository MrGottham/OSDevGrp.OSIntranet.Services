using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Resources;

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
        /// <param name="statusDato">Statusdato.</param>
        /// <returns>Adressekonti.</returns>
        protected virtual IEnumerable<AdresseBase> AdressekontoGetAllByRegnskabsnummer(int regnskabsnummer, DateTime statusDato)
        {
            var adresser = _adresseRepository.AdresseGetAll();
            _finansstyringRepository.RegnskabGet(regnskabsnummer, nummer =>
                                                                      {
                                                                          var adresse = adresser.SingleOrDefault(m => m.Nummer == nummer);
                                                                          if (adresse == null)
                                                                          {
                                                                              var message = Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, "AdresseBase", nummer);
                                                                              throw new IntranetRepositoryException(message);
                                                                          }
                                                                          return adresse;
                                                                      });
            foreach (ICalculatable calculatable in adresser)
            {
                calculatable.Calculate(statusDato);
            }
            return adresser;
        }

        /// <summary>
        /// Henter en liste af adressekonti, der har en saldo pr. en given statusdato, i et givent regnskab.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer.</param>
        /// <param name="statusDato">Statusdato.</param>
        /// <param name="overNul">Angivelse af, om saldo skal være større end eller mindre end 0.</param>
        /// <returns>Adressekonti.</returns>
        protected virtual IEnumerable<AdresseBase> AdressekontoGetAllWithValueByRegnskabsnummer(int regnskabsnummer, DateTime statusDato, bool overNul)
        {
            var adresser = AdressekontoGetAllByRegnskabsnummer(regnskabsnummer, statusDato);
            if (overNul)
            {
                return adresser.Where(m => m.SaldoPrStatusdato > 0M);
            }
            return adresser.Where(m => m.SaldoPrStatusdato < 0M);
        }

        /// <summary>
        /// Henter en adressekonto med en saldo i et givent regnskab på en given statusdato.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer.</param>
        /// <param name="statusDato">Statusdato.</param>
        /// <param name="nummer">Unik identifikation af adressekontoen.</param>
        /// <returns>Adressekonto.</returns>
        protected virtual AdresseBase AdressekontoGetByRegnskabsnummerAndNummer(int regnskabsnummer, DateTime statusDato, int nummer)
        {
            var adresse = AdressekontoGetAllByRegnskabsnummer(regnskabsnummer, statusDato)
                .SingleOrDefault(m => m.Nummer == nummer);
            if (adresse == null)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById,
                                                                                   "AdresseBase", nummer));
            }
            return adresse;
        }

        #endregion
    }
}
