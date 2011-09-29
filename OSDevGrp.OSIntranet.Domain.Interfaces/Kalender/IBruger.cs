using OSDevGrp.OSIntranet.Domain.Interfaces.Fælles;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Kalender
{
    /// <summary>
    /// Interface til en kalenderbruger.
    /// </summary>
    public interface IBruger
    {
        /// <summary>
        /// System under OSWEBDB, som brugeren er tilknyttet.
        /// </summary>
        ISystem System
        {
            get;
        }

        /// <summary>
        /// Unik identifikation for brugeren under systemet.
        /// </summary>
        int Id
        {
            get;
        }

        /// <summary>
        /// Initialer på brugeren.
        /// </summary>
        string Initialer
        {
            get;
            set;
        }

        /// <summary>
        /// Navn på brugeren.
        /// </summary>
        string Navn
        {
            get;
            set;
        }

        /// <summary>
        /// Brugernavn, som der logges på med under OSWEBDB.
        /// </summary>
        string UserName
        {
            get;
            set;
        }
    }
}
