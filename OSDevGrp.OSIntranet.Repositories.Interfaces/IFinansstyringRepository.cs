using System.Collections.Generic;
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
    }
}
