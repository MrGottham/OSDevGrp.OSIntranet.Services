using System.Collections.Generic;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces
{
    /// <summary>
    /// Interface for repository til adressekartoteket.
    /// </summary>
    public interface IAdresseRepository : IRepository
    {
        /// <summary>
        /// Henter alle adresser.
        /// </summary>
        /// <returns>Liste af adresser.</returns>
        IEnumerable<AdresseBase> AdresseGetAll();

        /// <summary>
        /// Henter alle adressegrupper.
        /// </summary>
        /// <returns>Liste af adressegrupper.</returns>
        IEnumerable<Adressegruppe> AdressegruppeGetAll();
    }
}
