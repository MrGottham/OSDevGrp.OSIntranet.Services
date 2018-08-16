using System.Data;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies
{
    /// <summary>
    /// Interface til angivelse af proxy for data fra en data provider.
    /// </summary>
    /// <typeparam name="T">Type of the database command for SQL statements which the proxy should create.</typeparam>
    public interface IDataProxyBase<out T> where T : IDbCommand
    {
        /// <summary>
        /// Mapper data fra en data reader.
        /// </summary>
        /// <param name="dataReader">Data reader for data provideren.</param>
        /// <param name="dataProvider">Data provider, hvorfra data mappes.</param>
        void MapData(object dataReader, IDataProviderBase<T> dataProvider);

        /// <summary>
        /// Mapper data proxyens relationer.
        /// </summary>
        /// <param name="dataProvider">Data provider, hvorfra data kan mappes.</param>
        void MapRelations(IDataProviderBase<T> dataProvider);

        /// <summary>
        /// Gemmer data proxyens relationer.
        /// </summary>
        /// <param name="dataProvider">Data provider, hvorfra data kan gemmes.</param>
        /// <param name="isInserting">Angivelse af, om der indsættes eller opdateres.</param>
        void SaveRelations(IDataProviderBase<T> dataProvider, bool isInserting);

        /// <summary>
        /// Sletter data proxyens relationer.
        /// </summary>
        /// <param name="dataProvider">Data provider, hvorfra data kan slettes.</param>
        void DeleteRelations(IDataProviderBase<T> dataProvider);

        /// <summary>
        /// Creates the SQL statement for getting this data proxy.
        /// </summary>
        /// <returns>SQL statement for getting this data proxy.</returns>
        T CreateGetCommand();

        /// <summary>
        /// Creates the SQL statement for inserting this data proxy.
        /// </summary>
        /// <returns>SQL statement for inserting this data proxy.</returns>
        T CreateInsertCommand();

        /// <summary>
        /// Creates the SQL statement for updating this data proxy.
        /// </summary>
        /// <returns>SQL statement for updating this data proxy.</returns>
        T CreateUpdateCommand();

        /// <summary>
        /// Creates the SQL statement for deleting this data proxy.
        /// </summary>
        /// <returns>SQL statement for deleting this data proxy.</returns>
        T CreateDeleteCommand();
    }
}
