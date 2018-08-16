using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.FoodWaste
{
    /// <summary>
    /// Interface for a data proxy to a given foreign key to a domain object in the food waste domain.
    /// </summary>
    public interface IForeignKeyProxy : IForeignKey, IMySqlDataProxy
    {
    }
}
