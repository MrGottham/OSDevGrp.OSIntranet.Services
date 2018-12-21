using System.Collections.Generic;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste
{
    /// <summary>
    /// Interface for a repository which can access system data for the food waste domain.
    /// </summary>
    public interface ISystemDataRepository : IDataRepository
    {
        /// <summary>
        /// Gets all the storage types.
        /// </summary>
        /// <returns>All storage types.</returns>
        IEnumerable<IStorageType> StorageTypeGetAll();

        /// <summary>
        /// Gets all the food items.
        /// </summary>
        /// <returns>All food items.</returns>
        IEnumerable<IFoodItem> FoodItemGetAll();

        /// <summary>
        /// Gets all the food items which belongs to a given food group.
        /// </summary>
        /// <param name="foodGroup">Food group which the food items should belong to.</param>
        /// <returns>All food items which belongs to the given food group.</returns>
        IEnumerable<IFoodItem> FoodItemGetAllForFoodGroup(IFoodGroup foodGroup);

        /// <summary>
        /// Gets a food item by a given data providers foreign key.
        /// </summary>
        /// <param name="dataProvider">Data provider.</param>
        /// <param name="foreignKeyValue">Foreign key value.</param>
        /// <returns>Food item.</returns>
        IFoodItem FoodItemGetByForeignKey(IDataProvider dataProvider, string foreignKeyValue);
            
        /// <summary>
        /// Gets all the food groups.
        /// </summary>
        /// <returns>All food groups.</returns>
        IEnumerable<IFoodGroup> FoodGroupGetAll();

        /// <summary>
        /// Gets all the food groups at the root.
        /// </summary>
        /// <returns>All food groups at the root.</returns>
        IEnumerable<IFoodGroup> FoodGroupGetAllOnRoot();

        /// <summary>
        /// Gets a food group by a given data providers foreign key.
        /// </summary>
        /// <param name="dataProvider">Data provider.</param>
        /// <param name="foreignKeyValue">Foreign key value.</param>
        /// <returns>Food group.</returns>
        IFoodGroup FoodGroupGetByForeignKey(IDataProvider dataProvider, string foreignKeyValue);

        /// <summary>
        /// Gets all the foreign keys for a given domain object.
        /// </summary>
        /// <param name="identifiableDomainObject">The identifiable domain object on which all the foreign keys should be returned.</param>
        /// <returns>All the foreign keys for the given domain object.</returns>
        IEnumerable<IForeignKey> ForeignKeysForDomainObjectGet(IIdentifiable identifiableDomainObject);

        /// <summary>
        /// Gets a static text by a given static text type.
        /// </summary>
        /// <param name="staticTextType">Static text type for which to get the static text.</param>
        /// <returns>Static text.</returns>
        IStaticText StaticTextGetByStaticTextType(StaticTextType staticTextType);

        /// <summary>
        /// Gets all the static texts.
        /// </summary>
        /// <returns>All the static texts.</returns>
        IEnumerable<IStaticText> StaticTextGetAll();

        /// <summary>
        /// Gets the default data provider for food items.
        /// </summary>
        /// <returns>Default data provider for food items.</returns>
        IDataProvider DataProviderForFoodItemsGet();

        /// <summary>
        /// Gets the default data provider for food groups.
        /// </summary>
        /// <returns>Default data provider for food groups.</returns>
        IDataProvider DataProviderForFoodGroupsGet();

        /// <summary>
        /// Gets all the data providers.
        /// </summary>
        /// <returns>All the data providers.</returns>
        IEnumerable<IDataProvider> DataProviderGetAll();

        /// <summary>
        /// Gets all the data providers who handles payments.
        /// </summary>
        /// <returns>All the data providers who handles payments.</returns>
        IEnumerable<IDataProvider> DataProviderWhoHandlesPaymentsGetAll();

        /// <summary>
        /// Gets all the translations for a given domain object.
        /// </summary>
        /// <param name="identifiableDomainObject">The identifiable domain object on which all the translations should be returned.</param>
        /// <returns>All translations for the given domain object.</returns>
        IEnumerable<ITranslation> TranslationsForDomainObjectGet(IIdentifiable identifiableDomainObject);

        /// <summary>
        /// Gets all the translation information which can be used for translation.
        /// </summary>
        /// <returns>All the translation information which can be used for translation.</returns>
        IEnumerable<ITranslationInfo> TranslationInfoGetAll();
    }
}
