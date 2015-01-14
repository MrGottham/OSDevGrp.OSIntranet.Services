using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Queries
{
    /// <summary>
    /// Query for getting a collection of translation informations.
    /// </summary>
    [DataContract(Name = "TranslationInfoCollectionGetQuery", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class TranslationInfoCollectionGetQuery : IQuery
    {
    }
}
