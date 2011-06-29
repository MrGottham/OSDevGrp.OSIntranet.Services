using System;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.QueryHandlers
{
    /// <summary>
    /// Basisklasse for en QueryHandler til adressekartoteket.
    /// </summary>
    public abstract class AdressekartotekQueryHandlerBase : QueryHandlerBase
    {
        #region Private variables

        private readonly IAdresseRepository _adresseRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner basisklasse for en QueryHandler til adressekartoteket.
        /// </summary>
        /// <param name="adresseRepository">Implementering af repository til adresser.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        protected AdressekartotekQueryHandlerBase(IAdresseRepository adresseRepository, IObjectMapper objectMapper)
            : base(objectMapper)
        {
            if (adresseRepository == null)
            {
                throw new ArgumentNullException("adresseRepository");
            }
            _adresseRepository = adresseRepository;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Repository til adresser.
        /// </summary>
        public virtual IAdresseRepository Repository
        {
            get
            {
                return _adresseRepository;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Henter og returnerer en given betalingsbetingelse.
        /// </summary>
        /// <param name="nummer">Unik identifikation af betalingsbetingelsen.</param>
        /// <returns>Betalingsbetingelse.</returns>
        public virtual Betalingsbetingelse BetalingsbetingelseGetByNummer(int nummer)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
