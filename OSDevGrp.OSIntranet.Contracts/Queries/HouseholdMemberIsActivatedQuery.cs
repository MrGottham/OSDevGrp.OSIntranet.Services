using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Queries
{
    /// <summary>
    /// Query which can check whether the current user has been activated.
    /// </summary>
    [DataContract(Name = "HouseholdMemberIsActivatedQuery", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class HouseholdMemberIsActivatedQuery : HouseholdMemberDataGetQueryBase
    {
    }
}
