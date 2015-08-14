using System;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Commands
{
    /// <summary>
    /// Command for importing a food group from a given data provider.
    /// </summary>
    [DataContract(Name = "ImportFoodGroupFromDataProviderCommand", Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class ImportFoodGroupFromDataProviderCommand : ICommand
    {
        /// <summary>
        /// Gets or sets the identifier for the data provider who imports the food group.
        /// </summary>
        [DataMember(IsRequired = true)]
        public Guid DataProviderIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the translation informations which should be used to translate the name of the food group.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string TranslationInfoIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the data providers primary key for the food group.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the name for the food group compared to the translaton informations.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets whether the food group should be active.
        /// </summary>
        [DataMember(IsRequired = true)]
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the data providers primary key for the food group which has this food group as a child.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string ParentKey { get; set; }
    }
}
