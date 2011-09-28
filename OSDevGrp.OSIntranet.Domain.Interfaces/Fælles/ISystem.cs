namespace OSDevGrp.OSIntranet.Domain.Interfaces.Fælles
{
    /// <summary>
    /// Interface for et system under OSWEBDB.
    /// </summary>
    public interface ISystem
    {
        /// <summary>
        /// Unik identifikation af systemet.
        /// </summary>
        int Nummer
        {
            get;
        }

        /// <summary>
        /// Titel på systemet.
        /// </summary>
        string Titel
        {
            get;
            set;
        }
    }
}
