using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.FoodWaste
{
    /// <summary>
    /// Interface for a data proxy to translation information which are used for translation.
    /// </summary>
    public interface ITranslationInfoProxy : ITranslationInfo, IMySqlDataProxy, IMySqlDataProxyCreator<ITranslationInfoProxy>
    {
    }
}
