using System;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Queries
{
    /// <summary>
    /// Query for getting one or more static texts.
    /// </summary>
    [DataContract(Name = "StaticTextGetQueryBase", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public abstract class StaticTextGetQueryBase : IQuery
    {
        /// <summary>
        /// Gets or sets the identifier for the translation informations used for translation.
        /// </summary>
        [DataMember(IsRequired = true)]
        public Guid TranslationInfoIdentifier { get; set; }
    }
}
