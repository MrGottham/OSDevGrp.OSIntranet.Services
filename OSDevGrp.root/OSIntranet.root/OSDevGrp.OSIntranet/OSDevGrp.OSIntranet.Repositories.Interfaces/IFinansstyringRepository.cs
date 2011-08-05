using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Fælles;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces
{
    /// <summary>
    /// Inteface for repository til finansstyring.
    /// </summary>
    public interface IFinansstyringRepository : IRepository
    {
        /// <summary>
        /// Henter en liste af regnskaber.
        /// </summary>
        /// <param name="getBrevhovedCallback">Callbackmetode til at hente et givent brevhoved.</param>
        /// <returns>Liste af regnskaber.</returns>
        IEnumerable<Regnskab> RegnskabslisteGet(Func<int, Brevhoved> getBrevhovedCallback);

        /// <summary>
        /// Henter et givent regnskab.
        /// </summary>
        /// <param name="nummer">Unik identifikation af regnskabet.</param>
        /// <param name="callback">Callbackmetode til at hente adressen for bogføringslinjer.</param>
        /// <returns>Regnskab.</returns>
        Regnskab RegnskabGet(int nummer, Func<int, AdresseBase> callback);

        /// <summary>
        /// Henter et givent regnskab.
        /// </summary>
        /// <param name="nummer">Unik identifikation af regnskabet.</param>
        /// <param name="getBrevhovedCallback">Callbackmetode til at hente et givent brevhoved.</param>
        /// <param name="getAdresseCallback">Callbackmetode til at hente en given adresse.</param>
        /// <returns>Regnskab.</returns>
        Regnskab RegnskabGet(int nummer, Func<int, Brevhoved> getBrevhovedCallback, Func<int, AdresseBase> getAdresseCallback);

        /// <summary>
        /// Henter alle kontogrupper.
        /// </summary>
        /// <returns>Liste af kontogrupper.</returns>
        IEnumerable<Kontogruppe> KontogruppeGetAll();

        /// <summary>
        /// Henter alle grupper til budgetkonti.
        /// </summary>
        /// <returns>Liste af grupper til budgetkonti.</returns>
        IEnumerable<Budgetkontogruppe> BudgetkontogruppeGetAll();

        /// <summary>
        /// Tilføjer en bogføringslinje.
        /// </summary>
        /// <param name="bogføringstidspunkt">Bogføringstidspunkt.</param>
        /// <param name="bilag">Bilag.</param>
        /// <param name="konto">Konto.</param>
        /// <param name="tekst">Tekst.</param>
        /// <param name="budgetkonto">Budgetkonto.</param>
        /// <param name="debit">Debitbeløb.</param>
        /// <param name="kredit">Kreditbeløb.</param>
        /// <param name="adressekonto">Adressekonto.</param>
        void BogføringslinjeAdd(DateTime bogføringstidspunkt, string bilag, Konto konto, string tekst, Budgetkonto budgetkonto, decimal debit, decimal kredit, AdresseBase adressekonto);
    }
}
