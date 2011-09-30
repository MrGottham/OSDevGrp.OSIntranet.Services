using OSDevGrp.OSIntranet.Domain.Interfaces.Fælles;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Kalender
{
    /// <summary>
    /// Interface for en brugers kalenderaftale.
    /// </summary>
    public interface IBrugeraftale : IAftaleBase
    {
        /// <summary>
        /// System under OSWEBDB, som aftalen er tilknyttet.
        /// </summary>
        ISystem System
        {
            get;
        }

        /// <summary>
        /// Aftale.
        /// </summary>
        IAftale Aftale
        {
            get;
        }

        /// <summary>
        /// Bruger.
        /// </summary>
        IBruger Bruger
        {
            get;
        }
    }
}
