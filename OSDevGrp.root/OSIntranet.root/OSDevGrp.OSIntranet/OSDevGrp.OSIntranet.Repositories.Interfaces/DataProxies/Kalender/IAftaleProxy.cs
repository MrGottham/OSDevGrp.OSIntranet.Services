using OSDevGrp.OSIntranet.Domain.Interfaces.Kalender;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.Kalender
{
    /// <summary>
    /// Interface til en data proxy for en kalenderaftale.
    /// </summary>
    public interface IAftaleProxy : IAftale, IMySqlDataProxy<IAftale>, ILazyLoadable
    {
    }
}
