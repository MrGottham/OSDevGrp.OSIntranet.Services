using System.Collections.Generic;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// System view for a tree of food groups.
    /// </summary>
    [DataContract(Name = "FoodGroupTreeSystemView", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class FoodGroupTreeSystemView : IView
    {
        /// <summary>
        /// Gets or sets food groups at the root of the tree.
        /// </summary>
        [DataMember(IsRequired = true)]
        public IEnumerable<FoodGroupSystemView> FoodGroups { get; set; }

        /// <summary>
        /// Gets or sets the data provider who has provided the food groups in the tree.
        /// </summary>
        [DataMember(IsRequired = true)]
        public DataProviderView DataProvider { get; set; }
    }
}
