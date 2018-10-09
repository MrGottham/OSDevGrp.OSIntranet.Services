using OSDevGrp.OSIntranet.Domain.Interfaces.Fælles;
using OSDevGrp.OSIntranet.Domain.Interfaces.Kalender;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Guards;

namespace OSDevGrp.OSIntranet.Domain.Kalender
{
    /// <summary>
    /// Kalenderbruger.
    /// </summary>
    public class Bruger : IBruger
    {
        #region Private variables

        private ISystem _system;
        private int _id;
        private string _initialer;
        private string _navn;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner en kalenderbruger.
        /// </summary>
        /// <param name="system">System under OSWEBDB, som brugeren er tilknyttet.</param>
        /// <param name="id">Unik identifikation for brugeren under systemet.</param>
        /// <param name="initialer">Initialer på brugeren.</param>
        /// <param name="navn">Navn på brugeren.</param>
        public Bruger(ISystem system, int id, string initialer, string navn)
        {
            ArgumentNullGuard.NotNull(system, nameof(system))
                .NotNullOrWhiteSpace(initialer, nameof(initialer))
                .NotNullOrWhiteSpace(navn, nameof(navn));

            _system = system;
            _id = id;
            _initialer = initialer;
            _navn = navn;
        }

        #endregion

        #region Properties

        /// <summary>
        /// System under OSWEBDB, som brugeren er tilknyttet.
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
        /// Unik identifikation for brugeren under systemet.
        /// </summary>
        public virtual int Id
        {
            get => _id;
            protected set => _id = value;
        }

        /// <summary>
        /// Initialer på brugeren.
        /// </summary>
        public virtual string Initialer
        {
            get => _initialer;
            set
            {
                ArgumentNullGuard.NotNullOrWhiteSpace(value, nameof(value));

                _initialer = value;
            }
        }

        /// <summary>
        /// Navn på brugeren.
        /// </summary>
        public virtual string Navn
        {
            get => _navn;
            set
            {
                ArgumentNullGuard.NotNullOrWhiteSpace(value, nameof(value));

                _navn = value;
            }
        }

        /// <summary>
        /// Brugernavn, som der logges på med under OSWEBDB.
        /// </summary>
        public virtual string UserName
        {
            get;
            set;
        }

        #endregion
    }
}
