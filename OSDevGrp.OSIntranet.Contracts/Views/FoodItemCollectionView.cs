using System.Collections.Generic;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// View for a collection of food items.
    /// </summary>
    [DataContract(Name = "FoodItemCollectionView", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class FoodItemCollectionView : IView
    {
        /// <summary>
        /// Gets or sets food items in the collection.
        /// </summary>
        [DataMember(IsRequired = true)]
        public IEnumerable<FoodItemView> FoodItems { get; set; }

        /// <summary>
        /// Gets or sets the data provider who has provided the food items in the collection.
        /// </summary>
        [DataMember(IsRequired = true)]
        public DataProviderView DataProvider { get; set; }
    }
}
