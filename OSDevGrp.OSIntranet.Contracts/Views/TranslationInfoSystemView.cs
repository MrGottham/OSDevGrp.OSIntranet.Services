using System;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// System view for translation informations which are used for translation.
    /// </summary>
    [DataContract(Name = "TranslationInfoSystemView", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class TranslationInfoSystemView : IView
    {
        /// <summary>
        /// Get or sets the identifier for the translation informations.
        /// </summary>
        [DataMember(IsRequired = true)]
        public Guid TranslationInfoIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the name for the culture which are used for translation.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string CultureName { get; set; }
    }
}
