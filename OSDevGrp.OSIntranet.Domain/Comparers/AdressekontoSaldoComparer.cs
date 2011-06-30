using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Comparers;

namespace OSDevGrp.OSIntranet.Domain.Comparers
{
    /// <summary>
    /// Klasse til sammenligning af adressekonti.
    /// </summary>
    public class AdressekontoSaldoComparer : IComparer<AdresseBase>
    {
        #region Private variables

        private readonly AdresseComparer _adresseComparer = new AdresseComparer();

        #endregion

        #region IComparer<AdresseBase> Members

        /// <summary>
        /// Sammenligning af adressekonti.
        /// </summary>
        public int Compare(AdresseBase x, AdresseBase y)
        {
            if (x == null)
            {
                throw new ArgumentNullException("x");
            }
            if (y == null)
            {
                throw new ArgumentNullException("y");
            }
            if (x.SaldoPrStatusdato == y.SaldoPrStatusdato)
            {
                return _adresseComparer.Compare(x, y);
            }
            return x.SaldoPrStatusdato.CompareTo(y.SaldoPrStatusdato);
        }

        #endregion
    }
}
