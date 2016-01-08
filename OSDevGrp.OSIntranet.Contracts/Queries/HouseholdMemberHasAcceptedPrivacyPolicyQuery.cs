using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Queries
{
    /// <summary>
    /// Query which can check whether the current user has accepted the privacy policy.
    /// </summary>
    [DataContract(Name = "HouseholdMemberHasAcceptedPrivacyPolicyQuery", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class HouseholdMemberHasAcceptedPrivacyPolicyQuery : HouseholdMemberDataGetQueryBase
    {
    }
}
