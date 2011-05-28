using System;
using System.Collections.Specialized;
using OSDevGrp.OSIntranet.CommonLibrary.Repositories;
using OSDevGrp.OSIntranet.CommonLibrary.Repositories.Interface.Exceptions;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories
{
    /// <summary>
    /// Konfigurationsrepository.
    /// </summary>
    public class KonfigurationRepository : KonfigurationRepositoryBase, IKonfigurationRepository
    {
        #region Constructor

        /// <summary>
        /// Danner konfigurationsrepository.
        /// </summary>
        /// <param name="nameValueCollection">Collection af navne og værdier.</param>
        public KonfigurationRepository(NameValueCollection nameValueCollection)
        {
            if (nameValueCollection == null)
            {
                throw new ArgumentNullException("nameValueCollection");
            }
            try
            {
                DebitorSaldoOverNul = base.GetBoolFromApplicationSettings(nameValueCollection, "DebitorSaldoOverNul");
                KreditorSaldoOverNul = base.GetBoolFromApplicationSettings(nameValueCollection, "KreditorSaldoOverNul");
                DageForBogføringsperiode = base.GetIntFromApplicationSettings(nameValueCollection, "DageForBogføringsperiode");
            }
            catch (CommonRepositoryException ex)
            {
                throw new IntranetRepositoryException(ex.Message, ex);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Angivelse af, om saldo for debitorer skal være større end 0.
        /// </summary>
        public bool DebitorSaldoOverNul
        {
            get;
            private set;
        }

        /// <summary>
        /// Angielse af, om saldo for kreditorer skal være større end 0.
        /// </summary>
        public bool KreditorSaldoOverNul
        {
            get;
            private set;
        }

        /// <summary>
        /// Angivelse af antal dage for bogføringsperiode.
        /// </summary>
        public int DageForBogføringsperiode
        {
            get;
            private set;
        }

        #endregion
    }
}
