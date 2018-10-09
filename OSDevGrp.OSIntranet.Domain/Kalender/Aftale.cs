using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.Domain.Comparers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Fælles;
using OSDevGrp.OSIntranet.Domain.Interfaces.Kalender;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Guards;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.Domain.Kalender
{
    /// <summary>
    /// Kalenderaftale.
    /// </summary>
    public class Aftale : AftaleBase, IAftale
    {
        #region Private variables

        private ISystem _system;
        private int _id;
        private DateTime _fraTidspunkt;
        private DateTime _tilTidspunkt;
        private string _emne;
        private IList<IBrugeraftale> _deltagere = new List<IBrugeraftale>();

        #endregion

        #region Constructor

        /// <summary>
        /// Danner en kalenderaftale.
        /// </summary>
        /// <param name="system">System under OSWEBDB, som aftalen er tilknyttet.</param>
        /// <param name="id">Unik identifikation for aftalen under systemet.</param>
        /// <param name="fraTidspunkt">Fra dato og tidspunkt.</param>
        /// <param name="tilTidspunkt">Til dato og tidspunkt.</param>
        /// <param name="emne">Emne.</param>
        /// <param name="properties">Værdi for aftalens properties.</param>
        public Aftale(ISystem system, int id, DateTime fraTidspunkt, DateTime tilTidspunkt, string emne, int properties = 0)
            : base(properties)
        {
            ArgumentNullGuard.NotNull(system, nameof(system))
                .NotNullOrWhiteSpace(emne, nameof(emne));

            if (fraTidspunkt.CompareTo(tilTidspunkt) >= 0)
            {
                throw new IntranetSystemException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, fraTidspunkt, "fraTidspunkt"));
            }

            _system = system;
            _id = id;
            _fraTidspunkt = fraTidspunkt;
            _tilTidspunkt = tilTidspunkt;
            _emne = emne;
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
        /// Unik identifikation for aftalen under systemet.
        /// </summary>
        public virtual int Id
        {
            get => _id;
            protected set => _id = value;
        }

        /// <summary>
        /// Fra dato og tidspunkt.
        /// </summary>
        public virtual DateTime FraTidspunkt
        {
            get => _fraTidspunkt;
            set
            {
                if (value.CompareTo(TilTidspunkt) >= 0)
                {
                    throw new IntranetSystemException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, value, "value"));
                }

                _fraTidspunkt = value;
            }
        }

        /// <summary>
        /// Til dato og tidspunkt.
        /// </summary>
        public virtual DateTime TilTidspunkt
        {
            get => _tilTidspunkt;
            set
            {
                if (value.CompareTo(FraTidspunkt) <= 0)
                {
                    throw new IntranetSystemException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, value, "value"));
                }

                _tilTidspunkt = value;
            }
        }

        /// <summary>
        /// Emne.
        /// </summary>
        public virtual string Emne
        {
            get => _emne;
            set
            {
                ArgumentNullGuard.NotNullOrWhiteSpace(value, nameof(value));

                _emne = value;
            }
        }

        /// <summary>
        /// Notat.
        /// </summary>
        public virtual string Notat
        {
            get;
            set;
        }

        /// <summary>
        /// Aftalens deltagere.
        /// </summary>
        public virtual IEnumerable<IBrugeraftale> Deltagere
        {
            get
            {
                var comparer = new BrugeraftaleComparer(new KalenderbrugerComparer());
                return _deltagere.OrderBy(m => m, comparer).ToList();
            }
            protected set
            {
                ArgumentNullGuard.NotNull(value, nameof(value));

                _deltagere = new List<IBrugeraftale>(value);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Tilføjer en deltager til aftalen.
        /// </summary>
        /// <param name="deltager">Brugeraftale for deltageren.</param>
        public virtual void TilføjDeltager(IBrugeraftale deltager)
        {
            ArgumentNullGuard.NotNull(deltager, nameof(deltager));

            if (deltager.System.Nummer != System.Nummer)
            {
                throw new IntranetSystemException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, deltager.System.Nummer, "deltager.System.Nummer"));
            }
            if (deltager.Aftale.System.Nummer != System.Nummer)
            {
                throw new IntranetSystemException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, deltager.Aftale.System.Nummer, "deltager.Aftale.System.Nummer"));
            }
            if (deltager.Aftale.Id != Id)
            {
                throw new IntranetSystemException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, deltager.Aftale.Id, "deltager.Aftale.Id"));
            }
            if (deltager.Bruger.System.Nummer != System.Nummer)
            {
                throw new IntranetSystemException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, deltager.Bruger.System.Nummer, "deltager.Bruger.System.Nummer"));
            }
            if (Deltagere.SingleOrDefault(m => m.Bruger.System.Nummer == deltager.Bruger.System.Nummer && m.Bruger.Id == deltager.Bruger.Id) != null)
            {
                throw new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.UserAppointmentAlreadyExists));
            }

            _deltagere.Add(deltager);
        }

        /// <summary>
        /// Fjerner en deltager fra aftalen.
        /// </summary>
        /// <param name="deltager">Brugeraftale for deltageren.</param>
        public virtual void FjernDeltager(IBrugeraftale deltager)
        {
            ArgumentNullGuard.NotNull(deltager, nameof(deltager));

            if (!_deltagere.Contains(deltager))
            {
                throw new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.UserAppointmentDontExists));
            }

            _deltagere.Remove(deltager);
        }

        #endregion
    }
}
