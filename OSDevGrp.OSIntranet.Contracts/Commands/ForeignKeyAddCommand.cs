using System;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Commands
{
    /// <summary>
    /// Command for adding a foreign key.
    /// </summary>
    [DataContract(Name = "ForeignKeyAddCommand", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class ForeignKeyAddCommand : ICommand
    {
        /// <summary>
        /// Gets or sets the identifier of the data provider who owns the foreign key.
        /// </summary>
        [DataMember(IsRequired = true)]
        public Guid DataProviderIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the domain object which has the foreign key.
        /// </summary>
        [DataMember(IsRequired = true)]
        public Guid ForeignKeyForIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the value of the foreign key.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string ForeignKeyValue { get; set; }
    }
}
