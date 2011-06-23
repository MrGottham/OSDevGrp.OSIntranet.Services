using System.Collections.Generic;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces
{
    /// <summary>
    /// Interface til at bygge objekter i domænemodellen.
    /// </summary>
    public interface IDomainObjectBuilder
    {
        /// <summary>
        /// Sætter adresser til brug ved bygning af domæneobjekter.
        /// </summary>
        /// <param name="adresser">Adresser til brug ved bygning af domæneobjekter.</param>
        void SætAdresser(IEnumerable<AdresseBase> adresser);

        /// <summary>
        /// Sætter adressegrupper til brug ved bygning af domæneobjekter.
        /// </summary>
        /// <param name="adressegrupper">Adressegrupper til brug ved bygning af domæneobjekter.</param>
        void SætAdressegrupper(IEnumerable<Adressegruppe> adressegrupper);

        /// <summary>
        /// Sætter betalingsbetingelser til brug ved bygning af domæneobjekter.
        /// </summary>
        /// <param name="betalingsbetingelser">Betalingsbetingelser til brug ved bygning af domæneobjekter.</param>
        void SætBetalingsbetingelser(IEnumerable<Betalingsbetingelse> betalingsbetingelser);

        /// <summary>
        /// Bygger objekt i domænemodellen.
        /// </summary>
        /// <typeparam name="TSource">Typen på objektet, hvorfra domæneobjektet skal bygges.</typeparam>
        /// <typeparam name="TDomainObject">Typen på domæneobjektet.</typeparam>
        /// <param name="source">Objektet, hvorfra domæneobjektet skal bygges.</param>
        /// <returns>Domæneobjekt.</returns>
        TDomainObject Build<TSource, TDomainObject>(TSource source);
    }
}
