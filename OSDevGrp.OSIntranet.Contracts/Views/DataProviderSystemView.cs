using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// System view for a data provider.
    /// </summary>
    [DataContract(Name = "DataProviderSystemView", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class DataProviderSystemView : IView
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
        /// Gets or sets whether the data provider handles payments.
        /// </summary>
        [DataMember(IsRequired = true)]
        public bool HandlesPayments { get; set; }

        /// <summary>
        /// Gets or sets the identifier for translations of the data providers data source statement.
        /// </summary>
        [DataMember(IsRequired = true)]
        public Guid DataSourceStatementIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the the translations of the data providers data source statement.
        /// </summary>
        [DataMember(IsRequired = true)]
        public IEnumerable<TranslationSystemView> DataSourceStatements { get; set; }
    }
}
