using System.Collections.Generic;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste
{
    /// <summary>
    /// Interface for a repository which can access system data for the food waste domain.
    /// </summary>
    public interface ISystemDataRepository : IRepository
    {
        /// <summary>
        /// Gets all the translation informations which can be used for translation.
        /// </summary>
        /// <returns>All the translation informations which can be used for translation.</returns>
        IEnumerable<ITranslationInfo> TranslationInfoGetAll();
    }
}
