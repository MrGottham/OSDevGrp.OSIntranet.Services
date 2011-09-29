using System;
using OSDevGrp.OSIntranet.Domain.Interfaces.Fælles;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Kalender
{
    /// <summary>
    /// Interface for en kalenderaftale.
    /// </summary>
    public interface IAftale
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
        /// Angivelse af offentliggørelse.
        /// </summary>
        bool Offentligtgørelse
        {
            get;
            set;
        }

        /// <summary>
        /// Angivelse af privat aftale.
        /// </summary>
        bool Privat
        {
            get;
            set;
        }

        /// <summary>
        /// Angivelse af alarm.
        /// </summary>
        bool Alarm
        {
            get;
            set;
        }

        /// <summary>
        /// Angivelse af, om aftalen er udført.
        /// </summary>
        bool Udført
        {
            get;
            set;
        }

        /// <summary>
        /// Angivelse af, at aftalen skal eksporteres.
        /// </summary>
        bool Eksporter
        {
            get;
            set;
        }

        /// <summary>
        /// Angivelse af, at aftalen er eksporteret.
        /// </summary>
        bool Eksporteret
        {
            get;
            set;
        }
    }
}
