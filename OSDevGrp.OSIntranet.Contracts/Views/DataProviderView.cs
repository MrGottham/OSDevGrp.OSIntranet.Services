using System;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// View for a data provider.
    /// </summary>
    [DataContract(Name = "DataProviderView", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class DataProviderView : IView
    {
        /// <summary>
        /// Gets or sets the identifier for the data provider.
        /// </summary>
        [DataMember(IsRequired = true)]
        public Guid DataProviderIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the name for the data provider.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the data providers data source statement.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string DataSourceStatement { get; set; }
    }
}
