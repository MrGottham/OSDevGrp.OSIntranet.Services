using System.Data;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies
{
    /// <summary>
    /// Interface til angivelse af proxy for data fra en data provider.
    /// </summary>
    /// <typeparam name="TDataReader">Type of the data reader which supports this data proxy.</typeparam>
    /// <typeparam name="TDbCommand">Type of the database command for SQL statements which the proxy should create.</typeparam>
    public interface IDataProxyBase<TDataReader, TDbCommand> where TDataReader : IDataReader where TDbCommand : IDbCommand
    {
        /// <summary>
        /// Mapper data fra en data reader.
        /// </summary>
        /// <param name="dataReader">Data reader for data provideren.</param>
        /// <param name="dataProvider">Data provider, hvorfra data mappes.</param>
        void MapData(TDataReader dataReader, IDataProviderBase<TDataReader, TDbCommand> dataProvider);

        /// <summary>
        /// Mapper data proxyens relationer.
        /// </summary>
        /// <param name="dataProvider">Data provider, hvorfra data kan mappes.</param>
        void MapRelations(IDataProviderBase<TDataReader, TDbCommand> dataProvider);

        /// <summary>
        /// Gemmer data proxyens relationer.
        /// </summary>
        /// <param name="dataProvider">Data provider, hvorfra data kan gemmes.</param>
        /// <param name="isInserting">Angivelse af, om der indsættes eller opdateres.</param>
        void SaveRelations(IDataProviderBase<TDataReader, TDbCommand> dataProvider, bool isInserting);

        /// <summary>
        /// Sletter data proxyens relationer.
        /// </summary>
        /// <param name="dataProvider">Data provider, hvorfra data kan slettes.</param>
        void DeleteRelations(IDataProviderBase<TDataReader, TDbCommand> dataProvider);

        /// <summary>
        /// Creates the SQL statement for getting this data proxy.
        /// </summary>
        /// <returns>SQL statement for getting this data proxy.</returns>
        TDbCommand CreateGetCommand();

        /// <summary>
        /// Creates the SQL statement for inserting this data proxy.
        /// </summary>
        /// <returns>SQL statement for inserting this data proxy.</returns>
        TDbCommand CreateInsertCommand();

        /// <summary>
        /// Creates the SQL statement for updating this data proxy.
        /// </summary>
        /// <returns>SQL statement for updating this data proxy.</returns>
        TDbCommand CreateUpdateCommand();

        /// <summary>
        /// Creates the SQL statement for deleting this data proxy.
        /// </summary>
        /// <returns>SQL statement for deleting this data proxy.</returns>
        TDbCommand CreateDeleteCommand();
    }
}
