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
    }
}
