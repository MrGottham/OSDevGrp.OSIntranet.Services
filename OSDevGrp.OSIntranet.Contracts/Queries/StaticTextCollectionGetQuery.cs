using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Queries
{
    /// <summary>
    /// Query for getting a collection of static texts.
    /// </summary>
    [DataContract(Name = "StaticTextCollectionGetQuery", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class StaticTextCollectionGetQuery : StaticTextGetQueryBase
    {
    }
}
