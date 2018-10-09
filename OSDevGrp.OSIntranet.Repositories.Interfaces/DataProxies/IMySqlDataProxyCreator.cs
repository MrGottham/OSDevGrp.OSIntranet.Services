using MySql.Data.MySqlClient;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies
{
    /// <summary>
    /// Interface for a data proxy creator which can creates data proxies within a MySQL data reader.
    /// </summary>
    /// <typeparam name="TDataProxy">Type of the data proxy which should be created.</typeparam>
    public interface IMySqlDataProxyCreator<out TDataProxy> : IDataProxyCreatorBase<TDataProxy, MySqlDataReader, MySqlCommand> where TDataProxy : IMySqlDataProxy
    {
    }
}
