using System;
using System.Data;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories
{
    /// <summary>
    /// Klasse til behandling af data i en record fra MySql.
    /// </summary>
    public class MySqlDataRecord : IMySqlDataRecord
    {
        #region Private variables

        private readonly IDataRecord _dataRecord;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner klasse til behandling af data i en record fra MySql.
        /// </summary>
        /// <param name="dataRecord">DataRecord fra MySql.</param>
        public MySqlDataRecord(IDataRecord dataRecord)
        {
            if (dataRecord == null)
            {
                throw new ArgumentNullException("dataRecord");
            }
            _dataRecord = dataRecord;
        }

        #endregion

        #region IMySqlDataRecord Members

        /// <summary>
        /// Henter værdi fra en given kolonne.
        /// </summary>
        /// <param name="columnNo">Index for kolonnen, hvorfra der skal hentes data.</param>
        /// <returns>Værdi.</returns>
        public short GetInt16(int columnNo)
        {
            return _dataRecord.GetInt16(columnNo);
        }

        /// <summary>
        /// Henter værdi fra en given kolonne.
        /// </summary>
        /// <param name="columnNo">Index for kolonnen, hvorfra der skal hentes data.</param>
        /// <returns>Værdi.</returns>
        public int GetInt32(int columnNo)
        {
            return _dataRecord.GetInt32(columnNo);
        }

        /// <summary>
        /// Henter værdi fra en given kolonne.
        /// </summary>
        /// <param name="columnNo">Index for kolonnen, hvorfra der skal hentes data.</param>
        /// <returns>Værdi.</returns>
        public long GetInt64(int columnNo)
        {
            return _dataRecord.GetInt64(columnNo);
        }

        /// <summary>
        /// Henter værdi fra en given kolonne.
        /// </summary>
        /// <param name="columnNo">Index for kolonnen, hvorfra der skal hentes data.</param>
        /// <returns>Værdi.</returns>
        public decimal GetDecimal(int columnNo)
        {
            return _dataRecord.GetDecimal(columnNo);
        }

        /// <summary>
        /// Henter værdi fra en given kolonne.
        /// </summary>
        /// <param name="columnNo">Index for kolonnen, hvorfra der skal hentes data.</param>
        /// <returns>Værdi.</returns>
        public bool GetBoolean(int columnNo)
        {
            return _dataRecord.GetBoolean(columnNo);
        }

        /// <summary>
        /// Henter værdi fra en given kolonne.
        /// </summary>
        /// <param name="columnNo">Index for kolonnen, hvorfra der skal hentes data.</param>
        /// <returns>Værdi.</returns>
        public string GetString(int columnNo)
        {
            return _dataRecord.GetString(columnNo);
        }

        /// <summary>
        /// Henter værdi fra en given kolonne.
        /// </summary>
        /// <param name="columnNo">Index for kolonnen, hvorfra der skal hentes data.</param>
        /// <returns>Værdi.</returns>
        public DateTime GetDateTime(int columnNo)
        {
            return _dataRecord.GetDateTime(columnNo);
        }

        /// <summary>
        /// Henter værdi fra en given kolonne.
        /// </summary>
        /// <param name="columnNo">Index for kolonnen, hvorfra der skal hentes data.</param>
        /// <returns>Værdi.</returns>
        public TimeSpan GetTimeSpan(int columnNo)
        {
            return _dataRecord.GetDateTime(columnNo).TimeOfDay;
        }

        #endregion
    }
}
