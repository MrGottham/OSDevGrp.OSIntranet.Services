using System.Collections.Generic;
using System.Data;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies;

namespace OSDevGrp.OSIntranet.Repositories.DataProviders
{
    /// <summary>
    /// Basis data provider.
    /// </summary>
    /// <typeparam name="T">Type of the database command for SQL statements which the data provider should use.</typeparam>
    public abstract class DataProviderBase<T> : IDataProviderBase<T> where T : IDbCommand
    {
        #region IDisposable Members

        /// <summary>
        /// Frigørelse af allokerede ressourcer i data provideren.
        /// </summary>
        public abstract void Dispose();

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Danner ny instans af data provideren.
        /// </summary>
        /// <returns>Ny instans af data provideren.</returns>
        public abstract object Clone();

        #endregion

        #region IDataProviderBase Members

        /// <summary>
        /// Henter og returnerer data fra data provideren.
        /// </summary>
        /// <typeparam name="TDataProxy">Typen for data proxy til data provideren.</typeparam>
        /// <param name="queryCommand">Database command for the SQL query statement.</param>
        /// <returns>Collection indeholdende data.</returns>
        public abstract IEnumerable<TDataProxy> GetCollection<TDataProxy>(T queryCommand) where TDataProxy : class, IDataProxyBase<T>, new();

        /// <summary>
        /// Henter data for en given data proxy i data provideren.
        /// </summary>
        /// <typeparam name="TDataProxy">Typen for data proxy til data provideren.</typeparam>
        /// <param name="dataProxy">Data proxy, som indeholder nødvendige værdier til fremsøgning.</param>
        /// <returns>Data proxy.</returns>
        public abstract TDataProxy Get<TDataProxy>(TDataProxy dataProxy) where TDataProxy : class, IDataProxyBase<T>, new();

        /// <summary>
        /// Tilføjer data til data provideren.
        /// </summary>
        /// <typeparam name="TDataProxy">Typen for data proxy til data provideren.</typeparam>
        /// <param name="dataProxy">Data proxy med data, som skal tilføjes data provideren.</param>
        /// <returns>Data proxy med tilføjede data.</returns>
        public abstract TDataProxy Add<TDataProxy>(TDataProxy dataProxy) where TDataProxy : class, IDataProxyBase<T>;

        /// <summary>
        /// Gemmer data i data provideren.
        /// </summary>
        /// <typeparam name="TDataProxy">Typen for data proxy til data provideren.</typeparam>
        /// <param name="dataProxy">Data proxy med data, som skal gemmes i data provideren.</param>
        /// <returns>Data proxy med gemte data.</returns>
        public abstract TDataProxy Save<TDataProxy>(TDataProxy dataProxy) where TDataProxy : class, IDataProxyBase<T>;

        /// <summary>
        /// Sletter data fra data provideren.
        /// </summary>
        /// <typeparam name="TDataProxy">Typen for data proxy til data provideren.</typeparam>
        /// <param name="dataProxy">Data proxy med data, som skal slettes fra data provideren.</param>
        public abstract void Delete<TDataProxy>(TDataProxy dataProxy) where TDataProxy : class, IDataProxyBase<T>;

        #endregion
    }
}
