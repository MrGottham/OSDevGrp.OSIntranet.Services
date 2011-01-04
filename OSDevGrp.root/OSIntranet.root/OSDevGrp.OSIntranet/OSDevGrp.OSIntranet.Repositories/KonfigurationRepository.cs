using System;
using System.Collections.Specialized;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.Repositories
{
    /// <summary>
    /// Konfigurationsrepository.
    /// </summary>
    public class KonfigurationRepository : IKonfigurationRepository
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
            var value = nameValueCollection["DebitorSaldoOverNul"];
            if (string.IsNullOrEmpty(value))
            {
                throw new IntranetRepositoryException(
                    Resource.GetExceptionMessage(ExceptionMessage.MissingApplicationSetting, "DebitorSaldoOverNul"));
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Angivelse af, om saldo for debitorer skal være større end 0.
        /// </summary>
        public bool DebitorSaldoOverNul
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Angielse af, om saldo for kreditorer skal være større end 0.
        /// </summary>
        public bool KreditorSaldoOverNul
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
