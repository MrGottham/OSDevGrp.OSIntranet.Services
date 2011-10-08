using System;
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
        /// Henter alle kalenderaftaler fra en given dato til et system under OSWEBDB.
        /// </summary>
        /// <param name="system">Unik identifikation af systemet under OSWEBDB.</param>
        /// <param name="fromDate">Datoen, hvorfra kalenderaftaler skal hentes.</param>
        /// <returns>Liste indeholdende kalenderaftaler til systemer.</returns>
        IEnumerable<IAftale> AftaleGetAllBySystem(int system, DateTime fromDate);

        /// <summary>
        /// Henter alle kalenderbrugere til et system under OSWEBDB.
        /// </summary>
        /// <param name="system">Unik identifikation af systemet under OSWEBDB.</param>
        /// <returns>Liste indeholdende kalenderbrugere til systemet.</returns>
        IEnumerable<IBruger> BrugerGetAllBySystem(int system);
    }
}
