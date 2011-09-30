namespace OSDevGrp.OSIntranet.Domain.Interfaces.Kalender
{
    /// <summary>
    /// Interface til en basiskalenderaftale.
    /// </summary>
    public interface IAftaleBase
    {
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
