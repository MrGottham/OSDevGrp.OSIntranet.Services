namespace OSDevGrp.OSIntranet.Domain.Interfaces
{
    /// <summary>
    /// Interface til en hjælper på en liste af domæneobjekter.
    /// </summary>
    /// <typeparam name="TDomainObject">Typen på listens domæneobjekter.</typeparam>
    /// <typeparam name="TId">Typen på Id for listens domæneobjekter.</typeparam>
    public interface IDomainObjectListHelper<out TDomainObject, in TId>
    {
        /// <summary>
        /// Henter og returnerer et givent domæneobjekt fra listen.
        /// </summary>
        /// <param name="id">Unik identifikation af domæneobjektet, der skal hentes fra listen.</param>
        /// <returns>Domæneobjekt.</returns>
        TDomainObject GetById(TId id);
    }
}
