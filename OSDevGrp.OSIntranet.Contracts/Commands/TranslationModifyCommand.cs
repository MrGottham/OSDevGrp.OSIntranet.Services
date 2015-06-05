using System;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Commands
{
    /// <summary>
    /// Command for modifying a translation.
    /// </summary>
    [DataContract(Name = "TranslationModifyCommand", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class TranslationModifyCommand : ICommand
    {
        /// <summary>
        /// Gets or sets the identifier for the translation to modify.
        /// </summary>
        [DataMember(IsRequired = true)]
        public Guid TranslationIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the translation value for the name of domain object.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string TranslationValue { get; set; }
    }
}
