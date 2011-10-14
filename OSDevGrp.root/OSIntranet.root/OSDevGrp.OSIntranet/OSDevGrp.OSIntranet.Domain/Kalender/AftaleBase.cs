using System;
using OSDevGrp.OSIntranet.Domain.Interfaces.Kalender;

namespace OSDevGrp.OSIntranet.Domain.Kalender
{
    /// <summary>
    /// Basiskalenderaftale.
    /// </summary>
    public abstract class AftaleBase : IAftaleBase
    {
        #region Private constants

        private const int PublicValue = 1;
        private const int PrivateValue = 2;
        private const int BellValue = 4;
        private const int DoneValue = 8;
        private const int ExportValue = 16;
        private const int ExportedValue = 32;

        #endregion

        #region Private variables

        private int _properties;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner en basiskalenderaftale.
        /// </summary>
        /// <param name="properties">Værdi for aftalens properties.</param>
        protected AftaleBase(int properties)
        {
            _properties = properties;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Egenskaber for aftalen.
        /// </summary>
        public virtual int Properties
        {
            get
            {
                return _properties;
            }
            protected set
            {
                _properties = value;
            }
        }

        /// <summary>
        /// Angivelse af offentliggørelse.
        /// </summary>
        public virtual bool Offentligtgørelse
        {
            get
            {
                return (Properties & PublicValue) != 0;
            }
            set
            {
                Properties = (Convert.ToInt32(value)*PublicValue) + (Convert.ToInt32(Privat)*PrivateValue) +
                             (Convert.ToInt32(Alarm)*BellValue) + (Convert.ToInt32(Udført)*DoneValue) +
                             (Convert.ToInt32(Eksporter)*ExportValue) + (Convert.ToInt32(Eksporteret)*ExportedValue);
            }
        }

        /// <summary>
        /// Angivelse af privat aftale.
        /// </summary>
        public virtual bool Privat
        {
            get
            {
                return (Properties & PrivateValue) != 0;
            }
            set
            {
                Properties = (Convert.ToInt32(Offentligtgørelse)*PublicValue) + (Convert.ToInt32(value)*PrivateValue) +
                             (Convert.ToInt32(Alarm)*BellValue) + (Convert.ToInt32(Udført)*DoneValue) +
                             (Convert.ToInt32(Eksporter)*ExportValue) + (Convert.ToInt32(Eksporteret)*ExportedValue);
            }
        }

        /// <summary>
        /// Angivelse af alarm.
        /// </summary>
        public virtual bool Alarm
        {
            get
            {
                return (Properties & BellValue) != 0;
            }
            set
            {
                if (value && Udført)
                {
                    Udført = false;
                }
                Properties = (Convert.ToInt32(Offentligtgørelse)*PublicValue) + (Convert.ToInt32(Privat)*PrivateValue) +
                             (Convert.ToInt32(value)*BellValue) + (Convert.ToInt32(Udført)*DoneValue) +
                             (Convert.ToInt32(Eksporter)*ExportValue) + (Convert.ToInt32(Eksporteret)*ExportedValue);
            }
        }

        /// <summary>
        /// Angivelse af, om aftalen er udført.
        /// </summary>
        public virtual bool Udført
        {
            get
            {
                return (Properties & DoneValue) != 0;
            }
            set
            {
                if (value && Alarm)
                {
                    Alarm = false;
                }
                Properties = (Convert.ToInt32(Offentligtgørelse)*PublicValue) + (Convert.ToInt32(Privat)*PrivateValue) +
                             (Convert.ToInt32(Alarm)*BellValue) + (Convert.ToInt32(value)*DoneValue) +
                             (Convert.ToInt32(Eksporter)*ExportValue) + (Convert.ToInt32(Eksporteret)*ExportedValue);
            }
        }

        /// <summary>
        /// Angivelse af, at aftalen skal eksporteres.
        /// </summary>
        public virtual bool Eksporter
        {
            get
            {
                return (Properties & ExportValue) != 0;
            }
            set
            {
                if (value && Eksporteret)
                {
                    Eksporteret = false;
                }
                Properties = (Convert.ToInt32(Offentligtgørelse)*PublicValue) + (Convert.ToInt32(Privat)*PrivateValue) +
                             (Convert.ToInt32(Alarm)*BellValue) + (Convert.ToInt32(Udført)*DoneValue) +
                             (Convert.ToInt32(value)*ExportValue) + (Convert.ToInt32(Eksporteret)*ExportedValue);
            }
        }

        /// <summary>
        /// Angivelse af, at aftalen er eksporteret.
        /// </summary>
        public virtual bool Eksporteret
        {
            get
            {
                return (Properties & ExportedValue) != 0;
            }
            set
            {
                if (value && Eksporter)
                {
                    Eksporter = false;
                }
                Properties = (Convert.ToInt32(Offentligtgørelse)*PublicValue) + (Convert.ToInt32(Privat)*PrivateValue) +
                             (Convert.ToInt32(Alarm)*BellValue) + (Convert.ToInt32(Udført)*DoneValue) +
                             (Convert.ToInt32(Eksporter)*ExportValue) + (Convert.ToInt32(value)*ExportedValue);
            }
        }

        #endregion
    }
}
