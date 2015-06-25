using System.Collections.Generic;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste
{
    /// <summary>
    /// Interface for a repository which can access system data for the food waste domain.
    /// </summary>
    public interface ISystemDataRepository : IDataRepository
    {
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
        /// Gets all the foreign keys for a given domain object.
        /// </summary>
        /// <param name="identifiableDomainObject">The identifiable domain object on which all the foreign keys should be returned.</param>
        /// <returns>All the foreign keys for the given domain object.</returns>
        IEnumerable<IForeignKey> ForeignKeysForDomainObjectGet(IIdentifiable identifiableDomainObject);

        /// <summary>
        /// Gets the default data provider for foods.
        /// </summary>
        /// <returns>Default data provider for foods</returns>
        IDataProvider DataProviderForFoodsGet();

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
        /// Gets all the translations for a given domain object.
        /// </summary>
        /// <param name="identifiableDomainObject">The identifiable domain object on which all the translations should be returned.</param>
        /// <returns>All translations for the given domain object.</returns>
        IEnumerable<ITranslation> TranslationsForDomainObjectGet(IIdentifiable identifiableDomainObject);

        /// <summary>
        /// Gets all the translation informations which can be used for translation.
        /// </summary>
        /// <returns>All the translation informations which can be used for translation.</returns>
        IEnumerable<ITranslationInfo> TranslationInfoGetAll();
    }
}
