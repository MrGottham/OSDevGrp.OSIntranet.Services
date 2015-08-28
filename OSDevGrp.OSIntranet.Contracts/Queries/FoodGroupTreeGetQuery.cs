using System;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Queries
{
    /// <summary>
    /// Query for getting the tree of food groups.
    /// </summary>
    [DataContract(Name = "FoodGroupTreeGetQuery", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class FoodGroupTreeGetQuery : IQuery
    {
        /// <summary>
        /// Gets or sets the identifier for the translation informations used for translation.
        /// </summary>
        [DataMember(IsRequired = true)]
        public Guid TranslationInfoIdentifier { get; set; }
    }
}
