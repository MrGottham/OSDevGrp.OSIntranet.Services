using System;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Commands
{
    /// <summary>
    /// Command for deleting a translation.
    /// </summary>
    [DataContract(Name = "TranslationDeleteCommand", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class TranslationDeleteCommand : ICommand
    {
        /// <summary>
        /// Gets or sets the identifier for the translation to delete.
        /// </summary>
        [DataMember(IsRequired = true)]
        public Guid TranslationIdentifier { get; set; }
    }
}
