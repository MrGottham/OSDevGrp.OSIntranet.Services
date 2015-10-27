using System;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Commands
{
    /// <summary>
    /// Command for importing a food item from a given data provider.
    /// </summary>
    [DataContract(Name = "FoodItemImportFromDataProviderCommand", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class FoodItemImportFromDataProviderCommand : ICommand
    {
        /// <summary>
        /// Gets or sets the identifier for the data provider who imports the food item.
        /// </summary>
        [DataMember(IsRequired = true)]
        public Guid DataProviderIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the translation informations which should be used to translate the name of the food item.
        /// </summary>
        [DataMember(IsRequired = true)]
        public Guid TranslationInfoIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the data providers primary key for the food item.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the name for the food item compared to the translaton informations.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the primary food group for the food item.
        /// </summary>
        [DataMember(IsRequired = true)]
        public Guid PrimaryFoodGroupIdentifier { get; set; }

        /// <summary>
        /// Gets or sets whether the food item should be active.
        /// </summary>
        [DataMember(IsRequired = true)]
        public bool IsActive { get; set; }
    }
}
