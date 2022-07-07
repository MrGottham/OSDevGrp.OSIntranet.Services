using System.Collections.Generic;
using OSDevGrp.OSIntranet.Domain.Interfaces.Fælles;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces
{
    /// <summary>
    /// Inteface for repository til fælles elementer i domænet.
    /// </summary>
    public interface IFællesRepository : IRepository
    {
        /// <summary>
        /// Henter alle systemer under OSWEBDB.
        /// </summary>
        /// <returns>Liste af systemer under OSWEBDB.</returns>
        IEnumerable<ISystem> SystemGetAll();
    }
}