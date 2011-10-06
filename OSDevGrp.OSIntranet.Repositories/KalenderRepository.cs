using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.Domain.Interfaces.Kalender;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;

namespace OSDevGrp.OSIntranet.Repositories
{
    /// <summary>
    /// Repository til kalenderaftaler under OSWEBDB.
    /// </summary>
    public class KalenderRepository : IKalenderRepository
    {
        #region Private variables

        private readonly IMySqlDataProvider _mySqlDataProvider;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner repository til kalenderaftaler under OSWEBDB.
        /// </summary>
        /// <param name="mySqlDataProvider">Data provider til MySql.</param>
        public KalenderRepository(IMySqlDataProvider mySqlDataProvider)
        {
            if (mySqlDataProvider == null)
            {
                throw new ArgumentNullException("mySqlDataProvider");
            }
            _mySqlDataProvider = mySqlDataProvider;
        }

        #endregion

        #region IKalenderRepository Members

        /// <summary>
        /// Henter alle kalenderbrugere til et system under OSWEBDB.
        /// </summary>
        /// <param name="system">Unik identifikation af systemet under OSWEBDB.</param>
        /// <returns>Liste indeholdende kalenderbrugere til systemet.</returns>
        public IEnumerable<IBruger> BrugerGetAllBySystem(int system)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
