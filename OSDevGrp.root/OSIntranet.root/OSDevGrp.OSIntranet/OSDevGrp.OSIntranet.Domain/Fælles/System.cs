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

        /// <summary>
        /// Danner system under OSWEBDB.
        /// </summary>
        /// <param name="nummer">Unik identifikation af systemet.</param>
        /// <param name="titel">Titel på systemet.</param>
        public System(int nummer, string titel)
        {
            _nummer = nummer;
            _titel = titel;
        }

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
                throw new global::System.NotImplementedException();
            }
        }

        #endregion
    }
}
