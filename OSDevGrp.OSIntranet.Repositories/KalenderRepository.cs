using System;
using System.Collections.Generic;
using System.Reflection;
using MySql.Data.MySqlClient;
using OSDevGrp.OSIntranet.Domain.Interfaces.Kalender;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Guards;
using OSDevGrp.OSIntranet.Repositories.DataProxies.Kalender;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Resources;

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
            ArgumentNullGuard.NotNull(mySqlDataProvider, nameof(mySqlDataProvider));

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
                MySqlCommand command = new CalenderCommandBuilder("SELECT ca.SystemNo,ca.CalId,ca.Date,ca.FromTime,ca.ToTime,ca.Properties,ca.Subject,ca.Note,s.Title AS SystemTitle,s.Properties AS SystemProperties FROM Calapps AS ca FORCE INDEX(IX_Calapps_SystemNo_Date) INNER JOIN Systems AS s ON s.SystemNo=ca.SystemNo WHERE ca.SystemNo=@systemNo AND ca.Date>=@date ORDER BY ca.Date DESC,ca.FromTime DESC,ca.ToTime DESC,ca.CalId DESC")
                    .AddSystemNoParameter(system)
                    .AddAppointmentDateParameter(fromDate)
                    .Build();
                return _mySqlDataProvider.GetCollection<AftaleProxy>(command);
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
                AftaleProxy queryForAftale = new AftaleProxy(system, id);
                return _mySqlDataProvider.Get(queryForAftale);
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

        /// <summary>
        /// Henter alle kalenderbrugere til et system under OSWEBDB.
        /// </summary>
        /// <param name="system">Unik identifikation af systemet under OSWEBDB.</param>
        /// <returns>Liste indeholdende kalenderbrugere til systemet.</returns>
        public IEnumerable<IBruger> BrugerGetAllBySystem(int system)
        {
            try
            {
                MySqlCommand command = new CalenderCommandBuilder("SELECT cu.SystemNo,cu.UserId,cu.UserName,cu.Name,cu.Initials,s.Title,s.Properties FROM Calusers AS cu INNER JOIN Systems AS s ON s.SystemNo=cu.SystemNo WHERE cu.SystemNo=@systemNo ORDER BY cu.Name,cu.Initials,cu.UserId")
                    .AddSystemNoParameter(system)
                    .Build();
                return _mySqlDataProvider.GetCollection<BrugerProxy>(command);
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
