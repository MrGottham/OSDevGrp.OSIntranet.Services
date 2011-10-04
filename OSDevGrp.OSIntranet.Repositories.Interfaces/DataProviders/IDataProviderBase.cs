﻿using System.Collections.Generic;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders
{
    /// <summary>
    /// Interface til en basis data provider.
    /// </summary>
    public interface IDataProviderBase
    {
        /// <summary>
        /// Henter og returnerer data fra data provideren.
        /// </summary>
        /// <typeparam name="TDataProxy">Typen for data proxy til data provideren.</typeparam>
        /// <param name="query">Foresprøgelse efter data.</param>
        /// <returns>Collection indeholdende data.</returns>
        IEnumerable<TDataProxy> GetCollection<TDataProxy>(string query) where TDataProxy : class, IDataProxyBase, new();

        /// <summary>
        /// Henter data for en given data proxy i data provideren.
        /// </summary>
        /// <typeparam name="TDataProxy">Typen for data proxy til data provideren.</typeparam>
        /// <typeparam name="TId">Typen på den unikke identifikation for data i data proxy.</typeparam>
        /// <param name="id">Unik identifikation for data proxy, der skal hentes.</param>
        /// <returns>Data proxy.</returns>
        TDataProxy Get<TDataProxy, TId>(TId id) where TDataProxy : class, IDataProxyBase, new();

        /// <summary>
        /// Tilføjer data til data provideren.
        /// </summary>
        /// <typeparam name="TDataProxy">Typen for data proxy til data provideren.</typeparam>
        /// <param name="dataProxy">Data proxy med data, som skal tilføjes data provideren.</param>
        /// <returns>Data proxy med tilføjede data.</returns>
        TDataProxy Add<TDataProxy>(TDataProxy dataProxy) where TDataProxy : class, IDataProxyBase;

        /// <summary>
        /// Gemmer data i data provideren.
        /// </summary>
        /// <typeparam name="TDataProxy">Typen for data proxy til data provideren.</typeparam>
        /// <param name="dataProxy">Data proxy med data, som skal gemmes i data provideren.</param>
        /// <returns>Data proxy med gemte data.</returns>
        TDataProxy Save<TDataProxy>(TDataProxy dataProxy) where TDataProxy : class, IDataProxyBase;

        /// <summary>
        /// Sletter data fra data provideren.
        /// </summary>
        /// <typeparam name="TDataProxy">Typen for data proxy til data provideren.</typeparam>
        /// <param name="dataProxy">Data proxy med data, som skal slettes fra data provideren.</param>
        void Delete<TDataProxy>(TDataProxy dataProxy) where TDataProxy : class, IDataProxyBase;
    }
}