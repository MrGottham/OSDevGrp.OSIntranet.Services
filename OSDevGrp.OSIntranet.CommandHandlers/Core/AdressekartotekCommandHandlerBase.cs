using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Comparers;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces.Core;
using OSDevGrp.OSIntranet.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.CommandHandlers.Core
{
    /// <summary>
    /// Basisklasse for en CommandHandler til adressekartoteket.
    /// </summary>
    public abstract class AdressekartotekCommandHandlerBase : CommandHandlerTransactionalBase
    {
        #region Private variables

        private readonly IAdresseRepository _adresseRepository;
        private readonly IObjectMapper _objectMapper;
        private readonly IExceptionBuilder _exceptionBuilder;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner basisklasse for en CommandHandler til adressekartoteket.
        /// </summary>
        /// <param name="adresseRepository">Implementering af repository til adresser.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        /// <param name="exceptionBuilder">Implementering af en builder, der kan bygge exceptions.</param>
        protected AdressekartotekCommandHandlerBase(IAdresseRepository adresseRepository, IObjectMapper objectMapper, IExceptionBuilder exceptionBuilder)
        {
            if (adresseRepository == null)
            {
                throw new ArgumentNullException("adresseRepository");
            }
            if (objectMapper == null)
            {
                throw new ArgumentNullException("objectMapper");
            }
            if (exceptionBuilder == null)
            {
                throw new ArgumentNullException("exceptionBuilder");
            }
            _adresseRepository = adresseRepository;
            _objectMapper = objectMapper;
            _exceptionBuilder = exceptionBuilder;
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

        /// <summary>
        /// Objektmapper.
        /// </summary>
        public virtual IObjectMapper ObjectMapper
        {
            get
            {
                return _objectMapper;
            }
        }

        /// <summary>
        /// Builder, der kan bygge exceptions.
        /// </summary>
        public virtual IExceptionBuilder ExceptionBuilder
        {
            get
            {
                return _exceptionBuilder;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Henter og returnerer en given adresse.
        /// </summary>
        /// <param name="nummer">Unik identifikation af adressen.</param>
        /// <returns>Adresse.</returns>
        public virtual AdresseBase AdresseGetByNummer(int nummer)
        {
            var adresselisteHelper = new AdresselisteHelper(Repository.AdresseGetAll());
            return adresselisteHelper.GetById(nummer);
        }

        /// <summary>
        /// Henter og returnerer alle personer.
        /// </summary>
        /// <returns>Alle personer.</returns>
        public virtual IEnumerable<Person> PersonGetAll()
        {
            var adresseComparer = new AdresseComparer();
            return Repository.AdresseGetAll().OfType<Person>().OrderBy(m => m, adresseComparer).ToList();
        }

        /// <summary>
        /// Henter og returnerer en given person.
        /// </summary>
        /// <param name="nummer">Unik identifikation af personen.</param>
        /// <returns>Person.</returns>
        public virtual Person PersonGetByNummer(int nummer)
        {
            var personer = PersonGetAll();
            try
            {
                return personer.Single(m => m.Nummer == nummer);
            }
            catch (InvalidOperationException ex)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (Person).Name, nummer), ex);
            }
        }

        /// <summary>
        /// Henter og returnerer alle firmaer.
        /// </summary>
        /// <returns>Alle firmaer.</returns>
        public virtual IEnumerable<Firma> FirmaGetAll()
        {
            var adresseComparer = new AdresseComparer();
            return Repository.AdresseGetAll().OfType<Firma>().OrderBy(m => m, adresseComparer).ToList();
        }

        /// <summary>
        /// Henter og returnerer et givent firma.
        /// </summary>
        /// <param name="nummer">Unik identifikation af firmaet.</param>
        /// <returns>Firma.</returns>
        public virtual Firma FirmaGetByNummer(int nummer)
        {
            var firmaer = FirmaGetAll();
            try
            {
                return firmaer.Single(m => m.Nummer == nummer);
            }
            catch (InvalidOperationException ex)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (Firma).Name, nummer), ex);
            }
        }

        /// <summary>
        /// Henter og returnerer en given adressegruppe.
        /// </summary>
        /// <param name="nummer">Unik identifikation af adressegruppen.</param>
        /// <returns>Adressegruppe.</returns>
        public virtual Adressegruppe AdressegruppeGetByNummer(int nummer)
        {
            var adressegruppelisteHelper = new AdressegruppelisteHelper(Repository.AdressegruppeGetAll());
            return adressegruppelisteHelper.GetById(nummer);
        }

        #endregion
    }
}
