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
