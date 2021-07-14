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