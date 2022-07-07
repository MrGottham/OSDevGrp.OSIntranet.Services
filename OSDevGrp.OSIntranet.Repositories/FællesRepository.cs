using System;
using System.Collections.Generic;
using System.Reflection;
using MySql.Data.MySqlClient;
using OSDevGrp.OSIntranet.Domain.Interfaces.Fælles;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Guards;
using OSDevGrp.OSIntranet.Repositories.DataProxies.Fælles;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.Repositories
{
    /// <summary>
    /// Repository til fælles elementer i domænet.
    /// </summary>
    public class FællesRepository : IFællesRepository
    {
        #region Private variables

        private readonly IMySqlDataProvider _mySqlDataProvider;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner repository til fælles elementer i domænet.
        /// </summary>
        /// <param name="mySqlDataProvider">Implementering af data provider til MySql.</param>
        public FællesRepository(IMySqlDataProvider mySqlDataProvider)
        {
            ArgumentNullGuard.NotNull(mySqlDataProvider, nameof(mySqlDataProvider));

            _mySqlDataProvider = mySqlDataProvider;
        }

        #endregion

        #region IFællesRepository Members

        /// <summary>
        /// Henter alle systemer under OSWEBDB.
        /// </summary>
        /// <returns>Liste af systemer under OSWEBDB.</returns>
        public IEnumerable<ISystem> SystemGetAll()
        {
            try
            {
                MySqlCommand command = new CommonCommandBuilder("SELECT SystemNo,Title,Properties FROM Systems ORDER BY SystemNo").Build();
                return _mySqlDataProvider.GetCollection<SystemProxy>(command);
            }
            catch (IntranetRepositoryException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, MethodBase.GetCurrentMethod().Name, ex.Message), ex);
            }
        }

        #endregion
    }
}