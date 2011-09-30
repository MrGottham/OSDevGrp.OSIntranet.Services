using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.Domain.Interfaces.Fælles;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Kalender
{
    /// <summary>
    /// Interface for en kalenderaftale.
    /// </summary>
    public interface IAftale : IAftaleBase
    {
        /// <summary>
        /// System under OSWEBDB, som aftalen er tilknyttet.
        /// </summary>
        ISystem System
        {
            get;
        }

        /// <summary>
        /// Unik identifikation for aftalen under systemet.
        /// </summary>
        int Id
        {
            get;
        }

        /// <summary>
        /// Fra dato og tidspunkt.
        /// </summary>
        DateTime FraTidspunkt
        {
            get;
            set;
        }

        /// <summary>
        /// Til dato og tidspunkt.
        /// </summary>
        DateTime TilTidspunkt
        {
            get;
            set;
        }

        /// <summary>
        /// Emne.
        /// </summary>
        string Emne
        {
            get;
            set;
        }

        /// <summary>
        /// Notat.
        /// </summary>
        string Notat
        {
            get;
            set;
        }

        /// <summary>
        /// Aftalens deltagere.
        /// </summary>
        IEnumerable<IBrugeraftale> Deltagere
        {
            get;
        }

        /// <summary>
        /// Tilføjer en deltager til aftalen.
        /// </summary>
        /// <param name="deltager">Brugeraftale for deltageren.</param>
        void TilføjDeltager(IBrugeraftale deltager);

        /// <summary>
        /// Fjerner en deltager fra aftalen.
        /// </summary>
        /// <param name="deltager">Brugeraftale for deltageren.</param>
        void FjernDeltager(IBrugeraftale deltager);
    }
}
