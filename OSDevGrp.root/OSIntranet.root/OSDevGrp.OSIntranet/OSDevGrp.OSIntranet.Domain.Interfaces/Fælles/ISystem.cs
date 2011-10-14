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

        /// <summary>
        /// Angivelse af, om systemet benytter kalenderen under OSWEBDB.
        /// </summary>
        bool Kalender
        {
            get;
            set;
        }
    }
}
