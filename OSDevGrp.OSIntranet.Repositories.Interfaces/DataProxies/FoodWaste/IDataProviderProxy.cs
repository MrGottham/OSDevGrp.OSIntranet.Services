using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.FoodWaste
{
    /// <summary>
    /// Interface for a data proxy to a given data provider.
    /// </summary>
    public interface IDataProviderProxy : IDataProvider, IMySqlDataProxy<IDataProvider>
    {
        /// <summary>
        /// Adds a data proxy for a data source statement to the given data provider.
        /// </summary>
        /// <param name="dataSourceStatementProxy">Data proxy for the data source statement to add.</param>
        void AddDataSourceStatement(ITranslationProxy dataSourceStatementProxy);
    }
}
