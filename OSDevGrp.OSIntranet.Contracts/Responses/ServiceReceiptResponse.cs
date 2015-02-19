using System;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Responses
{
    /// <summary>
    /// Response for a service receipt.
    /// </summary>
    [DataContract(Name = "ServiceReceipt", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class ServiceReceiptResponse : IView
    {
        /// <summary>
        /// Gets or sets the event date.
        /// </summary>
        [DataMember(IsRequired = true)]
        public DateTime EventDate { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the handled domain object.
        /// </summary>
        [DataMember(IsRequired = false)]
        public Guid? Identifier { get; set; }
    }
}
