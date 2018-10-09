using System.Data;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies
{
    /// <summary>
    /// Interface for a data proxy creator which can creates data proxies within a data reader.
    /// </summary>
    /// <typeparam name="TDataProxy">Type of the data proxy which should be created.</typeparam>
    /// <typeparam name="TDataReader">Type of the data reader from which to create data proxies.</typeparam>
    /// <typeparam name="TDbCommand">Type of the database command used by the data proxy which should be created.</typeparam>
    public interface IDataProxyCreatorBase<out TDataProxy, TDataReader, TDbCommand> where TDataProxy : IDataProxyBase<TDataReader, TDbCommand> where TDataReader : IDataReader where TDbCommand : IDbCommand
    {
        /// <summary>
        /// Creates an instance of the data proxy with values from the data reader.
        /// </summary>
        /// <param name="dataReader">Data reader from which column values should be read.</param>
        /// <param name="dataProvider">Data provider which supports the data reader.</param>
        /// <param name="columnNameCollection">Collection of column names which should be read from the data reader.</param>
        /// <returns>Instance of the data proxy with values from the data reader.</returns>
        TDataProxy Create(TDataReader dataReader, IDataProviderBase<TDataReader, TDbCommand> dataProvider, params string[] columnNameCollection);
    }
}
