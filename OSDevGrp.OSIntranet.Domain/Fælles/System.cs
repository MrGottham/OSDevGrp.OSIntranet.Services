using System;
using OSDevGrp.OSIntranet.Domain.Interfaces.Fælles;

namespace OSDevGrp.OSIntranet.Domain.Fælles
{
    /// <summary>
    /// System under OSWEBDB.
    /// </summary>
    public class System : ISystem
    {
        #region Private variables

        private readonly int _nummer;
        private string _titel;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner system under OSWEBDB.
        /// </summary>
        /// <param name="nummer">Unik identifikation af systemet.</param>
        /// <param name="titel">Titel på systemet.</param>
        public System(int nummer, string titel)
        {
            if (string.IsNullOrEmpty(titel))
            {
                throw new ArgumentNullException("titel");
            }
            _nummer = nummer;
            _titel = titel;
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
                _titel = value;
            }
        }

        #endregion
    }
}
