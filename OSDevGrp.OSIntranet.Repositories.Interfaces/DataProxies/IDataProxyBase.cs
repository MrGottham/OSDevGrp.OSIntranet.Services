using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies
{
    /// <summary>
    /// Interface til angivelse af proxy for data fra en data provider.
    /// </summary>
    public interface IDataProxyBase
    {
        /// <summary>
        /// Mapper data fra en data reader.
        /// </summary>
        /// <param name="dataReader">Data reader for data provideren.</param>
        /// <param name="dataProvider">Data provider, hvorfra data mappes.</param>
        void MapData(object dataReader, IDataProviderBase dataProvider);

        /// <summary>
        /// Mapper data proxyens relationer.
        /// </summary>
        /// <param name="dataProvider">Data provider, hvorfra data kan mappes.</param>
        void MapRelations(IDataProviderBase dataProvider);

        /// <summary>
        /// Gemmer data proxyens relationer.
        /// </summary>
        /// <param name="dataProvider">Data provider, hvorfra data kan gemmes.</param>
        /// <param name="isInserting">Angivelse af, om der indsættes eller opdateres.</param>
        void SaveRelations(IDataProviderBase dataProvider, bool isInserting);

        /// <summary>
        /// Sletter data aproxyens relationer.
        /// </summary>
        /// <param name="dataProvider">Data provider, hvorfra data kan slettes.</param>
        void DeleteRelations(IDataProviderBase dataProvider);
    }
}
