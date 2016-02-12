using System;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// View for a payment from a stakeholder.
    /// </summary>
    [DataContract(Name = "PaymentView", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class PaymentView : IView
    {
        /// <summary>
        /// Gets or sets the identifier for the payment.
        /// </summary>
        [DataMember(IsRequired = true)]
        public Guid PaymentIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the stakeholder who made the payment.
        /// </summary>
        [DataMember(IsRequired = true)]
        public StakeholderView Stakeholder { get; set; }

        /// <summary>
        /// Gets or sets the data provider who handled the payment.
        /// </summary>
        [DataMember(IsRequired = true)]
        public DataProviderView DataProvider { get; set; }

        /// <summary>
        /// Gets or sets the date and time for the payment.
        /// </summary>
        [DataMember(IsRequired = true)]
        public DateTime PaymentTime { get; set; }

        /// <summary>
        /// Gets or sets the payment reference from the data provider who has handled the payment.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string PaymentReference { get; set; }

        /// <summary>
        /// Gets or sets the Base64 encoded payment receipt from the data provider who has handled the payment.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string PaymentReceipt { get; set; }

        /// <summary>
        /// Gets or sets the creation date and time for the payment
        /// </summary>
        [DataMember(IsRequired = true)]
        public DateTime CreationTime { get; set; }
    }
}
