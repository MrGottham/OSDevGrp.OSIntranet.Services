using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Queries
{
    /// <summary>
    /// Query for getting household member data for the current user.
    /// </summary>
    [DataContract(Name = "HouseholdMemberDataGetQuery", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class HouseholdMemberDataGetQuery : HouseholdMemberTranslatableDataGetQueryBase
    {
    }
}
