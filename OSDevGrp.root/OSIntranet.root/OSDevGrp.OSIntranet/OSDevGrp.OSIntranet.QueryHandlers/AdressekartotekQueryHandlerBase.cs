using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Comparers;
using OSDevGrp.OSIntranet.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Resources;

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
                throw new IntranetRepositoryException(
                    Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (Person), nummer), ex);
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
                throw new IntranetRepositoryException(
                    Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (Firma), nummer), ex);
            }
        }

        /// <summary>
        /// Henter og returnerer et given postnummer.
        /// </summary>
        /// <param name="landekode">Landekode.</param>
        /// <param name="postnummer">Postnummer.</param>
        /// <returns>Postnummer.</returns>
        public virtual Postnummer PostnummerGetByLandekodeAndPostnummer(string landekode, string postnummer)
        {
            if (string.IsNullOrEmpty(landekode))
            {
                throw new ArgumentNullException("landekode");
            }
            if (string.IsNullOrEmpty(postnummer))
            {
                throw new ArgumentNullException("postnummer");
            }
            var postnumre = _adresseRepository.PostnummerGetAll();
            try
            {
                return postnumre
                    .Single(m => m.Landekode.CompareTo(landekode) == 0 && m.Postnr.CompareTo(postnummer) == 0);
            }
            catch (InvalidOperationException ex)
            {
                throw new IntranetRepositoryException(
                    Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (Postnummer),
                                                 string.Format("{0}-{1}", landekode, postnummer)), ex);
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

        /// <summary>
        /// Henter og returnerer en given betalingsbetingelse.
        /// </summary>
        /// <param name="nummer">Unik identifikation af betalingsbetingelsen.</param>
        /// <returns>Betalingsbetingelse.</returns>
        public virtual Betalingsbetingelse BetalingsbetingelseGetByNummer(int nummer)
        {
            var betalingsbetingelselisteHelper = new BetalingsbetingelselisteHelper(Repository.BetalingsbetingelseGetAll());
            return betalingsbetingelselisteHelper.GetById(nummer);
        }

        #endregion
    }
}
