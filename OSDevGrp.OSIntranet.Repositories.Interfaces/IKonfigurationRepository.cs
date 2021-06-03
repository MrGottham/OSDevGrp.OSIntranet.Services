namespace OSDevGrp.OSIntranet.Repositories.Interfaces
{
    /// <summary>
    /// Interface til et konfigurationsrepository.
    /// </summary>
    public interface IKonfigurationRepository
    {
        /// <summary>
        /// Angivelse af antal dage for bogføringsperiode.
        /// </summary>
        int DageForBogføringsperiode
        {
            get;
        }
    }
}