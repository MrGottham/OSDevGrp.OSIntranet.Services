using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Queries
{
    /// <summary>
    /// Query for getting a collection of data providers.
    /// </summary>
    [DataContract(Name = "DataProviderCollectionGetQuery", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class DataProviderCollectionGetQuery : IQuery
    {
    }
}
