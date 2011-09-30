using System;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces
{
    /// <summary>
    /// Interface til behandling af data i en record fra MySql.
    /// </summary>
    public interface IMySqlDataRecord
    {
        /// <summary>
        /// Henter værdi fra en given kolonne.
        /// </summary>
        /// <param name="columnNo">Index for kolonnen, hvorfra der skal hentes data.</param>
        /// <returns>Værdi.</returns>
        short GetInt16(int columnNo);

        /// <summary>
        /// Henter værdi fra en given kolonne.
        /// </summary>
        /// <param name="columnNo">Index for kolonnen, hvorfra der skal hentes data.</param>
        /// <returns>Værdi.</returns>
        int GetInt32(int columnNo);

        /// <summary>
        /// Henter værdi fra en given kolonne.
        /// </summary>
        /// <param name="columnNo">Index for kolonnen, hvorfra der skal hentes data.</param>
        /// <returns>Værdi.</returns>
        long GetInt64(int columnNo);

        /// <summary>
        /// Henter værdi fra en given kolonne.
        /// </summary>
        /// <param name="columnNo">Index for kolonnen, hvorfra der skal hentes data.</param>
        /// <returns>Værdi.</returns>
        decimal GetDecimal(int columnNo);

        /// <summary>
        /// Henter værdi fra en given kolonne.
        /// </summary>
        /// <param name="columnNo">Index for kolonnen, hvorfra der skal hentes data.</param>
        /// <returns>Værdi.</returns>
        bool GetBoolean(int columnNo);

        /// <summary>
        /// Henter værdi fra en given kolonne.
        /// </summary>
        /// <param name="columnNo">Index for kolonnen, hvorfra der skal hentes data.</param>
        /// <returns>Værdi.</returns>
        string GetString(int columnNo);

        /// <summary>
        /// Henter værdi fra en given kolonne.
        /// </summary>
        /// <param name="columnNo">Index for kolonnen, hvorfra der skal hentes data.</param>
        /// <returns>Værdi.</returns>
        DateTime GetDateTime(int columnNo);

        /// <summary>
        /// Henter værdi fra en given kolonne.
        /// </summary>
        /// <param name="columnNo">Index for kolonnen, hvorfra der skal hentes data.</param>
        /// <returns>Værdi.</returns>
        TimeSpan GetTimeSpan(int columnNo);
    }
}
