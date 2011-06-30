using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Comparers;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.Domain.Fælles;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.CommandHandlers.Core
{
    /// <summary>
    /// Basisklasse for en CommandHandler til regnskaber.
    /// </summary>
    public abstract class RegnskabCommandHandlerBase : FinansstyringCommandHandlerBase
    {
        #region Private variables

        private readonly IAdresseRepository _adresseRepository;
        private readonly IFællesRepository _fællesRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner basisklasse for en CommandHandler til regnskaber.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="adresseRepository">Implementering af repository til adresser.</param>
        /// <param name="fællesRepository">Implementering af repository til fælles elementer i domænet.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        protected RegnskabCommandHandlerBase(IFinansstyringRepository finansstyringRepository, IAdresseRepository adresseRepository, IFællesRepository fællesRepository, IObjectMapper objectMapper)
            : base(finansstyringRepository, objectMapper)
        {
            if (adresseRepository == null)
            {
                throw new ArgumentNullException("adresseRepository");
            }
            if (fællesRepository == null)
            {
                throw new ArgumentNullException("fællesRepository");
            }
            _adresseRepository = adresseRepository;
            _fællesRepository = fællesRepository;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Repository for adresser.
        /// </summary>
        protected virtual IAdresseRepository AdresseRepository
        {
            get
            {
                return _adresseRepository;
            }
        }

        /// <summary>
        /// Repository for fælles elementer i domænet.
        /// </summary>
        protected virtual IFællesRepository FællesRepository
        {
            get
            {
                return _fællesRepository;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Henter og returnerer er givent regnskab.
        /// </summary>
        /// <param name="nummer">Unik identifikation af regnskabet.</param>
        /// <returns>Regnskab.</returns>
        public virtual Regnskab RegnskabGetByNummer(int nummer)
        {
            var brevhovedlisteHelper = new BrevhovedlisteHelper(FællesRepository.BrevhovedGetAll());
            var adresselisteHelper = new AdresselisteHelper(AdresseRepository.AdresseGetAll());
            return Repository.RegnskabGet(nummer, brevhovedlisteHelper.GetById, adresselisteHelper.GetById);
        }

        /// <summary>
        /// Henter og returnerer alle konti i et givent regnskab.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer.</param>
        /// <returns>Alle konti i regnskabet.</returns>
        public virtual IEnumerable<Konto> KontoGetAllByRegnskab(int regnskabsnummer)
        {
            var regnskab = RegnskabGetByNummer(regnskabsnummer);
            var kontoComparer = new KontoComparer();
            return regnskab.Konti.OfType<Konto>().OrderBy(m => m, kontoComparer).ToList();
        }

        /// <summary>
        /// Henter og returnerer en given konto i et givent regnskab.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer.</param>
        /// <param name="kontonummer">Kontonummer.</param>
        /// <returns>Konto.</returns>
        public virtual Konto KontoGetByRegnskabAndKontonummer(int regnskabsnummer, string kontonummer)
        {
            if (string.IsNullOrEmpty(kontonummer))
            {
                throw new ArgumentNullException("kontonummer");
            }
            var konti = KontoGetAllByRegnskab(regnskabsnummer);
            try
            {
                return konti.Single(m => m.Kontonummer.CompareTo(kontonummer) == 0);
            }
            catch (InvalidOperationException ex)
            {
                throw new IntranetRepositoryException(
                    Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof(Konto), kontonummer), ex);
            }
        }

        /// <summary>
        /// Henter og returnerer alle budgetkonti i et givent regnskab.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer.</param>
        /// <returns>Alle budgetkonti i regnskabet.</returns>
        public virtual IEnumerable<Budgetkonto> BudgetkontoGetAllByRegnskab(int regnskabsnummer)
        {
            var regnskab = RegnskabGetByNummer(regnskabsnummer);
            var kontoComparer = new KontoComparer();
            return regnskab.Konti.OfType<Budgetkonto>().OrderBy(m => m, kontoComparer).ToList();
        }

        /// <summary>
        /// Henter og returnerer en given budgetkonto i et givent regnskab.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer.</param>
        /// <param name="kontonummer">Kontonummer.</param>
        /// <returns>Budgetkonto.</returns>
        public virtual Budgetkonto BudgetkontoGetByRegnskabAndKontonummer(int regnskabsnummer, string kontonummer)
        {
            if (string.IsNullOrEmpty(kontonummer))
            {
                throw new ArgumentNullException("kontonummer");
            }
            var budgetkonti = BudgetkontoGetAllByRegnskab(regnskabsnummer);
            try
            {
                return budgetkonti.Single(m => m.Kontonummer.CompareTo(kontonummer) == 0);
            }
            catch (InvalidOperationException ex)
            {
                throw new IntranetRepositoryException(
                    Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof(Budgetkonto), kontonummer),
                    ex);
            }
        }

        /// <summary>
        /// Henter og returnerer alle adressekonti i et givent regnskab.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer.</param>
        /// <returns>Alle adressekonti i et givent regnskab.</returns>
        public virtual IEnumerable<AdresseBase> AdressekontoGetAllByRegnskab(int regnskabsnummer)
        {
            var brevhovedlisteHelper = new BrevhovedlisteHelper(FællesRepository.BrevhovedGetAll());
            var adresselisteHelper = new AdresselisteHelper(AdresseRepository.AdresseGetAll());
            Repository.RegnskabGet(regnskabsnummer, brevhovedlisteHelper.GetById, adresselisteHelper.GetById);
            return adresselisteHelper.Adresser;
        }

        /// <summary>
        /// Henter og returnerer en given adressekonto i et givent regnskab.
        /// </summary>
        /// <param name="regnskabsnummer">Regnskabsnummer.</param>
        /// <param name="nummer">Unik identifikation af adressekontoen.</param>
        /// <returns>Adressekonto.</returns>
        public virtual AdresseBase AdressekontoGetByRegnskabAndNummer(int regnskabsnummer, int nummer)
        {
            var adresselisteHelper = new AdresselisteHelper(AdressekontoGetAllByRegnskab(regnskabsnummer));
            return adresselisteHelper.GetById(nummer);
        }

        #endregion
    }
}
