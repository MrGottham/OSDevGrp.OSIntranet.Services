using System;
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
        public IAdresseRepository Repository
        {
            get
            {
                return _adresseRepository;
            }
        }

        #endregion
    }
}
