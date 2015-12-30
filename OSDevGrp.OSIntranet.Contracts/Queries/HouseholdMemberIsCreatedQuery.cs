using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Queries
{
    /// <summary>
    /// Query which can check whether the current user has been created as a household member.
    /// </summary>
    [DataContract(Name = "HouseholdMemberIsCreatedQuery", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class HouseholdMemberIsCreatedQuery : HouseholdMemberDataGetQueryBase
    {
    }
}
