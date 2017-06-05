using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// View for an integer range.
    /// </summary>
    [DataContract(Name = "IntRangeView", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class IntRangeView : IView
    {
        /// <summary>
        /// Gets or sets the start value for the range.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int StartValue { get; set; }

        /// <summary>
        /// Gets or sets the end value for the range.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int EndValue { get; set; }
    }
}
