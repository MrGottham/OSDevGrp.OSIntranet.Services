namespace OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies
{
    /// <summary>
    /// Interface til en data proxy for data fra MySql.
    /// </summary>
    /// <typeparam name="TDataProxy">Typen på den data proxy, som der arbejdes med på MySql.</typeparam>
    public interface IMySqlDataProxy<in TDataProxy> : IDataProxyBase where TDataProxy : IDataProxyBase
    {
        /// <summary>
        /// Returnerer den unikke identifikation for data proxy.
        /// </summary>
        string UniqueId
        {
            get;
        }

        /// <summary>
        /// Returnerer SQL foresprøgelse til søgning efter en given data proxy på MySql.
        /// </summary>
        /// <param name="queryForDataProxy">Data proxy indeholdende de nødvendige værdier til fremsøgning på MySql.</param>
        /// <returns>SQL foresprøgelse.</returns>
        string GetSqlQueryForId(TDataProxy queryForDataProxy);

        /// <summary>
        /// Returnerer SQL kommando til oprettelse af data proxy på MySQL.
        /// </summary>
        /// <returns>SQL kommando til oprettelse.</returns>
        string GetSqlCommandForInsert();

        /// <summary>
        /// Returnerer SQL kommando til opdatering af data proxy på MySQL.
        /// </summary>
        /// <returns>SQL kommando til opdatering.</returns>
        string GetSqlCommandForUpdate();

        /// <summary>
        /// Returnerer SQL kommando til slening af data proxy fra MySQL.
        /// </summary>
        /// <returns>SQL kommando til sletning.</returns>
        string GetSqlCommandForDelete();
    }
}
