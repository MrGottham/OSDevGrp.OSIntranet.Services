﻿using System;
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
            try
            {
                DebitorSaldoOverNul = bool.Parse(value);
            }
            catch (Exception ex)
            {
                throw new IntranetRepositoryException(
                    Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, value, "DebitorSaldoOverNul"), ex);
            }
            value = nameValueCollection["KreditorSaldoOverNul"];
            if (string.IsNullOrEmpty(value))
            {
                throw new IntranetRepositoryException(
                    Resource.GetExceptionMessage(ExceptionMessage.MissingApplicationSetting, "KreditorSaldoOverNul"));
            }
            try
            {
                KreditorSaldoOverNul = bool.Parse(value);
            }
            catch (Exception ex)
            {
                throw new IntranetRepositoryException(
                    Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, value, "KreditorSaldoOverNul"), ex);
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

        #endregion
    }
}
