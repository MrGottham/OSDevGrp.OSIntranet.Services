using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.FoodWaste
{
    /// <summary>
    /// Interface for a data proxy to a given translation for a domain object.
    /// </summary>
    public interface ITranslationProxy : ITranslation, IMySqlDataProxy
    {
    }
}
