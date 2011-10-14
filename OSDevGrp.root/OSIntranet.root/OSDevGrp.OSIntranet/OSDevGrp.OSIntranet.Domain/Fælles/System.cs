using System;
using OSDevGrp.OSIntranet.Domain.Interfaces.Fælles;

namespace OSDevGrp.OSIntranet.Domain.Fælles
{
    /// <summary>
    /// System under OSWEBDB.
    /// </summary>
    public class System : ISystem
    {
        #region Private constants

        private const int CalenderValue = 8;

        #endregion

        #region Private variables

        private readonly int _nummer;
        private string _titel;
        private int _properties;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner et system under OSWEBDB.
        /// </summary>
        /// <param name="nummer">Unik identifikation af systemet.</param>
        /// <param name="titel">Titel på systemet.</param>
        /// <param name="properties">Egenskaber for systemet.</param>
        public System(int nummer, string titel, int properties = 0)
        {
            if (string.IsNullOrEmpty(titel))
            {
                throw new ArgumentNullException("titel");
            }
            _nummer = nummer;
            _titel = titel;
            _properties = properties;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Unik identifikation af systemet.
        /// </summary>
        public virtual int Nummer
        {
            get
            {
                return _nummer;
            }
        }

        /// <summary>
        /// Titel på systemet.
        /// </summary>
        public virtual string Titel
        {
            get
            {
                return _titel;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("value");
                }
                _titel = value;
            }
        }

        /// <summary>
        /// Egenskaber for systemet.
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
        /// Angivelse af, om systemet benytter kalenderen under OSWEBDB.
        /// </summary>
        public virtual bool Kalender
        {
            get
            {
                return (Properties & CalenderValue) != 0;
            }
            set
            {
                if (value)
                {
                    if (Kalender == false)
                    {
                        Properties += CalenderValue;
                    }
                }
                else if (Kalender)
                {
                    Properties -= CalenderValue;
                }
            }
        }

        #endregion
    }
}
