using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.Domain.Interfaces.Fælles;
using OSDevGrp.OSIntranet.Domain.Interfaces.Kalender;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.Domain.Kalender
{
    /// <summary>
    /// Kalenderaftale.
    /// </summary>
    public class Aftale : AftaleBase, IAftale
    {
        #region Private variables

        private readonly ISystem _system;
        private readonly int _id;
        private DateTime _fraTidspunkt;
        private DateTime _tilTidspunkt;
        private string _emne;
        private readonly IList<IBrugeraftale> _deltagere = new List<IBrugeraftale>();

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
            if (system == null)
            {
                throw new ArgumentNullException("system");
            }
            if (fraTidspunkt.CompareTo(tilTidspunkt) >= 0)
            {
                throw new IntranetSystemException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue,
                                                                               fraTidspunkt, "fraTidspunkt"));
            }
            if (string.IsNullOrEmpty(emne))
            {
                throw new ArgumentNullException("emne");
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
            get
            {
                return _system;
            }
        }

        /// <summary>
        /// Unik identifikation for aftalen under systemet.
        /// </summary>
        public virtual int Id
        {
            get
            {
                return _id;
            }
        }

        /// <summary>
        /// Fra dato og tidspunkt.
        /// </summary>
        public virtual DateTime FraTidspunkt
        {
            get
            {
                return _fraTidspunkt;
            }
            set
            {
                if (value.CompareTo(TilTidspunkt) >= 0)
                {
                    throw new IntranetSystemException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, value,
                                                                                   "value"));
                }
                _fraTidspunkt = value;
            }
        }

        /// <summary>
        /// Til dato og tidspunkt.
        /// </summary>
        public virtual DateTime TilTidspunkt
        {
            get
            {
                return _tilTidspunkt;
            }
            set
            {
                if (value.CompareTo(FraTidspunkt) <= 0)
                {
                    throw new IntranetSystemException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, value,
                                                                                   "value"));
                }
                _tilTidspunkt = value;
            }
        }

        /// <summary>
        /// Emne.
        /// </summary>
        public virtual string Emne
        {
            get
            {
                return _emne;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("value");
                }
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
                return _deltagere;
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
            if (deltager == null)
            {
                throw new ArgumentNullException("deltager");
            }
            if (deltager.System.Nummer != System.Nummer)
            {
                throw new IntranetSystemException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue,
                                                                               deltager.System.Nummer,
                                                                               "deltager.System.Nummer"));
            }
            if (deltager.Aftale.System.Nummer != System.Nummer)
            {
                throw new IntranetSystemException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue,
                                                                               deltager.Aftale.System.Nummer,
                                                                               "deltager.Aftale.System.Nummer"));
            }
            if (deltager.Aftale.Id != Id)
            {
                throw new IntranetSystemException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue,
                                                                               deltager.Aftale.Id, "deltager.Aftale.Id"));
            }
            if (deltager.Bruger.System.Nummer != System.Nummer)
            {
                throw new IntranetSystemException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue,
                                                                               deltager.Bruger.System.Nummer,
                                                                               "deltager.Bruger.System.Nummer"));
            }
            if (Deltagere.SingleOrDefault(m => m.Bruger.System.Nummer == deltager.Bruger.System.Nummer && m.Bruger.Id == deltager.Bruger.Id) != null)
            {
                throw new IntranetBusinessException(
                    Resource.GetExceptionMessage(ExceptionMessage.UserAppointmentAlreadyExists));
            }
            _deltagere.Add(deltager);
        }

        /// <summary>
        /// Fjerner en deltager fra aftalen.
        /// </summary>
        /// <param name="deltager">Brugeraftale for deltageren.</param>
        public virtual void FjernDeltager(IBrugeraftale deltager)
        {
            if (deltager == null)
            {
                throw new ArgumentNullException("deltager");
            }
            if (!_deltagere.Contains(deltager))
            {
                throw new IntranetBusinessException(
                    Resource.GetExceptionMessage(ExceptionMessage.UserAppointmentDontExists));
            }
            _deltagere.Remove(deltager);
        }

        #endregion
    }
}
