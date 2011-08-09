using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Finansstyring
{
    /// <summary>
    /// Interface til en bogføringsadvarsel.
    /// </summary>
    public interface IBogføringsadvarsel
    {
        /// <summary>
        /// Tekst for advarsel.
        /// </summary>
        string Advarsel
        {
            get;
        }

        /// <summary>
        /// Konto, hvorpå advarslen er opstået.
        /// </summary>
        KontoBase Konto
        {
            get;
        }

        /// <summary>
        /// Beløb for advarslen, eksempelvis beløbet, som kontoen er overtrukket med.
        /// </summary>
        decimal Beløb
        {
            get;
        }
    }
}
