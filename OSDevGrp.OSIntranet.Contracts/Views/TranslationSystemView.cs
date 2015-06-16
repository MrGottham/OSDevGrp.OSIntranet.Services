using System;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// System view for a translation of a domain object.
    /// </summary>
    [DataContract(Name = "Translation", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class TranslationSystemView : IView
    {
        /// <summary>
        /// Gets or sets the identifier for the translation.
        /// </summary>
        [DataMember(IsRequired = true)]
        public Guid TranslationIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the domain object on which this translation are used.
        /// </summary>
        [DataMember(IsRequired = true)]
        public Guid TranslationOfIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the translation informations which are used for translation.
        /// </summary>
        [DataMember(IsRequired = true)]
        public TranslationInfoSystemView TranslationInfo { get; set; }

        /// <summary>
        /// Gets or sets the translation for the domain object.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Translation { get; set; }
    }
}
