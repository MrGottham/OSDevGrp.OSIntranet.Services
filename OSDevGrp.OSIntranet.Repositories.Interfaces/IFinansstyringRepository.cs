using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;

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
        /// <returns>Liste af regnskaber.</returns>
        IList<Regnskab> RegnskabslisteGet();

        /// <summary>
        /// Henter et givent regnskab.
        /// </summary>
        /// <param name="nummer">Unik identifikation af regnskabet.</param>
        /// <returns>Regnskab.</returns>
        Regnskab RegnskabGet(int nummer);

        /// <summary>
        /// Henter et givent regnskab.
        /// </summary>
        /// <param name="nummer">Unik identifikation af regnskabet.</param>
        /// <param name="callback">Callbackmetode til at hente adressen for bogføringslinjer.</param>
        /// <returns>Regnskab.</returns>
        Regnskab RegnskabGet(int nummer, Func<int, AdresseBase> callback);

        /// <summary>
        /// Henter alle kontogrupper.
        /// </summary>
        /// <returns>Liste af kontogrupper.</returns>
        IList<Kontogruppe> KontogruppeGetAll();

        /// <summary>
        /// Hetner alle grupper til budgetkonti.
        /// </summary>
        /// <returns>Liste af grupper til budgetkonti.</returns>
        IList<Budgetkontogruppe> BudgetkontogruppeGetAll();
    }
}
