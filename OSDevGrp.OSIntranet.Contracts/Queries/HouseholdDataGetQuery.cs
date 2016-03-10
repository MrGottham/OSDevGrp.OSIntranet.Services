using System;
using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Queries
{
    /// <summary>
    /// Query for getting household data for one of the current user households.
    /// </summary>
    [DataContract(Name = "HouseholdDataGetQuery", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class HouseholdDataGetQuery : HouseholdMemberTranslatableDataGetQueryBase
    {
        /// <summary>
        /// Gets or sets the identifier for the household for which data should be returned.
        /// </summary>
        [DataMember(IsRequired = true)]
        public Guid HouseholdIdentifier { get; set; }
    }
}
