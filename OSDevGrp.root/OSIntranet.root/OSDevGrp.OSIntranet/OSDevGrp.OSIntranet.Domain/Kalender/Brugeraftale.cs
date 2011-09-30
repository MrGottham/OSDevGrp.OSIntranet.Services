using System;
using OSDevGrp.OSIntranet.Domain.Interfaces.Fælles;
using OSDevGrp.OSIntranet.Domain.Interfaces.Kalender;

namespace OSDevGrp.OSIntranet.Domain.Kalender
{
    /// <summary>
    /// Kalenderaftale for en bruger.
    /// </summary>
    public class Brugeraftale : AftaleBase, IBrugeraftale
    {
        #region Private variables

        private readonly ISystem _system;
        private readonly IAftale _aftale;
        private readonly IBruger _bruger;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner en kalenderaftale for en bruger.
        /// </summary>
        /// <param name="system">System under OSWEBDB, som brugerens aftale er tilknyttet.</param>
        /// <param name="aftale">Aftale.</param>
        /// <param name="bruger">Bruger.</param>
        /// <param name="properties">Værdi for aftalens properties.</param>
        public Brugeraftale(ISystem system, IAftale aftale, IBruger bruger, int properties = 0) 
            : base(properties)
        {
            if (system == null)
            {
                throw new ArgumentNullException("system");
            }
            if (aftale == null)
            {
                throw new ArgumentNullException("aftale");
            }
            if (bruger == null)
            {
                throw new ArgumentNullException("bruger");
            }
            _system = system;
            _aftale = aftale;
            _bruger = bruger;
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
        /// Aftale.
        /// </summary>
        public virtual IAftale Aftale
        {
            get
            {
                return _aftale;
            }
        }

        /// <summary>
        /// Bruger.
        /// </summary>
        public virtual IBruger Bruger
        {
            get
            {
                return _bruger;
            }
        }

        #endregion
    }
}
