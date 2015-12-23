using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// System view for a static text.
    /// </summary>
    [DataContract(Name = "StaticTextSystemView", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class StaticTextSystemView : IView
    {
        /// <summary>
        /// Gets or sets the identifier for the static text.
        /// </summary>
        [DataMember(IsRequired = true)]
        public Guid StaticTextIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the type of the static text.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int StaticTextType { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the subject translation.
        /// </summary>
        [DataMember(IsRequired = true)]
        public Guid SubjectTranslationIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the translation of the subject.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string SubjectTranslation { get; set; }

        /// <summary>
        /// Gets or sets the translations of the subject.
        /// </summary>
        [DataMember(IsRequired = true)]
        public IEnumerable<TranslationSystemView> SubjectTranslations { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the body translation.
        /// </summary>
        [DataMember(IsRequired = false)]
        public Guid? BodyTranslationIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the translation of the body.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string BodyTranslation { get; set; }

        /// <summary>
        /// Gets or sets the translations of the subject.
        /// </summary>
        [DataMember(IsRequired = false)]
        public IEnumerable<TranslationSystemView> BodyTranslations { get; set; }
    }
}
