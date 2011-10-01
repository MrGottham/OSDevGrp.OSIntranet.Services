namespace OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies
{
    /// <summary>
    /// Interface til en data proxy for data fra MySql.
    /// </summary>
    /// <typeparam name="TId">Typen på den unikke identifikation for data proxy på MySql.</typeparam>
    public interface IMySqlDataProxy<in TId> : IDataProxyBase
    {
        /// <summary>
        /// Returnerer SQL foresprøgelse til søgning efter en given data proxy på MySql.
        /// </summary>
        /// <param name="id">Unik identifikation af data proxy, som skal fremsøges.</param>
        /// <returns>SQL foresprøgelse.</returns>
        string GetSqlQueryForId(TId id);
    }
}
