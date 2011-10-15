using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.Domain.Interfaces.Kalender;

namespace OSDevGrp.OSIntranet.Domain.Comparers
{
    /// <summary>
    /// Klasse til sammenligning af deltagere på kalenderaftaler.
    /// </summary>
    public class BrugeraftaleComparer : IComparer<IBrugeraftale>
    {
        #region Private variables

        private readonly IComparer<IBruger> _brugerComparer;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner klasse til sammenligning af deltagere på kalenderaftaler.
        /// </summary>
        /// <param name="brugerComparer">Klasse til sammenligning af kalenderbrugere.</param>
        public BrugeraftaleComparer(IComparer<IBruger> brugerComparer)
        {
            if (brugerComparer == null)
            {
                throw new ArgumentNullException("brugerComparer");
            }
            _brugerComparer = brugerComparer;
        }

        #endregion

        #region IComparer<Brugeraftale> Members

        /// <summary>
        /// Sammenligning af deltagere på kalenderaftaler. 
        /// </summary>
        /// <param name="x">Deltager på kalenderaftale.</param>
        /// <param name="y">Deltager på kalenderaftale.</param>
        /// <returns>Resultat af sammenligning.</returns>
        public int Compare(IBrugeraftale x, IBrugeraftale y)
        {
            if (x == null)
            {
                throw new ArgumentNullException("x");
            }
            if (y == null)
            {
                throw new ArgumentNullException("y");
            }
            return _brugerComparer.Compare(x.Bruger, y.Bruger);
        }

        #endregion
    }
}
