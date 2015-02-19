using System;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Commands
{
    /// <summary>
    /// Command for adding a translation.
    /// </summary>
    [DataContract(Name = "TranslationAddCommand", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class TranslationAddCommand : ICommand
    {
        /// <summary>
        /// Gets or sets the identifier for the domain object which name can be translated by this translation.
        /// </summary>
        [DataMember(IsRequired = true)]
        public Guid TranslationOfIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the translation informations which should be used in the translation.
        /// </summary>
        [DataMember(IsRequired = true)]
        public Guid TranslationInfoIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the translation value for the name of domain object.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string TranslationValue { get; set; }
    }
}
