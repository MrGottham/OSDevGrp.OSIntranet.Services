using System;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Commands
{
    /// <summary>
    /// Command for modifying a foreign key.
    /// </summary>
    [DataContract(Name = "ForeignKeyModifyCommand", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class ForeignKeyModifyCommand : ICommand
    {
        /// <summary>
        /// Gets or sets the identifier for the foreign key to modify.
        /// </summary>
        [DataMember(IsRequired = true)]
        public Guid ForeignKeyIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the value of the foreign key.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string ForeignKeyValue { get; set; }
    }
}
