using System;
using System.Collections.Generic;
using System.Reflection;
using MySql.Data.MySqlClient;
using OSDevGrp.OSIntranet.Domain.Interfaces.Kalender;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.DataProxies.Kalender;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Resources;
using MySqlCommandBuilder = OSDevGrp.OSIntranet.Repositories.DataProxies.MySqlCommandBuilder;

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
        /// Henter alle kalenderaftaler fra en given dato til et system under OSWEBDB.
        /// </summary>
        /// <param name="system">Unik identifikation af systemet under OSWEBDB.</param>
        /// <param name="fromDate">Datoen, hvorfra kalenderaftaler skal hentes.</param>
        /// <returns>Liste indeholdende kalenderaftaler til systemer.</returns>
        public IEnumerable<IAftale> AftaleGetAllBySystem(int system, DateTime fromDate)
        {
            try
            {
                MySqlCommand command = new MySqlCommandBuilder(string.Format("SELECT SystemNo,CalId,Date,FromTime,ToTime,Properties,Subject,Note FROM Calapps FORCE INDEX (IX_Calapps_SystemNo_Date) WHERE SystemNo={0} AND Date>='{1}' ORDER BY Date DESC,FromTime DESC,ToTime DESC,CalId DESC", system, fromDate.ToString("yyyy-MM-dd"))).Build();
                return _mySqlDataProvider.GetCollection<AftaleProxy>(command);
            }
            catch (IntranetRepositoryException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new IntranetRepositoryException(
                    Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, MethodBase.GetCurrentMethod().Name,
                                                 ex.Message), ex);
            }
        }

        /// <summary>
        /// Henter en given kalenderaftale fra et system under OSWEBDB.
        /// </summary>
        /// <param name="system">Unik identifikation af systemet under OSWEBDB.</param>
        /// <param name="id">Unik identifikation af kalenderaftalen.</param>
        /// <returns>Kalenderaftale.</returns>
        public IAftale AftaleGetBySystemAndId(int system, int id)
        {
            try
            {
                var queryForAftale = new AftaleProxy(system, id);
                return _mySqlDataProvider.Get(queryForAftale);
            }
            catch (IntranetRepositoryException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new IntranetRepositoryException(
                    Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, MethodBase.GetCurrentMethod().Name,
                                                 ex.Message), ex);
            }
        }

        /// <summary>
        /// Henter alle kalenderbrugere til et system under OSWEBDB.
        /// </summary>
        /// <param name="system">Unik identifikation af systemet under OSWEBDB.</param>
        /// <returns>Liste indeholdende kalenderbrugere til systemet.</returns>
        public IEnumerable<IBruger> BrugerGetAllBySystem(int system)
        {
            try
            {
                MySqlCommand command = new MySqlCommandBuilder(string.Format("SELECT SystemNo,UserId,UserName,Name,Initials FROM Calusers WHERE SystemNo={0} ORDER BY Name,Initials,UserId", system)).Build();
                return _mySqlDataProvider.GetCollection<BrugerProxy>(command);
            }
            catch (IntranetRepositoryException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new IntranetRepositoryException(
                    Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, MethodBase.GetCurrentMethod().Name,
                                                 ex.Message), ex);
            }
        }

        #endregion
    }
}
