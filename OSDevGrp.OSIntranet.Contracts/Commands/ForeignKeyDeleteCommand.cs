using System;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Commands
{
    /// <summary>
    /// Command for deleting a foreign key.
    /// </summary>
    [DataContract(Name = "ForeignKeyDeleteCommand", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class ForeignKeyDeleteCommand : ICommand
    {
        /// <summary>
        /// Gets or sets the identifier for the foreign key to delete.
        /// </summary>
        [DataMember(IsRequired = true)]
        public Guid ForeignKeyIdentifier { get; set; }
    }
}
