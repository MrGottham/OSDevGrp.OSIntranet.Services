using System;
using System.Collections.Generic;
using System.Data;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Guards;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies;

namespace OSDevGrp.OSIntranet.Repositories.DataProviders
{
    /// <summary>
    /// Basis data provider.
    /// </summary>
    /// <typeparam name="TDataReader">Type of the data reader which the data provider should use.</typeparam>
    /// <typeparam name="TDbCommand">Type of the database command for SQL statements which the data provider should use.</typeparam>
    public abstract class DataProviderBase<TDataReader, TDbCommand> : IDataProviderBase<TDataReader, TDbCommand> where TDataReader : IDataReader where TDbCommand : IDbCommand
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
        public abstract IEnumerable<TDataProxy> GetCollection<TDataProxy>(TDbCommand queryCommand) where TDataProxy : class, IDataProxyBase<TDataReader, TDbCommand>, new();

        /// <summary>
        /// Henter data for en given data proxy i data provideren.
        /// </summary>
        /// <typeparam name="TDataProxy">Typen for data proxy til data provideren.</typeparam>
        /// <param name="dataProxy">Data proxy, som indeholder nødvendige værdier til fremsøgning.</param>
        /// <returns>Data proxy.</returns>
        public abstract TDataProxy Get<TDataProxy>(TDataProxy dataProxy) where TDataProxy : class, IDataProxyBase<TDataReader, TDbCommand>, new();

        /// <summary>
        /// Tilføjer data til data provideren.
        /// </summary>
        /// <typeparam name="TDataProxy">Typen for data proxy til data provideren.</typeparam>
        /// <param name="dataProxy">Data proxy med data, som skal tilføjes data provideren.</param>
        /// <returns>Data proxy med tilføjede data.</returns>
        public abstract TDataProxy Add<TDataProxy>(TDataProxy dataProxy) where TDataProxy : class, IDataProxyBase<TDataReader, TDbCommand>;

        /// <summary>
        /// Gemmer data i data provideren.
        /// </summary>
        /// <typeparam name="TDataProxy">Typen for data proxy til data provideren.</typeparam>
        /// <param name="dataProxy">Data proxy med data, som skal gemmes i data provideren.</param>
        /// <returns>Data proxy med gemte data.</returns>
        public abstract TDataProxy Save<TDataProxy>(TDataProxy dataProxy) where TDataProxy : class, IDataProxyBase<TDataReader, TDbCommand>;

        /// <summary>
        /// Sletter data fra data provideren.
        /// </summary>
        /// <typeparam name="TDataProxy">Typen for data proxy til data provideren.</typeparam>
        /// <param name="dataProxy">Data proxy med data, som skal slettes fra data provideren.</param>
        public abstract void Delete<TDataProxy>(TDataProxy dataProxy) where TDataProxy : class, IDataProxyBase<TDataReader, TDbCommand>;

        /// <summary>
        /// Creates an instance of the data proxy with values from the data reader.
        /// </summary>
        /// <typeparam name="TDataProxy">Type of the data proxy which should be created.</typeparam>
        /// <param name="dataProxyCreator">Data proxy creator which can create the data proxy.</param>
        /// <param name="dataReader">Data reader from which column values should be read.</param>
        /// <param name="columnNameCollection">Collection of column names which should be read from the data reader.</param>
        /// <returns>Instance of the data proxy with values from the data reader.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataProxyCreator"/>, <paramref name="dataReader"/> or <paramref name="columnNameCollection"/> is null.</exception>
        public TDataProxy Create<TDataProxy>(IDataProxyCreatorBase<TDataProxy, TDataReader, TDbCommand> dataProxyCreator, TDataReader dataReader, params string[] columnNameCollection) where TDataProxy : class, IDataProxyBase<TDataReader, TDbCommand>
        {
            ArgumentNullGuard.NotNull(dataProxyCreator, nameof(dataProxyCreator))
                .NotNull(dataReader, nameof(dataReader))
                .NotNull(columnNameCollection, nameof(columnNameCollection));

            return dataProxyCreator.Create(dataReader, this, columnNameCollection);
        }

        #endregion
    }
}
