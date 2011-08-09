using System.Collections.Generic;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Finansstyring
{
    /// <summary>
    /// Interface til et bogføringsresultat.
    /// </summary>
    public interface IBogføringsresultat
    {
        /// <summary>
        /// Bogføringslinjen, der medfører resultatet.
        /// </summary>
        Bogføringslinje Bogføringslinje
        {
            get;
        }

        /// <summary>
        /// Bogføringsadvarsler.
        /// </summary>
        IEnumerable<IBogføringsadvarsel> Advarsler
        {
            get;
        }
    }
}
