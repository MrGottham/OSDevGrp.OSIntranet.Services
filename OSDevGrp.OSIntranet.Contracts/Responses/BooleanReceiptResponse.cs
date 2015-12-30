using System;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Responses
{
    /// <summary>
    /// Response for a boolean.
    /// </summary>
    [DataContract(Name = "BooleanResponse", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class BooleanResponse : IView
    {
        /// <summary>
        /// Gets or sets the event date.
        /// </summary>
        [DataMember(IsRequired = true)]
        public DateTime EventDate { get; set; }

        /// <summary>
        /// Gets or sets result.
        /// </summary>
        [DataMember(IsRequired = false)]
        public bool Result { get; set; }
    }
}
