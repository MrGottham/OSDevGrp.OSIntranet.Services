using System;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Queries
{
    /// <summary>
    /// Query for getting the collection of food items.
    /// </summary>
    [DataContract(Name = "FoodItemCollectionGetQuery", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class FoodItemCollectionGetQuery : IQuery
    {
        /// <summary>
        /// Gets or sets the identifier for the translation informations used for translation.
        /// </summary>
        [DataMember(IsRequired = true)]
        public Guid TranslationInfoIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the food group on which to get the food items.
        /// </summary>
        [DataMember(IsRequired = false)]
        public Guid? FoodGroupIdentifier { get; set; }
    }
}
