using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.Domain.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.Domain.Finansstyring
{
    /// <summary>
    /// Bogføringsresultat.
    /// </summary>
    public class Bogføringsresultat : IBogføringsresultat
    {
        #region Private variables

        private readonly Bogføringslinje _bogføringslinje;
        private readonly List<IBogføringsadvarsel> _advarsler = new List<IBogføringsadvarsel>();

        #endregion

        #region Constructor

        /// <summary>
        /// Danner et bogføringsresultat for en bogføringslinje.
        /// </summary>
        /// <param name="bogføringslinje">Bogførignslinje.</param>
        public Bogføringsresultat(Bogføringslinje bogføringslinje)
        {
            if (bogføringslinje == null)
            {
                throw new ArgumentNullException("bogføringslinje");
            }
            // Initiering og validering af bogføringslinje.
            _bogføringslinje = bogføringslinje;
            if (_bogføringslinje.Konto == null)
            {
                throw new IntranetSystemException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, _bogføringslinje.Konto, "Konto"));
            }
            // Dan eventuelle bogføringsadvarsler.
            _bogføringslinje.Konto.Calculate(_bogføringslinje.Dato, _bogføringslinje.Løbenummer);
            if (_bogføringslinje.Konto.DisponibelPrStatusdato <= 0M)
            {
                _advarsler.Add(new Bogføringsadvarsel(Resource.GetExceptionMessage(ExceptionMessage.AccountIsOverdrawn), _bogføringslinje.Konto, Math.Abs(_bogføringslinje.Konto.DisponibelPrStatusdato)));
            }
            if (_bogføringslinje.Budgetkonto == null)
            {
                return;
            }
            _bogføringslinje.Budgetkonto.Calculate(_bogføringslinje.Dato, _bogføringslinje.Løbenummer);
            if (_bogføringslinje.Budgetkonto.BudgetPrStatusdato <= 0M && _bogføringslinje.Budgetkonto.DisponibelPrStatusdato <= 0M)
            {
                _advarsler.Add(new Bogføringsadvarsel(Resource.GetExceptionMessage(ExceptionMessage.BudgetAccountIsOverdrawn), _bogføringslinje.Budgetkonto, Math.Abs(_bogføringslinje.Budgetkonto.DisponibelPrStatusdato)));
            }
        }

        #endregion

        #region IBogføringsresultat Members

        /// <summary>
        /// Bogføringslinjen, der medfører resultatet.
        /// </summary>
        public virtual Bogføringslinje Bogføringslinje
        {
            get
            {
                return _bogføringslinje;
            }
        }

        /// <summary>
        /// Bogføringsadvarsler.
        /// </summary>
        public virtual IEnumerable<IBogføringsadvarsel> Advarsler
        {
            get
            {
                return _advarsler;
            }
        }

        #endregion
    }
}
