using System;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.Domain.Interfaces.Finansstyring;

namespace OSDevGrp.OSIntranet.Domain.Finansstyring
{
    /// <summary>
    /// Bogføringsadvarsel.
    /// </summary>
    public class Bogføringsadvarsel : IBogføringsadvarsel
    {
        #region Private variables

        private readonly string _advarsel;
        private readonly KontoBase _konto;
        private readonly decimal _beløb;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner en bogføringsadvarsel.
        /// </summary>
        /// <param name="advarsel">Tekst for advarsel.</param>
        /// <param name="konto">Konto, hvorpå advarslen er opstået.</param>
        /// <param name="beløb">Beløb for advarslen, eksempelvis beløbet, som kontoen er overtrukket med.</param>
        public Bogføringsadvarsel(string advarsel, KontoBase konto, decimal beløb)
        {
            if (string.IsNullOrEmpty(advarsel))
            {
                throw new ArgumentNullException("advarsel");
            }
            if (konto == null)
            {
                throw new ArgumentNullException("konto");
            }
            _advarsel = advarsel;
            _konto = konto;
            _beløb = beløb;
        }

        #endregion

        #region IBogføringsadvarsel Members

        /// <summary>
        /// Tekst for advarsel.
        /// </summary>
        public virtual string Advarsel
        {
            get
            {
                return _advarsel;
            }
        }

        /// <summary>
        /// Konto, hvorpå advarslen er opstået.
        /// </summary>
        public virtual KontoBase Konto
        {
            get
            {
                return _konto;
            }
        }

        /// <summary>
        /// Beløb for advarslen, eksempelvis beløbet, som kontoen er overtrukket med.
        /// </summary>
        public virtual decimal Beløb
        {
            get
            {
                return _beløb;
            }
        }

        #endregion
    }
}
