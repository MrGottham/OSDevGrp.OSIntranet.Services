using System.Collections.Generic;
using OSDevGrp.OSIntranet.Domain.Interfaces.Kalender;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces
{
    /// <summary>
    /// Interface til et repository for kalenderaftaler under OSWEBDB.
    /// </summary>
    public interface IKalenderRepository : IRepository
    {
        /// <summary>
        /// Henter alle kalenderbrugere til et system under OSWEBDB.
        /// </summary>
        /// <param name="system">Unik identifikation af systemet under OSWEBDB.</param>
        /// <returns>Liste indeholdende kalenderbrugere til systemet.</returns>
        IEnumerable<IBruger> BrugerGetAllBySystem(int system);
    }
}
