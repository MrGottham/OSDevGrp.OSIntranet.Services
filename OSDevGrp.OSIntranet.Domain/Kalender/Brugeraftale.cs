using OSDevGrp.OSIntranet.Domain.Interfaces.Fælles;
using OSDevGrp.OSIntranet.Domain.Interfaces.Kalender;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Guards;

namespace OSDevGrp.OSIntranet.Domain.Kalender
{
    /// <summary>
    /// Kalenderaftale for en bruger.
    /// </summary>
    public class Brugeraftale : AftaleBase, IBrugeraftale
    {
        #region Private variables

        private ISystem _system;
        private IAftale _aftale;
        private IBruger _bruger;

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
            ArgumentNullGuard.NotNull(system, nameof(system))
                .NotNull(aftale, nameof(aftale))
                .NotNull(bruger, nameof(bruger));

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
            get => _system;
            protected set
            {
                ArgumentNullGuard.NotNull(value, nameof(value));

                _system = value;
            }
        }

        /// <summary>
        /// Aftale.
        /// </summary>
        public virtual IAftale Aftale
        {
            get => _aftale;
            protected set
            {
                ArgumentNullGuard.NotNull(value, nameof(value));

                _aftale = value;
            }
        }

        /// <summary>
        /// Bruger.
        /// </summary>
        public virtual IBruger Bruger
        {
            get => _bruger;
            protected set
            {
                ArgumentNullGuard.NotNull(value, nameof(value));

                _bruger = value;
            }
        }

        #endregion
    }
}
