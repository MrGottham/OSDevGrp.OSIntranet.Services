using System;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// System view for foreign key.
    /// </summary>
    [DataContract(Name = "ForeignKeySystemView", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class ForeignKeySystemView : IView
    {
        /// <summary>
        /// Gets or sets the identification for the foreign key.
        /// </summary>
        [DataMember(IsRequired = true)]
        public Guid ForeignKeyIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the data provider who owns the foreign key.
        /// </summary>
        [DataMember(IsRequired = true)]
        public DataProviderSystemView DataProvider { get; set; }

        /// <summary>
        /// Gets or sets the identification for the domain object which has this foreign key.
        /// </summary>
        [DataMember(IsRequired = true)]
        public Guid ForeignKeyForIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the value for the foreign key.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string ForeignKey { get; set; }
    }
}
