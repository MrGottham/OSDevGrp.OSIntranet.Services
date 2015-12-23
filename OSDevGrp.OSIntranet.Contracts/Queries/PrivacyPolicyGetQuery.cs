using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Queries
{
    /// <summary>
    /// Query for getting the privacy policy.
    /// </summary>
    [DataContract(Name = "PrivacyPolicyGetQuery", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class PrivacyPolicyGetQuery : StaticTextGetQueryBase
    {
    }
}
