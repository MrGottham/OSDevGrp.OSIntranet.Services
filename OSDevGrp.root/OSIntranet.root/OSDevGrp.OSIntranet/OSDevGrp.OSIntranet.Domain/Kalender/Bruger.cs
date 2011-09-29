using System;
using OSDevGrp.OSIntranet.Domain.Interfaces.Fælles;
using OSDevGrp.OSIntranet.Domain.Interfaces.Kalender;

namespace OSDevGrp.OSIntranet.Domain.Kalender
{
    /// <summary>
    /// Kalenderbruger.
    /// </summary>
    public class Bruger : IBruger
    {
        #region Private variables

        private readonly ISystem _system;
        private readonly int _id;
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
            if (system == null)
            {
                throw new ArgumentNullException("system");
            }
            if (string.IsNullOrEmpty(initialer))
            {
                throw new ArgumentNullException("initialer");
            }
            if (string.IsNullOrEmpty(navn))
            {
                throw new ArgumentNullException("navn");
            }
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
            get
            {
                return _system;
            }
        }

        /// <summary>
        /// Unik identifikation for brugeren under systemet.
        /// </summary>
        public virtual int Id
        {
            get
            {
                return _id;
            }
        }

        /// <summary>
        /// Initialer på brugeren.
        /// </summary>
        public virtual string Initialer
        {
            get
            {
                return _initialer;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("value");
                }
                _initialer = value;
            }
        }

        /// <summary>
        /// Navn på brugeren.
        /// </summary>
        public virtual string Navn
        {
            get
            {
                return _navn;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("value");
                }
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
