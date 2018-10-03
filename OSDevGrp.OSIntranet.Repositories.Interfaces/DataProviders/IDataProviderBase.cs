using System;
using System.Collections.Generic;
using System.Data;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders
{
    /// <summary>
    /// Interface til en basis data provider.
    /// </summary>
    /// <typeparam name="TDataReader">Type of the data reader which the data provider should use.</typeparam>
    /// <typeparam name="TDbCommand">Type of the database command for SQL statements which the data provider should use.</typeparam>
    public interface IDataProviderBase<out TDataReader, in TDbCommand> : IDisposable, ICloneable where TDataReader : IDataReader where TDbCommand : IDbCommand
    {
        /// <summary>
        /// Henter og returnerer data fra data provideren.
        /// </summary>
        /// <typeparam name="TDataProxy">Typen for data proxy til data provideren.</typeparam>
        /// <param name="queryCommand">Database command for the SQL query statement.</param>
        /// <returns>Collection indeholdende data.</returns>
        IEnumerable<TDataProxy> GetCollection<TDataProxy>(TDbCommand queryCommand) where TDataProxy : class, IDataProxyBase<TDataReader, TDbCommand>, new();

        /// <summary>
        /// Henter data for en given data proxy i data provideren.
        /// </summary>
        /// <typeparam name="TDataProxy">Typen for data proxy til data provideren.</typeparam>
        /// <param name="dataProxy">Data proxy, som indeholder nødvendige værdier til fremsøgning.</param>
        /// <returns>Data proxy.</returns>
        TDataProxy Get<TDataProxy>(TDataProxy dataProxy) where TDataProxy : class, IDataProxyBase<TDataReader, TDbCommand>, new();

        /// <summary>
        /// Tilføjer data til data provideren.
        /// </summary>
        /// <typeparam name="TDataProxy">Typen for data proxy til data provideren.</typeparam>
        /// <param name="dataProxy">Data proxy med data, som skal tilføjes data provideren.</param>
        /// <returns>Data proxy med tilføjede data.</returns>
        TDataProxy Add<TDataProxy>(TDataProxy dataProxy) where TDataProxy : class, IDataProxyBase<TDataReader, TDbCommand>;

        /// <summary>
        /// Gemmer data i data provideren.
        /// </summary>
        /// <typeparam name="TDataProxy">Typen for data proxy til data provideren.</typeparam>
        /// <param name="dataProxy">Data proxy med data, som skal gemmes i data provideren.</param>
        /// <returns>Data proxy med gemte data.</returns>
        TDataProxy Save<TDataProxy>(TDataProxy dataProxy) where TDataProxy : class, IDataProxyBase<TDataReader, TDbCommand>;

        /// <summary>
        /// Sletter data fra data provideren.
        /// </summary>
        /// <typeparam name="TDataProxy">Typen for data proxy til data provideren.</typeparam>
        /// <param name="dataProxy">Data proxy med data, som skal slettes fra data provideren.</param>
        void Delete<TDataProxy>(TDataProxy dataProxy) where TDataProxy : class, IDataProxyBase<TDataReader, TDbCommand>;
    }
}
