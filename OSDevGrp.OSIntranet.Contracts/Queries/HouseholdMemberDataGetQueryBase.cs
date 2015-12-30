using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Queries
{
    /// <summary>
    /// Query for getting some data for a household member.
    /// </summary>
    [DataContract(Name = "HouseholdMemberDataGetQueryBase", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public abstract class HouseholdMemberDataGetQueryBase : IQuery
    {
    }
}
