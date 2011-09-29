using System;
using OSDevGrp.OSIntranet.Domain.Interfaces.Fælles;
using OSDevGrp.OSIntranet.Domain.Interfaces.Kalender;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.Domain.Kalender
{
    /// <summary>
    /// Kalenderaftale.
    /// </summary>
    public class Aftale : IAftale
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

        private readonly ISystem _system;
        private readonly int _id;
        private DateTime _fraTidspunkt;
        private DateTime _tilTidspunkt;
        private string _emne;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner en kalenderaftale.
        /// </summary>
        /// <param name="system">System under OSWEBDB, som aftalen er tilknyttet.</param>
        /// <param name="id">Unik identifikation for aftalen under systemet.</param>
        /// <param name="fraTidspunkt">Fra dato og tidspunkt.</param>
        /// <param name="tilTidspunkt">Til dato og tidspunkt.</param>
        /// <param name="emne">Emne.</param>
        public Aftale(ISystem system, int id, DateTime fraTidspunkt, DateTime tilTidspunkt, string emne)
        {
            if (system == null)
            {
                throw new ArgumentNullException("system");
            }
            if (fraTidspunkt.CompareTo(tilTidspunkt) >= 0)
            {
                throw new IntranetSystemException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue,
                                                                               fraTidspunkt, "fraTidspunkt"));
            }
            if (string.IsNullOrEmpty(emne))
            {
                throw new ArgumentNullException("emne");
            }
            _system = system;
            _id = id;
            _fraTidspunkt = fraTidspunkt;
            _tilTidspunkt = tilTidspunkt;
            _emne = emne;
        }

        #endregion

        #region Properties

        /// <summary>
        /// System under OSWEBDB, som aftalen er tilknyttet.
        /// </summary>
        public virtual ISystem System
        {
            get
            {
                return _system;
            }
        }

        /// <summary>
        /// Unik identifikation for aftalen under systemet.
        /// </summary>
        public virtual int Id
        {
            get
            {
                return _id;
            }
        }

        /// <summary>
        /// Fra dato og tidspunkt.
        /// </summary>
        public virtual DateTime FraTidspunkt
        {
            get
            {
                return _fraTidspunkt;
            }
            set
            {
                if (value.CompareTo(TilTidspunkt) >= 0)
                {
                    throw new IntranetSystemException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, value,
                                                                                   "value"));
                }
                _fraTidspunkt = value;
            }
        }

        /// <summary>
        /// Til dato og tidspunkt.
        /// </summary>
        public virtual DateTime TilTidspunkt
        {
            get
            {
                return _tilTidspunkt;
            }
            set
            {
                if (value.CompareTo(FraTidspunkt) <= 0)
                {
                    throw new IntranetSystemException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, value,
                                                                                   "value"));
                }
                _tilTidspunkt = value;
            }
        }

        /// <summary>
        /// Emne.
        /// </summary>
        public virtual string Emne
        {
            get
            {
                return _emne;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("value");
                }
                _emne = value;
            }
        }

        /// <summary>
        /// Notat.
        /// </summary>
        public virtual string Notat
        {
            get;
            set;
        }

        /// <summary>
        /// Egenskaber for aftalen.
        /// </summary>
        public virtual int Properties
        {
            get;
            protected set;
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
                // TODO: If (value) Udført = false;
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
                // TODO: If (value) Alarm = false;
                Properties = (Convert.ToInt32(Offentligtgørelse) * PublicValue) + (Convert.ToInt32(Privat) * PrivateValue) +
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
                // TODO: If (value) Eksporteret = false;
                Properties = (Convert.ToInt32(Offentligtgørelse) * PublicValue) + (Convert.ToInt32(Privat) * PrivateValue) +
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
                // TODO: If (value) Eksporter = false;
                Properties = (Convert.ToInt32(Offentligtgørelse) * PublicValue) + (Convert.ToInt32(Privat) * PrivateValue) +
                             (Convert.ToInt32(Alarm)*BellValue) + (Convert.ToInt32(Udført)*DoneValue) +
                             (Convert.ToInt32(Eksporter)*ExportValue) + (Convert.ToInt32(value)*ExportedValue);
            }
        }

        #endregion
    }
}
