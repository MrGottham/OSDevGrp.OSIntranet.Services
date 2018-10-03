using MySql.Data.MySqlClient;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies
{
    /// <summary>
    /// Interface til en data proxy for data fra MySql.
    /// </summary>
    public interface IMySqlDataProxy : IDataProxyBase<MySqlDataReader, MySqlCommand>
    {
        /// <summary>
        /// Returnerer den unikke identifikation for data proxy.
        /// </summary>
        string UniqueId { get; }
    }
}
