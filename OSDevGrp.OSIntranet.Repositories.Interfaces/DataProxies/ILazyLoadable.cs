namespace OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies
{
    /// <summary>
    /// Interface til angivelse af, at data proxy er lazy loadable.
    /// </summary>
    public interface ILazyLoadable
    {
        /// <summary>
        /// Angivelse af, om data er hentet.
        /// </summary>
        bool DataIsLoaded
        {
            get;
            set;
        }
    }
}
