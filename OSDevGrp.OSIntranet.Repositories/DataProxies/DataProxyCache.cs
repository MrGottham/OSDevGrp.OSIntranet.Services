using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Guards;

namespace OSDevGrp.OSIntranet.Repositories.DataProxies
{
    /// <summary>
    /// Functionality which can cache data proxies.
    /// </summary>
    internal static class DataProxyCache
    {
        #region Private variables

        private static readonly object SyncRoot = new object();

        #endregion

        /// <summary>
        /// Gets the cached collection of the <typeparamref name="TDataProxy"/> or an empty collection when no data has been cached.
        /// </summary>
        /// <typeparam name="TDataProxy">Type of the data proxy which should be cached.</typeparam>
        /// <param name="cacheName">Unique name for the cached data.</param>
        /// <returns>Cached collection of the <typeparamref name="TDataProxy"/> or an empty collection when no data has been cached.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="cacheName"/> is null, empty or white space.</exception>
        internal static HashSet<TDataProxy> GetCachedDataProxyCollection<TDataProxy>(string cacheName)
        {
            ArgumentNullGuard.NotNullOrWhiteSpace(cacheName, nameof(cacheName));

            lock (SyncRoot)
            {
                MemoryCache memoryCache = MemoryCache.Default;
                HashSet<TDataProxy> cachedDataProxyCollection = memoryCache.Get(cacheName) as HashSet<TDataProxy>;
                return cachedDataProxyCollection ?? new HashSet<TDataProxy>();
            }
        }

        /// <summary>
        /// Adds an instance of the <typeparamref name="TDataProxy"/> to the cache.
        /// </summary>
        /// <typeparam name="TDataProxy">Type of the data proxy which should be cached.</typeparam>
        /// <param name="cacheName">Unique name for the cached data.</param>
        /// <param name="dataProxy">The instance of the <typeparamref name="TDataProxy"/> which should be added to the cache.</param>
        /// <param name="dataProxyMatch">The predicate which can match any <typeparamref name="TDataProxy"/>.</param>
        /// <param name="minutesToStore">Number of minute to store cached data.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="cacheName"/> is null, empty or white space or when <paramref name="dataProxy"/> is null or when <paramref name="dataProxyMatch"/> is null.</exception>
        internal static void AddDataProxyCollectionToCache<TDataProxy>(string cacheName, TDataProxy dataProxy, Predicate<TDataProxy> dataProxyMatch, int minutesToStore = 15)
        {
            ArgumentNullGuard.NotNullOrWhiteSpace(cacheName, nameof(cacheName))
                .NotNull(dataProxy, nameof(dataProxy))
                .NotNull(dataProxyMatch, nameof(dataProxyMatch));

            AddDataProxyCollectionToCache(cacheName, new List<TDataProxy> {dataProxy}, dataProxyMatch, minutesToStore);
        }

        /// <summary>
        /// Adds a collection of the <typeparamref name="TDataProxy"/> to the cache.
        /// </summary>
        /// <typeparam name="TDataProxy">Type of the data proxy which should be cached.</typeparam>
        /// <param name="cacheName">Unique name for the cached data.</param>
        /// <param name="dataProxyCollection">The collection of the <typeparamref name="TDataProxy"/> which should be added to the cache.</param>
        /// <param name="dataProxyMatch">The predicate which can match any <typeparamref name="TDataProxy"/>.</param>
        /// <param name="minutesToStore">Number of minute to store cached data.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="cacheName"/> is null, empty or white space or when <paramref name="dataProxyCollection"/> is null or when <paramref name="dataProxyMatch"/> is null.</exception>
        internal static void AddDataProxyCollectionToCache<TDataProxy>(string cacheName, List<TDataProxy> dataProxyCollection, Predicate<TDataProxy> dataProxyMatch, int minutesToStore = 15)
        {
            ArgumentNullGuard.NotNullOrWhiteSpace(cacheName, nameof(cacheName))
                .NotNull(dataProxyCollection, nameof(dataProxyCollection))
                .NotNull(dataProxyMatch, nameof(dataProxyMatch));

            if (dataProxyCollection.Any() == false)
            {
                return;
            }

            lock (SyncRoot)
            {
                HashSet<TDataProxy> cachedDataProxyCollection = GetCachedDataProxyCollection<TDataProxy>(cacheName);
                cachedDataProxyCollection.RemoveWhere(dataProxyMatch);
                dataProxyCollection.ForEach(dataProxy => cachedDataProxyCollection.Add(dataProxy));

                MemoryCache memoryCache = MemoryCache.Default;
                memoryCache.Set(cacheName, cachedDataProxyCollection, DateTimeOffset.Now.AddMinutes(minutesToStore));
            }
        }

        /// <summary>
        /// Removes an instance of the <typeparamref name="TDataProxy"/> from the cache.
        /// </summary>
        /// <typeparam name="TDataProxy">Type of the data proxy which should be cached.</typeparam>
        /// <param name="cacheName">Unique name for the cached data.</param>
        /// <param name="dataProxy">The instance of the <typeparamref name="TDataProxy"/> which should be removed from the cache.</param>
        /// <param name="dataProxyMatch">The predicate which can match any <typeparamref name="TDataProxy"/>.</param>
        /// <param name="minutesToStore">Number of minute to store cached data.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="cacheName"/> is null, empty or white space or when <paramref name="dataProxy"/> is null or when <paramref name="dataProxyMatch"/> is null.</exception>
        internal static void RemoveDataProxyCollectionToCache<TDataProxy>(string cacheName, TDataProxy dataProxy, Predicate<TDataProxy> dataProxyMatch, int minutesToStore = 15)
        {
            ArgumentNullGuard.NotNullOrWhiteSpace(cacheName, nameof(cacheName))
                .NotNull(dataProxy, nameof(dataProxy))
                .NotNull(dataProxyMatch, nameof(dataProxyMatch));

            lock (SyncRoot)
            {
                HashSet<TDataProxy> cachedDataProxyCollection = GetCachedDataProxyCollection<TDataProxy>(cacheName);
                cachedDataProxyCollection.RemoveWhere(dataProxyMatch);

                MemoryCache memoryCache = MemoryCache.Default;
                memoryCache.Set(cacheName, cachedDataProxyCollection, DateTimeOffset.Now.AddMinutes(minutesToStore));
            }
        }
    }
}
