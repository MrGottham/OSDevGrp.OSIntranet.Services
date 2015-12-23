using System;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// View for a static text.
    /// </summary>
    [DataContract(Name = "StaticTextView", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class StaticTextView : IView
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
        /// Gets or sets the translation of the subject.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string SubjectTranslation { get; set; }

        /// <summary>
        /// Gets or sets the translation of the body.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string BodyTranslation { get; set; }
    }
}
