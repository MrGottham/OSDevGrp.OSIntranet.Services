using System;
using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Queries
{
    /// <summary>
    /// Query for getting a collection of data providers who handles payments.
    /// </summary>
    [DataContract(Name = "DataProviderWhoHandlesPaymentsCollectionGetQuery", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class DataProviderWhoHandlesPaymentsCollectionGetQuery : DataProviderCollectionGetQuery
    {
        /// <summary>
        /// Gets or sets the identifier for the translation informations used for translation.
        /// </summary>
        [DataMember(IsRequired = true)]
        public Guid TranslationInfoIdentifier { get; set; }
    }
}
