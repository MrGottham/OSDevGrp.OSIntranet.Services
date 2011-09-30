using System;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces
{
    /// <summary>
    /// Interface til en MySql klient.
    /// </summary>
    public interface IMySqlClient : IDisposable
    {
        /// <summary>
        /// Henter fra MySql en collection af en given type.
        /// </summary>
        /// <typeparam name="T">Typen, som skal hentes fra MySql.</typeparam>
        /// <param name="query">Sql-query til forespørgelse efter objekter af den givne type.</param>
        /// <param name="builder">Callbackmetode til bygning af objekt.</param>
        /// <returns>Collection af en given type.</returns>
        IEnumerable<T> GetCollection<T>(string query, Func<IMySqlDataRecord, T> builder);
    }
}
