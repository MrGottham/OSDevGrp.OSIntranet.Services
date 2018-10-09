using OSDevGrp.OSIntranet.Domain.Interfaces.Kalender;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.Kalender
{
    /// <summary>
    /// Interface til en data proxy for en brugers kalenderaftale.
    /// </summary>
    public interface IBrugeraftaleProxy : IBrugeraftale, IMySqlDataProxy
    {
        /// <summary>
        /// Egenskaber for brugerens kalenderaftale.
        /// </summary>
        int Properties { get; }
    }
}
