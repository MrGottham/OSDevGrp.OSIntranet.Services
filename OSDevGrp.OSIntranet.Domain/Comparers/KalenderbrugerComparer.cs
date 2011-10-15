using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.Domain.Interfaces.Kalender;

namespace OSDevGrp.OSIntranet.Domain.Comparers
{
    /// <summary>
    /// Klasse til sammenligning af kalenderbrugere.
    /// </summary>
    public class KalenderbrugerComparer : IComparer<IBruger>
    {
        #region IComparer<IBruger> Members

        /// <summary>
        /// Sammenligning af kalenderbrugere.
        /// </summary>
        /// <param name="x">Kalenderbruger.</param>
        /// <param name="y">Kalenderbruger.</param>
        /// <returns>Resultat af sammenligning.</returns>
        public int Compare(IBruger x, IBruger y)
        {
            if (x == null)
            {
                throw new ArgumentNullException("x");
            }
            if (y == null)
            {
                throw new ArgumentNullException("y");
            }
            var value = x.Navn == null ? 0 : x.Navn.CompareTo(y.Navn);
            if (value != 0)
            {
                return value;
            }
            value = x.Initialer == null ? 0 : x.Initialer.CompareTo(y.Initialer);
            return value != 0 ? value : x.Id.CompareTo(y.Id);
        }

        #endregion
    }
}
